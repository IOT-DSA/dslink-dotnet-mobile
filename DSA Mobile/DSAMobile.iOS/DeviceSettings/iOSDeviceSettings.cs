using System;
using DSAMobile.DeviceSettings;

namespace DSAMobile.iOS.DeviceSettings
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

