using DSLink.Nodes;

using System.Diagnostics;
using DSA_Mobile.Sensors;

namespace DSA_Mobile
{
	public class SensorsModule
	{
        private readonly BaseSensors _sensorsImpl;
        private readonly Node _sensors;
		private readonly Node _accelerometer;
		private readonly Node _accelerometer_x;
		private readonly Node _accelerometer_y;
		private readonly Node _accelerometer_z;
		private readonly Node _gyroscope;
		private readonly Node _gyroscope_x;
		private readonly Node _gyroscope_y;
		private readonly Node _gyroscope_z;
        private readonly Node _dmotion;
        private readonly Node _dmotion_x;
        private readonly Node _dmotion_y;
        private readonly Node _dmotion_z;
		private readonly Node _compass;

		public SensorsModule(Node superRoot, App app)
		{
			_sensors = superRoot.CreateChild("Sensors").BuildNode();
            _sensorsImpl = app.GetSensors();
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
			_sensorsImpl.CompassValueChanged += (double value) => {
				Debug.WriteLine(value);
			};
		}

		/*private void MotionChanged(object sender, SensorValueChangedEventArgs sensorArgs)
		{
			MotionVector vector;
			switch (sensorArgs.SensorType)
			{
				case MotionSensorType.Accelerometer:
					vector = (MotionVector)sensorArgs.Value;
					if (_accelerometer_x.Subscribed) _accelerometer_x.Value.Set(vector.X);
					if (_accelerometer_y.Subscribed) _accelerometer_y.Value.Set(vector.Y);
					if (_accelerometer_z.Subscribed) _accelerometer_z.Value.Set(vector.Z);
					break;
				case MotionSensorType.Gyroscope:
					vector = (MotionVector)sensorArgs.Value;
					if (_gyroscope_x.Subscribed) _gyroscope_x.Value.Set(vector.X);
					if (_gyroscope_y.Subscribed) _gyroscope_y.Value.Set(vector.Y);
					if (_gyroscope_z.Subscribed) _gyroscope_z.Value.Set(vector.Z);
					break;
				case MotionSensorType.Compass:
					double? val = sensorArgs.Value.Value;
					if (val.HasValue)
					{
						_compass.Value.Set(sensorArgs.Value.Value.Value);
					}
					break;
			}
		}*/
	}
}

