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
    public class cls_ctTRProjobpol
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRProjobpol() { }

        public string getMessage() { return this.Message.Replace("PRO_TR_PROJOBPOL", "").Replace("cls_ctTRProjobpol", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRProjobpol> getData(string condition)
        {
            List<cls_TRProjobpol> list_model = new List<cls_TRProjobpol>();
            cls_TRProjobpol model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PROJOBPOL_ID");
            
                obj_str.Append(", ISNULL(PROJOBPOL_TYPE, '') AS PROJOBPOL_TYPE");
                obj_str.Append(", ISNULL(PROJOBPOL_TIMEPOL, '') AS PROJOBPOL_TIMEPOL");
                obj_str.Append(", ISNULL(PROJOBPOL_SLIP, '') AS PROJOBPOL_SLIP");
                obj_str.Append(", ISNULL(PROJOBPOL_UNIFORM, '') AS PROJOBPOL_UNIFORM");

                obj_str.Append(", PROJECT_CODE");
                obj_str.Append(", PROJOBMAIN_CODE");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");    

                obj_str.Append(" FROM PRO_TR_PROJOBPOL");
                obj_str.Append(" WHERE 1=1");
                
                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PROJECT_CODE, PROJOBMAIN_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRProjobpol();

                    model.projobpol_id = Convert.ToInt32(dr["PROJOBPOL_ID"]);
                   
                    model.projobpol_type = dr["PROJOBPOL_TYPE"].ToString();
                    model.projobpol_timepol = dr["PROJOBPOL_TIMEPOL"].ToString();
                    model.projobpol_slip = dr["PROJOBPOL_SLIP"].ToString();
                    model.projobpol_uniform = dr["PROJOBPOL_UNIFORM"].ToString();

                    model.project_code = dr["PROJECT_CODE"].ToString();
                    model.projobmain_code = dr["PROJOBMAIN_CODE"].ToString(); 

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                                                                                            
                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "BNK001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRProjobpol> getDataByFillter(string project, string jobmain)
        {
            string strCondition = "";

            if (!project.Equals(""))
                strCondition += " AND PROJECT_CODE='" + project + "'";

            if (!jobmain.Equals(""))
                strCondition += " AND PROJOBMAIN_CODE='" + jobmain + "'";
            
            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(PROJOBPOL_ID, 1) ");
                obj_str.Append(" FROM PRO_TR_PROJOBPOL");
                obj_str.Append(" ORDER BY PROJOBPOL_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "BNK002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string project, string jobmain)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PROJOBPOL_CODE");
                obj_str.Append(" FROM PRO_TR_PROJOBPOL");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");                
                obj_str.Append(" AND PROJOBMAIN_CODE='" + jobmain + "'");
      
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "BNK003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string project)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_TR_PROJOBPOL");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BNK004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string project, string jobmain)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_TR_PROJOBPOL");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");                
                obj_str.Append(" AND PROJOBMAIN_CODE='" + jobmain + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BNK004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRProjobpol model)
        {
            bool blnResult = false;
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.project_code, model.projobmain_code))
                {
                    return this.update(model);               
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                          
                obj_str.Append("INSERT INTO PRO_TR_PROJOBPOL");
                obj_str.Append(" (");
                obj_str.Append("PROJOBPOL_ID ");
               
                obj_str.Append(", PROJOBPOL_TYPE ");  
                obj_str.Append(", PROJOBPOL_TIMEPOL ");
                obj_str.Append(", PROJOBPOL_SLIP ");
                obj_str.Append(", PROJOBPOL_UNIFORM ");

                obj_str.Append(", PROJECT_CODE ");
                obj_str.Append(", PROJOBMAIN_CODE ");    

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");          
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@PROJOBPOL_ID ");               
               
                obj_str.Append(", @PROJOBPOL_TYPE ");
                obj_str.Append(", @PROJOBPOL_TIMEPOL ");
                obj_str.Append(", @PROJOBPOL_SLIP ");
                obj_str.Append(", @PROJOBPOL_UNIFORM ");

                obj_str.Append(", @PROJECT_CODE ");
                obj_str.Append(", @PROJOBMAIN_CODE ");    

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");
                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.projobpol_id = this.getNextID();

                obj_cmd.Parameters.Add("@PROJOBPOL_ID", SqlDbType.Int); obj_cmd.Parameters["@PROJOBPOL_ID"].Value = model.projobpol_id;
            
                obj_cmd.Parameters.Add("@PROJOBPOL_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBPOL_TYPE"].Value = model.projobpol_type;
                obj_cmd.Parameters.Add("@PROJOBPOL_TIMEPOL", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBPOL_TIMEPOL"].Value = model.projobpol_timepol;
                obj_cmd.Parameters.Add("@PROJOBPOL_SLIP", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBPOL_SLIP"].Value = model.projobpol_slip;
                obj_cmd.Parameters.Add("@PROJOBPOL_UNIFORM", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBPOL_UNIFORM"].Value = model.projobpol_uniform;
                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
                obj_cmd.Parameters.Add("@PROJOBMAIN_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBMAIN_CODE"].Value = model.projobmain_code;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                                     
                obj_cmd.ExecuteNonQuery();
                                
                obj_conn.doClose();
                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "BNK005:" + ex.ToString();                
            }

            return blnResult;
        }

        public bool update(cls_TRProjobpol model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PRO_TR_PROJOBPOL SET ");
           
                obj_str.Append(" PROJOBPOL_TYPE=@PROJOBPOL_TYPE ");
                obj_str.Append(", PROJOBPOL_TIMEPOL=@PROJOBPOL_TIMEPOL ");
                obj_str.Append(", PROJOBPOL_SLIP=@PROJOBPOL_SLIP ");
                obj_str.Append(", PROJOBPOL_UNIFORM=@PROJOBPOL_UNIFORM ");
               
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE PROJOBPOL_ID=@PROJOBPOL_ID ");            

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROJOBPOL_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBPOL_TYPE"].Value = model.projobpol_type;
                obj_cmd.Parameters.Add("@PROJOBPOL_TIMEPOL", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBPOL_TIMEPOL"].Value = model.projobpol_timepol;
                obj_cmd.Parameters.Add("@PROJOBPOL_SLIP", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBPOL_SLIP"].Value = model.projobpol_slip;
                obj_cmd.Parameters.Add("@PROJOBPOL_UNIFORM", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBPOL_UNIFORM"].Value = model.projobpol_uniform;     

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@PROJOBPOL_ID", SqlDbType.Int); obj_cmd.Parameters["@PROJOBPOL_ID"].Value = model.projobpol_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "BNK006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}
