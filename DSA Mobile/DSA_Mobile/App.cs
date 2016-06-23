using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DSA_Mobile.Battery;
using DSA_Mobile.Camera;
using DSA_Mobile.DeviceInfo;
using DSA_Mobile.DeviceSettings;
using DSA_Mobile.Notifications;
using DSA_Mobile.Pages;
using DSA_Mobile.Sensors;
using DSLink;
using Xamarin.Forms;

namespace DSA_Mobile
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
                var configuration = new Configuration(new List<string>(), "DSAMobile",
                                                      responder: true,
                                                      requester: true,
                                                      keysLocation: StoragePath() + "/dsa_mobile.keys",
                                                      brokerUrl: Settings.Get("dsamobile.broker", "http://your.bro.ker/conn"));
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
        }

        public virtual DSLink PlatformDSLink(Configuration config) => new DSLink(config, this);
        protected abstract string StoragePath();
        public abstract BaseSensors GetSensors();
        public abstract BaseDeviceSettings GetDeviceSettings();
    }
}
