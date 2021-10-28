using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UI.Base;
using Utilities;

namespace UI.Pom
{
    public class Page_FormAuthentication : BaseClass
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Page_FormAuthentication));
        public Page_FormAuthentication(IWebDriver driver) 
            : base(driver)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromSeconds(20)));
        }

        #region UI Elements
        [FindsBy(How = How.XPath, Using = "//em[1]")]
        private IWebElement username_emphasized;

        [FindsBy(How = How.XPath, Using = "//em[2]")]
        private IWebElement password_emphasized;

        [FindsBy(How = How.XPath, Using = "//input[@id='username']")]
        private IWebElement username_textbox;

        [FindsBy(How = How.XPath, Using = "//input[@id='password']")]
        private IWebElement password_textbox;

        [FindsBy(How = How.XPath, Using = "//button[@class='radius']")]
        private IWebElement submit_button;

        [FindsBy(How = How.XPath, Using = "//div[@id='flash']")]
        private IWebElement dataalertAfterLogin;

        [FindsBy(How = How.XPath, Using = "//a[contains(@class, 'radius')]")]
        private IWebElement logout_button;
        #endregion

        #region Action Methods
        public KeyValuePair<string,string> GetUserCredentials()
        {
            var creds =  new KeyValuePair<string, string>(GetText(username_emphasized),GetText(password_emphasized));
            logger.Debug($"Obtained credentials from the web portal is username:'${creds.Key}' & password:'{creds.Value}'");
            return creds;
        }

        public void Login(string username, string password)
        {
            SendText(username_textbox, username);
            logger.Debug($"Entered the username {username} onto the username textbox successfully'");
            SendText(password_textbox, password);
            logger.Debug($"Entered the password {password} onto the password textbox successfully'");
            Click(submit_button);
            logger.Debug($"Successfully clicked the Login button after entering credentials.'");
        }

        public void Logout()
        {
            Click(logout_button);
            logger.Debug($"Successfully clicked the Logout button on the screen.'");
        }


        public void ValidateLoginSuccessMessage(string expectedValidMessage)
        {
            var alertMsg = GetText(dataalertAfterLogin);
            logger.Debug($"Obtained Alert message from the portal is '${alertMsg}");
            Assert.IsTrue(alertMsg.Contains(expectedValidMessage), $"{alertMsg} obtained from portal is valid.");
            logger.Info($"Login was successful with the valid credentials obtained from the portal.");
        }

        public void ValidateInvalidLoginFailureMessage(string expectedInvalidMessage)
        {
            var alertMsg = GetText(dataalertAfterLogin);
            logger.Debug($"Obtained Alert message from the portal is '${alertMsg}");
            Assert.IsTrue(alertMsg.Contains(expectedInvalidMessage), $"{alertMsg} obtained from portal is a valid failure message.");
            logger.Info($"Login attempt was failed due to invalid credentials which is as expected.");
        }
        #endregion
    }
}
