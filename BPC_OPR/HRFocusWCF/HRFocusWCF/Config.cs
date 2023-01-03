using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFocusWCF
{
    public class Config
    {



        static public string Database = ConfigurationManager.AppSettings["DB"];
        static public string Userid = ConfigurationManager.AppSettings["User"];
        //static public string Server = ".\\SQLEXPRESS";
        //static public string Password = "Sql2019*";
        static public string Server = ConfigurationManager.AppSettings["Server"];
        static public string Password = ConfigurationManager.AppSettings["Pass"];

        static public string FormatDateSQL = "MM/dd/yyyy";

        static public string PathFileImport = "D:\\OPR";
        static public string PathFileExport = "D:\\OPR\\Export";
        
    }
}
