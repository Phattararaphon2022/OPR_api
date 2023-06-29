using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTMailconfig
    {
        string Message = string.Empty;
        
        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTMailconfig() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTMailconfig> getData(string condition)
        {
            List<cls_MTMailconfig> list_model = new List<cls_MTMailconfig>();
            cls_MTMailconfig model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", MAIL_ID");
                obj_str.Append(", MAIL_SERVER");
                obj_str.Append(", MAIL_SERVERPORT");
                obj_str.Append(", MAIL_LOGIN");
                obj_str.Append(", MAIL_PASSWORD");
                obj_str.Append(", MAIL_FROMNAME");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM SELF_MT_MAILCONFIG");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY MAIL_ID");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTMailconfig();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.mail_id = Convert.ToInt32(dr["MAIL_ID"]);
                    model.mail_server = dr["MAIL_SERVER"].ToString();
                    model.mail_serverport = dr["MAIL_SERVERPORT"].ToString();
                    model.mail_login = dr["MAIL_LOGIN"].ToString();
                    model.mail_password = dr["MAIL_PASSWORD"].ToString();
                    model.mail_fromname = dr["MAIL_FROMNAME"].ToString();
       
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTMailconfig.getData)" + ex.ToString();
            }

            return list_model;
        }
        public List<cls_MTMailconfig> getDataByFillter(string com,int id)
        {
            string strCondition = "";
            if(!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!id.Equals(0))
                strCondition += " AND MAIL_ID='" + id + "'";
            return this.getData(strCondition);
        }
        public bool checkDataOld(string com, int id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAIL_ID");
                obj_str.Append(" FROM SELF_MT_MAILCONFIG");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                if(!id.Equals(0))
                    obj_str.Append(" AND MAIL_ID='" + id + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTMailconfig.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(string com,int id)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SELF_MT_MAILCONFIG");
                obj_str.Append(" WHERE 1=1 ");
                if (!com.Equals(""))
                    obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                if (!id.Equals(0))
                    obj_str.Append(" AND MAIL_ID='" + id + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(MTMailconfig.delete)" + ex.ToString();
            }

            return blnResult;
        }
        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(MAIL_ID) ");
                obj_str.Append(" FROM SELF_MT_MAILCONFIG");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTMailconfig.getNextID)" + ex.ToString();
            }

            return intResult;
        }
        public string insert(cls_MTMailconfig model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code,model.mail_id))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO SELF_MT_MAILCONFIG");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", MAIL_ID");
                obj_str.Append(", MAIL_SERVER");
                obj_str.Append(", MAIL_SERVERPORT");
                obj_str.Append(", MAIL_LOGIN");
                obj_str.Append(", MAIL_PASSWORD");
                obj_str.Append(", MAIL_FROMNAME");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @MAIL_ID ");
                obj_str.Append(", @MAIL_SERVER ");
                obj_str.Append(", @MAIL_SERVERPORT ");
                obj_str.Append(", @MAIL_LOGIN ");
                obj_str.Append(", @MAIL_PASSWORD ");
                obj_str.Append(", @MAIL_FROMNAME ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@MAIL_ID", SqlDbType.Int); obj_cmd.Parameters["@MAIL_ID"].Value = id; ;
                obj_cmd.Parameters.Add("@MAIL_SERVER", SqlDbType.VarChar); obj_cmd.Parameters["@MAIL_SERVER"].Value = model.mail_server;
                obj_cmd.Parameters.Add("@MAIL_SERVERPORT", SqlDbType.VarChar); obj_cmd.Parameters["@MAIL_SERVERPORT"].Value = model.mail_serverport;
                obj_cmd.Parameters.Add("@MAIL_LOGIN", SqlDbType.VarChar); obj_cmd.Parameters["@MAIL_LOGIN"].Value = model.mail_login;
                obj_cmd.Parameters.Add("@MAIL_PASSWORD", SqlDbType.VarChar); obj_cmd.Parameters["@MAIL_PASSWORD"].Value = model.mail_password;
                obj_cmd.Parameters.Add("@MAIL_FROMNAME", SqlDbType.VarChar); obj_cmd.Parameters["@MAIL_FROMNAME"].Value = model.mail_fromname;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTMailconfig.insert)" + ex.ToString();
            }

            return blnResult;
        }
        public string update(cls_MTMailconfig model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SELF_MT_MAILCONFIG SET ");
                obj_str.Append(" COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(", MAIL_SERVER=@MAIL_SERVER ");
                obj_str.Append(", MAIL_SERVERPORT=@MAIL_SERVERPORT ");
                obj_str.Append(", MAIL_LOGIN=@MAIL_LOGIN ");
                obj_str.Append(", MAIL_PASSWORD=@MAIL_PASSWORD ");
                obj_str.Append(", MAIL_FROMNAME=@MAIL_FROMNAME ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND MAIL_ID=@MAIL_ID ");
                
      

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());


                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@MAIL_ID", SqlDbType.Int); obj_cmd.Parameters["@MAIL_ID"].Value = model.mail_id; ;
                obj_cmd.Parameters.Add("@MAIL_SERVER", SqlDbType.VarChar); obj_cmd.Parameters["@MAIL_SERVER"].Value = model.mail_server;
                obj_cmd.Parameters.Add("@MAIL_SERVERPORT", SqlDbType.VarChar); obj_cmd.Parameters["@MAIL_SERVERPORT"].Value = model.mail_serverport;
                obj_cmd.Parameters.Add("@MAIL_LOGIN", SqlDbType.VarChar); obj_cmd.Parameters["@MAIL_LOGIN"].Value = model.mail_login;
                obj_cmd.Parameters.Add("@MAIL_PASSWORD", SqlDbType.VarChar); obj_cmd.Parameters["@MAIL_PASSWORD"].Value = model.mail_password;
                obj_cmd.Parameters.Add("@MAIL_FROMNAME", SqlDbType.VarChar); obj_cmd.Parameters["@MAIL_FROMNAME"].Value = model.mail_fromname;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.mail_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTMailconfig.update)" + ex.ToString();
            }

            return blnResult;
        }

    }
    }
