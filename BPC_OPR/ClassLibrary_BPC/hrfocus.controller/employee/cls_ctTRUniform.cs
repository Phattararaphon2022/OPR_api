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
    public class cls_ctTRUniform
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRUniform() { }

        public string getMessage() { return this.Message.Replace("EMP_TR_UNIFORM", "").Replace("cls_ctTRUniform", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }
        private List<cls_TRUniform> getData(string condition)
        {
            List<cls_TRUniform> list_model = new List<cls_TRUniform>();
            cls_TRUniform model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");
                obj_str.Append(", EMPUNIFORM_ID");
                obj_str.Append(", EMPUNIFORM_CODE");
                obj_str.Append(", EMPUNIFORM_QUANTITY");
                obj_str.Append(", EMPUNIFORM_AMOUNT");
                obj_str.Append(", ISNULL(EMPUNIFORM_ISSUEDATE, '') AS EMPUNIFORM_ISSUEDATE");
                obj_str.Append(", EMPUNIFORM_NOTE");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_TR_UNIFORM");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRUniform();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.empuniform_id = Convert.ToInt32(dr["EMPUNIFORM_ID"]);
                    model.empuniform_code = dr["EMPUNIFORM_CODE"].ToString();
                    model.empuniform_qauntity = Convert.ToDouble(dr["EMPUNIFORM_QUANTITY"]);
                    model.empuniform_amount = Convert.ToDouble(dr["EMPUNIFORM_AMOUNT"]);
                    model.empuniform_issuedate = Convert.ToDateTime(dr["EMPUNIFORM_ISSUEDATE"]);
                    model.empuniform_note = dr["EMPUNIFORM_NOTE"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "EMPUNF001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRUniform> getDataByFillter(string com, string emp)
        {
            string strCondition = "";

            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!emp.Equals(""))
                strCondition += " AND WORKER_CODE='" + emp + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(EMPUNIFORM_ID, 1) ");
                obj_str.Append(" FROM EMP_TR_UNIFORM");
                obj_str.Append(" ORDER BY EMPUNIFORM_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPUNF002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT EMPUNIFORM_ID");
                obj_str.Append(" FROM EMP_TR_UNIFORM");
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
                Message = "EMPUNF003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM EMP_TR_UNIFORM");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "EMPUNF004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRUniform model)
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

                obj_str.Append("INSERT INTO EMP_TR_UNIFORM");
                obj_str.Append(" (");
                obj_str.Append("EMPUNIFORM_ID ");
                obj_str.Append(", EMPUNIFORM_CODE ");
                obj_str.Append(", EMPUNIFORM_QUANTITY ");
                obj_str.Append(", EMPUNIFORM_AMOUNT ");
                obj_str.Append(", EMPUNIFORM_ISSUEDATE ");
                obj_str.Append(", EMPUNIFORM_NOTE ");
                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@EMPUNIFORM_ID ");
                obj_str.Append(", @EMPUNIFORM_CODE ");
                obj_str.Append(", @EMPUNIFORM_QUANTITY ");
                obj_str.Append(", @EMPUNIFORM_AMOUNT ");
                obj_str.Append(", @EMPUNIFORM_ISSUEDATE ");
                obj_str.Append(", @EMPUNIFORM_NOTE ");
                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());


                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@EMPUNIFORM_ID", SqlDbType.Int); obj_cmd.Parameters["@EMPUNIFORM_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@EMPUNIFORM_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@EMPUNIFORM_CODE"].Value = model.empuniform_code;
                obj_cmd.Parameters.Add("@EMPUNIFORM_QUANTITY", SqlDbType.Decimal); obj_cmd.Parameters["@EMPUNIFORM_QUANTITY"].Value = model.empuniform_qauntity;
                obj_cmd.Parameters.Add("@EMPUNIFORM_AMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@EMPUNIFORM_AMOUNT"].Value = model.empuniform_amount;
                obj_cmd.Parameters.Add("@EMPUNIFORM_ISSUEDATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPUNIFORM_ISSUEDATE"].Value = model.empuniform_issuedate;
                obj_cmd.Parameters.Add("@EMPUNIFORM_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@EMPUNIFORM_NOTE"].Value = model.empuniform_note;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                strResult = model.empuniform_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "EMPSUP005:" + ex.ToString();
                strResult = "";
            }

            return blnResult;
        }

        public bool update(cls_TRUniform model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE EMP_TR_UNIFORM SET ");

                obj_str.Append(" EMPUNIFORM_CODE=@EMPUNIFORM_CODE ");
                obj_str.Append(", EMPUNIFORM_QUANTITY=@EMPUNIFORM_QUANTITY ");
                obj_str.Append(", EMPUNIFORM_AMOUNT=@EMPUNIFORM_AMOUNT ");
                obj_str.Append(", EMPUNIFORM_ISSUEDATE=@EMPUNIFORM_ISSUEDATE ");
                obj_str.Append(", EMPUNIFORM_NOTE=@EMPUNIFORM_NOTE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE "); ;

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" AND EMPUNIFORM_ID=@EMPUNIFORM_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@EMPUNIFORM_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@EMPUNIFORM_CODE"].Value = model.empuniform_code;
                obj_cmd.Parameters.Add("@EMPUNIFORM_QUANTITY", SqlDbType.Decimal); obj_cmd.Parameters["@EMPUNIFORM_QUANTITY"].Value = model.empuniform_qauntity;
                obj_cmd.Parameters.Add("@EMPUNIFORM_AMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@EMPUNIFORM_AMOUNT"].Value = model.empuniform_amount;
                obj_cmd.Parameters.Add("@EMPUNIFORM_ISSUEDATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPUNIFORM_ISSUEDATE"].Value = model.empuniform_issuedate;
                obj_cmd.Parameters.Add("@EMPUNIFORM_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@EMPUNIFORM_NOTE"].Value = model.empuniform_note;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@EMPUNIFORM_ID", SqlDbType.Int); obj_cmd.Parameters["@EMPUNIFORM_ID"].Value = model.empuniform_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "EMPUNF006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
