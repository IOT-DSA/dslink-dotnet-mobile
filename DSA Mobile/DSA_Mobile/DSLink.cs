using System.Collections.Generic;
using DSLink;

namespace DSAMobile
{
    public class DSLink : DSLinkContainer
    {
        private readonly App _app;
        private readonly List<BaseModule> _requestedModules = new List<BaseModule>();
        private readonly List<BaseModule> _loadedModules = new List<BaseModule>();

        public DSLink(Configuration config, App app, List<BaseModule> requestedModules) : base(config)
        {
			_app = app;
            _requestedModules = requestedModules;
        }

        protected override void OnConnectionOpen()
        {
            foreach (BaseModule module in _requestedModules)
            {
                if (module.Supported)
                {
                    Logger.Info("Requesting permissions for " + module);
                    var granted = module.RequestPermissions();
                    if (granted)
                    {
                        module.AddNodes(Responder.SuperRoot);
                        _loadedModules.Add(module);
                    }
                }
            }
            _requestedModules.Clear();
        }
    }
}
