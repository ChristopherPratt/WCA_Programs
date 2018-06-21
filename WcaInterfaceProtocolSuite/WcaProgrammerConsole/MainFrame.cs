using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Timers;
using System.Windows.Forms;



namespace WcaDVConsole
{
    public partial class MainFrame : Form
    {
        Control[] myControls;
        Thread EVthread, checkComports;


        Program myProgram;
        Commands myCommands;
        EVtestMode ev;
        TimedEVtest0 timedEVTest0;
        LEDframe[] ledframe;
        SetESNframe[] setesnframe;
        delegate void dgetpMainFrame(Action job);
        delegate void dWriteToConsole(string myString);
        delegate void dEnableConsole(bool enable);
        delegate void dSetEsn(int dutNum, string esn, Color myColor);
        delegate void dgetLBfirmwareEVmode(ref string temp);
        List<string> favCommands, activeDUTControl;
        bool testCurrentlyRunning = false, textReady = false, firmwareSelected = false, cancelCommand = false, activeCommand = false, EVmode = false, enableStart = false, activeTest = false;
        public bool programmingEnabled = false, comportTimerBreak = false, autoScrollBreak = false;
        public string textReadyString, evfirmwareprogramtestmode, ProgramMode = "", Firmware = "", Console, tempConsole;
        string[] availableComports;
        public int ConsoleCount, rtbLength;
        
        List<Control> lprogramMode, lDUTnum, lFirmware, lvalidation, lvalidationStart, lCommands, lfavCommands, lConsole, lEOLmode, ldut1, ldut2, ldut3, ldut4, ldut5, ldut6, activeControls, lRadioButtons;
        List<List<Control>> lduts, ActiveDUTs;

