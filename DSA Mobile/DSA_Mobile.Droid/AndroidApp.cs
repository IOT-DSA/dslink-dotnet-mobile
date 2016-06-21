using Android.Content;
using Android.Hardware;
using Android.OS;
using DSA_Mobile.Motion;
using DSLink;

namespace DSA_Mobile.Droid
{
    public class AndroidApp : App
    {
        private MainActivity _mainActivity;

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
            return new AndroidMotionImplementation((SensorManager)_mainActivity.GetSystemService(Context.SensorService));
        }
    }
}
