using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

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

                case ThreadState.Suspended:
                    pictureBox1.Image = Properties.Resources.sleeping;
                    btnStartStop.Text = "Resume";
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
            if (p_propertyChangedEventArgs.PropertyName == "Afinnity")
            {
                UpdateAfinnity();
            }
            if (p_propertyChangedEventArgs.PropertyName == "LastErrorMessage")
            {
                UpdateErrorState();
            }
        }

        private DateTime? m_showedErrorTime = null;
        private void UpdateErrorState()
        {
            if (m_showedErrorTime.HasValue)
            {
                var errorVisibilityTime = DateTime.Now - m_showedErrorTime.Value;
                if (errorVisibilityTime.TotalMilliseconds < 500)
                {
                    int duration = 500; //in milliseconds
                    Timer timer = new Timer();
                    timer.Interval = duration;

                    timer.Tick += (arg1, arg2) =>
                    {
                        RefreshDisplayeError();
                        timer.Stop();
                            timer.Dispose();
                    };

                    timer.Start();
                }
                else
                {
                    RefreshDisplayeError();
                }
            }
            else
            {
                RefreshDisplayeError();
            }
        }

        private void RefreshDisplayeError()
        {
            var lastErrorMessage = m_wrapper.LastErrorMessage;
            var errorVisible = false == string.IsNullOrEmpty(lastErrorMessage);

            lblError.Text = lastErrorMessage;
            lblError.Visible = errorVisible;
            m_showedErrorTime = errorVisible ? (DateTime?) DateTime.Now : null;
        }

        private void RefreshAll()
        {
            UpdateStateView(m_wrapper.State);

            UpdateAfinnity();

            labeledComboLoad.Combo.SelectedItem = m_wrapper.LoadType;
            labeledComboPriority.Combo.SelectedItem = m_wrapper.Priority;
            UpdateErrorState();
        }

        private bool m_refreshAfinnityInprogress = false;
        private void UpdateAfinnity()
        {
            if (m_refreshAfinnityInprogress)
                return;

            m_refreshAfinnityInprogress = true;

            var cleared = false;
            this.SuspendLayout();
            var checkBoxes = flowPanelAfinnity.Controls.OfType<CheckBox>().ToArray();

            var count = m_wrapper.AfinnityArray.Count;
            if (count != checkBoxes.Length)
            {
                flowPanelAfinnity.Controls.Clear();
                cleared = true;
            }
            flowPanelAfinnity.SuspendLayout();
            for (int index = count - 1; index >= 0; index--)
            {
                var checkBox = cleared ? new CheckBox() : checkBoxes[index];

                var flag = m_wrapper.AfinnityArray[index];
                checkBox.Checked = flag;

                if (cleared)
                {
                    checkBox.AutoSize = true;
                    checkBox.BackColor = Color.Transparent;
                    checkBox.ForeColor = Color.YellowGreen;
                    checkBox.Text = "CPU " + (count - index - 1);
                    checkBox.Tag = index;
                    checkBox.CheckStateChanged += CheckBoxOnCheckStateChanged;
                    flowPanelAfinnity.Controls.Add(checkBox);
                    checkBox.Visible = true;
                }
            }
            flowPanelAfinnity.ResumeLayout(false);
            this.ResumeLayout();

            m_refreshAfinnityInprogress = false;
        }

        private void CheckBoxOnCheckStateChanged(object p_sender, EventArgs p_eventArgs)
        {
            if (m_refreshAfinnityInprogress)
                return;

            m_refreshAfinnityInprogress = true;
            var checkBox = p_sender as CheckBox;
            var index = (int) checkBox.Tag;

            m_wrapper.AfinnityArray[index] = checkBox.Checked;
            m_refreshAfinnityInprogress = false;
        }

        private void btnStartStop_Click( object sender, EventArgs e )
        {
            switch (m_wrapper.State)
            {
                case ThreadState.Stopped:
                case ThreadState.Suspended:
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

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            var clone = (BitArray)m_wrapper.AfinnityArray.Clone();
            for (int i = 0; i < clone.Count; i++)
                clone[i] = false;

            var number = clone.Count-1;
            clone[number] = true;
            m_wrapper.Afinnity = clone.Value;
        }
    }
}
