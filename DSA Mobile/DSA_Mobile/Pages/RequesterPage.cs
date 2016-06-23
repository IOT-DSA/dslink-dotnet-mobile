using System;
using DSA_Mobile.Views;
using Xamarin.Forms;

namespace DSA_Mobile.Pages
{
    public class RequesterPage : DGPage
    {
        public RequesterPage() : base()
        {
            Title = "Requester";
            if (PlatformHelper.iOS)
            {
                Icon = "ion-ios-cloud-download";
            }

            //Content = new RelativeLayout
        }
    }
}

