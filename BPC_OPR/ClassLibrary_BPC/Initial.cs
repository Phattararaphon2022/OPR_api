using System;
using System.Data;
using System.Data.Odbc;
using System.Xml;

namespace ClassLibrary_BPC
{	
	public class Initial
	{
        private static string PathFile;
        private static string PathXMLFile = AppDomain.CurrentDomain.BaseDirectory + "Initial.xml";

        private static string ServerName;
        private static string DBName;
        private static string DBLoginID;
        private static string DBPassword;
        private static string Language;
        private static string CompanyID;

		public static string configurationFilePath
		{
			set 
			{
                PathFile = value;
                PathXMLFile = PathFile + "\\Initial.xml";
			}
			get
			{
                return PathXMLFile;
			}
		}

        public static string getServerName()
        {
            

            OdbcDataAdapter ad = new OdbcDataAdapter();
            DataSet dsODBCName = new DataSet();
            dsODBCName.ReadXml(PathXMLFile);
            DataTable dtODBCName = dsODBCName.Tables[0];
            DataRow drODBCName;
            if (dtODBCName.Rows.Count > 0)
            {
                drODBCName = dtODBCName.Rows[0];
                ServerName = drODBCName["ServerName"].ToString().Trim();
            }
            return ServerName;
        }
		
		public static string getDBName()
		{
      

			OdbcDataAdapter ad = new OdbcDataAdapter();
			DataSet dsODBCName = new DataSet();
            dsODBCName.ReadXml(PathXMLFile);
			DataTable dtODBCName = dsODBCName.Tables[0];
			DataRow drODBCName;
			if (dtODBCName.Rows.Count > 0)
			{
				drODBCName = dtODBCName.Rows[0];
                DBName = drODBCName["DBName"].ToString().Trim();
			}
            return DBName;
		}

		public static string getDBLoginID()
		{
          

			OdbcDataAdapter ad = new OdbcDataAdapter();
			DataSet dsODBCLoginID = new DataSet();
            dsODBCLoginID.ReadXml(PathXMLFile);
			DataTable dtODBCLoginID = dsODBCLoginID.Tables[0];
			DataRow drODBCLoginID;
			if(dtODBCLoginID.Rows.Count > 0)
			{
				drODBCLoginID = dtODBCLoginID.Rows[0];
                DBLoginID = drODBCLoginID["DBLoginID"].ToString().Trim();
			}
            return DBLoginID;
		}

        public static string getDBPasword()
		{
          

			OdbcDataAdapter ad = new OdbcDataAdapter();
			DataSet dsODBCPasword = new DataSet();
            dsODBCPasword.ReadXml(PathXMLFile);
			DataTable dtODBCPasword = dsODBCPasword.Tables[0];
			DataRow drODBCPasword;
			if(dtODBCPasword.Rows.Count > 0)
			{
				drODBCPasword = dtODBCPasword.Rows[0];
                DBPassword = drODBCPasword["DBPassword"].ToString().Trim();
			}
            return DBPassword;
		}

        public static string getLanguage()
        {
            OdbcDataAdapter ad = new OdbcDataAdapter();
            DataSet dsODBCPasword = new DataSet();
            dsODBCPasword.ReadXml(PathXMLFile);
            DataTable dtODBCPasword = dsODBCPasword.Tables[0];
            DataRow drODBCPasword;
            if (dtODBCPasword.Rows.Count > 0)
            {
                drODBCPasword = dtODBCPasword.Rows[0];
                Language = drODBCPasword["Language"].ToString().Trim();
            }
            return Language;
        }

        public static string getCompanyID()
        {
            OdbcDataAdapter ad = new OdbcDataAdapter();
            DataSet dsODBCPasword = new DataSet();
            dsODBCPasword.ReadXml(PathXMLFile);
            DataTable dtODBCPasword = dsODBCPasword.Tables[0];
            DataRow drODBCPasword;
            if (dtODBCPasword.Rows.Count > 0)
            {
                drODBCPasword = dtODBCPasword.Rows[0];
                CompanyID = drODBCPasword["CompanyID"].ToString().Trim();
            }
            return CompanyID;
        }
        
        public static void setCompanyID(string str)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(PathXMLFile);
            XmlNode xNode;
            XmlElement xElem;
            xNode = xDoc.SelectSingleNode("//CompanyID");
            if (!(xNode == null))
            {
                xElem = (XmlElement)(xNode);
                xElem.InnerText = str;
                xDoc.Save(PathXMLFile);
            }
        }


        public static string getPathFile()
        {


            OdbcDataAdapter ad = new OdbcDataAdapter();
            DataSet dsODBCPasword = new DataSet();
            dsODBCPasword.ReadXml(PathXMLFile);
            DataTable dtODBCPasword = dsODBCPasword.Tables[0];
            DataRow drODBCPasword;
            if (dtODBCPasword.Rows.Count > 0)
            {
                drODBCPasword = dtODBCPasword.Rows[0];
                DBPassword = drODBCPasword["PathFile"].ToString().Trim();
            }
            return DBPassword;
        }
		

	}
}
