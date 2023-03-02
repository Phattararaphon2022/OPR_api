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
    public class cls_ctTRForeigner
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRForeigner() { }

        public string getMessage() { return this.Message.Replace("EMP_TR_FOREIGNER", "").Replace("cls_ctTRForeigner", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRForeigner> getData(string condition)
        {
            List<cls_TRForeigner> list_model = new List<cls_TRForeigner>();
            cls_TRForeigner model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");

                obj_str.Append(", FOREIGNER_ID");
                obj_str.Append(", PASSPORT_NO");
                obj_str.Append(", PASSPORT_ISSUE");
                obj_str.Append(", PASSPORT_EXPIRE");
                obj_str.Append(", VISA_NO");
                obj_str.Append(", VISA_ISSUE");
                obj_str.Append(", VISA_EXPIRE");
                obj_str.Append(", WORKPERMIT_NO");
                obj_str.Append(", WORKPERMIT_BY");
                obj_str.Append(", WORKPERMIT_ISSUE");
                obj_str.Append(", WORKPERMIT_EXPIRE");
                obj_str.Append(", ENTRY_DATE");
                obj_str.Append(", CERTIFICATE_NO");
                obj_str.Append(", CERTIFICATE_EXPIRE");
                obj_str.Append(", OTHERDOC_NO");
                obj_str.Append(", OTHERDOC_EXPIRE");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_TR_FOREIGNER");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRForeigner();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();

                    model.foreigner_id = Convert.ToInt32(dr["FOREIGNER_ID"]);
                    model.passport_no = dr["PASSPORT_NO"].ToString();
                    model.passport_issue = Convert.ToDateTime(dr["PASSPORT_ISSUE"]);
                    model.passport_expire = Convert.ToDateTime(dr["PASSPORT_EXPIRE"]);
                    model.visa_no = dr["VISA_NO"].ToString();
                    model.visa_issue = Convert.ToDateTime(dr["VISA_ISSUE"]);
                    model.visa_expire = Convert.ToDateTime(dr["VISA_EXPIRE"]);
                    model.workpermit_no = dr["WORKPERMIT_NO"].ToString();
                    model.workpermit_by = dr["WORKPERMIT_BY"].ToString();
                    model.workpermit_issue = Convert.ToDateTime(dr["WORKPERMIT_ISSUE"]);
                    model.workpermit_expire = Convert.ToDateTime(dr["WORKPERMIT_EXPIRE"]);
                    model.entry_date = Convert.ToDateTime(dr["ENTRY_DATE"]);
                    model.certificate_no = dr["CERTIFICATE_NO"].ToString();
                    model.certificate_expire = Convert.ToDateTime(dr["CERTIFICATE_EXPIRE"]);
                    model.otherdoc_no = dr["OTHERDOC_NO"].ToString();
                    model.otherdoc_expire = Convert.ToDateTime(dr["OTHERDOC_EXPIRE"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "EMPFGR001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRForeigner> getDataByFillter(string com, string emp)
        {
            string strCondition = "";

            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!emp.Equals(""))
                strCondition += " AND WORKER_CODE='" + emp + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(FOREIGNER_ID, 1) ");
                obj_str.Append(" FROM EMP_TR_FOREIGNER");
                obj_str.Append(" ORDER BY FOREIGNER_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPFGR002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp, string id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT FOREIGNER_ID");
                obj_str.Append(" FROM EMP_TR_FOREIGNER");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                obj_str.Append(" AND FOREIGNER_ID='" + id + "' ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPFGR003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM EMP_TR_FOREIGNER");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                obj_str.Append(" AND FOREIGNER_ID='" + id + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "EMPFGR004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_TRForeigner model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.company_code, model.worker_code, model.foreigner_id.ToString()))
                {
                    if (this.update(model))
                        return model.foreigner_id.ToString();
                    else
                        return "";
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_TR_FOREIGNER");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");

                obj_str.Append(", FOREIGNER_ID ");
                obj_str.Append(", PASSPORT_NO ");
                obj_str.Append(", PASSPORT_ISSUE ");
                obj_str.Append(", PASSPORT_EXPIRE ");
                obj_str.Append(", VISA_NO ");
                obj_str.Append(", VISA_ISSUE ");
                obj_str.Append(", VISA_EXPIRE ");
                obj_str.Append(", WORKPERMIT_NO ");
                obj_str.Append(", WORKPERMIT_BY");
                obj_str.Append(", WORKPERMIT_ISSUE ");
                obj_str.Append(", WORKPERMIT_EXPIRE ");
                obj_str.Append(", ENTRY_DATE");
                obj_str.Append(", CERTIFICATE_NO");
                obj_str.Append(", CERTIFICATE_EXPIRE");
                obj_str.Append(", OTHERDOC_NO");
                obj_str.Append(", OTHERDOC_EXPIRE");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @FOREIGNER_ID ");
                obj_str.Append(", @PASSPORT_NO ");
                obj_str.Append(", @PASSPORT_ISSUE ");
                obj_str.Append(", @PASSPORT_EXPIRE ");
                obj_str.Append(", @VISA_NO ");
                obj_str.Append(", @VISA_ISSUE ");
                obj_str.Append(", @VISA_EXPIRE ");
                obj_str.Append(", @WORKPERMIT_NO ");
                obj_str.Append(", @WORKPERMIT_BY ");
                obj_str.Append(", @WORKPERMIT_ISSUE ");
                obj_str.Append(", @WORKPERMIT_EXPIRE ");
                obj_str.Append(", @ENTRY_DATE ");
                obj_str.Append(", @CERTIFICATE_NO ");
                obj_str.Append(", @CERTIFICATE_EXPIRE ");
                obj_str.Append(", @OTHERDOC_NO ");
                obj_str.Append(", @OTHERDOC_EXPIRE ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.foreigner_id = this.getNextID();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@FOREIGNER_ID", SqlDbType.Int); obj_cmd.Parameters["@FOREIGNER_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@PASSPORT_NO", SqlDbType.VarChar); obj_cmd.Parameters["@PASSPORT_NO"].Value = model.passport_no;
                obj_cmd.Parameters.Add("@PASSPORT_ISSUE", SqlDbType.DateTime); obj_cmd.Parameters["@PASSPORT_ISSUE"].Value = model.passport_issue;
                obj_cmd.Parameters.Add("@PASSPORT_EXPIRE", SqlDbType.DateTime); obj_cmd.Parameters["@PASSPORT_EXPIRE"].Value = model.passport_expire;
                obj_cmd.Parameters.Add("@VISA_NO", SqlDbType.VarChar); obj_cmd.Parameters["@VISA_NO"].Value = model.visa_no;
                obj_cmd.Parameters.Add("@VISA_ISSUE", SqlDbType.DateTime); obj_cmd.Parameters["@VISA_ISSUE"].Value = model.visa_issue;
                obj_cmd.Parameters.Add("@VISA_EXPIRE", SqlDbType.DateTime); obj_cmd.Parameters["@VISA_EXPIRE"].Value = model.visa_expire;
                obj_cmd.Parameters.Add("@WORKPERMIT_NO", SqlDbType.VarChar); obj_cmd.Parameters["@WORKPERMIT_NO"].Value = model.workpermit_no;
                obj_cmd.Parameters.Add("@WORKPERMIT_BY", SqlDbType.VarChar); obj_cmd.Parameters["@WORKPERMIT_BY"].Value = model.workpermit_by;
                obj_cmd.Parameters.Add("@WORKPERMIT_ISSUE", SqlDbType.DateTime); obj_cmd.Parameters["@WORKPERMIT_ISSUE"].Value = model.workpermit_issue;
                obj_cmd.Parameters.Add("@WORKPERMIT_EXPIRE", SqlDbType.DateTime); obj_cmd.Parameters["@WORKPERMIT_EXPIRE"].Value = model.workpermit_expire;
                obj_cmd.Parameters.Add("@ENTRY_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@ENTRY_DATE"].Value = model.entry_date;
                obj_cmd.Parameters.Add("@CERTIFICATE_NO", SqlDbType.VarChar); obj_cmd.Parameters["@CERTIFICATE_NO"].Value = model.certificate_no;
                obj_cmd.Parameters.Add("@CERTIFICATE_EXPIRE", SqlDbType.DateTime); obj_cmd.Parameters["@CERTIFICATE_EXPIRE"].Value = model.certificate_expire;
                obj_cmd.Parameters.Add("@OTHERDOC_NO", SqlDbType.VarChar); obj_cmd.Parameters["@OTHERDOC_NO"].Value = model.otherdoc_no;
                obj_cmd.Parameters.Add("@OTHERDOC_EXPIRE", SqlDbType.DateTime); obj_cmd.Parameters["@OTHERDOC_EXPIRE"].Value = model.otherdoc_expire;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                strResult = model.foreigner_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "EMPFGR005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_TRForeigner model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE EMP_TR_FOREIGNER SET ");

                obj_str.Append(" PASSPORT_NO=@PASSPORT_NO ");
                obj_str.Append(", PASSPORT_ISSUE=@PASSPORT_ISSUE ");
                obj_str.Append(", PASSPORT_EXPIRE=@PASSPORT_EXPIRE ");
                obj_str.Append(", VISA_NO=@VISA_NO ");
                obj_str.Append(", VISA_ISSUE=@VISA_ISSUE ");
                obj_str.Append(", VISA_EXPIRE=@VISA_EXPIRE ");
                obj_str.Append(", WORKPERMIT_NO=@WORKPERMIT_NO ");
                obj_str.Append(", WORKPERMIT_BY=@WORKPERMIT_BY ");
                obj_str.Append(", WORKPERMIT_ISSUE=@WORKPERMIT_ISSUE ");
                obj_str.Append(", WORKPERMIT_EXPIRE=@WORKPERMIT_EXPIRE ");
                obj_str.Append(", ENTRY_DATE=@ENTRY_DATE ");
                obj_str.Append(", CERTIFICATE_NO=@CERTIFICATE_NO ");
                obj_str.Append(", CERTIFICATE_EXPIRE=@CERTIFICATE_EXPIRE ");
                obj_str.Append(", OTHERDOC_NO=@OTHERDOC_NO ");
                obj_str.Append(", OTHERDOC_EXPIRE=@OTHERDOC_EXPIRE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE FOREIGNER_ID=@FOREIGNER_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PASSPORT_NO", SqlDbType.VarChar); obj_cmd.Parameters["@PASSPORT_NO"].Value = model.passport_no;
                obj_cmd.Parameters.Add("@PASSPORT_ISSUE", SqlDbType.DateTime); obj_cmd.Parameters["@PASSPORT_ISSUE"].Value = model.passport_issue;
                obj_cmd.Parameters.Add("@PASSPORT_EXPIRE", SqlDbType.DateTime); obj_cmd.Parameters["@PASSPORT_EXPIRE"].Value = model.passport_expire;
                obj_cmd.Parameters.Add("@VISA_NO", SqlDbType.VarChar); obj_cmd.Parameters["@VISA_NO"].Value = model.visa_no;
                obj_cmd.Parameters.Add("@VISA_ISSUE", SqlDbType.DateTime); obj_cmd.Parameters["@VISA_ISSUE"].Value = model.visa_issue;
                obj_cmd.Parameters.Add("@VISA_EXPIRE", SqlDbType.DateTime); obj_cmd.Parameters["@VISA_EXPIRE"].Value = model.visa_expire;
                obj_cmd.Parameters.Add("@WORKPERMIT_NO", SqlDbType.VarChar); obj_cmd.Parameters["@WORKPERMIT_NO"].Value = model.workpermit_no;
                obj_cmd.Parameters.Add("@WORKPERMIT_BY", SqlDbType.VarChar); obj_cmd.Parameters["@WORKPERMIT_BY"].Value = model.workpermit_by;
                obj_cmd.Parameters.Add("@WORKPERMIT_ISSUE", SqlDbType.DateTime); obj_cmd.Parameters["@WORKPERMIT_ISSUE"].Value = model.workpermit_issue;
                obj_cmd.Parameters.Add("@WORKPERMIT_EXPIRE", SqlDbType.DateTime); obj_cmd.Parameters["@WORKPERMIT_EXPIRE"].Value = model.workpermit_expire;
                obj_cmd.Parameters.Add("@ENTRY_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@ENTRY_DATE"].Value = model.entry_date;
                obj_cmd.Parameters.Add("@CERTIFICATE_NO", SqlDbType.DateTime); obj_cmd.Parameters["@CERTIFICATE_NO"].Value = model.certificate_no;
                obj_cmd.Parameters.Add("@CERTIFICATE_EXPIRE", SqlDbType.DateTime); obj_cmd.Parameters["@CERTIFICATE_EXPIRE"].Value = model.certificate_expire;
                obj_cmd.Parameters.Add("@OTHERDOC_NO", SqlDbType.DateTime); obj_cmd.Parameters["@OTHERDOC_NO"].Value = model.otherdoc_no;
                obj_cmd.Parameters.Add("@OTHERDOC_EXPIRE", SqlDbType.DateTime); obj_cmd.Parameters["@OTHERDOC_EXPIRE"].Value = model.otherdoc_expire;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@FOREIGNER_ID", SqlDbType.Int); obj_cmd.Parameters["@FOREIGNER_ID"].Value = model.foreigner_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "EMPFGR006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
