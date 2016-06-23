using DSLink.Nodes;
using System.Diagnostics;
using System;
using System.Threading.Tasks;

namespace DSA_Mobile.Sensors
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
        private Task _subscriptionTask;

        public bool Supported => true;

        public SensorsModule(BaseSensors sensors)
		{
            _sensorsImpl = sensors;
            _subscriptionTask = new Task(UpdateSubscriptions);
		}

        public void UpdateSubscriptions()
        {
            while (!_subscriptionTask.IsCanceled)
            {
                if (!_sensorsImpl.AccelerometerActive &&
                    (_accel_x.Subscribed ||
                     _accel_y.Subscribed ||
                     _accel_z.Subscribed))
                {
                    Debug.WriteLine("Accelerometer started");
                    _sensorsImpl.Start(SensorType.Accelerometer);
                }
                if (!_sensorsImpl.GyroActive &&
                    (_gyro_x.Subscribed ||
                     _gyro_y.Subscribed ||
                     _gyro_z.Subscribed))
                {
                    Debug.WriteLine("Gyro started");
                    _sensorsImpl.Start(SensorType.Gyroscope);
                }
                if (!_sensorsImpl.DeviceMotionActive &&
                    (_dmotion_x.Subscribed ||
                     _dmotion_y.Subscribed ||
                     _dmotion_z.Subscribed))
                {
                    Debug.WriteLine("DeviceMotion started");
                    _sensorsImpl.Start(SensorType.DeviceMotion);
                }

                if (_sensorsImpl.AccelerometerActive &&
                    (!_accel_x.Subscribed &&
                     !_accel_y.Subscribed &&
                     !_accel_z.Subscribed))
                {
                    Debug.WriteLine("Accelerometer stopped");
                    _sensorsImpl.Stop(SensorType.Accelerometer);
                }
                if (_sensorsImpl.GyroActive &&
                    (!_gyro_x.Subscribed &&
                     !_gyro_y.Subscribed &&
                     !_gyro_z.Subscribed))
                {
                    Debug.WriteLine("Gyro stopped");
                    _sensorsImpl.Stop(SensorType.Gyroscope);
                }
                if (_sensorsImpl.DeviceMotionActive &&
                    (!_dmotion_x.Subscribed &&
                     !_dmotion_y.Subscribed &&
                     !_dmotion_z.Subscribed))
                {
                    Debug.WriteLine("DeviceMotion stopped");
                    _sensorsImpl.Stop(SensorType.DeviceMotion);
                }

                _subscriptionTask.Wait(500);
            }
        }

        public bool RequestPermissions()
        {
            return true;
        }

        public void AddNodes(Node superRoot)
        {
            _sensors = superRoot.CreateChild("Sensors").BuildNode();
            _accelerometer = _sensors.CreateChild("Accelerometer").BuildNode();
            _accel_x = _accelerometer.CreateChild("X")
                                             .SetType("number")
                                             .BuildNode();
            _accel_y = _accelerometer.CreateChild("Y")
                                             .SetType("number")
                                             .BuildNode();
            _accel_z = _accelerometer.CreateChild("Z")
                                             .SetType("number")
                                             .BuildNode();

            _gyroscope = _sensors.CreateChild("Gyroscope").BuildNode();
            _gyro_x = _gyroscope.CreateChild("X")
                                   .SetType("number")
                                   .BuildNode();
            _gyro_y = _gyroscope.CreateChild("Y")
                                   .SetType("number")
                                   .BuildNode();
            _gyro_z = _gyroscope.CreateChild("Z")
                                   .SetType("number")
                                   .BuildNode();
            _dmotion = _sensors.CreateChild("DeviceMotion").BuildNode();
            _dmotion_x = _dmotion.CreateChild("X")
                                 .SetType("number")
                                 .BuildNode();
            _dmotion_y = _dmotion.CreateChild("Y")
                                 .SetType("number")
                                 .BuildNode();
            _dmotion_z = _dmotion.CreateChild("Z")
                                 .SetType("number")
                                 .BuildNode();

            /*_compass = _sensors.CreateChild("Compass")
                              .SetType("number")
                              .BuildNode();*/
            
            //_sensorsImpl.Start(SensorType.Compass);
            _sensorsImpl.AccelerometerValueChanged += (MotionVector vector) =>
            {
                _accel_x.Value.Set(vector.X);
                _accel_y.Value.Set(vector.Y);
                _accel_z.Value.Set(vector.Z);
            };
            _sensorsImpl.GyroscopeValueChanged += (MotionVector vector) =>
            {
                _gyro_x.Value.Set(vector.X);
                _gyro_y.Value.Set(vector.Y);
                _gyro_z.Value.Set(vector.Z);
            };
            _sensorsImpl.DeviceMotionValueChanged += (MotionVector vector) =>
            {
                _dmotion_x.Value.Set(vector.X);
                _dmotion_y.Value.Set(vector.Y);
                _dmotion_z.Value.Set(vector.Z);
            };
            /*_sensorsImpl.CompassValueChanged += (double value) =>
            {
                //Debug.WriteLine(value);
            };*/

            _subscriptionTask.Start();
        }
    }
}

