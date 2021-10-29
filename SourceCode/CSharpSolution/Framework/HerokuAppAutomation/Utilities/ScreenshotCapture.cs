using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Utilities
{
    public static class ScreenshotCapture
    {
        public static string CurrentScreenshotPath
        {
            get { return currentScreenshotPath; }
        }
        private static string currentScreenshotPath;

        public static string CaptureScreenshot(IWebDriver driver, string currentReportFilePath)
        {
            ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
            Screenshot screenshot = screenshotDriver.GetScreenshot();
            var capturedScreenshotPath = Path.Combine(Path.GetDirectoryName(currentReportFilePath), DateTime.Now.ToString(Constants.CURRENTDATETIMEFORMAT)+".png");
            screenshot.SaveAsFile(capturedScreenshotPath,ScreenshotImageFormat.Png);
            return capturedScreenshotPath;
        }
    }
}