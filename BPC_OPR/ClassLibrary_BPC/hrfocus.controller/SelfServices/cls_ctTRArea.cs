using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRArea
    {
            string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRArea() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRArea> getData(string condition)
        {
            List<cls_TRArea> list_model = new List<cls_TRArea>();
            cls_TRArea model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", LOCATION_CODE");
                obj_str.Append(", WORKER_CODE");
             
                obj_str.Append(" FROM SELF_TR_AREA");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRArea();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.location_code = dr["LOCATION_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
  
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRArea.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRArea> getDataByFillter(string com,string location_code,string worker_code)
        {
            string strCondition = "";
            if(!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!location_code.Equals(""))
                strCondition += " AND LOCATION_CODE='" + location_code + "'";

            if (!worker_code.Equals(""))
                strCondition += " AND WORKER_CODE='" + worker_code + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string location_code, string worker_code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT LOCATION_CODE");
                obj_str.Append(" FROM SELF_TR_AREA");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                obj_str.Append(" AND LOCATION_CODE='" + location_code + "'");
                obj_str.Append(" AND WORKER_CODE='" + worker_code + "'");
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRArea.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(string com, string location_code, string worker_code)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SELF_TR_AREA");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND LOCATION_CODE='" + location_code + "'");
                if (!worker_code.Equals(""))
                    obj_str.Append(" AND WORKER_CODE='" + worker_code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(TRArea.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_TRArea model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.location_code,model.worker_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("INSERT INTO SELF_TR_AREA");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", LOCATION_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @LOCATION_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@LOCATION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@LOCATION_CODE"].Value = model.location_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = model.worker_code;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRArea.insert)" + ex.ToString();
            }

            return blnResult;
        }
        public bool insert(List<cls_TRArea> list_model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SELF_TR_AREA");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", LOCATION_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @LOCATION_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(" )");


                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                //-- Step 1 delete data old
                string emp = "";
                foreach (cls_TRArea model in list_model)
                {
                    emp += "'" + model.worker_code + "',";
                }
                if (emp.Length > 0)
                    emp = emp.Substring(0, emp.Length - 1);
                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM SELF_TR_AREA");
                obj_str2.Append(" WHERE 1=1 ");
                obj_str2.Append(" AND COMPANY_CODE='" + list_model[0].company_code + "'");
                obj_str2.Append(" AND LOCATION_CODE='" + list_model[0].location_code + "' ");
                //obj_str2.Append(" AND WORKER_CODE IN (" + emp + ")");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());
                if (blnResult)
                {
                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@LOCATION_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar);
                    foreach (cls_TRArea model in list_model)
                    {

                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                        obj_cmd.Parameters["@LOCATION_CODE"].Value = model.location_code;
                        obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
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
                Message = "ERROR::(TRArea.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_TRArea model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SELF_TR_AREA SET ");
                obj_str.Append(" COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(", LOCATION_CODE=@LOCATION_CODE ");
                obj_str.Append(", WORKER_CODE=@WORKER_CODE ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND LOCATION_CODE=@LOCATION_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
      

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());


                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@LOCATION_CODE", SqlDbType.Int); obj_cmd.Parameters["@LOCATION_CODE"].Value = model.location_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.worker_code;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRArea.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
