using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using UI.Pom;
using Utilities;

namespace UI.Tests.StepDefinitions
{
    [Binding]
    public class TC3_MultipleWindowsSteps
    {
        private readonly ScenarioContext scenarioContext;
        private readonly FeatureContext featureContext;
        private IWebDriver driver;
        private Page_MultipleWindows p_multiplewindows;
        private Page_Home p_home;
        private Dictionary<string, string> testData = null;

        public TC3_MultipleWindowsSteps(ScenarioContext scenarioContext, FeatureContext featureContext, IWebDriver driver)
        {
            if (scenarioContext == null) throw new ArgumentNullException("scenarioContext");
            this.scenarioContext = scenarioContext;

            if (featureContext == null) throw new ArgumentNullException("featureContext");
            this.featureContext = featureContext;

            this.driver = driver;
        }

        [BeforeScenario]
        public void Initialize()
        {
            Utilities.ExtentReportsHelper.CreateTest(scenarioContext.ScenarioInfo.Title, Constants.SuiteType.UI);
            p_multiplewindows = new Page_MultipleWindows(driver);
            p_home = new Page_Home(driver);
            testData = Utilities.ExcelDataManager.GetTestData(Constants.SuiteType.UI, scenarioContext.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void TearDown()
        {
            Utilities.ExtentReportsHelper.Close();
            driver.Quit();
        }

        [When]
        public void WhenTheUserClicksOnMultipleWindowsLink()
        {
            p_home.NavigateToItem(Page_Home.ListItem.MultipleWindows);
            Utilities.ExtentReportsHelper.SetStepStatusPass("Multiple Windows link is clicked fromt the main page.");
        }
        
        [When]
        public void WhenTheUserClicksOnClickHereLink()
        {
            string clickLink = testData["Click Link Text"];
            p_multiplewindows.ClickLink(clickLink);
            Utilities.ExtentReportsHelper.SetStepStatusPass("Dynamic Loading link is clicked fromt the main page.");
        }
        
        [Then]
        public void ThenLogTheURLOfTheNewlyOpenedTab()
        {
            p_multiplewindows.SwitchToTab(1);
            var url = p_multiplewindows.GetURLOfCurrentTab();
            Utilities.ExtentReportsHelper.SetStepStatusPass($"Currently opened URL of the web page is '{url}'");
        }


        [Then]
        public void ThenCloseTheNewlyOpenedTab()
        {
            p_multiplewindows.CloseCurrentTab();
            Utilities.ExtentReportsHelper.SetStepStatusPass("Current tab is closed.");
        }
        
        [Then]
        public void ThenLogTheTitleOfTheCurrentPage()
        {
            p_multiplewindows.SwitchToTab(0);
            var title = p_multiplewindows.GetTitleOfCurrentPage();
            Utilities.ExtentReportsHelper.SetStepStatusPass($"Title of the current opened window is '{title}'");
        }
    }
}
