using System;
using System.Collections;
using System.Collections.Generic;

namespace LoadTester
{
    
    public class BitArray :IList<bool>
    {
        public void SetCount( int p_count )
        {
            m_count = p_count;
        }

        private int m_count;
        private ulong m_value = UInt64.MaxValue;

        public int IndexOf(bool item)
        {
            throw new NotSupportedException();
        }

        public void Insert(int index, bool item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public bool this[int index]
        {
            get
            {
                unchecked
                {
                    ulong mask = ((ulong)1L) << index;
                    var result = m_value & mask;
                    return result > 0;
                }
            }

            set
            {
                ulong mask = ((ulong)1L) << index;
                if (value)
                {
                    m_value = m_value | mask;
                }
                else
                {
                    m_value = m_value & ~mask;
                }
                if (Changed != null)
                {
                    Changed(this, EventArgs.Empty);
                }
            }
        }

        public EventHandler Changed;


        public ulong Value
        {
            get { return m_value; }
            set
            {
                m_value = value;
                if (Changed != null)
                {
                    Changed(this, EventArgs.Empty);
                }
            }
        }

        #region Implementation of IEnumerable

        public IEnumerator<bool> GetEnumerator()
        {
            for (int i = 0; i < m_count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<bool>

        public void Add(bool item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(bool item)
        {
            throw new NotSupportedException();
        }

        public void CopyTo(bool[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        public bool Remove(bool item)
        {
            throw new NotSupportedException();
        }

        public int Count
        {
            get { return m_count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        #endregion
    }
}