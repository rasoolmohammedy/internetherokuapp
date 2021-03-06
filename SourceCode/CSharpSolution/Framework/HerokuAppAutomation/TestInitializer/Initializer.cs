using BoDi;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using UI.Pom;
using Utilities;
using static Utilities.Constants;

namespace TestExecutor
{
    [Binding]
    public sealed class Initializer
    {
        private static ILog logger { get; set; }
        public IWebDriver driver { get; set; }
        public RestClient restClient { get; set; }
        public static string currentReportPath { get; set; }
        private readonly ScenarioContext scenarioContext;
        private readonly FeatureContext featureContext;
        private readonly IObjectContainer objectcontainer;

        public Initializer(ScenarioContext scenarioContext, FeatureContext featureContext, IObjectContainer objectcontainer)
        {
            if (scenarioContext == null) throw new ArgumentNullException("scenarioContext");
            this.scenarioContext = scenarioContext;
            if (featureContext == null) throw new ArgumentNullException("featureContext");
            this.featureContext = featureContext;
            this.objectcontainer = objectcontainer;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            var aa = Environment.StackTrace;
            #region Initializing Logger
            if (logger == null)
            {
                logger = LogManager.GetLogger(typeof(Initializer));
                logger.Info($"Logger initialized. Automation Suite Execution Begins");
            }

            #endregion
            #region Initializing Extent Report
            SuiteType suiteType = Environment.StackTrace.Contains(Constants.APITESTS) ? SuiteType.API : SuiteType.UI;
            string reportPath = Utilities.Helpers.CreateReportPath(suiteType);
            currentReportPath = Path.Combine(reportPath, suiteType==SuiteType.API ? Constants.APIREPORTFILENAME:Constants.UIREPORTFILENAME);
            Utilities.ExtentReportsHelper.InitializeExtentReport(currentReportPath, "Automation Testing Report",
                "Regression Testing", "the-internet.herokuapp.com", "QA");
            #endregion
            KillChromeAndChromeDriverProcesses();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            Utilities.ExtentReportsHelper.Close();
            KillChromeAndChromeDriverProcesses();
        }

        [BeforeScenario(Order = 1000)]
        public void BeforeScenario()
        {
            if (scenarioContext.ScenarioInfo.Tags.Contains(Constants.SuiteType.UI.ToString()))
            {
                Constants.BrowserTypes currentBroswerType = (Constants.BrowserTypes)Enum.Parse(typeof(Constants.BrowserTypes), Constants.GlobalProperties.UI.BROSWERTYPE.ToLower());
                switch (currentBroswerType)
                {
                    case Constants.BrowserTypes.chrome:
                        instantiateChrome();
                        break;
                    default:
                        throw new Exception($"Unexpected browser type supplied in properties file. Kindly check the browser type specified in {Constants.PropFileConsts.UIGLOBALPROPERTIESFILENAME} is valid.");
                }
                #region Extent Report Create Test
                Utilities.ExtentReportsHelper.CreateTest(scenarioContext.ScenarioInfo.Title, Constants.SuiteType.UI);
                #endregion
                objectcontainer.RegisterInstanceAs<IWebDriver>(driver);
                KillChromeAndChromeDriverProcesses();
            }
            else
            {
                #region Initializing Rest Client
                restClient = new RestClient(Constants.GlobalProperties.API.BASEURL);
                #endregion
                #region Extent Report Create Test
                Utilities.ExtentReportsHelper.CreateTest(scenarioContext.ScenarioInfo.Title, Constants.SuiteType.API);
                #endregion
                objectcontainer.RegisterInstanceAs<RestClient>(restClient);
            }
            logger.Info(string.Format("Entering Before scenario: {0}, Feature: {1}", scenarioContext.ScenarioInfo.Title, featureContext.FeatureInfo.Title));
        }

        [AfterScenario(Order = 1000)]
        public void AfterScenario()
        {
            Exception lastError = scenarioContext.TestError;
            if (lastError != null)
            {
                ExtentReportsHelper.SetTestStatusFail(lastError);
            }
            Utilities.ExtentReportsHelper.Close();
            if (scenarioContext.ScenarioInfo.Tags.Contains(Constants.SuiteType.UI.ToString()))
            {
                KillChromeAndChromeDriverProcesses();
            }
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
            driver = new ChromeDriver(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, Constants.GlobalProperties.UI.CHROMEDRIVERRELATIVEPATH), chromeOptions);
        }

        private static void KillChromeAndChromeDriverProcesses()
        {
            try
            {
                var chromeProcesses = Process.GetProcessesByName("chrome.exe");
                if (chromeProcesses.Length > 0)
                {
                    foreach (var process in chromeProcesses)
                    {
                        process.Kill();
                    }
                }
                var chromeDriverProcesses = Process.GetProcessesByName("chromedriver.exe");
                if (chromeDriverProcesses.Length > 0)
                {
                    foreach (var process in chromeDriverProcesses)
                    {
                        process.Kill();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
