using log4net;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Utilities.Constants;
using RestSharp;

namespace Utilities
{
    public static class Helpers
    {
        public static readonly ILog logger = LogManager.GetLogger(typeof(Helpers));

        public static string CreateReportPath(SuiteType suiteType)
        {
            var reportBasePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, Constants.GlobalProperties.UI.BASEPATH);
            if (!Directory.Exists(reportBasePath))
                Directory.CreateDirectory(reportBasePath);
            var reportPath = Path.Combine(reportBasePath, DateTime.Now.ToString(Constants.CURRENTDATETIMEFORMAT) + "_" + suiteType.ToString());
            Directory.CreateDirectory(reportPath);
            return reportPath;
        }

        public static string GetXpath(IWebElement element)
        {
            try
            {
                string str = element.ToString();
                string[] listString = null;
                if (str.Contains("xpath"))
                    listString = str.Split("xpath:".ToCharArray());
                else if (str.Contains("id"))
                    listString = str.Split("id:".ToCharArray());
                string last = listString[1].Trim();
                return last.Substring(0, last.Length - 1);
            }
            catch (Exception ex)
            {
                logger.Warn($"Unexpected exception occurred while trying to get the XPath from the web element.", ex);
                return string.Empty;
            }
        }

        public static HttpStatusCode GetHttpStatusCodeFromStr(string str)
        {
            HttpStatusCode status;
            if (Enum.TryParse(str, false, out status))
            {
                logger.Debug($"Valid string is provided. Converted Status code is {status}");
                return status;
            }
            else
            {
                logger.Debug($"inValid string is provided. Unable to convert to a valid HTTP Status Code. Provided string is {str}");
                throw new Exception($"Unexpected string is provided for HTTP status code. {str}");
            }
        }

        public static bool Compare2JsonStrings(string jsonStr1, string jsonStr2)
        {
            var left = JToken.Parse(jsonStr1);
            var right = JToken.Parse(jsonStr2);
            return JToken.DeepEquals(left, right);
        }

        public static Dictionary<RequestHeaders, string> GetRequestHeaders(RequestHeaders[] headerKey)
        {
            Dictionary<RequestHeaders, string> requestHeaders = new Dictionary<RequestHeaders, string>();
            foreach (var key in headerKey)
            {
                requestHeaders.Add(key, Constants.API.HeaderConstants.APPLICATION_JSON);
            }
            return requestHeaders;
        }

        public static string[,] Get2DArrayFromCollection(Dictionary<RequestHeaders, string> collection)
        {
            string[,] output = new string[collection.Count + 1,2];
            int i = 1;
            output[0, 0] = "Header Name";
            output[0, 1] = "Header Value";
            foreach (KeyValuePair<RequestHeaders, string> item in collection)
            {
                output[i, 0] = item.Key.ToDescriptionString();
                output[i, 1] = item.Value;
                i++;
            }
            return output;
        }
        public static string[,] Get2DArrayFromCollection(Dictionary<Cookies, string> collection)
        {
            string[,] output = new string[collection.Count + 1, 2];
            int i = 1;
            output[0, 0] = "Cookie Name";
            output[0, 1] = "Cookie Value";
            foreach (KeyValuePair<Cookies, string> item in collection)
            {
                output[i, 0] = item.Key.ToDescriptionString();
                output[i, 1] = item.Value;
                i++;
            }
            return output;
        }

        public static string[,] Get2DArrayFromCollection(Dictionary<string, string> collection)
        {
            string[,] output = new string[collection.Count + 1, 2];
            int i = 1;
            output[0, 0] = "Parameter Name";
            output[0, 1] = "Parameter Value";
            foreach (KeyValuePair<string, string> item in collection)
            {
                output[i, 0] = item.Key;
                output[i, 1] = item.Value;
                i++;
            }
            return output;
        }

        public static string[,] Get2DArrayFromCollection(IList<Parameter> collection)
        {
            if (collection == null || collection.Count == 0)
                return null;
            string[,] output = new string[collection.Count+1,2];
            int i = 1;
            output[0, 0] = "Header Name";
            output[0, 1] = "Header Value";
            foreach (var item in collection)
            {
                output[i, 0] = item.Name;
                output[i, 1] = item.Value.ToString();
                i++;
            }
            return output;
        }
    }
}
