using System;
using Xamarin.Forms;

namespace DSA_Mobile
{
    public static class PlatformHelper
    {
        public static bool iOS => Device.OS == TargetPlatform.iOS;
        public static bool Android => Device.OS == TargetPlatform.Android;
        public static bool WindowsPhone => Device.OS == TargetPlatform.WinPhone;
    }
}

