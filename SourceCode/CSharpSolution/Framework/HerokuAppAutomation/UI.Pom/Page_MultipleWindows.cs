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
    public class Page_MultipleWindows : BaseClass
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Page_MultipleWindows));
        public Page_MultipleWindows(IWebDriver driver) 
            : base(driver)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromSeconds(20)));
        }

        #region UI Elements
        private string xpath_click_link= "//a[text()='{0}']";
        #endregion

        #region Action Methods

        public void ClickLink(string linkText)
        {
            Click(driver.FindElement(By.XPath(string.Format(xpath_click_link, linkText))));
            logger.Debug($"Successfully clicked the '{linkText}' hyperlink present on the Multiple Windows page.'");
        }

        public void SwitchToTab(int index)
        {
            base.SwitchToTab(index);
            logger.Debug($"Swithced to newly opened tab.");
        }

        public string GetURLOfCurrentTab()
        {
            var url = GetCurrentWindowURL();
            logger.Debug($"Current windows URL is {url}");
            return url;
        }

        public void CloseCurrentTab()
        {
            base.CloseCurrentTab();
            logger.Debug($"Current tab closed.");
        }
        public string GetTitleOfCurrentPage()
        {
            string title = GetCurrentWindowTitle();
            logger.Debug($"Current Windows Title is {title}");
            return title;
        }
        #endregion
    }
}
