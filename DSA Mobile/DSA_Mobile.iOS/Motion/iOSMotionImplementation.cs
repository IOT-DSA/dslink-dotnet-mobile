using System.Diagnostics;
using CoreLocation;
using CoreMotion;
using Foundation;

namespace DSA_Mobile.Motion
{
	public class iOSMotionImplementation : BaseMotionImplementation
	{
		private readonly CMMotionManager motionManager;
		private readonly CLLocationManager locationManager;

		public iOSMotionImplementation()
		{
			motionManager = new CMMotionManager();
			locationManager = new CLLocationManager();
			locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
			locationManager.HeadingFilter = 1;
		}

		public override void Start(SensorType sensorType)
		{
			switch (sensorType)
			{
				case SensorType.Accelerometer:
                    motionManager.AccelerometerUpdateInterval = 0.05;
					motionManager.StartAccelerometerUpdates(NSOperationQueue.MainQueue, (data, error) =>
					{
						EmitAccelerometer(new MotionVector(data.Acceleration.X, data.Acceleration.Y, data.Acceleration.Z));
					});
					break;
				case SensorType.Gyroscope:
                    motionManager.GyroUpdateInterval = 0.05;
					motionManager.StartGyroUpdates(NSOperationQueue.MainQueue, (data, error) =>
					{
						EmitGyroscope(new MotionVector(data.RotationRate.x, data.RotationRate.y, data.RotationRate.z));
					});
					break;
                case SensorType.DeviceMotion:
                    motionManager.DeviceMotionUpdateInterval = 0.05d;
                    motionManager.StartDeviceMotionUpdates(NSOperationQueue.MainQueue, (motion, error) =>
                    {
                        EmitDeviceMotion(new MotionVector(motion.Attitude.Roll, motion.Attitude.Pitch, motion.Attitude.Yaw));
                    });
                    break;
				case SensorType.Compass:
					locationManager.UpdatedHeading += (sender, eventArgs) =>
					{
						// TODO: Fix.
						EmitCompass(eventArgs.NewHeading.TrueHeading);
					};
					locationManager.StartUpdatingHeading();
					break;
			}
		}
	}
}
