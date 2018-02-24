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
                if (intervalIndex == m_intervals.Length)
                    intervalIndex = 0;

                foreach (var threadWrapper in ThreadWrappers)
                {
                    double threadWrapperLooped = threadWrapper.Looped;
                    threadWrapper.Speeds[intervalIndex] = threadWrapperLooped/milliseconds;
                    threadWrapper.Looped = 0;
                }
            }
            NativeMethods.CloseHandle(s_waitableTimer);
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
