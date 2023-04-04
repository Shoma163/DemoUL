using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemoUL
{
    public partial class MainWindow : Window
    {
        private user2Entities connection;
        public MainWindow()
        {
            InitializeComponent();

            connection = new user2Entities();

            BindingLvProduct();
        }

        public void BindingLvProduct()
        {
            ObservableCollection<Product> products = new ObservableCollection<Product>(connection.Products.ToList());

            LvProduct.SetBinding(ItemsControl.ItemsSourceProperty, new Binding()
            {
                Source = products
            });

        }

        private void Filter()
        {
            string searchString = Search.Text.Trim();
           // string category = "";

            var view = CollectionViewSource.GetDefaultView(LvProduct.ItemsSource);
            view.Filter = new Predicate<object>(o =>
            {
                Product product = o as Product;
                if (product == null) { return false; }

                bool isVisible = true;
                if (searchString.Length > 0)
                {
                    isVisible = product.Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1 ||
                        product.Description.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1;
                }

                //isVisible = isVisible && category == product.Category;

                return isVisible;
            });
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }
    }
}
