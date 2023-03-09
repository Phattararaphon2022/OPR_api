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
    public class cls_ctMTProject
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTProject() { }

        public string getMessage() { return this.Message.Replace("OPR_MT_PROJECT", "").Replace("cls_ctMTProject", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTProject> getData(string condition)
        {
            List<cls_MTProject> list_model = new List<cls_MTProject>();
            cls_MTProject model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PROJECT_ID");
                obj_str.Append(", PROJECT_CODE");
                obj_str.Append(", PROJECT_NAME_TH");
                obj_str.Append(", PROJECT_NAME_EN");
                obj_str.Append(", ISNULL(PROJECT_NAME_SHORT, '') AS PROJECT_NAME_SHORT");
                obj_str.Append(", PROJECT_TYPE");
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");    

                obj_str.Append(" FROM OPR_MT_PROJECT");
                obj_str.Append(" WHERE 1=1");
                
                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY CREATED_DATE DESC");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTProject();

                    model.project_id = Convert.ToInt32(dr["PROJECT_ID"]);
                    model.project_code = dr["PROJECT_CODE"].ToString();
                    model.project_name_th = dr["PROJECT_NAME_TH"].ToString();
                    model.project_name_en = dr["PROJECT_NAME_EN"].ToString();
                    model.project_name_short = dr["PROJECT_NAME_SHORT"].ToString();
                    model.project_type = dr["PROJECT_TYPE"].ToString();
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                                                                                            
                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "PRO001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTProject> getDataByFillter(string code, string type)
        {
            string strCondition = "";

            if (!code.Equals(""))
                strCondition += " AND PROJECT_CODE='" + code + "'";

            if (!type.Equals(""))
                strCondition += " AND PROJECT_TYPE='" + type + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(PROJECT_ID, 1) ");
                obj_str.Append(" FROM OPR_MT_PROJECT");
                obj_str.Append(" ORDER BY PROJECT_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "PRO002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PROJECT_CODE");
                obj_str.Append(" FROM OPR_MT_PROJECT");
                obj_str.Append(" WHERE PROJECT_CODE='" + code + "'");
      
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "PRO003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string code)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM OPR_MT_PROJECT");                
                obj_str.Append(" WHERE PROJECT_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "PRO004:" + ex.ToString();
            }

            return blnResult;
        }
        
        public string insert(cls_MTProject model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.project_code))
                {
                    if (this.update(model))
                        return model.project_id.ToString();
                    else
                        return "";                    
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                          
                obj_str.Append("INSERT INTO OPR_MT_PROJECT");
                obj_str.Append(" (");
                obj_str.Append("PROJECT_ID ");
                obj_str.Append(", PROJECT_CODE ");
                obj_str.Append(", PROJECT_NAME_TH ");
                obj_str.Append(", PROJECT_NAME_EN ");
                obj_str.Append(", PROJECT_NAME_SHORT ");
                obj_str.Append(", PROJECT_TYPE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");          
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@PROJECT_ID ");
                obj_str.Append(", @PROJECT_CODE ");
                obj_str.Append(", @PROJECT_NAME_TH ");
                obj_str.Append(", @PROJECT_NAME_EN ");
                obj_str.Append(", @PROJECT_NAME_SHORT ");
                obj_str.Append(", @PROJECT_TYPE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");
                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.project_id = this.getNextID();

                obj_cmd.Parameters.Add("@PROJECT_ID", SqlDbType.Int); obj_cmd.Parameters["@PROJECT_ID"].Value = model.project_id;
                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
                obj_cmd.Parameters.Add("@PROJECT_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_NAME_TH"].Value = model.project_name_th;
                obj_cmd.Parameters.Add("@PROJECT_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_NAME_EN"].Value = model.project_name_en;
                obj_cmd.Parameters.Add("@PROJECT_NAME_SHORT", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_NAME_SHORT"].Value = model.project_name_short;
                obj_cmd.Parameters.Add("@PROJECT_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_TYPE"].Value = model.project_type;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                                     
                obj_cmd.ExecuteNonQuery();
                                
                obj_conn.doClose();
                strResult = model.project_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "PRO005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTProject model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE OPR_MT_PROJECT SET ");
                obj_str.Append(" PROJECT_NAME_TH=@PROJECT_NAME_TH ");
                obj_str.Append(", PROJECT_NAME_EN=@PROJECT_NAME_EN ");
                obj_str.Append(", PROJECT_NAME_SHORT=@PROJECT_NAME_SHORT ");
                obj_str.Append(", PROJECT_TYPE=@PROJECT_TYPE ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE PROJECT_ID=@PROJECT_ID ");            

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROJECT_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_NAME_TH"].Value = model.project_name_th;
                obj_cmd.Parameters.Add("@PROJECT_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_NAME_EN"].Value = model.project_name_en;
                obj_cmd.Parameters.Add("@PROJECT_NAME_SHORT", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_NAME_SHORT"].Value = model.project_name_short;
                obj_cmd.Parameters.Add("@PROJECT_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_TYPE"].Value = model.project_type;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@PROJECT_ID", SqlDbType.Int); obj_cmd.Parameters["@PROJECT_ID"].Value = model.project_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "PRO006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}
