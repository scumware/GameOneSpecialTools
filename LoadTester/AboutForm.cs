using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoadTester
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            m_ActionBeginInvokeClose = () =>
            {
                if (false == IsDisposed)
                    BeginInvoke(new Action(Close));
            };
        }
        private bool m_faidedOut = false;
        private readonly Action m_ActionBeginInvokeClose;

        protected override void WndProc(ref Message m)
        {

            bool handled = false;
            if (m.Msg == NativeMethods.WM_NCACTIVATE && m.WParam.ToInt32() == 0)
            {
                handleDeactivate();
            }

            if (!handled)
                base.WndProc(ref m);
        }

        private void handleDeactivate()
        {
            FaidOut(m_ActionBeginInvokeClose);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (false == m_faidedOut)
            {
                e.Cancel = true;
                FaidOut(m_ActionBeginInvokeClose);
            }
            base.OnClosing(e);
        }


        private void FaidOut(Action p_actionOnAfterFadingOut)
        {
            int duration = 1000; //in milliseconds
            int steps = 100;
            Timer timer = new Timer();
            timer.Interval = duration/steps;

            int currentStep = 0;
            timer.Tick += (arg1, arg2) =>
            {
                Opacity -= ((double) currentStep)/steps;
                currentStep++;

                if (0==Opacity || currentStep >= steps)
                {
                    m_faidedOut = true;
                    timer.Stop();
                    timer.Dispose();
                    if (p_actionOnAfterFadingOut != null)
                        p_actionOnAfterFadingOut();
                }
            };

            timer.Start();
        }
    }
}
