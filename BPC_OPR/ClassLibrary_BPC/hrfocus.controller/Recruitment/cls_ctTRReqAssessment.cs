using ClassLibrary_BPC.hrfocus.model.Recruitment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus
{
    public class cls_ctTRReqAssessment
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRReqAssessment() { }

        public string getMessage() { return this.Message.Replace("REQ_TR_ASSESSMENT", "").Replace("cls_ctTRReqAssessment", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRReqAssessment> getData(string condition)
        {
            List<cls_TRReqAssessment> list_model = new List<cls_TRReqAssessment>();
            cls_TRReqAssessment model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", APPLYWORK_CODE");

                obj_str.Append(", REQASSESSMENT_ID");
                obj_str.Append(", REQASSESSMENT_LOCATION");
                obj_str.Append(", REQASSESSMENT_TOPIC");
                obj_str.Append(", REQASSESSMENT_FROMDATE");
                obj_str.Append(", REQASSESSMENT_TODATE");
                obj_str.Append(", REQASSESSMENT_COUNT");
                obj_str.Append(", REQASSESSMENT_RESULT");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM REQ_TR_ASSESSMENT");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, APPLYWORK_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRReqAssessment();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.applywork_code = dr["APPLYWORK_CODE"].ToString();

                    model.reqassessment_id = Convert.ToInt32(dr["REQASSESSMENT_ID"]);
                    model.reqassessment_location = dr["REQASSESSMENT_LOCATION"].ToString();
                    model.reqassessment_topic = dr["REQASSESSMENT_TOPIC"].ToString();
                    model.reqassessment_fromdate = Convert.ToDateTime(dr["REQASSESSMENT_FROMDATE"]);
                    model.reqassessment_todate = Convert.ToDateTime(dr["REQASSESSMENT_TODATE"]);
                    model.reqassessment_count = Convert.ToDouble(dr["REQASSESSMENT_COUNT"]);
                    model.reqassessment_result = dr["REQASSESSMENT_RESULT"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "REQASM001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRReqAssessment> getDataByFillter(string com, string emp)
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

                obj_str.Append("SELECT ISNULL(REQASSESSMENT_ID, 1) ");
                obj_str.Append(" FROM REQ_TR_ASSESSMENT");
                obj_str.Append(" ORDER BY REQASSESSMENT_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "REQASM002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT REQASSESSMENT_ID");
                obj_str.Append(" FROM REQ_TR_ASSESSMENT");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND APPLYWORK_CODE='" + emp + "' ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "REQASM003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM REQ_TR_ASSESSMENT");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND APPLYWORK_CODE='" + emp + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "REQASM004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRReqAssessment model)
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

                obj_str.Append("INSERT INTO REQ_TR_ASSESSMENT");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", APPLYWORK_CODE ");

                obj_str.Append(", REQASSESSMENT_ID ");
                obj_str.Append(", REQASSESSMENT_LOCATION ");
                obj_str.Append(", REQASSESSMENT_TOPIC ");
                obj_str.Append(", REQASSESSMENT_FROMDATE ");
                obj_str.Append(", REQASSESSMENT_TODATE ");
                obj_str.Append(", REQASSESSMENT_COUNT ");
                obj_str.Append(", REQASSESSMENT_RESULT ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @APPLYWORK_CODE ");
                obj_str.Append(", @REQASSESSMENT_ID ");
                obj_str.Append(", @REQASSESSMENT_LOCATION ");
                obj_str.Append(", @REQASSESSMENT_TOPIC ");
                obj_str.Append(", @REQASSESSMENT_FROMDATE ");
                obj_str.Append(", @REQASSESSMENT_TODATE ");
                obj_str.Append(", @REQASSESSMENT_COUNT ");
                obj_str.Append(", @REQASSESSMENT_RESULT ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.reqassessment_id = this.getNextID();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@APPLYWORK_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYWORK_CODE"].Value = model.applywork_code;

                obj_cmd.Parameters.Add("@REQASSESSMENT_ID", SqlDbType.Int); obj_cmd.Parameters["@REQASSESSMENT_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@REQASSESSMENT_LOCATION", SqlDbType.VarChar); obj_cmd.Parameters["@REQASSESSMENT_LOCATION"].Value = model.reqassessment_location;
                obj_cmd.Parameters.Add("@REQASSESSMENT_TOPIC", SqlDbType.VarChar); obj_cmd.Parameters["@REQASSESSMENT_TOPIC"].Value = model.reqassessment_topic;
                obj_cmd.Parameters.Add("@REQASSESSMENT_FROMDATE", SqlDbType.DateTime); obj_cmd.Parameters["@REQASSESSMENT_FROMDATE"].Value = model.reqassessment_fromdate;
                obj_cmd.Parameters.Add("@REQASSESSMENT_TODATE", SqlDbType.DateTime); obj_cmd.Parameters["@REQASSESSMENT_TODATE"].Value = model.reqassessment_todate;
                obj_cmd.Parameters.Add("@REQASSESSMENT_COUNT", SqlDbType.Decimal); obj_cmd.Parameters["@REQASSESSMENT_COUNT"].Value = model.reqassessment_count;
                obj_cmd.Parameters.Add("@REQASSESSMENT_RESULT", SqlDbType.VarChar); obj_cmd.Parameters["@REQASSESSMENT_RESULT"].Value = model.reqassessment_result;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                strResult = model.reqassessment_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "REQASM005:" + ex.ToString();
                strResult = "";
            }

            return blnResult;
        }

        public bool update(cls_TRReqAssessment model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE REQ_TR_ASSESSMENT SET ");

                obj_str.Append(" REQASSESSMENT_LOCATION=@REQASSESSMENT_LOCATION ");
                obj_str.Append(", REQASSESSMENT_TOPIC=@REQASSESSMENT_TOPIC ");
                obj_str.Append(", REQASSESSMENT_FROMDATE=@REQASSESSMENT_FROMDATE ");
                obj_str.Append(", REQASSESSMENT_TODATE=@REQASSESSMENT_TODATE ");
                obj_str.Append(", REQASSESSMENT_COUNT=@REQASSESSMENT_COUNT ");
                obj_str.Append(", REQASSESSMENT_RESULT=@REQASSESSMENT_RESULT ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE REQASSESSMENT_ID=@REQASSESSMENT_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@REQASSESSMENT_LOCATION", SqlDbType.VarChar); obj_cmd.Parameters["@REQASSESSMENT_LOCATION"].Value = model.reqassessment_location;
                obj_cmd.Parameters.Add("@REQASSESSMENT_TOPIC", SqlDbType.VarChar); obj_cmd.Parameters["@REQASSESSMENT_TOPIC"].Value = model.reqassessment_topic;
                obj_cmd.Parameters.Add("@REQASSESSMENT_FROMDATE", SqlDbType.DateTime); obj_cmd.Parameters["@REQASSESSMENT_FROMDATE"].Value = model.reqassessment_fromdate;
                obj_cmd.Parameters.Add("@REQASSESSMENT_TODATE", SqlDbType.DateTime); obj_cmd.Parameters["@REQASSESSMENT_TODATE"].Value = model.reqassessment_todate;
                obj_cmd.Parameters.Add("@REQASSESSMENT_COUNT", SqlDbType.Decimal); obj_cmd.Parameters["@REQASSESSMENT_COUNT"].Value = model.reqassessment_count;
                obj_cmd.Parameters.Add("@REQASSESSMENT_RESULT", SqlDbType.VarChar); obj_cmd.Parameters["@REQASSESSMENT_RESULT"].Value = model.reqassessment_result;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@REQASSESSMENT_ID", SqlDbType.Int); obj_cmd.Parameters["@REQASSESSMENT_ID"].Value = model.reqassessment_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "REQASM006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
