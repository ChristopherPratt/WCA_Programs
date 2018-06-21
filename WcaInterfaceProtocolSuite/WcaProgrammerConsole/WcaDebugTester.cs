using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.IO;
using WcaInterfaceLibrary;

namespace WcaDVConsole
{
    class WcaDebugTester : ICommandListener, IInterfaceListener
    {
        DeviceContext myrevlisten;
        private StreamWriter sw;
        private bool ascii = false;
        private WcaInterfaceLibrary.SerialInterface m_si = null;
        private Target m_target = null;
        private bool m_bootloader_ready = false;
        public Daq myDaq;
        public bool daq = false;
        public int coilTime = 1500;
        public WcaDebugTester()
        {
            sw = new StreamWriter("WcaDebugTester.log", true, Encoding.ASCII);
        }

        public void Run(string comport, string target_file)
        {
            //myrevlisten = new DeviceContext();
            try { myDaq = new Daq(new string[] { "0", "8", "1" }); } catch (Exception e) { daq = true; Console.WriteLine("Daq not available for readcoilv command."); } //initiate connection to the daq for querying the analog inputs 0 8 and 1

            bool abort = false;
            m_si = new WcaInterfaceLibrary.SerialInterface(comport, globals.baud, Parity.None, 8); // initiate serial connection to device
            m_target = new Target();
            m_si.RegisterListener(this);

            if (!m_si.Open()) // check if the comport is available and connected properly
            {
                Console.WriteLine("COM port could not be opened.");
                return;
            }

            Console.WriteLine("Application running.");

            //KeepBootloaderAlive();

            byte on_off = 0x01;
            string[] keyCommand = new string[12];
            int commandIndex = -1;
            while (!abort)
            {
                commandIndex++;// increment after each keystroke
                ConsoleKeyInfo key;
                key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        abort = true;
                        Console.WriteLine("\nAborted program. Returning to Command Console.");
                        break;

                    case ConsoleKey.Backspace:
                        if (--commandIndex < 0) // backspace to be at the proper index to remove entered character // just in case you are already at 0, don't decrement because we don't want a negative index
                        {
                            keyCommand = new string[12];
                            commandIndex = 0;
                        }
                        keyCommand[commandIndex] = null;   // remove unwanted character               
                        Console.Write("\r                       "); // clear line so there isn't garbage to overwrite.
                        Console.Write("\r" + string.Join("", keyCommand)); --commandIndex;//reprint the new line without the unwanted character and then decrement again to show proper index for new amount of characters
                        continue;
                }

                if (!Char.IsLetter(key.KeyChar)) // prevent the user from entering characters that aren't letters including the command keys like enter and ctrl and shift.
                {
                    commandIndex--;//don't increment if user enters invalid character
                    continue;// skip all the rest of the test because ya
                }




                if (commandIndex == keyCommand.Length-1) // make sure the user doesn't enter more characters than necessary
                {
                    Console.WriteLine("\nYou have entered too many characters for any valid command. Please try again.");
                    commandIndex = 0;
                    keyCommand = new string[12];
                    keyCommand[0] = "h";

                }
                else
                {
                    keyCommand[commandIndex] = key.KeyChar.ToString(); //saving the new key character into a new location
                    Console.Write("\r                       ");
                    Console.Write("\r" + string.Join("", keyCommand));
                }

                switch (string.Join("", keyCommand).ToLower()) // join returns a string of the combined string array of keyCommand[]                           
                {
                    //                                 Transmit Telegram
                    //offset:   0        1       2          3              4       5   6   7         8  9
                    //example: 55   |    07     00   |     64         |   24  |   02   01  00    |   36 4A  | //startapp command
                    //     |Sync Val| message length |Source/Dest/Code|Command|       Data       | Checksum |  //data can contain subcommand info



                    //                                 Response Telegram
                    //offset:   0        1       2          3              4       5   6       7  8
                    //example: 55   |    06     00   |2D (2f=negack)  |   01  |   01   13 |   36 4A  | //readpro command
                    //     |Sync Val| message length |Source/Dest/Code|Command|    Data   | Checksum |  //data can contain subcommand info

                    case "startapp": //used to start debugger mode in the device when in bootloader mode  (bootloader mode must be entered while in 115200 and by pressing 'b' 100ms after power up)
                        GeneralCommand set_app = new GeneralCommand(m_si, 0x01, 0x24, new byte[] { 0x02, 0x01, 0x00 }, "Start user application");
                        set_app.RegisterListener(this);
                        m_target.Queue(set_app);
                        Thread.Sleep(1000);
                        globals.baud = 9600;
                        m_si.Close();
                        m_si = new WcaInterfaceLibrary.SerialInterface(comport, globals.baud, Parity.None, 8); // initiate serial connection to device
                        m_si.Open();
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "starteol": // used to start EOL mode in 9600 when in bootloader mode (bootloader mode must be entered while in 115200 and by pressing 'b' 100ms after power up)
                        GeneralCommand set_app_eol = new GeneralCommand(m_si, 0x01, 0x24, new byte[] { 0x02, 0x07, 0x00 }, "Start End of Line Mode");
                        set_app_eol.RegisterListener(this);
                        m_target.Queue(set_app_eol);
                        Thread.Sleep(1000);
                        globals.baud = 9600;
                        m_si.Close();
                         m_si = new WcaInterfaceLibrary.SerialInterface(comport, globals.baud, Parity.None, 8); // initiate serial connection to device
                        m_si.Open();
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;


                    case "b": // used for when the user first applies power - if you press "b" within 500ms you can put the device in bootloader mode for programming and or choosing EOL or DEBUG mode.
                        byte[] cmdb = new byte[3];
                        cmdb[0] = 0x01;
                        cmdb[1] = 0x03;
                        cmdb[2] = 0x7D;
                        GeneralCommand set_bl = new GeneralCommand(m_si, 0x01, 0x24, cmdb, "keep bootloader running");
                        //Console.WriteLine("CMD:B:"+set_bl.ToString());
                        set_bl.RegisterListener(this);
                        m_target.Queue(set_bl);
                        m_target.Wait();
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "program": // for programming new firmware on the device
                        //string user_app = @"f:\projects\svn_projects\0_31X_P0517_moray_ruby1_2\trunk\development\software\releases\wca\release\MO_WC_11_1_8_6-10894\MO_WC_11_1_8_6.S19";
                        UploadApplication(m_si, m_target, target_file);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "h": // help menu
                        Console.WriteLine("\nesc = abort");
                        Console.WriteLine("b = keep bootloader running for programming mode");
                        Console.WriteLine("program = program device with new firmware");
                        Console.WriteLine("startapp = Start user application");
                        Console.WriteLine("starteol = Start EOL application");
                        Console.WriteLine("readpwm = PWM communication");
                        Console.WriteLine("readadcv = Requests the ADC count of the measured H - bridge input voltage.");
                        Console.WriteLine("readadcc = Requests the ADC count of the measured input H - bridge current.");
                        Console.WriteLine("readcurms = Requests the coil current RMS");
                        Console.WriteLine("readpow = Requests the power value the receiver has been reported in mW.");
                        Console.WriteLine("readpwrlos = Requests the internal system losses in mW.");
                        Console.WriteLine("readcurr = Requests the peak value of coil current. This value is only valid in the charging state.");
                        Console.WriteLine("readtemp = Requests the temperature values of all available sensors in 0.01 degree celcius.");
                        Console.WriteLine("readapp = Requests application version");
                        Console.WriteLine("readpro = Requests protocol version");
                        Console.WriteLine("readlib = Requests freescale library version");
                        Console.WriteLine("readboot = Requests bootLoader version");
                        Console.WriteLine("readesnl = Requests general device parameters for ESN (little endian)");
                        Console.WriteLine("readesnb = Requests general device parameters for ESN (big endian)");
                        Console.WriteLine("readcoilv = Requests the coil voltages reported by a DAQ (shortcut: d)");
                        Console.WriteLine("transpow = read transmit power");
                        Console.WriteLine("recpow = read receive power");
                        Console.WriteLine("readled = Requests LED color");
                        Console.WriteLine("setled = Set LED Color");
                        Console.WriteLine("setesn = Set ESN and HW");
                        Console.WriteLine("setcoil = Set transmitter coils (shortcut: a)");
                        Console.WriteLine("stopchrg = stop charging");
                        Console.WriteLine("startchg = restart charging");
                        //Console.WriteLine("setseq = Set sequence");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    ////////////////////////////////////// READS /////////////////////////////////////                   

                    case "readcapst":
                        GeneralCommand readcapst = new GeneralCommand(m_si, 0x01, 0x14, new byte[] { }, "Read the current capacitive sensor values.");
                        readcapst.RegisterListener(this);
                        m_target.Queue(readcapst);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "readbridgev":
                        GeneralCommand readbridgev = new GeneralCommand(m_si, 0x01, 0x04, new byte[] {0x07 }, "read runtime parameter: requests bridge voltage in mv");
                        readbridgev.RegisterListener(this);
                        m_target.Queue(readbridgev);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "readadcv":
                        GeneralCommand readadc = new GeneralCommand(m_si, 0x01, 0x04, new byte[] { 0x08 }, "read runtime parameter: Requests the ADC count of the measured H - bridge input voltage.");
                        readadc.RegisterListener(this);
                        m_target.Queue(readadc);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "readcurms":
                        GeneralCommand readcurrms = new GeneralCommand(m_si, 0x01, 0x04, new byte[] { 0x0A }, "read runtime parameter: ");
                        readcurrms.RegisterListener(this);
                        m_target.Queue(readcurrms);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "readadcc":
                        GeneralCommand readcadc = new GeneralCommand(m_si, 0x01, 0x04, new byte[] { 0x0d }, "read runtime parameter: ");
                        readcadc.RegisterListener(this);
                        m_target.Queue(readcadc);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "readpwm":
                        //TELEGRAM_PARAM_RUNTIME_VALUE_PWM_COMMUNICATION_SIGNAL
                        GeneralCommand pwm_cmd = new GeneralCommand(m_si, 0x01, 0x04, new byte[] { 0x18 }, "PWM Communication");
                        pwm_cmd.RegisterListener(this);
                        m_target.Queue(pwm_cmd);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "readapp":
                        GeneralCommand appver = new GeneralCommand(m_si, 0x01, 0x1E, new byte[] { }, "read application version");
                        appver.RegisterListener(this);
                        m_target.Queue(appver);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "readpro":
                        GeneralCommand protocolver = new GeneralCommand(m_si, 0x01, 0x01, new byte[] { }, "read protocol version");
                        protocolver.RegisterListener(this);
                        m_target.Queue(protocolver);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "readlib":
                        GeneralCommand libver = new GeneralCommand(m_si, 0x01, 0x18, new byte[] { }, "read freescale library version");
                        libver.RegisterListener(this);
                        m_target.Queue(libver);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "readboot":
                        GeneralCommand bootver = new GeneralCommand(m_si, 0x01, 0x1A, new byte[] { }, "read bootLoader version");
                        bootver.RegisterListener(this);
                        m_target.Queue(bootver);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "readesnl":
                        GeneralCommand readParaLE = new GeneralCommand(m_si, 0x01, 0x03, new byte[] { 0x02, 0x01 }, "read general device parameters for ESN (little endian)");
                        readParaLE.RegisterListener(this);
                        m_target.Queue(readParaLE);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "readesnb":
                        GeneralCommand readParaBE = new GeneralCommand(m_si, 0x01, 0x03, new byte[] { 0x03, 0x01 }, "read general device parameters for ESN (big endian)");
                        readParaBE.RegisterListener(this);
                        m_target.Queue(readParaBE);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;


                    case "readpow":
                        GeneralCommand readpow = new GeneralCommand(m_si, 0x01, 0x04, new byte[] { 0x01 }, "read runtime parameter: Requests the power value the receiver has been reported in mW.");
                        readpow.RegisterListener(this);
                        m_target.Queue(readpow);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "readpwrlos":
                        GeneralCommand readpowlos = new GeneralCommand(m_si, 0x01, 0x04, new byte[] { 0x05 }, "read runtime parameter: Requests the internal system losses in mW.");
                        readpowlos.RegisterListener(this);
                        m_target.Queue(readpowlos);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "readcurr":
                        GeneralCommand readcurr = new GeneralCommand(m_si, 0x01, 0x04, new byte[] { 0x09 }, "Requests the peak value of coil current. This value is only valid in the charging state.");
                        readcurr.RegisterListener(this);
                        m_target.Queue(readcurr);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "readtemp":
                        GeneralCommand readtemp = new GeneralCommand(m_si, 0x01, 0x04, new byte[] { 0x011 }, "read runtime parameter: Requests the temperature values of all available sensors in degree celcius.");
                        readtemp.RegisterListener(this);
                        m_target.Queue(readtemp);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "readled":
                        GeneralCommand readColor = new GeneralCommand(m_si, 0x01, 0x0E, new byte[] { }, "Read the LED color");
                        readColor.RegisterListener(this);
                        m_target.Queue(readColor);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;
                    case "d":
                    case "readcoilv": // Read the voltage of each coil
                        if (daq) { Console.WriteLine("DAQ not available!"); break; }

                        double[] coils = new double[3];
                        Array.Copy(myDaq.getVolts(), coils, 3);
                        Console.WriteLine("\nCoil0:{0:N2}  Coil1:{1:N2}  Coil2:{2:N2}", coils[0], coils[1], coils[2]);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "transpow":
                        GeneralCommand transpower = new GeneralCommand(m_si, 0x01, 0x04, new byte[] { 0x06 }, "read transmit power");
                        transpower.RegisterListener(this);
                        m_target.Queue(transpower);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "recpow":
                        GeneralCommand receivepower = new GeneralCommand(m_si, 0x01, 0x04, new byte[] { 0x01 }, "read receive power");
                        receivepower.RegisterListener(this);
                        m_target.Queue(receivepower);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;





                    ////////////////////////////////////// READS /////////////////////////////////////

                    ////////////////////////////////////// SETS //////////////////////////////////////
                    case "setvrmax":
                        GeneralCommand setVrailMax = new GeneralCommand(m_si, 0x01, 0x06, new byte[] { 0x21, 0x02 }, "set Vrail to maximum value");
                        setVrailMax.RegisterListener(this);
                        m_target.Queue(setVrailMax);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "setvrmin":
                        GeneralCommand setVrailMin = new GeneralCommand(m_si, 0x01, 0x06, new byte[] { 0x21, 0x01 }, "set Vrail to minimum value");
                        setVrailMin.RegisterListener(this);
                        m_target.Queue(setVrailMin);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "stopchrg":
                        GeneralCommand silencemode = new GeneralCommand(m_si, 0x01, 0x3B, new byte[] { 0x02 }, "stop charging");
                        silencemode.RegisterListener(this);
                        m_target.Queue(silencemode);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "startchg":
                        GeneralCommand silencemodeoff = new GeneralCommand(m_si, 0x01, 0x3B, new byte[] { 0x01 }, "restart charging");
                        silencemodeoff.RegisterListener(this);
                        m_target.Queue(silencemodeoff);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "setbridgev": //Sets the desired bridge voltage in mV.
                        Console.WriteLine("Enter the desired bridge voltage in mV between 0 and 32767.");
                        string temp = Console.ReadLine();
                        int voltsInt = Convert.ToInt32(temp); if (voltsInt > 32767){ Console.WriteLine("number greater than 32767"); break; }
                        byte[] tempArray = BitConverter.GetBytes(voltsInt);
                        byte[] voltArray = new byte[2];
                        for (int a = 0; a < tempArray.Length; a++) { voltArray[a] = tempArray[a]; }
                        GeneralCommand setbridgev = new GeneralCommand(m_si, 0x01, 0x06, new byte[] {0x07, voltArray[0], voltArray[1] }, "set bridge voltage in mv");
                        setbridgev.RegisterListener(this);
                        m_target.Queue(setbridgev);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;
                    case "setled":

                        Console.Write("\nPress a to enter hex values for red, green, and blue.");
                        Console.Write("\nPress b to enter percentage values for red, green, and blue.");
                        Console.Write("\nPress c to select pre-programmed colors\n");
                        ConsoleKeyInfo colorKey;
                        colorKey = Console.ReadKey();
                        if (colorKey.KeyChar.ToString() == "A" || colorKey.KeyChar.ToString() == "a")
                        {
                            UInt16 redVal, greenVal, blueVal;
                            Console.Write("\nEnter Red of RGB (0x0000):");
                            string redString = Console.ReadLine();
                            if (!UInt16.TryParse(redString, System.Globalization.NumberStyles.AllowHexSpecifier,
                                System.Globalization.NumberFormatInfo.InvariantInfo, out redVal))
                            {
                                Console.WriteLine("Invalid Red Value, can't perform command");
                                commandIndex = -1;
                                keyCommand = new string[12];
                                break;
                            }
                            Console.Write("Enter Green of RGB (0x0000):");
                            string greenString = Console.ReadLine();
                            if (!UInt16.TryParse(greenString, System.Globalization.NumberStyles.AllowHexSpecifier,
                                System.Globalization.NumberFormatInfo.InvariantInfo, out greenVal))
                            {
                                Console.WriteLine("Invalid Green Value, can't perform command");
                                commandIndex = -1;
                                keyCommand = new string[12];
                                break;
                            }
                            Console.Write("Enter Blue of RGB (0x0000):");
                            string blueString = Console.ReadLine();
                            if (!UInt16.TryParse(blueString, System.Globalization.NumberStyles.AllowHexSpecifier,
                                System.Globalization.NumberFormatInfo.InvariantInfo, out blueVal))
                            {
                                Console.WriteLine("Invalid Blue Value, can't perform command");
                                commandIndex = -1;
                                keyCommand = new string[12];
                                break;
                            }
                            byte[] redArray = BitConverter.GetBytes(redVal);
                            byte[] greenArray = BitConverter.GetBytes(greenVal);
                            byte[] blueArray = BitConverter.GetBytes(blueVal);

                            Console.WriteLine("Writing Red:{0:X}{1:X} Green:{2:X}{3:X} Blue:{4:X}{5:X}",
                                redArray[0], redArray[1], greenArray[0], greenArray[1], blueArray[0], blueArray[1]);

                            GeneralCommand writeColor = new GeneralCommand(m_si, 0x01, 0x0C, new byte[] { redArray[0], redArray[1], greenArray[0], greenArray[1], blueArray[0], blueArray[1] }, "set led color");
                            writeColor.RegisterListener(this);
                            m_target.Queue(writeColor);
                            commandIndex = -1;
                            keyCommand = new string[12];
                            Console.WriteLine("\nCommand executed, returned to main menu\n");
                            break;
                        }

                        if (colorKey.KeyChar.ToString() == "B" || colorKey.KeyChar.ToString() == "b")
                        {
                            double redValp, greenValp, blueValp;
                            bool[] myDigit = new bool[3];
                            Console.Write("\nEnter percentage of Red of RGB:");
                            string redString = Console.ReadLine();
                            myDigit[0] = double.TryParse(redString, out redValp);
                            Console.Write("\nEnter percentage of Green of RGB:");
                            string greenString = Console.ReadLine();
                            myDigit[1] = double.TryParse(greenString, out greenValp);
                            Console.Write("\nEnter percentage of Blue of RGB:");
                            string blueString = Console.ReadLine();
                            myDigit[2] = double.TryParse(blueString, out blueValp);

                            bool myExit = false;
                            for (int d = 0; d < 3; d++)
                            {
                                if (!myDigit[d])
                                {
                                    Console.Write("\nPlease enter numerical digits only. Returned to main menu.");
                                    commandIndex = -1;
                                    keyCommand = new string[12];
                                    myExit = true;
                                    break;
                                }
                            }
                            if (myExit) break;

                            Console.WriteLine("\n" + redValp + " " + greenValp + " " + blueValp);
                            redValp = (redValp * .01) * 65535.0;
                            greenValp = (greenValp * .01) * 65535.0;
                            blueValp = (blueValp * .01) * 65535.0;



                            int myRedi = Convert.ToInt32(redValp);
                            int myGreeni = Convert.ToInt32(greenValp);
                            int myBluei = Convert.ToInt32(blueValp);

                            byte[] redArray = BitConverter.GetBytes(myRedi);
                            byte[] greenArray = BitConverter.GetBytes(myGreeni);
                            byte[] blueArray = BitConverter.GetBytes(myBluei);
                            Console.WriteLine("\n" + myRedi + " " + myGreeni + " " + myBluei);
                            Console.WriteLine("Writing Red:{0:X}{1:X} Green:{2:X}{3:X} Blue:{4:X}{5:X}",
                                redArray[0], redArray[1], greenArray[0], greenArray[1], blueArray[0], blueArray[1]);

                            GeneralCommand writeColor = new GeneralCommand(m_si, 0x01, 0x0C, new byte[] { redArray[0], redArray[1], greenArray[0], greenArray[1], blueArray[0], blueArray[1] }, "set led color");
                            writeColor.RegisterListener(this);
                            m_target.Queue(writeColor);


                        }
                        if (colorKey.KeyChar.ToString() == "C" || colorKey.KeyChar.ToString() == "c")
                        {
                            Console.Write("\nPress a for red.");
                            Console.Write("\npress b for green.");
                            Console.Write("\nPress c for blue.");
                            Console.Write("\nPress d for amber.");

                            ConsoleKeyInfo preColorKey;


                            double myRed, myBlue, myGreen;
                            while (true)
                            {
                                preColorKey = Console.ReadKey();
                                if (preColorKey.KeyChar.ToString() == "A" || preColorKey.KeyChar.ToString() == "a")
                                {
                                    myRed = 100;
                                    myGreen = 0;
                                    myBlue = 0;
                                    break;
                                }
                                else if (preColorKey.KeyChar.ToString() == "B" || preColorKey.KeyChar.ToString() == "b")
                                {
                                    myRed = 0;
                                    myGreen = 100;
                                    myBlue = 0;
                                    break;
                                }
                                else if (preColorKey.KeyChar.ToString() == "C" || preColorKey.KeyChar.ToString() == "c")
                                {
                                    myRed = 0;
                                    myGreen = 16.67;
                                    myBlue = 83.33;
                                    break;
                                }
                                else if (preColorKey.KeyChar.ToString() == "D" || preColorKey.KeyChar.ToString() == "d")
                                {
                                    myRed = 83.33;
                                    myGreen = 16.67;
                                    myBlue = 0;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("\nPlease enter a valid option from the menu.\n");
                                    Console.Write("\nPress a for red.");
                                    Console.Write("\npress b for green.");
                                    Console.Write("\nPress c for blue.");
                                    Console.Write("\nPress d for amber.");
                                }
                            }

                            Console.Write("\nPlease enter the percentage of desired intensity and press enter.");
                            double intensity;
                            bool myDigitP = double.TryParse(Console.ReadLine(), out intensity);
                            if (!myDigitP)
                            {
                                Console.Write("\nPlease enter numerical digits only. Returned to main menu.");
                                commandIndex = -1;
                                keyCommand = new string[12];
                                break;
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

                            Console.WriteLine("Writing Red:{0:X}{1:X} Green:{2:X}{3:X} Blue:{4:X}{5:X}",
                                redArray[0], redArray[1], greenArray[0], greenArray[1], blueArray[0], blueArray[1]);

                            GeneralCommand writeColor = new GeneralCommand(m_si, 0x01, 0x0C, new byte[] { redArray[0], redArray[1], greenArray[0], greenArray[1], blueArray[0], blueArray[1] }, "set led color");
                            writeColor.RegisterListener(this);
                            m_target.Queue(writeColor);

                        }
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                    case "setesn":
                        Console.Write("\nPlease enter ESN: ");
                        ConsoleKeyInfo inputKey;
                        //Console.Write("Press \'#\' to change HW version.");
                        int esnCount = 0;
                        int esnVal = 0;
                        double hwVal = 0;
                        string hwvers = "";
                        string[] hwArray;
                        int[] esnArray = new int[13]; // 13 is the number of characters in the ESN serial
                        while (esnCount < 13) // make sure the user is entering number only
                        {

                            inputKey = Console.ReadKey(); // allows the typing or scanning in the ESN and automatically moving to ask for writing to OTP
                            //Console.WriteLine("input key is: " + inputKey.KeyChar.ToString());
                            if (inputKey.Key == ConsoleKey.Escape) break; //allows user to exit entering the ESN
                            if (inputKey.Key == ConsoleKey.Backspace && esnCount != 0)
                            {
                                esnArray[esnCount] = 0;
                                esnCount--; // allows user to backspace and overwrite previously entered numbers.

                                continue;
                            }

                            bool isInt = Int32.TryParse(inputKey.KeyChar.ToString(), out esnVal); // checks if input is numerical
                            if (isInt)
                            {
                                esnArray[esnCount] = esnVal; // populates array containing the ESN digit by digit automatically as the user enters it.
                                esnCount++;
                            }
                            else
                            {
                                Console.Write("\nPlease enter numerical digits only.\n");
                                for (int a = 0; a < esnCount; a++) Console.Write(esnArray[a]);
                            }
                        }
                        if (esnCount != 13) { Console.WriteLine("\nExiting ESN and HW writing mode"); break; }  // exits out of function because the user wants to exit this function.

                        Console.Write("\nPlease enter HW revision. (you must enter a decimal such as 1.2 where 1 is the major version and 2 is the minor version):  ");
                        while (true) // make sure that the user is entering decimals and at least one period
                        {
                            hwvers = Console.ReadLine();
                            bool myDigitP = double.TryParse(hwvers, out hwVal);
                            if (!myDigitP)
                            {
                                Console.WriteLine("\nPlease enter Numerical Digits Only");
                                Console.Write("\nPlease enter HW revision. (you must enter a decimal such as 1.2 where 1 is the major version and 2 is the minor version): ");
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
                                    Console.WriteLine("\nA decimal number must contain a period.");
                                    Console.Write("\nPlease enter HW revision. (you must enter a decimal) ((ex. 1.1)): ");
                                }
                                else
                                {
                                    if (majorV > 255 || minorV > 255) Console.WriteLine("\nThe major version and minor version must be between 0-255.");
                                    else if (majorV < 0 || minorV < 0) Console.WriteLine("\nThe major version and minor version must not be negative.");
                                    else break;
                                }

                            }
                        }

                        Console.Write("\nESN: "); for (int a = 0; a < 13; a++) Console.Write(esnArray[a]); Console.Write(" HW: " + hwvers + "\n"); // display what is about to be programmed
                        Console.WriteLine("Press \'y\' to accept or \'n\' to exit write mode.");
                        inputKey = Console.ReadKey();
                        if (inputKey.Key.ToString() == "n" || inputKey.Key.ToString() == "N")
                        {
                            Console.WriteLine("\nExiting ESN and HW writing mode");
                            commandIndex = -1;
                            keyCommand = new string[12];
                            break;
                        }
                        if (inputKey.Key.ToString() == "y" || inputKey.Key.ToString() == "Y")
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

                            GeneralCommand writeGenPara = new GeneralCommand(m_si, 0x01, 0x07, inputArray, "Write general device parameters");
                            //GeneralCommand writeGenPara = new GeneralCommand(m_si, 0x01, 0x07, new byte[] { 0x02, 0x01, 0x04, 0x00, 0x55, 0x73, 0x02, 0x74, 0x09, 0x00, 0x00, 0x00, 0x01, 0x00 }, "Write general device parameters");
                            writeGenPara.RegisterListener(this);
                            m_target.Queue(writeGenPara);
                        }
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;



                    case "a":
                    case "setcoil":
                        int coilId = 0;
                        bool skip = true;
                        bool loop = true;
                        while (loop)
                        {
                            Console.Write("\nPress 0 for coil 0\n");
                            Console.Write("Press 1 for coil 1\n");
                            Console.Write("Press 2 for coil 2\n");
                            Console.Write("Press any other key to stop all coils\n");
                            Console.Write("Press t to choose in milliseconds the wait time between coil on/off switch\n");

                            ConsoleKeyInfo coilkey = Console.ReadKey();
                            switch (coilkey.KeyChar.ToString())
                            {
                                case "0":
                                    {
                                        Console.WriteLine("\nset coil 0 to active");
                                        coilId = 0;
                                        loop = false; break;
                                    }
                                case "1":
                                    {
                                        Console.WriteLine("\nset coil 1 to active");
                                        coilId = 1;
                                        loop = false; break;
                                    }
                                case "2":
                                    {
                                        Console.WriteLine("\nset coil 2 to active");
                                        coilId = 2;
                                        loop = false; break;
                                    }
                                case "t":
                                    {
                                        Console.WriteLine("\nenter coil switch delay in milliseconds");
                                        coilTime = Convert.ToInt32(Console.ReadLine());
                                        Console.WriteLine("\ncoil switch delay is now " + coilTime + " milliseconds. Returned to setcoil menu.");
                                        break;
                                    }
                                default:
                                    {
                                        Console.WriteLine("\ndeactivate all coils");
                                        coilId = 0x0F;
                                        skip = false;
                                        loop = false; break;
                                    }
                            }
                        }

                        if (skip)
                        {
                            GeneralCommand stopcoils = new GeneralCommand(m_si, 0x01, 0x06, new byte[] { 0x19, 0x0F }, "stop active coil"); // have to stop all coils first before setting new coils.
                            stopcoils.RegisterListener(this);
                            m_target.Queue(stopcoils);

                            Thread.Sleep(coilTime); // should sleep a bit before setting new coil active
                        }

                        Console.WriteLine(Convert.ToByte(coilId));
                        GeneralCommand setactivecoil = new GeneralCommand(m_si, 0x01, 0x06, new byte[] { 0x19, Convert.ToByte(coilId) }, "set active coil");
                        setactivecoil.RegisterListener(this);
                        m_target.Queue(setactivecoil);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[12];
                        break;

                        ////////////////////////////////////// SETS //////////////////////////////////////


                        /*
                    case ConsoleKey.S:
                        //si.SetBaudrate(9600);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        break;
                        */
                }
            }
            m_si.Close();
            sw.Close();
        }

        private void KeepBootloaderAlive()
        {
            GeneralCommand set_bl = new GeneralCommand(m_si, 0x01, 0x24, new byte[] { 0x01, 0x03, 0x7D }, "KEEP_BL_RUNNING");
            set_bl.Timeout = 1000;
            set_bl.RegisterListener(this);

            int cnt = 30;

            while (!m_bootloader_ready && (--cnt) > 0)
            {
                Console.WriteLine("REQUEST:KEEP_BL_RUNNING");
                m_target.Queue(set_bl);
                m_target.Wait();
            }
        }


        private void UploadApplication(WcaInterfaceLibrary.SerialInterface si, Target target, string full_file_name)
        {
            StreamReader sr = new StreamReader(full_file_name, Encoding.ASCII);
            GeneralCommand upload_cmd;
            string line;
            byte[] sdata;
            ushort seg_cnt = 0;
            byte[] seg_cnt_ba;

            while ((line = sr.ReadLine()) != null)
            {
                line = "00" + line + "\r\n";
                Console.Write(line);

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
            Console.WriteLine("Upload application done.");
        }


        #region IInterfaceListener Members

        public void OnReceive(IInterface sender, byte[] data, int dataLength)
        {
            
                string str;
                if (ascii) str = System.Text.Encoding.Default.GetString(data);
                else str = WcaInterfaceLibrary.Converter.ByteArray2String(data);
               // Console.WriteLine(str);
            if (globals.baud == 115200)
            {
                //if ( str.Equals("55 09 00 02 26 01 00 00 00 00 94 7B"))
                if (data[4] == 0x26 && data[5] == 0x01)
                {
                    byte[] cmdb = new byte[3];
                    cmdb[0] = 0x01;
                    cmdb[1] = 0x03;
                    cmdb[2] = 0x7D;
                    GeneralCommand set_bl = new GeneralCommand(m_si, 0x01, 0x24, cmdb, "keep bootloader running");
                    //Console.WriteLine("CMD:B:"+set_bl.ToString());
                    set_bl.RegisterListener(this);
                    m_target.Queue(set_bl);
                   // m_target.Wait();
                }
            }
        }


        #endregion

        #region ICommandListener Members
        public void OnNotification(ICommand sender) // this function recieves a verified message from the device which we can then parse and log.
        {

            Console.WriteLine(DateTime.Now.ToShortTimeString() + " " + sender.ToString());
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


                
                if (!string.IsNullOrEmpty(str))
                {

                    string[] response = str.Split(' '); // we make the string of data bytes and turn it into a string array to parse out the values we want
                    if (response[0].Equals("55") && response[3].Equals("2D") && response.Length > 4) //here we are checking if the data we got is one from our wireless charger, a POS ACK, and more than 4 bytes
                    {
                        //Console.WriteLine("\n"+DateTime.Now.ToShortTimeString() + " [" + str + "] PosAck");
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
                                                Console.WriteLine("Read ADC Voltage Count \t" + BitConverter.ToInt32(myData, 0).ToString());
                                                break;
                                            }
                                        case "11": //readtemp
                                            {
                                                Console.WriteLine("Read Coil Temps \tCoil0: " + Convert.ToInt32(data[6]).ToString() + "C Coil1: " + Convert.ToInt32(data[7]).ToString() + "C Coil2: " + Convert.ToInt32(data[8]).ToString() + "C");
                                                break;
                                            }
                                        case "0A": //read current coil ma RMS
                                            {
                                                byte[] myData = { data[6], data[7], 0x00, 0x00 };
                                                Console.WriteLine("Read Current RMS\t" + BitConverter.ToInt32(myData, 0).ToString() + "mA");
                                                break;
                                            }
                                        case "19": //read active coil
                                            {
                                                Console.WriteLine("Read active coil \t Active coil: " + Convert.ToInt32(data[6]).ToString());
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

                                            Console.WriteLine("Set Coil sent");
                                            break;
                                    }
                                    break;
                                }
                            case "1E": // read application version
                                {
                                    Console.WriteLine("Read App Version \t" + Convert.ToInt32(data[6]).ToString() + "." + Convert.ToInt32(data[7]).ToString()
                                        + "." + Convert.ToInt32(data[8]).ToString() + "." + Convert.ToInt32(data[9]).ToString());
                                    break;
                                }
                            case "03": // read esn
                                {
                                    switch (response[5])
                                    {
                                        case "02":
                                            {
                                                string[] fullesn = str.Split(' ');
                                                string myesn = fullesn[12] + fullesn[11] + fullesn[10] + fullesn[9] + fullesn[8] + fullesn[7] + fullesn[6];
                                                string myhw = fullesn[14] + "." + fullesn[16];
                                                str2 = "ESN:" + myesn + ",HW:" + myhw;
                                                Console.WriteLine(" Read ESN and HW \tESN: " + myesn + " HW: " + myhw);
                                                break;
                                            }
                                        case "03":
                                            {
                                                string[] fullesn = str.Split(' ');
                                                string myesn = fullesn[7] + fullesn[8] + fullesn[9] + fullesn[10] + fullesn[11] + fullesn[12] + fullesn[13];
                                                string myhw = fullesn[15] + "." + fullesn[17];
                                                str2 = "ESN:" + myesn + ",HW:" + myhw;
                                                Console.WriteLine(" Read ESN and HW \tESN: " + myesn + " HW: " + myhw);
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
                                    Console.WriteLine("Read LED\t Color values = Red: " + (BitConverter.ToInt32(red, 0)/655.35).ToString() + " Green: " + (BitConverter.ToInt32(green, 0) / 655.35).ToString() + " Blue: " + (BitConverter.ToInt32(blue, 0) / 655.35).ToString());
                                    break;
                                }
                            case "0C": // set led
                                {
                                    Console.WriteLine("Set LED Color");
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

                    }
                }
                

            }
            else { Console.WriteLine(sender.ToString()); } // if there is no recieve message, then just print whatever response is available.


        }

        #endregion
    }
}
