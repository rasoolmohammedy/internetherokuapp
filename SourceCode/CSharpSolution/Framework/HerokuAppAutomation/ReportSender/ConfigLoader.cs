using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportSender
{
    public class SMTPDetails
    {
        [JsonProperty("smtpServerAddress")]
        public string SmtpServerAddress { get; set; }

        [JsonProperty("smtpServerPortno")]
        public int SmtpServerPortno { get; set; }

        [JsonProperty("ssl")]
        public bool Ssl { get; set; }

        [JsonProperty("fromAddr")]
        public string FromAddr { get; set; }

        [JsonProperty("toAddr")]
        public string ToAddr { get; set; }

        [JsonProperty("ccAddr")]
        public string CcAddr { get; set; }

        [JsonProperty("smtpServerUsername")]
        public string SmtpServerUsername { get; set; }

        [JsonProperty("smtpServerPassword")]
        public string SmtpServerPassword { get; set; }
    }

    public class ResultsConfiguration
    {
        [JsonProperty("sendResultsEmail")]
        public bool SendResultsEmail { get; set; }

        [JsonProperty("launchResultsAfterExecution")]
        public bool LaunchResultsAfterExecution { get; set; }
    }

    public class ExecutorSettings
    {
        [JsonProperty("AllBinariesBaseDir")]
        public string AllBinariesBaseDir { get; set; }

        [JsonProperty("NUnitConsoleRunnerExePath")]
        public string NUnitConsoleRunnerExePath { get; set; }

        [JsonProperty("TestCasesOrderConfigFilePath")]
        public string TestCasesOrderConfigFilePath { get; set; }

        [JsonProperty("APISuiteDLLPath")]
        public string APISuiteDLLPath { get; set; }

        [JsonProperty("UISuiteDLLPath")]
        public string UISuiteDLLPath { get; set; }
        [JsonProperty("NUnitResultsXMLPath")] 
        public string NUnitResultsXMLPath { get; set; }

        [JsonProperty("NUnitOutputPath")]
        public string NUnitOutputPath { get; set; }
    }

    public class ConfigLoader
    {
        [JsonProperty("SMTPDetails")]
        public SMTPDetails SMTPDetails { get; set; }

        [JsonProperty("ResultsConfiguration")]
        public ResultsConfiguration ResultsConfiguration { get; set; }

        [JsonProperty("ExecutorSettings")]
        public ExecutorSettings ExecutorSettings { get; set; }
    }
}
