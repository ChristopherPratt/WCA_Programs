using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;


namespace WcaDVConsole
{
    public partial class MainFrame : Form
    {
        Control[] myControls;
        Thread EVthread;

        Program myProgram;
        Commands myCommands;
        EVtestMode ev;
        LEDframe ledframe;
        delegate void dgetpMainFrame(Action job);
        delegate void dWriteToConsole(string myString);
        delegate void dEnableConsole(bool enable);
        delegate void dSetEsn(int dutNum, string esn, Color myColor);
        delegate void dgetLBfirmwareEVmode(ref string temp);
        List<string> favCommands , activeDUTControl;
        bool testCurrentlyRunning = false, textReady = false, firmwareSelected = false, cancelCommand = false, activeCommand = false, EVmode = false,  enableStart = false,  activeTest = false;
        public bool programmingEnabled = false;
        public string textReadyString, evfirmwareprogramtestmode, ProgramMode = "", Firmware = "";

        List<Control> lprogramMode,lDUTnum, lFirmware, lvalidation, lvalidationStart, lCommands,lfavCommands, lConsole, lEOLmode, ldut1,  ldut2,  ldut3, ldut4, ldut5, ldut6, activeControls, lRadioButtons;
        List<List<Control>> lduts, ActiveDUTs;

        List<string> ProgramModeList = new List<string> {
            "End of line test mode",
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
            "Read power(mW)",
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
        public MainFrame(Program p, LEDframeforCustomers l)
        {
            myCommands = new Commands(this, l);
            myProgram = p;
            favCommands = new List<string>();
            activeControls = new List<Control>();
            activeDUTControl = new List<string>();
            ActiveDUTs = new List<List<Control>>();
            InitializeComponent();
            populateControlLists();
            populateElements();
            setStartupControls();           
        }
        private void populateControlLists()
        {
            lprogramMode = new List<Control>() {lblprogramMode,lbProgramMode, Laird };
            lDUTnum = new List<Control>() { lblDUTS, lbDUTnumber };
            lFirmware = new List<Control>() { lblFirmwareEVmode, lbFirmwareFileEVprograms,bBrowseFirmwareFiles };
            lvalidation = new List<Control>() { lblFirmwareEVmode, lbFirmwareFileEVprograms };
            lvalidationStart = new List<Control>() { bStart, rtbConsole };
            lCommands = new List<Control>() { lblcommands, lbCommands };
            lfavCommands = new List<Control>() { lblfavcommands, lbFavCommands };
            lConsole = new List<Control>() { rtbConsole, tbConsoleCommands };
            lEOLmode = new List<Control>() { rtbConsole, tbConsoleCommands, lblfavcommands, lbFavCommands, lblcommands, lbCommands };
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
            populateAllCOMportlbs();
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
        private void populateAllCOMportlbs() //populates all comm ports in the lbCommports for each DUT
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports) lbComPorts1.Items.Add(port);
            foreach (string port in ports) lbComPorts2.Items.Add(port);
            foreach (string port in ports) lbComPorts3.Items.Add(port);
            foreach (string port in ports) lbComPorts4.Items.Add(port);
            foreach (string port in ports) lbComPorts5.Items.Add(port);
            foreach (string port in ports) lbComPorts6.Items.Add(port);
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
        private void disableForm()
        {
            List<Control> tempControl = new List<Control>();
            foreach (Control temp in pMainFrame.Controls) temp.Enabled = false;
            bStart.Enabled = true;
            rtbConsole.Enabled = true;
            Laird.Enabled = true;
        }
        public void enableForm()
        {
            foreach (Control temp in activeControls) temp.Enabled = true;
            DUTEnable(activeDUTControl);
        }
        private void changeToFirmwareMode()
        {

        }
        public void run(string p) // allows user to select a mode for the program. changes gui and warns user if test is already running.
        {
            if (!p.Equals(""))ProgramMode = p;

            List<Control> tempControlList = new List<Control>();
            switch (ProgramMode)
            {

                case "End of line test mode":
                    {
                        foreach (Control temp in lDUTnum) tempControlList.Add(temp);
                        foreach (Control temp in lprogramMode) tempControlList.Add(temp);
                        if (myCommands.Devices.Count > 0) foreach(Control temp in lEOLmode) tempControlList.Add(temp);
                        lbDUTnumber.SelectionMode = SelectionMode.One; // single selections are necessary for this mode since we only want to program one DUT at a time  
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
                        if (myCommands.Devices.Count > 0 && lbFirmwareFileEVprograms.SelectedItems.Count > 0) tempControlList.Add(bStart);
                        lbDUTnumber.SelectionMode = SelectionMode.MultiSimple; // since we use multiple DUTs, lets allow multiple selections
                        updateGUI(tempControlList);
                        updateDUTS(getSelectedDuts());
                        foreach (string temp in EVModeList) lbFirmwareFileEVprograms.Items.Add(temp); // populate list box of current validation modes

                        break;
                    }
                case "Programming mode":
                    {
                        foreach (Control temp in lprogramMode) tempControlList.Add(temp); // enable program mode section
                        foreach (Control temp in lDUTnum) tempControlList.Add(temp);// enable DUT number section  
                        foreach (Control temp in lFirmware) tempControlList.Add(temp); // enable firmware section
                        lbFirmwareFileEVprograms.Items.Clear();
                        if (myCommands.Devices.Count > 0 && lbFirmwareFileEVprograms.SelectedItems.Count > 0)
                        {
                            tempControlList.Add(bStart);
                        }
                        lbDUTnumber.SelectionMode = SelectionMode.One; // single selections are necessary for this mode since we only want to program one DUT at a time  
                        updateGUI(tempControlList);
                        updateDUTS(getSelectedDuts());

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
                    setDisabled(new List<Control> {temp});
                    deleteControls.Add(temp);
                }
            }
            foreach (Control temp in deleteControls)if (activeControls.Contains(temp)) activeControls.Remove(temp);
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
        public void updateDUTS(List<string> newDuts)
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
        private void handlingProgrammingMode()// disable radio buttons because programming mode requires 115200
        {
            if (ProgramMode.Equals("Programming mode"))
            {
                foreach (List<Control> templist in ActiveDUTs)
                {
                    ((Panel)templist[5]).Enabled = false;                        
                    ((RadioButton)templist[6]).Checked = false;
                    ((RadioButton)templist[7]).Checked = true; // set the radio button ( though it is diabled) to 115200 as that is the baud rate for programming
                }
            }
            else // re-enable radio buttons if not in programming mode
            {
                foreach (List<Control> templist in ActiveDUTs)
                {
                    ((Panel)templist[5]).Enabled = true;                    
                }
                if (myCommands.Devices.Count > 0)
                if (myCommands.Devices[0].programmingMode) myCommands.InitializeDUT(myCommands.Devices[0].dutNumber, myCommands.Devices[0].comport, ProgramMode, myCommands.Devices[0].baud);
            }

        }

        private void setEnabled(List<Control> controls)  //enables different controls in gui
        {
            foreach (Control temp in controls)
            {
                temp.Enabled = true;
                activeControls.Add(temp);
            }               
        }
        private void DUTDisable(string dut)
        {
            switch (dut)
            {
                case "DUT 1": { foreach (Control temp in ldut1) temp.Enabled = false; ActiveDUTs.Remove(ldut1); break; }
                case "DUT 2": { foreach (Control temp in ldut2) temp.Enabled = false; ActiveDUTs.Remove(ldut2); break; }
                case "DUT 3": { foreach (Control temp in ldut3) temp.Enabled = false; ActiveDUTs.Remove(ldut3); break; }
                case "DUT 4": { foreach (Control temp in ldut4) temp.Enabled = false; ActiveDUTs.Remove(ldut4); break; }
                case "DUT 5": { foreach (Control temp in ldut5) temp.Enabled = false; ActiveDUTs.Remove(ldut5); break; }
                case "DUT 6": { foreach (Control temp in ldut6) temp.Enabled = false; ActiveDUTs.Remove(ldut6); break; }
            }
        }
        public void setDisabled(List<Control> controls)  //disables different controls in gui
        {
            foreach (Control temp in controls)
            {
                temp.Enabled = false;                
                
            }
        }
        private void ControlReset() // reset all controls so that nothing is selected or colors are set.
        {
            if (lbCommands.Enabled == false) lbCommands.SelectedItems.Clear();
            if (lbFavCommands.Enabled == false) lbFavCommands.SelectedItems.Clear();
            if (lbFirmwareFileEVprograms.Enabled == false) lbFirmwareFileEVprograms.SelectedItems.Clear();
            if (lblDUT1status.Enabled == false) {setESN("DUT 1", "Not Connected", SystemColors.Control); lbComPorts1.SelectedItems.Clear(); myCommands.removeDUT("DUT 1"); }
            if (lblDUT2status.Enabled == false) {setESN("DUT 2", "Not Connected", SystemColors.Control); lbComPorts2.SelectedItems.Clear(); myCommands.removeDUT("DUT 2"); }
            if (lblDUT3status.Enabled == false) {setESN("DUT 3", "Not Connected", SystemColors.Control); lbComPorts3.SelectedItems.Clear(); myCommands.removeDUT("DUT 3"); }
            if (lblDUT4status.Enabled == false) {setESN("DUT 4", "Not Connected", SystemColors.Control); lbComPorts4.SelectedItems.Clear(); myCommands.removeDUT("DUT 4"); }
            if (lblDUT5status.Enabled == false) {setESN("DUT 5", "Not Connected", SystemColors.Control); lbComPorts5.SelectedItems.Clear(); myCommands.removeDUT("DUT 5"); }
            if (lblDUT6status.Enabled == false) {setESN("DUT 6", "Not Connected", SystemColors.Control); lbComPorts6.SelectedItems.Clear(); myCommands.removeDUT("DUT 6"); }
        }
        public void DUTReset(string myDUT) // change selected DUT back to a disconnected dut
        {
            switch (myDUT)
            {
                case "DUT 1": { lblESN1.Text = "Not Connected"; lblESN1.BackColor = SystemColors.Control; lblDUT1status.BackColor = SystemColors.Control; lbComPorts1.SelectedItems.Clear(); break; }
                case "DUT 2": { lblESN2.Text = "Not Connected"; lblESN2.BackColor = SystemColors.Control; lblDUT2status.BackColor = SystemColors.Control; lbComPorts2.SelectedItems.Clear(); break; }
                case "DUT 3": { lblESN3.Text = "Not Connected"; lblESN3.BackColor = SystemColors.Control; lblDUT3status.BackColor = SystemColors.Control; lbComPorts3.SelectedItems.Clear(); break; }
                case "DUT 4": { lblESN4.Text = "Not Connected"; lblESN4.BackColor = SystemColors.Control; lblDUT4status.BackColor = SystemColors.Control; lbComPorts4.SelectedItems.Clear(); break; }
                case "DUT 5": { lblESN5.Text = "Not Connected"; lblESN5.BackColor = SystemColors.Control; lblDUT5status.BackColor = SystemColors.Control; lbComPorts5.SelectedItems.Clear(); break; }
                case "DUT 6": { lblESN6.Text = "Not Connected"; lblESN6.BackColor = SystemColors.Control; lblDUT6status.BackColor = SystemColors.Control; lbComPorts6.SelectedItems.Clear(); break; }
            }
            myCommands.removeDUT(myDUT); // if the DUt that is being removed has an active device object and serial connection then it closes that too.
        }
        public void DUTEnable(List<string> tempduts) // enables or disables different controls in gui based on selection of DUTS
        {            
            List<Control> myDuts = new List<Control>();

            foreach (string dut in tempduts) // add certain DUT control panels for each DUT which was selected in lbDUTNumber
            {
                switch (dut)
                {
                    case "DUT 1": { foreach (Control temp in ldut1) { myDuts.Add(temp); } ActiveDUTs.Add(ldut1); break; }
                    case "DUT 2": { foreach (Control temp in ldut2) { myDuts.Add(temp); } ActiveDUTs.Add(ldut2); break; }
                    case "DUT 3": { foreach (Control temp in ldut3) { myDuts.Add(temp); } ActiveDUTs.Add(ldut3); break; }
                    case "DUT 4": { foreach (Control temp in ldut4) { myDuts.Add(temp); } ActiveDUTs.Add(ldut4); break; }
                    case "DUT 5": { foreach (Control temp in ldut5) { myDuts.Add(temp); } ActiveDUTs.Add(ldut5); break; }
                    case "DUT 6": { foreach (Control temp in ldut6) { myDuts.Add(temp); } ActiveDUTs.Add(ldut6); break; }
                }
                
            }
            foreach (Control temp in myDuts) temp.Enabled = true;
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
                    rtbConsole.AppendText(myString);
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

        public void setESN(string dutNum, string esn, Color myColor) // writes the esn and changes color of the dut box confirming the device is a wireless charger
        {
            Action temp2 = () =>
            {
                switch (dutNum)
                {
                    case "DUT 1": { lblESN1.Text = esn; lblESN1.BackColor = myColor; lblDUT1status.BackColor = myColor; break; }
                    case "DUT 2": { lblESN2.Text = esn; lblESN2.BackColor = myColor; lblDUT2status.BackColor = myColor; break; }
                    case "DUT 3": { lblESN3.Text = esn; lblESN3.BackColor = myColor; lblDUT3status.BackColor = myColor; break; }
                    case "DUT 4": { lblESN4.Text = esn; lblESN4.BackColor = myColor; lblDUT4status.BackColor = myColor; break; }
                    case "DUT 5": { lblESN5.Text = esn; lblESN5.BackColor = myColor; lblDUT5status.BackColor = myColor; break; }
                    case "DUT 6": { lblESN6.Text = esn; lblESN6.BackColor = myColor; lblDUT6status.BackColor = myColor; break; }
                }

                if (!esn.Equals("COM Port Unavailable") && !esn.Equals("Not Connected")) // if device is a wireless charger that speaks correctly by read esn command with a neg ack or a pos ack
                {
                    checkconfiguration();
                }

            };
            getpMainFrame(temp2);    
        }
        private void checkconfiguration()
        {
            if (myCommands.Devices.Count > 0) // double checking that we have at least 1 device connected
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
                    case "Programming mode":
                        {
                            if (!bStart.Enabled && lbFirmwareFileEVprograms.SelectedItems.Count > 0) setEnabled(lvalidationStart); // enable all controls for programming (same as validation mode)
                            break;
                        }

                }
            }
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
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Assign the cursor in the Stream to the Form's Cursor property.  
                Firmware = openFileDialog1.FileName;
            }
            lbFirmwareFileEVprograms.Items.Add(Firmware); // add filename to listbox
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
                lblProgressBar.Text = ((int)(((float)value / (float)max)*100)).ToString() + "%";
            };
            getpMainFrame(temp3);
        }
        public void resetProgressBar ()
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

