using AutozaphastiKulckov.Entities;
using AutozaphastiKulckov.Windows;
using System;
using System.Collections.Generic;
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


namespace AutozaphastiKulckov.Pages
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
  
    public partial class Admin : Page
    {
        User user = new User();
        public Admin(User currentUser)
        {
            InitializeComponent();

            var product = automarketKulckovEntities2.GetContext().Product.ToList();
            LViewProduct.ItemsSource = product;
            DataContext = this;

            txtallamount.Text = product.Count().ToString();

            UpdateData();
        }

        private void User()
        {
            if (automarketKulckovEntities2.GetContext().User != null)
                txtfullname.Text = user.fio.ToString();
            else
                txtfullname.Text = "Гость";
        }
        public string[] SortingList { get; set; } =
        {
            "Без сортировки",
            "Стоимость по возрастанию",
            "Стоимость по убыванию"
        };

        public string[] FilterList { get; set; } =
        {
            "Все диапазоны",
            "0%-9,99%",
            "10%-14,99%",
            "15% и более"
        };

        private void UpdateData()
        {
            var result = automarketKulckovEntities2.GetContext().Product.ToList();

            if (cmbSorting.SelectedIndex == 1)
                result = result.OrderBy(p => p.category).ToList();
            if (cmbSorting.SelectedIndex == 2)
                result = result.OrderByDescending(p => p.category).ToList();

            if (cmbFilter.SelectedIndex == 1)
                result = result.Where(p => p.CurrentDiscount >= 0 && p.CurrentDiscount < 10).ToList();
            if (cmbFilter.SelectedIndex == 2)
                result = result.Where(p => p.CurrentDiscount >= 10 && p.CurrentDiscount < 15).ToList();
            if (cmbFilter.SelectedIndex == 3)
                result = result.Where(p => p.CurrentDiscount >= 15).ToList();

            result = result.Where(p => p.name.ToLower().Contains(txtSearch.Text.ToLower())).ToList();
            LViewProduct.ItemsSource = result;

            txtResultAmount.Text = result.Count().ToString();
        }
        private void txtSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void cmbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        List<Product> orderproducts = new List<Product>();
        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            orderproducts.Add(LViewProduct.SelectedItem as Product);

            if (orderproducts.Count > 0)
            {
                btnOrder.Visibility = Visibility.Visible;
            }
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow order = new OrderWindow(orderproducts, user);
            order.ShowDialog();
        }

        private void btnAddNewProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditProductPage(LViewProduct.SelectedItem as Product));
        }

        private void LViewProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new AddEditProductPage(null));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility== Visibility.Visible)
            {
                automarketKulckovEntities2.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                LViewProduct.ItemsSource = automarketKulckovEntities2.GetContext().Product.ToList();
            }
        }
    }
}
