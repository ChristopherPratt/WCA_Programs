using System;
using System.Collections.Generic;
using System.Text;
using WcaInterfaceLibrary;

namespace WcaInterfaceLibrary
{
    public class GeneralCommand : BaseCommand
    {
        private byte[] m_receivedData = null;

        public GeneralCommand(IInterface inf, byte destAddress,byte cmd, byte[] data, string name)
            :base(inf,destAddress,cmd,data,name)
        {

        }

        public byte[] ReveivedData { get { return m_receivedData; } }

        protected override bool Validate(List<ProtocolFrame> rt)
        {
            bool result = false;

            foreach (ProtocolFrame pf in rt)
            {
                if (pf.DestinationAddress == ProtocolFrame.MyAddress &&
                    pf.Command == this.Command &&
                    pf.Code == (byte)ProtocolFrameType.POS_ACK)
                {
                    m_receivedData = pf.Data;
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
