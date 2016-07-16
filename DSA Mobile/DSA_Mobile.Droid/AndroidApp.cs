using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Hardware;
using DSAMobile.DeviceSettings;
using DSAMobile.Droid.DeviceSettings;
using DSAMobile.Droid.Sensors;
using DSAMobile.Sensors;
using DSLink;
using Environment = Android.OS.Environment;

namespace DSAMobile.Droid
{
    public class AndroidApp : App
    {
        public MainActivity MainActivity;
        public Action BaseStopLink;
        public Action BaseStartLink;

        public AndroidApp()
        {
            BaseStartLink = base.StartLink;
            BaseStopLink = base.StopLink;
        }

        protected override string StoragePath()
        {
            return Environment.ExternalStorageDirectory.Path;
        }

        public override DSLink PlatformDSLink(Configuration config, List<BaseModule> modules) => new AndroidDSLink(config, this, MainActivity, modules);

        public override void StartLink()
        {
            MainActivity.StartService(new Intent(MainActivity, typeof(AndroidService)));
        }

        public override void StopLink()
        {
            MainActivity.StopService(new Intent(MainActivity, typeof(AndroidService)));
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
