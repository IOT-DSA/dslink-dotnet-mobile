using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;

namespace DSAMobile.Droid
{
    [Service]
    public class AndroidService : Service
    {
        private const string Tag = "DSLinkService";

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Log.Info(Tag, "Starting service");
            ((AndroidApp)App.Instance).BaseStartLink();
            Log.Info(Tag, "Started service");

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            Log.Info(Tag, "Stopping service");
            ((AndroidApp)App.Instance).BaseStopLink();
            Log.Info(Tag, "Stopped service");
        }
    }
}
