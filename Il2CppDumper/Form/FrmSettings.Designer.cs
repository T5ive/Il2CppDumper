namespace Il2CppDumper
{
    partial class FrmSettings
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
            this.cbAutoSetDir = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkExtBin = new System.Windows.Forms.CheckBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.clbScripts = new System.Windows.Forms.CheckedListBox();
            this.chkExtDat = new System.Windows.Forms.CheckBox();
            this.rad64 = new System.Windows.Forms.RadioButton();
            this.rad32 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbAutoSetDir
            // 
            this.cbAutoSetDir.AutoSize = true;
            this.cbAutoSetDir.Checked = true;
            this.cbAutoSetDir.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoSetDir.Location = new System.Drawing.Point(6, 22);
            this.cbAutoSetDir.Name = "cbAutoSetDir";
            this.cbAutoSetDir.Size = new System.Drawing.Size(159, 19);
            this.cbAutoSetDir.TabIndex = 1;
            this.cbAutoSetDir.Text = "Auto set output directory";
            this.cbAutoSetDir.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.rad32);
            this.groupBox1.Controls.Add(this.rad64);
            this.groupBox1.Controls.Add(this.cbAutoSetDir);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(195, 71);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkExtDat);
            this.groupBox2.Controls.Add(this.chkExtBin);
            this.groupBox2.Location = new System.Drawing.Point(12, 89);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(195, 76);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File auto dumping";
            // 
            // chkExtBin
            // 
            this.chkExtBin.AutoSize = true;
            this.chkExtBin.Location = new System.Drawing.Point(6, 22);
            this.chkExtBin.Name = "chkExtBin";
            this.chkExtBin.Size = new System.Drawing.Size(117, 19);
            this.chkExtBin.TabIndex = 0;
            this.chkExtBin.Text = "Extract binary file";
            this.chkExtBin.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(12, 340);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(195, 23);
            this.btnApply.TabIndex = 4;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.clbScripts);
            this.groupBox3.Location = new System.Drawing.Point(12, 171);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(195, 163);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Auto copy script files";
            // 
            // clbScripts
            // 
            this.clbScripts.FormattingEnabled = true;
            this.clbScripts.Items.AddRange(new object[] {
            "ghidra.py",
            "ghidra_with_struct.py",
            "il2cpp_header_to_ghidra.py",
            "ida.py",
            "ida_py3.py",
            "ida_with_struct.py",
            "ida_with_struct_py3.py"});
            this.clbScripts.Location = new System.Drawing.Point(6, 22);
            this.clbScripts.Name = "clbScripts";
            this.clbScripts.Size = new System.Drawing.Size(183, 130);
            this.clbScripts.TabIndex = 1;
            // 
            // chkExtDat
            // 
            this.chkExtDat.AutoSize = true;
            this.chkExtDat.Location = new System.Drawing.Point(6, 47);
            this.chkExtDat.Name = "chkExtDat";
            this.chkExtDat.Size = new System.Drawing.Size(174, 19);
            this.chkExtDat.TabIndex = 1;
            this.chkExtDat.Text = "Extract Global-metadata.dat";
            this.chkExtDat.UseVisualStyleBackColor = true;
            // 
            // rad64
            // 
            this.rad64.AutoSize = true;
            this.rad64.Checked = true;
            this.rad64.Location = new System.Drawing.Point(133, 46);
            this.rad64.Name = "rad64";
            this.rad64.Size = new System.Drawing.Size(56, 19);
            this.rad64.TabIndex = 2;
            this.rad64.TabStop = true;
            this.rad64.Text = "64-bit";
            this.rad64.UseVisualStyleBackColor = true;
            // 
            // rad32
            // 
            this.rad32.AutoSize = true;
            this.rad32.Location = new System.Drawing.Point(66, 46);
            this.rad32.Name = "rad32";
            this.rad32.Size = new System.Drawing.Size(56, 19);
            this.rad32.TabIndex = 3;
            this.rad32.TabStop = true;
            this.rad32.Text = "32-bit";
            this.rad32.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Mach-O:";
            // 
            // FrmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(219, 375);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Settings";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FrmSettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox cbAutoSetDir;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkExtBin;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckedListBox clbScripts;
        private System.Windows.Forms.CheckBox chkExtDat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rad32;
        private System.Windows.Forms.RadioButton rad64;
    }
}