using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadTester
{
    public class ThreadPriorityWrapper
    {
        public static readonly ThreadPriorityWrapper[] AllValues;

        public ThreadPriorityWrapper(ThreadPriority p_value)
        {
            Value = p_value;
        }

        public ThreadPriority Value { get; private set; }
        public string DisplayName { get { return ToString(); } }

        public override string ToString()
        {
            var result = GetString() + "\t\t "+((int)Value).ToString("X4");
            return result;
        }

        private string GetString()
        {
            switch (Value)
            {
                case ThreadPriority.THREAD_PRIORITY_IDLE:
                    return "IDLE";
                case ThreadPriority.THREAD_PRIORITY_LOWEST:
                    return "LOWEST";
                case ThreadPriority.THREAD_PRIORITY_BELOW_NORMAL:
                    return "BELOW_NORMAL";
                case ThreadPriority.THREAD_PRIORITY_NORMAL:
                    return "NORMAL";
                case ThreadPriority.THREAD_PRIORITY_ABOVE_NORMAL:
                    return "ABOVE_NORMAL";
                case ThreadPriority.THREAD_PRIORITY_HIGHEST:
                    return "HIGHEST";
                case ThreadPriority.THREAD_PRIORITY_TIME_CRITICAL:
                    return "TIME_CRITICAL";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        static ThreadPriorityWrapper()
        {
            AllValues = new ThreadPriorityWrapper[Enum.GetValues(typeof(ThreadPriority)).Length];
            AllValues[0] = new ThreadPriorityWrapper(ThreadPriority.THREAD_PRIORITY_IDLE);
            AllValues[1] = new ThreadPriorityWrapper(ThreadPriority.THREAD_PRIORITY_LOWEST);
            AllValues[2] = new ThreadPriorityWrapper(ThreadPriority.THREAD_PRIORITY_BELOW_NORMAL);
            AllValues[3] = new ThreadPriorityWrapper(ThreadPriority.THREAD_PRIORITY_NORMAL);
            AllValues[4] = new ThreadPriorityWrapper(ThreadPriority.THREAD_PRIORITY_ABOVE_NORMAL);
            AllValues[5] = new ThreadPriorityWrapper(ThreadPriority.THREAD_PRIORITY_HIGHEST);
            AllValues[6] = new ThreadPriorityWrapper(ThreadPriority.THREAD_PRIORITY_TIME_CRITICAL);
        }
    }
}
