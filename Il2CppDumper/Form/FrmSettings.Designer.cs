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
            this.cbExtBin = new System.Windows.Forms.CheckBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.clbScripts = new System.Windows.Forms.CheckedListBox();
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
            this.groupBox1.Controls.Add(this.cbAutoSetDir);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(195, 55);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbExtBin);
            this.groupBox2.Location = new System.Drawing.Point(12, 73);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(195, 55);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File auto dumping";
            // 
            // cbExtBin
            // 
            this.cbExtBin.AutoSize = true;
            this.cbExtBin.Location = new System.Drawing.Point(6, 22);
            this.cbExtBin.Name = "cbExtBin";
            this.cbExtBin.Size = new System.Drawing.Size(117, 19);
            this.cbExtBin.TabIndex = 0;
            this.cbExtBin.Text = "Extract binary file";
            this.cbExtBin.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(12, 282);
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
            this.groupBox3.Location = new System.Drawing.Point(12, 134);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(195, 142);
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
            "ida.py",
            "ida_py3.py",
            "ida_with_struct.py",
            "ida_with_struct_py3.py"});
            this.clbScripts.Location = new System.Drawing.Point(6, 22);
            this.clbScripts.Name = "clbScripts";
            this.clbScripts.Size = new System.Drawing.Size(183, 112);
            this.clbScripts.TabIndex = 1;
            // 
            // FrmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(219, 317);
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
        private System.Windows.Forms.CheckBox cbExtBin;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckedListBox clbScripts;
    }
}