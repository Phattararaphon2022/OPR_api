﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;
namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTAccount
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTAccount() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTAccount> getData(string condition)
        {
            List<cls_MTAccount> list_model = new List<cls_MTAccount>();
            cls_MTAccount model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append(" COMPANY_CODE");
                obj_str.Append(", ACCOUNT_USER");
                obj_str.Append(", ACCOUNT_PWD");

                obj_str.Append(", ACCOUNT_TYPE");
                obj_str.Append(", ACCOUNT_LEVEL");
                obj_str.Append(", ACCOUNT_EMP");
                
                obj_str.Append(", ACCOUNT_EMAIL");
                obj_str.Append(", ACCOUNT_EMAIL_ALERT");
                obj_str.Append(", ACCOUNT_LINE");
                obj_str.Append(", ACCOUNT_LINE_ALERT");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");
                obj_str.Append(", FLAG");

                obj_str.Append(" FROM SELF_MT_ACCOUNT");

                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTAccount();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.account_user = dr["ACCOUNT_USER"].ToString();
                    model.account_pwd = dr["ACCOUNT_PWD"].ToString();

                    model.account_type = dr["ACCOUNT_TYPE"].ToString();
                    model.account_level = Convert.ToInt32(dr["ACCOUNT_LEVEL"].ToString());
                    model.account_emp = dr["ACCOUNT_EMP"].ToString();
                    model.account_email = dr["ACCOUNT_EMAIL"].ToString();
                    model.account_email_alert = Convert.ToBoolean(dr["ACCOUNT_EMAIL_ALERT"].ToString());
                    model.account_line = dr["ACCOUNT_LINE"].ToString();
                    model.account_line_alert = Convert.ToBoolean(dr["ACCOUNT_LINE_ALERT"].ToString());
                    model.flag = Convert.ToBoolean(dr["FLAG"].ToString());

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Account.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTAccount> getDataByFillter(string com,string user, string type ,string emp)
        {
            string strCondition = "";
            if(!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";
            if(!user.Equals(""))
                strCondition += " AND ACCOUNT_USER='" + user + "'";
            if (!type.Equals(""))
                strCondition += " AND ACCOUNT_TYPE='" + type + "'";
            if (!emp.Equals(""))
                strCondition += " AND ACCOUNT_EMP='" + emp + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string user, string type)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM SELF_MT_ACCOUNT");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND ACCOUNT_USER='" + user + "'");
                obj_str.Append(" AND ACCOUNT_TYPE='" + type + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Account.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(string com, string user, string type)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SELF_MT_ACCOUNT");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND ACCOUNT_USER='" + user + "'");
                obj_str.Append(" AND ACCOUNT_TYPE='" + type + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Account.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTAccount model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.account_user, model.account_type))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("INSERT INTO SELF_MT_ACCOUNT");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", ACCOUNT_USER ");
                obj_str.Append(", ACCOUNT_PWD ");

                obj_str.Append(", ACCOUNT_TYPE ");
                obj_str.Append(", ACCOUNT_LEVEL ");
                obj_str.Append(", ACCOUNT_EMP ");
                obj_str.Append(", ACCOUNT_EMAIL ");
                obj_str.Append(", ACCOUNT_EMAIL_ALERT ");
                obj_str.Append(", ACCOUNT_LINE ");
                obj_str.Append(", ACCOUNT_LINE_ALERT ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @ACCOUNT_USER ");
                obj_str.Append(", @ACCOUNT_PWD ");

                obj_str.Append(", @ACCOUNT_TYPE ");
                obj_str.Append(", @ACCOUNT_LEVEL ");
                obj_str.Append(", @ACCOUNT_EMP ");
                obj_str.Append(", @ACCOUNT_EMAIL ");
                obj_str.Append(", @ACCOUNT_EMAIL_ALERT ");
                obj_str.Append(", @ACCOUNT_LINE ");
                obj_str.Append(", @ACCOUNT_LINE_ALERT ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@ACCOUNT_USER", SqlDbType.VarChar); obj_cmd.Parameters["@ACCOUNT_USER"].Value = model.account_user;
                obj_cmd.Parameters.Add("@ACCOUNT_PWD", SqlDbType.VarChar); obj_cmd.Parameters["@ACCOUNT_PWD"].Value = model.account_pwd;

                obj_cmd.Parameters.Add("@ACCOUNT_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@ACCOUNT_TYPE"].Value = model.account_type;
                obj_cmd.Parameters.Add("@ACCOUNT_LEVEL", SqlDbType.Int); obj_cmd.Parameters["@ACCOUNT_LEVEL"].Value = model.account_level;
                obj_cmd.Parameters.Add("@ACCOUNT_EMP", SqlDbType.VarChar); obj_cmd.Parameters["@ACCOUNT_EMP"].Value = model.account_emp;
                obj_cmd.Parameters.Add("@ACCOUNT_EMAIL", SqlDbType.VarChar); obj_cmd.Parameters["@ACCOUNT_EMAIL"].Value = model.account_email;
                obj_cmd.Parameters.Add("@ACCOUNT_EMAIL_ALERT", SqlDbType.Bit); obj_cmd.Parameters["@ACCOUNT_EMAIL_ALERT"].Value = model.account_email_alert;
                obj_cmd.Parameters.Add("@ACCOUNT_LINE", SqlDbType.VarChar); obj_cmd.Parameters["@ACCOUNT_LINE"].Value = model.account_line;
                obj_cmd.Parameters.Add("@ACCOUNT_LINE_ALERT", SqlDbType.Bit); obj_cmd.Parameters["@ACCOUNT_LINE_ALERT"].Value = model.account_line_alert;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = model.account_user;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Account.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_MTAccount model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SELF_MT_ACCOUNT SET ");

                obj_str.Append("ACCOUNT_USER=@ACCOUNT_USER ");
                obj_str.Append(", ACCOUNT_PWD=@ACCOUNT_PWD ");

                obj_str.Append(", ACCOUNT_TYPE=@ACCOUNT_TYPE ");
                obj_str.Append(", ACCOUNT_LEVEL=@ACCOUNT_LEVEL ");
                obj_str.Append(", ACCOUNT_EMP=@ACCOUNT_EMP ");
                obj_str.Append(", ACCOUNT_EMAIL=@ACCOUNT_EMAIL ");
                obj_str.Append(", ACCOUNT_EMAIL_ALERT=@ACCOUNT_EMAIL_ALERT ");
                obj_str.Append(", ACCOUNT_LINE=@ACCOUNT_LINE ");
                obj_str.Append(", ACCOUNT_LINE_ALERT=@ACCOUNT_LINE_ALERT ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND ACCOUNT_USER=@ACCOUNT_USER ");
                obj_str.Append(" AND ACCOUNT_TYPE=@ACCOUNT_TYPE ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@ACCOUNT_USER", SqlDbType.VarChar); obj_cmd.Parameters["@ACCOUNT_USER"].Value = model.account_user;
                obj_cmd.Parameters.Add("@ACCOUNT_PWD", SqlDbType.VarChar); obj_cmd.Parameters["@ACCOUNT_PWD"].Value = model.account_pwd;

                obj_cmd.Parameters.Add("@ACCOUNT_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@ACCOUNT_TYPE"].Value = model.account_type;
                obj_cmd.Parameters.Add("@ACCOUNT_LEVEL", SqlDbType.Int); obj_cmd.Parameters["@ACCOUNT_LEVEL"].Value = model.account_level;
                obj_cmd.Parameters.Add("@ACCOUNT_EMP", SqlDbType.VarChar); obj_cmd.Parameters["@ACCOUNT_EMP"].Value = model.account_emp;
                obj_cmd.Parameters.Add("@ACCOUNT_EMAIL", SqlDbType.VarChar); obj_cmd.Parameters["@ACCOUNT_EMAIL"].Value = model.account_email;
                obj_cmd.Parameters.Add("@ACCOUNT_EMAIL_ALERT", SqlDbType.Bit); obj_cmd.Parameters["@ACCOUNT_EMAIL_ALERT"].Value = model.account_email_alert;
                obj_cmd.Parameters.Add("@ACCOUNT_LINE", SqlDbType.VarChar); obj_cmd.Parameters["@ACCOUNT_LINE"].Value = model.account_line;
                obj_cmd.Parameters.Add("@ACCOUNT_LINE_ALERT", SqlDbType.Bit); obj_cmd.Parameters["@ACCOUNT_LINE_ALERT"].Value = model.account_line_alert;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.account_user.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Account.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
