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
    public partial class PostProgrammingFrame : Form
    {
        Commands commands;
        MainFrame mainFrame;
        delegate void dgetpPostProgrammingFrame(Action job);

        int deviceIndex;


        public void getpPostProgrammingFrame(Action job) // set the gui console to enabled depending on some conditions
        {
            try
            {
                if (this.lblnewFirmware.InvokeRequired)
                {
                    dgetpPostProgrammingFrame d = new dgetpPostProgrammingFrame(getpPostProgrammingFrame);
                    this.Invoke(d, new object[] { job });
                }
                else
                {
                    job();
                }
            }
            catch (Exception e) { };
        }
        public PostProgrammingFrame(Commands c,MainFrame mf, int d)
        {
            commands = c;
            mainFrame = mf;
            deviceIndex = d;
            InitializeComponent();
            
        }
        public void updatelblFirwareVersion(string newVersion)
        {
            Action temp3 = () =>
            {
                lblnewFirmware.Text = newVersion;
            };
            getpPostProgrammingFrame(temp3);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            if (mainFrame.ProgramMode.Equals("Programming mode"))
            {
                mainFrame.startNewMode("End of line test mode");
                commands.Devices[deviceIndex].readesn = true;
                commands.runCommands("Start EOL", deviceIndex);
                mainFrame.resetGUI();
                mainFrame.returnFromProgrammingMode();                
            }
            else
            {
                commands.runCommands("Start EOL", deviceIndex);
            }
            this.Close();
        }

        private void bDebug_Click(object sender, EventArgs e)
        {
            commands.StartDebug(deviceIndex);
            if (mainFrame.ProgramMode.Equals("Programming mode"))
            {
                mainFrame.startNewMode("End of line test mode");
                mainFrame.resetGUI();
                mainFrame.returnFromProgrammingMode();               
            }
            this.Close();
        }

        private void bBootloader_Click(object sender, EventArgs e)
        {
            mainFrame.resetGUI();
            this.Close();
        }

        private void PostProgrammingFrame_Load(object sender, EventArgs e)
        {
            commands.runCommands("Read application version", deviceIndex);
        }
    }
}
