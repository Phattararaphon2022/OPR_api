using ClassLibrary_BPC.hrfocus.model;
using ClassLibrary_BPC.hrfocus.model.Attendance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller.Attendance
{
  public class cls_ctTRATTTimeot
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRATTTimeot() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRATTTimeot> getData(string language, string condition)
        {
            List<cls_TRATTTimeot> list_model = new List<cls_TRATTTimeot>();
            cls_TRATTTimeot model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append(" ATT_TR_TIMEOT.COMPANY_CODE");
                obj_str.Append(", ATT_TR_TIMEOT.WORKER_CODE");

                if (language.Equals("TH"))
                {
                    obj_str.Append(", INITIAL_NAME_TH + WORKER_FNAME_TH + ' ' + WORKER_LNAME_TH AS WORKER_DETAIL");
                }
                else
                {
                    obj_str.Append(", INITIAL_NAME_EN + WORKER_FNAME_EN + ' ' + WORKER_LNAME_EN AS WORKER_DETAIL");
                }

                obj_str.Append(", TIMEOT_ID");
                obj_str.Append(", ISNULL(TIMEOT_DOC, '') AS TIMEOT_DOC");

                obj_str.Append(", TIMEOT_WORKDATE");
                obj_str.Append(", ISNULL(TIMEOT_WORKTODATE,TIMEOT_WORKDATE) AS TIMEOT_WORKTODATE");


                obj_str.Append(", ISNULL(TIMEOT_BEFOREMIN, 0) AS TIMEOT_BEFOREMIN");
                obj_str.Append(", ISNULL(TIMEOT_NORMALMIN, 0) AS TIMEOT_NORMALMIN");
                obj_str.Append(", ISNULL(TIMEOT_AFTERMIN, 0) AS TIMEOT_AFTERMIN");
                obj_str.Append(", ISNULL(TIMEOT_BREAK, 0) AS TIMEOT_BREAK");

                obj_str.Append(", ISNULL(TIMEOT_NOTE, '') AS TIMEOT_NOTE");

                obj_str.Append(", ISNULL(REASON_CODE, '') AS REASON_CODE");
                obj_str.Append(", ISNULL(LOCATION_CODE, '') AS LOCATION_CODE");

                obj_str.Append(", ISNULL(ATT_TR_TIMEOT.MODIFIED_BY, ATT_TR_TIMEOT.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(ATT_TR_TIMEOT.MODIFIED_DATE, ATT_TR_TIMEOT.CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM ATT_TR_TIMEOT");

                obj_str.Append(" INNER JOIN EMP_MT_WORKER ON EMP_MT_WORKER.COMPANY_CODE=ATT_TR_TIMEOT.COMPANY_CODE AND EMP_MT_WORKER.WORKER_CODE=ATT_TR_TIMEOT.WORKER_CODE ");
                obj_str.Append(" INNER JOIN EMP_MT_INITIAL ON EMP_MT_INITIAL.INITIAL_CODE=EMP_MT_WORKER.WORKER_INITIAL");

                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY ATT_TR_TIMEOT.COMPANY_CODE, ATT_TR_TIMEOT.WORKER_CODE, ATT_TR_TIMEOT.TIMEOT_WORKDATE, ATT_TR_TIMEOT.TIMEOT_WORKTODATE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRATTTimeot();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();

                    model.worker_detail = dr["WORKER_DETAIL"].ToString();

                    model.timeot_id = Convert.ToInt32(dr["TIMEOT_ID"]);
                    model.timeot_doc = dr["TIMEOT_DOC"].ToString();

                    model.timeot_workdate = Convert.ToDateTime(dr["TIMEOT_WORKDATE"]);
                    model.timeot_worktodate = Convert.ToDateTime(dr["TIMEOT_WORKTODATE"]);

                    model.timeot_beforemin = Convert.ToInt32(dr["TIMEOT_BEFOREMIN"]);
                    model.timeot_normalmin = Convert.ToInt32(dr["TIMEOT_NORMALMIN"]);
                    model.timeot_aftermin = Convert.ToInt32(dr["TIMEOT_AFTERMIN"]);
                    model.timeot_break = Convert.ToInt32(dr["TIMEOT_BREAK"]);

                    model.timeot_note = dr["TIMEOT_NOTE"].ToString();
                    model.location_code = dr["LOCATION_CODE"].ToString();
                    model.reason_code = dr["REASON_CODE"].ToString();

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

        public List<cls_TRATTTimeot> getDataByFillter(string language, string com, string emp, DateTime datefrom, DateTime dateto)
        {
            string strCondition = "";

            strCondition += " AND ATT_TR_TIMEOT.COMPANY_CODE='" + com + "'";
            strCondition += " AND ATT_TR_TIMEOT.WORKER_CODE='" + emp + "'";
            strCondition += " AND (TIMEOT_WORKDATE BETWEEN '" + datefrom.ToString("MM/dd/yyyy") + "' AND '" + dateto.ToString("MM/dd/yyyy") + "'";
            strCondition += " OR TIMEOT_WORKTODATE BETWEEN '" + datefrom.ToString("MM/dd/yyyy") + "' AND '" + dateto.ToString("MM/dd/yyyy") + "')";

            //strCondition += " AND (TIMEOT_WORKDATE BETWEEN '" + datefrom.ToString("MM/dd/yyyy") + "' AND '" + dateto.ToString("MM/dd/yyyy") + "')";

            return this.getData(language, strCondition);
        }



        public bool checkDataOld(string com, string emp, DateTime date, DateTime dateto)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM ATT_TR_TIMEOT");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND WORKER_CODE='" + emp + "'");
                obj_str.Append(" AND TIMEOT_WORKDATE='" + date.ToString("MM/dd/yyyy") + "'");
                obj_str.Append(" AND TIMEOT_WORKTODATE='" + dateto.ToString("MM/dd/yyyy") + "'");

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

                obj_str.Append("SELECT ISNULL(TIMEOT_ID, 1) ");
                obj_str.Append(" FROM ATT_TR_TIMEOT");
                obj_str.Append(" ORDER BY TIMEOT_ID DESC ");

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



        

        public bool delete(string id)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_TIMEOT");
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

        public bool insert(cls_TRATTTimeot model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.worker_code, model.timeot_workdate, model.timeot_worktodate))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO ATT_TR_TIMEOT");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");

                obj_str.Append(", TIMEOT_ID ");
                obj_str.Append(", TIMEOT_DOC ");

                obj_str.Append(", TIMEOT_WORKDATE ");
                obj_str.Append(", TIMEOT_WORKTODATE ");

                obj_str.Append(", TIMEOT_BEFOREMIN ");
                obj_str.Append(", TIMEOT_NORMALMIN ");
                obj_str.Append(", TIMEOT_AFTERMIN ");
                obj_str.Append(", TIMEOT_BREAK ");

                obj_str.Append(", TIMEOT_NOTE ");
                obj_str.Append(", REASON_CODE ");
                obj_str.Append(", LOCATION_CODE ");

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
                obj_str.Append(", @TIMEOT_WORKTODATE ");

                obj_str.Append(", @TIMEOT_BEFOREMIN ");
                obj_str.Append(", @TIMEOT_NORMALMIN ");
                obj_str.Append(", @TIMEOT_AFTERMIN ");
                obj_str.Append(", @TIMEOT_BREAK ");

                obj_str.Append(", @TIMEOT_NOTE ");
                obj_str.Append(", @REASON_CODE ");
                obj_str.Append(", @LOCATION_CODE ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@TIMEOT_ID", SqlDbType.Int); obj_cmd.Parameters["@TIMEOT_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@TIMEOT_DOC", SqlDbType.VarChar); obj_cmd.Parameters["@TIMEOT_DOC"].Value = model.timeot_doc;
                obj_cmd.Parameters.Add("@TIMEOT_WORKDATE", SqlDbType.Date); obj_cmd.Parameters["@TIMEOT_WORKDATE"].Value = model.timeot_workdate;
                obj_cmd.Parameters.Add("@TIMEOT_WORKTODATE", SqlDbType.Date); obj_cmd.Parameters["@TIMEOT_WORKTODATE"].Value = model.timeot_worktodate;

                obj_cmd.Parameters.Add("@TIMEOT_BEFOREMIN", SqlDbType.Int); obj_cmd.Parameters["@TIMEOT_BEFOREMIN"].Value = model.timeot_beforemin;
                obj_cmd.Parameters.Add("@TIMEOT_NORMALMIN", SqlDbType.Int); obj_cmd.Parameters["@TIMEOT_NORMALMIN"].Value = model.timeot_normalmin;
                obj_cmd.Parameters.Add("@TIMEOT_AFTERMIN", SqlDbType.Int); obj_cmd.Parameters["@TIMEOT_AFTERMIN"].Value = model.timeot_aftermin;
                obj_cmd.Parameters.Add("@TIMEOT_BREAK", SqlDbType.Int); obj_cmd.Parameters["@TIMEOT_BREAK"].Value = model.timeot_break;

                obj_cmd.Parameters.Add("@TIMEOT_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@TIMEOT_NOTE"].Value = model.timeot_note;
                obj_cmd.Parameters.Add("@LOCATION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@LOCATION_CODE"].Value = model.location_code;
                obj_cmd.Parameters.Add("@REASON_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@REASON_CODE"].Value = model.reason_code;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Timeot.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public bool update(cls_TRATTTimeot model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE ATT_TR_TIMEOT SET ");

                obj_str.Append(" TIMEOT_DOC=@TIMEOT_DOC ");
                obj_str.Append(", TIMEOT_BEFOREMIN=@TIMEOT_BEFOREMIN ");
                obj_str.Append(", TIMEOT_NORMALMIN=@TIMEOT_NORMALMIN ");
                obj_str.Append(", TIMEOT_AFTERMIN=@TIMEOT_AFTERMIN ");
                obj_str.Append(", TIMEOT_BREAK=@TIMEOT_BREAK ");

                obj_str.Append(", TIMEOT_NOTE=@TIMEOT_NOTE ");
                obj_str.Append(", REASON_CODE=@REASON_CODE ");
                obj_str.Append(", LOCATION_CODE=@LOCATION_CODE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE TIMEOT_ID=@TIMEOT_ID ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@TIMEOT_DOC", SqlDbType.VarChar); obj_cmd.Parameters["@TIMEOT_DOC"].Value = model.timeot_doc;
                obj_cmd.Parameters.Add("@TIMEOT_BEFOREMIN", SqlDbType.Int); obj_cmd.Parameters["@TIMEOT_BEFOREMIN"].Value = model.timeot_beforemin;
                obj_cmd.Parameters.Add("@TIMEOT_NORMALMIN", SqlDbType.Int); obj_cmd.Parameters["@TIMEOT_NORMALMIN"].Value = model.timeot_normalmin;
                obj_cmd.Parameters.Add("@TIMEOT_AFTERMIN", SqlDbType.Int); obj_cmd.Parameters["@TIMEOT_AFTERMIN"].Value = model.timeot_aftermin;
                obj_cmd.Parameters.Add("@TIMEOT_BREAK", SqlDbType.Int); obj_cmd.Parameters["@TIMEOT_BREAK"].Value = model.timeot_break;

                obj_cmd.Parameters.Add("@TIMEOT_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@TIMEOT_NOTE"].Value = model.timeot_note;
                obj_cmd.Parameters.Add("@LOCATION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@LOCATION_CODE"].Value = model.location_code;
                obj_cmd.Parameters.Add("@REASON_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@REASON_CODE"].Value = model.reason_code;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@TIMEOT_ID", SqlDbType.Int); obj_cmd.Parameters["@TIMEOT_ID"].Value = model.timeot_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Timeot.update)" + ex.ToString();
            }

            return blnResult;
        }

       
    }
}
