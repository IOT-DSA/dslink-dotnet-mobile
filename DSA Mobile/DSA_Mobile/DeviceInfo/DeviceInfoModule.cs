using System;
using DSLink.Nodes;
using Plugin.DeviceInfo;

namespace DSAMobile.DeviceInfo
{
    public class DeviceInfoModule : BaseModule
    {
        private Node _os;
        private Node _osVersion;
        private Node _model;

        public bool Supported => true;

        public bool RequestPermissions()
        {
            return true;
        }

        public void AddNodes(Node superRoot)
        {
            _os = superRoot.CreateChild("os")
                           .SetDisplayName("OS")
                           .SetType("string")
                           .SetValue(CrossDeviceInfo.Current.Platform.ToString())
                           .BuildNode();

            _osVersion = superRoot.CreateChild("os_ver")
                                  .SetDisplayName("OS Version")
                                  .SetType("string")
                                  .SetValue(CrossDeviceInfo.Current.Version)
                                  .BuildNode();

            _model = superRoot.CreateChild("device_model")
                              .SetDisplayName("Device Model")
                              .SetType("string")
                              .SetValue(CrossDeviceInfo.Current.Model)
                              .BuildNode();
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}

