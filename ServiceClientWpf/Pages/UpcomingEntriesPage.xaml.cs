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
using System.Windows.Threading;

namespace ServiceClientWpf.Pages
{
    /// <summary>
    /// Логика взаимодействия для UpcomingEntriesPage.xaml
    /// </summary>
    public partial class UpcomingEntriesPage : Page
    {
        public UpcomingEntriesPage()
        {
            InitializeComponent();
            Refresh();
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 30);
            dispatcherTimer.Start();

        }

        private void Refresh() 
        {
            DateTime tomorrow = DateTime.Today.AddDays(1).AddHours(DateTime.Now.Hour);
            IEnumerable<ClientService> buffer = App.DB.ClientService.Where(
                x => x.StartTime >= DateTime.Now
                && x.StartTime <= tomorrow
                ).ToList().OrderBy(x => x.StartTime);

            if (buffer.Count() == 0) 
            {
                LVService.Visibility = Visibility.Hidden;
            }
            else
                LVService.Visibility = Visibility.Visible;

            LVService.ItemsSource = buffer.ToList();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Refresh();
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
