using System;
using System.IO;
using System.Collections.Generic;

namespace ProjectConverter
{
    public class VsSolutionInfo
    {
        private double _mVsVersion;
        private const string VbProjExt = ".vbproj";
        private const string CSharpProjExt = ".csproj";

        private List<string> _mSupportedProjList;
        private List<VsProjectFileInfo> _mProjList;

        /// <summary>
        /// Class constructor
        /// </summary>
        public VsSolutionInfo(string strFilePath)
        {
            this.SolnFilePath = strFilePath;
            _mSupportedProjList = new List<string>();
            _mProjList = new List<VsProjectFileInfo>();
            //Check the validity of the solution
            _mVsVersion = CheckValidSolution(strFilePath);
        }//constructor

        public string SolnFilePath
        {
            get;
            set;
        }//property: SolnFilePath

        public string SolnDirectory
        {
            get
            {
                return Path.GetDirectoryName(this.SolnFilePath);
            }//get
        }//property: SolnDirectory

        public string SolnFileFormatHeader { get; set; }

        public string SolnFileVersionHeader { get; set; }

        /// <summary>
        /// Gets and sets the Solution Format version
        /// </summary>
        public double FormatVersion
        {
            get
            {
                return _mVsVersion;
            }
            set
            {
                _mVsVersion = value;
            }//set
        }

        public int TotalProjectCount
        {
            get
            {
                return _mProjList.Count;
            }//get
        }//property: TotalProjectCount

        public int ConvertedProjectCount
        {
            get;
            set;
        }//property: ConvertedProjectCount

        /// <summary>
        /// Provides a collection of all Visual Studio
        /// projects in the Solution
        /// </summary>
        public List<VsProjectFileInfo> ProjectList
        {
            get
            {
                return _mProjList;
            }//get
        }//property: ProjectList

        /// <summary>
        /// Provides a collection of Supported
        /// Visual Studio Project Conversions
        /// </summary>
        public List<string> SupportedProjectConvList
        {
            get
            {
                return _mSupportedProjList;
            }//get
            private set
            {
                _mSupportedProjList = value;
            }//set
        }//property: ProjectList

        /// <summary>
        /// Gets the Solution file "format version".  
        /// Also verifies we have a valid solution file
        /// </summary>
        /// <param name="filePath">string containing the file path to the Solution File</param>
        /// <returns>double containing the Solution Format Version</returns>
        public double GetFormatVersion(string filePath)
        {
            string buf = null;
            var linecount = 0;
            var ans = 0.0;

            //Remove the Read-Only Flag from the Solution file if it exists
            FileOps.RemoveReadOnlyFlag(filePath);

            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                var sr = new StreamReader(fs);


                while (!sr.EndOfStream)
                {
                    buf = sr.ReadLine();

                    // is this a valid header?
                    if (buf.StartsWith("Microsoft Visual Studio Solution File, Format Version"))
                    {
                        ans = double.Parse(buf.Substring(54), System.Globalization.CultureInfo.GetCultureInfo("en-US"));
                        break;
                    }

                    linecount += 1;
                    // A valid solution should have a header at the 1st or 2nd line
                    if (linecount > 2)
                    {
                        break;
                    }
                }

                sr.Close();
                fs.Close();
            }//using

            //INSTANT C# NOTE: The following VB 'Select Case' included either a non-ordinal switch expression or non-ordinal, range-type, or non-constant 'Case' expressions and was converted to C# 'if-else' logic:
            //			Select Case ans
            //ORIGINAL LINE: Case 0
            if (ans == 0)
            {
                throw new ApplicationException("This doesn't appear to be a valid solution file");
            }
            //ORIGINAL LINE: Case 7.0
            else if (ans == 7.0)
            {
                throw new ApplicationException("Conversion of VS.Net (v7.0) solutions is not supported");
            }
            //ORIGINAL LINE: Case 8.0
            else if (ans == 8.0)
            {
                throw new ApplicationException("Conversion of VS2003 (v7.1) solutions is not supported");
            }

            return ans;
        }//method: GetFormatVersion


        /// <summary>
        /// Checks whether or not the solution is a valid solution file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>double containing the solution's format version</returns>
        public double CheckValidSolution(string filePath)
        {
            string buf = null;
            var linecount = 0;
            var ans = 0.0;
            
            //Remove the Read-Only Flag from the Solution file if it exists
            FileOps.RemoveReadOnlyFlag(filePath);

            ParseVsSolutionFile();
            ans = this.FormatVersion;

            
            if (ans == 0)
            {
                throw new ApplicationException("This doesn't appear to be a valid solution file");
            }
            else if (ans == 7.0)
            {
                throw new ApplicationException("Conversion of VS.Net (v7.0) solutions is not supported");
            }
            else if (ans == 8.0)
            {
                throw new ApplicationException("Conversion of VS2003 (v7.1) solutions is not supported");
            }

            return ans;
        }//method: CheckValidSolution

