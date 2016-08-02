using DSAMobile.Controls;
using DSAMobile.Views;
using Xamarin.Forms;

namespace DSAMobile.Pages
{
    public class SettingsPage : DGPage
    {
        private readonly EntryCell _brokerUrlCell;
        private readonly EntryCell _dslinkNameCell;
        private readonly Button _saveButton;

        public SettingsPage()
        {
            Title = "Settings";
            /*if (PlatformHelper.iOS)
            {
                Icon = "ion-toggle-filled";
            }*/

            _saveButton = new DGButton
            {
                Text = "Save",
                Command = new Command(() =>
                {
                    Settings.Broker = _brokerUrlCell.Text;
                    Settings.Name = _dslinkNameCell.Text;
                })
            };

            _brokerUrlCell = new EntryCell
            {
                Label = "Broker URL",
                Text = Settings.Broker,
                Keyboard = Keyboard.Url
            };

            _dslinkNameCell = new EntryCell
            {
                Label = "DSLink Name",
                Text = Settings.Name
            };

            Content = new StackLayout
            {
                Children =
                {
                    new TableView
                    {
                        Root = new TableRoot
                        {
                            new TableSection("DSLink")
                            {
                                _brokerUrlCell,
                                _dslinkNameCell
                            }
                        }
                    },
                    _saveButton
                }
            };
        }
    }
}
