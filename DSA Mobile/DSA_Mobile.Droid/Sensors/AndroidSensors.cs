using AndroidSensorType = Android.Hardware.SensorType;
using SensorType = DSA_Mobile.Sensors.SensorType;
using Android.Hardware;
using Android.Runtime;
using DSA_Mobile.Sensors;
using System;

namespace DSA_Mobile.Droid.Sensors
{
    public class AndroidSensors : BaseSensors
    {
        private SensorManager _sensorManager;
        private SensorEventListener _sensorListener;

        public AndroidSensors(SensorManager sensorManager)
        {
            _sensorManager = sensorManager;
            _sensorListener = new SensorEventListener(this);
        }

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
            }
        }

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
            }
        }

        private class SensorEventListener : Java.Lang.Object, ISensorEventListener
        {
            private AndroidSensors _sensors;

            public SensorEventListener(AndroidSensors motionImpl)
            {
                _sensors = motionImpl;
            }

            public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
            {
            }

            public void OnSensorChanged(SensorEvent e)
            {
                double x = e.Values[0];
                double y = e.Values[1];
                double z = e.Values[2];
                MotionVector vector = new MotionVector(x, y, z);
                switch (e.Sensor.Type)
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

