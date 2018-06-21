using System;
using System.Collections.Generic;
using System.Text;
using WcaInterfaceLibrary;

namespace WcaCommandLibrary
{
    public class GPIOControlCommand : BaseCommand
    {
        public GPIOControlCommand(IInterface inf, byte destAddress,byte cmd, byte[] data, string name)
            :base(inf,destAddress,cmd,data,name)
        {

        }

        protected override bool Validate(List<ProtocolFrame> rt)
        {
            bool result = false;

            foreach (ProtocolFrame pf in rt)
            {
                if (pf.DestinationAddress == ProtocolFrame.MyAddress &&
                    pf.Command == this.Command &&
                    pf.Code == (byte)ProtocolFrameType.POS_ACK)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
