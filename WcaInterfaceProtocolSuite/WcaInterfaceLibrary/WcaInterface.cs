using System;
using System.Runtime.InteropServices;
using System.IO.Ports;
using System.Diagnostics;
using System.Collections.Generic;

namespace WcaInterfaceLibrary
{
    [Guid("a893509a-405d-47aa-8612-073064a123d2"),
        ClassInterface(ClassInterfaceType.None),
        ComSourceInterfaces(typeof(IWcaInterfaceEvents))]
    [ComVisible(true)]
    public class WcaInterface : IWcaInterface
    {
        private SerialInterface m_SerialInterface = null;
        private Target m_Target = null;
        private bool m_Initilized = false;

        public WcaInterface()
        {
            
        }


        #region IWcaInterface Members

        public bool Open(WcaInterfaceType type, string name, int baudrate)
        {
            if (!m_Initilized)
            {
                Debug.WriteLine(String.Format("WcaInterface::Open:{0}:{1}:{2}", type.ToString(), name, baudrate));

                switch (type)
                {
                    case WcaInterfaceType.UART:
                        m_SerialInterface = new SerialInterface(name, baudrate, Parity.None, 8);
                        m_Initilized = m_SerialInterface.Open();

                        Debug.WriteLine(String.Format("WcaInterface::Open:Result:{0}", m_Initilized));

                        if (m_Initilized)
                        {
                            m_Target = new Target();
                        }
                        break;

                    case WcaInterfaceType.USB:
                        break;
                }
            }
            return m_Initilized;
        }

        public void Close()
        {
            if (m_Initilized)
            {
                m_SerialInterface.Close();
                m_Target.Dispose();
                m_Initilized = false;
            }
        }

        public WcaInterfaceCommandResult SendCommandSync(WcaInterfaceAddress destination, byte cmd, out byte[] receivedData)
        {
            return SendCommandSync(destination, cmd, null, out receivedData);
        }

        public WcaInterfaceCommandResult SendCommandSync(WcaInterfaceAddress destination, byte cmd, byte[] data, out byte[] receivedData)
        {
          //  byte result = 0;
            receivedData = null;
            WcaInterfaceCommandResult result = WcaInterfaceCommandResult.ExecutionTimeout;

            if (m_Initilized)
            {
                GeneralCommand gcmd = new GeneralCommand(m_SerialInterface, 0x01, cmd, data,"");
                m_Target.Queue(gcmd);
                m_Target.Wait();

                if (gcmd.Result == WcaInterfaceCommandResult.PosAck)
                {
                    receivedData = gcmd.ReveivedData;
                }/*
                else if (gcmd.Result == WcaInterfaceCommandResult.NegAck)
                {
                    result = 2;
                   // receivedData = gcmd.ReveivedData;
                }
                else
                {
                    result = 3;
                }*/
                result = gcmd.Result;

            }
            return result;
        }
       

        public bool SendCommandASync(WcaInterfaceAddress destination, byte cmd)
        {
            return false;
        }

        public bool SendCommandASync(WcaInterfaceAddress destination, byte cmd, byte[] data)
        {
            return false;
        }

        public string GetVersion()
        {
            string ver = String.Format("WcaInterfaceLibrary.Version: {0} / {1} / {2}.", WcaInterfaceLibrary.Version.GetAssemblyName(),
                    WcaInterfaceLibrary.Version.GetAssemblyVersionNumber(),
                    WcaInterfaceLibrary.Version.GetAssemblyCodeBase());

            return ver;
        }

        #endregion
    }
}
