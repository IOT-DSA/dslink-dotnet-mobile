using System;
using System.Collections.Generic;
using DSLink.Nodes;
using DSLink.Nodes.Actions;
using Action = DSLink.Nodes.Actions.Action;
using Plugin.LocalNotifications;
using System.Diagnostics;

namespace DSA_Mobile.Notifications
{
    public class NotificationModule : BaseModule
    {
        private int _notificationID = 0;

        public bool Supported => true;

        public void AddNodes(Node superRoot)
        {
            superRoot.CreateChild("Create Notification")
                     .AddParameter(new Parameter("Title", "string"))
                     .AddParameter(new Parameter("Message", "string"))
                     .AddColumn(new Column("Notification ID", "number"))
                     .SetConfig("$invokable", new Value("write"))
                     .SetAction(new Action(Permission.Write, (parameters, request) =>
                     {
                         string title = parameters["Title"].Get();
                         string message = parameters["Message"].Get();
                         int notificationID = _notificationID++;
                         CrossLocalNotifications.Current.Show(title, message, notificationID);
                         request.SendUpdates(new List<dynamic>
                         {
                             notificationID
                         });
                         request.Close();
                     }));

            superRoot.CreateChild("Cancel Notification")
                     .AddParameter(new Parameter("Notification ID", "number"))
                     .SetConfig("$invokable", new Value(Permission.Write.ToString()))
                     .SetAction(new Action(Permission.Write, (parameters, request) =>
                     {
                         float nid = parameters["Notification ID"].Get();
                         CrossLocalNotifications.Current.Cancel((int)nid);
                         request.SendUpdates(new List<dynamic>());
                         request.Close();
                     }));
        }

        public bool RequestPermissions()
        {
#if IOS
            var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert |
                                                                                      UIUserNotificationType.Badge |
                                                                                      UIUserNotificationType.Sound,
                                                                                      new NSSet());
            UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            Debug.WriteLine("REQUESTED PERMISSIONS");
#endif
            return true;
        }
    }
}

