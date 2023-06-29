using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTReason 
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTReason() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTReason> getData(string condition)
        {
            List<cls_MTReason> list_model = new List<cls_MTReason>();
            cls_MTReason model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("REASON_ID");
                obj_str.Append(", REASON_CODE");
                obj_str.Append(", ISNULL(REASON_NAME_TH, '') AS REASON_NAME_TH");
                obj_str.Append(", ISNULL(REASON_NAME_EN, '') AS REASON_NAME_EN");

                obj_str.Append(", REASON_GROUP");
                obj_str.Append(", COMPANY_CODE");
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM SYS_MT_REASON");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY REASON_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTReason();
                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.reason_id = Convert.ToInt32(dr["REASON_ID"]);
                    model.reason_code = dr["REASON_CODE"].ToString();
                    model.reason_name_th = dr["REASON_NAME_TH"].ToString();
                    model.reason_name_en = dr["REASON_NAME_EN"].ToString();

                    model.reason_group = dr["REASON_GROUP"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Reason.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTReason> getDataByFillter(string group, string id, string code, string com)
        {
            string strCondition = "";

            if (!group.Equals(""))
                strCondition += " AND REASON_GROUP='" + group + "'";

            if (!id.Equals(""))
                strCondition += " AND REASON_ID='" + id + "'";

            if (!code.Equals(""))
                strCondition += " AND REASON_CODE='" + code + "'";
            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string group, string code, string com, string id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT REASON_ID");
                obj_str.Append(" FROM SYS_MT_REASON");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND REASON_GROUP='" + group + "'");
                obj_str.Append(" AND REASON_CODE='" + code + "'");
                obj_str.Append(" AND REASON_ID='" + id + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Reason.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(REASON_ID) ");
                obj_str.Append(" FROM SYS_MT_REASON");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Reason.getNextID)" + ex.ToString();
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

                obj_str.Append(" DELETE FROM SYS_MT_REASON");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND REASON_ID='" + id + "'");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Reason.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTReason model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.reason_group, model.reason_code, model.company_code, model.reason_id.ToString()))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO SYS_MT_REASON");
                obj_str.Append(" (");
                obj_str.Append("REASON_ID ");
                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", REASON_CODE ");
                obj_str.Append(", REASON_NAME_TH ");
                obj_str.Append(", REASON_NAME_EN ");
                obj_str.Append(", REASON_GROUP ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append(" @REASON_ID ");
                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @REASON_CODE ");
                obj_str.Append(", @REASON_NAME_TH ");
                obj_str.Append(", @REASON_NAME_EN ");
                obj_str.Append(", @REASON_GROUP ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@REASON_ID", SqlDbType.Int); obj_cmd.Parameters["@REASON_ID"].Value = id;
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@REASON_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@REASON_CODE"].Value = model.reason_code;
                obj_cmd.Parameters.Add("@REASON_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@REASON_NAME_TH"].Value = model.reason_name_th;
                obj_cmd.Parameters.Add("@REASON_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@REASON_NAME_EN"].Value = model.reason_name_en;
                obj_cmd.Parameters.Add("@REASON_GROUP", SqlDbType.VarChar); obj_cmd.Parameters["@REASON_GROUP"].Value = model.reason_group;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Round.insert)" + ex.ToString();
                blnResult = "ERROR::(Round.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_MTReason model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SYS_MT_REASON SET ");

                obj_str.Append(" COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(", REASON_CODE=@REASON_CODE ");
                obj_str.Append(", REASON_NAME_TH=@REASON_NAME_TH ");
                obj_str.Append(", REASON_NAME_EN=@REASON_NAME_EN ");
                obj_str.Append(", REASON_GROUP=@REASON_GROUP ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");

                obj_str.Append(" WHERE REASON_ID=@REASON_ID ");
                obj_str.Append(" AND COMPANY_CODE=@COMPANY_CODE ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@REASON_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@REASON_CODE"].Value = model.reason_code;
                obj_cmd.Parameters.Add("@REASON_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@REASON_NAME_TH"].Value = model.reason_name_th;
                obj_cmd.Parameters.Add("@REASON_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@REASON_NAME_EN"].Value = model.reason_name_en;
                obj_cmd.Parameters.Add("@REASON_GROUP", SqlDbType.VarChar); obj_cmd.Parameters["@REASON_GROUP"].Value = model.reason_group;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@REASON_ID", SqlDbType.Int); obj_cmd.Parameters["@REASON_ID"].Value = model.reason_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.reason_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Reason.update)" + ex.ToString();
                blnResult = "ERROR::(Reason.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}