using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ServiceClientWpf.Model;
using System.Data.Entity;

namespace ServiceClientWpf.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServiceListPage.xaml
    /// </summary>
    public partial class ServiceListPage : Page
    {

        private int _maxPage = 0; 
        private int _currentPage = 0;
        private int _inPages = 0;

        public ServiceListPage()
        {
            InitializeComponent();
            InPages.SelectedIndex = 0;
            CBFilter.SelectedIndex = 0;
            CBSorting.SelectedIndex = 0;
            Refresh();
        }

        private void Refresh() 
        {
            IEnumerable<Service> services = App.DB.Service.Where(x => x.IsDelete == false).ToList();
            if (CBSorting.SelectedIndex == 1)
                services = services.OrderBy(x => x.CostDiscount);
            else if (CBSorting.SelectedIndex == 2)
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

            _maxPage = 0;
            int servCount = 0;
            do
            {
                servCount += _inPages;
                _maxPage++;
            } 
            while (servCount < services.Count());

            TBCountPage.Text = $"{_currentPage + 1}/{_maxPage}";
            services = services.Skip(_inPages * _currentPage).Take(_inPages);

            LVService.ItemsSource = services.ToList();
            TBCountService.Text = $"{services.Count()} из {App.DB.Service.Where(x => x.IsDelete == false).ToList().Count}";
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

            var checkSerivice = App.DB.ClientService.ToList();

            var fd = checkSerivice.FirstOrDefault(x => x.StartTime > DateTime.Now
            && x.Service == deleteService);

            if (checkSerivice.FirstOrDefault(x => x.StartTime > DateTime.Now
            && x.Service == deleteService) != null)
            {
                MessageBox.Show("Нельзя удалить услугу, т.к на неё есть запись", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                //var delete = App.DB.Service.FirstOrDefault(x => x.ID == deleteService.ID);
                deleteService.IsDelete = true;
            }

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
            switch (InPages.SelectedIndex)
            {
                case 0:
                    _inPages = 3;
                    break;
                case 1:
                    _inPages = 5;
                    break;
                case 2:
                    _inPages = 10;
                    break;
            }

            _currentPage = 0;
            Refresh();
        }

        private void BSlideList_Click(object sender, RoutedEventArgs e)
        {
            if (sender == BRight)
            {
                if (_currentPage == _maxPage - 1)
                    _currentPage = 0;
                else _currentPage++;
            }
            else if (sender == BLeft)
            {
                if (_currentPage == 0)
                    _currentPage = _maxPage - 1;
                else _currentPage--;
            }
            Refresh();
        }

        private void CBSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
    }
}
