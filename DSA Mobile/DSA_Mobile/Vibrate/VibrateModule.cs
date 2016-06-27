using DSLink.Nodes;
using Plugin.Vibrate;
using System.Collections.Generic;
using DSLink.Request;
using DSLink.Nodes.Actions;
using System.Diagnostics;

namespace DSAMobile.Vibrate
{
    public class VibrateModule : BaseModule
    {
        private Node _vibrate;

        public bool Supported => true;

        public bool RequestPermissions()
        {
            return true;
        }

        public void AddNodes(Node superRoot)
        {
            var vibrate = superRoot.CreateChild("vibrate")
                                   .SetDisplayName("Vibrate")
                                   .SetInvokable(Permission.Write)
                                   .SetAction(new Action(Permission.Write, Vibrate));

            if (!PlatformHelper.iOS)
            {
                vibrate.AddParameter(new Parameter("Duration", "number"));
            }

            _vibrate = vibrate.BuildNode();
        }

        public void RemoveNodes()
        {
            _vibrate.RemoveFromParent();
        }

        private void Vibrate(Dictionary<string, Value> parameters, InvokeRequest request)
        {
            if (parameters.ContainsKey("Duration"))
            {
                CrossVibrate.Current.Vibration((int) parameters["Duration"].Get());
            }
            else
            {
                CrossVibrate.Current.Vibration();
            }
            request.Close();
        }
    }
}