        List<string> ProgramModeList = new List<string> {
            "End of line test mode",
            "Debug mode",
            "Validation test mode",
            "Programming mode"};
        List<string> EVModeList = new List<string> {
            "Continuous test mode",
            "14.5 on, 6 off, 3.5 on 10 cycle test mode"};
        List<string> DUTlist = new List<string> {
            "DUT 1",
            "DUT 2",
            "DUT 3",
            "DUT 4",
            "DUT 5",
            "DUT 6"};
        List<string> CommandList = new List<string> {
            "Read application version",
            "Read bootloader version",
            "Read bridge voltage",
            "Read capacitive sensor",
            "Read coil current peak",
            "Read coil current RMS",
            "Read coil voltage from DAQ",
            "Read ADC current",
            "Read ADC voltage",
            "Read ESN (big endian)",
            "Read ESN (little endian)",
            "Read LED color",
            "Read library version",
            "Read power loss(mW)",
            "Read protocol version",
            "Read PWM communication",
            "Read receive power",
            "Read temperature(C)",
            "Read transmit power",
            "Set bridge voltage",
            "Set ESN",
            "Set LED color",
            "Set transmitter coil",
            "Set Vrail MAX",
            "Set Vrail MIN",
            "Start EOL",
            "Start charging",
            "Start debug",
            "Stop charging"};
        public MainFrame(Program p)
        {
            myCommands = new Commands(this);
            myProgram = p;
            favCommands = new List<string>();
            activeControls = new List<Control>();
            activeDUTControl = new List<string>();
            ActiveDUTs = new List<List<Control>>();
            InitializeComponent();
            populateControlLists();
            populateElements();
            setStartupControls();
            //startScrollTimer();
            setesnframe = new SetESNframe[6];
            ledframe = new LEDframe[6];
            rtbConsole.HideSelection = false;

        }
        private void populateControlLists()
        {
            lprogramMode = new List<Control>() { lblprogramMode, lbProgramMode, Laird };
            lDUTnum = new List<Control>() { lblDUTS, lbDUTnumber };
            lFirmware = new List<Control>() { lblFirmwareEVmode, lbFirmwareFileEVprograms, bBrowseFirmwareFiles };
            lvalidation = new List<Control>() { lblFirmwareEVmode, lbFirmwareFileEVprograms };
            lvalidationStart = new List<Control>() { bStart, rtbConsole, cbAutoScroll };
            lCommands = new List<Control>() { lblcommands, lbCommands };
            lfavCommands = new List<Control>() { lblfavcommands, lbFavCommands };
            lConsole = new List<Control>() { rtbConsole, tbConsoleCommands, cbAutoScroll };
            lEOLmode = new List<Control>() { rtbConsole, tbConsoleCommands, lblfavcommands, lbFavCommands, lblcommands, lbCommands, cbAutoScroll, };
            ldut1 = new List<Control>() { lblDUT1, lblESN1, lblDUT1status, lbComPorts1, lblbaud1, panel1, rb96001, rb1152001 };
            ldut2 = new List<Control>() { lblDUT2, lblESN2, lblDUT2status, lbComPorts2, lblbaud2, panel2, rb96002, rb1152002 };
            ldut3 = new List<Control>() { lblDUT3, lblESN3, lblDUT3status, lbComPorts3, lblbaud3, panel3, rb96003, rb1152003 };
            ldut4 = new List<Control>() { lblDUT4, lblESN4, lblDUT4status, lbComPorts4, lblbaud4, panel4, rb96004, rb1152004 };
            ldut5 = new List<Control>() { lblDUT5, lblESN5, lblDUT5status, lbComPorts5, lblbaud5, panel5, rb96005, rb1152005 };
            ldut6 = new List<Control>() { lblDUT6, lblESN6, lblDUT6status, lbComPorts6, lblbaud6, panel6, rb96006, rb1152006 };
            lduts = new List<List<Control>>() { ldut1, ldut2, ldut3, ldut4, ldut5, ldut6 };
            lRadioButtons = new List<Control>() { rb96001, rb1152001, panel1, rb96002, rb1152002, panel2, rb96003, rb1152003, panel3, rb96004, rb1152004, panel4, rb96005, rb1152005, panel5, rb96006, rb1152006, panel6 };

        }
        private void populateElements() // runs all functions for populating all list boxes.
        {
            populatelbProgramMode();
            populatelbDUTnumber();
            queryComPorts();
            populatelbCommands();
        }
        private void populatelbProgramMode() // populates list box for programming modes
        {
            foreach (string temp in ProgramModeList) lbProgramMode.Items.Add(temp);
        }
        private void populatelbDUTnumber() // populates list box DUT 
        {
            foreach (string temp in DUTlist) lbDUTnumber.Items.Add(temp);
        }
        private void populateAllCOMportlbs(List<string> ports) //populates all comm ports in the lbCommports for each DUT
        {
            foreach (string port in ports) lbComPorts1.Items.Add(port);
            foreach (string port in ports) lbComPorts2.Items.Add(port);
            foreach (string port in ports) lbComPorts3.Items.Add(port);
            foreach (string port in ports) lbComPorts4.Items.Add(port);
            foreach (string port in ports) lbComPorts5.Items.Add(port);
            foreach (string port in ports) lbComPorts6.Items.Add(port);
        }
        private void removeCOMportlbs(List<string> ports) //populates all comm ports in the lbCommports for each DUT
        {
            foreach (string port in ports) lbComPorts1.Items.Remove(port);
            foreach (string port in ports) lbComPorts2.Items.Remove(port);
            foreach (string port in ports) lbComPorts3.Items.Remove(port);
            foreach (string port in ports) lbComPorts4.Items.Remove(port);
            foreach (string port in ports) lbComPorts5.Items.Remove(port);
            foreach (string port in ports) lbComPorts6.Items.Remove(port);
        }
        private void populatelbCommands() //populates info in lbCommands
        {
            foreach (string temp in CommandList) lbCommands.Items.Add(temp);
        }
        private void setStartupControls()
        {
            List<Control> tempControl = new List<Control>();
            foreach (Control temp in pMainFrame.Controls) tempControl.Add(temp);
            setDisabled(tempControl);
            setEnabled(lprogramMode);
        }
        private void disableForm(bool programming)
        {
            List<Control> tempControl = new List<Control>();
            foreach (Control temp in pMainFrame.Controls) temp.Enabled = false;
            if (!programming) bStart.Enabled = true;
            rtbConsole.Enabled = true;
            Laird.Enabled = true;
            cbAutoScroll.Enabled = true;
        }
        public void enableForm()
        {
            foreach (Control temp in activeControls) temp.Enabled = true;
            DUTEnable(activeDUTControl);
        }
        private void changeToFirmwareMode()
        {

        }
        public void startNewMode(string mode)
        {
            Action temp2 = () =>
            {
                lbProgramMode.SelectedItem = mode;
                run(mode);
            };
            getpMainFrame(temp2);
        }
        private void queryComPorts()
        {
            checkComports = new Thread(delegate ()
            {
                while(!comportTimerBreak)
                {
                    OnTimedEvent();
                    Thread.Sleep(1000);
                }
            });
            checkComports.Name = "CheckComports";
            checkComports.Start();
        }
        private void OnTimedEvent()
        {
            if (availableComports == null)
            {
                availableComports = SerialPort.GetPortNames();
                List<string> tempAddcomports = new List<string>();
                foreach (string temp in availableComports) tempAddcomports.Add(temp);
                Action temp1 = () => { populateAllCOMportlbs(tempAddcomports); };
                getpMainFrame(temp1);
                return;
            }
            string[] tempPorts = SerialPort.GetPortNames();
            List<string> deadComports = new List<string>();
            foreach (string oldcom in availableComports)
            {
                bool matched = false;
                foreach (string newcom in tempPorts)
                {
                    if (oldcom == newcom) matched = true;
                }
                if (!matched) deadComports.Add(oldcom);
            }

            List<string> newComports = new List<string>();
            foreach (string newcom in tempPorts)
            {
                bool matched = false;
                foreach (string oldcom in availableComports)
                {
                    if (oldcom == newcom) matched = true;
                }
                if (!matched) newComports.Add(newcom);
            }

            if (deadComports.Count > 0)
            {
                Action temp2 = () => { removeCOMportlbs(deadComports); };
                getpMainFrame(temp2);
            }

            if (newComports.Count > 0)
            {
                Action temp2 = () => { populateAllCOMportlbs(newComports); };
                getpMainFrame(temp2);
            }

            availableComports = tempPorts;


        }
        private void changeAutoScroll()
        {
            if (cbAutoScroll.Checked)
            {
                rtbConsole.HideSelection = false;
            }
            else
            {
                rtbConsole.HideSelection = true;
                rtbConsole.SelectionStart = rtbConsole.Text.Length;
                rtbConsole.ScrollToCaret();
            }
        }


