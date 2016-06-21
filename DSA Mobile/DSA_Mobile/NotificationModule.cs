using System;
using System.Collections.Generic;
using DSLink.Nodes;
using DSLink.Nodes.Actions;
using Action = DSLink.Nodes.Actions.Action;
using Plugin.LocalNotifications;
using System.Diagnostics;

namespace DSA_Mobile
{
    public class NotificationModule : BaseModule
    {
        private int _notificationID = 0;

        public NotificationModule(Node superRoot)
        {
            superRoot.CreateChild("Create Notification")
                     .AddParameter(new Parameter("Title", "string"))
                     .AddParameter(new Parameter("Message", "string"))
                     //.AddParameter(new Parameter("Time", "string", editor: "date"))
                     .AddColumn(new Column("Notification ID", "number"))
                     .SetConfig("$invokable", new Value("write"))
                     .SetAction(new Action(Permission.Write, parameters =>
                     {
                         string title = parameters["Title"].Get();
                         string message = parameters["Message"].Get();
                         int notificationID = _notificationID++;
                         CrossLocalNotifications.Current.Show(title, message, notificationID);
                         return new List<dynamic>
                         {
                             notificationID
                         };
                     }));

            superRoot.CreateChild("Cancel Notification")
                     .AddParameter(new Parameter("Notification ID", "number"))
                     .SetConfig("$invokable", new Value(Permission.Write.ToString()))
                     .SetAction(new Action(Permission.Write, parameters =>
                     {
                         float nid = parameters["Notification ID"].Get();
                         CrossLocalNotifications.Current.Cancel((int)nid);
                         return new List<dynamic>();
                     }));
        }
    }
}

