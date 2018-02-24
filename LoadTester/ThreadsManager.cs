using System;
using System.Collections.Generic;

namespace LoadTester
{
    public static class ThreadsManager
    {
        private static IList<ThreadWrapper> m_threadWrappers;

        private static double[] m_intervals = new double[5 * 1000];

        private static readonly double[][] EmptyDoubles = new double[0][];


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
            int loops = 0;
            double miliseconds = 0;

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

        public static void Init()
        {
            m_threadWrappers = new List<ThreadWrapper>();
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
