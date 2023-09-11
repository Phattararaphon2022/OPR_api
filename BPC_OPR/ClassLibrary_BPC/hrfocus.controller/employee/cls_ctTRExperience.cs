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
    public class cls_ctTRExperience
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRExperience() { }

        public string getMessage() { return this.Message.Replace("EMP_TR_EXPERIENCE", "").Replace("cls_ctTRExperience", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRExperience> getData(string condition)
        {
            List<cls_TRExperience> list_model = new List<cls_TRExperience>();
            cls_TRExperience model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");
                obj_str.Append(", EXPERIENCE_ID");
                obj_str.Append(", ISNULL(COMPANY_NAME, '') AS COMPANY_NAME");
                obj_str.Append(", ISNULL(POSITION, '') AS POSITION");
                obj_str.Append(", ISNULL(SALARY, 0) AS SALARY");
                obj_str.Append(", STARTDATE");
                obj_str.Append(", ENDDATE");
                obj_str.Append(", ISNULL(DESCRIPTION, 0) AS DESCRIPTION");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_TR_EXPERIENCE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRExperience();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.experience_id = Convert.ToInt32(dr["EXPERIENCE_ID"]);
                    model.company_name = dr["COMPANY_NAME"].ToString();
                    model.position = dr["POSITION"].ToString();
                    model.salary = Convert.ToDouble(dr["SALARY"]);

                    model.startdate = Convert.ToDateTime(dr["STARTDATE"]);
                    model.enddate = Convert.ToDateTime(dr["ENDDATE"]);
                    model.description = dr["DESCRIPTION"].ToString();
                    
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "EMPEXP001:" + ex.ToString();
            }

            return list_model;
        }
        public List<cls_TRExperience> getDataByFillter(string com, string emp)
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

                obj_str.Append("SELECT ISNULL(EXPERIENCE_ID, 1) ");
                obj_str.Append(" FROM EMP_TR_EXPERIENCE");
                obj_str.Append(" ORDER BY EXPERIENCE_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPEXP002:" + ex.ToString();
            }

            return intResult;
        }
        public bool checkDataOld(string com, string emp, string id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT EXPERIENCE_ID");
                obj_str.Append(" FROM EMP_TR_EXPERIENCE");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                if (!id.ToString().Equals(""))
                {
                    obj_str.Append(" AND EXPERIENCE_ID='" + id + "' ");
                }


                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPEXP003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM EMP_TR_EXPERIENCE");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");


                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "EMPEXP004:" + ex.ToString();
            }

            return blnResult;
        }
        public bool insert(cls_TRExperience model)
        {
            bool blnResult = false;
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.company_code, model.worker_code, model.experience_id.ToString()))
                {
                    if (model.experience_id.Equals(0))
                    {
                        return false;
                    }
                    else
                    {
                        return this.update(model);
                    }
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_TR_EXPERIENCE");
                obj_str.Append(" (");
                obj_str.Append("EXPERIENCE_ID ");
                obj_str.Append(", COMPANY_NAME ");
                obj_str.Append(", POSITION ");
                obj_str.Append(", SALARY ");
                obj_str.Append(", STARTDATE ");
                obj_str.Append(", ENDDATE ");
                obj_str.Append(", DESCRIPTION ");
                
                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@EXPERIENCE_ID ");
                obj_str.Append(", @COMPANY_NAME ");
                obj_str.Append(", @POSITION ");
                obj_str.Append(", @SALARY ");
                obj_str.Append(", @STARTDATE ");
                obj_str.Append(", @ENDDATE ");
                obj_str.Append(", @DESCRIPTION ");

                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@EXPERIENCE_ID", SqlDbType.Int); obj_cmd.Parameters["@EXPERIENCE_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@COMPANY_NAME", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_NAME"].Value = model.company_name;
                obj_cmd.Parameters.Add("@POSITION", SqlDbType.VarChar); obj_cmd.Parameters["@POSITION"].Value = model.position;
                obj_cmd.Parameters.Add("@SALARY", SqlDbType.Decimal); obj_cmd.Parameters["@SALARY"].Value = model.salary;
                obj_cmd.Parameters.Add("@STARTDATE", SqlDbType.DateTime); obj_cmd.Parameters["@STARTDATE"].Value = model.startdate;
                obj_cmd.Parameters.Add("@ENDDATE", SqlDbType.DateTime); obj_cmd.Parameters["@ENDDATE"].Value = model.enddate;
                obj_cmd.Parameters.Add("@DESCRIPTION", SqlDbType.VarChar); obj_cmd.Parameters["@DESCRIPTION"].Value = model.description;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                strResult = model.experience_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "EMPEXP005:" + ex.ToString();
                strResult = "";
            }

            return blnResult;
        }
        public bool update(cls_TRExperience model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE EMP_TR_EXPERIENCE SET ");

                obj_str.Append(" COMPANY_NAME=@COMPANY_NAME ");
                obj_str.Append(", POSITION=@POSITION ");
                obj_str.Append(", SALARY=@SALARY ");
                obj_str.Append(", STARTDATE=@STARTDATE ");
                obj_str.Append(", ENDDATE=@ENDDATE ");
                obj_str.Append(", DESCRIPTION=@DESCRIPTION ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE "); ;

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" AND EMPDEP_ID=@EMPDEP_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                
                obj_cmd.Parameters.Add("@COMPANY_NAME", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_NAME"].Value = model.company_name;
                obj_cmd.Parameters.Add("@POSITION", SqlDbType.VarChar); obj_cmd.Parameters["@POSITION"].Value = model.position;
                obj_cmd.Parameters.Add("@SALARY", SqlDbType.Decimal); obj_cmd.Parameters["@SALARY"].Value = model.salary;
                obj_cmd.Parameters.Add("@STARTDATE", SqlDbType.DateTime); obj_cmd.Parameters["@STARTDATE"].Value = model.startdate;
                obj_cmd.Parameters.Add("@ENDDATE", SqlDbType.DateTime); obj_cmd.Parameters["@ENDDATE"].Value = model.enddate;
                obj_cmd.Parameters.Add("@DESCRIPTION", SqlDbType.VarChar); obj_cmd.Parameters["@DESCRIPTION"].Value = model.description;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

               
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@EXPERIENCE_ID", SqlDbType.Int); obj_cmd.Parameters["@EXPERIENCE_ID"].Value = model.experience_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "EMPEXP006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
