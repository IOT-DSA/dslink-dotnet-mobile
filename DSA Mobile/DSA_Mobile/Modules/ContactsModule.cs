using System.Linq;
using DSLink.Nodes;
using DSLink.Nodes.Actions;
using DSLink.Request;
using Newtonsoft.Json.Linq;
using Plugin.Contacts;
using Plugin.Contacts.Abstractions;

namespace DSAMobile.Modules
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
                                    .SetActionGroup("Communications")
                                    .SetInvokable(Permission.Write)
                                    .AddColumn(new Column("Name", "string"))
                                    .AddColumn(new Column("Phone Numbers", "array"))
                                    .AddColumn(new Column("Emails", "array"))
                                    .SetConfig("result", new Value("table"))
                                    .SetAction(new ActionHandler(Permission.Write, GetContacts))
                                    .BuildNode();
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        private void GetContacts(InvokeRequest request)
        {
            CrossContacts.Current.PreferContactAggregation = false;
            var contacts = CrossContacts.Current.Contacts.ToList();
            var table = new Table();

            foreach (Contact contact in contacts)
            {
                var contactPhones = new JArray();
                var contactEmails = new JArray();

                foreach (Phone phone in contact.Phones)
                {
                    contactPhones.Add(phone.Number);
                }

                foreach (Email email in contact.Emails)
                {
                    contactEmails.Add(email.Address);
                }

                table.Add(new Row
                {
                    contact.DisplayName,
                    contactPhones,
                    contactEmails
                });
            }

            request.UpdateTable(table);
            request.Close();
        }
    }
}
