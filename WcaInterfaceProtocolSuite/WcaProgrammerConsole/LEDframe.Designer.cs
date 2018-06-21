namespace WcaDVConsole
{
    partial class LEDframe
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
            this.label = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbRed = new System.Windows.Forms.TextBox();
            this.tbGreen = new System.Windows.Forms.TextBox();
            this.tbBlue = new System.Windows.Forms.TextBox();
            this.tbIntensity = new System.Windows.Forms.TextBox();
            this.rbHex = new System.Windows.Forms.RadioButton();
            this.rbPercentage = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbYellowOff = new System.Windows.Forms.RadioButton();
            this.rbYellowWhite = new System.Windows.Forms.RadioButton();
            this.rbCyanOff = new System.Windows.Forms.RadioButton();
            this.rbYellowGreen = new System.Windows.Forms.RadioButton();
            this.rbYellowRed = new System.Windows.Forms.RadioButton();
            this.rbYellowOrange = new System.Windows.Forms.RadioButton();
            this.rbCyanBlue = new System.Windows.Forms.RadioButton();
            this.rbCyanWhite = new System.Windows.Forms.RadioButton();
            this.rbCyanGreen = new System.Windows.Forms.RadioButton();
            this.rbCyanRed = new System.Windows.Forms.RadioButton();
            this.rbCyanOrange = new System.Windows.Forms.RadioButton();
            this.pCyan_Yellow = new System.Windows.Forms.Panel();
            this.rbYellow = new System.Windows.Forms.RadioButton();
            this.rbCyan = new System.Windows.Forms.RadioButton();
            this.lblRedtxt = new System.Windows.Forms.Label();
            this.lblGreentxt = new System.Windows.Forms.Label();
            this.lblBluetxt = new System.Windows.Forms.Label();
            this.lblIntensitytxt = new System.Windows.Forms.Label();
            this.lblDUT = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbarRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarBlue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarIntensity)).BeginInit();
            this.panel1.SuspendLayout();
            this.pCyan_Yellow.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbarRed
            // 
            this.tbarRed.Location = new System.Drawing.Point(47, 53);
            this.tbarRed.Maximum = 65535;
            this.tbarRed.Name = "tbarRed";
            this.tbarRed.Size = new System.Drawing.Size(282, 45);
            this.tbarRed.TabIndex = 0;
            this.tbarRed.Scroll += new System.EventHandler(this.tbarRed_Scroll);
            this.tbarRed.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbarRed_MouseUp);
            // 
            // tbarGreen
            // 
            this.tbarGreen.Location = new System.Drawing.Point(47, 104);
            this.tbarGreen.Maximum = 65535;
            this.tbarGreen.Name = "tbarGreen";
            this.tbarGreen.Size = new System.Drawing.Size(282, 45);
            this.tbarGreen.TabIndex = 1;
            this.tbarGreen.Scroll += new System.EventHandler(this.tbarGreen_Scroll);
            this.tbarGreen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbarGreen_MouseUp);
            // 
            // tbarBlue
            // 
            this.tbarBlue.Location = new System.Drawing.Point(47, 155);
            this.tbarBlue.Maximum = 65535;
            this.tbarBlue.Name = "tbarBlue";
            this.tbarBlue.Size = new System.Drawing.Size(282, 45);
            this.tbarBlue.TabIndex = 2;
            this.tbarBlue.Scroll += new System.EventHandler(this.tbarBlue_Scroll);
            this.tbarBlue.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbarBlue_MouseUp);
            // 
            // tbarIntensity
            // 
            this.tbarIntensity.Location = new System.Drawing.Point(47, 206);
            this.tbarIntensity.Maximum = 100;
            this.tbarIntensity.Name = "tbarIntensity";
            this.tbarIntensity.Size = new System.Drawing.Size(282, 45);
            this.tbarIntensity.TabIndex = 3;
            this.tbarIntensity.Scroll += new System.EventHandler(this.tbarIntensity_Scroll);
            this.tbarIntensity.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbarIntensity_MouseUp);
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(12, 59);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(27, 13);
            this.label.TabIndex = 4;
            this.label.Text = "Red";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Green";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 162);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Blue";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 210);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Intensity";
            // 
            // tbRed
            // 
            this.tbRed.Location = new System.Drawing.Point(331, 53);
            this.tbRed.Name = "tbRed";
            this.tbRed.Size = new System.Drawing.Size(47, 20);
            this.tbRed.TabIndex = 8;
            this.tbRed.Text = "0";
            this.tbRed.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbRed_KeyDown);
            // 
            // tbGreen
            // 
            this.tbGreen.Location = new System.Drawing.Point(331, 104);
            this.tbGreen.Name = "tbGreen";
            this.tbGreen.Size = new System.Drawing.Size(47, 20);
            this.tbGreen.TabIndex = 9;
            this.tbGreen.Text = "0";
            this.tbGreen.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbGreen_KeyDown);
            // 
            // tbBlue
            // 
            this.tbBlue.Location = new System.Drawing.Point(331, 155);
            this.tbBlue.Name = "tbBlue";
            this.tbBlue.Size = new System.Drawing.Size(47, 20);
            this.tbBlue.TabIndex = 10;
            this.tbBlue.Text = "0";
            this.tbBlue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbBlue_KeyDown);
            // 
            // tbIntensity
            // 
            this.tbIntensity.Location = new System.Drawing.Point(331, 207);
            this.tbIntensity.Name = "tbIntensity";
            this.tbIntensity.Size = new System.Drawing.Size(47, 20);
            this.tbIntensity.TabIndex = 11;
            this.tbIntensity.Text = "50";
            this.tbIntensity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbIntensity_KeyDown);
            // 
            // rbHex
            // 
            this.rbHex.AutoSize = true;
            this.rbHex.Location = new System.Drawing.Point(92, 30);
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
            this.rbPercentage.Location = new System.Drawing.Point(217, 31);
            this.rbPercentage.Name = "rbPercentage";
            this.rbPercentage.Size = new System.Drawing.Size(80, 17);
            this.rbPercentage.TabIndex = 13;
            this.rbPercentage.TabStop = true;
            this.rbPercentage.Text = "Percentage";
            this.rbPercentage.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbYellowOff);
            this.panel1.Controls.Add(this.rbYellowWhite);
            this.panel1.Controls.Add(this.rbCyanOff);
            this.panel1.Controls.Add(this.rbYellowGreen);
            this.panel1.Controls.Add(this.rbYellowRed);
            this.panel1.Controls.Add(this.rbYellowOrange);
            this.panel1.Controls.Add(this.rbCyanBlue);
            this.panel1.Controls.Add(this.rbCyanWhite);
            this.panel1.Controls.Add(this.rbCyanGreen);
            this.panel1.Controls.Add(this.rbCyanRed);
            this.panel1.Controls.Add(this.rbCyanOrange);
            this.panel1.Location = new System.Drawing.Point(1, 248);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(398, 45);
            this.panel1.TabIndex = 31;
            // 
            // rbYellowOff
            // 
            this.rbYellowOff.AutoSize = true;
            this.rbYellowOff.Location = new System.Drawing.Point(316, 25);
            this.rbYellowOff.Name = "rbYellowOff";
            this.rbYellowOff.Size = new System.Drawing.Size(39, 17);
            this.rbYellowOff.TabIndex = 37;
            this.rbYellowOff.TabStop = true;
            this.rbYellowOff.Text = "Off";
            this.rbYellowOff.UseVisualStyleBackColor = true;
            this.rbYellowOff.Visible = false;
            this.rbYellowOff.CheckedChanged += new System.EventHandler(this.rbYellowOff_CheckedChanged);
            // 
            // rbYellowWhite
            // 
            this.rbYellowWhite.AutoSize = true;
            this.rbYellowWhite.Location = new System.Drawing.Point(32, 25);
            this.rbYellowWhite.Name = "rbYellowWhite";
            this.rbYellowWhite.Size = new System.Drawing.Size(53, 17);
            this.rbYellowWhite.TabIndex = 29;
            this.rbYellowWhite.TabStop = true;
            this.rbYellowWhite.Text = "White";
            this.rbYellowWhite.UseVisualStyleBackColor = true;
            this.rbYellowWhite.Visible = false;
            this.rbYellowWhite.CheckedChanged += new System.EventHandler(this.rbYellowWhite_CheckedChanged);
            // 
            // rbCyanOff
            // 
            this.rbCyanOff.AutoSize = true;
            this.rbCyanOff.Location = new System.Drawing.Point(348, 4);
            this.rbCyanOff.Name = "rbCyanOff";
            this.rbCyanOff.Size = new System.Drawing.Size(39, 17);
            this.rbCyanOff.TabIndex = 30;
            this.rbCyanOff.TabStop = true;
            this.rbCyanOff.Text = "Off";
            this.rbCyanOff.UseVisualStyleBackColor = true;
            this.rbCyanOff.Visible = false;
            this.rbCyanOff.CheckedChanged += new System.EventHandler(this.rbCyanOff_CheckedChanged);
            // 
            // rbYellowGreen
            // 
            this.rbYellowGreen.AutoSize = true;
            this.rbYellowGreen.Location = new System.Drawing.Point(102, 25);
            this.rbYellowGreen.Name = "rbYellowGreen";
            this.rbYellowGreen.Size = new System.Drawing.Size(54, 17);
            this.rbYellowGreen.TabIndex = 28;
            this.rbYellowGreen.TabStop = true;
            this.rbYellowGreen.Text = "Green";
            this.rbYellowGreen.UseVisualStyleBackColor = true;
            this.rbYellowGreen.Visible = false;
            this.rbYellowGreen.CheckedChanged += new System.EventHandler(this.rbYellowGreen_CheckedChanged);
            // 
            // rbYellowRed
            // 
            this.rbYellowRed.AutoSize = true;
            this.rbYellowRed.Location = new System.Drawing.Point(251, 25);
            this.rbYellowRed.Name = "rbYellowRed";
            this.rbYellowRed.Size = new System.Drawing.Size(45, 17);
            this.rbYellowRed.TabIndex = 27;
            this.rbYellowRed.TabStop = true;
            this.rbYellowRed.Text = "Red";
            this.rbYellowRed.UseVisualStyleBackColor = true;
            this.rbYellowRed.Visible = false;
            this.rbYellowRed.CheckedChanged += new System.EventHandler(this.rbYellowRed_CheckedChanged);
            // 
            // rbYellowOrange
            // 
            this.rbYellowOrange.AutoSize = true;
            this.rbYellowOrange.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbYellowOrange.Location = new System.Drawing.Point(179, 25);
            this.rbYellowOrange.Name = "rbYellowOrange";
            this.rbYellowOrange.Size = new System.Drawing.Size(60, 17);
            this.rbYellowOrange.TabIndex = 26;
            this.rbYellowOrange.TabStop = true;
            this.rbYellowOrange.Text = "Orange";
            this.rbYellowOrange.UseVisualStyleBackColor = true;
            this.rbYellowOrange.Visible = false;
            this.rbYellowOrange.CheckedChanged += new System.EventHandler(this.rbYellowOrange_CheckedChanged);
            // 
            // rbCyanBlue
            // 
            this.rbCyanBlue.AutoSize = true;
            this.rbCyanBlue.Location = new System.Drawing.Point(142, 5);
            this.rbCyanBlue.Name = "rbCyanBlue";
            this.rbCyanBlue.Size = new System.Drawing.Size(46, 17);
            this.rbCyanBlue.TabIndex = 25;
            this.rbCyanBlue.TabStop = true;
            this.rbCyanBlue.Text = "Blue";
            this.rbCyanBlue.UseVisualStyleBackColor = true;
            this.rbCyanBlue.Visible = false;
            this.rbCyanBlue.CheckedChanged += new System.EventHandler(this.rbCyanBlue_CheckedChanged);
            // 
            // rbCyanWhite
            // 
            this.rbCyanWhite.AutoSize = true;
            this.rbCyanWhite.Location = new System.Drawing.Point(7, 3);
            this.rbCyanWhite.Name = "rbCyanWhite";
            this.rbCyanWhite.Size = new System.Drawing.Size(53, 17);
            this.rbCyanWhite.TabIndex = 24;
            this.rbCyanWhite.TabStop = true;
            this.rbCyanWhite.Text = "White";
            this.rbCyanWhite.UseVisualStyleBackColor = true;
            this.rbCyanWhite.Visible = false;
            this.rbCyanWhite.CheckedChanged += new System.EventHandler(this.rbCyanWhite_CheckedChanged);
            // 
            // rbCyanGreen
            // 
            this.rbCyanGreen.AutoSize = true;
            this.rbCyanGreen.Location = new System.Drawing.Point(77, 4);
            this.rbCyanGreen.Name = "rbCyanGreen";
            this.rbCyanGreen.Size = new System.Drawing.Size(54, 17);
            this.rbCyanGreen.TabIndex = 23;
            this.rbCyanGreen.TabStop = true;
            this.rbCyanGreen.Text = "Green";
            this.rbCyanGreen.UseVisualStyleBackColor = true;
            this.rbCyanGreen.Visible = false;
            this.rbCyanGreen.CheckedChanged += new System.EventHandler(this.rbCyanGreen_CheckedChanged);
            // 
            // rbCyanRed
            // 
            this.rbCyanRed.AutoSize = true;
            this.rbCyanRed.Location = new System.Drawing.Point(282, 4);
            this.rbCyanRed.Name = "rbCyanRed";
            this.rbCyanRed.Size = new System.Drawing.Size(45, 17);
            this.rbCyanRed.TabIndex = 22;
            this.rbCyanRed.TabStop = true;
            this.rbCyanRed.Text = "Red";
            this.rbCyanRed.UseVisualStyleBackColor = true;
            this.rbCyanRed.Visible = false;
            this.rbCyanRed.CheckedChanged += new System.EventHandler(this.rbCyanRed_CheckedChanged);
            // 
            // rbCyanOrange
            // 
            this.rbCyanOrange.AutoSize = true;
            this.rbCyanOrange.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbCyanOrange.Location = new System.Drawing.Point(206, 4);
            this.rbCyanOrange.Name = "rbCyanOrange";
            this.rbCyanOrange.Size = new System.Drawing.Size(60, 17);
            this.rbCyanOrange.TabIndex = 21;
            this.rbCyanOrange.TabStop = true;
            this.rbCyanOrange.Text = "Orange";
            this.rbCyanOrange.UseVisualStyleBackColor = true;
            this.rbCyanOrange.Visible = false;
            this.rbCyanOrange.CheckedChanged += new System.EventHandler(this.rbCyanOrange_CheckedChanged);
            // 
            // pCyan_Yellow
            // 
            this.pCyan_Yellow.Controls.Add(this.rbYellow);
            this.pCyan_Yellow.Controls.Add(this.rbCyan);
            this.pCyan_Yellow.Location = new System.Drawing.Point(65, 299);
            this.pCyan_Yellow.Name = "pCyan_Yellow";
            this.pCyan_Yellow.Size = new System.Drawing.Size(256, 29);
            this.pCyan_Yellow.TabIndex = 32;
            // 
            // rbYellow
            // 
            this.rbYellow.AutoSize = true;
            this.rbYellow.Location = new System.Drawing.Point(157, 6);
            this.rbYellow.Name = "rbYellow";
            this.rbYellow.Size = new System.Drawing.Size(91, 17);
            this.rbYellow.TabIndex = 1;
            this.rbYellow.TabStop = true;
            this.rbYellow.Text = "Morray Yellow";
            this.rbYellow.UseVisualStyleBackColor = true;
            this.rbYellow.CheckedChanged += new System.EventHandler(this.rbYellow_CheckedChanged);
            // 
            // rbCyan
            // 
            this.rbCyan.AutoSize = true;
            this.rbCyan.Checked = true;
            this.rbCyan.Location = new System.Drawing.Point(19, 6);
            this.rbCyan.Name = "rbCyan";
            this.rbCyan.Size = new System.Drawing.Size(84, 17);
            this.rbCyan.TabIndex = 0;
            this.rbCyan.TabStop = true;
            this.rbCyan.Text = "Morray Cyan";
            this.rbCyan.UseVisualStyleBackColor = true;
            this.rbCyan.CheckedChanged += new System.EventHandler(this.rbCyan_CheckedChanged);
            // 
            // lblRedtxt
            // 
            this.lblRedtxt.AutoSize = true;
            this.lblRedtxt.Location = new System.Drawing.Point(328, 76);
            this.lblRedtxt.Name = "lblRedtxt";
            this.lblRedtxt.Size = new System.Drawing.Size(0, 13);
            this.lblRedtxt.TabIndex = 33;
            // 
            // lblGreentxt
            // 
            this.lblGreentxt.AutoSize = true;
            this.lblGreentxt.Location = new System.Drawing.Point(328, 127);
            this.lblGreentxt.Name = "lblGreentxt";
            this.lblGreentxt.Size = new System.Drawing.Size(0, 13);
            this.lblGreentxt.TabIndex = 34;
            // 
            // lblBluetxt
            // 
            this.lblBluetxt.AutoSize = true;
            this.lblBluetxt.Location = new System.Drawing.Point(328, 178);
            this.lblBluetxt.Name = "lblBluetxt";
            this.lblBluetxt.Size = new System.Drawing.Size(0, 13);
            this.lblBluetxt.TabIndex = 35;
            // 
            // lblIntensitytxt
            // 
            this.lblIntensitytxt.AutoSize = true;
            this.lblIntensitytxt.Location = new System.Drawing.Point(328, 230);
            this.lblIntensitytxt.Name = "lblIntensitytxt";
            this.lblIntensitytxt.Size = new System.Drawing.Size(0, 13);
            this.lblIntensitytxt.TabIndex = 36;
            // 
            // lblDUT
            // 
            this.lblDUT.AutoSize = true;
            this.lblDUT.Location = new System.Drawing.Point(174, 9);
            this.lblDUT.Name = "lblDUT";
            this.lblDUT.Size = new System.Drawing.Size(0, 13);
            this.lblDUT.TabIndex = 37;
            // 
            // LEDframe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 337);
            this.Controls.Add(this.lblDUT);
            this.Controls.Add(this.lblIntensitytxt);
            this.Controls.Add(this.lblBluetxt);
            this.Controls.Add(this.lblGreentxt);
            this.Controls.Add(this.lblRedtxt);
            this.Controls.Add(this.pCyan_Yellow);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.rbPercentage);
            this.Controls.Add(this.rbHex);
            this.Controls.Add(this.tbIntensity);
            this.Controls.Add(this.tbBlue);
            this.Controls.Add(this.tbGreen);
            this.Controls.Add(this.tbRed);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label);
            this.Controls.Add(this.tbarIntensity);
            this.Controls.Add(this.tbarBlue);
            this.Controls.Add(this.tbarGreen);
            this.Controls.Add(this.tbarRed);
            this.Name = "LEDframe";
            this.Text = "Change LED Color";
            ((System.ComponentModel.ISupportInitialize)(this.tbarRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarBlue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarIntensity)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pCyan_Yellow.ResumeLayout(false);
            this.pCyan_Yellow.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar tbarRed;
        private System.Windows.Forms.TrackBar tbarGreen;
        private System.Windows.Forms.TrackBar tbarBlue;
        private System.Windows.Forms.TrackBar tbarIntensity;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbRed;
        private System.Windows.Forms.TextBox tbGreen;
        private System.Windows.Forms.TextBox tbBlue;
        private System.Windows.Forms.TextBox tbIntensity;
        private System.Windows.Forms.RadioButton rbHex;
        private System.Windows.Forms.RadioButton rbPercentage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbYellowWhite;
        private System.Windows.Forms.RadioButton rbYellowGreen;
        private System.Windows.Forms.RadioButton rbYellowRed;
        private System.Windows.Forms.RadioButton rbYellowOrange;
        private System.Windows.Forms.RadioButton rbCyanBlue;
        private System.Windows.Forms.RadioButton rbCyanWhite;
        private System.Windows.Forms.RadioButton rbCyanGreen;
        private System.Windows.Forms.RadioButton rbCyanRed;
        private System.Windows.Forms.RadioButton rbCyanOrange;
        private System.Windows.Forms.Panel pCyan_Yellow;
        private System.Windows.Forms.RadioButton rbYellow;
        private System.Windows.Forms.RadioButton rbCyan;
        private System.Windows.Forms.Label lblRedtxt;
        private System.Windows.Forms.Label lblGreentxt;
        private System.Windows.Forms.Label lblBluetxt;
        private System.Windows.Forms.Label lblIntensitytxt;
        private System.Windows.Forms.RadioButton rbCyanOff;
        private System.Windows.Forms.RadioButton rbYellowOff;
        private System.Windows.Forms.Label lblDUT;
    }
}