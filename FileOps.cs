using System;
using System.IO;
using System.Diagnostics;

namespace ProjectConverter
{
    public class FileOps
    {
        #region File IO operations

        /// <summary>
        /// Removes Read Only attributes from Files and Folders using the Attrib command
        /// </summary>
        /// <param name="strDirPath"></param>
        /// <param name="strStdOutput"></param>
        /// <returns></returns>
        public static string RemoveReadOnlyAttributes(string strFullPath, out string strStdOutput)
        {
            var strDirPath = Directory.GetParent(strFullPath).FullName;

            var procInfo = new ProcessStartInfo(@"C:\windows\system32\attrib.exe");
            procInfo.Arguments = string.Format("{0} {1}", "-R", "/S /D");
            procInfo.WorkingDirectory = strDirPath;
            procInfo.UseShellExecute = false; //required to use RedirectStandardOutput property
            procInfo.RedirectStandardOutput = true;
            procInfo.RedirectStandardError = true;

            var p = new Process();
            p.StartInfo = procInfo;
            p.Start();

            // Read the output stream first and then wait.
            strStdOutput = p.StandardOutput.ReadToEnd();
            var errOutput = p.StandardError.ReadToEnd();
            p.WaitForExit();

            return errOutput;
        }//method: RemoveReadOnlyAttributes()

        /// <summary>
        /// Determines whether or not a specified file is read-only
        /// </summary>
        /// <param name="strFilePath">string containing the path to the file</param>
        /// <returns>boolean indicating whether or not the file is set with the read-only flag</returns>
        public static bool IsReadOnlyFile(string strFilePath)
        {

            if (File.GetAttributes(strFilePath) == FileAttributes.ReadOnly)
            {
                return true;
            } // if
            else
            {
                return false;
            } // else


        }//method: IsReadOnlyFile

        /// <summary>
        /// Removes the read only flag from the specified file
        /// </summary>
        /// <param name="strFilePath">string containing the path to the file</param>
        /// <remarks>Files may be read-only if a Get operation was performed on a source control provider
        /// without a check-out operation ex: Get Latest Version from TFS only returns a read-only copy of the file</remarks>
        public static void RemoveReadOnlyFlag(string strFilePath)
        {
            if (IsReadOnlyFile(strFilePath))
            {
                File.SetAttributes(strFilePath, FileAttributes.Normal);
            }//if
        }//method: RemoveReadOnlyFlag 

        /// <summary>
        /// Gets the appropriate Program Files path for a 32-bit processes 
        /// based on whether or not the underlying OS is a 64-bit or 32-bit OS
        /// </summary>
        /// <returns>string containing the appropriate 32-bit Program Files directory</returns>
        /// <remarks>the 64-bit Program Files directory on a 64-bit OS will simply be Environment.SpecialFolder.ProgramFiles</remarks>
        public static string GetProgramFilesPath()
        {
            var strProgramFilesPath = string.Empty;

            if (Environment.Is64BitOperatingSystem)
            {
                strProgramFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            }//if
            else
            {
                //The OS is 32-bit
                strProgramFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            }//else

            return strProgramFilesPath;
        }//method: GetProgramFilesPath

        #endregion
    }
}
