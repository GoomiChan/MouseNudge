using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace MouseNudge
{
    public class MouseNudger
    {
        public int UpdateRate = 13; // How fast to check
        public int ScreenNudgeMargin = 5; // How close to the edge to be before nudging
        bool ShouldRun = false;
        private Thread ActiveThread = null;

        public void StartThread()
        {
            if (!ShouldRun)
            {
                ShouldRun = true;
                ActiveThread = new Thread(Run);
                ActiveThread.Start();
            }
        }

        public void Stop()
        {
            ShouldRun = false;
        }

        private void Run()
        {
            while (ShouldRun)
            {
                Update();
                Thread.Sleep(UpdateRate);
            }
        }

        private void Update()
        {
            bool IsFullscreen = Util.IsForegroundFullscreen();
            Debug.WriteLine("IsFullscreen: "+ IsFullscreen);

            if (!IsFullscreen)
            {
                Screen ActiveScreen = Screen.FromPoint(Cursor.Position);

                Rectangle LeftBounds = new Rectangle(new Point(ActiveScreen.Bounds.Left, ActiveScreen.Bounds.Top), new Size(ScreenNudgeMargin, ActiveScreen.Bounds.Height));
                Rectangle RightBounds = new Rectangle(new Point(ActiveScreen.Bounds.Right - ScreenNudgeMargin, ActiveScreen.Bounds.Top), new Size(ScreenNudgeMargin, ActiveScreen.Bounds.Height));

                Point NudgeAmount = new Point();
                if (LeftBounds.Contains(Cursor.Position) && NeedsNudge(ActiveScreen, TabAlignment.Left, out NudgeAmount))
                {
                    Cursor.Position = new Point(Cursor.Position.X, NudgeAmount.Y);
                }
                else if (RightBounds.Contains(Cursor.Position) && NeedsNudge(ActiveScreen, TabAlignment.Right, out NudgeAmount))
                {
                    Cursor.Position = new Point(Cursor.Position.X, NudgeAmount.Y);
                }
            }
        }

        // Check if we need to nudge the mosue down and by how much
        private bool NeedsNudge(Screen ActiveScreen, TabAlignment Dir, out Point NudgeAmount)
        {
            NudgeAmount = new Point(0, 0);

            if (Dir == TabAlignment.Left)
            {
                // Hack to get the screen to the left, I couldn't find any API call to get the layout of the screens so this should do for most cases
                Screen Left = Screen.FromPoint(new Point(Cursor.Position.X - ScreenNudgeMargin * 2, ActiveScreen.Bounds.Bottom - ActiveScreen.Bounds.Height / 2));

                if (Cursor.Position.Y <= Left.Bounds.Top)
                    NudgeAmount.Y = Left.Bounds.Top;
                else if (Cursor.Position.Y > Left.Bounds.Bottom)
                    NudgeAmount.Y = Left.Bounds.Bottom;
                else
                    return false;

                return true;
            }
            else if (Dir == TabAlignment.Right)
            {
                Screen Right = Screen.FromPoint(new Point(Cursor.Position.X + ScreenNudgeMargin * 2, ActiveScreen.Bounds.Bottom - ActiveScreen.Bounds.Height / 2));

                if (Cursor.Position.Y <= Right.Bounds.Top)
                    NudgeAmount.Y = Right.Bounds.Top;
                else if (Cursor.Position.Y > Right.Bounds.Bottom)
                    NudgeAmount.Y = Right.Bounds.Bottom;
                else
                    return false;

                return true;
            }

            return false;
        }
    }
}
