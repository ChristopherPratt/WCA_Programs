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
        public PostProgrammingFrame(Commands c,MainFrame mf)
        {
            commands = c;
            mainFrame = mf;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            commands.StartEOL(0);
            mainFrame.resetGUI();
            this.Close();
        }

        private void bDebug_Click(object sender, EventArgs e)
        {
            commands.StartDebug(0);
            mainFrame.resetGUI();

            this.Close();

        }

        private void bBootloader_Click(object sender, EventArgs e)
        {
            mainFrame.resetGUI();
            this.Close();
        }
    }
}
