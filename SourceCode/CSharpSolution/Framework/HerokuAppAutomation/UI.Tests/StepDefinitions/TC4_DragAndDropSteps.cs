using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;
using TechTalk.SpecFlow;
using UI.Pom;
using Utilities;

namespace UI.Tests.StepDefinitions
{
    [Binding]
    public class TC4_DragAndDropSteps
    {
        private readonly ScenarioContext scenarioContext;
        private readonly FeatureContext featureContext;
        private IWebDriver driver;
        private Page_DragAndDrag p_dragNdrop;
        private Page_Home p_home;
        private Dictionary<string, string> testData = null;
        private string firstBoxTitle;
        private string secondBoxTitle;


        public TC4_DragAndDropSteps(ScenarioContext scenarioContext, FeatureContext featureContext, IWebDriver driver)
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
            p_dragNdrop = new Page_DragAndDrag(driver);
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
        public void WhenUserLaunchTheWebsiteAndClickOnDragAndDropItem()
        {
            p_home.OpenHomePage();
            Utilities.ExtentReportsHelper.SetStepStatusPass("Home page navigated.");
            p_home.NavigateToItem(Page_Home.ListItem.DragAndDrop);
            Utilities.ExtentReportsHelper.SetStepStatusPass("Drag and Drop link is clicked fromt the main page.");
            string pageTitle = p_dragNdrop.GetTitleOfthePage();
            Utilities.ExtentReportsHelper.SetStepStatusInfo($"Title of the page is '{pageTitle}'");
            var firstBoxTitle = p_dragNdrop.GetFirstBoxTitle();
            var secondBoxTitle = p_dragNdrop.GetSecondBoxTitle();
            if (testData["First Box Title"] == firstBoxTitle)
                ExtentReportsHelper.SetStepStatusPass($"First Box Title is {firstBoxTitle}");
            else
                ExtentReportsHelper.SetStepStatusWarning($"First Box Title is {firstBoxTitle} which is not as expected. Expected title of the first box must be '{testData["First Box Title"]}'");
            if (testData["Second Box Title"] == secondBoxTitle)
                ExtentReportsHelper.SetStepStatusPass($"Second Box Title is {secondBoxTitle}");
            else
                ExtentReportsHelper.SetStepStatusWarning($"Second Box Title is {secondBoxTitle} which is not as expected. Expected title of the second box must be '{testData["Second Box Title"]}'");
        }
        
        [When]
        public void WhenUserDragBoxAAndDropInotBoxB()
        {
            p_dragNdrop.DragFirstBoxIntoSecondBox();
            Utilities.ExtentReportsHelper.SetStepStatusPass($"First box on the page is dragged and dropped on the second box successfully.");
        }
        
        [Then]
        public void ThenUserShouldBeAbleToSeeBoxAFirstFollowedByBoxB()
        {
            firstBoxTitle = p_dragNdrop.GetFirstBoxTitle();
            secondBoxTitle = p_dragNdrop.GetSecondBoxTitle();
            ExtentReportsHelper.SetStepStatusInfo($"Before drag and drop, at present First box title is '{firstBoxTitle}' and Second Box title is '{secondBoxTitle}'.");
        }

        [Then]
        public void ThenBoxBShouldBePresentFirstFollowedByBoxA()
        {
            var firstBoxTitle = p_dragNdrop.GetFirstBoxTitle();
            var secondBoxTitle = p_dragNdrop.GetSecondBoxTitle();
            Assert.IsTrue(this.firstBoxTitle == secondBoxTitle, $"First box title after drag and drop is '{firstBoxTitle}' which is expected.");
            Assert.IsTrue(this.secondBoxTitle == firstBoxTitle, $"Second box title after drag and drop is '{secondBoxTitle}' which is expected.");
            ExtentReportsHelper.SetStepStatusInfo($"After drag and drop, at present First box title is '{firstBoxTitle}' and Second Box title is '{secondBoxTitle}'.");
        }

        [Then]
        public void ThenTakeScreenshotOfTheCurrentWindow()
        {
            Utilities.ExtentReportsHelper.SetStepStatusInfo($"Current snapshot of the webdriver is captured.", ScreenshotCapture.CaptureScreenshot(driver, Utilities.ExtentReportsHelper.currentReportPath));
            Thread.Sleep(new TimeSpan(0,0,2));
        }

    }
}
