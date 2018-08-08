using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.IO;
using WcaInterfaceLibrary;
using System.Drawing;

namespace WcaDVConsole
{
    class rapidCyclingTest : Target, IInterfaceListener, ICommandListener
    {

        Thread startValidation, waitSleep;

        private bool ascii = false;
        private int deviceNum;
        private bool firstTime = true;
        public rerun[] myRerun;
        public int rerunIndex;
        public int ACKsleep = 250;
        public bool end = false, active = false, ValidationBreak = false;
        public List<Commands.DeviceData> device;
        public Commands commands;
        public int TotalTestTime;
        public int totalEndTime, validationOpen, validationClose;
        Daq digitalOut;



        public struct rerun // holds the data for any failed command //used for callback function
        {
            public string comport;
            public byte command;
            public byte[] data;
            public string name;
        }



        public void Run(List<Commands.DeviceData> d, Commands c)
        {
            digitalOut = new Daq(true);
            commands = c;
            device = d;
            deviceNum = d.Count;
            commands.startDebugmode = true;
            myResend = resetCommand; // initiating the delegate
            rerunIndex = 0;
            myRerun = new rerun[10];
            string time = getUTCtime();
            for (int a = 0; a < deviceNum; a++)
            {
                device[a].coilPF = new string[3];
                device[a].target = new Target(myResend);
                readAppID(a);
                readESN_HW(a);
                Thread.Sleep(3000); // need time for the device to respond and to save the device info into the variables             
                device[a].path = "EV_LogFile_" + device[a].esn + "_" + device[a].hw + "_" + device[a].appVer + "_" + time + ".csv";
                device[a].sw = new StreamWriter(device[a].path, true, Encoding.ASCII);
                device[a].sw.WriteLine("UTC Timestamp,Active Coil,Coil 0 Voltage,Coil 0 Pass/Fail,Coil 0 Temp," +
            "Coil 1 Voltage, Coil 1 Pass / Fail, Coil 1 Temp,Coil 2 Voltage,Coil 2 Pass/Fail,Coil 2 Temp," +
            "ADC Voltage,Coil RMS Current,PowerSupply Current,Chamber Temp, Command Ack Failures");
            }
            //270000 seconds
            programSetup(5, 3, 0, 30000);
            //programSetup(20, 2, 20, 2);
            //programSetup(90, 5, 90, 10);

        }

        private void programSetup(int validationTime1, int stopTime, int validationTime2, int loopNumber)
        {
            TotalTestTime = (validationTime1 + stopTime + validationTime2) * loopNumber;
            totalEndTime = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds + TotalTestTime;
            commands.myMainFrame.setupProgressBar(TotalTestTime);
            for (int a = 0; a < loopNumber; a++)
            {
                for (int b = 0; b < deviceNum; b++) device[b].sw.WriteLine(getUTCtime() + ",Start Loop " + (a + 1));
                startValidationMode(validationTime1);
                stopValidation(stopTime);
                //startValidationMode(validationTime2 - 5);
                //stopValidation(5);
                for (int b = 0; b < deviceNum; b++) device[b].sw.WriteLine(getUTCtime() + ",End Loop " + (a + 1));
            }
            endTest();
        }
        private void validationsStartup()
        {
            //int offSet = 0;
            //for (int a = 0; a < deviceNum; a++) // for each device we want to query and set some initial values for the log file and prepare for testing
            //{
            //    device[a].count = offSet; // initializing each devices offset                
            //    offSet -= 8; // each device will be 8 counts or 4 seconds apart from each other in the schedule     
            //    device[a].activeCoil = 2;
            //    readCADC(a);
            //    readVADC(a);
            //    readTemp(a);
            //    setCoil(a);
            //    setLED(a);
            //    device[a].enableVoltVeri = false;

            //}
            commands.writeLineToConsole("Statup Done");
        }

