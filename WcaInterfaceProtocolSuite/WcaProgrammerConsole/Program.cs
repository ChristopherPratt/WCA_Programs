using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using WcaInterfaceLibrary;
using System.Threading;
using System.IO;
using System.Windows.Forms;

namespace WcaDVConsole
{
    public class Program
    {
        //[STAThread]
        //public static void Main(string[] args)
        //{

        //}
        
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Laird Grand Blanc Wireless Charger EOL Version 4.1.3\nPress \"ctrl + c\" to exit the program\n");
            
            if (args.Length == 2) //keeping for old way of entering the program
            {
                WcaDebugTester tester = new WcaDebugTester();
                string comport = args[0];
                globals.program = args[1]; 
                if (File.Exists(globals.program))
                {
                    tester.Run(comport, globals.program);
                }
                else  { Console.WriteLine("File not found: " + globals.program); }
            }
            if (args.Length == 3) //keeping for old way of entering the program and allowing the changing of the baud rate
            {
                globals.baud = Convert.ToInt32(args[2]);
                WcaDebugTester tester = new WcaDebugTester();
                string comport = args[0];
                globals.program = args[1];
                if (File.Exists(globals.program))
                {
                    tester.Run(comport, globals.program);
                }
                else { Console.WriteLine("File not found: " + globals.program); }
            }

