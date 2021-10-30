using API.Base;
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
    public class F4_DeleteBookingSteps : BaseClass
    {
        private readonly ScenarioContext scenarioContext;
        private readonly FeatureContext featureContext;
        private RestClient restClient;
        private Dictionary<string, string> testData = null;
        private IRestResponse response = null;
        private string BookingId = null;

        public F4_DeleteBookingSteps(ScenarioContext scenarioContext, FeatureContext featureContext, RestClient restClient) : base(restClient)
        {
            if (scenarioContext == null) throw new ArgumentNullException("scenarioContext");
            this.scenarioContext = scenarioContext;
            if (featureContext == null) throw new ArgumentNullException("featureContext");
            this.featureContext = featureContext;
            this.restClient = restClient;
        }

        [BeforeScenario("DeleteBooking")]
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
        public void WhenTheUserTriesToDeleteABookingWithValidBookingId()
        {
            string uri = testData["uri"];
            string BookingId = testData["Valid Booking ID"];
            uri = string.Format(uri, BookingId);
            string authToken = testData["Auth Token"];
            var requestHeaders = Utilities.Helpers.GetRequestHeaders(new RequestHeaders[] { RequestHeaders.ACCEPT, RequestHeaders.CONTENT_TYPE });
            var cookies = new Dictionary<Cookies, string>() { { Cookies.TOKEN, authToken } };
            try
            {
                response = base.DeleteCall(uri, requestHeaders, cookies);
                Utilities.ExtentReportsHelper.SetStepStatusInfo($"Endpoint URL={new Uri(restClient.BaseUrl, uri).AbsoluteUri}");
            }
            catch (Exception ex)
            {
                Utilities.ExtentReportsHelper.SetTestStatusFail($"Unexpected exception occurred while trying to delete booking with booking ID {BookingId}\n{ex.Message}\n{ex.StackTrace}");
            }
        }
        
        [When]
        public void WhenTheUserTriesToDeleteABookingWithInvalidBookingId()
        {
            string uri = testData["uri"];
            string BookingId = testData["Invalid Booking ID"];
            uri = string.Format(uri, BookingId);
            string authToken = testData["Auth Token"];
            var requestHeaders = Utilities.Helpers.GetRequestHeaders(new RequestHeaders[] { RequestHeaders.ACCEPT, RequestHeaders.CONTENT_TYPE });
            var cookies = new Dictionary<Cookies, string>() { { Cookies.TOKEN, authToken } };
            try
            {
                response = base.DeleteCall(uri, requestHeaders, cookies);
                Utilities.ExtentReportsHelper.SetStepStatusInfo($"Endpoint URL={new Uri(restClient.BaseUrl, uri).AbsoluteUri}");
            }
            catch (Exception ex)
            {
                Utilities.ExtentReportsHelper.SetTestStatusFail($"Unexpected exception occurred while trying to update booking.\n{ex.Message}\n{ex.StackTrace}");
            }
        }
        
        [When]
        public void WhenTheUserTriesToDeleteABookingWithInvalidAuthTokenWithAValidBookingId()
        {
            string uri = testData["uri"];
            string BookingId = testData["Valid Booking ID"];
            uri = string.Format(uri, BookingId);
            string authToken = testData["Invalid Auth Token"];
            var requestHeaders = Utilities.Helpers.GetRequestHeaders(new RequestHeaders[] { RequestHeaders.ACCEPT, RequestHeaders.CONTENT_TYPE });
            var cookies = new Dictionary<Cookies, string>() { { Cookies.TOKEN, authToken } };
            try
            {
                response = base.DeleteCall(uri, requestHeaders, cookies);
                Utilities.ExtentReportsHelper.SetStepStatusInfo($"Endpoint URL={new Uri(restClient.BaseUrl, uri).AbsoluteUri}");
            }
            catch (Exception ex)
            {
                Utilities.ExtentReportsHelper.SetTestStatusFail($"Unexpected exception occurred while trying to update booking.\n{ex.Message}\n{ex.StackTrace}");
            }
        }
        
        [Then]
        public void ThenBookingInformationForThatParticularBookingIDMustBeDeleteSuccessfully()
        {
            string expectedResponse = testData["Expected Response Code"];
            string expectedResponseBody = testData["Expected Response Body"];
            HttpStatusCode expectedHttpResponse = Helpers.GetHttpStatusCodeFromStr(expectedResponse);
            if (response != null)
            {
                if (response.StatusCode == expectedHttpResponse)
                {
                    Utilities.ExtentReportsHelper.SetStepStatusPass($"Booking has been deleted successfully.");
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
        
        [Then]
        public void ThenForbiddenErrorMessageShouldBeReturnedToTheUser()
        {
            string expectedResponse = testData["Expected Response Code"];
            string expectedResponseBody = testData["Expected Response Body"];
            HttpStatusCode expectedHttpResponse = Utilities.Helpers.GetHttpStatusCodeFromStr(expectedResponse);
            if (response != null)
            {
                if (response.StatusCode == expectedHttpResponse)
                {
                    Utilities.ExtentReportsHelper.SetStepStatusPass($"Booking was not deleted successfully, as we have provided invalid input data.");
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
