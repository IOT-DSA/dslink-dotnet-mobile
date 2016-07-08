using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DSLink.Nodes;
using DSLink.Nodes.Actions;
using DSLink.Request;

namespace DSAMobile.DeviceSettings
{
    public class DeviceSettingsModule : BaseModule
    {
        private App _app;
        private readonly BaseDeviceSettings _deviceSettings;
        private CancellationTokenSource _updateToken;
        private readonly Task _updateTask;

        private Node _screenStatus;
        private Node _screenIdleTimer;
        private Node _wifiEnabled;
        private Node _scanWiFiNetworks;

        private Node _wifi;
        private Node _wifi_ssid;
        private Node _wifi_bssid;
        private Node _wifi_signalLevel;
        private Node _wifi_frequency;
        private Node _wifi_hiddenSsid;
        private Node _wifi_linkSpeed;
        private Node _wifi_ipAddress;

        public bool Supported => true;

        public DeviceSettingsModule(BaseDeviceSettings deviceSettings, App app)
        {
            _app = app;
            _deviceSettings = deviceSettings;
            _updateToken = new CancellationTokenSource();
            _updateTask = new Task(Update, _updateToken.Token);
        }

        public bool RequestPermissions()
        {
            return true;
        }

        public void AddNodes(Node superRoot)
        {
            _screenStatus = superRoot.CreateChild("screen_status")
                     .SetDisplayName("Screen Status")
                     .SetType("bool")
                     .SetValue(_deviceSettings.ScreenOn())
                     .BuildNode();

            _screenIdleTimer = superRoot.CreateChild("screen_idle_timer")
                                        .SetDisplayName("Screen Idle Timer")
                                        .SetType("bool")
                                        .SetWritable(Permission.Write)
                                        .SetValue(false)
                                        .BuildNode();
            _screenIdleTimer.Value.OnSet += (Value val) =>
            {
                _deviceSettings.SetScreenIdle(val.Get());
            };

            if (_deviceSettings.IsWiFiStateSupported)
            {
                _wifiEnabled = superRoot.CreateChild("wifi_enabled")
                                        .SetDisplayName("WiFi Enabled")
                                        .SetType("bool")
                                        .SetWritable(Permission.Config)
                                        .BuildNode();

                _wifiEnabled.Value.OnSet += (Value val) =>
                {
                    _deviceSettings.WiFiEnabled = ((Value)val).Get();
                };
            }

            if (_deviceSettings.WiFiScanningSupported)
            {
                _scanWiFiNetworks = superRoot.CreateChild("scan_wifi_networks")
                                             .SetDisplayName("Scan for WiFi Networks")
                                             .SetActionGroup(ActionGroups.Settings)
                                             .SetInvokable(Permission.Config)
                                             .AddColumn(new Column("SSID", "string"))
                                             .AddColumn(new Column("BSSID", "string"))
                                             .AddColumn(new Column("Signal Level", "number"))
                                             .AddColumn(new Column("Frequency", "number"))
                                             .SetConfig("result", new Value("table"))
                                             .SetAction(new Action(Permission.Config, ScanWiFiNetworks))
                                             .BuildNode();
            }

            if (_deviceSettings.GetWiFiInfoSupported)
            {
                _wifi = superRoot.CreateChild("wifi")
                                 .SetDisplayName("WiFi")
                                 .BuildNode();

                _wifi_ssid = _wifi.CreateChild("ssid")
                                  .SetDisplayName("SSID")
                                   .SetType("string")
                                   .BuildNode();

                _wifi_bssid = _wifi.CreateChild("bssid")
                                   .SetDisplayName("BSSID")
                                   .SetType("string")
                                   .BuildNode();

                _wifi_signalLevel = _wifi.CreateChild("signal_level")
                                         .SetDisplayName("Signal Level")
                                         .SetType("number")
                                         .BuildNode();

                _wifi_frequency = _wifi.CreateChild("frequency")
                                       .SetDisplayName("Frequency")
                                       .SetType("number")
                                       .BuildNode();

                _wifi_hiddenSsid = _wifi.CreateChild("hidden_ssid")
                                        .SetDisplayName("Hidden SSID")
                                        .SetType("bool")
                                        .BuildNode();

                _wifi_linkSpeed = _wifi.CreateChild("link_speed")
                                       .SetDisplayName("Link Speed")
                                       .SetAttribute("unit", new Value("Mbps"))
                                       .SetType("number")
                                       .BuildNode();

                _wifi_ipAddress = _wifi.CreateChild("ip_address")
                                       .SetDisplayName("IP Address")
                                       .SetType("string")
                                       .BuildNode();
            }

            _updateTask.Start();
        }

        public void RemoveNodes()
        {
            _updateToken.Cancel();
            _screenStatus.RemoveFromParent();
            _screenIdleTimer.RemoveFromParent();
            if (_deviceSettings.IsWiFiStateSupported)
            {
                _wifiEnabled.RemoveFromParent();
            }
            if (_deviceSettings.WiFiScanningSupported)
            {
                _scanWiFiNetworks.RemoveFromParent();
            }
        }

        private void Update()
        {
            while (!_updateTask.IsCanceled)
            {
                _screenStatus.Value.Set(_deviceSettings.ScreenOn());

                if (_deviceSettings.IsWiFiStateSupported)
                {
                    _wifiEnabled.Value.Set(_deviceSettings.WiFiEnabled);
                }

                if (_deviceSettings.GetWiFiInfoSupported)
                {
                    var info = _deviceSettings.WiFiInfo;
                    _wifi_ssid.Value.Set(info.Ssid);
                    _wifi_bssid.Value.Set(info.Bssid);
                    _wifi_signalLevel.Value.Set(info.SignalLevel);
                    _wifi_frequency.Value.Set(info.Frequency);
                    _wifi_hiddenSsid.Value.Set(info.HiddenSsid);
                    _wifi_linkSpeed.Value.Set(info.LinkSpeed);
                    _wifi_ipAddress.Value.Set(info.IpAddress.AddressString);
                }

                _updateTask.Wait(1000);
            }
        }

        private void ScanWiFiNetworks(Dictionary<string, Value> parameters, InvokeRequest request)
        {
            _deviceSettings.ScanWiFiPoints((List<WiFiAccessPoint> accessPoints) =>
            {
                var updates = new List<dynamic>();
                foreach (WiFiAccessPoint accessPoint in accessPoints)
                {
                    updates.Add(new List<dynamic>
                    {
                        accessPoint.Ssid,
                        accessPoint.Bssid,
                        accessPoint.SignalLevel,
                        accessPoint.Frequency
                    });
                }
                request.SendUpdates(updates, true);
            });
        }
    }
}
