using System;
using System.Linq;
using System.IO;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

namespace ProjectConverter
{
    public class VSSolutionInfo
    {
        private double m_vsVersion;
        private const string VBProjExt = ".vbproj";
        private const string CSharpProjExt = ".csproj";

        private List<string> m_supportedProjList;
        private List<VSProjectFileInfo> m_projList;

        /// <summary>
        /// Class constructor
        /// </summary>
        public VSSolutionInfo(string strFilePath)
        {
            this.SolnFilePath = strFilePath;
            m_supportedProjList = new List<string>();
            m_projList = new List<VSProjectFileInfo>();
            //Check the validity of the solution
            m_vsVersion = CheckValidSolution(strFilePath);
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
                return m_vsVersion;
            }
            set
            {
                m_vsVersion = value;
            }//set
        }

        public int TotalProjectCount
        {
            get
            {
                return m_projList.Count;
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
        public List<VSProjectFileInfo> ProjectList
        {
            get
            {
                return m_projList;
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
                return m_supportedProjList;
            }//get
            private set
            {
                m_supportedProjList = value;
            }//set
        }//property: ProjectList

        /// <summary>
        /// Gets the Solution file "format version".  
        /// Also verifies we have a valid solution file
        /// </summary>
        /// <param name="FilePath">string containing the file path to the Solution File</param>
        /// <returns>double containing the Solution Format Version</returns>
        public double GetFormatVersion(string FilePath)
        {
            string buf = null;
            int linecount = 0;
            double ans = 0.0;

            //Remove the Read-Only Flag from the Solution file if it exists
            FileOps.RemoveReadOnlyFlag(FilePath);

            using (FileStream fs = new FileStream(FilePath, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);


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
        /// <param name="FilePath"></param>
        /// <returns>double containing the solution's format version</returns>
        public double CheckValidSolution(string FilePath)
        {
            string buf = null;
            int linecount = 0;
            double ans = 0.0;
            
            //Remove the Read-Only Flag from the Solution file if it exists
            FileOps.RemoveReadOnlyFlag(FilePath);

            ParseVSSolutionFile();
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
        public void ParseVSSolutionFile()
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
                        this.FormatVersion = double.Parse(buf.Substring(VSSolutionFormat.Format_Header.Length), System.Globalization.CultureInfo.GetCultureInfo("en-US"));
                        
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
                        string[] ProjLine = null;
                        string[] ProjParts = null;
                        //Create a new instance of the class to store the Project File information
                        VSProjectFileInfo vsProjFileInfo = new VSProjectFileInfo();

                        //Split the entire project string using the equal separator
                        ProjLine = buf.Split('=');

                        //Split the second part of the project string using the comma separator
                        ProjParts = ProjLine[1].Split(',');

                        //TODO: Is there ever a scenario where there are more or less than 3 parts??
                        vsProjFileInfo.ProjectID = VSUtils.GetProjectPart(ProjLine[0]);
                        vsProjFileInfo.Name = VSUtils.GetProjectPart(ProjParts[0]);
                        vsProjFileInfo.ProjectRelativePath = VSUtils.GetProjectPart(ProjParts[1]);
                        vsProjFileInfo.Guid = VSUtils.GetProjectPart(ProjParts[2]);

                        vsProjFileInfo.ProjectFullPath = Path.Combine(this.SolnDirectory, vsProjFileInfo.ProjectRelativePath);
                        vsProjFileInfo.ProjectDirectory = Path.GetDirectoryName(vsProjFileInfo.ProjectFullPath);
                        vsProjFileInfo.ProjectExtension = Path.GetExtension(vsProjFileInfo.ProjectFullPath);
                        vsProjFileInfo.ProjectFileName = string.Concat(vsProjFileInfo.Name, vsProjFileInfo.ProjectExtension);

                        //Add the project file info to the collection
                        m_projList.Add(vsProjFileInfo);

                        // we only support VB.Net and C# project files
                        if (vsProjFileInfo.ProjectExtension.Equals(VBProjExt) || vsProjFileInfo.ProjectExtension.Equals(CSharpProjExt))
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
        /// <param name="objVSSolnInfo" type="ProjectConverter.VSSolutionInfo">
        ///     <para>
        ///         
        ///     </para>
        /// </param>
        /// <param name="ConvertTo" type="ProjectConverter.Versions">
        ///     <para>
        ///         
        ///     </para>
        /// </param>
        /// <returns>
        ///     A ProjectConverter.VSSolutionInfo value...
        /// </returns>
        public static VSSolutionInfo ConvertVSSolution(VSSolutionInfo objVSSolnInfo, Versions ConvertTo)
        {
            VSSolutionInfo vsSolnInfo = new VSSolutionInfo(objVSSolnInfo.SolnFilePath);

            VSSolutionFormat vsSolnFormat = VSSolutionFormat.VSSolutionFormatFactory((int) ConvertTo);
            

            switch (ConvertTo)
            {
                case Versions.Version8:
                    vsSolnInfo.SolnFileVersionHeader = vsSolnFormat.SolnHeader;
                    vsSolnInfo.SolnFileFormatHeader = vsSolnFormat.SolnFormat;
                    break;
                case Versions.Version9:
                    vsSolnFormat = VSSolutionFormat.VSSolutionFormatFactory(9);
                    vsSolnInfo.SolnFileVersionHeader = vsSolnFormat.SolnHeader;
                    vsSolnInfo.SolnFileFormatHeader = vsSolnFormat.SolnFormat;
                    break;
                case Versions.Version10:
                    vsSolnFormat = VSSolutionFormat.VSSolutionFormatFactory(10);
                    vsSolnInfo.SolnFileVersionHeader = vsSolnFormat.SolnHeader;
                    vsSolnInfo.SolnFileFormatHeader = vsSolnFormat.SolnFormat;
                    break;
                case Versions.Version11:
                    vsSolnFormat = VSSolutionFormat.VSSolutionFormatFactory(11);
                    vsSolnInfo.SolnFileVersionHeader = vsSolnFormat.SolnHeader;
                    vsSolnInfo.SolnFileFormatHeader = vsSolnFormat.SolnFormat;
                    break;
                case Versions.Version12:
                    vsSolnFormat = VSSolutionFormat.VSSolutionFormatFactory(11);
                    vsSolnInfo.SolnFileVersionHeader = vsSolnFormat.SolnHeader;
                    vsSolnInfo.SolnFileFormatHeader = vsSolnFormat.SolnFormat;
                    break;
            }//switch

            return vsSolnInfo;
        }//method: ConvertVSSolution()

        /// <summary>
        /// Performs the conversion of the Visual Studio Solution
        /// </summary>
        public void ConvertVSSolution()
        {
            FileStream fs = null;
            List<string> arrLines = new List<string>();

            //Remove Read-Only Flag from Solution
            FileOps.RemoveReadOnlyFlag(this.SolnFilePath);

            // OK now it's time to save the converted Solution file
            using (fs = new FileStream(this.SolnFilePath, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);

                while (sr.Peek() >= 0)
                {
                    string strLine = sr.ReadLine();

                    if (strLine.StartsWith(VSSolutionFormat.Format_Header))
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

            using (StreamWriter sw = new StreamWriter(this.SolnFilePath, false))
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
    internal class VSSolutionFormat
    {
        private const string VS2005SolnHeader = "# Visual Studio 2005";
        private const string VS2008SolnHeader = "# Visual Studio 2008";
        private const string VS2010SolnHeader = "# Visual Studio 2010";
        private const string VS2012SolnHeader = "# Visual Studio 11";
        public const string Format_Header = "Microsoft Visual Studio Solution File, Format Version";



        public double SolnID { get; set; }
        public string SolnFormat { get; set; }
        public string SolnHeader { get; set; }

        public static VSSolutionFormat VSSolutionFormatFactory(int SolnNumber)
        {
            VSSolutionFormat vsSolnFormat = new VSSolutionFormat() {SolnID = (double)SolnNumber};
            vsSolnFormat.SolnFormat = string.Format("{0} {1}", Format_Header, vsSolnFormat.SolnID);

            switch (SolnNumber)
            {
                case 9:
                    vsSolnFormat.SolnHeader = VS2005SolnHeader;
                    break;
                case 10:
                    vsSolnFormat.SolnHeader = VS2008SolnHeader;
                    break;
                case 11:
                    vsSolnFormat.SolnHeader = VS2010SolnHeader;
                    break;
                case 12:
                    vsSolnFormat.SolnHeader = VS2012SolnHeader;
                    break;
            }//switch
            return vsSolnFormat;
        }//method: CreateVSSolutionFormat()
    }
}
