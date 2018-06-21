using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Timers;


namespace WcaDVConsole
{
    public partial class LEDframe : Form
    {
        Thread color;
        Commands myCommands;
        byte[] redArray;
        byte[] greenArray;
        byte[] blueArray;
        double previousIntensity = 50;
        int redTemp, greenTemp, blueTemp, DeviceIndex;
        double redStep, greenStep, blueStep;
        bool redLock = false, greenLock = false, blueLock = false;
        double step = 1;

        private System.Timers.Timer readComportsTimer, readColorTimer;
        delegate void dgetLEDforCustomers(Action job);

        List<byte[]> ColorArray;
        List<byte[]> cyanRed, cyanOrange, cyanBlue, cyanGreen, cyanWhite, cyanOff, yellowRed, yellowOrange, yellow, yellowGreen, yellowWhite, yellowOff;
        List<List<byte[]>> cyanColors;
        List<List<byte[]>> yellowColors;

        List<Control> radioButtonsCyan, radioButtonsYellow;

        public LEDframe(Commands c, int d)
        {
            DeviceIndex = d;
            myCommands = c;
            InitializeComponent();
            redArray = new byte[4];
            greenArray = new byte[4];
            blueArray = new byte[4];
            tbarIntensity.Value = 50;

            radioButtonsCyan = new List<Control>() { rbCyanOrange, rbCyanRed, rbCyanGreen, rbCyanWhite, rbCyanBlue, rbCyanOff };
            radioButtonsYellow = new List<Control>() { rbYellowOrange, rbYellowRed, rbYellowGreen, rbYellowWhite, rbYellowOff };
            setVisible(radioButtonsCyan);
            setInvisible(radioButtonsYellow);
            initiateColorArrays();
            lblDUT.Text = myCommands.Devices[DeviceIndex].dutNumber;

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
            cyanRed = new List<byte[]> { new byte[] { 0xFF, 0x07, 0, 0 }, new byte[] { 0, 0, 0, 0 }, new byte[] { 0, 0, 0, 0 } };
            cyanOrange = new List<byte[]> { new byte[] { 0xFB, 0x27, 0, 0 }, new byte[] { 0xFF, 0x07, 0, 0 }, new byte[] { 0, 0, 0, 0 } };
            cyanBlue = new List<byte[]> { new byte[] { 0, 0, 0, 0 }, new byte[] { 0xFF, 0x01, 0, 0 }, new byte[] { 0xFB, 0x09, 0, 0 } };
            cyanGreen = new List<byte[]> { new byte[] { 0, 0, 0, 0 }, new byte[] { 0xFF, 0x07, 0, 0 }, new byte[] { 0, 0, 0, 0 } };
            cyanWhite = new List<byte[]> { new byte[] { 0xFF, 0x2F, 0, 0 }, new byte[] { 0xFF, 0x2F, 0, 0 }, new byte[] { 0xFF, 0x2F, 0, 0 } };
            cyanOff = new List<byte[]> { new byte[] { 0, 0, 0, 0 }, new byte[] { 0, 0, 0, 0 }, new byte[] { 0, 0, 0, 0 } };
            yellowRed = new List<byte[]> { new byte[] { 0xFF, 0xFF, 0, 0 }, new byte[] { 0, 0, 0, 0 }, new byte[] { 0, 0, 0, 0 } };
            yellowOrange = new List<byte[]> { new byte[] { 0xFF, 0x9F, 0, 0 }, new byte[] { 0xFF, 0x5F, 0, 0 }, new byte[] { 0xFF, 0x03, 0, 0 } };
            yellowGreen = new List<byte[]> { new byte[] { 0, 0, 0, 0 }, new byte[] { 0xFF, 0x3F, 0, 0 }, new byte[] { 0xFF, 0x01, 0, 0 } };
            yellowWhite = new List<byte[]> { new byte[] { 0xFF, 0x0A, 0, 0 }, new byte[] { 0xFF, 0x62, 0, 0 }, new byte[] { 0xFF, 0x0F, 0, 0 } };
            yellowOff = new List<byte[]> { new byte[] { 0, 0, 0, 0 }, new byte[] { 0, 0, 0, 0 }, new byte[] { 0, 0, 0, 0 } };
            cyanColors = new List<List<byte[]>> { cyanRed, cyanOrange, cyanBlue, cyanGreen, cyanWhite, cyanOff };
            yellowColors = new List<List<byte[]>> { yellowRed, yellowOrange, yellowGreen, yellowWhite, yellowOff };
            ColorArray = new List<byte[]> { redArray, greenArray, blueArray };

        }


       

