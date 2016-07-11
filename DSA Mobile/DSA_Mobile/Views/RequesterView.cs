using System.Collections.ObjectModel;
using DSAMobile.Views;
using DSLink.Nodes;
using Xamarin.Forms;

namespace DSAMobile
{
    public class RequesterView : ListView
    {
        private readonly string _path;
        private readonly ObservableCollection<NodeObject> _items;

        public RequesterView(string path, bool load = false)
        {
            _path = path;
            _items = new ObservableCollection<NodeObject>();

            ItemsSource = _items;

            ItemSelected += (sender, e) =>
            {
                ((ListView)sender).SelectedItem = null;
            };

            ItemTapped += (object sender, ItemTappedEventArgs e) =>
            {
                if (e.Item == null)
                {
                    return;
                }

                var node = ((NodeObject)e.Item).Node;

                var p = new ContentPage
                {
                    Title = node.Path,
                    Content = new RequesterView(node.Path, true)
                };

                Navigation.PushAsync(p);
            };

            if (load)
            {
                Load();
            }
        }

        public void Load()
        {
            App.Instance.DSLink.Requester.List(_path, (response) =>
            {
                foreach (Node node in response.Node.Children.Values)
                {
                    _items.Add(new NodeObject(node));
                }
            });
        }
    }

    public class NodeObject
    {
        public readonly Node Node;

        public NodeObject(Node node)
        {
            Node = node;
        }

        public override string ToString()
        {
            return Node.DisplayName ?? Node.Name;
        }
    }
}
