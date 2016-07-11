using System;
using Xamarin.Forms;

namespace DSAMobile.Views
{
    public class DGPage : ContentPage
    {
        public DGPage()
        {
            Padding = new Thickness(left: 0,
                                    top: Device.OnPlatform(
                                        iOS: 30,
                                        Android: 0,
                                        WinPhone: 0),
                                    right: 0,
                                    bottom: 0);
        }
    }
}

