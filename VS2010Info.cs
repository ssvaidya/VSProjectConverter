using System;

namespace ProjectConverter
{
    public class Vs2010Info: VsProjectVersionInfo, IVsIinfo, IVsVersionInfo
    {
        /// <summary>
        /// The Tools Version for Visual Studio 2010 is always 4.0
        /// </summary>
        const string TOOLS_VERSION = "4.0";

        public override string OldToolsVersion
        {
            get
            {
                return Settings.Default.VS2010_OldToolsVersion;
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
                return string.Empty;
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        /// <summary>
        /// Gets and sets the available Target Framework Versions 
        /// for Visual Studio 2010
        /// </summary>
        /// <remarks>Visual Studio 2010 supports .Net 2.0, .Net 3.0,
        /// .Net 3.5 and .Net 4.0 out of the box</remarks>
        public override string TargetFrameworkVersion
        {
            get
            {
                return Settings.Default.VS2010_TargetFramework;
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
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override string MaxFrameworkVersion
        {
            get
            {
                return "v4.0";
            }
        }//property

        public override string CheckFrameworkVersion(string strOldFrameworkVersion, string defaultFrameworkVersion = "v2.0")
        {
            return base.CheckFrameworkVersion(strOldFrameworkVersion, defaultFrameworkVersion);
        }
        
    }
}
