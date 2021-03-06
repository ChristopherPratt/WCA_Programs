﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.IO.Ports;
using System.Timers;
using System.Xml;





namespace WcaDVConsole
{
    public partial class LEDframeforCustomers : Form
    {
        bool defaultValuesDisabled = false;
        public string Version;
        private System.Timers.Timer readComportsTimer, readColorTimer;
        delegate void dgetLEDforCustomers(Action job);
        HelpFrame helpframe;
        Thread color, checkComports;
        Commands myCommands;
        byte[] redArray;
        byte[] greenArray;
        byte[] blueArray;
        List<byte[]> ColorArray;
        List<byte[]> cyanRed, cyanOrange, cyanBlue, cyanGreen, cyanWhite, cyanOff, yellowRed, yellowOrange, yellow, yellowGreen, yellowWhite, yellowOff;
        List<List<byte[]>> cyanColors;
        List<List<byte[]>> yellowColors;
        List<List<byte[]>> userColors;
        List<string> userColorNames;

        XmlDocument xmlRead;
        XmlDocument xmlUser;
        XmlNodeList nodeList;

        double previousIntensity = 50;
        int redTemp, greenTemp, blueTemp;
        double redStep, greenStep, blueStep;
        bool redLock = false, greenLock = false, blueLock = false, guiSet = false, setLED = true, disableUpdate = false;
        double step = 1;
        string[] availableComports;

        int tbarRedValue = 0;
        int tbarGreenValue = 0;
        int tbarBlueValue = 0;



        List<Control> ColorsTrackBars, radioButtonsCyan, radioButtonsYellow, dataTypes, initialDisable, IntensityTrackBar, UserColors;

        public LEDframeforCustomers(Program p)
        {
            Version = "IGNORE ESN YELLOW";
            myCommands = new Commands(new MainFrame(p, this), this);
            InitializeComponent();
            redArray = new byte[4];
            greenArray = new byte[4];
            blueArray = new byte[4];
            tbarIntensity.Value = 50;
            queryComPorts();
            initiateLists();
            setDisabled(initialDisable);
            initiateColorArrays();
            populateUserColorListbox();
            if (Version == "IGNORE ESN YELLOW") cbEnableSliders.Visible = false;
            tbarIntensity.MouseWheel += new MouseEventHandler(DoNothing_MouseWheel);
            tbarRed.MouseWheel += new MouseEventHandler(DoNothing_MouseWheel);
            tbarGreen.MouseWheel += new MouseEventHandler(DoNothing_MouseWheel);
            tbarBlue.MouseWheel += new MouseEventHandler(DoNothing_MouseWheel);
        }
        private void DoNothing_MouseWheel(object sender, EventArgs e)
        {
            HandledMouseEventArgs ee = (HandledMouseEventArgs)e;
            ee.Handled = true;
        }
        private void initiateColorArrays()
        {
            

            xmlRead = new XmlDocument();
            xmlUser = new XmlDocument();
            try
            { // get all color values from the .xml file
                xmlRead.Load(@"defaultValues.xml");
                if (Version == "IGNORE ESN CYAN")
                {
                    cyanRed = getColor(xmlRead.SelectNodes("LEDCOLORS")[0].SelectNodes("DEFAULTVALUES")[0].SelectNodes("CYAN")[0].SelectSingleNode("RED"));
                    cyanOrange = getColor(xmlRead.SelectNodes("LEDCOLORS")[0].SelectNodes("DEFAULTVALUES")[0].SelectNodes("CYAN")[0].SelectSingleNode("ORANGE"));
                    cyanBlue = getColor(xmlRead.SelectNodes("LEDCOLORS")[0].SelectNodes("DEFAULTVALUES")[0].SelectNodes("CYAN")[0].SelectSingleNode("BLUE"));
                    cyanGreen = getColor(xmlRead.SelectNodes("LEDCOLORS")[0].SelectNodes("DEFAULTVALUES")[0].SelectNodes("CYAN")[0].SelectSingleNode("GREEN"));
                    cyanWhite = getColor(xmlRead.SelectNodes("LEDCOLORS")[0].SelectNodes("DEFAULTVALUES")[0].SelectNodes("CYAN")[0].SelectSingleNode("WHITE"));
                    cyanOff = getColor(xmlRead.SelectNodes("LEDCOLORS")[0].SelectNodes("DEFAULTVALUES")[0].SelectNodes("CYAN")[0].SelectSingleNode("OFF"));
                }
                else if (Version == "IGNORE ESN YELLOW")
                {
                    yellowRed = getColor(xmlRead.SelectNodes("LEDCOLORS")[0].SelectNodes("DEFAULTVALUES")[0].SelectNodes("YELLOW")[0].SelectSingleNode("RED"));
                    yellowOrange = getColor(xmlRead.SelectNodes("LEDCOLORS")[0].SelectNodes("DEFAULTVALUES")[0].SelectNodes("YELLOW")[0].SelectSingleNode("ORANGE"));
                    yellowGreen = getColor(xmlRead.SelectNodes("LEDCOLORS")[0].SelectNodes("DEFAULTVALUES")[0].SelectNodes("YELLOW")[0].SelectSingleNode("GREEN"));
                    yellowWhite = getColor(xmlRead.SelectNodes("LEDCOLORS")[0].SelectNodes("DEFAULTVALUES")[0].SelectNodes("YELLOW")[0].SelectSingleNode("WHITE"));
                    yellowOff = getColor(xmlRead.SelectNodes("LEDCOLORS")[0].SelectNodes("DEFAULTVALUES")[0].SelectNodes("YELLOW")[0].SelectSingleNode("OFF"));
                }
            }
            catch (Exception e)
            {
                defaultValuesDisabled = true;
            }
            bool userValuesAvailable = true;
            try
            {
                xmlUser.Load(@"userValues.xml");
                if (xmlUser.SelectNodes("LEDCOLORS")[0].SelectNodes("USERVALUES")[0].HasChildNodes)
                {
                    foreach (XmlNode myNode in xmlUser.SelectNodes("LEDCOLORS")[0].SelectNodes("USERVALUES")[0].SelectNodes("COLOR"))
                    {
                        userColors.Add(getColor(myNode));
                        userColorNames.Add(myNode.Attributes.Item(0).Value);
                    }
                }
            }
            catch (Exception e)
            {
                userValuesAvailable = false;
                XmlDeclaration xmlDeclaration = xmlUser.CreateXmlDeclaration("1.0", "UTF-8", null);

                xmlUser.AppendChild(xmlUser.CreateElement("LEDCOLORS"));
                xmlUser.SelectNodes("LEDCOLORS")[0].AppendChild(xmlUser.CreateElement("USERVALUES"));
                xmlUser.Save("userValues.xml");
                
            }
            if (!userValuesAvailable && defaultValuesDisabled)
                MessageBox.Show("both userValues.xml and defaultValues.xml files are incorrect or unavailable. Default values will be disabled and user values will be empty. However, you will still be able to save new user values.");
            else if (!userValuesAvailable)
                MessageBox.Show("userValues.xml files is incorrect or unavailable. User values will be empty. However, you will still be able to save new user values.");
            else if (defaultValuesDisabled)
                MessageBox.Show("defaultValues.xml is incorrect or unavailable. Default values will be disabled.");






            //cyanRed = new List<byte[]> { new byte[] { 0xFF, 0xFF, 0, 0 }, new byte[] { 0, 0, 0, 0 }, new byte[] { 0, 0, 0, 0 } };
            //cyanOrange = new List<byte[]> { new byte[] { 0xFF, 0xFF, 0, 0 }, new byte[] { 0x00, 0x06, 0, 0 }, new byte[] { 0, 0, 0, 0 } };
            //cyanBlue = new List<byte[]> { new byte[] { 0, 0, 0, 0 }, new byte[] { 0xFF, 0x01, 0, 0 }, new byte[] { 0xFB, 0x09, 0, 0 } };
            //cyanGreen = new List<byte[]> { new byte[] { 0, 0, 0, 0 }, new byte[] { 0xFF, 0x03, 0, 0 }, new byte[] { 0, 0, 0, 0 } };
            //cyanWhite = new List<byte[]> { new byte[] { 0xAF, 0xAF, 0, 0 }, new byte[] { 0x1C, 0x1C, 0, 0 }, new byte[] { 0x3E, 0x3E, 0, 0 } };
            //cyanOff = new List<byte[]> { new byte[] { 0, 0, 0, 0 }, new byte[] { 0, 0, 0, 0 }, new byte[] { 0, 0, 0, 0 } };
            //yellowRed = new List<byte[]> { new byte[] { 0xFF, 0xFF, 0, 0 }, new byte[] { 0, 0, 0, 0 }, new byte[] { 0, 0, 0, 0 } };
            //yellowOrange = new List<byte[]> { new byte[] { 0x1D, 0xC5, 0, 0 }, new byte[] { 0xD8, 0x1A, 0, 0 }, new byte[] { 0x00, 0x00, 0, 0 } };
            //yellowGreen = new List<byte[]> { new byte[] { 0, 0, 0, 0 }, new byte[] { 0xFF, 0x3F, 0, 0 }, new byte[] { 0xFF, 0x01, 0, 0 } };
            //yellowWhite = new List<byte[]> { new byte[] { 0x32, 0xB3, 0, 0 }, new byte[] { 0x7D, 0x8B, 0, 0 }, new byte[] { 0xB1, 0x3E, 0, 0 } };
            //yellowOff = new List<byte[]> { new byte[] { 0, 0, 0, 0 }, new byte[] { 0, 0, 0, 0 }, new byte[] { 0, 0, 0, 0 } };

            cyanColors = new List<List<byte[]>> { cyanRed, cyanOrange, cyanBlue, cyanGreen, cyanWhite, cyanOff };
            yellowColors = new List<List<byte[]>> { yellowRed, yellowOrange, yellowGreen, yellowWhite, yellowOff };
            ColorArray = new List<byte[]> { redArray, greenArray, blueArray };

        }


