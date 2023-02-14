using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTPlanholiday
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTPlanholiday() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTPlanholiday> getData(string condition)
        {
            List<cls_MTPlanholiday> list_model = new List<cls_MTPlanholiday>();
            cls_MTPlanholiday model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PLANHOLIDAY_ID");
                obj_str.Append(", PLANHOLIDAY_CODE");
                obj_str.Append(", ISNULL(PLANHOLIDAY_NAME_TH, '') AS PLANHOLIDAY_NAME_TH");
                obj_str.Append(", ISNULL(PLANHOLIDAY_NAME_EN, '') AS PLANHOLIDAY_NAME_EN");
                obj_str.Append(", COMPANY_CODE");
                obj_str.Append(", YEAR_CODE");
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM ATT_MT_PLANHOLIDAY");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PLANHOLIDAY_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTPlanholiday();

                    model.planholiday_id = Convert.ToInt32(dr["PLANHOLIDAY_ID"]);
                    model.planholiday_code = dr["PLANHOLIDAY_CODE"].ToString();
                    model.planholiday_name_th = dr["PLANHOLIDAY_NAME_TH"].ToString();
                    model.planholiday_name_en = dr["PLANHOLIDAY_NAME_EN"].ToString();
                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.year_code = dr["YEAR_CODE"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(PLANHOLIDAY.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTPlanholiday> getDataByFillter(string com, string id, string code, string year)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!id.Equals(""))
                strCondition += " AND PLANHOLIDAY_ID='" + id + "'";

            if (!code.Equals(""))
                strCondition += " AND PLANHOLIDAY_CODE='" + code + "'";

            if (!year.Equals(""))
                strCondition += " AND YEAR_CODE='" + year + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string code )
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PLANHOLIDAY_ID");
                obj_str.Append(" FROM ATT_MT_PLANHOLIDAY");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PLANHOLIDAY_CODE='" + code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(PLANHOLIDAY.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(PLANHOLIDAY_ID) ");
                obj_str.Append(" FROM ATT_MT_PLANHOLIDAY");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(PLANHOLIDAY.getNextID)" + ex.ToString();
            }

            return intResult;
        }

        public bool delete(string id ,string com)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_MT_PLANHOLIDAY");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND PLANHOLIDAY_ID='" + id + "'");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(PLANHOLIDAY.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTPlanholiday model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.planholiday_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO ATT_MT_PLANHOLIDAY");
                obj_str.Append(" (");
                obj_str.Append("PLANHOLIDAY_ID ");
                obj_str.Append(", PLANHOLIDAY_CODE ");
                obj_str.Append(", PLANHOLIDAY_NAME_TH ");
                obj_str.Append(", PLANHOLIDAY_NAME_EN ");
                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", YEAR_CODE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append(" @PLANHOLIDAY_ID ");
                obj_str.Append(", @PLANHOLIDAY_CODE ");
                obj_str.Append(", @PLANHOLIDAY_NAME_TH ");
                obj_str.Append(", @PLANHOLIDAY_NAME_EN ");
                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @YEAR_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PLANHOLIDAY_ID", SqlDbType.Int); obj_cmd.Parameters["@PLANHOLIDAY_ID"].Value = id;
                obj_cmd.Parameters.Add("@PLANHOLIDAY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PLANHOLIDAY_CODE"].Value = model.planholiday_code;
                obj_cmd.Parameters.Add("@PLANHOLIDAY_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PLANHOLIDAY_NAME_TH"].Value = model.planholiday_name_th;
                obj_cmd.Parameters.Add("@PLANHOLIDAY_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PLANHOLIDAY_NAME_EN"].Value = model.planholiday_name_en;
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@YEAR_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@YEAR_CODE"].Value = model.year_code;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(PLANHOLIDAY.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_MTPlanholiday model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE ATT_MT_PLANHOLIDAY SET ");

                obj_str.Append(" PLANHOLIDAY_CODE=@PLANHOLIDAY_CODE ");
                obj_str.Append(", PLANHOLIDAY_NAME_TH=@PLANHOLIDAY_NAME_TH ");
                obj_str.Append(", PLANHOLIDAY_NAME_EN=@PLANHOLIDAY_NAME_EN ");
                obj_str.Append(", COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(", YEAR_CODE=@YEAR_CODE ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");


                obj_str.Append(" WHERE PLANHOLIDAY_ID=@PLANHOLIDAY_ID ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PLANHOLIDAY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PLANHOLIDAY_CODE"].Value = model.planholiday_code;
                obj_cmd.Parameters.Add("@PLANHOLIDAY_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PLANHOLIDAY_NAME_TH"].Value = model.planholiday_name_th;
                obj_cmd.Parameters.Add("@PLANHOLIDAY_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PLANHOLIDAY_NAME_EN"].Value = model.planholiday_name_en;
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@YEAR_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@YEAR_CODE"].Value = model.year_code;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@PLANHOLIDAY_ID", SqlDbType.Int); obj_cmd.Parameters["@PLANHOLIDAY_ID"].Value = model.planholiday_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.planholiday_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(PLANHOLIDAY.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
