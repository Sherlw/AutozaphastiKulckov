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

namespace AutozaphastiKulckov.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrderTickerPage.xaml
    /// </summary>
    public partial class OrderTickerPage : Page
    {
        List<Product> productList = new List<Product>();
        public OrderTickerPage(Order currentOrder, List<Product> products)
        {
            InitializeComponent();

            productList = products;
            DataContext= currentOrder;

            txtPickopPoint.Text = currentOrder.PickUpPoint.address;

            var result = "";

            foreach (var pl in productList)
                result += (result == "" ? "" : ", ") + pl.name.ToString();
            txtProductList.Text = result.ToString();

            var total = productList.Sum(p => Convert.ToDouble(p.category) - Convert.ToDouble(p.category) * Convert.ToDouble(p.CurrentDiscount / 100.00));
            txtCost.Text = total.ToString() + " рублей";

            var discountSum = productList.Sum(p => p.CurrentDiscount);
            txtDiscountSum.Text = discountSum.ToString() + " %";

        }

        private void btnSaveDocument_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            if(pd.ShowDialog() == true)
            {
                IDocumentPaginatorSource idp = flowDoc;
                pd.PrintDocument(idp.DocumentPaginator, Title);
            }
        }
    }
}
