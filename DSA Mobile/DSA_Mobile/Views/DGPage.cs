using System;
using Xamarin.Forms;

namespace DSA_Mobile.Views
{
    public class DGPage : ContentPage
    {
        public DGPage()
        {
            Padding = new Thickness(left: 10,
                                    top: Device.OnPlatform(
                                        iOS: 30,
                                        Android: 10,
                                        WinPhone: 10),
                                    right: 10,
                                    bottom: 10);
        }
    }
}

