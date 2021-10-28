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
                logger.Error($"Unable to find the webelement {elementDescription} even after waiting for {timeoutInSecs} seconds. Timeout has occurred.", ex);
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

        protected void SendText(IWebElement element, string input, string elementDescription = "")
        {
            waitForElement(element);
            try
            {
                element.SendKeys(input);
                logger.Debug($"Input value '{input}' was set on the text box {Utilities.Helpers.GetXpath(element)}");
            }
            catch (Exception ex)
            {
                logger.Error($"Unexpected exception occured while trying to Send the text {input} to the element {elementDescription}", ex);
            }
        }

        protected void WaitForElementToDisappear(By element, string elementDescription = "")
        {
            int timeoutInSecs = 60;
            DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver);
            fluentWait.Timeout = TimeSpan.FromSeconds(timeoutInSecs);
            fluentWait.PollingInterval = TimeSpan.FromMilliseconds(1000);
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.Message = "Element to disappear is still present in UI";
            try
            {
                fluentWait.Until(OpenQA.Selenium.Support.UI.ExpectedConditions.InvisibilityOfElementLocated(element));
                logger.Debug($"Element found on the webpage. ${elementDescription}");
            }
            catch (WebDriverTimeoutException ex)
            {
                logger.Error($"Element waited to disappear {elementDescription} is still present on UI even after waiting for {timeoutInSecs} seconds. Timeout has occurred.", ex);
            }
            catch (Exception ex)
            {
                logger.Error($"Unexpected exception occured while trying to Wait for the the element to disappear {elementDescription} even after waiting till ${timeoutInSecs} seconds.", ex);
            }
        }

        protected void SwitchToTab(int index)
        {
            try
            {
                driver.SwitchTo().Window(driver.WindowHandles[index]);
                logger.Debug($"Switched to the tab of inxex {index}");
            }
            catch (Exception ex)
            {
                logger.Error($"Unexpected error occurred while trying to switch to the tab of index {index}.", ex);
            }
            
        }

        protected string GetCurrentWindowTitle()
        {
            try
            {
                return driver.Title;
            }
            catch (Exception ex)
            {
                logger.Error($"Unexpected error occurred while trying to get the windows title.", ex);
                return null;
            }
        }

        protected void CloseCurrentTab()
        {
            try
            {
                driver.Close();
            }
            catch (Exception ex)
            {
                logger.Debug($"Unexpected error occurred while trying to close the current tab of the browser window",ex);
            }
        }

        protected string GetCurrentWindowURL()
        {
            try
            {
                return driver.Url;
            }
            catch (Exception ex)
            {
                logger.Error($"Unexpected error occurred while trying to get the URL from the current window.",ex);
                return null;
            }
        }
        #endregion
    }
}