        public void run(string p) // allows user to select a mode for the program. changes gui and warns user if test is already running.
        {
            if (!p.Equals("")) ProgramMode = p;
            for (int i = 0; i < 6; i++) if (myCommands.Devices[i] != null) if (myCommands.Devices[i].ready) { myCommands.Devices[i].setGUIESN = true; myCommands.runCommands("Read ESN (big endian)", i); myCommands.Devices[i].failCount = 0; }

            List<Control> tempControlList = new List<Control>();
            switch (ProgramMode)
            {
                case "Debug mode":
                case "End of line test mode":
                    {
                        foreach (Control temp in lDUTnum) tempControlList.Add(temp);
                        foreach (Control temp in lprogramMode) tempControlList.Add(temp);
                        if (myCommands.anyActiveDuts()) foreach (Control temp in lEOLmode) tempControlList.Add(temp);
                        //lbDUTnumber.SelectionMode = SelectionMode.One; // single selections are necessary for this mode since we only want to program one DUT at a time  
                        lbDUTnumber.SelectionMode = SelectionMode.MultiSimple; // since we use multiple DUTs, lets allow multiple selections

                        updateGUI(tempControlList);
                        updateDUTS(getSelectedDuts());
                        lbFirmwareFileEVprograms.Items.Clear();

                        break;
                    }
                case "Validation test mode":
                    {
                        foreach (Control temp in lprogramMode) tempControlList.Add(temp); // enable program mode section
                        foreach (Control temp in lDUTnum) tempControlList.Add(temp);// enable DUT number section  
                        foreach (Control temp in lvalidation) tempControlList.Add(temp); // enable validation section   
                        lbFirmwareFileEVprograms.Items.Clear();
                        if (myCommands.anyActiveDuts() && lbFirmwareFileEVprograms.SelectedItems.Count > 0) tempControlList.Add(bStart);
                        lbDUTnumber.SelectionMode = SelectionMode.MultiSimple; // since we use multiple DUTs, lets allow multiple selections
                        updateGUI(tempControlList);
                        updateDUTS(getSelectedDuts());
                        foreach (string temp in EVModeList) lbFirmwareFileEVprograms.Items.Add(temp); // populate list box of current validation modes
                        lblFirmwareEVmode.Text = "Validation Test Mode";
                        break;
                    }
                case "Programming mode":
                    {
                        foreach (Control temp in lprogramMode) tempControlList.Add(temp); // enable program mode section
                        foreach (Control temp in lDUTnum) tempControlList.Add(temp);// enable DUT number section  
                        foreach (Control temp in lFirmware) tempControlList.Add(temp); // enable firmware section
                        lbFirmwareFileEVprograms.Items.Clear();
                        if (myCommands.anyActiveDuts() && lbFirmwareFileEVprograms.SelectedItems.Count > 0)
                        {
                            tempControlList.Add(bStart);
                        }
                        //lbDUTnumber.SelectionMode = SelectionMode.One; // single selections are necessary for this mode since we only want to program one DUT at a time  
                        lbDUTnumber.SelectionMode = SelectionMode.MultiSimple; // since we use multiple DUTs, lets allow multiple selections
                        updateGUI(tempControlList);
                        updateDUTS(getSelectedDuts());
                        lblFirmwareEVmode.Text = "Firmware Files";

                        break;
                    }
            }
        }
        private void updateGUI(List<Control> controls)
        {
            bool matched;
            List<Control> deleteControls = new List<Control>();
            List<Control> addControls = new List<Control>();

            foreach (Control temp in activeControls) //for each control currently active
            {
                matched = false;
                for (int a = 0; a < controls.Count; a++) // for each control user wants enabled
                {
                    if (temp.Equals(controls[a])) matched = true;
                }
                if (!matched) // if they don't match disable the one that is currently active and delete the device object if its available.
                {
                    setDisabled(new List<Control> { temp });
                    deleteControls.Add(temp);
                }
            }
            foreach (Control temp in deleteControls) if (activeControls.Contains(temp)) activeControls.Remove(temp);
            foreach (Control temp in controls) // for each control that the user wants enabled
            {
                matched = false;
                for (int a = 0; a < activeControls.Count; a++) // for each control currently enabled
                {
                    if (temp.Equals(activeControls[a])) matched = true;
                }
                if (!matched) // if they don't match then add it to the gui
                {
                    setEnabled(new List<Control> { temp });
                }
            }
        }
        public void updateDUTS(List<string> newDuts) //if the user goes from validation mode to EOL mode the program cuts down from multiple duts to 1 dut, we want to disable those extra duts.
        {
            bool matched;
            List<string> deleteDUTs = new List<string>();
            List<string> addDUTs = new List<string>();

            foreach (string tempdut in activeDUTControl) //for each dut currently active
            {
                matched = false;
                for (int a = 0; a < newDuts.Count; a++) // for each dut user wants enabled
                {
                    if (tempdut.Equals(newDuts[a])) matched = true;
                }
                if (!matched) // if they don't match disable the one that is currently active and delete the device object if its available.
                {
                    DUTReset(tempdut);
                    DUTDisable(tempdut);
                    deleteDUTs.Add(tempdut);
                }
            }
            foreach (string temp in deleteDUTs) if (activeDUTControl.Contains(temp)) activeDUTControl.Remove(temp); // delete old items from active dut list
            foreach (string tempdut in newDuts) // for each dut that the user wants enabled
            {
                matched = false;
                for (int a = 0; a < activeDUTControl.Count; a++) // for each dut currently enabled
                {
                    if (tempdut.Equals(activeDUTControl[a])) matched = true;
                }
                if (!matched) // if they don't match then add it to the gui
                {
                    DUTEnable(new List<string> { tempdut });
                    addDUTs.Add(tempdut);
                }
            }
            foreach (string temp in addDUTs) activeDUTControl.Add(temp); // add new items to active dut list
            handlingProgrammingMode(); // disable radio buttons because programming mode requires 115200, reset and reinitialize dut from programming mode
        }
        public void handlingProgrammingMode()// disable radio buttons because programming mode requires 115200
        {
            if (ProgramMode.Equals("Programming mode"))
            {
                foreach (List<Control> templist in ActiveDUTs)
                {
                    ((Panel)templist[5]).Enabled = false;
                    ((RadioButton)templist[6]).Checked = false;
                    ((RadioButton)templist[7]).Checked = true; // set the radio button ( though it is diabled) to 115200 as that is the baud rate for programming
                }
                foreach (string dut in activeDUTControl)
                {
                    int deviceIndex = myCommands.findDUTindex(dut);
                    if (myCommands.Devices[deviceIndex] == null) continue;
                    if (myCommands.Devices[deviceIndex].bootloaderModeReady == false)
                    {
                        setESN(dut, "Cycle Power", Color.Yellow);
                        if (myCommands.updateSerial(myCommands.Devices[deviceIndex].comport, 115200, dut, false))
                        {
                            myCommands.Devices[deviceIndex].bootloaderModeReady = true;
                            myCommands.Devices[deviceIndex].wakeUp = true;
                            myCommands.Devices[deviceIndex].wakeUp = true;
                            myCommands.Devices[deviceIndex].wakeUp = true;
                        }
                    }
                }
            }
            else // re-enable radio buttons if not in programming mode
            {
                foreach (string dut in activeDUTControl)
                {
                    int deviceIndex = myCommands.findDUTindex(dut);
                    if (myCommands.Devices[deviceIndex] == null) continue;
                    myCommands.Devices[deviceIndex].bootloaderModeReady = false;
                    myCommands.Devices[deviceIndex].wakeUp = false;
                }
                foreach (List<Control> templist in ActiveDUTs)
                {
                    ((Panel)templist[5]).Enabled = true;
                }
                if (myCommands.anyActiveDuts()) { }
                   // if (myCommands.Devices[0].programmingMode) myCommands.checkifComPortExists(myCommands.Devices[0].dutNumber, myCommands.Devices[0].comport, ProgramMode, myCommands.Devices[0].baud, false);
            }
        }
        
