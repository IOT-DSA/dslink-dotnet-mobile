using DSAMobile.Views;
using Xamarin.Forms;

namespace DSAMobile.Pages
{
    public class ResponderPage : DGPage
    {
        private readonly SwitchCell _toggleServiceSwitch;

        public ResponderPage()
        {
            Title = "Responder";

            _toggleServiceSwitch = new SwitchCell
            {
                Text = "Service"
            };
            _toggleServiceSwitch.OnChanged += (sender, e) =>
            {
                if (e.Value && App.Instance.Disabled)
                {
                    App.Instance.StartLink();
                }
                else if (!e.Value && !App.Instance.Disabled)
                {
                    App.Instance.StopLink();
                }
            };

            Content = new StackLayout
            {
                Children =
                {
                    new TableView
                    {
                        Root = new TableRoot
                        {
                            new TableSection()
                            {
                                _toggleServiceSwitch
                            }
                        }
                    }
                }
            };
        }
    }
}

