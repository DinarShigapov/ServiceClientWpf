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
using ServiceClientWpf.Pages;

namespace ServiceClientWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new ServiceListPage());
            SPAdminPanel.Visibility = VisibilityAdminButton;



        }

        private void BAdmin_Click(object sender, RoutedEventArgs e)
        {
            AdminPanelWindow admin = new AdminPanelWindow();
            if (App.AdminCode == true)
            {
                App.AdminCode = false;
                MainFrame.Navigate(new ServiceListPage());
                SPAdminPanel.Visibility = Visibility.Collapsed;
            }
            else
                admin.Show();

        }

        private void BAddService_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AddEditServicePage(new Service()));
        }

        private void BEntries_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UpcomingEntriesPage());
        }

        private void BServiceList_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ServiceListPage());
        }

        public static Visibility VisibilityAdminButton
        {
            get
            {
                if (App.AdminCode == true)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }
    }
}
