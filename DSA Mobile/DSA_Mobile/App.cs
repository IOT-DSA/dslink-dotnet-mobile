using System;
using System.Collections.Generic;
using System.Diagnostics;
using DSAMobile.Battery;
using DSAMobile.Camera;
using DSAMobile.Communications;
using DSAMobile.Connectivity;
using DSAMobile.Contacts;
using DSAMobile.DeviceInfo;
using DSAMobile.DeviceSettings;
using DSAMobile.Location;
using DSAMobile.Notifications;
using DSAMobile.Pages;
using DSAMobile.Sensors;
using DSAMobile.Vibrate;
using DSLink;
using DSLink.Util.Logger;
using Xamarin.Forms;

namespace DSAMobile
{
    public abstract class App : Application
    {
        public static App Instance;

        protected DSLink _dslink;
        public bool Disabled = true;
        protected BaseSensors _sensors;
        protected BaseDeviceSettings _deviceSettings;
        protected List<BaseModule> enabledModules;

        protected App()
        {
            Instance = this;
            MainPage = new TabHost();
            enabledModules = new List<BaseModule>()
            {
                new BatteryModule(),
                new DeviceInfoModule(),
                new DeviceSettingsModule(GetDeviceSettings()),
                new SensorsModule(GetSensors()),
                new NotificationModule(),
                new CameraModule(),
                new VibrateModule(),
                new ContactsModule(),
                new ConnectivityModule(),
                new CommunicationsModule(),
                new LocationModule(),
            };
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public void StartLink()
        {
            if (_dslink == null)
            {
                InitializeDSLink();
            }
            else
            {
                _dslink.Connect();
            }
        }

        public void StopLink()
        {
            if (_dslink != null)
            {
                _dslink.Disconnect();
            }
        }

        protected virtual void InitializeDSLink()
        {
            try
            {
                var configuration = new Configuration(new List<string>(),
                                                      Settings.DSLinkName,
                                                      responder: true,
                                                      requester: true,
                                                      keysLocation: StoragePath() + "/dsa_mobile.keys",
                                                      brokerUrl: Settings.BrokerURL,
                                                      logLevel: LogLevel.Debug);
                _dslink = PlatformDSLink(configuration, enabledModules);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        public virtual DSLink PlatformDSLink(Configuration config, List<BaseModule> modules) => new DSLink(config, this, modules);
        protected abstract string StoragePath();
        public abstract BaseSensors GetSensors();
        public abstract BaseDeviceSettings GetDeviceSettings();
    }
}
