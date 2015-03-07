using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LeagueExtender
{
    public class ExternalFeatures
    {
        // http://stackoverflow.com/questions/6415222/get-a-windows-bounds-by-its-handle
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
            public int Middle;
            public int Width
            {
                get
                {
                    return this.Right - this.Left;
                }
            }
            public int Height
            {
                get
                {
                    return this.Bottom - this.Top;
                }
            }
        }
    }
}
