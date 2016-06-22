using System;
using DSA_Mobile.DeviceSettings;
using DSA_Mobile.iOS.DeviceSettings;
using DSA_Mobile.iOS.HealthKit;
using DSA_Mobile.Sensors;

namespace DSA_Mobile.iOS
{
	public class iOSApp : App
	{
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
        }

        protected override void InitializeDSLink()
        {
            base.InitializeDSLink();
            //_dslink.RegisterModule(new HealthKitModule());
        }
	}
}
