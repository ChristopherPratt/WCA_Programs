using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace WcaInterfaceLibrary
{
    public class CountdownLatch
    {
        private int m_remain;
        private readonly int m_startcount;
        private EventWaitHandle m_event;

        public CountdownLatch(int count)
        {
            m_startcount = count;
            m_remain = count;
            m_event = new ManualResetEvent(false);
        }

        public void Signal()
        {
            // The last thread to signal also sets the event.
            if (Interlocked.Decrement(ref m_remain) == 0)
                m_event.Set();
        }

        public void SignalAll()
        {
            Interlocked.Exchange(ref m_remain,0);
            m_event.Set();
        }

        /// <summary>
        /// Wait until countdown reaches zero or a timeout occurs.
        /// </summary>
        /// <returns>true - latch reaches zero, false - timeout</returns>
        public bool WaitOne(int timeout)
        {
            return m_event.WaitOne(timeout);
        }

        public void Reset()
        {
            m_remain = m_startcount;
            m_event.Reset();
        }

        public int Count
        {
            get
            {
                return m_startcount;
            }
        }
    }
}
