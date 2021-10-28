﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using API.Base;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Utilities;

namespace API.Tests.StepDefinitions
{
    [Binding]
    public class TC1_CreateBooking_PositiveCaseSteps:BaseClass
    {
        private readonly ScenarioContext scenarioContext;
        private readonly FeatureContext featureContext;
        private RestClient restClient;
        private Dictionary<string, string> testData = null;
        private IRestResponse response = null;

        public TC1_CreateBooking_PositiveCaseSteps(ScenarioContext scenarioContext, FeatureContext featureContext, RestClient restClient):base(restClient)
        {
            if (scenarioContext == null) throw new ArgumentNullException("scenarioContext");
            this.scenarioContext = scenarioContext;

            if (featureContext == null) throw new ArgumentNullException("featureContext");
            this.featureContext = featureContext;

            this.restClient = restClient;
        }

        [BeforeScenario]
        public void Initialize()
        {
            Utilities.ExtentReportsHelper.CreateTest(scenarioContext.ScenarioInfo.Title, Constants.SuiteType.API);
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
            Dictionary<string, string> requestHeaders = new Dictionary<string, string>();
            requestHeaders.Add(Constants.API.HeaderConstants.ACCEPT, Constants.API.HeaderConstants.APPLICATION_JSON);
            requestHeaders.Add(Constants.API.HeaderConstants.CONTENT_TYPE, Constants.API.HeaderConstants.APPLICATION_JSON);
            try
            {
                response = base.PostCall(uri, requestBody, requestHeaders);
                Utilities.ExtentReportsHelper.SetStepStatusInfo($"Endpoint URL={new Uri(restClient.BaseUrl, uri).AbsoluteUri+ Environment.NewLine}ReqestBody= {requestBody}");
            }
            catch (Exception ex)
            {
                Utilities.ExtentReportsHelper.SetTestStatusFail($"Unexpected exception occurred while trying to create booking.\n{ex.Message}\n{ex.StackTrace}");
            }
        }

        [Then]
        public void ThenBookingMustBeCreatedWithoutAnyError()
        {
            if (response != null)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Utilities.ExtentReportsHelper.SetStepStatusPass($"Booking created successfully.");
                    Utilities.ExtentReportsHelper.SetStepStatusInfo($"Response Body={Environment.NewLine + response.Content}");
                }
                else
                {
                    Utilities.ExtentReportsHelper.SetTestStatusFail($"Unexpected Status code received. Obtained status code is {response.StatusCode}");
                }
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, $"Unexpected Status code received from API.");
            }
            else
                Utilities.ExtentReportsHelper.SetTestStatusFail("Test case execution failed!.");
        }
    }
}
