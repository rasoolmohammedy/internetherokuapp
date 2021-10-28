using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Base;
using Utilities;
using log4net;
using OpenQA.Selenium.Support.PageObjects;

namespace UI.Pom
{
    public class Page_Home:BaseClass
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(BaseClass));
        public enum ListItem
        {
            FormAuthentication,
            DynamicLoading,
            MultipleWindows,
            DragAndDrop,
            Frames,
            JavascriptAlerts
        }
        public Page_Home(IWebDriver driver)
            : base(driver)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromSeconds(20)));
        }

        #region UI Elements
        [FindsBy(How = How.XPath, Using = "//div[@id='content']//a[text()='Form Authentication']")]
        private IWebElement anchor_form_authentication;

        [FindsBy(How = How.XPath, Using = "//div[@id='content']//a[text()='Dynamic Loading']")]
        private IWebElement anchor_dynamic_loading;

        [FindsBy(How = How.XPath, Using = "//div[@id='content']//a[text()='Multiple Windows']")]
        private IWebElement anchor_multiple_windows;

        [FindsBy(How = How.XPath, Using = "//div[@id='content']//a[text()='Drag and Drop']")]
        private IWebElement anchor_drag_drop;

        [FindsBy(How = How.XPath, Using = "//div[@id='content']//a[text()='Frames']")]
        private IWebElement anchor_frames;

        [FindsBy(How = How.XPath, Using = "//div[@id='content']//a[text()='JavaScript Alerts']")]
        private IWebElement anchor_javascript_alerts;

        #endregion

        #region Action Methods
        private void SelectListItem(IWebElement item, string itemDescrition)
        {
            Click(item);
            logger.Debug($"From Home Page, {itemDescrition} is clicked.");

        }
        #endregion

        #region Business Logic
        public void NavigateToItem(ListItem item)
        {
            switch (item)
            {
                case ListItem.FormAuthentication:
                    SelectListItem(anchor_form_authentication, "Form Authentication Item from Homepage.");
                    break;
                case ListItem.DynamicLoading:
                    SelectListItem(anchor_dynamic_loading, "Dynamic Loading Item from Homepage.");
                    break;
                case ListItem.MultipleWindows:
                    SelectListItem(anchor_multiple_windows, "Multiple Window Item from Homepage.");
                    break;
                case ListItem.DragAndDrop:
                    SelectListItem(anchor_drag_drop, "Drag and Drop Item from Homepage.");
                    break;
                case ListItem.Frames:
                    SelectListItem(anchor_frames, "Frames Item from Homepage.");
                    break;
                case ListItem.JavascriptAlerts:
                    SelectListItem(anchor_javascript_alerts, "Javascript Alerts Item from Homepage.");
                    break;
                default:
                    throw new Exception("Unexpected Item received, while trying to navigate from Home page.");
            }
        }

        public void OpenHomePage()
        {
            driver.Navigate().GoToUrl(Constants.GlobalProperties.UI.BASEURL);
        }
        #endregion
    }
}
