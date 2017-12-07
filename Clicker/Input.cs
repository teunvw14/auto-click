using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace Clicker
{
    class Input
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public delegate void OnInput(Keys k);
        public static event OnInput NotifyInputObservers;

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        public static void Setup()
        {
            try
            {
                _hookID = SetHook(_proc);
                Application.Run();
                UnhookWindowsHookEx(_hookID);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong with input setup.");
            }
        }


        private static Keys keyTyped;
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                //StreamWriter sw = new StreamWriter(Application.StartupPath + @"\log.txt", true);
                //sw.Write((Keys)vkCode);
                //sw.Close();
                keyTyped = (Keys)vkCode;
                NotifyInputObservers(keyTyped);
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }



        /// <summary>
        /// checks too see if the given string ends with the starting letters of the other string
        /// </summary>
        /// <param name="z"> ends with starting chars of strings </param>
        /// <param name="s"> </param>
        /// <returns></returns>
        private static bool CheckString(string z, string s)
        {
            bool result = false;
            string temp = String.Empty;
            foreach (char c in s)
            {
                temp += c;
                if (z.EndsWith(temp))
                {
                    result = true;
                    break;
                }
            }
            return result;

        }


        //imports
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
        IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        // The two dll imports below will handle the window hiding.

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;

    }

}
