using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LoadTester
{
    public partial class MainForm :Form
    {
        public MainForm()
        {
            InitializeComponent();
            btnFill.Text = "Add " + Environment.ProcessorCount + " threads";

            NativeMethods.DisableProcessWindowsGhosting();
            ThreadsManager.Init();
        }

        private void button1_Click( object sender, EventArgs e )
        {
            //m_threadWrappers 
            int count;
            if (sender == btnFill)
            {
                count = Environment.ProcessorCount;
            }
            else
            {
                count = 1;
            }
            IList<ThreadWrapper> newWrappers = new ThreadWrapper[count];
            for (int i = 0; i < newWrappers.Count; i++)
            {
                newWrappers[i] = new ThreadWrapper();
            }

            flowLayoutPanel.Parent.SuspendLayout();
            flowLayoutPanel.SuspendLayout();
            try
            {

                foreach (var threadWrapper in newWrappers)
                {
                    ThreadControl threadControl = new ThreadControl();
                    threadControl.Wrapper = threadWrapper;

                    //flowLayoutPanel.Controls.Add( threadControl, 0, m_threadWrappers.Count );
                    flowLayoutPanel.Controls.Add(threadControl);

                    threadControl.Anchor = AnchorStyles.Left;
                    threadControl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    threadControl.AutoSize = true;

                    threadControl.Visible = true;

                    //flowLayoutPanel.RowStyles.Add( new RowStyle( SizeType.AutoSize ) );

                    ThreadsManager.AddThread(threadWrapper);
                }

            }
            finally
            {
                flowLayoutPanel.Parent.ResumeLayout(false);
                flowLayoutPanel.ResumeLayout();
            }
        }

        private void btnStart_Click( object sender, EventArgs e )
        {
            ThreadsManager.StartAll();
        }

        private void btnStop_Click( object sender, EventArgs e )
        {
            ThreadsManager.StopAll();
        }

        private void MainForm_FormClosing( object sender, FormClosingEventArgs e )
        {
            ThreadsManager.StopAll();
        }
    }
}
