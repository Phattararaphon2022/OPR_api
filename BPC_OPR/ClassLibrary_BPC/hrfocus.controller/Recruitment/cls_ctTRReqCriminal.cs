using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus
{
    public class cls_ctTRReqCriminal
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRReqCriminal() { }

        public string getMessage() { return this.Message.Replace("REQ_TR_CRIMINAL", "").Replace("cls_ctTRReqCriminal", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRReqCriminal> getData(string condition)
        {
            List<cls_TRReqCriminal> list_model = new List<cls_TRReqCriminal>();
            cls_TRReqCriminal model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", APPLYWORK_CODE");

                obj_str.Append(", REQCRIMINAL_ID");
                obj_str.Append(", REQCRIMINAL_LOCATION");
                obj_str.Append(", REQCRIMINAL_FROMDATE");
                obj_str.Append(", REQCRIMINAL_TODATE");
                obj_str.Append(", REQCRIMINAL_COUNT");
                obj_str.Append(", REQCRIMINAL_RESULT");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM REQ_TR_CRIMINAL");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, APPLYWORK_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRReqCriminal();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.applywork_code = dr["APPLYWORK_CODE"].ToString();

                    model.reqcriminal_id = Convert.ToInt32(dr["REQCRIMINAL_ID"]);
                    model.reqcriminal_location = dr["REQCRIMINAL_LOCATION"].ToString();
                    model.reqcriminal_fromdate = Convert.ToDateTime(dr["REQCRIMINAL_FROMDATE"]);
                    model.reqcriminal_todate = Convert.ToDateTime(dr["REQCRIMINAL_TODATE"]);
                    model.reqcriminal_count = Convert.ToDouble(dr["REQCRIMINAL_COUNT"]);
                    model.reqcriminal_result = dr["REQCRIMINAL_RESULT"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "REQCRM001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRReqCriminal> getDataByFillter(string com, string emp)
        {
            string strCondition = "";

            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!emp.Equals(""))
                strCondition += " AND APPLYWORK_CODE='" + emp + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(REQCRIMINAL_ID, 1) ");
                obj_str.Append(" FROM REQ_TR_CRIMINAL");
                obj_str.Append(" ORDER BY REQCRIMINAL_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "REQCRM002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT REQCRIMINAL_ID");
                obj_str.Append(" FROM REQ_TR_CRIMINAL");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND APPLYWORK_CODE='" + emp + "' ");
                //obj_str.Append(" AND EMPCRIMINAL_ID='" + id + "' ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "REQCRM003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM REQ_TR_CRIMINAL");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND APPLYWORK_CODE='" + emp + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "REQCRM004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRReqCriminal model)
        {
            bool blnResult = false;
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.company_code, model.applywork_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO REQ_TR_CRIMINAL");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", APPLYWORK_CODE ");

                obj_str.Append(", REQCRIMINAL_ID ");
                obj_str.Append(", REQCRIMINAL_LOCATION ");
                obj_str.Append(", REQCRIMINAL_FROMDATE ");
                obj_str.Append(", REQCRIMINAL_TODATE ");
                obj_str.Append(", REQCRIMINAL_COUNT ");
                obj_str.Append(", REQCRIMINAL_RESULT ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @APPLYWORK_CODE ");
                obj_str.Append(", @REQCRIMINAL_ID ");
                obj_str.Append(", @REQCRIMINAL_LOCATION ");
                obj_str.Append(", @REQCRIMINAL_FROMDATE ");
                obj_str.Append(", @REQCRIMINAL_TODATE ");
                obj_str.Append(", @REQCRIMINAL_COUNT ");
                obj_str.Append(", @REQCRIMINAL_RESULT ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.reqcriminal_id = this.getNextID();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@APPLYWORK_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYWORK_CODE"].Value = model.applywork_code;

                obj_cmd.Parameters.Add("@REQCRIMINAL_ID", SqlDbType.Int); obj_cmd.Parameters["@REQCRIMINAL_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@REQCRIMINAL_LOCATION", SqlDbType.VarChar); obj_cmd.Parameters["@REQCRIMINAL_LOCATION"].Value = model.reqcriminal_location;
                obj_cmd.Parameters.Add("@REQCRIMINAL_FROMDATE", SqlDbType.DateTime); obj_cmd.Parameters["@REQCRIMINAL_FROMDATE"].Value = model.reqcriminal_fromdate;
                obj_cmd.Parameters.Add("@REQCRIMINAL_TODATE", SqlDbType.DateTime); obj_cmd.Parameters["@REQCRIMINAL_TODATE"].Value = model.reqcriminal_todate;
                obj_cmd.Parameters.Add("@REQCRIMINAL_COUNT", SqlDbType.Decimal); obj_cmd.Parameters["@REQCRIMINAL_COUNT"].Value = model.reqcriminal_count;
                obj_cmd.Parameters.Add("@REQCRIMINAL_RESULT", SqlDbType.VarChar); obj_cmd.Parameters["@REQCRIMINAL_RESULT"].Value = model.reqcriminal_result;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                strResult = model.reqcriminal_id.ToString();
                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "REQCRM005:" + ex.ToString();
                blnResult = false;
                strResult = "";
            }

            return blnResult;
        }

        public bool update(cls_TRReqCriminal model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE REQ_TR_CRIMINAL SET ");

                obj_str.Append(" REQCRIMINAL_LOCATION=@REQCRIMINAL_LOCATION ");
                obj_str.Append(", REQCRIMINAL_FROMDATE=@REQCRIMINAL_FROMDATE ");
                obj_str.Append(", REQCRIMINAL_TODATE=@REQCRIMINAL_TODATE ");
                obj_str.Append(", REQCRIMINAL_COUNT=@REQCRIMINAL_COUNT ");
                obj_str.Append(", REQCRIMINAL_RESULT=@REQCRIMINAL_RESULT ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE REQCRIMINAL_ID=@REQCRIMINAL_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@REQCRIMINAL_LOCATION", SqlDbType.VarChar); obj_cmd.Parameters["@REQCRIMINAL_LOCATION"].Value = model.reqcriminal_location;
                obj_cmd.Parameters.Add("@REQCRIMINAL_FROMDATE", SqlDbType.DateTime); obj_cmd.Parameters["@REQCRIMINAL_FROMDATE"].Value = model.reqcriminal_fromdate;
                obj_cmd.Parameters.Add("@REQCRIMINAL_TODATE", SqlDbType.DateTime); obj_cmd.Parameters["@REQCRIMINAL_TODATE"].Value = model.reqcriminal_todate;
                obj_cmd.Parameters.Add("@REQCRIMINAL_COUNT", SqlDbType.Decimal); obj_cmd.Parameters["@REQCRIMINAL_COUNT"].Value = model.reqcriminal_count;
                obj_cmd.Parameters.Add("@REQCRIMINAL_RESULT", SqlDbType.VarChar); obj_cmd.Parameters["@REQCRIMINAL_RESULT"].Value = model.reqcriminal_result;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@REQCRIMINAL_ID", SqlDbType.Int); obj_cmd.Parameters["@REQCRIMINAL_ID"].Value = model.reqcriminal_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "REQCRM005:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
