using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LoadTester
{
    public partial class ThreadControl :UserControl
    {
        private ThreadWrapper m_wrapper;

        public ThreadControl()
        {
            InitializeComponent();
            BackColor = System.Drawing.Color.FromArgb(32,10,20,0);

            labeledComboPriority.Combo.ValueMember = "Value";
            labeledComboPriority.Combo.DisplayMember = "DisplayName";
            labeledComboPriority.Combo.DataSource = ThreadPriorityWrapper.AllValues;

            labeledComboPriority.Combo.SelectedItem = ThreadPriority.THREAD_PRIORITY_LOWEST;

            labeledComboLoad.Combo.DataSource = Enum.GetValues( typeof( LoadType ) );
            labeledComboLoad.Combo.SelectedItem = LoadType.EmptyLoop;
            this.ResetBackColor();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if (disposing)
            {
                if (m_wrapper != null)
                {
                    m_wrapper.PropertyChanged -= ThreadWrapperPropertyChanged;
                }
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        public void UpdateStateView(ThreadState value)
        {
            switch (value)
            {
                case ThreadState.Started:
                    pictureBox1.Image = Properties.Resources.fire32;
                    btnStartStop.Text = "Stop";
                    break;

                case ThreadState.Stopped:
                    pictureBox1.Image = Properties.Resources._32px_Sert___dead_smile_svg;
                    btnStartStop.Text = "Start";
                    break;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ThreadWrapper Wrapper
        {
            get { return m_wrapper; }
            set
            {
                m_wrapper = value;
                m_wrapper.PropertyChanged += ThreadWrapperPropertyChanged;
                RefreshAll();
            }
        }

        private void ThreadWrapperPropertyChanged(object p_sender, PropertyChangedEventArgs p_propertyChangedEventArgs)
        {
            if (InvokeRequired)
            {
                var action = new Action(()=>ThreadWrapperPropertyChanged(p_sender, p_propertyChangedEventArgs));
                BeginInvoke(action);
                return;
            }

            if (string.IsNullOrEmpty(p_propertyChangedEventArgs.PropertyName))
            {
                RefreshAll();
                return;
            }

            if (p_propertyChangedEventArgs.PropertyName == "State")
            {
                UpdateStateView(m_wrapper.State);
                return;
            }
            if (p_propertyChangedEventArgs.PropertyName == "Priority")
            {
                labeledComboPriority.Combo.SelectedItem = m_wrapper.Priority;
            }
            if (p_propertyChangedEventArgs.PropertyName == "LoadType")
            {
                labeledComboLoad.Combo.SelectedItem = m_wrapper.LoadType;
            }

        }

        private void RefreshAll()
        {
            UpdateStateView(m_wrapper.State);

            if (m_wrapper.AfinnityArray.Count != flowPanelAfinnity.Controls.OfType<CheckBox>().Count())
            {
                flowPanelAfinnity.Controls.Clear();
            }
            this.SuspendLayout();
            flowPanelAfinnity.SuspendLayout();
            for (int index = m_wrapper.AfinnityArray.Count - 1; index >= 0; index--)
            {
                var flag = m_wrapper.AfinnityArray[index];
                var checkBox = new CheckBox();
                checkBox.AutoSize = true;
                checkBox.Checked = flag;
                checkBox.BackColor = Color.Transparent;
                checkBox.ForeColor = Color.YellowGreen;
                checkBox.Text = "CPU " + index;
                checkBox.Tag = index;
                checkBox.CheckStateChanged += CheckBoxOnCheckStateChanged;
                flowPanelAfinnity.Controls.Add( checkBox );
                checkBox.Visible = true;
            }
            flowPanelAfinnity.ResumeLayout(false);
            this.ResumeLayout();

            labeledComboLoad.Combo.SelectedItem = m_wrapper.LoadType;
            labeledComboPriority.Combo.SelectedItem = m_wrapper.Priority;
        }

        private void CheckBoxOnCheckStateChanged(object p_sender, EventArgs p_eventArgs)
        {
            var checkBox = p_sender as CheckBox;
            var index = (int) checkBox.Tag;

            m_wrapper.AfinnityArray[index] = checkBox.Checked;
        }

        private void btnStartStop_Click( object sender, EventArgs e )
        {
            switch (m_wrapper.State)
            {
                case ThreadState.Stopped:
                    m_wrapper.Start();
                    break;

                case ThreadState.Started:
                    m_wrapper.Stop();
                    break;
            }
        }

        private void labeledComboPriority_SelectedValueChanged( object sender, EventArgs e )
        {
            if (null == m_wrapper) return;
            m_wrapper.Priority = (ThreadPriority) labeledComboPriority.Combo.SelectedValue;
        }

        private void labeledComboLoad_SelectedValueChanged( object sender, EventArgs e )
        {
            if (null == m_wrapper) return;
            m_wrapper.LoadType = (LoadType)labeledComboLoad.Combo.SelectedItem;
        }
    }
}
