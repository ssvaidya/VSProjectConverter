using System.Diagnostics;

namespace ProjectConverter
{
    public class TfsOps
    {
        private const string PadQuotes = @"""";

        /// <summary>
        /// Launches the TF.exe process to be able to execute commands against TFS
        /// </summary>
        /// <param name="strTfExePath"></param>
        /// <param name="strTfExeArgs"></param>
        public static void LaunchTfExeProcess(string strTfExePath, string strTfExeArgs)
        {
            var objProcInfo = new ProcessStartInfo
            {
                Arguments = strTfExeArgs,
                FileName = strTfExePath,
                UseShellExecute = true
            };


            Process.Start(objProcInfo);
        }//method: LaunchTFExeProcess

        /// <summary>
        /// Builds the appropriate arguments for checking out a file from TFS
        /// </summary>
        /// <param name="strTfsFilePath"></param>
        /// <param name="tfsUserName"></param>
        /// <param name="tfsPassword"></param>
        /// <returns></returns>
        public static string BuildTfsCheckoutArgs(string strTfsFilePath, string tfsUserName, string tfsPassword)
        {

            var strFilePath = PadQuotes + strTfsFilePath + PadQuotes;
            var strArguments = string.Format("checkout /lock:none /login:{0},{1} {2}", tfsUserName, tfsPassword,
                                                strFilePath);

            return strArguments;
        }//method: BuildTFSCheckoutArgs

        /// <summary>
        /// Builds the appropriate arguments for checking a file into TFS
        /// </summary>
        /// <param name="strTfsFilePath"></param>
        /// <param name="tfsUserName"></param>
        /// <param name="tfsPassword"></param>
        /// <param name="strTfsCheckInComment"></param>
        /// <returns></returns>
        public static string BuildTfsCheckinArgs(string strTfsFilePath,
            string tfsUserName, string tfsPassword, string strTfsCheckInComment)
        {



            var strFilePath = PadQuotes + strTfsFilePath + PadQuotes;
            var strTfsComment = PadQuotes + strTfsCheckInComment + PadQuotes;
            var strArguments = string.Format("checkin /comment:{0} /noprompt /login:{1},{2} {3}",strTfsComment, tfsUserName,tfsPassword, strFilePath);

            return strArguments;
        }//method: BuildTFSCheckArgs
    }
}
