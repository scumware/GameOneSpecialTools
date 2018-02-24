using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Xml;

namespace LoadTester
{
    public static class ThreadsManager
    {
        private const int Period = 4;
        private static IList<ThreadWrapper> m_threadWrappers;

        private static double[] m_intervals = new double[5 * 1000];

        private static readonly double[][] EmptyDoubles = new double[0][];
        private static IntPtr s_waitableTimer;
        private static volatile bool m_disposed;


        public static double[] Intervals
        {
            get { return m_intervals; }
        }

        public static IList<ThreadWrapper> ThreadWrappers
        {
            get { return m_threadWrappers; }
        }

        public static void StartAll()
        {
            foreach (var threadWrapper in m_threadWrappers)
            {
                threadWrapper.Start();
            }
        }

        public static void StopAll()
        {
            foreach (var threadWrapper in m_threadWrappers)
            {
                threadWrapper.Stop();
            }
        }

        public static void FinishWork()
        {
            m_disposed = true;
        }

        public static void Init()
        {
            m_threadWrappers = new List<ThreadWrapper>();

            s_waitableTimer = NativeMethods.CreateWaitableTimer(default(IntPtr), false, "ThreadManager");

            m_stopwatch.Restart();
            var lPeriod = Period;
            long pDueTime = lPeriod;
            NativeMethods.SetWaitableTimer(s_waitableTimer, ref pDueTime, lPeriod, null, IntPtr.Zero, false);

            for (int index = 0; index < m_intervals.Length; index++)
                m_intervals[index] = Period;

            var thread = new Thread(
                UpdateSpeeds);
            thread.Start();
            thread.Priority = System.Threading.ThreadPriority.Highest;
        }

        private static Stopwatch m_stopwatch = new Stopwatch();
        private static void UpdateSpeeds()
        {
            var intervalIndex = 0;
            long milliseconds;
            while (false == m_disposed)
            {
                m_stopwatch.Restart();
                NativeMethods.WaitForSingleObject(s_waitableTimer, NativeMethods.INFINITE);

                milliseconds = m_stopwatch.ElapsedMilliseconds;
                if (milliseconds < Period)
                    continue;

                m_intervals[intervalIndex] = milliseconds;
                ++intervalIndex;
                bool needShifting = false;
                if (intervalIndex == m_intervals.Length)
                {
                    intervalIndex = m_intervals.Length-1;
                    needShifting = true;
                }
                ShiftArrayValues(m_intervals);

                foreach (var threadWrapper in ThreadWrappers)
                {
                    double threadWrapperLooped = threadWrapper.Looped;
                    threadWrapper.Speeds[intervalIndex] = threadWrapperLooped/milliseconds;
                    ShiftArrayValues(threadWrapper.Speeds);
                    threadWrapper.Looped = 0;
                }
            }
            NativeMethods.CloseHandle(s_waitableTimer);
        }

        private static void ShiftArrayValues<T>(T[] p_array)
        {
            for (int i = 1; i < p_array.Length; i++)
            {
                var interval = p_array[i];
                p_array[i - 1] = interval;
            }
        }

        public static ThreadWrapper[] AddThreads(int p_count)
        {
            var result = new ThreadWrapper[p_count];
            for (int i = 0; i < p_count; i++)
            {
                 
                var newThreadWrapper = new ThreadWrapper();
                result[i] = newThreadWrapper;
                m_threadWrappers.Add(newThreadWrapper);
            }
            return result;
        }
    }
}
