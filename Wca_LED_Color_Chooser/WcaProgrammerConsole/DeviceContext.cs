using System;
using System.Collections.Generic;
using System.Text;
using WcaInterfaceLibrary;
using System.IO.Ports;
using System.IO;

namespace WcaDVConsole
{


    public class DeviceContext : ICommandListener, IInterfaceListener
    {
        private SerialInterface m_if_progbox;
        private SerialInterface m_if_charger;
        private Target m_target_progbox;
        private Target m_target_charger;
        private State m_state = null;
        private bool m_initialized = false;

        public DeviceContext()
        {
           
        }

        public State State
        {
            get { return m_state; }
            set { m_state = value; }
        }

        public bool Initialize(string port_progbox, string port_charger)
        {
            bool result = true;

            if (!m_initialized)
            {
                ProgboxIf = new SerialInterface(port_progbox, globals.baud, Parity.None, 8);
                ChargerIf = new SerialInterface(port_charger, globals.baud, Parity.None, 8);
                Progbox = new Target();
                Charger = new Target();
                ProgboxIf.RegisterListener(this);
                ChargerIf.RegisterListener(this);
                

                if(!ProgboxIf.Open() || !ChargerIf.Open())
                {
                    result = false;
                    ProgboxIf.Close();
                    ChargerIf.Close();
                    ProgboxIf.Dispose();
                    ChargerIf.Dispose();
                }
                else
                {
                    m_initialized = true;
                }
            }
            return result;
        }

        public void DeInitialize()
        {
            if (m_initialized)
            {
                ProgboxIf.Close();
                ChargerIf.Close();
                ProgboxIf.Dispose();
                ChargerIf.Dispose();
                m_initialized = false;
            }
        }

        public void FlashCharger(string full_file_name)
        {
            string state_n = "";
            string state_m = "";

            if (File.Exists(full_file_name))
            {
                State = new InitState(this);
                state_n = State.GetType().Name;
                State.Trigger();
            }

            while (!state_m.Equals(state_n))
            {
                state_m = State.GetType().Name;
                State.Trigger();
                state_n = State.GetType().Name;
            }
        }

        public Target Progbox
        {
            get { return m_target_progbox; }
            set { m_target_progbox = value; }
        }

        public Target Charger
        {
            get { return m_target_charger; }
            set { m_target_charger = value; }
        }

        public SerialInterface ChargerIf
        {
            get { return m_if_charger; }
            set { m_if_charger = value; }
        }

        public SerialInterface ProgboxIf
        {
            get { return m_if_progbox; }
            set { m_if_progbox = value; }
        }


        #region IInterfaceListener Members

        public void OnReceive(IInterface sender, byte[] data, int dataLength)
        {
            if (m_state != null)
            {
                m_state.OnReceive(sender, data, dataLength);
            }
        }

        #endregion

        #region ICommandListener Members

        public void OnNotification(ICommand sender)
        {
            if (m_state != null)
            {
                m_state.OnNotification(sender);
            }
        }

        #endregion
    }
}
