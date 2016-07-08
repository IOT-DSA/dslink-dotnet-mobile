using CoreLocation;
using CoreMotion;
using Foundation;

namespace DSAMobile.Sensors
{
    /// <summary>
    /// iOS sensors implementation
    /// 
    /// Supports:
    /// - Accelerometer
    /// - Gyroscope
    /// - Device Rotation
    /// - Compass
    /// 
    /// Does not support:
    /// - Light level
    /// </summary>
	public class iOSSensors : BaseSensors
	{
        /// <summary>
        /// iOS Motion Manager.
        /// </summary>
		private readonly CMMotionManager motionManager;

        /// <summary>
        /// iOS Location Manager.
        /// </summary>
		private readonly CLLocationManager locationManager;

        public override bool SupportsAccelerometer => true;
        public override bool SupportsGyroscope => true;
        public override bool SupportsDeviceMotion => true;
        public override bool SupportsCompass => true;
        public override bool SupportsLightLevel => false;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:DSAMobile.Sensors.iOSSensors"/> class.
        /// </summary>
		public iOSSensors()
		{
			motionManager = new CMMotionManager();
			locationManager = new CLLocationManager();
			locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
			locationManager.HeadingFilter = 1;
		}

        /// <summary>
        /// Start the specified sensor type reading.
        /// </summary>
        /// <param name="sensorType">Sensor type</param>
		public override void Start(SensorType sensorType)
		{
			switch (sensorType)
			{
				case SensorType.Accelerometer:
                    AccelerometerActive = true;
                    motionManager.AccelerometerUpdateInterval = 0.05;
					motionManager.StartAccelerometerUpdates(NSOperationQueue.MainQueue, (data, error) =>
					{
						EmitAccelerometer(new MotionVector(data.Acceleration.X, data.Acceleration.Y, data.Acceleration.Z));
					});
					break;
				case SensorType.Gyroscope:
                    GyroActive = true;
                    motionManager.GyroUpdateInterval = 0.05;
					motionManager.StartGyroUpdates(NSOperationQueue.MainQueue, (data, error) =>
					{
						EmitGyroscope(new MotionVector(data.RotationRate.x, data.RotationRate.y, data.RotationRate.z));
					});
					break;
                case SensorType.DeviceMotion:
                    DeviceMotionActive = true;
                    motionManager.DeviceMotionUpdateInterval = 0.05d;
                    motionManager.StartDeviceMotionUpdates(NSOperationQueue.MainQueue, (motion, error) =>
                    {
                        EmitDeviceMotion(new MotionVector(motion.Attitude.Roll, motion.Attitude.Pitch, motion.Attitude.Yaw));
                    });
                    break;
				case SensorType.Compass:
                    CompassActive = true;
					locationManager.UpdatedHeading += (sender, eventArgs) =>
					{
						// TODO: Fix.
						EmitCompass(eventArgs.NewHeading.TrueHeading);
					};
					locationManager.StartUpdatingHeading();
					break;
                case SensorType.LightLevel:
                    LightLevelActive = false;
                    break;
			}
		}

        /// <summary>
        /// Stop the specified sensor type reading.
        /// </summary>
        /// <param name="sensorType">Sensor type</param>
        public override void Stop(SensorType sensorType)
        {
            switch (sensorType)
            {
                case SensorType.Accelerometer:
                    AccelerometerActive = false;
                    motionManager.StopAccelerometerUpdates();
                    break;
                case SensorType.Gyroscope:
                    GyroActive = false;
                    motionManager.StopGyroUpdates();
                    break;
                case SensorType.DeviceMotion:
                    DeviceMotionActive = false;
                    motionManager.StopDeviceMotionUpdates();
                    break;
                case SensorType.Compass:
                    CompassActive = false;
                    locationManager.StopUpdatingHeading();
                    break;
                case SensorType.LightLevel:
                    LightLevelActive = false;
                    break;
            }
        }
	}
}
