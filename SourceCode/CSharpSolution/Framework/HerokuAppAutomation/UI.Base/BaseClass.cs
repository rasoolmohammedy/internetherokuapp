using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Utilities;

namespace UI.Base
{
    public abstract class BaseClass
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(BaseClass));

        public IWebDriver driver;

        protected BaseClass(IWebDriver driver)
        {
            this.driver = driver;
        }

        #region Discrete Operations
        protected void Click(IWebElement element, string elementDescription = "")
        {
            try
            {
                waitForElement(element);
                element.Click();
            }
            catch (Exception ex)
            {
                logger.Error($"Unexpected exception occured while trying to Click the element {elementDescription}", ex);
            }
        }

        protected void waitForElement(IWebElement element, string elementDescription = "")
        {
            int timeoutInSecs = 5;
            DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver);
            fluentWait.Timeout = TimeSpan.FromSeconds(timeoutInSecs);
            fluentWait.PollingInterval = TimeSpan.FromMilliseconds(250);
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.Message = "Element to be searched not found";
            try
            {
                fluentWait.Until(OpenQA.Selenium.Support.UI.ExpectedConditions.ElementToBeClickable(element));
                logger.Debug($"Element found on the webpage. ${elementDescription}");
            }
            catch(WebDriverTimeoutException ex)
            {
                logger.Error($"Unable to find the webelement {elementDescription} even after waitinf for {timeoutInSecs} seconds. Timeout has occurred.", ex);
            }
            catch (Exception ex)
            {
                logger.Error($"Unexpected exception occured while trying to Wait for the the element's availability {elementDescription} even after waiting till ${timeoutInSecs} seconds.", ex);
            }

        }

        protected string GetText(IWebElement element, string elementDescription = "")
        {
            string text = null;
            waitForElement(element);
            try
            {
                text = element.Text;
            }
            catch (Exception ex)
            {
                logger.Error($"Unexpected exception occured while trying to Get the Text from the element {elementDescription}", ex);
            }
            return text;
        }

        protected void SendText(IWebElement element, string input)
        {
            waitForElement(element);
            try
            {
                element.SendKeys(input);
                logger.Debug($"Input value '{input}' was set on the text box {Utilities.Helpers.GetXpath(element)}");
            }
            catch (Exception ex)
            {
                logger.Error($"Unexpected exception occured while trying to Send the text {input} to the element {Helpers.GetXpath(element)}", ex);
            }
        }
        #endregion
    }
}
