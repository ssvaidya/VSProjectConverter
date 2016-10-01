using System.Xml.Linq;


namespace ProjectConverter
{
    public class VsProjectInfo: IVsIinfo
    {
        public const string VsProjNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";

        public VsProjectInfo(string strProjectFilePath)
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
            const string targetFrameworkNode = "TargetFrameworkVersion";
            const string oldToolsVersionNode = "OldToolsVersion";
            const string productVersionNode = "ProductVersion";

            XNamespace xProjNs = VsProjNamespace;

            //Load the project file into memory
            var xProjElement = XElement.Load(strProjectFilePath);

            //TODO: Determine if there is a better method to LINQ to XML to avoid iterating over entire Xml hierarchy
            foreach (var item in xProjElement.Elements(xProjNs + "PropertyGroup").Descendants())
            {
                //Get the TargetFrameworkVersion Xml Node
                if (item.Name.LocalName.Equals(targetFrameworkNode))
                {
                    //Get the text contents of the target framework node
                    var xTargetFramework = item;
                    this.TargetFrameworkVersion = item.Value;
                    System.Diagnostics.Debug.WriteLine(item.Value);
                }//if

                //Get the OldToolsVersion Xml Node
                if (item.Name.LocalName.Equals(oldToolsVersionNode))
                {
                    //Get the text contents of the target framework node
                    var xOldToolsVersion = item;
                    this.OldToolsVersion = item.Value;
                    System.Diagnostics.Debug.WriteLine(item.Value);
                }//if

                //Get the ProductVersion Xml Node
                if (item.Name.LocalName.Equals(productVersionNode))
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
