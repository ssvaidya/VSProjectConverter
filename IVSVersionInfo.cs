namespace ProjectConverter
{
    public interface IVsVersionInfo: IVsIinfo
    {
        string OldToolsVersion { get; set; }
        string TargetFrameworkVersion { get; set; }
        string ToolsVersion { get; set; }
    }
}
