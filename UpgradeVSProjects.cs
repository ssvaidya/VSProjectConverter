using System.Diagnostics;

namespace ProjectConverter
{
    public class UpgradeVsProjects
    {
        /// <summary>
        /// Executes the Visual Studio Solution Upgrade
        /// by directly invoking the Visual Studio devenv.exe executable
        /// and passing the /Upgrade switch
        /// </summary>
        /// <param name="strDevEnvExePath">string path to the Visual Studio devenv.exe executable</param>
        /// <param name="strVsSolnPath">string path to the Visual Studio solution</param>
        /// <param name="strStdOutput">out parameter for standard output</param>
        public static string DevEnvUpgrade(string strDevEnvExePath, string strVsSolnPath, out string strStdOutput)
        {
            var procInfo = new ProcessStartInfo(strDevEnvExePath);
            procInfo.Arguments = string.Format("{0} {1}", "/Upgrade", strVsSolnPath);
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
        }//method: DevEnvUpgrade()
    }
}
