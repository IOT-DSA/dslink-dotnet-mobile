using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Fitness;
using Android.Gms.Fitness.Data;
using Android.Gms.Fitness.Request;
using Android.OS;
using Java.Util.Concurrent;
using Debug = System.Diagnostics.Debug;

namespace DSAMobile.Droid
{
    /*public class FitnessModule
    {
        private const string TAG = "DSAFitness";
        private ClientConnectionCallback _clientConnectionCallback;
        private GoogleApiClient _client;
        private Activity _activity;

        public FitnessModule(Activity activity)
        {
            _activity = activity;
            stuff();
        }

        public async void stuff()
        {
            bool authInProgress = false;
            _clientConnectionCallback = new ClientConnectionCallback();
            _client = new GoogleApiClient.Builder(_activity.ApplicationContext)
                .AddApi(FitnessClass.HISTORY_API)
                .AddScope(new Scope(Scopes.FitnessActivityReadWrite))
                .AddConnectionCallbacks(_clientConnectionCallback)
                .AddOnConnectionFailedListener(result =>
                {
                    if (!result.HasResolution)
                    {
                        // Show the localized error dialog
                        GooglePlayServicesUtil.GetErrorDialog(result.ErrorCode, _activity, 0).Show();
                        return;
                    }
                    // The failure has a resolution. Resolve it.
                    // Called typically when the app is not yet authorized, and an
                    // authorization dialog is displayed to the user.
                    if (!authInProgress)
                    {
                        try
                        {
                            //Log.Info(TAG, "Attempting to resolve failed connection");
                            authInProgress = true;
                            result.StartResolutionForResult(_activity, 1);
                        }
                        catch (IntentSender.SendIntentException e)
                        {
                            //Log.Error(TAG, "Exception while starting resolution activity", e);
                        }
                    }
                }).Build();

            DateTime endTime = DateTime.Now;
            DateTime startTime = endTime.Subtract(TimeSpan.FromDays(7));
            long endTimeElapsed = MillisecondsFromEpoch(endTime);
            long startTimeElapsed = MillisecondsFromEpoch(startTime);

            var readRequest = new DataReadRequest.Builder()
                .Aggregate(DataType.TypeStepCountDelta, DataType.AggregateStepCountDelta)
                .BucketByTime(1, TimeUnit.Days)
                .SetTimeRange(startTimeElapsed, endTimeElapsed, TimeUnit.Milliseconds)
                .Build();
            var dataReadResult = await FitnessClass.HistoryApi.ReadDataAsync(_client, readRequest);
            foreach (Bucket bucket in dataReadResult.Buckets)
            {
                Debug.WriteLine(bucket.Activity);
            }
            Debug.WriteLine("fdsa");
        }

        private static long MillisecondsFromEpoch(DateTime dateTime)
        {
            return (long)dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        class ClientConnectionCallback : Java.Lang.Object, GoogleApiClient.IConnectionCallbacks
        {
            public Action OnConnectedImpl { get; set; }

            public void OnConnected(Bundle connectionHint)
            {
                //Log.Info(TAG, "Connected!!!");

                Task.Run(OnConnectedImpl);
            }

            public void OnConnectionSuspended(int cause)
            {
                if (cause == GoogleApiClient.ConnectionCallbacks.CauseNetworkLost)
                {
                    //Log.Info(TAG, "Connection lost. Cause: Network Lost.");
                }
                else if (cause == GoogleApiClient.ConnectionCallbacks.CauseServiceDisconnected)
                {
                    //Log.Info(TAG, "Connection lost. Reason: Service Disconnected");
                }
            }
        }
    }*/
}
