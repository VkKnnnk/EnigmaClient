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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Enigma_Client_V2.View.MainUserControls.WorkspaceUserControls
{
    /// <summary>
    /// Логика взаимодействия для AppWorkspaceUserControl.xaml
    /// </summary>
    public partial class AppWorkspaceUserControl : UserControl, INotifyPropertyChanged
    {
        private List<EProgramm> eProgramms;
        public List<EProgramm> EProgramms
        {
            get
            {
                return eProgramms;
            }
            set
            {
                eProgramms = value;
                OnPropertyChanged("EProgramms");
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public AppWorkspaceUserControl()
        {
            InitializeComponent();
            tiles.Source = new BitmapImage(new Uri(uriString: $@"{current_directory}\Resources\Images\AppDesign\tiles_ico.tif"));
            table.Source = new BitmapImage(new Uri(uriString: $@"{current_directory}\Resources\Images\AppDesign\table_ico.tif"));
            ChangeListboxImg = table;
            PlateVisibility = Visibility.Visible;
            TableVisibility = Visibility.Collapsed;
            EProgramms = AppSession.eProgramms;
            this.DataContext = this;
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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EProgramm eProgramm = new();
            if (tilesTariffs_listBox.SelectedItem is not null)
                eProgramm = (EProgramm)tilesTariffs_listBox.SelectedItem;
            else
                eProgramm = (EProgramm)tableTariffs_listBox.SelectedItem;
            System.Windows.Forms.DialogResult result = EnigmaMessageBox.Show($"Запустить игру {eProgramm.Name}?", "Подтверждение", EnigmaMessageBox.MessageBoxButton.Да, EnigmaMessageBox.MessageBoxButton.Нет);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                Workspace_Functions.RunProgramm(eProgramm);
            }
        }
    }
}
