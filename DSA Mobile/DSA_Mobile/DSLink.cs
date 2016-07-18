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
            App.Instance.Disabled = false;
            if (_requestedModules.Count > 0)
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
            foreach (BaseModule module in _loadedModules)
            {
                module.Start();
            }
            App.Instance.SetDSLinkStatus("DSLink is connected");
        }

        protected override void OnConnectionClosed()
        {
            App.Instance.Disabled = true;
            App.Instance.SetDSLinkStatus("DSLink is disconnected");
            foreach (BaseModule module in _loadedModules)
            {
                module.Stop();
            }
        }

        protected override void OnConnectionFailed()
        {
            App.Instance.Disabled = true;
            App.Instance.SetDSLinkStatus("DSLink failed to connect");
        }
    }
}
