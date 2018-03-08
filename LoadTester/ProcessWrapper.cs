using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using LoadTester.Annotations;

namespace LoadTester
{
    public class ProcessWrapper :INotifyPropertyChanged
    {
        private string m_lastErrorMessage;
        private UInt32 m_previousAfinnity;
        private readonly IntPtr m_processHandle;

        public UInt32 ProcessAfinnity
        {
            get { return (uint) ProcessAfinnityArray.Value; }
            set { ProcessAfinnityArray.Value = value; }
        }

        public BitArray ProcessAfinnityArray { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public string LastErrorMessage
        {
            get { return m_lastErrorMessage; }
            set
            {
                m_lastErrorMessage = value;
                OnPropertyChanged("LastErrorMessage");
            }
        }


        public ProcessWrapper(IntPtr p_processHandle)
        {
            m_processHandle = p_processHandle;
            ProcessAfinnityArray = new BitArray();
            ProcessAfinnityArray.SetCount(Environment.ProcessorCount);
            ProcessAfinnityArray.Changed += OnAfinnityChanged;

            UpdateBySystem();
        }

        public void UpdateBySystem()
        {
            UIntPtr lpProcessAffinityMask;
            UIntPtr lpSystemAffinityMask;
            NativeMethods.GetProcessAffinityMask(m_processHandle, out lpProcessAffinityMask, out lpSystemAffinityMask);

            // ReSharper disable once DelegateSubtraction
            ProcessAfinnityArray.Changed -= OnAfinnityChanged;
            ProcessAfinnity = lpProcessAffinityMask.ToUInt32();
            ProcessAfinnityArray.Changed += OnAfinnityChanged;

            OnPropertyChanged("ProcessAfinnity");
        }

        private void OnAfinnityChanged(object p_sender, EventArgs p_args)
        {
            SetAfinnityMask();
            OnPropertyChanged("ProcessAfinnity");
        }


        private void SetAfinnityMask()
        {
            UInt64 afinnityArrayValue = ProcessAfinnityArray.Value;
            afinnityArrayValue &= UInt32.MaxValue;

            var result = NativeMethods.SetProcessAffinityMask(m_processHandle, (UIntPtr)afinnityArrayValue);

            var lastError = Marshal.GetLastWin32Error();
            if (lastError != 0)
            {
                LastErrorMessage = new Win32Exception(lastError).Message;

                // ReSharper disable once DelegateSubtraction
                ProcessAfinnityArray.Changed -= OnAfinnityChanged;
                ProcessAfinnity = m_previousAfinnity;
                OnPropertyChanged("ProcessAfinnity");
                ProcessAfinnityArray.Changed += OnAfinnityChanged;
            }
            else
            {
                m_previousAfinnity = (UInt32)afinnityArrayValue;
                LastErrorMessage = string.Empty;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string p_propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(p_propertyName));
        }

        public bool SetPriority(NativeMethods.PriorityClass p_newPriorityClass)
        {
            NativeMethods.SetPriorityClass(m_processHandle, p_newPriorityClass);
            var lastError = Marshal.GetLastWin32Error();
            if (lastError != 0)
            {
                LastErrorMessage = new Win32Exception(lastError).Message;
                return false;
            }
            else
            {
                PriorityClass = p_newPriorityClass;
                LastErrorMessage = string.Empty;
                return true;
            }
        }

        private NativeMethods.PriorityClass m_priorityClass;
        private bool m_backgroundMode;

        public NativeMethods.PriorityClass PriorityClass
        {
            get { return m_priorityClass; }
            set
            {
                m_priorityClass = value;
                OnPropertyChanged("PriorityClass");
            }
        }

        public bool SetBackgroundMode(bool p_mode)
        {
            if (p_mode == m_backgroundMode)
                return true;

            NativeMethods.PriorityClass newPriorityClass;
            if (p_mode)
            {
                newPriorityClass = NativeMethods.PriorityClass.PROCESS_MODE_BACKGROUND_BEGIN;
            }
            else
            {
                newPriorityClass = NativeMethods.PriorityClass.PROCESS_MODE_BACKGROUND_END;
            }

            NativeMethods.SetPriorityClass(m_processHandle,
                                           newPriorityClass);
            var lastError = Marshal.GetLastWin32Error();
            if (lastError != 0)
            {
                LastErrorMessage = new Win32Exception(lastError).Message;
                return false;
            }
            else
            {
                m_backgroundMode = p_mode;
                LastErrorMessage = string.Empty;
                return true;
            }
        }
    }
}
