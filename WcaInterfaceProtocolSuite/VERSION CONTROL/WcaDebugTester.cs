using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.IO;
using WcaInterfaceLibrary;

/*
 * list of issues.
 * backspace creates some massive weirdness.
 * special keys cause blank space in array which will screw up comparison
 * enter key erases array
 * 
 * */
namespace WcaProgrammerConsole
{
    class WcaDebugTester: ICommandListener, IInterfaceListener
    {
        private StreamWriter sw;
        private bool ascii = false;
        private WcaInterfaceLibrary.SerialInterface m_si = null;
        private Target m_target = null;
        private bool m_bootloader_ready = false;

        public WcaDebugTester()
        {
            try
            {
                sw = new StreamWriter("WcaDebugTester.log", true, Encoding.ASCII);
            }
            catch (Exception e)  //allow 2 instances of the program to be run at the same time
            {
                sw = new StreamWriter("WcaDebugTester2.log", true, Encoding.ASCII);
            }
        }

        public void Run(string comport, string target_file)
        {
            bool abort = false;
            m_si = new WcaInterfaceLibrary.SerialInterface(comport, 115200, Parity.None, 8);
            //m_si = new WcaInterfaceLibrary.SerialInterface(comport, 9600, Parity.None, 8);
            m_target = new Target();
            m_si.RegisterListener(this);

            if (!m_si.Open())
            {
                Console.WriteLine("COM port could not be opened.");
                return;
            }

            Console.WriteLine("Application running.");

            //KeepBootloaderAlive();

            byte on_off = 0x01;
            string[] keyCommand = new string[10];
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
                                keyCommand = new string[10];
                                commandIndex = 0;
                            }
                        keyCommand[commandIndex] = null;   // remove unwanted character               
                        Console.Write("\r                       "); // clear line so there isn't garbage to overwrite.
                        Console.Write("\r" + string.Join("", keyCommand)); --commandIndex;//reprint the new line without the unwanted character and then decrement again to show proper index for new amount of characters
                        /*
                        Console.Write("\r\"");
                        for (int a = 0; a < 10; a++)
                        {
                            Console.Write(keyCommand[a]);
                        }
                        Console.Write("\" : " + --commandIndex);*/
                        continue;
                }

                if (!Char.IsLetter(key.KeyChar)) // prevent the user from entering characters that aren't letters including the command keys like enter and ctrl and shift.
                {
                    commandIndex--;//don't increment if user enters invalid character
                    continue;// skip all the rest of the test because ya
                }
                   

                

                if (commandIndex == 9) // make sure the user doesn't enter more characters than necessary
                {
                    Console.WriteLine("\nYou have entered too many characters for any valid command. Please try again.");
                    commandIndex = 0;
                    keyCommand = new string[10];
                    keyCommand[0] = "h";

                } else
                {
                    keyCommand[commandIndex] = key.KeyChar.ToString(); //saving the new key character into a new location
                            Console.Write("\r                       ");
                            Console.Write("\r" + string.Join("", keyCommand));                     
                    }

