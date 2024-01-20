using Enigma_Client_V2.Model;
using Enigma_Client_V2.View_Model;
using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Enigma_Client_V2.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Functions_Dictionary.SetCurrentPCId();
            if (AppSession.Context.Database.CanConnect())
            {
                Functions_Dictionary.LoadEProgramms();
                ClientAppConfig.current_window = this;
                Functions_Dictionary.SetRestrictions();
                if (!ClientAppConfig.demoStart)
                {
                    Functions_Dictionary.LoadLocalTariffs();
                    (Application.Current.MainWindow as MainWindow).mainWindow_contentPresenter.Content = new AuthenticationUserControl();
                }
                else
                    alert_grid.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Отсутствует подключение к базе данных", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Shutdown();
            }
        }
        private void toDefaultAuth_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).mainWindow_contentPresenter.Content = new AuthenticationUserControl();
        }
        private void showMessage_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(ClientAppConfig.lastError, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void unbanWinApi_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Параметры системы возвращены по умолчанию", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
            Functions_Dictionary.UnSetRestrictions();
        }
    }
}
