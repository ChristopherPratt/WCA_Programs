using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WcaDVConsole
{
    public partial class HelpFrame : Form
    {
        public HelpFrame()
        {
            InitializeComponent();
            lblHelp.Text =
                "0. Have Wireless Charger unplugged from power (ACC OFF) and USB serial from the PC.\n"
                + "1.Plug USB Serial cable into the computer.\n"
                + "2.Select the new COM port that appears in the ComPorts list box.\n"
                + "3.Apply 12 Volts (ACC ON) to wireless charger device.\n"
                + "4.Beneath the \"Help\" button the message should say \"Connected\".\n"
                + "5.The Controls for choosing the default colors for the LED should now be enabled.\n"
                + "(Note: Sometimes the device may not connect after cycling power.Simply cycle power\n"
                + "again to the device until it connects properly.)\n\n\n"


                + "If the message beneath the \"Help\" button says \"COM port could not be opened\" then\n"
                + "there must be another program currently connected to the COM port you have chosen.\n"
                + "First close all other programs which could be making this connection. If it still fails\n"
                + "to connect - try restarting the PC.";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
