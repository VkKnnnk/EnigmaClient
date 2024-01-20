using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;
using Enigma_Client_V2.Model;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows.Media.Animation;
using Enigma_Client_V2.View;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Enigma_Client_V2.View_Model
{
    public class Auth_Reg_Functions
    {
        private static string current_directory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        private static int previous_password_difficult = new();

        private static void ChangeImage(Image button, bool type_img)
        {
            if (type_img)
                button.Source = new BitmapImage(new Uri(uriString: $@"{current_directory}\Resources\Images\AppDesign\opened_eye_ico.tif"));
            else
                button.Source = new BitmapImage(new Uri(uriString: $@"{current_directory}\Resources\Images\AppDesign\closed_eye_ico.tif"));
        }
        public static int CheckPasswordDifficult(string password)
        {
            int password_difficult = 0;
            if (password.Length < 10)
                password_difficult += password.Length;
            else
                password_difficult += 10;
            if (password.Length > 6)
            {
                if (Regex.IsMatch(password, "[A-ZА-Я]")) //в строке есть заглавные буквы
                    password_difficult += 20;
                if (Regex.IsMatch(password, @"\d+")) //в строке есть цифра
                    password_difficult += 5;
                if (Regex.IsMatch(password, @"[A-Za-zА-Яа-я]+")) //в строке есть буква
                    password_difficult += 5;
                if (Regex.IsMatch(password, @"\d[A-Za-zА-Яа-я]+|[A-Za-zА-Яа-я]\d+")) //в строке есть сочетание буква-цифра
                    password_difficult += 10;
                if (Regex.IsMatch(password, @"[A-ZА-Я][a-zа-я]|[a-zа-я][A-ZА-Я]")) //в строке есть сочетание буква-заглавная буква
                    password_difficult += 25;
                if (Regex.IsMatch(password, @"(.)\1+")) //если символ повторяется > 1 раза
                    password_difficult -= 10;
                if (Regex.IsMatch(password, @"[^a-zA-Zа-яА-я\d]+")) //в строке есть спец-символ
                    password_difficult += 25;
            }
            return password_difficult;
        }
        public static void ChangeForegroundProgressBar(ProgressBar progressBar, Border exception_border)
        {
            Color white_color = Color.FromRgb(251, 238, 252); //#fbeefc white
            Color darkPurple_color = Color.FromRgb(45, 17, 117); //#2d1175 dark purple
            Color light_purple = Color.FromRgb(179, 102, 255); //#b366ff

            LinearGradientBrush clip_foreground = new LinearGradientBrush();

            GradientStop dynamic_gradientStop = new() { Color = darkPurple_color };
            GradientStop fixed_gradientStop = new() { Color = light_purple };

            GradientStopCollection gradientStops = new() { dynamic_gradientStop, fixed_gradientStop };
            clip_foreground.GradientStops = gradientStops;
            int difficult = int.Parse(progressBar.Value.ToString());
            if (difficult <= 30)
            {
                double[] offset = new double[2] { 0.5, 1 };
                gradientStops[0].Offset = offset[1];
                if (difficult != previous_password_difficult)
                {
                    if (difficult < previous_password_difficult)
                        Array.Reverse(offset);
                    Animation_Functions.ProgressBarFill(clip_foreground.GradientStops[0], TimeSpan.FromSeconds(0.5), offset, true);
                }

            }
            else if (difficult >= 30 && difficult <= 80)
            {
                double[] offset = new double[2] { 0.5, 0.85 };
                gradientStops[0].Offset = offset[1];
                if (difficult != previous_password_difficult)
                {
                    if (difficult < previous_password_difficult)
                        Array.Reverse(offset);
                    Animation_Functions.ProgressBarFill(clip_foreground.GradientStops[0], TimeSpan.FromSeconds(0.5), offset, true);
                }
            }
            else
            {
                double[] offset = new double[2] { 0.1, 0.5 };
                gradientStops[0].Offset = offset[1];
                if (difficult != previous_password_difficult)
                {
                    if (difficult < previous_password_difficult)
                        Array.Reverse(offset);
                    Animation_Functions.ProgressBarFill(clip_foreground.GradientStops[0], TimeSpan.FromSeconds(0.5), offset, true);
                    GradientStop gradientStop = GradientBorderColorSet(white_color, darkPurple_color, exception_border);
                    Animation_Functions.Appearance_animation(exception_border, TimeSpan.FromSeconds(0.5), true);
                    Animation_Functions.Appearance_animation(exception_border, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(0.5), true);
                    Animation_Functions.Appearance_animation(exception_border, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(1), true);
                    Animation_Functions.RunningColor_animation(gradientStop, TimeSpan.FromSeconds(1.5), true);
                }
            }
            clip_foreground.GradientStops = gradientStops;
            progressBar.Foreground = clip_foreground;
            previous_password_difficult = difficult;
        }
        public static bool ValidateRegistration(
            string phone, string password,
            string passwordRepeat, Border phone_border,
            Border password_border, Border passwordRepeat_border,
            Label message_label)
        {
            if (phone.Count(x => char.IsDigit(x)) < 2)
            {
                Animation_Functions.Highlight_animation(phone_border, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(2), true);
                Animation_Functions.Highlight_animation(message_label, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(5), true);
                message_label.Content = "Введите номер телефона";
                return false;
            }
            else if (phone.Count(x => char.IsDigit(x)) < 11)
            {
                Animation_Functions.Highlight_animation(phone_border, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(2), true);
                Animation_Functions.Highlight_animation(message_label, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(5), true);
                message_label.Content = "Недопустимый формат номера";
                return false;
            }
            else if (CheckUniquenessPhone(phone) == false)
            {
                Animation_Functions.Highlight_animation(phone_border, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(2), true);
                Animation_Functions.Highlight_animation(message_label, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(5), true);
                message_label.Content = "Телефон уже зарегестрирован";
                return false;
            }
            else if (password == String.Empty)
            {
                Animation_Functions.Highlight_animation(password_border, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(2), true);
                Animation_Functions.Highlight_animation(message_label, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(5), true);
                message_label.Content = "Введите пароль";
                return false;
            }
            else if (CheckPasswordDifficult(password) <= 30)
            {
                Animation_Functions.Highlight_animation(password_border, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(2), true);
                Animation_Functions.Highlight_animation(message_label, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(5), true);
                message_label.Content = "Слабый пароль";
                return false;
            }
            else if (passwordRepeat == String.Empty)
            {
                Animation_Functions.Highlight_animation(passwordRepeat_border, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(2), true);
                Animation_Functions.Highlight_animation(message_label, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(5), true);
                message_label.Content = "Повторите пароль";
                return false;
            }
            else if (passwordRepeat != password)
            {
                Animation_Functions.Highlight_animation(passwordRepeat_border, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(2), true);
                Animation_Functions.Highlight_animation(password_border, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(2), true);
                Animation_Functions.Highlight_animation(message_label, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(5), true);
                message_label.Content = "Пароли не совпадают";
                return false;
            }
            return true;
        }
        public static void ShowPasswordAction(PasswordBox hidden_password, TextBox visible_pass, Image showpass_button)
        {
            ChangeImage(showpass_button, true);
            hidden_password.Visibility = Visibility.Hidden;
            visible_pass.Visibility = Visibility.Visible;
            visible_pass.Text = hidden_password.Password;
        }
        public static void HidePasswordAction(PasswordBox hidden_password, TextBox visible_pass, Image showpass_button)
        {
            ChangeImage(showpass_button, false);
            visible_pass.Visibility = Visibility.Hidden;
            hidden_password.Visibility = Visibility.Visible;
            hidden_password.Focus();
        }
        public static void PlaceholderVisibility(string hidden_password, TextBox placeholder, Image showpass_button)
        {
            if (hidden_password != String.Empty)
            {
                placeholder.Visibility = Visibility.Hidden;
                showpass_button.Visibility = Visibility.Visible;
            }
            else
            {
                placeholder.Visibility = Visibility.Visible;
                showpass_button.Visibility = Visibility.Hidden;
            }
        }
        public static void PasswordDifficultVisibility(string hidden_password, ProgressBar progressBar, Border exception_border)
        {
            if (hidden_password != String.Empty)
            {
                progressBar.Visibility = Visibility.Visible;
                exception_border.Visibility = Visibility.Visible;
            }
            else
            {
                exception_border.Visibility = Visibility.Hidden;
                progressBar.Visibility = Visibility.Hidden;
            }
        }
        public static bool CheckUniquenessPhone(string phone)
        {
            try
            {
                if (AppSession.Context.Authdata.Select(x => x.Phone).Contains(phone))
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки уникальности номера телефона/ошибка подключения к базе данных\nКод ошибки: {ex}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return true;
        }
        public static Authdatum Registrate(Authdatum reg_authdata)
        {
            reg_authdata.Password = Hash(reg_authdata.Password);
            try
            {
                AppSession.Context.Authdata.Add(reg_authdata);
                AppSession.Context.SaveChanges();
                List<Authdatum> authdata_list = AppSession.Context.Authdata.ToList();
                Authdatum user_authdata = authdata_list.FirstOrDefault(x => x.Phone == reg_authdata.Phone);
                User user = new() { IdAuth = user_authdata.IdAuth, Name = "Enigma CLient", AuthStatus = false };
                AppSession.Context.Users.Add(user);
                AppSession.Context.SaveChanges();
                return new Authdatum();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка: {e.Message}\nКод ошибки: {e.HResult}", "Ошибка регистрации/подключения к базе данных", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return new Authdatum();
        }
        private static string Hash(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            return Convert.ToBase64String(hashBytes);
        }
        private static bool VerifyPassword(string entered_password, string db_password)
        {
            byte[] hashBytes = Convert.FromBase64String(db_password);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(entered_password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }
        public static int LogIn(Authdatum auth_authdata)
        {
            try
            {
                if (AppSession.Context.Authdata.Select(x => x.Phone).Contains(auth_authdata.Phone))
                {
                    Authdatum db_authdata = AppSession.Context.Authdata.Where(x => x.Phone == auth_authdata.Phone).FirstOrDefault();
                    string currentUser_password = db_authdata.Password;
                    if (VerifyPassword(auth_authdata.Password, currentUser_password))
                    {
                        AppSession.Context.Users.Load();
                        AppSession.Context.Users.FirstOrDefault(x => x.IdAuth == db_authdata.IdAuth).AuthStatus = true;
                        AppSession.Context.SaveChanges();
                        AppSession.current_user = AppSession.Context.Users.FirstOrDefault(x => x.IdAuth == db_authdata.IdAuth);
                        if (AppSession.Context.Users.FirstOrDefault(x => x.IdAuth == db_authdata.IdAuth).PersonalDiscount != null)
                            ClientAppConfig.personalDiscount = (int)AppSession.Context.Users.FirstOrDefault(x => x.IdAuth == AppSession.Context.Authdata.FirstOrDefault(x => x.Phone == auth_authdata.Phone).IdAuth).PersonalDiscount;
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    return 2;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка: {e.Message}\nКод ошибки: {e.HResult}",
                    "Ошибка авторизации/подключения к базе данных",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return 3;
            }
        }
        public static GradientStop GradientBorderColorSet(Color left_color, Color right_color, Border border)
        {
            LinearGradientBrush border_gradient = new LinearGradientBrush();
            GradientStop gradientStopP = new GradientStop() { Color = left_color, Offset = 0 };
            GradientStop gradientStopW = new GradientStop() { Color = right_color, Offset = 0 };
            border_gradient.GradientStops.Add(gradientStopP);
            border_gradient.GradientStops.Add(gradientStopW);
            border.BorderBrush = border_gradient;
            return gradientStopW;
        }
    }
}
