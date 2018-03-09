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
            SampleHistogrammDataFactory.ChartSize = ChartSize;
        }

        public Color SeriesColor { get; set; }
        public ThreadWrapper ThreadWrapper { get; set; }
        public const int ChartSize = 500;

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
                ChartType = SeriesChartType.StepLine,
                Color = seriesColor,
                LabelBackColor = Color.Maroon,
                LabelForeColor = Color.Lime,
                Legend = "Legend1",
                Name = "Thread +" + p_threadWrapper.ThreadId,
            };
            this.chart1.Series.Add(newSeries);

            chart1.ChartAreas.SuspendUpdates();

            for (int index = 0; index < ChartSize; index++)
            {
                var value = 0;

                var dataPointIndex = newSeries.Points.AddXY(0, 1);
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
            for (int index = 0; index < ChartSize; index++)
            {
                var resultedValue = resultedValues[index];
                if (resultedValue == null)
                    continue;

                var seriesPoint = series.Points[index];
                seriesPoint.YValues[0] = resultedValue.Count;
                seriesPoint.XValue = resultedValue.Value;
            }
            this.chart1.ChartAreas.ResumeUpdates();
        }
    }
}
