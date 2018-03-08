using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LoadTester
{
    public class ErrorDisplayingManager
    {
        private DateTime? m_showedErrorTime = null;
        private readonly Label lblError;
        private string m_lastErrorMessage;

        public ErrorDisplayingManager(Label p_lblError)
        {
            lblError = p_lblError;
        }


        public string LastErrorMessage
        {
            get { return m_lastErrorMessage; }
            set
            {
                m_lastErrorMessage = value;
                UpdateErrorState();
            }
        }

        private void UpdateErrorState()
        {
            Action refreshDisplayeErrorAction = RefreshDisplayeError;

            if (false == m_showedErrorTime.HasValue)
            {
                refreshDisplayeErrorAction();
            }
            else
            {
                var errorVisibilityTime = DateTime.Now - m_showedErrorTime.Value;
                if (errorVisibilityTime.TotalMilliseconds < 500)
                {
                    int duration = 500; //in milliseconds
                    Timer timer = new Timer();
                    timer.Interval = duration;

                    timer.Tick += (arg1, arg2) =>
                    {
                        refreshDisplayeErrorAction();
                        timer.Stop();
                        timer.Dispose();
                    };

                    timer.Start();
                }
                else
                {
                    refreshDisplayeErrorAction();
                }
            }
        }


        private void RefreshDisplayeError()
        {
            var lastErrorMessage = LastErrorMessage;
            var errorVisible = false == string.IsNullOrEmpty(lastErrorMessage);

            lblError.Text = lastErrorMessage;
            lblError.Visible = errorVisible;
            m_showedErrorTime = errorVisible ? (DateTime?)DateTime.Now : null;
        }
    }
}
