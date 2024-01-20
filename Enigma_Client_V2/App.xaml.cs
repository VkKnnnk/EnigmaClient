using Enigma_Client_V2.View_Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Enigma_Client_V2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.Exit += App_Exit;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            MessageBox.Show("Приложение закрыто. Параметры системы будут возвращены по умолчанию", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
            Functions_Dictionary.UnSetRestrictions();
        }
    }
}
