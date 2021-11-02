using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using static Utilities.Constants;

namespace ReportSender
{
    public class TCFields
    {
        public string testCaseName;
        public TCStatus testCaseStatus;
        public string startDateTime;
        public string duration;
    }

    public class ReportTemplate
    {
        public List<TCFields> tcFields;
        public string totalDuration;
        public int totalPassedTCs;
        public int totalFailedTCs;
    }

    public class MailProperties
    {
        public string fromAddr;
        public string toAddr;
        public string ccAddr;
        public string smtpServerAddress;
        public int smtpServerPortno;
        public string smtpServerUsername;
        public string smtpServerPassword;
        public bool ssl;
    }
    public enum ReporFileType
    {
        index,
        dashboard
    }

    public enum TCStatus
    {
        Fail,
        Pass
    }
    public static class Sender
    {

        public static string reportDirectory;
        public static SuiteType suiteType;
        public static MailProperties mailProperties;
        private static Dictionary<ReporFileType, string> reportFilePaths;
        private static string[] attachmentFiles;

        public static void SendEmail(string reportDirectory, SuiteType suiteType, MailProperties mailProperties)
        {
            string templateFilePath = null;
            if(suiteType==SuiteType.API)
                templateFilePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"Templates\APITemplate.html");
            else
                templateFilePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"Templates\UITemplate.html");
            StreamReader str = new StreamReader(templateFilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            var reportTemplate = ParseHTML(GetReportFilePaths(reportDirectory));
            MailText = PrePareMailBody(MailText, reportTemplate);
            MailMessage mailmsg;
            SmtpClient smtpClient = PrepareMail(mailProperties, MailText, suiteType, out mailmsg);
            smtpClient.Send(mailmsg);
        }

        private static ReportTemplate ParseHTML(Dictionary<ReporFileType,string> reportFilePaths)
        {
            HtmlDocument document = new HtmlDocument();
            ReportTemplate rpTemplate = new ReportTemplate();
            List<TCFields> listTcFields = new List<TCFields>();
            foreach (var reportType in reportFilePaths)
            {
                document.Load(reportType.Value);
                switch (reportType.Key)
                {
                    case ReporFileType.index:
                        HtmlNodeCollection categories = document.DocumentNode.SelectNodes("//ul[@class='test-list-item']/li");
                        for (int i = 0; i < categories.Count; i++)
                        {
                            TCFields tcFields = new TCFields();
                            tcFields.testCaseName = categories[i].SelectSingleNode(".//div[@class='test-detail']/p[@class='name']").InnerText;
                            tcFields.testCaseStatus = (categories[i].Attributes["status"].Value == "pass") ? TCStatus.Pass: TCStatus.Fail;
                            tcFields.startDateTime = categories[i].SelectSingleNode(".//div[@class='p-v-10 d-inline-block']/div[@class='info']/span[@class='badge badge-success']").InnerText;
                            tcFields.duration = categories[i].SelectSingleNode(".//div[@class='test-detail']/p[contains(@class, 'duration')]").InnerText;
                            listTcFields.Add(tcFields);
                        }
                        break;
                    case ReporFileType.dashboard:
                        rpTemplate.totalDuration = document.DocumentNode.SelectSingleNode("//div[@class='card']//p[text()='Duration']/parent::div/h6").InnerText;
                        break;
                    default:
                        throw new Exception("Unexpected Report File type is supplied. Please check the input.");
                }
            }
            rpTemplate.tcFields = listTcFields;
            rpTemplate.totalPassedTCs = rpTemplate.tcFields.Where(x => x.testCaseStatus == TCStatus.Pass).Count();
            rpTemplate.totalFailedTCs = rpTemplate.tcFields.Count()- rpTemplate.totalPassedTCs; 
            return rpTemplate;
        }

