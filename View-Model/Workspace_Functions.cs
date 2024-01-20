using Enigma_Client_V2.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Enigma_Client_V2.Model;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Enigma_Client_V2.View_Model
{
    public class Workspace_Functions
    {
        public static void LogOut()
        {
            try
            {
                AppSession.current_session = null;
                AppSession.Context.Users.FirstOrDefault(x => x.IdUser == AppSession.current_user.IdUser).AuthStatus = false;
                AppSession.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Код ошибки: {ex.HResult}. Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public static async void BuyTariff(SessionTariff tariff)
        {
            if (AppSession.current_user.Cash is not null)
            {
                if (tariff.Sum_price > AppSession.current_user.Cash)
                {
                    EnigmaMessageBox.Show($"На вашем балансе недостаточно средств. Не хватает {tariff.Sum_price - AppSession.current_user.Cash} руб.\nПополните баланс и повторите попытку", "Предупреждение", EnigmaMessageBox.MessageBoxButton.Ок, EnigmaMessageBox.MessageBoxButton.Отменить);
                }
                else
                {
                    Session tempSes = new();
                    AppSession.Context.Sessions.Load();
                    if (AppSession.Context.Sessions.Where(x => x.Status == true).Count() > 0)
                    {
                        if (AppSession.Context.Sessions.Where(x => x.Status == true).FirstOrDefault(x => x.IdUser == AppSession.current_user.IdUser) is not null)
                        {
                            Session userSession = AppSession.Context.Sessions.Where(x => x.Status == true).FirstOrDefault(x => x.IdUser == AppSession.current_user.IdUser);
                            AppSession.current_session = userSession;
                            EnigmaMessageBox.Show($"{AppSession.current_user.Name}, у вас уже запущена сессия с тарифом {userSession.IdTariffNavigation.Name}! Вы можете продлить сессию либо дождаться её окончания.", "Предупреждение", EnigmaMessageBox.MessageBoxButton.Ок, EnigmaMessageBox.MessageBoxButton.Отменить);
                        }
                        else
                        {
                            Session userSession = new() { IdComputer = ClientAppConfig.current_PC.IdComp, IdTariff = tariff.IdTariff, IdUser = AppSession.current_user.IdUser, StartSession = DateTime.Now, EndSession = DateTime.Now.AddHours(tariff.Duration), Status = true };
                            AppSession.Context.Sessions.Add(userSession);
                            AppSession.Context.Users.FirstOrDefault(x => x.IdUser == AppSession.current_user.IdUser).Cash -= tariff.Sum_price;
                            EnigmaMessageBox.Show($"{AppSession.current_user.Name}, сессия с тарифом {userSession.IdTariffNavigation.Name} успешно создана! Таймер запущен, наслаждайтесь процессом.", "Сообщение", EnigmaMessageBox.MessageBoxButton.Ок, EnigmaMessageBox.MessageBoxButton.Отменить);
                            await AppSession.Context.SaveChangesAsync();
                            AppSession.current_user = AppSession.Context.Users.FirstOrDefault(x => x.IdUser == AppSession.current_user.IdUser);
                            AppSession.current_session = AppSession.Context.Sessions.Where(x => x.Status).FirstOrDefault(x => x.IdUser == AppSession.current_user.IdUser);
                        }
                    }
                    else
                    {
                        Session userSession = new() { IdComputer = ClientAppConfig.current_PC.IdComp, IdTariff = tariff.IdTariff, IdUser = AppSession.current_user.IdUser, StartSession = DateTime.Now, EndSession = DateTime.Now.AddHours(tariff.Duration), Status = true };
                        AppSession.Context.Sessions.Add(userSession);
                        AppSession.Context.Users.FirstOrDefault(x => x.IdUser == AppSession.current_user.IdUser).Cash -= tariff.Sum_price;
                        EnigmaMessageBox.Show($"{AppSession.current_user.Name}, сессия с тарифом {userSession.IdTariffNavigation.Name} успешно создана! Таймер запущен, наслаждайтесь процессом.", "Сообщение", EnigmaMessageBox.MessageBoxButton.Ок, EnigmaMessageBox.MessageBoxButton.Отменить);
                        await AppSession.Context.SaveChangesAsync();
                        AppSession.current_user = AppSession.Context.Users.FirstOrDefault(x => x.IdUser == AppSession.current_user.IdUser);
                        AppSession.current_session = AppSession.Context.Sessions.Where(x => x.Status).FirstOrDefault(x => x.IdUser == AppSession.current_user.IdUser);
                    }
                }
            }
            else
                EnigmaMessageBox.Show($"{AppSession.current_user.Name}, пожалуйста, пополните баланс и повторите попытку.", "Сообщение", EnigmaMessageBox.MessageBoxButton.Ок, EnigmaMessageBox.MessageBoxButton.Отменить);
        }
        public static void AddUserMoney(string cashToDepos)
        {
            if (!cashToDepos.Contains(' '))
            {
                if (AppSession.Context.Users.FirstOrDefault(x => x.IdAuth == AppSession.current_user.IdAuth).Cash == null)
                    AppSession.Context.Users.FirstOrDefault(x => x.IdAuth == AppSession.current_user.IdAuth).Cash = 0;
                AppSession.Context.Users.FirstOrDefault(x => x.IdUser == AppSession.current_user.IdUser).Cash += int.Parse(cashToDepos);
                AppSession.Context.SaveChanges();
                AppSession.current_user = AppSession.Context.Users.FirstOrDefault(x => x.IdUser == AppSession.current_user.IdUser);
                System.Windows.Forms.DialogResult result = EnigmaMessageBox.Show($"{AppSession.current_user.Name}, ваш счёт пополнен на {cashToDepos} руб.", "Сообщение", EnigmaMessageBox.MessageBoxButton.Ок, EnigmaMessageBox.MessageBoxButton.Отменить);
            }
            else
                MessageBox.Show("Значение имеет пробелы");
        }
        public static bool CheckSession()
        {
            if (AppSession.current_session.EndSession < DateTime.Now)
            {
                AppSession.Context.Sessions.Where(x => x.Status).FirstOrDefault(x => x.IdUser == AppSession.current_user.IdUser).Status = false;
                AppSession.Context.SaveChanges();
                AppSession.current_session = null;
                EnigmaMessageBox.Show($"Ваша сессия закончилась", "Сообщение", EnigmaMessageBox.MessageBoxButton.Ок, EnigmaMessageBox.MessageBoxButton.Отменить);
                return false;
            }
            return true;
        }
        public static void AddTime()
        {
            if (AppSession.current_session is not null)
                if (AppSession.current_session.Status)
                {
                    if (AppSession.current_user.Cash is not null)
                    {
                        if (AppSession.current_user.Cash < 50)
                            EnigmaMessageBox.Show($"Вам не хватает {50 - AppSession.current_user.Cash}, пополните счёт и повторите попытку.", "Предупреждение", EnigmaMessageBox.MessageBoxButton.Ок, EnigmaMessageBox.MessageBoxButton.Отменить);
                        else
                        {
                            AppSession.Context.Sessions.Where(x => x.Status).FirstOrDefault(x => x.IdUser == AppSession.current_user.IdUser).EndSession = AppSession.current_session.EndSession.AddHours(1);
                            AppSession.Context.Users.FirstOrDefault(x => x.IdUser == AppSession.current_user.IdUser).Cash -= 50;
                            AppSession.Context.SaveChanges();
                            AppSession.current_session = AppSession.Context.Sessions.Where(x => x.Status).FirstOrDefault(x => x.IdUser == AppSession.current_user.IdUser);
                            AppSession.current_user = AppSession.Context.Users.FirstOrDefault(x => x.IdUser == AppSession.current_user.IdUser);
                            EnigmaMessageBox.Show($"Поздравляем! Ваша сессия успешно продлена на 1 час, с вашего баланса списано 50 р. Наслаждайтесь!", "Сообщение", EnigmaMessageBox.MessageBoxButton.Ок, EnigmaMessageBox.MessageBoxButton.Отменить);
                        }
                    }
                }
                else
                    EnigmaMessageBox.Show($"Ваша сессия закончилась, вы не можете продлить оконченную сессию", "Предупреждение", EnigmaMessageBox.MessageBoxButton.Ок, EnigmaMessageBox.MessageBoxButton.Отменить);
            else
                EnigmaMessageBox.Show($"Сессия отсутсвует, выберите и приобретите пакет и повторите попытку", "Предупреждение", EnigmaMessageBox.MessageBoxButton.Ок, EnigmaMessageBox.MessageBoxButton.Отменить);
        }
        public static void RunProgramm(EProgramm eProgramm)
        {
            try
            {
                Process.Start(eProgramm.Path);
            }
            catch
            {
                EnigmaMessageBox.Show("Не удалось запустить игру, попробуйте позднее или позовите системного администратора. Приносим свои извинения", "Ошибка", EnigmaMessageBox.MessageBoxButton.Ок, EnigmaMessageBox.MessageBoxButton.Отменить);
            }
        }
    }
}