                        switch (string.Join("", keyCommand).ToLower()) // join returns a string of the combined array.                            
                    {                            
                            /*
                        case "zzzz":
                            GeneralCommand gpio_cmd = new GeneralCommand(m_si, 0x01, 0x2A, new byte[] { 0x01, 0x01, on_off }, "Set LED 1");
                            gpio_cmd.RegisterListener(this);
                            m_target.Queue(gpio_cmd);

                            if (on_off == 0x01)
                            {
                                on_off = 0;
                            }
                            else
                            {
                                on_off = 0x01;
                            }
                            Console.WriteLine("\nCommand executed, returned to main menu\n");
                            break;*/

                        case "setseq":
                            //Sequenz 100ms Low und 100ms High --> 5 mal wiederholen
                            GeneralCommand gpio_sequence = new GeneralCommand(m_si, 0x01, 0x2B, new byte[] { 0x01, 0x01, 0x02, 0x64, 0x00, 0x64, 0x00, 5 }, "Set Sequence");
                            gpio_sequence.RegisterListener(this);
                            m_target.Queue(gpio_sequence);
                            Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                            keyCommand = new string[10];
                            break;

                        case "pwmcom":
                            //TELEGRAM_PARAM_RUNTIME_VALUE_PWM_COMMUNICATION_SIGNAL
                            GeneralCommand pwm_cmd = new GeneralCommand(m_si, 0x01, 0x04, new byte[] { 0x18 }, "PWM Communication");
                            pwm_cmd.RegisterListener(this);
                            m_target.Queue(pwm_cmd);
                            Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                            keyCommand = new string[10];
                            break;


                        case "startapp":
                            GeneralCommand set_app = new GeneralCommand(m_si, 0x01, 0x24, new byte[] { 0x02, 0x01, 0x00 }, "Start user application");
                            set_app.RegisterListener(this);
                            m_target.Queue(set_app);
                            m_target.Wait();
                            ascii = true;
                            Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                            keyCommand = new string[10];
                            break;

                        case "starteol":
                            GeneralCommand set_app_eol = new GeneralCommand(m_si, 0x01, 0x24, new byte[] { 0x02, 0x06, 0x00 }, "Start user application");
                            set_app_eol.RegisterListener(this);
                            m_target.Queue(set_app_eol);
                            m_target.Wait();
                            Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                            keyCommand = new string[10];
                            break;

                    case "eol9600":
                        GeneralCommand set_app_eol2 = new GeneralCommand(m_si, 0x01, 0x24, new byte[] { 0x02, 0x07, 0x00 }, "Start user application at 9600");
                        set_app_eol2.RegisterListener(this);
                        m_target.Queue(set_app_eol2);
                        m_target.Wait();
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[10];
                        break;


                    case "b":
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
                            keyCommand = new string[10];
                            break;

                        case "program":
                            //string user_app = @"f:\projects\svn_projects\0_31X_P0517_moray_ruby1_2\trunk\development\software\releases\wca\release\MO_WC_11_1_8_6-10894\MO_WC_11_1_8_6.S19";
                            UploadApplication(m_si, m_target, target_file);
                            Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                            keyCommand = new string[10];
                            break;

                        case "readapp":
                            GeneralCommand appver = new GeneralCommand(m_si, 0x01, 0x1E, new byte[] { }, "read application version");
                            appver.RegisterListener(this);
                            m_target.Queue(appver);
                            Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                            keyCommand = new string[10];
                            break;

                        case "readpro":
                            GeneralCommand protocolver = new GeneralCommand(m_si, 0x01, 0x01, new byte[] { }, "read protocol version");
                            protocolver.RegisterListener(this);
                            m_target.Queue(protocolver);
                            Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                            keyCommand = new string[10];
                            break;

                        case "readlib":
                            GeneralCommand libver = new GeneralCommand(m_si, 0x01, 0x18, new byte[] { }, "read freescale library version");
                            libver.RegisterListener(this);
                            m_target.Queue(libver);
                            Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                            keyCommand = new string[10];
                            break;

                        case "readboot":
                            GeneralCommand bootver = new GeneralCommand(m_si, 0x01, 0x1A, new byte[] { }, "read bootLoader version");
                            bootver.RegisterListener(this);
                            m_target.Queue(bootver);
                            Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                            keyCommand = new string[10];
                            break;

                        case "readesnl":
                            GeneralCommand readParaLE = new GeneralCommand(m_si, 0x01, 0x03, new byte[] { 0x02, 0x01 }, "read general device parameters for ESN (little endian)");
                            readParaLE.RegisterListener(this);
                            m_target.Queue(readParaLE);
                            Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                            keyCommand = new string[10];
                            break;

                        case "readesnb":
                            GeneralCommand readParaBE = new GeneralCommand(m_si, 0x01, 0x03, new byte[] { 0x03, 0x01 }, "read general device parameters for ESN (big endian)");
                            readParaBE.RegisterListener(this);
                            m_target.Queue(readParaBE);
                            Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                            keyCommand = new string[10];
                            break;

                        case "setvrmax":

                            UInt16 maxtimeVal;
                            Console.Write("\nEnter control time in mseconds (max 30000):");
                            string controltime = Console.ReadLine();
                            if (!UInt16.TryParse(controltime, out maxtimeVal))
                            {
                                Console.WriteLine("Invalid Control Time, can't perform command");
                                commandIndex = -1;
                                keyCommand = new string[10];
                                break;
                            }
                            if (maxtimeVal > 30000)
                            {
                                Console.WriteLine("Can't set time greater than 30000 msec, can't perform command");
                                commandIndex = -1;
                                keyCommand = new string[10];
                                break;
                            }

                            byte[] maxtimeArray = BitConverter.GetBytes(maxtimeVal);

                            Console.WriteLine("Writing MaxTime:{0:X}{1:X}",
                               maxtimeArray[0], maxtimeArray[1]);

                            GeneralCommand setVrailMax = new GeneralCommand(m_si, 0x01, 0x06, new byte[] { 0x21, 0x02, maxtimeArray[0], maxtimeArray[1] }, "set Vrail to maximum value");
                            setVrailMax.RegisterListener(this);
                            m_target.Queue(setVrailMax);
                            Console.WriteLine("\nCommand executed, returned to main menu\n");
                            commandIndex = -1;
                            keyCommand = new string[10];
                            break;

                        case "setvrmin":

                            UInt16 mintimeVal;
                            Console.Write("\nEnter control time in mseconds (max 30000):");
                            string mincontroltime = Console.ReadLine();
                            if (!UInt16.TryParse(mincontroltime, out mintimeVal))
                            {
                                Console.WriteLine("Invalid Control Time, can't perform command");
                                commandIndex = -1;
                                keyCommand = new string[10];
                                break;
                            }
                            if (mintimeVal > 30000)
                            {
                                Console.WriteLine("Can't set time greater than 30000 msec, can't perform command");
                                commandIndex = -1;
                                keyCommand = new string[10];
                                break;
                            }

                            byte[] mintimeArray = BitConverter.GetBytes(mintimeVal);

                            Console.WriteLine("Writing MaxTime:{0:X}{1:X}", mintimeArray[0], mintimeArray[1]);
                            GeneralCommand setVrailMin = new GeneralCommand(m_si, 0x01, 0x06, new byte[] { 0x21, 0x01, mintimeArray[0], mintimeArray[1] }, "set Vrail to minimum value");
                            setVrailMin.RegisterListener(this);
                            m_target.Queue(setVrailMin);
                            Console.WriteLine("\nCommand executed, returned to main menu\n");
                            commandIndex = -1;
                            keyCommand = new string[10];
                            break;

                        case "setled":
                            //add a Try Catch Here - Cause - Yeah
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
                                break;
                            }
                                Console.Write("Enter Green of RGB (0x0000):");
                            string greenString = Console.ReadLine();
                            if (!UInt16.TryParse(greenString, System.Globalization.NumberStyles.AllowHexSpecifier,
                                System.Globalization.NumberFormatInfo.InvariantInfo, out greenVal))
                            {
                                Console.WriteLine("Invalid Green Value, can't perform command");
                                break;
                            }
                                Console.Write("Enter Blue of RGB (0x0000):");
                            string blueString = Console.ReadLine();
                            if (!UInt16.TryParse(blueString, System.Globalization.NumberStyles.AllowHexSpecifier,
                                System.Globalization.NumberFormatInfo.InvariantInfo, out blueVal))
                            {
                                Console.WriteLine("Invalid Blue Value, can't perform command");
                                break;
                            }
                            byte[] redArray = BitConverter.GetBytes(redVal);
                            byte[] greenArray = BitConverter.GetBytes(greenVal);
                            byte[] blueArray = BitConverter.GetBytes(blueVal);

