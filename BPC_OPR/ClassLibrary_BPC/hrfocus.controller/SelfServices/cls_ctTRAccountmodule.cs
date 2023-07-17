using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRAccountmodule
    {
         string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRAccountmodule() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRAccountmodule> getData(string condition)
        {
            List<cls_TRAccountmodule> list_model = new List<cls_TRAccountmodule>();
            cls_TRAccountmodule model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", ACCOUNT_USER");
                obj_str.Append(", ACCOUNT_TYPE");
                obj_str.Append(", MODULE_CODE");

                obj_str.Append(" FROM SELF_TR_ACCOUNTMODULE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY ACCOUNT_USER");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRAccountmodule();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.account_user = dr["ACCOUNT_USER"].ToString();
                    model.account_type = dr["ACCOUNT_TYPE"].ToString();
                    model.module_code = dr["MODULE_CODE"].ToString();

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Accountmodule.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRAccountmodule> getDataByFillter(string com,string user,string type,string module)
        {
            string strCondition = "";
            if(!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!user.Equals(""))
                strCondition += " AND ACCOUNT_USER='" + user + "'";

            if (!type.Equals(""))
                strCondition += " AND ACCOUNT_TYPE='" + type + "'";

            if (!module.Equals(""))
                strCondition += " AND MODULE_CODE='" + module + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com,string user,string type,string module)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ACCOUNT_USER");
                obj_str.Append(" FROM SELF_TR_ACCOUNTMODULE");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                obj_str.Append(" AND ACCOUNT_USER='" + user + "'");
                obj_str.Append(" AND ACCOUNT_TYPE='" + type + "'");
                obj_str.Append(" AND MODULE_CODE='" + module + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Accountmodule.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(string com, string user, string type, string module)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SELF_TR_ACCOUNTMODULE");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND ACCOUNT_USER='" + user + "'");
                obj_str.Append(" AND ACCOUNT_TYPE='" + type + "'");
                if (!module.Equals(""))
                {
                    obj_str.Append(" AND MODULE_CODE='" + module + "'");
                }

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Accountmodule.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_TRAccountmodule model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.account_user,model.account_type,model.module_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("INSERT INTO SELF_TR_ACCOUNTMODULE");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", ACCOUNT_USER ");
                obj_str.Append(", ACCOUNT_TYPE ");
                obj_str.Append(", MODULE_CODE ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @ACCOUNT_USER ");
                obj_str.Append(", @ACCOUNT_TYPE ");
                obj_str.Append(", @MODULE_CODE ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@ACCOUNT_USER", SqlDbType.VarChar); obj_cmd.Parameters["@ACCOUNT_USER"].Value = model.account_user;
                obj_cmd.Parameters.Add("@ACCOUNT_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@ACCOUNT_TYPE"].Value = model.account_type;
                obj_cmd.Parameters.Add("@MODULE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@MODULE_CODE"].Value =model.module_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = model.account_user;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Accountmodule.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(List<cls_TRAccountmodule> list_model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SELF_TR_ACCOUNTMODULE");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", ACCOUNT_USER ");
                obj_str.Append(", ACCOUNT_TYPE ");
                obj_str.Append(", MODULE_CODE ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @ACCOUNT_USER ");
                obj_str.Append(", @ACCOUNT_TYPE ");
                obj_str.Append(", @MODULE_CODE ");
                obj_str.Append(" )");


                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                //-- Step 1 delete data old
                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM SELF_TR_ACCOUNTMODULE");
                obj_str2.Append(" WHERE 1=1 ");
                obj_str2.Append(" AND COMPANY_CODE='" + list_model[0].company_code + "'");
                obj_str2.Append(" AND ACCOUNT_USER='" + list_model[0].account_user + "' ");
                obj_str2.Append(" AND ACCOUNT_TYPE='" + list_model[0].account_type + "' ");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());
                if (blnResult)
                {
                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@ACCOUNT_USER", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@ACCOUNT_TYPE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@MODULE_CODE", SqlDbType.VarChar);
                    foreach (cls_TRAccountmodule model in list_model)
                    {

                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                        obj_cmd.Parameters["@ACCOUNT_USER"].Value = model.account_user;
                        obj_cmd.Parameters["@ACCOUNT_TYPE"].Value = model.account_type;
                        obj_cmd.Parameters["@MODULE_CODE"].Value = model.module_code;
                        obj_cmd.ExecuteNonQuery();

                    }

                    blnResult = obj_conn.doCommit();

                    if (!blnResult)
                    {
                        obj_conn.doRollback();
                    }
                }
                else
                {
                    obj_conn.doRollback();
                }
                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Accountmodule.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_TRAccountmodule model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SELF_TR_ACCOUNTMODULE SET ");
                obj_str.Append(" COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(", ACCOUNT_USER=@ACCOUNT_USER ");
                obj_str.Append(", ACCOUNT_TYPE=@ACCOUNT_TYPE ");
                obj_str.Append(", MODULE_CODE=@MODULE_CODE ");
                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND ACCOUNT_USER=@ACCOUNT_USER ");
                obj_str.Append(" AND ACCOUNT_TYPE=@ACCOUNT_TYPE ");
      

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());


                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@ACCOUNT_USER", SqlDbType.Int); obj_cmd.Parameters["@ACCOUNT_USER"].Value = model.account_user;
                obj_cmd.Parameters.Add("@ACCOUNT_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@ACCOUNT_TYPE"].Value = model.account_type;
                obj_cmd.Parameters.Add("@MODULE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@MODULE_CODE"].Value =model.module_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.account_user;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Accountmodule.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
