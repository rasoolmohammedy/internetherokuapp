using IniParser;
using IniParser.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Utilities
{
    public static class PropertiesReader
    {
        public static readonly ILog logger = LogManager.GetLogger(typeof(PropertiesReader));

        public static IniData ReadPropertiesFile(string iniFilePath)
        {
            logger.Info("=================Reading Configuration from Config file begins============");
            logger.Info($"Absolute File path of given properties file is {iniFilePath}");
            FileIniDataParser parser = new FileIniDataParser();
            parser.Parser.Configuration.CommentString = "#";
            logger.Debug($"Commented string for Config File is {parser.Parser.Configuration.CommentString}");
            IniData data;
            try
            {
                data = parser.ReadFile(iniFilePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"File path is invalid. Given configuration file path is {iniFilePath}", ex);
            }
            logger.Debug("---- Printing contents of the INI file ----\n");
            logger.Debug("\n" + data);
            logger.Debug("---- INI file Content ended ----\n");
            logger.Info("=================Reading Configuration from Config file ends============");
            return data;
        }
    }
}