        // takes individual colors from the RGB and returns the List of byte[] for the color set.
        private List<byte[]> getColor(XmlNode myNode)
        {
            byte[] myRed = getRGB(myNode.SelectSingleNode("R").InnerText.Split(','));
            byte[] myGreen = getRGB(myNode.SelectSingleNode("G").InnerText.ToString().Split(','));
            byte[] myBlue = getRGB(myNode.SelectSingleNode("B").InnerText.ToString().Split(','));
            return new List<byte[]> { myRed, myGreen, myBlue };
        }
        private byte[] getRGB(string[] myColor)
        {
            byte temp0 = Convert.ToByte(myColor[1], 16);//convert string to byte
            byte temp1 = Convert.ToByte(myColor[0], 16);
            return new byte[] { temp0, temp1, 0x00, 0x00 }; // return byte array with extra 2 empty bytes.
        }

        private void populateUserColorListbox()
        {
            foreach (string colorName in userColorNames) lbUserColors.Items.Add(colorName);
        }

        private void populateComports()
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports) lbComPorts.Items.Add(port);
        }

        private void initiateLists()
        {
            userColors = new List<List<byte[]>>();
            userColorNames = new List<string>();
            ColorsTrackBars = new List<Control>() { lblred, tbarRed, tbRed, lblGreen, tbarGreen, tbGreen, lblBlue, tbarBlue, tbBlue, lblUserColors, lbUserColors, tbUserColorsName, bSaveDelete };
            radioButtonsCyan = new List<Control>() { rbCyanRed, rbCyanOrange, rbCyanBlue, rbCyanGreen, rbCyanWhite, rbCyanOff };
            radioButtonsYellow = new List<Control>() { rbYellowRed, rbYellowOrange, rbYellowGreen, rbYellowWhite, rbYellowOff };
            dataTypes = new List<Control>() { rbHex, rbPercentage, gbDefault };
            initialDisable = new List<Control>() { rbHex, rbPercentage, cbEnableSliders, lblred, tbarRed, tbRed, lblGreen, tbarGreen, tbGreen, lblBlue, tbarBlue, tbBlue, lblIntensity, tbarIntensity, tbIntensity, gbDefault, lblUserColors, lbUserColors, tbUserColorsName, bSaveDelete };
            IntensityTrackBar = new List<Control> { lblIntensity, tbarIntensity, tbIntensity };
            UserColors = new List<Control> { lblUserColors, lbUserColors, tbUserColorsName, bSaveDelete };
        }
        private void setVisible(List<Control> list)
        {
            foreach (Control temp in list) temp.Visible = true;
        }
        private void setInvisible(List<Control> list)
        {
            foreach (Control temp in list) temp.Visible = false;
        }
        private void setEnabled(List<Control> controls)  //enables different controls in gui
        {
            foreach (Control temp in controls)
            {
                temp.Enabled = true;
            }
        }
        private void setDisabled(List<Control> controls)  //enables different controls in gui
        {
            foreach (Control temp in controls)
            {
                temp.Enabled = false;
            }
        }


        public void updateDeviceinfo(string info)
        {
            Action temp3 = () =>
            {
                lblESN.Text = info;

                string[] temp = info.Split(' '); //looking for "connected\nESN: " + myesn
                if (temp[0].Equals("Connected")) // found it                
                {
                    if ((Version.Equals("IGNORE ESN CYAN"))) // if the esn is " 097402" (Cyan)
                    {
                        if (!defaultValuesDisabled)
                        {
                            setVisible(radioButtonsCyan);
                            setInvisible(radioButtonsYellow);
                        }
                       
                        setEnabled(IntensityTrackBar);
                        setEnabled(dataTypes);
                        setDisabled(ColorsTrackBars);
                        setEnabled(new List<Control> { cbEnableSliders });
                        rbCyanWhite.Checked = true;
                    }
                    else if ((Version.Equals("IGNORE ESN YELLOW")))// if the esn is " 09710" (Yellow)
                    {
                        if (!defaultValuesDisabled)
                        {
                            setInvisible(radioButtonsCyan);
                            setVisible(radioButtonsYellow);
                        }
                        
                        setEnabled(ColorsTrackBars);
                        setEnabled(dataTypes);
                        setEnabled(IntensityTrackBar);
                        
                    }
                    else if ((temp[1][3] == '4' && Version.Equals("CYAN"))) // if the esn is " 097402" (Cyan)
                    {
                        if (!defaultValuesDisabled)
                        {
                            setVisible(radioButtonsCyan);
                            setInvisible(radioButtonsYellow);
                        }
                        
                        setEnabled(IntensityTrackBar);
                        setEnabled(dataTypes);
                        setDisabled(ColorsTrackBars);
                        setEnabled(new List<Control> { cbEnableSliders });
                        rbCyanWhite.Checked = true;
                    }
                    else if ((temp[1][3] == '1' && Version.Equals("YELLOW")))// if the esn is " 09710" (Yellow)
                    {
                        if (!defaultValuesDisabled)
                        {
                            setInvisible(radioButtonsCyan);
                            setVisible(radioButtonsYellow);
                        }
                        
                        setEnabled(ColorsTrackBars);
                        setEnabled(dataTypes);
                        setEnabled(IntensityTrackBar);
                    }
                    tbRed.MaxLength = 5;
                    tbGreen.MaxLength = 5;
                    tbBlue.MaxLength = 5;
                    readLEDs();
                }
            };
            getLEDforCustomers(temp3);
        }
        private void readLEDs()
        {
            readComportsTimer = new System.Timers.Timer(1000);
            // Hook up the Elapsed event for the timer. 
            readComportsTimer.Elapsed += readColorsEvent;
            readComportsTimer.AutoReset = true;
            readComportsTimer.Enabled = true;
        }

        private void readColorsEvent(Object source, ElapsedEventArgs e)
        {
            myCommands.runCommands("Read LED color", 0);
        }

        private void queryComPorts()
        {
            checkComports = new Thread(delegate ()
            {
                readComportsTimer = new System.Timers.Timer(1000);
                // Hook up the Elapsed event for the timer. 
                readComportsTimer.Elapsed += OnTimedEvent;
                readComportsTimer.AutoReset = true;
                readComportsTimer.Enabled = true;
            });
            checkComports.Start();

        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (availableComports == null)
            {
                availableComports = SerialPort.GetPortNames();
                Action temp1 = () => { foreach (string port in availableComports) lbComPorts.Items.Add(port); };
                getLEDforCustomers(temp1);
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
                Action temp2 = () => { foreach (string deletecom in deadComports) lbComPorts.Items.Remove(deletecom); };
                getLEDforCustomers(temp2);
            }

            if (newComports.Count > 0)
            {
                Action temp2 = () => { foreach (string newcom in newComports) lbComPorts.Items.Add(newcom); };
                getLEDforCustomers(temp2);
            }

            availableComports = tempPorts;


        }


        private void tbarRed_Scroll(object sender, EventArgs e)
        {
            uncheckDefaultColors();
            updateColorTextBox(tbarRed, tbRed, tbarRed.Value, ref redArray);
            if (tbarRed.Value == 0) lockTrackBar(tbarRed, ref redLock); // lock track bar if user sets it to 0 because of the way the intesity bar won't go below the 0 threshold
            else if (redLock) unlockTrackBar(tbarRed, ref redLock);
        }

        private void tbarGreen_Scroll(object sender, EventArgs e)
        {
            uncheckDefaultColors();
            updateColorTextBox(tbarGreen, tbGreen, tbarGreen.Value, ref greenArray);
            if (tbarGreen.Value == 0) lockTrackBar(tbarGreen, ref greenLock); // lock track bar if user sets it to 0 because of the way the intesity bar won't go below the 0 threshold
            else if (greenLock) unlockTrackBar(tbarGreen, ref greenLock);

        }

        private void tbarBlue_Scroll(object sender, EventArgs e)
        {
            uncheckDefaultColors();
            updateColorTextBox(tbarBlue, tbBlue, tbarBlue.Value, ref blueArray);
            if (tbarBlue.Value == 0) lockTrackBar(tbarBlue, ref blueLock); // lock track bar if user sets it to 0 because of the way the intesity bar won't go below the 0 threshold
            else if (blueLock) unlockTrackBar(tbarBlue, ref blueLock);
        }

        private void tbarIntensity_Scroll(object sender, EventArgs e)
        {
            resetColorlbl();
            double intensity = (Convert.ToDouble(tbarIntensity.Value)); // new intensity
            previousIntensity = intensity; // save new intensity as old intensity

            redTemp = (Int32)(intensity * redStep);
            greenTemp = (Int32)(intensity * greenStep);
            blueTemp = (Int32)(intensity * blueStep);
            if (redTemp > 65535 || redTemp < 0) if (!redLock) return;
            if (greenTemp > 65535 || greenTemp < 0) if (!greenLock) return;
            if (blueTemp > 65535 || blueTemp < 0) if (!blueLock) return;

            if (!redLock)
            {
                tbarRed.Value = redTemp;
                updateColorTextBox(tbarRed, tbRed, redTemp, ref redArray);
            }
            if (!greenLock)
            {
                tbarGreen.Value = greenTemp;
                updateColorTextBox(tbarGreen, tbGreen, greenTemp, ref greenArray);
            }
            if (!blueLock)
            {
                tbarBlue.Value = blueTemp;
                updateColorTextBox(tbarBlue, tbBlue, blueTemp, ref blueArray);
            }
            tbIntensity.Text = tbarIntensity.Value.ToString();
        }

        private void rbHex_CheckedChanged(object sender, EventArgs e)
        {
            if (rbHex.Checked)
            {
                //tbRed.Text = string.Format("{0:X}{1:X}", redArray[1], redArray[0]);
                //tbGreen.Text = string.Format("{0:X}{1:X}", greenArray[1], greenArray[0]);
                //tbBlue.Text = string.Format("{0:X}{1:X}", blueArray[1], blueArray[0]);

                tbRed.Text = string.Format("{0:X2}", redArray[1]) + string.Format("{0:X2}", redArray[0]);
                tbGreen.Text = string.Format("{0:X2}", greenArray[1]) + string.Format("{0:X2}", greenArray[0]);
                tbBlue.Text = string.Format("{0:X2}", blueArray[1]) + string.Format("{0:X2}", blueArray[0]);
                tbRed.MaxLength = 4;
                tbGreen.MaxLength = 4;
                tbBlue.MaxLength = 4;
            }
            else
            {

                tbRed.Text = String.Format("{0:0.##}", ((double)BitConverter.ToInt32(redArray, 0) * 100) / 65535);
                tbGreen.Text = String.Format("{0:0.##}", ((double)BitConverter.ToInt32(greenArray, 0) * 100) / 65535);
                tbBlue.Text = String.Format("{0:0.##}", ((double)BitConverter.ToInt32(blueArray, 0) * 100) / 65535);
                tbRed.MaxLength = 5;
                tbGreen.MaxLength = 5;
                tbBlue.MaxLength = 5;
            }
        }
        
        private void tbarRed_MouseUp(object sender, MouseEventArgs e)
        {
            disableUpdate = false;
            findAverageIntensity();
            color = new Thread(delegate ()
            {
                myCommands.SetLEDColor2(0, redArray, greenArray, blueArray);
            });
            color.Start();
        }

        private void tbarGreen_MouseUp(object sender, MouseEventArgs e)
        {
            disableUpdate = false;
            findAverageIntensity();
            color = new Thread(delegate ()
            {
                myCommands.SetLEDColor2(0, redArray, greenArray, blueArray);
            });
            color.Start();
        }

        private void tbarBlue_MouseUp(object sender, MouseEventArgs e)
        {
            disableUpdate = false;
            findAverageIntensity();
            color = new Thread(delegate ()
            {
                myCommands.SetLEDColor2(0, redArray, greenArray, blueArray);
            });
            color.Start();
        }
        private void tbarIntensity_MouseUp(object sender, MouseEventArgs e)
        {
            disableUpdate = false;
            color = new Thread(delegate ()
            {
                myCommands.SetLEDColor2(0, redArray, greenArray, blueArray);
            });
            color.Start();
        }

        private void tbRed_KeyDown(object sender, KeyEventArgs e)
        {
            lblRedtxt.Text = "Press Enter";
            if (e.KeyCode == Keys.Enter) // when user hits enter the text in the text box gets set to a string and used by "waitForEntry()"
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                textboxUpdateAll();
            }
        }
        private void tbGreen_KeyDown(object sender, KeyEventArgs e)
        {
            lblGreentxt.Text = "Press Enter";
            if (e.KeyCode == Keys.Enter) // when user hits enter the text in the text box gets set to a string and used by "waitForEntry()"
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                textboxUpdateAll();
            }
        }
        private void tbBlue_KeyDown(object sender, KeyEventArgs e)
        {
            lblBluetxt.Text = "Press Enter";
            if (e.KeyCode == Keys.Enter) // when user hits enter the text in the text box gets set to a string and used by "waitForEntry()"
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                textboxUpdateAll();
            }
               
        }
        private void tbIntensity_KeyDown(object sender, KeyEventArgs e)
        {
            intensityManualEnter(tbIntensity.Text, e);
        }

        private void tbarIntensity_MouseDown(object sender, MouseEventArgs e)
        {
            disableUpdate = true;
        }

        private void tbarRed_MouseDown(object sender, MouseEventArgs e)
        {
            disableUpdate = true;
        }

        private void tbarGreen_MouseDown(object sender, MouseEventArgs e)
        {
            disableUpdate = true;
        }

        private void tbarBlue_MouseDown(object sender, MouseEventArgs e)
        {
            disableUpdate = true;
        }
        private void textboxUpdateAll()
        {
            if (textBoxManualEnter(tbRed.Text, ref redArray, tbarRed))
            {
                //updateColorTextBox(tbarRed, tbRed, tbarRed.Value, ref redArray);
                if (tbarRed.Value == 0) lockTrackBar(tbarRed, ref redLock); // lock track bar if user sets it to 0 because of the way the intesity bar won't go below the 0 threshold
                else if (redLock) { unlockTrackBar(tbarRed, ref redLock); }
            }
            if (textBoxManualEnter(tbGreen.Text, ref greenArray, tbarGreen))
            {
                //updateColorTextBox(tbarGreen, tbGreen, tbarGreen.Value, ref greenArray);
                if (tbarGreen.Value == 0) lockTrackBar(tbarGreen, ref greenLock); // lock track bar if user sets it to 0 because of the way the intesity bar won't go below the 0 threshold
                else if (greenLock) { unlockTrackBar(tbarGreen, ref greenLock); }
            }
            if (textBoxManualEnter(tbBlue.Text, ref blueArray, tbarBlue))
            {
                //updateColorTextBox(tbarBlue, tbBlue, tbarBlue.Value, ref blueArray);
                if (tbarBlue.Value == 0) lockTrackBar(tbarBlue, ref blueLock); // lock track bar if user sets it to 0 because of the way the intesity bar won't go below the 0 threshold
                else if (blueLock) { unlockTrackBar(tbarBlue, ref blueLock); }                
            }
            findAverageIntensity();

        }


        private bool textBoxManualEnter(string newValue, ref byte[] myArray, TrackBar trackbar)
        {
            
            if (newValue.Equals("")) return false;
            uncheckDefaultColors();
            double temp = 0;
            byte[] temparray;
               
            if (rbPercentage.Checked)
            {
                bool isdigits = true;
                foreach (char c in newValue)
                {
                    if (!(c >= '0' || c <= '9' || c.Equals('.')))
                        isdigits = false;
                }
                if (!isdigits) { MessageBox.Show("Text box can only contain numbers when representing a percentage.", "Important Message"); return false; }
                else
                {
                    temp = (Convert.ToDouble(newValue) * 65535) / 100;
                    if (temp > 65535 || temp < 0) { MessageBox.Show("Please enter a number between 0 and 100.", "Important Message"); return false; }
                    trackbar.Value = (int)temp;
                    myArray = BitConverter.GetBytes((int)temp);
                    color = new Thread(delegate ()
                    {
                        myCommands.SetLEDColor2(0, redArray, greenArray, blueArray);
                    });
                    color.Start();
                }
            }
            else
            {
                if (newValue.Length > 4) { MessageBox.Show("Text box can only contain up to 4 characters in Hexidecimal mode.", "Important Message"); return false; }
                bool isdigits = true;
                foreach (char c in newValue)
                {
                    if (!((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f'))) // if this character isn't hex then isdigits is false
                        isdigits = false;
                }
                if (!isdigits) { MessageBox.Show("Text box can only contain numbers character '0' to '9' and 'A' to 'F' or 'a' to 'f' while in Hexidecimal mode.", "Important Message"); return false; }

                int NumberChars = newValue.Length;
                if (NumberChars % 2 == 1) { newValue = "0" + newValue; NumberChars = newValue.Length; }
                byte[] bytes = new byte[4];
                int a = 0;
                for (int i = NumberChars - 2; i >= 0; i -= 2)
                {
                    bytes[a] = Convert.ToByte(newValue.Substring(i, 2), 16);
                    a++;
                }
                // Array.Reverse(bytes);
                int number = BitConverter.ToInt32(bytes, 0);
                trackbar.Value = number;
                myArray = bytes;
                color = new Thread(delegate ()
                {
                    myCommands.SetLEDColor2(0, redArray, greenArray, blueArray);
                });
                color.Start();
            }
            return true;

            

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lbUserColors_SelectedIndexChanged(object sender, EventArgs e) // if user selects color in list box user radio button function to set colors.
        {
            if (lbUserColors.SelectedItem != null) // if no item is selected then it is likely that the button should be in save mode
            {
                string selectedColor = lbUserColors.SelectedItem.ToString();
                for (int a = 0; a < userColorNames.Count; a++)
                {
                    if (userColorNames[a].Equals(selectedColor)) changeColorbyRB(userColors[a][0], userColors[a][1], userColors[a][2]);
                }
                tbUserColorsName.Text = selectedColor;
                if (lbUserColors.SelectedItem != null) bSaveDelete.Text = "Delete";// if no item is selected then it is likely that the button should be in save mode
                
            }
            else bSaveDelete.Text = "Save"; // if an item is selected then the button should be in delete mode

        }

        private void saveNewColor()
        {
            //save custom color
            if (lbUserColors.SelectedItem == null) // if no item is selected then it is likely that the button should be in save mode
            {
                if (tbUserColorsName.Text.Equals("")) { MessageBox.Show("Please choose a name for color in the text box above the save button."); return; }
                foreach (string names in userColorNames)
                {
                    if (tbUserColorsName.Text.Equals(names))
                    { MessageBox.Show("please choose a unique name for the color you want to save."); return; }
                }
                XmlNode myNode = xmlUser.SelectNodes("LEDCOLORS")[0].SelectNodes("USERVALUES")[0].AppendChild(xmlUser.CreateElement("COLOR"));
                myNode.AppendChild(xmlUser.CreateElement("R"));
                myNode.AppendChild(xmlUser.CreateElement("G"));
                myNode.AppendChild(xmlUser.CreateElement("B"));
                myNode.SelectSingleNode("R").InnerText = redArray[1].ToString("X") + "," + redArray[0].ToString("X");
                myNode.SelectSingleNode("G").InnerText = greenArray[1].ToString("X") + "," + greenArray[0].ToString("X");
                myNode.SelectSingleNode("B").InnerText = blueArray[1].ToString("X") + "," + blueArray[0].ToString("X");
                myNode.Attributes.Append(xmlUser.CreateAttribute("name"));
                myNode.Attributes[0].Value = tbUserColorsName.Text;
                xmlUser.SelectNodes("LEDCOLORS")[0].SelectNodes("USERVALUES")[0].AppendChild(myNode);
                xmlUser.Save("userValues.xml");
                userColors.Add(new List<byte[]> { redArray, greenArray, blueArray });
                lbUserColors.Items.Add(tbUserColorsName.Text);
                userColorNames.Add(tbUserColorsName.Text);
            }
            //delete custom color
            else // if an item is selected then the button should be in delete mode
            {
                int deleteIndex = lbUserColors.SelectedIndex;
                userColorNames.RemoveAt(deleteIndex);
                userColors.RemoveAt(deleteIndex);
                lbUserColors.Items.RemoveAt(deleteIndex);
                xmlUser.SelectNodes("LEDCOLORS")[0].SelectNodes("USERVALUES")[0].RemoveChild(xmlUser.SelectNodes("LEDCOLORS")[0].SelectNodes("USERVALUES")[0].SelectNodes("COLOR")[deleteIndex]);
                xmlUser.Save("userValues.xml");
                tbUserColorsName.Text = "";
            }
        }

        private void bSaveDelete_Click(object sender, EventArgs e)
        {
            saveNewColor();
        }

        private void LEDframeforCustomers_Load(object sender, EventArgs e)
        {

        }

        private void tbUserColorsName_KeyUp(object sender, KeyEventArgs e)
        {
            lblcolorName.Text = "Press Enter";
            if (e.KeyCode == Keys.Enter)
            {
                saveNewColor();
                lblcolorName.Text = "";
            }
        }

        private void intensityManualEnter(string newValue, KeyEventArgs e)
        {
            lblIntensitytxt.Text = "Press Enter";
            if (e.KeyCode == Keys.Enter) // when user hits enter the text in the text box gets set to a string and used by "waitForEntry()"
            {
                resetColorlbl();
                if (newValue.Equals("")) return;
                byte[] temparray;
                e.Handled = true;
                e.SuppressKeyPress = true;
                bool isdigits = true;
                foreach (char c in newValue)
                {
                    if (c < '0' || c > '9')
                        isdigits = false;
                }
                if (!isdigits) { MessageBox.Show("Text box can only contain numbers.", "Important Message"); return; }
                int newValueInt = Convert.ToInt32(newValue);
                if (newValueInt > 100 || newValueInt < 0) { MessageBox.Show("Please enter a number between 0 and 100.", "Important Message"); return; }
                else
                {
                    tbarIntensity.Value = newValueInt;
                    double intensity = (Convert.ToDouble(tbarIntensity.Value)); // new intensity
                    previousIntensity = intensity; // save new intensity as old intensity

                    redTemp = (Int32)(intensity * redStep);
                    greenTemp = (Int32)(intensity * greenStep);
                    blueTemp = (Int32)(intensity * blueStep);
                    if (redTemp > 65535 || redTemp < 0) if (!redLock) return;
                    if (greenTemp > 65535 || greenTemp < 0) if (!greenLock) return;
                    if (blueTemp > 65535 || blueTemp < 0) if (!blueLock) return;

                    if (!redLock)
                    {
                        tbarRed.Value = redTemp;
                        updateColorTextBox(tbarRed, tbRed, redTemp, ref redArray);
                    }
                    if (!greenLock)
                    {
                        tbarGreen.Value = greenTemp;
                        updateColorTextBox(tbarGreen, tbGreen, greenTemp, ref greenArray);
                    }
                    if (!blueLock)
                    {
                        tbarBlue.Value = blueTemp;
                        updateColorTextBox(tbarBlue, tbBlue, blueTemp, ref blueArray);
                    }
                    color = new Thread(delegate ()
                    {
                        myCommands.SetLEDColor2(0, redArray, greenArray, blueArray);
                    });
                    color.Start();
                }
            }
        }
        private void uncheckDefaultColors()
        {
            Action temp1 = () => {
                if (Version == "IGNORE ESN CYAN") foreach (RadioButton rb in radioButtonsCyan) rb.Checked = false;
                if (Version == "IGNORE ESN YELLOW") foreach (RadioButton rb in radioButtonsYellow) rb.Checked = false;
            };
            getLEDforCustomers(temp1);

        }

        private int compareColorsWork(List<byte[]> currentColor, List<List<byte[]>> savedColor)
        {

            int matched = 0;
            int equal = 0;
            for (int a = 0; a < savedColor.Count; a++)
            {
                for (int b = 0; b < 3; b++)
                { if (currentColor[b].SequenceEqual(savedColor[a][b])) matched++; }
                if (matched >= 3)
                {
                    if (currentColor[0].SequenceEqual(redArray)) equal++;
                    if (currentColor[1].SequenceEqual(greenArray)) equal++;
                    if (currentColor[2].SequenceEqual(blueArray)) equal++;
                    if (equal == 3)
                    {
                        return a;
                    }

                }
                matched = 0;
                equal = 0;
            }
            return -1;
        }
        private bool compareColors(List<byte[]> color)
        {
            //int matched = 0;
            //int equal = 0;
            int myIndex;
            if (Version.Equals("IGNORE ESN CYAN"))
            {
                if (!defaultValuesDisabled)
                {
                    myIndex = compareColorsWork(color, cyanColors);
                    if (myIndex >= 0)
                    {
                        setLED = false;
                        Action temp1 = () =>
                        {
                            uncheckDefaultColors();
                            ((RadioButton)radioButtonsCyan[myIndex]).Checked = true;
                            lbUserColors.ClearSelected();
                            tbUserColorsName.Text = "";
                        };
                        getLEDforCustomers(temp1);
                        return true;
                    }                    
                }
                myIndex = compareColorsWork(color, userColors);
                if (myIndex >= 0)
                {
                    setLED = false;
                    Action temp1 = () =>
                    {
                        uncheckDefaultColors();
                        lbUserColors.SetSelected(myIndex, true);
                        tbUserColorsName.Text = userColorNames[myIndex];
                    };
                    getLEDforCustomers(temp1);
                    return true;
                }


                //for (int a = 0; a < cyanColors.Count; a++)
                //{
                //    for (int b = 0; b < 3; b++)
                //    { if (color[b].SequenceEqual(cyanColors[a][b])) matched++; }
                //    if (matched >= 3)
                //    {
                //        if (color[0].SequenceEqual(redArray)) equal++;
                //        if (color[1].SequenceEqual(greenArray)) equal++;
                //        if (color[2].SequenceEqual(blueArray)) equal++;
                //        if (equal<3)
                //        {
                //            setLED = false;
                //            Action temp1 = () =>
                //            {
                //                uncheckDefaultColors();
                //                ((RadioButton)radioButtonsCyan[a]).Checked = true;
                //            };
                //            getLEDforCustomers(temp1);
                //            return true;
                //        }                       
                //    }
                //    matched = 0;
                //}                
            }
            if (Version.Equals("IGNORE ESN YELLOW"))
            {
                if (!defaultValuesDisabled)
                {
                    myIndex = compareColorsWork(color, yellowColors);
                    if (myIndex >= 0)
                    {
                        setLED = false;
                        Action temp1 = () =>
                        {
                            uncheckDefaultColors();
                            ((RadioButton)radioButtonsYellow[myIndex]).Checked = true;
                            lbUserColors.ClearSelected();
                            tbUserColorsName.Text = "";
                        };
                        getLEDforCustomers(temp1);
                        return true;
                    }
                }
                myIndex = compareColorsWork(color, userColors);
                if (myIndex >= 0)
                {
                    setLED = false;
                    Action temp1 = () =>
                    {
                        uncheckDefaultColors();
                        lbUserColors.SetSelected(myIndex, true);
                        tbUserColorsName.Text = userColorNames[myIndex];
                    };
                    getLEDforCustomers(temp1);
                    return true;
                }
                //for (int a = 0; a < yellowColors.Count; a++)
                //{
                //    for (int b = 0; b < 3; b++)
                //    { if (color[b].SequenceEqual(yellowColors[a][b])) matched++; }
                //    if (matched >= 3)
                //    {
                //        if (color[0].SequenceEqual(redArray)) equal++;
                //        if (color[1].SequenceEqual(greenArray)) equal++;
                //        if (color[2].SequenceEqual(blueArray)) equal++;
                //        if (equal < 3)
                //        {
                //            setLED = false;
                //            Action temp1 = () =>
                //            {
                //                uncheckDefaultColors();
                //                ((RadioButton)radioButtonsYellow[a]).Checked = true;
                //            };
                //            getLEDforCustomers(temp1);
                //            return true;
                //        }
                //    }
                //    matched = 0;
                //}
                //return false;

            }

            Action temp2 = () =>
            {
                uncheckDefaultColors();
                lbUserColors.ClearSelected();
                //tbUserColorsName.Text = "";
            };
            getLEDforCustomers(temp2);

            return false;
        }
        public void updateGUI(byte[] red, byte[] green, byte[] blue)
        {
            if (disableUpdate) return;
            //if(!defaultValuesDisabled)
                if (compareColors(new List<byte[]> { red, green, blue }))
                    {
                        redArray = red;
                        greenArray = green;
                        blueArray = blue;
                        Action temp1 = () => {
                            if (rbPercentage.Checked)
                            {
                                tbRed.Text = String.Format("{0:0.##}", ((double)BitConverter.ToInt32(red, 0) * 100) / 65535);
                                tbGreen.Text = String.Format("{0:0.##}", ((double)BitConverter.ToInt32(green, 0) * 100) / 65535);
                                tbBlue.Text = String.Format("{0:0.##}", ((double)BitConverter.ToInt32(blue, 0) * 100) / 65535);
                            }
                            else
                            {
                                tbRed.Text = string.Format("{0:X2}{1:X2}", red[1], red[0]);
                                tbGreen.Text = string.Format("{0:X2}{1:X2}", green[1], green[0]);
                                tbBlue.Text = string.Format("{0:X2}{1:X2}", blue[1], blue[0]);
                            }




                            int redVal = BitConverter.ToInt32(red, 0); // update track bars
                            tbarRed.Value = redVal;
                            int greenVal = BitConverter.ToInt32(green, 0);
                            tbarGreen.Value = greenVal;
                            int blueVal = BitConverter.ToInt32(blue, 0);
                            tbarBlue.Value = blueVal;



                            if (tbarRed.Value == 0) lockTrackBar(tbarRed, ref redLock); // lock track bar if user sets it to 0 because of the way the intesity bar won't go below the 0 threshold
                            else if (redLock) { unlockTrackBar(tbarRed, ref redLock); }
                            if (tbarGreen.Value == 0) lockTrackBar(tbarGreen, ref greenLock); // lock track bar if user sets it to 0 because of the way the intesity bar won't go below the 0 threshold
                            else if (greenLock) { unlockTrackBar(tbarGreen, ref greenLock); }
                            if (tbarBlue.Value == 0) lockTrackBar(tbarBlue, ref blueLock); // lock track bar if user sets it to 0 because of the way the intesity bar won't go below the 0 threshold
                            else if (blueLock) { unlockTrackBar(tbarBlue, ref blueLock); }

                            findAverageIntensity(); // re-allign intesity trackbar
                        };
                        getLEDforCustomers(temp1);
                    }


        }
        private void resetColorlbl()
        {
            lblRedtxt.Text = "";
            lblGreentxt.Text = "";
            lblBluetxt.Text = "";
            lblIntensitytxt.Text = "";
        }
        private void changeColorbyRB(byte[] red, byte[] green, byte[] blue)
        {
            if (!setLED) { setLED = true; return; }            
            redArray = red; // update arrays
            greenArray = green;
            blueArray = blue;
            
            if (rbPercentage.Checked)
            {
                tbRed.Text = String.Format("{0:0.##}", ((double)BitConverter.ToInt32(red, 0) * 100) / 65535);
                tbGreen.Text = String.Format("{0:0.##}", ((double)BitConverter.ToInt32(green, 0) * 100) / 65535);
                tbBlue.Text = String.Format("{0:0.##}", ((double)BitConverter.ToInt32(blue, 0) * 100) / 65535);
            }
            else
            {
                tbRed.Text = string.Format("{0:X2}{1:X2}", red[1], red[0]);
                tbGreen.Text = string.Format("{0:X2}{1:X2}", green[1], green[0]);
                tbBlue.Text = string.Format("{0:X2}{1:X2}", blue[1], blue[0]);
            }
            
            int redVal = BitConverter.ToInt32(red, 0); // update track bars
            tbarRed.Value = redVal;
            int greenVal = BitConverter.ToInt32(green, 0);
            tbarGreen.Value = greenVal;
            int blueVal = BitConverter.ToInt32(blue, 0);
            tbarBlue.Value = blueVal;

            color = new Thread(delegate ()
            {
                myCommands.SetLEDColor2(0, redArray, greenArray, blueArray);
            });
            color.Start();

            if (tbarRed.Value == 0) lockTrackBar(tbarRed, ref redLock); // lock track bar if user sets it to 0 because of the way the intesity bar won't go below the 0 threshold
            else if (redLock) { unlockTrackBar(tbarRed, ref redLock); }
            if (tbarGreen.Value == 0) lockTrackBar(tbarGreen, ref greenLock); // lock track bar if user sets it to 0 because of the way the intesity bar won't go below the 0 threshold
            else if (greenLock) { unlockTrackBar(tbarGreen, ref greenLock); }
            if (tbarBlue.Value == 0) lockTrackBar(tbarBlue, ref blueLock); // lock track bar if user sets it to 0 because of the way the intesity bar won't go below the 0 threshold
            else if (blueLock) { unlockTrackBar(tbarBlue, ref blueLock); }

            findAverageIntensity(); // re-allign intesity trackbar

        }


        // ALL BELOW ARRAYs ARE SET FOR LITTLE ENDIAN

        private void rbCyanRed_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCyanRed.Checked)
            {
                
                changeColorbyRB(cyanRed[0], cyanRed[1], cyanRed[2]);
            }
        }
        private void rbCyanGreen_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCyanGreen.Checked)
            {

                changeColorbyRB(cyanGreen[0], cyanGreen[1], cyanGreen[2]);
            }
        }
        private void rbCyanBlue_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCyanBlue.Checked)
            {

                changeColorbyRB(cyanBlue[0], cyanBlue[1], cyanBlue[2]);
            }
        }
        private void rbCyanOrange_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCyanOrange.Checked)
            {

                changeColorbyRB(cyanOrange[0], cyanOrange[1], cyanOrange[2]);
            }
        }
        private void rbCyanWhite_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCyanWhite.Checked)
            {

                changeColorbyRB(cyanWhite[0], cyanWhite[1], cyanWhite[2]);
            }
        }

        private void rbPercentage_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rbCyanOff_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCyanOff.Checked)
            {

                changeColorbyRB(cyanOff[0], cyanOff[1], cyanOff[2]);
            }
        }

        private void rbYellowRed_CheckedChanged(object sender, EventArgs e)
        {
            if (rbYellowRed.Checked)
            {

                changeColorbyRB(yellowRed[0], yellowRed[1], yellowRed[2]);
            }
        }
        private void rbYellowGreen_CheckedChanged(object sender, EventArgs e)
        {
            if (rbYellowGreen.Checked)
            {

                changeColorbyRB(yellowGreen[0], yellowGreen[1], yellowGreen[2]);
            }
        }

        private void rbYellowOrange_CheckedChanged(object sender, EventArgs e)
        {
            if (rbYellowOrange.Checked)
            {

                changeColorbyRB(yellowOrange[0], yellowOrange[1], yellowOrange[2]);
            }
        }
        private void rbYellowWhite_CheckedChanged(object sender, EventArgs e)
        {
            if (rbYellowWhite.Checked)
            {

                changeColorbyRB(yellowWhite[0], yellowWhite[1], yellowWhite[2]);
            }
        }

        private void rbYellowOff_CheckedChanged(object sender, EventArgs e)
        {
            if (rbYellowOff.Checked)
            {
                changeColorbyRB(yellowOff[0], yellowOff[1], yellowOff[2]);
            }
        }

        private void cbEnableSliders_CheckedChanged(object sender, EventArgs e)
        {
            if (cbEnableSliders.Checked) setEnabled(ColorsTrackBars);
            else setDisabled(ColorsTrackBars);
        }


        public void getLEDforCustomers(Action job) // set the gui console to enabled depending on some conditions
        {
            try
            {
                if (this.lblESN.InvokeRequired)
                {
                    dgetLEDforCustomers d = new dgetLEDforCustomers(getLEDforCustomers);
                    this.Invoke(d, new object[] { job });
                }
                else
                {
                    job();
                }
            }
            catch (Exception e) { };
        }

        private void bHelp_Click(object sender, EventArgs e)
        {
            if (helpframe == null)
            {
                helpframe = new HelpFrame();
                helpframe.Show();
                return;
            }
            else if (helpframe.IsDisposed)
            {
                helpframe = null;
                helpframe = new HelpFrame();
                helpframe.Show();
                return;
            }
        }

        private void lbComPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbComPorts.SelectedItem != null)
            {
                myCommands.initializeDUTLED(lbComPorts.SelectedItem.ToString());
                //while (myCommands.Devices.Count == 0) myCommands.initializeDUTLED(lbComPorts.SelectedItem.ToString());
                myCommands.Devices[0].wakeUp = true;
                setDisabled(initialDisable);
                setInvisible(radioButtonsYellow);
                setInvisible(radioButtonsCyan);

            }
        }




        public void updateBaud(string myBaud)
        {
            Action temp1 = () =>
            {
                //lblBaud.Text = myBaud;
            };
            getLEDforCustomers(temp1);
        }
        private void updateColorTextBox(TrackBar trackBar, TextBox textBox, int myvalue, ref byte[] myColor)
        {
            myColor = BitConverter.GetBytes(myvalue);
            if (rbPercentage.Checked) textBox.Text = String.Format("{0:0.##}", ((Double)myvalue * 100.0 / 65535.0));
            else
            {
                textBox.Text = string.Format("{0:X2}{1:X2}", myColor[1], myColor[0]);
            }
        }
        private void lockTrackBar(TrackBar trackBar, ref bool mybool)
        {
            //trackBar.BackColor = Color.Gray;
            mybool = true;
        }
        private void unlockTrackBar(TrackBar trackBar, ref bool mybool)
        {
            //trackBar.BackColor = SystemColors.Control;
            mybool = false;
        }
        private void findAverageIntensity()
        {
            resetColorlbl();
            List<int> tempArray = new List<int>();
            int red = tbarRed.Value;
            int green = tbarGreen.Value;
            int blue = tbarBlue.Value;
            if (!redLock) tempArray.Add(red); // don't allow the value of track bar to be entered into the array if it is locked.
            if (!greenLock) tempArray.Add(green);
            if (!blueLock) tempArray.Add(blue);
            if (tempArray.Count == 0) return;
            double max = ((Double)tempArray.Max() * 100) / 65535;
            double min = ((Double)tempArray.Min() * 100) / 65535;
            //int distance = 100 - (max - min);
            //step = Convert.ToDouble(distance) / 100; // determine how much the rgb colors should step for every point of the intensity track bar
            int position = Convert.ToInt32(max);
            //if (step == 0) { position = 100; step = 1; }
            //else position = 100 - (int)(Convert.ToDouble(100 - max) / step); // see how many steps it would take based on new step count to get from max position to 100 intensity and place new intesity pointer there.
            // 0.04 * 65535 / 45321 or precentage of change per step * max / current value * max
            //ratio to 65535  *  max 
            if (red != 0) redStep = (red / max);
            if (green != 0) greenStep = (green / max);
            if (blue != 0) blueStep = (blue / max);
            tbarIntensity.Value = position;
            tbIntensity.Text = position.ToString();
            previousIntensity = position;
        }
    }
}
