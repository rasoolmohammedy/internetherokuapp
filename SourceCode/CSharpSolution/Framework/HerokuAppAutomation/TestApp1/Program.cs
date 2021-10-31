using ReportSender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.Constants;

namespace TestApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var suiteType = SuiteType.API;
            var reportDirectory = @"C:\Log\GitRepo\internetherokuapp\SourceCode\CSharpSolution\Framework\HerokuAppAutomation\API.Tests\bin\Debug\Reports\2021_10_30_21_10_03";
            var mailProperties = new MailProperties()
            {
                smtpServerAddress = "INMAARASOMOHA4P",
                smtpServerPortno = 25,
                ssl = false,
                fromAddr = "rasool.mohammed@xyz.com",
                toAddr = "rasool.mohammed@xyz.com",
                smtpServerUsername = "rasool.mohammed@xyz.com",
                smtpServerPassword = "Test@123",
                //ccAddr = "rasoolmailsend@gmail.com"
            };
            Sender.SendEmail(reportDirectory, suiteType, mailProperties);
        }
    }
}
