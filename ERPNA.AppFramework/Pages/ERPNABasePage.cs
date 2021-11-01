using Browser.Core.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using Microsoft.CSharp;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System;
using OpenQA.Selenium.Interactions;

namespace ERPNA.AppFramework
{
    /// <summary>
    /// This is the base page for the HTML website. It contains all elements that appear on EVERY page. So these elements are not specific to 1 page. It also
    /// includes methods can be called which will work on every page. It extends the Page class inside Browser.Core.Framework.
    /// </summary>
    public abstract class ERPNABasePage : Page
    {
        #region Constructors

        /// <summary>
        /// You will need this constructor for every page class that you create
        /// </summary>
        /// <param name="driver"></param>
        public ERPNABasePage(IWebDriver driver) : base(driver) { }

        #endregion

        #region Elements       

        /// <summary>
        /// We are retreiving these elements from the PageBy class. Specifically, <see cref="ERPNABasePageBys"/>. That class is 
        /// where we locate all elements by using the By type (xpath, id's, class name, linktext, etc.). Once you locate a new 
        /// element inside a PageBy class, you then need to return it inside the respective Page class, as shown below.
        /// </summary>
        public IWebElement RefreshBtn { get { return this.FindElement(Bys.SCMBasePage.RefreshBtn); } }
        public IWebElement MenuLnk { get { return this.FindElement(Bys.SCMBasePage.MenuLnk); } }
        public IWebElement HelpLnk { get { return this.FindElement(Bys.SCMBasePage.HelpLnk); } }
        public IWebElement SearchLnk { get { return this.FindElement(Bys.SCMBasePage.SearchLnk); } }
        public IWebElement UserProfileImg { get { return this.FindElement(Bys.SCMBasePage.UserProfileImg); } }
        public IWebElement SignoutLnk { get { return this.FindElement(Bys.SCMBasePage.SignoutLnk); } }

        public IWebElement AnalysisWorkbenchLnk { get { return this.FindElement(Bys.SCMBasePage.AnalysisWorkbenchLnk); } }

        public IWebElement SpendAnalysisLnk { get { return this.FindElement(Bys.SCMBasePage.SpendAnalysisLnk); } }



        #endregion Elements

        #region methods

        /// <summary>
        /// Clicks the user-specified button, link, tab, etc. and then waits for a window/element to close or open, or a page to load,
        /// depending on the element that was clicked. Once the Wait Criteria is satisfied, the test continues, and the method returns
        /// either a new Page class instance or nothing at all (hence the 'dynamic' return type). This specific method is for Base page
        /// elements. You should also include this method inside each specific (non-base) page class
        /// </summary>
        /// <param name="elemToClick">The element to click on</param>
        public dynamic ClickAndWaitBasePage(IWebElement elemToClick)
        {
            // Error handler to make sure that the button that the tester passed in the parameter is actually on the page
            if (Browser.Exists(Bys.SCMBasePage.MenuLnk))
            {
                // If statement to get executed if the element that the tester passed in the parameter (left side element)
                // matches the hard coded (right side element) element
                if (elemToClick.GetAttribute("outerHTML") == MenuLnk.GetAttribute("outerHTML"))
                
                {
                    // If all If statements passed above, then we click the element
                    
                    MenuLnk.Click();
                    
                    // We then add the Wait Criteria below the click. In this specific example, if we click the Search button, 
                    // then a brand new page loads. So we can just instantiate that page class, then use it's WaitForInitialize
                    // method, then return that Page object, and the method is then completed. 
                    //SearchResultsPage Page = new SearchResultsPage(Browser);
                    //Page.WaitForInitialize();
                    return null;


                }
            }

            if (Browser.Exists(Bys.SCMBasePage.AnalysisWorkbenchLnk))
            {
                // If statement to get executed if the element that the tester passed in the parameter (left side element)
                // matches the hard coded (right side element) element
                if (elemToClick.GetAttribute("outerHTML") == AnalysisWorkbenchLnk.GetAttribute("outerHTML"))

                {
                    // If all If statements passed above, then we click the element

                   
                    AnalysisWorkbenchLnk.Click();
                    
                    // We then add the Wait Criteria below the click. In this specific example, if we click the Search button, 
                    // then a brand new page loads. So we can just instantiate that page class, then use it's WaitForInitialize
                    // method, then return that Page object, and the method is then completed. 
                    //SearchResultsPage Page = new SearchResultsPage(Browser);
                    //Page.WaitForInitialize();
                    return null;


                }
            }

            if (Browser.Exists(Bys.SCMBasePage.SpendAnalysisLnk))
            {
                // If statement to get executed if the element that the tester passed in the parameter (left side element)
                // matches the hard coded (right side element) element
                if (elemToClick.GetAttribute("outerHTML") == SpendAnalysisLnk.GetAttribute("outerHTML"))

                {
                    // If all If statements passed above, then we click the element

                   
                    SpendAnalysisLnk.Click();
                    
                    // We then add the Wait Criteria below the click. In this specific example, if we click the Search button, 
                    // then a brand new page loads. So we can just instantiate that page class, then use it's WaitForInitialize
                    // method, then return that Page object, and the method is then completed. 
                    //SearchResultsPage Page = new SearchResultsPage(Browser);
                    //Page.WaitForInitialize();
                    return null;


                }
            }





            throw new Exception(string.Format("No element was found with your passed parameter, which was the '{0}' element. You either need to add " +
                "this element to a new If statement, or if the element is already added, then the page you were on did not contain the element.",
                elemToClick.GetAttribute("innerText")));
            
            
        }




        #endregion methods
    }
}