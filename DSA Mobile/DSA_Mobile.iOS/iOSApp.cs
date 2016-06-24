using System;
using DSAMobile.DeviceSettings;
using DSAMobile.iOS.DeviceSettings;
using DSAMobile.iOS.HealthKit;
using DSAMobile.Sensors;

namespace DSAMobile.iOS
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
