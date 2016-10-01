namespace ProjectConverter
{
    /// <summary>
    /// This class contains attribute information regarding the actual Visual Studio Project File
    /// such as File Path, Guid attributes etc.
    /// </summary>
    public class VsProjectFileInfo
    {
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public string Guid { get; set; }
        public string ProjectFileName { get; set; }
        public string ProjectFullPath { get; set; }
        public string ProjectDirectory { get; set; }
        public string ProjectRelativePath { get; set; }
        public string ProjectExtension { get; set; }
    }
}
