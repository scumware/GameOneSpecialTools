using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LoadTester
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            NativeMethods.SetThreadPriority(NativeMethods.GetCurrentThread(), ThreadPriority.THREAD_PRIORITY_HIGHEST);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            Application.Run( new MainForm() );
        }
    }
}
