using ClassLibrary_BPC.hrfocus.controller;
using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Text;

namespace ClassLibrary_BPC.hrfocus.service
{
    public class cls_srvAttendanceImport
    {
        public string Error = "";
        public DataTable doReadExcel(string fileName)
        {
            DataTable dt = new DataTable();

            string filePath = Path.Combine(ClassLibrary_BPC.Config.PathFileImport + "\\Imports\\", fileName);
            string xlConnStr = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=Yes;';";
            var xlConn = new OleDbConnection(xlConnStr);

            try
            {

                var da = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", xlConn);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                Error = ex.ToString();
            }
            finally
            {
                xlConn.Close();
            }

            return dt;
        }

        public string doImportExcel(string type, string filename, string by)
        {
            string strResult = "";

            try
            {

                int success = 0;
                StringBuilder objStr = new StringBuilder();

                switch (type)
                {
                    case "YEAR":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTYear controller = new cls_ctMTYear();
                                cls_MTYear model = new cls_MTYear();

                                model.company_code = dr["company_code"].ToString();
                                model.year_id = dr["year_id"].ToString();
                                model.year_code = dr["year_code"].ToString();
                                model.year_name_th = dr["year_name_th"].ToString();
                                model.year_name_en = dr["year_name_en"].ToString();
                                model.year_fromdate = Convert.ToDateTime(dr["year_fromdate"].ToString());
                                model.year_todate = Convert.ToDateTime(dr["year_todate"].ToString());
                                model.year_group = dr["year_group"].ToString();
                                model.modified_by = by;
                                model.flag = false;
                                string strID = controller.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.year_code +" "+ model.year_group);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        strResult = Error;
                        break;

                    case "REASON":
                        break;

                }

            }
            catch (Exception ex)
            {
                strResult = ex.ToString();
            }

            return strResult;
        }
    }
}
