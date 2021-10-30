﻿using API.Base;
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
    public class F2_GetookingSteps : BaseClass
    {
        private readonly ScenarioContext scenarioContext;
        private readonly FeatureContext featureContext;
        private RestClient restClient;
        private Dictionary<string, string> testData = null;
        private IRestResponse response = null;
        private string BookingId = null;
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
            BookingId = testData["Valid Booking ID"];
            uri = string.Format(uri, BookingId);
            var requestHeaders = Utilities.Helpers.GetRequestHeaders(new RequestHeaders[] { RequestHeaders.ACCEPT, RequestHeaders.CONTENT_TYPE });
            try
            {
                response = base.GetCall(uri,requestHeaders);
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
            string uri = testData["uri"];
            BookingId = testData["Invalid Booking ID"];
            uri = string.Format(uri, BookingId);
            var requestHeaders = Utilities.Helpers.GetRequestHeaders(new RequestHeaders[] { RequestHeaders.ACCEPT, RequestHeaders.CONTENT_TYPE });
            try
            {
                response = base.GetCall(uri, requestHeaders);
                Utilities.ExtentReportsHelper.SetStepStatusInfo($"Endpoint URL={new Uri(restClient.BaseUrl, uri).AbsoluteUri + Environment.NewLine}");
            }
            catch (Exception ex)
            {
                Utilities.ExtentReportsHelper.SetTestStatusFail($"Unexpected exception occurred while trying to create booking.\n{ex.Message}\n{ex.StackTrace}");
            }
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
                    Utilities.ExtentReportsHelper.SetStepStatusPass($"Booking details of valid booking ID {BookingId} is received sucessfully.");
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
            string expectedResponseCode = testData["Expected Response Code"];
            string expectedResponseBody = testData["Expected Response Body"];
            HttpStatusCode expectedHttpResponse = Utilities.Helpers.GetHttpStatusCodeFromStr(expectedResponseCode);
            if (response != null)
            {
                if (response.StatusCode == expectedHttpResponse)
                {
                    Utilities.ExtentReportsHelper.SetStepStatusPass($"Since invalid booking ID {BookingId} is provided, Server returned expected error code.");
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
