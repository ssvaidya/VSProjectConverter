using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ProjectConverter
{
    public static class ConvertVsProjects
    {


        /// <summary>
        /// Makes a backup copy of the existing Visual Studio project file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="existingVersion"></param>
        public static void MakeBackup(string fileName, double existingVersion)
        {
            var backup = string.Empty;

            var dictVsVersionString = new Dictionary<double, string>();

            //Get the list of available Visual Studio version numbers
            dictVsVersionString = VsUtils.PopulateVsVersionString();

            if (dictVsVersionString.ContainsKey(existingVersion))
            {
                var strVsVersion = dictVsVersionString[existingVersion];
                backup = Path.Combine(Path.GetDirectoryName(fileName), string.Format("{0}{1}{2}", Path.GetFileNameWithoutExtension(fileName), strVsVersion, Path.GetExtension(fileName)));
            }//if
            else
            {
                // not likely
                MessageBox.Show("Not a supported version", "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            File.Copy(fileName, backup, true);
        }

        #region Project Conversion Methods
        #region Convert Visual C# and VB.Net projects
        /// <summary>
        /// Convert VB.Net and C# Visual Studio project files
        /// </summary>
        /// <param name="projFile"></param>
        /// <param name="convertTo"></param>
        /// <param name="blnRemoveSccBindings"></param>
        /// <returns></returns>
        public static bool ConvertProject(string projFile, Versions convertTo, bool blnRemoveSccBindings = false)
        {

            //Declare the base namespace for Visual Studio project files
            const string vsProjNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";
            const string targetFrameworkNode = "TargetFrameworkVersion";
            const string propertyGroupNode = "PropertyGroup";
            const string toolsNode = "ToolsVersion";
            const string productVersionNode = "ProductVersion";
            const string oldToolsVersionNode = "OldToolsVersion";

            //Get all of the settings for the appropriate version of Visual Studio
            var vsProjectVersionInfo = VsProjectCreator.VsProjectFactory(convertTo);

            //Read in the settings from the existing Visual Studio project file
            var vsProjectInfo = new VsProjectInfo(projFile);

            //Load the Xml into a XElement
            var xmlVsProjFile = XElement.Load(projFile);

            //Declare the namespace for the Visual Studio project file
            XNamespace xns = vsProjNamespace;

            //Query for all of the necessary elements
            var oldToolsElement = GetVsProjElement(xmlVsProjFile, oldToolsVersionNode);

            //Use a Linq query to obtain the TargetFrameworkVersion
            var targetFrameworkElement = GetVsProjElement(xmlVsProjFile, targetFrameworkNode);

            var productVersionElement = GetVsProjElement(xmlVsProjFile, productVersionNode);

            var toolsVersionAttrib = xmlVsProjFile.Attribute("ToolsVersion");

            //Get the 1st PropertyGroup in the project
            var propGroup = xmlVsProjFile.Element(xns + propertyGroupNode);
            


            //Make sure that the root element of the file is a Visual Studio project file
            if (xmlVsProjFile.Name.LocalName.Equals("Project"))
            {
                //TODO: Re-analyze this logic since it is not true in all cases, esp. VS 2012
                if (toolsVersionAttrib != null)
                {
                    // converting to VS2008, but project is already at VS2008
                    if (convertTo == Versions.Version9 && toolsVersionAttrib.Value.Equals(vsProjectVersionInfo.ToolsVersion))
                    {
                        // exit quietly
                        return false;
                    }
                    // converting to VS2010, but project is already at VS2010
                    if (convertTo == Versions.Version10 && toolsVersionAttrib.Value.Equals(vsProjectVersionInfo.ToolsVersion))
                    {
                        // exit quietly
                        return false;
                    }//if
                }
                else
                {
                    // If converting to VS2005, but project is already at VS2005
                    if (convertTo == Versions.Version8)
                    {
                        // exit quietly
                        return false;
                    }
                }
            }
            else
            {
                // no such node?  That's bad, very bad...
                throw new ApplicationException("Invalid project file");
            }

            // the OldToolsVersion element in the first PropertyGoup
            var strOldToolsVersion = string.Empty;

            //Local variable for storing the existing Target Framework Version
            var existingTargetFrameworkVersion = string.Empty;
            
            switch (convertTo)
            {
                case Versions.Version8:
                    // it gets removed
                    xmlVsProjFile.Attribute(toolsNode).Remove();
                    //OldToolsVersion
                    oldToolsElement.Remove();
                    //TargetFrameworkVersion
                    targetFrameworkElement.Remove();
                    //Product Version
                    //Product Version element does not exist in VS 2012
                    if (productVersionElement.Count() == 0)
                    {
                        propGroup.Add(new XElement(xns + productVersionNode, Settings.Default.VS2005_Version));
                    }
                    else if (productVersionElement.Count() > 0)
                    {
                        productVersionElement.ElementAtOrDefault(0).Value = Settings.Default.VS2005_Version;
                    }
                    
                    break;
                case Versions.Version9:
                    if (toolsVersionAttrib != null)
                    {
                        xmlVsProjFile.SetAttributeValue(toolsNode, vsProjectVersionInfo.ToolsVersion); 
                    }//if
                    else
                    {
                        // add the attribute
                        xmlVsProjFile.Add(new XAttribute(toolsNode, vsProjectVersionInfo.ToolsVersion));    
                    }

                    //Product Version element does not exist in VS 2012
                    if (productVersionElement.Count() == 0)
                    {
                        propGroup.Add(new XElement(xns + productVersionNode, vsProjectVersionInfo.ProductVersion));
                    }
                    else if (productVersionElement.Count() > 0)
                    {
                        productVersionElement.ElementAtOrDefault(0).Value = vsProjectVersionInfo.ProductVersion;        
                    }
                    

                    // add a new element
                    //Use the coalescing operator to determine which value to use
                    strOldToolsVersion = vsProjectInfo.OldToolsVersion ?? vsProjectVersionInfo.OldToolsVersion;
                    if (oldToolsElement.ElementAtOrDefault(0) != null)
                    {
                        oldToolsElement.ElementAtOrDefault(0).Value = strOldToolsVersion;
                    }//if
                    else
                    {
                        propGroup.Add(new XElement(xns + oldToolsVersionNode, strOldToolsVersion));
                    }//else

                     existingTargetFrameworkVersion =
                        vsProjectVersionInfo.CheckFrameworkVersion(vsProjectInfo.TargetFrameworkVersion);
                    //Set the target Framework Version
                    if (targetFrameworkElement.ElementAtOrDefault(0) != null)
                    {
                        targetFrameworkElement.ElementAtOrDefault(0).Value = existingTargetFrameworkVersion;
                    }//if
                    else
                    {
                        propGroup.Add(new XElement(xns + targetFrameworkNode, existingTargetFrameworkVersion));
                    }//else

                    break;
                case Versions.Version10:
                    if (toolsVersionAttrib != null)
                    {
                        xmlVsProjFile.SetAttributeValue(toolsNode, vsProjectInfo.ToolsVersion);
                    }//if
                    else
                    {
                        // add the attribute
                        xmlVsProjFile.Add(new XAttribute(toolsNode, vsProjectVersionInfo.ToolsVersion));
                    }

                    if (productVersionElement.Count() > 0)
                    {
                        //Retain the product version if it exists, otherwise it is not needed for VS 2010 Support
                        productVersionElement.ElementAtOrDefault(0).Value = vsProjectVersionInfo.ProductVersion;    
                    }//if
                    

                    //Use the coalescing operator to determine which value to use
                    strOldToolsVersion = vsProjectInfo.OldToolsVersion ?? vsProjectVersionInfo.OldToolsVersion;
                    if (oldToolsElement.ElementAtOrDefault(0) != null)
                    {
                        oldToolsElement.ElementAtOrDefault(0).Value = strOldToolsVersion;
                    }//if
                    else
                    {
                        propGroup.Add(new XElement(xns + oldToolsVersionNode, strOldToolsVersion));
                    }//else

                      existingTargetFrameworkVersion = vsProjectVersionInfo.CheckFrameworkVersion(vsProjectInfo.TargetFrameworkVersion);
                    //Set the target Framework Version
                    if (targetFrameworkElement.ElementAtOrDefault(0) != null)
                    {
                        targetFrameworkElement.ElementAtOrDefault(0).Value = existingTargetFrameworkVersion;
                    }//if
                    else
                    {
                        propGroup.Add(new XElement(xns + targetFrameworkNode, existingTargetFrameworkVersion));
                    }//else
                    break;
                case Versions.Version11:
                    if (toolsVersionAttrib != null)
                    {
                        xmlVsProjFile.SetAttributeValue(toolsNode, vsProjectVersionInfo.ToolsVersion);
                    }//if
                    else
                    {
                        // add the attribute
                        xmlVsProjFile.Add(new XAttribute(toolsNode, vsProjectVersionInfo.ToolsVersion));
                    }

                    //Product Version information is no longer used in VS 2012
                    productVersionElement.Remove();

                    //Use the coalescing operator to determine which value to use
                    strOldToolsVersion = vsProjectInfo.OldToolsVersion ?? vsProjectVersionInfo.OldToolsVersion;
                    if (oldToolsElement.ElementAtOrDefault(0) != null)
                    {
                        oldToolsElement.ElementAtOrDefault(0).Value = strOldToolsVersion;
                    }//if
                    else
                    {
                        propGroup.Add(new XElement(xns + oldToolsVersionNode, strOldToolsVersion));
                    }//else

                      existingTargetFrameworkVersion = vsProjectVersionInfo.CheckFrameworkVersion(vsProjectInfo.TargetFrameworkVersion);
                    //Set the target Framework Version
                    if (targetFrameworkElement.ElementAtOrDefault(0) != null)
                    {
                        targetFrameworkElement.ElementAtOrDefault(0).Value = existingTargetFrameworkVersion;
                    }//if
                    else
                    {
                        propGroup.Add(new XElement(xns + targetFrameworkNode, existingTargetFrameworkVersion));
                    }//else
                    break;
            }//switch

            // The MSBuildToolsPath vs MSBuildBinPath environmental variable.  Oddly enough a fully patched VS2005
            // uses the newer MSBuildToolsPath.  So, this should only be required if you don't have VS2005 SP1 installed.
            // However, I can't detect that, so we take the worst case scenario, and use the older version
            var vsImportElement = from vsImportElements in xmlVsProjFile.Elements()
							where vsImportElements.Name.LocalName.StartsWith("Import")
							&& vsImportElements.FirstAttribute.Name.LocalName.Equals("Project")
							&& vsImportElements.FirstAttribute.Value.StartsWith("$(MS")
							select vsImportElements;

            //Project should always be the first attribute for the Import element
            var msBuildPathValue = vsImportElement.ElementAtOrDefault(0).FirstAttribute.Value;
            var strMsBuildPath = string.Empty;

            if (convertTo >= Versions.Version9)
            {
                // convert it to the newer MSBuildToolsPath
                if (msBuildPathValue.Contains("MSBuildBinPath"))
                {
                    strMsBuildPath = msBuildPathValue.Replace("MSBuildBinPath", "MSBuildToolsPath");
                    vsImportElement.ElementAtOrDefault(0).FirstAttribute.Value = strMsBuildPath;
                }//if
            }//if
            else
            {
                // convert it to the older MSBuildBinPath
                if (msBuildPathValue.Contains("MSBuildToolsPath"))
                {
                    strMsBuildPath = msBuildPathValue.Replace("MSBuildToolsPath", "MSBuildBinPath");
                    vsImportElement.ElementAtOrDefault(0).FirstAttribute.Value = strMsBuildPath;
                }//if
            }//else

            //Check if there is a requirement to remove the Scc Bindings
            //from the Visual Studio project file
            if (blnRemoveSccBindings)
            {
                
                //Obtain a handle to the Source Control elements in the Visual Studio project file
                var sccProjElement =
                    from sccElements in xmlVsProjFile.Elements(xns + "PropertyGroup").Elements()
                    where sccElements.Name.LocalName.StartsWith("Scc")
                    select sccElements;

                //Remove the SccElements from the collection
                sccProjElement.Remove();
            }//if

            //Attempt to save the changes back to the Visual Studio file
            try
            {
                //Save the changes back to the Visual Studio project file
                xmlVsProjFile.Save(projFile);
            }//try
            catch (UnauthorizedAccessException ex)
            {
                FileOps.RemoveReadOnlyFlag(projFile);
                
                //Save the changes back to the Visual Studio project file
                xmlVsProjFile.Save(projFile);
            } // catch
            

            return true;
        }


        private static IEnumerable<XElement> GetVsProjElement(XElement xmlVsProjFile, string vsProjNode)
        {
            const string propertyGroupNode = "PropertyGroup";
            XNamespace xns = "http://schemas.microsoft.com/developer/msbuild/2003";

            var vsProjElement = from vsProjElements in xmlVsProjFile.Elements(xns + propertyGroupNode).Elements()
                                where vsProjElements.Name.LocalName.StartsWith(vsProjNode)
                                select vsProjElements;
            return vsProjElement;
        }

        #endregion
        #endregion
    }
}
