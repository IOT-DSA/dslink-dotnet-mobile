using Android.OS;

namespace DSA_Mobile.Droid
{
    public class AndroidApp : App
    {
        protected override string StoragePath()
        {
            return Environment.ExternalStorageDirectory.Path;
        }
    }
}
