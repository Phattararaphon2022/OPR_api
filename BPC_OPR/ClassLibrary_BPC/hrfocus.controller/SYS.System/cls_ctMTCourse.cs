using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace ClassLibrary_BPC.hrfocus.controller
{
  public class cls_ctMTCourse
     {
   
     string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTCourse() { }

        public string getMessage() { return this.Message.Replace("SYS_MT_COURSE", "").Replace("cls_ctMTCourse", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTCourse> getData(string condition)
        {
            List<cls_MTCourse> list_model = new List<cls_MTCourse>();
            cls_MTCourse model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", COURSE_ID");
                obj_str.Append(", COURSE_CODE");
                obj_str.Append(", ISNULL(COURSE_NAME_TH, '') AS COURSE_NAME_TH");
                obj_str.Append(", ISNULL(COURSE_NAME_EN, '') AS COURSE_NAME_EN");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM SYS_MT_COURSE");
                obj_str.Append(" WHERE 1=1");
                
                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, COURSE_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTCourse();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                    model.course_id = Convert.ToInt32(dr["COURSE_ID"]);
                    model.course_code = Convert.ToString(dr["COURSE_CODE"]);
                    model.course_name_th = Convert.ToString(dr["COURSE_NAME_TH"]);
                    model.course_name_en = Convert.ToString(dr["COURSE_NAME_EN"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "COURSE001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTCourse> getDataByFillter(string com, string id, string code)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!id.Equals(""))
                strCondition += " AND COURSE_ID='" + id + "'";

            if (!code.Equals(""))
                strCondition += " AND COURSE_CODE='" + code + "'";

            return this.getData(strCondition);
        }
        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();



                obj_str.Append("SELECT MAX(COURSE_ID) ");
                obj_str.Append(" FROM SYS_MT_COURSE");      

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "COURSE002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();



                obj_str.Append("SELECT COURSE_ID");
                obj_str.Append(" FROM SYS_MT_COURSE");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND COURSE_CODE='" + code + "'");
      
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "COURSE003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string id)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SYS_MT_COURSE");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COURSE_ID='" + id + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "COURSE004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_MTCourse model)
        {
            bool blnResult = false;
            string strResult = "";
            try
            {

                //-- Check data old


                if (this.checkDataOld(model.company_code, model.course_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_MT_COURSE");
                obj_str.Append(" (");
                obj_str.Append("COURSE_ID ");
                obj_str.Append(", COURSE_CODE ");
                obj_str.Append(", COURSE_NAME_TH ");
                obj_str.Append(", COURSE_NAME_EN ");

                obj_str.Append(", COMPANY_CODE ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COURSE_ID ");
                obj_str.Append(", @COURSE_CODE ");
                obj_str.Append(", @COURSE_NAME_TH ");
                obj_str.Append(", @COURSE_NAME_EN ");
                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");
                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.course_id = this.getNextID();


                obj_cmd.Parameters.Add("@COURSE_ID", SqlDbType.Int); obj_cmd.Parameters["@COURSE_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@COURSE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COURSE_CODE"].Value = model.course_code;
                obj_cmd.Parameters.Add("@COURSE_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@COURSE_NAME_TH"].Value = model.course_name_th;
                obj_cmd.Parameters.Add("@COURSE_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@COURSE_NAME_EN"].Value = model.course_name_en;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.ExecuteNonQuery();
                                
                obj_conn.doClose();
                strResult = model.course_id.ToString();
                blnResult = true;

            }
            catch (Exception ex)
            {
                Message = "COURSE005:" + ex.ToString();
                strResult = "";
                blnResult = false;
            }

            return blnResult;
        }

        public bool update(cls_MTCourse model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE SYS_MT_COURSE SET ");

                obj_str.Append(" COURSE_CODE=@COURSE_CODE ");
                obj_str.Append(", COURSE_NAME_TH=@COURSE_NAME_TH ");
                obj_str.Append(", COURSE_NAME_EN=@COURSE_NAME_EN ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");


                obj_str.Append(" WHERE COURSE_ID=@COURSE_ID ");
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());


                obj_cmd.Parameters.Add("@COURSE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COURSE_CODE"].Value = model.course_code;
                obj_cmd.Parameters.Add("@COURSE_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@COURSE_NAME_TH"].Value = model.course_name_th;
                obj_cmd.Parameters.Add("@COURSE_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@COURSE_NAME_EN"].Value = model.course_name_en;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@COURSE_ID", SqlDbType.Int); obj_cmd.Parameters["@COURSE_ID"].Value = model.course_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "COURSE006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}

