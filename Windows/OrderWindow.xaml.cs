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
using System.Windows.Shapes;
using AutozaphastiKulckov.Entities;
using AutozaphastiKulckov.Windows;
using AutozaphastiKulckov.Pages;

namespace AutozaphastiKulckov.Windows
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        public OrderWindow(List<Product> products, User user)
        {
            InitializeComponent();

            frmOrder.Navigate(new OrderPage(products, user));
        }
    }
}
