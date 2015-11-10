using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MosueNudge;

namespace MouseNudge
{
    public class TaskBarTray
    {
        private ContextMenu menu;
        private MenuItem ExitBtt;
        public MenuItem UpdateRateMenu;
        public MenuItem NudgeMarginsMenu;
        public NotifyIcon notificationIcon;
        private MenuItem LastUpdateRate;
        private MenuItem LastNudgeMargin;
        private MenuItem PauseBtt;

        private int[] UpdateRatesMs = {16, 33, 66, 100, 200};
        private int[] NudgeMargins = {1, 2, 5, 10, 15};

        private bool isPaused = false;

        public void Init()
        {
            Settings Conf = Settings.Get();

            menu = new ContextMenu();
            ExitBtt = new MenuItem("Exit");
            UpdateRateMenu = new MenuItem("Update Rate");
            NudgeMarginsMenu = new MenuItem("Nudge Margins");
            PauseBtt = new MenuItem("Pause");

            menu.MenuItems.Add(PauseBtt);
            menu.MenuItems.Add(UpdateRateMenu);
            menu.MenuItems.Add(NudgeMarginsMenu);
            menu.MenuItems.Add(ExitBtt);

            for (int i = 0; i < UpdateRatesMs.Length; i++)
            {
                MenuItem item = new MenuItem(String.Format("{0} times a second", Math.Round((double)(1000/UpdateRatesMs[i]))));
                UpdateRateMenu.MenuItems.Add(item);
                item.Click += new EventHandler(OnUpdateRateChange);

                if (UpdateRatesMs[i] == Conf.UpdateRate)
                    item.Checked = true;
            }

            for (int i = 0; i < NudgeMargins.Length; i++)
            {
                MenuItem item = new MenuItem(String.Format("{0} pixels", NudgeMargins[i]));
                NudgeMarginsMenu.MenuItems.Add(item);
                item.Click += new EventHandler(OnNudgeMarginChange);

                if (NudgeMargins[i] == Conf.NudgeMargin)
                    item.Checked = true;
            }

            notificationIcon = new NotifyIcon()
            {
                Icon = MouseNudge.Properties.Resources.Icon,
                ContextMenu = menu,
                Text = "MouseNudger"
            };

            ExitBtt.Click += new EventHandler(OnExit);
            PauseBtt.Click += new EventHandler(OnPauseToggle);

            notificationIcon.Visible = true;
        }

        void OnExit(object sender, EventArgs e)
        {
            Program.OnExit();
        }

        void OnUpdateRateChange(object sender, EventArgs e)
        {
            if (LastUpdateRate != null)
                LastUpdateRate.Checked = false;

            MenuItem SenderMenuItem = (MenuItem)sender;
            SenderMenuItem.Checked = true;
            Program.SetUpdateRate(UpdateRatesMs[SenderMenuItem.Index]);
            LastUpdateRate = SenderMenuItem;
        }

        void OnNudgeMarginChange(object sender, EventArgs e)
        {
            if (LastNudgeMargin != null)
                LastNudgeMargin.Checked = false;

            MenuItem SenderMenuItem = (MenuItem)sender;
            SenderMenuItem.Checked = true;
            Program.SetNudgeMargin(NudgeMargins[SenderMenuItem.Index]);
            LastNudgeMargin = SenderMenuItem;
        }

        void OnPauseToggle(object sender, EventArgs e)
        {
            isPaused = !isPaused;

            Program.PauseNudging(isPaused);
            PauseBtt.Text = isPaused ? "Resume" : "Pause";
        }

        public void Dispose()
        {
            notificationIcon.Dispose();
        }
    }
}
