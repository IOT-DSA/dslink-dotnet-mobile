using System;
using DSA_Mobile.DeviceSettings;
using Android.Provider;
using Android.Content;
using Android.OS;

namespace DSA_Mobile.Droid.DeviceSettings
{
    public class AndroidDeviceSettings : BaseDeviceSettings
    {
        private PowerManager _powerManager;

        public AndroidDeviceSettings(AndroidApp app) : base(app)
        {
            _powerManager = app._mainActivity.GetSystemService(Context.PowerService) as PowerManager;
        }

        public override bool ScreenOn()
        {
            return _powerManager.IsInteractive;
        }
    }
}

