using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTJobtable
    {
          string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTJobtable() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTJobtable> getData(string condition)
        {
            List<cls_MTJobtable> list_model = new List<cls_MTJobtable>();
            cls_MTJobtable model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", JOBTABLE_ID");
                obj_str.Append(", JOB_ID");
                obj_str.Append(", JOB_TYPE");
                obj_str.Append(", STATUS_JOB");
                obj_str.Append(", JOB_NEXTSTEP");
                obj_str.Append(", JOB_DATE");
                obj_str.Append(", ISNULL(JOB_FINISHDATE, JOB_DATE) AS JOB_FINISHDATE");
                obj_str.Append(", WORKFLOW_CODE");

                obj_str.Append(", CREATED_BY");
                obj_str.Append(", CREATED_DATE");

                obj_str.Append(" FROM SELF_MT_JOBTABLE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY JOB_DATE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTJobtable();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.jobtable_id = Convert.ToInt32(dr["JOBTABLE_ID"]);
                    model.job_id = dr["JOB_ID"].ToString();
                    model.job_type = dr["JOB_TYPE"].ToString();
                    model.status_job = dr["STATUS_JOB"].ToString();
                    model.job_nextstep = Convert.ToInt32(dr["JOB_NEXTSTEP"]);
                    model.job_date = Convert.ToDateTime(dr["JOB_DATE"]);
                    model.job_finishdate = Convert.ToDateTime(dr["JOB_FINISHDATE"]);
                    model.workflow_code = dr["WORKFLOW_CODE"].ToString();
                    model.created_by = dr["CREATED_BY"].ToString();
                    model.created_date = Convert.ToDateTime(dr["CREATED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTJobtable.getData)" + ex.ToString();
            }

            return list_model;
        }
        public List<cls_MTJobtable> getDataByFillter(string com, int jabtable_id, string job_id, string job_type,string workflow_code, string status, string datefrom, string dateto)
        {
            string strCondition = "";
            if(!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!jabtable_id.Equals(0))
                strCondition += " AND JOBTABLE_ID='" + jabtable_id + "'";

            if (!job_id.Equals(""))
                strCondition += " AND JOB_ID='" + job_id + "'";

            if (!job_type.Equals(""))
                strCondition += " AND JOB_TYPE='" + job_type + "'";

            if (!workflow_code.Equals(""))
                strCondition += " AND WORKFLOW_CODE='" + workflow_code + "'";

            if (!status.Equals(""))
                strCondition += " AND STATUS_JOB='" + status + "'";

            if (!datefrom.Equals("") && !dateto.Equals(""))
                strCondition += " AND (JOB_DATE BETWEEN '" + datefrom + "' AND '" + dateto + "'";

            return this.getData(strCondition);
        }
        public bool checkDataOld(string com, int jobtable_id,string job_id,string job_type)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT JOB_ID");
                obj_str.Append(" FROM SELF_MT_JOBTABLE");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                if (!jobtable_id.Equals(0))
                    obj_str.Append(" AND JOBTABLE_ID='" + jobtable_id + "'");
                if (!job_id.Equals(""))
                    obj_str.Append(" AND JOB_ID='" + job_id + "'");
                if (!job_type.Equals(""))
                    obj_str.Append(" AND JOB_TYPE='" + job_type + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTJobtable.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(string com, int jobtable_id, string job_id, string job_type)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SELF_MT_JOBTABLE");
                obj_str.Append(" WHERE 1=1 ");
                if (!com.Equals(""))
                    obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                if (!jobtable_id.Equals(0))
                    obj_str.Append(" AND JOBTABLE_ID='" + jobtable_id + "'");
                if (!job_id.Equals(""))
                    obj_str.Append(" AND JOB_ID='" + job_id + "'");
                if (!job_type.Equals(""))
                    obj_str.Append(" AND JOB_TYPE='" + job_type + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(MTJobtable.delete)" + ex.ToString();
            }

            return blnResult;
        }
        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(JOBTABLE_ID) ");
                obj_str.Append(" FROM SELF_MT_JOBTABLE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTJobtable.getNextID)" + ex.ToString();
            }

            return intResult;
        }
        public string insert(cls_MTJobtable model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code,model.jobtable_id,model.job_id,model.job_type))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO SELF_MT_JOBTABLE");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", JOBTABLE_ID");
                obj_str.Append(", JOB_ID");
                obj_str.Append(", JOB_TYPE");
                obj_str.Append(", STATUS_JOB");
                obj_str.Append(", JOB_NEXTSTEP");
                obj_str.Append(", JOB_DATE");
                //obj_str.Append(", JOB_FINISHDATE");
                obj_str.Append(", WORKFLOW_CODE");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @JOBTABLE_ID ");
                obj_str.Append(", @JOB_ID ");
                obj_str.Append(", @JOB_TYPE ");
                obj_str.Append(", @STATUS_JOB ");
                obj_str.Append(", @JOB_NEXTSTEP ");
                obj_str.Append(", @JOB_DATE ");
                //obj_str.Append(", @JOB_FINISHDATE ");
                obj_str.Append(", @WORKFLOW_CODE ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@JOBTABLE_ID", SqlDbType.Int); obj_cmd.Parameters["@JOBTABLE_ID"].Value = id; ;
                obj_cmd.Parameters.Add("@JOB_ID", SqlDbType.VarChar); obj_cmd.Parameters["@JOB_ID"].Value = model.job_id;
                obj_cmd.Parameters.Add("@JOB_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@JOB_TYPE"].Value = model.job_type;
                obj_cmd.Parameters.Add("@STATUS_JOB", SqlDbType.Char); obj_cmd.Parameters["@STATUS_JOB"].Value = model.status_job;
                obj_cmd.Parameters.Add("@JOB_NEXTSTEP", SqlDbType.Int); obj_cmd.Parameters["@JOB_NEXTSTEP"].Value = model.job_nextstep;
                obj_cmd.Parameters.Add("@JOB_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@JOB_DATE"].Value = model.job_date;
                //obj_cmd.Parameters.Add("@JOB_FINISHDATE", SqlDbType.DateTime); obj_cmd.Parameters["@JOB_FINISHDATE"].Value = model.job_finishdate;
                obj_cmd.Parameters.Add("@WORKFLOW_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKFLOW_CODE"].Value = model.workflow_code;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.created_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTJobtable.insert)" + ex.ToString();
            }

            return blnResult;
        }
        public string update(cls_MTJobtable model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SELF_MT_JOBTABLE SET ");
                obj_str.Append(" COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(", JOB_TYPE=@JOB_TYPE ");
                obj_str.Append(", STATUS_JOB=@STATUS_JOB ");
                obj_str.Append(", JOB_NEXTSTEP=@JOB_NEXTSTEP ");
                //obj_str.Append(", JOB_DATE=@JOB_DATE ");
                if (!model.job_finishdate.Equals(null))
                    obj_str.Append(", JOB_FINISHDATE=@JOB_FINISHDATE ");
                obj_str.Append(", WORKFLOW_CODE=@WORKFLOW_CODE ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                if(!model.jobtable_id.Equals(0))
                    obj_str.Append(" AND JOBTABLE_ID=@JOBTABLE_ID ");
                if (!model.job_id.Equals(""))
                    obj_str.Append(" AND JOB_ID=@JOB_ID ");
                if (!model.job_type.Equals(""))
                    obj_str.Append(" AND JOB_TYPE=@JOB_TYPE ");
                
      

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@JOBTABLE_ID", SqlDbType.Int); obj_cmd.Parameters["@JOBTABLE_ID"].Value = model.jobtable_id; ;
                obj_cmd.Parameters.Add("@JOB_ID", SqlDbType.VarChar); obj_cmd.Parameters["@JOB_ID"].Value = model.job_id;
                obj_cmd.Parameters.Add("@JOB_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@JOB_TYPE"].Value = model.job_type;
                obj_cmd.Parameters.Add("@STATUS_JOB", SqlDbType.Char); obj_cmd.Parameters["@STATUS_JOB"].Value = model.status_job;
                obj_cmd.Parameters.Add("@JOB_NEXTSTEP", SqlDbType.Int); obj_cmd.Parameters["@JOB_NEXTSTEP"].Value = model.job_nextstep;
                //obj_cmd.Parameters.Add("@JOB_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@JOB_DATE"].Value = model.job_date;
                if (!model.job_finishdate.Equals(null))
                {
                    obj_cmd.Parameters.Add("@JOB_FINISHDATE", SqlDbType.DateTime); obj_cmd.Parameters["@JOB_FINISHDATE"].Value = model.job_finishdate;
                }
                obj_cmd.Parameters.Add("@WORKFLOW_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKFLOW_CODE"].Value = model.workflow_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.jobtable_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTJobtable.update)" + ex.ToString();
            }

            return blnResult;
        }

    }
}
