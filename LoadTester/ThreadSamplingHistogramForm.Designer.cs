namespace LoadTester
{
    partial class ThreadSamplingHistogramForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThreadSamplingHistogramForm));
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.lblUniqueValues = new System.Windows.Forms.Label();
            this.lblUniqueValuesValue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.chart1.BorderlineColor = System.Drawing.Color.Red;
            chartArea1.AxisX.LabelStyle.ForeColor = System.Drawing.Color.YellowGreen;
            chartArea1.AxisX.LineColor = System.Drawing.Color.Green;
            chartArea1.AxisX.LogarithmBase = 2D;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.Green;
            chartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisX.MajorTickMark.LineColor = System.Drawing.Color.DarkGreen;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisX.MinorGrid.LineColor = System.Drawing.Color.Bisque;
            chartArea1.AxisX.MinorTickMark.LineColor = System.Drawing.Color.DimGray;
            chartArea1.AxisX.ScaleBreakStyle.LineColor = System.Drawing.Color.DarkOliveGreen;
            chartArea1.AxisX.Title = "Speed (iterations / timeslice)";
            chartArea1.AxisX.TitleForeColor = System.Drawing.Color.Gold;
            chartArea1.AxisX2.Minimum = -500D;
            chartArea1.AxisX2.ScaleBreakStyle.LineColor = System.Drawing.Color.Maroon;
            chartArea1.AxisX2.TitleForeColor = System.Drawing.Color.ForestGreen;
            chartArea1.AxisY.LabelStyle.ForeColor = System.Drawing.Color.YellowGreen;
            chartArea1.AxisY.LabelStyle.Format = "{0.00} %";
            chartArea1.AxisY.LineColor = System.Drawing.Color.Green;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Green;
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.AxisY.ScaleBreakStyle.LineColor = System.Drawing.Color.DarkGreen;
            chartArea1.AxisY.Title = "Group percents %";
            chartArea1.AxisY.TitleForeColor = System.Drawing.Color.Gold;
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.BorderColor = System.Drawing.Color.ForestGreen;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            this.chart1.Size = new System.Drawing.Size(654, 337);
            this.chart1.TabIndex = 6;
            this.chart1.Text = "chart1";
            // 
            // timer
            // 
            this.timer.Interval = 500;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // lblUniqueValues
            // 
            this.lblUniqueValues.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblUniqueValues.AutoSize = true;
            this.lblUniqueValues.BackColor = System.Drawing.Color.Transparent;
            this.lblUniqueValues.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.lblUniqueValues.Location = new System.Drawing.Point(12, 315);
            this.lblUniqueValues.Name = "lblUniqueValues";
            this.lblUniqueValues.Size = new System.Drawing.Size(78, 13);
            this.lblUniqueValues.TabIndex = 7;
            this.lblUniqueValues.Text = "Unique values:";
            // 
            // lblUniqueValuesValue
            // 
            this.lblUniqueValuesValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblUniqueValuesValue.AutoSize = true;
            this.lblUniqueValuesValue.BackColor = System.Drawing.Color.Transparent;
            this.lblUniqueValuesValue.ForeColor = System.Drawing.Color.Gold;
            this.lblUniqueValuesValue.Location = new System.Drawing.Point(96, 315);
            this.lblUniqueValuesValue.Name = "lblUniqueValuesValue";
            this.lblUniqueValuesValue.Size = new System.Drawing.Size(13, 13);
            this.lblUniqueValuesValue.TabIndex = 7;
            this.lblUniqueValuesValue.Text = "0";
            // 
            // ThreadSamplingHistogramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(18)))));
            this.ClientSize = new System.Drawing.Size(654, 337);
            this.Controls.Add(this.lblUniqueValuesValue);
            this.Controls.Add(this.lblUniqueValues);
            this.Controls.Add(this.chart1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ThreadSamplingHistogramForm";
            this.Opacity = 0.85D;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thread speeds sampling histogram";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label lblUniqueValues;
        private System.Windows.Forms.Label lblUniqueValuesValue;
    }
}