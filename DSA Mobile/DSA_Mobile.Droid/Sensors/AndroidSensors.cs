using AndroidSensorType = Android.Hardware.SensorType;
using SensorType = DSAMobile.Sensors.SensorType;
using Android.Hardware;
using Android.Runtime;
using DSAMobile.Sensors;

namespace DSAMobile.Droid.Sensors
{
    /// <summary>
    /// Android sensors implementation.
    /// 
    /// Supports:
    /// - Accelerometer
    /// - Gyroscope
    /// - Device Rotation
    /// - Compass
    /// - Light Level
    /// </summary>
    public class AndroidSensors : BaseSensors
    {
        private SensorManager _sensorManager;
        private SensorEventListener _sensorListener;

        public override bool SupportsAccelerometer => true;
        public override bool SupportsGyroscope => true;
        public override bool SupportsDeviceMotion => true;
        public override bool SupportsCompass => true;
        public override bool SupportsLightLevel => true;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:DSAMobile.Droid.Sensors.AndroidSensors"/> class.
        /// </summary>
        /// <param name="sensorManager">Android sensor manager</param>
        public AndroidSensors(SensorManager sensorManager)
        {
            _sensorManager = sensorManager;
            _sensorListener = new SensorEventListener(this);
        }

        /// <summary>
        /// Start the specified sensor type reading.
        /// </summary>
        /// <param name="sensorType">Sensor type</param>
        public override void Start(SensorType sensorType)
        {
            switch (sensorType)
            {
                case SensorType.Accelerometer:
                    AccelerometerActive = true;
                    _sensorManager.RegisterListener(_sensorListener,
                                                    _sensorManager.GetDefaultSensor(AndroidSensorType.Accelerometer),
                                                    SensorDelay.Game);
                    break;
                case SensorType.Gyroscope:
                    GyroActive = true;
                    _sensorManager.RegisterListener(_sensorListener,
                                                    _sensorManager.GetDefaultSensor(AndroidSensorType.Gyroscope),
                                                    SensorDelay.Game);
                    break;
                case SensorType.DeviceMotion:
                    DeviceMotionActive = true;
                    _sensorManager.RegisterListener(_sensorListener,
                                                    _sensorManager.GetDefaultSensor(AndroidSensorType.RotationVector),
                                                    SensorDelay.Game);
                    break;
                case SensorType.Compass:
                    CompassActive = true;
                    break;
                case SensorType.LightLevel:
                    LightLevelActive = true;
                    _sensorManager.RegisterListener(_sensorListener,
                                                    _sensorManager.GetDefaultSensor(AndroidSensorType.Light),
                                                    SensorDelay.Normal);
                    break;
            }
        }

        /// <summary>
        /// Stop the specified sensor type reading.
        /// </summary>
        /// <param name="sensorType">Sensor type</param>
        public override void Stop(SensorType sensorType)
        {
            switch (sensorType)
            {
                case SensorType.Accelerometer:
                    AccelerometerActive = false;
                    _sensorManager.UnregisterListener(_sensorListener,
                                                      _sensorManager.GetDefaultSensor(AndroidSensorType.Accelerometer));
                    break;
                case SensorType.Gyroscope:
                    GyroActive = false;
                    _sensorManager.UnregisterListener(_sensorListener,
                                                      _sensorManager.GetDefaultSensor(AndroidSensorType.Gyroscope));
                    break;
                case SensorType.DeviceMotion:
                    DeviceMotionActive = false;
                    _sensorManager.UnregisterListener(_sensorListener,
                                                      _sensorManager.GetDefaultSensor(AndroidSensorType.RotationVector));
                    break;
                case SensorType.Compass:
                    CompassActive = false;
                    break;
                case SensorType.LightLevel:
                    LightLevelActive = false;
                    _sensorManager.UnregisterListener(_sensorListener,
                                                      _sensorManager.GetDefaultSensor(AndroidSensorType.Light));
                    break;
            }
        }

        /// <summary>
        /// Sensor event listener, implements the ISensorEventListener interface.
        /// </summary>
        private class SensorEventListener : Java.Lang.Object, ISensorEventListener
        {
            /// <summary>
            /// Instance of the app's Android implementation.
            /// </summary>
            private readonly AndroidSensors _sensors;

            /// <summary>
            /// Initializes a new instance of the
            /// <see cref="T:DSAMobile.Droid.Sensors.AndroidSensors.SensorEventListener"/> class.
            /// </summary>
            /// <param name="motionImpl">Motion impl.</param>
            public SensorEventListener(AndroidSensors motionImpl)
            {
                _sensors = motionImpl;
            }

            /// <summary>
            /// Event that occurs when the accuracy changes for a sensor.
            /// </summary>
            /// <param name="sensor">Sensor</param>
            /// <param name="accuracy">Accuracy of sensor</param>
            public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
            {
            }

            /// <summary>
            /// Event that occurs when a sensor value changes.
            /// </summary>
            /// <param name="sensorEvent">Sensor event</param>
            public void OnSensorChanged(SensorEvent sensorEvent)
            {
                if (sensorEvent.Values.Count == 1)
                {
                    var value = sensorEvent.Values[0];
                    switch (sensorEvent.Sensor.Type)
                    {
                        case AndroidSensorType.Light:
                            _sensors.EmitLightLevel(value);
                            break;
                    }
                }
                // In this case, the sensor is likely a vector.
                else if (sensorEvent.Values.Count == 3)
                {
                    var vector = new MotionVector(sensorEvent.Values[0], sensorEvent.Values[1], sensorEvent.Values[2]);
                    switch (sensorEvent.Sensor.Type)
                    {
                        case AndroidSensorType.Accelerometer:
                            _sensors.EmitAccelerometer(vector);
                            break;
                        case AndroidSensorType.Gyroscope:
                            _sensors.EmitGyroscope(vector);
                            break;
                        case AndroidSensorType.RotationVector:
                            _sensors.EmitDeviceMotion(vector);
                            break;
                    }
                }
            }
        }
    }
}

