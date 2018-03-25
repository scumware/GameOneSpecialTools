using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Win32.Interop;

namespace LoadTester
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            m_errorDisplayingManager = new ErrorDisplayingManager(lblLastError);
            btnFill.Text = "Add " + Environment.ProcessorCount + " threads";

            m_processWrapper = new ProcessWrapper(NativeMethods.GetCurrentProcess());
            m_processWrapper.PropertyChanged += ProcessWrapperOnPropertyChanged;

            cmbProcessPriority.DataSource = ProcessPriorityWrapper.AllValues;
            cmbProcessPriority.SelectedItem = m_previousProcessPriority = ProcessPriorityWrapper.IDLE_PRIORITY_CLASS;

            m_lastProcessPropsUpdateTime = DateTime.Now;

            NativeMethods.DisableProcessWindowsGhosting();
            ThreadsManager.Init();
            UpdateAfinnity();

            timer.Enabled = true;
        }

        protected override void OnShown(EventArgs e)
        {
            NativeMethods.SetThreadPriority(NativeMethods.GetCurrentThread(), ThreadPriority.THREAD_PRIORITY_HIGHEST);

            try
            {
                CurrentProcess.AdjustPrivileges(SecurityEntiryNames.SE_INC_BASE_PRIORITY_NAME, PrivilegeAction.Enable);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this,
                    "REALTIME_PRIORITY_CLASS will not be available!"
                    + Environment.NewLine + Environment.NewLine
                    + "Probably u r not an administrator or just UAC active. Try right click then and run as administrator."
                    + Environment.NewLine + Environment.NewLine
                    + exception.Message,
                    ":(",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Hand);
            }

            base.OnShown(e);
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
            if (p_propertyChangedEventArgs.PropertyName == "LastErrorMessage")
            {
                m_errorDisplayingManager.LastErrorMessage = m_processWrapper.LastErrorMessage;
            }
        }

        private Color[] chartColors = new Color[]{Color.Lime, Color.Red, Color.Yellow, Color.White, Color.BlueViolet, Color.GreenYellow, Color.OrangeRed, Color.Brown, Color.CadetBlue, Color.Aqua, Color.Azure, Color.Blue, Color.Coral, Color.DeepPink, Color.DarkSalmon, Color.Silver};


        private void button1_Click( object sender, EventArgs e )
        {
            /*
            var histogrammForm = new ThreadSamplingHistogramForm();
            histogrammForm.SeriesColor = Color.Wheat;
            histogrammForm.ThreadWrapper = new ThreadWrapperTest();
            histogrammForm.Show(this);
            return;
            */
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
                    threadControl.MinimumSize = new Size(flowLayoutPanel.ClientRectangle.Width - 2, 0);
                    threadControl.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                    threadControl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    threadControl.Margin = new Padding(0);
                    flowLayoutPanel.Controls.Add(threadControl);

                    var seriesColor = AddSeries(threadWrapper);
                    threadControl.SeriesColor = seriesColor;

                    var preferredSize = threadControl.PreferredSize;
                    threadControl.Size = new Size(preferredSize.Width - 2, preferredSize.Height);

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

            ThreadsManager.UpdateAfinnity(m_processWrapper.ProcessAfinnity);
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
                        var times = ThreadsManager.Times;
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

        private Color AddSeries(ThreadWrapper p_threadWrapper)
        {
            this.chart1.Series.SuspendUpdates();
            var colorIndex = this.chart1.Series.Count;
            if (colorIndex >= chartColors.Length)
            {
                colorIndex = 0;
            }

            var originalColor = chartColors[colorIndex];
            Color seriesColor = Color.FromArgb(132, originalColor.R, originalColor.G, originalColor.B);

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

            var times = (double[])ThreadsManager.Times.Clone();
            double previousTime = 0.0;
            for (int index = 0; index < p_threadWrapper.Speeds.Length; index++)
            {
                var value = p_threadWrapper.Speeds[index];
                var time = (double)times[index];
                if (previousTime > time)
                    time = previousTime;

                var dataPointIndex = newSeries.Points.AddXY(time, value);
                previousTime = time;
            }

            chart1.ChartAreas.ResumeUpdates();
            return seriesColor;
        }

        private void chart1_DoubleClick(object sender, EventArgs e)
        {
            timer.Enabled = !timer.Enabled;
        }

        // ReSharper disable once InconsistentNaming
        public const int SYSMENU_ABOUT_ID = 0x1;
        private bool m_refreshAfinnityInprogress;
        private readonly ProcessWrapper m_processWrapper;
        private ProcessPriorityWrapper m_previousProcessPriority;
        private readonly ErrorDisplayingManager m_errorDisplayingManager;

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
/*
            if (newValue == ProcessPriorityWrapper.REALTIME_PRIORITY_CLASS)
            {
                var newTimer = new Timer();
                {
                    var backColor = cmbProcessPriority.BackColor;
                    cmbProcessPriority.BackColor = Color.Red;
                    newTimer.Enabled = true;
                    newTimer.Interval = 1000;
                    newTimer.Tick += (p_sender, p_args) =>
                    {
                        var localTimer = newTimer;

                        cmbProcessPriority.BackColor = backColor;
                        localTimer.Enabled = false;
                        localTimer.Dispose();
                    };
                }

                ProcessWrapper.ShowPriorityWarning(this);
            }
*/
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
