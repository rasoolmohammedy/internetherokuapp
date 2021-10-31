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
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Utilities;
using System.Diagnostics;

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

        protected void WaitForFrameToLoad(By locator,string locatorDescription = "")
        {
            TimeSpan timeout = new TimeSpan(0,1,0);
            DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver);
            fluentWait.Timeout = TimeSpan.FromSeconds(timeout.TotalSeconds);
            fluentWait.PollingInterval = TimeSpan.FromMilliseconds(1000);
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.Message = "Element to disappear is still present in UI";
            try
            {
                fluentWait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(locator));

            }
            catch (WebDriverTimeoutException ex)
            {
                logger.Error($"Element waited to load {locatorDescription} is still not loaded on UI even after waiting for {timeout.TotalSeconds} seconds. Timeout has occurred.", ex);
            }
            catch (Exception ex)
            {
                logger.Error($"Unexpected exception occured while trying to Wait for the the element to load {locatorDescription} even after waiting till ${timeout.TotalSeconds} seconds.", ex);
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

        protected bool IsElementPresent(By element, string elementDescription = "")
        {
            int timeoutInSecs = 60;
            bool result = false;
            DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver);
            fluentWait.Timeout = TimeSpan.FromSeconds(timeoutInSecs);
            fluentWait.PollingInterval = TimeSpan.FromMilliseconds(1000);
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.Message = "Element is not present in UI";
            try
            {
                fluentWait.Until(OpenQA.Selenium.Support.UI.ExpectedConditions.ElementExists(element));
                logger.Debug($"Element found on the webpage. ${elementDescription}");
                result = true;
            }
            catch (WebDriverTimeoutException ex)
            {
                logger.Error($"Element {elementDescription} is not present on UI even after waiting for {timeoutInSecs} seconds. Timeout has occurred.", ex);
            }
            catch (Exception ex)
            {
                logger.Error($"Unexpected exception occured while trying to find whether element {elementDescription} is present or not even after waiting till ${timeoutInSecs} seconds.", ex);
            }
            return result;
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

        protected void DragAndDrog(IWebElement sourceElement, IWebElement targetElement)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            try
            {
                js.ExecuteScript("function createEvent(typeOfEvent) {\n" + "var event =document.createEvent(\"CustomEvent\");\n"
                        + "event.initCustomEvent(typeOfEvent,true, true, null);\n" + "event.dataTransfer = {\n" + "data: {},\n"
                        + "setData: function (key, value) {\n" + "this.data[key] = value;\n" + "},\n"
                        + "getData: function (key) {\n" + "return this.data[key];\n" + "}\n" + "};\n" + "return event;\n"
                        + "}\n" + "\n" + "function dispatchEvent(element, event,transferData) {\n"
                        + "if (transferData !== undefined) {\n" + "event.dataTransfer = transferData;\n" + "}\n"
                        + "if (element.dispatchEvent) {\n" + "element.dispatchEvent(event);\n"
                        + "} else if (element.fireEvent) {\n" + "element.fireEvent(\"on\" + event.type, event);\n" + "}\n"
                        + "}\n" + "\n" + "function simulateHTML5DragAndDrop(element, destination) {\n"
                        + "var dragStartEvent =createEvent('dragstart');\n" + "dispatchEvent(element, dragStartEvent);\n"
                        + "var dropEvent = createEvent('drop');\n"
                        + "dispatchEvent(destination, dropEvent,dragStartEvent.dataTransfer);\n"
                        + "var dragEndEvent = createEvent('dragend');\n"
                        + "dispatchEvent(element, dragEndEvent,dropEvent.dataTransfer);\n" + "}\n" + "\n"
                        + "var source = arguments[0];\n" + "var destination = arguments[1];\n"
                        + "simulateHTML5DragAndDrop(source,destination);", sourceElement, targetElement);
            }
            catch (Exception ex)
            {
                logger.Error($"Unexpected error occurred while trying to dragging the source element and dropping on the target element.", ex);
            }
        }

        protected void ClearText(IWebElement element)
        {
            try
            {
                element.Clear();
            }
            catch (Exception ex)
            {
                logger.Error($"Unexpected error occurred while trying to clear the text present in the textbox.");
            }
        }

        protected void SetText(IWebElement element, string inputText)
        {
            try
            {
                element.SendKeys(inputText);
            }
            catch (Exception ex)
            {
                logger.Error($"Unexpected error occurred while trying to enter the text in the textbox. Text tried to enter on the textbox is '{inputText}'",ex);
            }
        }

        protected bool isAlertPresent()
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
            try
            {
                wait.Until(ExpectedConditions.AlertIsPresent());
                IAlert alert = wait.Until(ExpectedConditions.AlertIsPresent());
                alert = driver.SwitchTo().Alert();
                logger.Debug("Alert is displayed.");
                return true;
            }   
            catch (NoAlertPresentException Ex)
            {
                logger.Debug("Alert is not displayed.");
                return false;
            } 
            catch (Exception ex)
            {
                logger.Debug($"Unexpected exception occurred while trying to ensure whether alert is displayed or not",ex);
                return false;
            }
        }

        protected bool CancelTheAlert()
        {
            if (!isAlertPresent())
                return false;
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
            try
            {
                IAlert alert = wait.Until(ExpectedConditions.AlertIsPresent());
                alert=driver.SwitchTo().Alert();
                alert.Dismiss();
                logger.Debug("Alert is dismissed successfully.");
                return true;
            }
            catch (Exception ex)
            {
                logger.Debug($"Unexpected exception occurred while trying to cancel the alert displayed or not",ex);
                return false;
            }
        }

        protected string GetTextFromAlert()
        {
            string alertText = null;
            if (!isAlertPresent())
                return alertText;
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
            try
            {
                IAlert alert = wait.Until(ExpectedConditions.AlertIsPresent());
                alert = driver.SwitchTo().Alert();
                alertText = alert.Text;
                logger.Debug("Text displayed in alert is '{alertText}'");
                return alertText;
            }
            catch (Exception ex)
            {
                logger.Debug($"Unexpected exception occurred while trying to get the text displayed in alert.",ex);
                return alertText;
            }
        }
        #endregion
    }
}