        public void returnFromProgrammingMode()
        {
            Action temp2 = () =>
            {
                foreach (List<Control> templist in ActiveDUTs)
                {
                    ((Panel)templist[5]).Enabled = true;
                    ((RadioButton)templist[6]).Checked = true;
                    ((RadioButton)templist[7]).Checked = false; // set the radio button ( though it is diabled) to 9600 as that is the baud rate for programming
                }
            };
            getpMainFrame(temp2);
        }
        public List<int> findActiveDUTindexes()
        {
            List<int> temp = new List<int>();
            for (int a = 0; a < 6; a++)
            {
                if (myCommands.Devices[a] == null) continue;
                if (myCommands.Devices[a].ready == true)
                temp.Add(a);
            }
            return temp;
        }
        private void setEnabled(List<Control> controls)  //enables different controls in gui
        {
            foreach (Control temp in controls)
            {
                temp.Enabled = true;
                activeControls.Add(temp);
            }
        }
        public void setDisabled(List<Control> controls)  //disables different controls in gui
        {
            foreach (Control temp in controls)
            {
                temp.Enabled = false;

            }
        }
        private List<Control> findDUTcontrols(string dut)
        {
            switch (dut)
            {
                case "DUT 1": { return ldut1; }
                case "DUT 2": { return ldut2; }
                case "DUT 3": { return ldut3; }
                case "DUT 4": { return ldut4; }
                case "DUT 5": { return ldut5; }
                case "DUT 6": { return ldut6; }
            }
            return new List<Control> { };
        }
        private void DUTDisable(string dut)
        {
            List<Control> temp = findDUTcontrols(dut);
            foreach (Control control in temp) control.Enabled = false; ActiveDUTs.Remove(temp);
        }

        public void DUTReset(string dut) // change selected DUT back to a disconnected dut
        {
            List<Control> temp = findDUTcontrols(dut);
            temp[1].Text = "Not Connected"; temp[1].BackColor = SystemColors.Control; temp[2].BackColor = SystemColors.Control; ((ListBox)temp[3]).SelectedItems.Clear();


            myCommands.removeDUT(dut); // if the DUt that is being removed has an active device object and serial connection then it closes that too.
        }


        public void DUTEnable(List<string> tempduts) // enables or disables different controls in gui based on selection of DUTS
        {
            List<Control> myDuts = new List<Control>();

            foreach (string dut in tempduts) // add certain DUT control panels for each DUT which was selected in lbDUTNumber
            {
                List<Control> temp = findDUTcontrols(dut);
                foreach (Control control in temp) { myDuts.Add(control); }
                ActiveDUTs.Add(temp);

            }
            foreach (Control temp in myDuts) temp.Enabled = true;
        }
        public void setESN(string dut, string esn, Color myColor) // writes the esn and changes color of the dut box confirming the device is a wireless charger
        {
            Action temp2 = () =>
            {
                List<Control> temp = findDUTcontrols(dut);
                temp[1].Text = esn; temp[1].BackColor = myColor; temp[2].BackColor = myColor;


                if (!esn.Equals("COM Port Unavailable") && !esn.Equals("Not Connected")) // if device is a wireless charger that speaks correctly by read esn command with a neg ack or a pos ack
                {
                    checkconfiguration();
                }

            };
            getpMainFrame(temp2);
        }


