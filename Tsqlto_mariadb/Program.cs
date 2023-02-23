using GoogleSheetsHelper;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Tsqlto_mariadb
{
    static class Program
    {
        //Path        
        private static string LocalPath = new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
        private static string LogFilePath = new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).LocalPath + @"\LOG\" + "LOG_" + DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.GetCultureInfo("en-US")) + ".txt";

        //CultureInfo & Encoding
        private static CultureInfo CuInfo = CultureInfo.GetCultureInfo("en-US");
        private static Encoding CurrentEncoding = Encoding.GetEncoding("windows-874");

        //API Google Sheet
        private static string SpreadsheetId = ConfigurationManager.AppSettings["SpreadsheetId"].ToString();
        
        private static string CredentialFileName = LocalPath + "\\client_secret.json";
        private static string SheetName = ConfigurationManager.AppSettings["IsCustomSheetName"].ToString() == "true"
                                            ? ConfigurationManager.AppSettings["SheetName"].ToString()
                                            : $"{DateTime.Now.AddMonths(-1).Year+543}-{DateTime.Now.AddMonths(-1).Month}";
        //         : $"{DateTime.Now.Year + 543}-{DateTime.Now.Month - 1}";        เต้เปลี่ยน code month ใหม่
        //ถ้าเป็น true ให้ดึง SheetName ซึ่งฟิกค่าไว้เป็นเดือนสิบ ถ้าเป็น false ให้เอาค่าที่ sheet มา
        private static int RangeColumnStart = Convert.ToInt32(ConfigurationManager.AppSettings["RangeColumnStart"].ToString());
        private static int RangeColumnEnd = Convert.ToInt32(ConfigurationManager.AppSettings["RangeColumnEnd"].ToString());
        private static int RangeRowStart = Convert.ToInt32(ConfigurationManager.AppSettings["RangeRowStart"].ToString());
        private static int RangeRowEnd = Convert.ToInt32(ConfigurationManager.AppSettings["RangeRowEnd"].ToString());
        private static bool FirstRowIsHeaders = ConfigurationManager.AppSettings["FirstRowIsHeaders"].ToString() == "true" ? true : false;


        //[STAThread]
        static void Main()
        {
            WriteLog("[Start] : Tsqlto_mariadb");
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                DataTable data_dat = RunGoogleSheet();
                Application.Run(new aleartPloblem(data_dat));
            }
            catch (Exception ex)
            {
                WriteLog($"Error : {ex.Message}");
            }
            WriteLog("[End] : Tsqlto_mariadb");
            Console.ReadLine();
        }

        private static DataTable RunGoogleSheet()
        {
            WriteLog("Start : RunGoogleSheet");
            var gsh = new GoogleSheetsHelperApp(CredentialFileName, SpreadsheetId);
            var gshParam = new GoogleSheetParameters();
            gshParam.RangeColumnStart = RangeColumnStart;
            gshParam.RangeColumnEnd = RangeColumnEnd;
            gshParam.RangeRowStart = RangeRowStart;
            gshParam.RangeRowEnd = RangeRowEnd;
            gshParam.SheetName = SheetName;
            gshParam.FirstRowIsHeaders = FirstRowIsHeaders;

            DataTable data = gsh.GetDataFromSheet(gshParam);

            foreach (DataRow dataRow in data.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    WriteLog(item.ToString());
                }
            }

            return data;
        }

        private static void WriteLog(string Str)
        {
            try
            {
                var path = new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
                var TMPPath = path + @"\TMP\" + DateTime.Now.ToString("yyyyMMdd", CuInfo) + @"\\";
                if (!Directory.Exists(TMPPath))
                {
                    Directory.CreateDirectory(TMPPath);
                }

                string LogPath = LocalPath + @"\LOG\";
                if (!Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);
                }

                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CuInfo) + " : " + Str);

                using (StreamWriter sr = new StreamWriter(LogFilePath, true, CurrentEncoding))
                {
                    sr.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CuInfo) + " : " + Str);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
        }
    }
}
