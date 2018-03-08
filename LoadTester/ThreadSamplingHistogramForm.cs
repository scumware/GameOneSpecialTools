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
        public const int ValuesCount = 500;

        public ThreadSamplingHistogramForm()
        {
            InitializeComponent();
        }

        public Color SeriesColor { get; set; }
        public ThreadWrapper ThreadWrapper { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AddSeries(ThreadWrapper);

        }


        private void AddSeries(ThreadWrapper p_threadWrapper)
        {
            this.chart1.Series.SuspendUpdates();

            Color seriesColor = SeriesColor;

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

            for (int index = 0; index < ValuesCount; index++)
            {
                var value = 0;

                var dataPointIndex = newSeries.Points.AddXY(value, 1);
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
            var uniqueValues = GetSortedUniques(speeds);

            var resultedValues = GetResultedValues(uniqueValues);

            this.chart1.Series.SuspendUpdates();
            var series = chart1.Series[0];
            for (int index = 0; index < ValuesCount; index++)
            {
                var resultedValue = resultedValues[index];
                if (resultedValue == null)
                    continue;

                var seriesPoint = series.Points[index];
                seriesPoint.YValues[0] = resultedValue.Count;
                seriesPoint.XValue = resultedValue.Value;
            }
            this.chart1.Series.ResumeUpdates();
        }

        private static UniqueValue[] GetResultedValues(UniqueValue[] p_uniqueValues)
        {
            UniqueValue[] resultedValues;
            if (p_uniqueValues.Length != ValuesCount)
            {
                resultedValues = new UniqueValue[ValuesCount];
                if (p_uniqueValues.Length < ValuesCount)
                {
                    var stepSize = ((double)ValuesCount)/(double)p_uniqueValues.Length;
                    int j = 0;
                    for (double i = 0; i < resultedValues.Length; i += stepSize)
                    {
                        var index = (int)Math.Floor(i);
                        if (j > p_uniqueValues.Length - 1)
                            j = p_uniqueValues.Length - 1;

                        var uniqueValue = p_uniqueValues[j];
                        resultedValues[index] = new UniqueValue();
                        resultedValues[index].Value = uniqueValue.Value;
                        resultedValues[index].Count = uniqueValue.Count;
                        ++j;
                    }
                }
                else if(p_uniqueValues.Length > ValuesCount)
                {
                    var stepSize = ((double)ValuesCount) / p_uniqueValues.Length;
                    double currentStep = 0.0;
                    for (int index = 0; index < p_uniqueValues.Length; index++)
                    {
                        var uniqueValue = p_uniqueValues[index];
                        currentStep += stepSize;
                        var j = (int) Math.Floor(currentStep);
                        if (j > ValuesCount-1)
                            j = ValuesCount-1;

                        resultedValues[j] = new UniqueValue();
                        resultedValues[j].Value = uniqueValue.Value;
                        resultedValues[j].Count = uniqueValue.Count;
                    }
                }
            }
            else
            {
                resultedValues = p_uniqueValues;
            }
            return resultedValues;
        }

        private static UniqueValue[] GetSortedUniques(double[] p_speeds)
        {
            Array.Sort(p_speeds);

            var uniqueValues = new List<UniqueValue>();

            double previousSpeed = 0.0;
            var count = 0;

            for (int index = 0; index < p_speeds.Length; index++)
            {
                ++count;
                var speed = p_speeds[index];
                if (index == 0)
                {
                    previousSpeed = speed;
                    continue;
                }
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (previousSpeed != speed)
                {
                    uniqueValues.Add(new UniqueValue() {Count = count, Value = previousSpeed});
                    count = 0;
                }
                previousSpeed = speed;
            }
            return uniqueValues.OrderBy(p_value => p_value.Value).ToArray();
        }

        class UniqueValue
        {
            public double Value;
            public int Count;
        }
    }
}