                            Console.WriteLine("Writing Red:{0:X}{1:X} Green:{2:X}{3:X} Blue:{4:X}{5:X}",
                                redArray[0], redArray[1], greenArray[0], greenArray[1], blueArray[0], blueArray[1]);

                            GeneralCommand writeColor = new GeneralCommand(m_si, 0x01, 0x0C, new byte[] { redArray[0], redArray[1], greenArray[0], greenArray[1], blueArray[0], blueArray[1]}, "set led color");
                            writeColor.RegisterListener(this);
                            m_target.Queue(writeColor);

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
                                    keyCommand = new string[10];
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
                                    keyCommand = new string[10];
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
                            keyCommand = new string[10];
                                break;

                        case "readled":

                            /*
                            //add a Try Catch Here - Cause - Yeah
                            Console.Write("\nLED Blink Test Code\n.");

                            byte cycletime, percentrising, dividercnt;
                            Console.Write("\nEnter Cycle Time (in centiseconds ie val * 100 to go to ms):");
                            string cycleString = Console.ReadLine();
                            if (!Byte.TryParse(cycleString, System.Globalization.NumberStyles.Integer,
                                System.Globalization.NumberFormatInfo.InvariantInfo, out cycletime))
                            {
                                Console.WriteLine("Invalid Cycle Time Value, can't perform command");
                                break;
                            }
                            Console.Write("\nEnter Percent of Cycle Time in Luminance Rising (in value out of 100):");
                            string percentString = Console.ReadLine();
                            if (!Byte.TryParse(percentString, System.Globalization.NumberStyles.Integer,
                                System.Globalization.NumberFormatInfo.InvariantInfo, out percentrising))
                            {
                                Console.WriteLine("Invalid Percent Value, can't perform command");
                                break;
                            }

                            Console.Write("\nEnter Full Cycle Divider Counts (target for a dimming time no lower than 50ms):");
                            string dividerString = Console.ReadLine();
                            if (!Byte.TryParse(dividerString, System.Globalization.NumberStyles.Integer,
                                System.Globalization.NumberFormatInfo.InvariantInfo, out dividercnt))
                            {
                                Console.WriteLine("Invalid Percent Value, can't perform command");
                                break;
                            }
                            

                            GeneralCommand readColor = new GeneralCommand(m_si, 0x01, 0x0E, new byte[] {cycletime, percentrising, dividercnt }, "Read the LED color");
                            */

