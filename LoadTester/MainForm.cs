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

        private Color[] chartColors = new Color[]{Color.Lime, Color.Red, Color.Yellow, Color.White, Color.BlueViolet, Color.GreenYellow, Color.OrangeRed, Color.Brown, Color.CadetBlue, Color.Aqua, Color.Azure, Color.Blue, Color.Coral, Color.DeepPink, Color.DarkSalmon, Color.Silver};


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
            ThreadsManager.FinishWork();
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
                        var times = (long[])ThreadsManager.Times.Clone();
                        double previousTime = 0.0;

                        for (int pointIdex = 0; pointIdex < series.Points.Count; pointIdex++)
                        {
                            var point = series.Points[pointIdex];
                            var speed = speeds[pointIdex];
                            point.YValues[0] = speed;

                            var time = (double)times[pointIdex];
                            if (previousTime > time)
                            {
                                time = previousTime;
                            }


                            point.XValue = time;
                            previousTime = time;
                        }
                    }
                }
            }
            chart1.ChartAreas.ResumeUpdates();
            chart1.ChartAreas[0].RecalculateAxesScale();
        }

        private void AddSeries(ThreadWrapper p_threadWrapper)
        {
            this.chart1.Series.SuspendUpdates();
            var colorIndex = this.chart1.Series.Count;
            if (colorIndex >= chartColors.Length)
            {
                colorIndex = 0;
            }

            var originalColor = chartColors[colorIndex];
            Color seriesColor = Color.FromArgb(196, originalColor.R, originalColor.G, originalColor.B);

            var newSeries = new Series
            {
                BorderColor = Color.White,
                BorderDashStyle = ChartDashStyle.Solid,
                ChartArea = chart1.ChartAreas[0].Name,
                ChartType = SeriesChartType.FastLine,
                Color = seriesColor,
                LabelBackColor = Color.Maroon,
                LabelForeColor = Color.Lime,
                Legend = "Legend1",
                Name = "Thread +" + p_threadWrapper.ThreadId,
                Tag = p_threadWrapper
            };
            this.chart1.Series.Add(newSeries);

            chart1.ChartAreas.SuspendUpdates();

            var times = (long[])ThreadsManager.Times.Clone();
            double previousTime = 0.0;
            for (int index = 0; index < p_threadWrapper.Speeds.Length; index++)
            {
                var value = p_threadWrapper.Speeds[index];
                var time = (double)times[index];
                if (previousTime > time)
                    time = previousTime;

                var dataPoint = newSeries.Points.AddXY(time, value);
                previousTime = time;
            }

            chart1.ChartAreas.ResumeUpdates();
        }
    }
}
