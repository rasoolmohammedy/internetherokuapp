using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Core;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Utilities;

namespace UI.Base
{
    public abstract class BaseClass
    {
        public ILog logger { get; set; }
        public static ExtentReportsHelper reporter { get; set; }
        public Dictionary<string, ExtentTest> TestCollector { get; set; }

        public IWebDriver driver;

        protected BaseClass(Type pageType)
        {
            logger = LogManager.GetLogger(pageType);
            #region Initializing Extent Report
            var reportBasePath = Path.Combine(Environment.CurrentDirectory, Constants.GlobalProperties.UI.BASEPATH);
            if (!Directory.Exists(reportBasePath))
                Directory.CreateDirectory(reportBasePath);
            var reportPath = Path.Combine(reportBasePath, Directory.CreateDirectory(DateTime.Now.ToString("yyyy_MM_dd_H_mm_ss")).FullName);
            reporter = new Utilities.ExtentReportsHelper(reportPath, "UI Automation Testing Report", "Regression Testing", "the-internet.herokuapp.com", "QA");
            #endregion
            #region Initializing web driver
            Constants.BrowserTypes currentBroswerType = (Constants.BrowserTypes)Enum.Parse(typeof(Constants.BrowserTypes), Constants.GlobalProperties.UI.BROSWERTYPE.ToLower());
            switch (currentBroswerType)
            {
                case Constants.BrowserTypes.chrome:
                    instantiateChrome();
                    break;
                default:
                    throw new Exception($"Unexpected browser type supplied in properties file. Kindly check the browser type specified in {Constants.PropFileConsts.UIGLOBALPROPERTIESFILENAME} is valid.");
            }
            #endregion
        }

        private void instantiateChrome()
        {
            if (driver != null)
            {
                driver.Quit();
            }
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddUserProfilePreference("profile.default_content_settings.popups", 0);
            chromeOptions.AddUserProfilePreference("safebrowsing.enabled", true);
            chromeOptions.AddUserProfilePreference("disable-popup-blocking", false);
            chromeOptions.AddArguments("--disable-notifications");
            chromeOptions.AddArguments("--disk-cache-size=1");
            chromeOptions.AddArguments("--media-cache-size=1");
            chromeOptions.AddArguments("start-maximized");
            driver = new ChromeDriver(chromeOptions);
        }
    }
}
