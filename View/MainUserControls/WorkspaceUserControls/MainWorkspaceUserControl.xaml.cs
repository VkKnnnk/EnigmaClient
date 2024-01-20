using Enigma_Client_V2.Model;
using Enigma_Client_V2.View_Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Enigma_Client_V2.View.MainUserControls.WorkspaceUserControls
{
    public partial class MainWorkspaceUserControl : UserControl, INotifyPropertyChanged
    {
        private User wrkspcUser;
        public User WrkspcUser
        {
            get { return wrkspcUser; }
            set
            {
                wrkspcUser = value;
                OnPropertyChanged("WrkspcUser");
            }
        }
        private List<SessionTariff> sessionTariffs;
        public List<SessionTariff> SessionTariffs
        {
            get { return sessionTariffs; }
            set
            {
                sessionTariffs = value;
                OnPropertyChanged("SessionTariffs");
            }
        }
        private Model.Session currentSession;
        public Model.Session CurrentSession
        {
            get
            {
                return currentSession;
            }
            set
            {
                currentSession = value;
                OnPropertyChanged("CurrentSession");
            }
        }
        private int userCashRowSpan = 1;
        public int UserCashRowSpan
        {
            get
            {
                return userCashRowSpan;
            }
            set
            {
                userCashRowSpan = value;
                OnPropertyChanged("UserCashRowSpan");
            }
        }

        public MainWorkspaceUserControl()
        {
            InitializeComponent();
            AppSession.Context.Sessions.Load();
            AppSession.Context.Tariffs.Load();
            SessionTariffs = AppSession.appTariffs;
            Functions_Dictionary.StartAppTimer().Tick += MainWorkspaceUserControl_Tick;
            WrkspcUser = AppSession.current_user;
            if (AppSession.current_session is not null)
                CurrentSession = AppSession.current_session;
            this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void MainWorkspaceUserControl_Tick(object sender, EventArgs e)
        {
            WrkspcUser = AppSession.current_user;
            if (AppSession.current_session is not null)
            {
                CurrentSession = AppSession.current_session;
                infoSession_grid.Visibility = Visibility.Visible;
            }
            else
            {
                CurrentSession = null;
                infoSession_grid.Visibility = Visibility.Hidden;
            }

            if (Math.Round(DateTime.Now.TimeOfDay.TotalSeconds, 0) == new TimeSpan(23, 59, 59).TotalSeconds)
            {
                Functions_Dictionary.LoadLocalTariffs();
                SessionTariffs = AppSession.appTariffs;
            }
            if (Math.Round(DateTime.Now.TimeOfDay.TotalSeconds, 0) == TimeSpan.FromHours(23).TotalSeconds || Math.Round(DateTime.Now.TimeOfDay.TotalSeconds, 0) == TimeSpan.FromHours(7).TotalSeconds)
            {
                Functions_Dictionary.LoadLocalTariffs();
                SessionTariffs = AppSession.appTariffs;
            }
            if (ClientAppConfig.timesWhenUpdateTariffs.Count > 0)
            {
                List<double> timesUpdate = ClientAppConfig.timesWhenUpdateTariffs;
                foreach (double time in timesUpdate)
                {
                    if (Math.Round(DateTime.Now.TimeOfDay.TotalSeconds, 0) == time)
                    {
                        Functions_Dictionary.LoadLocalTariffs();
                        SessionTariffs = AppSession.appTariffs;
                    }
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void refresh_image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Animation_Functions.Refresh(refresh_image, true);
            Functions_Dictionary.LoadLocalTariffs();
        }

        private void cashRefresh_image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WrkspcUser = AppSession.current_user;
            Animation_Functions.Refresh(cashRefresh_image, true);
        }
        private void addMoney_button_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.DialogResult result = new();
            if (deposit_textbox.Text == String.Empty)
                result = EnigmaMessageBox.Show("Пожалуйста, введите сумму к пополнению и повторите попытку", "Сообщение", EnigmaMessageBox.MessageBoxButton.Ок, EnigmaMessageBox.MessageBoxButton.Отменить);
            else
            {
                result = EnigmaMessageBox.Show($"Вы уверены, что хотите пополнить баланс на {deposit_textbox.Text} руб.", "Сообщение", EnigmaMessageBox.MessageBoxButton.Да, EnigmaMessageBox.MessageBoxButton.Нет);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    Workspace_Functions.AddUserMoney(deposit_textbox.Text);
                    WrkspcUser = AppSession.current_user;
                }
            }
        }

        private void deposit_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (deposit_textbox.Text.Length > 0)
                depositPlaceholder_textBox.Visibility = Visibility.Collapsed;
            else
                depositPlaceholder_textBox.Visibility = Visibility.Visible;
        }

        private void deposit_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            // Получаем код нажатой клавиши в виде числа типа VirtualKey
            int keyCode = KeyInterop.VirtualKeyFromKey(e.Key);

            // Проверяем, является ли нажатая клавиша символом кроме цифры, спецсимвола и пробела
            if (e.Key != Key.Back && !char.IsDigit((char)keyCode) && !char.IsSymbol((char)keyCode) && !char.IsWhiteSpace((char)keyCode) && !(e.Key == Key.Space || char.IsPunctuation((char)keyCode)))
            {
                e.Handled = true;
            }
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Получаем код нажатой клавиши в виде числа типа VirtualKey
            int keyCode = KeyInterop.VirtualKeyFromKey(e.Key);

            // Проверяем, является ли нажатая клавиша символом кроме цифры, спецсимвола и пробела
            if (e.Key != Key.Back && !char.IsDigit((char)keyCode) && !char.IsSymbol((char)keyCode) && !char.IsWhiteSpace((char)keyCode) && !(e.Key == Key.Space || char.IsPunctuation((char)keyCode)))
            {
                e.Handled = true;
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Получаем код нажатой клавиши в виде числа типа VirtualKey
            int keyCode = KeyInterop.VirtualKeyFromKey(e.Key);

            // Проверяем, является ли нажатая клавиша символом кроме цифры, спецсимвола и пробела
            if (e.Key != Key.Back && !char.IsDigit((char)keyCode) && !char.IsSymbol((char)keyCode) && !char.IsWhiteSpace((char)keyCode) && !(e.Key == Key.Space || char.IsPunctuation((char)keyCode)))
            {
                e.Handled = true;
            }
        }

        private void TextBox_KeyDown_1(object sender, KeyEventArgs e)
        {
            // Получаем код нажатой клавиши в виде числа типа VirtualKey
            int keyCode = KeyInterop.VirtualKeyFromKey(e.Key);

            // Проверяем, является ли нажатая клавиша символом кроме цифры, спецсимвола и пробела
            if (e.Key != Key.Back && !char.IsDigit((char)keyCode) && !char.IsSymbol((char)keyCode) && !char.IsWhiteSpace((char)keyCode) && !(e.Key == Key.Space || char.IsPunctuation((char)keyCode)))
            {
                e.Handled = true;
            }
        }

        private void MaskedTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Получаем код нажатой клавиши в виде числа типа VirtualKey
            int keyCode = KeyInterop.VirtualKeyFromKey(e.Key);

            // Проверяем, является ли нажатая клавиша символом кроме цифры, спецсимвола и пробела
            if (e.Key != Key.Back && !char.IsDigit((char)keyCode) && !char.IsSymbol((char)keyCode) && !char.IsWhiteSpace((char)keyCode) && !(e.Key == Key.Space || char.IsPunctuation((char)keyCode)))
            {
                e.Handled = true;
            }
        }
        private void minMax_button_Click(object sender, RoutedEventArgs e)
        {
            if (UserCashRowSpan == 1)
            {
                UserCashRowSpan = 2;
                addMoney_button.Visibility = Visibility.Visible;
                userCashContainer_grid.RowDefinitions.Add(new RowDefinition());
                userCashContainer_grid.RowDefinitions.Add(new RowDefinition());
                userCashContainer_grid.RowDefinitions[0].Height = new GridLength(0.25, GridUnitType.Star);
                userCashContainer_grid.RowDefinitions[1].Height = new GridLength(0.25, GridUnitType.Star);
                userCashContainer_grid.RowDefinitions[3].Height = new GridLength(0.45, GridUnitType.Star);
                userCashContainer_grid.RowDefinitions[4].Height = new GridLength(0.35, GridUnitType.Star);
                inputCashValueContainer_grid.Visibility = Visibility.Visible;
                depositContainer_grid.Visibility = Visibility.Visible;
                minMax_button.Content = "↑";
            }
            else
            {
                UserCashRowSpan = 1;
                addMoney_button.Visibility = Visibility.Hidden;
                userCashContainer_grid.RowDefinitions.RemoveRange(3, 2);
                userCashContainer_grid.RowDefinitions[0].Height = new GridLength(0.65, GridUnitType.Star);
                userCashContainer_grid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                userCashContainer_grid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
                inputCashValueContainer_grid.Visibility = Visibility.Collapsed;
                depositContainer_grid.Visibility = Visibility.Collapsed;
                minMax_button.Content = "↓";
            }
        }
    }
}
