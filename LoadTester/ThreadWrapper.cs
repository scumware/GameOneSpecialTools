using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using LoadTester.Annotations;

namespace LoadTester
{
    public class ThreadWrapper : INotifyPropertyChanged, IDisposable
    {
        public double[] Speeds;

        public delegate void LoopAction([MarshalAs(UnmanagedType.Bool)] ref bool p_flag);

        private static readonly LoopAction s_pause;
        private volatile LoadType m_loadType;
        private long m_looped;
        private bool m_looper;
        private ThreadPriority m_priority;
        private volatile bool m_restartLoop;
        private volatile ThreadState m_state;
        private volatile bool m_stopped;
        private IntPtr m_threadHandle;
        private UInt32 m_threadId;

        /// <summary>
        /// Magic! Do not remove
        /// </summary>
        private ThreadStart m_threadFunctionDelegeteReference;

        private bool m_disposed;

        static ThreadWrapper()
        {
            s_pause = GetPauseDelegate();
        }

        public const int countLastMeasurement = 5000;

        public ThreadWrapper()
        {
            Speeds = new double[5000];
            for (int i = 0; i < countLastMeasurement; i++)
            {
                Speeds[i] = -1;
            }

            AfinnityArray = new BitArray();
            Afinnity = UInt32.MaxValue;
            AfinnityArray.SetCount(Environment.ProcessorCount);
            AfinnityArray.Changed += OnAfinnityChanged;

            Reinit();

            Priority = ThreadPriority.THREAD_PRIORITY_LOWEST;
            m_loadType = LoadType.Sleep;
            State = ThreadState.Stopped;
            CreateNativeThread(true);
        }

        public ulong Afinnity
        {
            get { return AfinnityArray.Value; }
            set { AfinnityArray.Value = value; }
        }

