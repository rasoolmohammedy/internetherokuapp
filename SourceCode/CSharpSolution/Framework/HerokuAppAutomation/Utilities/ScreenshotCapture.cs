using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using OpenQA.Selenium;

namespace Utilities
{
    public static class ScreenshotCapture
    {
        public static readonly ILog logger = LogManager.GetLogger(typeof(Helpers));
        public static string CurrentScreenshotPath
        {
            get { return currentScreenshotPath; }
        }
        private static string currentScreenshotPath;

        public static string CaptureScreenshot(IWebDriver driver, string currentReportFilePath)
        {
            try
            {
                ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
                Screenshot screenshot = screenshotDriver.GetScreenshot();
                var capturedScreenshotPath = Path.Combine(Path.GetDirectoryName(currentReportFilePath), DateTime.Now.ToString(Constants.CURRENTDATETIMEFORMAT) + ".png");
                screenshot.SaveAsFile(capturedScreenshotPath, ScreenshotImageFormat.Png);
                logger.Debug($"Screenshot captured and stored at {capturedScreenshotPath}");
                return capturedScreenshotPath;
            }
            catch (Exception ex)
            {
                var errMsg = $"Unexpected exception occurred while trying to capture the screenshot of the current browser window.";
                ExtentReportsHelper.SetTestStatusFail(errMsg);
                ExtentReportsHelper.SetTestStatusFail(ex);
                logger.Error(errMsg,ex);
                return null;
            }
        }
    }
}