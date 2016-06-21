using System;
using DSA_Mobile.DeviceSettings;
using DSA_Mobile.iOS.DeviceSettings;
using DSA_Mobile.Sensors;
using Foundation;
using UIKit;

namespace DSA_Mobile.iOS
{
	public class iOSApp : App
	{
		private nint _backgroundTask;

		protected override string StoragePath()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		}

		public override BaseSensors GetSensors()
		{
			if (_sensors == null)
			{
				_sensors = new iOSSensors();
			}
			return _sensors;
		}

        public override BaseDeviceSettings GetDeviceSettings()
        {
            if (_deviceSettings == null)
            {
                _deviceSettings = new iOSDeviceSettings(this);
            }
            return _deviceSettings;
        }

        protected override void OnStart()
        {
            base.OnStart();
            var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert |
                                                                          UIUserNotificationType.Badge |
                                                                          UIUserNotificationType.Sound,
                                                                          new NSSet());
            UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
        }

		/*
		protected override void StartLinkPlatform()
		{
			Debug.WriteLine("Starting iOS Background Task");
			_backgroundTask = UIApplication.SharedApplication.BeginBackgroundTask(() =>
			{
				Debug.WriteLine("test");
				StartLink();
			});
			FinishLongRunningTask(_backgroundTask);
		}

		protected override void StopLinkPlatform()
		{
			Debug.WriteLine("Stopping iOS Background Task");
			UIApplication.SharedApplication.EndBackgroundTask(_backgroundTask);
			StopLink();
		}*/
	}
}
