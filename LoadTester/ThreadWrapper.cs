using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using LoadTester.Annotations;

namespace LoadTester
{
    public class ThreadWrapper :INotifyPropertyChanged
    {
        private Thread m_thread;
        private IntPtr m_threadHandle;
        private volatile bool m_looper;
        private volatile bool m_stopped;
        private volatile bool m_restartLoop;
        private volatile ThreadState m_state;
        private volatile LoadType m_loadType;


        public ThreadWrapper()
        {
            AfinnityArray = new BitArray();
            Afinnity = UInt32.MaxValue;
            AfinnityArray.SetCount(Environment.ProcessorCount);
            AfinnityArray.Changed += OnAfinnityChanged;
            m_stopped = true;
            Reinit();
        }

        private void OnAfinnityChanged(object p_sender, EventArgs p_args)
        {
            SetThreadAffinityMask(m_threadHandle, (UIntPtr) AfinnityArray.Value);
            OnPropertyChanged("Afinnity");
        }

        private void Reinit()
        {
            m_thread = new Thread(ThreadFunc);
            m_threadHandle = (IntPtr) (-1);
        }

        public ulong Afinnity
        {
            get { return AfinnityArray.Value; }
            set
            {
                AfinnityArray.Value = value;
            }
        }

        public BitArray AfinnityArray { get; private set; }
        /*
        public bool[] AfinnityArray
        {
            get { return m_afinnityArray; }
            set
            {
                m_afinnityArray = value;

                uint newAfinnity = 0;
                uint mask = 1;
                for (int i = 0; i < m_afinnityArray.Length; i++)
                {
                    var flag = m_afinnityArray[i];
                    mask = mask << 1;
                    if (flag)
                    {
                        newAfinnity = newAfinnity | mask;
                    }
                }
                Afinnity = newAfinnity;
            }
        }
        */
        public ThreadState State
        {
            get { return m_state; }
            protected set
            {
                m_state = value;
                OnPropertyChanged("State");
            }
        }

        public ThreadPriority Priority
        {
            get { return m_thread.Priority; }
            set { m_thread.Priority = value; }
        }

        public LoadType LoadType
        {
            get { return m_loadType; }
            set
            {
                m_loadType = value;
                m_restartLoop = true;
                Thread.MemoryBarrier();
                m_looper = false;
                GC.Collect();
                GC.Collect();
                OnPropertyChanged( "LoadType" );
            }
        }

        public void Start()
        {
            if (State == ThreadState.Started)
                return;

            Reinit();

            m_thread.Start();
        }

        public void Stop()
        {
            if (State == ThreadState.Stopped)
                return;

            m_looper = false;
            while (false == m_stopped) ;
        }

        protected void ThreadFunc()
        {
            Thread.BeginThreadAffinity();

            m_stopped = false;
            m_looper = true;
            State = ThreadState.Started;

            var threadId = GetCurrentThreadId();
            m_threadHandle =  OpenThread(ThreadAccess.QUERY_INFORMATION | ThreadAccess.SET_INFORMATION, false, threadId);
            SetThreadAffinityMask( m_threadHandle, (UIntPtr)Afinnity );

            Loop:
            switch (m_loadType)
            {
                case LoadType.EmptyLoop:
                    while (m_looper) ;
                    break;

                case LoadType.MemoryPressure:
                    while (m_looper)
                        MemoryPressureFunction();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (m_restartLoop)
            {
                m_looper = true;
                m_restartLoop = false;
                goto Loop;
            }

            m_stopped = true;
            State = ThreadState.Stopped;
            m_threadHandle = (IntPtr) (-1);
            Thread.EndThreadAffinity();
        }

        const int OneGb = 1024 * 1024 * 1024;
        const int s32k = 32 * 1024;

        private void MemoryPressureFunction()
        {
            Size32K[] array1G = new Size32K[OneGb/s32k];
            for (int i = 0; i < array1G.Length; i++)
            {
                var size32K = array1G[i] = new Size32K();
                size32K.Field000.Field000.Field000.Field000 = ulong.MaxValue;
                size32K.Field007 = size32K.Field006 = size32K.Field005 = size32K.Field004 = size32K.Field003 = size32K.Field002 = size32K.Field001 = size32K.Field000;
            }
            GC.Collect();
        }

        [DllImport( "kernel32.dll" )]
        static extern UIntPtr SetThreadAffinityMask( IntPtr hThread,
           UIntPtr dwThreadAffinityMask );
        
        [DllImport( "kernel32.dll" )]
        static extern uint GetCurrentThreadId();

        [DllImport( "kernel32.dll", SetLastError = true )]
        static extern IntPtr OpenThread( ThreadAccess dwDesiredAccess, bool bInheritHandle,
           uint dwThreadId );

        [Flags]
        public enum ThreadAccess :int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string p_propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(p_propertyName));
        }
    }
}