                            GeneralCommand readColor = new GeneralCommand(m_si, 0x01, 0x0E, new byte[] { }, "Read the LED color");

                            readColor.RegisterListener(this);
                            m_target.Queue(readColor);
                            Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                            keyCommand = new string[10];    
                            break;
                        /*
                        case "setesnol":
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
                                else { Console.Write("\nPlease enter numerical digits only.\n");
                                    for (int a = 0; a < esnCount; a++) Console.Write(esnArray[a]);
                                }
                            }
                            if (esnCount != 13) { Console.WriteLine("\nExiting ESN and HW writing mode"); break; }  // exits out of function because the user wants to exit this function.

                            Console.Write("\nPlease enter HW revision. (you must enter a decimal such as 1.2 where 1 is the major version and 2 is the minor version):  ");
                            while(true) // make sure that the user is entering decimals and at least one period
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
                            if (inputKey.Key.ToString() == "n" || inputKey.Key.ToString() == "N") { Console.WriteLine("\nExiting ESN and HW writing mode");
                            commandIndex = -1;
                                keyCommand = new string[10];
                                break; }
                            if (inputKey.Key.ToString() == "y" || inputKey.Key.ToString() == "Y")
                            {
                                byte[] inputArray = new byte[14]; // 14 is the number of bytes in the command for writing the ESN and the HW version
                                int inputIndex = 7; // the number of bytes necessary to populate the ESN alone
                                // hard populating certain parts of the inputArray that can't be parsed by the for loop im about to use
                                inputArray[0] = 0x02; //little endianess
                                inputArray[1] = 0x01; // write to serial number
                                // elements 2 through 8 (7 total) are used for the ESN. written literally in hex in little endian
                                for (int a = 1; a < 12; a+=2)
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
                                inputArray[10] = Convert.ToByte(subHW); ; //primary version// HW version here. supposed for rev 1.0 for elements 10 through 13 in little endian
                                inputArray[11] = 0x00;
                                inputArray[12] = Convert.ToByte(primaryHW); // sub versionprimaryHW
                                inputArray[13] = 0x00;

                                GeneralCommand writeGenPara = new GeneralCommand(m_si, 0x01, 0x07, inputArray, "Write general device parameters");
                                //GeneralCommand writeGenPara = new GeneralCommand(m_si, 0x01, 0x07, new byte[] { 0x02, 0x01, 0x04, 0x00, 0x55, 0x73, 0x02, 0x74, 0x09, 0x00, 0x00, 0x00, 0x01, 0x00 }, "Write general device parameters");
                                writeGenPara.RegisterListener(this);
                                m_target.Queue(writeGenPara);
                            }
                            Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                            keyCommand = new string[10];
                            break;
                    */
                    case "setesnnw":
                        Console.Write("\nPlease enter ESN: ");
                        ConsoleKeyInfo inputKey2;
                        //Console.Write("Press \'#\' to change HW version.");
                        int esnCount2 = 0;
                        int esnVal2 = 0;
                        double hwVal2 = 0;
                        string hwvers2 = "";
                        string[] hwArray2;
                        int[] esnArray2 = new int[16]; // 16 is the number of characters in the ESN serial
                        while (esnCount2 < 16) // make sure the user is entering number only
                        {

                            inputKey2 = Console.ReadKey(); // allows the typing or scanning in the ESN and automatically moving to ask for writing to OTP
                                                          //Console.WriteLine("input key is: " + inputKey.KeyChar.ToString());
                            if (inputKey2.Key == ConsoleKey.Escape) break; //allows user to exit entering the ESN
                            if (inputKey2.Key == ConsoleKey.Backspace && esnCount2 != 0)
                            {
                                esnArray2[esnCount2] = 0;
                                esnCount2--; // allows user to backspace and overwrite previously entered numbers.

                                continue;
                            }

                            bool isInt2 = Int32.TryParse(inputKey2.KeyChar.ToString(), out esnVal2); // checks if input is numerical
                            if (isInt2)
                            {
                                esnArray2[esnCount2] = esnVal2; // populates array containing the ESN digit by digit automatically as the user enters it.
                                esnCount2++;
                            }
                            else
                            {
                                Console.Write("\nPlease enter numerical digits only.\n");
                                for (int a = 0; a < esnCount2; a++) Console.Write(esnArray2[a]);
                            }
                        }
                        if (esnCount2 != 16) { Console.WriteLine("\nExiting ESN and HW writing mode"); break; }  // exits out of function because the user wants to exit this function.

