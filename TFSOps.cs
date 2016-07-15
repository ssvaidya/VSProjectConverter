using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ProjectConverter
{
    public class TFSOps
    {
        private const string PAD_QUOTES = @"""";

        /// <summary>
        /// Launches the TF.exe process to be able to execute commands against TFS
        /// </summary>
        /// <param name="strTFExePath"></param>
        /// <param name="strTFExeArgs"></param>
        public static void LaunchTFExeProcess(string strTFExePath, string strTFExeArgs)
        {
            ProcessStartInfo objProcInfo = new ProcessStartInfo();

            objProcInfo.Arguments = strTFExeArgs;
            objProcInfo.FileName = strTFExePath;
            objProcInfo.UseShellExecute = true;

            Process.Start(objProcInfo);
        }//method: LaunchTFExeProcess

        /// <summary>
        /// Builds the appropriate arguments for checking out a file from TFS
        /// </summary>
        /// <param name="strTFSFilePath"></param>
        /// <param name="TFSUserName"></param>
        /// <param name="TFSPassword"></param>
        /// <returns></returns>
        public static string BuildTFSCheckoutArgs(string strTFSFilePath, string TFSUserName, string TFSPassword)
        {

            string strFilePath = PAD_QUOTES + strTFSFilePath + PAD_QUOTES;
            string strArguments = string.Format("checkout /lock:none /login:{0},{1} {2}", TFSUserName, TFSPassword,
                                                strFilePath);

            return strArguments;
        }//method: BuildTFSCheckoutArgs

        /// <summary>
        /// Builds the appropriate arguments for checking a file into TFS
        /// </summary>
        /// <param name="strTFSFilePath"></param>
        /// <param name="TFSUserName"></param>
        /// <param name="TFSPassword"></param>
        /// <param name="strTFSCheckInComment"></param>
        /// <returns></returns>
        public static string BuildTFSCheckinArgs(string strTFSFilePath,
            string TFSUserName, string TFSPassword, string strTFSCheckInComment)
        {



            string strFilePath = PAD_QUOTES + strTFSFilePath + PAD_QUOTES;
            string strTFSComment = PAD_QUOTES + strTFSCheckInComment + PAD_QUOTES;
            string strArguments = string.Format("checkin /comment:{0} /noprompt /login:{1},{2} {3}",strTFSComment, TFSUserName,TFSPassword, strFilePath);

            return strArguments;
        }//method: BuildTFSCheckArgs
    }
}
