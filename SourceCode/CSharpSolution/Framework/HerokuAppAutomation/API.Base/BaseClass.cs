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
        protected IRestResponse PostCall(string uri, string body, Dictionary<string,string> requestHeaders)
        {
            RestRequest request = new RestRequest(uri, Method.POST);
            IRestResponse response = null;
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(body);
            foreach (var header in requestHeaders)
                request.AddHeader(header.Key, header.Value);
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
                throw new Exception(msg,ex);
            }
            return response;
        }

        protected IRestResponse GetCall(string uri)
        {
            RestRequest request = new RestRequest(uri, Method.GET);
            IRestResponse response = null;
            request.AddHeader(Constants.API.HeaderConstants.CONTENT_TYPE, Constants.API.HeaderConstants.APPLICATION_JSON);
            request.AddHeader(Constants.API.HeaderConstants.ACCEPT, Constants.API.HeaderConstants.APPLICATION_JSON);
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

        protected IRestResponse PutCall(string uri,string body, string token)
        {
            RestRequest request = new RestRequest(uri, Method.PUT);
            IRestResponse response = null;
            request.RequestFormat = DataFormat.Json;
            request.AddHeader(Constants.API.HeaderConstants.CONTENT_TYPE, Constants.API.HeaderConstants.APPLICATION_JSON);
            request.AddHeader(Constants.API.HeaderConstants.ACCEPT, Constants.API.HeaderConstants.APPLICATION_JSON);
            request.AddHeader(Constants.API.HeaderConstants.COOKIE, string.Format(Constants.API.HeaderConstants.APPLICATION_JSON,token));
            request.AddBody(body);
            try
            {
                response = client.Execute(request);
                logger.Debug($"API with PUT method has been executed successfully.");
                logger.Debug($"Status Code of the GET Method is {response.StatusCode + Environment.NewLine} Response is {response.Content}");
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

        protected IRestResponse DeleteCall(string uri, string bookingId, string token)
        {
            RestRequest request = new RestRequest(uri, Method.DELETE);
            IRestResponse response = null;
            request.RequestFormat = DataFormat.Json;
            request.AddHeader(Constants.API.HeaderConstants.CONTENT_TYPE, Constants.API.HeaderConstants.APPLICATION_JSON);
            request.AddHeader(Constants.API.HeaderConstants.COOKIE, string.Format(Constants.API.HeaderConstants.APPLICATION_JSON, token));
            try
            {
                response = client.Execute(request);
                logger.Debug($"API with PUT method has been executed successfully.");
                logger.Debug($"Status Code of the GET Method is {response.StatusCode + Environment.NewLine} Response is {response.Content}");
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
        #endregion

        private void PrintOnReportJsonObjects(string expectedJsonString, string actualJsonString)
        {
            ExtentReportsHelper.SetStepStatusInfoJsonMarkup(expectedJsonString);
            ExtentReportsHelper.SetStepStatusInfoJsonMarkup(actualJsonString);
        }
    }
}

