using System.Collections.Generic;
using Android.Content;
using Android.Hardware;
using Android.OS;
using DSAMobile.DeviceSettings;
using DSAMobile.Droid.DeviceSettings;
using DSAMobile.Droid.Sensors;
using DSAMobile.Sensors;
using DSLink;

namespace DSAMobile.Droid
{
    public class AndroidApp : App
    {
        public MainActivity MainActivity;

        protected override string StoragePath()
        {
            return Environment.ExternalStorageDirectory.Path;
        }

        public override DSLink PlatformDSLink(Configuration config, List<BaseModule> modules) => new AndroidDSLink(config, this, MainActivity, modules);

        public override void InitModules()
        {
            base.InitModules();
        }

        public override BaseSensors GetSensors()
        {
            if (_sensors == null)
            {
                _sensors = new AndroidSensors((SensorManager)MainActivity.GetSystemService(Context.SensorService));
            }
            return _sensors;
        }

        public override BaseDeviceSettings GetDeviceSettings()
        {
            if (_deviceSettings == null)
            {
                _deviceSettings = new AndroidDeviceSettings(this);
            }
            return _deviceSettings;
        }
    }
}
