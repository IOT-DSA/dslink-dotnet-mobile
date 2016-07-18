using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DSLink.Nodes;
using DSLink.Request;
using Xamarin.Forms;

namespace DSAMobile.Views
{
    public class RequesterView : TableView
    {
        private readonly string _path;
        public List<string> SubscribedPaths;
        public readonly TableSection Nodes;
        public readonly TableSection Values;
        public readonly TableSection Actions;

        public RequesterView(string path, bool load = false)
        {
            _path = path;
            SubscribedPaths = new List<string>();

            Root = new TableRoot();

            Nodes = new TableSection("Nodes");
            Values = new TableSection("Values");
            Actions = new TableSection("Actions");

            if (load)
            {
                Load();
            }
        }

        public async void Load()
        {
            await App.Instance.DSLink.Requester.List(_path, (response) =>
            {
                // TODO: We really need a way to relayout without removing everything...
                Device.BeginInvokeOnMainThread(() =>
                {
                    Root.Clear();
                    Nodes.Clear();
                    Values.Clear();
                    Actions.Clear();
                });

                //App.Instance.DSLink.Connector.EnableQueue = true;

                var nodes = new Dictionary<string, NodeCell>();
                var values = new Dictionary<string, ValueCell>();
                var actions = new Dictionary<string, ActionCell>();

                foreach (Node node in response.Node.Children.Values)
                {
                    if (node.Configurations.ContainsKey("$type"))
                    {
                        var cell = new ValueCell(node, Navigation, SubscribedPaths);
                        values.Add(cell.FriendlyName, cell);
                    }
                    else if (node.Configurations.ContainsKey("$invokable"))
                    {
                        var cell = new ActionCell(node, Navigation);
                        actions.Add(cell.FriendlyName, cell);
                    }
                    else
                    {
                        var cell = new NodeCell(node, Navigation);
                        nodes.Add(cell.FriendlyName, cell);
                    }
                }

                var sortedNodes = nodes.Keys.ToList();
                var sortedValues = values.Keys.ToList();
                var sortedActions = actions.Keys.ToList();
                sortedNodes.Sort();
                sortedValues.Sort();
                sortedActions.Sort();

                Device.BeginInvokeOnMainThread(() =>
                {
                    foreach (string node in sortedNodes)
                    {
                        Nodes.Add(nodes[node]);
                    }
                    foreach (string node in sortedValues)
                    {
                        Values.Add(values[node]);
                    }
                    foreach (string node in sortedActions)
                    {
                        Actions.Add(actions[node]);
                    }
                });

                //App.Instance.DSLink.Connector.EnableQueue = false;

                Device.BeginInvokeOnMainThread(() =>
                {
                    if (Nodes.Count > 0)
                    {
                        Root.Add(Nodes);
                    }
                    if (Values.Count > 0)
                    {
                        Root.Add(Values);
                    }
                    if (Actions.Count > 0)
                    {
                        Root.Add(Actions);
                    }
                });
            });
        }
    }

    public class CustomNodeCell : ViewCell
    {
        protected readonly Node _node;
        protected readonly INavigation _navigation;

        public CustomNodeCell(Node node, INavigation navigation)
        {
            _node = node;
            _navigation = navigation;
        }

        public string FriendlyName
        {
            get
            {
                if (_node.DisplayName != null)
                {
                    return _node.DisplayName;
                }
                return _node.Name;
            }
        }
    }

    public class NodeCell : CustomNodeCell
    {
        public NodeCell(Node node, INavigation navigation) : base(node, navigation)
        {
            Tapped += async (sender, e) =>
            {
                await _navigation.PushAsync(new RequesterPageWrapper(
                    _node.Path,
                    new RequesterView(_node.Path, true)
                ));
            };

            View = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(left: 5, right: 5, bottom: 0, top: 0),
                Children =
                {
                    new Label
                    {
                        Text = FriendlyName,
                        TextColor = Color.Default,
                        VerticalOptions = LayoutOptions.Center
                    },
                    /*new Label
                    {
                        Text = ">  ",
                        Scale = 3.0,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.EndAndExpand
                    }*/
                }
            };
        }
    }

    public class ValueCell : CustomNodeCell
    {
        private Label _valueLabel;

        public ValueCell(Node node, INavigation navigation, List<string> subscribedPaths) : base(node, navigation)
        {
            Tapped += async (sender, e) =>
            {
                await _navigation.PushAsync(new ValuePage(_node.Path));
            };

            _valueLabel = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };

            View = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(left: 5, right: 5, bottom: 0, top: 0),
                Children =
                {
                    new Label
                    {
                        Text = FriendlyName,
                        TextColor = Color.Default,
                        VerticalOptions = LayoutOptions.Center
                    },
                    _valueLabel
                }
            };

            subscribedPaths.Add(_node.Path);
            App.Instance.DSLink.Requester.Subscribe(_node.Path, ValueUpdate);
        }

        private void ValueUpdate(SubscriptionUpdate update)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                _valueLabel.Text = update.Value;
            });
        }
    }

    public class ActionCell : CustomNodeCell
    {
        public ActionCell(Node node, INavigation navigation) : base(node, navigation)
        {
            Tapped += async (sender, e) =>
            {
                await _navigation.PushAsync(new ActionPage(_node.Path)
                {
                    Title = FriendlyName
                });
            };

            View = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(left: 5, right: 5, bottom: 0, top: 0),
                Children =
                {
                    new Label
                    {
                        Text = FriendlyName,
                        TextColor = Color.Default,
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            };
        }
    }

    public class RequesterPageWrapper : ContentPage
    {
        private bool _unloaded;
        private readonly RequesterView _requesterView;

        public RequesterPageWrapper(string title, RequesterView requesterView)
        {
            Title = title;
            _requesterView = requesterView;
            Content = _requesterView;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_unloaded)
            {
                _requesterView.Load();
                _unloaded = false;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            App.Instance.DSLink.Requester.Unsubscribe(
                _requesterView.SubscribedPaths
            );

            _requesterView.SubscribedPaths.Clear();

            _unloaded = true;
        }
    }
}
