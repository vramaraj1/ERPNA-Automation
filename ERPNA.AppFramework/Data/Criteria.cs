namespace ERPNA.AppFramework
{
    /// <summary>
    /// Provides access to all the known "Criteria" for the application. Criteria are typically used when waiting
    /// for elements. I often wait until some "Criteria" is met before continuing with a test.
    /// </summary>
    public static class Criteria
    {
        public static readonly HomePageCriteria HomePage = new HomePageCriteria();
        public static readonly LoginPageCriteria LoginPage = new LoginPageCriteria();
        


    }
}