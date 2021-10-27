using ExcelDataReader;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class ExcelDataManager
    {
        public static Dictionary<string, string> GetTestData(Constants.SuiteType suiteType, string testcaseName)
        {
            Dictionary<string, string> testData = new Dictionary<string, string>();
            DataSet ds = ExcelDataManager.ReadExcel(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, Constants.TESTDATAFILEPATH));
            try
            {
                DataTable dt = suiteType == Constants.SuiteType.UI ? ds.Tables[Constants.TestDataConsts.UI_TABLE_INDEX] : ds.Tables[Constants.TestDataConsts.API_TABLE_INDEX];
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[Constants.TestDataConsts.HEADER_TESTCASENAME].ToString() == testcaseName)
                        testData.Add(dr[Constants.TestDataConsts.HEADER_PROPERTYNAME].ToString(), dr[Constants.TestDataConsts.HEADER_PROPERTYVALUE].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpceted exception occurred while trying to get the test data for the test case ${testcaseName}", ex);
            }
            return testData;
        }

        private static DataSet ReadExcel(string filePath)
        {
            try
            {
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream))
                    {
                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        };
                        var result = reader.AsDataSet(conf);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected exception occurred while trying to read data from the test data Excel file ${filePath}", ex);
            }
        }



        public static void UpdateUsernameAndPasswordIntoTestData(string username, string password)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, Constants.TESTDATAFILEPATH));
            ExcelPackage p = new ExcelPackage(fileInfo);
            ExcelWorksheet myWorksheet = p.Workbook.Worksheets[Constants.TestDataConsts.WORKSHEET_UI_SUITE];
            myWorksheet.Cells[2, 4].Value = username;
            myWorksheet.Cells[3, 4].Value = password;
            p.Save();
        }
    }
}
