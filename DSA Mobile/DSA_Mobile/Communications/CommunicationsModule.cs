using System.Collections.Generic;
using DSLink.Nodes;
using DSLink.Nodes.Actions;
using DSLink.Request;
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
                                    .SetInvokable(Permission.Write)
                                    .AddParameter(new Parameter("To", "string"))
                                    .AddParameter(new Parameter("Message", "string"))
                                    .SetAction(new Action(Permission.Write, SendSMS))
                                    .BuildNode();
            }

            if (_comms.PhoneDialer.CanMakePhoneCall)
            {
                _makePhoneCall = superRoot.CreateChild("make_phone_call")
                                          .SetDisplayName("Make Phone Call")
                                          .SetInvokable(Permission.Write)
                                          .AddParameter(new Parameter("To", "string"))
                                          .SetAction(new Action(Permission.Write, MakePhoneCall))
                                          .BuildNode();
            }

            if (_comms.EmailMessenger.CanSendEmail)
            {
                _sendEmail = superRoot.CreateChild("send_email")
                                      .SetDisplayName("Send Email")
                                      .SetInvokable(Permission.Write)
                                      .AddParameter(new Parameter("To", "string"))
                                      .AddParameter(new Parameter("Subject", "string"))
                                      .AddParameter(new Parameter("Message", "string"))
                                      .SetAction(new Action(Permission.Write, SendEmail))
                                      .BuildNode();
            }
        }

        public void RemoveNodes()
        {
            if (CrossMessaging.Current.SmsMessenger.CanSendSms)
            {
                _sendSms.RemoveFromParent();
            }

            if (CrossMessaging.Current.PhoneDialer.CanMakePhoneCall)
            {
                _makePhoneCall.RemoveFromParent();
            }

            if (CrossMessaging.Current.EmailMessenger.CanSendEmail)
            {
                _sendEmail.RemoveFromParent();
            }
        }

        private void SendSMS(Dictionary<string, Value> parameters, InvokeRequest request)
        {
            var to = parameters["To"].Get();
            var message = parameters["Message"].Get();
            CrossMessaging.Current.SmsMessenger.SendSms(to, message);

            request.Close();
        }

        private void MakePhoneCall(Dictionary<string, Value> parameters, InvokeRequest request)
        {
            var to = parameters["To"].Get();
            CrossMessaging.Current.PhoneDialer.MakePhoneCall(to);

            request.Close();
        }

        private void SendEmail(Dictionary<string, Value> parameters, InvokeRequest request)
        {
            var to = parameters["To"].Get();
            var subject = parameters["Subject"].Get();
            var message = parameters["Message"].Get();
            CrossMessaging.Current.EmailMessenger.SendEmail(to, subject, message);

            request.Close();
        }
    }
}

