using DSAMobile.Views;
using Xamarin.Forms;

namespace DSAMobile.Pages
{
    public class SettingsPage : DGPage
    {
        private Entry _brokerUrlEntry;
        private Entry _dslinkNameEntry;
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
                Placeholder = "Broker URL",
                Text = Settings.BrokerURL,
                Keyboard = Keyboard.Url
            };

            _dslinkNameEntry = new Entry
            {
                Placeholder = "DSLink Name",
                Text = Settings.DSLinkName
            };

            _saveButton = new Button
            {
                Text = "Save",
                Command = new Command(() =>
                {
                    Settings.BrokerURL = _brokerUrlEntry.Text;
                    Settings.DSLinkName = _dslinkNameEntry.Text;
                })
            };

            Content = new StackLayout
            {
                Children =
                {
                    _brokerUrlEntry,
                    _dslinkNameEntry,
                    _saveButton
                }
            };
        }
    }
}

