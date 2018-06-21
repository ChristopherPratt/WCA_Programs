using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WcaInterfaceLibrary
{
    public interface IInterface
    {
        bool Send(byte[] data);

        bool Send(StreamReader sr);
        
        void RegisterListener<T>(T listener) where T : IInterfaceListener;
        
        void UnregisterListener<T>(T listener) where T : IInterfaceListener;
        
        string GetName();
    }
}
