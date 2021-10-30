using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Utilities.Constants;

namespace Utilities
{
     public static class Constants
    {
        public const string CHROMEDRIVERRELATIVEPATH = @"Resources";
        public const string TESTDATAFILEPATH = @"Data\TestData.xlsx";
        public const string REPORTFILENAME = "ExtentReport.html";
        public const string CURRENTDATETIMEFORMAT = "yyyy_MM_dd_H_mm_ss";
        public const string REPLACER = "@@@";

        public enum BrowserTypes
        {
            chrome
        }

        public enum SuiteType
        {
            UI,
            API
        }

        public enum RequestHeaders
        {
            [Description("Content-Type")]
            CONTENT_TYPE,
            [Description("Accept")]
            ACCEPT,
        }

        public enum Cookies
        {
            [Description("token")]
            TOKEN
        }

        public static class TestDataConsts
        {
            public const string HEADER_TESTCASENAME = "Test Case Name";
            public const string HEADER_PROPERTYNAME = "Property Name";
            public const string HEADER_PROPERTYVALUE = "Property Value";
            public const string WORKSHEET_UI_SUITE = "UISuite";
            public const string WORKSHEET_API_SUITE = "APISuite";
            public const int UI_TABLE_INDEX = 0;
            public const int API_TABLE_INDEX = 1;
        }

        public static class PropFileConsts
        {
            public const string CONFIG = "Config";
            public const string UIGLOBALPROPERTIESFILENAME = "UIGlobalProperties.ini";
            public const string APIGLOBALPROPERTIESFILENAME = "APIGlobalProperties.ini";
            public const string ENDPOINT = "Endpoint";
            public const string BASEURL = "baseUrl";
            public const string BASEPATH = "basePath";
            public const string REPORTSETTINGS = "ReportSettings";
            public const string ENVIRONMENTSETTINGS = "EnvironmentSettings";
            public const string USERNAME = "username";
            public const string PASSWORD = "password";
            public const string BROSWERTYPE= "browsertype";
        }

        public static class API
        {
            public const string AUTHURI = "auth";
            public static class HeaderConstants
            {
                public const string APPLICATION_JSON = "application/json";
                public const string COOKIEVALUE= "token={0}";
            }
        }
        public static class GlobalProperties
        {
            public static class UI
            {
                public static readonly string BASEURL = null;
                public static readonly string BASEPATH = null;
                public static readonly string BROSWERTYPE = null;
                public static readonly ILog logger = LogManager.GetLogger(typeof(UI));

                static UI()
                {
                    var propFilePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, PropFileConsts.CONFIG, PropFileConsts.UIGLOBALPROPERTIESFILENAME);
                    var inidata = Utilities.PropertiesReader.ReadPropertiesFile(propFilePath);
                    #region Loading Endpoint Section
                    if (inidata.Sections.ContainsSection(PropFileConsts.ENDPOINT))
                    {
                        try
                        {
                            BASEURL = inidata.Sections.GetSectionData(PropFileConsts.ENDPOINT).Keys.GetKeyData(PropFileConsts.BASEURL).Value;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Unexpected exception occurred while trying to load the properties from the file {propFilePath}. Kindly ensure the key names are appropriately defined in the property file.", ex);
                        }
                    }
                    else
                    {
                        throw new Exception($"Configuration error! {PropFileConsts.UIGLOBALPROPERTIESFILENAME} doesn't contain section named {PropFileConsts.ENDPOINT}. Kindly update the configuration file present at {propFilePath} with correct section name");
                    }
                    #endregion
                    #region Loading Report Settings Section
                    if (inidata.Sections.ContainsSection(PropFileConsts.REPORTSETTINGS))
                    {
                        try
                        {
                            BASEPATH = inidata.Sections.GetSectionData(PropFileConsts.REPORTSETTINGS).Keys.GetKeyData(PropFileConsts.BASEPATH).Value;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Unexpected exception occurred while trying to load the properties from the file {propFilePath}. Kindly ensure the key names are appropriately defined in the property file.", ex);
                        }
                    }
                    else
                    {
                        throw new Exception($"Configuration error! {PropFileConsts.UIGLOBALPROPERTIESFILENAME} doesn't contain section named {PropFileConsts.REPORTSETTINGS}. Kindly update the configuration file present at {propFilePath} with correct section name");
                    }
                    #endregion
                    #region Loading EnvironmentSettings Section
                    if (inidata.Sections.ContainsSection(PropFileConsts.ENVIRONMENTSETTINGS))
                    {
                        try
                        {
                            BROSWERTYPE = inidata.Sections.GetSectionData(PropFileConsts.ENVIRONMENTSETTINGS).Keys.GetKeyData(PropFileConsts.BROSWERTYPE).Value;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Unexpected exception occurred while trying to load the properties from the file {propFilePath}. Kindly ensure the key names are appropriately defined in the property file.", ex);
                        }
                    }
                    else
                    {
                        throw new Exception($"Configuration error! {PropFileConsts.UIGLOBALPROPERTIESFILENAME} doesn't contain section named {PropFileConsts.ENVIRONMENTSETTINGS}. Kindly update the configuration file present at {propFilePath} with correct section name");
                    }
                    #endregion
                    logger.Info($"Properties loaded from the file ${propFilePath}");
                }
            }

            public static class API
            {
                public static readonly string BASEURL = null;
                public static readonly string USERNAME = null;
                public static readonly string PASSWORD = null;
                public static readonly ILog logger = LogManager.GetLogger(typeof(API));

                static API()
                {
                    var propFilePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, PropFileConsts.CONFIG, PropFileConsts.APIGLOBALPROPERTIESFILENAME);
                    var inidata = Utilities.PropertiesReader.ReadPropertiesFile(propFilePath);
                    #region Loading Endpoint Section
                    if (inidata.Sections.ContainsSection(PropFileConsts.ENDPOINT))
                    {
                        try
                        {
                            BASEURL = inidata.Sections.GetSectionData(PropFileConsts.ENDPOINT).Keys.GetKeyData(PropFileConsts.BASEURL).Value;
                            USERNAME = inidata.Sections.GetSectionData(PropFileConsts.ENDPOINT).Keys.GetKeyData(PropFileConsts.USERNAME).Value;
                            PASSWORD = inidata.Sections.GetSectionData(PropFileConsts.ENDPOINT).Keys.GetKeyData(PropFileConsts.PASSWORD).Value;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Unexpected exception occurred while trying to load the properties from the file {propFilePath}. Kindly ensure the key names are appropriately defined in the property file.", ex);
                        }
                    }
                    else
                    {
                        throw new Exception($"Configuration error! {PropFileConsts.APIGLOBALPROPERTIESFILENAME} doesn't contain section named {PropFileConsts.ENDPOINT}. Kindly update the configuration file present at {propFilePath} with correct section name");
                    }
                    #endregion
                    logger.Info($"Properties loaded from the file ${propFilePath}");
                }
            }
        }
    }
}
