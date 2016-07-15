using System.Collections.Generic;
using Xamarin.Forms;

namespace DSAMobile.Views.Cells
{
    public class PickerCell : ViewCell
    {
        private readonly Label _label;
        private readonly Picker _picker;

        public string Label
        {
            set
            {
                _label.Text = value;
            }
        }

        public PickerCell(List<string> items)
        {
            _label = new Label
            {
                VerticalOptions = LayoutOptions.Center
            };
            _picker = new Picker
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            foreach (string item in items)
            {
                _picker.Items.Add(item);
            }

            View = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    _label,
                    _picker
                },
                Padding = new Thickness(left: 10,
                                        right: 10,
                                        bottom: 0,
                                        top: 0)
            };
        }
    }
}
