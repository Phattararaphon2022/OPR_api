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
   public class cls_ctTRLostwages
   {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRLostwages() { }

        public string getMessage() { return this.Message; }

        private string FormatDateDB = "MM/dd/yyyy";

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRLostwages> getData(string condition)
        {
            List<cls_TRLostwages> list_model = new List<cls_TRLostwages>();
            cls_TRLostwages model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("ATT_TR_LOSTWAGES.COMPANY_CODE");
                obj_str.Append(", ATT_TR_LOSTWAGES.WORKER_CODE");

                obj_str.Append(", ATT_TR_LOSTWAGES.PROJECT_CODE");
                obj_str.Append(", ATT_TR_LOSTWAGES.PROJOB_CODE");
                //
                obj_str.Append(", ATT_TR_LOSTWAGES.LOSTWAGES_STATUS");

                obj_str.Append(", ATT_TR_LOSTWAGES.LOSTWAGES_INITIAL");
                obj_str.Append(", ATT_TR_LOSTWAGES.LOSTWAGES_CARDNO");
                obj_str.Append(", ATT_TR_LOSTWAGES.LOSTWAGES_GENDER");
                obj_str.Append(", ISNULL(ATT_TR_LOSTWAGES.LOSTWAGES_FNAME_TH, '') AS LOSTWAGES_FNAME_TH");
                obj_str.Append(", ISNULL(ATT_TR_LOSTWAGES.LOSTWAGES_LNAME_TH, '') AS LOSTWAGES_LNAME_TH");
 

                obj_str.Append(", LOSTWAGES_SALARY");
                obj_str.Append(", LOSTWAGES_DILIGENCE");
                obj_str.Append(", LOSTWAGES_TRAVELEXPENSES");
                obj_str.Append(", LOSTWAGES_OTHER");
                //
 
                obj_str.Append(", SHIFT_CODE");
                obj_str.Append(", LOSTWAGES_WORKDATE");
                obj_str.Append(", LOSTWAGES_DAYTYPE");
                obj_str.Append(", LOSTWAGES_COLOR");
                obj_str.Append(", ISNULL(LOSTWAGES_CH1, LOSTWAGES_WORKDATE) AS LOSTWAGES_CH1");
                obj_str.Append(", ISNULL(LOSTWAGES_CH2, LOSTWAGES_WORKDATE) AS LOSTWAGES_CH2");
                obj_str.Append(", ISNULL(LOSTWAGES_CH3, LOSTWAGES_WORKDATE) AS LOSTWAGES_CH3");
                obj_str.Append(", ISNULL(LOSTWAGES_CH4, LOSTWAGES_WORKDATE) AS LOSTWAGES_CH4");
                obj_str.Append(", ISNULL(LOSTWAGES_CH5, LOSTWAGES_WORKDATE) AS LOSTWAGES_CH5");
                obj_str.Append(", ISNULL(LOSTWAGES_CH6, LOSTWAGES_WORKDATE) AS LOSTWAGES_CH6");
                obj_str.Append(", ISNULL(LOSTWAGES_CH7, LOSTWAGES_WORKDATE) AS LOSTWAGES_CH7");
                obj_str.Append(", ISNULL(LOSTWAGES_CH8, LOSTWAGES_WORKDATE) AS LOSTWAGES_CH8");
                obj_str.Append(", ISNULL(LOSTWAGES_CH9, LOSTWAGES_WORKDATE) AS LOSTWAGES_CH9");
                obj_str.Append(", ISNULL(LOSTWAGES_CH10, LOSTWAGES_WORKDATE) AS LOSTWAGES_CH10");

                obj_str.Append(", ISNULL(LOSTWAGES_BEFORE_MIN, 0) AS LOSTWAGES_BEFORE_MIN");
                obj_str.Append(", ISNULL(LOSTWAGES_WORK1_MIN, 0) AS LOSTWAGES_WORK1_MIN");
                obj_str.Append(", ISNULL(LOSTWAGES_WORK2_MIN, 0) AS LOSTWAGES_WORK2_MIN");
                obj_str.Append(", ISNULL(LOSTWAGES_BREAK_MIN, 0) AS LOSTWAGES_BREAK_MIN");
                obj_str.Append(", ISNULL(LOSTWAGES_AFTER_MIN, 0) AS LOSTWAGES_AFTER_MIN");
                obj_str.Append(", ISNULL(LOSTWAGES_LATE_MIN, 0) AS LOSTWAGES_LATE_MIN");

                obj_str.Append(", ISNULL(LOSTWAGES_BEFORE_MIN_APP, 0) AS LOSTWAGES_BEFORE_MIN_APP");
                obj_str.Append(", ISNULL(LOSTWAGES_WORK1_MIN_APP, 0) AS LOSTWAGES_WORK1_MIN_APP");
                obj_str.Append(", ISNULL(LOSTWAGES_WORK2_MIN_APP, 0) AS LOSTWAGES_WORK2_MIN_APP");
                obj_str.Append(", ISNULL(LOSTWAGES_BREAK_MIN_APP, 0) AS LOSTWAGES_BREAK_MIN_APP");
                obj_str.Append(", ISNULL(LOSTWAGES_AFTER_MIN_APP, 0) AS LOSTWAGES_AFTER_MIN_APP");
                obj_str.Append(", ISNULL(LOSTWAGES_LATE_MIN_APP, 0) AS LOSTWAGES_LATE_MIN_APP");

                obj_str.Append(", ISNULL(LOSTWAGES_LEAVEPAY_MIN, 0) AS LOSTWAGES_LEAVEPAY_MIN");
                obj_str.Append(", ISNULL(LOSTWAGES_LEAVEDEDUCT_MIN, 0) AS LOSTWAGES_LEAVEDEDUCT_MIN");
                obj_str.Append(", ISNULL(LOSTWAGES_BEFORE_DG, 0) AS LOSTWAGES_BEFORE_DG");
                obj_str.Append(", ISNULL(LOSTWAGES_AFTER_DG, 0) AS LOSTWAGES_AFTER_DG");
                obj_str.Append(", ISNULL(LOSTWAGES_LOCK, 0) AS LOSTWAGES_LOCK");

                obj_str.Append(", ISNULL(ATT_TR_LOSTWAGES.MODIFIED_BY, ATT_TR_LOSTWAGES.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(ATT_TR_LOSTWAGES.MODIFIED_DATE, ATT_TR_LOSTWAGES.CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(", ISNULL(EMP_MT_WORKER.WORKER_FNAME_TH, '') + ' ' + ISNULL(EMP_MT_WORKER.WORKER_LNAME_TH, '') AS WORKER_NAME_TH");
                obj_str.Append(", ISNULL(EMP_MT_WORKER.WORKER_FNAME_EN, '') + ' ' + ISNULL(EMP_MT_WORKER.WORKER_LNAME_EN, '') AS WORKER_NAME_EN");

                obj_str.Append(", ISNULL(EMP_MT_WORKER.WORKER_CARDNO, 0) AS WORKER_CARDNO ");

              
                obj_str.Append(" FROM ATT_TR_LOSTWAGES");
                obj_str.Append(" LEFT JOIN EMP_MT_WORKER ON ATT_TR_LOSTWAGES.COMPANY_CODE = EMP_MT_WORKER.COMPANY_CODE AND ATT_TR_LOSTWAGES.WORKER_CODE = EMP_MT_WORKER.WORKER_CODE");

                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY ATT_TR_LOSTWAGES.COMPANY_CODE, ATT_TR_LOSTWAGES.WORKER_CODE, ATT_TR_LOSTWAGES.LOSTWAGES_WORKDATE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRLostwages();

                    model.company_code = dr["COMPANY_CODE"].ToString();                    
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.project_code = dr["PROJECT_CODE"].ToString();
                    model.projob_code = dr["PROJOB_CODE"].ToString();
                    //
                    model.lostwages_status = dr["LOSTWAGES_STATUS"].ToString();
                    //
                    model.lostwages_initial = dr["LOSTWAGES_INITIAL"].ToString();
                    model.lostwages_cardno = dr["LOSTWAGES_CARDNO"].ToString();
                    model.lostwages_gender = dr["LOSTWAGES_GENDER"].ToString();
                    model.lostwages_fname_th = dr["LOSTWAGES_FNAME_TH"].ToString();
                    model.lostwages_laname_th = dr["LOSTWAGES_LNAME_TH"].ToString();

                    //

                    model.lostwages_salary = dr["LOSTWAGES_SALARY"].ToString();
                    model.lostwages_diligence = dr["LOSTWAGES_DILIGENCE"].ToString();
                    model.lostwages_travelexpenses = dr["LOSTWAGES_TRAVELEXPENSES"].ToString();
                    model.lostwages_other = dr["LOSTWAGES_OTHER"].ToString();
                    //
                    
                    model.shift_code = dr["SHIFT_CODE"].ToString();

                    model.lostwages_workdate = Convert.ToDateTime(dr["LOSTWAGES_WORKDATE"]);

                    model.lostwages_daytype = dr["LOSTWAGES_DAYTYPE"].ToString();
                    model.lostwages_color = dr["LOSTWAGES_COLOR"].ToString();

                    model.lostwages_ch1 = Convert.ToDateTime(dr["LOSTWAGES_CH1"]);
                    model.lostwages_ch2 = Convert.ToDateTime(dr["LOSTWAGES_CH2"]);
                    model.lostwages_ch3 = Convert.ToDateTime(dr["LOSTWAGES_CH3"]);
                    model.lostwages_ch4 = Convert.ToDateTime(dr["LOSTWAGES_CH4"]);
                    model.lostwages_ch5 = Convert.ToDateTime(dr["LOSTWAGES_CH5"]);
                    model.lostwages_ch6 = Convert.ToDateTime(dr["LOSTWAGES_CH6"]);
                    model.lostwages_ch7 = Convert.ToDateTime(dr["LOSTWAGES_CH7"]);
                    model.lostwages_ch8 = Convert.ToDateTime(dr["LOSTWAGES_CH8"]);
                    model.lostwages_ch9 = Convert.ToDateTime(dr["LOSTWAGES_CH9"]);
                    model.lostwages_ch10 = Convert.ToDateTime(dr["LOSTWAGES_CH10"]);

                    model.lostwages_before_min = Convert.ToInt32(dr["LOSTWAGES_BEFORE_MIN"]);
                    model.lostwages_work1_min = Convert.ToInt32(dr["LOSTWAGES_WORK1_MIN"]);
                    model.lostwages_work2_min = Convert.ToInt32(dr["LOSTWAGES_WORK2_MIN"]);
                    model.lostwages_break_min = Convert.ToInt32(dr["LOSTWAGES_BREAK_MIN"]);
                    model.lostwages_after_min = Convert.ToInt32(dr["LOSTWAGES_AFTER_MIN"]);
                    model.lostwages_late_min = Convert.ToInt32(dr["LOSTWAGES_LATE_MIN"]);

                    model.lostwages_before_min_app = Convert.ToInt32(dr["LOSTWAGES_BEFORE_MIN_APP"]);
                    model.lostwages_work1_min_app = Convert.ToInt32(dr["LOSTWAGES_WORK1_MIN_APP"]);
                    model.lostwages_work2_min_app = Convert.ToInt32(dr["LOSTWAGES_WORK2_MIN_APP"]);
                    model.lostwages_break_min_app = Convert.ToInt32(dr["LOSTWAGES_BREAK_MIN_APP"]);
                    model.lostwages_after_min_app = Convert.ToInt32(dr["LOSTWAGES_AFTER_MIN_APP"]);
                    model.lostwages_late_min_app = Convert.ToInt32(dr["LOSTWAGES_LATE_MIN_APP"]);


                    model.lostwages_leavepay_min = Convert.ToInt32(dr["LOSTWAGES_LEAVEPAY_MIN"]);
                    model.lostwages_leavededuct_min = Convert.ToInt32(dr["LOSTWAGES_LEAVEDEDUCT_MIN"]);
                    model.lostwages_before_dg = Convert.ToBoolean(dr["LOSTWAGES_BEFORE_DG"]);
                    model.lostwages_after_dg = Convert.ToBoolean(dr["LOSTWAGES_AFTER_DG"]);
                    model.lostwages_lock = Convert.ToBoolean(dr["LOSTWAGES_LOCK"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                    model.worker_name_th = dr["WORKER_NAME_TH"].ToString();
                    model.worker_name_en = dr["WORKER_NAME_EN"].ToString();
                    model.worker_cardno = dr["WORKER_CARDNO"].ToString();

                    
                    
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(LOSTWAGES.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRLostwages> getDataByFilltercardnos(string com, string project, string cardno, DateTime fromdate, DateTime todate)
        {
            string strCondition = "";

            strCondition += " AND ATT_TR_LOSTWAGES.COMPANY_CODE='" + com + "'";

            if (!project.Equals(""))
                strCondition += " AND ATT_TR_LOSTWAGES.PROJECT_CODE='" + project + "'";

            strCondition += " AND (LOSTWAGES_WORKDATE BETWEEN '" + fromdate.ToString(this.FormatDateDB) + "' AND '" + todate.ToString(this.FormatDateDB) + "' )";

            if (!cardno.Equals(""))
                strCondition += " AND ATT_TR_LOSTWAGES.LOSTWAGES_CARDNO='" + cardno + "'";

            return this.getData(strCondition);
        }



        public List<cls_TRLostwages> getDataByFillter(string com, string project, string worker, string cardno, DateTime fromdate, DateTime todate)
        {
            string strCondition = "";

            strCondition += " AND ATT_TR_LOSTWAGES.COMPANY_CODE='" + com + "'";

            if (!project.Equals(""))
                strCondition += " AND ATT_TR_LOSTWAGES.PROJECT_CODE='" + project + "'";


            if (!worker.Equals(""))
                strCondition += " AND ATT_TR_LOSTWAGES.WORKER_CODE='" + worker + "'";
            if (!cardno.Equals(""))
                strCondition += " AND ATT_TR_LOSTWAGES.LOSTWAGES_CARDNO='" + cardno + "'";

            strCondition += " AND (LOSTWAGES_WORKDATE BETWEEN '" + fromdate.ToString(this.FormatDateDB) + "' AND '" + todate.ToString(this.FormatDateDB) + "' )";


            return this.getData(strCondition);
        }

        public List<cls_TRLostwages> getDataByFilltercardno(string com, string cardno)
        {
            string strCondition = "";

            strCondition += " AND ATT_TR_LOSTWAGES.COMPANY_CODE='" + com + "'";

            if (!cardno.Equals(""))
                strCondition += " AND ATT_TR_LOSTWAGES.LOSTWAGES_CARDNO='" + cardno + "'";

            return this.getData(strCondition);
        }
        public List<cls_TRLostwages> getDataByCompanyCode(string com)
        {
            return this.getData(" AND ATT_TR_LOSTWAGES.COMPANY_CODE='" + com + "'");
        }
        public List<cls_TRLostwages> getDataByJob(string com, string project, string job, DateTime fromdate, DateTime todate)
        {
            string strCondition = "";

            strCondition += " AND ATT_TR_LOSTWAGES.COMPANY_CODE='" + com + "'";
            strCondition += " AND ATT_TR_LOSTWAGES.PROJECT_CODE='" + project + "'";
            strCondition += " AND ATT_TR_LOSTWAGES.PROJOB_CODE='" + job + "'";
            strCondition += " AND (LOSTWAGES_WORKDATE BETWEEN '" + fromdate.ToString(this.FormatDateDB) + "' AND '" + todate.ToString(this.FormatDateDB) + "' )";

            

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string project, string job, string status ,string cardno, string worker, DateTime workdate)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT LOSTWAGES_WORKDATE");
                obj_str.Append(" FROM ATT_TR_LOSTWAGES");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROJOB_CODE='" + job + "'");

                obj_str.Append(" AND LOSTWAGES_STATUS='" + status + "'");

                obj_str.Append(" AND LOSTWAGES_CARDNO='" + cardno + "'");

                obj_str.Append(" AND WORKER_CODE='" + worker + "'");
                obj_str.Append(" AND LOSTWAGES_WORKDATE='" + workdate.ToString(this.FormatDateDB) + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(LOSTWAGES.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string project, string worker, string cardno)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_LOSTWAGES");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND WORKER_CODE='" + worker + "'");
                obj_str.Append(" AND LOSTWAGES_CARDNO='" + cardno + "'");

 
                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(LOSTWAGES.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete_nonproject(string com, string worker, DateTime workdate)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_LOSTWAGES");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");                
                obj_str.Append(" AND WORKER_CODE='" + worker + "'");

                obj_str.Append(" AND LOSTWAGES_WORKDATE='" + workdate.ToString(this.FormatDateDB) + "'");
                obj_str.Append(" AND (PROJECT_CODE IS NULL OR PROJECT_CODE='')");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(LOSTWAGES.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool clear(string com, string worker, string cardno ,DateTime fromdate, DateTime todate)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_LOSTWAGES");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND WORKER_CODE='" + worker + "'");
                obj_str.Append(" AND LOSTWAGES_CARDNO='" + cardno + "'");

                obj_str.Append(" AND (LOSTWAGES_WORKDATE BETWEEN '" + fromdate.ToString(this.FormatDateDB) + "' AND '" + todate.ToString(this.FormatDateDB) + "' )");


                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(LOSTWAGES.clear)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert_plantime(string com, string project, string worker, string cardno,DateTime fromdate, DateTime todate, List<cls_TRLostwages> list_model)
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

                obj_str.Append(" DELETE FROM ATT_TR_LOSTWAGES");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND WORKER_CODE='" + worker + "'");
                obj_str.Append(" AND LOSTWAGES_CARDNO='" + cardno + "'");

                obj_str.Append(" AND (LOSTWAGES_WORKDATE BETWEEN '" + fromdate.ToString(this.FormatDateDB) + "' AND '" + todate.ToString(this.FormatDateDB) + "' )");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str.ToString());

                //-- Step 2 insert
                if (blnResult)
                {
                    obj_str = new System.Text.StringBuilder();
                    obj_str.Append("INSERT INTO ATT_TR_LOSTWAGES");
                    obj_str.Append(" (");
                    obj_str.Append("COMPANY_CODE ");                    
                    obj_str.Append(", WORKER_CODE ");
                    obj_str.Append(", PROJECT_CODE ");
                    obj_str.Append(", PROJOB_CODE ");
                    obj_str.Append(", LOSTWAGES_STATUS ");
                    //
                    obj_str.Append(", LOSTWAGES_INITIAL ");
                    obj_str.Append(", LOSTWAGES_CARDNO ");
                    obj_str.Append(", LOSTWAGES_GENDER ");
                    obj_str.Append(", LOSTWAGES_FNAME_TH ");
                    obj_str.Append(", LOSTWAGES_LNAME_TH ");
                    //
                    
                    obj_str.Append(", SHIFT_CODE ");
                    obj_str.Append(", LOSTWAGES_WORKDATE ");
                    obj_str.Append(", LOSTWAGES_DAYTYPE ");
                    obj_str.Append(", LOSTWAGES_COLOR ");
                    obj_str.Append(", LOSTWAGES_LOCK ");
                    obj_str.Append(", CREATED_BY ");
                    obj_str.Append(", CREATED_DATE ");
                    obj_str.Append(", FLAG ");
                    obj_str.Append(" )");

                    obj_str.Append(" VALUES(");
                    obj_str.Append("@COMPANY_CODE ");                    
                    obj_str.Append(", @WORKER_CODE ");
                    obj_str.Append(", @PROJECT_CODE ");
                    obj_str.Append(", @PROJOB_CODE ");
                    obj_str.Append(", @LOSTWAGES_STATUS ");
                    //
                    obj_str.Append(", @LOSTWAGES_INITIAL ");
                    obj_str.Append(", @LOSTWAGES_CARDNO ");
                    obj_str.Append(", @LOSTWAGES_GENDER ");
                    obj_str.Append(", @LOSTWAGES_FNAME_TH ");
                    obj_str.Append(", @LOSTWAGES_LNAME_TH ");
                    //
                    
                    obj_str.Append(", @SHIFT_CODE ");
                    obj_str.Append(", @LOSTWAGES_WORKDATE ");
                    obj_str.Append(", @LOSTWAGES_DAYTYPE ");
                    obj_str.Append(", @LOSTWAGES_COLOR ");
                    obj_str.Append(", @LOSTWAGES_LOCK ");
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

                    obj_cmd.Parameters.Add("@LOSTWAGES_STATUS", SqlDbType.VarChar);
                    //
                    obj_cmd.Parameters.Add("@LOSTWAGES_INITIAL ", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@LOSTWAGES_CARDNO ", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@LOSTWAGES_GENDER ", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@LOSTWAGES_FNAME_TH ", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@LOSTWAGES_LNAME_TH ", SqlDbType.VarChar);
                    //
                    obj_cmd.Parameters.Add("@SHIFT_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@LOSTWAGES_WORKDATE", SqlDbType.Date);
                    obj_cmd.Parameters.Add("@LOSTWAGES_DAYTYPE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@LOSTWAGES_COLOR", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@LOSTWAGES_LOCK", SqlDbType.Bit);

                    obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime);
                    obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit);


                    foreach (cls_TRLostwages model in list_model)
                    {

                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;                        
                        obj_cmd.Parameters["@WORKER_CODE"].Value = worker;

                        obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code == null ? "" : model.project_code;
                        obj_cmd.Parameters["@PROJOB_CODE"].Value = model.projob_code == null ? "" : model.projob_code;
                        obj_cmd.Parameters["@LOSTWAGES_STATUS"].Value = model.lostwages_status;
                        //
                        obj_cmd.Parameters["@LOSTWAGES_INITIAL"].Value = model.lostwages_initial;
                        obj_cmd.Parameters["@LOSTWAGES_CARDNO"].Value = cardno;
                        obj_cmd.Parameters["@LOSTWAGES_GENDER"].Value = model.lostwages_gender;
                        obj_cmd.Parameters["@LOSTWAGES_FNAME_TH"].Value = model.lostwages_fname_th;
                        obj_cmd.Parameters["@LOSTWAGES_LNAME_TH"].Value = model.lostwages_laname_th;

                        //
                        
                        obj_cmd.Parameters["@SHIFT_CODE"].Value = model.shift_code;
                        obj_cmd.Parameters["@LOSTWAGES_WORKDATE"].Value = model.lostwages_workdate.Date;
                        obj_cmd.Parameters["@LOSTWAGES_DAYTYPE"].Value = model.lostwages_daytype;
                        obj_cmd.Parameters["@LOSTWAGES_COLOR"].Value = model.lostwages_color;
                        obj_cmd.Parameters["@LOSTWAGES_LOCK"].Value = false;

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
                Message = "ERROR::(LOSTWAGES.insert_plantime)" + ex.ToString();
                obj_conn.doRollback();
            }
            finally
            {
                obj_conn.doClose();
            }

            return blnResult;
        }

        public bool insert(cls_TRLostwages model)
        {
            bool blnResult = false;
            cls_ctConnection obj_conn = new cls_ctConnection();
            try
            {
                if (this.checkDataOld(model.company_code, model.project_code, model.projob_code, model.lostwages_status, model.lostwages_cardno, model.worker_code, model.lostwages_workdate))
                    return true;


                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();


                //-- Step 2 insert
                if (true)
                {
                    obj_str = new System.Text.StringBuilder();
                    obj_str.Append("INSERT INTO ATT_TR_LOSTWAGES");
                    obj_str.Append(" (");
                    obj_str.Append("COMPANY_CODE ");                    
                    obj_str.Append(", WORKER_CODE ");
                    obj_str.Append(", PROJECT_CODE ");
                    obj_str.Append(", PROJOB_CODE ");
                    obj_str.Append(", LOSTWAGES_STATUS ");
                    //
                    obj_str.Append(", LOSTWAGES_INITIAL ");
                    obj_str.Append(", LOSTWAGES_CARDNO ");
                    obj_str.Append(", LOSTWAGES_GENDER ");
                    obj_str.Append(", LOSTWAGES_FNAME_TH ");
                    obj_str.Append(", LOSTWAGES_LNAME_TH ");
                    //
                    
                    obj_str.Append(", SHIFT_CODE ");
                    obj_str.Append(", LOSTWAGES_WORKDATE ");
                    obj_str.Append(", LOSTWAGES_DAYTYPE ");
                    obj_str.Append(", LOSTWAGES_COLOR ");
                    obj_str.Append(", LOSTWAGES_LOCK ");
                    obj_str.Append(", CREATED_BY ");
                    obj_str.Append(", CREATED_DATE ");
                    obj_str.Append(", FLAG ");
                    obj_str.Append(" )");

                    obj_str.Append(" VALUES(");
                    obj_str.Append("@COMPANY_CODE ");                    
                    obj_str.Append(", @WORKER_CODE ");

                    obj_str.Append(", @PROJECT_CODE ");
                    obj_str.Append(", @PROJOB_CODE ");
                    obj_str.Append(", @LOSTWAGES_STATUS ");

                    //
                    obj_str.Append(", @LOSTWAGES_INITIAL ");
                    obj_str.Append(", @LOSTWAGES_CARDNO ");
                    obj_str.Append(", @LOSTWAGES_GENDER ");
                    obj_str.Append(", @LOSTWAGES_FNAME_TH ");
                    obj_str.Append(", @LOSTWAGES_LNAME_TH ");
                    //
                    obj_str.Append(", @SHIFT_CODE ");
                    obj_str.Append(", @LOSTWAGES_WORKDATE ");
                    obj_str.Append(", @LOSTWAGES_DAYTYPE ");
                    obj_str.Append(", @LOSTWAGES_COLOR ");
                    obj_str.Append(", @LOSTWAGES_LOCK ");
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
                    obj_cmd.Parameters.Add("@LOSTWAGES_STATUS", SqlDbType.VarChar);

                    //
                    obj_cmd.Parameters.Add("@LOSTWAGES_INITIAL", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@LOSTWAGES_CARDNO", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@LOSTWAGES_GENDER", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@LOSTWAGES_FNAME_TH", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@LOSTWAGES_LNAME_TH", SqlDbType.VarChar);

                    //


                    
                    obj_cmd.Parameters.Add("@SHIFT_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@LOSTWAGES_WORKDATE", SqlDbType.Date);
                    obj_cmd.Parameters.Add("@LOSTWAGES_DAYTYPE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@LOSTWAGES_COLOR", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@LOSTWAGES_LOCK", SqlDbType.Bit);

                    obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime);
                    obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit);


                    //
                    obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;                    
                    obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                    obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
                    obj_cmd.Parameters["@PROJOB_CODE"].Value = model.projob_code;
                    obj_cmd.Parameters["@LOSTWAGES_STATUS"].Value = model.lostwages_status;
                    //
                    obj_cmd.Parameters["@LOSTWAGES_INITIAL"].Value = model.lostwages_initial;
                    obj_cmd.Parameters["@LOSTWAGES_CARDNO"].Value = model.lostwages_cardno;
                    obj_cmd.Parameters["@LOSTWAGES_GENDER"].Value = model.lostwages_gender;
                    obj_cmd.Parameters["@LOSTWAGES_FNAME_TH"].Value = model.lostwages_fname_th;
                    obj_cmd.Parameters["@LOSTWAGES_LNAME_TH"].Value = model.lostwages_laname_th;

                    //
                    
                    obj_cmd.Parameters["@SHIFT_CODE"].Value = model.shift_code;
                    obj_cmd.Parameters["@LOSTWAGES_WORKDATE"].Value = model.lostwages_workdate.Date;
                    obj_cmd.Parameters["@LOSTWAGES_DAYTYPE"].Value = model.lostwages_daytype;
                    obj_cmd.Parameters["@LOSTWAGES_COLOR"].Value = model.lostwages_color;
                    obj_cmd.Parameters["@LOSTWAGES_LOCK"].Value = false;

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
                Message = "ERROR::(LOSTWAGES.insert)" + ex.ToString();
                obj_conn.doRollback();
            }
            finally
            {
                obj_conn.doClose();
            }

            return blnResult;
        }

        public bool update(cls_TRLostwages model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();


                obj_str.Append("UPDATE ATT_TR_LOSTWAGES SET ");
                //
                obj_str.Append(" LOSTWAGES_SALARY=@LOSTWAGES_SALARY ");
                obj_str.Append(", LOSTWAGES_DILIGENCE=@LOSTWAGES_DILIGENCE ");
                obj_str.Append(", LOSTWAGES_TRAVELEXPENSES=@LOSTWAGES_TRAVELEXPENSES ");
                obj_str.Append(", LOSTWAGES_OTHER=@LOSTWAGES_OTHER ");

                //
                obj_str.Append(" ,SHIFT_CODE=@SHIFT_CODE ");
                obj_str.Append(", LOSTWAGES_DAYTYPE=@LOSTWAGES_DAYTYPE ");
                obj_str.Append(", LOSTWAGES_COLOR=@LOSTWAGES_COLOR ");

                obj_str.Append(", LOSTWAGES_LOCK=@LOSTWAGES_LOCK ");

                if (model.before_scan)
                {
                    obj_str.Append(", LOSTWAGES_CH1=@LOSTWAGES_CH1 ");
                    obj_str.Append(", LOSTWAGES_CH2=@LOSTWAGES_CH2 ");
                }

                if (model.work1_scan)
                {
                    obj_str.Append(", LOSTWAGES_CH3=@LOSTWAGES_CH3 ");
                    obj_str.Append(", LOSTWAGES_CH4=@LOSTWAGES_CH4 ");
                }

                if (model.break_scan)
                {
                    obj_str.Append(", LOSTWAGES_CH5=@LOSTWAGES_CH5 ");
                    obj_str.Append(", LOSTWAGES_CH6=@LOSTWAGES_CH6 ");
                }

                if (model.work2_scan)
                {
                    obj_str.Append(", LOSTWAGES_CH7=@LOSTWAGES_CH7 ");
                    obj_str.Append(", LOSTWAGES_CH8=@LOSTWAGES_CH8 ");
                }

                if (model.after_scan)
                {
                    obj_str.Append(", LOSTWAGES_CH9=@LOSTWAGES_CH9 ");
                    obj_str.Append(", LOSTWAGES_CH10=@LOSTWAGES_CH10 ");
                }

                obj_str.Append(", LOSTWAGES_BEFORE_MIN=@LOSTWAGES_BEFORE_MIN ");
                obj_str.Append(", LOSTWAGES_WORK1_MIN=@LOSTWAGES_WORK1_MIN ");
                obj_str.Append(", LOSTWAGES_WORK2_MIN=@LOSTWAGES_WORK2_MIN ");
                obj_str.Append(", LOSTWAGES_BREAK_MIN=@LOSTWAGES_BREAK_MIN ");
                obj_str.Append(", LOSTWAGES_AFTER_MIN=@LOSTWAGES_AFTER_MIN ");
                obj_str.Append(", LOSTWAGES_LATE_MIN=@LOSTWAGES_LATE_MIN ");

                obj_str.Append(", LOSTWAGES_BEFORE_MIN_APP=@LOSTWAGES_BEFORE_MIN_APP ");
                obj_str.Append(", LOSTWAGES_WORK1_MIN_APP=@LOSTWAGES_WORK1_MIN_APP ");
                obj_str.Append(", LOSTWAGES_WORK2_MIN_APP=@LOSTWAGES_WORK2_MIN_APP ");
                obj_str.Append(", LOSTWAGES_BREAK_MIN_APP=@LOSTWAGES_BREAK_MIN_APP ");
                obj_str.Append(", LOSTWAGES_AFTER_MIN_APP=@LOSTWAGES_AFTER_MIN_APP ");
                obj_str.Append(", LOSTWAGES_LATE_MIN_APP=@LOSTWAGES_LATE_MIN_APP ");


                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");


                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");                
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" AND LOSTWAGES_CARDNO=@LOSTWAGES_CARDNO ");

                obj_str.Append(" AND PROJECT_CODE=@PROJECT_CODE ");
                obj_str.Append(" AND PROJOB_CODE=@PROJOB_CODE ");
                obj_str.Append(" AND LOSTWAGES_STATUS=@LOSTWAGES_STATUS ");

                

                obj_str.Append(" AND LOSTWAGES_WORKDATE=@LOSTWAGES_WORKDATE ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                //


                obj_cmd.Parameters.Add("@LOSTWAGES_SALARY", SqlDbType.VarChar); obj_cmd.Parameters["@LOSTWAGES_SALARY"].Value = model.lostwages_salary;
                obj_cmd.Parameters.Add("@LOSTWAGES_DILIGENCE", SqlDbType.VarChar); obj_cmd.Parameters["@LOSTWAGES_DILIGENCE"].Value = model.lostwages_diligence;
                obj_cmd.Parameters.Add("@LOSTWAGES_TRAVELEXPENSES", SqlDbType.VarChar); obj_cmd.Parameters["@LOSTWAGES_TRAVELEXPENSES"].Value = model.lostwages_travelexpenses;
                obj_cmd.Parameters.Add("@LOSTWAGES_OTHER", SqlDbType.VarChar); obj_cmd.Parameters["@LOSTWAGES_OTHER"].Value = model.lostwages_other;

                //

                obj_cmd.Parameters.Add("@SHIFT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CODE"].Value = model.shift_code;
                obj_cmd.Parameters.Add("@LOSTWAGES_DAYTYPE", SqlDbType.VarChar); obj_cmd.Parameters["@LOSTWAGES_DAYTYPE"].Value = model.lostwages_daytype;
                obj_cmd.Parameters.Add("@LOSTWAGES_COLOR", SqlDbType.VarChar); obj_cmd.Parameters["@LOSTWAGES_COLOR"].Value = model.lostwages_color;

                obj_cmd.Parameters.Add("@LOSTWAGES_LOCK", SqlDbType.Bit); obj_cmd.Parameters["@LOSTWAGES_LOCK"].Value = model.lostwages_lock;


                if (model.before_scan)
                {
                    obj_cmd.Parameters.Add("@LOSTWAGES_CH1", SqlDbType.DateTime); obj_cmd.Parameters["@LOSTWAGES_CH1"].Value = model.lostwages_ch1;
                    obj_cmd.Parameters.Add("@LOSTWAGES_CH2", SqlDbType.DateTime); obj_cmd.Parameters["@LOSTWAGES_CH2"].Value = model.lostwages_ch2;
                }

                if (model.work1_scan)
                {
                    obj_cmd.Parameters.Add("@LOSTWAGES_CH3", SqlDbType.DateTime); obj_cmd.Parameters["@LOSTWAGES_CH3"].Value = model.lostwages_ch3;
                    obj_cmd.Parameters.Add("@LOSTWAGES_CH4", SqlDbType.DateTime); obj_cmd.Parameters["@LOSTWAGES_CH4"].Value = model.lostwages_ch4;
                }

                if (model.break_scan)
                {
                    obj_cmd.Parameters.Add("@LOSTWAGES_CH5", SqlDbType.DateTime); obj_cmd.Parameters["@LOSTWAGES_CH5"].Value = model.lostwages_ch5;
                    obj_cmd.Parameters.Add("@LOSTWAGES_CH6", SqlDbType.DateTime); obj_cmd.Parameters["@LOSTWAGES_CH6"].Value = model.lostwages_ch6;
                }

                if (model.work2_scan)
                {
                    obj_cmd.Parameters.Add("@LOSTWAGES_CH7", SqlDbType.DateTime); obj_cmd.Parameters["@LOSTWAGES_CH7"].Value = model.lostwages_ch7;
                    obj_cmd.Parameters.Add("@LOSTWAGES_CH8", SqlDbType.DateTime); obj_cmd.Parameters["@LOSTWAGES_CH8"].Value = model.lostwages_ch8;
                }

                if (model.after_scan)
                {
                    obj_cmd.Parameters.Add("@LOSTWAGES_CH9", SqlDbType.DateTime); obj_cmd.Parameters["@LOSTWAGES_CH9"].Value = model.lostwages_ch9;
                    obj_cmd.Parameters.Add("@LOSTWAGES_CH10", SqlDbType.DateTime); obj_cmd.Parameters["@LOSTWAGES_CH10"].Value = model.lostwages_ch10;
                }

                obj_cmd.Parameters.Add("@LOSTWAGES_BEFORE_MIN", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_BEFORE_MIN"].Value = model.lostwages_before_min;
                obj_cmd.Parameters.Add("@LOSTWAGES_WORK1_MIN", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_WORK1_MIN"].Value = model.lostwages_work1_min;
                obj_cmd.Parameters.Add("@LOSTWAGES_WORK2_MIN", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_WORK2_MIN"].Value = model.lostwages_work2_min;
                obj_cmd.Parameters.Add("@LOSTWAGES_BREAK_MIN", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_BREAK_MIN"].Value = model.lostwages_break_min;
                obj_cmd.Parameters.Add("@LOSTWAGES_AFTER_MIN", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_AFTER_MIN"].Value = model.lostwages_after_min;
                obj_cmd.Parameters.Add("@LOSTWAGES_LATE_MIN", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_LATE_MIN"].Value = model.lostwages_late_min;

                obj_cmd.Parameters.Add("@LOSTWAGES_BEFORE_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_BEFORE_MIN_APP"].Value = model.lostwages_before_min_app;
                obj_cmd.Parameters.Add("@LOSTWAGES_WORK1_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_WORK1_MIN_APP"].Value = model.lostwages_work1_min_app;
                obj_cmd.Parameters.Add("@LOSTWAGES_WORK2_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_WORK2_MIN_APP"].Value = model.lostwages_work2_min_app;
                obj_cmd.Parameters.Add("@LOSTWAGES_BREAK_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_BREAK_MIN_APP"].Value = model.lostwages_break_min_app;
                obj_cmd.Parameters.Add("@LOSTWAGES_AFTER_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_AFTER_MIN_APP"].Value = model.lostwages_after_min_app;
                obj_cmd.Parameters.Add("@LOSTWAGES_LATE_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_LATE_MIN_APP"].Value = model.lostwages_late_min_app;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;                
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@LOSTWAGES_CARDNO", SqlDbType.VarChar); obj_cmd.Parameters["@LOSTWAGES_CARDNO"].Value = model.lostwages_cardno;

                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
                obj_cmd.Parameters.Add("@PROJOB_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOB_CODE"].Value = model.projob_code;

                obj_cmd.Parameters.Add("@LOSTWAGES_STATUS", SqlDbType.VarChar); obj_cmd.Parameters["@LOSTWAGES_STATUS"].Value = model.lostwages_status;

                obj_cmd.Parameters.Add("@LOSTWAGES_WORKDATE", SqlDbType.DateTime); obj_cmd.Parameters["@LOSTWAGES_WORKDATE"].Value = model.lostwages_workdate.Date;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(LOSTWAGES.update)" + ex.ToString();
            }

            return blnResult;
        }

        public bool updateWithCH(cls_TRLostwages model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();


                obj_str.Append("UPDATE ATT_TR_LOSTWAGES SET ");

                obj_str.Append(" SHIFT_CODE=@SHIFT_CODE ");
                obj_str.Append(", LOSTWAGES_DAYTYPE=@LOSTWAGES_DAYTYPE ");
                obj_str.Append(", LOSTWAGES_COLOR=@LOSTWAGES_COLOR ");

                if (model.lostwages_ch1_scan)
                    obj_str.Append(", LOSTWAGES_CH1=@LOSTWAGES_CH1 ");
                if (model.lostwages_ch2_scan)
                    obj_str.Append(", LOSTWAGES_CH2=@LOSTWAGES_CH2 ");
                if (model.lostwages_ch3_scan)
                    obj_str.Append(", LOSTWAGES_CH3=@LOSTWAGES_CH3 ");
                if (model.lostwages_ch4_scan)
                    obj_str.Append(", LOSTWAGES_CH4=@LOSTWAGES_CH4 ");
                if (model.lostwages_ch5_scan)
                    obj_str.Append(", LOSTWAGES_CH5=@LOSTWAGES_CH5 ");
                if (model.lostwages_ch6_scan)
                    obj_str.Append(", LOSTWAGES_CH6=@LOSTWAGES_CH6 ");
                if (model.lostwages_ch7_scan)
                    obj_str.Append(", LOSTWAGES_CH7=@LOSTWAGES_CH7 ");
                if (model.lostwages_ch8_scan)
                    obj_str.Append(", LOSTWAGES_CH8=@LOSTWAGES_CH8 ");
                if (model.lostwages_ch9_scan)
                    obj_str.Append(", LOSTWAGES_CH9=@LOSTWAGES_CH9 ");
                if (model.lostwages_ch10_scan)
                    obj_str.Append(", LOSTWAGES_CH10=@LOSTWAGES_CH10 ");


                obj_str.Append(", LOSTWAGES_BEFORE_MIN=@LOSTWAGES_BEFORE_MIN ");
                obj_str.Append(", LOSTWAGES_WORK1_MIN=@LOSTWAGES_WORK1_MIN ");
                obj_str.Append(", LOSTWAGES_WORK2_MIN=@LOSTWAGES_WORK2_MIN ");
                obj_str.Append(", LOSTWAGES_BREAK_MIN=@LOSTWAGES_BREAK_MIN ");
                obj_str.Append(", LOSTWAGES_AFTER_MIN=@LOSTWAGES_AFTER_MIN ");
                obj_str.Append(", LOSTWAGES_LATE_MIN=@LOSTWAGES_LATE_MIN ");

                obj_str.Append(", LOSTWAGES_BEFORE_MIN_APP=@LOSTWAGES_BEFORE_MIN_APP ");
                obj_str.Append(", LOSTWAGES_WORK1_MIN_APP=@LOSTWAGES_WORK1_MIN_APP ");
                obj_str.Append(", LOSTWAGES_WORK2_MIN_APP=@LOSTWAGES_WORK2_MIN_APP ");
                obj_str.Append(", LOSTWAGES_BREAK_MIN_APP=@LOSTWAGES_BREAK_MIN_APP ");
                obj_str.Append(", LOSTWAGES_AFTER_MIN_APP=@LOSTWAGES_AFTER_MIN_APP ");
                obj_str.Append(", LOSTWAGES_LATE_MIN_APP=@LOSTWAGES_LATE_MIN_APP ");


                obj_str.Append(", LOSTWAGES_LEAVEDEDUCT_MIN=@LOSTWAGES_LEAVEDEDUCT_MIN ");


                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");


                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");                
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" AND LOSTWAGES_CARDNO=@LOSTWAGES_CARDNO");
                
                obj_str.Append(" AND PROJECT_CODE=@PROJECT_CODE ");
                obj_str.Append(" AND PROJOB_CODE=@PROJOB_CODE ");
                obj_str.Append(" AND LOSTWAGES_STATUS=@LOSTWAGES_STATUS ");

                
                obj_str.Append(" AND LOSTWAGES_WORKDATE=@LOSTWAGES_WORKDATE ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@SHIFT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CODE"].Value = model.shift_code;
                obj_cmd.Parameters.Add("@LOSTWAGES_DAYTYPE", SqlDbType.VarChar); obj_cmd.Parameters["@LOSTWAGES_DAYTYPE"].Value = model.lostwages_daytype;
                obj_cmd.Parameters.Add("@LOSTWAGES_COLOR", SqlDbType.VarChar); obj_cmd.Parameters["@LOSTWAGES_COLOR"].Value = model.lostwages_color;

                if (model.lostwages_ch1_scan)
                {
                    obj_cmd.Parameters.Add("@LOSTWAGES_CH1", SqlDbType.DateTime); obj_cmd.Parameters["@LOSTWAGES_CH1"].Value = model.lostwages_ch1;
                }
                if (model.lostwages_ch2_scan)
                {
                    obj_cmd.Parameters.Add("@LOSTWAGES_CH2", SqlDbType.DateTime); obj_cmd.Parameters["@LOSTWAGES_CH2"].Value = model.lostwages_ch2;
                }
                if (model.lostwages_ch3_scan)
                {
                    obj_cmd.Parameters.Add("@LOSTWAGES_CH3", SqlDbType.DateTime); obj_cmd.Parameters["@LOSTWAGES_CH3"].Value = model.lostwages_ch3;
                }
                if (model.lostwages_ch4_scan)
                {
                    obj_cmd.Parameters.Add("@LOSTWAGES_CH4", SqlDbType.DateTime); obj_cmd.Parameters["@LOSTWAGES_CH4"].Value = model.lostwages_ch4;
                }
                if (model.lostwages_ch5_scan)
                {
                    obj_cmd.Parameters.Add("@LOSTWAGES_CH5", SqlDbType.DateTime); obj_cmd.Parameters["@LOSTWAGES_CH5"].Value = model.lostwages_ch5;
                }
                if (model.lostwages_ch6_scan)
                {
                    obj_cmd.Parameters.Add("@LOSTWAGES_CH6", SqlDbType.DateTime); obj_cmd.Parameters["@LOSTWAGES_CH6"].Value = model.lostwages_ch6;
                }
                if (model.lostwages_ch7_scan)
                {
                    obj_cmd.Parameters.Add("@LOSTWAGES_CH7", SqlDbType.DateTime); obj_cmd.Parameters["@LOSTWAGES_CH7"].Value = model.lostwages_ch7;
                }
                if (model.lostwages_ch8_scan)
                {
                    obj_cmd.Parameters.Add("@LOSTWAGES_CH8", SqlDbType.DateTime); obj_cmd.Parameters["@LOSTWAGES_CH8"].Value = model.lostwages_ch8;
                }
                if (model.lostwages_ch9_scan)
                {
                    obj_cmd.Parameters.Add("@LOSTWAGES_CH9", SqlDbType.DateTime); obj_cmd.Parameters["@LOSTWAGES_CH9"].Value = model.lostwages_ch9;
                }
                if (model.lostwages_ch10_scan)
                {
                    obj_cmd.Parameters.Add("@LOSTWAGES_CH10", SqlDbType.DateTime); obj_cmd.Parameters["@LOSTWAGES_CH10"].Value = model.lostwages_ch10;
                }


                obj_cmd.Parameters.Add("@LOSTWAGES_BEFORE_MIN", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_BEFORE_MIN"].Value = model.lostwages_before_min;
                obj_cmd.Parameters.Add("@LOSTWAGES_WORK1_MIN", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_WORK1_MIN"].Value = model.lostwages_work1_min;
                obj_cmd.Parameters.Add("@LOSTWAGES_WORK2_MIN", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_WORK2_MIN"].Value = model.lostwages_work2_min;
                obj_cmd.Parameters.Add("@LOSTWAGES_BREAK_MIN", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_BREAK_MIN"].Value = model.lostwages_break_min;
                obj_cmd.Parameters.Add("@LOSTWAGES_AFTER_MIN", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_AFTER_MIN"].Value = model.lostwages_after_min;
                obj_cmd.Parameters.Add("@LOSTWAGES_LATE_MIN", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_LATE_MIN"].Value = model.lostwages_late_min;

                obj_cmd.Parameters.Add("@LOSTWAGES_BEFORE_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_BEFORE_MIN_APP"].Value = model.lostwages_before_min_app;
                obj_cmd.Parameters.Add("@LOSTWAGES_WORK1_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_WORK1_MIN_APP"].Value = model.lostwages_work1_min_app;
                obj_cmd.Parameters.Add("@LOSTWAGES_WORK2_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_WORK2_MIN_APP"].Value = model.lostwages_work2_min_app;
                obj_cmd.Parameters.Add("@LOSTWAGES_BREAK_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_BREAK_MIN_APP"].Value = model.lostwages_break_min_app;
                obj_cmd.Parameters.Add("@LOSTWAGES_AFTER_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_AFTER_MIN_APP"].Value = model.lostwages_after_min_app;
                obj_cmd.Parameters.Add("@LOSTWAGES_LATE_MIN_APP", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_LATE_MIN_APP"].Value = model.lostwages_late_min_app;

                obj_cmd.Parameters.Add("@LOSTWAGES_LEAVEDEDUCT_MIN", SqlDbType.Int); obj_cmd.Parameters["@LOSTWAGES_LEAVEDEDUCT_MIN"].Value = model.lostwages_leavededuct_min;



                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;                
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@LOSTWAGES_CARDNO", SqlDbType.VarChar); obj_cmd.Parameters["@LOSTWAGES_CARDNO"].Value = model.lostwages_cardno;

                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
                obj_cmd.Parameters.Add("@PROJOB_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOB_CODE"].Value = model.projob_code;

                obj_cmd.Parameters.Add("@LOSTWAGES_STATUS", SqlDbType.VarChar); obj_cmd.Parameters["@LOSTWAGES_STATUS"].Value = model.lostwages_status;

                obj_cmd.Parameters.Add("@LOSTWAGES_WORKDATE", SqlDbType.DateTime); obj_cmd.Parameters["@LOSTWAGES_WORKDATE"].Value = model.lostwages_workdate.Date;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(LOSTWAGES.update)" + ex.ToString();
            }

            return blnResult;
        }

        public bool clearCH(cls_TRLostwages model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();


                obj_str.Append("UPDATE ATT_TR_LOSTWAGES SET ");

                obj_str.Append("LOSTWAGES_BEFORE_MIN=0 ");
                obj_str.Append(", LOSTWAGES_WORK1_MIN=0 ");
                obj_str.Append(", LOSTWAGES_WORK2_MIN=0 ");
                obj_str.Append(", LOSTWAGES_BREAK_MIN=0 ");
                obj_str.Append(", LOSTWAGES_AFTER_MIN=0 ");
                obj_str.Append(", LOSTWAGES_LATE_MIN=0 ");

                obj_str.Append(", LOSTWAGES_BEFORE_MIN_APP=0 ");
                obj_str.Append(", LOSTWAGES_WORK1_MIN_APP=0 ");
                obj_str.Append(", LOSTWAGES_WORK2_MIN_APP=0 ");
                obj_str.Append(", LOSTWAGES_BREAK_MIN_APP=0 ");
                obj_str.Append(", LOSTWAGES_AFTER_MIN_APP=0 ");
                obj_str.Append(", LOSTWAGES_LATE_MIN_APP=0 ");
                obj_str.Append(", LOSTWAGES_LEAVEDEDUCT_MIN=0 ");

                for (int i = 1; i <= 10; i++)
                {
                    obj_str.Append(", LOSTWAGES_CH" + i.ToString() + "=NULL ");
                }



                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");                
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" AND LOSTWAGES_CARDNO=@LOSTWAGES_CARDNO ");

                
                obj_str.Append(" AND PROJECT_CODE=@PROJECT_CODE ");
                obj_str.Append(" AND PROJOB_CODE=@PROJOB_CODE ");
                obj_str.Append(" AND LOSTWAGES_STATUS=@LOSTWAGES_STATUS ");

                
                obj_str.Append(" AND LOSTWAGES_WORKDATE=@LOSTWAGES_WORKDATE ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;                
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@LOSTWAGES_CARDNO", SqlDbType.VarChar); obj_cmd.Parameters["@LOSTWAGES_CARDNO"].Value = model.lostwages_cardno;

                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
                obj_cmd.Parameters.Add("@PROJOB_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOB_CODE"].Value = model.projob_code;

                obj_cmd.Parameters.Add("@LOSTWAGES_STATUS", SqlDbType.VarChar); obj_cmd.Parameters["@LOSTWAGES_STATUS"].Value = model.lostwages_status;
                obj_cmd.Parameters.Add("@LOSTWAGES_WORKDATE", SqlDbType.DateTime); obj_cmd.Parameters["@LOSTWAGES_WORKDATE"].Value = model.lostwages_workdate.Date;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Lostwages.update)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insertlist(string com, string project, List<cls_TRLostwages> list_model)
        {
            bool blnResult = false;
            try
            {

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO ATT_TR_LOSTWAGES");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", PROJECT_CODE ");
                obj_str.Append(", PROJOB_CODE ");
                obj_str.Append(", LOSTWAGES_STATUS ");

           
                //
                obj_str.Append(", LOSTWAGES_INITIAL ");
                obj_str.Append(", LOSTWAGES_CARDNO ");
                obj_str.Append(", LOSTWAGES_GENDER ");
                obj_str.Append(", LOSTWAGES_FNAME_TH ");
                obj_str.Append(", LOSTWAGES_LNAME_TH ");
             //
                
                obj_str.Append(", SHIFT_CODE ");
                obj_str.Append(", LOSTWAGES_WORKDATE ");
                obj_str.Append(", LOSTWAGES_DAYTYPE ");
                obj_str.Append(", LOSTWAGES_COLOR ");
                obj_str.Append(", LOSTWAGES_LOCK ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");

                obj_str.Append(", @PROJECT_CODE ");
                obj_str.Append(", @PROJOB_CODE ");
                obj_str.Append(", @LOSTWAGES_STATUS ");

                //
                obj_str.Append(", @LOSTWAGES_INITIAL ");
                obj_str.Append(", @LOSTWAGES_CARDNO ");
                obj_str.Append(", @LOSTWAGES_GENDER ");
                obj_str.Append(", @LOSTWAGES_FNAME_TH ");
                obj_str.Append(", @LOSTWAGES_LNAME_TH ");
                //
                obj_str.Append(", @SHIFT_CODE ");
                obj_str.Append(", @LOSTWAGES_WORKDATE ");
                obj_str.Append(", @LOSTWAGES_DAYTYPE ");
                obj_str.Append(", @LOSTWAGES_COLOR ");
                obj_str.Append(", @LOSTWAGES_LOCK ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                //-- Step 1 delete data old
                string strWorkerID = "";
                foreach (cls_TRLostwages model in list_model)
                {
                    strWorkerID += "'" + model.lostwages_cardno + "',";
                }
                if (strWorkerID.Length > 0)
                    strWorkerID = strWorkerID.Substring(0, strWorkerID.Length - 1);

                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM ATT_TR_LOSTWAGES");
                obj_str2.Append(" WHERE 1=1 ");
                obj_str2.Append(" AND COMPANY_CODE='" + list_model[0].company_code + "'");

                 //obj_str2.Append(" AND WORKER_CODE IN (" + strWorkerID + ")");
                 obj_str2.Append(" AND LOSTWAGES_CARDNO IN (" + strWorkerID + ")");

                 
                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());

                if (blnResult)
                {

                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@LOSTWAGES_CARDNO", SqlDbType.VarChar);

                    obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar);

                    obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime);
                    obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit);

                    foreach (cls_TRLostwages model in list_model)
                    {

                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                        obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                        obj_cmd.Parameters["@LOSTWAGES_CARDNO"].Value = model.lostwages_cardno;

                        obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;

                        obj_cmd.Parameters["@CREATED_BY"].Value = model.created_by;
                        obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                        obj_cmd.Parameters["@FLAG"].Value = false;

                        obj_cmd.ExecuteNonQuery();
                    }

                    blnResult = obj_conn.doCommit();

                    if (!blnResult)
                        obj_conn.doRollback();
                    obj_conn.doClose();

                }
                else
                {
                    obj_conn.doRollback();
                    obj_conn.doClose();
                }

            }
            catch (Exception ex)
            {
                Message = "PAYTRB007:" + ex.ToString();
            }

            return blnResult;
        }

    }
}