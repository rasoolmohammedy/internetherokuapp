using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using API.Base;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Utilities;
using Newtonsoft.Json.Linq;
using static Utilities.Constants;
using System.Collections;
using static Utilities.ExcelDataManager;

namespace API.Tests.StepDefinitions
{
    [Binding]
    public class F1_CreateBookingSteps : BaseClass
    {
        private readonly ScenarioContext scenarioContext;
        private readonly FeatureContext featureContext;
        private RestClient restClient;
        private Dictionary<string, string> testData = null;
        private IRestResponse response = null;

        public F1_CreateBookingSteps(ScenarioContext scenarioContext, FeatureContext featureContext, RestClient restClient) : base(restClient)
        {
            if (scenarioContext == null) throw new ArgumentNullException("scenarioContext");
            this.scenarioContext = scenarioContext;

            if (featureContext == null) throw new ArgumentNullException("featureContext");
            this.featureContext = featureContext;

            this.restClient = restClient;
        }

        [BeforeScenario("CreateBooking")]
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
        public void WhenTheUserTriesToCreateABookingWithValidInput()
        {
            string uri = testData["uri"];
            string requestBody = testData["Request Body"];
            var requestHeaders = Utilities.Helpers.GetRequestHeaders(new RequestHeaders[] {RequestHeaders.ACCEPT,RequestHeaders.CONTENT_TYPE });
            try
            {
                response = base.PostCall(uri, requestBody, requestHeaders);
                Utilities.ExtentReportsHelper.SetStepStatusInfo($"Endpoint URL={new Uri(restClient.BaseUrl, uri).AbsoluteUri + Environment.NewLine}ReqestBody= {requestBody}");
            }
            catch (Exception ex)
            {
                Utilities.ExtentReportsHelper.SetTestStatusFail($"Unexpected exception occurred while trying to create booking.\n{ex.Message}\n{ex.StackTrace}");
            }
        }


        [Then]
        public void ThenBookingMustBeCreatedWithoutAnyError()
        {
            string expectedResponse = testData["Expected Response Code"];
            HttpStatusCode expectedHttpResponse = Utilities.Helpers.GetHttpStatusCodeFromStr(expectedResponse);
            if (response != null)
            {
                if (response.StatusCode == expectedHttpResponse)
                {
                    Utilities.ExtentReportsHelper.SetStepStatusPass($"Booking created successfully.");
                    Utilities.ExtentReportsHelper.SetStepStatusInfo($"Response Body={Environment.NewLine + response.Content}");
                }
                else
                {
                    Utilities.ExtentReportsHelper.SetTestStatusFail($"Unexpected Status code received. Obtained status code is {response.StatusCode}");
                }
                Assert.AreEqual(expectedHttpResponse, response.StatusCode, $"Unexpected Status code received from API.");
            }
            else
                Utilities.ExtentReportsHelper.SetTestStatusFail("Test case execution failed!.");
        }

        [Then]
        public void ThenStoreTheCreatedBookingIDBackToTestData()
        {
            try
            {
                dynamic api = JObject.Parse(response.Content);
                int bookingId = api.bookingid;
                Utilities.ExcelDataManager.UpdatePropertyValueToTestData(Constants.SuiteType.API,6,4, bookingId.ToString());
                Utilities.ExcelDataManager.UpdatePropertyValueToTestData(Constants.SuiteType.API, 16, 4, bookingId.ToString());
                Utilities.ExcelDataManager.UpdatePropertyValueToTestData(Constants.SuiteType.API, 25, 4, bookingId.ToString());
                Utilities.ExcelDataManager.UpdatePropertyValueToTestData(Constants.SuiteType.API, 31, 4, bookingId.ToString());
                Utilities.ExcelDataManager.UpdatePropertyValueToTestData(Constants.SuiteType.API, 37, 4, bookingId.ToString());
                Utilities.ExcelDataManager.UpdatePropertyValueToTestData(Constants.SuiteType.API, 47, 4, bookingId.ToString());
                Utilities.ExtentReportsHelper.SetStepStatusPass($"Booking ID retrieve from API response is {bookingId.ToString()} and it is stored back to test data file.");
            }
            catch (Exception e)
            {
                Utilities.ExtentReportsHelper.SetTestStatusFail("Unexpected error occurred while trying to retrieve the booking Id from the API response and writing onto the test data excel");
            }
            
        }


