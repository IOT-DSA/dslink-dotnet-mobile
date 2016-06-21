using System;
using System.Diagnostics;
using DSA_Mobile.DeviceSettings;
using DSA_Mobile.iOS.DeviceSettings;
using DSA_Mobile.Motion;
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

		public override BaseMotionImplementation GetMotion()
		{
			if (_motion == null)
			{
				_motion = new iOSMotionImplementation();
			}
			return _motion;
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
