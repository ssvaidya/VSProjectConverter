using System;

namespace ProjectConverter
{
    public class Vs2008Info: VsProjectVersionInfo
    {
        /// <summary>
        /// The Tools Version for Visual Studio 2008 is always 3.5
        /// </summary>
        const string TOOLS_VERSION = "3.5";

        public override string OldToolsVersion
        {
            get
            {
                return Settings.Default.VS2008_OldToolsVersion;
            }//get
            set
            {
                throw new NotImplementedException();
            }//set
        }

        public override string ProductVersion
        {
            get
            {
                return Settings.Default.VS2008_Version;
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        /// <summary>
        /// Gets and sets the available Target Framework Versions 
        /// for Visual Studio 2008
        /// </summary>
        /// <remarks>Visual Studio 2010 supports .Net 2.0, .Net 3.0,
        /// and .Net 3.5</remarks>
        public override string TargetFrameworkVersion
        {
            get
            {
                return Settings.Default.VS2008_TargetFramework;
            }//get
            set
            {
                throw new NotImplementedException();
            }//set
        }

        public override string ToolsVersion
        {
            get
            {
                return TOOLS_VERSION;
            }//get
            set
            {
                throw new NotImplementedException();
            }
        }

        public override string  MaxFrameworkVersion
        {
            get 
            { 
	            return "v3.5";
            }
        }//property

        public override string CheckFrameworkVersion(string strOldFrameworkVersion, string defaultFrameworkVersion = "v2.0")
        {
            return base.CheckFrameworkVersion(strOldFrameworkVersion, defaultFrameworkVersion);
        }

       
    }
}
