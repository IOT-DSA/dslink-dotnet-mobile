using System.Diagnostics;
using System.Threading.Tasks;
using DSLink;
using Node = DSLink.Nodes.Node;

namespace DSA_Mobile
{
    public class DSLink : DSLinkContainer
    {
		private App _app;
        private BatteryModule _battery;
		private MotionModule _motion;

        public DSLink(Configuration config, App app) : base(config)
        {
			_app = app;
            //_battery = new Battery(Responder.SuperRoot);
			_motion = new MotionModule(Responder.SuperRoot, _app);
        }
    }
}
