using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcaInterfaceLibrary;

namespace WcaFlashApplicationManaged
{
    public class Device
    {
        private WcaInterface m_pInterface;
        private byte m_target_address;

        public Device(byte target_address)
        {
            m_target_address = target_address;
        }
        public bool Initialize(string pCOMPort, int baudrate)
        {
            return true;
        }

        public void DeInitialize()
        {

        }

        public WcaInterfaceCommandResult SendCommandSync(byte command, out byte[] prdata, out int written)
        {
            written = 0;
            prdata = new byte[1];

            return WcaInterfaceCommandResult.NegAck;
        }

        public WcaInterfaceCommandResult SendCommandSync_2(byte command, byte[] data, int length, out byte[] prdata, out int written)
        {
            written = 0;
            prdata = new byte[1];

            return WcaInterfaceCommandResult.NegAck;
        }

        public WcaInterfaceCommandResult SetOutput(byte port, byte pin, byte value)
        {
            return WcaInterfaceCommandResult.NegAck;
        }

        public WcaInterfaceCommandResult ReadInput(byte port, byte pin, out byte value)
        {
            value = 0;
            return WcaInterfaceCommandResult.NegAck;
        }
    }
}