        private void setVisible(List<Control> list)
        {
            foreach (Control temp in list) temp.Visible = true;
        }
        private void setInvisible(List<Control> list)
        {
            foreach (Control temp in list) temp.Visible = false;
        }

        private void tbarRed_Scroll(object sender, EventArgs e)
        {
            updateColorTextBox(tbarRed, tbRed, tbarRed.Value, ref redArray);
            if (tbarRed.Value == 0) lockTrackBar(tbarRed, ref redLock); // lock track bar if user sets it to 0 because of the way the intesity bar won't go below the 0 threshold
            else if (redLock) unlockTrackBar(tbarRed, ref redLock);
        }

        private void tbarGreen_Scroll(object sender, EventArgs e)
        {
            updateColorTextBox(tbarGreen, tbGreen, tbarGreen.Value, ref greenArray);
            if (tbarGreen.Value == 0) lockTrackBar(tbarGreen, ref greenLock); // lock track bar if user sets it to 0 because of the way the intesity bar won't go below the 0 threshold
            else if (greenLock) unlockTrackBar(tbarGreen, ref greenLock);

        }

        private void tbarBlue_Scroll(object sender, EventArgs e)
        {
            updateColorTextBox(tbarBlue, tbBlue, tbarBlue.Value, ref blueArray);
            if (tbarBlue.Value == 0) lockTrackBar(tbarBlue, ref blueLock); // lock track bar if user sets it to 0 because of the way the intesity bar won't go below the 0 threshold
            else if (blueLock)unlockTrackBar(tbarBlue, ref blueLock);
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
                tbRed.Text = string.Format("{0:X}{1:X}", redArray[1], redArray[0]);
                tbGreen.Text = string.Format("{0:X}{1:X}", greenArray[1], greenArray[0]);
                tbBlue.Text = string.Format("{0:X}{1:X}", blueArray[1], blueArray[0]);
                tbRed.MaxLength = 4;
                tbGreen.MaxLength = 4;
                tbBlue.MaxLength = 4;
            }
            else
            {
                tbRed.Text = ((BitConverter.ToInt32(redArray,0) * 100) / 65535).ToString();
                tbGreen.Text = ((BitConverter.ToInt32(greenArray, 0) * 100) / 65535).ToString();
                tbBlue.Text = ((BitConverter.ToInt32(blueArray, 0) * 100) / 65535).ToString();
                tbRed.MaxLength = 3;
                tbGreen.MaxLength = 3;
                tbBlue.MaxLength = 3;
            }
        }
        private void sendCommand()
        {
            color = new Thread(delegate ()
            {
                myCommands.SetLEDColor2(DeviceIndex, redArray, greenArray, blueArray);
            });
            color.Start();
        }

        private void tbarRed_MouseUp(object sender, MouseEventArgs e)
        {
            findAverageIntensity();
            sendCommand();
        }

        private void tbarGreen_MouseUp(object sender, MouseEventArgs e)
        {
            findAverageIntensity();
            sendCommand();

        }

