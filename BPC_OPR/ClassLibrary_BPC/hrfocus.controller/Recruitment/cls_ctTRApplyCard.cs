using ClassLibrary_BPC.hrfocus.model.Recruitment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRApplyCard
   {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRApplyCard() { }

        public string getMessage() { return this.Message.Replace("REQ_TR_CARD", "").Replace("cls_ctTRApplyCard", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRApplyCard> getData(string condition)
        {
            List<cls_TRApplyCard> list_model = new List<cls_TRApplyCard>();
            cls_TRApplyCard model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("CARD_ID");
                obj_str.Append(", CARD_CODE");
                obj_str.Append(", CARD_TYPE");
                obj_str.Append(", ISNULL(CARD_ISSUE, '') AS CARD_ISSUE");
                obj_str.Append(", ISNULL(CARD_EXPIRE, '') AS CARD_EXPIRE");

                obj_str.Append(", COMPANY_CODE");
                obj_str.Append(", APPLYWORK_CODE");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM REQ_TR_CARD");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY APPLYWORK_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRApplyCard();

                    model.card_id = Convert.ToInt32(dr["CARD_ID"]);
                    model.card_code = dr["CARD_CODE"].ToString();
                    model.card_type = dr["CARD_TYPE"].ToString();
                    model.card_issue = Convert.ToDateTime(dr["CARD_ISSUE"]);
                    model.card_expire = Convert.ToDateTime(dr["CARD_EXPIRE"]);

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.applywork_code = dr["APPLYWORK_CODE"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "REQCRD001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRApplyCard> getDataByFillter(string com, string emp)
        {
            string strCondition = "";

            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!emp.Equals(""))
                strCondition += " AND APPLYWORK_CODE='" + emp + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(CARD_ID, 1) ");
                obj_str.Append(" FROM REQ_TR_CARD");
                obj_str.Append(" ORDER BY CARD_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "REQCRD002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT CARD_ID");
                obj_str.Append(" FROM REQ_TR_CARD");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND APPLYWORK_CODE='" + emp + "' ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "REQCRD003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string emp)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM REQ_TR_CARD");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND APPLYWORK_CODE='" + emp + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "REQCRD004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRApplyCard model)
        {
            bool blnResult = false;
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.company_code, model.applywork_code))
                {
                    return this.update(model);

                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO REQ_TR_CARD");
                obj_str.Append(" (");
                obj_str.Append("CARD_ID ");
                obj_str.Append(", CARD_CODE ");
                obj_str.Append(", CARD_TYPE ");
                obj_str.Append(", CARD_ISSUE ");
                obj_str.Append(", CARD_EXPIRE ");

                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", APPLYWORK_CODE ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@CARD_ID ");
                obj_str.Append(", @CARD_CODE ");
                obj_str.Append(", @CARD_TYPE ");
                obj_str.Append(", @CARD_ISSUE ");
                obj_str.Append(", @CARD_EXPIRE ");

                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @APPLYWORK_CODE ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.card_id = this.getNextID();

                obj_cmd.Parameters.Add("@CARD_ID", SqlDbType.Int); obj_cmd.Parameters["@CARD_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@CARD_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@CARD_CODE"].Value = model.card_code;
                obj_cmd.Parameters.Add("@CARD_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@CARD_TYPE"].Value = model.card_type;
                obj_cmd.Parameters.Add("@CARD_ISSUE", SqlDbType.DateTime); obj_cmd.Parameters["@CARD_ISSUE"].Value = model.card_issue;
                obj_cmd.Parameters.Add("@CARD_EXPIRE", SqlDbType.DateTime); obj_cmd.Parameters["@CARD_EXPIRE"].Value = model.card_expire;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@APPLYWORK_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYWORK_CODE"].Value = model.applywork_code;


                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                strResult = model.card_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "REQCRD005:" + ex.ToString();
                strResult = "";
            }

            return blnResult;
        }

        public bool update(cls_TRApplyCard model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE REQ_TR_CARD SET ");

                obj_str.Append(" CARD_CODE=@CARD_CODE ");
                obj_str.Append(", CARD_ISSUE=@CARD_ISSUE ");
                obj_str.Append(", CARD_EXPIRE=@CARD_EXPIRE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND PAALYWORK_CODE=@PAALYWORK_CODE ");
                obj_str.Append(" AND CARD_ID=@CARD_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@CARD_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@CARD_CODE"].Value = model.card_code;

                obj_cmd.Parameters.Add("@CARD_ISSUE", SqlDbType.DateTime); obj_cmd.Parameters["@CARD_ISSUE"].Value = model.card_issue;
                obj_cmd.Parameters.Add("@CARD_EXPIRE", SqlDbType.DateTime); obj_cmd.Parameters["@CARD_EXPIRE"].Value = model.card_expire;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@PAALYWORK_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PAALYWORK_CODE"].Value = model.applywork_code;
                obj_cmd.Parameters.Add("@CARD_ID", SqlDbType.Int); obj_cmd.Parameters["@CARD_ID"].Value = model.card_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "REQCRD006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
