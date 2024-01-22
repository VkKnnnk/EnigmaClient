using Enigma_Client_V2.Model;
using Enigma_Client_V2.View.AuthorizationUserControls;
using Enigma_Client_V2.View_Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
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
using System.Windows.Threading;

namespace Enigma_Client_V2.View
{
    /// <summary>
    /// Логика взаимодействия для AuthenticationUserControl.xaml
    /// </summary>
    public partial class AuthenticationUserControl : UserControl, INotifyPropertyChanged
    {
        private DateTime currentAppTime;
        public DateTime CurrentAppTime
        {
            get { return currentAppTime; }
            set
            {
                currentAppTime = value;
                OnPropertyChanged("CurrentAppTime");
            }
        }
        private CultureInfo keyboardLayout;
        public CultureInfo KeyboardLayout
        {
            get { return keyboardLayout; }
            set
            {
                keyboardLayout = value;
                OnPropertyChanged("KeyboardLayout");
            }
        }
        private UserControl userControl;
        public UserControl UserControl
        {
            get { return userControl; }
            set
            {
                userControl = value;
                OnPropertyChanged("UserControl");
            }
        }
        public Computer currentPC { get; set; }
        public UserControl regUserControl = new RegUserControl();
        public UserControl authUserControl = new AuthUserControl();
        public AuthenticationUserControl()
        {
            InitializeComponent();
            videoBackground_mediaElement.Play();
            currentPC = ClientAppConfig.current_PC;
            CurrentAppTime = DateTime.Now;
            UserControl = authUserControl;
            this.DataContext = this;
            Functions_Dictionary.StartAppTimer().Tick += AuthenticationUserControl_Tick;
            KeyboardLayout = InputLanguageManager.Current.CurrentInputLanguage;
            InputLanguageManager.Current.InputLanguageChanged += Current_InputLanguageChanged;
        }

        private void Current_InputLanguageChanged(object sender, InputLanguageEventArgs e)
        {
            KeyboardLayout = InputLanguageManager.Current.CurrentInputLanguage;
        }

        private void AuthenticationUserControl_Tick(object sender, EventArgs e)
        {
            CurrentAppTime = DateTime.Now;
        }

        private async void regButton_label_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Functions_Dictionary.LoadingEvent();
            await Task.Run(() => UserControl = regUserControl);
            Functions_Dictionary.LoadingEvent(true);
            regAction_stackpanel.Visibility = Visibility.Hidden;
            goBack_button.Visibility = Visibility.Visible;
        }

        private async void goBack_button_Click(object sender, RoutedEventArgs e)
        {
            Functions_Dictionary.LoadingEvent();
            await Task.Run(() => UserControl = authUserControl);
            Functions_Dictionary.LoadingEvent(true);
            regAction_stackpanel.Visibility = Visibility.Visible;
            goBack_button.Visibility = Visibility.Hidden;
        }

        private void changeLayout_border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CultureInfo cultureInfo = InputLanguageManager.Current.CurrentInputLanguage;
            if (cultureInfo.Name == "ru-RU")
                InputLanguageManager.Current.CurrentInputLanguage = new("en-US");
            else
                InputLanguageManager.Current.CurrentInputLanguage = new("ru-RU");
            KeyboardLayout = InputLanguageManager.Current.CurrentInputLanguage;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void videoBackground_mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            MediaElement mediaElement = (MediaElement)sender;
            mediaElement.Stop();
            mediaElement.Play();
        }

        private void Label_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
