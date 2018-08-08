namespace WcaDVConsole
{
    partial class LEDframeforCustomers
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
            this.tbarRed = new System.Windows.Forms.TrackBar();
            this.tbarGreen = new System.Windows.Forms.TrackBar();
            this.tbarBlue = new System.Windows.Forms.TrackBar();
            this.tbarIntensity = new System.Windows.Forms.TrackBar();
            this.lblred = new System.Windows.Forms.Label();
            this.lblGreen = new System.Windows.Forms.Label();
            this.lblBlue = new System.Windows.Forms.Label();
            this.lblIntensity = new System.Windows.Forms.Label();
            this.tbRed = new System.Windows.Forms.TextBox();
            this.tbGreen = new System.Windows.Forms.TextBox();
            this.tbBlue = new System.Windows.Forms.TextBox();
            this.tbIntensity = new System.Windows.Forms.TextBox();
            this.rbHex = new System.Windows.Forms.RadioButton();
            this.rbPercentage = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.lbComPorts = new System.Windows.Forms.ListBox();
            this.bHelp = new System.Windows.Forms.Button();
            this.lblESN = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.rbCyanOrange = new System.Windows.Forms.RadioButton();
            this.rbCyanRed = new System.Windows.Forms.RadioButton();
            this.rbCyanGreen = new System.Windows.Forms.RadioButton();
            this.rbCyanWhite = new System.Windows.Forms.RadioButton();
            this.rbCyanBlue = new System.Windows.Forms.RadioButton();
            this.rbYellowWhite = new System.Windows.Forms.RadioButton();
            this.rbYellowGreen = new System.Windows.Forms.RadioButton();
            this.rbYellowRed = new System.Windows.Forms.RadioButton();
            this.rbYellowOrange = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbCyanOff = new System.Windows.Forms.RadioButton();
            this.rbYellowOff = new System.Windows.Forms.RadioButton();
            this.cbEnableSliders = new System.Windows.Forms.CheckBox();
            this.gbDefault = new System.Windows.Forms.GroupBox();
            this.lblBaud = new System.Windows.Forms.Label();
            this.lblRedtxt = new System.Windows.Forms.Label();
            this.lblGreentxt = new System.Windows.Forms.Label();
            this.lblBluetxt = new System.Windows.Forms.Label();
            this.lblIntensitytxt = new System.Windows.Forms.Label();
            this.lbUserColors = new System.Windows.Forms.ListBox();
            this.lblUserColors = new System.Windows.Forms.Label();
            this.tbUserColorsName = new System.Windows.Forms.TextBox();
            this.bSaveDelete = new System.Windows.Forms.Button();
            this.lblcolorName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbarRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarBlue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarIntensity)).BeginInit();
            this.panel1.SuspendLayout();
            this.gbDefault.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbarRed
            // 
            this.tbarRed.LargeChange = 655;
            this.tbarRed.Location = new System.Drawing.Point(143, 121);
            this.tbarRed.Maximum = 65535;
            this.tbarRed.Name = "tbarRed";
            this.tbarRed.Size = new System.Drawing.Size(318, 45);
            this.tbarRed.SmallChange = 655;
            this.tbarRed.TabIndex = 0;
            this.tbarRed.TickFrequency = 655;
            this.tbarRed.Scroll += new System.EventHandler(this.tbarRed_Scroll);
            this.tbarRed.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbarRed_MouseDown);
            this.tbarRed.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbarRed_MouseUp);
            // 
            // tbarGreen
            // 
            this.tbarGreen.LargeChange = 655;
            this.tbarGreen.Location = new System.Drawing.Point(143, 172);
            this.tbarGreen.Maximum = 65535;
            this.tbarGreen.Name = "tbarGreen";
            this.tbarGreen.Size = new System.Drawing.Size(318, 45);
            this.tbarGreen.SmallChange = 655;
            this.tbarGreen.TabIndex = 1;
            this.tbarGreen.TickFrequency = 655;
            this.tbarGreen.Scroll += new System.EventHandler(this.tbarGreen_Scroll);
            this.tbarGreen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbarGreen_MouseDown);
            this.tbarGreen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbarGreen_MouseUp);
            // 
            // tbarBlue
            // 
            this.tbarBlue.LargeChange = 655;
            this.tbarBlue.Location = new System.Drawing.Point(143, 223);
            this.tbarBlue.Maximum = 65535;
            this.tbarBlue.Name = "tbarBlue";
            this.tbarBlue.Size = new System.Drawing.Size(318, 45);
            this.tbarBlue.SmallChange = 655;
            this.tbarBlue.TabIndex = 2;
            this.tbarBlue.TickFrequency = 655;
            this.tbarBlue.Scroll += new System.EventHandler(this.tbarBlue_Scroll);
            this.tbarBlue.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbarBlue_MouseDown);
            this.tbarBlue.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbarBlue_MouseUp);
            // 
            // tbarIntensity
            // 
            this.tbarIntensity.Location = new System.Drawing.Point(143, 274);
            this.tbarIntensity.Maximum = 100;
            this.tbarIntensity.Name = "tbarIntensity";
            this.tbarIntensity.Size = new System.Drawing.Size(318, 45);
            this.tbarIntensity.TabIndex = 3;
            this.tbarIntensity.Scroll += new System.EventHandler(this.tbarIntensity_Scroll);
            this.tbarIntensity.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbarIntensity_MouseDown);
            this.tbarIntensity.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbarIntensity_MouseUp);
            // 
            // lblred
            // 
            this.lblred.AutoSize = true;
            this.lblred.Location = new System.Drawing.Point(108, 127);
            this.lblred.Name = "lblred";
            this.lblred.Size = new System.Drawing.Size(27, 13);
            this.lblred.TabIndex = 4;
            this.lblred.Text = "Red";
            // 
            // lblGreen
            // 
            this.lblGreen.AutoSize = true;
            this.lblGreen.Location = new System.Drawing.Point(108, 175);
            this.lblGreen.Name = "lblGreen";
            this.lblGreen.Size = new System.Drawing.Size(36, 13);
            this.lblGreen.TabIndex = 5;
            this.lblGreen.Text = "Green";
            // 
            // lblBlue
            // 
            this.lblBlue.AutoSize = true;
            this.lblBlue.Location = new System.Drawing.Point(107, 230);
            this.lblBlue.Name = "lblBlue";
            this.lblBlue.Size = new System.Drawing.Size(28, 13);
            this.lblBlue.TabIndex = 6;
            this.lblBlue.Text = "Blue";
            // 
            // lblIntensity
            // 
            this.lblIntensity.AutoSize = true;
            this.lblIntensity.Location = new System.Drawing.Point(102, 278);
            this.lblIntensity.Name = "lblIntensity";
            this.lblIntensity.Size = new System.Drawing.Size(46, 13);
            this.lblIntensity.TabIndex = 7;
            this.lblIntensity.Text = "Intensity";
            // 
            // tbRed
            // 
            this.tbRed.Location = new System.Drawing.Point(466, 118);
            this.tbRed.MaxLength = 5;
            this.tbRed.Name = "tbRed";
            this.tbRed.Size = new System.Drawing.Size(47, 20);
            this.tbRed.TabIndex = 8;
            this.tbRed.Text = "0";
            this.tbRed.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbRed_KeyDown);
            // 
            // tbGreen
            // 
            this.tbGreen.Location = new System.Drawing.Point(466, 169);
            this.tbGreen.MaxLength = 5;
            this.tbGreen.Name = "tbGreen";
            this.tbGreen.Size = new System.Drawing.Size(47, 20);
            this.tbGreen.TabIndex = 9;
            this.tbGreen.Text = "0";
            this.tbGreen.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbGreen_KeyDown);
            // 
            // tbBlue
            // 
            this.tbBlue.Location = new System.Drawing.Point(466, 220);
            this.tbBlue.MaxLength = 5;
            this.tbBlue.Name = "tbBlue";
            this.tbBlue.Size = new System.Drawing.Size(47, 20);
            this.tbBlue.TabIndex = 10;
            this.tbBlue.Text = "0";
            this.tbBlue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbBlue_KeyDown);
            // 
            // tbIntensity
            // 
            this.tbIntensity.Location = new System.Drawing.Point(466, 272);
            this.tbIntensity.MaxLength = 3;
            this.tbIntensity.Name = "tbIntensity";
            this.tbIntensity.Size = new System.Drawing.Size(47, 20);
            this.tbIntensity.TabIndex = 11;
            this.tbIntensity.Text = "50";
            this.tbIntensity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbIntensity_KeyDown);
            // 
            // rbHex
            // 
            this.rbHex.AutoSize = true;
            this.rbHex.Location = new System.Drawing.Point(188, 98);
            this.rbHex.Name = "rbHex";
            this.rbHex.Size = new System.Drawing.Size(82, 17);
            this.rbHex.TabIndex = 12;
            this.rbHex.TabStop = true;
            this.rbHex.Text = "Hexidecimal";
            this.rbHex.UseVisualStyleBackColor = true;
            this.rbHex.CheckedChanged += new System.EventHandler(this.rbHex_CheckedChanged);
            // 
            // rbPercentage
            // 
            this.rbPercentage.AutoSize = true;
            this.rbPercentage.Checked = true;
            this.rbPercentage.Location = new System.Drawing.Point(313, 99);
            this.rbPercentage.Name = "rbPercentage";
            this.rbPercentage.Size = new System.Drawing.Size(80, 17);
            this.rbPercentage.TabIndex = 13;
            this.rbPercentage.TabStop = true;
            this.rbPercentage.Text = "Percentage";
            this.rbPercentage.UseVisualStyleBackColor = true;
            this.rbPercentage.CheckedChanged += new System.EventHandler(this.rbPercentage_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Image = global::WcaProgrammerConsole.Properties.Resources.LairdLogo;
            this.label1.Location = new System.Drawing.Point(352, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 78);
            this.label1.TabIndex = 15;
            // 
            // lbComPorts
            // 
            this.lbComPorts.FormattingEnabled = true;
            this.lbComPorts.Location = new System.Drawing.Point(167, 20);
            this.lbComPorts.Name = "lbComPorts";
            this.lbComPorts.Size = new System.Drawing.Size(142, 69);
            this.lbComPorts.TabIndex = 16;
            this.lbComPorts.SelectedIndexChanged += new System.EventHandler(this.lbComPorts_SelectedIndexChanged);
            // 
            // bHelp
            // 
            this.bHelp.Location = new System.Drawing.Point(24, 31);
            this.bHelp.Name = "bHelp";
            this.bHelp.Size = new System.Drawing.Size(94, 37);
            this.bHelp.TabIndex = 18;
            this.bHelp.Text = "Help";
            this.bHelp.UseVisualStyleBackColor = true;
            this.bHelp.Click += new System.EventHandler(this.bHelp_Click);
            // 
            // lblESN
            // 
            this.lblESN.Location = new System.Drawing.Point(24, 71);
            this.lblESN.Name = "lblESN";
            this.lblESN.Size = new System.Drawing.Size(128, 27);
            this.lblESN.TabIndex = 19;
            this.lblESN.Text = "Not Connected";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(218, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "ComPorts";
            // 
            // rbCyanOrange
            // 
            this.rbCyanOrange.AutoSize = true;
            this.rbCyanOrange.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rbCyanOrange.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbCyanOrange.Location = new System.Drawing.Point(228, 5);
            this.rbCyanOrange.Name = "rbCyanOrange";
            this.rbCyanOrange.Size = new System.Drawing.Size(65, 43);
            this.rbCyanOrange.TabIndex = 21;
            this.rbCyanOrange.TabStop = true;
            this.rbCyanOrange.Text = "Orange\r\n(Protection)";
            this.rbCyanOrange.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbCyanOrange.UseVisualStyleBackColor = true;
            this.rbCyanOrange.Visible = false;
            this.rbCyanOrange.CheckedChanged += new System.EventHandler(this.rbCyanOrange_CheckedChanged);
            // 
            // rbCyanRed
            // 
            this.rbCyanRed.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rbCyanRed.Location = new System.Drawing.Point(299, 4);
            this.rbCyanRed.Name = "rbCyanRed";
            this.rbCyanRed.Size = new System.Drawing.Size(51, 44);
            this.rbCyanRed.TabIndex = 22;
            this.rbCyanRed.TabStop = true;
            this.rbCyanRed.Text = "Red\r\n(Defect)";
            this.rbCyanRed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbCyanRed.UseVisualStyleBackColor = true;
            this.rbCyanRed.Visible = false;
            this.rbCyanRed.CheckedChanged += new System.EventHandler(this.rbCyanRed_CheckedChanged);
            // 
            // rbCyanGreen
            // 
            this.rbCyanGreen.AutoSize = true;
            this.rbCyanGreen.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rbCyanGreen.Location = new System.Drawing.Point(84, 6);
            this.rbCyanGreen.Name = "rbCyanGreen";
            this.rbCyanGreen.Size = new System.Drawing.Size(81, 43);
            this.rbCyanGreen.TabIndex = 23;
            this.rbCyanGreen.TabStop = true;
            this.rbCyanGreen.Text = "Green\r\n(Fully Charged)";
            this.rbCyanGreen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbCyanGreen.UseVisualStyleBackColor = true;
            this.rbCyanGreen.Visible = false;
            this.rbCyanGreen.CheckedChanged += new System.EventHandler(this.rbCyanGreen_CheckedChanged);
            // 
            // rbCyanWhite
            // 
            this.rbCyanWhite.AutoSize = true;
            this.rbCyanWhite.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rbCyanWhite.Location = new System.Drawing.Point(0, 6);
            this.rbCyanWhite.Name = "rbCyanWhite";
            this.rbCyanWhite.Size = new System.Drawing.Size(88, 43);
            this.rbCyanWhite.TabIndex = 24;
            this.rbCyanWhite.TabStop = true;
            this.rbCyanWhite.Text = "White\r\n(Device Search)";
            this.rbCyanWhite.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbCyanWhite.UseVisualStyleBackColor = true;
            this.rbCyanWhite.Visible = false;
            this.rbCyanWhite.CheckedChanged += new System.EventHandler(this.rbCyanWhite_CheckedChanged);
            // 
            // rbCyanBlue
            // 
            this.rbCyanBlue.AutoSize = true;
            this.rbCyanBlue.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rbCyanBlue.Location = new System.Drawing.Point(163, 6);
            this.rbCyanBlue.Name = "rbCyanBlue";
            this.rbCyanBlue.Size = new System.Drawing.Size(59, 43);
            this.rbCyanBlue.TabIndex = 25;
            this.rbCyanBlue.TabStop = true;
            this.rbCyanBlue.Text = "Blue\r\n(Charging)";
            this.rbCyanBlue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbCyanBlue.UseVisualStyleBackColor = true;
            this.rbCyanBlue.Visible = false;
            this.rbCyanBlue.CheckedChanged += new System.EventHandler(this.rbCyanBlue_CheckedChanged);
            // 
            // rbYellowWhite
            // 
            this.rbYellowWhite.AutoSize = true;
            this.rbYellowWhite.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rbYellowWhite.Location = new System.Drawing.Point(21, 6);
            this.rbYellowWhite.Name = "rbYellowWhite";
            this.rbYellowWhite.Size = new System.Drawing.Size(88, 43);
            this.rbYellowWhite.TabIndex = 29;
            this.rbYellowWhite.TabStop = true;
            this.rbYellowWhite.Text = "White\r\n(Device Search)";
            this.rbYellowWhite.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rbYellowWhite.UseVisualStyleBackColor = true;
            this.rbYellowWhite.Visible = false;
            this.rbYellowWhite.CheckedChanged += new System.EventHandler(this.rbYellowWhite_CheckedChanged);
            // 
            // rbYellowGreen
            // 
            this.rbYellowGreen.AutoSize = true;
            this.rbYellowGreen.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rbYellowGreen.Location = new System.Drawing.Point(115, 6);
            this.rbYellowGreen.Name = "rbYellowGreen";
            this.rbYellowGreen.Size = new System.Drawing.Size(59, 43);
            this.rbYellowGreen.TabIndex = 28;
            this.rbYellowGreen.TabStop = true;
            this.rbYellowGreen.Text = "Green\r\n(Charging)";
            this.rbYellowGreen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbYellowGreen.UseVisualStyleBackColor = true;
            this.rbYellowGreen.Visible = false;
            this.rbYellowGreen.CheckedChanged += new System.EventHandler(this.rbYellowGreen_CheckedChanged);
            // 
            // rbYellowRed
            // 
            this.rbYellowRed.AutoSize = true;
            this.rbYellowRed.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rbYellowRed.Location = new System.Drawing.Point(273, 6);
            this.rbYellowRed.Name = "rbYellowRed";
            this.rbYellowRed.Size = new System.Drawing.Size(49, 43);
            this.rbYellowRed.TabIndex = 27;
            this.rbYellowRed.TabStop = true;
            this.rbYellowRed.Text = "Red\r\n(Defect)";
            this.rbYellowRed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbYellowRed.UseVisualStyleBackColor = true;
            this.rbYellowRed.Visible = false;
            this.rbYellowRed.CheckedChanged += new System.EventHandler(this.rbYellowRed_CheckedChanged);
            // 
            // rbYellowOrange
            // 
            this.rbYellowOrange.AutoSize = true;
            this.rbYellowOrange.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rbYellowOrange.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbYellowOrange.Location = new System.Drawing.Point(191, 6);
            this.rbYellowOrange.Name = "rbYellowOrange";
            this.rbYellowOrange.Size = new System.Drawing.Size(65, 43);
            this.rbYellowOrange.TabIndex = 26;
            this.rbYellowOrange.TabStop = true;
            this.rbYellowOrange.Text = "Orange\r\n(Protection)";
            this.rbYellowOrange.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rbYellowOrange.UseVisualStyleBackColor = true;
            this.rbYellowOrange.Visible = false;
            this.rbYellowOrange.CheckedChanged += new System.EventHandler(this.rbYellowOrange_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbCyanWhite);
            this.panel1.Controls.Add(this.rbCyanGreen);
            this.panel1.Controls.Add(this.rbCyanBlue);
            this.panel1.Controls.Add(this.rbCyanOrange);
            this.panel1.Controls.Add(this.rbCyanRed);
            this.panel1.Controls.Add(this.rbCyanOff);
            this.panel1.Controls.Add(this.rbYellowOff);
            this.panel1.Controls.Add(this.rbYellowGreen);
            this.panel1.Controls.Add(this.rbYellowRed);
            this.panel1.Controls.Add(this.rbYellowOrange);
            this.panel1.Controls.Add(this.rbYellowWhite);
            this.panel1.Location = new System.Drawing.Point(6, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(414, 59);
            this.panel1.TabIndex = 30;
            // 
            // rbCyanOff
            // 
            this.rbCyanOff.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rbCyanOff.Location = new System.Drawing.Point(355, 4);
            this.rbCyanOff.Name = "rbCyanOff";
            this.rbCyanOff.Size = new System.Drawing.Size(55, 44);
            this.rbCyanOff.TabIndex = 30;
            this.rbCyanOff.TabStop = true;
            this.rbCyanOff.Text = "Off\r\n(ACC Off)";
            this.rbCyanOff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbCyanOff.UseVisualStyleBackColor = true;
            this.rbCyanOff.Visible = false;
            this.rbCyanOff.CheckedChanged += new System.EventHandler(this.rbCyanOff_CheckedChanged);
            // 
            // rbYellowOff
            // 
            this.rbYellowOff.AutoSize = true;
            this.rbYellowOff.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rbYellowOff.Location = new System.Drawing.Point(337, 6);
            this.rbYellowOff.Name = "rbYellowOff";
            this.rbYellowOff.Size = new System.Drawing.Size(55, 43);
            this.rbYellowOff.TabIndex = 31;
            this.rbYellowOff.TabStop = true;
            this.rbYellowOff.Text = "Off\r\n(ACC Off)";
            this.rbYellowOff.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rbYellowOff.UseVisualStyleBackColor = true;
            this.rbYellowOff.Visible = false;
            this.rbYellowOff.CheckedChanged += new System.EventHandler(this.rbYellowOff_CheckedChanged);
            // 
            // cbEnableSliders
            // 
            this.cbEnableSliders.AutoSize = true;
            this.cbEnableSliders.Location = new System.Drawing.Point(8, 6);
            this.cbEnableSliders.Name = "cbEnableSliders";
            this.cbEnableSliders.Size = new System.Drawing.Size(120, 17);
            this.cbEnableSliders.TabIndex = 31;
            this.cbEnableSliders.Text = "Enable Color Sliders";
            this.cbEnableSliders.UseVisualStyleBackColor = true;
            this.cbEnableSliders.CheckedChanged += new System.EventHandler(this.cbEnableSliders_CheckedChanged);
            // 
            // gbDefault
            // 
            this.gbDefault.Controls.Add(this.panel1);
            this.gbDefault.Location = new System.Drawing.Point(103, 322);
            this.gbDefault.Name = "gbDefault";
            this.gbDefault.Size = new System.Drawing.Size(426, 78);
            this.gbDefault.TabIndex = 32;
            this.gbDefault.TabStop = false;
            this.gbDefault.Text = "Default Colors (LED States)";
            // 
            // lblBaud
            // 
            this.lblBaud.AutoSize = true;
            this.lblBaud.Location = new System.Drawing.Point(9, 390);
            this.lblBaud.Name = "lblBaud";
            this.lblBaud.Size = new System.Drawing.Size(0, 13);
            this.lblBaud.TabIndex = 33;
            // 
            // lblRedtxt
            // 
            this.lblRedtxt.Location = new System.Drawing.Point(463, 141);
            this.lblRedtxt.Name = "lblRedtxt";
            this.lblRedtxt.Size = new System.Drawing.Size(67, 25);
            this.lblRedtxt.TabIndex = 37;
            // 
            // lblGreentxt
            // 
            this.lblGreentxt.Location = new System.Drawing.Point(463, 192);
            this.lblGreentxt.Name = "lblGreentxt";
            this.lblGreentxt.Size = new System.Drawing.Size(67, 25);
            this.lblGreentxt.TabIndex = 38;
            // 
            // lblBluetxt
            // 
            this.lblBluetxt.Location = new System.Drawing.Point(463, 244);
            this.lblBluetxt.Name = "lblBluetxt";
            this.lblBluetxt.Size = new System.Drawing.Size(67, 25);
            this.lblBluetxt.TabIndex = 39;
            // 
            // lblIntensitytxt
            // 
            this.lblIntensitytxt.Location = new System.Drawing.Point(463, 295);
            this.lblIntensitytxt.Name = "lblIntensitytxt";
            this.lblIntensitytxt.Size = new System.Drawing.Size(67, 25);
            this.lblIntensitytxt.TabIndex = 40;
            // 
            // lbUserColors
            // 
            this.lbUserColors.FormattingEnabled = true;
            this.lbUserColors.Location = new System.Drawing.Point(7, 119);
            this.lbUserColors.Name = "lbUserColors";
            this.lbUserColors.Size = new System.Drawing.Size(90, 212);
            this.lbUserColors.TabIndex = 41;
            this.lbUserColors.SelectedIndexChanged += new System.EventHandler(this.lbUserColors_SelectedIndexChanged);
            // 
            // lblUserColors
            // 
            this.lblUserColors.AutoSize = true;
            this.lblUserColors.Location = new System.Drawing.Point(5, 102);
            this.lblUserColors.Name = "lblUserColors";
            this.lblUserColors.Size = new System.Drawing.Size(61, 13);
            this.lblUserColors.TabIndex = 42;
            this.lblUserColors.Text = "User Colors";
            this.lblUserColors.Click += new System.EventHandler(this.label2_Click);
            // 
            // tbUserColorsName
            // 
            this.tbUserColorsName.Location = new System.Drawing.Point(6, 337);
            this.tbUserColorsName.MaxLength = 15;
            this.tbUserColorsName.Name = "tbUserColorsName";
            this.tbUserColorsName.Size = new System.Drawing.Size(91, 20);
            this.tbUserColorsName.TabIndex = 43;
            this.tbUserColorsName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbUserColorsName_KeyUp);
            // 
            // bSaveDelete
            // 
            this.bSaveDelete.Location = new System.Drawing.Point(7, 380);
            this.bSaveDelete.Name = "bSaveDelete";
            this.bSaveDelete.Size = new System.Drawing.Size(91, 20);
            this.bSaveDelete.TabIndex = 44;
            this.bSaveDelete.Text = "Save";
            this.bSaveDelete.UseVisualStyleBackColor = true;
            this.bSaveDelete.Click += new System.EventHandler(this.bSaveDelete_Click);
            // 
            // lblcolorName
            // 
            this.lblcolorName.AutoSize = true;
            this.lblcolorName.Location = new System.Drawing.Point(9, 362);
            this.lblcolorName.Name = "lblcolorName";
            this.lblcolorName.Size = new System.Drawing.Size(0, 13);
            this.lblcolorName.TabIndex = 45;
            // 
            // LEDframeforCustomers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(534, 412);
            this.Controls.Add(this.lblcolorName);
            this.Controls.Add(this.bSaveDelete);
            this.Controls.Add(this.tbUserColorsName);
            this.Controls.Add(this.lblUserColors);
            this.Controls.Add(this.lbUserColors);
            this.Controls.Add(this.lblIntensitytxt);
            this.Controls.Add(this.lblBluetxt);
            this.Controls.Add(this.lblGreentxt);
            this.Controls.Add(this.lblRedtxt);
            this.Controls.Add(this.lblBaud);
            this.Controls.Add(this.gbDefault);
            this.Controls.Add(this.cbEnableSliders);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblESN);
            this.Controls.Add(this.bHelp);
            this.Controls.Add(this.lbComPorts);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbPercentage);
            this.Controls.Add(this.rbHex);
            this.Controls.Add(this.tbIntensity);
            this.Controls.Add(this.tbBlue);
            this.Controls.Add(this.tbGreen);
            this.Controls.Add(this.tbRed);
            this.Controls.Add(this.lblIntensity);
            this.Controls.Add(this.lblBlue);
            this.Controls.Add(this.lblGreen);
            this.Controls.Add(this.lblred);
            this.Controls.Add(this.tbarIntensity);
            this.Controls.Add(this.tbarBlue);
            this.Controls.Add(this.tbarGreen);
            this.Controls.Add(this.tbarRed);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LEDframeforCustomers";
            this.Text = "Mazda Change LED Color v2.1";
            this.Load += new System.EventHandler(this.LEDframeforCustomers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbarRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarBlue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarIntensity)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbDefault.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar tbarRed;
        private System.Windows.Forms.TrackBar tbarGreen;
        private System.Windows.Forms.TrackBar tbarBlue;
        private System.Windows.Forms.TrackBar tbarIntensity;
        private System.Windows.Forms.Label lblred;
        private System.Windows.Forms.Label lblGreen;
        private System.Windows.Forms.Label lblBlue;
        private System.Windows.Forms.Label lblIntensity;
        private System.Windows.Forms.TextBox tbRed;
        private System.Windows.Forms.TextBox tbGreen;
        private System.Windows.Forms.TextBox tbBlue;
        private System.Windows.Forms.TextBox tbIntensity;
        private System.Windows.Forms.RadioButton rbHex;
        private System.Windows.Forms.RadioButton rbPercentage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbComPorts;
        private System.Windows.Forms.Button bHelp;
        private System.Windows.Forms.Label lblESN;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rbCyanOrange;
        private System.Windows.Forms.RadioButton rbCyanRed;
        private System.Windows.Forms.RadioButton rbCyanGreen;
        private System.Windows.Forms.RadioButton rbCyanWhite;
        private System.Windows.Forms.RadioButton rbCyanBlue;
        private System.Windows.Forms.RadioButton rbYellowWhite;
        private System.Windows.Forms.RadioButton rbYellowGreen;
        private System.Windows.Forms.RadioButton rbYellowRed;
        private System.Windows.Forms.RadioButton rbYellowOrange;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox cbEnableSliders;
        private System.Windows.Forms.RadioButton rbCyanOff;
        private System.Windows.Forms.GroupBox gbDefault;
        private System.Windows.Forms.RadioButton rbYellowOff;
        private System.Windows.Forms.Label lblBaud;
        private System.Windows.Forms.Label lblRedtxt;
        private System.Windows.Forms.Label lblGreentxt;
        private System.Windows.Forms.Label lblBluetxt;
        private System.Windows.Forms.Label lblIntensitytxt;
        private System.Windows.Forms.ListBox lbUserColors;
        private System.Windows.Forms.Label lblUserColors;
        private System.Windows.Forms.TextBox tbUserColorsName;
        private System.Windows.Forms.Button bSaveDelete;
        private System.Windows.Forms.Label lblcolorName;
    }
}