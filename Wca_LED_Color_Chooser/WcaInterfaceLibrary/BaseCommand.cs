using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace WcaInterfaceLibrary
{
    public delegate void resend(string comport, byte command, byte[] data, string name);
    public static class globals
    {
        public static int baud = 9600;
        public static int timeout = 500;
        public static string program = "none";
        public static bool debug = false;
    }
    public abstract class BaseCommand : CommandBroker, ICommand, IInterfaceListener, IDisposable
    {
        //public delegate void resend(string comport, byte command, byte[] data, string name);
        private IInterface m_Interface;
        private int m_Timeout = globals.timeout;
        private string m_Name;
        private bool m_IsTimeout = false;
        private CountdownLatch m_Latch;
        private WcaInterfaceCommandResult m_CommandResult = WcaInterfaceCommandResult.WaitForExecution;
        private byte m_Command;
        private byte m_DestinationAddress;
        private byte[] m_Data;
        private Queue<byte> m_ReceivedDataQueue;
        private List<ProtocolFrame> m_ReceivedTelegrams;
        private ProtocolFrame m_TransmitTelegram;

        protected BaseCommand(IInterface inf, byte destinationAddress, byte cmd)
            : this(inf, destinationAddress, cmd, null, "")
        {
        }

        protected BaseCommand(IInterface inf, byte destinationAddress, byte cmd, byte[] data, string name)
        {
            m_Name = name;
            m_Interface = inf;
            m_Command = cmd;
            m_DestinationAddress = destinationAddress;
            m_Data = data;
            m_Latch = new CountdownLatch(1); //one telegram has to be received 
            m_ReceivedDataQueue = new Queue<byte>();
            m_ReceivedTelegrams = new List<ProtocolFrame>();
            m_TransmitTelegram = null;
        }

        protected IInterface Interface 
        {
            get { return m_Interface; }
        }

        protected void SetData(byte[] data)
        {
            m_Data = data;
        }

        protected void SetCommandType(byte cmd)
        {
            m_Command = cmd;
        }

        public int Timeout
        {
            get { return m_Timeout; }
            set { m_Timeout = value; }
        }

        protected bool IsTimeout
        {
            get
            {
                return m_IsTimeout;
            }
            private set
            {
                m_IsTimeout = value;
            }
        }

        #region ICommand Members

        /// <summary>
        /// This method is called by the Target in a separate thread
        /// </summary>
        public virtual void Execute(resend myRerun)
        {
            m_TransmitTelegram = new ProtocolFrame(m_DestinationAddress, m_Command, m_Data);
            {
                m_Latch.Reset();
                IsTimeout = false;
                m_Interface.RegisterListener(this);
                m_CommandResult = WcaInterfaceCommandResult.InProgress;

                m_Interface.Send(m_TransmitTelegram.Telegram);
                try
                {
                    if (!m_Latch.WaitOne(Timeout))
                    {
                        //timeout 
                        IsTimeout = true;
                        m_CommandResult = WcaInterfaceCommandResult.ExecutionTimeout;

                        if (myRerun != null) { myRerun(m_Interface.GetName(), m_Command, m_Data, m_Name); }
                    }
                    else if (Validate(m_ReceivedTelegrams))
                    {
                        m_CommandResult = WcaInterfaceCommandResult.PosAck;
                    }
                    else
                    {
                        m_CommandResult = WcaInterfaceCommandResult.NegAck;
                        if (myRerun != null) { myRerun(m_Interface.GetName(), m_Command, m_Data, m_Name); }
                    }

                    m_Latch.Reset();
                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                m_Interface.UnregisterListener(this);

                NotifyListener(this);
            }
        }

        protected abstract bool Validate(List<ProtocolFrame> rt);

       
        #endregion

        #region IEventListener Members

        public virtual void OnReceive(IInterface sender, byte[] data, int dataLength)
        {
            ProtocolFrame rf;
            
            foreach (byte b in data)
            {
                m_ReceivedDataQueue.Enqueue(b);
            }
            do 
            {
                rf = ProtocolFrame.AnalyzeQueue(ref m_ReceivedDataQueue);

                if (rf != null)
                {
                    m_ReceivedTelegrams.Add(rf);
                }
            } while (rf != null);

            if (m_ReceivedTelegrams.Count>0)
            {
                m_Latch.SignalAll();
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            m_Latch.SignalAll();
        }

        #endregion

        public override string ToString()
        {
            string s = "";
          
            if (m_TransmitTelegram != null)
            {
                s += ":S:" + Converter.ByteArray2String(m_TransmitTelegram.Telegram);
            }

            foreach (ProtocolFrame pf in m_ReceivedTelegrams)
            {
                s += ":R:" + Converter.ByteArray2String(pf.Telegram);
            }

            s += ":" + m_CommandResult.ToString();
          
            if (IsTimeout)
            {
                s += ":TO:";
            }
            return s;
        }

        #region ICommand Members

        public byte Command { get { return m_Command; } } // retrieve command byte

        public WcaInterfaceCommandResult Result { get { return m_CommandResult; } } // retrieve whether pos ack or neg ack

        public string Name { get { return m_Name; } } // description of command

        public string comport { get { return m_Interface.GetName(); } } //retrieve comport command was sent on

        public byte[] getData // return the first element of list byte array. (most of our commands will only respond with 1 byte set - so we won't need anything past the first element)
        {
            get
            {                
                return m_ReceivedTelegrams[0].Telegram;
            }
        }
        public bool isReceive { get // check to see if the notification is for recieving a message from device
            {
                if (m_ReceivedTelegrams.Count > 0)
                {
                    return true;
                }
                else return false;
            }
        }

        #endregion
    }
}
