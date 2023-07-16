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
    public class cls_ctTRWageday
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRWageday() { }

        public string getMessage() { return this.Message; }

        private string FormatDateDB = "MM/dd/yyyy";

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRWageday> getData(string language, string condition)
        {
            List<cls_TRWageday> list_model = new List<cls_TRWageday>();
            cls_TRWageday model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("ATT_TR_WAGEDAY.COMPANY_CODE");
                obj_str.Append(", ATT_TR_WAGEDAY.WORKER_CODE");

                obj_str.Append(", ATT_TR_WAGEDAY.PROJECT_CODE");
                obj_str.Append(", ATT_TR_WAGEDAY.PROJOB_CODE");

                obj_str.Append(", WAGEDAY_DATE");
                obj_str.Append(", WAGEDAY_WAGE");

                obj_str.Append(", ISNULL(WAGEDAY_BEFORE_RATE, 0) AS WAGEDAY_BEFORE_RATE");
                obj_str.Append(", ISNULL(WAGEDAY_NORMAL_RATE, 0) AS WAGEDAY_NORMAL_RATE");
                obj_str.Append(", ISNULL(WAGEDAY_BREAK_RATE, 0) AS WAGEDAY_BREAK_RATE");
                obj_str.Append(", ISNULL(WAGEDAY_AFTER_RATE, 0) AS WAGEDAY_AFTER_RATE");

                obj_str.Append(", ISNULL(WAGEDAY_BEFORE_MIN, 0) AS WAGEDAY_BEFORE_MIN");
                obj_str.Append(", ISNULL(WAGEDAY_NORMAL_MIN, 0) AS WAGEDAY_NORMAL_MIN");
                obj_str.Append(", ISNULL(WAGEDAY_BREAK_MIN, 0) AS WAGEDAY_BREAK_MIN");
                obj_str.Append(", ISNULL(WAGEDAY_AFTER_MIN, 0) AS WAGEDAY_AFTER_MIN");

                obj_str.Append(", ISNULL(WAGEDAY_BEFORE_AMOUNT, 0) AS WAGEDAY_BEFORE_AMOUNT");
                obj_str.Append(", ISNULL(WAGEDAY_NORMAL_AMOUNT, 0) AS WAGEDAY_NORMAL_AMOUNT");
                obj_str.Append(", ISNULL(WAGEDAY_BREAK_AMOUNT, 0) AS WAGEDAY_BREAK_AMOUNT");
                obj_str.Append(", ISNULL(WAGEDAY_AFTER_AMOUNT, 0) AS WAGEDAY_AFTER_AMOUNT");

                obj_str.Append(", ISNULL(OT1_MIN, 0) AS OT1_MIN");
                obj_str.Append(", ISNULL(OT15_MIN, 0) AS OT15_MIN");
                obj_str.Append(", ISNULL(OT2_MIN, 0) AS OT2_MIN");
                obj_str.Append(", ISNULL(OT3_MIN, 0) AS OT3_MIN");

                obj_str.Append(", ISNULL(OT1_AMOUNT, 0) AS OT1_AMOUNT");
                obj_str.Append(", ISNULL(OT15_AMOUNT, 0) AS OT15_AMOUNT");
                obj_str.Append(", ISNULL(OT2_AMOUNT, 0) AS OT2_AMOUNT");
                obj_str.Append(", ISNULL(OT3_AMOUNT, 0) AS OT3_AMOUNT");

                obj_str.Append(", ISNULL(LATE_MIN, 0) AS LATE_MIN");
                obj_str.Append(", ISNULL(LATE_AMOUNT, 0) AS LATE_AMOUNT");

                obj_str.Append(", ISNULL(LEAVE_MIN, 0) AS LEAVE_MIN");
                obj_str.Append(", ISNULL(LEAVE_AMOUNT, 0) AS LEAVE_AMOUNT");

                obj_str.Append(", ISNULL(ABSENT_MIN, 0) AS ABSENT_MIN");
                obj_str.Append(", ISNULL(ABSENT_AMOUNT, 0) AS ABSENT_AMOUNT");

                obj_str.Append(", ISNULL(ALLOWANCE_AMOUNT, 0) AS ALLOWANCE_AMOUNT");

                obj_str.Append(", ISNULL(ATT_TR_WAGEDAY.MODIFIED_BY, ATT_TR_WAGEDAY.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(ATT_TR_WAGEDAY.MODIFIED_DATE, ATT_TR_WAGEDAY.CREATED_DATE) AS MODIFIED_DATE");

                if (language.Equals("TH"))
                {
                    obj_str.Append(", INITIAL_NAME_TH + WORKER_FNAME_TH + ' ' + WORKER_LNAME_TH AS WORKER_DETAIL");
                }
                else
                {
                    obj_str.Append(", INITIAL_NAME_EN + WORKER_FNAME_EN + ' ' + WORKER_LNAME_EN AS WORKER_DETAIL");
                }


                obj_str.Append(" FROM ATT_TR_WAGEDAY");

                obj_str.Append(" INNER JOIN EMP_MT_WORKER ON EMP_MT_WORKER.COMPANY_CODE=ATT_TR_WAGEDAY.COMPANY_CODE AND EMP_MT_WORKER.WORKER_CODE=ATT_TR_WAGEDAY.WORKER_CODE");
                obj_str.Append(" INNER JOIN EMP_MT_INITIAL ON EMP_MT_INITIAL.INITIAL_CODE=EMP_MT_WORKER.WORKER_INITIAL ");

                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY ATT_TR_WAGEDAY.COMPANY_CODE, ATT_TR_WAGEDAY.WORKER_CODE, WAGEDAY_DATE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRWageday();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                    model.worker_code = Convert.ToString(dr["WORKER_CODE"]);

                    model.project_code = Convert.ToString(dr["PROJECT_CODE"]);
                    model.projob_code = Convert.ToString(dr["PROJOB_CODE"]);
                    
                    model.wageday_date = Convert.ToDateTime(dr["WAGEDAY_DATE"]);
                    model.wageday_wage = Convert.ToDouble(dr["WAGEDAY_WAGE"]);

                    model.wageday_before_rate = Convert.ToDouble(dr["WAGEDAY_BEFORE_RATE"]);
                    model.wageday_normal_rate = Convert.ToDouble(dr["WAGEDAY_NORMAL_RATE"]);
                    model.wageday_break_rate = Convert.ToDouble(dr["WAGEDAY_BREAK_RATE"]);
                    model.wageday_after_rate = Convert.ToDouble(dr["WAGEDAY_AFTER_RATE"]);

                    model.wageday_before_min = Convert.ToInt32(dr["WAGEDAY_BEFORE_MIN"]);
                    model.wageday_normal_min = Convert.ToInt32(dr["WAGEDAY_NORMAL_MIN"]);
                    model.wageday_break_min = Convert.ToInt32(dr["WAGEDAY_BREAK_MIN"]);
                    model.wageday_after_min = Convert.ToInt32(dr["WAGEDAY_AFTER_MIN"]);

                    model.wageday_before_amount = Convert.ToDouble(dr["WAGEDAY_BEFORE_AMOUNT"]);
                    model.wageday_normal_amount = Convert.ToDouble(dr["WAGEDAY_NORMAL_AMOUNT"]);
                    model.wageday_break_amount = Convert.ToDouble(dr["WAGEDAY_BREAK_AMOUNT"]);
                    model.wageday_after_amount = Convert.ToDouble(dr["WAGEDAY_AFTER_AMOUNT"]);

                    model.ot1_min = Convert.ToInt32(dr["OT1_MIN"]);
                    model.ot15_min = Convert.ToInt32(dr["OT15_MIN"]);
                    model.ot2_min = Convert.ToInt32(dr["OT2_MIN"]);
                    model.ot3_min = Convert.ToInt32(dr["OT3_MIN"]);

                    model.ot1_amount = Convert.ToDouble(dr["OT1_AMOUNT"]);
                    model.ot15_amount = Convert.ToDouble(dr["OT15_AMOUNT"]);
                    model.ot2_amount = Convert.ToDouble(dr["OT2_AMOUNT"]);
                    model.ot3_amount = Convert.ToDouble(dr["OT3_AMOUNT"]);

                    model.late_min = Convert.ToInt32(dr["LATE_MIN"]);
                    model.late_amount = Convert.ToDouble(dr["LATE_AMOUNT"]);

                    model.leave_min = Convert.ToInt32(dr["LEAVE_MIN"]);
                    model.leave_amount = Convert.ToDouble(dr["LEAVE_AMOUNT"]);

                    model.absent_min = Convert.ToInt32(dr["ABSENT_MIN"]);
                    model.absent_amount = Convert.ToDouble(dr["ABSENT_AMOUNT"]);

                    model.allowance_amount = Convert.ToDouble(dr["ALLOWANCE_AMOUNT"]);

                    model.worker_detail = Convert.ToString(dr["WORKER_DETAIL"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Wageday.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRWageday> getDataByFillter(string language, string com, string project, string job, DateTime datefrom, DateTime dateto, string emp)
        {
            string strCondition = "";

            strCondition += " AND ATT_TR_WAGEDAY.COMPANY_CODE='" + com + "'";
            strCondition += " AND (ATT_TR_WAGEDAY.WAGEDAY_DATE BETWEEN '" + datefrom.ToString("MM/dd/yyyy") + "' AND '" + dateto.ToString("MM/dd/yyyy") + "')";

            if (!emp.Equals(""))
                strCondition += " AND ATT_TR_WAGEDAY.WORKER_CODE='" + emp + "'";

            if (!project.Equals(""))
                strCondition += " AND ATT_TR_WAGEDAY.PROJECT_CODE='" + project + "'";

            if (!job.Equals(""))
                strCondition += " AND ATT_TR_WAGEDAY.PROJOB_CODE='" + job + "'";

            return this.getData(language, strCondition);
        }

        public List<cls_TRWageday> getDataByCreateDate(string language, string com, DateTime datefrom, DateTime dateto, DateTime datecreate)
        {
            string strCondition = "";

            strCondition += " AND ATT_TR_WAGEDAY.COMPANY_CODE='" + com + "'";
            strCondition += " AND (ATT_TR_WAGEDAY.WAGEDAY_DATE BETWEEN '" + datefrom.ToString("MM/dd/yyyy") + "' AND '" + dateto.ToString("MM/dd/yyyy") + "')";
            strCondition += " AND (ATT_TR_WAGEDAY.CREATED_DATE >= '" + datecreate.ToString("MM/dd/yyyy") + "' OR ATT_TR_WAGEDAY.MODIFIED_DATE >= '" + datecreate.ToString("MM/dd/yyyy") + "')";

            return this.getData(language, strCondition);
        }

        public bool delete(string com, string emp, string project, string job, DateTime date)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_WAGEDAY");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                obj_str.Append(" AND PROJECT_CODE='" + project + "' ");
                obj_str.Append(" AND PROJOB_CODE='" + job + "' ");
                obj_str.Append(" AND WAGEDAY_DATE='" + date.ToString("MM/dd/yyyy") + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Wageday.delete)" + ex.ToString();
            }

            return blnResult;
        }

    }
}