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

        protected App()
        {
            Instance = this;
            MainPage = new TabHost();
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
                                                      brokerUrl: Settings.BrokerURL);
                _dslink = PlatformDSLink(configuration);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            _dslink.RegisterModule(new BatteryModule());
            _dslink.RegisterModule(new DeviceInfoModule());
            _dslink.RegisterModule(new DeviceSettingsModule(GetDeviceSettings()));
            _dslink.RegisterModule(new SensorsModule(GetSensors()));
            _dslink.RegisterModule(new NotificationModule());
            _dslink.RegisterModule(new CameraModule());
            _dslink.RegisterModule(new VibrateModule());
            _dslink.RegisterModule(new ContactsModule());
            _dslink.RegisterModule(new ConnectivityModule());
            _dslink.RegisterModule(new CommunicationsModule());
            _dslink.RegisterModule(new LocationModule());
        }

        public virtual DSLink PlatformDSLink(Configuration config) => new DSLink(config, this);
        protected abstract string StoragePath();
        public abstract BaseSensors GetSensors();
        public abstract BaseDeviceSettings GetDeviceSettings();
    }
}
