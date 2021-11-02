using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using UI.Pom;
using Utilities;

namespace UI.Tests.StepDefinitions
{
    [Binding]
    public class TC5_FramesSteps
    {
        private readonly ScenarioContext scenarioContext;
        private readonly FeatureContext featureContext;
        private IWebDriver driver;
        private Page_Frames p_frames;
        private Page_Home p_home;
        private Dictionary<string, string> testData = null;
        private string firstBoxTitle;
        private string secondBoxTitle;

        public TC5_FramesSteps(ScenarioContext scenarioContext, FeatureContext featureContext, IWebDriver driver)
        {
            if (scenarioContext == null) throw new ArgumentNullException("scenarioContext");
            this.scenarioContext = scenarioContext;

            if (featureContext == null) throw new ArgumentNullException("featureContext");
            this.featureContext = featureContext;

            this.driver = driver;
        }

        [BeforeScenario("Frames")]
        public void Initialize()
        {
            p_frames = new Page_Frames(driver);
            p_home = new Page_Home(driver);
            testData = Utilities.ExcelDataManager.GetTestData(Constants.SuiteType.UI, scenarioContext.ScenarioInfo.Title);
            ExtentReportsHelper.SetStepStatusInfo($"Test Data collection obtained for the test case {scenarioContext.ScenarioInfo.Title} is printed below:");
            ExtentReportsHelper.SetStepStatusInfoTableMarkup(Helpers.Get2DArrayFromCollection(testData));
        }

        [AfterScenario]
        public void TearDown()
        {
            Utilities.ExtentReportsHelper.Close();
            driver.Quit();
        }
        [When]
        public void WhenUserLaunchTheWebsiteAndClickOnFramesItem()
        {
            p_home.OpenHomePage();
            Utilities.ExtentReportsHelper.SetStepStatusPass("Home page navigated.");
            p_home.NavigateToItem(Page_Home.ListItem.Frames);
            Utilities.ExtentReportsHelper.SetStepStatusPass("Frames link is clicked fromt the main page.");
        }
        
        [When]
        public void WhenUserClicksOnIFrame()
        {
            string linkText = testData["Frame Link Text"];
            p_frames.ClickLink(linkText);
            ExtentReportsHelper.SetStepStatusPass($"Clicked on the '{linkText}' link on the Frames page.");
        }
        
        [When]
        public void WhenUserClearsCurrentTextAndEnterNewTextAndApplyBoldStyle()
        {
            string newTextToEnter = testData["New Text Message"];
            p_frames.ClearTextBox();
            ExtentReportsHelper.SetStepStatusPass($"Textbox on the text editor is cleared successfully.");
            p_frames.SetTextInTextBox(newTextToEnter);
            ExtentReportsHelper.SetStepStatusPass($"Textbox on the text editor is entered with the input text '{newTextToEnter}' successfully.");
            p_frames.ClickBoldButton();
            ExtentReportsHelper.SetStepStatusPass($"Bold Button is clicked successfully on the text editor.");
        }
        
        [Then]
        public void ThenUserShouldBeAbleToSeeIFrameLink()
        {
            string linkText = testData["Frame Link Text"];
            p_frames.isiFrameLinkPresent(linkText);
            ExtentReportsHelper.SetStepStatusPass($"Frame Link viz., '{linkText}' is present on the screen");
        }
        
        [Then]
        public void ThenUserShouldBeAbleToSeeThePredefinedText()
        {
            p_frames.WaitForiFrameElementsToLoad();
            var currentText = p_frames.GetTextFromTextBox();
            var defaultTextMsg = testData["Default Text Message"];
            Assert.IsTrue(currentText == defaultTextMsg, $"Default Text present in the textbox is '{defaultTextMsg}' which is as expected.");
            ExtentReportsHelper.SetStepStatusPass($"The default text present on the text editor is '{defaultTextMsg}' which is expected.");
        }
    }
}
