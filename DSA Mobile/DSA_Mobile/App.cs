using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSLink;
using Xamarin.Forms;

#if __ANDROID__
using Android.OS;
#endif

namespace DSA_Mobile
{
    public abstract class App : Application
    {
        private DSLink _dslink;
        private Entry _brokerUrlEntry;
        private Button _toggleButton;

        protected App()
        {
            _brokerUrlEntry = new Entry()
            {
                Text = "http://octocat.local:8080/conn"
            };

            _toggleButton = new Button()
            {
                Text = "Connect to Broker",
                Command = new Command(() =>
                {
                    if (_dslink == null)
                    {
                        Task.Run(() => StartLink());
                        _toggleButton.Text = "Disconnect";
                    }
                    else
                    {
                        Task.Run(() => StopLink());
                        _toggleButton.Text = "Connect";
                    }
                })
            };

            MainPage = new ContentPage
            {
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
            _dslink = new DSLink(new Configuration(new List<string>(), "DSAMobile", true, true, StoragePath() + "/dsa_mobile.keys", brokerUrl: "http://octocat.local:8080/conn"));
        }

        protected void StopLink()
        {
            _dslink.Connector.Disconnect();
            _dslink = null;
        }

        protected abstract string StoragePath();
    }
}
