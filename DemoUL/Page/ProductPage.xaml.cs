using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public class SortItem
    {
        public string Name { get; set; }
        public SortDescription Sort { get; set; }
    }

    public partial class ProductPage : Page
    {
        public user2Entities connection;

        public List<SortItem> SortDescriptions { get; set; }
        public SortItem SelectedSortDescription { get; set; }
        public ProductPage()
        {
            InitializeComponent();

            connection = new user2Entities();

            BindingLvProduct();

            SortDescriptions = new List<SortItem>()
            {
                new SortItem()
                {
                    Name = "Цена по возрастанию",
                    Sort = new SortDescription("Cost", ListSortDirection.Ascending),
                },
                new SortItem()
                {
                    Name = "Цена по убыванию",
                    Sort = new SortDescription("Cost", ListSortDirection.Descending),
                },
                new SortItem()
                {
                    Name = "Количество по возрастанию",
                    Sort = new SortDescription("QuantityInStock", ListSortDirection.Ascending),
                },
                new SortItem()
                {
                    Name = "Количество по убыванию",
                    Sort = new SortDescription("QuantityInStock", ListSortDirection.Descending),
                },
            };
            DataContext = this;
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

        private void ApplySort()
        {
            var view = CollectionViewSource.GetDefaultView(LvProduct.ItemsSource);
            if (view == null || SelectedSortDescription == null) return; 

            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(SelectedSortDescription.Sort);
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplySort();
        }
    }
}
