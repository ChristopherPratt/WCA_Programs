using System.Drawing;
namespace WcaDVConsole
{
    partial class MainFrame
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrame));
            this.lblDUTS = new System.Windows.Forms.Label();
            this.lbDUTnumber = new System.Windows.Forms.ListBox();
            this.lbProgramMode = new System.Windows.Forms.ListBox();
            this.lblFirmwareEVmode = new System.Windows.Forms.Label();
            this.bBrowseFirmwareFiles = new System.Windows.Forms.Button();
            this.lbFirmwareFileEVprograms = new System.Windows.Forms.ListBox();
            this.lblprogramMode = new System.Windows.Forms.Label();
            this.pMainFrame = new System.Windows.Forms.Panel();
            this.Laird = new System.Windows.Forms.Label();
            this.lblProgressBar = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblESN1 = new System.Windows.Forms.Label();
            this.lblDUT1 = new System.Windows.Forms.Label();
            this.lbComPorts1 = new System.Windows.Forms.ListBox();
            this.lblDUT1status = new System.Windows.Forms.Label();
            this.lblESN2 = new System.Windows.Forms.Label();
            this.lblDUT2 = new System.Windows.Forms.Label();
            this.lbComPorts2 = new System.Windows.Forms.ListBox();
            this.lblDUT2status = new System.Windows.Forms.Label();
            this.lblESN3 = new System.Windows.Forms.Label();
            this.lblDUT3 = new System.Windows.Forms.Label();
            this.lbComPorts3 = new System.Windows.Forms.ListBox();
            this.lblDUT3status = new System.Windows.Forms.Label();
            this.lblESN4 = new System.Windows.Forms.Label();
            this.lblDUT4 = new System.Windows.Forms.Label();
            this.lbComPorts4 = new System.Windows.Forms.ListBox();
            this.lblDUT4status = new System.Windows.Forms.Label();
            this.lblESN5 = new System.Windows.Forms.Label();
            this.lblDUT5 = new System.Windows.Forms.Label();
            this.lbComPorts5 = new System.Windows.Forms.ListBox();
            this.lblDUT5status = new System.Windows.Forms.Label();
            this.lblESN6 = new System.Windows.Forms.Label();
            this.lblDUT6 = new System.Windows.Forms.Label();
            this.lbComPorts6 = new System.Windows.Forms.ListBox();
            this.lblDUT6status = new System.Windows.Forms.Label();
            this.lblfavcommands = new System.Windows.Forms.Label();
            this.lbFavCommands = new System.Windows.Forms.ListBox();
            this.lblcommands = new System.Windows.Forms.Label();
            this.lbCommands = new System.Windows.Forms.ListBox();
            this.rtbConsole = new System.Windows.Forms.RichTextBox();
            this.tbConsoleCommands = new System.Windows.Forms.TextBox();
            this.bStart = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblbaud2 = new System.Windows.Forms.Label();
            this.rb1152002 = new System.Windows.Forms.RadioButton();
            this.rb96002 = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblbaud1 = new System.Windows.Forms.Label();
            this.rb1152001 = new System.Windows.Forms.RadioButton();
            this.rb96001 = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblbaud3 = new System.Windows.Forms.Label();
            this.rb1152003 = new System.Windows.Forms.RadioButton();
            this.rb96003 = new System.Windows.Forms.RadioButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblbaud4 = new System.Windows.Forms.Label();
            this.rb1152004 = new System.Windows.Forms.RadioButton();
            this.rb96004 = new System.Windows.Forms.RadioButton();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lblbaud5 = new System.Windows.Forms.Label();
            this.rb1152005 = new System.Windows.Forms.RadioButton();
            this.rb96005 = new System.Windows.Forms.RadioButton();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lblbaud6 = new System.Windows.Forms.Label();
            this.rb1152006 = new System.Windows.Forms.RadioButton();
            this.rb96006 = new System.Windows.Forms.RadioButton();
            this.pMainFrame.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDUTS
            // 
            this.lblDUTS.AutoSize = true;
            this.lblDUTS.Location = new System.Drawing.Point(154, 6);
            this.lblDUTS.Name = "lblDUTS";
            this.lblDUTS.Size = new System.Drawing.Size(105, 26);
            this.lblDUTS.TabIndex = 48;
            this.lblDUTS.Text = "Connected DUTs\r\n(Select all that apply)";
            this.lblDUTS.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblDUTS.Click += new System.EventHandler(this.label4_Click_1);
            // 
            // lbDUTnumber
            // 
            this.lbDUTnumber.FormattingEnabled = true;
            this.lbDUTnumber.Location = new System.Drawing.Point(164, 35);
            this.lbDUTnumber.Name = "lbDUTnumber";
            this.lbDUTnumber.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbDUTnumber.Size = new System.Drawing.Size(82, 69);
            this.lbDUTnumber.TabIndex = 47;
            this.lbDUTnumber.SelectedIndexChanged += new System.EventHandler(this.lbDUTnumber_SelectedIndexChanged);
            // 
            // lbProgramMode
            // 
            this.lbProgramMode.FormattingEnabled = true;
            this.lbProgramMode.Location = new System.Drawing.Point(8, 34);
            this.lbProgramMode.Name = "lbProgramMode";
            this.lbProgramMode.Size = new System.Drawing.Size(134, 69);
            this.lbProgramMode.TabIndex = 46;
            this.lbProgramMode.SelectedIndexChanged += new System.EventHandler(this.lbProgramMode_SelectedIndexChanged_1);
            // 
            // lblFirmwareEVmode
            // 
            this.lblFirmwareEVmode.AutoSize = true;
            this.lblFirmwareEVmode.Location = new System.Drawing.Point(366, 18);
            this.lblFirmwareEVmode.Name = "lblFirmwareEVmode";
            this.lblFirmwareEVmode.Size = new System.Drawing.Size(73, 13);
            this.lblFirmwareEVmode.TabIndex = 45;
            this.lblFirmwareEVmode.Text = "Firmware Files";
            this.lblFirmwareEVmode.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // bBrowseFirmwareFiles
            // 
            this.bBrowseFirmwareFiles.ForeColor = System.Drawing.SystemColors.ControlText;
            this.bBrowseFirmwareFiles.Location = new System.Drawing.Point(540, 75);
            this.bBrowseFirmwareFiles.Name = "bBrowseFirmwareFiles";
            this.bBrowseFirmwareFiles.Size = new System.Drawing.Size(90, 28);
            this.bBrowseFirmwareFiles.TabIndex = 44;
            this.bBrowseFirmwareFiles.Text = "Browse";
            this.bBrowseFirmwareFiles.UseVisualStyleBackColor = true;
            this.bBrowseFirmwareFiles.Click += new System.EventHandler(this.bBrowseFirmwareFiles_Click);
            // 
            // lbFirmwareFileEVprograms
            // 
            this.lbFirmwareFileEVprograms.FormattingEnabled = true;
            this.lbFirmwareFileEVprograms.Location = new System.Drawing.Point(267, 34);
            this.lbFirmwareFileEVprograms.Name = "lbFirmwareFileEVprograms";
            this.lbFirmwareFileEVprograms.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lbFirmwareFileEVprograms.Size = new System.Drawing.Size(267, 69);
            this.lbFirmwareFileEVprograms.TabIndex = 43;
            this.lbFirmwareFileEVprograms.SelectedIndexChanged += new System.EventHandler(this.lbFirmwareFile_SelectedIndexChanged);
            this.lbFirmwareFileEVprograms.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgeqwrg);
            // 
            // lblprogramMode
            // 
            this.lblprogramMode.Location = new System.Drawing.Point(33, 12);
            this.lblprogramMode.Name = "lblprogramMode";
            this.lblprogramMode.Size = new System.Drawing.Size(76, 13);
            this.lblprogramMode.TabIndex = 41;
            this.lblprogramMode.Text = "Program Mode";
            // 
            // pMainFrame
            // 
            this.pMainFrame.BackColor = System.Drawing.SystemColors.Control;
            this.pMainFrame.Controls.Add(this.Laird);
            this.pMainFrame.Controls.Add(this.lblProgressBar);
            this.pMainFrame.Controls.Add(this.progressBar);
            this.pMainFrame.Controls.Add(this.lblESN1);
            this.pMainFrame.Controls.Add(this.lblDUT1);
            this.pMainFrame.Controls.Add(this.lbComPorts1);
            this.pMainFrame.Controls.Add(this.lblDUT1status);
            this.pMainFrame.Controls.Add(this.lblESN2);
            this.pMainFrame.Controls.Add(this.lblDUT2);
            this.pMainFrame.Controls.Add(this.lbComPorts2);
            this.pMainFrame.Controls.Add(this.lblDUT2status);
            this.pMainFrame.Controls.Add(this.lblESN3);
            this.pMainFrame.Controls.Add(this.lblDUT3);
            this.pMainFrame.Controls.Add(this.lbComPorts3);
            this.pMainFrame.Controls.Add(this.lblDUT3status);
            this.pMainFrame.Controls.Add(this.lblESN4);
            this.pMainFrame.Controls.Add(this.lblDUT4);
            this.pMainFrame.Controls.Add(this.lbComPorts4);
            this.pMainFrame.Controls.Add(this.lblDUT4status);
            this.pMainFrame.Controls.Add(this.lblESN5);
            this.pMainFrame.Controls.Add(this.lblDUT5);
            this.pMainFrame.Controls.Add(this.lbComPorts5);
            this.pMainFrame.Controls.Add(this.lblDUT5status);
            this.pMainFrame.Controls.Add(this.lblESN6);
            this.pMainFrame.Controls.Add(this.lblDUT6);
            this.pMainFrame.Controls.Add(this.lbComPorts6);
            this.pMainFrame.Controls.Add(this.lblDUT6status);
            this.pMainFrame.Controls.Add(this.lblfavcommands);
            this.pMainFrame.Controls.Add(this.lbFavCommands);
            this.pMainFrame.Controls.Add(this.lblcommands);
            this.pMainFrame.Controls.Add(this.lbCommands);
            this.pMainFrame.Controls.Add(this.rtbConsole);
            this.pMainFrame.Controls.Add(this.tbConsoleCommands);
            this.pMainFrame.Controls.Add(this.bStart);
            this.pMainFrame.Controls.Add(this.lblDUTS);
            this.pMainFrame.Controls.Add(this.lbDUTnumber);
            this.pMainFrame.Controls.Add(this.lbProgramMode);
            this.pMainFrame.Controls.Add(this.lblprogramMode);
            this.pMainFrame.Controls.Add(this.lblFirmwareEVmode);
            this.pMainFrame.Controls.Add(this.bBrowseFirmwareFiles);
            this.pMainFrame.Controls.Add(this.lbFirmwareFileEVprograms);
            this.pMainFrame.Controls.Add(this.panel2);
            this.pMainFrame.Controls.Add(this.panel1);
            this.pMainFrame.Controls.Add(this.panel3);
            this.pMainFrame.Controls.Add(this.panel4);
            this.pMainFrame.Controls.Add(this.panel5);
            this.pMainFrame.Controls.Add(this.panel6);
            this.pMainFrame.Location = new System.Drawing.Point(4, -1);
            this.pMainFrame.Name = "pMainFrame";
            this.pMainFrame.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pMainFrame.Size = new System.Drawing.Size(892, 660);
            this.pMainFrame.TabIndex = 73;
            this.pMainFrame.Paint += new System.Windows.Forms.PaintEventHandler(this.pMainFrame_Paint);
            // 
            // Laird
            // 
            this.Laird.Image = ((System.Drawing.Image)(resources.GetObject("Laird.Image")));
            this.Laird.Location = new System.Drawing.Point(540, 0);
            this.Laird.Name = "Laird";
            this.Laird.Size = new System.Drawing.Size(170, 72);
            this.Laird.TabIndex = 118;
            // 
            // lblProgressBar
            // 
            this.lblProgressBar.AutoSize = true;
            this.lblProgressBar.Location = new System.Drawing.Point(514, 294);
            this.lblProgressBar.Name = "lblProgressBar";
            this.lblProgressBar.Size = new System.Drawing.Size(21, 13);
            this.lblProgressBar.TabIndex = 117;
            this.lblProgressBar.Text = "0%";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(11, 288);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(491, 25);
            this.progressBar.TabIndex = 116;
            // 
            // lblESN1
            // 
            this.lblESN1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblESN1.Location = new System.Drawing.Point(9, 129);
            this.lblESN1.Name = "lblESN1";
            this.lblESN1.Size = new System.Drawing.Size(83, 28);
            this.lblESN1.TabIndex = 112;
            this.lblESN1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblDUT1
            // 
            this.lblDUT1.AutoSize = true;
            this.lblDUT1.Location = new System.Drawing.Point(27, 114);
            this.lblDUT1.Name = "lblDUT1";
            this.lblDUT1.Size = new System.Drawing.Size(39, 13);
            this.lblDUT1.TabIndex = 115;
            this.lblDUT1.Text = "DUT 1";
            // 
            // lbComPorts1
            // 
            this.lbComPorts1.FormattingEnabled = true;
            this.lbComPorts1.Location = new System.Drawing.Point(21, 160);
            this.lbComPorts1.Name = "lbComPorts1";
            this.lbComPorts1.Size = new System.Drawing.Size(61, 69);
            this.lbComPorts1.TabIndex = 113;
            this.lbComPorts1.SelectedIndexChanged += new System.EventHandler(this.lbComPorts1_SelectedIndexChanged);
            // 
            // lblDUT1status
            // 
            this.lblDUT1status.BackColor = System.Drawing.SystemColors.Control;
            this.lblDUT1status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDUT1status.ForeColor = System.Drawing.Color.Black;
            this.lblDUT1status.Location = new System.Drawing.Point(8, 128);
            this.lblDUT1status.Name = "lblDUT1status";
            this.lblDUT1status.Size = new System.Drawing.Size(87, 108);
            this.lblDUT1status.TabIndex = 114;
            // 
            // lblESN2
            // 
            this.lblESN2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblESN2.Location = new System.Drawing.Point(109, 129);
            this.lblESN2.Name = "lblESN2";
            this.lblESN2.Size = new System.Drawing.Size(82, 28);
            this.lblESN2.TabIndex = 111;
            this.lblESN2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblDUT2
            // 
            this.lblDUT2.AutoSize = true;
            this.lblDUT2.Location = new System.Drawing.Point(124, 114);
            this.lblDUT2.Name = "lblDUT2";
            this.lblDUT2.Size = new System.Drawing.Size(39, 13);
            this.lblDUT2.TabIndex = 110;
            this.lblDUT2.Text = "DUT 2";
            // 
            // lbComPorts2
            // 
            this.lbComPorts2.FormattingEnabled = true;
            this.lbComPorts2.Location = new System.Drawing.Point(118, 160);
            this.lbComPorts2.Name = "lbComPorts2";
            this.lbComPorts2.Size = new System.Drawing.Size(61, 69);
            this.lbComPorts2.TabIndex = 108;
            this.lbComPorts2.SelectedIndexChanged += new System.EventHandler(this.lbComPorts2_SelectedIndexChanged);
            // 
            // lblDUT2status
            // 
            this.lblDUT2status.BackColor = System.Drawing.SystemColors.Control;
            this.lblDUT2status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDUT2status.ForeColor = System.Drawing.Color.Maroon;
            this.lblDUT2status.Location = new System.Drawing.Point(105, 128);
            this.lblDUT2status.Name = "lblDUT2status";
            this.lblDUT2status.Size = new System.Drawing.Size(87, 108);
            this.lblDUT2status.TabIndex = 109;
            // 
            // lblESN3
            // 
            this.lblESN3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblESN3.Location = new System.Drawing.Point(206, 129);
            this.lblESN3.Name = "lblESN3";
            this.lblESN3.Size = new System.Drawing.Size(81, 29);
            this.lblESN3.TabIndex = 107;
            this.lblESN3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblDUT3
            // 
            this.lblDUT3.AutoSize = true;
            this.lblDUT3.Location = new System.Drawing.Point(221, 114);
            this.lblDUT3.Name = "lblDUT3";
            this.lblDUT3.Size = new System.Drawing.Size(39, 13);
            this.lblDUT3.TabIndex = 106;
            this.lblDUT3.Text = "DUT 3";
            // 
            // lbComPorts3
            // 
            this.lbComPorts3.FormattingEnabled = true;
            this.lbComPorts3.Location = new System.Drawing.Point(216, 161);
            this.lbComPorts3.Name = "lbComPorts3";
            this.lbComPorts3.Size = new System.Drawing.Size(61, 69);
            this.lbComPorts3.TabIndex = 104;
            this.lbComPorts3.SelectedIndexChanged += new System.EventHandler(this.lbComPorts3_SelectedIndexChanged);
            // 
            // lblDUT3status
            // 
            this.lblDUT3status.BackColor = System.Drawing.SystemColors.Control;
            this.lblDUT3status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDUT3status.ForeColor = System.Drawing.Color.Maroon;
            this.lblDUT3status.Location = new System.Drawing.Point(202, 128);
            this.lblDUT3status.Name = "lblDUT3status";
            this.lblDUT3status.Size = new System.Drawing.Size(87, 108);
            this.lblDUT3status.TabIndex = 105;
            // 
            // lblESN4
            // 
            this.lblESN4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblESN4.Location = new System.Drawing.Point(303, 129);
            this.lblESN4.Name = "lblESN4";
            this.lblESN4.Size = new System.Drawing.Size(80, 29);
            this.lblESN4.TabIndex = 103;
            this.lblESN4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblDUT4
            // 
            this.lblDUT4.AutoSize = true;
            this.lblDUT4.Location = new System.Drawing.Point(318, 114);
            this.lblDUT4.Name = "lblDUT4";
            this.lblDUT4.Size = new System.Drawing.Size(39, 13);
            this.lblDUT4.TabIndex = 102;
            this.lblDUT4.Text = "DUT 4";
            // 
            // lbComPorts4
            // 
            this.lbComPorts4.FormattingEnabled = true;
            this.lbComPorts4.Location = new System.Drawing.Point(313, 160);
            this.lbComPorts4.Name = "lbComPorts4";
            this.lbComPorts4.Size = new System.Drawing.Size(61, 69);
            this.lbComPorts4.TabIndex = 100;
            this.lbComPorts4.SelectedIndexChanged += new System.EventHandler(this.lbComPorts4_SelectedIndexChanged);
            // 
            // lblDUT4status
            // 
            this.lblDUT4status.BackColor = System.Drawing.SystemColors.Control;
            this.lblDUT4status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDUT4status.ForeColor = System.Drawing.Color.Maroon;
            this.lblDUT4status.Location = new System.Drawing.Point(299, 128);
            this.lblDUT4status.Name = "lblDUT4status";
            this.lblDUT4status.Size = new System.Drawing.Size(87, 108);
            this.lblDUT4status.TabIndex = 101;
            // 
            // lblESN5
            // 
            this.lblESN5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblESN5.Location = new System.Drawing.Point(400, 129);
            this.lblESN5.Name = "lblESN5";
            this.lblESN5.Size = new System.Drawing.Size(82, 29);
            this.lblESN5.TabIndex = 99;
            this.lblESN5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblDUT5
            // 
            this.lblDUT5.AutoSize = true;
            this.lblDUT5.Location = new System.Drawing.Point(415, 114);
            this.lblDUT5.Name = "lblDUT5";
            this.lblDUT5.Size = new System.Drawing.Size(39, 13);
            this.lblDUT5.TabIndex = 98;
            this.lblDUT5.Text = "DUT 5";
            // 
            // lbComPorts5
            // 
            this.lbComPorts5.FormattingEnabled = true;
            this.lbComPorts5.Location = new System.Drawing.Point(409, 160);
            this.lbComPorts5.Name = "lbComPorts5";
            this.lbComPorts5.Size = new System.Drawing.Size(61, 69);
            this.lbComPorts5.TabIndex = 96;
            this.lbComPorts5.SelectedIndexChanged += new System.EventHandler(this.lbComPorts5_SelectedIndexChanged);
            // 
            // lblDUT5status
            // 
            this.lblDUT5status.BackColor = System.Drawing.SystemColors.Control;
            this.lblDUT5status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDUT5status.ForeColor = System.Drawing.Color.Maroon;
            this.lblDUT5status.Location = new System.Drawing.Point(396, 128);
            this.lblDUT5status.Name = "lblDUT5status";
            this.lblDUT5status.Size = new System.Drawing.Size(87, 108);
            this.lblDUT5status.TabIndex = 97;
            // 
            // lblESN6
            // 
            this.lblESN6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblESN6.Location = new System.Drawing.Point(498, 129);
            this.lblESN6.Name = "lblESN6";
            this.lblESN6.Size = new System.Drawing.Size(81, 29);
            this.lblESN6.TabIndex = 95;
            this.lblESN6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblDUT6
            // 
            this.lblDUT6.AutoSize = true;
            this.lblDUT6.Location = new System.Drawing.Point(512, 114);
            this.lblDUT6.Name = "lblDUT6";
            this.lblDUT6.Size = new System.Drawing.Size(39, 13);
            this.lblDUT6.TabIndex = 94;
            this.lblDUT6.Text = "DUT 6";
            // 
            // lbComPorts6
            // 
            this.lbComPorts6.FormattingEnabled = true;
            this.lbComPorts6.Location = new System.Drawing.Point(507, 160);
            this.lbComPorts6.Name = "lbComPorts6";
            this.lbComPorts6.Size = new System.Drawing.Size(61, 69);
            this.lbComPorts6.TabIndex = 92;
            this.lbComPorts6.SelectedIndexChanged += new System.EventHandler(this.lbComPorts6_SelectedIndexChanged);
            // 
            // lblDUT6status
            // 
            this.lblDUT6status.BackColor = System.Drawing.SystemColors.Control;
            this.lblDUT6status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDUT6status.ForeColor = System.Drawing.Color.Maroon;
            this.lblDUT6status.Location = new System.Drawing.Point(493, 128);
            this.lblDUT6status.Name = "lblDUT6status";
            this.lblDUT6status.Size = new System.Drawing.Size(87, 108);
            this.lblDUT6status.TabIndex = 93;
            // 
            // lblfavcommands
            // 
            this.lblfavcommands.AutoSize = true;
            this.lblfavcommands.Location = new System.Drawing.Point(744, 390);
            this.lblfavcommands.Name = "lblfavcommands";
            this.lblfavcommands.Size = new System.Drawing.Size(100, 13);
            this.lblfavcommands.TabIndex = 91;
            this.lblfavcommands.Text = "Favorite Commands";
            // 
            // lbFavCommands
            // 
            this.lbFavCommands.FormattingEnabled = true;
            this.lbFavCommands.Location = new System.Drawing.Point(716, 406);
            this.lbFavCommands.Name = "lbFavCommands";
            this.lbFavCommands.Size = new System.Drawing.Size(173, 212);
            this.lbFavCommands.TabIndex = 90;
            this.lbFavCommands.SelectedIndexChanged += new System.EventHandler(this.lbFavCommands_SelectedIndexChanged);
            // 
            // lblcommands
            // 
            this.lblcommands.AutoSize = true;
            this.lblcommands.Location = new System.Drawing.Point(744, 3);
            this.lblcommands.Name = "lblcommands";
            this.lblcommands.Size = new System.Drawing.Size(59, 13);
            this.lblcommands.TabIndex = 89;
            this.lblcommands.Text = "Commands";
            // 
            // lbCommands
            // 
            this.lbCommands.FormattingEnabled = true;
            this.lbCommands.Location = new System.Drawing.Point(716, 19);
            this.lbCommands.Name = "lbCommands";
            this.lbCommands.Size = new System.Drawing.Size(173, 355);
            this.lbCommands.TabIndex = 88;
            this.lbCommands.SelectedIndexChanged += new System.EventHandler(this.lbCommands_SelectedIndexChanged);
            // 
            // rtbConsole
            // 
            this.rtbConsole.Location = new System.Drawing.Point(11, 324);
            this.rtbConsole.Name = "rtbConsole";
            this.rtbConsole.ReadOnly = true;
            this.rtbConsole.Size = new System.Drawing.Size(662, 306);
            this.rtbConsole.TabIndex = 87;
            this.rtbConsole.Text = "";
            this.rtbConsole.TextChanged += new System.EventHandler(this.rtbConsole_TextChanged);
            // 
            // tbConsoleCommands
            // 
            this.tbConsoleCommands.Location = new System.Drawing.Point(11, 636);
            this.tbConsoleCommands.Name = "tbConsoleCommands";
            this.tbConsoleCommands.Size = new System.Drawing.Size(662, 20);
            this.tbConsoleCommands.TabIndex = 86;
            this.tbConsoleCommands.TextChanged += new System.EventHandler(this.tbConsoleCommands_TextChanged_1);
            this.tbConsoleCommands.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbConsoleCommands_KeyDown);
            // 
            // bStart
            // 
            this.bStart.Location = new System.Drawing.Point(567, 287);
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size(106, 26);
            this.bStart.TabIndex = 85;
            this.bStart.Text = "Start";
            this.bStart.UseVisualStyleBackColor = true;
            this.bStart.Click += new System.EventHandler(this.bStart_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblbaud2);
            this.panel2.Controls.Add(this.rb1152002);
            this.panel2.Controls.Add(this.rb96002);
            this.panel2.Location = new System.Drawing.Point(102, 230);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(88, 58);
            this.panel2.TabIndex = 138;
            // 
            // lblbaud2
            // 
            this.lblbaud2.AutoSize = true;
            this.lblbaud2.Location = new System.Drawing.Point(19, 6);
            this.lblbaud2.Name = "lblbaud2";
            this.lblbaud2.Size = new System.Drawing.Size(53, 13);
            this.lblbaud2.TabIndex = 124;
            this.lblbaud2.Text = "Baud rate";
            this.lblbaud2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // rb1152002
            // 
            this.rb1152002.AutoSize = true;
            this.rb1152002.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rb1152002.Location = new System.Drawing.Point(37, 19);
            this.rb1152002.Name = "rb1152002";
            this.rb1152002.Size = new System.Drawing.Size(47, 30);
            this.rb1152002.TabIndex = 123;
            this.rb1152002.TabStop = true;
            this.rb1152002.Text = "115200";
            this.rb1152002.UseVisualStyleBackColor = true;
            // 
            // rb96002
            // 
            this.rb96002.AutoSize = true;
            this.rb96002.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rb96002.Checked = true;
            this.rb96002.Location = new System.Drawing.Point(3, 19);
            this.rb96002.Name = "rb96002";
            this.rb96002.Size = new System.Drawing.Size(35, 30);
            this.rb96002.TabIndex = 122;
            this.rb96002.TabStop = true;
            this.rb96002.Text = "9600";
            this.rb96002.UseVisualStyleBackColor = true;
            this.rb96002.CheckedChanged += new System.EventHandler(this.rb96002_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblbaud1);
            this.panel1.Controls.Add(this.rb1152001);
            this.panel1.Controls.Add(this.rb96001);
            this.panel1.Location = new System.Drawing.Point(6, 230);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(88, 57);
            this.panel1.TabIndex = 137;
            // 
            // lblbaud1
            // 
            this.lblbaud1.AutoSize = true;
            this.lblbaud1.Location = new System.Drawing.Point(21, 6);
            this.lblbaud1.Name = "lblbaud1";
            this.lblbaud1.Size = new System.Drawing.Size(53, 13);
            this.lblbaud1.TabIndex = 121;
            this.lblbaud1.Text = "Baud rate";
            this.lblbaud1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // rb1152001
            // 
            this.rb1152001.AutoSize = true;
            this.rb1152001.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rb1152001.Location = new System.Drawing.Point(39, 19);
            this.rb1152001.Name = "rb1152001";
            this.rb1152001.Size = new System.Drawing.Size(47, 30);
            this.rb1152001.TabIndex = 120;
            this.rb1152001.TabStop = true;
            this.rb1152001.Text = "115200";
            this.rb1152001.UseVisualStyleBackColor = true;
            this.rb1152001.CheckedChanged += new System.EventHandler(this.rb1152001_CheckedChanged);
            // 
            // rb96001
            // 
            this.rb96001.AutoSize = true;
            this.rb96001.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rb96001.Checked = true;
            this.rb96001.Location = new System.Drawing.Point(5, 19);
            this.rb96001.Name = "rb96001";
            this.rb96001.Size = new System.Drawing.Size(35, 30);
            this.rb96001.TabIndex = 119;
            this.rb96001.TabStop = true;
            this.rb96001.Text = "9600";
            this.rb96001.UseVisualStyleBackColor = true;
            this.rb96001.CheckedChanged += new System.EventHandler(this.rb96001_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblbaud3);
            this.panel3.Controls.Add(this.rb1152003);
            this.panel3.Controls.Add(this.rb96003);
            this.panel3.Location = new System.Drawing.Point(200, 234);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(88, 54);
            this.panel3.TabIndex = 139;
            // 
            // lblbaud3
            // 
            this.lblbaud3.AutoSize = true;
            this.lblbaud3.Location = new System.Drawing.Point(18, 2);
            this.lblbaud3.Name = "lblbaud3";
            this.lblbaud3.Size = new System.Drawing.Size(53, 13);
            this.lblbaud3.TabIndex = 127;
            this.lblbaud3.Text = "Baud rate";
            this.lblbaud3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // rb1152003
            // 
            this.rb1152003.AutoSize = true;
            this.rb1152003.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rb1152003.Location = new System.Drawing.Point(36, 15);
            this.rb1152003.Name = "rb1152003";
            this.rb1152003.Size = new System.Drawing.Size(47, 30);
            this.rb1152003.TabIndex = 126;
            this.rb1152003.TabStop = true;
            this.rb1152003.Text = "115200";
            this.rb1152003.UseVisualStyleBackColor = true;
            // 
            // rb96003
            // 
            this.rb96003.AutoSize = true;
            this.rb96003.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rb96003.Checked = true;
            this.rb96003.Location = new System.Drawing.Point(2, 15);
            this.rb96003.Name = "rb96003";
            this.rb96003.Size = new System.Drawing.Size(35, 30);
            this.rb96003.TabIndex = 125;
            this.rb96003.TabStop = true;
            this.rb96003.Text = "9600";
            this.rb96003.UseVisualStyleBackColor = true;
            this.rb96003.CheckedChanged += new System.EventHandler(this.rb96003_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.lblbaud4);
            this.panel4.Controls.Add(this.rb1152004);
            this.panel4.Controls.Add(this.rb96004);
            this.panel4.Location = new System.Drawing.Point(288, 233);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(105, 55);
            this.panel4.TabIndex = 140;
            // 
            // lblbaud4
            // 
            this.lblbaud4.AutoSize = true;
            this.lblbaud4.Location = new System.Drawing.Point(27, 3);
            this.lblbaud4.Name = "lblbaud4";
            this.lblbaud4.Size = new System.Drawing.Size(53, 13);
            this.lblbaud4.TabIndex = 130;
            this.lblbaud4.Text = "Baud rate";
            this.lblbaud4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // rb1152004
            // 
            this.rb1152004.AutoSize = true;
            this.rb1152004.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rb1152004.Location = new System.Drawing.Point(45, 16);
            this.rb1152004.Name = "rb1152004";
            this.rb1152004.Size = new System.Drawing.Size(47, 30);
            this.rb1152004.TabIndex = 129;
            this.rb1152004.TabStop = true;
            this.rb1152004.Text = "115200";
            this.rb1152004.UseVisualStyleBackColor = true;
            // 
            // rb96004
            // 
            this.rb96004.AutoSize = true;
            this.rb96004.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rb96004.Checked = true;
            this.rb96004.Location = new System.Drawing.Point(11, 16);
            this.rb96004.Name = "rb96004";
            this.rb96004.Size = new System.Drawing.Size(35, 30);
            this.rb96004.TabIndex = 128;
            this.rb96004.TabStop = true;
            this.rb96004.Text = "9600";
            this.rb96004.UseVisualStyleBackColor = true;
            this.rb96004.CheckedChanged += new System.EventHandler(this.rb96004_CheckedChanged);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.lblbaud5);
            this.panel5.Controls.Add(this.rb1152005);
            this.panel5.Controls.Add(this.rb96005);
            this.panel5.Location = new System.Drawing.Point(392, 231);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(95, 55);
            this.panel5.TabIndex = 141;
            // 
            // lblbaud5
            // 
            this.lblbaud5.AutoSize = true;
            this.lblbaud5.Location = new System.Drawing.Point(20, 5);
            this.lblbaud5.Name = "lblbaud5";
            this.lblbaud5.Size = new System.Drawing.Size(53, 13);
            this.lblbaud5.TabIndex = 133;
            this.lblbaud5.Text = "Baud rate";
            this.lblbaud5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // rb1152005
            // 
            this.rb1152005.AutoSize = true;
            this.rb1152005.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rb1152005.Location = new System.Drawing.Point(38, 18);
            this.rb1152005.Name = "rb1152005";
            this.rb1152005.Size = new System.Drawing.Size(47, 30);
            this.rb1152005.TabIndex = 132;
            this.rb1152005.TabStop = true;
            this.rb1152005.Text = "115200";
            this.rb1152005.UseVisualStyleBackColor = true;
            // 
            // rb96005
            // 
            this.rb96005.AutoSize = true;
            this.rb96005.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rb96005.Checked = true;
            this.rb96005.Location = new System.Drawing.Point(4, 18);
            this.rb96005.Name = "rb96005";
            this.rb96005.Size = new System.Drawing.Size(35, 30);
            this.rb96005.TabIndex = 131;
            this.rb96005.TabStop = true;
            this.rb96005.Text = "9600";
            this.rb96005.UseVisualStyleBackColor = true;
            this.rb96005.CheckedChanged += new System.EventHandler(this.rb96005_CheckedChanged);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.lblbaud6);
            this.panel6.Controls.Add(this.rb1152006);
            this.panel6.Controls.Add(this.rb96006);
            this.panel6.Location = new System.Drawing.Point(486, 231);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(101, 57);
            this.panel6.TabIndex = 142;
            // 
            // lblbaud6
            // 
            this.lblbaud6.AutoSize = true;
            this.lblbaud6.Location = new System.Drawing.Point(23, 5);
            this.lblbaud6.Name = "lblbaud6";
            this.lblbaud6.Size = new System.Drawing.Size(53, 13);
            this.lblbaud6.TabIndex = 136;
            this.lblbaud6.Text = "Baud rate";
            this.lblbaud6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // rb1152006
            // 
            this.rb1152006.AutoSize = true;
            this.rb1152006.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rb1152006.Location = new System.Drawing.Point(41, 18);
            this.rb1152006.Name = "rb1152006";
            this.rb1152006.Size = new System.Drawing.Size(47, 30);
            this.rb1152006.TabIndex = 135;
            this.rb1152006.TabStop = true;
            this.rb1152006.Text = "115200";
            this.rb1152006.UseVisualStyleBackColor = true;
            // 
            // rb96006
            // 
            this.rb96006.AutoSize = true;
            this.rb96006.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rb96006.Checked = true;
            this.rb96006.Location = new System.Drawing.Point(7, 18);
            this.rb96006.Name = "rb96006";
            this.rb96006.Size = new System.Drawing.Size(35, 30);
            this.rb96006.TabIndex = 134;
            this.rb96006.TabStop = true;
            this.rb96006.Text = "9600";
            this.rb96006.UseVisualStyleBackColor = true;
            this.rb96006.CheckedChanged += new System.EventHandler(this.rb96006_CheckedChanged);
            // 
            // MainFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(902, 665);
            this.Controls.Add(this.pMainFrame);
            this.Name = "MainFrame";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "WcaProgrammerConsole v5.0.0";
            this.Load += new System.EventHandler(this.MainFrame_Load);
            this.pMainFrame.ResumeLayout(false);
            this.pMainFrame.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblDUTS;
        private System.Windows.Forms.ListBox lbDUTnumber;
        private System.Windows.Forms.ListBox lbProgramMode;
        private System.Windows.Forms.Label lblFirmwareEVmode;
        private System.Windows.Forms.Button bBrowseFirmwareFiles;
        private System.Windows.Forms.ListBox lbFirmwareFileEVprograms;
        private System.Windows.Forms.Label lblprogramMode;
        private System.Windows.Forms.Panel pMainFrame;
        private System.Windows.Forms.Label lblESN1;
        private System.Windows.Forms.Label lblDUT1;
        private System.Windows.Forms.ListBox lbComPorts1;
        private System.Windows.Forms.Label lblDUT1status;
        private System.Windows.Forms.Label lblESN2;
        private System.Windows.Forms.Label lblDUT2;
        private System.Windows.Forms.ListBox lbComPorts2;
        private System.Windows.Forms.Label lblDUT2status;
        private System.Windows.Forms.Label lblESN3;
        private System.Windows.Forms.Label lblDUT3;
        private System.Windows.Forms.ListBox lbComPorts3;
        private System.Windows.Forms.Label lblDUT3status;
        private System.Windows.Forms.Label lblESN4;
        private System.Windows.Forms.Label lblDUT4;
        private System.Windows.Forms.ListBox lbComPorts4;
        private System.Windows.Forms.Label lblDUT4status;
        private System.Windows.Forms.Label lblESN5;
        private System.Windows.Forms.Label lblDUT5;
        private System.Windows.Forms.ListBox lbComPorts5;
        private System.Windows.Forms.Label lblDUT5status;
        private System.Windows.Forms.Label lblESN6;
        private System.Windows.Forms.Label lblDUT6;
        private System.Windows.Forms.ListBox lbComPorts6;
        private System.Windows.Forms.Label lblDUT6status;
        private System.Windows.Forms.Label lblfavcommands;
        private System.Windows.Forms.ListBox lbFavCommands;
        private System.Windows.Forms.Label lblcommands;
        private System.Windows.Forms.ListBox lbCommands;
        private System.Windows.Forms.RichTextBox rtbConsole;
        private System.Windows.Forms.TextBox tbConsoleCommands;
        private System.Windows.Forms.Button bStart;
        private System.Windows.Forms.Label lblProgressBar;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label Laird;
        private System.Windows.Forms.Label lblbaud6;
        private System.Windows.Forms.RadioButton rb1152006;
        private System.Windows.Forms.RadioButton rb96006;
        private System.Windows.Forms.Label lblbaud5;
        private System.Windows.Forms.RadioButton rb1152005;
        private System.Windows.Forms.RadioButton rb96005;
        private System.Windows.Forms.Label lblbaud4;
        private System.Windows.Forms.RadioButton rb1152004;
        private System.Windows.Forms.RadioButton rb96004;
        private System.Windows.Forms.Label lblbaud3;
        private System.Windows.Forms.RadioButton rb1152003;
        private System.Windows.Forms.RadioButton rb96003;
        private System.Windows.Forms.Label lblbaud2;
        private System.Windows.Forms.RadioButton rb1152002;
        private System.Windows.Forms.RadioButton rb96002;
        private System.Windows.Forms.Label lblbaud1;
        private System.Windows.Forms.RadioButton rb1152001;
        private System.Windows.Forms.RadioButton rb96001;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
    }
}