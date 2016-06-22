using System;
using Android.Content;
using Android.Provider;
using DSA_Mobile.Camera;

namespace DSA_Mobile.Droid.Camera
{
    public class AndroidCamera : BaseCamera
    {
        private AndroidApp _androidApp => _app as AndroidApp;

        public AndroidCamera(AndroidApp app) : base(app)
        {
        }

        public override void OpenCamera()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
        }
    }
}

