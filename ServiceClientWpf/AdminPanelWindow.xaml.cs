using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ServiceClientWpf.Pages;

namespace ServiceClientWpf
{
    /// <summary>
    /// Логика взаимодействия для AdminPanelWindow.xaml
    /// </summary>
    public partial class AdminPanelWindow : Window
    {
        public AdminPanelWindow()
        {
            InitializeComponent();
        }

        private void BSign_Click(object sender, RoutedEventArgs e)
        {
            var window = Application.Current.MainWindow as MainWindow;
            if (window != null && TBCode.Text == "0000")
            {
                App.AdminCode = true;
                window.MainFrame.Navigate(new ServiceListPage());
                this.Close();
                window.BEntries.Visibility = Visibility.Visible;
            }
        }
    }
}
