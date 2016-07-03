using System.Diagnostics;
using CoreLocation;
using CoreMotion;
using Foundation;

namespace DSAMobile.Sensors
{
	public class iOSSensors : BaseSensors
	{
		private readonly CMMotionManager motionManager;
		private readonly CLLocationManager locationManager;

		public iOSSensors()
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
			}
		}

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
            }
        }
	}
}
