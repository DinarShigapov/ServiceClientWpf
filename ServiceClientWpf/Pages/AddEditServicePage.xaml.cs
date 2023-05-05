using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using ServiceClientWpf.Model;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using ValidateService = System.ComponentModel.DataAnnotations.ValidationResult;

namespace ServiceClientWpf.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditServicePage.xaml
    /// </summary>
    public partial class AddEditServicePage : Page
    {

        private int _currentGroupList = 0;
        private int _maxPage = 4;
        private Service _contextService;
        private Service _contextServiceSave;
        private List<ServicePhoto> _servicePhotoList = new List<ServicePhoto>();
        private List<ServicePhoto> _deletedPhotoList = new List<ServicePhoto>();



        public AddEditServicePage(Service service)
        {
            InitializeComponent();
            _contextService = service.Clone();
            _contextServiceSave = service;
            if (_contextService.ID != 0)
            {
                TBIdClient.Visibility = Visibility.Visible;
                _contextService.DurationInSeconds /= 60;
                _contextService.Discount *= 100;
            }
            DataContext = _contextService;
            LVAddImage.ItemsSource = _servicePhotoList = App.DB.ServicePhoto.Where(x => x.ServiceID == service.ID).ToList();
            Update();
        }

        private void Update()
        {
            IEnumerable<ServicePhoto> photoList = _servicePhotoList.ToList();
            photoList = photoList.Skip(_maxPage * _currentGroupList).Take(_maxPage);
            LVAddImage.ItemsSource = photoList;
        }

        bool Validate(Service service)
        {
            var results = new List<ValidateService>();
            var context = new ValidationContext(service);
            if (!Validator.TryValidateObject(service, context, results, true))
            {
                string errorBuffer = "";
                foreach (var error in results)
                {
                    errorBuffer += $"{error}\n";
                }
                MessageBox.Show(errorBuffer, "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return true;
            }
            return false;
        }

        private void BAddImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                _contextService.MainImage = File.ReadAllBytes(dialog.FileName);
                DataContext = null;
                DataContext = _contextService;
            }
        }

        private void BSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validate(_contextService))
                return;

            string errorMessage = "";
            if (_contextService.DurationInSeconds > 240
                || _contextService.DurationInSeconds < 30)
            {
                errorMessage += "Время не входит в диапазон (30 - 240 мин.)\n";
            }
            if (_contextService.Cost > 10000
                || _contextService.Cost < 900)
            {
                errorMessage += "Цена не входит в диапазон (900 - 10000 руб.)\n";
            }
            if (string.IsNullOrWhiteSpace(errorMessage) == false)
            {
                MessageBox.Show(errorMessage);
                return;
            }
            if (string.IsNullOrWhiteSpace(errorMessage) == false)
            {
                MessageBox.Show(errorMessage);
                return;
            }

            _contextService.DurationInSeconds *= 60;
            _contextService.Discount /= 100;

            foreach (PropertyInfo property in typeof(Service).GetProperties().Where(p => p.CanWrite))
            {

                if (property.Name == "ClientService")
                {
                    break;
                }
                property.SetValue(_contextServiceSave, property.GetValue(_contextService, null), null);
            }

            if (_contextService.ID == 0)
            {
                App.DB.Service.Add(_contextService);
            }
            App.DB.SaveChanges();

            foreach (var item in _servicePhotoList)
            {
                if (item.ServiceID != 0)
                {
                    continue;
                }
                else
                {
                    App.DB.ServicePhoto.Add(new ServicePhoto { ServiceID = _contextService.ID, ServiceImage = item.ServiceImage});
                    App.DB.SaveChanges();
                }
            }

            if (_deletedPhotoList.Count > 0)
            {
                foreach (var item in _deletedPhotoList)
                {
                    App.DB.ServicePhoto.Remove(item);
                    App.DB.SaveChanges();
                }
            }
            NavigationService.Navigate(new ServiceListPage());
        }

        private void BBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ServiceListPage());
        }

        private void BSlideBack_Click(object sender, RoutedEventArgs e)
        {
            _currentGroupList--;
            if (_currentGroupList < 0)
                _currentGroupList = 0;
            Update();
        }

        private void BSlideNext_Click(object sender, RoutedEventArgs e)
        {
            _currentGroupList++;
            if (LVAddImage.Items.Count < 4)
                _currentGroupList--;
            Update();
        }

        private void MIAddImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                _servicePhotoList.Add(new ServicePhoto { ServiceImage = File.ReadAllBytes(dialog.FileName) });
                Update();
            }
        }

        private void MIDeleteImage_Click(object sender, RoutedEventArgs e)
        {
            var selectImage = LVAddImage.SelectedItem as ServicePhoto;

            if (selectImage == null)
                return;

            if (selectImage.ServiceID != 0)
            {
                _deletedPhotoList.Add(selectImage);
                _servicePhotoList.Remove(selectImage);
                LVAddImage.ItemsSource = _servicePhotoList;
                _currentGroupList = 0;
                _maxPage = 4;
                Update();
            }
            else
            {
                _servicePhotoList.Remove(selectImage);
                LVAddImage.ItemsSource = _servicePhotoList;
                Update();
            }
        }

        private void TBCost_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"[0-9.]") == false)
                e.Handled = true;
            if (TBCost.Text.Contains(".") && e.Text == ".")
                e.Handled = true;
            if (TBCost.Text.Length == 0
                && e.Text == ".")
                e.Handled = true;
        }

        private void TBTitle_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"[A-zА-я]") == false)
            {
                e.Handled = true;
            }
        }

        private void TBTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"[0-9]") == false)
            {
                e.Handled = true;
                return;
            }
        }

        private void TBTime_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void TBCost_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void TBCost_LostFocus(object sender, RoutedEventArgs e)
        {
            var text = TBCost.Text;
            if (text[text.Length - 1] == '.')
                text = text.Remove(text.Length - 1);
        }
    }
}
