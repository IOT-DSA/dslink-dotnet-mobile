using System.Threading;
using System.Threading.Tasks;
using DSAMobile.Controls;
using DSAMobile.Views;
using Xamarin.Forms;

namespace DSAMobile.Pages
{
    public class ResponderPage : DGPage
    {
        public readonly Label LinkStatus;
        private readonly Button _toggleService;

        public ResponderPage()
        {
            Title = "Responder";
            /*if (PlatformHelper.iOS)
            {
                Icon = "ion-ios-cloud-upload";
            }*/

            _toggleService = new DGButton
            {
                Text = "Start/Stop",
                Command = new Command(() =>
                {
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
            };

            LinkStatus = new Label
            {
                Text = "DSLink is disconnected",
                HorizontalTextAlignment = TextAlignment.Center
            };

            Content = new StackLayout
            {
                Children =
                {
                    _toggleService,
                    LinkStatus
                }
            };
        }
    }
}

