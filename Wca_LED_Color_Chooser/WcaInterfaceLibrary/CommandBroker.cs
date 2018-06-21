using System;
using System.Collections.Generic;
using System.Text;

namespace WcaInterfaceLibrary
{
    public abstract class CommandBroker : ICommandBroker
    {
        private delegate void m_eventHandler(ICommand sender);
        private event m_eventHandler m_event = null;

        public CommandBroker()
        {

        }

        protected void NotifyListener(ICommand sender)
        {
            if (m_event != null)
            {
                m_event(sender);
            }
        }

        public void RegisterListener<T>(T listener) where T : ICommandListener
        {
            m_event += new m_eventHandler(listener.OnNotification);
        }
        public void UnregisterListener<T>(T listener) where T : ICommandListener
        {
            m_event -= new m_eventHandler(listener.OnNotification);
        }
    }
}
