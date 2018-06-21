using System;
using System.Collections.Generic;
using System.Text;

namespace WcaInterfaceLibrary
{
    public interface ICommandBroker
    {
        void RegisterListener<T>(T listener) where T : ICommandListener;

        void UnregisterListener<T>(T listener) where T : ICommandListener;
    }
}
