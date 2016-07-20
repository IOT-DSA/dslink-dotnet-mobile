using System.Collections.Generic;
using DSLink.Nodes;
using DSLink.Nodes.Actions;
using DSLink.Request;
using ZXing;
using ZXing.Mobile;
using Action = DSLink.Nodes.Actions.Action;

namespace DSAMobile.ZXing
{
    public class ZXingModule : BaseModule
    {
        public bool Supported => true;

        private MobileBarcodeScanner _scanner;
        private Node _scanCode;

        public ZXingModule()
        {
            _scanner = new MobileBarcodeScanner();
        }

        public bool RequestPermissions()
        {
            return true;
        }

        public void AddNodes(Node superRoot)
        {
            _scanCode = superRoot.CreateChild("scan_barcode")
                                 .SetDisplayName("Scan Barcode")
                                 .SetActionGroup(ActionGroups.Camera)
                                 .AddColumn(new Column("result", "dynamic"))
                                 .AddColumn(new Column("type", "string"))
                                 .AddColumn(new Column("timestamp", "number"))
                                 .SetAction(new Action(Permission.Read, ScanCode))
                                 .BuildNode();
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        private async void ScanCode(Dictionary<string, Value> parameters, InvokeRequest request)
        {
            var result = await _scanner.Scan();

            if (result == null)
            {
                request.Close();
                return;
            }

            request.SendUpdates(new List<dynamic>
            {
                new List<dynamic>
                {
                    result.Text,
                    result.BarcodeFormat.ToString(),
                    result.Timestamp
                }
            }, close: true);
        }
    }
}