        [When]
        public void WhenTheUserTriesToCreateABookingWithInvalidInputWithNullValue()
        {
            string uri = testData["uri"];
            string requestBody = testData["Request Body"];
            var requestHeaders = Helpers.GetRequestHeaders(new RequestHeaders[] { RequestHeaders.ACCEPT, RequestHeaders.CONTENT_TYPE });
            try
            {
                response = base.PostCall(uri, requestBody, requestHeaders);
                Utilities.ExtentReportsHelper.SetStepStatusInfo($"Endpoint URL={new Uri(restClient.BaseUrl, uri).AbsoluteUri + Environment.NewLine}ReqestBody= {requestBody}");
            }
            catch (Exception ex)
            {
                Utilities.ExtentReportsHelper.SetTestStatusFail($"Unexpected exception occurred while trying to create booking with invalid request body.\n{ex.Message}\n{ex.StackTrace}");
            }
        }

        [Then]
        public void ThenBookingMustNotBeCreatedErrorMustBeThrown()
        {
            string expectedResponse = testData["Expected Response Code"];
            HttpStatusCode expectedHttpResponse = Utilities.Helpers.GetHttpStatusCodeFromStr(expectedResponse);
            if (response != null)
            {
                if (response.StatusCode == expectedHttpResponse)
                {
                    Utilities.ExtentReportsHelper.SetStepStatusPass($"Booking was not created successfully, as we have provided incorrect request body.");
                    Utilities.ExtentReportsHelper.SetStepStatusInfo($"Response Body={Environment.NewLine + response.Content}");
                }
                else
                {
                    Utilities.ExtentReportsHelper.SetTestStatusFail($"Unexpected Status code received. Obtained status code is {response.StatusCode}");
                }
                Assert.AreEqual(expectedHttpResponse, response.StatusCode, $"Unexpected Status code received from API.");
            }
            else
                Utilities.ExtentReportsHelper.SetTestStatusFail("Test case execution failed!.");
        }

        [When]
        public void WhenTheUserTriesToCreateABookingWithInvalidInputWithInvalidDateValue()
        {
            string uri = testData["uri"];
            string requestBody = testData["Request Body"];
            var requestHeaders = Helpers.GetRequestHeaders(new RequestHeaders[] { RequestHeaders.ACCEPT, RequestHeaders.CONTENT_TYPE });
            try
            {
                response = base.PostCall(uri, requestBody, requestHeaders);
                Utilities.ExtentReportsHelper.SetStepStatusInfo($"Endpoint URL={new Uri(restClient.BaseUrl, uri).AbsoluteUri + Environment.NewLine}ReqestBody= {requestBody}");
            }
            catch (Exception ex)
            {
                Utilities.ExtentReportsHelper.SetTestStatusFail($"Unexpected exception occurred while trying to create booking with invalid request body.\n{ex.Message}\n{ex.StackTrace}");
            }
        }

        [Then]
        public void ThenBookingMustNotBeCreatedErrorMustBeThrownStatingInvalidDate()
        {
            string expectedResponse = testData["Expected Response Code"];
            string expectedResponseMessage = testData["Expected Response Body"];
            HttpStatusCode expectedHttpResponse = Utilities.Helpers.GetHttpStatusCodeFromStr(expectedResponse);
            if (response != null)
            {
                if (response.StatusCode == expectedHttpResponse)
                {
                    Utilities.ExtentReportsHelper.SetStepStatusPass($"Booking was not created successfully, as we have provided incorrect request body.");
                    Utilities.ExtentReportsHelper.SetStepStatusInfo($"Response Body={Environment.NewLine + response.Content}");
                }
                else
                {
                    Utilities.ExtentReportsHelper.SetTestStatusFail($"Unexpected Status code received. Obtained status code is {response.StatusCode}");
                }
                Assert.AreEqual(expectedHttpResponse, response.StatusCode, $"Unexpected Status code received from API.");
                if (response.Content.Contains(expectedResponseMessage))
                    Utilities.ExtentReportsHelper.SetStepStatusPass($"API Response contains the expected error messate '{expectedResponseMessage}'");
                else
                    Utilities.ExtentReportsHelper.SetStepStatusWarning($"API response doesnot contain the expected error message '{expectedResponseMessage}'");
            }
            else
                Utilities.ExtentReportsHelper.SetTestStatusFail("Test case execution failed!.");
        }
     }
}
