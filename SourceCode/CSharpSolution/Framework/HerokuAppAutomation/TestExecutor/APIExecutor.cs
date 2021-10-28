using BoDi;
using log4net;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Utilities;

namespace TestExecutor
{
    [Binding]
    public sealed class APIExecutor : iExecutor
    {
        public ILog logger { get; set; }
        public string currentReportPath { get; set; }
        private readonly ScenarioContext scenarioContext;
        private readonly FeatureContext featureContext;
        private readonly IObjectContainer objectcontainer;
        public RestClient restClient { get; set; }

        public APIExecutor(ScenarioContext scenarioContext, FeatureContext featureContext, IObjectContainer objectcontainer)
        {
            #region Initializing Logger
            if (logger == null)
            {
                logger = LogManager.GetLogger(typeof(APIExecutor));
                logger.Info($"Logger initialized. UI Automatioin Suite Execution Begins");
            }
            else
            {
                logger.Info($"Logger is already initialized. API Automation Suite Execution Begins");
            }
            #endregion

            #region Initializing Extent Report
            if (ExtentReportsHelper.extent == null)
            {
                string reportPath = Utilities.Helpers.CreateReportPath();
                currentReportPath = Path.Combine(reportPath, Constants.REPORTFILENAME);
                Utilities.ExtentReportsHelper.InitializeExtentReport(currentReportPath, "Automation Testing Report", "Regression Testing", "the-internet.herokuapp.com", "QA");
            }
            else
            {
                logger.Info($"Extent report is already initialized. Current Report Path is {Utilities.ExtentReportsHelper.currentReportPath}");
                currentReportPath = Utilities.ExtentReportsHelper.currentReportPath;
            }
            #endregion
            if (scenarioContext == null) throw new ArgumentNullException("scenarioContext");
            this.scenarioContext = scenarioContext;
            if (featureContext == null) throw new ArgumentNullException("featureContext");
            this.featureContext = featureContext;
            this.objectcontainer = objectcontainer;
            Initializer();
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            Utilities.ExtentReportsHelper.Close();
        }

        public void Initializer()
        {

        }
    }
}
