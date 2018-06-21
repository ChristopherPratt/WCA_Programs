using System;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace WcaInterfaceLibrary
{
    public class SerialInterface : IInterface, IDisposable
    {
        private delegate void m_eventHandler(IInterface sender, byte[] data, int dataLength);
        private event m_eventHandler m_event;

        private SerialPort m_port;
        private string m_name;

        public SerialInterface(string portName, int speed, Parity parity, int numberOfDataBits)
        {
            m_name = portName;
            m_port = new SerialPort(portName, speed, parity, numberOfDataBits, StopBits.One);
            m_port.Handshake = Handshake.None;
            m_port.DataReceived += new SerialDataReceivedEventHandler(m_port_DataReceived);
            m_port.ReadTimeout = 500;
            m_port.WriteTimeout = 500;
            m_port.Encoding = Encoding.ASCII;
            m_port.NewLine = "\r";
        }

        void m_port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] data = new byte[m_port.BytesToRead];
                int noOfBytes = m_port.Read(data, 0, data.Length);

                if (m_event != null)
                {
                    m_event(this, data, noOfBytes);
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public bool Open()
        {
            bool result = false;

            try
            {
                if (!m_port.IsOpen)
                {
                    m_port.Open();
                }
                result = true;
                Debug.WriteLine(String.Format("SerialInterface::Open:Result:{0}", result));
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(String.Format("SerialInterface::Open:Result:{0}", ex.Message));
            }

            return result;
        }

        public bool isOpen()
        {
            bool result = false;

            try
            {
                if (m_port.IsOpen)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(String.Format("SerialInterface::Open:Result:{0}", ex.Message));
            }

            return result;
        }


        public void Close()
        {
            try
            {
                if (m_port.IsOpen)
                {
                    m_port.Close();
                }
                Console.WriteLine(String.Format("SerialInterface::Close:Result:true"));
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(String.Format("SerialInterface::Close:Result:{0}", ex.Message));
            }
        }

        #region IInterface Members

        public bool Send(byte[] data)
        {

            if (m_port.IsOpen && data != null && data.Length > 0)
            {
                m_port.Write(data, 0, data.Length);
                m_port.DiscardOutBuffer();
                return true;
            }

            return false;
        }
        public bool Send(StreamReader sr)
        {
            bool result = false;
            String line;

            if (sr != null)
            {
                try
                {
                    m_port.Handshake = Handshake.XOnXOff;

                    while ((line = sr.ReadLine()) != null)
                    {
                        m_port.WriteLine(line);
                    }
                    result = true;
                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                finally
                {
                    m_port.DiscardOutBuffer();

                    if (sr != null)
                    {
                        sr.Close();
                        sr.Dispose();
                    }
                    
                    m_port.Handshake = Handshake.None;
                }
            }
            return result;
        }

        public void RegisterListener<T>(T listener) where T : IInterfaceListener
        {
            m_event += new m_eventHandler(listener.OnReceive);
        }

        public void UnregisterListener<T>(T listener) where T : IInterfaceListener
        {
            m_event -= new m_eventHandler(listener.OnReceive);
        }

        public string GetName()
        {
            return m_name;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Close();
        }

        #endregion
    }
}
