﻿using DSLink.Nodes;
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
                                   .SetActionGroup(ActionGroups.Notifications)
                                   .SetInvokable(Permission.Write)
                                   .SetAction(new Action(Permission.Write, Vibrate));

            if (PlatformHelper.Android)
            {
                vibrate.AddParameter(new Parameter("Duration", "number"));
            }

            _vibrate = vibrate.BuildNode();
        }

        public void Start()
        {
        }

        public void Stop()
        {
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

