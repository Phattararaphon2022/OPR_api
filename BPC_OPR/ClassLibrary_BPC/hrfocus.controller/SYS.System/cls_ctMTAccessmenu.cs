using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTAccessmenu
    {
         string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTAccessmenu() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTAccessmenu> getData(string condition)
        {
            List<cls_MTAccessmenu> list_model = new List<cls_MTAccessmenu>();
            cls_MTAccessmenu model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", POLMENU_CODE");
                obj_str.Append(", ACCESSMENU_MODULE");
                obj_str.Append(", ACCESSMENU_TYPE");
                obj_str.Append(", ACCESSMENU_CODE");

                obj_str.Append(" FROM SYS_MT_ACCESSMENU");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTAccessmenu();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.polmenu_code = dr["POLMENU_CODE"].ToString();
                    model.accessmenu_module = dr["ACCESSMENU_MODULE"].ToString();
                    model.accessmenu_type = dr["ACCESSMENU_TYPE"].ToString();
                    model.accessmenu_code = dr["ACCESSMENU_CODE"].ToString();

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Accessmenu.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTAccessmenu> getDataByFillter(string com, string code,  string module,string type, string accessmenu_code)
        {
            string strCondition = "";
            if(!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";
            if (!code.Equals(""))
                strCondition += " AND POLMENU_CODE='" + code + "'";
            if (!module.Equals(""))
                strCondition += " AND ACCESSMENU_MODULE='" + module + "'";
            if (!type.Equals(""))
                strCondition += " AND ACCESSMENU_TYPE='" + type + "'";
            if (!accessmenu_code.Equals(""))
                strCondition += " AND ACCESSMENU_CODE='" + accessmenu_code + "'";


            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string code, string module, string type, string accessmenu_code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM SYS_MT_ACCESSMENU");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                if (!code.Equals(""))
                    obj_str.Append(" AND POLMENU_CODE='" + code + "'");

                if (!module.Equals(""))
                    obj_str.Append(" AND ACCESSMENU_MODULE='" + module + "' ");

                if (!type.Equals(""))
                    obj_str.Append(" AND ACCESSMENU_TYPE='" + type + "' ");

                if (!accessmenu_code.Equals(""))
                    obj_str.Append(" AND ACCESSMENU_CODE='" + accessmenu_code + "' ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Accessmenu.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(string com, string code, string module,string type,string accessmenu_code)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SYS_MT_ACCESSMENU");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                if (!module.Equals(""))
                    obj_str.Append(" AND ACCESSMENU_MODULE='" + module + "' ");
                if (!code.Equals(""))
                    obj_str.Append(" AND POLMENU_CODE='" + code + "'");
                if (!code.Equals(""))
                    obj_str.Append(" AND ACCESSMENU_TYPE='" + type + "'");
                if (!code.Equals(""))
                    obj_str.Append(" AND ACCESSMENU_CODE='" + accessmenu_code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Accessmenu.delete)" + ex.ToString();
            }

            return blnResult;
        }
        public string insert(cls_MTAccessmenu model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code,model.polmenu_code,model.accessmenu_module,model.accessmenu_type,model.accessmenu_code))
                {
                    return this.update(model);
                }
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("INSERT INTO SYS_MT_ACCESSMENU");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", POLMENU_CODE ");
                obj_str.Append(", ACCESSMENU_MODULE ");
                obj_str.Append(", ACCESSMENU_TYPE ");
                obj_str.Append(", ACCESSMENU_CODE ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @POLMENU_CODE ");
                obj_str.Append(", @ACCESSMENU_MODULE ");
                obj_str.Append(", @ACCESSMENU_TYPE ");
                obj_str.Append(", @ACCESSMENU_CODE ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@POLMENU_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@POLMENU_CODE"].Value = model.polmenu_code;
                obj_cmd.Parameters.Add("@ACCESSMENU_MODULE", SqlDbType.VarChar); obj_cmd.Parameters["@ACCESSMENU_MODULE"].Value = model.accessmenu_module;
                obj_cmd.Parameters.Add("@ACCESSMENU_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@ACCESSMENU_TYPE"].Value = model.accessmenu_type;
                obj_cmd.Parameters.Add("@ACCESSMENU_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@ACCESSMENU_CODE"].Value = model.accessmenu_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = model.accessmenu_code;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Accessmenu.insert)" + ex.ToString();
            }

            return blnResult;
        }
        public bool insert(List<cls_MTAccessmenu> list_model, string polmenu_code)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_MT_ACCESSMENU");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", POLMENU_CODE ");
                obj_str.Append(", ACCESSMENU_MODULE ");
                obj_str.Append(", ACCESSMENU_TYPE ");
                obj_str.Append(", ACCESSMENU_CODE ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @POLMENU_CODE ");
                obj_str.Append(", @ACCESSMENU_MODULE ");
                obj_str.Append(", @ACCESSMENU_TYPE ");
                obj_str.Append(", @ACCESSMENU_CODE ");
                obj_str.Append(" )");


                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                //-- Step 1 delete data old
                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM SYS_MT_ACCESSMENU");
                obj_str2.Append(" WHERE 1=1 ");
                obj_str2.Append(" AND COMPANY_CODE='" + list_model[0].company_code + "'");
                obj_str2.Append(" AND POLMENU_CODE='" + polmenu_code + "'");
                obj_str2.Append(" AND ACCESSMENU_MODULE='" + list_model[0].accessmenu_module + "'");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());
                if (blnResult)
                {
                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@POLMENU_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@ACCESSMENU_MODULE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@ACCESSMENU_TYPE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@ACCESSMENU_CODE", SqlDbType.VarChar);
                    foreach (cls_MTAccessmenu model in list_model)
                    {

                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                        obj_cmd.Parameters["@POLMENU_CODE"].Value = polmenu_code;
                        obj_cmd.Parameters["@ACCESSMENU_MODULE"].Value = model.accessmenu_module;
                        obj_cmd.Parameters["@ACCESSMENU_TYPE"].Value = model.accessmenu_type;
                        obj_cmd.Parameters["@ACCESSMENU_CODE"].Value = model.accessmenu_code;
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
                Message = "ERROR::(Accessmenu.insert)" + ex.ToString();
            }

            return blnResult;
        }


        public string update(cls_MTAccessmenu model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SYS_MT_ACCESSMENU SET ");
                obj_str.Append(" COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(", POLMENU_CODE=@POLMENU_CODE ");
                obj_str.Append(", ACCESSMENU_MODULE=@ACCESSMENU_MODULE ");
                obj_str.Append(", ACCESSMENU_TYPE=@ACCESSMENU_TYPE ");
                obj_str.Append(", ACCESSMENU_CODE=@ACCESSMENU_CODE ");
                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND POLMENU_CODE=@POLMENU_CODE ");
                obj_str.Append(" AND ACCESSMENU_TYPE=@ACCESSMENU_TYPE ");
                obj_str.Append(" AND ACCESSMENU_MODULE=@ACCESSMENU_MODULE ");
      

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());


                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@POLMENU_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@POLMENU_CODE"].Value = model.polmenu_code;
                obj_cmd.Parameters.Add("@ACCESSMENU_MODULE", SqlDbType.VarChar); obj_cmd.Parameters["@ACCESSMENU_MODULE"].Value = model.accessmenu_module;
                obj_cmd.Parameters.Add("@ACCESSMENU_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@ACCESSMENU_TYPE"].Value = model.accessmenu_type;
                obj_cmd.Parameters.Add("@ACCESSMENU_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@ACCESSMENU_CODE"].Value = model.accessmenu_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.accessmenu_code;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Accessmenu.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
