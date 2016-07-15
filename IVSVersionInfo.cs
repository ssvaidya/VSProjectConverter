using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectConverter
{
    public interface IVSVersionInfo: IVSIinfo
    {
        string OldToolsVersion { get; set; }
        string TargetFrameworkVersion { get; set; }
        string ToolsVersion { get; set; }
    }
}
