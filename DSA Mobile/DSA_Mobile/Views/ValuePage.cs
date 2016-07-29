using DSLink.Request;
using DSLink.Respond;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace DSAMobile.Views
{
    public class ValuePage : ContentPage
    {
        private readonly TableView _tableView;
        private readonly TextCell _valueCell;
        private readonly TextCell _typeCell;
        private string _path;

        public ValuePage(string path)
        {
            _path = path;

            _valueCell = new TextCell();
            _typeCell = new TextCell();

            _tableView = new TableView
            {
                Root = new TableRoot
                {
                    new TableSection
                    {
                        _valueCell,
                        _typeCell
                    }
                }
            };

            Content = _tableView;

            Appearing += async delegate
            {
                await App.Instance.DSLink.Requester.List(_path, ListUpdate);
                await App.Instance.DSLink.Requester.Subscribe(_path, ValueUpdate);
            };

            Disappearing += delegate
            {
                App.Instance.DSLink.Requester.Unsubscribe(_path);
            };
        }

        public async void ListUpdate(ListResponse response)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                _typeCell.Text = response.Node.GetConfig("type").String;
            });
            await response.Close();
        }

        public void ValueUpdate(SubscriptionUpdate update)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                _valueCell.Text = update.Value.ToString();
            });
        }
    }
}
