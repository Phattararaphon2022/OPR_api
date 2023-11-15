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
    public class cls_ctTREmpresign
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTREmpresign() { }

        public string getMessage() { return this.Message.Replace("EMP_TR_RESIGN", "").Replace("cls_ctTREmpresign", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TREmpresign> getData(string condition)
        {
            List<cls_TREmpresign> list_model = new List<cls_TREmpresign>();
            cls_TREmpresign model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");
                obj_str.Append(", EMPRESIGN_DATE");
                obj_str.Append(", EMPRESIGN_ID");
                obj_str.Append(", CARD_NO ");
                obj_str.Append(", ISNULL(REASON_CODE, '') AS REASON_CODE");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_TR_RESIGN");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TREmpresign();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();

                    model.card_no = dr["CARD_NO"].ToString();
                    model.empresign_id = Convert.ToInt32(dr["EMPRESIGN_ID"]);
                    model.empresign_date = Convert.ToDateTime(dr["EMPRESIGN_DATE"]);
                    model.reason_code = dr["REASON_CODE"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "EMPRES001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TREmpresign> getDataByFillter(string com, string emp, string date,string card)
        {
            string strCondition = "";

            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!emp.Equals(""))
                strCondition += " AND WORKER_CODE='" + emp + "'";

            if (!date.Equals(""))
                strCondition += " AND EMPRESIGN_DATE='" + date + "'";

            if (!card.Equals(""))
                strCondition += " AND CARD_NO='" + card + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(EMPRESIGN_ID, 1) ");
                obj_str.Append(" FROM EMP_TR_RESIGN");
                obj_str.Append(" ORDER BY EMPRESIGN_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPRES002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp, string card)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT EMPRESIGN_ID");
                obj_str.Append(" FROM EMP_TR_RESIGN");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND CARD_NO='" + card + "' ");
                if (!emp.Equals(""))
                {
                    obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                }


                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPRES003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM EMP_TR_RESIGN");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");


                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "EMPPAY004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_TREmpresign model)
        {
            bool blnResult = false;
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.company_code, model.worker_code, model.card_no))
                {

                    if (this.update(model))
                        return model.card_no;
                    else
                        return "";
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_TR_RESIGN");
                obj_str.Append(" (");
                obj_str.Append("EMPRESIGN_ID ");
                obj_str.Append(", CARD_NO ");
                obj_str.Append(", EMPRESIGN_DATE ");
                obj_str.Append(", REASON_CODE ");

                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@EMPRESIGN_ID ");
                obj_str.Append(", @CARD_NO ");
                obj_str.Append(", @EMPRESIGN_DATE ");
                obj_str.Append(", @REASON_CODE ");

                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.empresign_id = this.getNextID();

                obj_cmd.Parameters.Add("@CARD_NO", SqlDbType.VarChar); obj_cmd.Parameters["@CARD_NO"].Value = model.card_no;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@EMPRESIGN_ID", SqlDbType.Int); obj_cmd.Parameters["@EMPRESIGN_ID"].Value = model.empresign_id;
                obj_cmd.Parameters.Add("@EMPRESIGN_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPRESIGN_DATE"].Value = model.empresign_date;
                obj_cmd.Parameters.Add("@REASON_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@REASON_CODE"].Value = model.reason_code;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                strResult = model.empresign_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "EMPRES005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_TREmpresign model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE EMP_TR_RESIGN SET ");

                obj_str.Append(" CARD_NO=@CARD_NO ");
                obj_str.Append(", EMPRESIGN_DATE=@EMPRESIGN_DATE ");
                obj_str.Append(", REASON_CODE=@REASON_CODE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE "); ;

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());



                obj_cmd.Parameters.Add("@CARD_NO", SqlDbType.VarChar); obj_cmd.Parameters["@CARD_NO"].Value = model.card_no;
                obj_cmd.Parameters.Add("@EMPRESIGN_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPRESIGN_DATE"].Value = model.empresign_date;
                obj_cmd.Parameters.Add("@REASON_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@REASON_CODE"].Value = model.reason_code;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "EMPRES006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
