namespace LoadTester
{
    partial class ThreadControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;



        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableMain = new LoadTester.DoubleBufferedTableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.flowPanelAfinnity = new System.Windows.Forms.FlowLayoutPanel();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.labeledComboPriority = new LoadTester.LabeledCombo();
            this.labeledComboLoad = new LoadTester.LabeledCombo();
            this.lblError = new System.Windows.Forms.Label();
            this.tableMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableMain
            // 
            this.tableMain.AutoSize = true;
            this.tableMain.ColumnCount = 4;
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableMain.Controls.Add(this.pictureBox1, 0, 0);
            this.tableMain.Controls.Add(this.flowPanelAfinnity, 1, 0);
            this.tableMain.Controls.Add(this.btnStartStop, 3, 0);
            this.tableMain.Controls.Add(this.labeledComboPriority, 2, 0);
            this.tableMain.Controls.Add(this.labeledComboLoad, 2, 1);
            this.tableMain.Controls.Add(this.lblError, 0, 2);
            this.tableMain.Location = new System.Drawing.Point(0, 0);
            this.tableMain.Margin = new System.Windows.Forms.Padding(0);
            this.tableMain.Name = "tableMain";
            this.tableMain.RowCount = 3;
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableMain.Size = new System.Drawing.Size(554, 91);
            this.tableMain.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox1.Image = global::LoadTester.Properties.Resources._32px_Sert___dead_smile_svg;
            this.pictureBox1.Location = new System.Drawing.Point(5, 23);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(5);
            this.pictureBox1.Name = "pictureBox1";
            this.tableMain.SetRowSpan(this.pictureBox1, 2);
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.DoubleClick += new System.EventHandler(this.pictureBox1_DoubleClick);
            // 
            // flowPanelAfinnity
            // 
            this.flowPanelAfinnity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.flowPanelAfinnity.AutoSize = true;
            this.flowPanelAfinnity.Location = new System.Drawing.Point(45, 39);
            this.flowPanelAfinnity.Name = "flowPanelAfinnity";
            this.tableMain.SetRowSpan(this.flowPanelAfinnity, 2);
            this.flowPanelAfinnity.Size = new System.Drawing.Size(1, 0);
            this.flowPanelAfinnity.TabIndex = 2;
            // 
            // btnStartStop
            // 
            this.btnStartStop.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnStartStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStartStop.ForeColor = System.Drawing.Color.YellowGreen;
            this.btnStartStop.Location = new System.Drawing.Point(449, 27);
            this.btnStartStop.Margin = new System.Windows.Forms.Padding(8, 6, 3, 6);
            this.btnStartStop.Name = "btnStartStop";
            this.tableMain.SetRowSpan(this.btnStartStop, 2);
            this.btnStartStop.Size = new System.Drawing.Size(69, 23);
            this.btnStartStop.TabIndex = 1;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.UseVisualStyleBackColor = false;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // labeledComboPriority
            // 
            this.labeledComboPriority.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labeledComboPriority.AutoSize = true;
            this.labeledComboPriority.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labeledComboPriority.Combo.BackColor = System.Drawing.Color.Black;
            this.labeledComboPriority.Combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.labeledComboPriority.Combo.DropDownWidth = 160;
            this.labeledComboPriority.Combo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labeledComboPriority.Combo.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labeledComboPriority.Combo.ForeColor = System.Drawing.Color.LimeGreen;
            this.labeledComboPriority.Combo.Location = new System.Drawing.Point(109, 3);
            this.labeledComboPriority.Combo.Name = "comboBox";
            this.labeledComboPriority.Combo.Size = new System.Drawing.Size(240, 24);
            this.labeledComboPriority.Combo.TabIndex = 1;
            this.labeledComboPriority.ForeColor = System.Drawing.Color.YellowGreen;
            // 
            // 
            // 
            this.labeledComboPriority.Label.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labeledComboPriority.Label.Location = new System.Drawing.Point(3, 3);
            this.labeledComboPriority.Label.MinimumSize = new System.Drawing.Size(65, 0);
            this.labeledComboPriority.Label.Name = "label";
            this.labeledComboPriority.Label.TabIndex = 0;
            this.labeledComboPriority.Label.Text = "Priority base";
            this.labeledComboPriority.Location = new System.Drawing.Point(51, 3);
            this.labeledComboPriority.Name = "labeledComboPriority";
            this.labeledComboPriority.Size = new System.Drawing.Size(355, 33);
            this.labeledComboPriority.TabIndex = 3;
            this.labeledComboPriority.SelectedValueChanged += new System.EventHandler(this.labeledComboPriority_SelectedValueChanged);
            // 
            // labeledComboLoad
            // 
            this.labeledComboLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labeledComboLoad.AutoSize = true;
            this.labeledComboLoad.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labeledComboLoad.Combo.BackColor = System.Drawing.Color.Black;
            this.labeledComboLoad.Combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.labeledComboLoad.Combo.DropDownWidth = 160;
            this.labeledComboLoad.Combo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labeledComboLoad.Combo.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labeledComboLoad.Combo.ForeColor = System.Drawing.Color.LimeGreen;
            this.labeledComboLoad.Combo.Location = new System.Drawing.Point(109, 3);
            this.labeledComboLoad.Combo.Name = "comboBox";
            this.labeledComboLoad.Combo.Size = new System.Drawing.Size(240, 24);
            this.labeledComboLoad.Combo.TabIndex = 1;
            this.labeledComboLoad.ForeColor = System.Drawing.Color.YellowGreen;
            // 
            // 
            // 
            this.labeledComboLoad.Label.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labeledComboLoad.Label.Location = new System.Drawing.Point(3, 3);
            this.labeledComboLoad.Label.MinimumSize = new System.Drawing.Size(65, 0);
            this.labeledComboLoad.Label.Name = "label";
            this.labeledComboLoad.Label.TabIndex = 0;
            this.labeledComboLoad.Label.Text = "Load type";
            this.labeledComboLoad.Location = new System.Drawing.Point(51, 42);
            this.labeledComboLoad.Name = "labeledComboLoad";
            this.labeledComboLoad.Size = new System.Drawing.Size(355, 33);
            this.labeledComboLoad.TabIndex = 3;
            this.labeledComboLoad.SelectedValueChanged += new System.EventHandler(this.labeledComboLoad_SelectedValueChanged);
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.tableMain.SetColumnSpan(this.lblError, 4);
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(3, 78);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(0, 13);
            this.lblError.TabIndex = 4;
            this.lblError.Visible = false;
            // 
            // ThreadControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.tableMain);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.YellowGreen;
            this.Name = "ThreadControl";
            this.Size = new System.Drawing.Size(556, 129);
            this.tableMain.ResumeLayout(false);
            this.tableMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DoubleBufferedTableLayoutPanel tableMain;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.FlowLayoutPanel flowPanelAfinnity;
        private LabeledCombo labeledComboPriority;
        private LabeledCombo labeledComboLoad;
        private System.Windows.Forms.Label lblError;
    }
}
