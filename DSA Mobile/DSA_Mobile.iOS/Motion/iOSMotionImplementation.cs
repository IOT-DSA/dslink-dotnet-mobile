using System.Diagnostics;
using CoreMotion;
using Foundation;

namespace DSA_Mobile.Motion
{
	public class iOSMotionImplementation : BaseMotionImplementation
	{
		private readonly CMMotionManager motionManager;

		public iOSMotionImplementation()
		{
			motionManager = new CMMotionManager();
		}

		public override void Start(SensorType sensorType)
		{
			switch (sensorType)
			{
				case SensorType.Accelerometer:
					motionManager.StartAccelerometerUpdates(NSOperationQueue.MainQueue, (data, error) => {
						EmitAccelerometer(new MotionVector(data.Acceleration.X, data.Acceleration.Y, data.Acceleration.Z));
					});
					break;
			}
		}
	}
}
