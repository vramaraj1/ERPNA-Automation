using Browser.Core.Framework;
using ERPNA.AppFramework;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;


namespace ERPNA.UITest
{
    /// <summary>
    /// Extending BrowserTest. Handles setup and configuration for all of selenium tests to run tests against multiple
    /// web browsers (Chrome, Firefox, Edge).
    /// </summary>
    public abstract class TestBase : BrowserTest
    {
        public IWebDriver browser;
        public ERPNAHelperMethods Help = new ERPNAHelperMethods();
        

        #region Constructors
        // Local Selenium Test
        public TestBase(string browserName, string emulationDevice) : base(browserName, emulationDevice) { }

        // Remote Selenium Grid Test
        public TestBase(string browserName, string emulationDevice, string version, string platform, string hubUri, string extrasUri)
                                    : base(browserName, emulationDevice, version , platform, hubUri, extrasUri) { }
        #endregion Constructors

        /// <summary>
        /// Use this to override TestSetup in BrowserTest to perform setup logic that should occur before EVERY TEST inside this project. 
        /// Can use TestTearDown also
        /// </summary>
        /*public override void TestSetup()
        {
            base.TestSetup();
            browser = base.Browser;

            // Uncomment the below line of code to debug any build server resolution issues
            //Browser.Manage().Window.Size = new System.Drawing.Size(1040, 784);
        }*/

        public override void BeforeTest()
        {
            base.BeforeTest();
            browser = base.Browser;

            // Uncomment the below line of code to debug any build server resolution issues
            //Browser.Manage().Window.Size = new System.Drawing.Size(1040, 784);
        }
        public override void AfterTest()
        {
            base.AfterTest();
 
        }

        // See AppSettings.CS in Browser Core. We are going to set the config there for now. If there are limitations or better designs
        // for using it here instead, we will do it here, but for now, it lives in AppSettings.cs
        //public IConfiguration Config { get { return GetConfig(); } }
        //    public IConfiguration GetConfig()
        //    {
        //        Startup blah = new Startup(new ConfigurationBuilder().SetBasePath(TestContext.CurrentContext.TestDirectory)
        //.AddJsonFile("appsettings.json")
        //.Build());
        //        return blah.Configuration;
        //    }
    }
}