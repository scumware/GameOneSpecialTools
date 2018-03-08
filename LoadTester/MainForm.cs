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
    public partial class MainForm :Form
    {
        public MainForm()
        {
            InitializeComponent();
            btnFill.Text = "Add " + Environment.ProcessorCount + " threads";

            m_processWrapper = new ProcessWrapper(NativeMethods.GetCurrentProcess());
            m_processWrapper.PropertyChanged += ProcessWrapperOnPropertyChanged;

            cmbProcessPriority.DataSource = ProcessPriorityWrapper.AllValues;
            cmbProcessPriority.SelectedItem = m_previousProcessPriority = ProcessPriorityWrapper.IDLE_PRIORITY_CLASS;

            m_lastProcessPropsUpdateTime = DateTime.Now;
            UpdateAfinnity();

            NativeMethods.DisableProcessWindowsGhosting();
            ThreadsManager.Init();
        }

        private void ProcessWrapperOnPropertyChanged(object p_sender, PropertyChangedEventArgs p_propertyChangedEventArgs)
        {
            if (p_propertyChangedEventArgs.PropertyName == "Priority")
            {
                //labeledComboPriority.Combo.SelectedItem = m_wrapper.Priority;
            }
            if (p_propertyChangedEventArgs.PropertyName == "ProcessAfinnity")
            {
                UpdateAfinnity();
            }
        }

        private Color[] chartColors = new Color[]{Color.Lime, Color.Red, Color.Yellow, Color.White, Color.BlueViolet, Color.GreenYellow, Color.OrangeRed, Color.Brown, Color.CadetBlue, Color.Aqua, Color.Azure, Color.Blue, Color.Coral, Color.DeepPink, Color.DarkSalmon, Color.Silver};


        private void button1_Click( object sender, EventArgs e )
        {
            //m_threadWrappers 
            int count;
            if (sender == btnFill)
            {
                count = Environment.ProcessorCount;
            }
            else
            {
                count = 1;
            }
            IList<ThreadWrapper> newWrappers = ThreadsManager.AddThreads(count);

            flowLayoutPanel.Parent.SuspendLayout();
            flowLayoutPanel.SuspendLayout();
            try
            {

                foreach (var threadWrapper in newWrappers)
                {
                    ThreadControl threadControl = new ThreadControl();
                    threadControl.Wrapper = threadWrapper;

                    flowLayoutPanel.Controls.Add(threadControl);
                    AddSeries(threadWrapper);

                    threadControl.Anchor = AnchorStyles.Left;
                    threadControl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    threadControl.AutoSize = true;

                    threadControl.Visible = true;
                }

            }
            finally
            {
                flowLayoutPanel.Parent.ResumeLayout(false);
                flowLayoutPanel.ResumeLayout();
            }
        }

        private void btnStart_Click( object sender, EventArgs e )
        {
            ThreadsManager.StartAll();
        }

        private void btnStop_Click( object sender, EventArgs e )
        {
            ThreadsManager.StopAll();
        }

        private void MainForm_FormClosing( object sender, FormClosingEventArgs e )
        {
            ThreadsManager.FinishWork();
            ThreadsManager.StopAll();
        }


        private void UpdateAfinnity()
        {
            if (m_refreshAfinnityInprogress)
                return;

            m_refreshAfinnityInprogress = true;

            var cleared = false;
            this.SuspendLayout();
            var checkBoxes = flowPanelAfinnity.Controls.OfType<CheckBox>().ToArray();

            var count = m_processWrapper.ProcessAfinnityArray.Count;
            if (count != checkBoxes.Length)
            {
                flowPanelAfinnity.Controls.Clear();
                cleared = true;
            }
            flowPanelAfinnity.SuspendLayout();
            for (int index = 0; index < count; index++)
            {
                var checkBox = cleared ? new CheckBox() : checkBoxes[index];

                var flag = m_processWrapper.ProcessAfinnityArray[index];
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
            var index = (int)checkBox.Tag;

            m_processWrapper.ProcessAfinnityArray[index] = checkBox.Checked;
        }


        private DateTime m_lastProcessPropsUpdateTime;
        private void timer_Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            var timeSpan = now - m_lastProcessPropsUpdateTime;
            if (timeSpan.TotalMilliseconds>= 1000)
            {
                m_processWrapper.UpdateBySystem();
                m_lastProcessPropsUpdateTime = now;
            }

            UpdateChart();
        }

        private void UpdateChart()
        {
            this.chart1.Series.SuspendUpdates();
            foreach (var threadWrapper in ThreadsManager.ThreadWrappers)
            {
                foreach (Series series in chart1.Series)
                {
                    if (series.Tag == threadWrapper)
                    {
                        var speeds = (double[])threadWrapper.Speeds.Clone();
                        var times = (long[])ThreadsManager.Times.Clone();
                        double previousTime = 0.0;

                        for (int pointIdex = 0; pointIdex < series.Points.Count; pointIdex++)
                        {
                            var point = series.Points[pointIdex];
                            var speed = speeds[pointIdex];
                            point.YValues[0] = speed;

                            var time = (double)times[pointIdex];
                            if (previousTime > time)
                            {
                                time = previousTime;
                            }


                            point.XValue = time;
                            previousTime = time;
                        }
                    }
                }
            }
            chart1.ChartAreas.ResumeUpdates();
            chart1.ChartAreas[0].RecalculateAxesScale();
        }

        private void AddSeries(ThreadWrapper p_threadWrapper)
        {
            this.chart1.Series.SuspendUpdates();
            var colorIndex = this.chart1.Series.Count;
            if (colorIndex >= chartColors.Length)
            {
                colorIndex = 0;
            }

            var originalColor = chartColors[colorIndex];
            Color seriesColor = Color.FromArgb(196, originalColor.R, originalColor.G, originalColor.B);

            var newSeries = new Series
            {
                BorderColor = Color.White,
                BorderDashStyle = ChartDashStyle.Solid,
                ChartArea = chart1.ChartAreas[0].Name,
                ChartType = SeriesChartType.FastLine,
                Color = seriesColor,
                LabelBackColor = Color.Maroon,
                LabelForeColor = Color.Lime,
                Legend = "Legend1",
                Name = "Thread +" + p_threadWrapper.ThreadId,
                Tag = p_threadWrapper
            };
            this.chart1.Series.Add(newSeries);

            chart1.ChartAreas.SuspendUpdates();

            var times = (long[])ThreadsManager.Times.Clone();
            double previousTime = 0.0;
            for (int index = 0; index < p_threadWrapper.Speeds.Length; index++)
            {
                var value = p_threadWrapper.Speeds[index];
                var time = (double)times[index];
                if (previousTime > time)
                    time = previousTime;

                var dataPoint = newSeries.Points.AddXY(time, value);
                previousTime = time;
            }

            chart1.ChartAreas.ResumeUpdates();
        }

        private void chart1_DoubleClick(object sender, EventArgs e)
        {
            timer.Enabled = !timer.Enabled;
        }

        // ReSharper disable once InconsistentNaming
        private int SYSMENU_ABOUT_ID = 0x1;
        private bool m_refreshAfinnityInprogress;
        private readonly ProcessWrapper m_processWrapper;
        private ProcessPriorityWrapper m_previousProcessPriority;

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // Get a handle to a copy of this form's system (window) menu
            IntPtr hSysMenu = NativeMethods.GetSystemMenu(this.Handle, false);

            // Add a separator
            NativeMethods.AppendMenu(hSysMenu, NativeMethods.MF_SEPARATOR, 0, string.Empty);

            // Add the About menu item
            NativeMethods.AppendMenu(hSysMenu, NativeMethods.MF_STRING, SYSMENU_ABOUT_ID, "&About…");
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // Test if the About item was selected from the system menu
            if ((m.Msg == NativeMethods.WM_SYSCOMMAND) && ((int)m.WParam == SYSMENU_ABOUT_ID))
            {
                var aboutDlg = new AboutForm();
                aboutDlg.Show(this);
                {
                    var x = Location.X + (Width - aboutDlg.Width) / 2;
                    var y = Location.Y + (Height - aboutDlg.Height) / 2;
                    aboutDlg.Location = new Point(Math.Max(x, 0), Math.Max(y, 0));
                }
            }
        }

        private void cmbProcessPriority_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var newValue = (ProcessPriorityWrapper)cmbProcessPriority.SelectedItem;
            var operationResult = m_processWrapper.SetPriority(newValue.Value);
            if (false == operationResult)
            {
                cmbProcessPriority.SelectedItem = m_previousProcessPriority;
            }
            else
            {
                m_previousProcessPriority = newValue;
            }
        }

        private void chkBackgroundMode_CheckedChanged(object sender, EventArgs e)
        {
            bool operationResult = m_processWrapper.SetBackgroundMode(chkBackgroundMode.Checked);
            if (false == operationResult)
            {
                chkBackgroundMode.Checked = !chkBackgroundMode.Checked;
            }
        }
    }
}
