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
		}

		public override void Start(SensorType sensorType)
		{
			switch (sensorType)
			{
				case SensorType.Accelerometer:
					motionManager.StartAccelerometerUpdates(NSOperationQueue.MainQueue, (data, error) =>
					{
						EmitAccelerometer(new MotionVector(data.Acceleration.X, data.Acceleration.Y, data.Acceleration.Z));
					});
					break;
				case SensorType.Gyroscope:
					motionManager.StartGyroUpdates(NSOperationQueue.MainQueue, (data, error) =>
					{
						EmitGyroscope(new MotionVector(data.RotationRate.x, data.RotationRate.y, data.RotationRate.z));
					});
					break;
				case SensorType.Compass:
					locationManager.StartUpdatingHeading();
					locationManager.UpdatedHeading += (sender, eventArgs) =>
					{
						EmitCompass(eventArgs.NewHeading.TrueHeading);
					};
					break;
			}
		}
	}
}
