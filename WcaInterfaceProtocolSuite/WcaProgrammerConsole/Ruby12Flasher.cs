using System;
using System.Collections.Generic;
using System.Text;
using WcaInterfaceLibrary;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace WcaDVConsole
{
    class Ruby12Flasher: ICommandListener, IInterfaceListener
    {
        private AutoResetEvent m_telegram_acknowledged;
        private ManualResetEvent m_start_indication_received;
        //private Thread m_upload_thread;
        private WcaInterfaceLibrary.SerialInterface m_interface;
        private string m_full_file_name;
        private bool m_aborted = false;
        Target m_target;

        public Ruby12Flasher()
        {
            m_telegram_acknowledged = new AutoResetEvent(false);
            m_start_indication_received = new ManualResetEvent(false);
            //m_upload_thread = new Thread(new ThreadStart(Upload));
            //m_upload_thread.IsBackground = true;
        }

        public void Run(string com_port, string full_file_name)
        {
            bool abort = false;
            m_full_file_name = full_file_name;
            m_interface = new WcaInterfaceLibrary.SerialInterface(com_port, globals.baud, Parity.None, 8);
            m_target = new Target();
            m_interface.RegisterListener(this);

            if (m_interface.Open() && File.Exists(m_full_file_name))
            {
                while (!abort)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKey key = Console.ReadKey().Key;

                        switch (key)
                        {
                            case ConsoleKey.Escape:
                                abort = true;
                                break;

                        }
                    }

                    if (m_start_indication_received.WaitOne(500))
                    {
                        m_start_indication_received.Reset();
                        Thread.Sleep(1000);
                        GeneralCommand set_bl = new GeneralCommand(m_interface, 0x01, 0x24, new byte[] { 0x01, 0x03, 0x7D }, "keep bootloader running");
                        set_bl.RegisterListener(this);
                        m_target.Queue(set_bl);
                        m_target.Wait();

                        GetApplicationVersionSync();
                        Upload();
                        GetApplicationVersionSync();
                        abort = true;
                    }
                }
            }
            else
            {
                Console.WriteLine("COM port cannot be opened or file does not exists. Pls. check both of them.");
            }

            m_interface.Close();
        }


        private void GetApplicationVersionSync()
        {
            GeneralCommand app_vers = new GeneralCommand(m_interface, 0x01, 0x1E, null, "get application version");
            app_vers.RegisterListener(this);
            m_target.Queue(app_vers);
            m_target.Wait();
        }

        private void Upload()
        {
            StreamReader sr = new StreamReader(m_full_file_name, Encoding.ASCII);
            GeneralCommand upload_cmd;
            string line;
            byte[] sdata;
            ushort seg_cnt = 0;
            byte[] seg_cnt_ba;

            m_telegram_acknowledged.Reset();

            while ((line = sr.ReadLine()) != null && !m_aborted)
            {
                line = "00" + line + "\r\n";
                //Console.Write(".");

                sdata = Encoding.Default.GetBytes(line.ToCharArray());
                seg_cnt_ba = BitConverter.GetBytes(seg_cnt);
                sdata[0] = seg_cnt_ba[0];
                sdata[1] = seg_cnt_ba[1];
                seg_cnt++;

                upload_cmd = new GeneralCommand(m_interface, 0x01, 0x2E, sdata, "");
                m_target.Queue(upload_cmd);
                m_target.Wait();

                if (!m_telegram_acknowledged.WaitOne(1000))
                {
                    break;
                }
            }
            
            sr.Close();
        }
                       

        #region IInterfaceListener Members

        public void OnReceive(IInterface sender, byte[] data, int dataLength)
        {
            m_telegram_acknowledged.Set();

            if (dataLength>5)
            {
                if (data[0] == 0x55 && data[4] == 0x26 && data[5]==0x01)
                {
                    m_start_indication_received.Set();
                }
                else if (data[0] == 0x55 && data[4] == 0x1E)
                {
                    Console.WriteLine("Version: " + data[5] + "." + data[6] + "." + data[7] + "." + data[8]);
                }
                else if(data[0] == 0x55 && data[4] == 0x2E && data[5] == 0x00)
                {
                    Console.Write(".");
                }
                else
                {
                    Console.WriteLine(WcaInterfaceLibrary.Converter.ByteArray2String(data));
                }
            }

            
        }

        #endregion

        #region ICommandListener Members

        public void OnNotification(ICommand sender)
        {
            Console.WriteLine(sender.ToString());
        }

        #endregion
    }
}


