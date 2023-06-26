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
    public class cls_ctTRTimecard
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRTimecard() { }

        public string getMessage() { return this.Message; }

        private string FormatDateDB = "MM/dd/yyyy";

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRTimecard> getData(string condition)
        {
            List<cls_TRTimecard> list_model = new List<cls_TRTimecard>();
            cls_TRTimecard model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("ATT_TR_TIMECARD.COMPANY_CODE");
                obj_str.Append(", ATT_TR_TIMECARD.WORKER_CODE");

                obj_str.Append(", ATT_TR_TIMECARD.PROJECT_CODE");
                obj_str.Append(", ATT_TR_TIMECARD.PROJOB_CODE");

                obj_str.Append(", SHIFT_CODE");
                obj_str.Append(", TIMECARD_WORKDATE");
                obj_str.Append(", TIMECARD_DAYTYPE");
                obj_str.Append(", TIMECARD_COLOR");
                obj_str.Append(", ISNULL(TIMECARD_CH1, TIMECARD_WORKDATE) AS TIMECARD_CH1");
                obj_str.Append(", ISNULL(TIMECARD_CH2, TIMECARD_WORKDATE) AS TIMECARD_CH2");
                obj_str.Append(", ISNULL(TIMECARD_CH3, TIMECARD_WORKDATE) AS TIMECARD_CH3");
                obj_str.Append(", ISNULL(TIMECARD_CH4, TIMECARD_WORKDATE) AS TIMECARD_CH4");
                obj_str.Append(", ISNULL(TIMECARD_CH5, TIMECARD_WORKDATE) AS TIMECARD_CH5");
                obj_str.Append(", ISNULL(TIMECARD_CH6, TIMECARD_WORKDATE) AS TIMECARD_CH6");
                obj_str.Append(", ISNULL(TIMECARD_CH7, TIMECARD_WORKDATE) AS TIMECARD_CH7");
                obj_str.Append(", ISNULL(TIMECARD_CH8, TIMECARD_WORKDATE) AS TIMECARD_CH8");
                obj_str.Append(", ISNULL(TIMECARD_CH9, TIMECARD_WORKDATE) AS TIMECARD_CH9");
                obj_str.Append(", ISNULL(TIMECARD_CH10, TIMECARD_WORKDATE) AS TIMECARD_CH10");

                obj_str.Append(", ISNULL(TIMECARD_BEFORE_MIN, 0) AS TIMECARD_BEFORE_MIN");
                obj_str.Append(", ISNULL(TIMECARD_WORK1_MIN, 0) AS TIMECARD_WORK1_MIN");
                obj_str.Append(", ISNULL(TIMECARD_WORK2_MIN, 0) AS TIMECARD_WORK2_MIN");
                obj_str.Append(", ISNULL(TIMECARD_BREAK_MIN, 0) AS TIMECARD_BREAK_MIN");
                obj_str.Append(", ISNULL(TIMECARD_AFTER_MIN, 0) AS TIMECARD_AFTER_MIN");
                obj_str.Append(", ISNULL(TIMECARD_LATE_MIN, 0) AS TIMECARD_LATE_MIN");

                obj_str.Append(", ISNULL(TIMECARD_BEFORE_MIN_APP, 0) AS TIMECARD_BEFORE_MIN_APP");
                obj_str.Append(", ISNULL(TIMECARD_WORK1_MIN_APP, 0) AS TIMECARD_WORK1_MIN_APP");
                obj_str.Append(", ISNULL(TIMECARD_WORK2_MIN_APP, 0) AS TIMECARD_WORK2_MIN_APP");
                obj_str.Append(", ISNULL(TIMECARD_BREAK_MIN_APP, 0) AS TIMECARD_BREAK_MIN_APP");
                obj_str.Append(", ISNULL(TIMECARD_AFTER_MIN_APP, 0) AS TIMECARD_AFTER_MIN_APP");
                obj_str.Append(", ISNULL(TIMECARD_LATE_MIN_APP, 0) AS TIMECARD_LATE_MIN_APP");

                obj_str.Append(", ISNULL(TIMECARD_LEAVEPAY_MIN, 0) AS TIMECARD_LEAVEPAY_MIN");
                obj_str.Append(", ISNULL(TIMECARD_LEAVEDEDUCT_MIN, 0) AS TIMECARD_LEAVEDEDUCT_MIN");
                obj_str.Append(", ISNULL(TIMECARD_BEFORE_DG, 0) AS TIMECARD_BEFORE_DG");
                obj_str.Append(", ISNULL(TIMECARD_AFTER_DG, 0) AS TIMECARD_AFTER_DG");
                obj_str.Append(", ISNULL(TIMECARD_LOCK, 0) AS TIMECARD_LOCK");

                obj_str.Append(", ISNULL(ATT_TR_TIMECARD.MODIFIED_BY, ATT_TR_TIMECARD.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(ATT_TR_TIMECARD.MODIFIED_DATE, ATT_TR_TIMECARD.CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(", ISNULL(EMP_MT_INITIAL.INITIAL_NAME_TH, '') + ISNULL(WORKER_FNAME_TH, '') + ' ' + ISNULL(WORKER_LNAME_TH, '') AS WORKER_NAME_TH");
                obj_str.Append(", ISNULL(EMP_MT_INITIAL.INITIAL_NAME_EN, '') + ISNULL(WORKER_FNAME_EN, '') + ' ' + ISNULL(WORKER_LNAME_EN, '') AS WORKER_NAME_EN");

                obj_str.Append(" FROM ATT_TR_TIMECARD");
                obj_str.Append(" INNER JOIN EMP_MT_WORKER ON ATT_TR_TIMECARD.COMPANY_CODE=EMP_MT_WORKER.COMPANY_CODE AND ATT_TR_TIMECARD.WORKER_CODE=EMP_MT_WORKER.WORKER_CODE");
                obj_str.Append(" LEFT JOIN EMP_MT_INITIAL ON EMP_MT_INITIAL.INITIAL_CODE=EMP_MT_WORKER.WORKER_INITIAL ");

                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY ATT_TR_TIMECARD.COMPANY_CODE, ATT_TR_TIMECARD.WORKER_CODE, ATT_TR_TIMECARD.TIMECARD_WORKDATE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRTimecard();

                    model.company_code = dr["COMPANY_CODE"].ToString();                    
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.project_code = dr["PROJECT_CODE"].ToString();
                    model.projob_code = dr["PROJOB_CODE"].ToString();

                    model.shift_code = dr["SHIFT_CODE"].ToString();

                    model.timecard_workdate = Convert.ToDateTime(dr["TIMECARD_WORKDATE"]);

                    model.timecard_daytype = dr["TIMECARD_DAYTYPE"].ToString();
                    model.timecard_color = dr["TIMECARD_COLOR"].ToString();

                    model.timecard_ch1 = Convert.ToDateTime(dr["TIMECARD_CH1"]);
                    model.timecard_ch2 = Convert.ToDateTime(dr["TIMECARD_CH2"]);
                    model.timecard_ch3 = Convert.ToDateTime(dr["TIMECARD_CH3"]);
                    model.timecard_ch4 = Convert.ToDateTime(dr["TIMECARD_CH4"]);
                    model.timecard_ch5 = Convert.ToDateTime(dr["TIMECARD_CH5"]);
                    model.timecard_ch6 = Convert.ToDateTime(dr["TIMECARD_CH6"]);
                    model.timecard_ch7 = Convert.ToDateTime(dr["TIMECARD_CH7"]);
                    model.timecard_ch8 = Convert.ToDateTime(dr["TIMECARD_CH8"]);
                    model.timecard_ch9 = Convert.ToDateTime(dr["TIMECARD_CH9"]);
                    model.timecard_ch10 = Convert.ToDateTime(dr["TIMECARD_CH10"]);

                    model.timecard_before_min = Convert.ToInt32(dr["TIMECARD_BEFORE_MIN"]);
                    model.timecard_work1_min = Convert.ToInt32(dr["TIMECARD_WORK1_MIN"]);
                    model.timecard_work2_min = Convert.ToInt32(dr["TIMECARD_WORK2_MIN"]);
                    model.timecard_break_min = Convert.ToInt32(dr["TIMECARD_BREAK_MIN"]);
                    model.timecard_after_min = Convert.ToInt32(dr["TIMECARD_AFTER_MIN"]);
                    model.timecard_late_min = Convert.ToInt32(dr["TIMECARD_LATE_MIN"]);

                    model.timecard_before_min_app = Convert.ToInt32(dr["TIMECARD_BEFORE_MIN_APP"]);
                    model.timecard_work1_min_app = Convert.ToInt32(dr["TIMECARD_WORK1_MIN_APP"]);
                    model.timecard_work2_min_app = Convert.ToInt32(dr["TIMECARD_WORK2_MIN_APP"]);
                    model.timecard_break_min_app = Convert.ToInt32(dr["TIMECARD_BREAK_MIN_APP"]);
                    model.timecard_after_min_app = Convert.ToInt32(dr["TIMECARD_AFTER_MIN_APP"]);
                    model.timecard_late_min_app = Convert.ToInt32(dr["TIMECARD_LATE_MIN_APP"]);


                    model.timecard_leavepay_min = Convert.ToInt32(dr["TIMECARD_LEAVEPAY_MIN"]);
                    model.timecard_leavededuct_min = Convert.ToInt32(dr["TIMECARD_LEAVEDEDUCT_MIN"]);
                    model.timecard_before_dg = Convert.ToBoolean(dr["TIMECARD_BEFORE_DG"]);
                    model.timecard_after_dg = Convert.ToBoolean(dr["TIMECARD_AFTER_DG"]);
                    model.timecard_lock = Convert.ToBoolean(dr["TIMECARD_LOCK"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    model.worker_name_th = dr["WORKER_NAME_TH"].ToString();
                    model.worker_name_en = dr["WORKER_NAME_EN"].ToString();
                    

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Timecard.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRTimecard> getDataByFillter(string com, string project, string worker, DateTime fromdate, DateTime todate)
        {
            string strCondition = "";

            strCondition += " AND ATT_TR_TIMECARD.COMPANY_CODE='" + com + "'";

            if (!project.Equals(""))
                strCondition += " AND ATT_TR_TIMECARD.PROJECT_CODE='" + project + "'";

            strCondition += " AND (TIMECARD_WORKDATE BETWEEN '" + fromdate.ToString(this.FormatDateDB) + "' AND '" + todate.ToString(this.FormatDateDB) + "' )";

            if (!worker.Equals(""))
                strCondition += " AND ATT_TR_TIMECARD.WORKER_CODE='" + worker + "'";

            return this.getData(strCondition);
        }

        public List<cls_TRTimecard> getDataByJob(string com, string project, string job, DateTime fromdate, DateTime todate)
        {
            string strCondition = "";

            strCondition += " AND ATT_TR_TIMECARD.COMPANY_CODE='" + com + "'";
            strCondition += " AND ATT_TR_TIMECARD.PROJECT_CODE='" + project + "'";
            strCondition += " AND ATT_TR_TIMECARD.PROJOB_CODE='" + job + "'";

            strCondition += " AND (TIMECARD_WORKDATE BETWEEN '" + fromdate.ToString(this.FormatDateDB) + "' AND '" + todate.ToString(this.FormatDateDB) + "' )";

            

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string project, string worker, DateTime workdate)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT TIMECARD_WORKDATE");
                obj_str.Append(" FROM ATT_TR_TIMECARD");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND WORKER_CODE='" + worker + "'");
                obj_str.Append(" AND TIMECARD_WORKDATE='" + workdate.ToString(this.FormatDateDB) + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Timecard.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string project, string worker, DateTime workdate)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_TIMECARD");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND WORKER_CODE='" + worker + "'");
                obj_str.Append(" AND TIMECARD_WORKDATE='" + workdate.ToString(this.FormatDateDB) + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Timecard.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool clear(string com, string worker, DateTime fromdate, DateTime todate)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_TIMECARD");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND WORKER_CODE='" + worker + "'");
                obj_str.Append(" AND (TIMECARD_WORKDATE BETWEEN '" + fromdate.ToString(this.FormatDateDB) + "' AND '" + todate.ToString(this.FormatDateDB) + "' )");


                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Timecard.clear)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert_plantime(string com, string project, string worker, DateTime fromdate, DateTime todate, List<cls_TRTimecard> list_model)
        {
            bool blnResult = false;
            cls_ctConnection obj_conn = new cls_ctConnection();
            try
            {

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                //-- Step 1 delete data old
                obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_TIMECARD");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND WORKER_CODE='" + worker + "'");
                obj_str.Append(" AND (TIMECARD_WORKDATE BETWEEN '" + fromdate.ToString(this.FormatDateDB) + "' AND '" + todate.ToString(this.FormatDateDB) + "' )");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str.ToString());

                //-- Step 2 insert
                if (blnResult)
                {
                    obj_str = new System.Text.StringBuilder();
                    obj_str.Append("INSERT INTO ATT_TR_TIMECARD");
                    obj_str.Append(" (");
                    obj_str.Append("COMPANY_CODE ");                    
                    obj_str.Append(", WORKER_CODE ");
                    obj_str.Append(", PROJECT_CODE ");
                    obj_str.Append(", PROJOB_CODE ");
                    obj_str.Append(", SHIFT_CODE ");
                    obj_str.Append(", TIMECARD_WORKDATE ");
                    obj_str.Append(", TIMECARD_DAYTYPE ");
                    obj_str.Append(", TIMECARD_COLOR ");
                    obj_str.Append(", TIMECARD_LOCK ");
                    obj_str.Append(", CREATED_BY ");
                    obj_str.Append(", CREATED_DATE ");
                    obj_str.Append(", FLAG ");
                    obj_str.Append(" )");

                    obj_str.Append(" VALUES(");
                    obj_str.Append("@COMPANY_CODE ");                    
                    obj_str.Append(", @WORKER_CODE ");
                    obj_str.Append(", @PROJECT_CODE ");
                    obj_str.Append(", @PROJOB_CODE ");

                    obj_str.Append(", @SHIFT_CODE ");
                    obj_str.Append(", @TIMECARD_WORKDATE ");
                    obj_str.Append(", @TIMECARD_DAYTYPE ");
                    obj_str.Append(", @TIMECARD_COLOR ");
                    obj_str.Append(", @TIMECARD_LOCK ");
                    obj_str.Append(", @CREATED_BY ");
                    obj_str.Append(", @CREATED_DATE ");
                    obj_str.Append(", @FLAG ");
                    obj_str.Append(" )");

                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);                    
                    obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@PROJOB_CODE", SqlDbType.VarChar);

                    obj_cmd.Parameters.Add("@SHIFT_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@TIMECARD_WORKDATE", SqlDbType.Date);
                    obj_cmd.Parameters.Add("@TIMECARD_DAYTYPE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@TIMECARD_COLOR", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@TIMECARD_LOCK", SqlDbType.Bit);

                    obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime);
                    obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit);


                    foreach (cls_TRTimecard model in list_model)
                    {

                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;                        
                        obj_cmd.Parameters["@WORKER_CODE"].Value = worker;

                        obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code == null ? "" : model.project_code;
                        obj_cmd.Parameters["@PROJOB_CODE"].Value = model.projob_code == null ? "" : model.projob_code;

                        obj_cmd.Parameters["@SHIFT_CODE"].Value = model.shift_code;
                        obj_cmd.Parameters["@TIMECARD_WORKDATE"].Value = model.timecard_workdate.Date;
                        obj_cmd.Parameters["@TIMECARD_DAYTYPE"].Value = model.timecard_daytype;
                        obj_cmd.Parameters["@TIMECARD_COLOR"].Value = model.timecard_color;
                        obj_cmd.Parameters["@TIMECARD_LOCK"].Value = false;

                        obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                        obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                        obj_cmd.Parameters["@FLAG"].Value = false;

                        obj_cmd.ExecuteNonQuery();

                    }

                    blnResult = obj_conn.doCommit();

                }
                else
                {
                    obj_conn.doRollback();
                }

            }


            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Timecard.insert_plantime)" + ex.ToString();
                obj_conn.doRollback();
            }
            finally
            {
                obj_conn.doClose();
            }

            return blnResult;
        }

        public bool insert(cls_TRTimecard model)
        {
            bool blnResult = false;
            cls_ctConnection obj_conn = new cls_ctConnection();
            try
            {
                if (this.checkDataOld(model.company_code, model.project_code, model.worker_code, model.timecard_workdate))
                    return true;


                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();


                //-- Step 2 insert
                if (true)
                {
                    obj_str = new System.Text.StringBuilder();
                    obj_str.Append("INSERT INTO ATT_TR_TIMECARD");
                    obj_str.Append(" (");
                    obj_str.Append("COMPANY_CODE ");                    
                    obj_str.Append(", WORKER_CODE ");
                    obj_str.Append(", PROJECT_CODE ");
                    obj_str.Append(", PROJOB_CODE ");
                    
                    obj_str.Append(", SHIFT_CODE ");
                    obj_str.Append(", TIMECARD_WORKDATE ");
                    obj_str.Append(", TIMECARD_DAYTYPE ");
                    obj_str.Append(", TIMECARD_COLOR ");
                    obj_str.Append(", TIMECARD_LOCK ");
                    obj_str.Append(", CREATED_BY ");
                    obj_str.Append(", CREATED_DATE ");
                    obj_str.Append(", FLAG ");
                    obj_str.Append(" )");

                    obj_str.Append(" VALUES(");
                    obj_str.Append("@COMPANY_CODE ");                    
                    obj_str.Append(", @WORKER_CODE ");

                    obj_str.Append(", @PROJECT_CODE ");
                    obj_str.Append(", @PROJOB_CODE ");

                    obj_str.Append(", @SHIFT_CODE ");
                    obj_str.Append(", @TIMECARD_WORKDATE ");
                    obj_str.Append(", @TIMECARD_DAYTYPE ");
                    obj_str.Append(", @TIMECARD_COLOR ");
                    obj_str.Append(", @TIMECARD_LOCK ");
                    obj_str.Append(", @CREATED_BY ");
                    obj_str.Append(", @CREATED_DATE ");
                    obj_str.Append(", @FLAG ");
                    obj_str.Append(" )");

                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);                    
                    obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar);

                    obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@PROJOB_CODE", SqlDbType.VarChar);

                    obj_cmd.Parameters.Add("@SHIFT_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@TIMECARD_WORKDATE", SqlDbType.Date);
                    obj_cmd.Parameters.Add("@TIMECARD_DAYTYPE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@TIMECARD_COLOR", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@TIMECARD_LOCK", SqlDbType.Bit);

                    obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime);
                    obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit);

                    obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;                    
                    obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                    obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
                    obj_cmd.Parameters["@PROJOB_CODE"].Value = model.projob_code;

                    obj_cmd.Parameters["@SHIFT_CODE"].Value = model.shift_code;
                    obj_cmd.Parameters["@TIMECARD_WORKDATE"].Value = model.timecard_workdate.Date;
                    obj_cmd.Parameters["@TIMECARD_DAYTYPE"].Value = model.timecard_daytype;
                    obj_cmd.Parameters["@TIMECARD_COLOR"].Value = model.timecard_color;
                    obj_cmd.Parameters["@TIMECARD_LOCK"].Value = false;

                    obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                    obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                    obj_cmd.Parameters["@FLAG"].Value = false;

                    obj_cmd.ExecuteNonQuery();

                    blnResult = obj_conn.doCommit();

                }
                else
                {
                    obj_conn.doRollback();
                }

            }


            catch (Exception ex)
            {
                Message = "ERROR::(Timecard.insert)" + ex.ToString();
                obj_conn.doRollback();
            }
            finally
            {
                obj_conn.doClose();
            }

            return blnResult;
        }

        public bool update(cls_TRTimecard model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();


                obj_str.Append("UPDATE ATT_TR_TIMECARD SET ");

                obj_str.Append(" SHIFT_CODE=@SHIFT_CODE ");
                obj_str.Append(", TIMECARD_DAYTYPE=@TIMECARD_DAYTYPE ");
                obj_str.Append(", TIMECARD_COLOR=@TIMECARD_COLOR ");

                obj_str.Append(", TIMECARD_LOCK=@TIMECARD_LOCK ");

                if (model.before_scan)
                {
                    obj_str.Append(", TIMECARD_CH1=@TIMECARD_CH1 ");
                    obj_str.Append(", TIMECARD_CH2=@TIMECARD_CH2 ");
                }

                if (model.work1_scan)
                {
                    obj_str.Append(", TIMECARD_CH3=@TIMECARD_CH3 ");
                    obj_str.Append(", TIMECARD_CH4=@TIMECARD_CH4 ");
                }

                if (model.break_scan)
                {
                    obj_str.Append(", TIMECARD_CH5=@TIMECARD_CH5 ");
                    obj_str.Append(", TIMECARD_CH6=@TIMECARD_CH6 ");
                }

                if (model.work2_scan)
                {
                    obj_str.Append(", TIMECARD_CH7=@TIMECARD_CH7 ");
                    obj_str.Append(", TIMECARD_CH8=@TIMECARD_CH8 ");
                }

                if (model.after_scan)
                {
                    obj_str.Append(", TIMECARD_CH9=@TIMECARD_CH9 ");
                    obj_str.Append(", TIMECARD_CH10=@TIMECARD_CH10 ");
                }

                obj_str.Append(", TIMECARD_BEFORE_MIN=@TIMECARD_BEFORE_MIN ");
                obj_str.Append(", TIMECARD_WORK1_MIN=@TIMECARD_WORK1_MIN ");
                obj_str.Append(", TIMECARD_WORK2_MIN=@TIMECARD_WORK2_MIN ");
                obj_str.Append(", TIMECARD_BREAK_MIN=@TIMECARD_BREAK_MIN ");
                obj_str.Append(", TIMECARD_AFTER_MIN=@TIMECARD_AFTER_MIN ");
                obj_str.Append(", TIMECARD_LATE_MIN=@TIMECARD_LATE_MIN ");

                obj_str.Append(", TIMECARD_BEFORE_MIN_APP=@TIMECARD_BEFORE_MIN_APP ");
                obj_str.Append(", TIMECARD_WORK1_MIN_APP=@TIMECARD_WORK1_MIN_APP ");
                obj_str.Append(", TIMECARD_WORK2_MIN_APP=@TIMECARD_WORK2_MIN_APP ");
                obj_str.Append(", TIMECARD_BREAK_MIN_APP=@TIMECARD_BREAK_MIN_APP ");
                obj_str.Append(", TIMECARD_AFTER_MIN_APP=@TIMECARD_AFTER_MIN_APP ");
                obj_str.Append(", TIMECARD_LATE_MIN_APP=@TIMECARD_LATE_MIN_APP ");


                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");


                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");                
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" AND PROJECT_CODE=@PROJECT_CODE ");
                obj_str.Append(" AND PROJOB_CODE=@PROJOB_CODE ");
                obj_str.Append(" AND TIMECARD_WORKDATE=@TIMECARD_WORKDATE ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@SHIFT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CODE"].Value = model.shift_code;
                obj_cmd.Parameters.Add("@TIMECARD_DAYTYPE", SqlDbType.VarChar); obj_cmd.Parameters["@TIMECARD_DAYTYPE"].Value = model.timecard_daytype;
                obj_cmd.Parameters.Add("@TIMECARD_COLOR", SqlDbType.VarChar); obj_cmd.Parameters["@TIMECARD_COLOR"].Value = model.timecard_color;

                obj_cmd.Parameters.Add("@TIMECARD_LOCK", SqlDbType.Bit); obj_cmd.Parameters["@TIMECARD_LOCK"].Value = model.timecard_lock;


                if (model.before_scan)
                {
                    obj_cmd.Parameters.Add("@TIMECARD_CH1", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECARD_CH1"].Value = model.timecard_ch1;
                    obj_cmd.Parameters.Add("@TIMECARD_CH2", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECARD_CH2"].Value = model.timecard_ch2;
                }

                if (model.work1_scan)
                {
                    obj_cmd.Parameters.Add("@TIMECARD_CH3", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECARD_CH3"].Value = model.timecard_ch3;
                    obj_cmd.Parameters.Add("@TIMECARD_CH4", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECARD_CH4"].Value = model.timecard_ch4;
                }

                if (model.break_scan)
                {
                    obj_cmd.Parameters.Add("@TIMECARD_CH5", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECARD_CH5"].Value = model.timecard_ch5;
                    obj_cmd.Parameters.Add("@TIMECARD_CH6", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECARD_CH6"].Value = model.timecard_ch6;
                }

                if (model.work2_scan)
                {
                    obj_cmd.Parameters.Add("@TIMECARD_CH7", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECARD_CH7"].Value = model.timecard_ch7;
                    obj_cmd.Parameters.Add("@TIMECARD_CH8", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECARD_CH8"].Value = model.timecard_ch8;
                }

                if (model.after_scan)
                {
                    obj_cmd.Parameters.Add("@TIMECARD_CH9", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECARD_CH9"].Value = model.timecard_ch9;
                    obj_cmd.Parameters.Add("@TIMECARD_CH10", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECARD_CH10"].Value = model.timecard_ch10;
                }

                obj_cmd.Parameters.Add("@TIMECARD_BEFORE_MIN", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_BEFORE_MIN"].Value = model.timecard_before_min;
                obj_cmd.Parameters.Add("@TIMECARD_WORK1_MIN", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_WORK1_MIN"].Value = model.timecard_work1_min;
                obj_cmd.Parameters.Add("@TIMECARD_WORK2_MIN", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_WORK2_MIN"].Value = model.timecard_work2_min;
                obj_cmd.Parameters.Add("@TIMECARD_BREAK_MIN", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_BREAK_MIN"].Value = model.timecard_break_min;
                obj_cmd.Parameters.Add("@TIMECARD_AFTER_MIN", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_AFTER_MIN"].Value = model.timecard_after_min;
                obj_cmd.Parameters.Add("@TIMECARD_LATE_MIN", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_LATE_MIN"].Value = model.timecard_late_min;

                obj_cmd.Parameters.Add("@TIMECARD_BEFORE_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_BEFORE_MIN_APP"].Value = model.timecard_before_min_app;
                obj_cmd.Parameters.Add("@TIMECARD_WORK1_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_WORK1_MIN_APP"].Value = model.timecard_work1_min_app;
                obj_cmd.Parameters.Add("@TIMECARD_WORK2_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_WORK2_MIN_APP"].Value = model.timecard_work2_min_app;
                obj_cmd.Parameters.Add("@TIMECARD_BREAK_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_BREAK_MIN_APP"].Value = model.timecard_break_min_app;
                obj_cmd.Parameters.Add("@TIMECARD_AFTER_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_AFTER_MIN_APP"].Value = model.timecard_after_min_app;
                obj_cmd.Parameters.Add("@TIMECARD_LATE_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_LATE_MIN_APP"].Value = model.timecard_late_min_app;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;                
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
                obj_cmd.Parameters.Add("@PROJOB_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOB_CODE"].Value = model.projob_code;

                obj_cmd.Parameters.Add("@TIMECARD_WORKDATE", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECARD_WORKDATE"].Value = model.timecard_workdate.Date;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Timecard.update)" + ex.ToString();
            }

            return blnResult;
        }

        public bool updateWithCH(cls_TRTimecard model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();


                obj_str.Append("UPDATE ATT_TR_TIMECARD SET ");

                obj_str.Append(" SHIFT_CODE=@SHIFT_CODE ");
                obj_str.Append(", TIMECARD_DAYTYPE=@TIMECARD_DAYTYPE ");
                obj_str.Append(", TIMECARD_COLOR=@TIMECARD_COLOR ");

                if (model.timecard_ch1_scan)
                    obj_str.Append(", TIMECARD_CH1=@TIMECARD_CH1 ");
                if (model.timecard_ch2_scan)
                    obj_str.Append(", TIMECARD_CH2=@TIMECARD_CH2 ");
                if (model.timecard_ch3_scan)
                    obj_str.Append(", TIMECARD_CH3=@TIMECARD_CH3 ");
                if (model.timecard_ch4_scan)
                    obj_str.Append(", TIMECARD_CH4=@TIMECARD_CH4 ");
                if (model.timecard_ch5_scan)
                    obj_str.Append(", TIMECARD_CH5=@TIMECARD_CH5 ");
                if (model.timecard_ch6_scan)
                    obj_str.Append(", TIMECARD_CH6=@TIMECARD_CH6 ");
                if (model.timecard_ch7_scan)
                    obj_str.Append(", TIMECARD_CH7=@TIMECARD_CH7 ");
                if (model.timecard_ch8_scan)
                    obj_str.Append(", TIMECARD_CH8=@TIMECARD_CH8 ");
                if (model.timecard_ch9_scan)
                    obj_str.Append(", TIMECARD_CH9=@TIMECARD_CH9 ");
                if (model.timecard_ch10_scan)
                    obj_str.Append(", TIMECARD_CH10=@TIMECARD_CH10 ");


                obj_str.Append(", TIMECARD_BEFORE_MIN=@TIMECARD_BEFORE_MIN ");
                obj_str.Append(", TIMECARD_WORK1_MIN=@TIMECARD_WORK1_MIN ");
                obj_str.Append(", TIMECARD_WORK2_MIN=@TIMECARD_WORK2_MIN ");
                obj_str.Append(", TIMECARD_BREAK_MIN=@TIMECARD_BREAK_MIN ");
                obj_str.Append(", TIMECARD_AFTER_MIN=@TIMECARD_AFTER_MIN ");
                obj_str.Append(", TIMECARD_LATE_MIN=@TIMECARD_LATE_MIN ");

                obj_str.Append(", TIMECARD_BEFORE_MIN_APP=@TIMECARD_BEFORE_MIN_APP ");
                obj_str.Append(", TIMECARD_WORK1_MIN_APP=@TIMECARD_WORK1_MIN_APP ");
                obj_str.Append(", TIMECARD_WORK2_MIN_APP=@TIMECARD_WORK2_MIN_APP ");
                obj_str.Append(", TIMECARD_BREAK_MIN_APP=@TIMECARD_BREAK_MIN_APP ");
                obj_str.Append(", TIMECARD_AFTER_MIN_APP=@TIMECARD_AFTER_MIN_APP ");
                obj_str.Append(", TIMECARD_LATE_MIN_APP=@TIMECARD_LATE_MIN_APP ");


                obj_str.Append(", TIMECARD_LEAVEDEDUCT_MIN=@TIMECARD_LEAVEDEDUCT_MIN ");


                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");


                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");                
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" AND PROJECT_CODE=@PROJECT_CODE ");
                obj_str.Append(" AND PROJOB_CODE=@PROJOB_CODE ");

                obj_str.Append(" AND TIMECARD_WORKDATE=@TIMECARD_WORKDATE ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@SHIFT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CODE"].Value = model.shift_code;
                obj_cmd.Parameters.Add("@TIMECARD_DAYTYPE", SqlDbType.VarChar); obj_cmd.Parameters["@TIMECARD_DAYTYPE"].Value = model.timecard_daytype;
                obj_cmd.Parameters.Add("@TIMECARD_COLOR", SqlDbType.VarChar); obj_cmd.Parameters["@TIMECARD_COLOR"].Value = model.timecard_color;

                if (model.timecard_ch1_scan)
                {
                    obj_cmd.Parameters.Add("@TIMECARD_CH1", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECARD_CH1"].Value = model.timecard_ch1;
                }
                if (model.timecard_ch2_scan)
                {
                    obj_cmd.Parameters.Add("@TIMECARD_CH2", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECARD_CH2"].Value = model.timecard_ch2;
                }
                if (model.timecard_ch3_scan)
                {
                    obj_cmd.Parameters.Add("@TIMECARD_CH3", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECARD_CH3"].Value = model.timecard_ch3;
                }
                if (model.timecard_ch4_scan)
                {
                    obj_cmd.Parameters.Add("@TIMECARD_CH4", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECARD_CH4"].Value = model.timecard_ch4;
                }
                if (model.timecard_ch5_scan)
                {
                    obj_cmd.Parameters.Add("@TIMECARD_CH5", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECARD_CH5"].Value = model.timecard_ch5;
                }
                if (model.timecard_ch6_scan)
                {
                    obj_cmd.Parameters.Add("@TIMECARD_CH6", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECARD_CH6"].Value = model.timecard_ch6;
                }
                if (model.timecard_ch7_scan)
                {
                    obj_cmd.Parameters.Add("@TIMECARD_CH7", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECARD_CH7"].Value = model.timecard_ch7;
                }
                if (model.timecard_ch8_scan)
                {
                    obj_cmd.Parameters.Add("@TIMECARD_CH8", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECARD_CH8"].Value = model.timecard_ch8;
                }
                if (model.timecard_ch9_scan)
                {
                    obj_cmd.Parameters.Add("@TIMECARD_CH9", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECARD_CH9"].Value = model.timecard_ch9;
                }
                if (model.timecard_ch10_scan)
                {
                    obj_cmd.Parameters.Add("@TIMECARD_CH10", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECARD_CH10"].Value = model.timecard_ch10;
                }


                obj_cmd.Parameters.Add("@TIMECARD_BEFORE_MIN", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_BEFORE_MIN"].Value = model.timecard_before_min;
                obj_cmd.Parameters.Add("@TIMECARD_WORK1_MIN", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_WORK1_MIN"].Value = model.timecard_work1_min;
                obj_cmd.Parameters.Add("@TIMECARD_WORK2_MIN", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_WORK2_MIN"].Value = model.timecard_work2_min;
                obj_cmd.Parameters.Add("@TIMECARD_BREAK_MIN", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_BREAK_MIN"].Value = model.timecard_break_min;
                obj_cmd.Parameters.Add("@TIMECARD_AFTER_MIN", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_AFTER_MIN"].Value = model.timecard_after_min;
                obj_cmd.Parameters.Add("@TIMECARD_LATE_MIN", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_LATE_MIN"].Value = model.timecard_late_min;

                obj_cmd.Parameters.Add("@TIMECARD_BEFORE_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_BEFORE_MIN_APP"].Value = model.timecard_before_min_app;
                obj_cmd.Parameters.Add("@TIMECARD_WORK1_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_WORK1_MIN_APP"].Value = model.timecard_work1_min_app;
                obj_cmd.Parameters.Add("@TIMECARD_WORK2_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_WORK2_MIN_APP"].Value = model.timecard_work2_min_app;
                obj_cmd.Parameters.Add("@TIMECARD_BREAK_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_BREAK_MIN_APP"].Value = model.timecard_break_min_app;
                obj_cmd.Parameters.Add("@TIMECARD_AFTER_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_AFTER_MIN_APP"].Value = model.timecard_after_min_app;
                obj_cmd.Parameters.Add("@TIMECARD_LATE_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_LATE_MIN_APP"].Value = model.timecard_late_min_app;

                obj_cmd.Parameters.Add("@TIMECARD_LEAVEDEDUCT_MIN", SqlDbType.Int); obj_cmd.Parameters["@TIMECARD_LEAVEDEDUCT_MIN"].Value = model.timecard_leavededuct_min;



                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;                
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
                obj_cmd.Parameters.Add("@PROJOB_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOB_CODE"].Value = model.projob_code;

                obj_cmd.Parameters.Add("@TIMECARD_WORKDATE", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECARD_WORKDATE"].Value = model.timecard_workdate.Date;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Timecard.update)" + ex.ToString();
            }

            return blnResult;
        }

        public bool clearCH(cls_TRTimecard model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();


                obj_str.Append("UPDATE ATT_TR_TIMECARD SET ");

                obj_str.Append("TIMECARD_BEFORE_MIN=0 ");
                obj_str.Append(", TIMECARD_WORK1_MIN=0 ");
                obj_str.Append(", TIMECARD_WORK2_MIN=0 ");
                obj_str.Append(", TIMECARD_BREAK_MIN=0 ");
                obj_str.Append(", TIMECARD_AFTER_MIN=0 ");
                obj_str.Append(", TIMECARD_LATE_MIN=0 ");

                obj_str.Append(", TIMECARD_BEFORE_MIN_APP=0 ");
                obj_str.Append(", TIMECARD_WORK1_MIN_APP=0 ");
                obj_str.Append(", TIMECARD_WORK2_MIN_APP=0 ");
                obj_str.Append(", TIMECARD_BREAK_MIN_APP=0 ");
                obj_str.Append(", TIMECARD_AFTER_MIN_APP=0 ");
                obj_str.Append(", TIMECARD_LATE_MIN_APP=0 ");
                obj_str.Append(", TIMECARD_LEAVEDEDUCT_MIN=0 ");

                for (int i = 1; i <= 10; i++)
                {
                    obj_str.Append(", TIMECARD_CH" + i.ToString() + "=NULL ");
                }



                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");                
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");

                obj_str.Append(" AND PROJECT_CODE=@PROJECT_CODE ");
                obj_str.Append(" AND PROJOB_CODE=@PROJOB_CODE ");

                obj_str.Append(" AND TIMECARD_WORKDATE=@TIMECARD_WORKDATE ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;                
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
                obj_cmd.Parameters.Add("@PROJOB_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOB_CODE"].Value = model.projob_code;

                obj_cmd.Parameters.Add("@TIMECARD_WORKDATE", SqlDbType.DateTime); obj_cmd.Parameters["@TIMECARD_WORKDATE"].Value = model.timecard_workdate.Date;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Timecard.update)" + ex.ToString();
            }

            return blnResult;
        }

    }
}