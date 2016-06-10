using System.Diagnostics;
using DSLink;

namespace DSA_Mobile
{
    public class DSLink : DSLinkContainer
    {
        private Battery _battery;

        public DSLink(Configuration config) : base(config)
        {
            _battery = new Battery(Responder.SuperRoot);
        }
    }
}
