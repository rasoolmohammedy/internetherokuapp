﻿using log4net;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities
{
    public static class Helpers
    {
        public static readonly ILog logger = LogManager.GetLogger(typeof(Helpers));

        public static string CreateReportPath()
        {
            var reportBasePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, Constants.GlobalProperties.UI.BASEPATH);
            if (!Directory.Exists(reportBasePath))
                Directory.CreateDirectory(reportBasePath);
            var reportPath = Path.Combine(reportBasePath, DateTime.Now.ToString("yyyy_MM_dd_H_mm_ss"));
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
    }
}