        private static string PrePareMailBody(string mailBody, ReportTemplate rpTemplate)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(mailBody);
            if (rpTemplate.tcFields.Count > 2)
            {
                HtmlNode root = document.DocumentNode.SelectSingleNode("//tbody");
                HtmlNode categoryNodeReport = document.DocumentNode.SelectSingleNode("(//tr[@class='node-category'])[1]");
                for (int i = 1; i <= rpTemplate.tcFields.Count - 1; i++)
                {
                    HtmlNode cloneNode = categoryNodeReport.Clone();
                    root.InsertAfter(cloneNode, categoryNodeReport);
                }
            }
            HtmlNode categoryNodeMail = document.DocumentNode.SelectSingleNode("(//tr[@class='node-category'])");
            for (int i = 0; i < categoryNodeMail.SelectNodes("//td[contains(@class,'sp-tcname')]").Count; i++)
            {
                categoryNodeMail.SelectNodes("//td[contains(@class,'sp-tcname')]")[i].InnerHtml = rpTemplate.tcFields[i].testCaseName;
                categoryNodeMail.SelectNodes("//td[contains(@class,'sp-tcstatus')]")[i].InnerHtml = rpTemplate.tcFields[i].testCaseStatus.ToString();
                categoryNodeMail.SelectNodes("//td[contains(@class,'sp-starttime')]")[i].InnerHtml = rpTemplate.tcFields[i].startDateTime;
                categoryNodeMail.SelectNodes("//td[contains(@class,'sp-duration')]")[i].InnerHtml = rpTemplate.tcFields[i].duration;
            }
            #region Replacing Total PlaceHolders
            HtmlNode NodeTotal = document.DocumentNode.SelectSingleNode("(//tr[@class='node-total'])");
            string tempStr = NodeTotal.SelectSingleNode("//td[contains(@class,'sp-passtotal')]").InnerHtml;
            NodeTotal.SelectSingleNode("//td[contains(@class,'sp-passtotal')]").InnerHtml= tempStr.Replace("~PASSTOTAL~", rpTemplate.totalPassedTCs.ToString());
            tempStr = NodeTotal.SelectSingleNode("//td[contains(@class,'sp-failtotal')]").InnerHtml;
            NodeTotal.SelectSingleNode("//td[contains(@class,'sp-failtotal')]").InnerHtml = tempStr.Replace("~FAILTOTAL~", rpTemplate.totalFailedTCs.ToString());
            tempStr = NodeTotal.SelectSingleNode("//td[contains(@class,'sp-totalduration')]").InnerHtml;
            NodeTotal.SelectSingleNode("//td[contains(@class,'sp-totalduration')]").InnerHtml = tempStr.Replace("~TOTALDURATION~", rpTemplate.totalDuration.ToString());
            #endregion
            return document.DocumentNode.InnerHtml;
        }
        private static SmtpClient PrepareMail(MailProperties mailProperties, string mailText, SuiteType suiteType, out MailMessage mailmsg)
        {
            string emailSender = mailProperties.smtpServerUsername;
            string emailSenderPassword = mailProperties.smtpServerPassword;
            string emailSenderHost = mailProperties.smtpServerAddress;
            MailMessage _mailmsg = new MailMessage();
            _mailmsg.IsBodyHtml = true;
            _mailmsg.From = new MailAddress(mailProperties.fromAddr);
            if (mailProperties.toAddr.Contains(";"))
            {
                foreach (var address in mailProperties.toAddr.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    _mailmsg.To.Add(address);
                }
            }
            else
                _mailmsg.To.Add(mailProperties.toAddr);
            if(!String.IsNullOrEmpty(mailProperties.ccAddr))
                if (mailProperties.ccAddr.Contains(";"))
                {
                    foreach (var address in mailProperties.ccAddr.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        _mailmsg.CC.Add(address);
                    }
                }
            _mailmsg.Subject = $"{suiteType.ToString()} Automation Execution Status Report";
            _mailmsg.Body = mailText;
            SmtpClient _smtp = new SmtpClient();
            _smtp.Host = emailSenderHost;
            _smtp.Port = mailProperties.smtpServerPortno;
            _smtp.EnableSsl = mailProperties.ssl;
            foreach (var attachment in attachmentFiles)
                _mailmsg.Attachments.Add(new Attachment(attachment));
            NetworkCredential _network = new NetworkCredential(emailSender, emailSenderPassword);
            _smtp.Credentials = _network;
            mailmsg = _mailmsg;
            return _smtp;
        }

        private static Dictionary<ReporFileType, string> GetReportFilePaths(string reportDirectory)
        {
            Dictionary<ReporFileType, string> reportFilePaths = new Dictionary<ReporFileType, string>();
            string[] reportFiles = new string[1];
            var indexFile = Path.Combine(reportDirectory, Constants.INDEXFILENAME);
            var dashboardFile = Path.Combine(reportDirectory, Constants.DASHBOARDFILENAME);
            reportFilePaths.Add(ReporFileType.index, indexFile);
            reportFilePaths.Add(ReporFileType.dashboard, dashboardFile);
            reportFiles[0] = indexFile;
            attachmentFiles = reportFiles;
            return reportFilePaths;
        }
    }
}
