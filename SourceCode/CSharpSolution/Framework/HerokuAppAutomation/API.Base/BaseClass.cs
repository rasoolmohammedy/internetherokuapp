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
        protected IRestResponse PostCall(string uri, string body, Dictionary<RequestHeaders, string> requestHeaders)
        {
            IRestResponse response;
            RestRequest request = PrepareRestRequest(uri,Method.POST, requestHeaders, body);
            try
            {
                response = client.Execute(request);
                logger.Debug($"API with POST method has been executed successfully.");
                logger.Debug($"Status Code of the POST Method is {response.StatusCode + Environment.NewLine} Response is {response.Content}");
            }
            catch (Exception ex)
            {
                var msg = $"Unexpected exception occurred while trying to perform post call on the URI {uri}" +
                          ex.Message;
                logger.Debug(msg);
                throw new Exception(msg, ex);
            }
            return response;
        }

        protected IRestResponse GetCall(string uri, Dictionary<RequestHeaders, string> requestHeaders)
        {
            IRestResponse response = null;
            RestRequest request = PrepareRestRequest(uri, Method.GET, requestHeaders);
            try
            {
                response = client.Execute(request);
                logger.Debug($"API with GET method has been executed successfully.");
                logger.Debug($"Status Code of the GET Method is {response.StatusCode + Environment.NewLine} Response is {response.Content}");
            }
            catch (Exception ex)
            {
                var msg = $"Unexpected exception occurred while trying to perform get call on the URI {uri}" +
                          ex.Message;
                logger.Debug(msg);
                throw new Exception(msg, ex);
            }
            return response;
        }

        protected IRestResponse PutCall(string uri, Dictionary<RequestHeaders, string> requestHeaders, string body, Dictionary<Cookies, string> cookies)
        {
            IRestResponse response = null;
            RestRequest request = PrepareRestRequest(uri, Method.PUT, requestHeaders,body, cookies);
            try
            {
                response = client.Execute(request);
                logger.Debug($"API with PUT method has been executed successfully.");
                logger.Debug($"Status Code of the PUT Method is {response.StatusCode + Environment.NewLine} Response is {response.Content}");
            }
            catch (Exception ex)
            {
                var msg = $"Unexpected exception occurred while trying to perform put call on the URI {uri}" +
                          ex.Message;
                logger.Debug(msg);
                throw new Exception(msg, ex);
            }
            return response;
        }

        protected IRestResponse DeleteCall(string uri, Dictionary<RequestHeaders, string> requestHeaders, Dictionary<Cookies, string> cookies)
        {
            IRestResponse response = null;
            RestRequest request = PrepareRestRequest(uri, Method.DELETE, requestHeaders,cookies:cookies);
            try
            {
                response = client.Execute(request);
                logger.Debug($"API with Delete method has been executed successfully.");
                logger.Debug($"Status Code of the DELETE Method is {response.StatusCode + Environment.NewLine} Response is {response.Content}");
            }
            catch (Exception ex)
            {
                var msg = $"Unexpected exception occurred while trying to perform Delete call on the URI {uri}" +
                          ex.Message;
                logger.Debug(msg);
                throw new Exception(msg, ex);
            }
            return response;
        }

        protected void AssertJsonResponses(string expectedJsonString, string actualJsonString)
        {
            bool isEqual = Utilities.Helpers.Compare2JsonStrings(expectedJsonString, actualJsonString);
            if(isEqual)
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

        private static RestRequest PrepareRestRequest(string uri, Method methodType, Dictionary<RequestHeaders, string> requestHeaders, string body=null, Dictionary<Cookies, string> cookies=null)
        {
            RestRequest request = new RestRequest(uri, methodType);
            if (body != null)
                request.AddJsonBody(body);
            if(cookies!=null)
                foreach (KeyValuePair<Cookies,string> cookie in cookies)
                {
                    request.AddCookie(cookie.Key.ToDescriptionString(), cookie.Value);
                }
            foreach (KeyValuePair< RequestHeaders,string> header in requestHeaders)
                request.AddHeader(header.Key.ToDescriptionString(), header.Value);
            return request;
        }
    }
}

