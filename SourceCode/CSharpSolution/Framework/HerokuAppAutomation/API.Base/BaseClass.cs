using log4net;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

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
            foreach (var header in requestHeaders)
                request.AddHeader(header.Key, header.Value);
            request.AddBody(body);
            try
            {
                response = client.Execute(request);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected exception occurred while trying to perform post call on the URI {uri}", ex);
            }
            return response;
        }

        protected IRestResponse GetCall(string uri)
        {
            RestRequest request = new RestRequest(uri, Method.GET);
            IRestResponse response = null;
            request.AddHeader(Constants.API.HeaderConstants.CONTENT_TYPE, Constants.API.HeaderConstants.APPLICATION_JSON);
            try
            {
                response = client.Execute(request);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected exception occurred while trying to perform get call on the URI {uri}", ex);
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
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected exception occurred while trying to perform put call on the URI {uri}", ex);
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
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected exception occurred while trying to perform Delete call on the URI {uri}", ex);
            }
            return response;
        }
        #endregion
    }
}

