namespace LoadTester
{
    partial class LabeledCombo
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainTable = new DoubleBufferedTableLayoutPanel();
            this.label = new System.Windows.Forms.Label();
            this.comboBox = new System.Windows.Forms.ComboBox();
            this.mainTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTable
            // 
            this.mainTable.AutoSize = true;
            this.mainTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mainTable.ColumnCount = 2;
            this.mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mainTable.Controls.Add(this.label, 0, 0);
            this.mainTable.Controls.Add(this.comboBox, 1, 0);
            this.mainTable.Location = new System.Drawing.Point(0, 0);
            this.mainTable.Name = "mainTable";
            this.mainTable.RowCount = 1;
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainTable.Size = new System.Drawing.Size(167, 27);
            this.mainTable.TabIndex = 0;
            // 
            // label
            // 
            this.label.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(3, 7);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(35, 13);
            this.label.TabIndex = 0;
            this.label.Text = "label1";
            // 
            // comboBox
            // 
            this.comboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Location = new System.Drawing.Point(44, 3);
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new System.Drawing.Size(120, 21);
            this.comboBox.TabIndex = 1;
            this.comboBox.SelectedValueChanged += new System.EventHandler(this.comboBox_SelectedValueChanged);
            // 
            // LabeledCombo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainTable);
            this.Name = "LabeledCombo";
            this.Size = new System.Drawing.Size(434, 150);
            this.mainTable.ResumeLayout(false);
            this.mainTable.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DoubleBufferedTableLayoutPanel mainTable;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.ComboBox comboBox;
    }
}
