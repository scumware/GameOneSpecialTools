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
            ChartSize = ThreadsManager.LastMeasurementsCount/50;

            m_sampleHistogrammDataFactory = new SampleHistogrammDataFactory(ChartSize);
        }

        private readonly SampleHistogrammDataFactory m_sampleHistogrammDataFactory;
        public Color SeriesColor { get; set; }
        public ThreadWrapper ThreadWrapper { get; set; }
        public int ChartSize;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AddSeries(ThreadWrapper);
        }


        private void AddSeries(ThreadWrapper p_threadWrapper)
        {
            this.chart1.Series.SuspendUpdates();

            Color seriesColor = Color.FromArgb(255, SeriesColor.R, SeriesColor.G, SeriesColor.B);

            var newSeries = new Series
            {
                BorderColor = Color.White,
                BorderDashStyle = ChartDashStyle.Solid,
                ChartArea = chart1.ChartAreas[0].Name,
                ChartType = SeriesChartType.Area,
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
                var value = 0;

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
            var speeds = (double[])ThreadWrapper.Speeds.Clone();
            var uniqueValues = m_sampleHistogrammDataFactory.GetSortedUniques(speeds);
            lblUniqueValuesValue.Text = uniqueValues.Length.ToString();

            var resultedValues = m_sampleHistogrammDataFactory.GetHistagrammValues(uniqueValues);

            this.chart1.ChartAreas.SuspendUpdates();
            var series = chart1.Series[0];
            SampleHistogrammDataFactory.UniqueValue lastResultedNotNullValue = null;
            double lastXValue = 0.0;
            double lastYValue = 0.0;

            int i = 1;
            for (; i < ChartSize-1; i++)
            {
                SampleHistogrammDataFactory.UniqueValue resultedValue = resultedValues[i];
                    var seriesPoint = series.Points[i];
                if (resultedValue == null)
                {
                    seriesPoint.YValues[0] = 0;
                    seriesPoint.XValue = lastXValue;

                    continue;
                }

                lastResultedNotNullValue = resultedValue;

                seriesPoint.YValues[0] = lastYValue= resultedValue.Count;
                seriesPoint.XValue = lastXValue=  resultedValue.Value;
            }

            if (lastResultedNotNullValue != null)
            {
                var index = ChartSize-1;
                var dataPoint = series.Points[index];
                dataPoint.XValue = lastXValue + 1;
                dataPoint.YValues[0] = 0;
            }

            var values = resultedValues.Where(p_value => null != p_value);
            if (values.Any())
            {
                double min = values.Min(p_value => p_value.Value);
                series.Points[0].XValue = min;
                series.Points[0].YValues[0] = min;
            }

            this.chart1.ChartAreas.ResumeUpdates();
            this.chart1.ChartAreas[0].RecalculateAxesScale();
        }
    }
}
