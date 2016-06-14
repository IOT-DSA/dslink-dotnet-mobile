using DSLink.Nodes;

using System.Diagnostics;
using DSA_Mobile.Motion;

namespace DSA_Mobile
{
	public class MotionModule
	{
		private readonly BaseMotionImplementation _motionImpl;
		private readonly Node _motion;
		private readonly Node _accelerometer;
		private readonly Node _accelerometer_x;
		private readonly Node _accelerometer_y;
		private readonly Node _accelerometer_z;
		private readonly Node _gyroscope;
		private readonly Node _gyroscope_x;
		private readonly Node _gyroscope_y;
		private readonly Node _gyroscope_z;
		private readonly Node _compass;

		public MotionModule(Node superRoot, DSA_Mobile.App app)
		{
			_motion = superRoot.CreateChild("Motion").BuildNode();
			_motionImpl = app.GetMotion();
			_accelerometer = _motion.CreateChild("Accelerometer").BuildNode();
			_accelerometer_x = _accelerometer.CreateChild("X")
			                                 .SetType("number")
			                                 .BuildNode();
			_accelerometer_y = _accelerometer.CreateChild("Y")
			                                 .SetType("number")
			                                 .BuildNode();
			_accelerometer_z = _accelerometer.CreateChild("Z")
											 .SetType("number")
			                                 .BuildNode();

			_gyroscope = _motion.CreateChild("Gyroscope").BuildNode();
			_gyroscope_x = _gyroscope.CreateChild("X")
			                       .SetType("number")
			                       .BuildNode();
			_gyroscope_y = _gyroscope.CreateChild("Y")
								   .SetType("number")
								   .BuildNode();
			_gyroscope_z = _gyroscope.CreateChild("Z")
								   .SetType("number")
								   .BuildNode();

			_compass = _motion.CreateChild("Compass")
			                  .SetType("number")
			                  .BuildNode();

			_motionImpl.Start(SensorType.Accelerometer);
			_motionImpl.Start(SensorType.Gyroscope);
			_motionImpl.Start(SensorType.Compass);
			_motionImpl.AccelerometerValueChanged += (MotionVector vector) =>
			{
				_accelerometer_x.Value.Set(vector.X);
				_accelerometer_y.Value.Set(vector.Y);
				_accelerometer_z.Value.Set(vector.Z);
			};
			_motionImpl.GyroscopeValueChanged += (MotionVector vector) =>
			{
				_gyroscope_x.Value.Set(vector.X);
				_gyroscope_y.Value.Set(vector.Y);
				_gyroscope_z.Value.Set(vector.Z);
			};
			_motionImpl.CompassValueChanged += _compass.Value.Set;
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

