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
   public class cls_ctTRATTTimeonsite
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRATTTimeonsite() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRATTTimeonsite> getData(string language, string condition)
        {
            List<cls_TRATTTimeonsite> list_model = new List<cls_TRATTTimeonsite>();
            cls_TRATTTimeonsite model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append(" ATT_TR_TIMEONSITE.COMPANY_CODE");
                obj_str.Append(", ATT_TR_TIMEONSITE.WORKER_CODE");

                obj_str.Append(", TIMEONSITE_ID");
                obj_str.Append(", ISNULL(TIMEONSITE_DOC, '') AS TIMEONSITE_DOC");

                obj_str.Append(", TIMEONSITE_WORKDATE");

                obj_str.Append(", ISNULL(TIMEONSITE_IN, '00:00') AS TIMEONSITE_IN");
                obj_str.Append(", ISNULL(TIMEONSITE_OUT, '00:00') AS TIMEONSITE_OUT");


                obj_str.Append(", ISNULL(TIMEONSITE_NOTE, '') AS TIMEONSITE_NOTE");

                obj_str.Append(", ISNULL(ATT_TR_TIMEONSITE.REASON_CODE, '') AS REASON_CODE");
                obj_str.Append(", ISNULL(ATT_TR_TIMEONSITE.LOCATION_CODE, '') AS LOCATION_CODE");

                if (language.Equals("TH"))
                {
                    obj_str.Append(", ISNULL(LOCATION_NAME_TH, '') AS LOCATION_DETAIL");
                    obj_str.Append(", ISNULL(REASON_NAME_TH, '') AS REASON_DETAIL");
                    obj_str.Append(", INITIAL_NAME_TH + WORKER_FNAME_TH + ' ' + WORKER_LNAME_TH AS WORKER_DETAIL");
                }
                else
                {
                    obj_str.Append(", ISNULL(LOCATION_NAME_EN, '') AS LOCATION_DETAIL");
                    obj_str.Append(", ISNULL(REASON_NAME_EN, '') AS REASON_DETAIL");
                    obj_str.Append(", INITIAL_NAME_EN + WORKER_FNAME_EN + ' ' + WORKER_LNAME_EN AS WORKER_DETAIL");
                }


                obj_str.Append(", ISNULL(ATT_TR_TIMEONSITE.MODIFIED_BY, ATT_TR_TIMEONSITE.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(ATT_TR_TIMEONSITE.MODIFIED_DATE, ATT_TR_TIMEONSITE.CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM ATT_TR_TIMEONSITE");

                obj_str.Append("  INNER JOIN EMP_MT_WORKER ON EMP_MT_WORKER.COMPANY_CODE=ATT_TR_TIMEONSITE.COMPANY_CODE AND EMP_MT_WORKER.WORKER_CODE=ATT_TR_TIMEONSITE.WORKER_CODE");
                obj_str.Append(" INNER JOIN EMP_MT_INITIAL ON EMP_MT_INITIAL.INITIAL_CODE=EMP_MT_WORKER.WORKER_INITIAL");

                obj_str.Append(" LEFT JOIN SYS_MT_REASON ON SYS_MT_REASON.REASON_CODE=ATT_TR_TIMEONSITE.REASON_CODE AND REASON_GROUP = 'ONSITE'");
                obj_str.Append(" LEFT JOIN SYS_MT_LOCATION ON SYS_MT_LOCATION.LOCATION_CODE=ATT_TR_TIMEONSITE.LOCATION_CODE  ");

                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY ATT_TR_TIMEONSITE.COMPANY_CODE, ATT_TR_TIMEONSITE.WORKER_CODE, TIMEONSITE_WORKDATE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRATTTimeonsite();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();

                    model.timeonsite_id = Convert.ToInt32(dr["TIMEONSITE_ID"]);
                    model.timeonsite_doc = dr["TIMEONSITE_DOC"].ToString();

                    model.timeonsite_workdate = Convert.ToDateTime(dr["TIMEONSITE_WORKDATE"]);

                    model.timeonsite_in = dr["TIMEONSITE_IN"].ToString();
                    model.timeonsite_out = dr["TIMEONSITE_OUT"].ToString();

                    model.timeonsite_note = dr["TIMEONSITE_NOTE"].ToString();
                    model.location_code = dr["LOCATION_CODE"].ToString();
                    model.reason_code = dr["REASON_CODE"].ToString();

                    model.worker_detail = dr["WORKER_DETAIL"].ToString();
                    model.location_detail = dr["LOCATION_DETAIL"].ToString();
                    model.reason_detail = dr["REASON_DETAIL"].ToString();

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

        public List<cls_TRATTTimeonsite> getDataByFillter(string language, string com, string emp, DateTime datefrom, DateTime dateto)
        {
            string strCondition = "";

            strCondition += " AND ATT_TR_TIMEONSITE.COMPANY_CODE='" + com + "'";
            strCondition += " AND ATT_TR_TIMEONSITE.WORKER_CODE='" + emp + "'";
            //strCondition += " AND (TIMEONSITE_WORKDATE BETWEEN '" + datefrom.ToString("MM/dd/yyyy") + "' AND '" + dateto.ToString("MM/dd/yyyy") + "')";
            //strCondition += " AND TIMEONSITE_WORKDATE='" + datefrom.ToString("MM/dd/yyyy") + "'";

            return this.getData(language, strCondition);
        }
       
        public cls_TRATTTimeonsite getDataByID(string id)
        {

            string strCondition = " AND ATT_TR_TIMELEAVE.TIMELEAVE_ID='" + id + "'";

            List<cls_TRATTTimeonsite> list_model = this.getData("EN", strCondition);

            if (list_model.Count > 0)
                return list_model[0];
            else
                return null;

        }

        public bool checkDataOld(string com, string emp, DateTime date)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM ATT_TR_TIMEONSITE");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND WORKER_CODE='" + emp + "'");
                obj_str.Append(" AND TIMEONSITE_WORKDATE='" + date.ToString("MM/dd/yyyy") + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Timeleave.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }


        


        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(TIMEONSITE_ID, 1) ");
                obj_str.Append(" FROM ATT_TR_TIMEONSITE");
                obj_str.Append(" ORDER BY TIMEONSITE_ID DESC ");

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

                obj_str.Append(" DELETE FROM ATT_TR_TIMEONSITE");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND TIMEONSITE_ID='" + id + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Timeot.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRATTTimeonsite model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.worker_code, model.timeonsite_workdate))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("INSERT INTO ATT_TR_TIMEONSITE");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");

                obj_str.Append(", TIMEONSITE_ID ");
                obj_str.Append(", TIMEONSITE_DOC ");

                obj_str.Append(", TIMEONSITE_WORKDATE ");

                obj_str.Append(", TIMEONSITE_IN ");
                obj_str.Append(", TIMEONSITE_OUT ");

                obj_str.Append(", TIMEONSITE_NOTE ");
                obj_str.Append(", REASON_CODE ");
                obj_str.Append(", LOCATION_CODE ");

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

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@TIMEONSITE_ID", SqlDbType.Int); obj_cmd.Parameters["@TIMEONSITE_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@TIMEONSITE_DOC", SqlDbType.VarChar); obj_cmd.Parameters["@TIMEONSITE_DOC"].Value = model.timeonsite_doc;
                obj_cmd.Parameters.Add("@TIMEONSITE_WORKDATE", SqlDbType.Date); obj_cmd.Parameters["@TIMEONSITE_WORKDATE"].Value = model.timeonsite_workdate;

                obj_cmd.Parameters.Add("@TIMEONSITE_IN", SqlDbType.Char); obj_cmd.Parameters["@TIMEONSITE_IN"].Value = model.timeonsite_in;
                obj_cmd.Parameters.Add("@TIMEONSITE_OUT", SqlDbType.Char); obj_cmd.Parameters["@TIMEONSITE_OUT"].Value = model.timeonsite_out;

                obj_cmd.Parameters.Add("@TIMEONSITE_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@TIMEONSITE_NOTE"].Value = model.timeonsite_note;
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

        public bool update(cls_TRATTTimeonsite model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();


                obj_str.Append("UPDATE ATT_TR_TIMEONSITE SET ");

                obj_str.Append(" TIMEONSITE_DOC=@TIMEONSITE_DOC ");
                obj_str.Append(", TIMEONSITE_IN=@TIMEONSITE_IN ");
                obj_str.Append(", TIMEONSITE_OUT=@TIMEONSITE_OUT ");

                obj_str.Append(", TIMEONSITE_NOTE=@TIMEONSITE_NOTE ");
                obj_str.Append(", REASON_CODE=@REASON_CODE ");
                obj_str.Append(", LOCATION_CODE=@LOCATION_CODE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE TIMEONSITE_ID=@TIMEONSITE_ID ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@TIMEONSITE_DOC", SqlDbType.VarChar); obj_cmd.Parameters["@TIMEONSITE_DOC"].Value = model.timeonsite_doc;

                obj_cmd.Parameters.Add("@TIMEONSITE_IN", SqlDbType.Char); obj_cmd.Parameters["@TIMEONSITE_IN"].Value = model.timeonsite_in;
                obj_cmd.Parameters.Add("@TIMEONSITE_OUT", SqlDbType.Char); obj_cmd.Parameters["@TIMEONSITE_OUT"].Value = model.timeonsite_out;

                obj_cmd.Parameters.Add("@TIMEONSITE_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@TIMEONSITE_NOTE"].Value = model.timeonsite_note;
                obj_cmd.Parameters.Add("@LOCATION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@LOCATION_CODE"].Value = model.location_code;
                obj_cmd.Parameters.Add("@REASON_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@REASON_CODE"].Value = model.reason_code;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@TIMEONSITE_ID", SqlDbType.Int); obj_cmd.Parameters["@TIMEONSITE_ID"].Value = model.timeonsite_id;

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
