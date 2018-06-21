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
            this.bColor = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tbarRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarBlue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarIntensity)).BeginInit();
            this.SuspendLayout();
            // 
            // tbarRed
            // 
            this.tbarRed.Location = new System.Drawing.Point(47, 34);
            this.tbarRed.Maximum = 65535;
            this.tbarRed.Name = "tbarRed";
            this.tbarRed.Size = new System.Drawing.Size(282, 45);
            this.tbarRed.TabIndex = 0;
            this.tbarRed.Scroll += new System.EventHandler(this.tbarRed_Scroll);
            this.tbarRed.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbarRed_MouseUp);
            // 
            // tbarGreen
            // 
            this.tbarGreen.Location = new System.Drawing.Point(47, 85);
            this.tbarGreen.Maximum = 65535;
            this.tbarGreen.Name = "tbarGreen";
            this.tbarGreen.Size = new System.Drawing.Size(282, 45);
            this.tbarGreen.TabIndex = 1;
            this.tbarGreen.Scroll += new System.EventHandler(this.tbarGreen_Scroll);
            this.tbarGreen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbarGreen_MouseUp);
            // 
            // tbarBlue
            // 
            this.tbarBlue.Location = new System.Drawing.Point(47, 136);
            this.tbarBlue.Maximum = 65535;
            this.tbarBlue.Name = "tbarBlue";
            this.tbarBlue.Size = new System.Drawing.Size(282, 45);
            this.tbarBlue.TabIndex = 2;
            this.tbarBlue.Scroll += new System.EventHandler(this.tbarBlue_Scroll);
            this.tbarBlue.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbarBlue_MouseUp);
            // 
            // tbarIntensity
            // 
            this.tbarIntensity.Location = new System.Drawing.Point(47, 187);
            this.tbarIntensity.Maximum = 100;
            this.tbarIntensity.Name = "tbarIntensity";
            this.tbarIntensity.Size = new System.Drawing.Size(282, 45);
            this.tbarIntensity.TabIndex = 3;
            this.tbarIntensity.Scroll += new System.EventHandler(this.tbarIntensity_Scroll);
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(12, 40);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(27, 13);
            this.label.TabIndex = 4;
            this.label.Text = "Red";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Green";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 143);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Blue";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 191);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Intensity";
            // 
            // tbRed
            // 
            this.tbRed.Location = new System.Drawing.Point(331, 34);
            this.tbRed.Name = "tbRed";
            this.tbRed.Size = new System.Drawing.Size(47, 20);
            this.tbRed.TabIndex = 8;
            this.tbRed.Text = "0";
            this.tbRed.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbRed_KeyDown);
            // 
            // tbGreen
            // 
            this.tbGreen.Location = new System.Drawing.Point(331, 85);
            this.tbGreen.Name = "tbGreen";
            this.tbGreen.Size = new System.Drawing.Size(47, 20);
            this.tbGreen.TabIndex = 9;
            this.tbGreen.Text = "0";
            this.tbGreen.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbGreen_KeyDown);
            // 
            // tbBlue
            // 
            this.tbBlue.Location = new System.Drawing.Point(331, 136);
            this.tbBlue.Name = "tbBlue";
            this.tbBlue.Size = new System.Drawing.Size(47, 20);
            this.tbBlue.TabIndex = 10;
            this.tbBlue.Text = "0";
            this.tbBlue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbBlue_KeyDown);
            // 
            // tbIntensity
            // 
            this.tbIntensity.Location = new System.Drawing.Point(331, 188);
            this.tbIntensity.Name = "tbIntensity";
            this.tbIntensity.Size = new System.Drawing.Size(47, 20);
            this.tbIntensity.TabIndex = 11;
            this.tbIntensity.Text = "50";
            this.tbIntensity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbIntensity_KeyDown);
            // 
            // rbHex
            // 
            this.rbHex.AutoSize = true;
            this.rbHex.Location = new System.Drawing.Point(92, 11);
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
            this.rbPercentage.Location = new System.Drawing.Point(217, 12);
            this.rbPercentage.Name = "rbPercentage";
            this.rbPercentage.Size = new System.Drawing.Size(80, 17);
            this.rbPercentage.TabIndex = 13;
            this.rbPercentage.TabStop = true;
            this.rbPercentage.Text = "Percentage";
            this.rbPercentage.UseVisualStyleBackColor = true;
            // 
            // bColor
            // 
            this.bColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bColor.Location = new System.Drawing.Point(112, 238);
            this.bColor.Name = "bColor";
            this.bColor.Size = new System.Drawing.Size(173, 36);
            this.bColor.TabIndex = 14;
            this.bColor.Text = "Change Color";
            this.bColor.UseVisualStyleBackColor = true;
            this.bColor.Click += new System.EventHandler(this.bColor_Click);
            // 
            // LEDframe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 297);
            this.Controls.Add(this.bColor);
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
        private System.Windows.Forms.Button bColor;
    }
}