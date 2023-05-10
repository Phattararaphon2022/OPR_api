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
    public class cls_ctTRSupply
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRSupply() { }

        public string getMessage() { return this.Message.Replace("EMP_TR_SUPPLY", "").Replace("cls_ctTRSupply", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }
        private List<cls_TRSupply> getData(string condition)
        {
            List<cls_TRSupply> list_model = new List<cls_TRSupply>();
            cls_TRSupply model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");
                obj_str.Append(", EMPSUPPLY_ID");
                obj_str.Append(", EMPSUPPLY_CODE");
                obj_str.Append(", EMPSUPPLY_QUANTITY");
                obj_str.Append(", ISNULL(EMPSUPPLY_ISSUEDATE, '') AS EMPSUPPLY_ISSUEDATE");
                obj_str.Append(", EMPSUPPLY_NOTE");
                obj_str.Append(", ISNULL(EMPSUPPLY_RETURNDATE,'') AS EMPSUPPLY_RETURNDATE");
                obj_str.Append(", EMPSUPPLY_RETURNSTATUS");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_TR_SUPPLY");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRSupply();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.empsupply_id = Convert.ToInt32(dr["EMPSUPPLY_ID"]);
                    model.empsupply_qauntity = Convert.ToDouble(dr["EMPSUPPLY_QUANTITY"]);
                    model.empsupply_code = dr["EMPSUPPLY_CODE"].ToString();
                    model.empsupply_issuedate = Convert.ToDateTime(dr["EMPSUPPLY_ISSUEDATE"]);
                    model.empsupply_note = dr["EMPSUPPLY_NOTE"].ToString();

                    model.empsupply_returndate = Convert.ToDateTime(dr["EMPSUPPLY_RETURNDATE"]);
                    model.empsupply_returnstatus = Convert.ToBoolean(dr["EMPSUPPLY_RETURNSTATUS"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "EMPSUP001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRSupply> getDataByFillter(string com, string emp)
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

                obj_str.Append("SELECT ISNULL(EMPSUPPLY_ID, 1) ");
                obj_str.Append(" FROM EMP_TR_SUPPLY");
                obj_str.Append(" ORDER BY EMPSUPPLY_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPSUP002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT EMPSUPPLY_ID");
                obj_str.Append(" FROM EMP_TR_SUPPLY");
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
                Message = "EMPSUP003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM EMP_TR_SUPPLY");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "EMPSUP004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRSupply model)
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

                obj_str.Append("INSERT INTO EMP_TR_SUPPLY");
                obj_str.Append(" (");
                obj_str.Append("EMPSUPPLY_ID ");
                obj_str.Append(", EMPSUPPLY_CODE ");
                obj_str.Append(", EMPSUPPLY_QUANTITY ");
                obj_str.Append(", EMPSUPPLY_ISSUEDATE ");
                obj_str.Append(", EMPSUPPLY_NOTE ");
                if (model.empsupply_returndate.Equals(""))
                {
                    obj_str.Append(", EMPSUPPLY_RETURNDATE ");
                }
                obj_str.Append(", EMPSUPPLY_RETURNSTATUS ");
                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@EMPSUPPLY_ID ");
                obj_str.Append(", @EMPSUPPLY_CODE ");
                obj_str.Append(", @EMPSUPPLY_QUANTITY ");
                obj_str.Append(", @EMPSUPPLY_ISSUEDATE ");
                obj_str.Append(", @EMPSUPPLY_NOTE ");
                if (model.empsupply_returndate.Equals(""))
                {
                    obj_str.Append(", @EMPSUPPLY_RETURNDATE ");
                }
                obj_str.Append(", @EMPSUPPLY_RETURNSTATUS ");
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

                obj_cmd.Parameters.Add("@EMPSUPPLY_ID", SqlDbType.Int); obj_cmd.Parameters["@EMPSUPPLY_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@EMPSUPPLY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@EMPSUPPLY_CODE"].Value = model.empsupply_code;
                obj_cmd.Parameters.Add("@EMPSUPPLY_QUANTITY", SqlDbType.Decimal); obj_cmd.Parameters["@EMPSUPPLY_QUANTITY"].Value = model.empsupply_qauntity;
                obj_cmd.Parameters.Add("@EMPSUPPLY_ISSUEDATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPSUPPLY_ISSUEDATE"].Value = model.empsupply_issuedate;
                obj_cmd.Parameters.Add("@EMPSUPPLY_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@EMPSUPPLY_NOTE"].Value = model.empsupply_note;
                if (model.empsupply_returndate.Equals(""))
                {
                    obj_cmd.Parameters.Add("@EMPSUPPLY_RETURNDATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPSUPPLY_RETURNDATE"].Value = model.empsupply_returndate;
                }
                obj_cmd.Parameters.Add("@EMPSUPPLY_RETURNSTATUS", SqlDbType.Bit); obj_cmd.Parameters["@EMPSUPPLY_RETURNSTATUS"].Value = model.empsupply_returnstatus;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                strResult = model.empsupply_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "EMPSUP005:" + ex.ToString();
                strResult = "";
            }

            return blnResult;
        }

        public bool update(cls_TRSupply model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE EMP_TR_SUPPLY SET ");

                obj_str.Append(" EMPSUPPLY_CODE=@EMPSUPPLY_CODE ");
                obj_str.Append(", EMPSUPPLY_QUANTITY=@EMPSUPPLY_QUANTITY ");
                obj_str.Append(", EMPSUPPLY_ISSUEDATE=@EMPSUPPLY_ISSUEDATE ");
                obj_str.Append(", EMPSUPPLY_NOTE=@EMPSUPPLY_NOTE ");
                obj_str.Append(", EMPSUPPLY_RETURNDATE=@EMPSUPPLY_RETURNDATE ");
                obj_str.Append(", EMPSUPPLY_RETURNSTATUS=@EMPSUPPLY_RETURNSTATUS ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE "); ;

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" AND EMPSUPPLY_ID=@EMPSUPPLY_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@EMPSUPPLY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@EMPSUPPLY_CODE"].Value = model.empsupply_code;
                obj_cmd.Parameters.Add("@EMPSUPPLY_QUANTITY", SqlDbType.Decimal); obj_cmd.Parameters["@EMPSUPPLY_QUANTITY"].Value = model.empsupply_qauntity;
                obj_cmd.Parameters.Add("@EMPSUPPLY_ISSUEDATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPSUPPLY_ISSUEDATE"].Value = model.empsupply_issuedate;
                obj_cmd.Parameters.Add("@EMPSUPPLY_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@EMPSUPPLY_NOTE"].Value = model.empsupply_note;
                obj_cmd.Parameters.Add("@EMPSUPPLY_RETURNDATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPSUPPLY_RETURNDATE"].Value = model.empsupply_returndate;
                obj_cmd.Parameters.Add("@EMPSUPPLY_RETURNSTATUS", SqlDbType.Bit); obj_cmd.Parameters["@EMPSUPPLY_RETURNSTATUS"].Value = model.empsupply_returnstatus;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@EMPSUPPLY_ID", SqlDbType.Int); obj_cmd.Parameters["@EMPSUPPLY_ID"].Value = model.empsupply_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "EMPSUP006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}
