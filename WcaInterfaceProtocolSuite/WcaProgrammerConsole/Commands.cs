using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.IO;
using WcaInterfaceLibrary;
using System.Drawing;

namespace WcaDVConsole
{
    public class Commands : ICommandListener, IInterfaceListener
    {
        public List<Thread> commandThread = new List<Thread>();
        PostProgrammingFrame[] postProgrammingFrame;
        private StreamWriter sw;
        public MainFrame myMainFrame;
        private bool ascii = false;
        private Target m_target = null;
         public bool m_bootloader_ready = false, startEOLmode = false, startDebugmode = false;
        //public Daq myDaq;
        public bool daq = false;
        public int coilTime = 1500;
        private string comport;

        public DeviceData[] Devices;

        public class DeviceData // holds all the data needed for each DUT
        {
            public WcaInterfaceLibrary.SerialInterface m_si;
            public string comport, esn, hw, appVer, timeStamp, reqCoilID, coil0V, coil1V, coil2V, coil0PF, coil1PF, coil2PF, coil0temp, coil1temp, coil2temp, adcV, adcC, Temp, powSupC, chamberTemp, path, ledColor, ackFailures, dutNumber,OTP = "4";
            public string[] coilPF, daqports;
            public int count, activeCoil, flush, baud, readESNtries = 2,commandTries = 2, index, failCount;
            public double[] coilV;
            public StreamWriter sw;
            public Daq myDaq;
            public Target target;
            public byte resentCommand;
            public byte[] resentData;

            public bool fail, enableVoltVeri, bootloaderMode = false, programmingMode = false, startup = true, wakeUp = true, starteol = false, readesn = false, ready = false, programmingStartup = false; //bool vars that tell if an ACK has been recieved for each command.
            public bool bootloaderModeReady = false, setGUIESN = false;
            //public bool[] failMode;

        }

        public Commands(MainFrame m)
        {
            Devices = new DeviceData[6];
            postProgrammingFrame = new PostProgrammingFrame[6];
            myMainFrame = m;
        }

        private void wakeupDut(string step, int DeviceIndex)
        {
            switch (step)
            {
                case "wake up":
                    {
                        
                        if (!Devices[DeviceIndex].wakeUp) return; Devices[DeviceIndex].wakeUp = false;
                        Devices[DeviceIndex].starteol = true;
                        //Thread.Sleep(delayCommand(DeviceIndex));
                        runCommands("Start bootloader", DeviceIndex);
                        break;
                    }
                case "start eol":
                    {
                        if (Devices[DeviceIndex].bootloaderModeReady)
                        {
                            myMainFrame.setESN(Devices[DeviceIndex].dutNumber, "Ready to program", Color.Green);
                            Devices[DeviceIndex].bootloaderMode = true;
                            Devices[DeviceIndex].starteol = false;
                            break; // don't want to go to eol if we want bootloader mode
                        }
                        if (!Devices[DeviceIndex].starteol) return; Devices[DeviceIndex].starteol = false;
                        //Thread.Sleep(delayCommand(DeviceIndex));
                        Devices[DeviceIndex].readesn = true;
                        runCommands("Start EOL", DeviceIndex);
                        break;
                    }
                case "read esn":
                    {
                        if (!Devices[DeviceIndex].readesn) return; Devices[DeviceIndex].readesn = false;
                        //Thread.Sleep(delayCommand(DeviceIndex));
                        Devices[DeviceIndex].baud = 9600;
                        Devices[DeviceIndex].m_si.Close();
                        Devices[DeviceIndex].m_si = new WcaInterfaceLibrary.SerialInterface(Devices[DeviceIndex].comport, Devices[DeviceIndex].baud, Parity.None, 8); // initiate serial connection to device
                        Devices[DeviceIndex].m_si.Open();
                        Devices[DeviceIndex].setGUIESN = true;
                        myMainFrame.changeBaudRadioButtons(Devices[DeviceIndex].dutNumber, 9600);
                        runCommands("Read ESN (big endian)", DeviceIndex);
                        break;
                    }
            }
        }


        public void InitializeDUTforProgramming(string dutnum, string COMport)
        {
            int deviceIndex = findDUTindex(dutnum); // match device to sender comport

            DeviceData temp = new DeviceData();
            temp.dutNumber = dutnum;
            temp.comport = COMport;
            temp.baud = 115200;
            try
            {
                if (temp.m_si != null) temp.m_si.Close();
                temp.m_si = new WcaInterfaceLibrary.SerialInterface(temp.comport, temp.baud, Parity.None, 8); // initiate serial connection to device
            }
            catch (Exception e)
            {
                writeLineToConsole("COM port could not be opened.");
                myMainFrame.setESN(temp.dutNumber, "COM Port Unavailable", Color.Red);
                temp.fail = true;
                return;
            }
            Thread openCom = new Thread(delegate ()
            {
                temp.m_si.Open(); // check if the comport is available and connected properly
            });
            openCom.Start();

            writeLineToConsole("Device connected at 115200 for programming mode only");
            myMainFrame.setESN(temp.dutNumber, "Baud set at 115200", Color.Green);
            temp.programmingMode = true;

            temp.target = new Target();
            temp.m_si.RegisterListener(this);
            Devices[deviceIndex] = temp;
        }
        public void InitializeDUT(string dutnum, string COMport, string programMode, int baud, bool restart) // try to connect to the comport, if it fails return false;
        {
            int deviceIndex = findDUTindex(dutnum); // match device to sender comport
            //if (programMode.Equals("Programming mode")) { InitializeDUTforProgramming(dutnum, COMport); return; }
            DeviceData temp = new DeviceData();
            temp.index = deviceIndex;
            temp.dutNumber = dutnum;
            temp.comport = COMport;
            temp.baud = baud;        
            temp.m_si = changeSerial(COMport, baud, dutnum);
            if (temp.m_si == null) return;
            temp.m_si.RegisterListener(this);
            temp.target = new Target();
            try { temp.myDaq = new Daq(getDAQ(temp.dutNumber)); } catch (Exception e) { daq = true; writeLineToConsole("Daq not available for 'Read coil voltage' command."); } //initiate connection to the daq for querying the analog inputs 0 8 and 1
            
            Devices[deviceIndex] = temp;
            Devices[deviceIndex].setGUIESN = true;
            if (baud == 9600) runCommands("Read ESN (big endian)", deviceIndex); // check if the device connected to us responds properly   
            else
            {
                if (programMode.Equals("Programming mode"))Devices[deviceIndex].bootloaderModeReady = true;
                myMainFrame.setESN(dutnum, "Cycle Power", Color.Yellow);
            }
            //openCom.Join();
        }
        

