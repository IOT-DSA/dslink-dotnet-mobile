using System.Collections.Generic;
using System.Diagnostics;
using DSAMobile.Controls;
using DSAMobile.Views;
using DSLink.Nodes;
using DSLink.Respond;
using Xamarin.Forms;

namespace DSAMobile.Pages
{
    public class RequesterPage : ContentPage
    {
        private readonly RequesterView _requesterView;

        public RequesterPage() : base()
        {
            Title = "Requester";
            /*if (PlatformHelper.iOS)
            {
                Icon = "ion-ios-cloud-download";
            }*/

            _requesterView = new RequesterView("/");

            var button = new DGButton
            {
                Text = "Test Requester",
                Command = new Command((obj) =>
                {
                    _requesterView.Load();
                })
            };

            Content = new StackLayout
            {
                Children =
                {
                    button,
                    _requesterView
                }
            };
        }
    }
}
