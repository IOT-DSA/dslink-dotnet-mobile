using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using System.Threading;

namespace DSAMobile.Droid
{
    [Service]
    public class AndroidService : Service
    {
        private const string Tag = "DSLinkService";
        private Notification _serviceNotification;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnCreate()
        {
            Notification.Builder builder = new Notification.Builder(ApplicationContext);
            builder.SetContentTitle("DSAMobile");
            builder.SetContentText("DSAMobile service is running");
            builder.SetOngoing(true);
            _serviceNotification = builder.Build();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            new Thread(new ThreadStart(() =>
            {
                Log.Info(Tag, "Starting service");
                ((AndroidApp)App.Instance).BaseStartLink();
                Log.Info(Tag, "Started service");
            })).Start();

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Notify(0, _serviceNotification);
            StartForeground(0, _serviceNotification);

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            StopForeground(true);
            new Thread(new ThreadStart(() =>
            {
                Log.Info(Tag, "Stopping service");
                ((AndroidApp)App.Instance).BaseStopLink();
                Log.Info(Tag, "Stopped service");
            })).Start();

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Cancel(0);
        }
    }
}
