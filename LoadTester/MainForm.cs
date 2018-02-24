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

            flowLayoutPanel.Parent.SuspendLayout();
            flowLayoutPanel.SuspendLayout();
            try
            {

                foreach (var threadWrapper in newWrappers)
                {
                    ThreadControl threadControl = new ThreadControl();
                    threadControl.Wrapper = threadWrapper;

                    flowLayoutPanel.Controls.Add(threadControl);
                    AddSeries(threadWrapper);

                    threadControl.Anchor = AnchorStyles.Left;
                    threadControl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    threadControl.AutoSize = true;

                    threadControl.Visible = true;
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
            this.chart1.Series.SuspendUpdates();
            foreach (var threadWrapper in ThreadsManager.ThreadWrappers)
            {
                foreach (Series series in chart1.Series)
                {
                    if (series.Tag == threadWrapper)
                    {
                        var speeds = (double[])threadWrapper.Speeds.Clone();
                        for (int pointIdex = 0; pointIdex < series.Points.Count; pointIdex++)
                        {
                            var point = series.Points[pointIdex];
                            var speed = speeds[pointIdex];
                            point.YValues[0] = speed;
                        }
                    }
                }
            }
            chart1.ChartAreas.ResumeUpdates();
        }

        private void AddSeries(ThreadWrapper p_threadWrapper)
        {
            this.chart1.Series.SuspendUpdates();

            var newSeries = new Series
            {
                BorderColor = Color.White,
                BorderDashStyle = ChartDashStyle.Solid,
                ChartArea = chart1.ChartAreas[0].Name,
                ChartType = SeriesChartType.StepLine,
                Color = Color.Lime,
                LabelBackColor = Color.Maroon,
                LabelForeColor = Color.Lime,
                Legend = "Legend1",
                Name = "Thread +" + p_threadWrapper.ThreadId,
                Tag = p_threadWrapper
            };
            this.chart1.Series.Add(newSeries);

            chart1.ChartAreas.SuspendUpdates();
            for (int index = 0; index < p_threadWrapper.Speeds.Length; index++)
            {
                var value = p_threadWrapper.Speeds[index];
                var dataPoint = newSeries.Points.Add(value);
            }

            chart1.ChartAreas.ResumeUpdates();
        }
    }
}
