using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC
{
    public class Config
    {


        static public string Database = Initial.getDBName();
        static public string Userid = Initial.getDBLoginID();
        static public string Server = Initial.getServerName();
        static public string Password = Initial.getDBPasword();

        static public string FormatDateSQL = "MM/dd/yyyy";

        static public string PathFileImport = Initial.getPathFile();
        //static public string PathFileExport = Initial.getPathFile() + "\\Export";
        static public string PathFileExport = Initial.getPathFile() + "\\OPR\\Export";

        //static public string PathFileExport = "F:\\Temp\\HR365\\Export";

        //static public string PathFileDownloads = Initial.getPathFile() + "\\Downloads";
        //static public string PathFileExports = "F:\\Temp\\HR365\\Export";

        
    }
}
