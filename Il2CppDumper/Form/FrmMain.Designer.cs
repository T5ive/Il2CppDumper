namespace Il2CppDumper
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.txtDat = new System.Windows.Forms.TextBox();
            this.txtDir = new System.Windows.Forms.TextBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.txtMeta = new System.Windows.Forms.TextBox();
            this.btnFile = new System.Windows.Forms.Button();
            this.btnDat = new System.Windows.Forms.Button();
            this.btnDir = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.rad32 = new System.Windows.Forms.RadioButton();
            this.rad64 = new System.Windows.Forms.RadioButton();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnDump = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbLog = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.openBin = new System.Windows.Forms.OpenFileDialog();
            this.openDat = new System.Windows.Forms.OpenFileDialog();
            this.openDir = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Executable file:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "global-metadata.dat:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Output directory:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "Code Registration:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(239, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "Metadata Registration:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(311, 139);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 15);
            this.label6.TabIndex = 5;
            this.label6.Text = "Mach-O:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(125, 212);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(259, 15);
            this.label9.TabIndex = 8;
            this.label9.Text = "Drop APK or decrypted IPA file to start dumping";
            // 
            // txtFile
            // 
            this.txtFile.Location = new System.Drawing.Point(136, 12);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(281, 23);
            this.txtFile.TabIndex = 9;
            // 
            // txtDat
            // 
            this.txtDat.Location = new System.Drawing.Point(136, 41);
            this.txtDat.Name = "txtDat";
            this.txtDat.Size = new System.Drawing.Size(281, 23);
            this.txtDat.TabIndex = 10;
            // 
            // txtDir
            // 
            this.txtDir.Location = new System.Drawing.Point(136, 70);
            this.txtDir.Name = "txtDir";
            this.txtDir.Size = new System.Drawing.Size(229, 23);
            this.txtDir.TabIndex = 11;
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(136, 99);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(100, 23);
            this.txtCode.TabIndex = 12;
            // 
            // txtMeta
            // 
            this.txtMeta.Location = new System.Drawing.Point(371, 102);
            this.txtMeta.Name = "txtMeta";
            this.txtMeta.Size = new System.Drawing.Size(126, 23);
            this.txtMeta.TabIndex = 13;
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(423, 11);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(74, 23);
            this.btnFile.TabIndex = 14;
            this.btnFile.Text = "Select";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // btnDat
            // 
            this.btnDat.Location = new System.Drawing.Point(423, 41);
            this.btnDat.Name = "btnDat";
            this.btnDat.Size = new System.Drawing.Size(74, 23);
            this.btnDat.TabIndex = 14;
            this.btnDat.Text = "Select";
            this.btnDat.UseVisualStyleBackColor = true;
            this.btnDat.Click += new System.EventHandler(this.btnDat_Click);
            // 
            // btnDir
            // 
            this.btnDir.Location = new System.Drawing.Point(423, 71);
            this.btnDir.Name = "btnDir";
            this.btnDir.Size = new System.Drawing.Size(74, 23);
            this.btnDir.TabIndex = 14;
            this.btnDir.Text = "Select";
            this.btnDir.UseVisualStyleBackColor = true;
            this.btnDir.Click += new System.EventHandler(this.btnDir_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(371, 70);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(46, 23);
            this.btnOpen.TabIndex = 14;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // rad32
            // 
            this.rad32.AutoSize = true;
            this.rad32.Location = new System.Drawing.Point(380, 137);
            this.rad32.Name = "rad32";
            this.rad32.Size = new System.Drawing.Size(56, 19);
            this.rad32.TabIndex = 15;
            this.rad32.Text = "32-bit";
            this.rad32.UseVisualStyleBackColor = true;
            // 
            // rad64
            // 
            this.rad64.AutoSize = true;
            this.rad64.Checked = true;
            this.rad64.Location = new System.Drawing.Point(442, 137);
            this.rad64.Name = "rad64";
            this.rad64.Size = new System.Drawing.Size(56, 19);
            this.rad64.TabIndex = 16;
            this.rad64.TabStop = true;
            this.rad64.Text = "64-bit";
            this.rad64.UseVisualStyleBackColor = true;
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(12, 135);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(75, 23);
            this.btnSettings.TabIndex = 17;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnDump
            // 
            this.btnDump.Location = new System.Drawing.Point(12, 164);
            this.btnDump.Name = "btnDump";
            this.btnDump.Size = new System.Drawing.Size(485, 45);
            this.btnDump.TabIndex = 18;
            this.btnDump.Text = "Dump";
            this.btnDump.UseVisualStyleBackColor = true;
            this.btnDump.Click += new System.EventHandler(this.btnDump_ClickAsync);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbLog);
            this.groupBox1.Location = new System.Drawing.Point(12, 230);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(485, 219);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log Output";
            // 
            // rbLog
            // 
            this.rbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbLog.Location = new System.Drawing.Point(3, 19);
            this.rbLog.Name = "rbLog";
            this.rbLog.Size = new System.Drawing.Size(479, 197);
            this.rbLog.TabIndex = 0;
            this.rbLog.Text = "";
            this.rbLog.TextChanged += new System.EventHandler(this.rbLog_TextChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCopy});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(103, 26);
            // 
            // menuCopy
            // 
            this.menuCopy.Name = "menuCopy";
            this.menuCopy.Size = new System.Drawing.Size(102, 22);
            this.menuCopy.Text = "Copy";
            this.menuCopy.Click += new System.EventHandler(this.menuCopy_Click);
            // 
            // openBin
            // 
            this.openBin.Filter = "Il2Cpp binary file|*.*";
            this.openBin.Title = "Select Il2Cpp binary file";
            // 
            // openDat
            // 
            this.openDat.Filter = "global-metadata|global-metadata.dat";
            this.openDat.Title = "Select global-metadata.dat";
            // 
            // openDir
            // 
            this.openDir.Description = "Select output directory";
            // 
            // FrmMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(509, 461);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnDump);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.rad64);
            this.Controls.Add(this.rad32);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnDir);
            this.Controls.Add(this.btnDat);
            this.Controls.Add(this.btnFile);
            this.Controls.Add(this.txtMeta);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.txtDir);
            this.Controls.Add(this.txtDat);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Il2Cpp Dumper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FrmMain_DragDropAsync);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FrmMain_DragEnter);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.FrmMain_DragOver);
            this.groupBox1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.TextBox txtDat;
        private System.Windows.Forms.TextBox txtDir;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.TextBox txtMeta;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.Button btnDat;
        private System.Windows.Forms.Button btnDir;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.RadioButton rad32;
        private System.Windows.Forms.RadioButton rad64;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnDump;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox rbLog;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuCopy;
        private System.Windows.Forms.OpenFileDialog openBin;
        private System.Windows.Forms.OpenFileDialog openDat;
        private System.Windows.Forms.FolderBrowserDialog openDir;
    }
}

