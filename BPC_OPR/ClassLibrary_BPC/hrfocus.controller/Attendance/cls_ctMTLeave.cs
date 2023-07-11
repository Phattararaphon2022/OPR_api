using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTLeave
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTLeave() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTLeave> getData(string condition)
        {
            List<cls_MTLeave> list_model = new List<cls_MTLeave>();
            cls_MTLeave model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", LEAVE_ID");
                obj_str.Append(", LEAVE_CODE");
                obj_str.Append(", ISNULL(LEAVE_NAME_TH, '') AS LEAVE_NAME_TH");
                obj_str.Append(", ISNULL(LEAVE_NAME_EN, '') AS LEAVE_NAME_EN");

                obj_str.Append(", ISNULL(LEAVE_DAY_PERYEAR, '0') AS LEAVE_DAY_PERYEAR");
                obj_str.Append(", ISNULL(LEAVE_DAY_ACC, '0') AS LEAVE_DAY_ACC");
                obj_str.Append(", ISNULL(LEAVE_DAY_ACCEXPIRE, '01/01/1900') AS LEAVE_DAY_ACCEXPIRE");

                obj_str.Append(", ISNULL(LEAVE_INCHOLIDAY, '0') AS LEAVE_INCHOLIDAY");
                obj_str.Append(", ISNULL(LEAVE_PASSPRO, '0') AS LEAVE_PASSPRO");
                obj_str.Append(", ISNULL(LEAVE_DEDUCT, '0') AS LEAVE_DEDUCT");
                obj_str.Append(", ISNULL(LEAVE_CALDILIGENCE, '0') AS LEAVE_CALDILIGENCE");

                obj_str.Append(", ISNULL(LEAVE_AGEWORK, '') AS LEAVE_AGEWORK");
                obj_str.Append(", ISNULL(LEAVE_AHEAD, '0') AS LEAVE_AHEAD");

                obj_str.Append(", ISNULL(LEAVE_MIN_HRS, '00:00') AS LEAVE_MIN_HRS");
                obj_str.Append(", ISNULL(LEAVE_MAX_DAY, '999') AS LEAVE_MAX_DAY");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM ATT_MT_LEAVE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY LEAVE_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTLeave();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                    model.leave_id = Convert.ToInt32(dr["LEAVE_ID"]);
                    model.leave_code = Convert.ToString(dr["LEAVE_CODE"]);
                    model.leave_name_th = Convert.ToString(dr["LEAVE_NAME_TH"]);
                    model.leave_name_en = Convert.ToString(dr["LEAVE_NAME_EN"]);
                    model.leave_day_peryear = Convert.ToDouble(dr["LEAVE_DAY_PERYEAR"]);
                    model.leave_day_acc = Convert.ToDouble(dr["LEAVE_DAY_ACC"]);
                    model.leave_day_accexpire = Convert.ToDateTime(dr["LEAVE_DAY_ACCEXPIRE"]);
                    model.leave_incholiday = Convert.ToString(dr["LEAVE_INCHOLIDAY"]);
                    model.leave_passpro = Convert.ToString(dr["LEAVE_PASSPRO"]);
                    model.leave_deduct = Convert.ToString(dr["LEAVE_DEDUCT"]);
                    model.leave_caldiligence = Convert.ToString(dr["LEAVE_CALDILIGENCE"]);
                    model.leave_agework = Convert.ToString(dr["LEAVE_AGEWORK"]);
                    model.leave_ahead = Convert.ToInt32(dr["LEAVE_AHEAD"]);
                    model.leave_min_hrs = Convert.ToString(dr["LEAVE_MIN_HRS"]);
                    model.leave_max_day = Convert.ToDouble(dr["LEAVE_MAX_DAY"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Leave.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTLeave> getDataByFillter(string com, string id, string code)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!id.Equals(""))
                strCondition += " AND LEAVE_ID='" + id + "'";

            if (!code.Equals(""))
                strCondition += " AND LEAVE_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT LEAVE_ID");
                obj_str.Append(" FROM ATT_MT_LEAVE");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND LEAVE_CODE='" + code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Leave.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(LEAVE_ID) ");
                obj_str.Append(" FROM ATT_MT_LEAVE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Leave.getNextID)" + ex.ToString();
            }

            return intResult;
        }

        public bool delete(string id, string com)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_MT_LEAVE");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND LEAVE_ID='" + id + "'");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Leave.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTLeave model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.leave_code))
                {
                    if (model.leave_id.Equals(0))
                    {
                        return "D";
                    }
                    else
                    {
                        return this.update(model);
                    }
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO ATT_MT_LEAVE");
                obj_str.Append(" (");
                obj_str.Append("LEAVE_ID ");
                obj_str.Append(", LEAVE_CODE ");
                obj_str.Append(", LEAVE_NAME_TH ");
                obj_str.Append(", LEAVE_NAME_EN ");

                obj_str.Append(", LEAVE_DAY_PERYEAR ");
                obj_str.Append(", LEAVE_DAY_ACC ");
                obj_str.Append(", LEAVE_DAY_ACCEXPIRE ");

                obj_str.Append(", LEAVE_INCHOLIDAY ");
                obj_str.Append(", LEAVE_PASSPRO ");
                obj_str.Append(", LEAVE_DEDUCT ");
                obj_str.Append(", LEAVE_CALDILIGENCE ");

                obj_str.Append(", LEAVE_AGEWORK ");
                obj_str.Append(", LEAVE_AHEAD ");

                obj_str.Append(", LEAVE_MIN_HRS ");
                obj_str.Append(", LEAVE_MAX_DAY ");

                obj_str.Append(", COMPANY_CODE ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@LEAVE_ID ");
                obj_str.Append(", @LEAVE_CODE ");
                obj_str.Append(", @LEAVE_NAME_TH ");
                obj_str.Append(", @LEAVE_NAME_EN ");

                obj_str.Append(", @LEAVE_DAY_PERYEAR ");
                obj_str.Append(", @LEAVE_DAY_ACC ");
                obj_str.Append(", @LEAVE_DAY_ACCEXPIRE ");

                obj_str.Append(", @LEAVE_INCHOLIDAY ");
                obj_str.Append(", @LEAVE_PASSPRO ");
                obj_str.Append(", @LEAVE_DEDUCT ");
                obj_str.Append(", @LEAVE_CALDILIGENCE ");

                obj_str.Append(", @LEAVE_AGEWORK ");
                obj_str.Append(", @LEAVE_AHEAD ");

                obj_str.Append(", @LEAVE_MIN_HRS ");
                obj_str.Append(", @LEAVE_MAX_DAY ");

                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@LEAVE_ID", SqlDbType.Int); obj_cmd.Parameters["@LEAVE_ID"].Value = id;
                obj_cmd.Parameters.Add("@LEAVE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@LEAVE_CODE"].Value = model.leave_code;
                obj_cmd.Parameters.Add("@LEAVE_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@LEAVE_NAME_TH"].Value = model.leave_name_th;
                obj_cmd.Parameters.Add("@LEAVE_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@LEAVE_NAME_EN"].Value = model.leave_name_en;

                obj_cmd.Parameters.Add("@LEAVE_DAY_PERYEAR", SqlDbType.Decimal); obj_cmd.Parameters["@LEAVE_DAY_PERYEAR"].Value = model.leave_day_peryear;
                obj_cmd.Parameters.Add("@LEAVE_DAY_ACC", SqlDbType.Decimal); obj_cmd.Parameters["@LEAVE_DAY_ACC"].Value = model.leave_day_acc;
                obj_cmd.Parameters.Add("@LEAVE_DAY_ACCEXPIRE", SqlDbType.DateTime); obj_cmd.Parameters["@LEAVE_DAY_ACCEXPIRE"].Value = model.leave_day_accexpire;

                obj_cmd.Parameters.Add("@LEAVE_INCHOLIDAY", SqlDbType.Char); obj_cmd.Parameters["@LEAVE_INCHOLIDAY"].Value = model.leave_incholiday;
                obj_cmd.Parameters.Add("@LEAVE_PASSPRO", SqlDbType.Char); obj_cmd.Parameters["@LEAVE_PASSPRO"].Value = model.leave_passpro;
                obj_cmd.Parameters.Add("@LEAVE_DEDUCT", SqlDbType.Char); obj_cmd.Parameters["@LEAVE_DEDUCT"].Value = model.leave_deduct;
                obj_cmd.Parameters.Add("@LEAVE_CALDILIGENCE", SqlDbType.Char); obj_cmd.Parameters["@LEAVE_CALDILIGENCE"].Value = model.leave_caldiligence;

                obj_cmd.Parameters.Add("@LEAVE_AGEWORK", SqlDbType.Char); obj_cmd.Parameters["@LEAVE_AGEWORK"].Value = model.leave_agework;
                obj_cmd.Parameters.Add("@LEAVE_AHEAD", SqlDbType.Int); obj_cmd.Parameters["@LEAVE_AHEAD"].Value = model.leave_ahead;

                obj_cmd.Parameters.Add("@LEAVE_MIN_HRS", SqlDbType.Char); obj_cmd.Parameters["@LEAVE_MIN_HRS"].Value = model.leave_min_hrs;
                obj_cmd.Parameters.Add("@LEAVE_MAX_DAY", SqlDbType.Int); obj_cmd.Parameters["@LEAVE_MAX_DAY"].Value = model.leave_max_day;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Leave.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_MTLeave model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE ATT_MT_LEAVE SET ");

                obj_str.Append(" LEAVE_CODE=@LEAVE_CODE ");
                obj_str.Append(", LEAVE_NAME_TH=@LEAVE_NAME_TH ");
                obj_str.Append(", LEAVE_NAME_EN=@LEAVE_NAME_EN ");

                obj_str.Append(", LEAVE_DAY_PERYEAR=@LEAVE_DAY_PERYEAR ");
                obj_str.Append(", LEAVE_DAY_ACC=@LEAVE_DAY_ACC ");
                obj_str.Append(", LEAVE_DAY_ACCEXPIRE=@LEAVE_DAY_ACCEXPIRE ");

                obj_str.Append(", LEAVE_INCHOLIDAY=@LEAVE_INCHOLIDAY ");
                obj_str.Append(", LEAVE_PASSPRO=@LEAVE_PASSPRO ");
                obj_str.Append(", LEAVE_DEDUCT=@LEAVE_DEDUCT ");
                obj_str.Append(", LEAVE_CALDILIGENCE=@LEAVE_CALDILIGENCE ");

                obj_str.Append(", LEAVE_AGEWORK=@LEAVE_AGEWORK ");
                obj_str.Append(", LEAVE_AHEAD=@LEAVE_AHEAD ");

                obj_str.Append(", LEAVE_MIN_HRS=@LEAVE_MIN_HRS ");
                obj_str.Append(", LEAVE_MAX_DAY=@LEAVE_MAX_DAY ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");
                obj_str.Append(" WHERE 1=1 ");
                if (!model.leave_id.Equals(0))
                {
                    obj_str.Append(" AND LEAVE_ID=@LEAVE_ID ");

                }
                else
                {
                    obj_str.Append(" AND COMPANY_CODE='" + model.company_code + "'");
                    obj_str.Append(" AND LEAVE_CODE='" + model.leave_code + "'");
                }


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@LEAVE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@LEAVE_CODE"].Value = model.leave_code;
                obj_cmd.Parameters.Add("@LEAVE_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@LEAVE_NAME_TH"].Value = model.leave_name_th;
                obj_cmd.Parameters.Add("@LEAVE_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@LEAVE_NAME_EN"].Value = model.leave_name_en;

                obj_cmd.Parameters.Add("@LEAVE_DAY_PERYEAR", SqlDbType.Decimal); obj_cmd.Parameters["@LEAVE_DAY_PERYEAR"].Value = model.leave_day_peryear;
                obj_cmd.Parameters.Add("@LEAVE_DAY_ACC", SqlDbType.Decimal); obj_cmd.Parameters["@LEAVE_DAY_ACC"].Value = model.leave_day_acc;
                obj_cmd.Parameters.Add("@LEAVE_DAY_ACCEXPIRE", SqlDbType.DateTime); obj_cmd.Parameters["@LEAVE_DAY_ACCEXPIRE"].Value = model.leave_day_accexpire;

                obj_cmd.Parameters.Add("@LEAVE_INCHOLIDAY", SqlDbType.Char); obj_cmd.Parameters["@LEAVE_INCHOLIDAY"].Value = model.leave_incholiday;
                obj_cmd.Parameters.Add("@LEAVE_PASSPRO", SqlDbType.Char); obj_cmd.Parameters["@LEAVE_PASSPRO"].Value = model.leave_passpro;
                obj_cmd.Parameters.Add("@LEAVE_DEDUCT", SqlDbType.Char); obj_cmd.Parameters["@LEAVE_DEDUCT"].Value = model.leave_deduct;
                obj_cmd.Parameters.Add("@LEAVE_CALDILIGENCE", SqlDbType.Char); obj_cmd.Parameters["@LEAVE_CALDILIGENCE"].Value = model.leave_caldiligence;

                obj_cmd.Parameters.Add("@LEAVE_AGEWORK", SqlDbType.Char); obj_cmd.Parameters["@LEAVE_AGEWORK"].Value = model.leave_agework;
                obj_cmd.Parameters.Add("@LEAVE_AHEAD", SqlDbType.Int); obj_cmd.Parameters["@LEAVE_AHEAD"].Value = model.leave_ahead;

                obj_cmd.Parameters.Add("@LEAVE_MIN_HRS", SqlDbType.Char); obj_cmd.Parameters["@LEAVE_MIN_HRS"].Value = model.leave_min_hrs;
                obj_cmd.Parameters.Add("@LEAVE_MAX_DAY", SqlDbType.Int); obj_cmd.Parameters["@LEAVE_MAX_DAY"].Value = model.leave_max_day;


                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@LEAVE_ID", SqlDbType.Int); obj_cmd.Parameters["@LEAVE_ID"].Value = model.leave_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.leave_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Leave.update)" + ex.ToString();
            }

            return blnResult;
        }
        public string totalleave(string com, string emp, string year)
        {
            string blnResult = "0";
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(SUM(TIMELEAVE_MIN),0) from ATT_TR_TIMELEAVE WHERE WORKER_CODE = '" + emp + "' AND COMPANY_CODE = '" + com + "' AND YEAR(TIMELEAVE_TODATE) ='" + year + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                blnResult = ex.ToString();
                Message = "ERROR::(Level.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
