using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;
namespace ClassLibrary_BPC.hrfocus.controller
{
   public class cls_ctTRTimeonsite
    {
         string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRTimeonsite() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRTimeonsite> getData(string condition)
        {
            List<cls_TRTimeonsite> list_model = new List<cls_TRTimeonsite>();
            cls_TRTimeonsite model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append(" SELF_TR_TIMEONSITE.COMPANY_CODE");
                obj_str.Append(", SELF_TR_TIMEONSITE.WORKER_CODE");
                obj_str.Append(", SELF_TR_TIMEONSITE.TIMEONSITE_ID");
                obj_str.Append(", SELF_TR_TIMEONSITE.TIMEONSITE_DOC");
                obj_str.Append(", SELF_TR_TIMEONSITE.TIMEONSITE_WORKDATE");
                obj_str.Append(", SELF_TR_TIMEONSITE.TIMEONSITE_IN");
                obj_str.Append(", SELF_TR_TIMEONSITE.TIMEONSITE_OUT");
                obj_str.Append(", SELF_TR_TIMEONSITE.TIMEONSITE_NOTE");
                obj_str.Append(", SELF_TR_TIMEONSITE.STATUS");
                obj_str.Append(", INITIAL_NAME_TH + WORKER_FNAME_TH + ' ' + WORKER_LNAME_TH AS WORKER_DETAIL_TH");
                obj_str.Append(", INITIAL_NAME_EN + WORKER_FNAME_EN + ' ' + WORKER_LNAME_EN AS WORKER_DETAIL_EN");
                obj_str.Append(", ISNULL(SELF_TR_TIMEONSITE.REASON_CODE, '') AS REASON_CODE");
                obj_str.Append(", ISNULL(SYS_MT_REASON.REASON_NAME_TH, '') AS REASON_NAME_TH");
                obj_str.Append(", ISNULL(SYS_MT_REASON.REASON_NAME_EN, '') AS REASON_NAME_EN");
                obj_str.Append(", ISNULL(SELF_TR_TIMEONSITE.LOCATION_CODE, '') AS LOCATION_CODE");
                obj_str.Append(", ISNULL(SYS_MT_LOCATION.LOCATION_NAME_TH, '') AS LOCATION_NAME_TH");
                obj_str.Append(", ISNULL(SYS_MT_LOCATION.LOCATION_NAME_EN, '') AS LOCATION_NAME_EN");
                obj_str.Append(", ISNULL(SYS_MT_LOCATION.LOCATION_NAME_EN, '') AS LOCATION_NAME_EN");

                obj_str.Append(", ISNULL(SELF_TR_TIMEONSITE.MODIFIED_BY, SELF_TR_TIMEONSITE.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(SELF_TR_TIMEONSITE.MODIFIED_DATE, SELF_TR_TIMEONSITE.CREATED_DATE) AS MODIFIED_DATE");
                obj_str.Append(", ISNULL(SELF_TR_TIMEONSITE.FLAG, 0) AS FLAG");
                obj_str.Append(", SELF_MT_JOBTABLE.STATUS_JOB");
                //
                obj_str.Append(", ATT_TR_LOSTWAGES.LOSTWAGES_CARDNO");
                //
                obj_str.Append(" FROM SELF_TR_TIMEONSITE");
                obj_str.Append(" INNER JOIN EMP_MT_WORKER ON EMP_MT_WORKER.COMPANY_CODE=SELF_TR_TIMEONSITE.COMPANY_CODE AND EMP_MT_WORKER.WORKER_CODE=SELF_TR_TIMEONSITE.WORKER_CODE");
                obj_str.Append(" INNER JOIN EMP_MT_INITIAL ON EMP_MT_INITIAL.INITIAL_CODE=EMP_MT_WORKER.WORKER_INITIAL");
                obj_str.Append(" INNER JOIN SYS_MT_REASON ON SELF_TR_TIMEONSITE.COMPANY_CODE=SYS_MT_REASON.COMPANY_CODE");
                obj_str.Append(" AND SYS_MT_REASON.REASON_CODE=SELF_TR_TIMEONSITE.REASON_CODE AND SYS_MT_REASON.REASON_GROUP = 'ONS'");
                obj_str.Append(" INNER JOIN SYS_MT_LOCATION ON SELF_TR_TIMEONSITE.COMPANY_CODE=SYS_MT_LOCATION.COMPANY_CODE");
                obj_str.Append(" AND SYS_MT_LOCATION.LOCATION_CODE=SELF_TR_TIMEONSITE.LOCATION_CODE");
                obj_str.Append(" INNER JOIN SELF_MT_JOBTABLE ON SELF_TR_TIMEONSITE.COMPANY_CODE=SELF_MT_JOBTABLE.COMPANY_CODE");
                obj_str.Append(" AND SELF_MT_JOBTABLE.JOB_ID = SELF_TR_TIMEONSITE.TIMEONSITE_ID AND SELF_MT_JOBTABLE.JOB_TYPE = 'ONS'");
                //
                obj_str.Append(" LEFT JOIN ATT_TR_LOSTWAGES ON ATT_TR_LOSTWAGES.COMPANY_CODE=SELF_TR_TIMEONSITE.COMPANY_CODE  AND ATT_TR_LOSTWAGES.WORKER_CODE=SELF_TR_TIMEONSITE.WORKER_CODE");
                //
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY SELF_TR_TIMEONSITE.TIMEONSITE_ID");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRTimeonsite();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.worker_detail_en = dr["WORKER_DETAIL_EN"].ToString();
                    model.worker_detail_th = dr["WORKER_DETAIL_TH"].ToString();
                    model.timeonsite_id = Convert.ToInt32(dr["TIMEONSITE_ID"]);
                    model.timeonsite_doc = dr["TIMEONSITE_DOC"].ToString();
                    model.timeonsite_workdate = Convert.ToDateTime(dr["TIMEONSITE_WORKDATE"]);
                    model.timeonsite_in = dr["TIMEONSITE_IN"].ToString();
                    model.timeonsite_out = dr["TIMEONSITE_OUT"].ToString();
                    model.timeonsite_note = dr["TIMEONSITE_NOTE"].ToString();

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
                    model.flag = Convert.ToBoolean(dr["FLAG"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Trtimeonsite.getData)" + ex.ToString();
            }

            return list_model;
        }
        public List<cls_TRTimeonsite> getDataByFillter(string com,int id,string location_code,string worker_code,string datefrom,string dateto,int status)
        {
            string strCondition = "";
            if(!com.Equals(""))
                strCondition += " AND SELF_TR_TIMEONSITE.COMPANY_CODE='" + com + "'";

            if (!id.Equals(0))
                strCondition += " AND SELF_TR_TIMEONSITE.TIMEONSITE_ID='" + id + "'";

            if (!datefrom.Equals("") && !dateto.Equals(""))
                strCondition += " AND (SELF_TR_TIMEONSITE.TIMEONSITE_WORKDATE BETWEEN '" + datefrom + "' AND '" + dateto + "')";

            if (!location_code.Equals(""))
                strCondition += " AND SELF_TR_TIMEONSITE.LOCATION_CODE='" + location_code + "'";

            if (!worker_code.Equals(""))
                strCondition += " AND SELF_TR_TIMEONSITE.WORKER_CODE='" + worker_code + "'";

            if (!status.Equals(1))
                strCondition += " AND SELF_TR_TIMEONSITE.STATUS='" + status + "'";

            return this.getData(strCondition);
        }

