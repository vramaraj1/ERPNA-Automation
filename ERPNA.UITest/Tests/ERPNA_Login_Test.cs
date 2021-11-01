using Browser.Core.Framework;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using System;
using System.Data;
using Browser.Core.Framework.Data;
using ERPNA.AppFramework;

/// <summary>
/// The below test class will represent the Aperek User Login
/// Be sure to go inside of each method (Right click on the method name, then click "Go To Definition") to view the code 
/// and to view more comment explanations. Do this for Page class methods and other "SCM.AppFramework" code, and even code 
/// inside of the Browser.Core.Framework project (Our lowest layer of framework that handles the setup/configuration/teardown of 
/// the Browser itself, and also many shared utilities that can be used across different applications. i.e. "ElemGet", which is 
/// used inside this test class)
/// </summary>
namespace ERPNA.UITest
{
    /// <summary>
    /// The below lines of code represent remote or local test fixtures that show up in your Test Explorer window depending
    /// on if they are commented out or not. By including these test fixtures, you can run on any browser, and can also run
    /// on a mobile device via emulation inside Chrome. Note that inside the Test Explorer window, the remote instances will 
    /// have the double quotations in parenthesis, while local instances do not
    /// </summary>
    [LocalSeleniumTestFixture(BrowserNames.Chrome)]
    [LocalSeleniumTestFixture(BrowserNames.Edge)]

    //[LocalSeleniumTestFixture(BrowserNames.Firefox)]
    //[LocalSeleniumTestFixture(BrowserNames.InternetExplorer)]
    //[LocalSeleniumTestFixture(BrowserNames.Chrome, EmulationDevices.iPhoneX)]
    //[RemoteSeleniumTestFixture(BrowserNames.Chrome, "", "", "Windows")]
    //[RemoteSeleniumTestFixture(BrowserNames.Edge, "", "", "Windows")]
    //[RemoteSeleniumTestFixture(BrowserNames.Edge, "", "", "Windows")]
    //[RemoteSeleniumTestFixture(BrowserNames.Firefox)]
    //[RemoteSeleniumTestFixture(BrowserNames.InternetExplorer)]
    //[RemoteSeleniumTestFixture(BrowserNames.Chrome, EmulationDevices.iPhoneX, "", Platforms.Windows, "", "")]


    [TestFixture]
    public class ERPNA_Login_Test : TestBase
    {
        #region Constructors

        /// <summary>
        /// The below 2 constructors represent local versus remote. The remote constructor is the second one. Notice it has more
        /// parameters defined. You will see this represented on the Test Explorer window. The local instances in that window
        /// will only have 1 parameter (Browser name), the remote will have multiple empty double quote parameters
        /// </summary>
        public ERPNA_Login_Test(string browserName, string emulationDevice) : base(browserName, emulationDevice) { }
        public ERPNA_Login_Test(string browserName, string emulationDevice, string version, string platform, string hubUri, string extrasUri)
                                    : base(browserName, emulationDevice, version, platform, hubUri, extrasUri)
        { }

        #endregion Constructors


        #region Tests

        /// <summary>
        /// When creating any test method, be sure to include the below NUnit attributes (Test, Description, Status and Author)
        /// </summary>
        [Test]
        [Description("Login to ERPNA")]
        [Property("Status", "Complete")]
        [Author("Srikanth Murugavel")]

        public void ERPNALoginTest()
        {
            // Include test step comments in your tests, precede them with 3 slashes to differentiate between them and code comments:
            /// 1. Navigate to the loginpage and get the Aperek credential from appsettings 
            // The below line of code will most likely be used first in every test method. It navigates to any page you want and 
            // returns that initialized page object. See inside the Navigation class for explanation of how this works

            Navigation.GoToLogin(Browser);

            
        }


       
        #endregion tests
    }
}






