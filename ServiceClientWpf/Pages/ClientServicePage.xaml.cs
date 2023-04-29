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
using ServiceClientWpf.Model;

namespace ServiceClientWpf.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientServicePage.xaml
    /// </summary>
    public partial class ClientServicePage : Page
    {
        Service contextService;

        public ClientServicePage(Service service)
        {
            InitializeComponent();
            contextService = service;
            TBService.DataContext = contextService;
            CBClient.ItemsSource = App.DB.Client.ToList();
        }

        private void BSave_Click(object sender, RoutedEventArgs e)
        {
            string errorMessage = "";

            if (CBClient.SelectedItem == null)
            {
                errorMessage += "Выберите клиента\n";
            }

            if (!(contextService.DurationInSeconds > 0 && contextService.DurationInSeconds <= 14400))
            {
                errorMessage += "Введите время\n";
            }

            if (string.IsNullOrWhiteSpace(errorMessage) == false)
            {
                MessageBox.Show(errorMessage);
                return;
            }





            if (contextService.ID == 0)
            {
                App.DB.Service.Add(contextService);
            }

            App.DB.SaveChanges();

            NavigationService.Navigate(new ServiceListPage());
        }
    }
}