        public void getpMainFrame(Action job) // set the gui console to enabled depending on some conditions
        {
            try
            {
                if (this.pMainFrame.InvokeRequired)
                {
                    dgetpMainFrame d = new dgetpMainFrame(getpMainFrame);
                    this.Invoke(d, new object[] { job });
                }
                else
                {
                    job();
                }
            }
            catch (Exception e) { };
        }
        public void startupFailure(int deviceIndex)
        {
            List<Control> temp = findDUTcontrols(myCommands.Devices[deviceIndex].dutNumber);
            Action temp3 = () =>
            {
                ((RadioButton)temp[6]).Checked = false;
                ((RadioButton)temp[7]).Checked = true;
                temp[1].Text = "Cycle Power";
                temp[1].BackColor = Color.Yellow;
                temp[2].BackColor = Color.Yellow;
                myCommands.updateSerial(myCommands.Devices[deviceIndex].comport, 115200, myCommands.Devices[deviceIndex].dutNumber, true);
                myCommands.Devices[deviceIndex].wakeUp = true;
                myCommands.Devices[deviceIndex].ready = false;
                myCommands.Devices[deviceIndex].commandTries = 2;
                //myCommands.checkifComPortExists(myCommands.Devices[deviceIndex].dutNumber, myCommands.Devices[deviceIndex].comport, ProgramMode, 115200, true);
            };
            getpMainFrame(temp3);
        }
        public void startEOLmode(int deviceIndex)
        {

            List<Control> temp = findDUTcontrols(myCommands.Devices[deviceIndex].dutNumber);
            Action temp3 = () =>
            {
                ((RadioButton)temp[6]).Checked = true;
                ((RadioButton)temp[7]).Checked = false;
                myCommands.checkifComPortExists(myCommands.Devices[deviceIndex].dutNumber, myCommands.Devices[deviceIndex].comport, ProgramMode, 9600, false);
            };
            getpMainFrame(temp3);
        }
        public void getLBfirmwareEVmode(ref string temp)
        {
            try
            {
                if (this.lbFirmwareFileEVprograms.InvokeRequired)
                {
                    dgetLBfirmwareEVmode d = new dgetLBfirmwareEVmode(getLBfirmwareEVmode);
                    this.Invoke(d, new object[] { temp });
                }
                else
                {
                    evfirmwareprogramtestmode = lbFirmwareFileEVprograms.SelectedItem.ToString();
                }

            }
            catch (Exception e) { writeToConsole(e.ToString()); };
        }
        public void writeToConsole(string myString) // writes to console window in gui
        {
            try
            {
                if (this.rtbConsole.InvokeRequired)
                {
                    dWriteToConsole d = new dWriteToConsole(writeToConsole);
                    this.Invoke(d, new object[] { myString });
                }
                else
                {
                    ConsoleCount++;
                    if (ConsoleCount > 1000)
                    {
                        rtbConsole.Text = tempConsole;
                        tempConsole = "";
                        ConsoleCount = 0;
                    }
                    tempConsole += myString;
                    rtbConsole.AppendText(myString);
                   // rtbConsole.Text = Console;
                }
            }
            catch (Exception e) { };
        }
        public string waitForEntry() // waits for user to hit enter in text box and return text entered from text box to Commands class or whoever needs it
        {
            activeCommand = true;
            while (!textReady)
            {
                Thread.Sleep(50);
                if (cancelCommand)
                {
                    cancelCommand = false;
                    activeCommand = false;
                    return "abortThread";
                }
            }
            activeCommand = false;
            textReady = false;
            return textReadyString;
        }

        private List<int> readyToProgram()
        {
            List<int> readyDevices = new List<int>();
            for(int a = 0; a < 6; a++)
            {
                if (myCommands.Devices[a] == null) continue;
                if (myCommands.Devices[a].bootloaderMode) readyDevices.Add(a);
            }
            return readyDevices;
        }

        public void changeBaudRadioButtons(string dutnum, int baud)
        {
            List<Control> templist = findDUTcontrols(dutnum);
            if (baud == 9600)
            {
                Action temp2 = () => {
                    ((RadioButton)templist[6]).Checked = true;
                    ((RadioButton)templist[7]).Checked = false;
                };
                getpMainFrame(temp2);
                
            }
            else
            {
                Action temp2 = () => {
                    ((RadioButton)templist[6]).Checked = false;
                    ((RadioButton)templist[7]).Checked = true;
                };
                getpMainFrame(temp2);
                
            }
        }
        private void checkconfiguration()
        {
            if (myCommands.anyActiveDuts()) // double checking that we have at least 1 device connected
            {
                switch (ProgramMode) // enabling new controls depending on the program mode
                {
                    case "End of line test mode":
                        {
                            if (!lbCommands.Enabled) setEnabled(lEOLmode); // enable all controls for sending commands
                            break;
                        }
                    case "Validation test mode":
                        {
                            if (!bStart.Enabled && lbFirmwareFileEVprograms.SelectedItems.Count > 0) setEnabled(lvalidationStart); // enable all controls for starting a test
                            break;
                        }
                    //case "Programming mode":
                    //    {
                    //        if (!bStart.Enabled && lbFirmwareFileEVprograms.SelectedItems.Count > 0) setEnabled(lvalidationStart); // enable all controls for programming (same as validation mode)
                    //        break;
                    //    }

                }
            }
            List<int> readyDevices = readyToProgram(); // making sure the available devices have properly made it into bootloader mode.
            if (readyDevices.Count != 0 && lbFirmwareFileEVprograms.SelectedItems.Count > 0) setEnabled(lvalidationStart); // enable all controls for programming)
        }
        private void addCommandToFav(string myString)
        {
            foreach (object temp in lbFavCommands.Items)
            {
                if (temp.ToString() == myString) { return; }
            }
            lbFavCommands.Items.Add(myString);
        }

