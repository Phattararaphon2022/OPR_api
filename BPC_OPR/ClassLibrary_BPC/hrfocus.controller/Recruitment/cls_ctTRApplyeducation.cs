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
    public class cls_ctTRApplyeducation
   {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRApplyeducation() { }

        public string getMessage() { return this.Message.Replace("REQ_TR_EDUCATION", "").Replace("cls_ctTRApplyeducation", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }
        private List<cls_TREducation> getData(string condition)
        {
            List<cls_TREducation> list_model = new List<cls_TREducation>();
            cls_TREducation model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");
                obj_str.Append(", REQEDUCATION_NO");
                obj_str.Append(", ISNULL(INSTITUTE_CODE, '') AS INSTITUTE_CODE");
                obj_str.Append(", ISNULL(FACULTY_CODE, '') AS FACULTY_CODE");
                obj_str.Append(", ISNULL(MAJOR_CODE, '') AS MAJOR_CODE");
                obj_str.Append(", ISNULL(QUALIFICATION_CODE, '') AS QUALIFICATION_CODE");
                obj_str.Append(", ISNULL(REQEDUCATION_GPA, '') AS REQEDUCATION_GPA");
                obj_str.Append(", ISNULL(REQEDUCATION_START, '01/01/1900') AS REQEDUCATION_START");
                obj_str.Append(", ISNULL(REQEDUCATION_FINISH, '01/01/1900') AS REQEDUCATION_FINISH");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM REQ_TR_EDUCATION");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TREducation();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.empeducation_no = Convert.ToInt32(dr["REQEDUCATION_NO"]);
                    model.institute_code = Convert.ToString(dr["INSTITUTE_CODE"]);
                    model.faculty_code = Convert.ToString(dr["FACULTY_CODE"]);
                    model.major_code = Convert.ToString(dr["MAJOR_CODE"]);
                    model.qualification_code = Convert.ToString(dr["QUALIFICATION_CODE"]);
                    model.empeducation_gpa = Convert.ToString(dr["REQEDUCATION_GPA"]);
                    model.empeducation_start = Convert.ToDateTime(dr["REQEDUCATION_START"]);
                    model.empeducation_finish = Convert.ToDateTime(dr["REQEDUCATION_FINISH"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "REQEDT001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TREducation> getDataByFillter(string com, string emp)
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

                obj_str.Append("SELECT ISNULL(REQEDUCATION_NO, 1) ");
                obj_str.Append(" FROM REQ_TR_EDUCATION");
                obj_str.Append(" ORDER BY REQEDUCATION_NO DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "REQEDT002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT REQEDUCATION_NO");
                obj_str.Append(" FROM REQ_TR_EDUCATION");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "REQEDT003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM REQ_TR_EDUCATION");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "REQEDT004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TREducation model)
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

                obj_str.Append("INSERT INTO REQ_TR_EDUCATION");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", REQEDUCATION_NO ");
                obj_str.Append(", INSTITUTE_CODE ");
                obj_str.Append(", FACULTY_CODE ");
                obj_str.Append(", MAJOR_CODE ");
                obj_str.Append(", QUALIFICATION_CODE ");
                obj_str.Append(", REQEDUCATION_GPA ");
                obj_str.Append(", REQEDUCATION_START ");
                obj_str.Append(", REQEDUCATION_FINISH ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @REQEDUCATION_NO ");
                obj_str.Append(", @INSTITUTE_CODE ");
                obj_str.Append(", @FACULTY_CODE ");
                obj_str.Append(", @MAJOR_CODE ");
                obj_str.Append(", @QUALIFICATION_CODE ");
                obj_str.Append(", @REQEDUCATION_GPA ");
                obj_str.Append(", @REQEDUCATION_START ");
                obj_str.Append(", @REQEDUCATION_FINISH ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.empeducation_no = this.getNextID();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@REQEDUCATION_NO", SqlDbType.Int); obj_cmd.Parameters["@REQEDUCATION_NO"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@INSTITUTE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@INSTITUTE_CODE"].Value = model.institute_code;
                obj_cmd.Parameters.Add("@FACULTY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@FACULTY_CODE"].Value = model.faculty_code;
                obj_cmd.Parameters.Add("@MAJOR_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@MAJOR_CODE"].Value = model.major_code;
                obj_cmd.Parameters.Add("@QUALIFICATION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@QUALIFICATION_CODE"].Value = model.qualification_code;
                obj_cmd.Parameters.Add("@REQEDUCATION_GPA", SqlDbType.VarChar); obj_cmd.Parameters["@REQEDUCATION_GPA"].Value = model.empeducation_gpa;
                obj_cmd.Parameters.Add("@REQEDUCATION_START", SqlDbType.DateTime); obj_cmd.Parameters["@REQEDUCATION_START"].Value = model.empeducation_start;
                obj_cmd.Parameters.Add("@REQEDUCATION_FINISH", SqlDbType.DateTime); obj_cmd.Parameters["@REQEDUCATION_FINISH"].Value = model.empeducation_finish;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                strResult = model.empeducation_no.ToString();
            }
            catch (Exception ex)
            {
                Message = "REQEDT005:" + ex.ToString();
                strResult = "";
            }

            return blnResult;
        }

        public bool update(cls_TREducation model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE REQ_TR_EDUCATION SET ");

                obj_str.Append("INSTITUTE_CODE=@INSTITUTE_CODE ");
                obj_str.Append(", FACULTY_CODE=@FACULTY_CODE ");
                obj_str.Append(", MAJOR_CODE=@MAJOR_CODE ");
                obj_str.Append(", QUALIFICATION_CODE=@QUALIFICATION_CODE ");
                obj_str.Append(", REQEDUCATION_GPA=@REQEDUCATION_GPA ");
                obj_str.Append(", EQEDUCATION_START=@REQEDUCATION_START ");
                obj_str.Append(", REQEDUCATION_FINISH=@REQEDUCATION_FINISH ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE "); ;

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" AND REQEDUCATION_NO=@REQEDUCATION_NO ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@INSTITUTE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@INSTITUTE_CODE"].Value = model.institute_code;
                obj_cmd.Parameters.Add("@FACULTY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@FACULTY_CODE"].Value = model.faculty_code;
                obj_cmd.Parameters.Add("@MAJOR_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@MAJOR_CODE"].Value = model.major_code;
                obj_cmd.Parameters.Add("@QUALIFICATION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@QUALIFICATION_CODE"].Value = model.qualification_code;
                obj_cmd.Parameters.Add("@REQEDUCATION_GPA", SqlDbType.VarChar); obj_cmd.Parameters["@REQEDUCATION_GPA"].Value = model.empeducation_gpa;
                obj_cmd.Parameters.Add("@EQEDUCATION_START", SqlDbType.DateTime); obj_cmd.Parameters["@REQEDUCATION_START"].Value = model.empeducation_start;
                obj_cmd.Parameters.Add("@REQEDUCATION_FINISH", SqlDbType.DateTime); obj_cmd.Parameters["@REQEDUCATION_FINISH"].Value = model.empeducation_finish;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@REQEDUCATION_NO", SqlDbType.Int); obj_cmd.Parameters["@REQEDUCATION_NO"].Value = model.empeducation_no;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "REQEDT006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
