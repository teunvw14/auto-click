using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;

namespace auto_clicker
{
    class Program
    {
        static void Main()
        {
            var consoleWindow = GetConsoleWindow();

            ShowWindow(consoleWindow, SW_HIDE);

            CheckForProcesses();

            Clicker.SetupClicker();

            Input.NotifyInputObservers += OnInput;
            Input.Setup();
            Application.Run();
            Input.UnHook();
        }
        

        private static void OnInput(Keys k)
        {
            if (k == Keys.F9)
            {
                Console.WriteLine(k);

                if (Clicker.running)
                    Clicker.StopClicker();
                else
                    Clicker.StartClicker();
            }

            if (Clicker.running)
            {
                if (k == Keys.Up)
                {
                    Clicker.ChangeClickSpeed(1.05F);
                }
                if (k == Keys.Down)
                {
                    Clicker.ChangeClickSpeed(0.95F);
                }
            }
        }

        private static void RestartApp()
        {
            Process.Start(Application.ExecutablePath); // Start a new instance of the app.
            Application.Exit(); // Close current instance of the app.
        }

        private static void CheckForProcesses()
        {
            Process[] existingProcesses = Process.GetProcessesByName("auto-click");
            if (existingProcesses.Length > 1)
            {
                foreach (Process p in existingProcesses)
                {
                    p.Kill();
                }
            }
        }

        // The two dll imports below will handle the window hiding.

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
    }
}
