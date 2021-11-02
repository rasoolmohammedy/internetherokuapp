using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReportSender;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using static Utilities.Constants;

namespace TestExecutor
{
    public class Executor
    {
        private static ConfigLoader config;
        private static string currentArtifactDirectory;
        private static string artifactDirName = "Artifacts";
        private static string artifactroryRelativePath = $"{artifactDirName}\\" + DateTime.Now.ToString(Constants.CURRENTDATETIMEFORMAT);
        static void Main(string[] args)
        {
            #region Handle Parameters
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Number of supplied arguments is {args.Length}");
            Console.WriteLine($"Arguments details as follows:");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(String.Join(",", args));
            Console.ResetColor();
            #endregion
            #region Loading Configuration
            if (args.Length > 1)
            {
                Console.WriteLine("Unexpected number of arguments supplied. Program will exit now after pressing any key...");
                Console.ReadKey();
                throw new Exception("Unexpected number of arguments supplied. Only 1 argument is allowed to pass to load the configuration file.");
            }
            else if (args.Length == 1)
                config = LoadConfiguration(args[0]);
            else
                config = LoadConfiguration(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"Config\ExecutorConfiguration.json"));
            #endregion
            CreateArtifactDirectory();
            UpdateLog4NetConfigDirectory($@"{artifactroryRelativePath}\");
            UpdateReportsDirectory($@"{artifactroryRelativePath}\");
            ExecuteNunitConsoleRunner();
            Dictionary<SuiteType, string> attachmentResultFiles = FindResultsFile();
            #region Launch Generated Report File
            if (config.ResultsConfiguration.LaunchResultsAfterExecution)
            {
                foreach (var file in attachmentResultFiles)
                {
                    System.Diagnostics.Process.Start(Path.Combine(file.Value, Constants.INDEXFILENAME));
                }
            }
            #endregion
            #region Send Report in EMail
            if (config.ResultsConfiguration.SendResultsEmail)
            {
                MailProperties mailProperties =
                new MailProperties()
                {
                    smtpServerAddress = config.SMTPDetails.SmtpServerAddress,
                    smtpServerUsername = config.SMTPDetails.SmtpServerUsername,
                    smtpServerPassword = config.SMTPDetails.SmtpServerPassword,
                    smtpServerPortno = config.SMTPDetails.SmtpServerPortno,
                    ssl = config.SMTPDetails.Ssl,
                    fromAddr = config.SMTPDetails.FromAddr,
                    toAddr = config.SMTPDetails.ToAddr,
                    ccAddr = config.SMTPDetails.CcAddr
                };
                foreach (var file in attachmentResultFiles)
                {
                    switch (file.Key)
                    {
                        case SuiteType.UI:
                            Sender.SendEmail(attachmentResultFiles[SuiteType.UI], SuiteType.UI, mailProperties);
                            break;
                        case SuiteType.API:
                            Sender.SendEmail(attachmentResultFiles[SuiteType.API], SuiteType.API, mailProperties);
                            break;
                        default:
                            throw new Exception("Unxpected Suite Type received.");
                    }
                }
            }
            #endregion
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\nExecution completed. Press any key to exit...");
            Console.ResetColor();
            Console.ReadKey();
        }

        static ConfigLoader LoadConfiguration(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    ConfigLoader configLoader = JsonConvert.DeserializeObject<ConfigLoader>(File.ReadAllText(filePath));
                    return configLoader;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected exception occurred while trying to load the configuration from the file {filePath}. Press any key to exit the application. ...");
                    Console.ReadKey();
                    throw new Exception($"Unexpected exception occurred while trying to load the configuration {filePath}", ex);
                }
            }
            else
            {
                Console.WriteLine($"Provided configuration file path doesn't exist {filePath}. Press any key to exit the application. ...");
                Console.ReadKey();
                throw new Exception($"Provided config file doesn't exist. {filePath}");
            }
        }

        static void CreateArtifactDirectory()
        {
            currentArtifactDirectory = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, artifactroryRelativePath);
            if (!Directory.Exists(currentArtifactDirectory))
            {
                Directory.CreateDirectory(currentArtifactDirectory);
            }
        }

        static void UpdateLog4NetConfigDirectory(string appendLogPathString)
        {
            string log4netConfigFilePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, $"log4net.config");
            string[] lines;
            try
            {
                lines = File.ReadAllLines(log4netConfigFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected exception occurred while trying to read the log path on log4net.config file at {log4netConfigFilePath} Press any key to exit the application. ...");
                Console.ReadKey();
                throw new Exception($"Unexpected exception occurred while reading log4net.config file.", ex);
            }
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("<file"))
                {
                    var pattern = "value=\"";
                    lines[i] = $"<file type=\"log4net.Util.PatternString\" value=\"{appendLogPathString}Logs\\%date{{dd-MM-yyyy_hh-mm-ss}}_%random{3}.log\" />";
                }

                try
                {
                    File.WriteAllLines(log4netConfigFilePath, lines);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected exception occurred while trying to write the log path on log4net.config file at {log4netConfigFilePath} Press any key to exit the application. ...");
                    Console.ReadKey();
                    throw new Exception($"Unexpected exception occurred while updating log4net.config file.", ex);
                }
            }
        }
        static void UpdateReportsDirectory(string appendLogPathString)
        {
            string uiGlobalPropertiesFilePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, $"Config\\{Constants.PropFileConsts.UIGLOBALPROPERTIESFILENAME}");
            string[] lines;
            try
            {
                lines = File.ReadAllLines(uiGlobalPropertiesFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected exception occurred while trying to read the reports path on uiglobalproperties file at {uiGlobalPropertiesFilePath} Press any key to exit the application. ...");
                Console.ReadKey();
                throw new Exception($"Unexpected exception occurred while reading uiglobalproperties file.", ex);
            }
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("basePath"))
                {
                    lines[i] = $"basePath = {appendLogPathString}Reports";
                }
            }
            try
            {
                File.WriteAllLines(uiGlobalPropertiesFilePath, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected exception occurred while trying to write the report path on uiglobalproperties file at {uiGlobalPropertiesFilePath} Press any key to exit the application. ...");
                Console.ReadKey();
                throw new Exception($"Unexpected exception occurred while updating uiglobalproperties file.", ex);
            }

        }

        static void ExecuteNunitConsoleRunner()
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = false;
            p.StartInfo.RedirectStandardError = false;
            p.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, config.ExecutorSettings.NUnitConsoleRunnerExePath);
            p.StartInfo.Arguments = $@"--testlist={config.ExecutorSettings.TestCasesOrderConfigFilePath} --result={artifactroryRelativePath}\{config.ExecutorSettings.NUnitResultsXMLPath} --output={artifactroryRelativePath}\{config.ExecutorSettings.NUnitOutputPath} {config.ExecutorSettings.APISuiteDLLPath} {config.ExecutorSettings.UISuiteDLLPath}";
            p.Start();
            p.WaitForExit();
        }

        static Dictionary<SuiteType, string> FindResultsFile()
        {
            Dictionary<SuiteType, string> output = new Dictionary<SuiteType, string>();
            string[] files = Directory.GetFiles(currentArtifactDirectory, Constants.INDEXFILENAME,SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (file.Contains(SuiteType.API.ToString()))
                    output.Add(SuiteType.API, Path.GetDirectoryName(file));
                else
                    output.Add(SuiteType.UI, Path.GetDirectoryName(file));
            }
            return output;
        }
    }
}
