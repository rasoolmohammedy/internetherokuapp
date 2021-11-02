using API.Base;
using AventStack.ExtentReports.MarkupUtils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using TechTalk.SpecFlow;
using Utilities;
using static Utilities.Constants;

namespace API.Tests.StepDefinitions
{
    [Binding]
    public class F3_UpdateBookingSteps : BaseClass
    {
        private readonly ScenarioContext scenarioContext;
        private readonly FeatureContext featureContext;
        private RestClient restClient;
        private Dictionary<string, string> testData = null;
        private IRestResponse response = null;
        private string BookingId = null;

        public F3_UpdateBookingSteps(ScenarioContext scenarioContext, FeatureContext featureContext, RestClient restClient) : base(restClient)
        {
            if (scenarioContext == null) throw new ArgumentNullException("scenarioContext");
            this.scenarioContext = scenarioContext;

            if (featureContext == null) throw new ArgumentNullException("featureContext");
            this.featureContext = featureContext;

            this.restClient = restClient;
        }

        [BeforeScenario("UpdateBooking")]
        public void Initialize()
        {
            testData = Utilities.ExcelDataManager.GetTestData(Constants.SuiteType.API, scenarioContext.ScenarioInfo.Title);
            ExtentReportsHelper.SetStepStatusInfo($"Test Data collection obtained for the test case {scenarioContext.ScenarioInfo.Title} is printed below:");
            ExtentReportsHelper.SetStepStatusInfoTableMarkup(Helpers.Get2DArrayFromCollection(testData));
        }

        [AfterScenario]
        public void TearDown()
        {
            Utilities.ExtentReportsHelper.Close();
        }

        [When]
        public void WhenUserRequestsAAuthToken()
        {
            string uri = Constants.API.AUTHURI;
            string requestBody = JsonConvert.SerializeObject(new
            {
                username = Constants.GlobalProperties.API.USERNAME,
                password = Constants.GlobalProperties.API.PASSWORD
            }) ;
            var requestHeaders = Helpers.GetRequestHeaders(new RequestHeaders[] {RequestHeaders.CONTENT_TYPE });
            try
            {
                response = base.PostCall(uri, requestBody, requestHeaders,false);
            }
            catch (Exception ex)
            {
                Utilities.ExtentReportsHelper.SetTestStatusFail($"Unexpected exception occurred while trying to create auth token.\n{ex.Message}\n{ex.StackTrace}");
            }
        }
        
        [When]
        public void WhenTheUserTriesToUpdateABookingWithValidInputs()
        {
            string uri = testData["uri"];
            string BookingId = testData["Valid Booking ID"];
            uri = string.Format(uri, BookingId);
            string authToken = testData["Auth Token"];
            string requestBody = testData["Request Body"];
            var requestHeaders = Utilities.Helpers.GetRequestHeaders(new RequestHeaders[] { RequestHeaders.ACCEPT, RequestHeaders.CONTENT_TYPE});
            var cookies = new Dictionary<Cookies, string>() { { Cookies.TOKEN, authToken } };
            try
            {
                response = base.PutCall(uri,requestHeaders, requestBody,cookies);
            }
            catch (Exception ex)
            {
                Utilities.ExtentReportsHelper.SetTestStatusFail($"Unexpected exception occurred while trying to update booking.\n{ex.Message}\n{ex.StackTrace}");
            }
        }
        
        [When]
        public void WhenTheUserTriesToUpdateABookingWithInvalidDate()
        {
            string uri = testData["uri"];
            string BookingId = testData["Valid Booking ID"];
            uri = string.Format(uri, BookingId);
            string authToken = testData["Auth Token"];
            string requestBody = testData["Request Body"];
            var cookies = new Dictionary<Cookies, string>() { { Cookies.TOKEN, authToken } };
            var requestHeaders = Utilities.Helpers.GetRequestHeaders(new RequestHeaders[] { RequestHeaders.ACCEPT, RequestHeaders.CONTENT_TYPE});
            try
            {
                response = base.PutCall(uri, requestHeaders, requestBody, cookies);
            }
            catch (Exception ex)
            {
                Utilities.ExtentReportsHelper.SetTestStatusFail($"Unexpected exception occurred while trying to update booking.\n{ex.Message}\n{ex.StackTrace}");
            }
        }
        
        [Then]
        public void ThenAValidAuthTokenMustBeGrantedAndUpdateTheTokenIntoTestDataAt_P0_Row(int rowNumber)
        {
            try
            {
                dynamic api = JObject.Parse(response.Content);
                string authToken = api.token;
                Utilities.ExcelDataManager.UpdatePropertyValueToTestData(Constants.SuiteType.API, rowNumber, 4, authToken);
                Initialize();
                Utilities.ExtentReportsHelper.SetStepStatusPass($"Auth Token generated successfully and it is stored back to test data file.");
                ExtentReportsHelper.SetStepStatusInfoLabelMarkup($"Auth Token: {authToken}",ExtentColor.Indigo,ExtentColor.White);
            }
            catch (Exception e)
            {
                Utilities.ExtentReportsHelper.SetTestStatusFail("Unexpected error occurred while trying to retrieve the booking Id from the API response and writing onto the test data excel");
            }
        }


        [Then]
        public void ThenBookingInformationForThatParticularBookingIDMustBeUpdatedSuccessfully()
        {
            string expectedResponse = testData["Expected Response Code"];
            string expectedResponseBody = testData["Expected Response Body"];
            HttpStatusCode expectedHttpResponse = Helpers.GetHttpStatusCodeFromStr(expectedResponse);
            if (response != null)
            {
                if (response.StatusCode == expectedHttpResponse)
                {
                    Utilities.ExtentReportsHelper.SetStepStatusPass($"Booking has been updated successfully.");
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
        public void ThenErrorMessageShouldBeReturnedToTheUserForUpdatingWithInvalidDate()
        {
            string expectedResponse = testData["Expected Response Code"];
            string expectedResponseBody = testData["Expected Response Body"];
            HttpStatusCode expectedHttpResponse = Utilities.Helpers.GetHttpStatusCodeFromStr(expectedResponse);
            if (response != null)
            {
                if (response.StatusCode == expectedHttpResponse)
                {
                    Utilities.ExtentReportsHelper.SetStepStatusPass($"Booking was not updated successfully, as we have provided incorrect request body with invalid date.");
                    Utilities.ExtentReportsHelper.SetStepStatusInfo($"Response Body={Environment.NewLine + response.Content}");
                }
                else
                {
                    Utilities.ExtentReportsHelper.SetTestStatusFail($"Unexpected Status code received. Obtained status code is {response.StatusCode}");
                }
                Assert.AreEqual(expectedHttpResponse, response.StatusCode, $"Unexpected Status code received from API.");
                AssertNonJsonResponess(expectedResponseBody, response.Content);
            }
            else
                Utilities.ExtentReportsHelper.SetTestStatusFail("Test case execution failed!.");
        }
    }
}