        public BitArray AfinnityArray { get; private set; }


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
            get { return m_priority; }
            set
            {
                m_priority = value;
                RestartLoop();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RestartLoop()
        {
            if (false == m_stopped)
            {
                m_restartLoop = true;
                Thread.MemoryBarrier();
                m_looper = false;
            }
        }

        public
            LoadType LoadType
        {
            get { return m_loadType; }
            set
            {
                m_loadType = value;
                RestartLoop();

                OnPropertyChanged("LoadType");
            }
        }

        public long Looped
        {
            get { return m_looped; }
            set
            {
                Thread.MemoryBarrier();
                m_looped = value;
            }
        }

        public uint ThreadId
        {
            get { return m_threadId; }
            private set
            {
                m_threadId = value;
                OnPropertyChanged("ThreadId");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public static LoopAction GetPauseDelegate()
        {
            // @formatter:off
            byte[] codeBytes2 = new byte[]
            {

                0x90                //nop
                , 0xF3, 0x90        //f390   pause
                , 0x80, 0x3a, 0x00  //803a00 cmp     byte ptr [rdx],0
                , 0x75, 0xF9,       //75F9   jne ->pause command
                0xc3                //c3     ret
            };
            // @formatter:on

            var code = codeBytes2;

            var codePointer = NativeMethods.VirtualAlloc(
                IntPtr.Zero,
                new UIntPtr((uint) code.Length),
                NativeMethods.AllocationType.COMMIT | NativeMethods.AllocationType.RESERVE,
                NativeMethods.MemoryProtection.EXECUTE_READWRITE
            );

            Marshal.Copy(code, 0, codePointer, code.Length);

            var cpuPauseDelgate =
                (LoopAction) Marshal.GetDelegateForFunctionPointer(codePointer, typeof(LoopAction));

            return cpuPauseDelgate;
        }

        private void OnAfinnityChanged(object p_sender, EventArgs p_args)
        {
            NativeMethods.SetThreadAffinityMask(m_threadHandle, (UIntPtr) AfinnityArray.Value);
            OnPropertyChanged("Afinnity");
        }

        private void Reinit()
        {
            m_threadHandle = (IntPtr) (-1);
            m_stopped = true;
        }

        public void Start()
        {
            if (State == ThreadState.Started)
                return;

            if (State == ThreadState.Stopped)
            {
                Reinit();

                CreateNativeThread(false);
            }
            else
            {
                NativeMethods.ResumeThread((uint) m_threadHandle);
            }
        }

        private void CreateNativeThread(bool p_createSuspended)
        {
            uint lpThreadId;
            m_threadHandle = (IntPtr) StartThread(ThreadFunc, out lpThreadId, 0, p_createSuspended);
            ThreadId = lpThreadId;
        }


        unsafe uint StartThread(ThreadStart p_threadFunc, out uint p_lpThreadId, int StackSize = 0, bool p_createSuspended = false)
        {
            m_threadFunctionDelegeteReference = p_threadFunc;

            uint i = 0;
            uint* lpParam = &i;

            var dwCreationFlags = NativeMethods.ThreadCreationFlags.CREATE_NORMAL;
            if (p_createSuspended)
            {
                dwCreationFlags |= NativeMethods.ThreadCreationFlags.CREATE_SUSPENDED;
            }
            uint dwHandle = NativeMethods.CreateThread(null, (uint) StackSize, p_threadFunc, lpParam, dwCreationFlags, out p_lpThreadId);
            if (dwHandle == 0)
                throw new Exception("Unable to create thread!");

            if (p_createSuspended)
            {
                State = ThreadState.Suspended;
            }
            return dwHandle;
        }

        public void Stop(bool p_wait = true)
        {
            switch (State)
            {
                case ThreadState.Stopped:
                    return;

                case ThreadState.Suspended:
                    m_looper = m_restartLoop = false;

                    NativeMethods.ResumeThread((uint) m_threadHandle);
                    while (p_wait  &&  (false == m_looper)) 
                        Thread.SpinWait(10);
                    break;

                case ThreadState.Started:
                    m_looper = false;
                    while (p_wait && (false == m_stopped))
                        Thread.SpinWait(10);
                    break;
            }
        }

        protected void ThreadFunc()
        {
            m_stopped = false;
            State = ThreadState.Started;

            NativeMethods.SetThreadAffinityMask(m_threadHandle, (UIntPtr) Afinnity);
            Thread.BeginCriticalRegion();
            do
            {
                NativeMethods.SetThreadPriority(m_threadHandle, m_priority);

                m_looper = true;
                Thread.MemoryBarrier();
                m_restartLoop = false;

                switch (m_loadType)
                {
                    case LoadType.YieldExecution:
                        while (m_looper)
                        {
                            NativeMethods.SwitchToThread();
                            Interlocked.Increment(ref m_looped);
                        }
                        break;

                    case LoadType.Sleep:
                        while (m_looper)
                        {
                            Thread.Sleep(1);
                            Interlocked.Increment(ref m_looped);
                        }
                        break;

                    case LoadType.EmptyLoop:
                        while (m_looper)
                            Interlocked.Increment(ref m_looped);
                        break;

                    case LoadType.SpinWait:
                        // while (m_looper.Value) PAUSE
                        //not implemented Interlocked.Increment(ref m_looped);
                        s_pause(ref m_looper);
                        break;

                    case LoadType.MemoryPressure:
                        while (m_looper)
                        {
                            MemoryPressureFunction();
                            Interlocked.Increment(ref m_looped);
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            } while (m_restartLoop);

            State = ThreadState.Stopped;
            Thread.EndCriticalRegion();

            NativeMethods.CloseHandle(m_threadHandle);
            Reinit();
        }

        private void MemoryPressureFunction()
        {
            Size32K[] array1G = new Size32K[GeneralConstants.OneGb/GeneralConstants.S32K];
            for (int i = 0; i < array1G.Length; i++)
            {
                var size32K = array1G[i] = new Size32K();
                size32K.Field000.Field000.Field000.Field000 = ulong.MaxValue;
                size32K.Field007
                    = size32K.Field006
                        = size32K.Field005
                            = size32K.Field004
                                = size32K.Field003
                                    = size32K.Field002
                                        = size32K.Field001
                                            = size32K.Field000;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string p_propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(p_propertyName));
        }

        public void Dispose()
        {
            if (m_disposed)
                return;

            GC.SuppressFinalize(this);
            m_disposed = true;
            Stop();
        }
    }
}