using System;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;

namespace LoadTester
{
    [SuppressMessage("ReSharper", "LocalizableElement")]
    [SuppressMessage("ReSharper", "ArrangeThisQualifier")]
    [SuppressMessage("ReSharper", "UseNullPropagation")]
    public sealed partial class ThreadControl :UserControl
    {
        private ThreadWrapper m_wrapper;
        private readonly ErrorDisplayingManager m_errorDisplayingManager;

        public ThreadControl()
        {
            InitializeComponent();
            // this.labeledComboPriority.Combo.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));

            m_errorDisplayingManager = new ErrorDisplayingManager(lblError);

           // BackColor = Color.FromArgb(32,10,20,0);

            labeledComboPriority.Combo.ValueMember = "Value";
            labeledComboPriority.Combo.DisplayMember = "DisplayName";
            labeledComboPriority.Combo.DataSource = ThreadPriorityWrapper.AllValues;

            labeledComboPriority.Combo.SelectedItem = ThreadPriority.THREAD_PRIORITY_LOWEST;

            labeledComboLoad.Combo.DataSource = Enum.GetValues( typeof( LoadType ) );
            labeledComboLoad.Combo.SelectedItem = LoadType.EmptyLoop;
            //this.ResetBackColor();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="p_disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool p_disposing )
        {
            if (p_disposing)
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
            base.Dispose( p_disposing );
        }

        public void UpdateStateView(ThreadState p_value)
        {
            switch (p_value)
            {
                case ThreadState.Started:
                    picState.Image = Properties.Resources.fire32;
                    btnStartStop.Text = "Stop";
                    break;

                case ThreadState.Suspended:
                    picState.Image = Properties.Resources.sleeping;
                    btnStartStop.Text = "Resume";
                    break;

                case ThreadState.Stopped:
                    picState.Image = Properties.Resources._32px_Sert___dead_smile_svg;
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

        public Color SeriesColor { get; set; }

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
                m_errorDisplayingManager.LastErrorMessage = m_wrapper.LastErrorMessage;
            }
        }


        private void RefreshAll()
        {
            UpdateStateView(m_wrapper.State);

            UpdateAfinnity();

            labeledComboLoad.Combo.SelectedItem = m_wrapper.LoadType;
            labeledComboPriority.Combo.SelectedItem = m_wrapper.Priority;
            m_errorDisplayingManager.LastErrorMessage = m_wrapper.LastErrorMessage;
        }

        private bool m_refreshAfinnityInprogress;

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
            for (int index = 0; index < count; index++)
            {
                var checkBox = cleared ? new CheckBox() : checkBoxes[index];

                var flag = m_wrapper.AfinnityArray[index];
                checkBox.Checked = flag;

                if (cleared)
                {
                    checkBox.AutoSize = true;
                    checkBox.BackColor = Color.Transparent;
                    checkBox.ForeColor = Color.DarkGoldenrod;
                    checkBox.Text = "CPU " + index;
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

            var checkBox = p_sender as CheckBox;
#if DEBUG
            Debug.Assert(checkBox != null, "checkBox != null");
#endif
            var index = (int) checkBox.Tag;

            m_wrapper.AfinnityArray[index] = checkBox.Checked;
        }

        private void btnStartStop_Click( object p_sender, EventArgs p_eventArgs)
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

        private void labeledComboPriority_SelectedValueChanged( object p_sender, EventArgs p_eventArgs )
        {
            if (null == m_wrapper) return;
            if(ThreadPriority.THREAD_PRIORITY_TIME_CRITICAL == (ThreadPriority) labeledComboPriority.Combo.SelectedValue)
                ProcessWrapper.ShowPriorityWarning(this);

            m_wrapper.Priority = (ThreadPriority) labeledComboPriority.Combo.SelectedValue;
        }

        private void labeledComboLoad_SelectedValueChanged( object p_sender, EventArgs p_eventArgs )
        {
            if (null == m_wrapper) return;
            m_wrapper.LoadType = (LoadType)labeledComboLoad.Combo.SelectedItem;
        }

        private void pictureBox1_DoubleClick(object p_sender, EventArgs p_eventArgs)
        {
            var clone = (BitArray)m_wrapper.AfinnityArray.Clone();
            for (int i = 0; i < clone.Count; i++)
                clone[i] = false;

            var number = clone.Count-1;
            clone[number] = true;
            m_wrapper.Afinnity = clone.Value;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var histogrammForm = new ThreadSamplingHistogramForm();
            histogrammForm.SeriesColor = SeriesColor;
            histogrammForm.ThreadWrapper = m_wrapper;
            histogrammForm.Show(this);
        }
    }
}
