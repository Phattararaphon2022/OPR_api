using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTStatus
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTStatus() { }

        public string getMessage() { return this.Message.Replace("EMP_MT_STATUS", "").Replace("cls_ctMTStatus", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTStatus> getData(string condition)
        {
            List<cls_MTStatus> list_model = new List<cls_MTStatus>();
            cls_MTStatus model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("STATUS_ID");
                obj_str.Append(", STATUS_CODE");
                obj_str.Append(", STATUS_NAME_TH");
                obj_str.Append(", STATUS_NAME_EN");
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_MT_STATUS");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY STATUS_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTStatus();

                    model.status_id = Convert.ToInt32(dr["STATUS_ID"]);
                    model.status_code = dr["STATUS_CODE"].ToString();
                    model.status_name_th = dr["STATUS_NAME_TH"].ToString();
                    model.status_name_en = dr["STATUS_NAME_EN"].ToString();
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "STT001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTStatus> getDataByFillter(string code)
        {
            string strCondition = "";

            if (!code.Equals(""))
                strCondition += " AND STATUS_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(STATUS_ID, 1) ");
                obj_str.Append(" FROM EMP_MT_STATUS");
                obj_str.Append(" ORDER BY STATUS_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "STT002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string id,string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT STATUS_CODE");
                obj_str.Append(" FROM EMP_MT_STATUS");
                obj_str.Append(" WHERE STATUS_ID='" + id + "'");
                obj_str.Append(" AND STATUS_CODE='" + code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "STT003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string code)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM EMP_MT_STATUS");
                obj_str.Append(" WHERE STATUS_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "STT004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTStatus model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.status_id.ToString(),model.status_code))
                {
                    if (this.update(model))
                        return model.status_id.ToString();
                    else
                        return "";
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_MT_STATUS");
                obj_str.Append(" (");
                obj_str.Append("STATUS_ID ");
                obj_str.Append(", STATUS_CODE ");
                obj_str.Append(", STATUS_NAME_TH ");
                obj_str.Append(", STATUS_NAME_EN ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@STATUS_ID ");
                obj_str.Append(", @STATUS_CODE ");
                obj_str.Append(", @STATUS_NAME_TH ");
                obj_str.Append(", @STATUS_NAME_EN ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.status_id = this.getNextID();

                obj_cmd.Parameters.Add("@STATUS_ID", SqlDbType.Int); obj_cmd.Parameters["@STATUS_ID"].Value = model.status_id;
                obj_cmd.Parameters.Add("@STATUS_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@STATUS_CODE"].Value = model.status_code;
                obj_cmd.Parameters.Add("@STATUS_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@STATUS_NAME_TH"].Value = model.status_name_th;
                obj_cmd.Parameters.Add("@STATUS_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@STATUS_NAME_EN"].Value = model.status_name_en;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                strResult = model.status_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "STT005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTStatus model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE EMP_MT_STATUS SET ");
                obj_str.Append(" STATUS_NAME_TH=@STATUS_NAME_TH ");
                obj_str.Append(", STATUS_NAME_EN=@STATUS_NAME_EN ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE STATUS_ID=@STATUS_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@STATUS_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@STATUS_NAME_TH"].Value = model.status_name_th;
                obj_cmd.Parameters.Add("@STATUS_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@STATUS_NAME_EN"].Value = model.status_name_en;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@STATUS_ID", SqlDbType.Int); obj_cmd.Parameters["@STATUS_ID"].Value = model.status_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "STT006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
