using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary_BPC.hrfocus.model;
using System.Data.SqlClient;
using System.Data;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTReportjob
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTReportjob() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTReportjob> getData(string condition)
        {
            List<cls_MTReportjob> list_model = new List<cls_MTReportjob>();
            cls_MTReportjob model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", PROJECT_CODE");
                obj_str.Append(", REPORTJOB_ID");
                obj_str.Append(", REPORTJOB_REF");
                obj_str.Append(", REPORTJOB_TYPE");
                obj_str.Append(", REPORTJOB_STATUS");

                obj_str.Append(", ISNULL(REPORTJOB_FROMDATE, '01/01/1900') AS REPORTJOB_FROMDATE");
                obj_str.Append(", ISNULL(REPORTJOB_TODATE, '01/01/1900') AS REPORTJOB_TODATE");
                obj_str.Append(", ISNULL(REPORTJOB_PAYDATE, '01/01/1900') AS REPORTJOB_PAYDATE");

                obj_str.Append(", ISNULL(REPORTJOB_LANGUAGE, '') AS REPORTJOB_LANGUAGE");
                obj_str.Append(", ISNULL(REPORTJOB_NOTE, '') AS REPORTJOB_NOTE");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM SYS_MT_REPORTJOB");

                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY CREATED_DATE DESC");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTReportjob();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                    model.reportjob_id = Convert.ToInt32(dr["REPORTJOB_ID"]);
                    model.reportjob_type = Convert.ToString(dr["REPORTJOB_TYPE"]);
                    model.reportjob_status = Convert.ToString(dr["REPORTJOB_STATUS"]);
                    model.reportjob_fromdate = Convert.ToDateTime(dr["REPORTJOB_FROMDATE"]);
                    model.reportjob_todate = Convert.ToDateTime(dr["REPORTJOB_TODATE"]);
                    model.reportjob_paydate = Convert.ToDateTime(dr["REPORTJOB_PAYDATE"]);

                    model.reportjob_ref = Convert.ToString(dr["REPORTJOB_REF"]);

                    model.reportjob_language = Convert.ToString(dr["REPORTJOB_LANGUAGE"]);

                    model.reportjob_note = Convert.ToString(dr["REPORTJOB_NOTE"]);


                    model.created_by = dr["CREATED_BY"].ToString();
                    model.created_date = Convert.ToDateTime(dr["CREATED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(REPORTJOB.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRReportjobwhose> getDataWhose(string id)
        {
            List<cls_TRReportjobwhose> list_model = new List<cls_TRReportjobwhose>();

            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("REPORTJOB_ID");
                obj_str.Append(", WORKER_CODE");
                obj_str.Append(" FROM SYS_TR_REPORTJOBWHOSE");
                obj_str.Append(" WHERE REPORTJOB_ID='" + id + "' ");

                obj_str.Append(" ORDER BY WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    cls_TRReportjobwhose model = new cls_TRReportjobwhose();

                    model.reportjob_id = Convert.ToInt32(dr["REPORTJOB_ID"]);
                    model.worker_code = Convert.ToString(dr["WORKER_CODE"]);
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(REPORTJOB.getDataWhose)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTReportjob> getDataByFillter(string com, string id, string type, string status)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!id.Equals(""))
                strCondition += " AND REPORTJOB_ID='" + id + "'";

            if (!type.Equals(""))
                strCondition += " AND REPORTJOB_TYPE='" + type + "'";

            if (!status.Equals(""))
                strCondition += " AND REPORTJOB_STATUS='" + status + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(REPORTJOB_ID) ");
                obj_str.Append(" FROM SYS_MT_REPORTJOB");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(REPORTJOB.getNextID)" + ex.ToString();
            }

            return intResult;
        }

        public bool delete(string id)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SYS_MT_REPORTJOB");
                obj_str.Append(" WHERE REPORTJOB_ID ='" + id + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

                if (blnResult)
                {
                    obj_str.Append(" DELETE FROM SYS_TR_REPORTJOBWHOSE");
                    obj_str.Append(" WHERE REPORTJOB_ID='" + id + "'");

                    obj_conn.doExecuteSQL(obj_str.ToString());
                }

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(REPORTJOB.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTReportjob model, List<cls_TRReportjobwhose> list_whose)
        {
            string strResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_MT_REPORTJOB");
                obj_str.Append(" (");
                obj_str.Append("REPORTJOB_ID ");
                obj_str.Append(", REPORTJOB_REF ");
                obj_str.Append(", REPORTJOB_TYPE ");
                obj_str.Append(", REPORTJOB_STATUS ");
                obj_str.Append(", REPORTJOB_LANGUAGE ");
                obj_str.Append(", REPORTJOB_FROMDATE ");
                obj_str.Append(", REPORTJOB_TODATE ");
                obj_str.Append(", REPORTJOB_PAYDATE ");
                obj_str.Append(", REPORTJOB_NOTE ");

                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@REPORTJOB_ID ");
                obj_str.Append(", @REPORTJOB_REF ");
                obj_str.Append(", @REPORTJOB_TYPE ");
                obj_str.Append(", @REPORTJOB_STATUS ");
                obj_str.Append(", @REPORTJOB_LANGUAGE ");
                obj_str.Append(", @REPORTJOB_FROMDATE ");
                obj_str.Append(", @REPORTJOB_TODATE ");
                obj_str.Append(", @REPORTJOB_PAYDATE ");
                obj_str.Append(", @REPORTJOB_NOTE ");

                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");

                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                int intREPORTJOBID = this.getNextID();
                string reportjob_ref = this.doRandomString(20) + DateTime.Now.ToString("yyMMddHHmmss");

                obj_cmd.Parameters.Add("@REPORTJOB_ID", SqlDbType.Int); obj_cmd.Parameters["@REPORTJOB_ID"].Value = intREPORTJOBID;
                obj_cmd.Parameters.Add("@REPORTJOB_REF", SqlDbType.VarChar); obj_cmd.Parameters["@REPORTJOB_REF"].Value = reportjob_ref;
                obj_cmd.Parameters.Add("@REPORTJOB_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@REPORTJOB_TYPE"].Value = model.reportjob_type;
                obj_cmd.Parameters.Add("@REPORTJOB_STATUS", SqlDbType.Char); obj_cmd.Parameters["@REPORTJOB_STATUS"].Value = model.reportjob_status;
                obj_cmd.Parameters.Add("@REPORTJOB_LANGUAGE", SqlDbType.Char); obj_cmd.Parameters["@REPORTJOB_LANGUAGE"].Value = model.reportjob_language;
                obj_cmd.Parameters.Add("@REPORTJOB_FROMDATE", SqlDbType.DateTime); obj_cmd.Parameters["@REPORTJOB_FROMDATE"].Value = model.reportjob_fromdate;
                obj_cmd.Parameters.Add("@REPORTJOB_TODATE", SqlDbType.DateTime); obj_cmd.Parameters["@REPORTJOB_TODATE"].Value = model.reportjob_todate;
                obj_cmd.Parameters.Add("@REPORTJOB_PAYDATE", SqlDbType.DateTime); obj_cmd.Parameters["@REPORTJOB_PAYDATE"].Value = model.reportjob_paydate;
                obj_cmd.Parameters.Add("@REPORTJOB_NOTE", SqlDbType.Char); obj_cmd.Parameters["@REPORTJOB_NOTE"].Value = model.reportjob_note;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.created_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                int intCount = obj_cmd.ExecuteNonQuery();

                if (intCount > 0)
                {
                    strResult = reportjob_ref;

                    if (list_whose.Count > 0)
                    {
                        obj_str = new System.Text.StringBuilder();
                        obj_str.Append("INSERT INTO SYS_TR_REPORTJOBWHOSE");
                        obj_str.Append(" (");
                        obj_str.Append("REPORTJOB_ID ");
                        obj_str.Append(", WORKER_CODE ");
                        obj_str.Append(" )");

                        obj_str.Append(" VALUES(");
                        obj_str.Append("@REPORTJOB_ID ");
                        obj_str.Append(", @WORKER_CODE ");
                        obj_str.Append(" )");

                        obj_conn.doOpenTransaction();

                        obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                        obj_cmd.Transaction = obj_conn.getTransaction();

                        obj_cmd.Parameters.Add("@REPORTJOB_ID", SqlDbType.Int);
                        obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar);


                        foreach (cls_TRReportjobwhose whose in list_whose)
                        {

                            obj_cmd.Parameters["@REPORTJOB_ID"].Value = intREPORTJOBID;
                            obj_cmd.Parameters["@WORKER_CODE"].Value = whose.worker_code;

                            obj_cmd.ExecuteNonQuery();

                        }

                        obj_conn.doCommit();

                    }
                }


                obj_conn.doClose();


            }
            catch (Exception ex)
            {
                Message = "ERROR::(REPORTJOB.insert)" + ex.ToString();
            }

            return strResult;
        }

        public bool updateStatus(cls_MTReportjob model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SYS_MT_REPORTJOB SET ");
                obj_str.Append(" REPORTJOB_STATUS=@REPORTJOB_STATUS ");
                obj_str.Append(" WHERE REPORTJOB_ID=@REPORTJOB_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@REPORTJOB_STATUS", SqlDbType.VarChar); obj_cmd.Parameters["@REPORTJOB_STATUS"].Value = model.reportjob_status;
                obj_cmd.Parameters.Add("@REPORTJOB_ID", SqlDbType.Int); obj_cmd.Parameters["@REPORTJOB_ID"].Value = model.reportjob_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(REPORTJOB.updateStatus)" + ex.ToString();
            }

            return blnResult;
        }

        private Random random = new Random();
        private string doRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
