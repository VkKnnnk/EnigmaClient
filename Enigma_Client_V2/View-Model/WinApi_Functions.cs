using Enigma_Client_V2.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Enigma_Client_V2.View_Model
{
    class WinApi_Functions
    {
        public static class TaskbarManipulations
        {
            // Import the necessary WinAPI functions
            [DllImport("user32.dll")]
            private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
            [DllImport("user32.dll")]
            private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

            // WinApi функции панели задач
            [DllImport("user32.dll")]
            private static extern int FindWindow(string className, string windowText);

            [DllImport("user32.dll")]
            private static extern int ShowWindow(int hwnd, int command);

            // Define constants for WindowStyles and ExtendedWindowStyles
            private const int GWL_STYLE = -16;
            private const int GWL_EXSTYLE = -20;
            private const int WS_MINIMIZE = 0x20000000;
            private const int WS_EX_APPWINDOW = 0x00040000;
            private const int WS_EX_TOOLWINDOW = 0x00000080;

            // Константы панели задач
            private const int SW_HIDE = 0;
            private const int SW_SHOW = 1;

            // Переменные
            private static int style = new();
            private static int exStyle = new();
            private static IntPtr hWnd = new();
            // Устанавливает хэндл для указанного окна
            public static void SetHookToWindow()
            {
                hWnd = new WindowInteropHelper(ClientAppConfig.current_window).Handle;
                style = GetWindowLong(hWnd, GWL_STYLE);
                exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
            }
            // Скрывает окно из панели задач, удаляя флаг WS_EX_APPWINDOW
            public static void HideWindowFromTaskbar()
            {
                exStyle &= ~WS_EX_APPWINDOW;
                SetWindowLong(hWnd, GWL_EXSTYLE, exStyle);
            }
            // Показывает окно в панели задач, добавляя флаг WS_EX_APPWINDOW и минимизируя окно при помощи флага WS_MINIMIZE и метода SetWindowLong
            public static void ShowWindowInTaskbar()
            {
                exStyle |= WS_EX_APPWINDOW;
                SetWindowLong(hWnd, GWL_EXSTYLE, exStyle);
                SetWindowLong(hWnd, GWL_STYLE, style | WS_MINIMIZE);
            }
            // Скрывает окно из списка Alt-Tab, добавляя флаг WS_EX_TOOLWINDOW
            public static void HideWindowFromAltTab()
            {
                exStyle |= WS_EX_TOOLWINDOW;
                SetWindowLong(hWnd, GWL_EXSTYLE, exStyle);
            }
            public static void HideTaskbar()
            {
                int hwnd = FindWindow("Shell_TrayWnd", "");
                ShowWindow(hwnd, SW_HIDE);
            }
            public static void ShowTaskbar()
            {
                int hwnd = FindWindow("Shell_TrayWnd", "");
                ShowWindow(hwnd, SW_SHOW);
            }
        }
        public class KeyboardManipulations
        {
            [DllImport("user32.dll")]
            private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
            private static bool ShowWindow(IntPtr hWnd, Consts.SHOWWINDOW nCmdShow)
            {
                return ShowWindow(hWnd, (int)nCmdShow);
            }

            [DllImport("user32.dll")]
            private static extern bool SetForegroundWindow(IntPtr hWnd);

            private static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
            {
                int error = 0;
                IntPtr result = IntPtr.Zero;
                SetLastError(0);

                if (IntPtr.Size == 4)
                {
                    Int32 tempResult = IntSetWindowLong(hWnd, nIndex, IntPtrToInt32(dwNewLong));
                    error = Marshal.GetLastWin32Error();
                    result = new IntPtr(tempResult);
                }
                else
                {
                    result = IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
                    error = Marshal.GetLastWin32Error();
                }

                if ((result == IntPtr.Zero) && (error != 0))
                {
                    throw new System.ComponentModel.Win32Exception(error);
                }

                return result;
            }

            [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
            private static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

            [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
            private static extern Int32 IntSetWindowLong(IntPtr hWnd, int nIndex, Int32 dwNewLong);

            private static int IntPtrToInt32(IntPtr intPtr)
            {
                return unchecked((int)intPtr.ToInt64());
            }

            [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
            private static extern void SetLastError(int dwErrorCode);

            private struct KBDLLHOOKSTRUCT
            {
                public int vkCode;
                int scanCode;
                public int flags;
                int time;
                int dwExtraInfo;
            }

            private delegate int LowLevelKeyboardProcDelegate(int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam);

            [DllImport("user32.dll")]
            private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProcDelegate lpfn, IntPtr hMod, int dwThreadId);

            [DllImport("user32.dll")]
            private static extern bool UnhookWindowsHookEx(IntPtr hHook);

            [DllImport("user32.dll")]
            private static extern int CallNextHookEx(int hHook, int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam);

            [DllImport("kernel32.dll")]
            private static extern IntPtr GetModuleHandle(IntPtr path);

            private static IntPtr hHook;
            private static LowLevelKeyboardProcDelegate hookProc;

            private static int LowLevelKeyboardProc(int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam)
            {
                if (nCode >= 0)
                    switch (wParam)
                    {
                        case 256: // WM_KEYDOWN
                        case 257: // WM_KEYUP
                        case 260: // WM_SYSKEYDOWN
                        case 261: // M_SYSKEYUP
                            if (
                                //(lParam.vkCode == 0x09 && lParam.flags == 32) || // Alt+Tab
                                (lParam.vkCode == 0x1b && lParam.flags == 32) || // Alt+Esc
                                (lParam.vkCode == 0x73 && lParam.flags == 32) || // Alt+F4
                                (lParam.vkCode == 0x1b && lParam.flags == 0) || // Ctrl+Esc
                                (lParam.vkCode == 0x5b && lParam.flags == 1) || // Left Windows Key 
                                (lParam.vkCode == 0x5c && lParam.flags == 1))  // Right Windows Key 
                            {
                                return 1; //Do not handle key events
                            }
                            break;
                    }
                return CallNextHookEx(0, nCode, wParam, ref lParam);
            }
            //Устанавливает хук на нажатия клавиш на клавиатуре
            public static void HookKeyboard()
            {
                IntPtr hModule = GetModuleHandle(IntPtr.Zero);
                hookProc = new LowLevelKeyboardProcDelegate(LowLevelKeyboardProc);
                hHook = SetWindowsHookEx(Consts.WH_KEYBOARD_LL, hookProc, hModule, 0);
                if (hHook == IntPtr.Zero)
                {
                    MessageBox.Show("Failed to set hook, error = " + Marshal.GetLastWin32Error());
                }
            }
            //Удаляет хук на нажатия клавиш на клавиатуре
            public static void UnhookKeyboard()
            {
                if (hHook != IntPtr.Zero)
                {
                    UnhookWindowsHookEx(hHook);
                    hHook = IntPtr.Zero;
                }
            }
            private class Consts
            {
                public enum SHOWWINDOW : int
                {
                    SW_HIDE = 0,
                    SW_SHOWNORMAL = 1,
                    SW_NORMAL = 1,
                    SW_SHOWMINIMIZED = 2,
                    SW_SHOWMAXIMIZED = 3,
                    SW_MAXIMIZE = 3,
                    SW_SHOWNOACTIVATE = 4,
                    SW_SHOW = 5,
                    SW_MINIMIZE = 6,
                    SW_SHOWMINNOACTIVE = 7,
                    SW_SHOWNA = 8,
                    SW_RESTORE = 9,
                    SW_SHOWDEFAULT = 10,
                    SW_FORCEMINIMIZE = 11,
                    SW_MAX = 11
                }

                public const int WH_KEYBOARD_LL = 13;
            }
        }
        public class OtherManipulations
        {
            public static void DisableSystemParam()
            {
                string key = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
                Process process = new Process();
                process.StartInfo.FileName = "reg.exe";
                process.StartInfo.Verb = "runas"; // Запуск процесса с повышенными правами

                // Отключение рабочего стола
                string valueName1 = "NoDesktop";
                int valueData1 = 1;
                process.StartInfo.Arguments = $"add \"HKCU\\{key}\" /v {valueName1} /t REG_DWORD /d {valueData1} /f";
                process.Start();
                process.WaitForExit();

                // Отключение диспетчера задач
                string valueName2 = "DisableTaskMgr";
                int valueData2 = 1;
                process.StartInfo.Arguments = $"add \"HKCU\\{key}\" /v {valueName2} /t REG_DWORD /d {valueData2} /f";
                process.Start();
                process.WaitForExit();

                //// Путь к ключу реестра
                //string keyPath1 = @"SOFTWARE\Policies\Chromium";

                //// Создание ключа "FileBrowserDisabled" со значением 1 для запрета доступа к файловой системе
                //Registry.SetValue("HKEY_LOCAL_MACHINE\\" + keyPath1, "FileBrowserDisabled", 1, RegistryValueKind.DWord);

                //// Путь к ключу реестра для Yandex браузера
                //string keyPath2 = @"SOFTWARE\Policies\Yandex\YandexBrowser";
                //const string baseKey = "HKEY_CURRENT_USER\\";

                //// Создание ключа "DisabledFilePicker" со значением 1 для запрета открытия диалоговых окон выбора файлов
                //Registry.SetValue(baseKey + keyPath2, "DisabledFilePicker", 1, RegistryValueKind.DWord);
            }
            public static void EnableSystemParam()
            {
                string key1 = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
                Process process = new Process();
                process.StartInfo.FileName = "reg.exe";
                process.StartInfo.Verb = "runas"; // Запуск процесса с повышенными правами

                // Удаление значения NoDesktop
                string valueName1 = "NoDesktop";
                process.StartInfo.Arguments = $"delete \"HKCU\\{key1}\" /v {valueName1} /f";
                process.Start();
                process.WaitForExit();

                // Удаление значения DisableTaskMgr
                string valueName2 = "DisableTaskMgr";
                process.StartInfo.Arguments = $"delete \"HKCU\\{key1}\" /v {valueName2} /f";
                process.Start();
                process.WaitForExit();

                //// Путь к ключу реестра
                //string keyPath = @"SOFTWARE\Policies\Chromium";

                //// Удаление ключа "FileBrowserDisabled"
                //Registry.LocalMachine.DeleteSubKey(keyPath);

                //// Путь к ключу реестра для Yandex браузера
                //string keyPath2 = @"SOFTWARE\Policies\Yandex\YandexBrowser";

                //// Удаление ключа "DisabledFilePicker"
                //RegistryKey key2 = Registry.CurrentUser.OpenSubKey(keyPath2, true);
                //if (key2 != null)
                //{
                //    key2.DeleteValue("DisabledFilePicker", false);
                //    key2.Close();
                //}
            }
        }
    }
}