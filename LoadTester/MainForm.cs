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
/*
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            //series1.BorderColor = System.Drawing.Color.White;
            series1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Color = System.Drawing.Color.Lime;
            series1.EmptyPointStyle.Color = System.Drawing.Color.Transparent;
            series1.LabelBackColor = System.Drawing.Color.Maroon;
            series1.LabelForeColor = System.Drawing.Color.Lime;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            series1.MarkerColor = Color.Aqua;
            var values = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 9, 9, 9, 9, 8, 8, 8, 7, 7, 7, 6, 6, 5, 5, 5, 4, 4, 3, 2, 1 };
                var points = series1.Points;
            for (int index = 0; index < values.Length; index++)
            {
                var value = values[index];

                var dataPoint = points.Add((double)value);
                dataPoint.Color = Color.Aqua;
                dataPoint.AxisLabel = "dsf";
                dataPoint.LabelBackColor = Color.Aqua;
                points[index].XValue = index;
                points.Add(dataPoint);
            }
 */
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
    }
}