        private void stopEVTestMode()
        {
            string mode = "";
            getLBfirmwareEVmode(ref mode);
            switch (evfirmwareprogramtestmode)
            {
                case "Continuous test mode":
                    {
                        continuousTestMode(false);
                        break;
                    }
                case "14.5 on, 6 off, 3.5 on 10 cycle test mode":
                    {
                        EV14_5on6off3_5on10cycleYestMode(false);
                        break;
                    }
            }
        }

        private void bBrowseFirmwareFiles_Click(object sender, EventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a Cursor.  
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = ".S19 Firmware File|*.S19";
            openFileDialog1.Title = "Select a .S19 Firmware File";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .CUR file was selected, open it.  
            Firmware = "";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Assign the cursor in the Stream to the Form's Cursor property.  
                Firmware = openFileDialog1.FileName;
            }
            if (!Firmware.Equals("")) lbFirmwareFileEVprograms.Items.Add(Firmware); // add filename to listbox
            lbFirmwareFileEVprograms.SelectedIndex = (lbFirmwareFileEVprograms.Items.Count - 1); //automatically select most recent choice into listbox



            //temp.Join();
        }


        public void setupProgressBar(int max)
        {
            Action temp3 = () =>
            {
                lblProgressBar.Enabled = true;
                progressBar.Maximum = max;
            };
            getpMainFrame(temp3);
        }
        public void updateProgressBar(int value, int max)
        {
            Action temp3 = () =>
            {
                progressBar.Value = value + 1;
                progressBar.Value = value;
                lblProgressBar.Text = ((int)(((float)value / (float)max) * 100)).ToString() + "%";
            };
            getpMainFrame(temp3);
        }
        public void resetProgressBar()
        {
            Action temp3 = () =>
            {
                progressBar.Value = 0;
                lblProgressBar.Text = "0%";
            };
            getpMainFrame(temp3);
        }
        public void updateProgrammingMode(string mode)
        {
            Action temp3 = () =>
            {
                ProgramMode = mode;
                lbProgramMode.SelectedItem = ProgramMode;
            };
            getpMainFrame(temp3);

        }

        private void runEVTestMode(bool start)
        {
            string mode = "";
            getLBfirmwareEVmode(ref mode);
            switch (evfirmwareprogramtestmode)
            {
                case "Continuous test mode":
                    {
                        continuousTestMode(start);
                        break;
                    }
                case "14.5 on, 6 off, 3.5 on 10 cycle test mode":
                    {
                        EV14_5on6off3_5on10cycleYestMode(start);
                        break;
                    }
            }
        }

        private void dgeqwrg(object sender, KeyEventArgs e)
        {

        }


        private List<string> getSelectedDuts()
        {
            List<string> tempdut = new List<string>();
            foreach (string tempnum in lbDUTnumber.SelectedItems) // for each selected dut
            {
                tempdut.Add(tempnum); // add it to tmpdut list
            }
            return tempdut;
        }

        private void rb96001_MouseDown(object sender, MouseEventArgs e)
        {
            if (myCommands.Devices[0] != null) myCommands.updateSerial(myCommands.Devices[0].comport, 9600, "DUT 1", true);
        }
        private void rb1152001_MouseDown(object sender, MouseEventArgs e)
        {
            if (myCommands.Devices[0] != null) myCommands.updateSerial(myCommands.Devices[0].comport, 115200, "DUT 1", true);
        }
        private void rb96002_MouseDown(object sender, MouseEventArgs e)
        {
            if (myCommands.Devices[1] != null) myCommands.updateSerial(myCommands.Devices[1].comport, 9600, "DUT 2", true);
        }
        private void rb1152002_MouseDown(object sender, MouseEventArgs e)
        {
            if (myCommands.Devices[1] != null) myCommands.updateSerial(myCommands.Devices[1].comport, 115200, "DUT 2", true);
        }

