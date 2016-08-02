using DSAMobile.Modules;
using Android.Content;
using Android.OS;
using System.Collections.Generic;
using Android.Net.Wifi;
using System;

namespace DSAMobile.Droid.DeviceSettings
{
    public class AndroidDeviceSettings : BaseDeviceSettings
    {
        private readonly MainActivity _activity;
        private readonly PowerManager _powerManager;
        private readonly WifiManager _wifiManager;
        private PowerManager.WakeLock _wakeLock;

        public override bool IsWiFiStateSupported => true;
        public override bool WiFiScanningSupported => true;
        public override bool GetWiFiInfoSupported => true;

        public AndroidDeviceSettings(AndroidApp app) : base(app)
        {
            _activity = app.MainActivity;
            _powerManager = _activity.GetSystemService(Context.PowerService) as PowerManager;
            _wifiManager = _activity.GetSystemService(Context.WifiService) as WifiManager;
            _wakeLock = _powerManager.NewWakeLock(WakeLockFlags.Full, "DSAMobile");
        }

        public override bool ScreenOn()
        {
            return _powerManager.IsInteractive;
        }

        public override void SetScreenIdle(bool screenIdleState)
        {
            if (screenIdleState)
            {
                _wakeLock.Acquire();
            }
            else
            {
                _wakeLock.Release();
            }
        }

        public override bool WiFiEnabled
        {
            get
            {
                return _wifiManager.IsWifiEnabled;
            }
            set
            {
                _wifiManager.SetWifiEnabled(value);
            }
        }

        public override WiFiInfo WiFiInfo
        {
            get
            {
                var connInfo = _wifiManager.ConnectionInfo;
                var ret = new WiFiInfo
                {
                    Ssid = connInfo.SSID,
                    Bssid = connInfo.BSSID,
                    SignalLevel = connInfo.Rssi,
                    Frequency = connInfo.Frequency,
                    HiddenSsid = connInfo.HiddenSSID,
                    LinkSpeed = connInfo.LinkSpeed,
                    IpAddress = new IpAddress(connInfo.IpAddress)
                };
                return ret;
            }
        }

        public override void ScanWiFiPoints(Action<List<WiFiAccessPoint>> callback)
        {
            _activity.RegisterReceiver(new WiFiReceiver(callback, this),
                                       new IntentFilter(WifiManager.ScanResultsAvailableAction));
            _wifiManager.StartScan();
        }

        private class WiFiReceiver : BroadcastReceiver
        {
            private Action<List<WiFiAccessPoint>> _callback;
            private readonly AndroidDeviceSettings _deviceSettings;

            public WiFiReceiver(Action<List<WiFiAccessPoint>> callback,
                                AndroidDeviceSettings deviceSettings)
            {
                _callback = callback;
                _deviceSettings = deviceSettings;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                var results = _deviceSettings._wifiManager.ScanResults;
                var accessPoints = new List<WiFiAccessPoint>();
                foreach (ScanResult result in results)
                {
                    accessPoints.Add(new WiFiAccessPoint
                    {
                        Ssid = result.Ssid,
                        Bssid = result.Bssid,
                        SignalLevel = result.Level,
                        Frequency = result.Frequency
                    });
                }
                _callback(accessPoints);
            }
        }
    }
}
