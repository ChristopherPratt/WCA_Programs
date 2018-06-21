using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;


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
        int redTemp, greenTemp, blueTemp;
        bool redLock = false, greenLock = false, blueLock = false;
        double step = 1;

        public LEDframe(Commands c)
        {
            myCommands = c;
            InitializeComponent();
            redArray = new byte[4];
            greenArray = new byte[4];
            blueArray = new byte[4];
            tbarIntensity.Value = 50;

        }

        private void bColor_Click(object sender, EventArgs e)
        {
            color = new Thread(delegate()
             {
                 myCommands.SetLEDColor2(0, redArray, greenArray, blueArray);
             });
            color.Start();
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
            double intensity = (Convert.ToDouble(tbarIntensity.Value)); // new intensity
            double percentageChange = (intensity - previousIntensity) * step; // how much to change all other rgb values
            previousIntensity = intensity; // save new intensity as old intensity
            double colorChange = (percentageChange * 65535) / 100; // determine how many points to slide all rgb values based on a percentage
            redTemp = tbarRed.Value + (int)colorChange; // add the amount to change to the existing values of color
            greenTemp = tbarGreen.Value + (int)colorChange;
            blueTemp = tbarBlue.Value + (int)colorChange;
            if (redTemp > 65535 || redTemp < 0) if (!redLock) return;
            if (greenTemp > 65535 || greenTemp < 0) if (!greenLock) return;
            if (blueTemp > 65535 || blueTemp < 0) if (!blueLock) return;

            if(!redLock)
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



            //tbIntensity.Text = (Convert.ToInt32(tbIntensity.Text) + percentageChange).ToString();
            tbIntensity.Text = tbarIntensity.Value.ToString();
        }

        private void rbHex_CheckedChanged(object sender, EventArgs e)
        {
            if (rbHex.Checked)
            {               
                tbRed.Text = string.Format("{0:X}{1:X}", redArray[1], redArray[0]);
                tbGreen.Text = string.Format("{0:X}{1:X}", greenArray[1], greenArray[0]);
                tbBlue.Text = string.Format("{0:X}{1:X}", blueArray[1], blueArray[0]);
            }
            else
            {
                tbRed.Text = ((BitConverter.ToInt32(redArray,0) * 100) / 65535).ToString();
                tbGreen.Text = ((BitConverter.ToInt32(greenArray, 0) * 100) / 65535).ToString();
                tbBlue.Text = ((BitConverter.ToInt32(blueArray, 0) * 100) / 65535).ToString();
            }
        }

        private void tbarRed_MouseUp(object sender, MouseEventArgs e)
        {
            findAverageIntensity();
        }

        private void tbarGreen_MouseUp(object sender, MouseEventArgs e)
        {
            findAverageIntensity();
        }

        private void tbarBlue_MouseUp(object sender, MouseEventArgs e)
        {
            findAverageIntensity();
        }

        private void tbRed_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBoxManualEnter(tbRed.Text, e, ref redArray, tbarRed))
            {
                //updateColorTextBox(tbarRed, tbRed, tbarRed.Value, ref redArray);
                if (tbarRed.Value == 0) lockTrackBar(tbarRed, ref redLock); // lock track bar if user sets it to 0 because of the way the intesity bar won't go below the 0 threshold
                else if (redLock) { unlockTrackBar(tbarRed, ref redLock); }
                findAverageIntensity();
            }
        }
        private void tbGreen_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBoxManualEnter(tbGreen.Text, e, ref greenArray, tbarGreen))
            {
                //updateColorTextBox(tbarGreen, tbGreen, tbarGreen.Value, ref greenArray);
                if (tbarGreen.Value == 0) lockTrackBar(tbarGreen, ref greenLock); // lock track bar if user sets it to 0 because of the way the intesity bar won't go below the 0 threshold
                else if (greenLock) { unlockTrackBar(tbarGreen, ref greenLock); }
                findAverageIntensity();
            }
        }
        private void tbBlue_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBoxManualEnter(tbBlue.Text, e, ref blueArray, tbarBlue))
                {
                //updateColorTextBox(tbarBlue, tbBlue, tbarBlue.Value, ref blueArray);
                if (tbarBlue.Value == 0) lockTrackBar(tbarBlue, ref blueLock); // lock track bar if user sets it to 0 because of the way the intesity bar won't go below the 0 threshold
                else if (blueLock) { unlockTrackBar(tbarBlue, ref blueLock); }
                findAverageIntensity();
                }
            
        }      
        private void tbIntensity_KeyDown(object sender, KeyEventArgs e)
        {
            intensityManualEnter(tbIntensity.Text, e);
        }



        private bool textBoxManualEnter(string newValue, KeyEventArgs e,ref byte[] myArray, TrackBar trackbar)
        {
            if (e.KeyCode == Keys.Enter) // when user hits enter the text in the text box gets set to a string and used by "waitForEntry()"
            {
                int temp = 0;
                byte[] temparray;
                e.Handled = true;
                e.SuppressKeyPress = true;
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
                    }      
                }
                else
                {
                    int NumberChars = newValue.Length;
                    if (NumberChars % 2 == 1) { newValue = "0" + newValue; NumberChars = newValue.Length; }
                    byte[] bytes = new byte[4];
                    int a = 0;
                    for (int i = NumberChars-2; i >= 0 ; i -= 2)
                    {
                        bytes[a] = Convert.ToByte(newValue.Substring(i, 2), 16);
                        a++;
                    }
                   // Array.Reverse(bytes);
                    int number = BitConverter.ToInt32(bytes, 0);
                    trackbar.Value = number;
                    myArray = bytes;
                }
                return true;

            }
            return false;
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
        private void findAverageIntensity ()
        {
            List<int> tempArray = new List<int>();

            if (!redLock)  tempArray.Add(tbarRed.Value); // don't allow the value of track bar to be entered into the array if it is locked.
            if (!greenLock) tempArray.Add(tbarGreen.Value);
            if (!blueLock) tempArray.Add(tbarBlue.Value);
            if (tempArray.Count == 0) return;
            int max = (tempArray.Max() * 100) / 65535;
            int min = (tempArray.Min() * 100) / 65535;
            int distance = 100 - (max - min);
            step = Convert.ToDouble(distance) / 100; // determine how much the rgb colors should step for every point of the intensity track bar
            int position = 0;
            if (step == 0) position = 50;
            else position = 100 - (int)(Convert.ToDouble(100 - max) / step); // see how many steps it would take based on new step count to get from max position to 100 intensity and place new intesity pointer there.

            tbarIntensity.Value = position;
            tbIntensity.Text = position.ToString();
            previousIntensity = position;
        }
    }
}
