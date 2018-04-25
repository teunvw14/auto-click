using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;

namespace auto_clicker
{
    class Clicker
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public static bool running = false;

        public static System.Windows.Forms.Timer click_Timer = new System.Windows.Forms.Timer();
        private static float clicksPerSecond = 1.0F;

        public static void SetupClicker()
        {
            click_Timer.Tick += new System.EventHandler(DoMouseClick);
            click_Timer.Interval = (int) (1000.0F / clicksPerSecond);
        }
        
        public static void StartClicker()
        {
            click_Timer.Start();
            running = true;
        }

        public static void StopClicker()
        {
            click_Timer.Stop();
            running = false;
        }

        private static void DoMouseClick(object sender, System.EventArgs e)
        {
            //Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }

        public static void ChangeClickSpeed(float factor)
        {
            if (factor > 0)
            {
                clicksPerSecond *= factor;
                click_Timer.Interval = (int)(1000.0F / clicksPerSecond);
            }
        }
    }
}
