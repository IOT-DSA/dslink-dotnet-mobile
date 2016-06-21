using Android.Content;
using Android.Hardware;
using Android.OS;
using DSA_Mobile.DeviceSettings;
using DSA_Mobile.Droid.DeviceSettings;
using DSA_Mobile.Motion;
using DSLink;

namespace DSA_Mobile.Droid
{
    public class AndroidApp : App
    {
        public MainActivity _mainActivity;

        public AndroidApp(MainActivity activity)
        {
            _mainActivity = activity;
        }

        protected override string StoragePath()
        {
            return Environment.ExternalStorageDirectory.Path;
        }

        public override DSLink PlatformDSLink(Configuration config) => new AndroidDSLink(config, this, _mainActivity);

        public override BaseMotionImplementation GetMotion()
        {
            if (_motion == null)
            {
                _motion = new AndroidMotionImplementation((SensorManager)_mainActivity.GetSystemService(Context.SensorService));
            }
            return _motion;
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