        private void runEVTestMode()
        {
            string mode = "";
            getLBfirmwareEVmode(ref mode);
            switch (evfirmwareprogramtestmode)
            {
                case "Continuous test mode":
                    {
                        continuousTestMode(true);
                        break;
                    }
                case "14.5 on, 6 off, 3.5 on 10 cycle test mode":
                    {
                        EV14_5on6off3_5on10cycleYestMode(true);
                        break;
                    }
            }
        }

        private void dgeqwrg(object sender, KeyEventArgs e)
        {

        }

        private void continuousTestMode(bool start)
        {
            if (start)
            {
                ev = new EVtestMode();
                ev.Run(myCommands.Devices, myCommands);
            }
            else
            {
                ev.stop();
            }
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

        private void rb96001_CheckedChanged(object sender, EventArgs e)
        {
            if (lbComPorts1.SelectedItem == null) return;
            myCommands.InitializeDUT("DUT 1", lbComPorts1.SelectedItem.ToString(), ProgramMode, getBaud(rb96001));
        }

        private void rb96002_CheckedChanged(object sender, EventArgs e)
        {
            if (lbComPorts2.SelectedItem == null) return;
            myCommands.InitializeDUT("DUT 2", lbComPorts2.SelectedItem.ToString(), ProgramMode, getBaud(rb96002));
        }

        private void rb96003_CheckedChanged(object sender, EventArgs e)
        {
            if (lbComPorts3.SelectedItem == null) return;
            myCommands.InitializeDUT("DUT 3", lbComPorts3.SelectedItem.ToString(), ProgramMode, getBaud(rb96003));
        }

        private void rb96004_CheckedChanged(object sender, EventArgs e)
        {
            if (lbComPorts4.SelectedItem == null) return;
            myCommands.InitializeDUT("DUT 4", lbComPorts4.SelectedItem.ToString(), ProgramMode, getBaud(rb96004));
        }

        private void tbConsoleCommands_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void rb1152001_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rb96005_CheckedChanged(object sender, EventArgs e)
        {
            if (lbComPorts5.SelectedItem == null) return;
            myCommands.InitializeDUT("DUT 5", lbComPorts5.SelectedItem.ToString(), ProgramMode, getBaud(rb96005));
        }

        private void rb96006_CheckedChanged(object sender, EventArgs e)
        {
            if (lbComPorts6.SelectedItem == null) return;
            myCommands.InitializeDUT("DUT 6", lbComPorts6.SelectedItem.ToString(), ProgramMode, getBaud(rb96006));
        }

        public int getBaud(RadioButton myControl)
        {
            if (myControl.Checked) return 9600;
            else return 115200;
        }
        private void EV14_5on6off3_5on10cycleYestMode(bool start)
        {

        }
        private void MainFrame_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void lbProgramMode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        /*selected
         * dut1
         * dut2
         * 
         * currently active
         * dut2
         * dut3
         * 
         * does selected = any active? no: add dut | yes: do nothing
         * does active = any selected? no: delete | yes: do nothing
         * */
        

        private void lbDUTnumber_SelectedIndexChanged(object sender, EventArgs e)
        {            
            updateDUTS(getSelectedDuts());
            if (myCommands.Devices.Count == 0 && lbCommands.Enabled) setDisabled(lEOLmode); // disable all device controls if no valid duts are selected            
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
            if(lbCommands.SelectedItem.ToString().Equals("Set LED color")) // open new LEDframe if the set led color command was selected and there currently isn't a LEDframe open
            {
                if (ledframe == null)
                {
                    ledframe = new LEDframe(myCommands);
                    ledframe.Show();
                    return;
                }
                else if (ledframe.IsDisposed)
                {
                    ledframe = null;
                    ledframe = new LEDframe(myCommands);
                    ledframe.Show();
                    return;
                }
                return;          // do nothing if form is already opened      
            }
            if (activeCommand) { cancelCommand = true; Thread.Sleep(25); } // to prevent multiple commands being used at the same time. this can be confusing to the user
            Object selecteditem = lbCommands.SelectedItem;
            for (int i = 0; i < myCommands.Devices.Count; i++)
            { myCommands.runCommands(selecteditem.ToString(), i);}
            addCommandToFav(selecteditem.ToString());
            tbConsoleCommands.Focus();
            tbConsoleCommands.Clear();
        }

        private void tbConsoleCommands_TextChanged(object sender, EventArgs e)
        {
        }

        private void lbComPorts1_SelectedIndexChanged(object sender, EventArgs e) // initialze DUT subclass for holding all relevant info
        {
            if (lbComPorts1.SelectedItem == null) return;
            myCommands.InitializeDUT("DUT 1", lbComPorts1.SelectedItem.ToString(),ProgramMode, getBaud(rb96001));
            if (myCommands.Devices.Count == 0 && lbCommands.Enabled) setDisabled(lEOLmode);
        }
        private void lbComPorts2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbComPorts2.SelectedItem == null) return;
            myCommands.InitializeDUT("DUT 2", lbComPorts2.SelectedItem.ToString(), ProgramMode, getBaud(rb96002));
            if (myCommands.Devices.Count == 0 && lbCommands.Enabled) setDisabled(lEOLmode);
        }
        private void lbComPorts3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbComPorts3.SelectedItem == null) return;
            myCommands.InitializeDUT("DUT 3", lbComPorts3.SelectedItem.ToString(), ProgramMode, getBaud(rb96003));
            if (myCommands.Devices.Count == 0 && lbCommands.Enabled) setDisabled(lEOLmode);
        }
        private void lbComPorts4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbComPorts4.SelectedItem == null) return;
            myCommands.InitializeDUT("DUT 4", lbComPorts4.SelectedItem.ToString(),ProgramMode, getBaud(rb96004));
            if (myCommands.Devices.Count == 0 && lbCommands.Enabled) setDisabled(lEOLmode);
        }
        private void lbComPorts5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbComPorts5.SelectedItem == null) return;
            myCommands.InitializeDUT("DUT 5", lbComPorts5.SelectedItem.ToString(), ProgramMode, getBaud(rb96005));
            if (myCommands.Devices.Count == 0 && lbCommands.Enabled) setDisabled(lEOLmode);
        }
        private void lbComPorts6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbComPorts6.SelectedItem == null) return;
            myCommands.InitializeDUT("DUT 6", lbComPorts6.SelectedItem.ToString(), ProgramMode, getBaud(rb96006));
            if (myCommands.Devices.Count == 0 && lbCommands.Enabled) setDisabled(lEOLmode);
        }
        public void resetGUI ()
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
                    bStart.Text = "Stop";
                    activeTest = true;
                    disableForm();
                    EVthread = new Thread(delegate ()
                    {
                        runEVTestMode();
                    });
                    EVthread.Start();
                }       
                else if(ProgramMode.Equals("Programming mode"))
                {
                    //myCommands.InitializeDUTforProgramming(myCommands.Devices[0].dutNumber, myCommands.Devices[0].comport);// reinitialize dut with new baud rate
                    if (!myCommands.Devices[0].bootloaderMode)
                        MessageBox.Show("You must cycle power to the device in order to program new firmware", "Important Message");
                    else
                    {
                        bStart.Text = "Stop";
                        activeTest = true;
                        disableForm();

                        Thread programDevice = new Thread(delegate ()
                        {
                            myCommands.UploadApplication(myCommands.Devices[0].m_si, myCommands.Devices[0].target, Firmware);
                        });
                        programDevice.Start();
                        
                    }
                }
            }
            else
            {
               
                if (ProgramMode.Equals("Validation test mode"))
                {
                    bStart.Text = "Start";
                    activeTest = false;
                    enableForm();
                    stopEVTestMode();
                }
                else if (ProgramMode.Equals("Programming mode"))
                {
                    MessageBox.Show("You should not stop a device in the middle of being programmed",
                    "Caution");
                }
            }
        }

        private void lbFirmwareFile_SelectedIndexChanged(object sender, EventArgs e)
        {
                checkconfiguration();
        }
        private void lbFavCommands_SelectedIndexChanged(object sender, EventArgs e)
        {
            Object selecteditem = lbFavCommands.SelectedItem;
            for (int i = 0; i < myCommands.Devices.Count; i++)
            { myCommands.runCommands(selecteditem.ToString(), i); }
        }

        private void rtbConsole_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            rtbConsole.SelectionStart = rtbConsole.Text.Length;
            // scroll it automatically
            rtbConsole.ScrollToCaret();
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
