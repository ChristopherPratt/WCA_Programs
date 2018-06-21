using System;
using System.Collections.Generic;
using System.Text;
using WcaInterfaceLibrary;

namespace WcaDVConsole
{
    abstract public class State : ICommandListener, IInterfaceListener
    {
        private DeviceContext m_DeviceContext;

        public State(DeviceContext context)
        {
            m_DeviceContext = context;
        }

        protected DeviceContext DeviceContext
        {
            get { return m_DeviceContext; }
        }

        public abstract void Trigger();

        #region ICommandListener Members

        public abstract void OnNotification(ICommand sender);

        #endregion

        #region IInterfaceListener Members

        public abstract void OnReceive(IInterface sender, byte[] data, int dataLength);

        #endregion
    }
}
