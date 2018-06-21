namespace WcaDVConsole
{
    partial class PostProgrammingFrame
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
            this.label1 = new System.Windows.Forms.Label();
            this.bEOL = new System.Windows.Forms.Button();
            this.bDebug = new System.Windows.Forms.Button();
            this.bBootloader = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(238, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select which mode you want the device to enter.\r\n";
            // 
            // bEOL
            // 
            this.bEOL.Location = new System.Drawing.Point(13, 54);
            this.bEOL.Name = "bEOL";
            this.bEOL.Size = new System.Drawing.Size(90, 32);
            this.bEOL.TabIndex = 1;
            this.bEOL.Text = "EOL";
            this.bEOL.UseVisualStyleBackColor = true;
            this.bEOL.Click += new System.EventHandler(this.button1_Click);
            // 
            // bDebug
            // 
            this.bDebug.Location = new System.Drawing.Point(133, 54);
            this.bDebug.Name = "bDebug";
            this.bDebug.Size = new System.Drawing.Size(90, 32);
            this.bDebug.TabIndex = 2;
            this.bDebug.Text = "Debug";
            this.bDebug.UseVisualStyleBackColor = true;
            this.bDebug.Click += new System.EventHandler(this.bDebug_Click);
            // 
            // bBootloader
            // 
            this.bBootloader.Location = new System.Drawing.Point(249, 54);
            this.bBootloader.Name = "bBootloader";
            this.bBootloader.Size = new System.Drawing.Size(90, 32);
            this.bBootloader.TabIndex = 3;
            this.bBootloader.Text = "Bootloader";
            this.bBootloader.UseVisualStyleBackColor = true;
            this.bBootloader.Click += new System.EventHandler(this.bBootloader_Click);
            // 
            // PostProgrammingFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 118);
            this.Controls.Add(this.bBootloader);
            this.Controls.Add(this.bDebug);
            this.Controls.Add(this.bEOL);
            this.Controls.Add(this.label1);
            this.Name = "PostProgrammingFrame";
            this.Text = "Choose device mode.";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bEOL;
        private System.Windows.Forms.Button bDebug;
        private System.Windows.Forms.Button bBootloader;
    }
}