        private void startValidationMode(int time)
        {
            if (end) return;
            digitalOut.setHi(new bool[] { true, true, true });
            validationsStartup();
            commands.writeLineToConsole(getUTCtime() + " Start Validation");
            for (int a = 0; a < deviceNum; a++)
            {
                device[a].sw.WriteLine(getUTCtime() + ",Start Validation");
            }
            startValidation = new Thread(delegate ()
            {
                //RunCoilValidation();
            });
            startValidation.Name = "ValidationThread";
            startValidation.Start();
            if (sleep(time)) return;// if end == true then return
        }
        private void stopValidation(int time)
        {
            digitalOut.setLo(new bool[] { true, true, true });
            commands.writeLineToConsole(getUTCtime() + " Stop Validation");
            for (int a = 0; a < deviceNum; a++)
            {
                device[a].sw.WriteLine(getUTCtime() + ",Stop Validation");
            }
            if (startValidation != null)
            {
                ValidationBreak = true;
                validationClose++;
            }
            if (startValidation != null) startValidation.Join();
            ValidationBreak = false;
            //for (int a = 0; a < deviceNum; a++) stopCoils(a);
            if (sleep(time)) return; // if end == true then return
        }
        public void endTestFromMainFrame()
        {
            end = true;
        }
        public void endTest()
        {
            commands.writeLineToConsole(getUTCtime() + " End Validation");
            //if (ValidationTimer != null)
            //{
            //    ValidationTimer.Stop(); ValidationTimer.Dispose();
            //}
            //if (startValidation != null) startValidation.Join();
            //for (int a = 0; a < deviceNum; a++) stopCoils(a);
            while (active) Thread.Sleep(100);
            for (int a = 0; a < deviceNum; a++)
            {
                device[a].sw.Flush();
                device[a].sw.WriteLine(getUTCtime() + ",End Validation");
                device[a].sw.Close();
            }
            commands.myMainFrame.returnfromValidationMode();
            commands.myMainFrame.updateProgressBar(0, TotalTestTime);

        }
        //public void closeLog()
        //{
        //    for (int c = 0; c < deviceNum; c++) {  } // close serial port and close log file
        //}
        private bool sleep(int time)
        {
            int endTime = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds + time;
            int currentTime;
            while (endTime > (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds)
            {
                if (end) return true;
                currentTime = TotalTestTime - (totalEndTime - (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);
                commands.myMainFrame.updateProgressBar(currentTime, TotalTestTime);
                Thread.Sleep(1000);

            }
            return false;
        }
        public void resetCommand(string comport, byte command, byte[] data, string name)// this is the delegate call back function which we use to collect the info of any command
        {                                                                               // which responded with either a execution timeout or a neg ack. we put each failed command
            myRerun[rerunIndex].comport = comport;                                      // into a struct array and rerun the commands in the next execution of the check function.
            myRerun[rerunIndex].command = command;
            myRerun[rerunIndex].data = data;
            myRerun[rerunIndex].name = name;
            string myData = "";
            if (data.Length > 0) // this next section is just for printing the result of the resent command to the console
            {
                myData = data[0].ToString();
                for (int a = 1; a < data.Length; a++)
                {
                    myData += " " + data[a].ToString();
                }
            }
            commands.writeLineToConsole(getUTCtime() + " Resent " + name + " on " + comport + " Command: " + myRerun[rerunIndex].command.ToString() + " Data:[" + myData + "] ");

            rerunIndex++; // increment this in so the check function knows that there was a failed command and to rerun it.
        }

        public void resendCommand() // command called by the check funciton if there is a failed command. it repackages the command info and resends it.
        {

            for (int a = 0; a < rerunIndex; a++) //runs through all failed commands regardless if there is only 1 failed command
            {
                int currentIndex = 0;
                for (int b = 0; b < deviceNum; b++) // runs through all devices refardless if there is only 1 device
                {
                    if (device[b].comport.Equals(myRerun[a].comport)) currentIndex = b; // need to know what device index needs a command to be resent - so i compare comports
                }
                if (device[currentIndex].count < 58) // we don't want any repetitive commands from this coil loop to enter into the next coil loop to we stop sending commands for the last second of the loop.
                {
                    GeneralCommand rerunCommand = new GeneralCommand(device[currentIndex].m_si, 0x01, myRerun[a].command, myRerun[a].data, "resent command");
                    rerunCommand.RegisterListener(this); device[currentIndex].target.Queue(rerunCommand);
                    device[currentIndex].ackFailures = myRerun[a].name;
                    writeToLog(currentIndex, true);
                    device[currentIndex].ackFailures = "";
                }
            }
            rerunIndex = 0; // after all failed commands are resent, set this value back to 0
            myRerun = new rerun[20]; // also creat a new stuct array
        }

        private void RunCoilValidation() // creates a timer which calls a function every .5 seconds.
        {
            validationOpen++;
            while (!ValidationBreak) // timing loops determining how often to send the check() function.
            {
                double endTime = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds + 0.5; // get time 0.5 seconds from now
                //Check();

                while (endTime > (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds) // check current time to endTime and sleep if it isn't greater than end time.
                {
                    Thread.Sleep(10);
                }
            }
        }
        private void Check() // im not sure what the arguments here do, but they are necessary for the timing class
        {

            active = true;
            if (rerunIndex > 0) resendCommand();// check to see if any commands need to be resent, if so, then send any commands.            
            for (int a = 0; a < deviceNum; a++) // iterate through all devices schedules every 500ms to check if it is time for an event
            {
                int tempfail = 0;
                bool failMode = false;
                if (device[a].enableVoltVeri) // only check coil voltage if the coil has been set for 10 seconds.
                {
                    device[a].coilV = device[a].myDaq.getVolts(); // get coil voltages
                    for (int b = 0; b < 3; b++)
                    {
                        if (!((device[a].coilV[b] > 2.0) ^ (device[a].activeCoil == b))) // if the coil we set as active has more than 5 volts
                        {
                            device[a].coilPF[b] = "Pass";
                        }
                        else
                        {
                            tempfail++;
                            device[a].coilPF[b] = "Fail";
                            failMode = true; // if the wrong coil is on when it is not supposed to: then enter failure mode. this writes to the log much faster until it corrects itself.
                            if (globals.debug)
                            {
                                //commands.writeLineToConsole(getUTCtime() + " device " + device[a].esn + " failed to connect to coil " + device[a].activeCoil);
                                //device[a].sw.WriteLine(getUTCtime() + " device " + device[a].esn + " failed to connect to coil " + device[a].activeCoil);
                                //for (int c = 0; c < deviceNum; c++) { device[c].m_si.Close(); device[c].sw.Flush(); ; device[c].sw.Close(); } // close serial port and close log file
                                //commands.writeLineToConsole("Exiting Application");
                                //Environment.Exit(0);
                            }
                        }
                    }
                }
                if (failMode) // query temp, current, and voltage and log all other available info.
                {
                    readCADC(a);
                    readVADC(a);
                    readTemp(a);
                    writeToLog(a, false); // figure out a way to clear all variables so fresh ones are called at the time of logging
                }
                switch (device[a].count)
                {
                    case 0: // turn on new coil                        
                        {
                            if (device[a].failCount > 0) commands.myMainFrame.setESN(device[a].dutNumber, device[a].esn + "\nError Count: " + device[a].failCount, Color.Orange);
                            if (device[a].activeCoil == 0) device[a].activeCoil = 3;
                            device[a].activeCoil--;
                            setCoil(a);
                            setLED(a);
                            break;
                        }
                    case 8: // request device info and DAC info                        
                        {
                            readCADC(a);
                            readVADC(a);
                            readTemp(a);
                            break;
                        }
                    case 20:
                        {
                            device[a].enableVoltVeri = true; // allow the program to verify the proper coil is recieving voltage
                            break;                           // we wait 10 seconds here because it takes time for the device to make a secure connection to the coil.
                        }
                    case 21:
                        {
                            writeToLog(a, false);
                            break;
                        }
                    case 56: // request device info and DAC info again                        
                        {
                            readCADC(a);
                            readVADC(a);
                            readTemp(a);
                            break;
                        }
                    case 58:
                        {
                            writeToLog(a, false); // write to log all available information
                            device[a].enableVoltVeri = false; // since we are about to turn off the coil we want to stop checking for coil voltage
                            stopCoils(a);
                            break;
                        }
                    case 59:
                        {
                            device[a].flush += device[a].count + 1; // for flushing the buffer                            
                            device[a].count = -1; // reset count to -1 because of the count ++ below. but need to restart for the switch case only goes to 59 counts
                            break;
                        }
                }

                if (device[a].flush > 119) { device[a].sw.Flush(); device[a].flush = 0; } // flush buffer ever minute
                if (tempfail > 0)
                {
                    device[a].failCount += tempfail;
                    if (device[a].failCount > 0) commands.myMainFrame.setESN(device[a].dutNumber, device[a].esn + "\nError Count: " + device[a].failCount, Color.Red);
                }
                if (device[a].count++ > 59)
                    device[a].count = 0; // increment count for each device           
            }
            active = false;
        }
        public string getUTCtime()
        {
            string datePatt = @"M.d.yyyy-hh.mm.ss.ff";//date pattern
            DateTime saveUtcNow = DateTime.UtcNow;
            return saveUtcNow.ToString(datePatt);
        }
        public void readLED(int myDevice) // command to request the LED color
        {
            GeneralCommand readColor = new GeneralCommand(device[myDevice].m_si, 0x01, 0x0E, new byte[] { }, "read led color");
            readColor.RegisterListener(this); device[myDevice].target.Queue(readColor);

        }
        public void readCoil(int myDevice) // command to request which coil is active
        {
            GeneralCommand readactivecoil = new GeneralCommand(device[myDevice].m_si, 0x01, 0x06, new byte[] { 0x19 }, "read active coil");
            readactivecoil.RegisterListener(this); device[myDevice].target.Queue(readactivecoil);

        }
        public void setLED(int myDevice) // command to set LED color
        {
            byte[] color = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            switch (device[myDevice].activeCoil) // based on which coil is currently active - determine which color the LED should be.
            {
                case 0:
                    {
                        color[0] = 0xff;
                        color[1] = 0xff;
                        device[myDevice].ledColor = "red";
                        break;
                    }
                case 1:
                    {
                        color[2] = 0xff;
                        color[3] = 0xff;
                        device[myDevice].ledColor = "green";
                        break;
                    }
                case 2:
                    {
                        color[4] = 0xff;
                        color[5] = 0xff;
                        device[myDevice].ledColor = "blue";
                        break;
                    }
            }
            GeneralCommand writeColor = new GeneralCommand(device[myDevice].m_si, 0x01, 0x0C, new byte[] { color[0], color[1], color[2], color[3], color[5], color[5] }, "set led color");
            writeColor.RegisterListener(this); device[myDevice].target.Queue(writeColor);
        }
        public void setCoil(int myDevice) // set which coil should be active
        {
            GeneralCommand setactivecoil = new GeneralCommand(device[myDevice].m_si, 0x01, 0x06, new byte[] { 0x19, Convert.ToByte(device[myDevice].activeCoil) }, "set active coil");
            setactivecoil.RegisterListener(this); device[myDevice].target.Queue(setactivecoil);
        }
        public void stopCoils(int myDevice) // stop any active coil.
        {
            GeneralCommand stopcoils = new GeneralCommand(device[myDevice].m_si, 0x01, 0x06, new byte[] { 0x19, 0x0F }, "stop active coil"); // have to stop all coils first before setting new coils.
            stopcoils.RegisterListener(this); device[myDevice].target.Queue(stopcoils);
        }
        public void readVADC(int myDevice)// read adc count for voltage
        {
            GeneralCommand readVadc = new GeneralCommand(device[myDevice].m_si, 0x01, 0x04, new byte[] { 0x08 }, "read ADC Voltage");//"read runtime parameter: Requests the ADC count of the measured H - bridge input voltage.");
            readVadc.RegisterListener(this); device[myDevice].target.Queue(readVadc);
        }
        public void readCADC(int myDevice) // read coil current rms value
        {
            GeneralCommand readcadc = new GeneralCommand(device[myDevice].m_si, 0x01, 0x04, new byte[] { 0x0A }, "read coil current RMS");//"read runtime parameter: Requests the Coil RMS Current");
            readcadc.RegisterListener(this); device[myDevice].target.Queue(readcadc);
        }
        public void readTemp(int myDevice) // read temp values for all 3 coils
        {
            GeneralCommand readtemp = new GeneralCommand(device[myDevice].m_si, 0x01, 0x04, new byte[] { 0x11 }, "read coil temp");//"read runtime parameter: Requests the temperature values of all available sensors in degree celcius.");
            readtemp.RegisterListener(this); device[myDevice].target.Queue(readtemp); // read coil temps          
        }
        public void readAppID(int myDevice) // read the application ID
        {
            GeneralCommand readAppVer = new GeneralCommand(device[myDevice].m_si, 0x01, 0x1E, new byte[] { }, "read Application Version");
            readAppVer.RegisterListener(this); device[myDevice].target.Queue(readAppVer);
        }
        public void readESN_HW(int myDevice) // read the esn and hw version 
        {
            GeneralCommand readParaLE = new GeneralCommand(device[myDevice].m_si, 0x01, 0x03, new byte[] { 0x03, 0x01 }, "read ESN and HW");//"read general device parameters for ESN (little endian)");
            readParaLE.RegisterListener(this); device[myDevice].target.Queue(readParaLE);
        }
        private void writeToLog(int myDevice, bool resend) // append to the end of the log file
        {
            if (resend) // used only for if the command failed
            {
                device[myDevice].sw.WriteLine(getUTCtime() + "," + device[myDevice].activeCoil + "," + "," + "," + device[myDevice].coil0temp +
                "," + "," + "," + device[myDevice].coil1temp + "," + "," + "," +
                device[myDevice].coil2temp + "," + device[myDevice].adcV + "," + device[myDevice].adcC + "," + "," + device[myDevice].ackFailures);

                device[myDevice].ackFailures = "";
            }
            else // used for any other logging command.
            {
                device[myDevice].sw.WriteLine(getUTCtime() + "," + device[myDevice].activeCoil + "," + device[myDevice].coilV[0] + "," + device[myDevice].coilPF[0] + "," + device[myDevice].coil0temp +
                "," + device[myDevice].coilV[1] + "," + device[myDevice].coilPF[1] + "," + device[myDevice].coil1temp + "," + device[myDevice].coilV[2] + "," + device[myDevice].coilPF[2] + "," +
                device[myDevice].coil2temp + "," + device[myDevice].adcV + "," + device[myDevice].adcC + "," + ",");
            }


        }

        #region IInterfaceListener Members
        public void OnReceive(IInterface sender, byte[] data, int dataLength)
        {


        }// we have to have this interface so i have this function and since I am not using it i am leaving it empty
        #endregion

        #region ICommandListener Members
        public void OnNotification(ICommand sender) // this function recieves a verified message from the device which we can then parse and log.
        {
            if (sender.Name.Equals("KEEP_BL_RUNNING"))
            {
                if (sender.Result == WcaInterfaceCommandResult.PosAck) { }
            }
            else if (sender.isReceive)
            {
                byte[] data = sender.getData; // sender.getData gets byte array of the response telegram which we used to determine which command was sent and what the data response is.
                int deviceIndex = 0;
                string str = "";
                string str2 = "";
                if (ascii) str = System.Text.Encoding.Default.GetString(data);
                else str = WcaInterfaceLibrary.Converter.ByteArray2String(data);

                for (int a = 0; a < deviceNum; a++) { if (device[a].comport.Equals(sender.comport)) { deviceIndex = a; break; } } // sender.comport gets the comport of the device that responded with this message

                if (!string.IsNullOrEmpty(str))
                {

                    string[] response = str.Split(' '); // we make the string of data bytes and turn it into a string array to parse out the values we want
                    if (response[0].Equals("55") && response[3].Equals("2D") && response.Length > 4) //here we are checking if the data we got is one from our wireless charger, a POS ACK, and more than 4 bytes
                    {
                        commands.writeToConsole(getUTCtime() + " " + sender.Name + " Count " + device[deviceIndex].count + " Coil " + device[deviceIndex].activeCoil);
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
                                                device[deviceIndex].adcV = BitConverter.ToInt32(myData, 0).ToString();
                                                commands.writeToConsole(" Read ADC Voltage \t");
                                                break;
                                            }
                                        case "11": //readtemp
                                            {
                                                device[deviceIndex].coil0temp = Convert.ToInt32(data[6]).ToString();
                                                device[deviceIndex].coil1temp = Convert.ToInt32(data[7]).ToString();
                                                device[deviceIndex].coil2temp = Convert.ToInt32(data[8]).ToString();
                                                commands.writeToConsole(" Read Coil Temps \t");
                                                break;
                                            }
                                        case "0A": //read current coil ma RMS
                                            {
                                                byte[] myData = { data[6], data[7], 0x00, 0x00 };
                                                device[deviceIndex].adcC = BitConverter.ToInt32(myData, 0).ToString();
                                                commands.writeToConsole(" Read ADC Current \t");
                                                break;
                                            }
                                        case "19": //read active coil
                                            {
                                                if (Convert.ToInt32(data[6]) == device[deviceIndex].activeCoil)
                                                {
                                                    commands.writeToConsole(" Coil " + device[deviceIndex].activeCoil + " Confirmed \t");
                                                }
                                                else
                                                {
                                                    commands.writeToConsole(" Coil NOT Confirmed \t");
                                                    setCoil(deviceIndex);
                                                }
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
                                            commands.writeToConsole(" Set Coil " + device[deviceIndex].activeCoil + " on \t");
                                            break;
                                    }
                                    break;
                                }
                            case "1E": // read application version
                                {
                                    device[deviceIndex].appVer = (Convert.ToInt32(data[6]).ToString() + "." + Convert.ToInt32(data[7]).ToString()
                                        + "." + Convert.ToInt32(data[8]).ToString() + "." + Convert.ToInt32(data[9]).ToString());
                                    commands.writeToConsole(" Read App Version \t");
                                    break;
                                }
                            case "03": // read esn
                                {
                                    string[] fullesn = str.Split(' ');
                                    if (fullesn.Length == 22) // add exttra digit because there is an empty element at the end of this string array
                                    {
                                        string myesn = fullesn[7] + fullesn[8] + fullesn[9] + fullesn[10] + fullesn[11] + fullesn[12] + fullesn[13];
                                        string myhw = fullesn[15] + "." + fullesn[17];
                                        str2 = "ESN:" + myesn + ",HW:" + myhw;
                                        device[deviceIndex].esn = myesn;
                                        device[deviceIndex].hw = myhw;
                                        commands.writeToConsole(" Read ESN and HW \t");
                                    }
                                    else if (fullesn.Length == 20)
                                    {
                                        string myesn = fullesn[6] + fullesn[7] + fullesn[8] + fullesn[9] + fullesn[10] + fullesn[11] + fullesn[12] + fullesn[13];
                                        string myhw = fullesn[14] + "." + fullesn[15];
                                        str2 = "ESN:" + myesn + ",HW:" + myhw;
                                        device[deviceIndex].esn = myesn;
                                        device[deviceIndex].hw = myhw;
                                        commands.writeToConsole(" Read ESN and HW \t");
                                    }
                                    break;
                                }

                            case "0E": // Read LED color
                                {
                                    byte[] red = { data[5], data[6], 0x00, 0x00 }; //bitConverter requires at least 4 bytes.....
                                    byte[] green = { data[7], data[8], 0x00, 0x00 }; //bitConverter requires at least 4 bytes.....
                                    byte[] blue = { data[9], data[10], 0x00, 0x00 }; //bitConverter requires at least 4 bytes.....      
                                    if (BitConverter.ToInt32(red, 0) == 65535 && device[deviceIndex].ledColor.Equals("red"))
                                    {

                                        commands.writeToConsole(" Color " + device[deviceIndex].ledColor + " Confirmed \t");
                                    }
                                    else if (Convert.ToInt32(blue) == 65535 && device[deviceIndex].ledColor.Equals("green"))
                                    {

                                        commands.writeToConsole(" Color " + device[deviceIndex].ledColor + " Confirmed \t");
                                    }
                                    else if (Convert.ToInt32(green) == 65535 && device[deviceIndex].ledColor.Equals("blue"))
                                    {

                                        commands.writeToConsole(" Color " + device[deviceIndex].ledColor + " Confirmed \t");
                                    }
                                    else
                                    {
                                        commands.writeToConsole(" Color NOT Confirmed \t");
                                        setLED(deviceIndex);
                                    }
                                    break;
                                }
                            case "0C": // set led
                                {
                                    commands.writeToConsole(" Set Color " + device[deviceIndex].ledColor + " \t");
                                    break;
                                }
                            case "24": // set active application
                                {
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
                        commands.writeLineToConsole(" [" + str + "]");
                    }
                }

            }


        }

        #endregion
    }
}
