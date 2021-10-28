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
    public class Page_DynamicLoading : BaseClass
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Page_DynamicLoading));
        public Page_DynamicLoading(IWebDriver driver) 
            : base(driver)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromSeconds(20)));
        }

        #region UI Elements
        private string xpath_example2_hyperlink= "//a[text()='{0}']";

        private string xpath_start_button = "//button[text()='{0}']";

        [FindsBy(How = How.Id, Using = "loading")]
        private IWebElement loading_text_webElement;

        private By loading_text_by = By.XPath("//div[@id='loading'");

        [FindsBy(How = How.XPath, Using = "//div[@id='finish']/h4")]
        private IWebElement finish_text;
        #endregion

        #region Action Methods

        public void ClickExample2(string example2Text)
        {
            Click(driver.FindElement(By.XPath(string.Format(xpath_example2_hyperlink,example2Text))));
            logger.Debug($"Successfully clicked the example 2 hyperlink present on the Dynamic Loading page.'");
        }

        public void ClickStartButton(string buttonText)
        {
            Click(driver.FindElement(By.XPath(string.Format(xpath_start_button, buttonText))));
            logger.Debug($"Successfully clicked the Start button on the screen.'");
        }


        public void WaitForProgressBarToDisappear(string loadText)
        {
            string actualLoadText = GetText(loading_text_webElement);
            if (actualLoadText != loadText)
                logger.Warn($"Loading text appeared is not having expected value. Actual Value {actualLoadText}, Expected value {loadText}");
            WaitForElementToDisappear(loading_text_by);
            logger.Info($"Waited till the Loading Progress bar disappears");
        }

        public void ValidateFinalTextMessage(string expectedFinalTextMsg)
        {
            var actualFinalMsg = GetText(finish_text);
            logger.Debug($"Obtained Final Text message from the portal is '${actualFinalMsg}");
            Assert.IsTrue(actualFinalMsg.Contains(expectedFinalTextMsg), $"{actualFinalMsg} obtained from portal is a valid final text message.");
        }
        #endregion
    }
}
