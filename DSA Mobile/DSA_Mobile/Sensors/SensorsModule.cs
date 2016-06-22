using DSLink.Nodes;
using System.Diagnostics;
using System;

namespace DSA_Mobile.Sensors
{
    public class SensorsModule : BaseModule
	{
        private readonly BaseSensors _sensorsImpl;
        private Node _sensors;
		private Node _accelerometer;
		private Node _accelerometer_x;
		private Node _accelerometer_y;
		private Node _accelerometer_z;
		private Node _gyroscope;
		private Node _gyroscope_x;
		private Node _gyroscope_y;
		private Node _gyroscope_z;
        private Node _dmotion;
        private Node _dmotion_x;
        private Node _dmotion_y;
        private Node _dmotion_z;
		private Node _compass;

        public bool Supported => true;

        public SensorsModule(BaseSensors sensors)
		{
            _sensorsImpl = sensors;
		}

        public bool RequestPermissions()
        {
            return true;
        }

        public void AddNodes(Node superRoot)
        {
            _sensors = superRoot.CreateChild("Sensors").BuildNode();
            _accelerometer = _sensors.CreateChild("Accelerometer").BuildNode();
            _accelerometer_x = _accelerometer.CreateChild("X")
                                             .SetType("number")
                                             .BuildNode();
            _accelerometer_y = _accelerometer.CreateChild("Y")
                                             .SetType("number")
                                             .BuildNode();
            _accelerometer_z = _accelerometer.CreateChild("Z")
                                             .SetType("number")
                                             .BuildNode();

            _gyroscope = _sensors.CreateChild("Gyroscope").BuildNode();
            _gyroscope_x = _gyroscope.CreateChild("X")
                                   .SetType("number")
                                   .BuildNode();
            _gyroscope_y = _gyroscope.CreateChild("Y")
                                   .SetType("number")
                                   .BuildNode();
            _gyroscope_z = _gyroscope.CreateChild("Z")
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

            _compass = _sensors.CreateChild("Compass")
                              .SetType("number")
                              .BuildNode();

            _sensorsImpl.Start(SensorType.Accelerometer);
            _sensorsImpl.Start(SensorType.Gyroscope);
            _sensorsImpl.Start(SensorType.DeviceMotion);
            //_sensorsImpl.Start(SensorType.Compass);
            _sensorsImpl.AccelerometerValueChanged += (MotionVector vector) =>
            {
                _accelerometer_x.Value.Set(vector.X);
                _accelerometer_y.Value.Set(vector.Y);
                _accelerometer_z.Value.Set(vector.Z);
            };
            _sensorsImpl.GyroscopeValueChanged += (MotionVector vector) =>
            {
                _gyroscope_x.Value.Set(vector.X);
                _gyroscope_y.Value.Set(vector.Y);
                _gyroscope_z.Value.Set(vector.Z);
            };
            _sensorsImpl.DeviceMotionValueChanged += (MotionVector vector) =>
            {
                _dmotion_x.Value.Set(vector.X);
                _dmotion_y.Value.Set(vector.Y);
                _dmotion_z.Value.Set(vector.Z);
            };
            _sensorsImpl.CompassValueChanged += (double value) =>
            {
                //Debug.WriteLine(value);
            };
        }
    }
}