        public bool checkifComPortExists(string tempdut, string tempcomport,  string programMode, int baud, bool restart) // cannot have 2 device with the same comport // we check if the comport being selected already exists, and if it does we delete the old device object and clear the dut list box it was on.
        {
            int dutIndex = findDUTindex(tempdut);
            //if (Devices.Count == 0) return false;

            foreach (DeviceData dut in Devices)
            {
                if (dut == null) continue;
                if (dut.dutNumber == tempdut)
                {
                    if (Devices[dut.index].m_si != null) { Devices[dut.index].m_si.Close(); Devices[dut.index].m_si.Dispose(); Devices[dut.index].m_si = null; }
                }
            }
                foreach (DeviceData dut in Devices)
            {
                if (dut == null) continue;
                if (dut.comport.ToString().Equals(tempcomport)) // if new comport = existing comport
                {
                    if (dut.dutNumber != tempdut) //if new dut != existing dut | yes: reset existing dut Controls and saved old dut object to new device index
                    {
                        myMainFrame.DUTReset(dut.dutNumber); //clear old dut controls
                        Devices[dutIndex] = dut; // save old dut to new dut index
                        Devices[dutIndex].dutNumber = tempdut;
                        Devices[dutIndex].index = dutIndex;
                        // Devices[findDUTindex(dut.dutNumber)] = null; // null old dut
                        updateSerial(tempcomport, baud, tempdut, false);
                        return true;
                    }
                    else// if comport AND dut number is the same, reset the serial port for good measure
                    {
                        updateSerial(tempcomport, baud, tempdut, false);
                        return true;
                    }
                }
                else if (dut.dutNumber == tempdut) // if old dut comport != new dut comport but old dutnumber == new dutnumber DO NOTHING
                {
                    //updateSerial(tempcomport, baud, tempdut);
                }
            }
            InitializeDUT(tempdut, tempcomport,  programMode, baud, restart);
            return false;
        }
        public WcaInterfaceLibrary.SerialInterface changeSerial(string comport, int baud, string dutnumber)
        {
            WcaInterfaceLibrary.SerialInterface tempSerial;
            try
            {
                tempSerial = new WcaInterfaceLibrary.SerialInterface(comport, baud, Parity.None, 8); // initiate serial connection to device
            }
            catch (Exception e)
            {
                writeLineToConsole("COM port could not be opened.");
                myMainFrame.setESN(dutnumber, "COM Port Unavailable", Color.Red);
                return null;
            }
            Thread openCom = new Thread(delegate ()
            {
                tempSerial.Open(); // check if the comport is available and connected properly
            });
            openCom.Start();
            int intTemp = 0;
            bool boolTemp = false;
            while (true)
            {
                intTemp++;
                Thread.Sleep(50);
                if (tempSerial.isOpen()) { boolTemp = true; break; }
                if (intTemp > 10) { boolTemp = false; break; }
            }
            if (!boolTemp)
            {
                writeLineToConsole("COM port could not be opened.");
                myMainFrame.setESN(dutnumber, "COM Port Unavailable", Color.Red);
                return null;
            }
            tempSerial.RegisterListener(this);
            
            return tempSerial;
        }
        public bool updateSerial (string comport, int baud, string dutnumber, bool restart)
        {
            int dutIndex = findDUTindex(dutnumber);
            if (Devices[dutIndex] == null) return false; ;
            if (Devices[dutIndex].m_si != null)
            {
                //Thread.Sleep(250);
                Devices[dutIndex].m_si.Close();
                //Devices[dutIndex].m_si.Dispose();
                Devices[dutIndex].m_si = null;
            }
            Devices[dutIndex].baud = baud;
            Devices[dutIndex].comport = comport;
            //Thread.Sleep(250);
            Devices[dutIndex].m_si = changeSerial(comport, baud, dutnumber);
           // Thread.Sleep(250);
            if (Devices[dutIndex].m_si == null) return false;
            Devices[dutIndex].readESNtries = 2;
            if (baud != 115200)
            {
                Devices[dutIndex].setGUIESN = true;
                runCommands("Read ESN (big endian)", dutIndex);
            }
            return true;
        }

        public void removeDUT(string dutnum)// close comport and set dut to null
        {
            int dutIndex = findDUTindex(dutnum);            
            if (Devices[dutIndex] == null) return;
            if (Devices[dutIndex].m_si != null)
            {
                //myMainFrame.DUTReset(dutnum);
                Devices[dutIndex].m_si.Close();
                Devices[dutIndex].m_si.Dispose();
            }
            Devices[dutIndex] = null;      
        }
        public List<int> ActiveDUTList()
        {
            List<int> temp = new List<int>();
            for(int a = 0; a < 6; a++)           
            { if (Devices[a] != null) if (Devices[a].ready) temp.Add(a); }
            return temp;
        }
        public bool anyActiveDuts()
        {
            foreach (DeviceData dut in Devices)
            { if (dut != null) if (dut.ready) return true; }
            return false;
        }
        private string[] getDAQ(string mydutnum) // get the analog input ports of the DAQ that we need to collect the coil volatages
        {
            switch (mydutnum)
            {

                case "DUT 1": return new string[] { "0", "8", "1" };
                case "DUT 2": return new string[] { "9", "2", "10" };
                case "DUT 3": return new string[] { "3", "11", "4" };
                case "DUT 4": return new string[] { "12", "5", "13" };
                case "DUT 5": return new string[] { "6", "14", "7" };
                case "DUT 6": return new string[] { }; // daq doesn't have enough analog input ports for 6 DUTs
            }
            return new string[] { };
        }

