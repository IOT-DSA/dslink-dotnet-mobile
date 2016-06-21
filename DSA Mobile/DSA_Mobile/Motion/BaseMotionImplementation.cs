using System;

namespace DSA_Mobile.Motion
{
	public abstract class BaseMotionImplementation
	{
		public event Action<MotionVector> AccelerometerValueChanged;
		public event Action<MotionVector> GyroscopeValueChanged;
        public event Action<MotionVector> DeviceMotionValueChanged;
		public event Action<double> CompassValueChanged;

		public abstract void Start(SensorType sensorType);

		public void EmitAccelerometer(MotionVector vector)
		{
			AccelerometerValueChanged?.Invoke(vector);
		}

		public void EmitGyroscope(MotionVector vector)
		{
			GyroscopeValueChanged?.Invoke(vector);
		}

        public void EmitDeviceMotion(MotionVector vector)
        {
            DeviceMotionValueChanged?.Invoke(vector);
        }

		public void EmitCompass(double value)
		{
			CompassValueChanged?.Invoke(value);
		}
	}
}

