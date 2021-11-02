using log4net;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using RestSharp.Serialization.Json;
using RestSharp.Serializers.NewtonsoftJson;
using AventStack.ExtentReports;
using static Utilities.Constants;
using AventStack.ExtentReports.MarkupUtils;

namespace API.Base
{
    public abstract class BaseClass
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(BaseClass));

        private RestClient client;

        public BaseClass(RestClient client)
        {
            this.client = client;
        }

        #region Discrete Operations
        protected IRestResponse PostCall(string uri, string body, Dictionary<RequestHeaders, string> requestHeaders, bool needLogging = true)
        {
            IRestResponse response;
            RestRequest request = PrepareRestRequest(uri, Method.POST, requestHeaders, body, needLogging: needLogging);
            try
            {
                response = client.Execute(request);
                string successMsg = $"API with POST method has been executed successfully.";
                logger.Debug(successMsg);
                ExtentReportsHelper.SetStepStatusInfo(successMsg);
                if(needLogging)
                    ReportResponse(response);
                logger.Debug($"Status Code of the POST Method is {response.StatusCode + Environment.NewLine} Response is {response.Content}");
            }
            catch (Exception ex)
            {
                var msg = $"Unexpected exception occurred while trying to perform post call on the URI {uri}" +
                          ex.Message;
                logger.Debug(msg);
                ExtentReportsHelper.SetTestStatusFail(ex);
                throw new Exception(msg, ex);
            }
            return response;
        }

        protected IRestResponse GetCall(string uri, Dictionary<RequestHeaders, string> requestHeaders, bool needLogging = true)
        {
            IRestResponse response = null;
            RestRequest request = PrepareRestRequest(uri, Method.GET, requestHeaders, needLogging: needLogging);
            try
            {
                response = client.Execute(request);
                var successMsg = $"API with GET method has been executed successfully.";
                logger.Debug(successMsg);
                ExtentReportsHelper.SetStepStatusInfo(successMsg);
                if (needLogging)
                    ReportResponse(response);
                logger.Debug($"Status Code of the GET Method is {response.StatusCode + Environment.NewLine} Response is {response.Content}");
            }
            catch (Exception ex)
            {
                var msg = $"Unexpected exception occurred while trying to perform get call on the URI {uri}" +
                          ex.Message;
                logger.Debug(msg);
                ExtentReportsHelper.SetTestStatusFail(ex);
                throw new Exception(msg, ex);
            }
            return response;
        }

        protected IRestResponse PutCall(string uri, Dictionary<RequestHeaders, string> requestHeaders, string body, Dictionary<Cookies, string> cookies, bool needLogging = true)
        {
            IRestResponse response = null;
            RestRequest request = PrepareRestRequest(uri, Method.PUT, requestHeaders, body, cookies);
            try
            {
                response = client.Execute(request);
                var successMsg = $"API with PUT method has been executed successfully.";
                logger.Debug(successMsg);
                ExtentReportsHelper.SetStepStatusInfo(successMsg);
                if (needLogging)
                    ReportResponse(response);
                logger.Debug($"Status Code of the PUT Method is {response.StatusCode + Environment.NewLine} Response is {response.Content}");
            }
            catch (Exception ex)
            {
                var msg = $"Unexpected exception occurred while trying to perform put call on the URI {uri}" +
                          ex.Message;
                logger.Debug(msg);
                ExtentReportsHelper.SetTestStatusFail(ex);
                throw new Exception(msg, ex);
            }
            return response;
        }

        protected IRestResponse DeleteCall(string uri, Dictionary<RequestHeaders, string> requestHeaders, Dictionary<Cookies, string> cookies, bool needLogging = true)
        {
            IRestResponse response = null;
            RestRequest request = PrepareRestRequest(uri, Method.DELETE, requestHeaders, cookies: cookies);
            try
            {
                response = client.Execute(request);
                var successMsg = $"API with Delete method has been executed successfully.";
                logger.Debug(successMsg);
                ExtentReportsHelper.SetStepStatusInfo(successMsg);
                if (needLogging)
                    ReportResponse(response);
                logger.Debug($"Status Code of the DELETE Method is {response.StatusCode + Environment.NewLine} Response is {response.Content}");
            }
            catch (Exception ex)
            {
                var msg = $"Unexpected exception occurred while trying to perform Delete call on the URI {uri}" +
                          ex.Message;
                logger.Debug(msg);
                ExtentReportsHelper.SetTestStatusFail(ex);
                throw new Exception(msg, ex);
            }
            return response;
        }

        protected void AssertJsonResponses(string expectedJsonString, string actualJsonString)
        {
            bool isEqual = Utilities.Helpers.Compare2JsonStrings(expectedJsonString, actualJsonString);
            if (isEqual)
            {
                ExtentReportsHelper.SetStepStatusPass("Expected json object and actual json objects are equal. Expected json object and Actual json objects are printed below respectively.");
                PrintOnReportJsonObjects(expectedJsonString, actualJsonString);
            }
            else
            {
                ExtentReportsHelper.SetTestStatusFail("Expected json object and actual json objects are not equal. Expected json object and Actual json objects are printed below respectively.");
                PrintOnReportJsonObjects(expectedJsonString, actualJsonString);
            }
        }

        protected void AssertNonJsonResponess(string expectedNonJsonString, string actualNonJsonString)
        {
            bool isEqual = expectedNonJsonString == actualNonJsonString;
            if (isEqual)
            {
                ExtentReportsHelper.SetStepStatusPass("Expected response string and actual response string  are equal.");
                PrintOnReportNonJsonStrings(expectedNonJsonString, actualNonJsonString);
            }
            else
            {
                ExtentReportsHelper.SetTestStatusFail("Expected response string and actual response string  are not equal.");
                PrintOnReportNonJsonStrings(expectedNonJsonString, actualNonJsonString);
            }
        }

        private static void PrintOnReportNonJsonStrings(string expectedNonJsonString, string actualNonJsonString)
        {
            ExtentReportsHelper.SetStepStatusInfo($"Expected Response String is '{expectedNonJsonString}'");
            ExtentReportsHelper.SetStepStatusInfo($"Actual Response String is '{actualNonJsonString}'");
        }
        #endregion

        private void PrintOnReportJsonObjects(string expectedJsonString, string actualJsonString)
        {
            ExtentReportsHelper.SetStepStatusInfo($"Expected JSON objected is printed below:");
            ExtentReportsHelper.SetStepStatusInfoJsonMarkup(expectedJsonString);
            ExtentReportsHelper.SetStepStatusInfo($"Actual JSON objected is printed below:");
            ExtentReportsHelper.SetStepStatusInfoJsonMarkup(actualJsonString);
        }

        private RestRequest PrepareRestRequest(string uri, Method methodType, Dictionary<RequestHeaders, string> requestHeaders, string body = null, Dictionary<Cookies, string> cookies = null, bool needLogging = true)
        {
            if (needLogging)
            {
                ExtentReportsHelper.SetStepStatusInfo($"Base URL: '{client.BaseUrl}'");
                ExtentReportsHelper.SetStepStatusInfo($"Endpoint URI: '{new Uri(client.BaseUrl, uri).AbsoluteUri}'");
                ExtentReportsHelper.SetStepStatusInfoLabelMarkup($"Method Type : {methodType.ToString()}", ExtentColor.Orange, ExtentColor.White);
            };
            RestRequest request = new RestRequest(uri, methodType);
            if (body != null)
            {
                request.AddJsonBody(body);
                if (needLogging)
                {
                    ExtentReportsHelper.SetStepStatusInfo($"Json Request Body printed below:");
                    ExtentReportsHelper.SetStepStatusInfoJsonMarkup(body);
                }
            }
            if (needLogging)
            {
                ExtentReportsHelper.SetStepStatusInfo($"Request Headers information is printed below:");
                ExtentReportsHelper.SetStepStatusInfoTableMarkup(Helpers.Get2DArrayFromCollection(requestHeaders));
            }
            foreach (KeyValuePair<RequestHeaders, string> header in requestHeaders)
                request.AddHeader(header.Key.ToDescriptionString(), header.Value);
            if (cookies != null)
            {
                if (needLogging)
                {
                    ExtentReportsHelper.SetStepStatusInfo($"Request Cookie information is printed below:");
                    ExtentReportsHelper.SetStepStatusInfoTableMarkup(Helpers.Get2DArrayFromCollection(cookies));
                }
                foreach (KeyValuePair<Cookies, string> cookie in cookies)
                    request.AddCookie(cookie.Key.ToDescriptionString(), cookie.Value);
            }
            return request;
        }

        private void ReportResponse(IRestResponse response)
        {
            ExtentReportsHelper.SetStepStatusInfoLabelMarkup($"Status Code: {(int)response.StatusCode} | {response.StatusCode}", ExtentColor.Orange, ExtentColor.White);
            ExtentReportsHelper.SetStepStatusInfo($"Response Status:{response.ResponseStatus}");
            ExtentReportsHelper.SetStepStatusInfo($"Response Content Type : '{response.ContentType}'");
            ExtentReportsHelper.SetStepStatusInfo($"Response Content Length : '{response.ContentLength}'");
            ExtentReportsHelper.SetStepStatusInfo($"Response Content is printed below: ");
            if (response.ContentType.Contains("json"))
                ExtentReportsHelper.SetStepStatusInfoJsonMarkup(response.Content);
            else
                ExtentReportsHelper.SetStepStatusInfo(response.Content);
            ExtentReportsHelper.SetStepStatusInfo($"Response Headers information is printed below:");
            ExtentReportsHelper.SetStepStatusInfoTableMarkup(Helpers.Get2DArrayFromCollection(response.Headers));
        }
    }
}

