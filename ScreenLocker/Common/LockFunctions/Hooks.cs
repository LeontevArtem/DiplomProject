using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScreenLocker.Common.LockFunctions
{
    public class Hooks
    {
        private const int WH_KEYBOARD_LL = 13;//Keyboard hook;

        //Keys data structure
        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            public Keys key;
        }

        //Using callbacks
        private static LowLevelKeyboardProcDelegate m_callback;
        private static LowLevelKeyboardProcDelegate m_callback_1;
        private static LowLevelKeyboardProcDelegate m_callback_2;
        private static LowLevelKeyboardProcDelegate m_callback_3;
        private static LowLevelKeyboardProcDelegate m_callback_4;
        private static LowLevelKeyboardProcDelegate m_callback_5;

        //Using hooks
        private static IntPtr m_hHook;
        private static IntPtr m_hHook_1;
        private static IntPtr m_hHook_2;
        private static IntPtr m_hHook_3;
        private static IntPtr m_hHook_4;
        private static IntPtr m_hHook_5;

        //Set hook on keyboard
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProcDelegate lpfn, IntPtr hMod, int dwThreadId);

        //Unhook keyboard
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        //Hook handle
        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandle(IntPtr lpModuleName);

        //Calling the next hook
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);


        //<Alt>+<Tab> blocking
        public static IntPtr LowLevelKeyboardHookProc_alt_tab(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)//If not alredy captured
            {
                KBDLLHOOKSTRUCT objKeyInfo = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));//Memory allocation and importing code data to KBDLLHOOKSTRUCT
                if (objKeyInfo.key == Keys.Alt || objKeyInfo.key == Keys.Tab)
                {
                    return (IntPtr)1;//<Alt>+<Tab> blocking
                }
            }
            return CallNextHookEx(m_hHook, nCode, wParam, lParam);//Go to next hook
        }

        //<WinKey> capturing
        public static IntPtr LowLevelKeyboardHookProc_win(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)//If not alredy captured
            {
                KBDLLHOOKSTRUCT objKeyInfo = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));//Memory allocation and importing code data to KBDLLHOOKSTRUCT
                if (objKeyInfo.key == Keys.RWin || objKeyInfo.key == Keys.LWin)
                {
                    return (IntPtr)1;//<WinKey> blocking
                }
            }
            return CallNextHookEx(m_hHook_1, nCode, wParam, lParam);//Go to next hook
        }

        //<Delete> capturing
        public static IntPtr LowLevelKeyboardHookProc_del(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)//If not alredy captured
            {
                KBDLLHOOKSTRUCT objKeyInfo = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));//Memory allocation and importing code data to KBDLLHOOKSTRUCT
                if (objKeyInfo.key == Keys.Delete)
                {
                    return (IntPtr)1;//<Delete> blocking
                }
            }
            return CallNextHookEx(m_hHook_3, nCode, wParam, lParam);//Go to next hook
        }

        //<Control> capturing
        public static IntPtr LowLevelKeyboardHookProc_ctrl(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)//If not alredy captured
            {
                KBDLLHOOKSTRUCT objKeyInfo = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));//Memory allocation and importing code data to KBDLLHOOKSTRUCT
                if (objKeyInfo.key == Keys.RControlKey || objKeyInfo.key == Keys.LControlKey)
                {
                    return (IntPtr)1;//<Control> blocking
                }
            }
            return CallNextHookEx(m_hHook_2, nCode, wParam, lParam);//Go to next hook
        }

        //<Alt> capturing
        public static IntPtr LowLevelKeyboardHookProc_alt(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)//If not alredy captured
            {
                KBDLLHOOKSTRUCT objKeyInfo = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));//Memory allocation and importing code data to KBDLLHOOKSTRUCT
                if (objKeyInfo.key == Keys.Alt)
                {
                    return (IntPtr)1;//<Alt> blocking
                }
            }
            return CallNextHookEx(m_hHook_4, nCode, wParam, lParam);//Go to next hook
        }

        //<Alt>+<Space> blocking
        public static IntPtr LowLevelKeyboardHookProc_alt_space(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)//If not alredy captured
            {
                KBDLLHOOKSTRUCT objKeyInfo = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));//Memory allocation and importing code data to KBDLLHOOKSTRUCT
                if (objKeyInfo.key == Keys.Alt || objKeyInfo.key == Keys.Space)
                {
                    return (IntPtr)1;//<Alt>+<Space> blocking
                }
            }
            return CallNextHookEx(m_hHook_5, nCode, wParam, lParam);//Go to next hook
        }

        //Delegate for using hooks
        private delegate IntPtr LowLevelKeyboardProcDelegate(int nCode, IntPtr wParam, IntPtr lParam);

        //Setting all hooks
        public static void SetHook()
        {
            //Hooks callbacks by delegate
            m_callback = LowLevelKeyboardHookProc_win;
            m_callback_1 = LowLevelKeyboardHookProc_alt_tab;
            m_callback_2 = LowLevelKeyboardHookProc_ctrl;
            m_callback_3 = LowLevelKeyboardHookProc_del;
            m_callback_4 = LowLevelKeyboardHookProc_alt;
            m_callback_5 = LowLevelKeyboardHookProc_alt_space;
            //Hooks setting
            m_hHook = SetWindowsHookEx(WH_KEYBOARD_LL, m_callback, GetModuleHandle(IntPtr.Zero), 0);
            m_hHook_1 = SetWindowsHookEx(WH_KEYBOARD_LL, m_callback_1, GetModuleHandle(IntPtr.Zero), 0);
            m_hHook_2 = SetWindowsHookEx(WH_KEYBOARD_LL, m_callback_2, GetModuleHandle(IntPtr.Zero), 0);
            m_hHook_3 = SetWindowsHookEx(WH_KEYBOARD_LL, m_callback_3, GetModuleHandle(IntPtr.Zero), 0);
            m_hHook_4 = SetWindowsHookEx(WH_KEYBOARD_LL, m_callback_4, GetModuleHandle(IntPtr.Zero), 0);
            m_hHook_5 = SetWindowsHookEx(WH_KEYBOARD_LL, m_callback_5, GetModuleHandle(IntPtr.Zero), 0);

        }

        //Keyboard unhooking
        public static void Unhook()
        {
            UnhookWindowsHookEx(m_hHook);
            UnhookWindowsHookEx(m_hHook_1);
            UnhookWindowsHookEx(m_hHook_2);
            UnhookWindowsHookEx(m_hHook_3);
            UnhookWindowsHookEx(m_hHook_4);
            UnhookWindowsHookEx(m_hHook_5);
        }
    }
}
