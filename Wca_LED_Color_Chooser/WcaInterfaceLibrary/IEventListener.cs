using System;
using System.Collections.Generic;
using System.Text;

namespace WcaInterfaceLibrary
{
    public interface IInterfaceListener
    {
        void OnReceive(IInterface sender, byte[] data, int dataLength);
    }
}
