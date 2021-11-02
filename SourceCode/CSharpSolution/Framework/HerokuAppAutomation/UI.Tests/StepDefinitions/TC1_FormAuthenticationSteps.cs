using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using UI.Pom;
using Utilities;

namespace UI.Tests.StepDefinitions
{
    [Binding]
    public class TC1_FormAuthenticationSteps
    {
        private readonly ScenarioContext scenarioContext;
        private readonly FeatureContext featureContext;
        private IWebDriver driver;
        private Page_FormAuthentication p_formAuthentication;
        private Page_Home p_home;
        private Dictionary<string, string> testData = null;

        public TC1_FormAuthenticationSteps(ScenarioContext scenarioContext, FeatureContext featureContext, IWebDriver driver)
        {
            if (scenarioContext == null) throw new ArgumentNullException("scenarioContext");
            this.scenarioContext = scenarioContext;

            if (featureContext == null) throw new ArgumentNullException("featureContext");
            this.featureContext = featureContext;

            this.driver = driver;
        }

        [BeforeScenario("FormAuthentication")]
        public void Initialize()
        {
            p_formAuthentication = new Page_FormAuthentication(driver);
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
        public void WhenILaunchTheWebsiteAndClickOnFormAuthentication()
        {
            p_home.OpenHomePage();
            Utilities.ExtentReportsHelper.SetStepStatusPass("Home page navigated.");
            p_home.NavigateToItem(Page_Home.ListItem.FormAuthentication);
            Utilities.ExtentReportsHelper.SetStepStatusPass("Form Authentication link is clicked fromt the main page.");
        }
        
        [When]
        public void WhenIReadThoseStoredUsernameAndPasswordFileFromTestDataFileAndTryToLoginWithThoseCredentials()
        {
            string validUsername = testData["Valid Username Obtained from Portal"];
            string validPassword = testData["Valid Password Obtained from Portal"];
            Utilities.ExtentReportsHelper.SetStepStatusInfo($"Test data obtained for valid username is {validUsername}");
            Utilities.ExtentReportsHelper.SetStepStatusInfo($"Test data obtained for valid password is {validPassword}");
            p_formAuthentication.Login(validUsername, validPassword);
            Utilities.ExtentReportsHelper.SetStepStatusPass($"Username and password entered and login button clicked. Entered username='{validUsername}' password='{validPassword}'");
        }
        
        [When]
        public void WhenIEnterAnyInvalidCredentials()
        {
            p_formAuthentication.Logout();
            string invalidUsername = testData["Invalid Username"];
            string invalidPassword = testData["Invalid Password"];
            Utilities.ExtentReportsHelper.SetStepStatusInfo($"Test data obtained for invalid username is {invalidUsername}");
            Utilities.ExtentReportsHelper.SetStepStatusInfo($"Test data obtained for invalid password is {invalidPassword}");
            p_formAuthentication.Login(invalidUsername, invalidPassword);
            Utilities.ExtentReportsHelper.SetStepStatusPass($"Username and password entered and login button clicked. Entered username='{invalidUsername}' password='{invalidPassword}'");
        }

        [Then]
        public void ThenGetTheUsernameAndPasswordDisplayedOnScreenAndStoreThemInTestDataFile()
        {
            var creds = p_formAuthentication.GetUserCredentials();
            Utilities.ExtentReportsHelper.SetStepStatusInfo($"Obtained username from web application is '{creds.Key} and the password is {creds.Value}'");
            Utilities.ExtentReportsHelper.SetStepStatusInfo($"Credentials obtained from web application is stored into Test Data Excel file successfully.'");
            Utilities.ExcelDataManager.UpdateUsernameAndPasswordIntoTestData(creds.Key, creds.Value);
            testData = Utilities.ExcelDataManager.GetTestData(Constants.SuiteType.UI, scenarioContext.ScenarioInfo.Title);//Loading the test data again from Excel to get the updated username and password.
        }
        
        [Then]
        public void ThenUserMustBeLoggedInSuccessfully()
        {
            string successLoginMessage = testData["Success Login Message"];
            Utilities.ExtentReportsHelper.SetStepStatusInfo($"Test data obtained for success login messgae is {successLoginMessage}");
            p_formAuthentication.ValidateLoginSuccessMessage(successLoginMessage);
            Utilities.ExtentReportsHelper.SetStepStatusPass($"After logging in with valid credentials, user successfully logged in to the application and success message '{successLoginMessage}' appeared on the screen");
        }
        
        [Then]
        public void ThenIMustSeeFailureErrorMessage()
        {
            string failureLoginMessage = testData["Failure Login Message"];
            Utilities.ExtentReportsHelper.SetStepStatusInfo($"Test data obtained for failure loging messgae is {failureLoginMessage}");
            p_formAuthentication.ValidateLoginSuccessMessage(failureLoginMessage);
            Utilities.ExtentReportsHelper.SetStepStatusPass($"After logging in with invalid credentials, user was not allowed to login to the application and failure message '{failureLoginMessage}' appeared on the screen");
            Utilities.ExtentReportsHelper.SetStepStatusInfo($"Failure Message screenshot", Utilities.ScreenshotCapture.CaptureScreenshot(driver, Utilities.ExtentReportsHelper.currentReportPath));
        }
    }
}
