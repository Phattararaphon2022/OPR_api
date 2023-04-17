using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRAccountdep
    {
           string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRAccountdep() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRAccountdep> getData(string condition)
        {
            List<cls_TRAccountdep> list_model = new List<cls_TRAccountdep>();
            cls_TRAccountdep model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", ACCOUNT_USER");
                obj_str.Append(", ACCOUNT_TYPE");
                obj_str.Append(", LEVEL_CODE");
                obj_str.Append(", DEP_CODE");

                obj_str.Append(" FROM SELF_TR_ACCOUNTDEP");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY ACCOUNT_USER");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRAccountdep();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.account_user = dr["ACCOUNT_USER"].ToString();
                    model.account_type = dr["ACCOUNT_TYPE"].ToString();
                    model.level_code = dr["LEVEL_CODE"].ToString();
                    model.dep_code = dr["DEP_CODE"].ToString();

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Accountdep.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRAccountdep> getDataByFillter(string com,string user,string type,string level,string dep)
        {
            string strCondition = "";
            if(!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!user.Equals(""))
                strCondition += " AND ACCOUNT_USER='" + user + "'";

            if (!type.Equals(""))
                strCondition += " AND ACCOUNT_TYPE='" + type + "'";

            if (!level.Equals(""))
                strCondition += " AND LEVEL_CODE='" + level + "'";

            if (!dep.Equals(""))
                strCondition += " AND DEP_CODE='" + dep + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string user, string type, string level, string dep)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ACCOUNT_USER");
                obj_str.Append(" FROM SELF_TR_ACCOUNTDEP");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                obj_str.Append(" AND ACCOUNT_USER='" + user + "'");
                obj_str.Append(" AND ACCOUNT_TYPE='" + type + "'");
                obj_str.Append(" AND LEVEL_CODE='" + level + "'");
                obj_str.Append(" AND DEP_CODE='" + dep + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Accountdep.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(string com, string user, string type, string level, string dep)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SELF_TR_ACCOUNTDEP");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND ACCOUNT_USER='" + user + "'");
                obj_str.Append(" AND ACCOUNT_TYPE='" + type + "'");
                obj_str.Append(" AND LEVEL_CODE='" + level + "'");
                obj_str.Append(" AND DEP_CODE='" + dep + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Accountdep.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_TRAccountdep model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.account_user,model.account_type,model.level_code,model.dep_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("INSERT INTO SELF_TR_ACCOUNTDEP");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", ACCOUNT_USER ");
                obj_str.Append(", ACCOUNT_TYPE ");
                obj_str.Append(", LEVEL_CODE ");
                obj_str.Append(", DEP_CODE ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @ACCOUNT_USER ");
                obj_str.Append(", @ACCOUNT_TYPE ");
                obj_str.Append(", @LEVEL_CODE ");
                obj_str.Append(", @DEP_CODE ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@ACCOUNT_USER", SqlDbType.VarChar); obj_cmd.Parameters["@ACCOUNT_USER"].Value = model.account_user;
                obj_cmd.Parameters.Add("@ACCOUNT_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@ACCOUNT_TYPE"].Value = model.account_type;
                obj_cmd.Parameters.Add("@LEVEL_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@LEVEL_CODE"].Value = model.level_code;
                obj_cmd.Parameters.Add("@DEP_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@DEP_CODE"].Value = model.dep_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = model.account_user;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Accountdep.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_TRAccountdep model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SELF_TR_ACCOUNTDEP SET ");
                obj_str.Append(" COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(", ACCOUNT_USER=@ACCOUNT_USER ");
                obj_str.Append(", ACCOUNT_TYPE=@ACCOUNT_TYPE ");
                obj_str.Append(", LEVEL_CODE=@LEVEL_CODE ");
                obj_str.Append(", DEP_CODE=@DEP_CODE ");
                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND ACCOUNT_USER=@ACCOUNT_USER ");
                obj_str.Append(" AND ACCOUNT_TYPE=@ACCOUNT_TYPE ");
      

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());


                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@ACCOUNT_USER", SqlDbType.Int); obj_cmd.Parameters["@ACCOUNT_USER"].Value = model.account_user;
                obj_cmd.Parameters.Add("@ACCOUNT_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@ACCOUNT_TYPE"].Value = model.account_type;
                obj_cmd.Parameters.Add("@LEVEL_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@LEVEL_CODE"].Value = model.level_code;
                obj_cmd.Parameters.Add("@DEP_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@DEP_CODE"].Value = model.dep_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.account_user;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Accountdep.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
