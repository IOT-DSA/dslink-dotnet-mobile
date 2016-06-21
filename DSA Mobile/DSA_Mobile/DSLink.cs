using DSLink;

namespace DSA_Mobile
{
    public class DSLink : DSLinkContainer
    {
        private readonly App _app;
        private readonly BatteryModule _battery;
		private readonly MotionModule _motion;

        public DSLink(Configuration config, App app) : base(config)
        {
			_app = app;
            _battery = new BatteryModule(Responder.SuperRoot);
			_motion = new MotionModule(Responder.SuperRoot, _app);
        }
    }
}
