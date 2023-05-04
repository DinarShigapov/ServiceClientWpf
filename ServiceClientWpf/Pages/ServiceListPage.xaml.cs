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

        int MaxPage = 0; 
        int currentPage = 0;
        int inpages = 0;

        public ServiceListPage()
        {
            InitializeComponent();
            InPages.SelectedIndex = 0;
            CBFilter.SelectedIndex = 0;
            CBSort.SelectedIndex = 0;
            Refresh();
        }



        private void Refresh() 
        {
            IEnumerable<Service> services = App.DB.Service.Where(x => x.IsDelete == false).ToList();
            if (CBSort.SelectedIndex == 1)
                services = services.OrderBy(x => x.CostDiscount);
            else if (CBSort.SelectedIndex == 2)
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
                default:
                    break;
                    
            }



            if (TBSearch.Text.Length > 0)
                services = services.Where(x => x.Title.ToLower().StartsWith(TBSearch.Text.ToLower())
                || x.Description.ToLower().StartsWith(TBSearch.Text.ToLower()));

            MaxPage = 0;
            int servCount = 0;
            do
            {
                servCount += inpages; 
                MaxPage++;
            } 
            while (servCount < services.Count());

            TBCountPage.Text = $"{currentPage + 1}/{MaxPage}";
            services = services.Skip(inpages * currentPage).Take(inpages);

            LVService.ItemsSource = services.ToList();
            TBCountService.Text = $"{services.Count()} из {App.DB.Service.Where(x => x.IsDelete == false).ToList().Count}";
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

            //var checkSerivice = App.DB.ClientService.ToList();

            //if (checkSerivice.FirstOrDefault(x => x.StartTime > DateTime.Now
            //&& x.Service == deleteService) != null)
            //{
            //    MessageBox.Show("Нельзя удалить услугу, т.к на неё есть запись");
            //    return;
            //}
            //else
            //{
            //    deleteService.IsDelete = true;
            //}
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

        private void InPages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InPages.SelectedIndex == 0)
            {
                inpages = 3;
            }
            if (InPages.SelectedIndex == 1)
            {
                inpages = 5;
            }
            if (InPages.SelectedIndex == 2)
            {
                inpages = 10;
            }
            currentPage = 0;
            Refresh();
        }

        private void BButton_Click(object sender, RoutedEventArgs e)
        {

            if (sender == BRight)
            {
                if (currentPage == MaxPage - 1)
                    currentPage = 0;
                else currentPage++;
            }
            else if (sender == BLeft)
            {
                if (currentPage == 0)
                    currentPage = MaxPage - 1;
                else currentPage--;
            }
            Refresh();
        }

    }
}
