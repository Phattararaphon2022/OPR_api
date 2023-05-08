using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRApplytraining
   {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRApplytraining() { }

        public string getMessage() { return this.Message.Replace("REQ_TR_TRAINING", "").Replace("cls_ctTRApplytraining", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }
        private List<cls_TRTraining> getData(string condition)
        {
            List<cls_TRTraining> list_model = new List<cls_TRTraining>();
            cls_TRTraining model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");
                obj_str.Append(", REQTRAINING_NO");
                obj_str.Append(", ISNULL(INSTITUTE_CODE, '') AS INSTITUTE_CODE");
                obj_str.Append(", ISNULL(COURSE_CODE, '') AS COURSE_CODE");
                obj_str.Append(", ISNULL(INSTITUTE_OTHER, '') AS INSTITUTE_OTHER");
                obj_str.Append(", ISNULL(COURSE_OTHER, '') AS COURSE_OTHER");
                obj_str.Append(", ISNULL(REQTRAINING_START, '01/01/1900') AS REQTRAINING_START");
                obj_str.Append(", ISNULL(REQTRAINING_FINISH, '01/01/1900') AS REQTRAINING_FINISH");
                obj_str.Append(", ISNULL(REQTRAINING_STATUS, '') AS REQTRAINING_STATUS");
                obj_str.Append(", ISNULL(REQTRAINING_HOURS, 0) AS REQTRAINING_HOURS");
                obj_str.Append(", ISNULL(REQTRAINING_COST, 0) AS REQTRAINING_COST");
                obj_str.Append(", ISNULL(REQTRAINING_NOTE, '') AS REQTRAINING_NOTE");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM REQ_TR_TRAINING");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRTraining();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.emptraining_no = Convert.ToInt32(dr["REQTRAINING_NO"]);
                    model.institute_code = Convert.ToString(dr["INSTITUTE_CODE"]);
                    model.course_code = Convert.ToString(dr["COURSE_CODE"]);
                    model.institute_other = Convert.ToString(dr["INSTITUTE_OTHER"]);
                    model.course_other = Convert.ToString(dr["COURSE_OTHER"]);

                    model.emptraining_start = Convert.ToDateTime(dr["REQTRAINING_START"]);
                    model.emptraining_finish = Convert.ToDateTime(dr["REQTRAINING_FINISH"]);

                    model.emptraining_status = Convert.ToString(dr["REQTRAINING_STATUS"]);
                    model.emptraining_hours = Convert.ToDouble(dr["REQTRAINING_HOURS"]);
                    model.emptraining_cost = Convert.ToDouble(dr["REQTRAINING_COST"]);
                    model.emptraining_note = Convert.ToString(dr["REQTRAINING_NOTE"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "REQTRN001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRTraining> getDataByFillter(string com, string emp)
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

                obj_str.Append("SELECT ISNULL(REQTRAINING_NO, 1) ");
                obj_str.Append(" FROM REQ_TR_TRAINING");
                obj_str.Append(" ORDER BY REQTRAINING_NO DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "REQTRN002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT REQTRAINING_NO");
                obj_str.Append(" FROM REQ_TR_TRAINING");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORRKER_CODE='" + emp + "' ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "REQTRN003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM REQ_TR_TRAINING");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "REQTRN004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRTraining model)
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

                obj_str.Append("INSERT INTO REQ_TR_TRAINING");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", REQTRAINING_NO ");
                obj_str.Append(", INSTITUTE_CODE ");
                obj_str.Append(", COURSE_CODE ");
                obj_str.Append(", INSTITUTE_OTHER ");
                obj_str.Append(", COURSE_OTHER ");
                obj_str.Append(", REQTRAINING_START ");
                obj_str.Append(", REQTRAINING_FINISH ");
                obj_str.Append(", REQTRAINING_STATUS ");
                obj_str.Append(", REQTRAINING_HOURS ");
                obj_str.Append(", REQTRAINING_COST ");
                obj_str.Append(", REQTRAINING_NOTE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @REQTRAINING_NO ");
                obj_str.Append(", @INSTITUTE_CODE ");
                obj_str.Append(", @COURSE_CODE ");
                obj_str.Append(", @INSTITUTE_OTHER ");
                obj_str.Append(", @COURSE_OTHER ");
                obj_str.Append(", @REQTRAINING_START ");
                obj_str.Append(", @REQTRAINING_FINISH ");
                obj_str.Append(", @REQTRAINING_STATUS ");
                obj_str.Append(", @REQTRAINING_HOURS ");
                obj_str.Append(", @REQTRAINING_COST ");
                obj_str.Append(", @REQTRAINING_NOTE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@REQTRAINING_NO", SqlDbType.Int); obj_cmd.Parameters["@REQTRAINING_NO"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@INSTITUTE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@INSTITUTE_CODE"].Value = model.institute_code;
                obj_cmd.Parameters.Add("@COURSE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COURSE_CODE"].Value = model.course_code;
                obj_cmd.Parameters.Add("@INSTITUTE_OTHER", SqlDbType.VarChar); obj_cmd.Parameters["@INSTITUTE_OTHER"].Value = model.institute_other;
                obj_cmd.Parameters.Add("@COURSE_OTHER", SqlDbType.VarChar); obj_cmd.Parameters["@COURSE_OTHER"].Value = model.course_other;
                obj_cmd.Parameters.Add("@REQTRAINING_START", SqlDbType.DateTime); obj_cmd.Parameters["@REQTRAINING_START"].Value = model.emptraining_start;
                obj_cmd.Parameters.Add("@REQTRAINING_FINISH", SqlDbType.DateTime); obj_cmd.Parameters["@REQTRAINING_FINISH"].Value = model.emptraining_finish;
                obj_cmd.Parameters.Add("@REQTRAINING_STATUS", SqlDbType.VarChar); obj_cmd.Parameters["@REQTRAINING_STATUS"].Value = model.emptraining_status;
                obj_cmd.Parameters.Add("@REQTRAINING_HOURS", SqlDbType.Decimal); obj_cmd.Parameters["@REQTRAINING_HOURS"].Value = model.emptraining_hours;
                obj_cmd.Parameters.Add("@REQTRAINING_COST", SqlDbType.Decimal); obj_cmd.Parameters["@REQTRAINING_COST"].Value = model.emptraining_cost;
                obj_cmd.Parameters.Add("@REQTRAINING_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@REQTRAINING_NOTE"].Value = model.emptraining_note;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                strResult = model.emptraining_no.ToString();
            }
            catch (Exception ex)
            {
                Message = "REQTRN005:" + ex.ToString();
                strResult = "";
            }

            return blnResult;
        }

        public bool update(cls_TRTraining model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE REQ_TR_TRAINING SET ");

                obj_str.Append("INSTITUTE_CODE=@INSTITUTE_CODE ");
                obj_str.Append(", COURSE_CODE=@COURSE_CODE ");
                obj_str.Append(", INSTITUTE_OTHER=@INSTITUTE_OTHER ");
                obj_str.Append(", COURSE_OTHER=@COURSE_OTHER ");
                obj_str.Append(", REQTRAINING_START=@REQTRAINING_START ");
                obj_str.Append(", REQTRAINING_FINISH=@REQTRAINING_FINISH ");
                obj_str.Append(", REQTRAINING_STATUS=@REQTRAINING_STATUS ");
                obj_str.Append(", REQTRAINING_HOURS=@REQTRAINING_HOURS ");
                obj_str.Append(", REQTRAINING_COST=@REQTRAINING_COST ");
                obj_str.Append(", REQTRAINING_NOTE=@REQTRAINING_NOTE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE "); ;

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" AND REQTRAINING_NO=@REQTRAINING_NO ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@INSTITUTE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@INSTITUTE_CODE"].Value = model.institute_code;
                obj_cmd.Parameters.Add("@COURSE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COURSE_CODE"].Value = model.course_code;
                obj_cmd.Parameters.Add("@INSTITUTE_OTHER", SqlDbType.VarChar); obj_cmd.Parameters["@INSTITUTE_OTHER"].Value = model.institute_other;
                obj_cmd.Parameters.Add("@COURSE_OTHER", SqlDbType.VarChar); obj_cmd.Parameters["@COURSE_OTHER"].Value = model.course_other;
                obj_cmd.Parameters.Add("@REQTRAINING_START", SqlDbType.DateTime); obj_cmd.Parameters["@REQTRAINING_START"].Value = model.emptraining_start;
                obj_cmd.Parameters.Add("@REQTRAINING_FINISH", SqlDbType.DateTime); obj_cmd.Parameters["@REQTRAINING_FINISH"].Value = model.emptraining_finish;
                obj_cmd.Parameters.Add("@REQTRAINING_STATUS", SqlDbType.VarChar); obj_cmd.Parameters["@REQTRAINING_STATUS"].Value = model.emptraining_status;
                obj_cmd.Parameters.Add("@REQTRAINING_HOURS", SqlDbType.Decimal); obj_cmd.Parameters["@REQTRAINING_HOURS"].Value = model.emptraining_hours;
                obj_cmd.Parameters.Add("@REQTRAINING_COST", SqlDbType.Decimal); obj_cmd.Parameters["@REQTRAINING_COST"].Value = model.emptraining_cost;
                obj_cmd.Parameters.Add("@REQTRAINING_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@REQTRAINING_NOTE"].Value = model.emptraining_note;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@REQTRAINING_NO", SqlDbType.Int); obj_cmd.Parameters["@REQTRAINING_NO"].Value = model.emptraining_no;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "REQTRN006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
