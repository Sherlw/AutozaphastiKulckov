using AutozaphastiKulckov.Entities;
using Microsoft.Win32;
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
    /// Логика взаимодействия для AddEditProductPage.xaml
    /// </summary>
    public partial class AddEditProductPage : Page
    {
        Product product = new Product();
        public AddEditProductPage(Product currentProduct)
        {
            InitializeComponent();

            if(currentProduct != null)
            {
                product = currentProduct;

                btndeleteproduct.Visibility = Visibility.Visible;
                txtArticle.IsEnabled = false;
            }
            DataContext = product;
            cmbCategory.ItemsSource = CategoryList;
        }

        public string[] CategoryList =
        {
            "Аксессуары",
            "Автозапчасти",
            "Автосервис",
            "Съемники подшипников",
            "Ручные инструменты",
            "Зарядные устройства"
        };
        private void btnenterimage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog GetImageDialog = new OpenFileDialog();

            GetImageDialog.Filter = "Файлы изображений: (*.png, *.jpg, *.jpeg)| *.png, *.jpg, *.jpeg";
            GetImageDialog.InitialDirectory = "C:\\Users\\Cab-042-n9\\Desktop\\AutozaphastiKulckov\\Resources";
            if (GetImageDialog.ShowDialog() == true)
            {
                product.Photo = GetImageDialog.SafeFileName;
            }
        }

        private void btndeleteproduct_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show($"Вы действительно хотите удалить {product.name}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    automarketKulckovEntities2.GetContext().Product.Remove(product);
                    automarketKulckovEntities2.GetContext().SaveChanges();
                    MessageBox.Show("Запись удалена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.GoBack();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnsaveproduct_click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (product.category < 0)
                errors.AppendLine("Стоимость не может быть отрицательной!");
            if (product.Count_warehouse < 0)
                errors.AppendLine("Минимальное количество на складе не может быть отрицательным!");
            if (product.CurrentDiscount > product.max_discount)
                errors.AppendLine("Действующая скидка на товар не может быть больше максимальной скидки!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (product.articul == null)
                automarketKulckovEntities2.GetContext().Product.Add(product);
            try
            {
                automarketKulckovEntities2.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}