                        Console.Write("\nPlease enter HW revision. (you must enter a decimal such as 1.2 where 1 is the major version and 2 is the minor version):  ");
                        while (true) // make sure that the user is entering decimals and at least one period
                        {
                            hwvers2 = Console.ReadLine();
                            bool myDigitP2 = double.TryParse(hwvers2, out hwVal2);
                            if (!myDigitP2)
                            {
                                Console.WriteLine("\nPlease enter Numerical Digits Only");
                                Console.Write("\nPlease enter HW revision. (you must enter a decimal such as 1.2 where 1 is the major version and 2 is the minor version, max value is 99.99): ");
                                continue;
                            }
                            else
                            {
                                hwArray2 = hwvers2.Split('.');
                                int majorV2;
                                int minorV2;
                                int.TryParse(hwArray2[0], out majorV2);
                                int.TryParse(hwArray2[1], out minorV2);
                                if (hwArray2.Length == 1)
                                {
                                    Console.WriteLine("\nA decimal number must contain a period.");
                                    Console.Write("\nPlease enter HW revision. (you must enter a decimal) ((ex. 1.1)): ");
                                }
                                else
                                {
                                    if (majorV2 > 99 || minorV2 > 99) Console.WriteLine("\nThe major version and minor version must be between 0-99.");
                                    else if (majorV2 < 0 || minorV2 < 0) Console.WriteLine("\nThe major version and minor version must not be negative.");
                                    else break;
                                }

                            }
                        }

