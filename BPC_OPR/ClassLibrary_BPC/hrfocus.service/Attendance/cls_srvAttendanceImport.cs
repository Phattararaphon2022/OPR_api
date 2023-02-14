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
                DataTable dt = doReadExcel(filename);
                switch (type)
                {
                    #region YEAR
                    case "YEAR":
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
                    #endregion

                    #region PERIOD
                    case "PERIOD":
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTPeriod controller = new cls_ctMTPeriod();
                                cls_MTPeriod model = new cls_MTPeriod();

                                model.company_code = dr["company_code"].ToString();
                                model.period_id = dr["period_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["period_id"].ToString());
                                model.period_type = dr["period_type"].ToString();
                                model.emptype_code = dr["emptype_code"].ToString();
                                model.year_code = dr["year_code"].ToString();
                                model.period_no = dr["period_no"].ToString();
                                model.period_name_th = dr["period_name_th"].ToString();
                                model.period_name_en = dr["period_name_en"].ToString();
                                model.period_from = Convert.ToDateTime(dr["period_from"].ToString());
                                model.period_to = Convert.ToDateTime(dr["period_to"].ToString());
                                model.period_payment = Convert.ToDateTime(dr["period_payment"].ToString());
                                model.period_dayonperiod = dr["period_dayonperiod"].ToString().Equals("1") ? true : false;
                                model.modified_by = by;
                                model.flag = false;
                                string strID = controller.insert(model);
                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.period_id + " " + model.period_no);
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
                    #endregion

                    #region REASON
                    case "REASON":
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTReason controller = new cls_ctMTReason();
                                cls_MTReason model = new cls_MTReason();

                                model.reason_id = dr["reason_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["reason_id"].ToString());
                                model.company_code = dr["company_code"].ToString();
                                model.reason_code = dr["reason_code"].ToString();
                                model.reason_name_th = dr["reason_name_th"].ToString();
                                model.reason_name_en = dr["reason_name_en"].ToString();
                                model.reason_group = dr["reason_group"].ToString();
                                model.modified_by = by;
                                model.flag = false;
                                string strID = controller.insert(model);
                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.reason_id + " " + model.reason_group);
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

                    #endregion

                    #region LOCATION
                    case "LOCATION":
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTLocation controller = new cls_ctMTLocation();
                                cls_MTLocation model = new cls_MTLocation();

                                model.company_code = dr["company_code"].ToString();
                                model.location_id = dr["location_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["location_id"].ToString());
                                model.location_code = dr["location_code"].ToString();
                                model.location_name_th = dr["location_name_th"].ToString();
                                model.location_name_en = dr["location_name_en"].ToString();
                                model.location_detail = dr["location_detail"].ToString();
                                model.location_lat = dr["location_lat"].ToString();
                                model.location_long = dr["location_long"].ToString();
                                model.modified_by = by;
                                model.flag = false;
                                string strID = controller.insert(model);
                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.location_id + " " + model.location_code);
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
                    #endregion

                    case "REASONs":
                        break;

                }

            }
            catch (Exception ex)
            {
                strResult = ex.ToString();
            }
            if (!Error.Equals(""))
            {
                strResult = Error;
            }
            return strResult;
        }
    }
}
