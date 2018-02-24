using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

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
            IList<ThreadWrapper> newWrappers = ThreadsManager.AddThreads(count);
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

        private void timer_Tick(object sender, EventArgs e)
        {
            UpdateChart();
        }

        private void UpdateChart()
        {
            this.chart1.Series.Clear();

            foreach (var threadWrapper in ThreadsManager.ThreadWrappers)
            {
                var series1 = new Series();
                series1.BorderColor = Color.White;
                series1.BorderDashStyle = ChartDashStyle.Solid;
                series1.ChartArea = "ChartArea1";
                series1.ChartType = SeriesChartType.StepLine;
                series1.Color = Color.Lime;
                series1.LabelBackColor = Color.Maroon;
                series1.LabelForeColor = Color.Lime;
                series1.Legend = "Legend1";
                series1.Name = "Thread +" + threadWrapper.GetHashCode();
                this.chart1.Series.Add(series1);

                chart1.ChartAreas.SuspendUpdates();
                int[] values = new int[]
                    {1, 2, 3, 4, 5, 6, 7, 8, 9, 9, 9, 9, 3, 8, 8, 8, 7, 7, 7, 6, 6, 5, 5, 5, 4, 4, 3, 2, 1};
                for (int i = 0; i < 1000; i++)
                    for (int index = 0; index < values.Length; index++)
                    {
                        var value = values[index];
                        var dataPoint = series1.Points.Add((double) value*10);
                    }
                chart1.ChartAreas.ResumeUpdates();

            }
        }
    }
}
