using System;
using System.Collections.Generic;
using System.Text;

namespace WcaInterfaceLibrary
{
    public interface ICommandListener
    {
        void OnNotification(ICommand sender);
    }
}