        public List<cls_TRTimeonsite> getDataByFillterLostwages(string com, int id, string location_code, string cardno, string datefrom, string dateto, int status)
        {
            string strCondition = "";
            if (!com.Equals(""))
                strCondition += " AND SELF_TR_TIMEONSITE.COMPANY_CODE='" + com + "'";

            if (!id.Equals(0))
                strCondition += " AND SELF_TR_TIMEONSITE.TIMEONSITE_ID='" + id + "'";

            if (!datefrom.Equals("") && !dateto.Equals(""))
                strCondition += " AND (SELF_TR_TIMEONSITE.TIMEONSITE_WORKDATE BETWEEN '" + datefrom + "' AND '" + dateto + "')";

            if (!location_code.Equals(""))
                strCondition += " AND SELF_TR_TIMEONSITE.LOCATION_CODE='" + location_code + "'";

            if (!cardno.Equals(""))
                strCondition += " AND ATT_TR_LOSTWAGES.LOSTWAGES_CARDNO='" + cardno + "'";

            if (!status.Equals(1))
                strCondition += " AND SELF_TR_TIMEONSITE.STATUS='" + status + "'";

            return this.getData(strCondition);
        }
        public bool checkDataOld(string com, int id,string worker_code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT TIMEONSITE_ID");
                obj_str.Append(" FROM SELF_TR_TIMEONSITE");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                obj_str.Append(" AND TIMEONSITE_ID='" + id + "'");
                obj_str.Append(" AND WORKER_CODE='" + worker_code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Trtimeonsite.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(string com,int id,string worker_code)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SELF_TR_TIMEONSITE");
                obj_str.Append(" WHERE 1=1 ");
                if (!com.Equals(""))
                    obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                if (!id.Equals(0))
                    obj_str.Append(" AND TIMEONSITE_ID='" + id + "'");
                if (!worker_code.Equals(""))
                    obj_str.Append(" AND WORKER_CODE='" + worker_code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Trtimeonsite.delete)" + ex.ToString();
            }

            return blnResult;
        }
        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(TIMEONSITE_ID) ");
                obj_str.Append(" FROM SELF_TR_TIMEONSITE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Trtimeonsite.getNextID)" + ex.ToString();
            }

