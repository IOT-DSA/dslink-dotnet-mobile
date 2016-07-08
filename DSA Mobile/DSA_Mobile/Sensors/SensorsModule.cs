using DSLink.Nodes;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace DSAMobile.Sensors
{
    public class SensorsModule : BaseModule
	{
        private readonly BaseSensors _sensorsImpl;
        private Node _sensors;
		private Node _accelerometer;
        private Node _accel_x;
        private Node _accel_y;
        private Node _accel_z;
		private Node _gyroscope;
        private Node _gyro_x;
        private Node _gyro_y;
		private Node _gyro_z;
        private Node _dmotion;
        private Node _dmotion_x;
        private Node _dmotion_y;
        private Node _dmotion_z;
		private Node _compass;
        private Node _lightLevel;
        private CancellationTokenSource _subToken;
        private Task _subTask;

        public bool Supported => true;

        public SensorsModule(BaseSensors sensors)
		{
            _sensorsImpl = sensors;
            _subToken = new CancellationTokenSource();
            _subTask = new Task(UpdateSubscriptions, _subToken.Token);
		}

        public bool RequestPermissions()
        {
            return true;
        }

        public void AddNodes(Node superRoot)
        {
            _sensors = superRoot.CreateChild("Sensors").BuildNode();

            if (_sensorsImpl.SupportsAccelerometer)
            {
                _accelerometer = _sensors.CreateChild("accelerometer")
                                     .SetDisplayName("Accelerometer")
                                     .BuildNode();
                _accel_x = _accelerometer.CreateChild("x")
                                                 .SetType("number")
                                                 .BuildNode();
                _accel_y = _accelerometer.CreateChild("y")
                                                 .SetType("number")
                                                 .BuildNode();
                _accel_z = _accelerometer.CreateChild("z")
                                                 .SetType("number")
                                                 .BuildNode();

                _sensorsImpl.AccelerometerValueChanged += (MotionVector vector) =>
                {
                    _accel_x.Value.Set(vector.X);
                    _accel_y.Value.Set(vector.Y);
                    _accel_z.Value.Set(vector.Z);
                };
            }

            if (_sensorsImpl.SupportsGyroscope)
            {
                _gyroscope = _sensors.CreateChild("gyroscope")
                                 .SetDisplayName("Gyroscope")
                                 .BuildNode();
                _gyro_x = _gyroscope.CreateChild("x")
                                       .SetType("number")
                                       .BuildNode();
                _gyro_y = _gyroscope.CreateChild("y")
                                       .SetType("number")
                                       .BuildNode();
                _gyro_z = _gyroscope.CreateChild("z")
                                       .SetType("number")
                                       .BuildNode();

                _sensorsImpl.GyroscopeValueChanged += (MotionVector vector) =>
                {
                    _gyro_x.Value.Set(vector.X);
                    _gyro_y.Value.Set(vector.Y);
                    _gyro_z.Value.Set(vector.Z);
                };
            }

            if (_sensorsImpl.SupportsDeviceMotion)
            {
                _dmotion = _sensors.CreateChild("device_motion")
                               .SetDisplayName("Device Motion")
                               .BuildNode();
                _dmotion_x = _dmotion.CreateChild("x")
                                     .SetType("number")
                                     .BuildNode();
                _dmotion_y = _dmotion.CreateChild("y")
                                     .SetType("number")
                                     .BuildNode();
                _dmotion_z = _dmotion.CreateChild("z")
                                     .SetType("number")
                                     .BuildNode();

                _sensorsImpl.DeviceMotionValueChanged += (MotionVector vector) =>
                {
                    _dmotion_x.Value.Set(vector.X);
                    _dmotion_y.Value.Set(vector.Y);
                    _dmotion_z.Value.Set(vector.Z);
                };
            }

            if (_sensorsImpl.SupportsCompass)
            {
                /*_compass = _sensors.CreateChild("Compass")
                              .SetType("number")
                              .BuildNode();
                _sensorsImpl.CompassValueChanged += (double value) =>
                {
                    //Debug.WriteLine(value);
                };*/
            }

            if (_sensorsImpl.SupportsLightLevel)
            {
                _lightLevel = _sensors.CreateChild("light_level")
                                  .SetDisplayName("Light Level")
                                  .SetType("number")
                                  .BuildNode();

                _sensorsImpl.LightLevelValueChanged += _lightLevel.Value.Set;
            }

            _subTask.Start();
        }

        public void RemoveNodes()
        {
            _subToken.Cancel();

            if (_sensorsImpl.SupportsAccelerometer)
            {
                _accelerometer.RemoveFromParent();
                _accel_x.RemoveFromParent();
                _accel_y.RemoveFromParent();
                _accel_z.RemoveFromParent();
            }

            if (_sensorsImpl.SupportsGyroscope)
            {
                _gyroscope.RemoveFromParent();
                _gyro_x.RemoveFromParent();
                _gyro_y.RemoveFromParent();
                _gyro_z.RemoveFromParent();
            }

            if (_sensorsImpl.SupportsDeviceMotion)
            {
                _dmotion.RemoveFromParent();
                _dmotion_x.RemoveFromParent();
                _dmotion_y.RemoveFromParent();
                _dmotion_z.RemoveFromParent();
            }

            if (_sensorsImpl.SupportsLightLevel)
            {
                _lightLevel.RemoveFromParent();
            }

            _sensors.RemoveFromParent();
        }

        private void UpdateSubscriptions()
        {
            while (!_subTask.IsCanceled)
            {
                if (_sensorsImpl.SupportsAccelerometer)
                {
                    if (!_sensorsImpl.AccelerometerActive &&
                    (_accel_x.Subscribed ||
                     _accel_y.Subscribed ||
                     _accel_z.Subscribed))
                    {
                        Debug.WriteLine("Accelerometer started");
                        _sensorsImpl.Start(SensorType.Accelerometer);
                    }
                    if (_sensorsImpl.AccelerometerActive &&
                    (!_accel_x.Subscribed &&
                     !_accel_y.Subscribed &&
                     !_accel_z.Subscribed))
                    {
                        Debug.WriteLine("Accelerometer stopped");
                        _sensorsImpl.Stop(SensorType.Accelerometer);
                    }
                }

                if (_sensorsImpl.SupportsGyroscope)
                {
                    if (!_sensorsImpl.GyroActive &&
                    (_gyro_x.Subscribed ||
                     _gyro_y.Subscribed ||
                     _gyro_z.Subscribed))
                    {
                        Debug.WriteLine("Gyro started");
                        _sensorsImpl.Start(SensorType.Gyroscope);
                    }
                    if (_sensorsImpl.GyroActive &&
                    (!_gyro_x.Subscribed &&
                     !_gyro_y.Subscribed &&
                     !_gyro_z.Subscribed))
                    {
                        Debug.WriteLine("Gyro stopped");
                        _sensorsImpl.Stop(SensorType.Gyroscope);
                    }
                }

                if (_sensorsImpl.SupportsDeviceMotion)
                {
                    if (!_sensorsImpl.DeviceMotionActive &&
                    (_dmotion_x.Subscribed ||
                     _dmotion_y.Subscribed ||
                     _dmotion_z.Subscribed))
                    {
                        Debug.WriteLine("DeviceMotion started");
                        _sensorsImpl.Start(SensorType.DeviceMotion);
                    }
                    if (_sensorsImpl.DeviceMotionActive &&
                    (!_dmotion_x.Subscribed &&
                     !_dmotion_y.Subscribed &&
                     !_dmotion_z.Subscribed))
                    {
                        Debug.WriteLine("DeviceMotion stopped");
                        _sensorsImpl.Stop(SensorType.DeviceMotion);
                    }
                }

                if (_sensorsImpl.SupportsLightLevel)
                {
                    if (!_sensorsImpl.LightLevelActive &&
                    _lightLevel.Subscribed)
                    {
                        Debug.WriteLine("Light Level stopped");
                        _sensorsImpl.Start(SensorType.LightLevel);
                    }
                    if (_sensorsImpl.LightLevelActive &&
                        !_lightLevel.Subscribed)
                    {
                        Debug.WriteLine("Light Level stopped");
                        _sensorsImpl.Stop(SensorType.LightLevel);
                    }
                }

                _subTask.Wait(100);
            }
        }
    }
}

