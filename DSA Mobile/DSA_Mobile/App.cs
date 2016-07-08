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
using DSAMobile.Flash;
using DSAMobile.Location;
using DSAMobile.Notifications;
using DSAMobile.Pages;
using DSAMobile.Sensors;
using DSAMobile.Share;
using DSAMobile.TTS;
using DSAMobile.Vibrate;
using DSAMobile.ZXing;
using DSLink;
using DSLink.Util.Logger;
using Xamarin.Forms;

namespace DSAMobile
{
    public abstract class App : Application
    {
        public static App Instance;

        public DSLink DSLink
        {
            get;
            private set;
        }
        public bool Disabled = true;
        protected BaseSensors _sensors;
        protected BaseDeviceSettings _deviceSettings;
        protected List<BaseModule> enabledModules;

        protected App()
        {
            Instance = this;
            MainPage = new TabHost();
            enabledModules = new List<BaseModule>();
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

        public virtual void InitModules()
        {
            enabledModules.Add(new BatteryModule());
            enabledModules.Add(new DeviceInfoModule());
            enabledModules.Add(new DeviceSettingsModule(GetDeviceSettings(), this));
            enabledModules.Add(new SensorsModule(GetSensors()));
            enabledModules.Add(new NotificationModule());
            enabledModules.Add(new CameraModule());
            enabledModules.Add(new VibrateModule());
            enabledModules.Add(new ContactsModule());
            enabledModules.Add(new ConnectivityModule());
            enabledModules.Add(new CommunicationsModule());
            enabledModules.Add(new LocationModule());
            enabledModules.Add(new ZXingModule());
            enabledModules.Add(new TTSModule());
            enabledModules.Add(new FlashModule());
            enabledModules.Add(new ShareModule());
        }

        public void StartLink()
        {
            if (DSLink == null)
            {
                InitModules();
                InitializeDSLink();
            }
            else
            {
                DSLink.Config.Name = Settings.DSLinkName;
                DSLink.Config.BrokerUrl = Settings.BrokerURL;
                DSLink.Connect();
            }
        }

        public void StopLink()
        {
            if (DSLink != null)
            {
                DSLink.Disconnect();
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
                                                      logLevel: LogLevel.Info);
                DSLink = PlatformDSLink(configuration, enabledModules);
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
