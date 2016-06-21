using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DSA_Mobile.DeviceSettings;
using DSA_Mobile.Sensors;
using DSLink;
using Xamarin.Forms;

namespace DSA_Mobile
{
    public abstract class App : Application
    {
        private DSLink _dslink;
        private Entry _brokerUrlEntry;
        private Button _toggleButton;
        private bool _toggleState;
        protected BaseSensors _sensors;
        protected BaseDeviceSettings _deviceSettings;

        protected App()
        {
            _brokerUrlEntry = new Entry()
            {
                Text = "http://dglux.dev.dglogik.com/conn"
            };

            _toggleButton = new Button()
            {
                Text = "Connect",
                Command = new Command(() =>
                {
                    if (!_toggleState)
                    {
                        _toggleState = true;
                        Task.Run(() => StartLink());
                        _toggleButton.Text = "Disconnect";
                    }
                    else
                    {
                        _toggleState = false;
                        Task.Run(() => StopLink());
                        _toggleButton.Text = "Connect";
                    }
                })
            };

            MainPage = new ContentPage
            {
                Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5),
                Content = new StackLayout
                {
                    Children = {
                        _brokerUrlEntry,
                        _toggleButton
                    }
                }
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

        protected void StartLink()
        {
            if (_dslink == null)
            {
                try
                {
                    var configuration = new Configuration(new List<string>(), "DSAMobile", true, true, StoragePath() + "/dsa_mobile.keys", brokerUrl: _brokerUrlEntry.Text);
                    _dslink = PlatformDSLink(configuration);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                _dslink.Connect();
            }
        }

        protected void StopLink()
        {
            _dslink.Disconnect();
        }

        public virtual DSLink PlatformDSLink(Configuration config) => new DSLink(config, this);
        protected abstract string StoragePath();
        public abstract BaseSensors GetSensors();
        public abstract BaseDeviceSettings GetDeviceSettings();
    }
}
