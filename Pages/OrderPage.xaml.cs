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
using AutozaphastiKulckov.Entities;
using AutozaphastiKulckov.Pages;


namespace AutozaphastiKulckov.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        List<Product> productList = new List<Product>();
        public OrderPage(List<Product> products, User user)
        {
            InitializeComponent();

            DataContext = this;
            productList = products;
            lvieworder.ItemsSource = productList;

            cmbpickuppoint.ItemsSource = automarketKulckovEntities2.GetContext().PickUpPoint.ToList();

            
        }

        public string Total
        {
            get
            {
                var total = productList.Sum(p => Convert.ToDouble(p.category) - Convert.ToDouble(p.category) * Convert.ToDouble(p.CurrentDiscount / 100.00));
                return total.ToString();
            }
        }
        private void btndeleteproduct_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что хотите удалить этот элемент?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                productList.Remove(lvieworder.SelectedItem as Product);
        }

        private void btnordersave_Click(object sender, RoutedEventArgs e)
        {
            var productArticle = productList.Select(p => p.articul).ToArray();
            Random random = new Random();
            var date = DateTime.Now;
            if (productList.Any(p => p.Count_warehouse < 3))
                date = date.AddDays(6);
            else
                date = date.AddDays(3);

            if (cmbpickuppoint.SelectedItem == null)
            {
                MessageBox.Show("Выберите пункт выдачи!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                Order neworder = new Order()
                {
                    Idstatus = 1,
                    Create_data = DateTime.Now,
                    IdPickUpPount = cmbpickuppoint.SelectedIndex + 1,
                    DeliveryDate = date,
                    CodeForGet = random.Next(100, 1000),
                    idClient = Convert.ToInt32(txtUser.Text)
                
                };
                automarketKulckovEntities2.GetContext().Order.Add(neworder);

                for (int i = 0; i < productArticle.Count(); i++)
                {
                    OrderComposition newOrderProduct = new OrderComposition()
                    {
                        IdOrder= neworder.id,
                        ProductArticle = productArticle[i],
                        CountOrder = 1

                    };
                }

                automarketKulckovEntities2.GetContext().SaveChanges();
                MessageBox.Show("Заказ оформлен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new OrderTickerPage(neworder, productList));
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void cmbpickuppoint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void lvieworder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
