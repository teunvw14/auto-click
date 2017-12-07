using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Timers;

namespace Clicker
{
    class Program
    {

        private static System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        static void Main()
        {
            CheckForProcesses();

            Clicker.clickThread.Start();

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

                Clicker.enabled = !Clicker.enabled; //WORKS!
            }
        }



        //private static void ManageException()
        //{
        //    DialogResult dRes = MessageBox.Show("Something went wrong with the CommandMe app, do you want to restart it?", "CommandMe Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
        //    if (dRes == DialogResult.Yes)
        //    {
        //        RestartApp();
        //    }
        //    else if (dRes == DialogResult.No)
        //    {
        //        Application.Exit();
        //    }
        //}

        private static void RestartApp()
        {
            MessageBox.Show("Something went wrong, restarting application.");
            Process.Start(Application.ExecutablePath); // Start a new instance of the app.
            Application.Exit(); // Close current instance of the app.
        }

        private static void CheckForProcesses()
        {
            Process[] existingProcesses = Process.GetProcessesByName("CommandMe");
            if (existingProcesses.Length > 1)
            {
                MessageBox.Show("An instance of CommandMe is already running.", "CommandMe already running", MessageBoxButtons.OK, MessageBoxIcon.Information);
                for (int i = 1; i < existingProcesses.Length; i++)
                {
                    existingProcesses[i].Kill();
                }
            }
        }

    }
}
