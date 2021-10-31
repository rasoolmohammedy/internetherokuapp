using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Base;

namespace UI.Pom
{
    public class Page_Frames : BaseClass
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Page_Frames));
        public Page_Frames(IWebDriver driver)
            : base(driver)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromSeconds(20)));
        }

        #region UI Elements
        private By iFrame = By.Id("mce_0_ifr");

        private string xpath_FrameLink = "//a[text()='{0}']";

        [FindsBy(How = How.XPath, Using = "//body/p")]
        private IWebElement para_textBox_text_webElement;

        [FindsBy(How = How.XPath, Using = "//button[@title='Bold']")]
        private IWebElement button_bold;
        #endregion

        #region Action Methods
        public bool isiFrameLinkPresent(string linkText)
        {
            bool isPresent = IsElementPresent(By.XPath(string.Format(xpath_FrameLink, linkText)));
            if (isPresent)
                logger.Debug("'{linkText}' Element is present on the screen.");
            else
                logger.Debug("'{linkText}' Element is not present on the screen.");
            return isPresent;
        }

        public void ClickLink(string linkText)
        {
            Click(driver.FindElement(By.XPath(string.Format(xpath_FrameLink, linkText))));

            logger.Debug($"Successfully clicked the '{linkText}' hyperlink present on the Frames page.'");
        }

        public void ClearTextBox()
        {
            ClearText(para_textBox_text_webElement);
            logger.Debug($"Textbox cleared. Existing text is cleared successfully."); 
        }

        public void SetTextInTextBox(string input)
        {
            SetText(para_textBox_text_webElement, input);
            logger.Debug($"Textbox is entered with the input data '{input}' successfully.");
        }

        public string GetTextFromTextBox()
        {
            var currentTxt = GetText(para_textBox_text_webElement, "Textbox of the text editor");
            logger.Debug($"Content present in the textbox is {currentTxt}.");
            return currentTxt;
        }

        public void ClickBoldButton()
        {
            Click(button_bold);
            logger.Debug($"Bold Button is clicked to make the currently entered text to apply bold style.");
        }

        public void WaitForiFrameElementsToLoad()
        {
            WaitForFrameToLoad(iFrame, "iFrame to load");
        }
        #endregion
    }
}
