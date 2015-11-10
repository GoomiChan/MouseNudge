using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseNudge
{
    public class Util
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        private static extern IntPtr GetShellWindow();
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowRect(IntPtr hwnd, out RECT rc);

        private static IntPtr DestopHandle;
        private static IntPtr ShellHandle;

        public Util()
        {
            DestopHandle = GetDesktopWindow();
            ShellHandle = GetShellWindow();
        }

        // http://www.richard-banks.org/2007/09/how-to-detect-if-another-application-is.html
        public static bool IsForegroundFullscreen()
        {
            RECT appBounds;
            Rectangle screenBounds;
            IntPtr hWnd;

            hWnd = GetForegroundWindow();
            if (hWnd != null && !hWnd.Equals(IntPtr.Zero))
            {
                if (!(hWnd.Equals(DestopHandle) || hWnd.Equals(ShellHandle)))
                {
                    GetWindowRect(hWnd, out appBounds);
                    screenBounds = Screen.FromHandle(hWnd).Bounds;
                    if ((appBounds.Bottom - appBounds.Top) == screenBounds.Height && (appBounds.Right - appBounds.Left) == screenBounds.Width)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