        public void writeToConsole(string myString)
        {
            myMainFrame.writeToConsole(myString);
        }
        public void writeLineToConsole(string myString)
        {
            myMainFrame.writeToConsole(myString + "\n");
        }
        public void UploadApplication(WcaInterfaceLibrary.SerialInterface si, Target target, string full_file_name, int index)
        {

            int lineCount = File.ReadLines(full_file_name).Count(); // gets all lines in the firmware file saves it to lineCount
            myMainFrame.setupProgressBar(lineCount); // changes the max value of the progress bar to the total line count of the file
            StreamReader sr = new StreamReader(full_file_name, Encoding.ASCII);
            GeneralCommand upload_cmd;
            string line;
            byte[] sdata;
            ushort seg_cnt = 0;
            byte[] seg_cnt_ba;
            while ((line = sr.ReadLine()) != null)
            {
                myMainFrame.updateProgressBar(seg_cnt, lineCount); // update the progress bar as the program completes
                line = "00" + line + "\r\n";
                //writeLineToConsole(line);

                sdata = Encoding.Default.GetBytes(line.ToCharArray());
                seg_cnt_ba = BitConverter.GetBytes(seg_cnt);
                sdata[0] = seg_cnt_ba[0];
                sdata[1] = seg_cnt_ba[1];
                seg_cnt++;

                upload_cmd = new GeneralCommand(si, 0x01, 0x2E, sdata, "");
                target.Queue(upload_cmd);
                target.Wait();
            }
            sr.Close();
            writeLineToConsole("Upload application done.");
            writeLineToConsole("Device is still in bootloader mode, select Start Debug or Start EOL to continue using the device.");
            
            myMainFrame.resetProgressBar(); // reset progress bar
            postProgrammingFrame[index] = new PostProgrammingFrame(this, myMainFrame, index);
            postProgrammingFrame[index].ShowDialog();
        }
        public void runCommands(string command, int deviceindex)
        {
            //if (commandThread != null)
            //{
            //    writeLineToConsole(commandThread.ThreadState.ToString());
            //    if (commandThread.IsAlive) commandThread.Abort();
            //    writeLineToConsole(commandThread.ThreadState.ToString());
            //}
            Thread.Sleep(20);
            if (commandThread.Count > 1) commandThread[commandThread.Count - 2].Abort();
            commandThread.Add(new Thread(delegate ()
            {
                switch (command)
                {
                    case "Read application version": ReadApplicationVersion(deviceindex); break;
                    case "Read bootloader version": ReadBootloaderVersion(deviceindex); break;
                    case "Read bridge voltage": ReadBridgeVoltage(deviceindex); break;
                    case "Read capacitive sensor": ReadCapacitiveSensor(deviceindex); break;
                    case "Read capacitive state": ReadCapacitiveSensor(deviceindex); break;
                    case "Read coil current peak": ReadCoilCurrentPeak(deviceindex); break;
                    case "Read coil current RMS": ReadCoilCurrentRMS(deviceindex); break;
                    case "d":
                    case "Read coil voltage from DAQ": ReadCoilVoltageFromDAQ(deviceindex); break; // Read the voltage of each coil
                    case "Read ADC current": ReadADCCurrent(deviceindex); break;
                    case "Read ADC voltage": ReadADCVoltage(deviceindex); break;
                    case "Read ESN (big endian)": ReadESNbigEndian(deviceindex); break;
                    case "Read ESN (little endian)": ReadESNlittleEndian(deviceindex); break;
                    case "Read LED color": ReadLEDColor(deviceindex); break;
                    case "Read library version": ReadLibraryVersion(deviceindex); break;
                    case "Read power loss(mW)": ReadPowerLoss(deviceindex); break;
                    case "Read protocol version": ReadProtocolVersion(deviceindex); break;
                    case "Read PWM communication": ReadPWMCommunication(deviceindex); break;
                    case "Read receive power": ReadReceivePower(deviceindex); break;
                    case "Read temperature(C)": ReadTemperature(deviceindex); break;
                    case "Read transmit power": ReadTransmitPower(deviceindex); break;
                    case "b": SetBootloader(deviceindex); break;// used for when the user first applies power - if you press "b" within 500ms you can put the device in bootloader mode for programming and or choosing EOL or DEBUG mode.
                    case "Set bridge voltage": SetBridgeVoltage(deviceindex); break;//Sets the desired bridge voltage in mV.
                    case "Set ESN": SetESN(); break;
                    case "Set LED color": SetLEDColor(deviceindex); break;
                    case "a":
                    case "Set transmitter coil": SetTransmitterCoil(deviceindex); break;
                    case "Set Vrail MAX": SetVrailMAX(deviceindex); break;
                    case "Set Vrail MIN": SetVrailMIN(deviceindex); break;
                    case "Start bootloader": startBootloader(deviceindex); break;
                    case "Start EOL": StartEOL(deviceindex); break; // used to start EOL mode in 9600 when in bootloader mode (bootloader mode must be entered while in 115200 and by pressing 'b' 100ms after power up)
                    case "Start charging": StartCharging(deviceindex); break;
                    case "Start debug": StartDebug(deviceindex); break;//used to start debugger mode in the device when in bootloader mode  (bootloader mode must be entered while in 115200 and by pressing 'b' 100ms after power up)
                    case "Stop charging": StopCharging(deviceindex); break;
                    case "Resent Commands": resendTimeout(deviceindex); break;
                    case "program": // for programming new firmware on the device
                                    //string user_app = @"f:\projects\svn_projects\0_31X_P0517_moray_ruby1_2\trunk\development\software\releases\wca\release\MO_WC_11_1_8_6-10894\MO_WC_11_1_8_6.S19";
                                    //UploadApplication(m_si, m_target, target_file);
                        break;
                }
            }));

            commandThread[commandThread.Count - 1].Start();
        }


