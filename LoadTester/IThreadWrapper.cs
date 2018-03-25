namespace LoadTester
{
    public interface IThreadWrapper
    {
        double[] GetSpeeds();
        uint ThreadId { get; }
    }

    public class ThreadWrapperTest :IThreadWrapper
    {
        private double[] m_speeds;

        public ThreadWrapperTest()
        {
            var length = 10;
            m_speeds = new double[length];
            for (int i = 0; i < length / 2; i++)
            {
                m_speeds[i] = 1;
            }

            for (int i = length / 2; i < length; i++)
            {
                m_speeds[i] = 10;
            }
        }

        public double[] GetSpeeds()
        {
            return m_speeds;
        }

        public uint ThreadId { get; }
    }

}