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
            var result = GetString()/* + "\t\t "+((int)Value).ToString("X4")*/;
            return result;
        }

        private string GetString()
        {
            switch (Value)
            {
                case ThreadPriority.THREAD_PRIORITY_IDLE:
                    return "Idle";
                case ThreadPriority.THREAD_PRIORITY_LOWEST:
                    return "Lowest";
                case ThreadPriority.THREAD_PRIORITY_BELOW_NORMAL:
                    return "Below normal";
                case ThreadPriority.THREAD_PRIORITY_NORMAL:
                    return "Normal";
                case ThreadPriority.THREAD_PRIORITY_ABOVE_NORMAL:
                    return "Above normal";
                case ThreadPriority.THREAD_PRIORITY_HIGHEST:
                    return "Highest";
                case ThreadPriority.THREAD_PRIORITY_TIME_CRITICAL:
                    return "TIME_CRITICAL     (I hope u r know that u r doing.";
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
