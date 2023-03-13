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
    public class cls_ctTRProjobworking
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRProjobworking() { }

        public string getMessage() { return this.Message.Replace("PRO_TR_PROJOBWORKING", "").Replace("cls_ctTRProjobemp", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRProjobworking> getData(string condition)
        {
            List<cls_TRProjobworking> list_model = new List<cls_TRProjobworking>();
            cls_TRProjobworking model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");
                obj_str.Append("PROJOBWORKING_ID");
                obj_str.Append(", PROJOBWORKING_EMP");
                obj_str.Append(", PROJOBWORKING_WORKDATE");
                obj_str.Append(", PROJOBWORKING_IN");
                obj_str.Append(", PROJOBWORKING_OUT");
                obj_str.Append(", PROJOBWORKING_STATUS");

                obj_str.Append(", PROJOB_CODE");     
                obj_str.Append(", PROJECT_CODE");                

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PRO_TR_PROJOBWORKING");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PROJECT_CODE, PROJOB_CODE, PROJOBWORKING_EMP");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRProjobworking();

                    model.projobworking_id = Convert.ToInt32(dr["PROJOBWORKING_ID"]);
                    model.projobworking_emp = Convert.ToString(dr["PROJOBWORKING_EMP"]);

                    model.projobworking_workdate = Convert.ToDateTime(dr["PROJOBWORKING_WORKDATE"]);
                    model.projobworking_in = Convert.ToDateTime(dr["PROJOBWORKING_IN"]);
                    model.projobworking_out = Convert.ToDateTime(dr["PROJOBWORKING_OUT"]);

                    model.projobworking_status = Convert.ToString(dr["PROJOBWORKING_STATUS"]); 
                    
                    model.projob_code = Convert.ToString(dr["PROJOB_CODE"]);                                        
                    model.project_code = Convert.ToString(dr["PROJECT_CODE"]);
                   
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "BNK001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRProjobworking> getDataByFillter(string project, string job)
        {
            string strCondition = "";

            if (!project.Equals(""))
                strCondition += " AND PROJECT_CODE='" + project + "'";

            if (!job.Equals(""))
                strCondition += " AND PROJOB_CODE='" + job + "'";

            return this.getData(strCondition);
        }
                
        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(PROJOBWORKING_ID, 1) ");
                obj_str.Append(" FROM PRO_TR_PROJOBWORKING");
                obj_str.Append(" ORDER BY PROJOBWORKING_ID DESC ");

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

        public bool checkDataOld(string project, string job, string emp, DateTime workdate)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PROJOBWORKING_EMP");
                obj_str.Append(" FROM PRO_TR_PROJOBWORKING");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROJOB_CODE='" + job + "'");
                obj_str.Append(" AND PROJOBWORKING_EMP='" + emp + "'");
                obj_str.Append(" AND PROJOBWORKING_WORKDATE='" + workdate.ToString("MM/dd/yyyy") + "'");

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

        public bool delete(string project, string job, string emp, DateTime workdate)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_TR_PROJOBWORKING");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROJOB_CODE='" + job + "'");
                obj_str.Append(" AND PROJOBWORKING_EMP='" + emp + "'");
                obj_str.Append(" AND PROJOBWORKING_WORKDATE='" + workdate.ToString("MM/dd/yyyy") + "'");
                
                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BNK004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string project, string job)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_TR_PROJOBWORKING");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROJOB_CODE='" + job + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BNK004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRProjobworking model)
        {
            bool blnResult = false;
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.project_code, model.projob_code, model.projobworking_emp, model.projobworking_workdate))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PRO_TR_PROJOBWORKING");
                obj_str.Append(" (");
                obj_str.Append("PROJOBWORKING_ID ");
                obj_str.Append(", PROJOBWORKING_EMP ");
                obj_str.Append(", PROJOBWORKING_WORKDATE ");
                obj_str.Append(", PROJOBWORKING_IN ");
                obj_str.Append(", PROJOBWORKING_OUT ");
                obj_str.Append(", PROJOBWORKING_STATUS ");
                
                obj_str.Append(", PROJOB_CODE ");     
                obj_str.Append(", PROJECT_CODE ");      
         
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@PROJOBWORKING_ID ");
                obj_str.Append(", @PROJOBWORKING_EMP ");
                obj_str.Append(", @PROJOBWORKING_WORKDATE ");
                obj_str.Append(", @PROJOBWORKING_IN ");
                obj_str.Append(", @PROJOBWORKING_OUT ");
                obj_str.Append(", @PROJOBWORKING_STATUS ");

                obj_str.Append(", @PROJOB_CODE ");
                obj_str.Append(", @PROJECT_CODE ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROJOBWORKING_ID", SqlDbType.Int); obj_cmd.Parameters["@PROJOBWORKING_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@PROJOBWORKING_EMP", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBWORKING_EMP"].Value = model.projobworking_emp;

                obj_cmd.Parameters.Add("@PROJOBWORKING_WORKDATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROJOBWORKING_IN"].Value = model.projobworking_workdate;
                obj_cmd.Parameters.Add("@PROJOBWORKING_IN", SqlDbType.DateTime); obj_cmd.Parameters["@PROJOBWORKING_IN"].Value = model.projobworking_in;
                obj_cmd.Parameters.Add("@PROJOBWORKING_OUT", SqlDbType.DateTime); obj_cmd.Parameters["@PROJOBWORKING_OUT"].Value = model.projobworking_out;

                obj_cmd.Parameters.Add("@PROJOBWORKING_STATUS", SqlDbType.Char); obj_cmd.Parameters["@PROJOBWORKING_STATUS"].Value = model.projobworking_status;
                
                obj_cmd.Parameters.Add("@PROJOB_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOB_CODE"].Value = model.projob_code;               
                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
                
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

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

        public bool update(cls_TRProjobworking model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PRO_TR_PROJOBWORKING SET ");

                obj_str.Append(" PROJOBWORKING_IN=@PROJOBWORKING_IN ");
                obj_str.Append(", PROJOBWORKING_OUT=@PROJOBWORKING_OUT ");              
                obj_str.Append(", PROJOBWORKING_STATUS=@PROJOBWORKING_STATUS ");                                  

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE PROJOBWORKING_ID=@PROJOBWORKING_ID ");
               
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());


                obj_cmd.Parameters.Add("@PROJOBWORKING_IN", SqlDbType.DateTime); obj_cmd.Parameters["@PROJOBWORKING_IN"].Value = model.projobworking_in;
                obj_cmd.Parameters.Add("@PROJOBWORKING_OUT", SqlDbType.DateTime); obj_cmd.Parameters["@PROJOBWORKING_OUT"].Value = model.projobworking_out;

                obj_cmd.Parameters.Add("@PROJOBWORKING_STATUS", SqlDbType.Char); obj_cmd.Parameters["@PROJOBWORKING_STATUS"].Value = model.projobworking_status;  

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@PROJOBWORKING_ID", SqlDbType.Int); obj_cmd.Parameters["@PROJOBWORKING_ID"].Value = model.projobworking_id;

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
