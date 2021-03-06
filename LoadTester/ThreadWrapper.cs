﻿using System;
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
    public class ThreadWrapper : INotifyPropertyChanged, IDisposable, IThreadWrapper
    {
        public double[] Speeds;
        public long LoopedPreviously;

        public unsafe delegate void LoopAction(long p1, byte* p_flag);

        private static readonly LoopAction s_pause;
        private volatile LoadType m_loadType;
        private long m_looped;
        private volatile bool m_looper;
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
        private string m_lastErrorMessage;
        private UInt32 m_previousAfinnity;

        static unsafe ThreadWrapper()
        {
            s_pause = GetPauseDelegate();
        }

        public ThreadWrapper()
        {
            Speeds = new double[ThreadsManager.LastMeasurementsCount];
            for (int i = 0; i < ThreadsManager.LastMeasurementsCount; i++)
            {
                Speeds[i] = 0;
            }

            AfinnityArray = new BitArray();
            AfinnityArray.SetCount(Environment.ProcessorCount);
            AfinnityArray.Changed += OnAfinnityChanged;

            Reinit();

            Priority = ThreadPriority.THREAD_PRIORITY_LOWEST;
            m_loadType = LoadType.Sleep;
            State = ThreadState.Stopped;
            CreateNativeThread(true);
        }

        public double[] GetSpeeds()
        {
            var speeds = (double[])Speeds.Clone();
            return speeds;
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

#if !Net40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif

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

        public string LastErrorMessage
        {
            get { return m_lastErrorMessage; }
            set
            {
                m_lastErrorMessage = value;
                OnPropertyChanged("LastErrorMessage");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public static unsafe LoopAction GetPauseDelegate()
        {
            // @formatter:off
            byte[] codeBytes2 = new byte[]
            {

                0x90 //nop
                //0xcc                //int 3
                , 0xF3, 0x90 //f390   pause
                , 0x80, 0x3a, 0x00 //803a00 cmp     byte ptr [rdx],0
                , 0x75, 0xF9, //75F9   jne ->pause command
                0xc3 //c3     ret
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
            SetAfinnityMask();
            OnPropertyChanged("Afinnity");
        }

        private void SetAfinnityMask()
        {
            UInt64 afinnityArrayValue = AfinnityArray.Value;
            afinnityArrayValue &= UInt32.MaxValue;

            var result = NativeMethods.SetThreadAffinityMask(m_threadHandle, (UIntPtr) afinnityArrayValue);

            var lastError = Marshal.GetLastWin32Error();
            if (lastError != 0)
            {
                LastErrorMessage = new Win32Exception(lastError).Message;

                // ReSharper disable once DelegateSubtraction
                AfinnityArray.Changed -= OnAfinnityChanged;
                Afinnity = m_previousAfinnity;
                OnPropertyChanged("Afinnity");
                AfinnityArray.Changed += OnAfinnityChanged;
            }
            else
            {
                m_previousAfinnity = (UInt32) afinnityArrayValue;
                LastErrorMessage = string.Empty;
            }
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
            m_previousAfinnity = (UInt32) NativeMethods.SetThreadAffinityMask(m_threadHandle, (UIntPtr) UInt32.MaxValue);
            Afinnity = m_previousAfinnity;
        }


        unsafe uint StartThread(ThreadStart p_threadFunc, out uint p_lpThreadId, int StackSize = 0,
            bool p_createSuspended = false)
        {
            m_threadFunctionDelegeteReference = p_threadFunc;

            uint i = 0;
            uint* lpParam = &i;

            var dwCreationFlags = NativeMethods.ThreadCreationFlags.CREATE_NORMAL;
            if (p_createSuspended)
            {
                dwCreationFlags |= NativeMethods.ThreadCreationFlags.CREATE_SUSPENDED;
            }
            uint dwHandle = NativeMethods.CreateThread(null, (uint) StackSize, p_threadFunc, lpParam, dwCreationFlags,
                out p_lpThreadId);
            var lastWin32Error = Marshal.GetLastWin32Error();
            if (dwHandle == 0)
            {
                var win32Exception = new Win32Exception(lastWin32Error);
                LastErrorMessage = win32Exception.Message;
                throw win32Exception;
            }
            else
            {
                LastErrorMessage = string.Empty;
            }

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
                    NativeMethods.ResumeThread((uint) m_threadHandle);
                    var lastError = Marshal.GetLastWin32Error();
                    if (lastError != 0)
                    {
                        LastErrorMessage = new Win32Exception(lastError).Message;
                    }
                    else
                    {
                        LastErrorMessage = string.Empty;
                    }
                    StopAliveThreadLoopAndWait(p_wait);
                    break;

                case ThreadState.Started:
                    StopAliveThreadLoopAndWait(p_wait);
                    break;
            }
        }

        private void StopAliveThreadLoopAndWait(bool p_wait)
        {
            while (p_wait && (false == m_stopped))
            {
                m_restartLoop = false;
                Thread.MemoryBarrier();
                m_looper = false;
                Thread.SpinWait(10);
            }
        }

        protected void ThreadFunc()
        {
            m_stopped = false;
            State = ThreadState.Started;

            SetAfinnityMask();

            Thread.BeginCriticalRegion();
            int lastError;
            do
            {
                NativeMethods.SetThreadPriority(m_threadHandle, m_priority);
                lastError = Marshal.GetLastWin32Error();
                if (lastError != 0)
                {
                    LastErrorMessage = new Win32Exception(lastError).Message;
                }
                else
                {
                    LastErrorMessage = string.Empty;
                }

                m_looper = true;
                Thread.MemoryBarrier();
                m_restartLoop = false;

                switch (m_loadType)
                {
                    case LoadType.YieldExecution:
                        while (m_looper)
                        {
                            NativeMethods.SwitchToThread();
                            ++m_looped;
                            Thread.MemoryBarrier();
                        }
                        break;

                    case LoadType.Sleep:
                        while (m_looper)
                        {
                            Thread.Sleep(0);

                            ++m_looped;
                            Thread.MemoryBarrier();
                        }
                        break;

                    case LoadType.EmptyLoop:
                        while (m_looper)
                        {
                            ++m_looped;
                            Thread.MemoryBarrier();
                        }
                        break;

                    case LoadType.PrepareSampleHistogramm50k:
                    {
                        SampleHistogrammDataFactory dataFactory = new SampleHistogrammDataFactory(500);
                        while (m_looper)
                        {
                            ++m_looped;
                            Thread.MemoryBarrier();

                            var sortedUniques = dataFactory.GetSortedUniques(TestData.TestDoubles50k);
                            var histagrammValues = dataFactory.GetHistagrammValues(sortedUniques);
                            GC.KeepAlive(histagrammValues);
                        }
                    }
                        break;

                    case LoadType.SpinWait:
                        // while (m_looper.Value) PAUSE
                        //not implemented Interlocked.Increment(ref m_looped);
                        unsafe
                        {
                            fixed (void* v = &m_looper)
                            {
                                s_pause(0, (byte*) v);
                            }
                        }

                        break;

                    case LoadType.MemoryPressure:
                        while (m_looper)
                        {
                            MemoryPressureFunction();
                            {
                                ++m_looped;
                                Thread.MemoryBarrier();
                            }
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            } while (m_restartLoop);

            State = ThreadState.Stopped;
            Thread.EndCriticalRegion();

            NativeMethods.CloseHandle(m_threadHandle);
            lastError = Marshal.GetLastWin32Error();
            if (lastError != 0)
            {
                LastErrorMessage = new Win32Exception(lastError).Message;
            }
            else
            {
                LastErrorMessage = string.Empty;
            }

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

        public void UpdateAfinnityByProcess(uint p_processAfinnity)
        {
            var newAfinnity =  Afinnity;
            if (newAfinnity == p_processAfinnity)
                return;

            var previousAfinnity = newAfinnity;

            newAfinnity &= p_processAfinnity;
            if (newAfinnity == 0)
                newAfinnity = p_processAfinnity;

            if (newAfinnity == previousAfinnity)
                return;

            Afinnity = newAfinnity;
        }
    }
}