using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTLate
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTLate() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTLate> getData(string condition)
        {
            List<cls_MTLate> list_model = new List<cls_MTLate>();
            cls_MTLate model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", LATE_ID");
                obj_str.Append(", LATE_CODE");
                obj_str.Append(", ISNULL(LATE_NAME_TH, '') AS LATE_NAME_TH");
                obj_str.Append(", ISNULL(LATE_NAME_EN, '') AS LATE_NAME_EN");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM ATT_MT_LATE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, LATE_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTLate();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                    model.late_id = Convert.ToInt32(dr["LATE_ID"]);
                    model.late_code = Convert.ToString(dr["LATE_CODE"]);
                    model.late_name_th = Convert.ToString(dr["LATE_NAME_TH"]);
                    model.late_name_en = Convert.ToString(dr["LATE_NAME_EN"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Late.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTLate> getDataByFillter(string com, string id, string code)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!id.Equals(""))
                strCondition += " AND LATE_ID='" + id + "'";

            if (!code.Equals(""))
                strCondition += " AND LATE_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT LATE_ID");
                obj_str.Append(" FROM ATT_MT_LATE");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND LATE_CODE='" + code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Late.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(LATE_ID) ");
                obj_str.Append(" FROM ATT_MT_LATE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Late.getNextID)" + ex.ToString();
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

                obj_str.Append(" DELETE FROM ATT_TR_LATE");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND LATE_CODE IN (SELECT LATE_CODE FROM ATT_MT_LATE WHERE LATE_ID='" + id + "')");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

                if (blnResult)
                {
                    obj_str.Append(" DELETE FROM ATT_MT_LATE");
                    obj_str.Append(" WHERE 1=1 ");
                    obj_str.Append(" AND LATE_ID='" + id + "'");

                    blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

                }

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Late.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTLate model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.late_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO ATT_MT_LATE");
                obj_str.Append(" (");
                obj_str.Append("LATE_ID ");
                obj_str.Append(", LATE_CODE ");
                obj_str.Append(", LATE_NAME_TH ");
                obj_str.Append(", LATE_NAME_EN ");

                obj_str.Append(", COMPANY_CODE ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@LATE_ID ");
                obj_str.Append(", @LATE_CODE ");
                obj_str.Append(", @LATE_NAME_TH ");
                obj_str.Append(", @LATE_NAME_EN ");
                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@LATE_ID", SqlDbType.Int); obj_cmd.Parameters["@LATE_ID"].Value = id;
                obj_cmd.Parameters.Add("@LATE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@LATE_CODE"].Value = model.late_code;
                obj_cmd.Parameters.Add("@LATE_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@LATE_NAME_TH"].Value = model.late_name_th;
                obj_cmd.Parameters.Add("@LATE_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@LATE_NAME_EN"].Value = model.late_name_en;

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
                Message = "ERROR::(Late.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_MTLate model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE ATT_MT_LATE SET ");

                obj_str.Append(" LATE_CODE=@LATE_CODE ");
                obj_str.Append(", LATE_NAME_TH=@LATE_NAME_TH ");
                obj_str.Append(", LATE_NAME_EN=@LATE_NAME_EN ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");


                obj_str.Append(" WHERE LATE_ID=@LATE_ID ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@LATE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@LATE_CODE"].Value = model.late_code;
                obj_cmd.Parameters.Add("@LATE_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@LATE_NAME_TH"].Value = model.late_name_th;
                obj_cmd.Parameters.Add("@LATE_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@LATE_NAME_EN"].Value = model.late_name_en;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.Parameters.Add("@LATE_ID", SqlDbType.Int); obj_cmd.Parameters["@LATE_ID"].Value = model.late_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.late_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Late.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
