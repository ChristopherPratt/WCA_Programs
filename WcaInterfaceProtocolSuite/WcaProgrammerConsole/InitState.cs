using System;
using System.Collections.Generic;
using System.Text;

namespace WcaDVConsole
{
    public class InitState : State
    {
        public InitState(DeviceContext context)
            :base(context)
        {

        }



        public override void OnNotification(WcaInterfaceLibrary.ICommand sender)
        {
            
        }

        public override void OnReceive(WcaInterfaceLibrary.IInterface sender, byte[] data, int dataLength)
        {
            
        }

        public override void Trigger()
        {
            
        }
    }
}
