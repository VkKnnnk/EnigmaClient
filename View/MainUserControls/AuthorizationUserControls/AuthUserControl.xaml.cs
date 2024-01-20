using Enigma_Client_V2.Model;
using Enigma_Client_V2.View.MainUserControls.AuthorizationUserControls;
using Enigma_Client_V2.View_Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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

namespace Enigma_Client_V2.View.AuthorizationUserControls
{
    public partial class AuthUserControl : UserControl, INotifyPropertyChanged
    {
        private Authdatum auth_authdata;

        public event PropertyChangedEventHandler PropertyChanged;

        public Authdatum Auth_authdata
        {
            get { return auth_authdata; }
            set
            {
                auth_authdata = value;
                OnPropertyChanged("Auth_authdata");
            }
        }
        public AuthUserControl()
        {
            InitializeComponent();
            Auth_authdata = new();
            this.DataContext = this;
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        private void auth_showPasswordButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Auth_Reg_Functions.ShowPasswordAction(auth_hiddenPassword_passwordbox, auth_visiblePassword_textbox, auth_showPasswordButton);
        }

        private void auth_hiddenPassword_passwordbox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Auth_Reg_Functions.PlaceholderVisibility(auth_hiddenPassword_passwordbox.Password, auth_password_placeholder, auth_showPasswordButton);
        }

        private void auth_showPasswordButton_MouseLeave(object sender, MouseEventArgs e)
        {
            Auth_Reg_Functions.HidePasswordAction(auth_hiddenPassword_passwordbox, auth_visiblePassword_textbox, auth_showPasswordButton);
        }

        private void auth_showPasswordButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Auth_Reg_Functions.HidePasswordAction(auth_hiddenPassword_passwordbox, auth_visiblePassword_textbox, auth_showPasswordButton);
        }

        private void phone_maskedTextbox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
                e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        private async void LogIn_button_Click(object sender, RoutedEventArgs e)
        {
            Auth_authdata.Password = auth_hiddenPassword_passwordbox.Password;
            Functions_Dictionary.LoadingEvent();
            int auth_result = await Task.Run(() => Auth_Reg_Functions.LogIn(Auth_authdata));
            Functions_Dictionary.LoadingEvent(true);
            if (auth_result == 0)
            {
                Auth_authdata = new();
                auth_hiddenPassword_passwordbox.Password = String.Empty;
                (Application.Current.MainWindow as MainWindow).mainWindow_contentPresenter.Content = new WorkspaceUserControl();
            }
            else if (auth_result == 1 || auth_result == 2)
            {
                Animation_Functions.Highlight_animation(auth_phoneException_border, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(2), true);
                Animation_Functions.Highlight_animation(auth_passwordException_border, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(2), true);
                auth_message_label.Content = "Неверный логин/пароль";
                Animation_Functions.Highlight_animation(auth_message_label, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(4), true);
            }
        }

        private void Label_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.DialogResult rez = EnigmaMessageBox.Show("Вы уверены, что хотите сбросить пароль?", "Подтверждение", EnigmaMessageBox.MessageBoxButton.Да, EnigmaMessageBox.MessageBoxButton.Нет);
            if (rez == System.Windows.Forms.DialogResult.Yes)
            {
                ForgetPasswordUserControl forgetPasswordUserControl = new();
                forgetPasswordUserControl.ShowDialog();
            }
        }
    }
}
