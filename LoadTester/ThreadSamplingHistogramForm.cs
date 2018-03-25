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
    public partial class ThreadSamplingHistogramForm : Form
    {
        public ThreadSamplingHistogramForm()
        {
            InitializeComponent();
            ChartSize = 250;

            m_sampleHistogrammDataFactory = new SampleHistogrammDataFactory(ChartSize);
        }

        private readonly SampleHistogrammDataFactory m_sampleHistogrammDataFactory;
        public Color SeriesColor { get; set; }
        public IThreadWrapper ThreadWrapper { get; set; }
        public int ChartSize;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AddSeries(ThreadWrapper);
        }


        private void AddSeries(IThreadWrapper p_threadWrapper)
        {
            this.chart1.Series.SuspendUpdates();

            Color seriesColor = Color.FromArgb(255, SeriesColor.R, SeriesColor.G, SeriesColor.B);

            var newSeries = new Series
            {
                BorderColor = seriesColor,
                BorderDashStyle = ChartDashStyle.Solid,
                ChartArea = chart1.ChartAreas[0].Name,
                ChartType = SeriesChartType.Column,
                Color = seriesColor,
                LabelBackColor = Color.Maroon,
                LabelForeColor = Color.Lime,
                Legend = "Legend1",
                Name = "Thread +" + p_threadWrapper.ThreadId,
            };
            this.chart1.Series.Add(newSeries);

            chart1.ChartAreas.SuspendUpdates();

            for (int index = 0; index < ChartSize+ 2; index++)
            {
                var dataPointIndex = newSeries.Points.AddXY(0, 0);
            }

            chart1.ChartAreas.ResumeUpdates();
            UpdateChart();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            timer.Enabled = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            UpdateChart();
        }

        private void UpdateChart()
        {
            var speeds = ThreadWrapper.GetSpeeds();
            var uniqueValues = m_sampleHistogrammDataFactory.GetSortedUniques(speeds);
            lblUniqueValuesValue.Text = uniqueValues.Length.ToString();

            var resultedValues = m_sampleHistogrammDataFactory.GetHistagrammValues(uniqueValues);

            this.chart1.ChartAreas.SuspendUpdates();
            var series = chart1.Series[0];
            double lastXValue = 0.0;
            double lastYValue = 0.0;

            int i = 0;
            for (; i < ChartSize; i++)
            {
                SampleHistogrammDataFactory.UniqueValue resultedValue = resultedValues[i];
                var seriesPoint = series.Points[i];


                seriesPoint.YValues[0] = resultedValue.Count;
                seriesPoint.XValue = resultedValue.Value;
            }

            this.chart1.ChartAreas[0].RecalculateAxesScale();
            this.chart1.ChartAreas.ResumeUpdates();
        }
    }
}
