using System;
using System.Collections.Generic;
using System.Diagnostics;
using DSAMobile.Modules;
using DSAMobile.Pages;
using DSAMobile.Sensors;
using DSLink;
using DSLink.Util.Logger;
using Xamarin.Forms;

namespace DSAMobile
{
    public abstract class App : Application
    {
        public static App Instance;

        // TODO: Make static?
        public DSLink DSLink
        {
            get;
            private set;
        }
        public readonly TabHost TabHost;
        public bool Disabled = true;
        public bool AllowServiceToggle = true;
        protected BaseSensors _sensors;
        protected BaseDeviceSettings _deviceSettings;
        protected List<BaseModule> _enabledModules;

        protected App()
        {
            Instance = this;
            TabHost = new TabHost();
            MainPage = TabHost;
            _enabledModules = new List<BaseModule>();
        }

        protected override void OnStart()
        {
            // TODO: We should probably use this.
        }

        protected override void OnSleep()
        {
            // TODO: We should probably use this.
        }

        protected override void OnResume()
        {
            // TODO: We should probably use this.
        }

        public virtual void InitModules()
        {
            _enabledModules.Add(new BatteryModule());
            _enabledModules.Add(new DeviceInfoModule());
            _enabledModules.Add(new DeviceSettingsModule(GetDeviceSettings(), this));
            _enabledModules.Add(new SensorsModule(GetSensors()));
            _enabledModules.Add(new NotificationModule());
            _enabledModules.Add(new CameraModule());
            _enabledModules.Add(new VibrateModule());
            _enabledModules.Add(new ContactsModule());
            _enabledModules.Add(new ConnectivityModule());
            _enabledModules.Add(new CommunicationsModule());
            _enabledModules.Add(new LocationModule());
            _enabledModules.Add(new ZXingModule());
            _enabledModules.Add(new TTSModule());
            _enabledModules.Add(new FlashModule());
            _enabledModules.Add(new ShareModule());
        }

        public virtual void StartLink()
        {
            AllowServiceToggle = false;
            SetDSLinkStatus("DSLink is connecting");
            try
            {
                if (DSLink == null)
                {
                    InitModules();
                    InitializeDSLink();
                }
                else
                {
                    DSLink.Config.Name = Settings.Name;
                    DSLink.Config.BrokerUrl = Settings.Broker;
                }
                DSLink.Connect();
            }
            catch (Exception e)
            {
                SetDSLinkStatus(string.Format("DSLink failed to start: {0}", e.ToString()));
                Debug.WriteLine(e.ToString());
            }
            AllowServiceToggle = true;
        }

        public virtual void StopLink()
        {
            AllowServiceToggle = false;
            if (DSLink != null)
            {
                DSLink.Disconnect();
            }
            AllowServiceToggle = true;
        }

        protected virtual void InitializeDSLink()
        {
            var configuration = new Configuration(new List<string>(),
                                                  Settings.Name,
                                                  responder: true,
                                                  requester: true,
                                                  keysLocation: StoragePath() + "/dsa_mobile.keys",
                                                  brokerUrl: Settings.Broker,
                                                  communicationFormat: CommunicationFormat,
                                                  logLevel: LogLevel.Debug,
                                                  connectionAttemptLimit: 2);
            DSLink = PlatformDSLink(configuration, _enabledModules);
        }

        public void SetDSLinkStatus(string text)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                TabHost.ResponderPage.LinkStatus.Text = text;
            });
        }

        public virtual DSLink PlatformDSLink(Configuration config, List<BaseModule> modules) => new DSLink(config, this, modules);
        public virtual string CommunicationFormat => "json";
        protected abstract string StoragePath();
        public abstract BaseSensors GetSensors();
        public abstract BaseDeviceSettings GetDeviceSettings();
    }
}
