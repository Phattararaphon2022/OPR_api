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
   public class cls_ctMTATTReqdocument
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTATTReqdocument() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTATTReqdocument> getData(string condition)
        {
            List<cls_MTATTReqdocument> list_model = new List<cls_MTATTReqdocument>();
            cls_MTATTReqdocument model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", DOCUMENT_ID");
                obj_str.Append(", JOB_ID");
                obj_str.Append(", JOB_TYPE");
                obj_str.Append(", DOCUMENT_NAME");
                obj_str.Append(", DOCUMENT_TYPE");
                obj_str.Append(", DOCUMENT_PATH");
              
                obj_str.Append(", CREATED_BY");
                obj_str.Append(", CREATED_DATE");

                obj_str.Append(" FROM ATT_MT_REQDOCUMENT");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY DOCUMENT_ID");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTATTReqdocument();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.document_id = Convert.ToInt32(dr["DOCUMENT_ID"]);
                    model.job_id = dr["JOB_ID"].ToString();
                    model.job_type = dr["JOB_TYPE"].ToString();
                    model.document_name = dr["DOCUMENT_NAME"].ToString();
                    model.document_type = dr["DOCUMENT_TYPE"].ToString();
                    model.document_path = dr["DOCUMENT_PATH"].ToString();
              
                    model.created_by = dr["CREATED_BY"].ToString();
                    model.created_date = Convert.ToDateTime(dr["CREATED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTReqdocument.getData)" + ex.ToString();
            }

            return list_model;
        }
        public List<cls_MTATTReqdocument> getDataByFillter(string com, int doc_id, string job_id, string job_type)
        {
            string strCondition = "";
            if(!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!doc_id.Equals(0))
                strCondition += " AND DOCUMENT_ID='" + doc_id + "'";

            if (!job_id.Equals(""))
                strCondition += " AND JOB_ID='" + job_id + "'";

            if (!job_type.Equals(""))
                strCondition += " AND JOB_TYPE='" + job_type + "'";

            return this.getData(strCondition);
        }
        public bool checkDataOld(string com, int doc_id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT DOCUMENT_ID");
                obj_str.Append(" FROM ATT_MT_REQDOCUMENT");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                obj_str.Append(" AND DOCUMENT_ID='" + doc_id + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTReqdocument.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(string com, int doc_id, string job_id, string job_type)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_MT_REQDOCUMENT");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                if (!doc_id.Equals(0))
                    obj_str.Append(" AND DOCUMENT_ID='" + doc_id + "'");

                if (!job_id.Equals(""))
                    obj_str.Append(" AND JOB_ID='" + job_id + "'");

                if (!job_type.Equals(""))
                    obj_str.Append(" AND JOB_TYPE='" + job_type + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(MTReqdocument.delete)" + ex.ToString();
            }

            return blnResult;
        }
        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(DOCUMENT_ID) ");
                obj_str.Append(" FROM ATT_MT_REQDOCUMENT");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTReqdocument.getNextID)" + ex.ToString();
            }

            return intResult;
        }
        public string insert(cls_MTATTReqdocument model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code,model.document_id))
                {
                    return model.document_id.ToString();
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO ATT_MT_REQDOCUMENT");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", DOCUMENT_ID");
                obj_str.Append(", JOB_ID");
                obj_str.Append(", JOB_TYPE");
                obj_str.Append(", DOCUMENT_NAME");
                obj_str.Append(", DOCUMENT_TYPE");
                obj_str.Append(", DOCUMENT_PATH");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @DOCUMENT_ID ");
                obj_str.Append(", @JOB_ID ");
                obj_str.Append(", @JOB_TYPE ");
                obj_str.Append(", @DOCUMENT_NAME ");
                obj_str.Append(", @DOCUMENT_TYPE ");
                obj_str.Append(", @DOCUMENT_PATH ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@DOCUMENT_ID", SqlDbType.Int); obj_cmd.Parameters["@DOCUMENT_ID"].Value = id; ;
                obj_cmd.Parameters.Add("@JOB_ID", SqlDbType.VarChar); obj_cmd.Parameters["@JOB_ID"].Value = model.job_id;
                obj_cmd.Parameters.Add("@JOB_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@JOB_TYPE"].Value = model.job_type;
                obj_cmd.Parameters.Add("@DOCUMENT_NAME", SqlDbType.VarChar); obj_cmd.Parameters["@DOCUMENT_NAME"].Value = model.document_name;
                obj_cmd.Parameters.Add("@DOCUMENT_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@DOCUMENT_TYPE"].Value = model.document_type;
                obj_cmd.Parameters.Add("@DOCUMENT_PATH", SqlDbType.VarChar); obj_cmd.Parameters["@DOCUMENT_PATH"].Value = model.document_path;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.created_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTReqdocument.insert)" + ex.ToString();
            }

            return blnResult;
        }
        public string update(cls_MTATTReqdocument model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE ATT_MT_REQDOCUMENT SET ");
                obj_str.Append(" COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(", DOCUMENT_NAME=@DOCUMENT_NAME ");
                obj_str.Append(", DOCUMENT_TYPE=@DOCUMENT_TYPE ");
                obj_str.Append(", DOCUMENT_PATH=@DOCUMENT_PATH ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                if(!model.document_id.Equals(0))
                    obj_str.Append(" AND DOCUMENT_ID=@DOCUMENT_ID ");
                if (!model.job_id.Equals(""))
                    obj_str.Append(" AND JOB_ID=@JOB_ID ");
                if (!model.job_type.Equals(""))
                    obj_str.Append(" AND JOB_TYPE=@JOB_TYPE ");
                
      

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@DOCUMENT_ID", SqlDbType.Int); obj_cmd.Parameters["@DOCUMENT_ID"].Value = model.document_id; ;
                obj_cmd.Parameters.Add("@JOB_ID", SqlDbType.VarChar); obj_cmd.Parameters["@JOB_ID"].Value = model.job_id;
                obj_cmd.Parameters.Add("@JOB_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@JOB_TYPE"].Value = model.job_type;
                obj_cmd.Parameters.Add("@DOCUMENT_NAME", SqlDbType.VarChar); obj_cmd.Parameters["@DOCUMENT_NAME"].Value = model.document_name;
                obj_cmd.Parameters.Add("@DOCUMENT_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@DOCUMENT_TYPE"].Value = model.document_type;
                obj_cmd.Parameters.Add("@DOCUMENT_PATH", SqlDbType.VarChar); obj_cmd.Parameters["@DOCUMENT_PATH"].Value = model.document_path;
             
                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.document_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTReqdocument.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
