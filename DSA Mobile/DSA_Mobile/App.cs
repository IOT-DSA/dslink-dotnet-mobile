using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DSA_Mobile.Motion;
using DSLink;
using Xamarin.Forms;

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
                Text = "http://dglux.dev.dglogik.com/conn"
            };

            _toggleButton = new Button()
            {
                Text = "Connect",
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
			try
			{
				var configuration = new Configuration(new List<string>(), "DSAMobile", true, true, StoragePath() + "/dsa_mobile.keys", brokerUrl: _brokerUrlEntry.Text);
				_dslink = new DSLink(configuration, this);
			}
			catch(Exception e)
			{
				Debug.WriteLine(e.ToString());
			}
        }

        protected void StopLink()
        {
            _dslink.Connector.Disconnect();
            _dslink = null;
        }

        protected abstract string StoragePath();
		public abstract BaseMotionImplementation GetMotion();
    }
}
