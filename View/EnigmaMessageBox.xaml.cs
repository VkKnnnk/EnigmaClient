using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Enigma_Client_V2.View
{
    /// <summary>
    /// Логика взаимодействия для EnigmaMessageBox.xaml
    /// </summary>
    public partial class EnigmaMessageBox : Window
    {
        public EnigmaMessageBox()
        {
            InitializeComponent();
        }

        private static EnigmaMessageBox enigmaMessage;
        static DialogResult result = System.Windows.Forms.DialogResult.No;
        public enum MessageBoxButton
        {
            Ок,
            Нет,
            Да,
            Отменить,
            Подтвердить
        }
        public static DialogResult Show(string message, string title, MessageBoxButton buttonOk, MessageBoxButton buttonNo)
        {
            enigmaMessage = new();
            enigmaMessage.msg_textblock.Text = message;
            enigmaMessage.ok_button.Content = enigmaMessage.GetMessageButton(buttonOk);
            enigmaMessage.cancel_button.Content = enigmaMessage.GetMessageButton(buttonNo);
            enigmaMessage.title_label.Content = title;
            enigmaMessage.ShowDialog();
            return result;

        }
        public string GetMessageButton(MessageBoxButton value)
        {
            return Enum.GetName(typeof(MessageBoxButton), value);
        }

        private void ok_button_Click(object sender, RoutedEventArgs e)
        {
            result = System.Windows.Forms.DialogResult.Yes;
            enigmaMessage.Close();
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            result = System.Windows.Forms.DialogResult.No;
            enigmaMessage.Close();
        }
    }
}
