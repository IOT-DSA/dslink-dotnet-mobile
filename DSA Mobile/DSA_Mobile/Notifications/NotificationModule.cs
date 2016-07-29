using System.Collections.Generic;
using DSLink.Nodes;
using DSLink.Nodes.Actions;
using Plugin.LocalNotifications;
using DSLink.Request;
using Newtonsoft.Json.Linq;

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
            return true;
        }

        public void AddNodes(Node superRoot)
        {
            _create = superRoot.CreateChild("create_notification")
                               .SetDisplayName("Create Notification")
                               .SetActionGroup(ActionGroups.Notifications)
                               .AddParameter(new Parameter("title", "string"))
                               .AddParameter(new Parameter("message", "string"))
                               .AddColumn(new Column("notificationId", "number"))
                               .SetInvokable(Permission.Write)
                               .SetAction(new ActionHandler(Permission.Write, CreateNotification))
                               .BuildNode();

            _cancel = superRoot.CreateChild("cancel_notification")
                               .SetDisplayName("Cancel Notification")
                               .SetActionGroup(ActionGroups.Notifications)
                               .AddParameter(new Parameter("notificationId", "number"))
                               .SetInvokable(Permission.Write)
                               .SetAction(new ActionHandler(Permission.Write, CancelNotification))
                               .BuildNode();
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        private void CreateNotification(InvokeRequest request)
        {
            string title = request.Parameters["title"].Value<string>();
            string message = request.Parameters["message"].Value<string>();
            int notificationID = _notificationID++;
            CrossLocalNotifications.Current.Show(title, message, notificationID);
            request.UpdateTable(new Table
            {
                new Row
                {
                    notificationID
                }
            });
            request.Close();
        }

        private void CancelNotification(InvokeRequest request)
        {
            int nid = request.Parameters["notificationId"].Value<int>();
            CrossLocalNotifications.Current.Cancel((int)nid);
            request.Close();
        }
    }
}

