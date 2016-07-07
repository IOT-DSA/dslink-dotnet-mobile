using System;
using DSAMobile.DeviceSettings;
using Android.Provider;
using Android.Content;
using Android.OS;

namespace DSAMobile.Droid.DeviceSettings
{
    public class AndroidDeviceSettings : BaseDeviceSettings
    {
        private readonly PowerManager _powerManager;

        public AndroidDeviceSettings(AndroidApp app) : base(app)
        {
            _powerManager = app.MainActivity.GetSystemService(Context.PowerService) as PowerManager;
        }

        public override bool ScreenOn()
        {
            return _powerManager.IsInteractive;
        }
    }
}

