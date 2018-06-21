using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Collections;

namespace WcaInterfaceLibrary
{

 
    public class Target : IDisposable
    {
        public resend myResend;
        private Thread m_thread;
        private ManualResetEvent m_queueEvent;
        private ManualResetEvent m_finishedEvent;
        private bool m_abort = false;
        private ConcurrentQueue<ICommand> m_Commands;

        public Target()
        {
            
            m_Commands = new ConcurrentQueue<ICommand>();
            m_queueEvent = new ManualResetEvent(false);
            m_finishedEvent = new ManualResetEvent(false);
            m_thread = new Thread(new ThreadStart(Run));
            m_thread.IsBackground = true;
            m_thread.Start();
        }
        public Target(resend inResend)
        {
            myResend = inResend;
            m_Commands = new ConcurrentQueue<ICommand>();
            m_queueEvent = new ManualResetEvent(false);
            m_finishedEvent = new ManualResetEvent(false);
            m_thread = new Thread(new ThreadStart(Run));
            m_thread.IsBackground = true;
            m_thread.Start();
        }

        public void Queue(ICommand cmd)
        {
            m_Commands.Enqueue(cmd);
            m_finishedEvent.Reset();
            m_queueEvent.Set();
        }

        public void Wait()
        {
            m_finishedEvent.WaitOne();
        }

        private void Run()
        {
            while (!m_abort)
            {
                m_queueEvent.WaitOne();
                while (m_Commands.Count>0 && !m_abort)
                {
                    ICommand cmd = m_Commands.Dequeue();
                    cmd.Execute(myResend);
                }
                m_queueEvent.Reset();
                m_finishedEvent.Set();
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_abort = true;
            m_queueEvent.Set();
            m_finishedEvent.Set();
        }
        #endregion
    }
}
