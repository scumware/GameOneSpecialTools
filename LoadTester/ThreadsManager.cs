using System.Collections.Generic;

namespace LoadTester
{
    public static class ThreadsManager
    {
        private static IList<ThreadWrapper> m_threadWrappers;

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