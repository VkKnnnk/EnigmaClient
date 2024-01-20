using Enigma_Client_V2.Model;
using Enigma_Client_V2.View_Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Enigma_Client_V2.View.AuthorizationUserControls
{
    /// <summary>
    /// Логика взаимодействия для RegUserControl.xaml
    /// </summary>
    public partial class RegUserControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Authdatum reg_authdata;

        public Authdatum Reg_authdata
        {
            get { return reg_authdata; }
            set
            {
                reg_authdata = value;
                OnPropertyChanged("Reg_authdata");
            }
        }
        public RegUserControl()
        {
            InitializeComponent();
            Reg_authdata = new();
            this.DataContext = this;
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void hiddenPassword_passwordbox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Auth_Reg_Functions.PlaceholderVisibility(hiddenPassword_passwordbox.Password, password_placeholder, showPasswordButton);
            passwordDifficult_progressBar.Value = Auth_Reg_Functions.CheckPasswordDifficult(hiddenPassword_passwordbox.Password);
            Auth_Reg_Functions.PasswordDifficultVisibility(hiddenPassword_passwordbox.Password, passwordDifficult_progressBar, passwordDifficultEXEPT_border);
            Auth_Reg_Functions.ChangeForegroundProgressBar(passwordDifficult_progressBar, passwordDifficultEXEPT_border);
        }

        private void hiddenPasswordRepeat_passwordbox_PasswordChanged(object sender, RoutedEventArgs e)
            => Auth_Reg_Functions.PlaceholderVisibility(hiddenPasswordRepeat_passwordbox.Password, passwordRepeat_placeholder, showPasswordRepeatButton);

        private void showPasswordButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
            => Auth_Reg_Functions.HidePasswordAction(hiddenPassword_passwordbox, visiblePassword_textbox, showPasswordButton);
        private void showPasswordButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
            => Auth_Reg_Functions.ShowPasswordAction(hiddenPassword_passwordbox, visiblePassword_textbox, showPasswordButton);

        private void showPasswordButton_MouseLeave(object sender, MouseEventArgs e)
            => Auth_Reg_Functions.HidePasswordAction(hiddenPassword_passwordbox, visiblePassword_textbox, showPasswordButton);

        private void showPasswordRepeatButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
            => Auth_Reg_Functions.HidePasswordAction(hiddenPasswordRepeat_passwordbox, visiblePasswordRepeat_textbox, showPasswordRepeatButton);

        private void showPasswordRepeatButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
            => Auth_Reg_Functions.ShowPasswordAction(hiddenPasswordRepeat_passwordbox, visiblePasswordRepeat_textbox, showPasswordRepeatButton);

        private void showPasswordRepeatButton_MouseLeave(object sender, MouseEventArgs e)
            => Auth_Reg_Functions.HidePasswordAction(hiddenPasswordRepeat_passwordbox, visiblePasswordRepeat_textbox, showPasswordRepeatButton);

        private async void Registration_button_Click(object sender, RoutedEventArgs e)
        {
            bool validate = Auth_Reg_Functions.ValidateRegistration(
                phone_maskedTextbox.Text, hiddenPassword_passwordbox.Password,
                hiddenPasswordRepeat_passwordbox.Password, phoneException_border,
                passwordException_border, passwordRepeatException_border,
                Message_label);
            if (validate)
            {
                Reg_authdata.Password = hiddenPassword_passwordbox.Password;
                Functions_Dictionary.LoadingEvent();
                Reg_authdata = await Task.Run(() => Auth_Reg_Functions.Registrate(Reg_authdata));
                Functions_Dictionary.LoadingEvent(true);
                hiddenPassword_passwordbox.Password = Reg_authdata.Password;
                hiddenPasswordRepeat_passwordbox.Password = Reg_authdata.Password;
                Message_label.Content = "Успешная регистрация";
                Animation_Functions.Highlight_animation(Message_label, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(4), true);
                Reg_authdata = new();
            }
        }

        private void hiddenPassword_passwordbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
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
    }
}
