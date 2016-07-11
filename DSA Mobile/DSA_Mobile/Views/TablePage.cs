using Xamarin.Forms;

namespace DSAMobile.Views
{
    public class TablePage : DGPage
    {
        protected readonly TableView _table;

        public TablePage()
        {
            _table = new TableView()
            {
                Root = new TableRoot()
                {
                },
                Intent = TableIntent.Data
            };

            Content = _table;
        }
    }
}
