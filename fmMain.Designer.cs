//INSTANT C# NOTE: Formerly VB project-level imports:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

using System.IO;
using System.Text;
using System.Collections.Specialized;

namespace ProjectConverter
{
	[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
	public partial class fmMain : System.Windows.Forms.Form
	{

		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && components != null)
				{
					components.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		//Required by the Windows Form Designer
		private System.ComponentModel.IContainer components;

		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fmMain));
            this.ofdMain = new System.Windows.Forms.OpenFileDialog();
            this.tbProjectConverter = new System.Windows.Forms.TabControl();
            this.tbSolnConversion = new System.Windows.Forms.TabPage();
            this.chkRemoveScc = new System.Windows.Forms.CheckBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.lbVersion = new System.Windows.Forms.ListBox();
            this.lbSolnPath = new System.Windows.Forms.Label();
            this.lbMessage = new System.Windows.Forms.Label();
            this.cbBackup = new System.Windows.Forms.CheckBox();
            this.tbSolutionFile = new System.Windows.Forms.TextBox();
            this.bnBrowse = new System.Windows.Forms.Button();
            this.bnConvert = new System.Windows.Forms.Button();
            this.tbUpgradeSoln = new System.Windows.Forms.TabPage();
            this.chkTFSCheckout = new System.Windows.Forms.CheckBox();
            this.lbUpgradeStatus = new System.Windows.Forms.Label();
            this.lbVSSolnPath = new System.Windows.Forms.Label();
            this.txtVSSolnPath = new System.Windows.Forms.TextBox();
            this.cmdVSSolnPath = new System.Windows.Forms.Button();
            this.cmdUpgradeVSSoln = new System.Windows.Forms.Button();
            this.cmdBrowseDevEnv = new System.Windows.Forms.Button();
            this.txtDevEnvPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ofdDevEnv = new System.Windows.Forms.OpenFileDialog();
            this.ofdTFExe = new System.Windows.Forms.OpenFileDialog();
            this.tbMVCConverter = new System.Windows.Forms.TabPage();
            this.txtMVCConverterExe = new System.Windows.Forms.TextBox();
            this.lblMVCConverter = new System.Windows.Forms.Label();
            this.cmdLaunchMVCExe = new System.Windows.Forms.Button();
            this.cmdBrowseMVCExe = new System.Windows.Forms.Button();
            this.ofdMVCExe = new System.Windows.Forms.OpenFileDialog();
            this.tbProjectConverter.SuspendLayout();
            this.tbSolnConversion.SuspendLayout();
            this.tbUpgradeSoln.SuspendLayout();
            this.tbMVCConverter.SuspendLayout();
            this.SuspendLayout();
            // 
            // ofdMain
            // 
            this.ofdMain.Filter = "Microsoft Visual Studio Solution File (*.sln)|*.sln|All files (*.*)|*.*";
            // 
            // tbProjectConverter
            // 
            this.tbProjectConverter.Controls.Add(this.tbSolnConversion);
            this.tbProjectConverter.Controls.Add(this.tbUpgradeSoln);
            this.tbProjectConverter.Controls.Add(this.tbMVCConverter);
            this.tbProjectConverter.Location = new System.Drawing.Point(2, 12);
            this.tbProjectConverter.Name = "tbProjectConverter";
            this.tbProjectConverter.SelectedIndex = 0;
            this.tbProjectConverter.Size = new System.Drawing.Size(664, 337);
            this.tbProjectConverter.TabIndex = 0;
            // 
            // tbSolnConversion
            // 
            this.tbSolnConversion.Controls.Add(this.chkRemoveScc);
            this.tbSolnConversion.Controls.Add(this.Label1);
            this.tbSolnConversion.Controls.Add(this.lbVersion);
            this.tbSolnConversion.Controls.Add(this.lbSolnPath);
            this.tbSolnConversion.Controls.Add(this.lbMessage);
            this.tbSolnConversion.Controls.Add(this.cbBackup);
            this.tbSolnConversion.Controls.Add(this.tbSolutionFile);
            this.tbSolnConversion.Controls.Add(this.bnBrowse);
            this.tbSolnConversion.Controls.Add(this.bnConvert);
            this.tbSolnConversion.Location = new System.Drawing.Point(4, 22);
            this.tbSolnConversion.Name = "tbSolnConversion";
            this.tbSolnConversion.Padding = new System.Windows.Forms.Padding(3);
            this.tbSolnConversion.Size = new System.Drawing.Size(656, 311);
            this.tbSolnConversion.TabIndex = 0;
            this.tbSolnConversion.Text = "Convert Solution";
            this.tbSolnConversion.UseVisualStyleBackColor = true;
            // 
            // chkRemoveScc
            // 
            this.chkRemoveScc.AutoSize = true;
            this.chkRemoveScc.Location = new System.Drawing.Point(168, 160);
            this.chkRemoveScc.Name = "chkRemoveScc";
            this.chkRemoveScc.Size = new System.Drawing.Size(131, 17);
            this.chkRemoveScc.TabIndex = 17;
            this.chkRemoveScc.Text = "Remove Scc Bindings";
            this.chkRemoveScc.UseVisualStyleBackColor = true;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(20, 60);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(101, 13);
            this.Label1.TabIndex = 16;
            this.Label1.Text = "Convert To Version:";
            // 
            // lbVersion
            // 
            this.lbVersion.FormattingEnabled = true;
            this.lbVersion.Location = new System.Drawing.Point(20, 76);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(352, 43);
            this.lbVersion.TabIndex = 15;
            this.lbVersion.SelectedIndexChanged += new System.EventHandler(this.lbVersion_SelectedIndexChanged);
            // 
            // lbSolnPath
            // 
            this.lbSolnPath.AutoSize = true;
            this.lbSolnPath.Location = new System.Drawing.Point(20, 19);
            this.lbSolnPath.Name = "lbSolnPath";
            this.lbSolnPath.Size = new System.Drawing.Size(163, 13);
            this.lbSolnPath.TabIndex = 14;
            this.lbSolnPath.Text = "Visual Studio Solution to convert:";
            // 
            // lbMessage
            // 
            this.lbMessage.AutoSize = true;
            this.lbMessage.Location = new System.Drawing.Point(17, 195);
            this.lbMessage.Name = "lbMessage";
            this.lbMessage.Size = new System.Drawing.Size(47, 13);
            this.lbMessage.TabIndex = 13;
            this.lbMessage.Text = "Ready...";
            // 
            // cbBackup
            // 
            this.cbBackup.AutoSize = true;
            this.cbBackup.Checked = global::ProjectConverter.Settings.Default.BackupFirst;
            this.cbBackup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbBackup.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ProjectConverter.Settings.Default, "BackupFirst", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbBackup.Location = new System.Drawing.Point(20, 160);
            this.cbBackup.Name = "cbBackup";
            this.cbBackup.Size = new System.Drawing.Size(120, 17);
            this.cbBackup.TabIndex = 12;
            this.cbBackup.Text = "Make Backup Copy";
            this.cbBackup.UseVisualStyleBackColor = true;
            // 
            // tbSolutionFile
            // 
            this.tbSolutionFile.Location = new System.Drawing.Point(20, 36);
            this.tbSolutionFile.Name = "tbSolutionFile";
            this.tbSolutionFile.Size = new System.Drawing.Size(352, 20);
            this.tbSolutionFile.TabIndex = 11;
            // 
            // bnBrowse
            // 
            this.bnBrowse.Location = new System.Drawing.Point(378, 36);
            this.bnBrowse.Name = "bnBrowse";
            this.bnBrowse.Size = new System.Drawing.Size(30, 22);
            this.bnBrowse.TabIndex = 10;
            this.bnBrowse.Text = "...";
            this.bnBrowse.UseVisualStyleBackColor = true;
            this.bnBrowse.Click += new System.EventHandler(this.bnBrowse_Click);
            // 
            // bnConvert
            // 
            this.bnConvert.Location = new System.Drawing.Point(168, 219);
            this.bnConvert.Name = "bnConvert";
            this.bnConvert.Size = new System.Drawing.Size(88, 37);
            this.bnConvert.TabIndex = 9;
            this.bnConvert.Text = "Convert Solution";
            this.bnConvert.UseVisualStyleBackColor = true;
            this.bnConvert.Click += new System.EventHandler(this.bnConvert_Click);
            // 
            // tbUpgradeSoln
            // 
            this.tbUpgradeSoln.Controls.Add(this.chkTFSCheckout);
            this.tbUpgradeSoln.Controls.Add(this.lbUpgradeStatus);
            this.tbUpgradeSoln.Controls.Add(this.lbVSSolnPath);
            this.tbUpgradeSoln.Controls.Add(this.txtVSSolnPath);
            this.tbUpgradeSoln.Controls.Add(this.cmdVSSolnPath);
            this.tbUpgradeSoln.Controls.Add(this.cmdUpgradeVSSoln);
            this.tbUpgradeSoln.Controls.Add(this.cmdBrowseDevEnv);
            this.tbUpgradeSoln.Controls.Add(this.txtDevEnvPath);
            this.tbUpgradeSoln.Controls.Add(this.label3);
            this.tbUpgradeSoln.Location = new System.Drawing.Point(4, 22);
            this.tbUpgradeSoln.Name = "tbUpgradeSoln";
            this.tbUpgradeSoln.Padding = new System.Windows.Forms.Padding(3);
            this.tbUpgradeSoln.Size = new System.Drawing.Size(656, 311);
            this.tbUpgradeSoln.TabIndex = 1;
            this.tbUpgradeSoln.Text = "Upgrade Solution";
            this.tbUpgradeSoln.UseVisualStyleBackColor = true;
            // 
            // chkTFSCheckout
            // 
            this.chkTFSCheckout.AutoSize = true;
            this.chkTFSCheckout.Location = new System.Drawing.Point(23, 153);
            this.chkTFSCheckout.Name = "chkTFSCheckout";
            this.chkTFSCheckout.Size = new System.Drawing.Size(118, 17);
            this.chkTFSCheckout.TabIndex = 23;
            this.chkTFSCheckout.Text = "Checkout from TFS";
            this.chkTFSCheckout.UseVisualStyleBackColor = true;
            this.chkTFSCheckout.Visible = false;
            // 
            // lbUpgradeStatus
            // 
            this.lbUpgradeStatus.AutoSize = true;
            this.lbUpgradeStatus.Location = new System.Drawing.Point(17, 195);
            this.lbUpgradeStatus.Name = "lbUpgradeStatus";
            this.lbUpgradeStatus.Size = new System.Drawing.Size(47, 13);
            this.lbUpgradeStatus.TabIndex = 22;
            this.lbUpgradeStatus.Text = "Ready...";
            // 
            // lbVSSolnPath
            // 
            this.lbVSSolnPath.AutoSize = true;
            this.lbVSSolnPath.Location = new System.Drawing.Point(20, 74);
            this.lbVSSolnPath.Name = "lbVSSolnPath";
            this.lbVSSolnPath.Size = new System.Drawing.Size(149, 13);
            this.lbVSSolnPath.TabIndex = 21;
            this.lbVSSolnPath.Text = "Path to Visual Studio Solution:";
            // 
            // txtVSSolnPath
            // 
            this.txtVSSolnPath.Location = new System.Drawing.Point(20, 91);
            this.txtVSSolnPath.Name = "txtVSSolnPath";
            this.txtVSSolnPath.Size = new System.Drawing.Size(352, 20);
            this.txtVSSolnPath.TabIndex = 20;
            // 
            // cmdVSSolnPath
            // 
            this.cmdVSSolnPath.Location = new System.Drawing.Point(378, 91);
            this.cmdVSSolnPath.Name = "cmdVSSolnPath";
            this.cmdVSSolnPath.Size = new System.Drawing.Size(30, 22);
            this.cmdVSSolnPath.TabIndex = 19;
            this.cmdVSSolnPath.Text = "...";
            this.cmdVSSolnPath.UseVisualStyleBackColor = true;
            this.cmdVSSolnPath.Click += new System.EventHandler(this.cmdVSSolnPath_Click);
            // 
            // cmdUpgradeVSSoln
            // 
            this.cmdUpgradeVSSoln.Location = new System.Drawing.Point(229, 227);
            this.cmdUpgradeVSSoln.Name = "cmdUpgradeVSSoln";
            this.cmdUpgradeVSSoln.Size = new System.Drawing.Size(88, 37);
            this.cmdUpgradeVSSoln.TabIndex = 18;
            this.cmdUpgradeVSSoln.Text = "Upgrade Solution";
            this.cmdUpgradeVSSoln.UseVisualStyleBackColor = true;
            this.cmdUpgradeVSSoln.Click += new System.EventHandler(this.cmdUpgradeVSSoln_Click);
            // 
            // cmdBrowseDevEnv
            // 
            this.cmdBrowseDevEnv.Location = new System.Drawing.Point(378, 36);
            this.cmdBrowseDevEnv.Name = "cmdBrowseDevEnv";
            this.cmdBrowseDevEnv.Size = new System.Drawing.Size(30, 22);
            this.cmdBrowseDevEnv.TabIndex = 17;
            this.cmdBrowseDevEnv.Text = "...";
            this.cmdBrowseDevEnv.UseVisualStyleBackColor = true;
            this.cmdBrowseDevEnv.Click += new System.EventHandler(this.cmdBrowseDevEnv_Click);
            // 
            // txtDevEnvPath
            // 
            this.txtDevEnvPath.Location = new System.Drawing.Point(20, 36);
            this.txtDevEnvPath.Name = "txtDevEnvPath";
            this.txtDevEnvPath.Size = new System.Drawing.Size(352, 20);
            this.txtDevEnvPath.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(164, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Path to Visual Studio Executable:";
            // 
            // ofdDevEnv
            // 
            this.ofdDevEnv.DefaultExt = "exe";
            this.ofdDevEnv.FileName = "devenv.exe";
            this.ofdDevEnv.Filter = "Executable|*.exe";
            this.ofdDevEnv.InitialDirectory = "C:\\Program Files\\Microsoft Visual Studio 10.0\\Common7\\IDE";
            this.ofdDevEnv.Title = "Select the path to DevEnv.exe";
            // 
            // ofdTFExe
            // 
            this.ofdTFExe.FileName = "TF.exe";
            this.ofdTFExe.Filter = "Executable|*.exe";
            this.ofdTFExe.InitialDirectory = "C:\\Program Files\\Microsoft Visual Studio 10.0\\Common7\\IDE";
            this.ofdTFExe.Title = "Select the path to TF.exe";
            // 
            // tbMVCConverter
            // 
            this.tbMVCConverter.Controls.Add(this.cmdBrowseMVCExe);
            this.tbMVCConverter.Controls.Add(this.cmdLaunchMVCExe);
            this.tbMVCConverter.Controls.Add(this.lblMVCConverter);
            this.tbMVCConverter.Controls.Add(this.txtMVCConverterExe);
            this.tbMVCConverter.Location = new System.Drawing.Point(4, 22);
            this.tbMVCConverter.Name = "tbMVCConverter";
            this.tbMVCConverter.Size = new System.Drawing.Size(656, 311);
            this.tbMVCConverter.TabIndex = 3;
            this.tbMVCConverter.Text = "MVC Converter";
            this.tbMVCConverter.UseVisualStyleBackColor = true;
            // 
            // txtMVCConverterExe
            // 
            this.txtMVCConverterExe.Location = new System.Drawing.Point(32, 43);
            this.txtMVCConverterExe.Name = "txtMVCConverterExe";
            this.txtMVCConverterExe.Size = new System.Drawing.Size(352, 20);
            this.txtMVCConverterExe.TabIndex = 17;
            // 
            // lblMVCConverter
            // 
            this.lblMVCConverter.AutoSize = true;
            this.lblMVCConverter.Location = new System.Drawing.Point(29, 27);
            this.lblMVCConverter.Name = "lblMVCConverter";
            this.lblMVCConverter.Size = new System.Drawing.Size(175, 13);
            this.lblMVCConverter.TabIndex = 18;
            this.lblMVCConverter.Text = "Path to MVC Converter Executable:";
            // 
            // cmdLaunchMVCExe
            // 
            this.cmdLaunchMVCExe.Location = new System.Drawing.Point(247, 202);
            this.cmdLaunchMVCExe.Name = "cmdLaunchMVCExe";
            this.cmdLaunchMVCExe.Size = new System.Drawing.Size(137, 20);
            this.cmdLaunchMVCExe.TabIndex = 19;
            this.cmdLaunchMVCExe.Text = "Launch MVC Converter";
            this.cmdLaunchMVCExe.UseVisualStyleBackColor = true;
            this.cmdLaunchMVCExe.Click += new System.EventHandler(this.cmdLaunchMVCExe_Click);
            // 
            // cmdBrowseMVCExe
            // 
            this.cmdBrowseMVCExe.Location = new System.Drawing.Point(390, 43);
            this.cmdBrowseMVCExe.Name = "cmdBrowseMVCExe";
            this.cmdBrowseMVCExe.Size = new System.Drawing.Size(30, 22);
            this.cmdBrowseMVCExe.TabIndex = 20;
            this.cmdBrowseMVCExe.Text = "...";
            this.cmdBrowseMVCExe.UseVisualStyleBackColor = true;
            this.cmdBrowseMVCExe.Click += new System.EventHandler(this.cmdBrowseMVCExe_Click);
            // 
            // ofdMVCExe
            // 
            this.ofdMVCExe.FileName = "MVCAppConverter";
            this.ofdMVCExe.Filter = "Executable|*.exe";
            this.ofdMVCExe.Title = "Select the path to MVCAppConverter.exe";
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 348);
            this.Controls.Add(this.tbProjectConverter);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "fmMain";
            this.Text = "Visual Studio Project Version Converter";
            this.Load += new System.EventHandler(this.fmMain_Load);
            this.tbProjectConverter.ResumeLayout(false);
            this.tbSolnConversion.ResumeLayout(false);
            this.tbSolnConversion.PerformLayout();
            this.tbUpgradeSoln.ResumeLayout(false);
            this.tbUpgradeSoln.PerformLayout();
            this.tbMVCConverter.ResumeLayout(false);
            this.tbMVCConverter.PerformLayout();
            this.ResumeLayout(false);

        }
        internal System.Windows.Forms.OpenFileDialog ofdMain;
        private TabControl tbProjectConverter;
        private TabPage tbSolnConversion;
        private TabPage tbUpgradeSoln;
        private CheckBox chkRemoveScc;
        internal Label Label1;
        internal ListBox lbVersion;
        internal Label lbSolnPath;
        internal Label lbMessage;
        internal CheckBox cbBackup;
        internal TextBox tbSolutionFile;
        internal Button bnBrowse;
        internal Button bnConvert;
        internal Label label3;
        internal TextBox txtDevEnvPath;
        internal Button cmdBrowseDevEnv;
        private OpenFileDialog ofdDevEnv;
        internal Button cmdUpgradeVSSoln;
        internal Label lbVSSolnPath;
        internal TextBox txtVSSolnPath;
        internal Button cmdVSSolnPath;
        internal Label lbUpgradeStatus;
        private OpenFileDialog ofdTFExe;
        private CheckBox chkTFSCheckout;
        private TabPage tbMVCConverter;
        internal Label lblMVCConverter;
        internal TextBox txtMVCConverterExe;
        internal Button cmdLaunchMVCExe;
        internal Button cmdBrowseMVCExe;
        private OpenFileDialog ofdMVCExe;

	}

}