        /// <summary>
        /// Parses through the Visual Studio Solution file
        /// to populate all of the required properties
        /// </summary>
        public void ParseVsSolutionFile()
        {
            FileStream fs = null;
            StreamReader sr = null;
            BinaryReader br = null;
            string buf = null;
            byte[] bom = null;

            // First we read the solution file and build a list of 
            // project files that we find inside.  We need both a binary
            // reader and stream reader.
            using (fs = new FileStream(this.SolnFilePath, FileMode.Open))
            {
                sr = new StreamReader(fs);
                br = new BinaryReader(fs);

                // let's read Unicode Byte Order Mark (with CRLF)
                bom = br.ReadBytes(5);

                // if we don't have a BOM, we create a default one
                if (bom[0] != 0XEF)
                {
                    bom[0] = 0XEF;
                    bom[1] = 0XBB;
                    bom[2] = 0XBF;
                    bom[3] = 0XD;
                    bom[4] = 0XA;

                    // rewind the streamreaders
                    fs.Seek(0, SeekOrigin.Begin);
                }

                while (sr.Peek() >= 0)
                {
                    buf = sr.ReadLine();

                    // no need for any fancy parsing routines
                    if (buf.StartsWith("Microsoft Visual Studio Solution File, Format Version"))
                    {

                        //int intFormatIdx = buf.IndexOf("Format");
                        //this.SolnFileFormatHeader = buf.Substring(intFormatIdx);
                        this.SolnFileFormatHeader = buf;
                        this.FormatVersion = double.Parse(buf.Substring(VsSolutionFormat.FormatHeader.Length), System.Globalization.CultureInfo.GetCultureInfo("en-US"));
                        
                        continue;
                    }//if

                    if (buf.StartsWith("# Visual"))
                    {
                        //const string starting_Chars = "# ";
                        //this.SolnFileVersionHeader = buf.Remove(0, starting_Chars.Length);
                        this.SolnFileVersionHeader = buf;
                        continue;
                    }//if
                    

                    // parse the project files
                    if (buf.StartsWith("Project("))
                    {
                        string[] projLine = null;
                        string[] projParts = null;
                        //Create a new instance of the class to store the Project File information
                        var vsProjFileInfo = new VsProjectFileInfo();

                        //Split the entire project string using the equal separator
                        projLine = buf.Split('=');

                        //Split the second part of the project string using the comma separator
                        projParts = projLine[1].Split(',');

                        //TODO: Is there ever a scenario where there are more or less than 3 parts??
                        vsProjFileInfo.ProjectId = VsUtils.GetProjectPart(projLine[0]);
                        vsProjFileInfo.Name = VsUtils.GetProjectPart(projParts[0]);
                        vsProjFileInfo.ProjectRelativePath = VsUtils.GetProjectPart(projParts[1]);
                        vsProjFileInfo.Guid = VsUtils.GetProjectPart(projParts[2]);

                        vsProjFileInfo.ProjectFullPath = Path.Combine(this.SolnDirectory, vsProjFileInfo.ProjectRelativePath);
                        vsProjFileInfo.ProjectDirectory = Path.GetDirectoryName(vsProjFileInfo.ProjectFullPath);
                        vsProjFileInfo.ProjectExtension = Path.GetExtension(vsProjFileInfo.ProjectFullPath);
                        vsProjFileInfo.ProjectFileName = string.Concat(vsProjFileInfo.Name, vsProjFileInfo.ProjectExtension);

                        //Add the project file info to the collection
                        _mProjList.Add(vsProjFileInfo);

                        // we only support VB.Net and C# project files
                        if (vsProjFileInfo.ProjectExtension.Equals(VbProjExt) || vsProjFileInfo.ProjectExtension.Equals(CSharpProjExt))
                        {
                            //Add all of the supported project conversions to the List
                            this.SupportedProjectConvList.Add(vsProjFileInfo.ProjectFullPath);
                        }//if
                    }
                }
                fs.Close();
            }//using
        }


