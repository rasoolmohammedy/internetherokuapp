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
    public class Page_DragAndDrag:BaseClass
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Page_DragAndDrag));
        public Page_DragAndDrag(IWebDriver driver)
            : base(driver)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromSeconds(20)));
        }

        #region UI Elements
        [FindsBy(How = How.XPath, Using = "//h3")]
        private IWebElement header_pageTitle;

        [FindsBy(How = How.XPath, Using = "//div[@id='column-a']/header")]
        private IWebElement firstBox_Title;

        [FindsBy(How = How.XPath, Using = "//div[@id='column-b']/header")]
        private IWebElement secondtBox_Title;
        #endregion

        #region Action Methods
        public string GetTitleOfthePage()
        {
            string pageTitle = GetText(header_pageTitle, "Page Title");
            logger.Debug($"Page title obtained is '{pageTitle}'.");
            return pageTitle;
        }

        public string GetFirstBoxTitle()
        {
            string firstBoxTitle = GetText(firstBox_Title, "First Box Title");
            logger.Debug($"Title obtained from first box is '{firstBoxTitle}' from the Drag and Drop page.");
            return firstBoxTitle;
        }

        public string GetSecondBoxTitle()
        {
            string secondBoxTitle = GetText(secondtBox_Title, "Second Box Title");
            logger.Debug($"Title obtained from first box is '{secondBoxTitle}' from the Drag and Drop page.");
            return secondBoxTitle;
        }

        public void DragFirstBoxIntoSecondBox()
        {
            DragAndDrog(firstBox_Title, secondtBox_Title);
            logger.Debug($"Swithced to newly opened tab.");
        }
        #endregion
    }
}
