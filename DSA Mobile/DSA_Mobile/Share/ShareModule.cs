using System.Collections.Generic;
using DSLink.Nodes;
using DSLink.Nodes.Actions;
using DSLink.Request;
using Plugin.Share;
using Plugin.Share.Abstractions;

namespace DSAMobile.Share
{
    public class ShareModule : BaseModule
    {
        private Node _openInBrowser;
        private Node _share;
        private Node _shareLink;
        private Node _setClipboard;

        public bool Supported => true;

        public bool RequestPermissions()
        {
            return true;
        }

        public void AddNodes(Node superRoot)
        {
            NodeFactory factory; 

            factory = superRoot.CreateChild("open_in_browser")
                               .SetDisplayName("Open in Browser")
                               .SetActionGroup(ActionGroups.Share)
                               .SetInvokable(Permission.Write)
                               .AddParameter(new Parameter("URL", "string"))
                               .SetAction(new Action(Permission.Write, OpenInBrowser));

            if (PlatformHelper.Android)
            {
                factory.AddParameter(new Parameter("Show Title", "bool"));
            }

            if (PlatformHelper.iOS)
            {
                factory.AddParameter(new Parameter("Reader Mode", "bool"));
            }

            _openInBrowser = factory.BuildNode();

            _share = superRoot.CreateChild("share")
                              .SetDisplayName("Share")
                              .SetActionGroup(ActionGroups.Share)
                              .SetInvokable(Permission.Write)
                              .AddParameter(new Parameter("Title", "string", "Share"))
                              .AddParameter(new Parameter("Text", "string"))
                              .SetAction(new Action(Permission.Write, Share))
                              .BuildNode();

            _shareLink = superRoot.CreateChild("share_link")
                                  .SetDisplayName("Share Link")
                                  .SetActionGroup(ActionGroups.Share)
                                  .SetInvokable(Permission.Write)
                                  .AddParameter(new Parameter("URL", "string"))
                                  .AddParameter(new Parameter("Message", "string"))
                                  .AddParameter(new Parameter("Title", "string"))
                                  .SetAction(new Action(Permission.Write, ShareLink))
                                  .BuildNode();

            if (CrossShare.Current.SupportsClipboard)
            {
                _setClipboard = superRoot.CreateChild("set_clipboard")
                                         .SetDisplayName("Set Clipboard")
                                         .SetActionGroup(ActionGroups.Share)
                                         .SetInvokable(Permission.Write)
                                         .AddParameter(new Parameter("Text", "string"))
                                         .SetAction(new Action(Permission.Write, SetClipboardContents))
                                         .BuildNode();
            }
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        private void OpenInBrowser(Dictionary<string, Value> parameters, InvokeRequest request)
        {
            // We should just close the request now.
            request.Close();

            var url = parameters["URL"].Get();
            var options = new BrowserOptions();

            if (PlatformHelper.Android && parameters.ContainsKey("Show Title"))
            {
                options.ChromeShowTitle = parameters["Show Title"].Get();
            }

            if (PlatformHelper.iOS && parameters.ContainsKey("Reader Mode"))
            {
                options.UseSafairReaderMode = parameters["Reader Mode"].Get();
            }

            CrossShare.Current.OpenBrowser(url, options);
        }

        private void Share(Dictionary<string, Value> parameters, InvokeRequest request)
        {
            request.Close();

            var title = parameters["Title"].Get();
            var text = parameters["Text"].Get();

            CrossShare.Current.Share(text, title);
        }

        private void ShareLink(Dictionary<string, Value> parameters, InvokeRequest request)
        {
            request.Close();

            var url = parameters["URL"].Get();
            var message = parameters["Message"].Get();
            var title = parameters["Title"].Get();

            CrossShare.Current.ShareLink(url, message, title);
        }

        private void SetClipboardContents(Dictionary<string, Value> parameters, InvokeRequest request)
        {
            request.Close();

            var text = parameters["Text"].Get();

            CrossShare.Current.SetClipboardText(text);
        }
    }
}

