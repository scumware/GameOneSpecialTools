using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadTester
{
    public class ProcessPriorityWrapper
    {
        public static readonly ProcessPriorityWrapper[] AllValues;
        public static readonly ProcessPriorityWrapper IDLE_PRIORITY_CLASS;
        public static readonly ProcessPriorityWrapper NORMAL_PRIORITY_CLASS;
        public static ProcessPriorityWrapper REALTIME_PRIORITY_CLASS;

        public ProcessPriorityWrapper(NativeMethods.PriorityClass p_value)
        {
            Value = p_value;
        }

        public NativeMethods.PriorityClass Value { get; private set; }
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
                case NativeMethods.PriorityClass.ABOVE_NORMAL_PRIORITY_CLASS:
                    return "Above normal";

                case NativeMethods.PriorityClass.BELOW_NORMAL_PRIORITY_CLASS:
                    return "Below normal";

                case NativeMethods.PriorityClass.HIGH_PRIORITY_CLASS:
                    return "High";

                case NativeMethods.PriorityClass.IDLE_PRIORITY_CLASS:
                    return "Idle";

                case NativeMethods.PriorityClass.NORMAL_PRIORITY_CLASS:
                    return "Normal";

                case NativeMethods.PriorityClass.PROCESS_MODE_BACKGROUND_BEGIN:
                    return "BACKGROUND_MODE (New for Windows Vista)";

                case NativeMethods.PriorityClass.PROCESS_MODE_BACKGROUND_END:
                    return "MODE_BACKGROUND_END";

                case NativeMethods.PriorityClass.REALTIME_PRIORITY_CLASS:
                    return "REALTIME (Please, just do not kill me)";

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        static ProcessPriorityWrapper()
        {
            AllValues = new ProcessPriorityWrapper[Enum.GetValues(typeof(NativeMethods.PriorityClass)).Length - 2];
            /*
;            PROCESS_MODE_BACKGROUND_END = new ProcessPriorityWrapper(NativeMethods.PriorityClass.PROCESS_MODE_BACKGROUND_END);
;            BACKGROUND_BEGIN = new ProcessPriorityWrapper(NativeMethods.PriorityClass.PROCESS_MODE_BACKGROUND_BEGIN);
            */
            AllValues[0] = IDLE_PRIORITY_CLASS = new ProcessPriorityWrapper(NativeMethods.PriorityClass.IDLE_PRIORITY_CLASS); ;
            AllValues[1] = new ProcessPriorityWrapper(NativeMethods.PriorityClass.BELOW_NORMAL_PRIORITY_CLASS);
            AllValues[2] = NORMAL_PRIORITY_CLASS = new ProcessPriorityWrapper(NativeMethods.PriorityClass.NORMAL_PRIORITY_CLASS);;
            AllValues[3] = new ProcessPriorityWrapper(NativeMethods.PriorityClass.ABOVE_NORMAL_PRIORITY_CLASS);
            AllValues[4] = new ProcessPriorityWrapper(NativeMethods.PriorityClass.HIGH_PRIORITY_CLASS);
            AllValues[5] = REALTIME_PRIORITY_CLASS = new ProcessPriorityWrapper(NativeMethods.PriorityClass.REALTIME_PRIORITY_CLASS); ;
        }
    }
}
