using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectConverter
{
    public class VSUtils
    {
        public static Dictionary<double, string> PopulateVSExistingVersion()
        {
            Dictionary<double, string> dictVSCurrentVersion = new Dictionary<double, string>();

            //Initialize the possible existing versions of Visual Studio 
            dictVSCurrentVersion.Add(9.0, "Convert existing VS2005 (v8.0) solution");
            dictVSCurrentVersion.Add(10.0, "Convert existing VS2008 (v9.0) solution");
            dictVSCurrentVersion.Add(11.0, "Convert existing VS2010 (v10.0) solution");
            dictVSCurrentVersion.Add(12.0, "Convert existing VS2012 (v11.0) solution");
            dictVSCurrentVersion.Add(13.0, "Convert existing VS2013 (v11.0) solution");

            return dictVSCurrentVersion;
        }

        public static Dictionary<double, string> PopulateVSSupportedVersion()
        {
            Dictionary<double, string> dictVSVersion = new Dictionary<double, string>();

            //Initialize the possible supported versions of Visual Studio 
            dictVSVersion.Add(9.0, "Visual Studio 2005 (v8.0)");
            dictVSVersion.Add(10.0, "Visual Studio 2008 (v9.0)");
            dictVSVersion.Add(11.0, "Visual Studio 2010 (v10.0)");
            dictVSVersion.Add(12.0, "Visual Studio 2012 (v11.0)");
            dictVSVersion.Add(13.0, "Visual Studio 2013 (v11.0)");

            return dictVSVersion;
        }

        public static Dictionary<double, string>  PopulateVSVersionString()
        {
            Dictionary<double, string> dictVSVersionString = new Dictionary<double, string>();

            //Initialize the possible supported versions of Visual Studio 
            dictVSVersionString.Add(9.0, "v8");
            dictVSVersionString.Add(10.0, "v9");
            dictVSVersionString.Add(11.0, "v10");
            dictVSVersionString.Add(12.0, "v11");
            dictVSVersionString.Add(13.0, "v12");

            return dictVSVersionString;
        }

        public static string GetProjectPart(string projPartString)
        {
            char doubleQuoteChar = (char)(34);

            string vsProjPart = projPartString.Trim().Trim(doubleQuoteChar);

            return vsProjPart;
        }//method: GetProjectPart
            

            

            
    }
}
