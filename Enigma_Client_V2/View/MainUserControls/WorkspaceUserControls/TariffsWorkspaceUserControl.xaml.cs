using Enigma_Client_V2.Model;
using Enigma_Client_V2.View_Model;
using Microsoft.EntityFrameworkCore;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Enigma_Client_V2.View.MainUserControls.WorkspaceUserControls
{
    /// <summary>
    /// Логика взаимодействия для TariffsWorkspaceUserControl.xaml
    /// </summary>
    public partial class TariffsWorkspaceUserControl : UserControl, INotifyPropertyChanged
    {
        private User t_user;

        public event PropertyChangedEventHandler PropertyChanged;

        public User T_user
        {
            get { return t_user; }
            set
            {
                t_user = value;
                OnPropertyChanged("T_user");
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
        private Visibility plateVisibility;
        public Visibility PlateVisibility
        {
            get { return plateVisibility; }
            set
            {
                plateVisibility = value;
                OnPropertyChanged("PlateVisibility");
            }
        }
        private Visibility tableVisibility;
        public Visibility TableVisibility
        {
            get { return tableVisibility; }
            set
            {
                tableVisibility = value;
                OnPropertyChanged("TableVisibility");
            }
        }
        private Image changeListboxImg;
        public Image ChangeListboxImg
        {
            get { return changeListboxImg; }
            set
            {
                changeListboxImg = value;
                OnPropertyChanged("ChangeListboxImg");
            }
        }
        private Image tiles = new();
        private Image table = new();
        private static string current_directory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        public TariffsWorkspaceUserControl()
        {
            InitializeComponent();
            tiles.Source = new BitmapImage(new Uri(uriString: $@"{current_directory}\Resources\Images\AppDesign\tiles_ico.tif"));
            table.Source = new BitmapImage(new Uri(uriString: $@"{current_directory}\Resources\Images\AppDesign\table_ico.tif"));
            ChangeListboxImg = table;
            PlateVisibility = Visibility.Visible;
            TableVisibility = Visibility.Collapsed;
            SessionTariffs = AppSession.appTariffs;
            T_user = AppSession.current_user;
            this.DataContext = this;
            Functions_Dictionary.StartAppTimer().Tick += TariffsWorkspaceUserControl_Tick;
        }

        private void TariffsWorkspaceUserControl_Tick(object sender, EventArgs e)
        {
            T_user = AppSession.current_user;
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
        private void cashRefresh_image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            T_user = AppSession.current_user;
            Animation_Functions.Refresh(cashRefresh_image, true);
        }

        private void changeListboxButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (PlateVisibility == Visibility.Visible)
            {
                PlateVisibility = Visibility.Collapsed;
                TableVisibility = Visibility.Visible;
                ChangeListboxImg = tiles;
            }
            else
            {
                PlateVisibility = Visibility.Visible;
                TableVisibility = Visibility.Collapsed;
                ChangeListboxImg = table;
            }
        }

        private void refreshButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Animation_Functions.Refresh(refreshButton, true);
            Functions_Dictionary.LoadLocalTariffs();
        }

        private void buyTariff_button_Click(object sender, RoutedEventArgs e)
        {
            SessionTariff selectedTariff = new();
            if (tilesTariffs_listBox.SelectedItem is not null)
                selectedTariff = (SessionTariff)tilesTariffs_listBox.SelectedItem;
            else
                selectedTariff = (SessionTariff)tableTariffs_listBox.SelectedItem;
            System.Windows.Forms.DialogResult result = EnigmaMessageBox.Show($"Вы уверены, что хотите купить тариф {selectedTariff.Name} за {selectedTariff.Sum_price} руб.?", "Подтверждение", EnigmaMessageBox.MessageBoxButton.Да, EnigmaMessageBox.MessageBoxButton.Нет);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                Workspace_Functions.BuyTariff(selectedTariff);
                T_user = AppSession.current_user;
            }
        }
    }
}
