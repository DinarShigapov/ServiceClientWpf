using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using ServiceClientWpf.Model;
using System.Data.Entity;

namespace ServiceClientWpf.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServiceListPage.xaml
    /// </summary>
    public partial class ServiceListPage : Page
    {

        public ServiceListPage()
        {
            InitializeComponent();
            Refresh();
            CBFilter.SelectedIndex = 0;
            CBSort.SelectedIndex = 0;
            
        }

        private void Refresh() 
        {
            IEnumerable<Service> services = App.DB.Service.Where(x => x.IsDelete == false).ToList();
            if (CBSort.SelectedIndex == 1)
                services = services.OrderBy(x => x.CostDiscount);
            else
                services = services.OrderByDescending(x => x.CostDiscount);

            switch (CBFilter.SelectedIndex)
            {
                case 1:
                    services = services.Where(x => x.Discount >= 0 && x.Discount < 0.05 && x.Discount == null).ToList();
                    break;
                case 2:
                    services = services.Where(x => x.Discount >= 0.05 && x.Discount < 0.15).ToList();
                    break;
                case 3:
                    services = services.Where(x => x.Discount >= 0.15 && x.Discount < 0.30).ToList();
                    break;
                case 4:
                    services = services.Where(x => x.Discount >= 0.30 && x.Discount < 0.70).ToList();
                    break;
                case 5:
                    services = services.Where(x => x.Discount >= 0.70 && x.Discount < 1).ToList();
                    break;
            }

            if (TBSearch.Text.Length > 0)
                services = services.Where(x => x.Title.ToLower().StartsWith(TBSearch.Text.ToLower())
                || x.Description.ToLower().StartsWith(TBSearch.Text.ToLower()));

            LVService.ItemsSource = services.ToList();

        }

        private void CBSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void TBSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Refresh();
        }

        private void CBFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void BEdit_Click(object sender, RoutedEventArgs e)
        {
            var editService = (sender as Button).DataContext as Service;
            if (editService == null) return;

            NavigationService.Navigate(new AddEditServicePage(editService));
        }

        private void BDelete_Click(object sender, RoutedEventArgs e)
        {
            var deleteService = (sender as Button).DataContext as Service;
            if (deleteService == null) return;

            deleteService.IsDelete = true;
            App.DB.SaveChanges();
            Refresh();
        }

        private void BAdd_Click(object sender, RoutedEventArgs e)
        {
            var addClientService = (sender as Button).DataContext as Service;
            if (addClientService == null) return;
            NavigationService.Navigate(new ClientServicePage(addClientService));
        }
    }
}
