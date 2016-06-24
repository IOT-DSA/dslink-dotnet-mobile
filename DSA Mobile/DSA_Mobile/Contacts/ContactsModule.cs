using System;
using System.Collections.Generic;
using System.Linq;
using DSLink.Nodes;
using DSLink.Nodes.Actions;
using DSLink.Request;
using Plugin.Contacts;
using Plugin.Contacts.Abstractions;
using Action = DSLink.Nodes.Actions.Action;

namespace DSA_Mobile.Contacts
{
    public class ContactsModule : BaseModule
    {
        private Node _getContacts;

        public bool Supported => true;

        public bool RequestPermissions()
        {
            return CrossContacts.Current.RequestPermission().Result;
        }

        public void AddNodes(Node superRoot)
        {
            _getContacts = superRoot.CreateChild("get_contacts")
                                    .SetDisplayName("Get Contacts")
                                    .SetInvokable(Permission.Write)
                                    .AddColumn(new Column("Name", "string"))
                                    .AddColumn(new Column("Phone Numbers", "array"))
                                    .AddColumn(new Column("Emails", "array"))
                                    .SetConfig("result", new Value("table"))
                                    .SetAction(new Action(Permission.Write, GetContacts))
                                    .BuildNode();
        }

        public void GetContacts(Dictionary<string, Value> parameters, InvokeRequest request)
        {
            CrossContacts.Current.PreferContactAggregation = false;
            var contacts = CrossContacts.Current.Contacts.ToList();
            var updates = new List<dynamic>();

            foreach (Contact contact in contacts)
            {
                var contactPhones = new List<string>();
                var contactEmails = new List<string>();

                foreach (Phone phone in contact.Phones)
                {
                    contactPhones.Add(phone.Number);
                }

                foreach (Email email in contact.Emails)
                {
                    contactEmails.Add(email.Address);
                }

                updates.Add(new List<dynamic>
                {
                    contact.DisplayName,
                    contactPhones,
                    contactEmails
                });
            }

            request.SendUpdates(updates);
            request.Close();
        }
    }
}
