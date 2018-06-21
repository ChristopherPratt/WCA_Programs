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
        LEDframeforCustomers LEDffc;
        public List<Thread> commandThread = new List<Thread>();
        private StreamWriter sw;
        private MainFrame myMainFrame;
        private bool ascii = false;
        private Target m_target = null;
        private bool m_bootloader_ready = false;
        //public Daq myDaq;
        public bool daq = false, setGUIESN = false;
        public int coilTime = 1500;
        private string comport;
        

        public List<DeviceData> Devices;

        public class DeviceData // holds all the data needed for each DUT
        {
            public WcaInterfaceLibrary.SerialInterface m_si;
            public string comport, esn, hw, appVer, timeStamp, reqCoilID, coil0V, coil1V, coil2V, coil0PF, coil1PF, coil2PF, coil0temp, coil1temp, coil2temp, adcV, adcC, Temp, powSupC, chamberTemp, path, ledColor, ackFailures, dutNumber;
            public string[] coilPF, daqports;
            public int count, activeCoil, flush, baud;
            public double[] coilV;
            public StreamWriter sw;
            public Daq myDaq;
            public Target target;

            public bool fail, enableVoltVeri, bootloaderMode = false, programmingMode = false, wakeUp = true, startup = false, startEOL = false; //bool vars that tell if an ACK has been recieved for each command.

            //public bool[] failMode;

        }
        public Commands(MainFrame m, LEDframeforCustomers l)
        {
            LEDffc = l;
            myMainFrame = m;
            Devices = new List<DeviceData>();
        }
        public void initializeDUTLED(string comport)
        {
            if (Devices.Count == 1) removeDUT("DUT 0");
            DeviceData temp = new DeviceData();
            temp.dutNumber = "DUT 0";
            temp.comport = comport;
            temp.baud = 115200;
            try
            {
                if (temp.m_si != null) temp.m_si.Close();
                temp.m_si = new WcaInterfaceLibrary.SerialInterface(temp.comport, temp.baud, Parity.None, 8); // initiate serial connection to device
                LEDffc.updateBaud("115200");
            }
            catch (Exception e)
            {
                //writeLineToConsole("COM port could not be opened.");
                LEDffc.updateDeviceinfo("ComPort Unavailable");
                temp.fail = true;
                return;
            }
            
            //

            Thread openCom = new Thread(delegate ()
            {
                temp.m_si.Open(); // check if the comport is available and connected properly
            });
            openCom.Start();
            int intTemp = 0;
            bool boolTemp = false;
            while (true)
            {
                intTemp++;
                Thread.Sleep(50);
                if (temp.m_si.isOpen()) { boolTemp = true; break; }
                if (intTemp > 100) { boolTemp = false; break; }
            }
            if (!boolTemp)
            {
                LEDffc.updateDeviceinfo("COM port could not be opened.");
                //myMainFrame.setESN(temp.dutNumber, "COM Port Unavailable", Color.Red);
                //temp.fail = true;
                return;
            }


            //
            writeLineToConsole("Device connected at 115200 for programming mode only");
            LEDffc.updateDeviceinfo("Cycle Power to Connect");
            temp.programmingMode = true;

            temp.target = new Target();
            temp.m_si.RegisterListener(this);
            temp.startup = true;
            Devices.Add(temp);

           
            openCom.Join();

        }
        public void InitializeDUTforProgramming(string dutnum, string COMport)
        {
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




            //if (!temp.m_si.Open()) // check if the comport is available and connected properly
            //{
            //    writeLineToConsole("COM port could not be opened.");
            //    myMainFrame.setESN(temp.dutNumber, "COM Port Unavailable", Color.Red);
            //    temp.fail = true;
            //    return;
            //}
            //myMainFrame.enableConsole(true);
            //myMainFrame.run("");
            temp.target = new Target();
            temp.m_si.RegisterListener(this);
            Devices.Add(temp);
            openCom.Join();
        }
        public void InitializeDUT(string dutnum, string COMport, string programMode, int baud) // try to connect to the comport, if it fails return false;
        {
            checkifComPortExists(COMport, dutnum); // deletes duplicate duts
            if (programMode.Equals("Programming mode")) { InitializeDUTforProgramming(dutnum, COMport); return; }
            //CheckifDUTExist(dutnum);
            DeviceData temp = new DeviceData();
            temp.dutNumber = dutnum;
            temp.comport = COMport;
            temp.baud = baud;
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
            int intTemp = 0;
            bool boolTemp = false;
            while (true)
            {
                intTemp++;
                Thread.Sleep(50);
                if (temp.m_si.isOpen()) { boolTemp = true; break; }
                if (intTemp > 10) { boolTemp = false; break; }
            }
            if (!boolTemp)
            {
                writeLineToConsole("COM port could not be opened.");
                myMainFrame.setESN(temp.dutNumber, "COM Port Unavailable", Color.Red);
                temp.fail = true;
                return;
            }


            //if (!temp.m_si.Open()) // check if the comport is available and connected properly
            //{
            //    writeLineToConsole("COM port could not be opened.");
            //    myMainFrame.setESN(temp.dutNumber, "COM Port Unavailable", Color.Red);
            //    temp.fail = true;
            //    return;
            //}
            //myMainFrame.enableConsole(true);
            //myMainFrame.run("");
            setGUIESN = true;
            temp.target = new Target();
            temp.m_si.RegisterListener(this);
            try { temp.myDaq = new Daq(getDAQ(temp.dutNumber)); } catch (Exception e) { daq = true; writeLineToConsole("Daq not available for 'Read coil voltage' command."); } //initiate connection to the daq for querying the analog inputs 0 8 and 1


            Devices.Add(temp);
            runCommands("Read ESN (big endian)", Devices.Count - 1); // check if the device connected us responds properly
            openCom.Join();
        }

        public bool checkifComPortExists(string tempcomport, string tempdut) // cannot have 2 device with the same comport // we check if the comport being selected already exists, and if it does we delete the old device object and clear the dut list box it was on.
        {
            List<DeviceData> temp = new List<DeviceData>();
            if (Devices.Count == 0) return false;
            foreach (DeviceData dut1 in Devices)
            {
                if (dut1.comport.ToString().Equals(tempcomport)) // if new comport = existing comport
                {
                    if (dut1.dutNumber != tempdut) //if new dut = existing dut | yes: reset existing dut and remove from device list
                    {
                        myMainFrame.DUTReset(dut1.dutNumber);
                        //dut1.m_si.Close();
                        //dut1.m_si.Dispose();
                        //Devices.Remove(dut1);
                        return true;
                    }
                    else // close port and remove dut to allow for reset of dut - don't clear seleced item in comports list box
                    {
                        //myMainFrame.DUTReset(dut1.dutNumber);
                        dut1.m_si.Close();
                        dut1.m_si.Dispose();
                        Devices.Remove(dut1);
                        return true;
                    }
                }
                else if (dut1.dutNumber == tempdut)
                {
                    dut1.m_si.Close();
                    dut1.m_si.Dispose();
                    Devices.Remove(dut1);
                    return true;
                }
            }
            return false;
        }
        public void CheckifDUTExist(string dutnumtemp) // cannot have 2 devices in the same dut slot
        {
            bool matched = false;
            foreach (DeviceData dut1 in Devices)
            {
                if (dut1.dutNumber != dutnumtemp)
                {
                    matched = true;
                }
            }
            if (!matched)
            {
                foreach (DeviceData dut2 in Devices)
                {
                    if (dut2.dutNumber == dutnumtemp)
                    {
                        myMainFrame.DUTReset(dut2.dutNumber);
                        dut2.m_si.Close();
                        dut2.m_si.Dispose();
                        Devices.Remove(dut2);
                        break;
                    }
                }
            }
        }
        public void removeDUT(string dutnum)
        {
            foreach (DeviceData dut in Devices)
            {
                if (dut.dutNumber == dutnum)
                {
                    //myMainFrame.DUTReset(dutnum);
                    dut.m_si.Close();
                    dut.m_si.Dispose();
                    Devices.Remove(dut);
                    break;
                }
            }
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
            //myMainFrame.writeToConsole(myString);
        }
        public void writeLineToConsole(string myString)
        {
            //myMainFrame.writeToConsole(myString + "\n");
        }
        public void UploadApplication(WcaInterfaceLibrary.SerialInterface si, Target target, string full_file_name)
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
            PostProgrammingFrame ppf = new PostProgrammingFrame(this, myMainFrame);
            ppf.ShowDialog();
        }
        public void runCommands(string command, int deviceindex)
        {
            //if (commandThread != null)
            //{
            //    writeLineToConsole(commandThread.ThreadState.ToString());
            //    if (commandThread.IsAlive) commandThread.Abort();
            //    writeLineToConsole(commandThread.ThreadState.ToString());
            //}

            if (commandThread.Count > 1) commandThread[commandThread.Count - 2].Abort();
            commandThread.Add(new Thread(delegate ()
            {
                switch (command)
                {
                    case "Read application version": ReadApplicationVersion(deviceindex); break;
                    case "Read bootloader version": ReadBootloaderVersion(deviceindex); break;
                    case "Read bridge voltage": ReadBridgeVoltage(deviceindex); break;
                    case "Read capacitive sensor": ReadCapacitiveSensor(deviceindex); break;
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
                    case "Read power(mW)": ReadPower(deviceindex); break;
                    case "Read power loss(mW)": ReadPowerLoss(deviceindex); break;
                    case "Read protocol version": ReadProtocolVersion(deviceindex); break;
                    case "Read PWM communication": ReadPWMCommunication(deviceindex); break;
                    case "Read receive power": ReadReceivePower(deviceindex); break;
                    case "Read temperature(C)": ReadTemperature(deviceindex); break;
                    case "Read transmit power": ReadTransmitPower(deviceindex); break;
                    case "b": SetBootloader(deviceindex); break;// used for when the user first applies power - if you press "b" within 500ms you can put the device in bootloader mode for programming and or choosing EOL or DEBUG mode.
                    case "Set bridge voltage": SetBridgeVoltage(deviceindex); break;//Sets the desired bridge voltage in mV.
                    case "Set ESN": SetESN(deviceindex); break;
                    case "Set LED color": SetLEDColor(deviceindex); break;
                    case "a":
                    case "Set transmitter coil": SetTransmitterCoil(deviceindex); break;
                    case "Set Vrail MAX": SetVrailMAX(deviceindex); break;
                    case "Set Vrail MIN": SetVrailMIN(deviceindex); break;
                    case "Start EOL": StartEOL(deviceindex); break; // used to start EOL mode in 9600 when in bootloader mode (bootloader mode must be entered while in 115200 and by pressing 'b' 100ms after power up)
                    case "Start charging": StartCharging(deviceindex); break;
                    case "Start debug": StartDebug(deviceindex); break;//used to start debugger mode in the device when in bootloader mode  (bootloader mode must be entered while in 115200 and by pressing 'b' 100ms after power up)
                    case "Stop charging": StopCharging(deviceindex); break;
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
        }
        public void ReadESNlittleEndian(int DeviceIndex)
        {
            GeneralCommand readParaLE = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x03, new byte[] { 0x02, 0x01 }, "read general device parameters for ESN (little endian)");
            readParaLE.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(readParaLE);
        }
        public void ReadLEDColor(int DeviceIndex)
        {
            try
            {
                GeneralCommand readColor = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x0E, new byte[] { }, "Read the LED color");
                readColor.RegisterListener(this);
                Devices[DeviceIndex].target.Queue(readColor);
            }
            catch(Exception e) {}            
        }
        public void ReadLibraryVersion(int DeviceIndex)
        {
            GeneralCommand libver = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x18, new byte[] { }, "read freescale library version");
            libver.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(libver);
        }
        public void ReadPower(int DeviceIndex)
        {
            GeneralCommand readpow = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x04, new byte[] { 0x01 }, "read runtime parameter: Requests the power value the receiver has been reported in mW.");
            readpow.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(readpow);
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
            GeneralCommand transpower = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x04, new byte[] { 0x06 }, "read transmit power");
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

        public void SetESN(int DeviceIndex)
        {
            writeToConsole("\nPlease enter ESN: ");
            string inputKey;
            //writeToConsole("Press \'#\' to change HW version.");
            int esnCount = 0;
            int esnVal = 0;
            double hwVal = 0;
            string hwvers = "";
            string[] hwArray;
            int[] esnArray = new int[13]; // 13 is the number of characters in the ESN serial


            inputKey = myMainFrame.waitForEntry(); if (inputKey.Equals("abortThread")) { writeLineToConsole("\nCommand canceled"); return; } // allows the typing or scanning in the ESN and automatically moving to ask for writing to OTP



            bool isInt = Int32.TryParse(inputKey, out esnVal); // checks if input is numerical
            if (isInt)
            {
                esnArray[esnCount] = esnVal; // populates array containing the ESN digit by digit automatically as the user enters it.
                esnCount++;
            }
            else
            {
                writeToConsole("\nPlease enter numerical digits only.\n");
                for (int a = 0; a < esnCount; a++) writeToConsole(esnArray[a].ToString());
            }

            if (esnCount != 13) { writeLineToConsole("\nExiting ESN and HW writing mode - too many characters"); return; }  // exits out of function because the user wants to exit this function.

            writeToConsole("\nPlease enter HW revision. (you must enter a decimal such as 1.2 where 1 is the major version and 2 is the minor version):  ");
            while (true) // make sure that the user is entering decimals and at least one period
            {
                hwvers = myMainFrame.waitForEntry(); if (hwvers.Equals("abortThread")) { writeLineToConsole("\nCommand canceled"); return; }
                bool myDigitP = double.TryParse(hwvers, out hwVal);
                if (!myDigitP)
                {
                    writeLineToConsole("\nPlease enter Numerical Digits Only");
                    writeToConsole("\nPlease enter HW revision. (you must enter a decimal such as 1.2 where 1 is the major version and 2 is the minor version): ");
                    continue;
                }
                else
                {
                    hwArray = hwvers.Split('.');
                    int majorV;
                    int minorV;
                    int.TryParse(hwArray[0], out majorV);
                    int.TryParse(hwArray[1], out minorV);
                    if (hwArray.Length == 1)
                    {
                        writeLineToConsole("\nA decimal number must contain a period.");
                        writeToConsole("\nPlease enter HW revision. (you must enter a decimal) ((ex. 1.1)): ");
                    }
                    else
                    {
                        if (majorV > 255 || minorV > 255) writeLineToConsole("\nThe major version and minor version must be between 0-255.");
                        else if (majorV < 0 || minorV < 0) writeLineToConsole("\nThe major version and minor version must not be negative.");
                        else break;
                    }

                }
            }

            writeToConsole("\nESN: "); for (int a = 0; a < 13; a++) writeToConsole(esnArray[a].ToString()); writeToConsole(" HW: " + hwvers + "\n"); // display what is about to be programmed
            writeLineToConsole("Press \'y\' to accept or \'n\' to exit write mode.");
            inputKey = myMainFrame.waitForEntry(); if (inputKey.Equals("abortThread")) { writeLineToConsole("\nCommand canceled"); return; }
            if (inputKey == "n" || inputKey == "N")
            {
                writeLineToConsole("\nExiting ESN and HW writing mode");
                return;
            }
            if (inputKey == "y" || inputKey == "Y")
            {
                byte[] inputArray = new byte[14]; // 14 is the number of bytes in the command for writing the ESN and the HW version
                int inputIndex = 7; // the number of bytes necessary to populate the ESN alone
                                    // hard populating certain parts of the inputArray that can't be parsed by the for loop im about to use
                inputArray[0] = 0x02; //little endianess
                inputArray[1] = 0x01; // write to serial number
                                      // elements 2 through 8 (7 total) are used for the ESN. written literally in hex in little endian
                for (int a = 1; a < 12; a += 2)
                {
                    inputArray[inputIndex] = Convert.ToByte((esnArray[a] * 16) + esnArray[a + 1]); // convert the ESN in to Literal HEX in little endian
                    inputIndex--;
                }
                // hard populating certain parts of the inputArray that can't be parsed by the for loop I used.
                inputArray[8] = Convert.ToByte(esnArray[0]);
                inputArray[9] = 0x00; // separator byte between ESN and HW 

                int primaryHW;
                int subHW;
                Int32.TryParse(hwArray[0], out primaryHW); // convert string array elements to int which can then be converted to byte
                Int32.TryParse(hwArray[1], out subHW);
                inputArray[10] = Convert.ToByte(subHW); // sub version
                inputArray[11] = 0x00;
                inputArray[12] = Convert.ToByte(primaryHW); ; //primary version// HW version here. supposed for rev 1.0 for elements 10 through 13 in little endian
                inputArray[13] = 0x00;

                GeneralCommand writeGenPara = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x07, inputArray, "Write general device parameters");
                //GeneralCommand writeGenPara = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x07, new byte[] { 0x02, 0x01, 0x04, 0x00, 0x55, 0x73, 0x02, 0x74, 0x09, 0x00, 0x00, 0x00, 0x01, 0x00 }, "Write general device parameters");
                writeGenPara.RegisterListener(this);
                Devices[DeviceIndex].target.Queue(writeGenPara);
            }

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
            writeToConsole("\nPress a to enter hex values for red, green, and blue.");
            writeToConsole("\nPress b to enter percentage values for red, green, and blue.");
            writeToConsole("\nPress c to select pre-programmed colors\n");

            string colorKey = myMainFrame.waitForEntry(); if (colorKey.Equals("abortThread")) { writeLineToConsole("\nCommand canceled"); return; }
            if (colorKey == "A" || colorKey == "a")
            {
                UInt16 redVal, greenVal, blueVal;
                writeLineToConsole("\nEnter Red of RGB (0x0000):");
                string redString = myMainFrame.waitForEntry(); if (redString.Equals("abortThread")) { writeLineToConsole("\nCommand canceled"); return; }
                if (!UInt16.TryParse(redString, System.Globalization.NumberStyles.AllowHexSpecifier,
                    System.Globalization.NumberFormatInfo.InvariantInfo, out redVal))
                {
                    writeLineToConsole("Invalid Red Value, can't perform command");
                    return;
                }
                writeLineToConsole("Enter Green of RGB (0x0000):");
                string greenString = myMainFrame.waitForEntry(); if (greenString.Equals("abortThread")) { writeLineToConsole("\nCommand canceled"); return; }
                if (!UInt16.TryParse(greenString, System.Globalization.NumberStyles.AllowHexSpecifier,
                    System.Globalization.NumberFormatInfo.InvariantInfo, out greenVal))
                {
                    writeLineToConsole("Invalid Green Value, can't perform command");
                    return;
                }
                writeLineToConsole("Enter Blue of RGB (0x0000):");
                string blueString = myMainFrame.waitForEntry(); if (blueString.Equals("abortThread")) { writeLineToConsole("\nCommand canceled"); return; }
                if (!UInt16.TryParse(blueString, System.Globalization.NumberStyles.AllowHexSpecifier,
                    System.Globalization.NumberFormatInfo.InvariantInfo, out blueVal))
                {
                    writeLineToConsole("Invalid Blue Value, can't perform command");
                    return;
                }
                byte[] redArray = BitConverter.GetBytes(redVal);
                byte[] greenArray = BitConverter.GetBytes(greenVal);
                byte[] blueArray = BitConverter.GetBytes(blueVal);

                writeLineToConsole(string.Format("Writing Red:{0:X}{1:X} Green:{2:X}{3:X} Blue:{4:X}{5:X}",
                    redArray[0], redArray[1], greenArray[0], greenArray[1], blueArray[0], blueArray[1]));

                GeneralCommand writeColor = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x0C, new byte[] { redArray[0], redArray[1], greenArray[0], greenArray[1], blueArray[0], blueArray[1] }, "set led color");
                writeColor.RegisterListener(this);
                Devices[DeviceIndex].target.Queue(writeColor);
                writeLineToConsole("\nCommand executed, returned to main menu\n");
                return;
            }

            if (colorKey == "B" || colorKey == "b")
            {
                double redValp, greenValp, blueValp;
                bool[] myDigit = new bool[3];
                writeLineToConsole("\nEnter percentage of Red of RGB:");
                string redString = myMainFrame.waitForEntry(); if (redString.Equals("abortThread")) { writeLineToConsole("\nCommand canceled"); return; }
                myDigit[0] = double.TryParse(redString, out redValp);
                writeLineToConsole("Enter percentage of Green of RGB:");
                string greenString = myMainFrame.waitForEntry(); if (greenString.Equals("abortThread")) { writeLineToConsole("\nCommand canceled"); return; }
                myDigit[1] = double.TryParse(greenString, out greenValp);
                writeLineToConsole("Enter percentage of Blue of RGB:");
                string blueString = myMainFrame.waitForEntry(); if (blueString.Equals("abortThread")) { writeLineToConsole("\nCommand canceled"); return; }
                myDigit[2] = double.TryParse(blueString, out blueValp);

                bool myExit = false;
                for (int d = 0; d < 3; d++)
                {
                    if (!myDigit[d])
                    {
                        writeToConsole("\nPlease enter numerical digits only. Returned to main menu.");
                        myExit = true;
                        return;
                    }
                }
                if (myExit) return;

                writeLineToConsole("\n" + redValp + " " + greenValp + " " + blueValp);
                redValp = (redValp * .01) * 65535.0;
                greenValp = (greenValp * .01) * 65535.0;
                blueValp = (blueValp * .01) * 65535.0;



                int myRedi = Convert.ToInt32(redValp);
                int myGreeni = Convert.ToInt32(greenValp);
                int myBluei = Convert.ToInt32(blueValp);

                byte[] redArray = BitConverter.GetBytes(myRedi);
                byte[] greenArray = BitConverter.GetBytes(myGreeni);
                byte[] blueArray = BitConverter.GetBytes(myBluei);
                writeLineToConsole("\n" + myRedi + " " + myGreeni + " " + myBluei);
                writeLineToConsole(string.Format("Writing Red:{0:X}{1:X} Green:{2:X}{3:X} Blue:{4:X}{5:X}",
                    redArray[0], redArray[1], greenArray[0], greenArray[1], blueArray[0], blueArray[1]));

                GeneralCommand writeColor = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x0C, new byte[] { redArray[0], redArray[1], greenArray[0], greenArray[1], blueArray[0], blueArray[1] }, "set led color");
                writeColor.RegisterListener(this);
                Devices[DeviceIndex].target.Queue(writeColor);


            }
            if (colorKey == "C" || colorKey == "c")
            {
                writeToConsole("\nPress a for red.");
                writeToConsole("\npress b for green.");
                writeToConsole("\nPress c for blue.");
                writeLineToConsole("\nPress d for amber.");




                double myRed, myBlue, myGreen;
                while (true)
                {
                    string preColorKey = myMainFrame.waitForEntry(); if (preColorKey.Equals("abortThread")) { writeLineToConsole("\nCommand canceled"); return; }
                    if (preColorKey == "A" || preColorKey == "a")
                    {
                        myRed = 100;
                        myGreen = 0;
                        myBlue = 0;
                        break;
                    }
                    else if (preColorKey == "B" || preColorKey == "b")
                    {
                        myRed = 0;
                        myGreen = 100;
                        myBlue = 0;
                        break;
                    }
                    else if (preColorKey == "C" || preColorKey == "c")
                    {
                        myRed = 0;
                        myGreen = 16.67;
                        myBlue = 83.33;
                        break;
                    }
                    else if (preColorKey == "D" || preColorKey == "d")
                    {
                        myRed = 83.33;
                        myGreen = 16.67;
                        myBlue = 0;
                        break;
                    }
                    else
                    {
                        writeLineToConsole("\nPlease enter a valid option from the menu.\n");
                        writeToConsole("\nPress a for red.");
                        writeToConsole("\npress b for green.");
                        writeToConsole("\nPress c for blue.");
                        writeLineToConsole("\nPress d for amber.");
                    }
                }

                writeLineToConsole("\nPlease enter the percentage of desired intensity and press enter.");
                double intensity;
                string temp3 = myMainFrame.waitForEntry(); if (temp3.Equals("abortThread")) { writeLineToConsole("\nCommand canceled"); return; }
                bool myDigitP = double.TryParse(temp3, out intensity);
                if (!myDigitP)
                {
                    writeLineToConsole("\nPlease enter numerical digits only. Returned to main menu.");
                    return;
                }
                intensity = intensity * .01;
                myRed = intensity * (myRed * .01) * 65535;
                myGreen = intensity * (myGreen * .01) * 65535;
                myBlue = intensity * (myBlue * .01) * 65535;

                int myRedi = Convert.ToInt32(myRed);
                int myGreeni = Convert.ToInt32(myGreen);
                int myBluei = Convert.ToInt32(myBlue);

                byte[] redArray = BitConverter.GetBytes(myRedi);
                byte[] greenArray = BitConverter.GetBytes(myGreeni);
                byte[] blueArray = BitConverter.GetBytes(myBluei);

                writeLineToConsole(string.Format("Writing Red:{0:X}{1:X} Green:{2:X}{3:X} Blue:{4:X}{5:X}",
                    redArray[0], redArray[1], greenArray[0], greenArray[1], blueArray[0], blueArray[1]));

                GeneralCommand writeColor = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x0C, new byte[] { redArray[0], redArray[1], greenArray[0], greenArray[1], blueArray[0], blueArray[1] }, "set led color");
                writeColor.RegisterListener(this);
                Devices[DeviceIndex].target.Queue(writeColor);

            }

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
            myMainFrame.updateProgrammingMode("End of line test mode");
            GeneralCommand set_app = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x24, new byte[] { 0x02, 0x01, 0x00 }, "Start user application");
            set_app.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(set_app);
            Thread.Sleep(1000);
            Devices[DeviceIndex].baud = 9600;
            Devices[DeviceIndex].m_si.Close();
            Devices[DeviceIndex].m_si = new WcaInterfaceLibrary.SerialInterface(Devices[0].comport, Devices[DeviceIndex].baud, Parity.None, 8); // initiate serial connection to device
            Devices[DeviceIndex].m_si.Open();
            setGUIESN = true;
            ReadESNbigEndian(DeviceIndex);
        }
        public void StartEOL(int DeviceIndex)
        {
            myMainFrame.updateProgrammingMode("End of line test mode");
            Devices[DeviceIndex].startEOL = true;
            GeneralCommand set_app_eol = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x24, new byte[] { 0x02, 0x07, 0x00 }, "Start End of Line Mode");
            set_app_eol.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(set_app_eol);
            //Thread.Sleep(1000);
            //Devices[DeviceIndex].baud = 9600;
            //Devices[DeviceIndex].m_si.Close();
            //Devices[DeviceIndex].m_si = new WcaInterfaceLibrary.SerialInterface(Devices[0].comport, Devices[DeviceIndex].baud, Parity.None, 8); // initiate serial connection to device
            //Devices[DeviceIndex].m_si.Open();
            //setGUIESN = true;
            //ReadESNbigEndian(DeviceIndex);
        }
        public void StopCharging(int DeviceIndex)
        {
            GeneralCommand silencemode = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x3B, new byte[] { 0x02 }, "stop charging");
            silencemode.RegisterListener(this);
            Devices[DeviceIndex].target.Queue(silencemode);
        }
        ////////////////////////////////////// SETS //////////////////////////////////////

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
            int DeviceIndex;
            string str;
            if (ascii) str = System.Text.Encoding.Default.GetString(data);
            else str = WcaInterfaceLibrary.Converter.ByteArray2String(data);
            for (int a = 0; a < Devices.Count; a++)
            {
                if (Devices[a].comport.Equals(sender.GetName()))
                {
                    DeviceIndex = a;
                    if (Devices[DeviceIndex].baud == 115200 && Devices[DeviceIndex].wakeUp)
                    {
                        //if ( str.Equals("55 09 00 02 26 01 00 00 00 00 94 7B"))
                        if (data.Length > 6)
                        //if (data[4] == 0x26 && data[5] == 0x01)
                        {
                            Devices[DeviceIndex].wakeUp = false;
                            Devices[DeviceIndex].startup = true;
                            byte[] cmdb = new byte[3];
                            cmdb[0] = 0x01;
                            cmdb[1] = 0x03;
                            cmdb[2] = 0x7D;        
                            
                            GeneralCommand set_bl = new GeneralCommand(Devices[DeviceIndex].m_si, 0x01, 0x24, cmdb, "keep bootloader running");
                            set_bl.RegisterListener(this);
                            Devices[DeviceIndex].target.Queue(set_bl);
                            //LEDffc.updateDeviceinfo("Connected");
                            //if (myMainFrame.ProgramMode.Equals("Programming mode"))
                            //{
                            //    //Thread.Sleep(2000);
                            //    //UploadApplication(Devices[0].m_si, Devices[0].target, myMainFrame.Firmware);
                            //}
                        }
                    }
                    break;
                }
            } // sender.comport gets the comport of the device that responded with this message
            // writeLineToConsole(str);            
        }


        #endregion

        #region ICommandListener Members
        public void OnNotification(ICommand sender) // this function recieves a verified message from the device which we can then parse and log.
        {
            int DeviceIndex;
            writeLineToConsole(DateTime.Now.ToShortTimeString() + " " + sender.ToString());
            if (sender.Name.Equals("KEEP_BL_RUNNING"))
            {
                if (sender.Result == WcaInterfaceCommandResult.PosAck) { }
            }
            if (sender.Result == WcaInterfaceCommandResult.ExecutionTimeout && setGUIESN) // for setting the gui to match dut to comport
            {
                //myMainFrame.setESN(Devices[].dutNumber, "Device Unavailable", Color.Red);
                //setGUIESN = false;
            }
            else if (sender.isReceive)
            {
                byte[] data = sender.getData; // sender.getData gets byte array of the response telegram which we used to determine which command was sent and what the data response is.
                int deviceIndex = 0;
                string str = "";
                string str2 = "";
                if (ascii) str = System.Text.Encoding.Default.GetString(data);
                else str = WcaInterfaceLibrary.Converter.ByteArray2String(data);

                for (int a = 0; a < Devices.Count; a++)
                {
                    if (Devices[a].comport.Equals(sender.comport))  // sender.comport gets the comport of the device that responded with this message
                    {
                        DeviceIndex = a;

                        if (!string.IsNullOrEmpty(str))
                        {
                            string[] response = str.Split(' '); // we make the string of data bytes and turn it into a string array to parse out the values we want
                            if (sender.Result == WcaInterfaceCommandResult.NegAck && setGUIESN) // for setting the gui to match dut to comport
                            {                                                                   // if repsonse is NegAck then send "no esn" to gui
                                if (response[4] == "03")
                                {
                                    myMainFrame.setESN(Devices[DeviceIndex].dutNumber, "No ESN", Color.SkyBlue);
                                    setGUIESN = false;
                                }
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
                                    case "1E": // read application version
                                        {
                                            writeLineToConsole("Read App Version \t" + Convert.ToInt32(data[6]).ToString() + "." + Convert.ToInt32(data[7]).ToString()
                                                + "." + Convert.ToInt32(data[8]).ToString() + "." + Convert.ToInt32(data[9]).ToString());
                                            break;
                                        }
                                    case "03": // read esn
                                        {
                                            switch (response[5])
                                            {
                                                case "02": // little endian
                                                    {
                                                        string[] fullesn = str.Split(' ');
                                                        string myesn = fullesn[12] + fullesn[11] + fullesn[10] + fullesn[9] + fullesn[8] + fullesn[7] + fullesn[6];
                                                        string myhw = fullesn[14] + "." + fullesn[16];
                                                        str2 = "ESN:" + myesn + ",HW:" + myhw;
                                                        writeLineToConsole(" Read ESN and HW \tESN: " + myesn + " HW: " + myhw);
                                                        if (setGUIESN)
                                                        {
                                                            myMainFrame.setESN(Devices[DeviceIndex].dutNumber, myesn, Color.LightBlue);
                                                            setGUIESN = false;
                                                        }

                                                        break;
                                                    }
                                                case "03": // big endian
                                                    {
                                                        string[] fullesn = str.Split(' ');
                                                        string myesn = fullesn[7] + fullesn[8] + fullesn[9] + fullesn[10] + fullesn[11] + fullesn[12] + fullesn[13];
                                                        string myhw = fullesn[15] + "." + fullesn[17];
                                                        str2 = "ESN:" + myesn + ",HW:" + myhw;
                                                        writeLineToConsole(" Read ESN and HW \tESN: " + myesn + " HW: " + myhw);
                                                        if (setGUIESN)
                                                        {
                                                            myMainFrame.setESN(Devices[DeviceIndex].dutNumber, myesn, Color.LightBlue);
                                                            setGUIESN = false;
                                                        }
                                                        if (Devices[0].startEOL)
                                                        {
                                                            if (LEDffc.Version.Equals("IGNORE ESN CYAN") || LEDffc.Version.Equals("IGNORE ESN YELLOW")) LEDffc.updateDeviceinfo("Connected ");
                                                            else LEDffc.updateDeviceinfo("Connected \nESN: " + myesn);

                                                            Devices[0].startEOL = false;
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
                                            LEDffc.updateGUI(red,green,blue);
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
                                            if (myMainFrame.ProgramMode.Equals("Programming mode")) Devices[0].bootloaderMode = true;
                                            if (Devices[0].startup)                                                
                                            {
                                                //Thread.Sleep(250);
                                                runCommands("Start EOL", 0);
                                                Devices[0].startup = false;                                                
                                            }
                                            if (Devices[0].startEOL)
                                            {
                                                Thread.Sleep(250);
                                                Devices[DeviceIndex].baud = 9600;
                                                LEDffc.updateBaud("9600");
                                                Devices[DeviceIndex].m_si.Close();
                                                Devices[DeviceIndex].m_si = new WcaInterfaceLibrary.SerialInterface(Devices[0].comport, Devices[DeviceIndex].baud, Parity.None, 8); // initiate serial connection to device
                                                Devices[DeviceIndex].m_si.Open();
                                                setGUIESN = true;
                                                ReadESNbigEndian(DeviceIndex);
                                            }
                                            
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

                                    case "07": // set esn and hw
                                        {
                                            break;
                                        }

                                }

                            }
                        }
                        break;
                    }
                }

            }
            else
            {
                writeLineToConsole(sender.ToString()); // if there is no recieve message, then just print whatever response is available.
            } 


        }

        #endregion
    }
}
