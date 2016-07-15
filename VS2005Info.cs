using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectConverter
{
    public class VS2005Info: IVSIinfo 
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
