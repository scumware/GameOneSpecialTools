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
            SampleHistogrammDataFactory.ChartSize = ChartSize;
        }

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
                ChartType = SeriesChartType.Line,
                Color = seriesColor,
                LabelBackColor = Color.Maroon,
                LabelForeColor = Color.Lime,
                Legend = "Legend1",
                Name = "Thread +" + p_threadWrapper.ThreadId,
            };
            this.chart1.Series.Add(newSeries);

            chart1.ChartAreas.SuspendUpdates();

            for (int index = 0; index < ChartSize *3 + 2; index++)
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
            var uniqueValues = SampleHistogrammDataFactory.GetSortedUniques(speeds);
            lblUniqueValuesValue.Text = uniqueValues.Length.ToString();

            var resultedValues = SampleHistogrammDataFactory.GetResultedValues(uniqueValues);

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
                ++i;
                if (i < series.Points.Count)
                {
                    series.Points[i].XValue = lastXValue;
                    series.Points[i].YValues[0] = 0;
                }
                ++i;
                if (i < series.Points.Count)
                {
                    series.Points[i].XValue = lastXValue;
                    series.Points[i].YValues[0] = lastYValue;
                }
            }
            if (lastResultedNotNullValue != null)
            {
                var index = ChartSize-1;
                var dataPoint = series.Points[index];
                dataPoint.XValue = lastXValue + 1;
                dataPoint.YValues[0] = 0;
            }
            this.chart1.ChartAreas.ResumeUpdates();
            this.chart1.ChartAreas[0].RecalculateAxesScale();
        }
    }
}
