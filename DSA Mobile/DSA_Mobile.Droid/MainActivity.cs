using Android.App;
using Android.Content.PM;
using Android.OS;
using DSLink.Android;
using FormsPlugin.Iconize.Droid;
using Plugin.Iconize;
using Plugin.Iconize.Fonts;

namespace DSA_Mobile.Droid
{
    [
        Activity(Label = "DSA Mobile",
                 Icon = "@drawable/icon",
                 MainLauncher = true,
                 ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
                 ScreenOrientation = ScreenOrientation.Portrait)
    ]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            AndroidPlatform.Initialize();

            // Iconize
            //Iconize.With(new MaterialModule());
            //IconControls.Init(Resource.Id.);

            // Xamarin bootstrap
            Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new AndroidApp(this));
        }
    }
}

