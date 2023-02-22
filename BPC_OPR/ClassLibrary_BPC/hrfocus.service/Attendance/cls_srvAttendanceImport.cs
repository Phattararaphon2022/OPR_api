using ClassLibrary_BPC.hrfocus.controller;
using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Text;
using System.Collections.Generic;

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
        public string checkshiftemty(string val)
        {
            string result = "00:00";
            if (val.Equals(""))
            {
                return result;
            }
            try {
                var resultcal = val.Split(' ')[1].Split(':');
                result = resultcal[0] + ":" + resultcal[1];
            }
            catch
            {
                result = val;
            }
            return result;
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
                        break;
                    #endregion

                    #region HOLIDAY
                    case "HOLIDAY":
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTPlanholiday controller = new cls_ctMTPlanholiday();
                                cls_MTPlanholiday model = new cls_MTPlanholiday();

                                model.company_code = dr["company_code"].ToString();
                                model.planholiday_id = dr["planholiday_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["planholiday_id"].ToString());
                                model.planholiday_code = dr["planholiday_code"].ToString();
                                model.planholiday_name_th = dr["planholiday_name_th"].ToString();
                                model.planholiday_name_en = dr["planholiday_name_en"].ToString();
                                model.year_code = dr["year_code"].ToString();
                                model.modified_by = by;
                                model.flag = false;
                                string strID = controller.insert(model);
                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.planholiday_id + " " + model.planholiday_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;
                    #endregion

                    #region SHIFT
                    case "SHIFT":
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTShift controller = new cls_ctMTShift();
                                cls_MTShift model = new cls_MTShift();

                                model.company_code = dr["company_code"].ToString();

                                model.shift_id = dr["shift_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["shift_id"].ToString());
                                model.shift_code = dr["shift_code"].ToString();
                                model.shift_name_th = dr["shift_name_th"].ToString();
                                model.shift_name_en = dr["shift_name_en"].ToString();
                                model.shift_ch1 = this.checkshiftemty(dr["shift_ch1"].ToString());
                                model.shift_ch2 = this.checkshiftemty(dr["shift_ch2"].ToString());
                                model.shift_ch3 = this.checkshiftemty(dr["shift_ch3"].ToString());
                                model.shift_ch4 = this.checkshiftemty(dr["shift_ch4"].ToString());
                                model.shift_ch5 = this.checkshiftemty(dr["shift_ch5"].ToString());
                                model.shift_ch6 = this.checkshiftemty(dr["shift_ch6"].ToString());
                                model.shift_ch7 = this.checkshiftemty(dr["shift_ch7"].ToString());
                                model.shift_ch8 = this.checkshiftemty(dr["shift_ch8"].ToString());
                                model.shift_ch9 = this.checkshiftemty(dr["shift_ch9"].ToString());
                                model.shift_ch10 =this.checkshiftemty(dr["shift_ch10"].ToString());

                                model.shift_ch3_from = this.checkshiftemty(dr["shift_ch3_from"].ToString());
                                model.shift_ch3_to = this.checkshiftemty(dr["shift_ch3_to"].ToString());
                                model.shift_ch4_from = this.checkshiftemty(dr["shift_ch4_from"].ToString());
                                model.shift_ch4_to = this.checkshiftemty(dr["shift_ch4_to"].ToString());

                                model.shift_ch7_from = this.checkshiftemty(dr["shift_ch7_from"].ToString());
                                model.shift_ch7_to = this.checkshiftemty(dr["shift_ch7_to"].ToString());
                                model.shift_ch8_from = this.checkshiftemty(dr["shift_ch8_from"].ToString());
                                model.shift_ch8_to = this.checkshiftemty(dr["shift_ch8_to"].ToString());

                                model.shift_otin_min = Convert.ToInt32(dr["shift_otin_min"].ToString().Equals("") ? "0" : dr["shift_otin_min"].ToString());
                                model.shift_otin_max = Convert.ToInt32(dr["shift_otin_max"].ToString().Equals("") ? "0" : dr["shift_otin_max"].ToString());
                                model.shift_otout_min = Convert.ToInt32(dr["shift_otout_min"].ToString().Equals("") ? "0" : dr["shift_otout_min"].ToString());
                                model.shift_otout_max = Convert.ToInt32(dr["shift_otout_max"].ToString().Equals("") ? "0" : dr["shift_otout_max"].ToString());

                                model.shift_flexiblebreak = dr["shift_flexiblebreak"].ToString().Equals("1") ? true : false;

                                model.modified_by = by;
                                model.flag = false;
                                string strID = controller.insert(model);
                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.shift_id + " " + model.shift_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;
                    #endregion

                    #region PLANSHIFT
                    case "PLANSHIFT":
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTPlanshift controller = new cls_ctMTPlanshift();
                                cls_MTPlanshift model = new cls_MTPlanshift();

                                model.company_code = dr["company_code"].ToString();
                                model.planshift_id = dr["planshift_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["planshift_id"].ToString());
                                model.planshift_code = dr["planshift_code"].ToString();
                                model.planshift_name_th = dr["planshift_name_th"].ToString();
                                model.planshift_name_en = dr["planshift_name_en"].ToString();
                                model.modified_by = by;
                                model.flag = false;
                                string strID = controller.insert(model);
                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.planshift_id + " " + model.planshift_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;
                    #endregion

                    #region LEAVE
                    case "LEAVE":
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTLeave controller = new cls_ctMTLeave();
                                cls_MTLeave model = new cls_MTLeave();

                                model.company_code = dr["company_code"].ToString();
                                model.leave_id = dr["leave_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["leave_id"].ToString());
                                model.leave_code = dr["leave_code"].ToString();
                                model.leave_name_th = dr["leave_name_th"].ToString();
                                model.leave_name_en = dr["leave_name_en"].ToString();
                                model.leave_day_peryear = dr["leave_day_peryear"].ToString().Equals("") ? 0 : Convert.ToDouble(dr["leave_day_peryear"].ToString());
                                model.leave_day_acc = dr["leave_day_acc"].ToString().Equals("") ? 0 : Convert.ToDouble(dr["leave_day_acc"].ToString());

                                string strExpire = "9999-12-31";
                                try
                                {
                                    if (dr["leave_day_accexpire"].ToString() != "")
                                        strExpire = dr["leave_day_accexpire"].ToString();
                                }
                                catch { }

                                model.leave_day_accexpire = Convert.ToDateTime(strExpire);
                                model.leave_incholiday = dr["leave_incholiday"].ToString().Equals("") ? "N" : dr["leave_incholiday"].ToString();
                                model.leave_passpro = dr["leave_passpro"].ToString().Equals("") ? "N" : dr["leave_passpro"].ToString();
                                model.leave_deduct = dr["leave_deduct"].ToString().Equals("") ? "N" : dr["leave_deduct"].ToString();
                                model.leave_caldiligence = dr["leave_caldiligence"].ToString().Equals("") ? "N" : dr["leave_caldiligence"].ToString();
                                model.leave_agework = dr["leave_agework"].ToString().Equals("") ? "N" : dr["leave_agework"].ToString();
                                model.leave_ahead = dr["leave_ahead"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["leave_ahead"].ToString());
                                model.leave_min_hrs = this.checkshiftemty(dr["leave_min_hrs"].ToString());
                                model.leave_max_day = dr["leave_max_day"].ToString().Equals("") ? 0 : Convert.ToDouble(dr["leave_max_day"].ToString());
                                model.modified_by = by;
                                model.flag = false;
                                string strID = controller.insert(model);
                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.leave_id + " " + model.leave_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;
                    #endregion

                    #region PLANLEAVE
                    case "PLANLEAVE":
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTPlanleave controller = new cls_ctMTPlanleave();
                                cls_MTPlanleave model = new cls_MTPlanleave();

                                model.company_code = dr["company_code"].ToString();
                                model.planleave_id = dr["planleave_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["planleave_id"].ToString());
                                model.planleave_code = dr["planleave_code"].ToString();
                                model.planleave_name_th = dr["planleave_name_th"].ToString();
                                model.planleave_name_en = dr["planleave_name_en"].ToString();
                                model.modified_by = by;
                                model.flag = false;
                                string strID = controller.insert(model);
                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.planleave_id + " " + model.planleave_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
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
