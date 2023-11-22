using ClassLibrary_BPC.hrfocus.model.Project;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller.Project
{
   public class cls_ctTRProimages
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRProimages() { }

        public string getMessage() { return this.Message.Replace("PRO_TR_PROIMAGES", "").Replace("cls_ctTRProimages", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRProimages> getData(string condition)
        {
            List<cls_TRProimages> list_model = new List<cls_TRProimages>();
            cls_TRProimages model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", PROJECT_CODE");
                obj_str.Append(", PROIMAGES_NO");
                obj_str.Append(", PROIMAGES_IMAGES");
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PRO_TR_PROIMAGES");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, PROJECT_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRProimages();

                    object binaryData = dr["PROIMAGES_IMAGES"];

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.project_code = dr["PROJECT_CODE"].ToString();
                    model.proimages_no = Convert.ToInt32(dr["PROIMAGES_NO"]);
                    model.proimages_images = (byte[])binaryData;

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "IMGWRK001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRProimages> getDataByFillter(string com, string project)
        {
            string strCondition = " AND COMPANY_CODE='" + com + "'";

            if (!project.Equals(""))
                strCondition += " AND PROJECT_CODE='" + project + "'";


            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string project, string no)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PROJECT_CODE");
                obj_str.Append(" FROM PRO_TR_PROIMAGES");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROIMAGES_NO='" + no + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "IMGWRK002:" + ex.ToString();
            }

            return blnResult;
        }
                public bool delete(string com, string project, string no)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM PRO_TR_PROIMAGES");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROIMAGES_NO='" + no + "'");
                                          
                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "IMGWRK003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool clear(string com, string project)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM PRO_TR_PROIMAGES");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PROJECT_CODE='" + project + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "IMGWRK004:" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextNo(string com, string project)
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(PROIMAGES_NO) ");
                obj_str.Append(" FROM PRO_TR_PROIMAGES");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PROJECT_CODE='" + project + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "IMGWRK005:" + ex.ToString();
            }

            return intResult;
        }

        public bool insert(cls_TRProimages model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.project_code, model.proimages_no.ToString()))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PRO_TR_PROIMAGES");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", PROJECT_CODE ");
                obj_str.Append(", PROIMAGES_NO ");
                obj_str.Append(", PROIMAGES_IMAGES ");                                        
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @PROJECT_CODE ");
                obj_str.Append(", @PROIMAGES_NO ");
                obj_str.Append(", @PROIMAGES_IMAGES ");                
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
                obj_cmd.Parameters.Add("@PROIMAGES_NO", SqlDbType.Int); obj_cmd.Parameters["@PROIMAGES_NO"].Value = this.getNextNo(model.company_code, model.project_code);
                obj_cmd.Parameters.Add("@PROIMAGES_IMAGES", SqlDbType.Image); obj_cmd.Parameters["@PROIMAGES_IMAGES"].Value = model.proimages_images;                                               
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;
     
                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "IMGWRK006:" + ex.ToString();
            }

            return blnResult;
        }

        public bool update(cls_TRProimages model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE PRO_TR_PROIMAGES SET ");

                obj_str.Append(" PROIMAGES_IMAGES=@PROIMAGES_IMAGES ");
                

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND PROJECT_CODE=@PROJECT_CODE ");
                obj_str.Append(" AND PROIMAGES_NO=@PROIMAGES_NO ");
                               
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROIMAGES_IMAGES", SqlDbType.Image); obj_cmd.Parameters["@PROIMAGES_IMAGES"].Value = model.proimages_images;
                                 
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
                obj_cmd.Parameters.Add("@PROIMAGES_NO", SqlDbType.Int); obj_cmd.Parameters["@PROIMAGES_NO"].Value = model.proimages_no;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "IMGWRK007:" + ex.ToString();
            }

            return blnResult;
        }
    
    }
}
