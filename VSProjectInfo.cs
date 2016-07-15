using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;


namespace ProjectConverter
{
    public class VSProjectInfo: IVSIinfo
    {
        public const string VSProjNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";

        public VSProjectInfo(string strProjectFilePath)
        {
            ReadProjectFile(strProjectFilePath);
        }

        public string OldToolsVersion
        {
            get;
            set;
        }

        public string ProductVersion
        {
            get;
            set;
        }

        public string ProjectVersion
        {
            get;
            set;
        }

        public string TargetFrameworkVersion
        {
            get;
            set;
        }

        public string ToolsVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Reads an existing Visual Studio project file
        /// </summary>
        /// <param name="strProjectFilePath">string path to a Visual Studio project file</param>
        public void ReadProjectFile(string strProjectFilePath)
        {
            string strTargetFramework = string.Empty, strOldToolsVersion = string.Empty;
            const string TargetFrameworkNode = "TargetFrameworkVersion";
            const string OldToolsVersionNode = "OldToolsVersion";
            const string ProductVersionNode = "ProductVersion";

            XNamespace xProjNS = VSProjNamespace;

            //Load the project file into memory
            XElement xProjElement = XElement.Load(strProjectFilePath);

            //TODO: Determine if there is a better method to LINQ to XML to avoid iterating over entire Xml hierarchy
            foreach (var item in xProjElement.Elements(xProjNS + "PropertyGroup").Descendants())
            {
                //Get the TargetFrameworkVersion Xml Node
                if (item.Name.LocalName.Equals(TargetFrameworkNode))
                {
                    //Get the text contents of the target framework node
                    var xTargetFramework = item;
                    this.TargetFrameworkVersion = item.Value;
                    System.Diagnostics.Debug.WriteLine(item.Value);
                }//if

                //Get the OldToolsVersion Xml Node
                if (item.Name.LocalName.Equals(OldToolsVersionNode))
                {
                    //Get the text contents of the target framework node
                    var xOldToolsVersion = item;
                    this.OldToolsVersion = item.Value;
                    System.Diagnostics.Debug.WriteLine(item.Value);
                }//if

                //Get the ProductVersion Xml Node
                if (item.Name.LocalName.Equals(ProductVersionNode))
                {
                    //Get the text contents of the target framework node
                    var xProductVersionNode = item;
                    this.ProductVersion = item.Value;
                    System.Diagnostics.Debug.WriteLine(item.Value);
                }//if
            }//foreach
        }//method: ReadProjectFile
    }
}
