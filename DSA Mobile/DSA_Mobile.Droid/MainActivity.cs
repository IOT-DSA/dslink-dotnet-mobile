using Android.App;
using Android.Content.PM;
using Android.OS;
using DSLink.Android;

namespace DSA_Mobile.Droid
{
    [Activity(Label = "DSA_Mobile", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            AndroidPlatform.Initialize();
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new AndroidApp());
        }
    }
}

