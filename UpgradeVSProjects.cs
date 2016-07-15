using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ProjectConverter
{
    public class UpgradeVSProjects
    {
        /// <summary>
        /// Executes the Visual Studio Solution Upgrade
        /// by directly invoking the Visual Studio devenv.exe executable
        /// and passing the /Upgrade switch
        /// </summary>
        /// <param name="strDevEnvExePath">string path to the Visual Studio devenv.exe executable</param>
        /// <param name="strVSSolnPath">string path to the Visual Studio solution</param>
        /// <param name="strStdOutput">out parameter for standard output</param>
        public static string DevEnvUpgrade(string strDevEnvExePath, string strVSSolnPath, out string strStdOutput)
        {
            ProcessStartInfo procInfo = new ProcessStartInfo(strDevEnvExePath);
            procInfo.Arguments = string.Format("{0} {1}", "/Upgrade", strVSSolnPath);
            procInfo.UseShellExecute = false; //required to use RedirectStandardOutput property
            procInfo.RedirectStandardOutput = true;
            procInfo.RedirectStandardError = true;

            Process p = new Process();
            p.StartInfo = procInfo;
            p.Start();

            // Read the output stream first and then wait.
            strStdOutput = p.StandardOutput.ReadToEnd();
            string errOutput = p.StandardError.ReadToEnd();
            p.WaitForExit();

            return errOutput;
        }//method: DevEnvUpgrade()
    }
}
