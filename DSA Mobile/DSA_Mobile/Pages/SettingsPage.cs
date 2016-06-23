using System;
using DSA_Mobile.Views;
using Xamarin.Forms;

namespace DSA_Mobile.Pages
{
    public class SettingsPage : DGPage
    {
        private Entry _brokerUrlEntry;
        private Button _saveButton;

        public SettingsPage()
        {
            Title = "Settings";
            if (PlatformHelper.iOS)
            {
                Icon = "ion-toggle-filled";
            }

            _brokerUrlEntry = new Entry
            {
                Placeholder = "http://your.bro.ker/conn",
                Text = Settings.Get("dsamobile.broker", "http://your.bro.ker/conn"),
                Keyboard = Keyboard.Url
            };

            _saveButton = new Button
            {
                Text = "Save",
                Command = new Command(() =>
                {
                    Settings.Set("dsamobile.broker", _brokerUrlEntry.Text);
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