            return intResult;
        }
        public string insert(cls_TRTimeonsite model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code,model.timeonsite_id,model.worker_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO SELF_TR_TIMEONSITE");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE");
                obj_str.Append(", TIMEONSITE_ID");
                obj_str.Append(", TIMEONSITE_DOC");
                obj_str.Append(", TIMEONSITE_WORKDATE");
                obj_str.Append(", TIMEONSITE_IN");
                obj_str.Append(", TIMEONSITE_OUT");
                obj_str.Append(", TIMEONSITE_NOTE");
                obj_str.Append(", REASON_CODE");
                obj_str.Append(", LOCATION_CODE");
                obj_str.Append(", STATUS");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @TIMEONSITE_ID ");
                obj_str.Append(", @TIMEONSITE_DOC ");
                obj_str.Append(", @TIMEONSITE_WORKDATE ");
                obj_str.Append(", @TIMEONSITE_IN ");
                obj_str.Append(", @TIMEONSITE_OUT ");
                obj_str.Append(", @TIMEONSITE_NOTE ");
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
                obj_cmd.Parameters.Add("@TIMEONSITE_ID", SqlDbType.Int); obj_cmd.Parameters["@TIMEONSITE_ID"].Value = id; ;
                obj_cmd.Parameters.Add("@TIMEONSITE_DOC", SqlDbType.VarChar); obj_cmd.Parameters["@TIMEONSITE_DOC"].Value = model.timeonsite_doc;
                obj_cmd.Parameters.Add("@TIMEONSITE_WORKDATE", SqlDbType.DateTime); obj_cmd.Parameters["@TIMEONSITE_WORKDATE"].Value = model.timeonsite_workdate;
                obj_cmd.Parameters.Add("@TIMEONSITE_IN", SqlDbType.VarChar); obj_cmd.Parameters["@TIMEONSITE_IN"].Value = model.timeonsite_in;
                obj_cmd.Parameters.Add("@TIMEONSITE_OUT", SqlDbType.VarChar); obj_cmd.Parameters["@TIMEONSITE_OUT"].Value = model.timeonsite_out;
                obj_cmd.Parameters.Add("@TIMEONSITE_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@TIMEONSITE_NOTE"].Value = model.timeonsite_note;
                obj_cmd.Parameters.Add("@REASON_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@REASON_CODE"].Value = model.reason_code;
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
                Message = "ERROR::(Trtimeonsite.insert)" + ex.ToString();
            }

            return blnResult;
        }
        public string update(cls_TRTimeonsite model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SELF_TR_TIMEONSITE SET ");
                obj_str.Append(" COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(", TIMEONSITE_DOC=@TIMEONSITE_DOC ");
                obj_str.Append(", TIMEONSITE_WORKDATE=@TIMEONSITE_WORKDATE ");
                obj_str.Append(", TIMEONSITE_IN=@TIMEONSITE_IN ");
                obj_str.Append(", TIMEONSITE_OUT=@TIMEONSITE_OUT ");
                obj_str.Append(", TIMEONSITE_NOTE=@TIMEONSITE_NOTE ");
                obj_str.Append(", REASON_CODE=@REASON_CODE ");
                obj_str.Append(", LOCATION_CODE=@LOCATION_CODE ");
                obj_str.Append(", STATUS=@STATUS ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");
                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND TIMEONSITE_ID=@TIMEONSITE_ID ");
                
      

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());


                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@TIMEONSITE_ID", SqlDbType.Int); obj_cmd.Parameters["@TIMEONSITE_ID"].Value = model.timeonsite_id ;
                obj_cmd.Parameters.Add("@TIMEONSITE_DOC", SqlDbType.VarChar); obj_cmd.Parameters["@TIMEONSITE_DOC"].Value = model.timeonsite_doc;
                obj_cmd.Parameters.Add("@TIMEONSITE_WORKDATE", SqlDbType.DateTime); obj_cmd.Parameters["@TIMEONSITE_WORKDATE"].Value = model.timeonsite_workdate;
                obj_cmd.Parameters.Add("@TIMEONSITE_IN", SqlDbType.VarChar); obj_cmd.Parameters["@TIMEONSITE_IN"].Value = model.timeonsite_in;
                obj_cmd.Parameters.Add("@TIMEONSITE_OUT", SqlDbType.VarChar); obj_cmd.Parameters["@TIMEONSITE_OUT"].Value = model.timeonsite_out;
                obj_cmd.Parameters.Add("@TIMEONSITE_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@TIMEONSITE_NOTE"].Value = model.timeonsite_note;
                obj_cmd.Parameters.Add("@REASON_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@REASON_CODE"].Value = model.reason_code;
                obj_cmd.Parameters.Add("@LOCATION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@LOCATION_CODE"].Value = model.location_code;
                obj_cmd.Parameters.Add("@STATUS", SqlDbType.Int); obj_cmd.Parameters["@STATUS"].Value = model.status;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;
                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.timeonsite_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Trtimeonsite.update)" + ex.ToString();
            }

            return blnResult;
        }

    }
}
