using Enigma_Client_V2.View_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace Enigma_Client_V2.View.MainUserControls.AuthorizationUserControls
{
    /// <summary>
    /// Логика взаимодействия для ForgetPasswordUserControl.xaml
    /// </summary>
    public partial class ForgetPasswordUserControl : Window
    {
        public string secMsg = String.Empty;
        public ForgetPasswordUserControl()
        {
            InitializeComponent();
        }
        private void hiddenPasswordRepeat_passwordbox_PasswordChanged(object sender, RoutedEventArgs e)
            => Auth_Reg_Functions.PlaceholderVisibility(hiddenPasswordRepeat_passwordbox.Password, passwordRepeat_placeholder, showPasswordRepeatButton);
        private void showPasswordRepeatButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
            => Auth_Reg_Functions.HidePasswordAction(hiddenPasswordRepeat_passwordbox, visiblePasswordRepeat_textbox, showPasswordRepeatButton);
        private void showPasswordRepeatButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
            => Auth_Reg_Functions.ShowPasswordAction(hiddenPasswordRepeat_passwordbox, visiblePasswordRepeat_textbox, showPasswordRepeatButton);

        private void showPasswordRepeatButton_MouseLeave(object sender, MouseEventArgs e)
            => Auth_Reg_Functions.HidePasswordAction(hiddenPasswordRepeat_passwordbox, visiblePasswordRepeat_textbox, showPasswordRepeatButton);
        private void phone_maskedTextbox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
                e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        private void accept_button_Click(object sender, RoutedEventArgs e)
        {
            string phone = phone_maskedTextbox.Text;
            if (phone.Count(x => char.IsDigit(x)) < 2)
            {
                MessageBox.Show("Неккоректный номер телефона, исправьте и повторите попытку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            else if (phone.Count(x => char.IsDigit(x)) < 11)
            {
                MessageBox.Show("Неккоректный номер телефона, исправьте и повторите попытку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                phone = phone.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "");
                secMsg = SendMessage(phone);
            }
            code_grid.Visibility = Visibility.Visible;
            save_button.Visibility = Visibility.Visible;
            accept_button.Visibility = Visibility.Hidden;
        }

        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            if (code_textBox.Text == secMsg)
            {
                EnigmaMessageBox.Show("Вы успешно сменили пароль", "Сообщение", EnigmaMessageBox.MessageBoxButton.Ок, EnigmaMessageBox.MessageBoxButton.Отменить);
            }
            else
                EnigmaMessageBox.Show("Вы ввели неверный код, повторите попытку", "Сообщение", EnigmaMessageBox.MessageBoxButton.Ок, EnigmaMessageBox.MessageBoxButton.Отменить);

        }

        public static string SendMessage(string msg)
        {
            // Указываем IP-адрес и порт сервера, к которому будем подключаться.
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 12345;

            // Создаем объект TcpClient.
            TcpClient client = new TcpClient();

            try
            {
                // Подключаемся к серверу.
                client.Connect(ipAddress, port);
                Console.WriteLine("Connected to server.");

                // Получаем поток для записи данных на сервер.
                NetworkStream stream = client.GetStream();

                // Отправляем сообщение на сервер.
                string message = msg;
                byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent message: " + message);

                // Читаем ответ от сервера.
                data = new byte[1024];
                int bytesRead = stream.Read(data, 0, data.Length);
                message = System.Text.Encoding.ASCII.GetString(data, 0, bytesRead);
                Console.WriteLine("Received response: " + message);
                return message;
            }
            catch (Exception e)
            {
                return null;
                Console.WriteLine("Error: " + e.ToString());
            }
            finally
            {
                // Закрываем соединение с сервером.
                client.Close();
            }
        }
    }
}
