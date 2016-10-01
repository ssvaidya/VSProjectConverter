using System;

namespace ProjectConverter
{
    public class Vs2005Info: IVsIinfo 
    {
      
        public string ProductVersion
        {
            get
            {
                return Settings.Default.VS2005_Version;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ProjectVersion
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
