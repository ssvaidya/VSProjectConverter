namespace ProjectConverter
{
    public class VsProjectCreator
    {
        /// <summary>
        /// Factory Method to determine the appropriate version 
        /// of the Visual Studio Project
        /// </summary>
        /// <param name="convertTo">enumeration containing the version of Visual Studio
        /// for conversion</param>
        /// <returns>class instance of the IVSInfo interface</returns>
        public static VsProjectVersionInfo VsProjectFactory(Versions convertTo)
        {
            switch (convertTo)
            {
                case Versions.Version9:
                    return new Vs2008Info();
                case Versions.Version10:
                    return new Vs2010Info();
                case Versions.Version11:
                    return new Vs2012Info();
                case Versions.Version12:
                    return new Vs2013Info();
                default:
                    return new Vs2010Info();
            }//switch

        }//method: VSProjectFactory()
    }
}
