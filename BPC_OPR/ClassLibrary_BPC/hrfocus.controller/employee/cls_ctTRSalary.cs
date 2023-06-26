using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRSalary
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRSalary() { }

        public string getMessage() { return this.Message.Replace("EMP_TR_SALARY", "").Replace("cls_ctTRSalary", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRSalary> getData(string condition)
        {
            List<cls_TRSalary> list_model = new List<cls_TRSalary>();
            cls_TRSalary model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");
                obj_str.Append(", EMPSALARY_ID");

                obj_str.Append(", EMPSALARY_AMOUNT");
                obj_str.Append(", EMPSALARY_DATE");
                obj_str.Append(", EMPSALARY_REASON");

                obj_str.Append(", ISNULL(EMPSALARY_INCAMOUNT, 0) AS EMPSALARY_INCAMOUNT");
                obj_str.Append(", ISNULL(EMPSALARY_INCPERCENT, 0) AS EMPSALARY_INCPERCENT");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_TR_SALARY");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRSalary();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.empsalary_id = Convert.ToInt32(dr["EMPSALARY_ID"]);

                    model.empsalary_amount = Convert.ToDouble(dr["EMPSALARY_AMOUNT"]);
                    model.empsalary_date = Convert.ToDateTime(dr["EMPSALARY_DATE"]);
                    model.empsalary_reason = dr["EMPSALARY_REASON"].ToString();

                    model.empsalary_incamount = Convert.ToDouble(dr["EMPSALARY_INCAMOUNT"]);
                    model.empsalary_incpercent = Convert.ToDouble(dr["EMPSALARY_INCPERCENT"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "EMPSLR001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRSalary> getDataByFillter(string com, string emp , string date)
        {
            string strCondition = "";

            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!emp.Equals(""))
                strCondition += " AND WORKER_CODE='" + emp + "'";

            if (!date.Equals(""))
                strCondition += " AND EMPSALARY_DATE='" + Convert.ToDateTime(date) + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(EMPSALARY_ID, 1) ");
                obj_str.Append(" FROM EMP_TR_SALARY");
                obj_str.Append(" ORDER BY EMPSALARY_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPSLR002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT EMPSALARY_ID");
                obj_str.Append(" FROM EMP_TR_SALARY");
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
                Message = "EMPSLR003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string emp)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM EMP_TR_SALARY");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "EMPSLR004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRSalary model)
        {
            bool blnResult = false;
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.company_code, model.worker_code))
                {
                        return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_TR_SALARY");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");

                obj_str.Append(", EMPSALARY_ID ");
                obj_str.Append(", EMPSALARY_AMOUNT ");
                obj_str.Append(", EMPSALARY_DATE ");
                obj_str.Append(", EMPSALARY_REASON ");
                obj_str.Append(", EMPSALARY_INCAMOUNT ");
                obj_str.Append(", EMPSALARY_INCPERCENT ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");

                obj_str.Append(", @EMPSALARY_ID ");
                obj_str.Append(", @EMPSALARY_AMOUNT ");
                obj_str.Append(", @EMPSALARY_DATE ");
                obj_str.Append(", @EMPSALARY_REASON ");
                obj_str.Append(", @EMPSALARY_INCAMOUNT ");
                obj_str.Append(", @EMPSALARY_INCPERCENT ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.empsalary_id = this.getNextID();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@EMPSALARY_ID", SqlDbType.Int); obj_cmd.Parameters["@EMPSALARY_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@EMPSALARY_AMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@EMPSALARY_AMOUNT"].Value = model.empsalary_amount;
                obj_cmd.Parameters.Add("@EMPSALARY_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPSALARY_DATE"].Value = model.empsalary_date;
                obj_cmd.Parameters.Add("@EMPSALARY_REASON", SqlDbType.VarChar); obj_cmd.Parameters["@EMPSALARY_REASON"].Value = model.empsalary_reason;

                obj_cmd.Parameters.Add("@EMPSALARY_INCAMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@EMPSALARY_INCAMOUNT"].Value = model.empsalary_incamount;
                obj_cmd.Parameters.Add("@EMPSALARY_INCPERCENT", SqlDbType.Decimal); obj_cmd.Parameters["@EMPSALARY_INCPERCENT"].Value = model.empsalary_incpercent;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                strResult = model.empsalary_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "EMPSLR005:" + ex.ToString();
                strResult = "";
            }

            return blnResult;
        }

        public bool update(cls_TRSalary model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE EMP_TR_SALARY SET ");

                obj_str.Append(" EMPSALARY_AMOUNT=@EMPSALARY_AMOUNT ");
                obj_str.Append(", EMPSALARY_REASON=@EMPSALARY_REASON ");

                obj_str.Append(", EMPSALARY_INCAMOUNT=@EMPSALARY_INCAMOUNT ");
                obj_str.Append(", EMPSALARY_INCPERCENT=@EMPSALARY_INCPERCENT ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE "); ;

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" AND EMPSALARY_ID=@EMPSALARY_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@EMPSALARY_AMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@EMPSALARY_AMOUNT"].Value = model.empsalary_amount;
                obj_cmd.Parameters.Add("@EMPSALARY_REASON", SqlDbType.VarChar); obj_cmd.Parameters["@EMPSALARY_REASON"].Value = model.empsalary_reason;

                obj_cmd.Parameters.Add("@EMPSALARY_INCAMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@EMPSALARY_INCAMOUNT"].Value = model.empsalary_incamount;
                obj_cmd.Parameters.Add("@EMPSALARY_INCPERCENT", SqlDbType.Decimal); obj_cmd.Parameters["@EMPSALARY_INCPERCENT"].Value = model.empsalary_incpercent;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@EMPSALARY_ID", SqlDbType.Int); obj_cmd.Parameters["@EMPSALARY_ID"].Value = model.empsalary_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "EMPSLR006:" + ex.ToString();
            }

            return blnResult;
        }

        public List<cls_TRSalary> getDataBatch(string com, DateTime date, double amount)
        {
            List<cls_TRSalary> list_model = new List<cls_TRSalary>();
            cls_TRSalary model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("EMP_TR_SALARY.COMPANY_CODE");
                obj_str.Append(", EMP_TR_SALARY.WORKER_CODE");
                obj_str.Append(", EMP_TR_SALARY.EMPSALARY_ID");

                obj_str.Append(", EMP_TR_SALARY.EMPSALARY_AMOUNT");
                obj_str.Append(", EMP_TR_SALARY.EMPSALARY_DATE");
                obj_str.Append(", EMP_TR_SALARY.EMPSALARY_REASON");

                obj_str.Append(", INITIAL_NAME_TH + WORKER_FNAME_TH + ' ' + WORKER_LNAME_TH AS WORKER_DETAIL_TH");
                obj_str.Append(", INITIAL_NAME_EN + WORKER_FNAME_EN + ' ' + WORKER_LNAME_EN AS WORKER_DETAIL_EN");

                obj_str.Append(", ISNULL(EMP_TR_SALARY.MODIFIED_BY, EMP_TR_SALARY.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(EMP_TR_SALARY.MODIFIED_DATE, EMP_TR_SALARY.CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_TR_SALARY");
                obj_str.Append(" INNER JOIN EMP_MT_WORKER ON EMP_MT_WORKER.COMPANY_CODE=EMP_TR_SALARY.COMPANY_CODE AND EMP_MT_WORKER.WORKER_CODE=EMP_TR_SALARY.WORKER_CODE");
                obj_str.Append(" INNER JOIN EMP_MT_INITIAL ON EMP_MT_INITIAL.INITIAL_CODE=EMP_MT_WORKER.WORKER_INITIAL ");
                obj_str.Append(" WHERE 1=1");
                obj_str.Append(" AND EMP_TR_SALARY.COMPANY_CODE='" + com + "' ");

                if (!date.Equals(""))
                    obj_str.Append(" AND EMP_TR_SALARY.EMPSALARY_DATE='" + date.ToString("yyyy-MM-ddTHH:mm:ss") + "' ");
                if (!amount.Equals(""))
                    obj_str.Append(" AND EMP_TR_SALARY.EMPSALARY_AMOUNT='" + amount + "' ");

                obj_str.Append(" ORDER BY EMP_TR_SALARY.WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRSalary();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.empsalary_id = Convert.ToInt32(dr["EMPSALARY_ID"]);

                    model.empsalary_amount = Convert.ToDouble(dr["EMPSALARY_AMOUNT"]);
                    model.empsalary_date = Convert.ToDateTime(dr["EMPSALARY_DATE"]);
                    model.empsalary_reason = dr["EMPSALARY_REASON"].ToString();

                    model.worker_detail_th = dr["WORKER_DETAIL_TH"].ToString();
                    model.worker_detail_en = dr["WORKER_DETAIL_EN"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "EMPSLR007:" + ex.ToString();
            }

            return list_model;
        }

        public bool insertlist(List<cls_TRSalary> list_model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_TR_SALARY");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");

                obj_str.Append(", EMPSALARY_ID ");
                obj_str.Append(", EMPSALARY_AMOUNT ");
                obj_str.Append(", EMPSALARY_DATE ");
                obj_str.Append(", EMPSALARY_REASON ");
                obj_str.Append(", EMPSALARY_INCAMOUNT ");
                obj_str.Append(", EMPSALARY_INCPERCENT ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");

                obj_str.Append(", @EMPSALARY_ID ");
                obj_str.Append(", @EMPSALARY_AMOUNT ");
                obj_str.Append(", @EMPSALARY_DATE ");
                obj_str.Append(", @EMPSALARY_REASON ");
                obj_str.Append(", @EMPSALARY_INCAMOUNT ");
                obj_str.Append(", @EMPSALARY_INCPERCENT ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                //-- Step 1 delete data old
                string strWorkerID = "";
                foreach (cls_TRSalary model in list_model)
                {
                    strWorkerID += "'" + model.worker_code + "',";
                }
                if (strWorkerID.Length > 0)
                    strWorkerID = strWorkerID.Substring(0, strWorkerID.Length - 1);
                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM EMP_TR_SALARY");
                obj_str2.Append(" WHERE 1=1 ");
                obj_str2.Append(" AND COMPANY_CODE='" + list_model[0].company_code + "'");
                obj_str2.Append(" AND WORKER_CODE IN (" + strWorkerID + ")");
                obj_str2.Append(" AND EMPSALARY_DATE='" + list_model[0].empsalary_date.ToString("yyyy-MM-ddTHH:mm:ss") + "'");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());

                if (blnResult)
                {
                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPSALARY_ID", SqlDbType.Int);
                    obj_cmd.Parameters.Add("@EMPSALARY_AMOUNT", SqlDbType.Decimal);
                    obj_cmd.Parameters.Add("@EMPSALARY_DATE", SqlDbType.DateTime);
                    obj_cmd.Parameters.Add("@EMPSALARY_REASON", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPSALARY_INCAMOUNT", SqlDbType.Decimal);
                    obj_cmd.Parameters.Add("@EMPSALARY_INCPERCENT", SqlDbType.Decimal); 
                    obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime);

                    foreach (cls_TRSalary model in list_model)
                    {
                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                        obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                        obj_cmd.Parameters["@EMPSALARY_ID"].Value = this.getNextID();
                        obj_cmd.Parameters["@EMPSALARY_AMOUNT"].Value = model.empsalary_amount;
                        obj_cmd.Parameters["@EMPSALARY_DATE"].Value = model.empsalary_date;
                        obj_cmd.Parameters["@EMPSALARY_REASON"].Value = model.empsalary_reason;
                        obj_cmd.Parameters["@EMPSALARY_INCAMOUNT"].Value = model.empsalary_incamount;
                        obj_cmd.Parameters["@EMPSALARY_INCPERCENT"].Value = model.empsalary_incpercent;
                        obj_cmd.Parameters["@CREATED_BY"].Value = model.created_by;
                        obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

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
                Message = "EMPSLR099:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
