using System;
using DSA_Mobile.DeviceSettings;

namespace DSA_Mobile.iOS.DeviceSettings
{
    public class iOSDeviceSettings : BaseDeviceSettings
    {
        public iOSDeviceSettings(App app) : base(app)
        {
        }

        public override bool ScreenOn()
        {
            // TODO
            return true;
        }
    }
}

