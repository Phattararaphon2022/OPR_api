using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRTimeot
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRTimeot() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRTimeot> getData(string condition)
        {
            List<cls_TRTimeot> list_model = new List<cls_TRTimeot>();
            cls_TRTimeot model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append(" SELF_TR_TIMEOT.COMPANY_CODE");
                obj_str.Append(", SELF_TR_TIMEOT.WORKER_CODE");

                obj_str.Append(", INITIAL_NAME_TH + WORKER_FNAME_TH + ' ' + WORKER_LNAME_TH AS WORKER_DETAIL_TH");
                obj_str.Append(", INITIAL_NAME_EN + WORKER_FNAME_EN + ' ' + WORKER_LNAME_EN AS WORKER_DETAIL_EN");
          
                obj_str.Append(", TIMEOT_ID");
                obj_str.Append(", ISNULL(TIMEOT_DOC, '') AS TIMEOT_DOC");

                obj_str.Append(", TIMEOT_WORKDATE");

                obj_str.Append(", ISNULL(TIMEOT_BEFOREMIN, 0) AS TIMEOT_BEFOREMIN");
                obj_str.Append(", ISNULL(TIMEOT_NORMALMIN, 0) AS TIMEOT_NORMALMIN");
                obj_str.Append(", ISNULL(TIMEOT_BREAK, 0) AS TIMEOT_BREAK");
                obj_str.Append(", ISNULL(TIMEOT_AFTERMIN, 0) AS TIMEOT_AFTERMIN");

                obj_str.Append(", ISNULL(TIMEOT_NOTE, '') AS TIMEOT_NOTE");

                obj_str.Append(", ISNULL(SELF_TR_TIMEOT.REASON_CODE, '') AS REASON_CODE");
                obj_str.Append(", ISNULL(SYS_MT_REASON.REASON_NAME_TH, '') AS REASON_NAME_TH");
                obj_str.Append(", ISNULL(SYS_MT_REASON.REASON_NAME_EN, '') AS REASON_NAME_EN");
                obj_str.Append(", ISNULL(SELF_TR_TIMEOT.LOCATION_CODE, '') AS LOCATION_CODE");
                obj_str.Append(", ISNULL(SYS_MT_LOCATION.LOCATION_NAME_TH, '') AS LOCATION_NAME_TH");
                obj_str.Append(", ISNULL(SYS_MT_LOCATION.LOCATION_NAME_EN, '') AS LOCATION_NAME_EN");
                obj_str.Append(", ISNULL(STATUS, 0) AS STATUS");

                obj_str.Append(", ISNULL(SELF_TR_TIMEOT.MODIFIED_BY, SELF_TR_TIMEOT.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(SELF_TR_TIMEOT.MODIFIED_DATE, SELF_TR_TIMEOT.CREATED_DATE) AS MODIFIED_DATE");
                obj_str.Append(", SELF_MT_JOBTABLE.STATUS_JOB");
                //
                obj_str.Append(", ATT_TR_LOSTWAGES.LOSTWAGES_CARDNO");
                //
                obj_str.Append(" FROM SELF_TR_TIMEOT");

                obj_str.Append(" INNER JOIN EMP_MT_WORKER ON EMP_MT_WORKER.COMPANY_CODE=SELF_TR_TIMEOT.COMPANY_CODE AND EMP_MT_WORKER.WORKER_CODE=SELF_TR_TIMEOT.WORKER_CODE");
                obj_str.Append(" INNER JOIN EMP_MT_INITIAL ON EMP_MT_INITIAL.INITIAL_CODE=EMP_MT_WORKER.WORKER_INITIAL ");
                obj_str.Append(" INNER JOIN SYS_MT_REASON ON SELF_TR_TIMEOT.COMPANY_CODE=SYS_MT_REASON.COMPANY_CODE AND SYS_MT_REASON.REASON_CODE=SELF_TR_TIMEOT.REASON_CODE AND SYS_MT_REASON.REASON_GROUP = 'OT' ");
                obj_str.Append(" INNER JOIN SYS_MT_LOCATION ON SELF_TR_TIMEOT.COMPANY_CODE=SYS_MT_LOCATION.COMPANY_CODE AND SYS_MT_LOCATION.LOCATION_CODE=SELF_TR_TIMEOT.LOCATION_CODE ");
                obj_str.Append(" INNER JOIN SELF_MT_JOBTABLE ON SELF_TR_TIMEOT.COMPANY_CODE=SELF_MT_JOBTABLE.COMPANY_CODE ");
                obj_str.Append(" AND SELF_MT_JOBTABLE.JOB_ID = SELF_TR_TIMEOT.TIMEOT_ID AND SELF_MT_JOBTABLE.JOB_TYPE = 'OT' ");
                //
                obj_str.Append(" LEFT JOIN ATT_TR_LOSTWAGES ON ATT_TR_LOSTWAGES.COMPANY_CODE=SELF_TR_TIMEOT.COMPANY_CODE  AND ATT_TR_LOSTWAGES.WORKER_CODE=SELF_TR_TIMEOT.WORKER_CODE");
                //
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY SELF_TR_TIMEOT.COMPANY_CODE, SELF_TR_TIMEOT.WORKER_CODE, SELF_TR_TIMEOT.TIMEOT_WORKDATE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRTimeot();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();

                    model.worker_detail_th = dr["WORKER_DETAIL_TH"].ToString();
                    model.worker_detail_en = dr["WORKER_DETAIL_EN"].ToString();

                    model.timeot_id = Convert.ToInt32(dr["TIMEOT_ID"]);
                    model.timeot_doc = dr["TIMEOT_DOC"].ToString();

                    model.timeot_workdate = Convert.ToDateTime(dr["TIMEOT_WORKDATE"]);

                    model.timeot_beforemin = Convert.ToInt32(dr["TIMEOT_BEFOREMIN"]);
                    model.timeot_normalmin = Convert.ToInt32(dr["TIMEOT_NORMALMIN"]);
                    model.timeot_breakmin = Convert.ToInt32(dr["TIMEOT_BREAK"]);
                    model.timeot_aftermin = Convert.ToInt32(dr["TIMEOT_AFTERMIN"]);

                    model.timeot_note = dr["TIMEOT_NOTE"].ToString();
                    model.location_code = dr["LOCATION_CODE"].ToString();
                    model.location_name_th = dr["LOCATION_NAME_TH"].ToString();
                    model.location_name_en = dr["LOCATION_NAME_EN"].ToString();
                    model.reason_code = dr["REASON_CODE"].ToString();
                    model.reason_name_th = dr["REASON_NAME_TH"].ToString();
                    model.reason_name_en = dr["REASON_NAME_EN"].ToString();
                    model.status = Convert.ToInt32(dr["STATUS"]);
                    model.status_job = dr["STATUS_JOB"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Timeot.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRTimeot> getDataByFillter(int id,int status,string com, string emp, DateTime datefrom, DateTime dateto)
        {
            string strCondition = "";
            if(!id.Equals(0))
                strCondition += " AND SELF_TR_TIMEOT.TIMEOT_ID='" + id + "'";
            if (!status.Equals(1))
                strCondition += " AND SELF_TR_TIMEOT.STATUS='" + status + "'";
            if(!com.Equals(""))
                strCondition += " AND SELF_TR_TIMEOT.COMPANY_CODE='" + com + "'";
            if(!emp.Equals(""))
                strCondition += " AND SELF_TR_TIMEOT.WORKER_CODE='" + emp + "'";
            strCondition += " AND (TIMEOT_WORKDATE BETWEEN '" + datefrom.ToString("MM/dd/yyyy") + "' AND '" + dateto.ToString("MM/dd/yyyy") + "')";

            return this.getData(strCondition);
        }

        public List<cls_TRTimeot> getDataByFillterLostwages(int id, int status, string com, string cardno, DateTime datefrom, DateTime dateto)
        {
            string strCondition = "";
            if (!id.Equals(0))
                strCondition += " AND SELF_TR_TIMEOT.TIMEOT_ID='" + id + "'";
            if (!status.Equals(1))
                strCondition += " AND SELF_TR_TIMEOT.STATUS='" + status + "'";
            if (!com.Equals(""))
                strCondition += " AND SELF_TR_TIMEOT.COMPANY_CODE='" + com + "'";
            if (!cardno.Equals(""))
                strCondition += " AND ATT_TR_LOSTWAGES.LOSTWAGES_CARDNO='" + cardno + "'";
            strCondition += " AND (TIMEOT_WORKDATE BETWEEN '" + datefrom.ToString("MM/dd/yyyy") + "' AND '" + dateto.ToString("MM/dd/yyyy") + "')";

            return this.getData(strCondition);
        }


        public bool checkDataOld(string com, string emp, DateTime date)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM SELF_TR_TIMEOT");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND WORKER_CODE='" + emp + "'");
                obj_str.Append(" AND TIMEOT_WORKDATE='" + date.ToString("MM/dd/yyyy") + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Timeot.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(TIMEOT_ID) ");
                obj_str.Append(" FROM SELF_TR_TIMEOT");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Timeot.getNextID)" + ex.ToString();
            }

            return intResult;
        }

        public bool delete(int id)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SELF_TR_TIMEOT");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND TIMEOT_ID='" + id + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Timeot.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_TRTimeot model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.worker_code, model.timeot_workdate))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO SELF_TR_TIMEOT");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");

                obj_str.Append(", TIMEOT_ID ");
                obj_str.Append(", TIMEOT_DOC ");

                obj_str.Append(", TIMEOT_WORKDATE ");

                obj_str.Append(", TIMEOT_BEFOREMIN ");
                obj_str.Append(", TIMEOT_NORMALMIN ");
                obj_str.Append(", TIMEOT_BREAK ");
                obj_str.Append(", TIMEOT_AFTERMIN ");

                obj_str.Append(", TIMEOT_NOTE ");
                obj_str.Append(", REASON_CODE ");
                obj_str.Append(", LOCATION_CODE ");
                obj_str.Append(", STATUS ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");

                obj_str.Append(", @TIMEOT_ID ");
                obj_str.Append(", @TIMEOT_DOC ");

                obj_str.Append(", @TIMEOT_WORKDATE ");

                obj_str.Append(", @TIMEOT_BEFOREMIN ");
                obj_str.Append(", @TIMEOT_NORMALMIN ");
                obj_str.Append(", @TIMEOT_BREAK ");
                obj_str.Append(", @TIMEOT_AFTERMIN ");

                obj_str.Append(", @TIMEOT_NOTE ");
                obj_str.Append(", @REASON_CODE ");
                obj_str.Append(", @LOCATION_CODE ");
                obj_str.Append(", @STATUS ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@TIMEOT_ID", SqlDbType.Int); obj_cmd.Parameters["@TIMEOT_ID"].Value = id;
                obj_cmd.Parameters.Add("@TIMEOT_DOC", SqlDbType.VarChar); obj_cmd.Parameters["@TIMEOT_DOC"].Value = model.timeot_doc;
                obj_cmd.Parameters.Add("@TIMEOT_WORKDATE", SqlDbType.Date); obj_cmd.Parameters["@TIMEOT_WORKDATE"].Value = model.timeot_workdate;

                obj_cmd.Parameters.Add("@TIMEOT_BEFOREMIN", SqlDbType.Int); obj_cmd.Parameters["@TIMEOT_BEFOREMIN"].Value = model.timeot_beforemin;
                obj_cmd.Parameters.Add("@TIMEOT_NORMALMIN", SqlDbType.Int); obj_cmd.Parameters["@TIMEOT_NORMALMIN"].Value = model.timeot_normalmin;
                obj_cmd.Parameters.Add("@TIMEOT_BREAK", SqlDbType.Int); obj_cmd.Parameters["@TIMEOT_BREAK"].Value = model.timeot_breakmin;
                obj_cmd.Parameters.Add("@TIMEOT_AFTERMIN", SqlDbType.Int); obj_cmd.Parameters["@TIMEOT_AFTERMIN"].Value = model.timeot_aftermin;

                obj_cmd.Parameters.Add("@TIMEOT_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@TIMEOT_NOTE"].Value = model.timeot_note;
                obj_cmd.Parameters.Add("@LOCATION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@LOCATION_CODE"].Value = model.location_code;
                obj_cmd.Parameters.Add("@REASON_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@REASON_CODE"].Value = model.reason_code;
                obj_cmd.Parameters.Add("@STATUS", SqlDbType.Int); obj_cmd.Parameters["@STATUS"].Value = model.status;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = id.ToString() ;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Timeot.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_TRTimeot model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SELF_TR_TIMEOT SET ");

                obj_str.Append(" TIMEOT_DOC=@TIMEOT_DOC ");
                obj_str.Append(", TIMEOT_BEFOREMIN=@TIMEOT_BEFOREMIN ");
                obj_str.Append(", TIMEOT_NORMALMIN=@TIMEOT_NORMALMIN ");
                obj_str.Append(", TIMEOT_BREAK=@TIMEOT_BREAK ");
                obj_str.Append(", TIMEOT_AFTERMIN=@TIMEOT_AFTERMIN ");

                obj_str.Append(", TIMEOT_NOTE=@TIMEOT_NOTE ");
                obj_str.Append(", REASON_CODE=@REASON_CODE ");
                obj_str.Append(", LOCATION_CODE=@LOCATION_CODE ");
                obj_str.Append(", STATUS=@STATUS ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE TIMEOT_ID=@TIMEOT_ID ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@TIMEOT_DOC", SqlDbType.VarChar); obj_cmd.Parameters["@TIMEOT_DOC"].Value = model.timeot_doc;
                obj_cmd.Parameters.Add("@TIMEOT_BEFOREMIN", SqlDbType.Int); obj_cmd.Parameters["@TIMEOT_BEFOREMIN"].Value = model.timeot_beforemin;
                obj_cmd.Parameters.Add("@TIMEOT_NORMALMIN", SqlDbType.Int); obj_cmd.Parameters["@TIMEOT_NORMALMIN"].Value = model.timeot_normalmin;
                obj_cmd.Parameters.Add("@TIMEOT_BREAK", SqlDbType.Int); obj_cmd.Parameters["@TIMEOT_BREAK"].Value = model.timeot_breakmin;
                obj_cmd.Parameters.Add("@TIMEOT_AFTERMIN", SqlDbType.Int); obj_cmd.Parameters["@TIMEOT_AFTERMIN"].Value = model.timeot_aftermin;

                obj_cmd.Parameters.Add("@TIMEOT_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@TIMEOT_NOTE"].Value = model.timeot_note;
                obj_cmd.Parameters.Add("@LOCATION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@LOCATION_CODE"].Value = model.location_code;
                obj_cmd.Parameters.Add("@REASON_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@REASON_CODE"].Value = model.reason_code;
                obj_cmd.Parameters.Add("@STATUS", SqlDbType.Int); obj_cmd.Parameters["@STATUS"].Value = model.status;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@TIMEOT_ID", SqlDbType.Int); obj_cmd.Parameters["@TIMEOT_ID"].Value = model.timeot_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.timeot_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Timeot.update)" + ex.ToString();
            }

            return blnResult;
        }

        //-- F add 06/01/2024
        public List<cls_TRTimeot> getDocApprove(string com, string emp, DateTime datefrom, DateTime dateto)
        {
            List<cls_TRTimeot> list_model = new List<cls_TRTimeot>();
            cls_TRTimeot model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append(" SELF_TR_TIMEOT.COMPANY_CODE");
                obj_str.Append(", SELF_TR_TIMEOT.WORKER_CODE");

                obj_str.Append(", '' AS WORKER_DETAIL_TH");
                obj_str.Append(", '' AS WORKER_DETAIL_EN");

                obj_str.Append(", TIMEOT_ID");
                obj_str.Append(", ISNULL(TIMEOT_DOC, '') AS TIMEOT_DOC");

                obj_str.Append(", TIMEOT_WORKDATE");

                obj_str.Append(", ISNULL(TIMEOT_BEFOREMIN, 0) AS TIMEOT_BEFOREMIN");
                obj_str.Append(", ISNULL(TIMEOT_NORMALMIN, 0) AS TIMEOT_NORMALMIN");
                obj_str.Append(", ISNULL(TIMEOT_BREAK, 0) AS TIMEOT_BREAK");
                obj_str.Append(", ISNULL(TIMEOT_AFTERMIN, 0) AS TIMEOT_AFTERMIN");

                obj_str.Append(", ISNULL(TIMEOT_NOTE, '') AS TIMEOT_NOTE");

                obj_str.Append(", ISNULL(SELF_TR_TIMEOT.REASON_CODE, '') AS REASON_CODE");
                obj_str.Append(", '' AS REASON_NAME_TH");
                obj_str.Append(", '' AS REASON_NAME_EN");
                obj_str.Append(", '' AS LOCATION_CODE");
                obj_str.Append(", '' AS LOCATION_NAME_TH");
                obj_str.Append(", '' AS LOCATION_NAME_EN");
                obj_str.Append(", ISNULL(STATUS, 0) AS STATUS");

                obj_str.Append(", ISNULL(SELF_TR_TIMEOT.MODIFIED_BY, SELF_TR_TIMEOT.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(SELF_TR_TIMEOT.MODIFIED_DATE, SELF_TR_TIMEOT.CREATED_DATE) AS MODIFIED_DATE");
                obj_str.Append(", '' AS STATUS_JOB");

                obj_str.Append(" FROM SELF_TR_TIMEOT");
                obj_str.Append(" WHERE 1=1");

                string condition = "";

                condition += " AND COMPANY_CODE='" + com + "'";
                condition += " AND WORKER_CODE='" + emp + "'";
                condition += " AND (TIMEOT_WORKDATE BETWEEN '" + datefrom.ToString("MM/dd/yyyy") + "' AND '" + dateto.ToString("MM/dd/yyyy") + "')";
                condition += " AND STATUS='3'";

                obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY SELF_TR_TIMEOT.COMPANY_CODE, SELF_TR_TIMEOT.WORKER_CODE, SELF_TR_TIMEOT.TIMEOT_WORKDATE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRTimeot();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();

                    model.worker_detail_th = dr["WORKER_DETAIL_TH"].ToString();
                    model.worker_detail_en = dr["WORKER_DETAIL_EN"].ToString();

                    model.timeot_id = Convert.ToInt32(dr["TIMEOT_ID"]);
                    model.timeot_doc = dr["TIMEOT_DOC"].ToString();

                    model.timeot_workdate = Convert.ToDateTime(dr["TIMEOT_WORKDATE"]);

                    model.timeot_beforemin = Convert.ToInt32(dr["TIMEOT_BEFOREMIN"]);
                    model.timeot_normalmin = Convert.ToInt32(dr["TIMEOT_NORMALMIN"]);
                    model.timeot_breakmin = Convert.ToInt32(dr["TIMEOT_BREAK"]);
                    model.timeot_aftermin = Convert.ToInt32(dr["TIMEOT_AFTERMIN"]);

                    model.timeot_note = dr["TIMEOT_NOTE"].ToString();
                    model.location_code = dr["LOCATION_CODE"].ToString();
                    model.location_name_th = dr["LOCATION_NAME_TH"].ToString();
                    model.location_name_en = dr["LOCATION_NAME_EN"].ToString();
                    model.reason_code = dr["REASON_CODE"].ToString();
                    model.reason_name_th = dr["REASON_NAME_TH"].ToString();
                    model.reason_name_en = dr["REASON_NAME_EN"].ToString();
                    model.status = Convert.ToInt32(dr["STATUS"]);
                    model.status_job = dr["STATUS_JOB"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Timeot.getDocApprove)" + ex.ToString();
            }

            return list_model;
        }

        //--
    }
}
