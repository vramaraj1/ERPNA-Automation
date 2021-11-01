using OpenQA.Selenium;

namespace ERPNA.AppFramework
{
    /// <summary>
    /// This is where we locate our elements. Please use standard naming conventions and group your elements as shown below. Standard naming 
    /// conventions are defined here: https://code.premierinc.com/docs/display/PGHLMSDOCS/Best+Practices
    /// </summary>
    public class ERPNABasePageBys
    {
        // Banners


        // Buttons        
        public readonly By RefreshBtn = By.Id("butRefresh");

        // Charts

        // Check boxes

        // Frames

        // images
        public readonly By UserProfileImg = By.XPath("//img[@alt='User Menu Icon']");



        // Labels    

        // Links
        public readonly By HelpLnk = By.XPath("//td[text()='Help']");
        public readonly By MenuLnk = By.XPath("//span[text()='Menu']");
        public readonly By SearchLnk = By.XPath("//td[text()='Search']");
        //public readonly By SignoutLnk = By.XPath("//a[contains(text(),'Sign out')]");
        public readonly By SignoutLnk = By.XPath("//div[@class='main-menu-options']");

        
        public readonly By AnalysisWorkbenchLnk = By.XPath("(//div[text()='Analysis Workbench'])[1]");

        public readonly By SpendAnalysisLnk = By.XPath("(//div[text()='Spend Analysis'])[1]");








        //Menu Items    

        // Radio buttons

        // Tables       

        // Tabs

        // Text boxes
        //public readonly By SearchTxt = By.Id("searchInput");

        // Icons
        //This icon xpath need to be verified 
        //public readonly By HelpIcon = By.XPath("//div[text()='?']");














    }
}