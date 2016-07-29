using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DSAMobile.Utils;
using DSLink.Nodes;
using DSLink.Nodes.Actions;
using DSLink.Request;
using Newtonsoft.Json.Linq;
using ValueType = DSLink.Nodes.ValueType;

namespace DSAMobile.DeviceSettings
{
    public class DeviceSettingsModule : BaseModule
    {
        private App _app;
        private readonly BaseDeviceSettings _deviceSettings;
        private CancellationTokenSource _updateToken;
        private Task _updateTask;

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
        }

        public bool RequestPermissions()
        {
            return true;
        }

        public void AddNodes(Node superRoot)
        {
            _screenStatus = superRoot.CreateChild("screen_status")
                     .SetDisplayName("Screen Status")
                     .SetType(ValueType.Boolean)
                     .SetValue(_deviceSettings.ScreenOn())
                     .BuildNode();

            _screenIdleTimer = superRoot.CreateChild("screen_idle_timer")
                                        .SetDisplayName("Screen Idle Timer")
                                        .SetType(ValueType.Boolean)
                                        .SetWritable(Permission.Write)
                                        .SetValue(false)
                                        .BuildNode();
            _screenIdleTimer.Value.OnSet += (Value val) =>
            {
                _deviceSettings.SetScreenIdle(val.Boolean);
            };

            if (_deviceSettings.IsWiFiStateSupported)
            {
                _wifiEnabled = superRoot.CreateChild("wifi_enabled")
                                        .SetDisplayName("WiFi Enabled")
                                        .SetType(ValueType.Boolean)
                                        .SetWritable(Permission.Config)
                                        .BuildNode();

                _wifiEnabled.Value.OnSet += (val) =>
                {
                    _deviceSettings.WiFiEnabled = val.Boolean;
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
                                             .SetAction(new ActionHandler(Permission.Config, ScanWiFiNetworks))
                                             .BuildNode();
            }

            if (_deviceSettings.GetWiFiInfoSupported)
            {
                _wifi = superRoot.CreateChild("wifi")
                                 .SetDisplayName("WiFi")
                                 .BuildNode();

                _wifi_ssid = _wifi.CreateChild("ssid")
                                  .SetDisplayName("SSID")
                                   .SetType(ValueType.String)
                                   .BuildNode();

                _wifi_bssid = _wifi.CreateChild("bssid")
                                   .SetDisplayName("BSSID")
                                   .SetType(ValueType.String)
                                   .BuildNode();

                _wifi_signalLevel = _wifi.CreateChild("signal_level")
                                         .SetDisplayName("Signal Level")
                                         .SetType(ValueType.Number)
                                         .BuildNode();

                _wifi_frequency = _wifi.CreateChild("frequency")
                                       .SetDisplayName("Frequency")
                                       .SetType(ValueType.Number)
                                       .BuildNode();

                _wifi_hiddenSsid = _wifi.CreateChild("hidden_ssid")
                                        .SetDisplayName("Hidden SSID")
                                        .SetType(ValueType.Boolean)
                                        .BuildNode();

                _wifi_linkSpeed = _wifi.CreateChild("link_speed")
                                       .SetDisplayName("Link Speed")
                                       .SetAttribute("unit", new Value("Mbps"))
                                       .SetType(ValueType.Number)
                                       .BuildNode();

                _wifi_ipAddress = _wifi.CreateChild("ip_address")
                                       .SetDisplayName("IP Address")
                                       .SetType(ValueType.String)
                                       .BuildNode();
            }
        }

        public void Start()
        {
            _updateToken = new CancellationTokenSource();
            _updateTask = Repeat.Interval(TimeSpan.FromSeconds(1), Update, _updateToken.Token);
        }

        public void Stop()
        {
            _updateToken.Cancel();
        }

        private void Update()
        {
            _screenStatus.Value.Set(_deviceSettings.ScreenOn());

            if (_deviceSettings.IsWiFiStateSupported)
            {
                _wifiEnabled.Value.Set(_deviceSettings.WiFiEnabled);
            }

            if (_deviceSettings.GetWiFiInfoSupported)
            {
                var info = _deviceSettings.WiFiInfo;
                if (_wifi_ssid.Subscribed)
                {
                    _wifi_ssid.Value.Set(info.Ssid);
                }

                if (_wifi_bssid.Subscribed)
                {
                    _wifi_bssid.Value.Set(info.Bssid);
                }

                if (_wifi_signalLevel.Subscribed)
                {
                    _wifi_signalLevel.Value.Set(info.SignalLevel);
                }

                if (_wifi_frequency.Subscribed)
                {
                    _wifi_frequency.Value.Set(info.Frequency);
                }

                if (_wifi_hiddenSsid.Subscribed)
                {
                    _wifi_hiddenSsid.Value.Set(info.HiddenSsid);
                }

                if (_wifi_linkSpeed.Subscribed)
                {
                    _wifi_linkSpeed.Value.Set(info.LinkSpeed);
                }

                if (_wifi_ipAddress.Subscribed)
                {
                    _wifi_ipAddress.Value.Set(info.IpAddress.AddressString);
                }
            }
        }

        private async void ScanWiFiNetworks(InvokeRequest request)
        {
            _deviceSettings.ScanWiFiPoints((List<WiFiAccessPoint> accessPoints) =>
            {
                Table table = new Table();
                foreach (WiFiAccessPoint accessPoint in accessPoints)
                {
                    table.Add(new Row
                    {
                        accessPoint.Ssid,
                        accessPoint.Bssid,
                        accessPoint.SignalLevel,
                        accessPoint.Frequency
                    });
                }
                request.UpdateTable(table);
                request.Close();
            });
        }
    }
}
