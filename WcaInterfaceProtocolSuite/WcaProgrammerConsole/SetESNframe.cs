using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WcaDVConsole
{
    public partial class SetESNframe : Form
    {
        Thread sendCommand;
        Commands commands;
        int DeviceIndex;
        public SetESNframe(Commands c, int d)
        {
            DeviceIndex = d;
            commands = c;
            InitializeComponent();
            if (commands.Devices[DeviceIndex].OTP != null) lblOTP.Text = commands.Devices[DeviceIndex].OTP;
            lblDUT.Text = commands.Devices[DeviceIndex].dutNumber;
        }

        private void bsetESN_Click(object sender, EventArgs e)
        {
            List<int> esnArray = new List<int>();

            string esn = tbESN.Text;
            string hw = tbHW.Text;
            byte[] finalArray;
            if (!checkESNlength(esn)) return;
            if (!checkESNdigits(esn, ref esnArray)) return;
            if (!checkHWlength(hw)) return;
            if (!checkHWdigits(hw)) return;
            if (esnArray.Count == 16) finalArray = packageInfoNEW(esnArray, hw);
            else finalArray = packageInfoOLD(esnArray, hw);
            setESNcommand(finalArray);
            lblOTP.Text = (Convert.ToInt32(commands.Devices[DeviceIndex].OTP) - 1).ToString();
        }
        private bool checkESNlength(string esn_t)
        {
            if (esn_t.Length != 13 && esn_t.Length != 16)
            {
                lblESNstatus.Text = "The ESN must be either 13 or 16 digits long.";
                return false;
            }
            lblESNstatus.Text = "";
            return true;
        }
        private bool checkESNdigits(string esn_t, ref List<int> esnArray_t)
        {
            int q = 0;
            bool isdigits = true;
            foreach (char c in esn_t)
            {
                if (c < '0' || c > '9') { isdigits = false; }
                esnArray_t.Add(Convert.ToInt32(c.ToString()));
            }
            if (!isdigits)
            {
                lblESNstatus.Text = "Please enter numerical digits only.";
                return false;
            }
            lblESNstatus.Text = "";
            return true;
        }
        private bool checkHWlength(string hw_t)
        {
            if (hw_t.Length < 3 && hw_t.Length > 6)
            {
                lblHWstatus.Text = "The HW must be between 3 and 5 digits long.";
                return false;
            }
            lblHWstatus.Text = "";
            return true;
        }
        private bool checkHWdigits(string hw_t)
        {
            string[] temphw = hw_t.Split('.');
            if (temphw.Length != 2) { lblHWstatus.Text = "You must include a period to separate major and minor versions"; return false;}
            foreach(string temp in temphw)
            {
                if (temp.Length > 2 || temp.Length < 1) { lblHWstatus.Text = "The major and minor version must be either 1 or 2 digits long"; return false; }
                int q = 0;
                bool isdigits = true;
                foreach (char c in temp)
                {
                    if (c < '0' || c > '9') { isdigits = false; }
                }
                if (!isdigits)
                {
                    lblHWstatus.Text = "Please enter numerical digits only.";
                    return false;
                }
            }
            lblHWstatus.Text = "";
            return true;
        }
        private byte[] packageInfoNEW(List<int> esnArray_t, string hw_t)
        {
            string[] hwArray2 = hw_t.Split('.');
            byte[] inputArray2 = new byte[12]; // 12 is the number of bytes in the command for writing the ESN and the HW version
            int inputIndex2 = 9; // the number of bytes necessary to populate the ESN alone
                                 // hard populating certain parts of the inputArray that can't be parsed by the for loop im about to use
            inputArray2[0] = 0x02; //little endianess
            inputArray2[1] = 0x01; // write to serial number
                                   // elements 0 through 8 are used for the ESN. written literally in hex in little endian
            for (int a = 0; a < 16; a += 2)
            {
                inputArray2[inputIndex2] = Convert.ToByte((esnArray_t[a] * 16) + esnArray_t[a + 1]); // convert the ESN in to Literal HEX in little endian
                inputIndex2--;
            }

            int[] primaryHWarray2 = new int[2];
            int[] subHWarray2 = new int[2];

            //already verified that the read in value is at max 2 digits
            if (hwArray2[0].Length > 1)  //it was two digits
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

            commands.writeLineToConsole( string.Format("Set General Device Data Array: {0:X2} {1:X2} {2:X2} {3:X2} {4:X2} {5:X2} {6:X2} {7:X2} {8:X2} {9:X2} {10:X2} {11:X2}",
                inputArray2[0], inputArray2[1], inputArray2[2], inputArray2[3], inputArray2[4], inputArray2[5], inputArray2[6], inputArray2[7],
                inputArray2[8], inputArray2[9], inputArray2[10], inputArray2[11]));

            return inputArray2;
        }
        private byte[] packageInfoOLD(List<int> esnArray_t, string hw_t)
        {
            string[] hwArray = hw_t.Split('.');
            byte[] inputArray = new byte[14]; // 14 is the number of bytes in the command for writing the ESN and the HW version
            int inputIndex = 7; // the number of bytes necessary to populate the ESN alone
                                // hard populating certain parts of the inputArray that can't be parsed by the for loop im about to use
            inputArray[0] = 0x02; //little endianess
            inputArray[1] = 0x01; // write to serial number
                                  // elements 2 through 8 (7 total) are used for the ESN. written literally in hex in little endian
            for (int a = 1; a < 12; a += 2)
            {
                inputArray[inputIndex] = Convert.ToByte((esnArray_t[a] * 16) + esnArray_t[a + 1]); // convert the ESN in to Literal HEX in little endian
                inputIndex--;
            }
            // hard populating certain parts of the inputArray that can't be parsed by the for loop I used.
            inputArray[8] = Convert.ToByte(esnArray_t[0]);
            inputArray[9] = 0x00; // separator byte between ESN and HW 

            int primaryHW;
            int subHW;
            Int32.TryParse(hwArray[0], out primaryHW); // convert string array elements to int which can then be converted to byte
            Int32.TryParse(hwArray[1], out subHW);
            inputArray[10] = Convert.ToByte(subHW); // sub version
            inputArray[11] = 0x00;
            inputArray[12] = Convert.ToByte(primaryHW); ; //primary version// HW version here. supposed for rev 1.0 for elements 10 through 13 in little endian
            inputArray[13] = 0x00;

            commands.writeLineToConsole(string.Format("Set General Device Data Array: {0:X2} {1:X2} {2:X2} {3:X2} {4:X2} {5:X2} {6:X2} {7:X2} {8:X2} {9:X2} {10:X2} {11:X2} {12:X2} {13:X2}",
                inputArray[0], inputArray[1], inputArray[2], inputArray[3], inputArray[4], inputArray[5], inputArray[6], inputArray[7],
                inputArray[8], inputArray[9], inputArray[10], inputArray[11], inputArray[12], inputArray[13]));

            return inputArray;

        }

        private void setESNcommand(byte[] finalArray_t)
        {
            sendCommand = new Thread(delegate ()
            {
                commands.SetESNfromForm(DeviceIndex, finalArray_t);
            });
            sendCommand.Start();
            commands.Devices[DeviceIndex].setGUIESN = true;
            commands.runCommands("Read ESN(big endian)", DeviceIndex);
        }

        private void lblHW_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
