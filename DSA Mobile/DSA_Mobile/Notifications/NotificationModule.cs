﻿using System;
using System.Collections.Generic;
using DSLink.Nodes;
using DSLink.Nodes.Actions;
using Action = DSLink.Nodes.Actions.Action;
using Plugin.LocalNotifications;
using System.Diagnostics;
using DSLink.Request;

namespace DSAMobile.Notifications
{
    public class NotificationModule : BaseModule
    {
        private Node _create;
        private Node _cancel;
        private int _notificationID = 0;

        public bool Supported => true;

        public bool RequestPermissions()
        {
            // TODO: Does this even work?
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

        public void AddNodes(Node superRoot)
        {
            _create = superRoot.CreateChild("Create Notification")
                               .AddParameter(new Parameter("Title", "string"))
                               .AddParameter(new Parameter("Message", "string"))
                               .AddColumn(new Column("Notification ID", "number"))
                               .SetInvokable(Permission.Write)
                               .SetAction(new Action(Permission.Write, CreateNotification))
                               .BuildNode();

            _cancel = superRoot.CreateChild("Cancel Notification")
                               .AddParameter(new Parameter("Notification ID", "number"))
                               .SetInvokable(Permission.Write)
                               .SetAction(new Action(Permission.Write, CancelNotification))
                               .BuildNode();
        }

        public void RemoveNodes()
        {
            _create.RemoveFromParent();
            _cancel.RemoveFromParent();
        }

        private void CreateNotification(Dictionary<string, Value> parameters, InvokeRequest request)
        {
            string title = parameters["Title"].Get();
            string message = parameters["Message"].Get();
            int notificationID = _notificationID++;
            CrossLocalNotifications.Current.Show(title, message, notificationID);
            request.SendUpdates(new List<dynamic>
            {
                new List<dynamic>
                {
                    notificationID
                }
            });
            request.Close();
        }

        private void CancelNotification(Dictionary<string, Value> parameters, InvokeRequest request)
        {
            float nid = parameters["Notification ID"].Get();
            CrossLocalNotifications.Current.Cancel((int)nid);
            request.Close();
        }
    }
}

