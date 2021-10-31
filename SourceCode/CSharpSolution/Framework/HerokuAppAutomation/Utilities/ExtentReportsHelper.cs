using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports.MarkupUtils;

namespace Utilities
{
    public static class ExtentReportsHelper
    {
        public static ExtentReports extent { get; set; }
        public static ExtentHtmlReporter reporter { get; set; }
        public static ExtentTest test { get; set; }

        public static string currentReportPath;
        public static void InitializeExtentReport(string reportPath, string docTitle, string reportName, string appUnderTest, string environment)
        {
            currentReportPath = reportPath;
            extent = new ExtentReports();
            reporter = new ExtentHtmlReporter(reportPath);
            reporter.Config.CSS ="img.r-img { width: 100%; }";
            reporter.Config.DocumentTitle = docTitle;
            reporter.Config.ReportName = reportName;
            reporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;
            extent.AttachReporter(reporter);
            extent.AddSystemInfo("Application Under Test", appUnderTest);
            extent.AddSystemInfo("Environment", environment);
            extent.AddSystemInfo("Machine", Environment.MachineName);
            extent.AddSystemInfo("OS", Environment.OSVersion.VersionString);
        }
        public static void CreateTest(string testName,Constants.SuiteType category)
        {
            test = extent.CreateTest(testName);
            test.AssignCategory(category.ToString());
        }
        public static void SetStepStatusPass(string stepDescription)
        {
            test.Log(Status.Pass, stepDescription);
        }
        public static void SetStepStatusWarning(string stepDescription)
        {
            test.Log(Status.Warning, stepDescription);
        }
        public static void SetStepStatusInfo(string stepDescription)
        {
            test.Log(Status.Info, stepDescription);
        }

        public static void SetStepStatusInfo(string stepDescription, string screenshotPath)
        {
            MediaEntityModelProvider meiaModel = MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build();
            test.Log(Status.Info, stepDescription, meiaModel);
        }

        public static void SetStepStatusInfoJsonMarkup(string jsonString)
        {
            test.Info(MarkupHelper.CreateCodeBlock(jsonString, CodeLanguage.Json));
        }

        public static void SetTestStatusPass()
        {
            test.Pass("Test Executed Sucessfully!");
        }
        public static void SetTestStatusFail(string message = null)
        {
            var printMessage = "<p><b>Test FAILED!</b></p>";
            if (!string.IsNullOrEmpty(message))
            {
                printMessage += $"Message: <br>{message}<br>";
            }
            test.Fail(printMessage);
        }
        public static void AddScreenshot(string base64ScreenCapture)
        {
            test.AddScreenCaptureFromPath(base64ScreenCapture, "Screenshot on Error:");  
        }
        public static void SetTestStatusSkipped()
        {
            test.Skip("Test skipped!");
        }
        public static void Close()
        {
            extent.Flush();
        }
    }
}