            if (args.Length == 0) // main entry for program - this way will not let you program the device. for that you must put it in the arguments in the command prompt.
            {
                Program myProgram = new Program();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainFrame(myProgram));
                while (false)
                {

                    Console.WriteLine("Select one of the Following:"); // for those who don't care about programming at the moment
                    Console.WriteLine("0: Enter programming mode.");
                    Console.WriteLine("1: Enter EOL test mode.");
                    Console.WriteLine("2: Enter EV test mode.");
                    Console.WriteLine("3: Change baud rate. (current is " + globals.baud + ")");
                    Console.WriteLine("4: Change Timeout (current is " + globals.timeout + "ms)");
                    Console.WriteLine("5: Enter firmware file for programming (" + globals.program + " selected)");
                    Console.WriteLine("6: Enter Programmer Debug mode (" + globals.debug + " selected)"); // for finding coil failure
                    Console.WriteLine("7: Enter Bootloader mode.");
                    ConsoleKeyInfo key;
                    key = Console.ReadKey();
                    if (key.KeyChar.ToString() == "0") //programming mode
                    {
                        WcaDebugTester tester = new WcaDebugTester();
                        Console.Write("\nPlease enter the COM Port for the device you want to use and press enter (ex: 23): ");
                        string comport = "COM" + Console.ReadLine();
                        while(true)
                        {
                            Console.WriteLine("\nPlease enter the new file name now.");
                            string temp = Console.ReadLine();
                            string[] arrayTemp = temp.Split('.');
                            if (arrayTemp.Length > 0 && arrayTemp[arrayTemp.Length - 1].ToUpper().Equals("S19"))
                            {
                                globals.program = temp;
                                break;
                            }
                            else Console.WriteLine("File name entered incorrectly, please try again. (expecting a file name ending in \"s19\"");
                        }
                        globals.baud = 115200; // in order to program you must communicate to the dut in 115200. after that though you must change to 9600 because that is what the debug messages talk as.
                        tester.Run(comport, globals.program);
                    }
                    if (key.KeyChar.ToString() == "1") // eol mode
                    {
                        WcaDebugTester tester = new WcaDebugTester();
                        Console.Write("\nPlease enter the COM Port for the device you want to use and press enter (ex: 23): ");
                        string comport = "COM" + Console.ReadLine();
                        if (globals.program.Equals("none")) globals.program = "";
                        tester.Run(comport, globals.program); // Run function requires 2 arguments, but since we aren't requiring people to enter a .S19 file for programming, i leave it like ""
                        break;
                    }
                    else if (key.KeyChar.ToString() == "2") // ev test mode
                    {
                        Console.WriteLine("Select one of the Following:");
                        Console.WriteLine("0: Enter Continuous EV Test mode");
                        Console.WriteLine("1: Enter Timed Test Mode");
                        ConsoleKeyInfo evkey;
                        evkey = Console.ReadKey();
                        if (evkey.KeyChar.ToString() == "0") // ev test mode
                        {
                            WcaDebugTester tester = new WcaDebugTester();
                            bool end = true;
                            string comms = "";
                            string ports = "";
                            string[] dacport = { "0", "8", "1", "9", "2", "10", "3", "11", "4", "12", "5", "13", "6", "14", "7" };// all analog input daq inputs in the order that they should be plugged in.
                            int dIndex = 0;
                            int cIndex = 0;
                            while (end) // loop for entering in multiple devices and related DAC port for EV testing.
                            { // what is happening in this loop is I am appending to a string and using commas to separate them, that way I can split by the commas and make an array with
                              // the exact amount of elements that are needed.
                                Console.Write("\nPlease enter the COM Port for the Wireless Chargers you want to use press enter (ex: 24): ");
                                comms += "COM" + Console.ReadLine().ToUpper();
                                //Console.Write("Please enter the analog input for coil 0 of the above device: ");
                                //ports += Console.ReadLine().ToUpper();
                                //Console.Write("Please enter the analog input for coil 1 of the above device: ");
                                //ports += ("," + Console.ReadLine().ToUpper());
                                //Console.Write("Please enter the analog input for coil 2 of the above device: ");
                                //ports += ("," + Console.ReadLine().ToUpper());
                                cIndex = 0;
                                Console.Write("Do you wish to add another Wireless Charger? enter y or n\n");
                                if (Console.ReadKey().KeyChar.ToString().ToUpper() == "Y")
                                {
                                    //ports += ",";
                                    comms += ",";
                                }
                                else { end = false; }
                            }
                            string[] comport = comms.Split(',');
                            //string[] dacport = ports.Split(',');
                            string[,] allDacPort = new string[comport.Length, 3];// this part is a little weird because each device needs 3 inputs from the DAC. so i just made a 2  dimensional array and stored them that way
                            int c = 0;
                            for (int a = 0; a < comport.Length; a++)
                            {
                                for (int b = 0; b < 3; b++)
                                {
                                    allDacPort[a, b] = dacport[c];
                                    c++;
                                }
                            }
                            EVtestMode EV = new EVtestMode();
                            //EV.Run(comport, allDacPort);
                            break;
                        }
                        else if (evkey.KeyChar.ToString() == "1") // timed test mode
                        {
                            Console.WriteLine("Select one of the Following:");
                            Console.WriteLine("0: Enter timed test with 14.5 hours normal, 6 hours no coils, 3.5 hours hours normal");
                            ConsoleKeyInfo timedkey;
                            timedkey = Console.ReadKey();
                            if (timedkey.KeyChar.ToString() == "0")
                            {
                                WcaDebugTester tester = new WcaDebugTester();
                                bool end = true;
                                string comms = "";
                                string ports = "";
                                string[] dacport = { "0", "8", "1", "9", "2", "10", "3", "11", "4", "12", "5", "13", "6", "14", "7" };// all analog input daq inputs in the order that they should be plugged in.
                                int dIndex = 0;
                                int cIndex = 0;
                                while (end) // loop for entering in multiple devices and related DAC port for EV testing.
                                { // what is happening in this loop is I am appending to a string and using commas to separate them, that way I can split by the commas and make an array with
                                  // the exact amount of elements that are needed.
                                    Console.Write("\nPlease enter the COM Port for the Wireless Chargers you want to use press enter (ex: 24): ");
                                    comms += "COM" + Console.ReadLine().ToUpper();
                                    //Console.Write("Please enter the analog input for coil 0 of the above device: ");
                                    //ports += Console.ReadLine().ToUpper();
                                    //Console.Write("Please enter the analog input for coil 1 of the above device: ");
                                    //ports += ("," + Console.ReadLine().ToUpper());
                                    //Console.Write("Please enter the analog input for coil 2 of the above device: ");
                                    //ports += ("," + Console.ReadLine().ToUpper());
                                    cIndex = 0;
                                    Console.Write("Do you wish to add another Wireless Charger? enter y or n\n");
                                    if (Console.ReadKey().KeyChar.ToString().ToUpper() == "Y")
                                    {
                                        //ports += ",";
                                        comms += ",";
                                    }
                                    else { end = false; }
                                }
                                string[] comport = comms.Split(',');
                                //string[] dacport = ports.Split(',');
                                string[,] allDacPort = new string[comport.Length, 3];// this part is a little weird because each device needs 3 inputs from the DAC. so i just made a 2  dimensional array and stored them that way
                                int c = 0;
                                for (int a = 0; a < comport.Length; a++)
                                {
                                    for (int b = 0; b < 3; b++)
                                    {
                                        allDacPort[a, b] = dacport[c];
                                        c++;
                                    }
                                }
                                TimedEVtest0 TE = new TimedEVtest0();
                                //TE.Run(comport, allDacPort);
                                break;
                            }
                        }
                    }
                    else if (key.KeyChar.ToString() == "3") //select baud rate
                    {
                        Console.WriteLine("\nSelect the baud rate you wish to use.");
                        Console.WriteLine("1: 115200");
                        Console.WriteLine("2: 9600");
                        ConsoleKeyInfo mkey;
                        mkey = Console.ReadKey();
                        if (mkey.KeyChar.ToString() == "1")
                        {
                            globals.baud = 115200;
                        }
                        else if (mkey.KeyChar.ToString() == "2")
                        {
                            globals.baud = 9600;
                        }
                        else Console.WriteLine("command not recognized");
                    }
                    else if (key.KeyChar.ToString() == "4") // change timeout time
                    {
                        Console.WriteLine("\nPlease enter the timeout time now (in milliseconds).");
                        globals.timeout = Convert.ToInt32(Console.ReadLine());
                    }
                    else if (key.KeyChar.ToString() == "5") // select programming file
                    {
                        Console.WriteLine("\nPlease enter the file name now.");
                        string temp = Console.ReadLine();
                        string[] arrayTemp = temp.Split('.');
                        if (arrayTemp.Length > 0 && arrayTemp[arrayTemp.Length - 1].ToUpper().Equals("S19")) globals.program = temp;
                        else Console.WriteLine("File name entered incorrectly");
                    }
                    else if (key.KeyChar.ToString() == "6") // select debug mode
                    {
                        globals.debug = true;
                    }
                    if (key.KeyChar.ToString() == "7") // enter bootloader mode
                    {
                        globals.baud = 115200;
                        WcaDebugTester tester = new WcaDebugTester();
                        Console.Write("\nPlease enter the COM Port for the device you want to use and press enter (ex: 23): ");
                        string comport = "COM" + Console.ReadLine();
                        if (globals.program.Equals("none")) globals.program = "";
                        Console.WriteLine("\nPlease cycle power to device.");
                        tester.Run(comport, globals.program); // Run function requires 2 arguments, but since we aren't requiring people to enter a .S19 file for programming, i leave it like ""                        
                        break;
                    }
                }
            }

        }
    }
}
