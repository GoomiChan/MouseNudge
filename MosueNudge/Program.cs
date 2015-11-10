using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MouseNudge;

namespace MosueNudge
{
    public class Program
    {
        static MouseNudger Nudger = null;
        static TaskBarTray Tray = null;
        static Settings Conf = Settings.Get();

        static void Main(string[] args)
        {
            Conf.Load();

            Nudger = new MouseNudger();
            Nudger.UpdateRate = Conf.UpdateRate;
            Nudger.ScreenNudgeMargin = Conf.NudgeMargin;
            Nudger.StartThread();

            Tray = new TaskBarTray();
            Tray.Init();

            Application.Run();
        }

        public static void OnExit()
        {
            Nudger.Stop();
            Tray.Dispose();
            Application.Exit();
        }

        public static void SetUpdateRate(int rate)
        {
            Conf.UpdateRate = rate;
            Nudger.UpdateRate = rate;
            Conf.Save();
        }

        public static void SetNudgeMargin(int margin)
        {
            Conf.NudgeMargin = margin;
            Nudger.ScreenNudgeMargin = margin;
            Conf.Save();
        }

        public static void PauseNudging(bool shouldPause)
        {
            if (shouldPause)
            {
                Nudger.Stop();
            }
            else
            {
                Nudger.StartThread();
            }
        }
    }
}
