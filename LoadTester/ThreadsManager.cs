using System.Collections.Generic;

namespace LoadTester
{
    public class ThreadsManager
    {
        private IList<ThreadWrapper> m_threadWrappers;

        public void StartAll()
        {
            foreach (var threadWrapper in m_threadWrappers)
            {
                threadWrapper.Start();
            }
        }

        public void StopAll()
        {
            foreach (var threadWrapper in m_threadWrappers)
            {
                threadWrapper.Stop();
            }
        }

        public void Init()
        {
            this.m_threadWrappers = new List<ThreadWrapper>();
        }

        public void AddThread(ThreadWrapper threadWrapper)
        {
            this.m_threadWrappers.Add(threadWrapper);
        }
    }
}