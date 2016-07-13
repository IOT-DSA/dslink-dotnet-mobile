using DSLink.Nodes;
using FormsPlugin.Iconize;
using Xamarin.Forms;

namespace DSAMobile.Views
{
    public class RequesterView : TableView
    {
        private readonly string _path;
        private readonly TableSection _nodesSection;
        private readonly TableSection _valuesSection;
        private readonly TableSection _actionsSection;

        public RequesterView(string path, bool load = false)
        {
            _path = path;

            _nodesSection = new TableSection("Nodes");
            _valuesSection = new TableSection("Values");
            _actionsSection = new TableSection("Actions");

            Root = new TableRoot
            {
                _nodesSection,
                _valuesSection,
                _actionsSection
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
                    _nodesSection.Clear();
                    _valuesSection.Clear();
                    _actionsSection.Clear();
                    foreach (Node node in response.Node.Children.Values)
                    {
                        if (node.Configurations.ContainsKey("$type"))
                        {
                            _valuesSection.Add(new ValueCell(node, Navigation));
                        }
                        else if (node.Configurations.ContainsKey("$invokable"))
                        {
                            _actionsSection.Add(new ActionCell(node, Navigation));
                        }
                        else
                        {
                            _nodesSection.Add(new NodeCell(node, Navigation));
                        }
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
                    new IconButton
                    {
                    }
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

        protected override void OnTapped()
        {
            base.OnTapped();

            _navigation.PushAsync(new ContentPage
            {
                Title = _node.DisplayName,
                Content = new RequesterView(_node.Path, true)
            });
        }
    }

    public class ValueCell : CustomNodeCell
    {
        public ValueCell(Node node, INavigation navigation) : base(node, navigation)
        {
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

        protected override void OnTapped()
        {
            base.OnTapped();

            _navigation.PushAsync(new ValuePage(_node.Path));
        }
    }

    public class ActionCell : CustomNodeCell
    {
        public ActionCell(Node node, INavigation navigation) : base(node, navigation)
        {
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

        protected override void OnTapped()
        {
            base.OnTapped();

            _navigation.PushAsync(new ContentPage
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
