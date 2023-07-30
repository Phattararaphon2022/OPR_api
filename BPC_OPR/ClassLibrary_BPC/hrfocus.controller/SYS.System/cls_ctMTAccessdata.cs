using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;
namespace ClassLibrary_BPC.hrfocus.controller
{
   public class cls_ctMTAccessdata
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTAccessdata() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTAccessdata> getData(string condition)
        {
            List<cls_MTAccessdata> list_model = new List<cls_MTAccessdata>();
            cls_MTAccessdata model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", POLMENU_CODE");
                obj_str.Append(", ACCESSDATA_MODULE");
                obj_str.Append(", ACCESSDATA_NEW");
                obj_str.Append(", ACCESSDATA_EDIT");
                obj_str.Append(", ACCESSDATA_DELETE");
                obj_str.Append(", ACCESSDATA_SALARY");

                obj_str.Append(" FROM SYS_MT_ACCESSDATA");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTAccessdata();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.polmenu_code = dr["POLMENU_CODE"].ToString();
                    model.accessdata_module = dr["ACCESSDATA_MODULE"].ToString();
                    model.accessdata_new = Convert.ToBoolean(dr["ACCESSDATA_NEW"]);
                    model.accessdata_edit = Convert.ToBoolean(dr["ACCESSDATA_EDIT"]);
                    model.accessdata_delete = Convert.ToBoolean(dr["ACCESSDATA_DELETE"]);
                    model.accessdata_salary = Convert.ToBoolean(dr["ACCESSDATA_SALARY"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Accessdata.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTAccessdata> getDataByFillter(string com,string code)
        {
            string strCondition = "";
            if(!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";
            if (!code.Equals(""))
                strCondition += " AND POLMENU_CODE='" + code + "'";


            return this.getData(strCondition);
        }

        public bool checkDataOld(string com,string code,string module)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM SYS_MT_ACCESSDATA");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                if (!code.Equals(""))
                    obj_str.Append(" AND POLMENU_CODE='" + code + "'");
                if (!module.Equals(""))
                    obj_str.Append(" AND ACCESSDATA_MODULE='" + module + "' ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Accessdata.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(string com, string code, string module)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SYS_MT_ACCESSDATA");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                if (!module.Equals(""))
                    obj_str.Append(" AND ACCESSDATA_MODULE='" + module + "' ");
                if (!code.Equals(""))
                    obj_str.Append(" AND POLMENU_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Accessdata.delete)" + ex.ToString();
            }

            return blnResult;
        }
        public string insert(cls_MTAccessdata model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code,model.polmenu_code,model.accessdata_module))
                {
                    return this.update(model);
                }
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("INSERT INTO SYS_MT_ACCESSDATA");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", POLMENU_CODE ");
                obj_str.Append(", ACCESSDATA_MODULE ");
                obj_str.Append(", ACCESSDATA_NEW ");
                obj_str.Append(", ACCESSDATA_EDIT ");
                obj_str.Append(", ACCESSDATA_DELETE ");
                obj_str.Append(", ACCESSDATA_SALARY ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @POLMENU_CODE ");
                obj_str.Append(", @ACCESSDATA_MODULE ");
                obj_str.Append(", @ACCESSDATA_NEW ");
                obj_str.Append(", @ACCESSDATA_EDIT ");
                obj_str.Append(", @ACCESSDATA_DELETE ");
                obj_str.Append(", @ACCESSDATA_SALARY ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@POLMENU_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@POLMENU_CODE"].Value = model.polmenu_code;
                obj_cmd.Parameters.Add("@ACCESSDATA_MODULE", SqlDbType.VarChar); obj_cmd.Parameters["@ACCESSDATA_MODULE"].Value = model.accessdata_module;
                obj_cmd.Parameters.Add("@ACCESSDATA_NEW", SqlDbType.Bit); obj_cmd.Parameters["@ACCESSDATA_NEW"].Value = model.accessdata_new;
                obj_cmd.Parameters.Add("@ACCESSDATA_EDIT", SqlDbType.Bit); obj_cmd.Parameters["@ACCESSDATA_EDIT"].Value = model.accessdata_edit;
                obj_cmd.Parameters.Add("@ACCESSDATA_DELETE", SqlDbType.Bit); obj_cmd.Parameters["@ACCESSDATA_DELETE"].Value = model.accessdata_delete;
                obj_cmd.Parameters.Add("@ACCESSDATA_SALARY", SqlDbType.Bit); obj_cmd.Parameters["@ACCESSDATA_SALARY"].Value = model.accessdata_salary;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = model.polmenu_code;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Accessdata.insert)" + ex.ToString();
            }

            return blnResult;
        }
        public bool insert(List<cls_MTAccessdata> list_model,string polmenu_code)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_MT_ACCESSDATA");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", POLMENU_CODE ");
                obj_str.Append(", ACCESSDATA_MODULE ");
                obj_str.Append(", ACCESSDATA_NEW ");
                obj_str.Append(", ACCESSDATA_EDIT ");
                obj_str.Append(", ACCESSDATA_DELETE ");
                obj_str.Append(", ACCESSDATA_SALARY ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @POLMENU_CODE ");
                obj_str.Append(", @ACCESSDATA_MODULE ");
                obj_str.Append(", @ACCESSDATA_NEW ");
                obj_str.Append(", @ACCESSDATA_EDIT ");
                obj_str.Append(", @ACCESSDATA_DELETE ");
                obj_str.Append(", @ACCESSDATA_SALARY ");
                obj_str.Append(" )");


                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                //-- Step 1 delete data old
                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM SYS_MT_ACCESSDATA");
                obj_str2.Append(" WHERE 1=1 ");
                obj_str2.Append(" AND COMPANY_CODE='" + list_model[0].company_code + "'");
                obj_str2.Append(" AND POLMENU_CODE='" + polmenu_code + "'");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());
                if (blnResult)
                {
                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@POLMENU_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@ACCESSDATA_MODULE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@ACCESSDATA_NEW", SqlDbType.Bit);
                    obj_cmd.Parameters.Add("@ACCESSDATA_EDIT", SqlDbType.Bit);
                    obj_cmd.Parameters.Add("@ACCESSDATA_DELETE", SqlDbType.Bit);
                    obj_cmd.Parameters.Add("@ACCESSDATA_SALARY", SqlDbType.Bit);
                    foreach (cls_MTAccessdata model in list_model)
                    {

                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                        obj_cmd.Parameters["@POLMENU_CODE"].Value = polmenu_code;
                        obj_cmd.Parameters["@ACCESSDATA_MODULE"].Value = model.accessdata_module;
                        obj_cmd.Parameters["@ACCESSDATA_NEW"].Value = model.accessdata_new;
                        obj_cmd.Parameters["@ACCESSDATA_EDIT"].Value = model.accessdata_edit;
                        obj_cmd.Parameters["@ACCESSDATA_DELETE"].Value = model.accessdata_delete;
                        obj_cmd.Parameters["@ACCESSDATA_SALARY"].Value = model.accessdata_salary;
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
                Message = "ERROR::(Accessdata.insert)" + ex.ToString();
            }

            return blnResult;
        }


        public string update(cls_MTAccessdata model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SYS_MT_ACCESSDATA SET ");
                obj_str.Append(" COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(", POLMENU_CODE=@POLMENU_CODE ");
                obj_str.Append(", ACCESSDATA_MODULE=@ACCESSDATA_MODULE ");
                obj_str.Append(", ACCESSDATA_NEW=@ACCESSDATA_NEW ");
                obj_str.Append(", ACCESSDATA_EDIT=@ACCESSDATA_EDIT ");
                obj_str.Append(", ACCESSDATA_DELETE=@ACCESSDATA_DELETE ");
                obj_str.Append(", ACCESSDATA_SALARY=@ACCESSDATA_SALARY ");
                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND POLMENU_CODE=@POLMENU_CODE ");
                obj_str.Append(" AND ACCESSDATA_MODULE=@ACCESSDATA_MODULE ");
      

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());


                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@POLMENU_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@POLMENU_CODE"].Value = model.polmenu_code;
                obj_cmd.Parameters.Add("@ACCESSDATA_MODULE", SqlDbType.VarChar); obj_cmd.Parameters["@ACCESSDATA_MODULE"].Value = model.accessdata_module;
                obj_cmd.Parameters.Add("@ACCESSDATA_NEW", SqlDbType.Bit); obj_cmd.Parameters["@ACCESSDATA_NEW"].Value = model.accessdata_new;
                obj_cmd.Parameters.Add("@ACCESSDATA_EDIT", SqlDbType.Bit); obj_cmd.Parameters["@ACCESSDATA_EDIT"].Value = model.accessdata_edit;
                obj_cmd.Parameters.Add("@ACCESSDATA_DELETE", SqlDbType.Bit); obj_cmd.Parameters["@ACCESSDATA_DELETE"].Value = model.accessdata_delete;
                obj_cmd.Parameters.Add("@ACCESSDATA_SALARY", SqlDbType.Bit); obj_cmd.Parameters["@ACCESSDATA_SALARY"].Value = model.accessdata_salary;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.accessdata_module;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Accessdata.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
