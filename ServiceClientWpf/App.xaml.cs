using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ServiceClientWpf.Model;

namespace ServiceClientWpf
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceDataBaseEntities DB = new ServiceDataBaseEntities();
        public static bool AdminCode = false; 
    }
}
