//INSTANT C# NOTE: Formerly VB project-level imports:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;

using System.IO;
using System.Text;
using System.Collections.Specialized;

namespace ProjectConverter
{
	public partial class fmMain
	{

		internal fmMain()
		{
			InitializeComponent();
		}

        private Versions ConvertTo;
        private VSSolutionInfo m_vsSolutionInfo = null;
        private const string m_vsDevEnvDir = @"Microsoft Visual Studio 10.0\Common7\IDE";

        private static fmMain _DefaultInstance;
        public static fmMain DefaultInstance
        {
            get
            {
                if (_DefaultInstance == null)
                    _DefaultInstance = new fmMain();

                return _DefaultInstance;
            }
        }

        #region Properties
        private double SolutionFormatVersion
        {
            get
            {
                return m_vsSolutionInfo.FormatVersion;
            }//get
        }//property: SolutionFormatVersion

        private VSSolutionInfo SolutionInfo
        {
            get
            {
                return m_vsSolutionInfo;
            }//get
            set
            {
                m_vsSolutionInfo = value;
            }//set
        }//property: SolutionInfo 
        #endregion

        #region Methods
        /// <summary>
        /// Populates the Solution Information
        /// </summary>
        private void PopulateSolutionInfo(string strSolutionFilePath)
        {
            m_vsSolutionInfo = new VSSolutionInfo(strSolutionFilePath);
            this.SolutionInfo = m_vsSolutionInfo;
        }//method: PopulateSolutionInfo()


        /// <summary>
        /// Loads the appropriate version of Visual Studio
        /// available for conversion to display in the listbox
        /// </summary>
        /// <param name="version">double indicating the current version number
        /// of the selected Visual Studio project/solution file</param>
        private void LoadChoices(double version)
        {

            Dictionary<double, string> dictVSVersion = new Dictionary<double, string>();
            Dictionary<double, string> dictVSCurrentVersion = new Dictionary<double, string>();
            string vsCurrentVersion = string.Empty;

            //Clear out the current contents of the listbox
            lbVersion.Items.Clear();

            //Initialize the possible existing versions of Visual Studio 
            dictVSCurrentVersion = VSUtils.PopulateVSExistingVersion();

            //Initialize the possible supported versions of Visual Studio 
            dictVSVersion = VSUtils.PopulateVSSupportedVersion();


            //Check if the existing element already exists in the collection
            if (dictVSCurrentVersion.ContainsKey(version))
            {
                vsCurrentVersion = dictVSCurrentVersion[version];
            }//if

            //Remove any elements which are invalid conversion options
            dictVSVersion.Remove(version);

            //Loop through the available valid conversion options
            //and add the items to the listbox for display
            foreach (var vsVersion in dictVSVersion.Keys)
            {
                lbVersion.Items.Add(dictVSVersion[vsVersion]);
            }//foreach

            //Set the label text to display the currently selected version Visual Studio project/solution file
            lbMessage.Text = vsCurrentVersion;
        } 
        #endregion

        #region Event Handlers
        /// <summary>
        /// Select the version of Visual Studio for conversion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbVersion_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            double v = 0;
            int ending = 0;

            // chop off any previous ending of the message
            ending = lbMessage.Text.IndexOf("solution");
            lbMessage.Text = lbMessage.Text.Substring(0, ending + 8);

