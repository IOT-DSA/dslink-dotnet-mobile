using System;
using DSAMobile.Views;
using Xamarin.Forms;

namespace DSAMobile.Pages
{
    public class SettingsPage : DGPage
    {
        private Entry _brokerUrlEntry;
        private Button _saveButton;

        public SettingsPage()
        {
            Title = "Settings";
            /*if (PlatformHelper.iOS)
            {
                Icon = "ion-toggle-filled";
            }*/

            _brokerUrlEntry = new Entry
            {
                Placeholder = "http://your.bro.ker/conn",
                Text = Settings.BrokerURL,
                Keyboard = Keyboard.Url
            };

            _saveButton = new Button
            {
                Text = "Save",
                Command = new Command(() =>
                {
                    Settings.BrokerURL = _brokerUrlEntry.Text;
                })
            };

            Content = new StackLayout
            {
                Children =
                {
                    _brokerUrlEntry,
                    _saveButton
                }
            };
        }
    }
}

