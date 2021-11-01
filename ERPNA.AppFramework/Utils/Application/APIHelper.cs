using Browser.Core.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ERPNA.AppFramework
{
    public static class APIHelper
    {

        #region properties

        #endregion properties        

        #region methods

        /// <summary>
        /// Gets the access token for you to be able to execute any LMS API
        /// </summary>
        /// <param name="baseURL"></param>
        /// <param name="siteCode"></param>
        /// <param name="accountKey"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static String GetToken(string baseURL, String siteCode, String accountKey, String password)
        {
            var bodyData = new { AccountKey = accountKey, Password = password, SiteCode = siteCode };
            string body = JsonConvert.SerializeObject(bodyData);
            string fullAPIUrl = baseURL + "api/accessToken";
            var tokenResponse = new { Token = "", Expiration = "" };

            var tokenModel = APIUtils.ExecuteAPI_Post(fullAPIUrl, body);

            var tokenAnon = JsonConvert.DeserializeAnonymousType(tokenModel, tokenResponse);

            return tokenAnon.Token;
        }

        /// <summary>
        /// Gets the user access token for you to be able to specify a user for any API call that is user-related 
        /// (i.e. delete/unregister a user
        /// from an activity)
        /// </summary>
        /// <param name="baseURL"></param>
        /// <param name="siteCode"></param>
        /// <param name="accountKey"></param>
        /// <param name="password"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static String GetUserAccessToken(String accountKey, String password, String username)
        {
            var bodyData = new { AccountKey = accountKey, Password = password, UserName = username };
            string body = JsonConvert.SerializeObject(bodyData);
            String fullAPIUrl = String.Format("{0}api/accessToken", AppSettings.Config["APIUrl"].ToString());
            var tokenResponse = new { Token = "", Expiration = "" };

            var tokenModel = APIUtils.ExecuteAPI_Post(fullAPIUrl, body);

            var tokenAnon = JsonConvert.DeserializeAnonymousType(tokenModel, tokenResponse);

            return tokenAnon.Token;
        }

        /// <summary>
        /// Gets the user access token from the browser cookie. Useful if you dont have the username and/or other things 
        /// necessary to get this token when using <see cref="GetUserAccessToken(Constants.SiteCodes, string, string, string)"/>
        /// from an activity)
        /// </summary>
        /// <param name="Browser"></param>
        /// <returns></returns>
        public static string GetUserAccessTokenFromCookie(IWebDriver Browser)
        {
            var token = Browser.Manage().Cookies.GetCookieNamed("APIAccessToken");
            return token.Value;
        }


        /// <summary>
        /// Gets the username of the currently logged in user
        /// </summary>
        /// <returns></returns>
        public static string GetHomeMenu(IWebDriver Browser)
        {
            string token = GetUserAccessTokenFromCookie(Browser);
            String fullAPIUrl = String.Format("{0}api/user/name", AppSettings.Config["APIUrl"].ToString());

            string username = APIUtils.ExecuteAPI_Get(fullAPIUrl, token);
            string usernameDoubleQuotesRemoved = username.Replace('"', ' ').Trim();
            return usernameDoubleQuotesRemoved;
        }



        #endregion methods

        #region API Objects


        #endregion API Objects


    }

}
