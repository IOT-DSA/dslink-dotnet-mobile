using DSAMobile.DeviceSettings;
using UIKit;

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

        public override void SetScreenIdle(bool screenIdleState)
        {
            UIApplication.SharedApplication.IdleTimerDisabled = screenIdleState;
        }
    }
}
