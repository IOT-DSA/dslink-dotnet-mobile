using System.Collections.Generic;
using DSLink.Nodes;
using DSLink.Nodes.Actions;
using DSLink.Request;
using Newtonsoft.Json.Linq;
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

            factory = superRoot.CreateChild("openInBrowser")
                               .SetDisplayName("Open in Browser")
                               .SetActionGroup(ActionGroups.Share)
                               .SetInvokable(Permission.Write)
                               .AddParameter(new Parameter("url", "string"))
                               .SetAction(new ActionHandler(Permission.Write, OpenInBrowser));

            if (PlatformHelper.Android)
            {
                factory.AddParameter(new Parameter("showTitle", "bool"));
            }

            if (PlatformHelper.iOS)
            {
                factory.AddParameter(new Parameter("readerMode", "bool"));
            }

            _openInBrowser = factory.BuildNode();

            _share = superRoot.CreateChild("share")
                              .SetDisplayName("Share")
                              .SetActionGroup(ActionGroups.Share)
                              .SetInvokable(Permission.Write)
                              .AddParameter(new Parameter("title", "string", "Share"))
                              .AddParameter(new Parameter("text", "string"))
                              .SetAction(new ActionHandler(Permission.Write, Share))
                              .BuildNode();

            _shareLink = superRoot.CreateChild("shareLink")
                                  .SetDisplayName("Share Link")
                                  .SetActionGroup(ActionGroups.Share)
                                  .SetInvokable(Permission.Write)
                                  .AddParameter(new Parameter("url", "string"))
                                  .AddParameter(new Parameter("msg", "string"))
                                  .AddParameter(new Parameter("title", "string"))
                                  .SetAction(new ActionHandler(Permission.Write, ShareLink))
                                  .BuildNode();

            if (CrossShare.Current.SupportsClipboard)
            {
                _setClipboard = superRoot.CreateChild("setClipboard")
                                         .SetDisplayName("Set Clipboard")
                                         .SetActionGroup(ActionGroups.Share)
                                         .SetInvokable(Permission.Write)
                                         .AddParameter(new Parameter("text", "string"))
                                         .SetAction(new ActionHandler(Permission.Write, SetClipboardContents))
                                         .BuildNode();
            }
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        private async void OpenInBrowser(InvokeRequest request)
        {
            // We should just close the request now.
            await request.Close();

            var url = request.Parameters["url"].Value<string>();
            var options = new BrowserOptions();

            var showTitle = request.Parameters["showTitle"];
            var readerMode = request.Parameters["readerMode"];

            if (PlatformHelper.Android && showTitle != null && showTitle.Type == JTokenType.Boolean)
            {
                options.ChromeShowTitle = showTitle.Value<bool>();
            }

            if (PlatformHelper.iOS && readerMode != null && readerMode.Type == JTokenType.String)
            {
                options.UseSafairReaderMode = readerMode.Value<bool>();
            }

            await CrossShare.Current.OpenBrowser(url, options);
        }

        private async void Share(InvokeRequest request)
        {
            await request.Close();

            var title = request.Parameters["title"].Value<string>();
            var text = request.Parameters["text"].Value<string>();

            await CrossShare.Current.Share(text, title);
        }

        private async void ShareLink(InvokeRequest request)
        {
            await request.Close();

            var url = request.Parameters["url"].Value<string>();
            var message = request.Parameters["msg"].Value<string>();
            var title = request.Parameters["title"].Value<string>();

            await CrossShare.Current.ShareLink(url, message, title);
        }

        private async void SetClipboardContents(InvokeRequest request)
        {
            await request.Close();

            var text = request.Parameters["text"].Value<string>();

            await CrossShare.Current.SetClipboardText(text);
        }
    }
}

