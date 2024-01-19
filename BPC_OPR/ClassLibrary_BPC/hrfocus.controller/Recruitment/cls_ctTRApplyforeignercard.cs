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
    public class cls_ctTRApplyforeignercard
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRApplyforeignercard() { }

        public string getMessage() { return this.Message.Replace("REQ_TR_FOREIGNERCARD", "").Replace("cls_ctTRApplyforeignercard", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }
        private List<cls_TRForeignercard> getData(string condition)
        {
            List<cls_TRForeignercard> list_model = new List<cls_TRForeignercard>();
            cls_TRForeignercard model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("FOREIGNERCARD_ID");
                obj_str.Append(", FOREIGNERCARD_CODE");
                obj_str.Append(", FOREIGNERCARD_TYPE");
                obj_str.Append(", FOREIGNERCARD_ISSUE");
                obj_str.Append(", FOREIGNERCARD_EXPIRE");

                obj_str.Append(", COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM REQ_TR_FOREIGNERCARD");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY FOREIGNERCARD_ID");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRForeignercard();

                    model.foreignercard_id = Convert.ToInt32(dr["FOREIGNERCARD_ID"]);
                    model.foreignercard_code = dr["FOREIGNERCARD_CODE"].ToString();
                    model.foreignercard_type = dr["FOREIGNERCARD_TYPE"].ToString();
                    model.foreignercard_issue = Convert.ToDateTime(dr["FOREIGNERCARD_ISSUE"]).ToString("yyyy/MM/dd");
                    model.foreignercard_expire = Convert.ToDateTime(dr["FOREIGNERCARD_EXPIRE"]).ToString("yyyy/MM/dd");

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "REQFOR001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRForeignercard> getDataByFillter(string com, string emp, string type)
        {
            string strCondition = "";

            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!emp.Equals(""))
                strCondition += " AND WORKER_CODE='" + emp + "'";

            if (!type.Equals(""))
                strCondition += " AND FOREIGNERCARD_TYPE='" + type + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(FOREIGNERCARD_ID, 1) ");
                obj_str.Append(" FROM REQ_TR_FOREIGNERCARD");
                obj_str.Append(" ORDER BY FOREIGNERCARD_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "REQFOR002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp, string id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT FOREIGNERCARD_ID");
                obj_str.Append(" FROM REQ_TR_FOREIGNERCARD");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                if (!id.ToString().Equals(""))
                {
                    obj_str.Append(" AND FOREIGNERCARD_ID='" + id + "' ");
                }

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "REQFOR003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string emp, string id)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM REQ_TR_FOREIGNERCARD");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                if (!id.ToString().Equals(""))
                {
                    obj_str.Append(" AND FOREIGNERCARD_ID='" + id + "' ");
                }

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "REQFOR004:" + ex.ToString();
            }

            return blnResult;
        }
        public bool clear(string com, string code)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM REQ_TR_FOREIGNERCARD");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND WORKER_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Foreignercard.clear)" + ex.ToString();
            }

            return blnResult;
        }
        public bool insert(cls_TRForeignercard model)
        {
            bool blnResult = false;
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.company_code, model.worker_code, model.foreignercard_id.ToString()))
                {
                    return this.update(model);

                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO REQ_TR_FOREIGNERCARD");
                obj_str.Append(" (");
                obj_str.Append("FOREIGNERCARD_ID ");
                obj_str.Append(", FOREIGNERCARD_CODE ");
                obj_str.Append(", FOREIGNERCARD_TYPE ");
                obj_str.Append(", FOREIGNERCARD_ISSUE ");
                obj_str.Append(", FOREIGNERCARD_EXPIRE ");

                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@FOREIGNERCARD_ID ");
                obj_str.Append(", @FOREIGNERCARD_CODE ");
                obj_str.Append(", @FOREIGNERCARD_TYPE ");
                obj_str.Append(", @FOREIGNERCARD_ISSUE ");
                obj_str.Append(", @FOREIGNERCARD_EXPIRE ");

                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                //model.card_id = this.getNextID();

                obj_cmd.Parameters.Add("@FOREIGNERCARD_ID", SqlDbType.Int); obj_cmd.Parameters["@FOREIGNERCARD_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@FOREIGNERCARD_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@FOREIGNERCARD_CODE"].Value = model.foreignercard_code;
                obj_cmd.Parameters.Add("@FOREIGNERCARD_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@FOREIGNERCARD_TYPE"].Value = model.foreignercard_type;
                obj_cmd.Parameters.Add("@FOREIGNERCARD_ISSUE", SqlDbType.DateTime); obj_cmd.Parameters["@FOREIGNERCARD_ISSUE"].Value = model.foreignercard_issue;
                obj_cmd.Parameters.Add("@FOREIGNERCARD_EXPIRE", SqlDbType.DateTime); obj_cmd.Parameters["@FOREIGNERCARD_EXPIRE"].Value = model.foreignercard_expire;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;


                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                strResult = model.foreignercard_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "REQFOR005:" + ex.ToString();
                strResult = "";
            }

            return blnResult;
        }

        public bool update(cls_TRForeignercard model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE REQ_TR_FOREIGNERCARD SET ");

                obj_str.Append(" FOREIGNERCARD_CODE=@FOREIGNERCARD_CODE ");
                obj_str.Append(", FOREIGNERCARD_ISSUE=@FOREIGNERCARD_ISSUE ");
                obj_str.Append(", FOREIGNERCARD_EXPIRE=@FOREIGNERCARD_EXPIRE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" AND FOREIGNERCARD_ID=@FOREIGNERCARD_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@FOREIGNERCARD_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@FOREIGNERCARD_CODE"].Value = model.foreignercard_code;
                obj_cmd.Parameters.Add("@FOREIGNERCARD_ISSUE", SqlDbType.DateTime); obj_cmd.Parameters["@FOREIGNERCARD_ISSUE"].Value = model.foreignercard_issue;
                obj_cmd.Parameters.Add("@FOREIGNERCARD_EXPIRE", SqlDbType.DateTime); obj_cmd.Parameters["@FOREIGNERCARD_EXPIRE"].Value = model.foreignercard_expire;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@CARD_ID", SqlDbType.Int); obj_cmd.Parameters["@CARD_ID"].Value = model.foreignercard_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "REQFOR006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
