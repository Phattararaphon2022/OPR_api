using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRAssessment
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRAssessment() { }

        public string getMessage() { return this.Message.Replace("EMP_TR_ASSESSMENT", "").Replace("cls_ctTRAssessment", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRAssessment> getData(string condition)
        {
            List<cls_TRAssessment> list_model = new List<cls_TRAssessment>();
            cls_TRAssessment model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");

                obj_str.Append(", EMPASSESSMENT_ID");
                obj_str.Append(", EMPASSESSMENT_LOCATION");
                obj_str.Append(", EMPASSESSMENT_TOPIC");
                obj_str.Append(", EMPASSESSMENT_FROMDATE");
                obj_str.Append(", EMPASSESSMENT_TODATE");
                obj_str.Append(", EMPASSESSMENT_COUNT");
                obj_str.Append(", EMPASSESSMENT_RESULT");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_TR_ASSESSMENT");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRAssessment();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();

                    model.empassessment_id = Convert.ToInt32(dr["EMPASSESSMENT_ID"]);
                    model.empassessment_location = dr["EMPASSESSMENT_LOCATION"].ToString();
                    model.empassessment_topic = dr["EMPASSESSMENT_TOPIC"].ToString();
                    model.empassessment_fromdate = Convert.ToDateTime(dr["EMPASSESSMENT_FROMDATE"]);
                    model.empassessment_todate = Convert.ToDateTime(dr["EMPASSESSMENT_TODATE"]);
                    model.empassessment_count = Convert.ToDouble(dr["EMPASSESSMENT_COUNT"]);
                    model.empassessment_result = dr["EMPASSESSMENT_RESULT"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "EMPASM001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRAssessment> getDataByFillter(string com, string emp)
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

                obj_str.Append("SELECT ISNULL(EMPASSESSMENT_ID, 1) ");
                obj_str.Append(" FROM EMP_TR_ASSESSMENT");
                obj_str.Append(" ORDER BY EMPASSESSMENT_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPASM002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT EMPASSESSMENT_ID");
                obj_str.Append(" FROM EMP_TR_ASSESSMENT");
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
                Message = "EMPASM003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM EMP_TR_ASSESSMENT");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "EMPASM004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRAssessment model)
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

                obj_str.Append("INSERT INTO EMP_TR_ASSESSMENT");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");

                obj_str.Append(", EMPASSESSMENT_ID ");
                obj_str.Append(", EMPASSESSMENT_LOCATION ");
                obj_str.Append(", EMPASSESSMENT_TOPIC ");
                obj_str.Append(", EMPASSESSMENT_FROMDATE ");
                obj_str.Append(", EMPASSESSMENT_TODATE ");
                obj_str.Append(", EMPASSESSMENT_COUNT ");
                obj_str.Append(", EMPASSESSMENT_RESULT ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @EMPASSESSMENT_ID ");
                obj_str.Append(", @EMPASSESSMENT_LOCATION ");
                obj_str.Append(", @EMPASSESSMENT_TOPIC ");
                obj_str.Append(", @EMPASSESSMENT_FROMDATE ");
                obj_str.Append(", @EMPASSESSMENT_TODATE ");
                obj_str.Append(", @EMPASSESSMENT_COUNT ");
                obj_str.Append(", @EMPASSESSMENT_RESULT ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.empassessment_id = this.getNextID();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@EMPASSESSMENT_ID", SqlDbType.Int); obj_cmd.Parameters["@EMPASSESSMENT_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@EMPASSESSMENT_LOCATION", SqlDbType.VarChar); obj_cmd.Parameters["@EMPASSESSMENT_LOCATION"].Value = model.empassessment_location;
                obj_cmd.Parameters.Add("@EMPASSESSMENT_TOPIC", SqlDbType.VarChar); obj_cmd.Parameters["@EMPASSESSMENT_TOPIC"].Value = model.empassessment_topic;
                obj_cmd.Parameters.Add("@EMPASSESSMENT_FROMDATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPASSESSMENT_FROMDATE"].Value = model.empassessment_fromdate;
                obj_cmd.Parameters.Add("@EMPASSESSMENT_TODATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPASSESSMENT_TODATE"].Value = model.empassessment_todate;
                obj_cmd.Parameters.Add("@EMPASSESSMENT_COUNT", SqlDbType.Decimal); obj_cmd.Parameters["@EMPASSESSMENT_COUNT"].Value = model.empassessment_count;
                obj_cmd.Parameters.Add("@EMPASSESSMENT_RESULT", SqlDbType.VarChar); obj_cmd.Parameters["@EMPASSESSMENT_RESULT"].Value = model.empassessment_result;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                strResult = model.empassessment_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "EMPASM005:" + ex.ToString();
                strResult = "";
            }

            return blnResult;
        }

        public bool update(cls_TRAssessment model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE EMP_TR_ASSESSMENT SET ");

                obj_str.Append(" EMPASSESSMENT_LOCATION=@EMPASSESSMENT_LOCATION ");
                obj_str.Append(", EMPASSESSMENT_TOPIC=@EMPASSESSMENT_TOPIC ");
                obj_str.Append(", EMPASSESSMENT_FROMDATE=@EMPASSESSMENT_FROMDATE ");
                obj_str.Append(", EMPASSESSMENT_TODATE=@EMPASSESSMENT_TODATE ");
                obj_str.Append(", EMPASSESSMENT_COUNT=@EMPASSESSMENT_COUNT ");
                obj_str.Append(", EMPASSESSMENT_RESULT=@EMPASSESSMENT_RESULT ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE EMPASSESSMENT_ID=@EMPASSESSMENT_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@EMPASSESSMENT_LOCATION", SqlDbType.VarChar); obj_cmd.Parameters["@EMPASSESSMENT_LOCATION"].Value = model.empassessment_location;
                obj_cmd.Parameters.Add("@EMPASSESSMENT_TOPIC", SqlDbType.VarChar); obj_cmd.Parameters["@EMPASSESSMENT_TOPIC"].Value = model.empassessment_topic;
                obj_cmd.Parameters.Add("@EMPASSESSMENT_FROMDATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPASSESSMENT_FROMDATE"].Value = model.empassessment_fromdate;
                obj_cmd.Parameters.Add("@EMPASSESSMENT_TODATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPASSESSMENT_TODATE"].Value = model.empassessment_todate;
                obj_cmd.Parameters.Add("@EMPASSESSMENT_COUNT", SqlDbType.Decimal); obj_cmd.Parameters["@EMPASSESSMENT_COUNT"].Value = model.empassessment_count;
                obj_cmd.Parameters.Add("@EMPASSESSMENT_RESULT", SqlDbType.VarChar); obj_cmd.Parameters["@EMPASSESSMENT_RESULT"].Value = model.empassessment_result;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@EMPASSESSMENT_ID", SqlDbType.Int); obj_cmd.Parameters["@EMPASSESSMENT_ID"].Value = model.empassessment_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "EMPASM006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
