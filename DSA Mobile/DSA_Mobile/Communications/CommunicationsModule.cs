using System.Collections.Generic;
using DSLink.Nodes;
using DSLink.Nodes.Actions;
using DSLink.Request;
using Newtonsoft.Json.Linq;
using Plugin.Messaging;

namespace DSAMobile.Communications
{
    public class CommunicationsModule : BaseModule
    {
        private Node _sendSms;
        private Node _makePhoneCall;
        private Node _sendEmail;

        public bool Supported => true;

        public bool RequestPermissions()
        {
            return true;
        }

        public void AddNodes(Node superRoot)
        {
            var _comms = CrossMessaging.Current;

            if (_comms.SmsMessenger.CanSendSms)
            {
                _sendSms = superRoot.CreateChild("send_sms")
                                    .SetDisplayName("Send SMS")
                                    .SetActionGroup(ActionGroups.Communications)
                                    .SetInvokable(Permission.Write)
                                    .AddParameter(new Parameter("To", "string"))
                                    .AddParameter(new Parameter("Message", "string"))
                                    .SetAction(new ActionHandler(Permission.Write, SendSMS))
                                    .BuildNode();
            }

            if (_comms.PhoneDialer.CanMakePhoneCall)
            {
                _makePhoneCall = superRoot.CreateChild("make_phone_call")
                                          .SetDisplayName("Make Phone Call")
                                          .SetActionGroup(ActionGroups.Communications)
                                          .SetInvokable(Permission.Write)
                                          .AddParameter(new Parameter("To", "string"))
                                          .SetAction(new ActionHandler(Permission.Write, MakePhoneCall))
                                          .BuildNode();
            }

            if (_comms.EmailMessenger.CanSendEmail)
            {
                _sendEmail = superRoot.CreateChild("send_email")
                                      .SetDisplayName("Send Email")
                                      .SetActionGroup(ActionGroups.Communications)
                                      .SetInvokable(Permission.Write)
                                      .AddParameter(new Parameter("To", "string"))
                                      .AddParameter(new Parameter("Subject", "string"))
                                      .AddParameter(new Parameter("Message", "string"))
                                      .SetAction(new ActionHandler(Permission.Write, SendEmail))
                                      .BuildNode();
            }
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        private void SendSMS(InvokeRequest request)
        {
            var to = request.Parameters["to"].Value<string>();
            var message = request.Parameters["message"].Value<string>();
            CrossMessaging.Current.SmsMessenger.SendSms(to, message);
            request.Close();
        }

        private void MakePhoneCall(InvokeRequest request)
        {
            var to = request.Parameters["To"].Value<string>();
            CrossMessaging.Current.PhoneDialer.MakePhoneCall(to);
            request.Close();
        }

        private void SendEmail(InvokeRequest request)
        {
            var to = request.Parameters["To"].Value<string>();
            var subject = request.Parameters["Subject"].Value<string>();
            var message = request.Parameters["Message"].Value<string>();
            CrossMessaging.Current.EmailMessenger.SendEmail(to, subject, message);
            request.Close();
        }
    }
}

