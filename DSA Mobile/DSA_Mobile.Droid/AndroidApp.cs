using Android.Content;
using Android.Hardware;
using Android.OS;
using DSA_Mobile.DeviceSettings;
using DSA_Mobile.Droid.DeviceSettings;
using DSA_Mobile.Droid.Sensors;
using DSA_Mobile.Sensors;
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

        public override BaseSensors GetSensors()
        {
            if (_sensors == null)
            {
                _sensors = new AndroidSensors((SensorManager)_mainActivity.GetSystemService(Context.SensorService));
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