                        Console.Write("\nESN: "); for (int a = 0; a < 16; a++) Console.Write(esnArray2[a]); Console.Write(" HW: " + hwvers2 + "\n"); // display what is about to be programmed
                        Console.WriteLine("Press \'y\' to accept or \'n\' to exit write mode.");
                        inputKey2 = Console.ReadKey();
                        if (inputKey2.Key.ToString() == "n" || inputKey2.Key.ToString() == "N")
                        {
                            Console.WriteLine("\nExiting ESN and HW writing mode");
                            commandIndex = -1;
                            keyCommand = new string[10];
                            break;
                        }
                        if (inputKey2.Key.ToString() == "y" || inputKey2.Key.ToString() == "Y")
                        {
                            byte[] inputArray2 = new byte[12]; // 12 is the number of bytes in the command for writing the ESN and the HW version
                            int inputIndex2 = 9; // the number of bytes necessary to populate the ESN alone
                                                // hard populating certain parts of the inputArray that can't be parsed by the for loop im about to use
                            inputArray2[0] = 0x02; //little endianess
                            inputArray2[1] = 0x01; // write to serial number
                                                  // elements 0 through 8 are used for the ESN. written literally in hex in little endian
                            for (int a = 0; a < 16; a += 2)
                            {
                                inputArray2[inputIndex2] = Convert.ToByte((esnArray2[a] * 16) + esnArray2[a + 1]); // convert the ESN in to Literal HEX in little endian
                                inputIndex2--;
                            }

                            int[] primaryHWarray2 = new int[2];
                            int[] subHWarray2 = new int[2];
                            
                            //already verified that the read in value is at max 2 digits
                            if(hwArray2[0].Length > 1)  //it was two digits
                            {
                                primaryHWarray2[0] = hwArray2[0][0] - '0';
                                primaryHWarray2[1] = hwArray2[0][1] - '0';
                            }
                            else  //it was only one digit
                            {
                                primaryHWarray2[0] = 0;
                                primaryHWarray2[1] = hwArray2[0][0] - '0';
                            }

                            //already verified that the read in value is at max 2 digits
                            if (hwArray2[1].Length > 1)  //it was two digits
                            {
                                subHWarray2[0] = hwArray2[1][0] - '0';
                                subHWarray2[1] = hwArray2[1][1] - '0';
                            }
                            else  //it was only one digit
                            {
                                subHWarray2[0] = 0;
                                subHWarray2[1] = hwArray2[1][0] - '0';
                            }

                            inputArray2[10] = Convert.ToByte((subHWarray2[0] * 16) + subHWarray2[1]);
                            inputArray2[11] = Convert.ToByte((primaryHWarray2[0] * 16) + primaryHWarray2[1]);

                            Console.WriteLine("Set General Device Data Array: {0:X2} {1:X2} {2:X2} {3:X2} {4:X2} {5:X2} {6:X2} {7:X2} {8:X2} {9:X2} {10:X2} {11:X2}",
                                inputArray2[0], inputArray2[1], inputArray2[2], inputArray2[3], inputArray2[4], inputArray2[5], inputArray2[6], inputArray2[7],
                                inputArray2[8], inputArray2[9], inputArray2[10], inputArray2[11]);

                            
                            GeneralCommand writeGenPara2 = new GeneralCommand(m_si, 0x01, 0x07, inputArray2, "Write general device parameters");
                            //GeneralCommand writeGenPara2 = new GeneralCommand(m_si, 0x01, 0x07, new byte[] { 0x02, 0x01, 0x04, 0x00, 0x55, 0x73, 0x02, 0x74, 0x09, 0x00, 0x00, 0x00, 0x01, 0x00 }, "Write general device parameters");
                            writeGenPara2.RegisterListener(this);
                            m_target.Queue(writeGenPara2);
                            
                        }
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[10];
                        break;

