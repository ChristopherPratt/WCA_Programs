namespace WcaDVConsole
{
    partial class SetESNframe
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
            this.tbESN = new System.Windows.Forms.TextBox();
            this.tbHW = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblHW = new System.Windows.Forms.Label();
            this.lblESNstatus = new System.Windows.Forms.Label();
            this.lblHWstatus = new System.Windows.Forms.Label();
            this.bsetESN = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblOTP = new System.Windows.Forms.Label();
            this.lblDUT = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbESN
            // 
            this.tbESN.Location = new System.Drawing.Point(52, 33);
            this.tbESN.Name = "tbESN";
            this.tbESN.Size = new System.Drawing.Size(199, 20);
            this.tbESN.TabIndex = 0;
            // 
            // tbHW
            // 
            this.tbHW.Location = new System.Drawing.Point(52, 82);
            this.tbHW.Name = "tbHW";
            this.tbHW.Size = new System.Drawing.Size(44, 20);
            this.tbHW.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "ESN:";
            // 
            // lblHW
            // 
            this.lblHW.AutoSize = true;
            this.lblHW.Location = new System.Drawing.Point(17, 85);
            this.lblHW.Name = "lblHW";
            this.lblHW.Size = new System.Drawing.Size(29, 13);
            this.lblHW.TabIndex = 3;
            this.lblHW.Text = "HW:";
            this.lblHW.Click += new System.EventHandler(this.lblHW_Click);
            // 
            // lblESNstatus
            // 
            this.lblESNstatus.Location = new System.Drawing.Point(49, 56);
            this.lblESNstatus.Name = "lblESNstatus";
            this.lblESNstatus.Size = new System.Drawing.Size(231, 26);
            this.lblESNstatus.TabIndex = 4;
            // 
            // lblHWstatus
            // 
            this.lblHWstatus.Location = new System.Drawing.Point(49, 102);
            this.lblHWstatus.Name = "lblHWstatus";
            this.lblHWstatus.Size = new System.Drawing.Size(231, 33);
            this.lblHWstatus.TabIndex = 5;
            // 
            // bsetESN
            // 
            this.bsetESN.Location = new System.Drawing.Point(64, 138);
            this.bsetESN.Name = "bsetESN";
            this.bsetESN.Size = new System.Drawing.Size(155, 25);
            this.bsetESN.TabIndex = 6;
            this.bsetESN.Text = "Set new ESN";
            this.bsetESN.UseVisualStyleBackColor = true;
            this.bsetESN.Click += new System.EventHandler(this.bsetESN_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(89, 166);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "ESN OTP slots left:";
            // 
            // lblOTP
            // 
            this.lblOTP.AutoSize = true;
            this.lblOTP.Location = new System.Drawing.Point(183, 166);
            this.lblOTP.Name = "lblOTP";
            this.lblOTP.Size = new System.Drawing.Size(13, 13);
            this.lblOTP.TabIndex = 8;
            this.lblOTP.Text = "0";
            // 
            // lblDUT
            // 
            this.lblDUT.AutoSize = true;
            this.lblDUT.Location = new System.Drawing.Point(121, 9);
            this.lblDUT.Name = "lblDUT";
            this.lblDUT.Size = new System.Drawing.Size(0, 13);
            this.lblDUT.TabIndex = 9;
            // 
            // SetESNframe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 191);
            this.Controls.Add(this.lblDUT);
            this.Controls.Add(this.lblOTP);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bsetESN);
            this.Controls.Add(this.lblHWstatus);
            this.Controls.Add(this.lblESNstatus);
            this.Controls.Add(this.lblHW);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbHW);
            this.Controls.Add(this.tbESN);
            this.Name = "SetESNframe";
            this.Text = "SetESNframe";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbESN;
        private System.Windows.Forms.TextBox tbHW;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblHW;
        private System.Windows.Forms.Label lblESNstatus;
        private System.Windows.Forms.Label lblHWstatus;
        private System.Windows.Forms.Button bsetESN;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblOTP;
        private System.Windows.Forms.Label lblDUT;
    }
}