using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace Clicker
{
    class Clicker
    {
        public static bool enabled = false;

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private static int clicksPerSecond = 50;
        private static int msBetweenClicks = (int)1000 / clicksPerSecond;

        public static Thread clickThread = new Thread(new ThreadStart(SimulateMouseClick));
        
        private static void SimulateMouseClick()
        {
            while (true)
            {
                Console.WriteLine("test");
                if (!enabled)
                {
                    Thread.Sleep(500);
                }
                else
                {
                    DoMouseClick();
                    Thread.Sleep(msBetweenClicks);
                }
            }
        }
        
        private static void DoMouseClick()
        {
            //Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }

    }
}
