using System;

namespace DSA_Mobile.Motion
{
	public abstract class BaseMotionImplementation
	{
		public event Action<MotionVector> AccelerometerValueChanged;
		public event Action<MotionVector> GyroscopeValueChanged;
		public event Action<double> CompassValueChanged;

		public abstract void Start(SensorType sensorType);

		protected void EmitAccelerometer(MotionVector vector)
		{
			AccelerometerValueChanged?.Invoke(vector);
		}

		protected void EmitGyroscope(MotionVector vector)
		{
			GyroscopeValueChanged?.Invoke(vector);
		}

		protected void EmitCompass(double value)
		{
			CompassValueChanged?.Invoke(value);
		}
	}
}

