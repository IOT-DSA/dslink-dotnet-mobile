using System.Collections.ObjectModel;
using DSLink.Nodes;
using Xamarin.Forms;

namespace DSAMobile.Views
{
    public class RequesterView : ListView
    {
        private readonly string _path;
        private readonly ObservableCollection<NodeObject> _items;

        public RequesterView(string path, bool load = false)
        {
            _path = path;
            _items = new ObservableCollection<NodeObject>();

            IsPullToRefreshEnabled = true;
            RefreshCommand = new Command(Load);

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

                var nodeObject = ((NodeObject)e.Item);

                if (nodeObject.Action)
                {
                    Navigation.PushAsync(new ContentPage
                    {
                        Content = new StackLayout
                        {
                            Children =
                            {
                                new Label
                                {
                                    Text = "I'm an action!"
                                }
                            }
                        }
                    });
                }
                else if (nodeObject.Value)
                {
                    Navigation.PushAsync(new ValuePage(nodeObject.Node.Path));
                }
                else
                {
                    Navigation.PushAsync(new ContentPage
                    {
                        Title = nodeObject.Node.DisplayName,
                        Content = new RequesterView(nodeObject.Node.Path, true)
                    });
                }
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
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsRefreshing = true;
                    _items.Clear();
                    foreach (Node node in response.Node.Children.Values)
                    {
                        _items.Add(new NodeObject(node));
                    }
                    IsRefreshing = false;
                });
            });
        }
    }

    public class NodeObject
    {
        public readonly Node Node;
        public readonly bool Value;
        public readonly bool Action;

        public NodeObject(Node node)
        {
            Node = node;
            Value = node.Configurations.ContainsKey("$type");
            Action = node.Configurations.ContainsKey("$invokable");
        }

        public override string ToString()
        {
            return Node.DisplayName ?? Node.Name;
        }
    }
}
