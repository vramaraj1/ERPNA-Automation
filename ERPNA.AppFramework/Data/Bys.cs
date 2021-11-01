namespace ERPNA.AppFramework
{
    /// <summary>
    /// Provides access to all known "Bys" for the application. Bys are used to locate elements
    /// </summary>
    public static class Bys
    {
        public static readonly HomePageBys HomePage = new HomePageBys();
       
        public static readonly ERPNABasePageBys SCMBasePage = new ERPNABasePageBys();
       
        public static readonly LoginPageBys LoginPage = new LoginPageBys();
       



    }
}