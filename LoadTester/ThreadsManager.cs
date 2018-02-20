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

        public static void AddThread(ThreadWrapper threadWrapper)
        {
            m_threadWrappers.Add(threadWrapper);
        }
    }
}