using API.Base;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using TechTalk.SpecFlow;
using Utilities;

namespace API.Tests.StepDefinitions
{
    [Binding]
    public class F2_GetookingSteps : BaseClass
    {
        private readonly ScenarioContext scenarioContext;
        private readonly FeatureContext featureContext;
        private RestClient restClient;
        private Dictionary<string, string> testData = null;
        private IRestResponse response = null;
        private string validBookingId = null;
        public F2_GetookingSteps(ScenarioContext scenarioContext, FeatureContext featureContext, RestClient restClient) : base(restClient)
        {
            if (scenarioContext == null) throw new ArgumentNullException("scenarioContext");
            this.scenarioContext = scenarioContext;

            if (featureContext == null) throw new ArgumentNullException("featureContext");
            this.featureContext = featureContext;

            this.restClient = restClient;
        }

        [BeforeScenario("GetBooking")]
        public void Initialize()
        {
            testData = Utilities.ExcelDataManager.GetTestData(Constants.SuiteType.API, scenarioContext.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void TearDown()
        {
            Utilities.ExtentReportsHelper.Close();
        }
        [When]
        public void WhenTheUserTriesToGetABookingWithValidBookingID()
        {
            string uri = testData["uri"];
            validBookingId = testData["Valid Booking ID"];
            uri = string.Format(uri, validBookingId);
            try
            {
                response = base.GetCall(uri);
                Utilities.ExtentReportsHelper.SetStepStatusInfo($"Endpoint URL={new Uri(restClient.BaseUrl, uri).AbsoluteUri + Environment.NewLine}");
            }
            catch (Exception ex)
            {
                Utilities.ExtentReportsHelper.SetTestStatusFail($"Unexpected exception occurred while trying to create booking.\n{ex.Message}\n{ex.StackTrace}");
            }
        }
        
        [When]
        public void WhenTheUserTriesToGetABookingWithInvalidBookingID()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then]
        public void ThenBookingInformationForThatParticularBookingIDMustBeReturnedToTheUser()
        {
            string expectedResponseCode = testData["Expected Response Code"];
            string expectedResponseBody = testData["Expected Response Body"];
            HttpStatusCode expectedHttpResponse = Utilities.Helpers.GetHttpStatusCodeFromStr(expectedResponseCode);
            if (response != null)
            {
                if (response.StatusCode == expectedHttpResponse)
                {
                    Utilities.ExtentReportsHelper.SetStepStatusPass($"Booking details of valid booking ID {validBookingId} is received sucessfully.");
                    Utilities.ExtentReportsHelper.SetStepStatusInfo($"Response Body={Environment.NewLine + response.Content}");
                }
                else
                {
                    Utilities.ExtentReportsHelper.SetTestStatusFail($"Unexpected Status code received. Obtained status code is {response.StatusCode}");
                }
                Assert.AreEqual(expectedHttpResponse, response.StatusCode, $"Unexpected Status code received from API.");
                AssertJsonResponses(expectedResponseBody, response.Content);
            }
            else
                Utilities.ExtentReportsHelper.SetTestStatusFail("Test case execution failed!.");
        }
        
        [Then]
        public void ThenErrorMessageShouldBeReturnedToTheUser()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
