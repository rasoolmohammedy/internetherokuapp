using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using UI.Pom;
using Utilities;

namespace UI.Tests.StepDefinitions
{
    [Binding]
    public class TC6_JavaScriptAlertsSteps
    {
        private readonly ScenarioContext scenarioContext;
        private readonly FeatureContext featureContext;
        private IWebDriver driver;
        private Page_JavaScriptAlerts p_jsalerts;
        private Page_Home p_home;
        private Dictionary<string, string> testData = null;
        private string firstBoxTitle;
        private string secondBoxTitle;

        public TC6_JavaScriptAlertsSteps(ScenarioContext scenarioContext, FeatureContext featureContext, IWebDriver driver)
        {
            if (scenarioContext == null) throw new ArgumentNullException("scenarioContext");
            this.scenarioContext = scenarioContext;

            if (featureContext == null) throw new ArgumentNullException("featureContext");
            this.featureContext = featureContext;

            this.driver = driver;
        }

        [BeforeScenario("jsAlerts")]
        public void Initialize()
        {
            p_jsalerts = new Page_JavaScriptAlerts(driver);
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
        public void WhenUserLaunchTheWebsiteAndClickOnJavaScriptLink()
        {
            p_home.OpenHomePage();
            Utilities.ExtentReportsHelper.SetStepStatusPass("Home page navigated.");
            p_home.NavigateToItem(Page_Home.ListItem.JavascriptAlerts);
            Utilities.ExtentReportsHelper.SetStepStatusPass("JavaScript Alerts link is clicked fromt the main page.");
        }

        [When]
        public void WhenUserClicksCancelButton()
        {
            bool isAlertDismissed = p_jsalerts.DismissAlert();
            if (isAlertDismissed)
                ExtentReportsHelper.SetStepStatusPass($"Alert displayed on the web browser is successfully dismissed.");
            else
                ExtentReportsHelper.SetTestStatusFail($"Unable to dismiss the Alert popup displayed on the web browser.");
        }

        [Then]
        public void ThenUserShouldBeAbleToSeeClickForJSConfirmButton()
        {
            string buttonText = testData["Button Text"];
            p_jsalerts.ClickJSConfirmButtonMustBePresent(buttonText);
            ExtentReportsHelper.SetStepStatusPass($"'{buttonText}' button is present on the JavaScript Alerts webpage");
        }

        [Then]
        public void ThenUserShouldBeAbleToSeeCancelledValidationTextMessageOnScreen()
        {
            string expectedResultMsg = testData["Result Text Message"];
            p_jsalerts.ValidateResultTextMessage(expectedResultMsg);
            ExtentReportsHelper.SetStepStatusPass($"'{expectedResultMsg}' message is present on the page which is as expected, after clickiing on Cancel button from the JavaScript Alert");
        }

        [When]
        public void WhenUserClicksOnJSConfirmButton()
        {
            string buttonText = testData["Button Text"];
            p_jsalerts.ClickJSConfirmButton(buttonText);
            ExtentReportsHelper.SetStepStatusPass($"'{buttonText}' button is clicked on the JavaScript Alerts webpage");
        }

        [Then]
        public void ThenJavaScriptAlertMustBeShown()
        {
            p_jsalerts.ValidateWhetherAlertIsShown();
            string alertText = p_jsalerts.GetAlertText();
            if(alertText!=null)
                ExtentReportsHelper.SetStepStatusPass($"Text message displayed on the alert popup is '{alertText}'.");
            else
                ExtentReportsHelper.SetTestStatusFail($"Unable to get text message from alert displayed.");
        }
    }
}