        /// <summary>
        /// Converts the Visual Studio Solution from one version to another
        /// </summary>
        /// <param name="objVsSolnInfo" type="ProjectConverter.VSSolutionInfo">
        ///     <para>
        ///         
        ///     </para>
        /// </param>
        /// <param name="convertTo" type="ProjectConverter.Versions">
        ///     <para>
        ///         
        ///     </para>
        /// </param>
        /// <returns>
        ///     A ProjectConverter.VSSolutionInfo value...
        /// </returns>
        public static VsSolutionInfo ConvertVsSolution(VsSolutionInfo objVsSolnInfo, Versions convertTo)
        {
            var vsSolnInfo = new VsSolutionInfo(objVsSolnInfo.SolnFilePath);

            var vsSolnFormat = VsSolutionFormat.VsSolutionFormatFactory((int) convertTo);
            

            switch (convertTo)
            {
                case Versions.Version8:
                    vsSolnInfo.SolnFileVersionHeader = vsSolnFormat.SolnHeader;
                    vsSolnInfo.SolnFileFormatHeader = vsSolnFormat.SolnFormat;
                    break;
                case Versions.Version9:
                    vsSolnFormat = VsSolutionFormat.VsSolutionFormatFactory(9);
                    vsSolnInfo.SolnFileVersionHeader = vsSolnFormat.SolnHeader;
                    vsSolnInfo.SolnFileFormatHeader = vsSolnFormat.SolnFormat;
                    break;
                case Versions.Version10:
                    vsSolnFormat = VsSolutionFormat.VsSolutionFormatFactory(10);
                    vsSolnInfo.SolnFileVersionHeader = vsSolnFormat.SolnHeader;
                    vsSolnInfo.SolnFileFormatHeader = vsSolnFormat.SolnFormat;
                    break;
                case Versions.Version11:
                    vsSolnFormat = VsSolutionFormat.VsSolutionFormatFactory(11);
                    vsSolnInfo.SolnFileVersionHeader = vsSolnFormat.SolnHeader;
                    vsSolnInfo.SolnFileFormatHeader = vsSolnFormat.SolnFormat;
                    break;
                case Versions.Version12:
                    vsSolnFormat = VsSolutionFormat.VsSolutionFormatFactory(11);
                    vsSolnInfo.SolnFileVersionHeader = vsSolnFormat.SolnHeader;
                    vsSolnInfo.SolnFileFormatHeader = vsSolnFormat.SolnFormat;
                    break;
            }//switch

            return vsSolnInfo;
        }//method: ConvertVSSolution()

        /// <summary>
        /// Performs the conversion of the Visual Studio Solution
        /// </summary>
        public void ConvertVsSolution()
        {
            FileStream fs = null;
            var arrLines = new List<string>();

            //Remove Read-Only Flag from Solution
            FileOps.RemoveReadOnlyFlag(this.SolnFilePath);

            // OK now it's time to save the converted Solution file
            using (fs = new FileStream(this.SolnFilePath, FileMode.Open))
            {
                var sr = new StreamReader(fs);

                while (sr.Peek() >= 0)
                {
                    var strLine = sr.ReadLine();

                    if (strLine.StartsWith(VsSolutionFormat.FormatHeader))
                    {
                        arrLines.Add(this.SolnFileFormatHeader);
                    }//if
                    else if (strLine.StartsWith("# Visual"))
                    {
                        arrLines.Add(this.SolnFileVersionHeader);
                    }//if
                    else
                    {
                        arrLines.Add(strLine);
                    }//else

                }//while

                //Close the file stream
                fs.Close();
            }//using

            using (var sw = new StreamWriter(this.SolnFilePath, false))
            {
                foreach (var arrLine in arrLines)
                {
                    sw.WriteLine(arrLine);
                }//foreach
            }//using
        }//method: ConvertVSSolution

       
    }

    /// <summary>
    /// Stores the Solution Format information
    /// </summary>
    internal class VsSolutionFormat
    {
        private const string Vs2005SolnHeader = "# Visual Studio 2005";
        private const string Vs2008SolnHeader = "# Visual Studio 2008";
        private const string Vs2010SolnHeader = "# Visual Studio 2010";
        private const string Vs2012SolnHeader = "# Visual Studio 11";
        public const string FormatHeader = "Microsoft Visual Studio Solution File, Format Version";



        public double SolnId { get; set; }
        public string SolnFormat { get; set; }
        public string SolnHeader { get; set; }

        public static VsSolutionFormat VsSolutionFormatFactory(int solnNumber)
        {
            var vsSolnFormat = new VsSolutionFormat() {SolnId = (double)solnNumber};
            vsSolnFormat.SolnFormat = string.Format("{0} {1}", FormatHeader, vsSolnFormat.SolnId);

            switch (solnNumber)
            {
                case 9:
                    vsSolnFormat.SolnHeader = Vs2005SolnHeader;
                    break;
                case 10:
                    vsSolnFormat.SolnHeader = Vs2008SolnHeader;
                    break;
                case 11:
                    vsSolnFormat.SolnHeader = Vs2010SolnHeader;
                    break;
                case 12:
                    vsSolnFormat.SolnHeader = Vs2012SolnHeader;
                    break;
            }//switch
            return vsSolnFormat;
        }//method: CreateVSSolutionFormat()
    }
}
