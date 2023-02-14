using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTHoliday
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTHoliday() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTHoliday> getData(string condition)
        {
            List<cls_MTHoliday> list_model = new List<cls_MTHoliday>();
            cls_MTHoliday model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("HOLIDAY_ID");
                obj_str.Append(", HOLIDAY_DATE");
                obj_str.Append(", ISNULL(HOLIDAY_NAME_TH, '') AS HOLIDAY_NAME_TH");
                obj_str.Append(", ISNULL(HOLIDAY_NAME_EN, '') AS HOLIDAY_NAME_EN");

                obj_str.Append(", YEAR_CODE");
                obj_str.Append(", COMPANY_CODE");

                obj_str.Append(", ISNULL(HOLIDAY_DAYTYPE, 'H') AS HOLIDAY_DAYTYPE");
                obj_str.Append(", ISNULL(HOLIDAY_PAYPER, 100) AS HOLIDAY_PAYPER");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM ATT_MT_HOLIDAY");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY YEAR_CODE, HOLIDAY_DATE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTHoliday();

                    model.holiday_id = Convert.ToInt32(dr["HOLIDAY_ID"]);
                    model.holiday_date = Convert.ToDateTime(dr["HOLIDAY_DATE"]);
                    model.holiday_name_th = dr["HOLIDAY_NAME_TH"].ToString();
                    model.holiday_name_en = dr["HOLIDAY_NAME_EN"].ToString();

                    model.year_code = dr["YEAR_CODE"].ToString();
                    model.company_code = dr["COMPANY_CODE"].ToString();

                    model.holiday_daytype = dr["HOLIDAY_DAYTYPE"].ToString();
                    model.holiday_payper = Convert.ToDouble(dr["HOLIDAY_PAYPER"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Holiday.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTHoliday> getDataByFillter(string com, string year, string id)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!year.Equals(""))
                strCondition += " AND YEAR_CODE='" + year + "'";

            if (!id.Equals(""))
                strCondition += " AND HOLIDAY_ID='" + id + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string year, DateTime date)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT HOLIDAY_ID");
                obj_str.Append(" FROM ATT_MT_HOLIDAY");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                obj_str.Append(" AND YEAR_CODE='" + year + "'");
                obj_str.Append(" AND HOLIDAY_DATE='" + date.ToString("MM/dd/yyyy") + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Holiday.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(HOLIDAY_ID) ");
                obj_str.Append(" FROM ATT_MT_HOLIDAY");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Holiday.getNextID)" + ex.ToString();
            }

            return intResult;
        }

        public bool delete(string id)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_MT_HOLIDAY");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND HOLIDAY_ID='" + id + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Holiday.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_MTHoliday model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.year_code, model.holiday_date))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO ATT_MT_HOLIDAY");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", HOLIDAY_ID ");
                obj_str.Append(", HOLIDAY_DATE ");
                obj_str.Append(", HOLIDAY_NAME_TH ");
                obj_str.Append(", HOLIDAY_NAME_EN ");
                obj_str.Append(", YEAR_CODE ");

                obj_str.Append(", HOLIDAY_DAYTYPE ");
                obj_str.Append(", HOLIDAY_PAYPER ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @HOLIDAY_ID ");
                obj_str.Append(", @HOLIDAY_DATE ");
                obj_str.Append(", @HOLIDAY_NAME_TH ");
                obj_str.Append(", @HOLIDAY_NAME_EN ");
                obj_str.Append(", @YEAR_CODE ");

                obj_str.Append(", @HOLIDAY_DAYTYPE ");
                obj_str.Append(", @HOLIDAY_PAYPER ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@HOLIDAY_ID", SqlDbType.Int); obj_cmd.Parameters["@HOLIDAY_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@HOLIDAY_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@HOLIDAY_DATE"].Value = model.holiday_date;
                obj_cmd.Parameters.Add("@HOLIDAY_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@HOLIDAY_NAME_TH"].Value = model.holiday_name_th;
                obj_cmd.Parameters.Add("@HOLIDAY_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@HOLIDAY_NAME_EN"].Value = model.holiday_name_en;
                obj_cmd.Parameters.Add("@YEAR_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@YEAR_CODE"].Value = model.year_code;

                obj_cmd.Parameters.Add("@HOLIDAY_DAYTYPE", SqlDbType.VarChar); obj_cmd.Parameters["@HOLIDAY_DAYTYPE"].Value = model.holiday_daytype;
                obj_cmd.Parameters.Add("@HOLIDAY_PAYPER", SqlDbType.Decimal); obj_cmd.Parameters["@HOLIDAY_PAYPER"].Value = model.holiday_payper;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Holiday.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public bool update(cls_MTHoliday model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE ATT_MT_HOLIDAY SET ");
                obj_str.Append(" HOLIDAY_DATE=@HOLIDAY_DATE ");
                obj_str.Append(", HOLIDAY_NAME_TH=@HOLIDAY_NAME_TH ");
                obj_str.Append(", HOLIDAY_NAME_EN=@HOLIDAY_NAME_EN ");
                obj_str.Append(", YEAR_CODE=@YEAR_CODE ");

                obj_str.Append(", HOLIDAY_DAYTYPE=@HOLIDAY_DAYTYPE ");
                obj_str.Append(", HOLIDAY_PAYPER=@HOLIDAY_PAYPER ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE HOLIDAY_ID=@HOLIDAY_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@HOLIDAY_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@HOLIDAY_DATE"].Value = model.holiday_date;
                obj_cmd.Parameters.Add("@HOLIDAY_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@HOLIDAY_NAME_TH"].Value = model.holiday_name_th;
                obj_cmd.Parameters.Add("@HOLIDAY_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@HOLIDAY_NAME_EN"].Value = model.holiday_name_en;
                obj_cmd.Parameters.Add("@YEAR_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@YEAR_CODE"].Value = model.year_code;

                obj_cmd.Parameters.Add("@HOLIDAY_DAYTYPE", SqlDbType.VarChar); obj_cmd.Parameters["@HOLIDAY_DAYTYPE"].Value = model.holiday_daytype;
                obj_cmd.Parameters.Add("@HOLIDAY_PAYPER", SqlDbType.Decimal); obj_cmd.Parameters["@HOLIDAY_PAYPER"].Value = model.holiday_payper;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;


                obj_cmd.Parameters.Add("@HOLIDAY_ID", SqlDbType.Int); obj_cmd.Parameters["@HOLIDAY_ID"].Value = model.holiday_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Holiday.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
