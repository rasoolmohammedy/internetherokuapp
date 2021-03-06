using log4net;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Base;
using Utilities;

namespace UI.Pom
{
    public class Page_JavaScriptAlerts : BaseClass
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Page_JavaScriptAlerts));
        public Page_JavaScriptAlerts(IWebDriver driver)
            : base(driver)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromSeconds(20)));
        }

        #region UI Elements
        private string xpath_button_jsConfirm = "//button[text()='{0}']";

        [FindsBy(How = How.XPath, Using = "//p[@id='result']")]
        private IWebElement result_text;
        #endregion

        #region Action Methods
        public void ClickJSConfirmButtonMustBePresent(string buttonText)
        {
            if (IsElementPresent(By.XPath(string.Format(xpath_button_jsConfirm, buttonText))))
                ExtentReportsHelper.SetStepStatusPass($"JavaScript Confirm button is present on the screen after clicking on the JavaScript Alerts link.");
            else
                ExtentReportsHelper.SetStepStatusPass($"JavaScript Confirm button is not present on the screen after clicking on the JavaScript Alerts link.");
        }

        public void ClickJSConfirmButton(string buttonText)
        {
            Click(driver.FindElement(By.XPath(string.Format(xpath_button_jsConfirm, buttonText))));
            logger.Debug($"Successfully clicked the '{buttonText}' button on the Java Script Alerts page.'");
        }

        public void ValidateResultTextMessage(string expectedResultMsg)
        {
            string actualResultMsg = GetText(result_text, "Result Text Message element");
            Assert.IsTrue(actualResultMsg == expectedResultMsg, $"Actual Result message '{actualResultMsg}' and expected result message '{expectedResultMsg}' are not same. Assertion failed.");
            logger.Debug($"Swithced to newly opened tab.");
        }

        public void ValidateWhetherAlertIsShown()
        {
            if(isAlertPresent())
                ExtentReportsHelper.SetStepStatusPass($"JavaScript Alert popup is shown.");
            else
                ExtentReportsHelper.SetTestStatusFail($"JavaScript Alert popup is not shown.");
        }

        public string GetAlertText()
        {
            string alertText = GetTextFromAlert();
            if (alertText != null)
                logger.Debug($"JavaScript Alert popup is shown with the text message '{alertText}'.");
            else
                logger.Debug($"Either Exception occurred or unable to get text from JavaScript Alert popup.");
            return alertText;
        }

        public bool DismissAlert()
        {
            bool isDismissed = CancelTheAlert();
            if(isDismissed)
            {
                logger.Debug($"Alert displayed on the web browser is successfully dismissed.");
                return true;
            }
            else
            {
                logger.Debug($"Unable to dismiss the Alert on the web browser.");
                return false;
            }
        }
        #endregion

    }
}
