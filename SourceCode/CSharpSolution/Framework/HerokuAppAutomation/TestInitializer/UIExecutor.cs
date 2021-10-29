﻿using BoDi;
using log4net;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using UI.Base;
using Utilities;

namespace TestExecutor
{
    [Binding]
    public sealed class UIExecutor : iExecutor
    {
        public ILog logger { get; set; }
        private readonly ScenarioContext scenarioContext;
        private readonly FeatureContext featureContext;
        private readonly IObjectContainer objectcontainer;
        public IWebDriver driver { get; set; }
        public string currentReportPath { get ; set ; }

        public UIExecutor(ScenarioContext scenarioContext, FeatureContext featureContext, IObjectContainer objectcontainer)
        {
            #region Initializing Logger
            logger = LogManager.GetLogger(typeof(UIExecutor));
            logger.Info($"Logger initialized. UI Automatioin Suite Execution Begins");
            #endregion

            #region Initializing Extent Report
            string reportPath = Utilities.Helpers.CreateReportPath();
            currentReportPath = Path.Combine(reportPath, Constants.REPORTFILENAME);
            Utilities.ExtentReportsHelper.InitializeExtentReport(currentReportPath, "Automation Testing Report", "Regression Testing", "the-internet.herokuapp.com", "QA");
            #endregion
            if (scenarioContext == null) throw new ArgumentNullException("scenarioContext");
            this.scenarioContext = scenarioContext;
            if (featureContext == null) throw new ArgumentNullException("featureContext");
            this.featureContext = featureContext;
            this.objectcontainer = objectcontainer;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
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
            logger.Info(string.Format("Entering Before scenario: {0}, Feature: {1}", scenarioContext.ScenarioInfo.Title, featureContext.FeatureInfo.Title));
            Initializer();
        }

        [AfterScenario(Order = 1000)]
        public void AfterScenario()
        {
            Utilities.ExtentReportsHelper.Close();
        }

        public void Initializer()
        {
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
            objectcontainer.RegisterInstanceAs<IWebDriver>(driver);
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
            driver = new ChromeDriver(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, Constants.CHROMEDRIVERRELATIVEPATH), chromeOptions);
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