                    case "h": // help menu
                            Console.WriteLine("\nesc = abort");
                            Console.WriteLine("b = keep bootloader running for programming mode");
                            Console.WriteLine("program = program device with new firmware");
                            Console.WriteLine("pwncomm = PWM communication");
                            Console.WriteLine("startapp = Start user application in debug mode");
                            Console.WriteLine("starteol = Start EOL application");
                            Console.WriteLine("eol9600 = Start EOL application at 9600");
                            Console.WriteLine("readapp = read application version");
                            Console.WriteLine("readpro = read protocol version");
                            Console.WriteLine("readlib = read freescale library version");
                            Console.WriteLine("readboot = read bootLoader version");
                            Console.WriteLine("readesnl = read general device parameters for ESN (little endian)");
                            Console.WriteLine("readesnb = read general device parameters for ESN (big endian)");
                            Console.WriteLine("recpow - read the received power");
                            Console.WriteLine("setled = set LED Color");
                            Console.WriteLine("readled = read LED color");
                            //Console.WriteLine("setesnol = set ESN and HW the old way, RC5 and before"); No longer supported
                            Console.WriteLine("setesnnw = set ESN and HW the new way, RC6 and after");
                            Console.WriteLine("setcoil or d = set transmitter coils");
                            Console.WriteLine("setseq = set sequence");
                            Console.WriteLine("transpow = read trasnmitted power");
                            Console.WriteLine("startchg = start charging - leave silence mode");
                            Console.WriteLine("stopchrg = stop charging - enter silence mode");
                            Console.WriteLine("setvrmax = Set Vrail to maximum value");
                            Console.WriteLine("setvrmin = Set Vrail to minimum value");
                            Console.WriteLine("capstate = Read capacitive sensor state");
                            Console.WriteLine("capvalue = Set capacitive sensor values");
                        commandIndex = -1;
                            keyCommand = new string[10];
                            break;

                        case "setcoil":
                        case "d":
                            Console.Write("\nPress Numpad 0 for coil 0\n");
                            Console.Write("Press Numpad 1 for coil 1\n");
                            Console.Write("Press Numpad 2 for coil 2\n");
                            Console.Write("Press any other key to remove user control of coils\n");
                            ConsoleKey coilkey = Console.ReadKey().Key;
                            byte coilId = 0xFF;
                            if (coilkey == ConsoleKey.NumPad0)
                            {
                                coilId = 0x00;
                            }
                            else if (coilkey == ConsoleKey.NumPad1)
                            {
                                coilId = 0x01;
                            }
                            else if (coilkey == ConsoleKey.NumPad2)
                            {
                                coilId = 0x02;
                            }
                            else
                            {
                            commandIndex = -1;
                                keyCommand = new string[10];
                                Console.WriteLine("\nCommand not recognized, returned to main menu\n");
                            break;

                            }

                        GeneralCommand setactivecoil = new GeneralCommand(m_si, 0x01, 0x06, new byte[] { 0x19, coilId }, "set active coil");
                            setactivecoil.RegisterListener(this);
                            m_target.Queue(setactivecoil);
                            Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                            keyCommand = new string[10];
                            break;


                    case "transpow":
                        GeneralCommand transpower = new GeneralCommand(m_si, 0x01, 0x04, new byte[] { 0x06 }, "read transmit power");
                        transpower.RegisterListener(this);
                        m_target.Queue(transpower);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[10];
                        break;

