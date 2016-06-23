using System;

namespace DSA_Mobile.Sensors
{
	public abstract class BaseSensors
	{
		public event Action<MotionVector> AccelerometerValueChanged;
		public event Action<MotionVector> GyroscopeValueChanged;
        public event Action<MotionVector> DeviceMotionValueChanged;
		public event Action<double> CompassValueChanged;
        public event Action<double> AirPressureValueChanged;
        public event Action<double> LightLevelValueChanged;

        public bool AccelerometerActive
        {
            protected set;
            get;
        }

        public bool GyroActive
        {
            protected set;
            get;
        }

        public bool DeviceMotionActive
        {
            protected set;
            get;
        }

        public bool CompassActive
        {
            protected set;
            get;
        }

		public abstract void Start(SensorType sensorType);

        public abstract void Stop(SensorType sensorType);

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

        public void EmitAirPressure(double value)
        {
            AirPressureValueChanged?.Invoke(value);
        }

        public void EmitLightLevel(double value)
        {
            LightLevelValueChanged?.Invoke(value);
        }
	}
}

