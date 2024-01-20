using Enigma_Client_V2.Model;
using Enigma_Client_V2.View;
using Enigma_Client_V2.View.AuthorizationUserControls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Enigma_Client_V2.View_Model
{
    public class Functions_Dictionary
    {
        private static TimeSpan nearNightStart = TimeSpan.FromHours(23);
        private static TimeSpan nearNightEnd = new TimeSpan(23, 59, 59);
        private static TimeSpan nightStart = new TimeSpan(0, 0, 0);
        private static TimeSpan nightEnd = new TimeSpan(6, 59, 59);
        private static TimeSpan currentTime = DateTime.Now.TimeOfDay;
        private static Ellipse ellipse { get; set; }
        private static Grid appGrid = (Application.Current.MainWindow as MainWindow).loadFigure_grid;

        //Устанавливает параметры для фигуры загрузки
        private static void SetParamLoadingFigure()
        {
            Color white_color = Color.FromRgb(251, 238, 252); //#fbeefc white
            Color darkPurple_color = Color.FromRgb(45, 17, 117); //#2d1175 dark purple

            LinearGradientBrush linearGradientBrush = new();
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = white_color });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = darkPurple_color, Offset = 0.95 });

            Ellipse tempEllipse = new();
            tempEllipse.Width = 50;
            tempEllipse.Height = 50;
            tempEllipse.VerticalAlignment = VerticalAlignment.Center;
            tempEllipse.HorizontalAlignment = HorizontalAlignment.Center;
            tempEllipse.Stroke = linearGradientBrush;
            tempEllipse.StrokeThickness = 10;
            tempEllipse.Opacity = 1;
            tempEllipse.LayoutTransform = new RotateTransform();
            ellipse = tempEllipse;
        }
        //Отображает фигуру загрузки и блокирует основное окно приложения
        public static void LoadingEvent()
        {
            SetParamLoadingFigure();
            appGrid.Children.Add(ellipse);
            Application.Current.MainWindow.IsEnabled = false;
            appGrid.Opacity = 0.98;
            Animation_Functions.LoadingAnimation(ellipse, true);
        }
        //Скрывает фигуру загрузки, останавливает анимацию и разблокирует основное окно приложения
        public static void LoadingEvent(bool stop)
        {
            Animation_Functions.LoadingAnimation(ellipse, true, true);
            appGrid.Children.Remove(ellipse);
            Application.Current.MainWindow.IsEnabled = true;
            appGrid.Opacity = 1;
        }
        //Устанавливает id текущего компьютера
        public static void SetCurrentPCId()
        {
            string idPC = Regex.Replace(Dns.GetHostName(), "[^0-9.]", "");

            try
            {
                int idComputer = int.Parse(idPC);
                AppSession.Context.Computers.Load();
                AppSession.Context.TypeTariffs.Load();
                ClientAppConfig.current_PC = AppSession.Context.Computers.FirstOrDefault(x => x.IdComp == idComputer);
            }
            catch (Exception ex)
            {
                ClientAppConfig.demoStart = true;
                ClientAppConfig.lastError = $"Возможно, неверно задано имя компьютера или нет подключения к БД, пожалуйста, обратитесь с системному администратору для решения проблемы\nКод ошибки:{ ex.HResult}\n{ ex.Message}";
                MessageBox.Show(ClientAppConfig.lastError, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //Загружает локальные тарифы
        public static async void LoadLocalTariffs()
        {
            currentTime = DateTime.Now.TimeOfDay;
            List<double> timesUpdate = new();

            AppSession.Context.TypeTariffs.Load();
            AppSession.Context.Tariffs.Load();

            List<SessionTariff> appTariffs = new();
            List<Tariff> dbTariffs = AppSession.Context.Tariffs.Where(x => x.IdTypeTariffs == AppSession.Context.Computers.FirstOrDefault(x => x.IdComp == ClientAppConfig.current_PC.IdComp).IdTypeTariffs).ToList();

            foreach (Tariff tariff in dbTariffs)
            {
                if (tariff.AppearanceHour != null)
                {
                    DateTime appearanceDate = DateTime.Now.Date + TimeSpan.FromHours((double)tariff.AppearanceHour);
                    DateTime dissapDate = appearanceDate.AddHours((double)tariff.AppearanceDuration);

                    timesUpdate.Add(appearanceDate.TimeOfDay.TotalSeconds);
                    timesUpdate.Add(dissapDate.TimeOfDay.TotalSeconds);

                    if (DateTime.Now <= dissapDate)
                    {
                        if (DateTime.Now >= appearanceDate)
                        {
                            appTariffs.Add(new SessionTariff()
                            {
                                IdTariff = tariff.IdTariff,
                                Duration = tariff.Duration,
                                Name = tariff.Name,
                                IdTypeTariffs = tariff.IdTypeTariffs,
                                IdTypeTariffsNavigation = tariff.IdTypeTariffsNavigation,
                                Sum_price = tariff.IdTypeTariffsNavigation.PriceHour,
                                FixPrice = tariff.FixPrice,
                                AppearanceDuration = tariff.AppearanceDuration,
                                AppearanceHour = tariff.AppearanceHour
                            });
                        }
                    }
                }
                else
                {
                    appTariffs.Add(new SessionTariff()
                    {
                        IdTariff = tariff.IdTariff,
                        Duration = tariff.Duration,
                        Name = tariff.Name,
                        IdTypeTariffs = tariff.IdTypeTariffs,
                        IdTypeTariffsNavigation = tariff.IdTypeTariffsNavigation,
                        Sum_price = tariff.IdTypeTariffsNavigation.PriceHour,
                        FixPrice = tariff.FixPrice
                    });
                }
            }

            AppSession.appTariffs = appTariffs;
            ClientAppConfig.timesWhenUpdateTariffs = timesUpdate;

            LoadingEvent();
            await Task.Run(() => CalculatePriceTariff());
            LoadingEvent(true);
        }
        //Рассчитывает цену на тарифы
        private static void CalculatePriceTariff()
        {
            AppSession.Context.TariffAllowances.Load();
            float discountForHour = AppSession.Context.TariffAllowances.FirstOrDefault(x => x.IdAllowance == 4).AllowancePercent;
            float discount = 0;

            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                discount += AppSession.Context.TariffAllowances.FirstOrDefault(x => x.IdAllowance == 1).AllowancePercent;
            }
            else if ((currentTime >= nearNightStart && currentTime < nearNightEnd) || (currentTime >= nightStart && currentTime < nightEnd))
            {
                discount += AppSession.Context.TariffAllowances.FirstOrDefault(x => x.IdAllowance == 3).AllowancePercent;
            }
            else if (ClientAppConfig.personalDiscount != 0)
            {
                discount += ClientAppConfig.personalDiscount;
            }

            foreach (SessionTariff appTariff in AppSession.appTariffs)
            {
                if (appTariff.FixPrice == null)
                {
                    if (discount < 0)
                    {
                        if (appTariff.Duration > 1)
                        {
                            float sum = appTariff.Sum_price * appTariff.Duration;
                            appTariff.Sum_price = (float)Math.Round(sum - sum * discountForHour / 100 + sum * discount / 100 * -1, 0);
                        }
                        else
                        {
                            float sum = appTariff.Sum_price;
                            appTariff.Sum_price = (float)Math.Round(sum + sum * discount / 100 * -1, 0);
                        }
                    }
                    else
                    {
                        if (appTariff.Duration > 1)
                        {
                            float sum = appTariff.Sum_price * appTariff.Duration;
                            appTariff.Sum_price = (float)Math.Round(sum - sum * discountForHour / 100 - sum * discount / 100, 0);
                        }
                        else
                        {
                            float sum = appTariff.Sum_price;
                            appTariff.Sum_price = (float)Math.Round(sum - sum * discount / 100, 0);
                        }
                    }
                }
                else
                {
                    appTariff.Sum_price = (float)appTariff.FixPrice;
                }
            }
        }
        //Запускает таймер приложения
        public static DispatcherTimer StartAppTimer()
        {
            DispatcherTimer appTimer = new DispatcherTimer();
            appTimer.Interval = TimeSpan.FromSeconds(1);
            appTimer.Start();
            return appTimer;
        }

        //Устанавливает ограничения на окно приложения с помощью WinApi
        public static void SetRestrictions()
        {
            try
            {
                WinApi_Functions.TaskbarManipulations.SetHookToWindow();
                WinApi_Functions.TaskbarManipulations.HideWindowFromTaskbar();
                WinApi_Functions.TaskbarManipulations.HideWindowFromAltTab();
                WinApi_Functions.KeyboardManipulations.HookKeyboard();
                WinApi_Functions.TaskbarManipulations.HideTaskbar();
                WinApi_Functions.OtherManipulations.DisableSystemParam();
            }
            catch (Exception ex)
            {
                ClientAppConfig.demoStart = true;
                ClientAppConfig.lastError = $"Возможно, программа запущена не от имени Администратора\nКод ошибки:{ex.HResult}\n{ex.Message}";
                MessageBox.Show(ClientAppConfig.lastError, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public static void UnSetRestrictions()
        {
            try
            {
                WinApi_Functions.TaskbarManipulations.SetHookToWindow();
                WinApi_Functions.TaskbarManipulations.ShowWindowInTaskbar();
                WinApi_Functions.KeyboardManipulations.UnhookKeyboard();
                WinApi_Functions.TaskbarManipulations.ShowTaskbar();
                WinApi_Functions.OtherManipulations.EnableSystemParam();
            }
            catch (Exception ex)
            {
                ClientAppConfig.demoStart = true;
                ClientAppConfig.lastError = $"Возможно, программа запущена не от имени Администратора\nКод ошибки:{ex.HResult}\n{ex.Message}";
                MessageBox.Show(ClientAppConfig.lastError, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public static void LoadEProgramms()
        {
            List<EProgramm> programms = new();
            string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(uninstallKey))
            {
                foreach (string skName in rk.GetSubKeyNames())
                {
                    using (RegistryKey sk = rk.OpenSubKey(skName))
                    {
                        string displayName = sk.GetValue("DisplayName") as string;
                        if (!string.IsNullOrEmpty(displayName))
                        {
                            if (!displayName.Contains("Пакет") && !displayName.Contains("пакет"))
                            {
                                string appPath = GetExecutablePath(displayName);
                                if (appPath is not null)
                                {
                                    BitmapImage img = SetImage(appPath);
                                    if (img is not null)
                                        programms.Add(new EProgramm() { Name = displayName, Path = appPath, Img_source = img });
                                }
                            }
                        }
                    }
                }
            }
            AppSession.eProgramms = programms;
        }
        private static string GetExecutablePath(string programName)
        {
            try
            {
                string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(uninstallKey))
                {
                    foreach (string skName in rk.GetSubKeyNames())
                    {
                        using (RegistryKey sk = rk.OpenSubKey(skName))
                        {
                            string displayName = sk.GetValue("DisplayName") as string;
                            if (!string.IsNullOrEmpty(displayName) && displayName.Contains(programName))
                            {
                                string exePath = sk.GetValue("DisplayIcon") as string;
                                if (!string.IsNullOrEmpty(exePath))
                                {
                                    int index = exePath.IndexOf(".exe");
                                    if (index != -1)
                                    {
                                        return exePath.Substring(0, index + 4);
                                    }
                                    else
                                    {
                                        return exePath;
                                    }
                                }
                            }
                        }
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        private static BitmapImage SetImage(string path)
        {
            try
            {
                var icon = System.Drawing.Icon.ExtractAssociatedIcon(path);
                var bmp = icon.ToBitmap();
                int newWidth = 256; // Новая ширина изображения
                int newHeight = 256; // Новая высота изображения
                using (var ms = new MemoryStream())
                {
                    // Устанавливаем новый размер и более высокое разрешение
                    System.Drawing.Bitmap newBmp = new System.Drawing.Bitmap(newWidth, newHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    newBmp.SetResolution(300, 300); // 300 dpi

                    // Рисуем старое изображение на новом
                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(newBmp))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(bmp, new System.Drawing.Rectangle(0, 0, newWidth, newHeight));
                    }

                    // Сохраняем новое изображение в MemoryStream
                    newBmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = ms;
                    bitmapImage.EndInit();
                    return bitmapImage;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}