namespace LoadTester
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose( disposing );
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
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.tableMain = new LoadTester.DoubleBufferedTableLayoutPanel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tableBottom = new System.Windows.Forms.TableLayoutPanel();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnFill = new System.Windows.Forms.Button();
            this.lblProcessAffinnityMask = new System.Windows.Forms.Label();
            this.flowPanelAfinnity = new System.Windows.Forms.FlowLayoutPanel();
            this.lblProcessPriority = new System.Windows.Forms.Label();
            this.cmbProcessPriority = new System.Windows.Forms.ComboBox();
            this.tableMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.tableBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // tableMain
            // 
            this.tableMain.BackColor = System.Drawing.Color.Transparent;
            this.tableMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tableMain.ColumnCount = 1;
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.Controls.Add(this.splitContainer, 0, 0);
            this.tableMain.Controls.Add(this.tableBottom, 0, 2);
            this.tableMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableMain.Location = new System.Drawing.Point(0, 0);
            this.tableMain.Name = "tableMain";
            this.tableMain.RowCount = 3;
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableMain.Size = new System.Drawing.Size(1008, 561);
            this.tableMain.TabIndex = 4;
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer.ForeColor = System.Drawing.Color.Green;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer.Panel1.Controls.Add(this.flowLayoutPanel);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.chart1);
            this.splitContainer.Size = new System.Drawing.Size(1008, 493);
            this.splitContainer.SplitterDistance = 334;
            this.splitContainer.SplitterWidth = 5;
            this.splitContainer.TabIndex = 5;
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.AutoScroll = true;
            this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(1006, 332);
            this.flowLayoutPanel.TabIndex = 6;
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.chart1.BorderlineColor = System.Drawing.Color.Red;
            chartArea1.AxisX.IsStartedFromZero = false;
            chartArea1.AxisX.LabelStyle.ForeColor = System.Drawing.Color.YellowGreen;
            chartArea1.AxisX.LineColor = System.Drawing.Color.Green;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.Green;
            chartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisX.MajorTickMark.LineColor = System.Drawing.Color.DarkGreen;
            chartArea1.AxisX.MinorGrid.LineColor = System.Drawing.Color.Bisque;
            chartArea1.AxisX.MinorTickMark.LineColor = System.Drawing.Color.DimGray;
            chartArea1.AxisX.ScaleBreakStyle.LineColor = System.Drawing.Color.DarkOliveGreen;
            chartArea1.AxisX.Title = "Time";
            chartArea1.AxisX.TitleForeColor = System.Drawing.Color.LimeGreen;
            chartArea1.AxisX2.TitleForeColor = System.Drawing.Color.ForestGreen;
            chartArea1.AxisY.LabelStyle.ForeColor = System.Drawing.Color.YellowGreen;
            chartArea1.AxisY.LineColor = System.Drawing.Color.Green;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Green;
            chartArea1.AxisY.ScaleBreakStyle.LineColor = System.Drawing.Color.DarkGreen;
            chartArea1.AxisY.Title = "Iterations";
            chartArea1.AxisY.TitleForeColor = System.Drawing.Color.YellowGreen;
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.BorderColor = System.Drawing.Color.ForestGreen;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.BackColor = System.Drawing.Color.Transparent;
            legend1.ForeColor = System.Drawing.Color.Green;
            legend1.HeaderSeparatorColor = System.Drawing.Color.Green;
            legend1.ItemColumnSeparatorColor = System.Drawing.Color.Green;
            legend1.Name = "Legend1";
            legend1.Title = "Threads";
            legend1.TitleBackColor = System.Drawing.Color.Transparent;
            legend1.TitleForeColor = System.Drawing.Color.Green;
            legend1.TitleSeparator = System.Windows.Forms.DataVisualization.Charting.LegendSeparatorStyle.DoubleLine;
            legend1.TitleSeparatorColor = System.Drawing.Color.Green;
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            this.chart1.Size = new System.Drawing.Size(1006, 152);
            this.chart1.TabIndex = 5;
            this.chart1.Text = "chart1";
            this.chart1.DoubleClick += new System.EventHandler(this.chart1_DoubleClick);
            // 
            // tableBottom
            // 
            this.tableBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableBottom.AutoSize = true;
            this.tableBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableBottom.BackColor = System.Drawing.Color.Transparent;
            this.tableBottom.ColumnCount = 4;
            this.tableBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableBottom.Controls.Add(this.btnStop, 3, 1);
            this.tableBottom.Controls.Add(this.btnStart, 3, 0);
            this.tableBottom.Controls.Add(this.btnAdd, 2, 1);
            this.tableBottom.Controls.Add(this.btnFill, 2, 0);
            this.tableBottom.Controls.Add(this.lblProcessAffinnityMask, 0, 0);
            this.tableBottom.Controls.Add(this.flowPanelAfinnity, 1, 0);
            this.tableBottom.Controls.Add(this.lblProcessPriority, 0, 1);
            this.tableBottom.Controls.Add(this.cmbProcessPriority, 1, 1);
            this.tableBottom.Location = new System.Drawing.Point(0, 493);
            this.tableBottom.Margin = new System.Windows.Forms.Padding(0);
            this.tableBottom.Name = "tableBottom";
            this.tableBottom.RowCount = 2;
            this.tableBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableBottom.Size = new System.Drawing.Size(1008, 68);
            this.tableBottom.TabIndex = 5;
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.AutoSize = true;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnStop.ForeColor = System.Drawing.Color.Yellow;
            this.btnStop.Location = new System.Drawing.Point(865, 42);
            this.btnStop.Margin = new System.Windows.Forms.Padding(8, 8, 3, 3);
            this.btnStop.MinimumSize = new System.Drawing.Size(140, 0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(140, 23);
            this.btnStop.TabIndex = 0;
            this.btnStop.Text = "Stop All";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.AutoSize = true;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnStart.ForeColor = System.Drawing.Color.Yellow;
            this.btnStart.Location = new System.Drawing.Point(865, 8);
            this.btnStart.Margin = new System.Windows.Forms.Padding(8, 8, 3, 3);
            this.btnStart.MinimumSize = new System.Drawing.Size(140, 0);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(140, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start All";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.AutoSize = true;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAdd.ForeColor = System.Drawing.Color.Yellow;
            this.btnAdd.Location = new System.Drawing.Point(714, 42);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(8, 8, 3, 3);
            this.btnAdd.MinimumSize = new System.Drawing.Size(140, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(140, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "Add 1 thread";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnFill
            // 
            this.btnFill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFill.AutoSize = true;
            this.btnFill.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFill.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnFill.ForeColor = System.Drawing.Color.Yellow;
            this.btnFill.Location = new System.Drawing.Point(714, 8);
            this.btnFill.Margin = new System.Windows.Forms.Padding(8, 8, 3, 3);
            this.btnFill.MinimumSize = new System.Drawing.Size(140, 0);
            this.btnFill.Name = "btnFill";
            this.btnFill.Size = new System.Drawing.Size(140, 23);
            this.btnFill.TabIndex = 3;
            this.btnFill.Text = "Add";
            this.btnFill.UseVisualStyleBackColor = true;
            this.btnFill.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblProcessAffinnityMask
            // 
            this.lblProcessAffinnityMask.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblProcessAffinnityMask.AutoSize = true;
            this.lblProcessAffinnityMask.ForeColor = System.Drawing.Color.Yellow;
            this.lblProcessAffinnityMask.Location = new System.Drawing.Point(0, 9);
            this.lblProcessAffinnityMask.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.lblProcessAffinnityMask.Name = "lblProcessAffinnityMask";
            this.lblProcessAffinnityMask.Size = new System.Drawing.Size(116, 13);
            this.lblProcessAffinnityMask.TabIndex = 5;
            this.lblProcessAffinnityMask.Text = "Process Affinnity mask:";
            // 
            // flowPanelAfinnity
            // 
            this.flowPanelAfinnity.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flowPanelAfinnity.AutoSize = true;
            this.flowPanelAfinnity.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowPanelAfinnity.BackColor = System.Drawing.Color.Transparent;
            this.flowPanelAfinnity.Location = new System.Drawing.Point(122, 17);
            this.flowPanelAfinnity.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.flowPanelAfinnity.Name = "flowPanelAfinnity";
            this.flowPanelAfinnity.Size = new System.Drawing.Size(0, 0);
            this.flowPanelAfinnity.TabIndex = 4;
            this.flowPanelAfinnity.WrapContents = false;
            // 
            // lblProcessPriority
            // 
            this.lblProcessPriority.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblProcessPriority.AutoSize = true;
            this.lblProcessPriority.ForeColor = System.Drawing.Color.Yellow;
            this.lblProcessPriority.Location = new System.Drawing.Point(3, 44);
            this.lblProcessPriority.Name = "lblProcessPriority";
            this.lblProcessPriority.Size = new System.Drawing.Size(108, 13);
            this.lblProcessPriority.TabIndex = 6;
            this.lblProcessPriority.Text = "Process priority class:";
            // 
            // cmbProcessPriority
            // 
            this.cmbProcessPriority.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbProcessPriority.BackColor = System.Drawing.Color.Black;
            this.cmbProcessPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProcessPriority.DropDownWidth = 160;
            this.cmbProcessPriority.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbProcessPriority.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbProcessPriority.FormattingEnabled = true;
            this.cmbProcessPriority.Location = new System.Drawing.Point(122, 40);
            this.cmbProcessPriority.Name = "cmbProcessPriority";
            this.cmbProcessPriority.Size = new System.Drawing.Size(240, 21);
            this.cmbProcessPriority.TabIndex = 7;
            this.cmbProcessPriority.SelectedIndexChanged += new System.EventHandler(this.cmbProcessPriority_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(33)))), ((int)(((byte)(30)))));
            this.BackgroundImage = global::LoadTester.Properties.Resources.DarkGrayWallpepper;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(1008, 561);
            this.Controls.Add(this.tableMain);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Lime;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "game one special tools - LoadTester";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.tableMain.ResumeLayout(false);
            this.tableMain.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.tableBottom.ResumeLayout(false);
            this.tableBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnFill;
        private DoubleBufferedTableLayoutPanel tableMain;
        private System.Windows.Forms.TableLayoutPanel tableBottom;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.FlowLayoutPanel flowPanelAfinnity;
        private System.Windows.Forms.Label lblProcessAffinnityMask;
        private System.Windows.Forms.Label lblProcessPriority;
        private System.Windows.Forms.ComboBox cmbProcessPriority;
    }
}

