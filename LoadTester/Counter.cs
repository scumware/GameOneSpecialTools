using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WindowsFormsApplication28
{
    public struct Counter
    {
        private long m_counter;
        private long m_started;

        public long Count {get { return m_counter; }}

        public void Begin()
        {
            if (m_started > 0)
                return;
            ++m_started ;
            ++m_counter;
        }

        public void End()
        {
            --m_started;
        }
    }
}