                    case "recpow":
                        GeneralCommand receivepower = new GeneralCommand(m_si, 0x01, 0x04, new byte[] { 0x01 }, "read receive power");
                        receivepower.RegisterListener(this);
                        m_target.Queue(receivepower);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[10];
                        break;

                    case "capstate":
                        GeneralCommand capState = new GeneralCommand(m_si, 0x01, 0x12, new byte[] {  }, "read cap sensor state");
                        capState.RegisterListener(this);
                        m_target.Queue(capState);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[10];
                        break;

                    case "capvalue":
                        GeneralCommand capValue = new GeneralCommand(m_si, 0x01, 0x14, new byte[] { }, "read cap sensor values");
                        capValue.RegisterListener(this);
                        m_target.Queue(capValue);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[10];
                        break;

                    case "stopchrg":
                        GeneralCommand silencemode = new GeneralCommand(m_si, 0x01, 0x3B, new byte[] { 0x02 }, "stop charging");
                        silencemode.RegisterListener(this);
                        m_target.Queue(silencemode);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[10];
                        break;

                    case "startchg":
                        GeneralCommand silencemodeoff = new GeneralCommand(m_si, 0x01, 0x3B, new byte[] { 0x01 }, "restart charging");
                        silencemodeoff.RegisterListener(this);
                        m_target.Queue(silencemodeoff);
                        Console.WriteLine("\nCommand executed, returned to main menu\n");
                        commandIndex = -1;
                        keyCommand = new string[10];
                        break;

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
            
            while(!m_bootloader_ready && (--cnt)>0)
            {
                Console.WriteLine("REQUEST:KEEP_BL_RUNNING");
                m_target.Queue(set_bl);
                m_target.Wait();
            }
        }


        private void UploadApplication(WcaInterfaceLibrary.SerialInterface si,Target target, string full_file_name)
        {
            StreamReader sr = new StreamReader(full_file_name, Encoding.ASCII);
            GeneralCommand upload_cmd;
            string line;
            byte[] sdata;
            ushort seg_cnt = 0;
            byte[] seg_cnt_ba;
            
            while((line = sr.ReadLine()) != null)
            {
                line = "00" + line + "\r\n";
                Console.Write(line);

                sdata = Encoding.Default.GetBytes(line.ToCharArray());
                seg_cnt_ba = BitConverter.GetBytes(seg_cnt);
                sdata[0] = seg_cnt_ba[0];
                sdata[1] = seg_cnt_ba[1];
                seg_cnt++;

                upload_cmd = new GeneralCommand(si, 0x01, 0x2E, sdata , "");
                target.Queue(upload_cmd);
                target.Wait();
            }
            sr.Close();
            Console.WriteLine("Upload application done.");
        }


        #region IInterfaceListener Members

        public void OnReceive(IInterface sender, byte[] data, int dataLength)
        {
            if (ascii)
            {
                //Console.WriteLine("\nThis is OnReceive writing ascii...");
                string str = System.Text.Encoding.Default.GetString(data);
                Console.WriteLine(str);
                sw.WriteLine(str);
            }
            else
            {
                //Console.WriteLine("\nThis is OnReceive writing raw data...");
                Console.WriteLine(WcaInterfaceLibrary.Converter.ByteArray2String(data));
                sw.WriteLine(WcaInterfaceLibrary.Converter.ByteArray2String(data));
            }
        }

        #endregion

        #region ICommandListener Members

        public void OnNotification(ICommand sender)
        {
            if(sender.Name.Equals("KEEP_BL_RUNNING"))
            {
                Console.WriteLine("BL:" + sender.Result.ToString());
                if(sender.Result == WcaInterfaceCommandResult.PosAck)
                {
                    m_bootloader_ready = true;
                }
            }
            else
            {
                //Console.WriteLine("This is OnNotification print out....");
                Console.WriteLine(sender.ToString());
            }

            
        }

        #endregion
    }
}
