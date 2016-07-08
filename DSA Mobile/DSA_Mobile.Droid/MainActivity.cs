using Android.App;
using Android.Content.PM;
using Android.OS;
using DSLink.Android;
using Xamarin.Forms.Platform.Android;
using ZXing.Net.Mobile.Forms.Android;
using ZXingPlatform = ZXing.Net.Mobile.Forms.Android.Platform;

namespace DSAMobile.Droid
{
    [
        Activity(Label = "DSAMobile",
                 Icon = "@drawable/icon",
                 MainLauncher = true,
                 ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
                 ScreenOrientation = ScreenOrientation.Portrait)
    ]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            AndroidPlatform.Initialize();

            // Iconize
            //Iconize.With(new MaterialModule());
            //IconControls.Init(Resource.Id.);

            // ZXing
            ZXingPlatform.Init();

            // Xamarin bootstrap
            Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new AndroidApp
            {
                MainActivity = this
            });
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

