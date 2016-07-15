using System;
using System.Linq;

namespace ProjectConverter
{
    /// <summary>
    /// Abstract class to be used for all version specific implementations
    /// of Visual Studio
    /// </summary>
    public abstract class VSProjectVersionInfo: IVSVersionInfo
    {

        public virtual string OldToolsVersion { get; set; }
        public virtual string TargetFrameworkVersion { get; set; }
        public virtual string ToolsVersion { get; set; }
        public virtual string ProductVersion { get; set; }
        public virtual string ProjectVersion { get; set; }

        /// <summary>
        /// Gets the Maximum Target Framework version
        /// supported by the specific release of Visual Studio
        /// </summary>
        public virtual string MaxFrameworkVersion
        {
            get
            {
                return "v3.5";
            }//get
        }//property

        /// <summary>
        /// Checks the Target Framework version to both retain existing framework
        /// versions when possible and ensure compatibility
        /// with the targeted release of Visual Studio
        /// </summary>
        /// <param name="strOldFrameworkVersion"></param>
        /// <param name="defaultFrameworkVersion"></param>
        /// <returns></returns>
        /// <remarks>Since the tool supports both forward and backward conversions of Visual Studio versions
        /// additional checks need to be made to ensure that the Targeted Framework Versions are compatible with the 
        /// specific targeted release versions of Visual Studio</remarks>
        public virtual string CheckFrameworkVersion(string strOldFrameworkVersion, string defaultFrameworkVersion = "v2.0")
        {
            string strSupportedFrameworkVersion = string.Empty;
            double dblOldFrameworkVersion, dblMaxFrameworkVersion;

            if (string.IsNullOrEmpty(strOldFrameworkVersion))
            {
                strSupportedFrameworkVersion = defaultFrameworkVersion;
            }//if
            else if (!string.IsNullOrEmpty(strOldFrameworkVersion))
            {
                //Remove the "v" from the beginning string of the framework version
                dblOldFrameworkVersion = Convert.ToDouble(strOldFrameworkVersion.Remove(0, 1));
                dblMaxFrameworkVersion = Convert.ToDouble(this.MaxFrameworkVersion.Remove(0, 1));

                //If the version of the .Net Framework is greater than the maximum Framework version
                //supported by that version of Visual Studio
                if (dblOldFrameworkVersion > dblMaxFrameworkVersion)
                {
                    //Leave the existing .Net Framework version intact
                    strSupportedFrameworkVersion = this.MaxFrameworkVersion;
                } //if
                else
                {
                    strSupportedFrameworkVersion = strOldFrameworkVersion;
                } //else
            }//else if

            return strSupportedFrameworkVersion;
        }

    }
}
