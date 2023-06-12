using ClassLibrary_BPC.hrfocus.model.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller.Payroll
{
    public class cls_ctTRPolProvident
   {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRPolProvident() { }

        public string getMessage() { return this.Message.Replace("PAY_TR_PAYPOLPROVIDENT", "").Replace("cls_ctTRProvident", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }
        private List<cls_TRPolProvident> getData(string language, string condition)
        {
            List<cls_TRPolProvident> list_model = new List<cls_TRPolProvident>();
            cls_TRPolProvident model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");
                obj_str.Append("PAY_TR_PAYPOLPROVIDENT.COMPANY_CODE");
                obj_str.Append(", PAY_TR_PAYPOLPROVIDENT.WORKER_CODE");
                obj_str.Append(", PAY_TR_PAYPOLPROVIDENT.PAYPOLPROVIDENT_CODE");
                obj_str.Append(", PAY_TR_PAYPOLPROVIDENT.CREATED_BY");
                obj_str.Append(", PAY_TR_PAYPOLPROVIDENT.CREATED_DATE");

                if (language.Equals("TH"))
                {
                    obj_str.Append(", INITIAL_NAME_TH + WORKER_FNAME_TH + ' ' + WORKER_LNAME_TH AS WORKER_DETAIL");
                }
                else
                {
                    obj_str.Append(", INITIAL_NAME_EN + WORKER_FNAME_EN + ' ' + WORKER_LNAME_EN AS WORKER_DETAIL");
                }

                obj_str.Append(" FROM PAY_TR_PAYPOLPROVIDENT");
                obj_str.Append(" INNER JOIN EMP_MT_WORKER ON EMP_MT_WORKER.COMPANY_CODE=PAY_TR_PAYPOLPROVIDENT.COMPANY_CODE AND EMP_MT_WORKER.WORKER_CODE=PAY_TR_PAYPOLPROVIDENT.WORKER_CODE");
                obj_str.Append(" INNER JOIN EMP_MT_INITIAL ON EMP_MT_INITIAL.INITIAL_CODE=EMP_MT_WORKER.WORKER_INITIAL ");

                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PAY_TR_PAYPOLPROVIDENT.COMPANY_CODE, PAY_TR_PAYPOLPROVIDENT.WORKER_CODE, PAY_TR_PAYPOLPROVIDENT.CREATED_DATE DESC");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRPolProvident();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.paypolprovident_code = dr["PAYPOLPROVIDENT_CODE"].ToString();
                    model.worker_detail = dr["WORKER_DETAIL"].ToString();

                    model.created_by = dr["CREATED_BY"].ToString();
                    model.created_date = Convert.ToDateTime(dr["CREATED_DATE"]);
                  list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "PAYTRPP001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRPolProvident> getDataByFillter(string language, string access_emp, string com, string provident)
        {
            string strCondition = " AND PAY_TR_PAYPOLPROVIDENT.COMPANY_CODE='" + com + "'";

            if (!provident.Equals(""))
                strCondition += " AND PAY_TR_PAYPOLPROVIDENT.PAYPOLPROVIDENT_CODE='" + provident + "'";
            return this.getData(language, strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" FROM PAY_TR_PAYPOLPROVIDENT");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "PAYTRPP002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" FROM PAY_TR_PAYPOLPROVIDENT");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "PAYTRPP003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string emp, string provident)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM PAY_TR_PAYPOLPROVIDENT");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND WORKER_CODE='" + emp + "'");
                obj_str.Append(" AND PAYPOLPROVIDENT_CODE='" + provident + "'");
                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "PAYTRPP004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRPolProvident model)

        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.worker_code))
                {
                    return this.update(model);
                }
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PAY_TR_PAYPOLPROVIDENT");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", PAYPOLPROVIDENT_CODE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @PAYPOLPROVIDENT_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");                


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@PAYPOLPROVIDENT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PAYPOLPROVIDENT_CODE"].Value = model.paypolprovident_code;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.created_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "PAYTRPP005:" + ex.ToString();
            }

            return blnResult;
        }

        public bool update(cls_TRPolProvident model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PAY_TR_PAYPOLPROVIDENT SET ");

                obj_str.Append(" PAYPOLPROVIDENT_CODE=@PAYPOLPROVIDENT_CODE ");
            
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE "); ;

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PAYPOLPROVIDENT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PAYPOLPROVIDENT_CODE"].Value = model.paypolprovident_code;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "PAYTRPP006:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insertlist(string com, string provident, List<cls_TRPolProvident> list_model) 
        {
            bool blnResult = false;
            try
            {

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PAY_TR_PAYPOLPROVIDENT");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", PAYPOLPROVIDENT_CODE ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @PAYPOLPROVIDENT_CODE ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                //-- Step 1 delete data old
                string strWorkerID = "";
                foreach (cls_TRPolProvident model in list_model)
                {
                    strWorkerID += "'" + model.worker_code + "',";
                }
                if (strWorkerID.Length > 0)
                    strWorkerID = strWorkerID.Substring(0, strWorkerID.Length - 1);

                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM PAY_TR_PAYPOLPROVIDENT");
                obj_str2.Append(" WHERE 1=1 ");
                obj_str2.Append(" AND COMPANY_CODE='" + list_model[0].company_code + "'");
                obj_str2.Append(" AND WORKER_CODE IN (" + strWorkerID + ")");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());

                if (blnResult)
                {

                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@PAYPOLPROVIDENT_CODE", SqlDbType.VarChar);

                    obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime);
                    obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit);

                    foreach (cls_TRPolProvident model in list_model)
                    {

                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                        obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                        obj_cmd.Parameters["@PAYPOLPROVIDENT_CODE"].Value = model.paypolprovident_code;

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
                Message = "PAYTRPP007:" + ex.ToString();
            }

            return blnResult;
        }
   }
}
