using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LoadTester
{
    public static class NativeMethods
    {
        [DllImport( "user32.dll", CharSet = CharSet.Unicode )]
        public static extern void SendMessage( IntPtr p_hWnd, uint p_msg, IntPtr p_wParam, IntPtr p_lParam );

        public const uint WS_EX_COMPOSITED = 0x02000000;
        public const uint WM_SETREDRAW = 0xB;
    }
}