            v = double.Parse(lbVersion.SelectedItem.ToString().Substring(21, 3), System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            //INSTANT C# NOTE: The following VB 'Select Case' included either a non-ordinal switch expression or non-ordinal, range-type, or non-constant 'Case' expressions and was converted to C# 'if-else' logic:
            //			Select Case v
            //ORIGINAL LINE: Case 8.0
            if (v == 8.0)
            {
                ConvertTo = Versions.Version8;
                lbMessage.Text += " to VS2005 (v8.0)";
            }
            //ORIGINAL LINE: Case 9.0
            else if (v == 9.0)
            {
                ConvertTo = Versions.Version9;
                lbMessage.Text += " to VS2008 (v9.0)";
            }
            //ORIGINAL LINE: Case 10.0
            else if (v == 10.0)
            {
                ConvertTo = Versions.Version10;
                lbMessage.Text += " to VS2010 (v10.0)";
            }
            else if (v == 11.0)
            {
                ConvertTo = Versions.Version11;
                lbMessage.Text += " to VS2012 (v11.0)";
            }
            //ORIGINAL LINE: Case Else
            else
            {
                // not likely
                MessageBox.Show("Not a valid choice", "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Event handler for the Conversion
        /// to convert the Visual Studio Solution as well as all of the associated projects
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bnConvert_Click(object sender, System.EventArgs e)
        {

            double ExistingVersion = 0;
            int convertedProjects = 0;

            // A quick sanity check
            if (tbSolutionFile.Text == "")
            {
                MessageBox.Show("Please enter a valid Visual Studio solution file", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (ConvertTo == Versions.NotSelected || lbVersion.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter a select a version to convert to", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Let's make sure this is valid solution file
            try
            {
                ExistingVersion = this.SolutionFormatVersion;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading the solution file" + "\r" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Did you ask to make a backup copy?
            if (cbBackup.Checked)
            {
                try
                {
                    ConvertVSProjects.MakeBackup(tbSolutionFile.Text, ExistingVersion);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not create backup file" + "\r" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            VSSolutionInfo vsSolnInfo = new VSSolutionInfo(tbSolutionFile.Text);


            //Read the properties of the Visual Studio Solution file
            VSSolutionInfo convertedSolnInfo = VSSolutionInfo.ConvertVSSolution(vsSolnInfo, ConvertTo);


            //Convert all of the projects in the solution
            foreach (var projFile in convertedSolnInfo.SupportedProjectConvList)
            {
                //Perform a backup of the project file prior to attempting the conversion
                if (cbBackup.Checked)
                {
                    ConvertVSProjects.MakeBackup(projFile, ExistingVersion);
                }//if

                // convert the VB/C# project files
                if (ConvertVSProjects.ConvertProject(projFile, ConvertTo, chkRemoveScc.Checked))
                {
                    convertedSolnInfo.ConvertedProjectCount++;
                }//if

                //TODO: Log the errors to a log file instead
                //MessageBox.Show("Could not convert project file" + "\r" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }//foreach

            //Set the value to the total converted projects
            convertedProjects = convertedSolnInfo.ConvertedProjectCount;

            //Convert the Visual Studio Solution file
            convertedSolnInfo.ConvertVSSolution();

            // Tell 'em what we did
            string strMessage = string.Format("Converted 1 solution and {0} project file{1} out of a total of {2} project{3} in the solution", convertedProjects,
                ((convertedProjects > 1) ? "s" : "").ToString(),
                convertedSolnInfo.TotalProjectCount,
                ((convertedSolnInfo.TotalProjectCount > 1) ? "s" : "").ToString());
            MessageBox.Show(strMessage, "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
           

            // reload the version (in case you want to hit the convert button a 2nd time)
            LoadChoices(this.SolutionFormatVersion);


            // if started from a shell extension, then close when we're done
            if (Environment.GetCommandLineArgs().Length == 2)
            {
                this.Close();
            }
        }


        /// <summary>
        /// Event handler for the Form Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fmMain_Load(object sender, System.EventArgs e)
        {
            // if started via a shell extension
            if (Environment.GetCommandLineArgs().Length == 2)
            {
                tbSolutionFile.Text = Environment.GetCommandLineArgs()[1];

                // Let's make sure this is a valid solution file
                try
                {
                    PopulateSolutionInfo(tbSolutionFile.Text);
                    LoadChoices(this.SolutionFormatVersion);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error reading the solution file" + "\r" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }//if

            //Default the Initial directory for the Open File Dialog for the DevEnv.exe and TF.exe path
            ofdDevEnv.InitialDirectory = Path.Combine(FileOps.GetProgramFilesPath(), m_vsDevEnvDir);
            ofdTFExe.InitialDirectory = Path.Combine(FileOps.GetProgramFilesPath(), m_vsDevEnvDir);
        }

        /// <summary>
        /// Event handler for the Browse button
        /// to find a solution file for conversion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bnBrowse_Click(object sender, System.EventArgs e)
        {
            ofdMain.AddExtension = true;
            ofdMain.Filter = "Microsoft Visual Studio Solution File (*.sln)|*.sln|All files (*.*)|*.*";
            if (ofdMain.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbSolutionFile.Text = ofdMain.FileName;
                PopulateSolutionInfo(tbSolutionFile.Text);

                try
                {
                    LoadChoices(this.SolutionFormatVersion);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error reading the solution file" + "\r" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        /// <summary>
        /// Event handler for browsing to Visual Studio DevEnv.exe path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdBrowseDevEnv_Click(object sender, EventArgs e)
        {
            if (ofdDevEnv.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtDevEnvPath.Text = ofdDevEnv.FileName;
            }//if
        }//event: cmdBrowseDevEnv_Click

        /// <summary>
        /// Event handler for browsing to Visual Studio Solution Path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdVSSolnPath_Click(object sender, EventArgs e)
        {
            if (ofdMain.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtVSSolnPath.Text = ofdMain.FileName;
            }//if
        }

        private void cmdUpgradeVSSoln_Click(object sender, EventArgs e)
        {
            string strStdOutput = string.Empty;

            //Remove Read Only attributes from entire solution directory and subdirectories
            FileOps.RemoveReadOnlyAttributes(txtVSSolnPath.Text, out strStdOutput);

            //If the TFS Checkout checkbox is checked
            //if (chkTFSCheckout.Checked)
            //{
            //    //Build the TFS Checkout Arguments
            //    string strTFExeArgs = TFSOps.BuildTFSCheckoutArgs(txtVSSolnPath.Text, txtTFSUserName.Text, txtTFSPassword.Text);
            //    //Launch the TF.exe process to perform the file checkout
            //    TFSOps.LaunchTFExeProcess(txtTFExePath.Text, strTFExeArgs);
            //}//if

            string strErrOutput = UpgradeVSProjects.DevEnvUpgrade(txtDevEnvPath.Text, txtVSSolnPath.Text, out strStdOutput);

            if (string.IsNullOrEmpty(strErrOutput))
            {
                lbUpgradeStatus.Text = "Solution Upgraded Successfully.";
            }//if
            else
            {
                lbUpgradeStatus.Text = strErrOutput;
            }
        }

        //private void cmdBrowseTFExe_Click(object sender, EventArgs e)
        //{
        //    if (ofdTFExe.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        txtTFExePath.Text = ofdTFExe.FileName;
        //    }//if
        //} 
        #endregion

        private void cmdBrowseMVCExe_Click(object sender, EventArgs e)
        {
            if (ofdMVCExe.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtMVCConverterExe.Text = ofdMVCExe.FileName;
            }//if
        }

        private void cmdLaunchMVCExe_Click(object sender, EventArgs e)
        {
            //Launch the MVCAppConverter Executable
            Process.Start(txtMVCConverterExe.Text);
        }

    

    
	}

}