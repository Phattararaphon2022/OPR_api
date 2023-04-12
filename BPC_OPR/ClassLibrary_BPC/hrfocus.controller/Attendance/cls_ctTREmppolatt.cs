using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;


namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTREmppolatt
    {
   string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTREmppolatt() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TREmppolatt> getData(string condition)
        {
            List<cls_TREmppolatt> list_model = new List<cls_TREmppolatt>();
            cls_TREmppolatt model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");

                obj_str.Append(", EMPPOLATT_POLICY_CODE");
                obj_str.Append(", EMPPOLATT_POLICY_TYPE");
                obj_str.Append(", ISNULL(EMPPOLATT_POLICY_NOTE, '') AS EMPPOLATT_POLICY_NOTE");

                obj_str.Append(", CREATED_BY");
                obj_str.Append(", CREATED_DATE");

                obj_str.Append(" FROM ATT_TR_EMPPOLATT");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, WORKER_CODE, EMPPOLATT_POLICY_TYPE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TREmppolatt();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();

                    model.emppolatt_policy_code = dr["EMPPOLATT_POLICY_CODE"].ToString();
                    model.emppolatt_policy_type = dr["EMPPOLATT_POLICY_TYPE"].ToString();
                    model.emppolatt_policy_note = dr["EMPPOLATT_POLICY_NOTE"].ToString();


                    model.created_by = dr["CREATED_BY"].ToString();
                    model.created_date = Convert.ToDateTime(dr["CREATED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Emppolatt.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TREmppolatt> getDataByFillter(string com, string worker, string type)
        {
            string strCondition = " AND COMPANY_CODE='" + com + "'";

            if (!worker.Equals(""))
                strCondition += " AND WORKER_CODE='" + worker + "'";

            if (!type.Equals(""))
                strCondition += " AND EMPPOLATT_POLICY_TYPE='" + type + "'";


            return this.getData(strCondition);
        }


        public bool delete(string com, string emp, string type)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_EMPPOLATT");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND WORKER_CODE='" + emp + "'");
                obj_str.Append(" AND EMPPOLATT_POLICY_TYPE='" + type + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Emppolatt.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool deleteMutileEmp(string com, string emp, string type)
        {
            bool blnResult = true;
            try
            {



                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_EMPPOLATT");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND WORKER_COD IN (" + emp + ")");
                obj_str.Append(" AND EMPPOLATT_POLICY_TYPE='" + type + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Emppolatt.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(List<cls_TREmppolatt> list_model)
        {
            bool blnResult = false;
            try
            {


                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO ATT_TR_EMPPOLATT");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", EMPPOLATT_POLICY_CODE ");
                obj_str.Append(", EMPPOLATT_POLICY_TYPE ");
                obj_str.Append(", EMPPOLATT_POLICY_NOTE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @EMPPOLATT_POLICY_CODE ");
                obj_str.Append(", @EMPPOLATT_POLICY_TYPE ");
                obj_str.Append(", @EMPPOLATT_POLICY_NOTE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");


                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                //-- Step 1 delete data old
                string strWorkerID = "";
                foreach (cls_TREmppolatt model in list_model)
                {
                    strWorkerID += "'" + model.worker_code + "',";
                }
                if (strWorkerID.Length > 0)
                    strWorkerID = strWorkerID.Substring(0, strWorkerID.Length - 1);

                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM ATT_TR_EMPPOLATT");
                obj_str2.Append(" WHERE 1=1 ");
                obj_str2.Append(" AND COMPANY_CODE='" + list_model[0].company_code + "'");
                obj_str2.Append(" AND WORKER_CODE IN (" + strWorkerID + ")");
                obj_str2.Append(" AND EMPPOLATT_POLICY_TYPE='" + list_model[0].emppolatt_policy_type + "'");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());

                if (blnResult)
                {

                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPPOLATT_POLICY_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPPOLATT_POLICY_TYPE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPPOLATT_POLICY_NOTE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime);
                    obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit);


                    foreach (cls_TREmppolatt model in list_model)
                    {

                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                        obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                        obj_cmd.Parameters["@EMPPOLATT_POLICY_CODE"].Value = model.emppolatt_policy_code;
                        obj_cmd.Parameters["@EMPPOLATT_POLICY_TYPE"].Value = model.emppolatt_policy_type;
                        obj_cmd.Parameters["@EMPPOLATT_POLICY_NOTE"].Value = model.emppolatt_policy_note;
                        obj_cmd.Parameters["@CREATED_BY"].Value = model.created_by;
                        obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                        obj_cmd.Parameters["@FLAG"].Value = false;

                        obj_cmd.ExecuteNonQuery();

                    }

                    blnResult = obj_conn.doCommit();

                    if (!blnResult)
                        obj_conn.doRollback();
                    obj_conn.doClose();


                }
                else
                {
                    obj_conn.doRollback();
                    obj_conn.doClose();
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Emppolatt.insert)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
