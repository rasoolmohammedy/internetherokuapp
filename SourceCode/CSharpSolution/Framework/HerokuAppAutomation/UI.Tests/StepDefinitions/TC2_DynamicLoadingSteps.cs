using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using UI.Pom;
using Utilities;

namespace UI.Tests.StepDefinitions
{
    [Binding]
    public class TC2_DynamicLoadingSteps
    {
        private readonly ScenarioContext scenarioContext;
        private readonly FeatureContext featureContext;
        private IWebDriver driver;
        private Page_DynamicLoading p_dynamicLoading;
        private Page_Home p_home;
        private Dictionary<string, string> testData = null;

        public TC2_DynamicLoadingSteps(ScenarioContext scenarioContext, FeatureContext featureContext, IWebDriver driver)
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
            p_dynamicLoading = new Page_DynamicLoading(driver);
            p_home = new Page_Home(driver);
            testData = Utilities.ExcelDataManager.GetTestData(Constants.SuiteType.UI, scenarioContext.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void TearDown()
        {
            Utilities.ExtentReportsHelper.Close();
            driver.Quit();
        }

        [Given]
        public void GivenTheUserLandedIntoHomepage()
        {
            p_home.OpenHomePage();
            Utilities.ExtentReportsHelper.SetStepStatusPass("Home page navigated.");
        }
        
        [When]
        public void WhenTheUserClicksOnDynamicLoadingIteam()
        {
            p_home.NavigateToItem(Page_Home.ListItem.DynamicLoading);
            Utilities.ExtentReportsHelper.SetStepStatusPass("Dynamic Loading link is clicked fromt the main page.");
        }
        
        [When]
        public void WhenClickOnExample_P0(int p0)
        {
            string example2Text = testData["Text to click"];
            p_dynamicLoading.ClickExample2(example2Text);
            Utilities.ExtentReportsHelper.SetStepStatusPass($"Example 2 viz., '{example2Text}' link is clicked fromt the main page.");
        }
        
        [When]
        public void WhenClickStartButton()
        {
            string startButtonText = testData["Button Text"];
            p_dynamicLoading.ClickStartButton(startButtonText);
            Utilities.ExtentReportsHelper.SetStepStatusPass($"Start button with text '{startButtonText}' is clicked successfully.");
        }
        
        [When]
        public void WhenTheUserWaitsTillProgressBarToDisappear()
        {
            string loadingText = testData["Loading Text"];
            p_dynamicLoading.WaitForProgressBarToDisappear(loadingText);
            Utilities.ExtentReportsHelper.SetStepStatusPass($"Progress bar with text '{loadingText}' appeared and waiting without any hard coded value till the progress bar disappear.");
        }
        
      
        [Then]
        public void ThenUserMustSeeTheFinalMessage()
        {
            string loadingText = testData["After Loading Message"];
            p_dynamicLoading.ValidateFinalTextMessage(loadingText);
            Utilities.ExtentReportsHelper.SetStepStatusPass($"After progress bar is disappred final messge displayed was '{loadingText}' which is the expected message to be displayed.");
        }
    }
}
