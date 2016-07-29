using System;
using System.Collections.Generic;
using System.Diagnostics;
using DSAMobile.Controls;
using DSAMobile.Views.Cells;
using DSLink.Nodes;
using DSLink.Respond;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace DSAMobile
{
    public class ActionPage : ContentPage
    {
        private readonly string _path;
        private readonly DGButton _invokeButton;
        private readonly TableRoot _tableRoot;
        private readonly TableSection _parametersSection;
        private readonly TableSection _columnsSection;
        private readonly Dictionary<string, Cell> _parameterCells;
        private readonly List<Cell> _columnCells;
        private Permission _invokePermission;

        public ActionPage(string path)
        {
            _path = path;
            _parameterCells = new Dictionary<string, Cell>();
            _columnCells = new List<Cell>();

            _invokeButton = new DGButton
            {
                Text = "Invoke",
                IsEnabled = false,
                Command = new Command(() =>
                {
                    if (_invokePermission == null)
                    {
                        throw new Exception("Invoke permission is null.");
                    }
                    App.Instance.DSLink.Requester.Invoke(_path,
                                                         _invokePermission,
                                                         CollectParameters(),
                                                         (InvokeResponse response) =>
                    {
                        SetOutputs(response.Updates);
                        response.Close();
                    });
                })
            };
            _parametersSection = new TableSection("Parameters");
            _columnsSection = new TableSection("Columns");

            _tableRoot = new TableRoot();

            Content = new StackLayout
            {
                Children =
                {
                    _invokeButton,
                    new TableView
                    {
                        Root = _tableRoot,
                        Intent = TableIntent.Form
                    }
                }
            };

            App.Instance.DSLink.Requester.List(_path, ListCallback);
        }

        private void SetOutputs(JArray updates)
        {
            if (updates != null && updates[0] != null && updates[0].Type == JTokenType.Array)
            {
                JArray update = updates[0].Value<JArray>();
                for (int i = 0; i < update.Count; i++)
                {
                    var cell = _columnCells[i];
                    var val = update[i];
                    if (cell != null)
                    {
                        if (cell is EntryCell)
                        {
                            ((EntryCell)cell).Text = val.ToString();
                        }
                        else if (cell is SwitchCell)
                        {
                            if (val.Type == JTokenType.Boolean)
                            {
                                ((SwitchCell)cell).On = val.Value<bool>();
                            }
                        }
                        else if (cell is PickerCell)
                        {
                            // TODO
                        }
                    }
                }
            }
        }

        private JObject CollectParameters()
        {
            var obj = new JObject();

            foreach (KeyValuePair<string, Cell> kp in _parameterCells)
            {
                if (kp.Value is EntryCell)
                {
                    obj[kp.Key] = ((EntryCell)kp.Value).Text;
                }
                else if (kp.Value is SwitchCell)
                {
                    obj[kp.Key] = ((SwitchCell)kp.Value).On;
                }
            }

            return obj;
        }

        private void ListCallback(ListResponse response)
        {
            _invokeButton.IsEnabled = true;
            var parameters = response.Node.Parameters;
            var columns = response.Node.Columns;
            _invokePermission = response.Node.Invokable;

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    Cell cell = null;
                    var name = param["name"].Value<string>();
                    var type = param["type"].Value<string>();
                    string[] typeParams = new string[0];
                    var bracketIndex = type.IndexOf('[');
                    if (bracketIndex != -1)
                    {
                        type = type.Substring(0, bracketIndex);
                        typeParams = param["type"].Value<string>().Substring(bracketIndex + 1).TrimEnd(new char[] { ']' }).Split(',');
                    }
                    switch (type)
                    {
                        case "string":
                            {
                                cell = new EntryCell
                                {
                                    Label = name
                                };
                                break;
                            }
                        case "int":
                        case "number":
                            {
                                cell = new EntryCell
                                {
                                    Label = name,
                                    Keyboard = Keyboard.Numeric
                                };
                                break;
                            }
                        case "bool":
                            {
                                cell = new SwitchCell
                                {
                                    Text = name
                                };
                                break;
                            }
                        case "enum":
                            cell = new PickerCell(new List<string>(typeParams))
                            {
                                Label = name
                            };
                            break;
                        case "binary":
                        case "group":
                        case "dynamic":
                            {
                                cell = new EntryCell
                                {
                                    Label = name
                                };
                                break;
                            }
                        default:
                            {
                                Debug.WriteLine(string.Format("Unknown type: {0}", type));
                            }
                            break;
                    }
                    if (cell != null)
                    {
                        _parameterCells.Add(name, cell);
                        Device.BeginInvokeOnMainThread(() => _parametersSection.Add(cell));
                    }
                }
                if (parameters.Count > 0)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        _tableRoot.Add(_parametersSection);
                    });
                }
            }

            if (columns != null)
            {
                foreach (var column in columns)
                {
                    var name = columns["name"].Value<string>();
                    var type = columns["type"].Value<string>();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Cell cell;
                        switch (type)
                        {
                            case "string":
                            case "binary":
                            case "dynamic":
                                {
                                    cell = new EntryCell
                                    {
                                        IsEnabled = false,
                                        Label = name
                                    };
                                    break;
                                }
                            case "number":
                            case "int":
                                {
                                    cell = new EntryCell
                                    {
                                        IsEnabled = false,
                                        Label = name
                                    };
                                    break;
                                }
                            case "bool":
                                {
                                    cell = new SwitchCell
                                    {
                                        IsEnabled = false,
                                        Text = name
                                    };
                                    break;
                                }
                            default:
                                {
                                    throw new Exception(string.Format("Unknown type: {0}", column.Type));
                                }
                        }
                        if (cell != null)
                        {
                            _columnCells.Add(cell);
                            Device.BeginInvokeOnMainThread(() => _columnsSection.Add(cell));
                        }
                    });
                }
                if (columns.Count > 0)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        _tableRoot.Add(_columnsSection);
                    });
                }
            }

            response.Close();
        }
    }
}
