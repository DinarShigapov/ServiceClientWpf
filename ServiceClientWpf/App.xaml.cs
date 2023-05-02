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
        public static ServiceDataBaseEntities2 DB = new ServiceDataBaseEntities2();
        public static bool AdminCode = false;

        public App() : base()
        {
            QuickConverter.EquationTokenizer.AddNamespace(typeof(object));
            QuickConverter.EquationTokenizer.AddNamespace(typeof(System.Windows.Visibility));
        }
    }
}
