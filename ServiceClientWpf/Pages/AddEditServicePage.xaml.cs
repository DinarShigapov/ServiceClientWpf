using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Reflection;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ServiceClientWpf.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditServicePage.xaml
    /// </summary>
    public partial class AddEditServicePage : Page
    {
        Service contextService;
        Service contextServiceSave;
        List<ServicePhoto> servicePhotoList = new List<ServicePhoto>();


        public AddEditServicePage(Service service)
        {
            InitializeComponent();
            contextService = service.Clone();
            contextServiceSave = service;
            DataContext = contextService;
            LVAddImage.ItemsSource = servicePhotoList = App.DB.ServicePhoto.Where(x => x.ServiceID == service.ID).ToList();
            Update();
        }

        private void BAddImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                contextService.MainImage = File.ReadAllBytes(dialog.FileName);
                DataContext = null;
                DataContext = contextService;
            }
        }

        private void BSave_Click(object sender, RoutedEventArgs e)
        {
            
            TBTitle.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            string errorMessage = "";
            if (string.IsNullOrWhiteSpace(contextService.Title) == true)
            {
                errorMessage += "Введите название\n";
            }
            if (contextService.Cost < 0)
            {
                errorMessage += "Введите корректную цену\n";
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



            foreach (PropertyInfo property in typeof(Service).GetProperties().Where(p => p.CanWrite))
            {

                if (property.Name == "ClientService")
                {
                    break;
                }
                property.SetValue(contextServiceSave, property.GetValue(contextService, null), null);
            }



            if (contextService.ID == 0)
            {
                App.DB.Service.Add(contextService);
            }
            App.DB.SaveChanges();

            foreach (var item in servicePhotoList)
            {
                if (item.ServiceID != 0)
                {
                    continue;
                }
                else
                {
                    App.DB.ServicePhoto.Add(new ServicePhoto { ServiceID = contextService.ID, ServiceImage = item.ServiceImage});
                    App.DB.SaveChanges();
                }
            }



            NavigationService.Navigate(new ServiceListPage());

        }

        private void TBCost_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"[0-9]") == false)
            {
                e.Handled = true;
            }
        }

        private void TBTitle_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"[A-zА-я]") == false)
            {
                e.Handled = true;
            }
        }

        private void BBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ServiceListPage());
        }


        int CurrentGroupList = 0;
        int MaxPage = 4;

        private void BSlideBack_Click(object sender, RoutedEventArgs e)
        {
            CurrentGroupList--;
            if (CurrentGroupList < 0)
                CurrentGroupList = 0;
            Update();
        }

        private void BSlideNext_Click(object sender, RoutedEventArgs e)
        {
            CurrentGroupList++;
            if (LVAddImage.Items.Count < 4)
                CurrentGroupList--;
            Update();
        }

        private void Update()
        {
            IEnumerable<ServicePhoto> photoList = servicePhotoList.ToList();
            photoList = photoList.Skip(MaxPage * CurrentGroupList).Take(MaxPage);
            LVAddImage.ItemsSource = photoList;
        }

        private void MIAddImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                servicePhotoList.Add(new ServicePhoto { ServiceImage = File.ReadAllBytes(dialog.FileName) });
                Update();
            }
        }

        private void MIDelImage_Click(object sender, RoutedEventArgs e)
        {
            var selectImage = LVAddImage.SelectedItem as ServicePhoto;

            if (selectImage == null)
                return;

            if (selectImage.ServiceID != 0)
            {
                App.DB.ServicePhoto.Remove(selectImage);
                servicePhotoList.Remove(selectImage);
                LVAddImage.ItemsSource = servicePhotoList;
                CurrentGroupList = 0;
                MaxPage = 4;
                Update();
            }
            else
            {
                servicePhotoList.Remove(selectImage);
                LVAddImage.ItemsSource = servicePhotoList;
                Update();
            }
        }
    }
}
