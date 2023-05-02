using ServiceClientWpf.Model;
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

namespace ServiceClientWpf.Pages
{
    /// <summary>
    /// Логика взаимодействия для UpcomingEntriesPage.xaml
    /// </summary>
    public partial class UpcomingEntriesPage : Page
    {
        public UpcomingEntriesPage()
        {
            DateTime tomorrow = DateTime.Today.AddDays(1).AddHours(DateTime.Now.Hour);
            InitializeComponent();
            LVService.ItemsSource = App.DB.ClientService.Where(
                x => x.StartTime >= DateTime.Now
                && x.StartTime <= tomorrow
                ).ToList();
        }
    }
}
