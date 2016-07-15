using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ProjectConverter
{
    public class ConvertVSProjects
    {


        /// <summary>
        /// Makes a backup copy of the existing Visual Studio project file
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="ExistingVersion"></param>
        public static void MakeBackup(string FileName, double ExistingVersion)
        {
            string Backup = string.Empty;

            Dictionary<double, string> dictVSVersionString = new Dictionary<double, string>();

            //Get the list of available Visual Studio version numbers
            dictVSVersionString = VSUtils.PopulateVSVersionString();

            if (dictVSVersionString.ContainsKey(ExistingVersion))
            {
                string strVSVersion = dictVSVersionString[ExistingVersion];
                Backup = Path.Combine(Path.GetDirectoryName(FileName), string.Format("{0}{1}{2}", Path.GetFileNameWithoutExtension(FileName), strVSVersion, Path.GetExtension(FileName)));
            }//if
            else
            {
                // not likely
                MessageBox.Show("Not a supported version", "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            File.Copy(FileName, Backup, true);
        }

        #region Project Conversion Methods
        #region Convert Visual C# and VB.Net projects
        /// <summary>
        /// Convert VB.Net and C# Visual Studio project files
        /// </summary>
        /// <param name="ProjFile"></param>
        /// <param name="ConvertTo"></param>
        /// <param name="blnRemoveSccBindings"></param>
        /// <returns></returns>
        public static bool ConvertProject(string ProjFile, Versions ConvertTo, bool blnRemoveSccBindings = false)
        {

            //Declare the base namespace for Visual Studio project files
            const string VSProjNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";
            const string TargetFrameworkNode = "TargetFrameworkVersion";
            const string PropertyGroupNode = "PropertyGroup";
            const string ToolsNode = "ToolsVersion";
            const string ProductVersionNode = "ProductVersion";
            const string OldToolsVersionNode = "OldToolsVersion";

            //Get all of the settings for the appropriate version of Visual Studio
            var vsProjectVersionInfo = VSProjectCreator.VSProjectFactory(ConvertTo);

            //Read in the settings from the existing Visual Studio project file
            VSProjectInfo vsProjectInfo = new VSProjectInfo(ProjFile);

            //Load the Xml into a XElement
            XElement xmlVSProjFile = XElement.Load(ProjFile);

            //Declare the namespace for the Visual Studio project file
            XNamespace xns = VSProjNamespace;

            //Query for all of the necessary elements
            var oldToolsElement = GetVSProjElement(xmlVSProjFile, OldToolsVersionNode);

            //Use a Linq query to obtain the TargetFrameworkVersion
            var targetFrameworkElement = GetVSProjElement(xmlVSProjFile, TargetFrameworkNode);

            var productVersionElement = GetVSProjElement(xmlVSProjFile, ProductVersionNode);

            var toolsVersionAttrib = xmlVSProjFile.Attribute("ToolsVersion");

            //Get the 1st PropertyGroup in the project
            var propGroup = xmlVSProjFile.Element(xns + PropertyGroupNode);
            


            //Make sure that the root element of the file is a Visual Studio project file
            if (xmlVSProjFile.Name.LocalName.Equals("Project"))
            {
                //TODO: Re-analyze this logic since it is not true in all cases, esp. VS 2012
                if (toolsVersionAttrib != null)
                {
                    // converting to VS2008, but project is already at VS2008
                    if (ConvertTo == Versions.Version9 && toolsVersionAttrib.Value.Equals(vsProjectVersionInfo.ToolsVersion))
                    {
                        // exit quietly
                        return false;
                    }
                    // converting to VS2010, but project is already at VS2010
                    if (ConvertTo == Versions.Version10 && toolsVersionAttrib.Value.Equals(vsProjectVersionInfo.ToolsVersion))
                    {
                        // exit quietly
                        return false;
                    }//if
                }
                else
                {
                    // If converting to VS2005, but project is already at VS2005
                    if (ConvertTo == Versions.Version8)
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
            string strOldToolsVersion = string.Empty;

            //Local variable for storing the existing Target Framework Version
            var existingTargetFrameworkVersion = string.Empty;
            
            switch (ConvertTo)
            {
                case Versions.Version8:
                    // it gets removed
                    xmlVSProjFile.Attribute(ToolsNode).Remove();
                    //OldToolsVersion
                    oldToolsElement.Remove();
                    //TargetFrameworkVersion
                    targetFrameworkElement.Remove();
                    //Product Version
                    //Product Version element does not exist in VS 2012
                    if (productVersionElement.Count() == 0)
                    {
                        propGroup.Add(new XElement(xns + ProductVersionNode, Settings.Default.VS2005_Version));
                    }
                    else if (productVersionElement.Count() > 0)
                    {
                        productVersionElement.ElementAtOrDefault(0).Value = Settings.Default.VS2005_Version;
                    }
                    
                    break;
                case Versions.Version9:
                    if (toolsVersionAttrib != null)
                    {
                        xmlVSProjFile.SetAttributeValue(ToolsNode, vsProjectVersionInfo.ToolsVersion); 
                    }//if
                    else
                    {
                        // add the attribute
                        xmlVSProjFile.Add(new XAttribute(ToolsNode, vsProjectVersionInfo.ToolsVersion));    
                    }

                    //Product Version element does not exist in VS 2012
                    if (productVersionElement.Count() == 0)
                    {
                        propGroup.Add(new XElement(xns + ProductVersionNode, vsProjectVersionInfo.ProductVersion));
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
                        propGroup.Add(new XElement(xns + OldToolsVersionNode, strOldToolsVersion));
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
                        propGroup.Add(new XElement(xns + TargetFrameworkNode, existingTargetFrameworkVersion));
                    }//else

                    break;
                case Versions.Version10:
                    if (toolsVersionAttrib != null)
                    {
                        xmlVSProjFile.SetAttributeValue(ToolsNode, vsProjectInfo.ToolsVersion);
                    }//if
                    else
                    {
                        // add the attribute
                        xmlVSProjFile.Add(new XAttribute(ToolsNode, vsProjectVersionInfo.ToolsVersion));
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
                        propGroup.Add(new XElement(xns + OldToolsVersionNode, strOldToolsVersion));
                    }//else

                      existingTargetFrameworkVersion = vsProjectVersionInfo.CheckFrameworkVersion(vsProjectInfo.TargetFrameworkVersion);
                    //Set the target Framework Version
                    if (targetFrameworkElement.ElementAtOrDefault(0) != null)
                    {
                        targetFrameworkElement.ElementAtOrDefault(0).Value = existingTargetFrameworkVersion;
                    }//if
                    else
                    {
                        propGroup.Add(new XElement(xns + TargetFrameworkNode, existingTargetFrameworkVersion));
                    }//else
                    break;
                case Versions.Version11:
                    if (toolsVersionAttrib != null)
                    {
                        xmlVSProjFile.SetAttributeValue(ToolsNode, vsProjectVersionInfo.ToolsVersion);
                    }//if
                    else
                    {
                        // add the attribute
                        xmlVSProjFile.Add(new XAttribute(ToolsNode, vsProjectVersionInfo.ToolsVersion));
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
                        propGroup.Add(new XElement(xns + OldToolsVersionNode, strOldToolsVersion));
                    }//else

                      existingTargetFrameworkVersion = vsProjectVersionInfo.CheckFrameworkVersion(vsProjectInfo.TargetFrameworkVersion);
                    //Set the target Framework Version
                    if (targetFrameworkElement.ElementAtOrDefault(0) != null)
                    {
                        targetFrameworkElement.ElementAtOrDefault(0).Value = existingTargetFrameworkVersion;
                    }//if
                    else
                    {
                        propGroup.Add(new XElement(xns + TargetFrameworkNode, existingTargetFrameworkVersion));
                    }//else
                    break;
            }//switch

            // The MSBuildToolsPath vs MSBuildBinPath environmental variable.  Oddly enough a fully patched VS2005
            // uses the newer MSBuildToolsPath.  So, this should only be required if you don't have VS2005 SP1 installed.
            // However, I can't detect that, so we take the worst case scenario, and use the older version
            var vsImportElement = from vsImportElements in xmlVSProjFile.Elements()
							where vsImportElements.Name.LocalName.StartsWith("Import")
							&& vsImportElements.FirstAttribute.Name.LocalName.Equals("Project")
							&& vsImportElements.FirstAttribute.Value.StartsWith("$(MS")
							select vsImportElements;

            //Project should always be the first attribute for the Import element
            var msBuildPathValue = vsImportElement.ElementAtOrDefault(0).FirstAttribute.Value;
            string strMSBuildPath = string.Empty;

            if (ConvertTo >= Versions.Version9)
            {
                // convert it to the newer MSBuildToolsPath
                if (msBuildPathValue.Contains("MSBuildBinPath"))
                {
                    strMSBuildPath = msBuildPathValue.Replace("MSBuildBinPath", "MSBuildToolsPath");
                    vsImportElement.ElementAtOrDefault(0).FirstAttribute.Value = strMSBuildPath;
                }//if
            }//if
            else
            {
                // convert it to the older MSBuildBinPath
                if (msBuildPathValue.Contains("MSBuildToolsPath"))
                {
                    strMSBuildPath = msBuildPathValue.Replace("MSBuildToolsPath", "MSBuildBinPath");
                    vsImportElement.ElementAtOrDefault(0).FirstAttribute.Value = strMSBuildPath;
                }//if
            }//else

            //Check if there is a requirement to remove the Scc Bindings
            //from the Visual Studio project file
            if (blnRemoveSccBindings)
            {
                
                //Obtain a handle to the Source Control elements in the Visual Studio project file
                var sccProjElement =
                    from sccElements in xmlVSProjFile.Elements(xns + "PropertyGroup").Elements()
                    where sccElements.Name.LocalName.StartsWith("Scc")
                    select sccElements;

                //Remove the SccElements from the collection
                sccProjElement.Remove();
            }//if

            //Attempt to save the changes back to the Visual Studio file
            try
            {
                //Save the changes back to the Visual Studio project file
                xmlVSProjFile.Save(ProjFile);
            }//try
            catch (UnauthorizedAccessException ex)
            {
                FileOps.RemoveReadOnlyFlag(ProjFile);
                
                //Save the changes back to the Visual Studio project file
                xmlVSProjFile.Save(ProjFile);
            } // catch
            

            return true;
        }


        private static IEnumerable<XElement> GetVSProjElement(XElement xmlVSProjFile, string vsProjNode)
        {
            const string PropertyGroupNode = "PropertyGroup";
            XNamespace xns = "http://schemas.microsoft.com/developer/msbuild/2003";

            var vsProjElement = from vsProjElements in xmlVSProjFile.Elements(xns + PropertyGroupNode).Elements()
                                where vsProjElements.Name.LocalName.StartsWith(vsProjNode)
                                select vsProjElements;
            return vsProjElement;
        }

        #endregion
        #endregion
    }
}
