using System;
using Android.Hardware;
using Android.Runtime;

namespace DSA_Mobile.Motion
{
    public class AndroidMotionImplementation : BaseMotionImplementation
    {
        private SensorManager _sensorManager;
        private SensorEventListener _sensorListener;

        public AndroidMotionImplementation(SensorManager sensorManager)
        {
            _sensorManager = sensorManager;
            _sensorListener = new SensorEventListener(this);
        }

        public override void Start(SensorType sensorType)
        {
            switch (sensorType)
            {
                case SensorType.Accelerometer:
                    _sensorManager.RegisterListener(_sensorListener,
                                                    _sensorManager.GetDefaultSensor(Android.Hardware.SensorType.Accelerometer),
                                                    SensorDelay.Normal);
                    break;
                case SensorType.Gyroscope:
                    _sensorManager.RegisterListener(_sensorListener,
                                                    _sensorManager.GetDefaultSensor(Android.Hardware.SensorType.Gyroscope),
                                                    SensorDelay.Normal);
                    break;
                case SensorType.Compass:
                    break;
            }
        }

        private class SensorEventListener : Java.Lang.Object, ISensorEventListener
        {
            private BaseMotionImplementation _motionImpl;

            public SensorEventListener(AndroidMotionImplementation motionImpl)
            {
                _motionImpl = motionImpl;
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
                    case Android.Hardware.SensorType.Accelerometer:
                        _motionImpl.EmitAccelerometer(vector);
                        break;
                    case Android.Hardware.SensorType.Gyroscope:
                        _motionImpl.EmitGyroscope(vector);
                        break;
                }
            }
        }
    }
}

