using System;
using Android.App;
using Android.Content;
using Android.OS;

namespace DSA_Mobile.Droid
{
    [Service]
    public class DSLinkService : Service
    {
        public DSLinkService()
        {
        }

        public override IBinder OnBind(Intent intent)
        {
            //return base.OnBind(intent);
            return null;
        }

        public override ComponentName StartService(Intent service)
        {
            return base.StartService(service);
        }

        public override bool StopService(Intent name)
        {
            return base.StopService(name);
        }
    }
}