        private void tbarBlue_MouseUp(object sender, MouseEventArgs e)
        {
            findAverageIntensity();
            sendCommand();
        }
        private void tbarIntensity_MouseUp(object sender, MouseEventArgs e)
        {
            sendCommand();
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
            int temp = 0;
            byte[] temparray;

            if (rbPercentage.Checked)
            {
                bool isdigits = true;
                foreach (char c in newValue)
                {
                    if (c < '0' || c > '9')
                        isdigits = false;
                }
                if (!isdigits) { MessageBox.Show("Text box can only contain numbers when representing a percentage.", "Important Message"); return false; }
                else
                {
                    temp = (Convert.ToInt32(newValue) * 65535) / 100;
                    if (temp > 65535 || temp < 0) { MessageBox.Show("Please enter a number between 0 and 100.", "Important Message"); return false; }
                    trackbar.Value = temp;
                    myArray = BitConverter.GetBytes(temp);
                    sendCommand();
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
                sendCommand();
                color.Start();
            }
            return true;
        }    
        private void resetColorlbl()
        {
            lblRedtxt.Text = "";
            lblGreentxt.Text = "";
            lblBluetxt.Text = "";
            lblIntensitytxt.Text = "";
        }

        

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




        private void changeColorbyRB(byte[] red, byte[] green, byte[] blue)
        {

            redArray = red; // update arrays
            greenArray = green;
            blueArray = blue;

            if (rbPercentage.Checked)
            {
                tbRed.Text = ((BitConverter.ToInt32(red, 0) * 100) / 65535).ToString();
                tbGreen.Text = ((BitConverter.ToInt32(green, 0) * 100) / 65535).ToString();
                tbBlue.Text = ((BitConverter.ToInt32(blue, 0) * 100) / 65535).ToString();
            }
            else
            {
                tbRed.Text = string.Format("{0:X}{1:X}", red[1], red[0]);
                tbGreen.Text = string.Format("{0:X}{1:X}", green[1], green[0]);
                tbBlue.Text = string.Format("{0:X}{1:X}", blue[1], blue[0]);
            }




            int redVal = BitConverter.ToInt32(red, 0); // update track bars
            tbarRed.Value = redVal;
            int greenVal = BitConverter.ToInt32(green, 0);
            tbarGreen.Value = greenVal;
            int blueVal = BitConverter.ToInt32(blue, 0);
            tbarBlue.Value = blueVal;

            sendCommand();

            if (tbarRed.Value == 0) lockTrackBar(tbarRed, ref redLock); // lock track bar if user sets it to 0 because of the way the intesity bar won't go below the 0 threshold
            else if (redLock) { unlockTrackBar(tbarRed, ref redLock); }
            if (tbarGreen.Value == 0) lockTrackBar(tbarGreen, ref greenLock); // lock track bar if user sets it to 0 because of the way the intesity bar won't go below the 0 threshold
            else if (greenLock) { unlockTrackBar(tbarGreen, ref greenLock); }
            if (tbarBlue.Value == 0) lockTrackBar(tbarBlue, ref blueLock); // lock track bar if user sets it to 0 because of the way the intesity bar won't go below the 0 threshold
            else if (blueLock) { unlockTrackBar(tbarBlue, ref blueLock); }

            findAverageIntensity(); // re-allign intesity trackbar

        }



        private void intensityManualEnter(string newValue, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // when user hits enter the text in the text box gets set to a string and used by "waitForEntry()"
            {
                
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
                    double intensity = (Convert.ToDouble(newValueInt)); // new intensity
                    double percentageChange = (intensity - previousIntensity) * step; // how much to change all other rgb values
                    previousIntensity = intensity; // save new intensity as old intensity
                    double colorChange = (percentageChange * 65535) / 100; // determine how many points to slide all rgb values based on a percentage
                    redTemp = tbarRed.Value + (int)colorChange; // add the amount to change to the existing values of color
                    greenTemp = tbarGreen.Value + (int)colorChange;
                    blueTemp = tbarBlue.Value + (int)colorChange;
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
                }
                    

            }
            
        }

        private void rbYellow_CheckedChanged(object sender, EventArgs e)
        {
            if (rbYellow.Checked)
            {
                setVisible(radioButtonsYellow);
                setInvisible(radioButtonsCyan);
            }
            else
            {
                setVisible(radioButtonsCyan);
                setInvisible(radioButtonsYellow);
            }


        }

        private void rbCyan_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void updateColorTextBox(TrackBar trackBar, TextBox textBox, int myvalue, ref byte[] myColor)
        {            
            myColor = BitConverter.GetBytes(myvalue);
            if (rbPercentage.Checked) textBox.Text = ((myvalue * 100) / 65535).ToString();
            else
            {
                textBox.Text = string.Format("{0:X}{1:X}", myColor[1], myColor[0]);
            }
        }
        private void lockTrackBar(TrackBar trackBar, ref bool mybool)
        {
            trackBar.BackColor = Color.Gray;
            mybool = true;
        }
        private void unlockTrackBar(TrackBar trackBar, ref bool mybool)
        {
            trackBar.BackColor = SystemColors.Control;
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
            int position = Convert.ToInt32(max);
            if (red != 0) redStep = (red / max);
            if (green != 0) greenStep = (green / max);
            if (blue != 0) blueStep = (blue / max);
            tbarIntensity.Value = position;
            tbIntensity.Text = position.ToString();
            previousIntensity = position;
        }
    }
}
