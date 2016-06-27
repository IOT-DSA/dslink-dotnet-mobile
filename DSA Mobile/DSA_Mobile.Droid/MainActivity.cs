using Android.App;
using Android.Content.PM;
using Android.OS;
using DSLink.Android;

namespace DSAMobile.Droid
{
    [
        Activity(Label = "DSAMobile",
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

            // ZXing


            // Xamarin bootstrap
            Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new AndroidApp(this));
        }
    }
}

