using System;
using DSLink.Nodes;
using Plugin.DeviceInfo;

namespace DSAMobile.DeviceInfo
{
    public class DeviceInfoModule : BaseModule
    {
        private Node _operatingSystem;
        private Node _operatingSystemVersion;
        private Node _deviceModel;

        public bool Supported => true;

        public bool RequestPermissions()
        {
            return true;
        }

        public void AddNodes(Node superRoot)
        {
            superRoot.CreateChild("os")
                     .SetDisplayName("OS")
                     .SetType("string")
                     .SetValue(CrossDeviceInfo.Current.Platform.ToString());

            superRoot.CreateChild("os_ver")
                     .SetDisplayName("OS Version")
                     .SetType("string")
                     .SetValue(CrossDeviceInfo.Current.Version);

            superRoot.CreateChild("device_model")
                     .SetDisplayName("Device Model")
                     .SetType("string")
                     .SetValue(CrossDeviceInfo.Current.Model);
        }
    }
}

