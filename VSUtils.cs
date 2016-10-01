using System.Collections.Generic;

namespace ProjectConverter
{
    public static class VsUtils
    {
        public static Dictionary<double, string> PopulateVsExistingVersion()
        {
            var dictVsCurrentVersion = new Dictionary<double, string>
            {
                {9.0, "Convert existing VS2005 (v8.0) solution"},
                {10.0, "Convert existing VS2008 (v9.0) solution"},
                {11.0, "Convert existing VS2010 (v10.0) solution"},
                {12.0, "Convert existing VS2012 (v11.0) solution"},
                {13.0, "Convert existing VS2013 (v11.0) solution"}
            };

            //Initialize the possible existing versions of Visual Studio 

            return dictVsCurrentVersion;
        }

        public static Dictionary<double, string> PopulateVsSupportedVersion()
        {
            var dictVsVersion = new Dictionary<double, string>
            {
                {9.0, "Visual Studio 2005 (v8.0)"},
                {10.0, "Visual Studio 2008 (v9.0)"},
                {11.0, "Visual Studio 2010 (v10.0)"},
                {12.0, "Visual Studio 2012 (v11.0)"},
                {13.0, "Visual Studio 2013 (v11.0)"}
            };

            //Initialize the possible supported versions of Visual Studio 

            return dictVsVersion;
        }

        public static Dictionary<double, string> PopulateVsVersionString()
        {
            var dictVsVersionString = new Dictionary<double, string>
            {
                {9.0, "v8"},
                {10.0, "v9"},
                {11.0, "v10"},
                {12.0, "v11"},
                {13.0, "v12"}
            };

            //Initialize the possible supported versions of Visual Studio 

            return dictVsVersionString;
        }

        public static string GetProjectPart(string projPartString)
        {
            const char doubleQuoteChar = (char) (34);

            var vsProjPart = projPartString.Trim().Trim(doubleQuoteChar);

            return vsProjPart;
        }
    }
}