using Enigma_Client_V2.Model;
using Enigma_Client_V2.View.MainUserControls.WorkspaceUserControls;
using Enigma_Client_V2.View_Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Enigma_Client_V2.View
{
    /// <summary>
    /// Логика взаимодействия для WorkspaceUserControl.xaml
    /// </summary>
    public partial class WorkspaceUserControl : UserControl, INotifyPropertyChanged
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


        private MainWorkspaceUserControl mainWorkspaceUserControl = new();
        private TariffsWorkspaceUserControl tariffsWorkspaceUserControl = new();
        private AppWorkspaceUserControl appWorkspaceUserControl = new();
        public event PropertyChangedEventHandler PropertyChanged;
        public WorkspaceUserControl()
        {
            InitializeComponent();
            AppSession.Context.Sessions.Load();
            AppSession.Context.Tariffs.Load();
            CurrentAppTime = DateTime.Now;
            KeyboardLayout = InputLanguageManager.Current.CurrentInputLanguage;
            Functions_Dictionary.StartAppTimer().Tick += WorkspaceUserControl_Tick; ;
            InputLanguageManager.Current.InputLanguageChanged += Current_InputLanguageChanged;
            if (AppSession.Context.Sessions.Where(x => x.Status == true).Count() > 0)
                AppSession.current_session = AppSession.Context.Sessions.Where(x => x.Status == true).FirstOrDefault(x => x.IdUser == AppSession.current_user.IdUser);
            if (AppSession.current_session is not null)
            {
                CurrentSession = AppSession.current_session;
                gamesButton_grid.IsEnabled = true;
            }
            else
            {
                gamesButton_grid.IsEnabled = false;
            }
            userControl = mainWorkspaceUserControl;
            this.DataContext = this;
        }

        private void WorkspaceUserControl_Tick(object sender, EventArgs e)
        {
            CurrentAppTime = DateTime.Now;
            if (gamesButton_grid.IsEnabled == false)
            {
                if (AppSession.current_session is not null)
                    if (AppSession.current_session.Status)
                    {
                        gamesButton_grid.IsEnabled = true;
                    }
            }
            if (AppSession.current_session is not null)
                if (AppSession.current_session.Status)
                    if (Workspace_Functions.CheckSession())
                        CurrentSession = AppSession.current_session;
                    else
                    {
                        gamesButton_grid.IsEnabled = false;
                        CurrentSession = null;
                    }
        }

        private void Current_InputLanguageChanged(object sender, InputLanguageEventArgs e)
        {
            KeyboardLayout = InputLanguageManager.Current.CurrentInputLanguage;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
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

        private async void exitButton_grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.DialogResult result = EnigmaMessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение", EnigmaMessageBox.MessageBoxButton.Подтвердить, EnigmaMessageBox.MessageBoxButton.Отменить);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                Functions_Dictionary.LoadingEvent();
                await Task.Run(() => Workspace_Functions.LogOut());
                Functions_Dictionary.LoadingEvent(true);
                (Application.Current.MainWindow as MainWindow).mainWindow_contentPresenter.Content = new AuthenticationUserControl();
            }
        }

        private async void buyTariffButton_grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Functions_Dictionary.LoadingEvent();
            await Task.Run(() => UserControl = tariffsWorkspaceUserControl);
            Functions_Dictionary.LoadingEvent(true);
            buyTariffButton_grid.IsEnabled = false;
            if (AppSession.current_session is not null)
                if (AppSession.current_session.Status)
                    gamesButton_grid.IsEnabled = true;
            desktopButton.IsEnabled = true;
        }

        private async void gamesButton_grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Functions_Dictionary.LoadingEvent();
            await Task.Run(() => UserControl = appWorkspaceUserControl);
            Functions_Dictionary.LoadingEvent(true);
            buyTariffButton_grid.IsEnabled = true;
            gamesButton_grid.IsEnabled = false;
            desktopButton.IsEnabled = true;
        }

        private async void desktopButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Functions_Dictionary.LoadingEvent();
            await Task.Run(() => UserControl = mainWorkspaceUserControl);
            Functions_Dictionary.LoadingEvent(true);
            buyTariffButton_grid.IsEnabled = true;
            if (AppSession.current_session is not null)
                if (AppSession.current_session.Status)
                    gamesButton_grid.IsEnabled = true;
            desktopButton.IsEnabled = false;
        }

        private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.DialogResult rez = EnigmaMessageBox.Show($"{AppSession.current_user.Name}, к вашему пакету может быть добавлен допольнительный 1 час, это будет стоить 50 р.", "Сообщение", EnigmaMessageBox.MessageBoxButton.Ок, EnigmaMessageBox.MessageBoxButton.Отменить);
            if (rez == System.Windows.Forms.DialogResult.Yes)
            {
                Workspace_Functions.AddTime();
                CurrentSession = AppSession.current_session;
            }

        }
    }
}
