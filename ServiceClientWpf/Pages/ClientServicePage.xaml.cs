﻿using System;
using System.Collections.Generic;
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

            if (TBDate.SelectedDate == null || TBDate.SelectedDate.Value < DateTime.Now)
            {
                errorMessage += "Введите корректную дату\n";
            }

            if (int.Parse(TBTimeHour.Text) < 7)
            {
                errorMessage += "Введите время\n";
            }

            if (string.IsNullOrWhiteSpace(errorMessage) == false)
            {
                MessageBox.Show(errorMessage);
                return;
            }

            var date = TBDate.DisplayDate;

            App.DB.ClientService.Add(new ClientService 
            { 
                Service = contextService, 
                Client = (Client)CBClient.SelectedItem,
                StartTime = new DateTime
                (
                    date.Year, 
                    date.Month, 
                    date.Day, 
                    int.Parse(TBTimeHour.Text.ToString()), 
                    int.Parse(TBTimeMinute.Text.ToString()),
                    0
                )
            });

            App.DB.SaveChanges();

            NavigationService.Navigate(new ServiceListPage());
        }

        private void TBTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;

            if (Regex.IsMatch(e.Text, @"[0-9]") == false)
            {
                e.Handled = true;
                return;
            }


            if (textBox == TBTimeMinute 
                && int.Parse($"{textBox.Text}{e.Text}") >= 60)
            {
                e.Handled = true;
            }

            if (textBox == TBTimeHour
                && int.Parse($"{textBox.Text}{e.Text}") > 16)
            {
                e.Handled = true;
            }
        }

        private void TBTimeMinute_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TBTimeMinute.Text == "")
            {
                TBTimeMinute.Text = "00";
            }
        }
    }
}