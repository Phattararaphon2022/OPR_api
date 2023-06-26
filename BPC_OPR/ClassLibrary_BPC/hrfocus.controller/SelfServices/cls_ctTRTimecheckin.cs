using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRTimecheckin
    {
           string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRTimecheckin() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRTimecheckin> getData(string condition)
        {
            List<cls_TRTimecheckin> list_model = new List<cls_TRTimecheckin>();
            cls_TRTimecheckin model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");
                obj_str.Append("SELF_TR_TIMECHECKIN.COMPANY_CODE");
                obj_str.Append(", TIMECHECKIN_ID");
                obj_str.Append(", TIMECHECKIN_DOC");
                obj_str.Append(", TIMECHECKIN_WORKDATE");
                obj_str.Append(", TIMECHECKIN_TIME");
                obj_str.Append(", TIMECHECKIN_TYPE");
                obj_str.Append(", TIMECHECKIN_LAT");
                obj_str.Append(", TIMECHECKIN_LONG");
                obj_str.Append(", TIMECHECKIN_NOTE");
                obj_str.Append(", ISNULL(SELF_TR_TIMECHECKIN.LOCATION_CODE, '') AS LOCATION_CODE");
                obj_str.Append(", ISNULL(SYS_MT_LOCATION.LOCATION_NAME_TH, '') AS LOCATION_NAME_TH");
                obj_str.Append(", ISNULL(SYS_MT_LOCATION.LOCATION_NAME_EN, '') AS LOCATION_NAME_EN");
                obj_str.Append(", SELF_TR_TIMECHECKIN.WORKER_CODE");
                obj_str.Append(", INITIAL_NAME_TH + WORKER_FNAME_TH + ' ' + WORKER_LNAME_TH AS WORKER_DETAIL_TH");
                obj_str.Append(", INITIAL_NAME_EN + WORKER_FNAME_EN + ' ' + WORKER_LNAME_EN AS WORKER_DETAIL_EN");
                obj_str.Append(", ISNULL(SELF_TR_TIMECHECKIN.MODIFIED_BY, SELF_TR_TIMECHECKIN.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(SELF_TR_TIMECHECKIN.MODIFIED_DATE, SELF_TR_TIMECHECKIN.CREATED_DATE) AS MODIFIED_DATE");
                obj_str.Append(", ISNULL(SELF_TR_TIMECHECKIN.FLAG, 0) AS FLAG");
                obj_str.Append(", SELF_TR_TIMECHECKIN.STATUS");
                obj_str.Append(", SELF_MT_JOBTABLE.STATUS_JOB");

                obj_str.Append(" FROM SELF_TR_TIMECHECKIN");
                obj_str.Append(" INNER JOIN EMP_MT_WORKER ON EMP_MT_WORKER.COMPANY_CODE=SELF_TR_TIMECHECKIN.COMPANY_CODE");
                obj_str.Append(" AND EMP_MT_WORKER.WORKER_CODE=SELF_TR_TIMECHECKIN.WORKER_CODE");
                obj_str.Append(" INNER JOIN EMP_MT_INITIAL ON EMP_MT_INITIAL.INITIAL_CODE=EMP_MT_WORKER.WORKER_INITIAL");
                obj_str.Append(" INNER JOIN SYS_MT_LOCATION ON SELF_TR_TIMECHECKIN.COMPANY_CODE=SYS_MT_LOCATION.COMPANY_CODE");
                obj_str.Append(" INNER JOIN SELF_MT_JOBTABLE ON SELF_TR_TIMECHECKIN.COMPANY_CODE=SELF_MT_JOBTABLE.COMPANY_CODE ");
                obj_str.Append(" AND SELF_MT_JOBTABLE.JOB_ID = SELF_TR_TIMECHECKIN.TIMECHECKIN_ID AND SELF_MT_JOBTABLE.JOB_TYPE = 'CI' ");
                obj_str.Append(" AND SYS_MT_LOCATION.LOCATION_CODE=SELF_TR_TIMECHECKIN.LOCATION_CODE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY TIMECHECKIN_ID");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRTimecheckin();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.worker_detail_en = dr["WORKER_DETAIL_EN"].ToString();
                    model.worker_detail_th = dr["WORKER_DETAIL_TH"].ToString();
                    model.timecheckin_id = Convert.ToInt32(dr["TIMECHECKIN_ID"]);
                    model.timecheckin_doc = dr["TIMECHECKIN_DOC"].ToString();
                    model.timecheckin_workdate = Convert.ToDateTime(dr["TIMECHECKIN_WORKDATE"]);
                    model.timecheckin_time = dr["TIMECHECKIN_TIME"].ToString();
                    model.timecheckin_type = dr["TIMECHECKIN_TYPE"].ToString();
                    model.timecheckin_lat = Convert.ToDouble(dr["TIMECHECKIN_LAT"]);
                    model.timecheckin_long = Convert.ToDouble(dr["TIMECHECKIN_LONG"]);
                    model.timecheckin_note = dr["TIMECHECKIN_NOTE"].ToString();
                    model.location_code = dr["LOCATION_CODE"].ToString();
                    model.location_name_en = dr["LOCATION_NAME_EN"].ToString();
                    model.location_name_th = dr["LOCATION_NAME_TH"].ToString();
                    model.status = Convert.ToInt32(dr["STATUS"]);
                    model.status_job = dr["STATUS_JOB"].ToString();
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                    model.flag = Convert.ToBoolean(dr["FLAG"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Trtimecheckin.getData)" + ex.ToString();
            }

            return list_model;
        }
        public List<cls_TRTimecheckin> getDataByFillter(string com,int id,string time,string type,string location_code,string worker_code,string datefrom,string dateto,int status)
        {
            string strCondition = "";
            if(!com.Equals(""))
                strCondition += " AND SELF_TR_TIMECHECKIN.COMPANY_CODE='" + com + "'";

            if (!id.Equals(0))
                strCondition += " AND SELF_TR_TIMECHECKIN.TIMECHECKIN_ID='" + id + "'";

            if (!datefrom.Equals("") && !dateto.Equals(""))
                strCondition += " AND (SELF_TR_TIMECHECKIN.TIMECHECKIN_WORKDATE BETWEEN '" + datefrom + "' AND '" + dateto + "')";

            if (!time.Equals(""))
                strCondition += " AND SELF_TR_TIMECHECKIN.TIMECHECKIN_TIME='" + time + "'";

            if (!type.Equals(""))
                strCondition += " AND SELF_TR_TIMECHECKIN.TIMECHECKIN_TYPE='" + type + "'";

            if (!location_code.Equals(""))
                strCondition += " AND SELF_TR_TIMECHECKIN.LOCATION_CODE='" + location_code + "'";

            if (!worker_code.Equals(""))
                strCondition += " AND SELF_TR_TIMECHECKIN.WORKER_CODE='" + worker_code + "'";

            if (!status.Equals(1))
                strCondition += " AND SELF_TR_TIMECHECKIN.STATUS='" + status + "'";

            return this.getData(strCondition);
        }
        public bool checkDataOld(string com, string date,string time,string type, string worker_code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT TIMECHECKIN_ID");
                obj_str.Append(" FROM SELF_TR_TIMECHECKIN");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                obj_str.Append(" AND TIMECHECKIN_WORKDATE ='" + date + "'");
                obj_str.Append(" AND TIMECHECKIN_TIME='" + time + "'");
                obj_str.Append(" AND WORKER_CODE='" + worker_code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Trtimecheckin.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(string com,string id, string time, string type,string date,string worker_code)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SELF_TR_TIMECHECKIN");
                obj_str.Append(" WHERE 1=1 ");
                if (!com.Equals(""))
                    obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                if (!id.Equals(""))
                    obj_str.Append(" AND TIMECHECKIN_ID='" + id + "'");
                if (!time.Equals(""))
                    obj_str.Append(" AND TIMECHECKIN_TIME='" + time + "'");
                if (!date.Equals(""))
                    obj_str.Append(" AND TIMECHECKIN_WORKDATE='" + date + "'");
                if (!worker_code.Equals(""))
                    obj_str.Append(" AND WORKER_CODE='" + worker_code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Trtimecheckin.delete)" + ex.ToString();
            }

            return blnResult;
        }
        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(TIMECHECKIN_ID) ");
                obj_str.Append(" FROM SELF_TR_TIMECHECKIN");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Trtimecheckin.getNextID)" + ex.ToString();
            }

            return intResult;
        }
        public string insert(cls_TRTimecheckin model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.timecheckin_workdate.ToString("MM/dd/yyyy"), model.timecheckin_time, model.timecheckin_type, model.worker_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO SELF_TR_TIMECHECKIN");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", TIMECHECKIN_ID ");
                obj_str.Append(", TIMECHECKIN_DOC ");
                obj_str.Append(", TIMECHECKIN_WORKDATE ");
                obj_str.Append(", TIMECHECKIN_TIME ");
                obj_str.Append(", TIMECHECKIN_TYPE ");
                obj_str.Append(", TIMECHECKIN_LAT ");
                obj_str.Append(", TIMECHECKIN_LONG ");
                obj_str.Append(", TIMECHECKIN_NOTE ");
                obj_str.Append(", LOCATION_CODE ");
                obj_str.Append(", STATUS ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @TIMECHECKIN_ID ");
                obj_str.Append(", @TIMECHECKIN_DOC ");
                obj_str.Append(", @TIMECHECKIN_WORKDATE ");
                obj_str.Append(", @TIMECHECKIN_TIME ");
                obj_str.Append(", @TIMECHECKIN_TYPE ");
                obj_str.Append(", @TIMECHECKIN_LAT ");
                obj_str.Append(", @TIMECHECKIN_LONG ");
                obj_str.Append(", @TIMECHECKIN_NOTE ");
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
                obj_cmd.Parameters.Add("@TIMECHECKIN_ID", SqlDbType.Int); obj_cmd.Parameters["@TIMECHECKIN_ID"].Value = id;
                obj_cmd.Parameters.Add("@TIMECHECKIN_DOC", SqlDbType.VarChar); obj_cmd.Parameters["@TIMECHECKIN_DOC"].Value = model.timecheckin_doc;
                obj_cmd.Parameters.Add("@TIMECHECKIN_WORKDATE", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECHECKIN_WORKDATE"].Value = model.timecheckin_workdate;
                obj_cmd.Parameters.Add("@TIMECHECKIN_TIME", SqlDbType.VarChar); obj_cmd.Parameters["@TIMECHECKIN_TIME"].Value = model.timecheckin_time;
                obj_cmd.Parameters.Add("@TIMECHECKIN_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@TIMECHECKIN_TYPE"].Value = model.timecheckin_type;
                obj_cmd.Parameters.Add("@TIMECHECKIN_LAT", SqlDbType.Float); obj_cmd.Parameters["@TIMECHECKIN_LAT"].Value = model.timecheckin_lat;
                obj_cmd.Parameters.Add("@TIMECHECKIN_LONG", SqlDbType.Float); obj_cmd.Parameters["@TIMECHECKIN_LONG"].Value = model.timecheckin_long;
                obj_cmd.Parameters.Add("@TIMECHECKIN_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@TIMECHECKIN_NOTE"].Value = model.timecheckin_note;
                obj_cmd.Parameters.Add("@LOCATION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@LOCATION_CODE"].Value = model.location_code;
                obj_cmd.Parameters.Add("@STATUS", SqlDbType.Int); obj_cmd.Parameters["@STATUS"].Value = model.status;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Trtimecheckin.insert)" + ex.ToString();
            }

            return blnResult;
        }
        public string update(cls_TRTimecheckin model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SELF_TR_TIMECHECKIN SET ");
                obj_str.Append(" COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(", TIMECHECKIN_DOC=@TIMECHECKIN_DOC ");
                obj_str.Append(", TIMECHECKIN_WORKDATE=@TIMECHECKIN_WORKDATE ");
                obj_str.Append(", TIMECHECKIN_TIME=@TIMECHECKIN_TIME ");
                obj_str.Append(", TIMECHECKIN_TYPE=@TIMECHECKIN_TYPE ");
                obj_str.Append(", TIMECHECKIN_LAT=@TIMECHECKIN_LAT ");
                obj_str.Append(", TIMECHECKIN_LONG=@TIMECHECKIN_LONG ");
                obj_str.Append(", TIMECHECKIN_NOTE=@TIMECHECKIN_NOTE ");
                obj_str.Append(", LOCATION_CODE=@LOCATION_CODE ");
                obj_str.Append(", STATUS=@STATUS ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");
                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND TIMECHECKIN_ID=@TIMECHECKIN_ID ");
      

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());


                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@TIMECHECKIN_ID", SqlDbType.Int); obj_cmd.Parameters["@TIMECHECKIN_ID"].Value = model.timecheckin_id;
                obj_cmd.Parameters.Add("@TIMECHECKIN_DOC", SqlDbType.VarChar); obj_cmd.Parameters["@TIMECHECKIN_DOC"].Value = model.timecheckin_doc;
                obj_cmd.Parameters.Add("@TIMECHECKIN_WORKDATE", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECHECKIN_WORKDATE"].Value = model.timecheckin_workdate;
                obj_cmd.Parameters.Add("@TIMECHECKIN_TIME", SqlDbType.VarChar); obj_cmd.Parameters["@TIMECHECKIN_TIME"].Value = model.timecheckin_time;
                obj_cmd.Parameters.Add("@TIMECHECKIN_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@TIMECHECKIN_TYPE"].Value = model.timecheckin_type;
                obj_cmd.Parameters.Add("@TIMECHECKIN_LAT", SqlDbType.Float); obj_cmd.Parameters["@TIMECHECKIN_LAT"].Value = model.timecheckin_lat;
                obj_cmd.Parameters.Add("@TIMECHECKIN_LONG", SqlDbType.Float); obj_cmd.Parameters["@TIMECHECKIN_LONG"].Value = model.timecheckin_long;
                obj_cmd.Parameters.Add("@TIMECHECKIN_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@TIMECHECKIN_NOTE"].Value = model.timecheckin_note;
                obj_cmd.Parameters.Add("@LOCATION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@LOCATION_CODE"].Value = model.location_code;
                obj_cmd.Parameters.Add("@STATUS", SqlDbType.Int); obj_cmd.Parameters["@STATUS"].Value = model.status;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;
                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.timecheckin_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Trtimecheckin.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
