using DSAMobile.Views;
using Xamarin.Forms;

namespace DSAMobile.Pages
{
    public class ResponderPage : DGPage
    {
        public ResponderPage()
        {
            Title = "Responder";
            /*if (PlatformHelper.iOS)
            {
                Icon = "ion-ios-cloud-upload";
            }*/

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Button
                    {
                        Text = "Start/Stop",
                        Command = new Command(() => {
                            if (App.Instance.Disabled)
                            {
                                App.Instance.StartLink();
                            }
                            else
                            {
                                App.Instance.StopLink();
                            }
                            App.Instance.Disabled = !App.Instance.Disabled;
                        })
                    }
                }
            };
        }
    }
}

