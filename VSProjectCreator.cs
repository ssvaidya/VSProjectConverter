using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectConverter
{
    public class VSProjectCreator
    {
        /// <summary>
        /// Factory Method to determine the appropriate version 
        /// of the Visual Studio Project
        /// </summary>
        /// <param name="ConvertTo">enumeration containing the version of Visual Studio
        /// for conversion</param>
        /// <returns>class instance of the IVSInfo interface</returns>
        public static VSProjectVersionInfo VSProjectFactory(Versions ConvertTo)
        {
            switch (ConvertTo)
            {
                case Versions.Version9:
                    return new VS2008Info();
                case Versions.Version10:
                    return new VS2010Info();
                case Versions.Version11:
                    return new VS2012Info();
                case Versions.Version12:
                    return new VS2013Info();
                default:
                    return new VS2010Info();
            }//switch

        }//method: VSProjectFactory()
    }
}