        //                                 Transmit Telegram
        //offset:   0        1       2          3              4       5   6   7         8  9
        //example: 55   |    07     00   |     64         |   24  |   02   01  00    |   36 4A  | //startapp command
        //     |Sync Val| message length |Source/Dest/Code|Command|       Data       | Checksum |  //data can contain subcommand info



        //                                 Response Telegram
        //offset:   0        1       2          3              4       5   6       7  8
        //example: 55   |    06     00   |2D (2f=negack)  |   01  |   01   13 |   36 4A  | //readpro command
        //     |Sync Val| message length |Source/Dest/Code|Command|    Data   | Checksum |  //data can contain subcommand info

        ////////////////////////////////////// READS /////////////////////////////////////

        public void ReadApplicationVersion(int DeviceIndex)
        {
            GeneralCommand appver = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x1E, new byte[] { }, "read application version");
            appver.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(appver);
        }
        public void ReadBootloaderVersion(int DeviceIndex)
        {
            GeneralCommand bootver = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x1A, new byte[] { }, "read bootLoader version");
            bootver.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(bootver);
        }
        public void ReadBridgeVoltage(int DeviceIndex)
        {
            GeneralCommand readbridgev = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x04, new byte[] { 0x07 }, "read runtime parameter: requests bridge voltage in mv");
            readbridgev.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(readbridgev);
        }
        public void ReadCapacitiveSensor(int DeviceIndex)
        {
            GeneralCommand readcapst = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x14, new byte[] { }, "Read the current capacitive sensor values.");
            readcapst.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(readcapst);
        }
        public void ReadCapacitiveState(int DeviceIndex)
        {
            GeneralCommand capState = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x12, new byte[] { }, "read cap sensor state");
            capState.RegisterListener(this);
            m_target.Queue(capState);
        }
        public void ReadCoilCurrentPeak(int DeviceIndex)
        {
            GeneralCommand readcurr = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x04, new byte[] { 0x09 }, "Requests the peak value of coil current. This value is only valid in the charging state.");
            readcurr.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(readcurr);
        }
        public void ReadCoilCurrentRMS(int DeviceIndex)
        {
            GeneralCommand readcurrms = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x04, new byte[] { 0x0A }, "read runtime parameter: ");
            readcurrms.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(readcurrms);
        }
        public void ReadCoilVoltageFromDAQ(int DeviceIndex)
        {
            if (daq) { writeLineToConsole("DAQ not available!"); return; }
            double[] coils = new double[3];
            Array.Copy(Devices[DeviceIndex].myDaq.getVolts(), coils, 3);
            writeLineToConsole(string.Format("\nCoil0:{0:N2}  Coil1:{1:N2}  Coil2:{2:N2}", coils[0], coils[1], coils[2]));
        }
        public void ReadADCCurrent(int DeviceIndex)
        {
            GeneralCommand readcadc = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x04, new byte[] { 0x0d }, "read runtime parameter: ");
            readcadc.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(readcadc);
        }
        public void ReadADCVoltage(int DeviceIndex)
        {
            GeneralCommand readadc = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x04, new byte[] { 0x08 }, "read runtime parameter: Requests the ADC count of the measured H - bridge input voltage.");
            readadc.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(readadc);
        }
        public void ReadESNbigEndian(int DeviceIndex)
        {
            GeneralCommand readParaBE = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x03, new byte[] { 0x03, 0x01 }, "read general device parameters for ESN (big endian)");
            readParaBE.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(readParaBE);
            //if (Devices[DeviceIndex].readesn == 0) Devices[DeviceIndex].readesn = 5;
        }
        public void ReadESNlittleEndian(int DeviceIndex)
        {
            GeneralCommand readParaLE = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x03, new byte[] { 0x02, 0x01 }, "read general device parameters for ESN (little endian)");
            readParaLE.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(readParaLE);
        }
        public void ReadLEDColor(int DeviceIndex)
        {
            GeneralCommand readColor = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x0E, new byte[] { }, "Read the LED color");
            readColor.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(readColor);
        }
        public void ReadLibraryVersion(int DeviceIndex)
        {
            GeneralCommand libver = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x18, new byte[] { }, "read freescale library version");
            libver.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(libver);
        }
        public void ReadPowerLoss(int DeviceIndex)
        {
            GeneralCommand readpowlos = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x04, new byte[] { 0x05 }, "read runtime parameter: Requests the internal system losses in mW.");
            readpowlos.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(readpowlos);
        }
        public void ReadProtocolVersion(int DeviceIndex)
        {
            GeneralCommand protocolver = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x01, new byte[] { }, "read protocol version");
            protocolver.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(protocolver);
        }
        public void ReadPWMCommunication(int DeviceIndex)
        {
            GeneralCommand pwm_cmd = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x04, new byte[] { 0x18 }, "PWM Communication");
            pwm_cmd.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(pwm_cmd);
        }
        public void ReadReceivePower(int DeviceIndex)
        {
            GeneralCommand receivepower = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x04, new byte[] { 0x01 }, "read receive power");
            receivepower.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(receivepower);
        }
        public void ReadTemperature(int DeviceIndex)
        {
            GeneralCommand readtemp = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x04, new byte[] { 0x011 }, "read runtime parameter: Requests the temperature values of all available sensors in degree celcius.");
            readtemp.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(readtemp);
        }
        public void ReadTransmitPower(int DeviceIndex)
        {
            GeneralCommand transpower = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x04, new byte[] { 0x06 }, "read input power on the H-bridge");
            transpower.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(transpower);
        }


        ////////////////////////////////////// READS /////////////////////////////////////

        ////////////////////////////////////// SETS //////////////////////////////////////
        public void SetBootloader(int DeviceIndex)
        {
            byte[] cmdb = new byte[3];
            cmdb[0] = 0x01;
            cmdb[1] = 0x03;
            cmdb[2] = 0x7D;
            GeneralCommand set_bl = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x24, cmdb, "keep bootloader running");
            //writeLineToConsole("CMD:B:"+set_bl.ToString());
            set_bl.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(set_bl);
            m_target.Wait();
        }
        public void SetBridgeVoltage(int DeviceIndex)
        {
            writeLineToConsole("Enter the desired bridge voltage in mV between 0 and 32767.");
            string temp = myMainFrame.waitForEntry(); if (temp.Equals("abortThread")) { writeLineToConsole("\nCommand canceled"); return; }
            int voltsInt = Convert.ToInt32(temp); if (voltsInt > 32767) { writeLineToConsole("number greater than 32767"); return; }
            byte[] tempArray = BitConverter.GetBytes(voltsInt);
            byte[] voltArray = new byte[2];
            for (int a = 0; a < voltArray.Length; a++) { voltArray[a] = tempArray[a]; }
            GeneralCommand setbridgev = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x06, new byte[] { 0x07, voltArray[0], voltArray[1] }, "set bridge voltage in mv");
            setbridgev.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(setbridgev);
        }
        public void SetESN()
        {
            
        }
        public void SetESNfromForm(int DeviceIndex, byte[] finalArray_t)
        {
            GeneralCommand setESN = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x07, finalArray_t, "Set ESN");
            setESN.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(setESN);
        }

            public void SetESNOLD(int DeviceIndex)
        {
            

        }
        //    lbCommands.Items.Add("Set LED color");
        public void SetLEDColor2 (int DeviceIndex, byte[] redArray, byte[] greenArray, byte[] blueArray)
        {



            GeneralCommand writeColor = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x0C, new byte[] { redArray[0], redArray[1], greenArray[0], greenArray[1], blueArray[0], blueArray[1] }, "set led color");
            writeColor.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(writeColor);
        }

        public void SetLEDColor(int DeviceIndex)
        {
           
        }
        //    lbCommands.Items.Add("Set transmitter coil");
        public void SetTransmitterCoil(int DeviceIndex)
        {
            int coilId = 0;
            bool skip = true;
            bool loop = true;
            while (loop)
            {
                writeToConsole("\nPress 0 for coil 0\n");
                writeToConsole("Press 1 for coil 1\n");
                writeToConsole("Press 2 for coil 2\n");
                writeToConsole("Press any other key to stop all coils\n");
                writeToConsole("Press t to choose in milliseconds the wait time between coil on/off switch\n");
                string temp1 = myMainFrame.waitForEntry(); if (temp1.Equals("abortThread")) { writeLineToConsole("\nCommand canceled"); return; }
                if (temp1.Equals("abortThread")) { writeLineToConsole("\nCommand canceled"); return; }
                switch (temp1)
                {
                    case "0":
                        {
                            writeLineToConsole("\nset coil 0 to active");
                            coilId = 0;
                            loop = false; break;
                        }
                    case "1":
                        {
                            writeLineToConsole("\nset coil 1 to active");
                            coilId = 1;
                            loop = false; break;
                        }
                    case "2":
                        {
                            writeLineToConsole("\nset coil 2 to active");
                            coilId = 2;
                            loop = false; break;
                        }
                    case "t":
                        {
                            writeLineToConsole("\nenter coil switch delay in milliseconds");
                            string coiltemp = myMainFrame.waitForEntry(); if (coiltemp.Equals("abortThread")) { writeLineToConsole("\nCommand canceled"); return; }
                            coilTime = Convert.ToInt32(coiltemp);
                            writeLineToConsole("\ncoil switch delay is now " + coilTime + " milliseconds. Returned to setcoil menu.");
                            break;
                        }
                    default:
                        {
                            writeLineToConsole("\ndeactivate all coils");
                            coilId = 0x0F;
                            skip = false;
                            loop = false; break;
                        }
                }
            }
            if (skip)
            {
                GeneralCommand stopcoils = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x06, new byte[] { 0x19, 0x0F }, "stop active coil"); // have to stop all coils first before setting new coils.
                stopcoils.RegisterListener(this);
                Devices[DeviceIndex].target.Queue(stopcoils);

                Thread.Sleep(coilTime); // should sleep a bit before setting new coil active
            }
            writeLineToConsole(coilId.ToString());
            GeneralCommand setactivecoil = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x06, new byte[] { 0x19, Convert.ToByte(coilId) }, "set active coil");
            setactivecoil.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(setactivecoil);
        }
        //    lbCommands.Items.Add("Set Vrail MAX");
        public void SetVrailMAX(int DeviceIndex)
        {
            UInt16 maxtimeVal;
            writeLineToConsole("\nEnter control time in mseconds (max 30000):");
            string controltime = myMainFrame.waitForEntry(); if (controltime.Equals("abortThread")) { writeLineToConsole("\nCommand canceled"); return; }
            if (!UInt16.TryParse(controltime, out maxtimeVal))
            {
                writeLineToConsole("Invalid Control Time, can't perform command");
                writeLineToConsole("\nCommand canceled"); return;
            }
            if (maxtimeVal > 30000)
            {
                writeLineToConsole("Can't set time greater than 30000 msec, can't perform command");
                writeLineToConsole("\nCommand canceled"); return;

            }
            byte[] maxtimeArray = BitConverter.GetBytes(maxtimeVal);
            writeLineToConsole(string.Format("Writing MaxTime:{0:X}{1:X}",
               maxtimeArray[0], maxtimeArray[1]));
            GeneralCommand setVrailMax = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x06, new byte[] { 0x21, 0x02, maxtimeArray[0], maxtimeArray[1] }, "set Vrail to maximum value");
            setVrailMax.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(setVrailMax);
        }
        public void SetVrailMIN(int DeviceIndex)
        {
            UInt16 mintimeVal;
            writeLineToConsole("\nEnter control time in mseconds (max 30000):");
            string mincontroltime = myMainFrame.waitForEntry(); if (mincontroltime.Equals("abortThread")) { writeLineToConsole("\nCommand canceled"); return; }
            if (!UInt16.TryParse(mincontroltime, out mintimeVal))
            {
                writeLineToConsole("Invalid Control Time, can't perform command");
                writeLineToConsole("\nCommand canceled"); return;
            }
            if (mintimeVal > 30000)
            {
                writeLineToConsole("Can't set time greater than 30000 msec, can't perform command");
                writeLineToConsole("\nCommand canceled"); return;
            }
            byte[] mintimeArray = BitConverter.GetBytes(mintimeVal);
            writeLineToConsole(string.Format("Writing MaxTime:{0:X}{1:X}", mintimeArray[0], mintimeArray[1]));
            GeneralCommand setVrailMin = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x06, new byte[] { 0x21, 0x01, mintimeArray[0], mintimeArray[1] }, "set Vrail to minimum value");
            setVrailMin.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(setVrailMin);
        }
        public void StartCharging(int DeviceIndex)
        {
            GeneralCommand silencemodeoff = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x3B, new byte[] { 0x01 }, "restart charging");
            silencemodeoff.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(silencemodeoff);
        }
        public void StartDebug(int DeviceIndex)
        {
            //if (myMainFrame.ProgramMode.Equals("Programming mode")) myMainFrame.updateProgrammingMode("End of line test mode");
            GeneralCommand set_app = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x24, new byte[] { 0x02, 0x01, 0x00 }, "Start user application");
            set_app.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(set_app);
            startDebugmode = true;

        }
        public void StartEOL(int DeviceIndex)
        {
            //if (myMainFrame.ProgramMode.Equals("Programming mode")) myMainFrame.updateProgrammingMode("End of line test mode");

            GeneralCommand set_app_eol = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x24, new byte[] { 0x02, 0x07, 0x00 }, "Start End of Line Mode");
            set_app_eol.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(set_app_eol);
            //if (Devices[DeviceIndex].starteol == 0) Devices[DeviceIndex].starteol = 5;
            
        }
        public void StopCharging(int DeviceIndex)
        {
            GeneralCommand silencemode = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x3B, new byte[] { 0x02 }, "stop charging");
            silencemode.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(silencemode);
        }
        public void startBootloader(int DeviceIndex)
        {
            GeneralCommand set_bl = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x24, new byte[] { 0x01, 0x03, 0x7D }, "keep bootloader running");
            set_bl.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(set_bl);
        }
        ////////////////////////////////////// SETS //////////////////////////////////////
        public void resendTimeout(int DeviceIndex)
        {
            GeneralCommand resendTimeout = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, Devices[DeviceIndex].resentCommand, Devices[DeviceIndex].resentData, "resendTimeout");
            resendTimeout.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(resendTimeout);
        }
        private void getResentCommandInfo(string[] resent, int deviceIndex)
        {

            Devices[deviceIndex].resentCommand = StringToByteArray(resent[4])[0];
            List<byte> data_ = new List<byte>();
            for (int a = 5; a < resent.Length - 3; a++)
            {
                data_.Add(StringToByteArray(resent[a])[0]);
            }
            Devices[deviceIndex].resentData = new byte[data_.Count];
            for (int b = 0; b < data_.Count; b++) Devices[deviceIndex].resentData[b] = data_[b];
        }
        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        public int findDUTindex(string dut)
        {
            switch (dut)
            {
                case "DUT 1": { return 0; }
                case "DUT 2": { return 1; }
                case "DUT 3": { return 2; }
                case "DUT 4": { return 3; }
                case "DUT 5": { return 4; }
                case "DUT 6": { return 5; }
            }
            return 0;
        }
        private int delayCommand(int index)
        {
            int delay = 0;
            switch (index)
            {
                case 0: { delay = 20; break; }
                case 1: { delay = 40; break; }
                case 2: { delay = 60; break; }
                case 3: { delay = 80; break; }
                case 4: { delay = 100; break; }
                case 5: { delay = 120; break; }
            }
            return delay;
        }
        private int MatchDevicetoComPort(string comport)
        {
            for (int a = 0; a < 6; a++)
            {
                if (Devices[a] == null) continue;
                if (Devices[a].comport.Equals(comport)) return a;
            }
            return 0;
        }
        private string getTime()
        {
            string datePatt = @"hh.mm.ss.ff";
            DateTime saveUtcNow = DateTime.UtcNow;
            return saveUtcNow.ToString(datePatt);
        }
        //                                 Transmit Telegram
        //offset:   0        1       2          3              4       5   6   7         8  9
        //example: 55   |    07     00   |     64         |   24  |   02   01  00    |   36 4A  | //startapp command
        //     |Sync Val| message length |Source/Dest/Code|Command|       Data       | Checksum |  //data can contain subcommand info



        //                                 Response Telegram
        //offset:   0        1       2          3              4       5   6       7  8
        //example: 55   |    06     00   |2D (2f=negack)  |   01  |   01   13 |   36 4A  | //readpro command
        //     |Sync Val| message length |Source/Dest/Code|Command|    Data   | Checksum |  //data can contain subcommand info
        #region IInterfaceListener Members

        public void OnReceive(IInterface sender, byte[] data, int dataLength)
        {
            int DeviceIndex = MatchDevicetoComPort(sender.GetName()); // match device to sender comport            
            string str;
            //if (ascii)
                str = System.Text.Encoding.Default.GetString(data);
            //else str = WcaInterfaceLibrary.Converter.ByteArray2String(data);   
            if (myMainFrame.ProgramMode.Equals("Debug mode"))
                writeLineToConsole(Devices[DeviceIndex].dutNumber + ": " + getTime() + " " + str);
            if (Devices[DeviceIndex].baud == 115200)
            {
                //if ( str.Equals("55 09 00 02 26 01 00 00 00 00 94 7B"))

                //if (data[4] == 0x26 && data[5] == 0x01)
                if (data.Length > 6 && data.Length < 20)
                {
                    for(int a = 0; a < data.Length; a++)
                    {
                        if (data[a] == 0x26)
                        {
                            if(data[a + 1] == 0x01)
                            {
                                if (data[a + 2] == 0x00) wakeupDut("wake up", DeviceIndex);

                            }
                        }
                    }
                }
            }          
        }


        #endregion

        #region ICommandListener Members
        public void OnNotification(ICommand sender) // this function recieves a verified message from the device which we can then parse and log.
        {
            try
            {
                int deviceIndex = MatchDevicetoComPort(sender.comport); // match device to sender comport
                writeLineToConsole(Devices[deviceIndex].dutNumber + ": " + getTime() + " " + sender.ToString());
                if (sender.Result == WcaInterfaceCommandResult.ExecutionTimeout)
                {
                    string sentCommand = sender.ToString();
                    string[] sentBytes = sentCommand.Split(':')[2].Split(' ');
                    if (Devices[deviceIndex].commandTries > 0)
                    {
                        getResentCommandInfo(sentBytes, deviceIndex);
                        runCommands("Resent Commands", deviceIndex);
                        Devices[deviceIndex].commandTries--; return;
                    }
                    if (Devices[deviceIndex].commandTries <= 0) // checking to see if the device is talking.)
                    {
                        myMainFrame.startupFailure(deviceIndex);
                        writeLineToConsole("Please power cycle the device on " + Devices[deviceIndex].dutNumber); return;
                    }
                }
                if (sender.isReceive)
                {
                    byte[] data = sender.getData; // sender.getData gets byte array of the response telegram which we used to determine which command was sent and what the data response is.

                    string str = "";
                    string str2 = "";
                    if (ascii) str = System.Text.Encoding.Default.GetString(data);
                    else str = WcaInterfaceLibrary.Converter.ByteArray2String(data);


                    if (!string.IsNullOrEmpty(str))
                    {
                        string[] response = str.Split(' '); // we make the string of data bytes and turn it into a string array to parse out the values we want
                        if (sender.Result == WcaInterfaceCommandResult.NegAck) // for setting the gui to match dut to comport
                        {                                                                   // if repsonse is NegAck then send "no esn" to gui
                            if (response[4] == "03")
                            {
                                Devices[deviceIndex].ready = true; // if device has esn then device is ready
                                myMainFrame.setESN(Devices[deviceIndex].dutNumber, "No ESN", Color.SkyBlue);

                            }
                        }
                        if (sender.Result == WcaInterfaceCommandResult.NegAck && Devices[deviceIndex].startup) // for setting the gui to match dut to comport
                        {
                            Devices[deviceIndex].wakeUp = false; // now we know that the device is talking and in eol mode

                        }
                        if (sender.Result == WcaInterfaceCommandResult.PosAck)
                        {
                            Devices[deviceIndex].commandTries = 2;
                        }

                        if (response[0].Equals("55") && response[3].Equals("2D") && response.Length > 4) //here we are checking if the data we got is one from our wireless charger, a POS ACK, and more than 4 bytes
                        {
                            //writeLineToConsole("\n"+DateTime.Now.ToShortTimeString() + " [" + str + "] PosAck");
                            //offset:                        0        1       2          3              4       5   6       7  8
                            switch (response[4]) //example: 55   |    06     00   |2D (2f=negack)  |   01  |   01   13 |   36 4A  | //readpro command
                            {                    //     |Sync Val| message length |Source/Dest/Code|Command|    Data   | Checksum |  //data can contain subcommand info
                                case "04": // read runtime parameter
                                    {
                                        switch (response[5])
                                        {
                                            case "08": //readadcv
                                                {
                                                    byte[] myData = { data[6], data[7], 0x00, 0x00 }; //bitConverter requires at least 4 bytes.....
                                                    writeLineToConsole("Read ADC Voltage Count \t" + BitConverter.ToInt32(myData, 0).ToString());
                                                    break;
                                                }
                                            case "11": //readtemp
                                                {
                                                    writeLineToConsole("Read Coil Temps \tCoil0: " + Convert.ToInt32(data[6]).ToString() + "C Coil1: " + Convert.ToInt32(data[7]).ToString() + "C Coil2: " + Convert.ToInt32(data[8]).ToString() + "C");
                                                    break;
                                                }
                                            case "0A": //read current coil ma RMS
                                                {
                                                    byte[] myData = { data[6], data[7], 0x00, 0x00 };
                                                    writeLineToConsole("Read Current RMS\t" + BitConverter.ToInt32(myData, 0).ToString() + "mA");
                                                    break;
                                                }
                                            case "19": //read active coil
                                                {
                                                    writeLineToConsole("Read active coil \t Active coil: " + Convert.ToInt32(data[6]).ToString());
                                                    break;
                                                }
                                            case "18": //readpwm
                                                break;
                                            case "01": //readpow
                                                break;
                                            case "05": //readpwrlos
                                                break;
                                            case "09": //readcurr
                                                break;

                                        }
                                        break;
                                    }
                                case "06": // set Runtime Parameter
                                    {
                                        switch (response[5])
                                        {
                                            case "19":

                                                writeLineToConsole("Set Coil sent");
                                                break;
                                        }
                                        break;
                                    }
                                case "07": // set Genreal Parameter usually ESN and HW
                                    {
                                        Devices[deviceIndex].OTP = (Convert.ToInt32(Devices[deviceIndex].OTP) - 1).ToString();

                                        break;
                                    }
                                case "1E": // read application version
                                    {
                                        string appversion = Convert.ToInt32(data[5]).ToString() + "." + Convert.ToInt32(data[6]).ToString()
                                            + "." + Convert.ToInt32(data[7]).ToString() + "." + Convert.ToInt32(data[8]).ToString();
                                        writeLineToConsole("Read App Version \t" + appversion);
                                        if (postProgrammingFrame[deviceIndex] != null) postProgrammingFrame[deviceIndex].updatelblFirwareVersion(appversion);
                                        break;
                                    }
                                case "03": // read esn
                                    {
                                        switch (response[5])
                                        {
                                            case "02": // little endian
                                                {

                                                    string[] fullesn = str.Split(' ');
                                                    if (fullesn.Length == 22) // add exttra digit because there is an empty element at the end of this string array
                                                    {
                                                        string myesn = fullesn[12] + fullesn[11] + fullesn[10] + fullesn[9] + fullesn[8] + fullesn[7] + fullesn[6];
                                                        string myhw = fullesn[16] + "." + fullesn[14];
                                                        Devices[deviceIndex].OTP = (4 - Convert.ToInt32(fullesn[18])).ToString();
                                                        str2 = "ESN:" + myesn + ",HW:" + myhw;
                                                        writeLineToConsole(" Read ESN and HW \tESN: " + myesn + " HW: " + myhw);
                                                        if (Devices[deviceIndex].setGUIESN)
                                                        {
                                                            myMainFrame.setESN(Devices[deviceIndex].dutNumber, myesn, Color.LightBlue);
                                                            Devices[deviceIndex].setGUIESN = false;
                                                        }
                                                    }
                                                    if (fullesn.Length == 20) // add exttra digit because there is an empty element at the end of this string array
                                                    {
                                                        string myesn = fullesn[13] + fullesn[12] + fullesn[11] + fullesn[10] + fullesn[9] + fullesn[8] + fullesn[7] + fullesn[6];
                                                        string myhw = fullesn[15] + "." + fullesn[14];
                                                        Devices[deviceIndex].OTP = (4 - Convert.ToInt32(fullesn[15])).ToString();
                                                        str2 = "ESN:" + myesn + ",HW:" + myhw;
                                                        writeLineToConsole(" Read ESN and HW \tESN: " + myesn + " HW: " + myhw);
                                                        if (Devices[deviceIndex].setGUIESN)
                                                        {
                                                            myMainFrame.setESN(Devices[deviceIndex].dutNumber, myesn, Color.LightBlue);
                                                            Devices[deviceIndex].setGUIESN = false;
                                                        }
                                                    }


                                                    break;
                                                }
                                            case "03": // big endian
                                                {
                                                    Devices[deviceIndex].ready = true; // if device has esn then device is ready
                                                    string[] fullesn = str.Split(' ');
                                                    if (fullesn.Length == 22) // add exttra digit because there is an empty element at the end of this string array
                                                    {
                                                        string myesn = fullesn[7] + fullesn[8] + fullesn[9] + fullesn[10] + fullesn[11] + fullesn[12] + fullesn[13];
                                                        string myhw = fullesn[15] + "." + fullesn[17];
                                                        string tempOTP = (4 - Convert.ToInt32(fullesn[18])).ToString();

                                                        Devices[deviceIndex].OTP = tempOTP;
                                                        str2 = "ESN:" + myesn + ",HW:" + myhw;
                                                        writeLineToConsole(" Read ESN and HW \tESN: " + myesn + " HW: " + myhw);

                                                        myMainFrame.setESN(Devices[deviceIndex].dutNumber, myesn, Color.LightBlue);
                                                        Devices[deviceIndex].setGUIESN = false;

                                                    }
                                                    else if (fullesn.Length == 20)
                                                    {
                                                        string myesn = fullesn[6] + fullesn[7] + fullesn[8] + fullesn[9] + fullesn[10] + fullesn[11] + fullesn[12] + fullesn[13];
                                                        string myhw = fullesn[14] + "." + fullesn[15];
                                                        string tempOTP = (4 - Convert.ToInt32(fullesn[16])).ToString();

                                                        Devices[deviceIndex].OTP = tempOTP;
                                                        Devices[deviceIndex].esn = myesn;
                                                        Devices[deviceIndex].hw = myhw;
                                                        str2 = "ESN:" + myesn + ",HW:" + myhw;
                                                        writeLineToConsole(" Read ESN and HW \tESN: " + myesn + " HW: " + myhw);

                                                        if (Devices[deviceIndex].setGUIESN)
                                                        {
                                                            myMainFrame.setESN(Devices[deviceIndex].dutNumber, myesn, Color.LightBlue);
                                                            Devices[deviceIndex].setGUIESN = false;
                                                        }
                                                    }



                                                    break;
                                                }
                                        }
                                        break;
                                    }

                                case "0E": // Read LED color
                                    {
                                        byte[] red = { data[5], data[6], 0x00, 0x00 }; //bitConverter requires at least 4 bytes.....
                                        byte[] green = { data[7], data[8], 0x00, 0x00 }; //bitConverter requires at least 4 bytes.....
                                        byte[] blue = { data[9], data[10], 0x00, 0x00 }; //bitConverter requires at least 4 bytes.....                                    
                                        writeLineToConsole("Read LED\t Color values = Red: " + (BitConverter.ToInt32(red, 0) / 655.35).ToString() + " Green: " + (BitConverter.ToInt32(green, 0) / 655.35).ToString() + " Blue: " + (BitConverter.ToInt32(blue, 0) / 655.35).ToString());
                                        break;
                                    }
                                case "0C": // set led
                                    {
                                        writeLineToConsole("Set LED Color");
                                        break;
                                    }
                                case "24": // set active application
                                    {
                                        if (Devices[deviceIndex].bootloaderModeReady) Devices[deviceIndex].bootloaderMode = true;
                                        wakeupDut("read esn", deviceIndex);
                                        wakeupDut("start eol", deviceIndex);

                                        if (startDebugmode || startEOLmode)
                                        {
                                            //myMainFrame.startEOLmode(deviceIndex);
                                            //startEOLmode = false;
                                            //startDebugmode = false;
                                            ////Devices[deviceIndex].starteol = 0;

                                        }
                                        //if (Devices[deviceIndex].programmingStartup)
                                        //{

                                        //    Devices[deviceIndex].programmingStartup = false;
                                        //    postProgrammingFrame = new PostProgrammingFrame(this, myMainFrame, deviceIndex);
                                        //    postProgrammingFrame.ShowDialog();

                                        //}


                                        break;
                                    }
                                case "01": // read protocol version
                                    {
                                        break;
                                    }
                                case "18": // read library version
                                    {
                                        break;
                                    }
                                case "1A": // read bootloader version
                                    {
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
            catch (Exception e) { }
            
            //else { writeLineToConsole(sender.ToString()); } // if there is no recieve message, then just print whatever response is available.
        }
        #endregion
    }
}