        private void MainFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            comportTimerBreak = true;
            if (checkComports != null) checkComports.Join();
            if (timedEVTest0 != null) timedEVTest0.end = true;
            if (ev != null) ev.end = true;
            if (EVthread != null) EVthread.Join();
            Application.Exit();            
        }

        private void rb96003_MouseDown(object sender, MouseEventArgs e)
        {
            if (myCommands.Devices[2] != null) myCommands.updateSerial(myCommands.Devices[2].comport, 9600, "DUT 3", true);
        }

        private void rtbConsole_VScroll(object sender, EventArgs e)
        {

        }

        private void rb1152003_MouseDown(object sender, MouseEventArgs e)
        {
            if (myCommands.Devices[2] != null) myCommands.updateSerial(myCommands.Devices[2].comport, 115200, "DUT 3", true);
        }

        private void rb96004_MouseDown(object sender, MouseEventArgs e)
        {
            if (myCommands.Devices[3] != null) myCommands.updateSerial(myCommands.Devices[3].comport, 9600, "DUT 4", true);
        }


        private void rb1152004_MouseDown(object sender, MouseEventArgs e)
        {
            if (myCommands.Devices[3] != null) myCommands.updateSerial(myCommands.Devices[3].comport, 115200, "DUT 4", true);
        }
        private void rb96005_MouseDown(object sender, MouseEventArgs e)
        {
            if (myCommands.Devices[4] != null) myCommands.updateSerial(myCommands.Devices[4].comport, 9600, "DUT 5", true);
        }
        private void rb1152005_MouseDown(object sender, MouseEventArgs e)
        {
            if (myCommands.Devices[4] != null) myCommands.updateSerial(myCommands.Devices[4].comport, 115200, "DUT 5", true);
        }
        private void rb96006_MouseDown(object sender, MouseEventArgs e)
        {
            if (myCommands.Devices[5] != null) myCommands.updateSerial(myCommands.Devices[5].comport, 9600, "DUT 6", true);
        }
        private void rb1152006_MouseDown(object sender, MouseEventArgs e)
        {
            if (myCommands.Devices[5] != null) myCommands.updateSerial(myCommands.Devices[5].comport, 115200, "DUT 6", true);
        }
        private void cbAutoScroll_MouseDown(object sender, MouseEventArgs e)
        {

        }
       
        private void cbAutoScroll_MouseUp(object sender, MouseEventArgs e)
        {
            changeAutoScroll();
        }
        public int getBaud(RadioButton myControl)
        {
            if (myControl.Checked) return 9600;
            else return 115200;
        }
        private void EV14_5on6off3_5on10cycleYestMode(bool start)
        {
            if (start)
            {
                timedEVTest0 = new TimedEVtest0();
                List<Commands.DeviceData> temp = new List<Commands.DeviceData>();
                for (int i = 0; i < 6; i++)
                    if (myCommands.Devices[i] != null)
                        if (myCommands.Devices[i].ready)
                        {
                            temp.Add(myCommands.Devices[i]);
                            myCommands.Devices[i].setGUIESN = true;
                            myCommands.runCommands("Read ESN (big endian)", i);
                            myCommands.Devices[i].failCount = 0;
                        }
                timedEVTest0.Run(temp, myCommands);

            }
            else
            {
                if (timedEVTest0 != null) timedEVTest0.endTestFromMainFrame();
            }            
        }       
                    
        private void continuousTestMode(bool start)
        {
            if (start)
            {
                ev = new EVtestMode();
                List<Commands.DeviceData> temp = new List<Commands.DeviceData>();
                for (int i = 0; i < 6; i++)
                    if (myCommands.Devices[i] != null) if (myCommands.Devices[i].ready) { temp.Add(myCommands.Devices[i]); myCommands.Devices[i].setGUIESN = true; myCommands.runCommands("Read ESN (big endian)", i); myCommands.Devices[i].failCount = 0; }
                ev.Run(temp, myCommands);
            }
            else
            {
                if (ev != null) ev.endTest();
                EVthread.Join();
                //ev.closeLog();
            }
        }

        private void lbDUTnumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateDUTS(getSelectedDuts());
            //if (myCommands.anyActiveDuts() && lbCommands.Enabled) setDisabled(lEOLmode); // disable all device controls if no valid duts are selected            
        }
        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void lbProgramMode_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            Object selecteditem = lbProgramMode.SelectedItem;
            run(selecteditem.ToString());
        }

        private void lbCommands_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbCommands.SelectedItem.ToString().Equals("Set LED color")) // open new LEDframe if the set led color command was selected and there currently isn't a LEDframe open
            {
                List<int> list = findActiveDUTindexes();
                foreach (int temp in list) openLEDcolorForm(temp);
                return;
            }
            else if (lbCommands.SelectedItem.ToString().Equals("Set ESN"))
            {
                List<int> list = findActiveDUTindexes();
                foreach (int temp in list) openSetESNform(temp);
                return;
            }
            if (activeCommand) { cancelCommand = true; Thread.Sleep(25); } // to prevent multiple commands being used at the same time. this can be confusing to the user
            Object selecteditem = lbCommands.SelectedItem;
            for (int i = 0; i < 6; i++)
            {
                if (myCommands.Devices[i] == null) continue;
                if (myCommands.Devices[i].ready)
                {
                    Thread.Sleep(100);
                    myCommands.runCommands(selecteditem.ToString(), i);
                }
            }
            addCommandToFav(selecteditem.ToString());
            tbConsoleCommands.Focus();
            tbConsoleCommands.Clear();
        }
        private void openLEDcolorForm(int dutIndex)
        {
            if (ledframe[dutIndex] == null)
            {
                ledframe[dutIndex] = new LEDframe(myCommands, dutIndex);
                ledframe[dutIndex].Show();
                return;
            }
            else if (ledframe[dutIndex].IsDisposed)
            {
                ledframe[dutIndex] = null;
                ledframe[dutIndex] = new LEDframe(myCommands, dutIndex);
                ledframe[dutIndex].Show();
                //return;
            }
            return;          // do nothing if form is already opened      
        }
        private void openSetESNform(int dutIndex)
        {
            if (setesnframe[dutIndex] == null)
            {
                setesnframe[dutIndex] = new SetESNframe(myCommands,dutIndex);
                setesnframe[dutIndex].Show();
                return;
            }
            else if (setesnframe[dutIndex].IsDisposed)
            {
                setesnframe[dutIndex] = null;
                setesnframe[dutIndex] = new SetESNframe(myCommands,dutIndex);
                setesnframe[dutIndex].Show();
                return;
            }
            return;          // do nothing if form is already opened 
        }
        private void tbConsoleCommands_TextChanged(object sender, EventArgs e)
        {
        }

        private void lbComPorts1_SelectedIndexChanged(object sender, EventArgs e) // initialze DUT subclass for holding all relevant info
        {
            if (lbComPorts1.SelectedItem == null) return;
            myCommands.checkifComPortExists("DUT 1", lbComPorts1.SelectedItem.ToString(), ProgramMode, getBaud(rb96001), false);
            if (myCommands.anyActiveDuts() && lbCommands.Enabled) setDisabled(lEOLmode);
        }
        private void lbComPorts2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbComPorts2.SelectedItem == null) return;
            myCommands.checkifComPortExists("DUT 2", lbComPorts2.SelectedItem.ToString(), ProgramMode, getBaud(rb96002), false);
            if (myCommands.anyActiveDuts() && lbCommands.Enabled) setDisabled(lEOLmode);
        }
        private void lbComPorts3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbComPorts3.SelectedItem == null) return;
            myCommands.checkifComPortExists("DUT 3", lbComPorts3.SelectedItem.ToString(), ProgramMode, getBaud(rb96003), false);
            if (myCommands.anyActiveDuts() && lbCommands.Enabled) setDisabled(lEOLmode);
        }
        private void lbComPorts4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbComPorts4.SelectedItem == null) return;
            myCommands.checkifComPortExists("DUT 4", lbComPorts4.SelectedItem.ToString(), ProgramMode, getBaud(rb96004), false);
            if (myCommands.anyActiveDuts() && lbCommands.Enabled) setDisabled(lEOLmode);
        }
        private void lbComPorts5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbComPorts5.SelectedItem == null) return;
            myCommands.checkifComPortExists("DUT 5", lbComPorts5.SelectedItem.ToString(), ProgramMode, getBaud(rb96005), false);
            if (myCommands.anyActiveDuts() && lbCommands.Enabled) setDisabled(lEOLmode);
        }
        private void lbComPorts6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbComPorts6.SelectedItem == null) return;
            myCommands.checkifComPortExists("DUT 6", lbComPorts6.SelectedItem.ToString(), ProgramMode, getBaud(rb96006), false);
            if (myCommands.anyActiveDuts() && lbCommands.Enabled) setDisabled(lEOLmode);
        }
        public void resetGUI()
        {
            Action temp3 = () =>
            {
                bStart.Text = "Start";
                activeTest = false;
                enableForm();
            };
            getpMainFrame(temp3);
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            if (!activeTest)
            {

                if (ProgramMode.Equals("Validation test mode"))
                {
                    
                    if (lbFirmwareFileEVprograms.SelectedItem.ToString().Equals("Continuous test mode"))
                    {
                        bStart.Text = "Stop";
                        activeTest = true;
                        disableForm(false);
                        EVthread = new Thread(delegate ()
                        {
                            runEVTestMode(true);
                        });
                        EVthread.Name = "EVthread";
                        EVthread.Start();
                    }
                    else if(lbFirmwareFileEVprograms.SelectedItem.ToString().Equals("14.5 on, 6 off, 3.5 on 10 cycle test mode"))
                    {
                        bStart.Text = "Stop";
                        activeTest = true;
                        disableForm(false);
                        EVthread = new Thread(delegate ()
                        {
                            runEVTestMode(true);
                        });
                        EVthread.Name = "EVthread";
                        EVthread.Start();
                    }
                    
                }
               
                else if (ProgramMode.Equals("Programming mode"))
                {
                    //myCommands.checkifComPortExistsforProgramming(myCommands.Devices[0].dutNumber, myCommands.Devices[0].comport);// reinitialize dut with new baud rate
                    List<int> readyDevices = readyToProgram(); // making sure the available devices have properly made it into bootloader mode.
                    if (readyDevices.Count == 0)
                        MessageBox.Show("You must cycle power to the device in order to program new firmware", "Important Message");
                    else
                    {
                        bStart.Text = "Stop";
                        activeTest = true;
                        disableForm(true);

                        foreach (int dut in readyDevices)
                        {
                            Thread programDevice = new Thread(delegate ()
                            {
                                myCommands.UploadApplication(myCommands.Devices[dut].m_si, myCommands.Devices[dut].target, Firmware, dut);
                                myCommands.Devices[dut].bootloaderMode = false;
                                myCommands.Devices[dut].bootloaderModeReady = false;
                                myCommands.Devices[dut].readesn = true;
                                myCommands.Devices[dut].setGUIESN = true;
                            });
                            programDevice.Start();
                        }


                    }
                }
            }
            else
            {

                if (ProgramMode.Equals("Validation test mode"))
                {
                    //bStart.Text = "Start";
                    //activeTest = false;
                    //enableForm();
                    stopEVTestMode();
                }
                else if (ProgramMode.Equals("Programming mode"))
                {
                    MessageBox.Show("You should not stop a device in the middle of being programmed",
                    "Caution");
                }
            }
        }
        public void returnfromValidationMode()
        {
            Action temp3 = () =>
            {
                bStart.Text = "Start";
                activeTest = false;
                enableForm();
            };
            getpMainFrame(temp3);
           
        }
        private void lbFirmwareFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkconfiguration();
        }
        private void lbFavCommands_SelectedIndexChanged(object sender, EventArgs e)
        {
            Object selecteditem = lbFavCommands.SelectedItem;
            for (int i = 0; i < 6; i++)
                if (myCommands.Devices[i].ready) myCommands.runCommands(selecteditem.ToString(), i);
        }


        private void rtbConsole_TextChanged(object sender, EventArgs e)
        {
            //// set the current caret position to the end
            //if (rtbLength != rtbConsole.Text.Length)
            //{
            //    rtbLength = rtbConsole.Text.Length;
            //    rtbConsole.SelectionStart = rtbLength;
            //    // scroll it automatically
            //    rtbConsole.ScrollToCaret();
            //}
        }

        private void pMainFrame_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tbConsoleCommands_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (activeCommand) cancelCommand = true;
                tbConsoleCommands.Clear();
                e.Handled = true;
                e.SuppressKeyPress = true;

            }
            else if (e.KeyCode == Keys.Enter) // when user hits enter the text in the text box gets set to a string and used by "waitForEntry()"
            {
                textReadyString = tbConsoleCommands.Text;
                textReady = true;
                tbConsoleCommands.Clear();
                e.Handled = true;
                e.SuppressKeyPress = true;

            }

        }
    }
}
