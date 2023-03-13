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
    public class cls_ctTRProjobmachine
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRProjobmachine() { }

        public string getMessage() { return this.Message.Replace("PRO_TR_PROJOBMACHINE", "").Replace("cls_ctTRProjobmachine", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRProjobmachine> getData(string condition)
        {
            List<cls_TRProjobmachine> list_model = new List<cls_TRProjobmachine>();
            cls_TRProjobmachine model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PROJOBMACHINE_ID");
                obj_str.Append(", PROJOBMACHINE_IP");
                obj_str.Append(", PROJOBMACHINE_PORT");

                obj_str.Append(", ISNULL(PROJOBMACHINE_ENABLE, 0) AS PROJOBMACHINE_ENABLE");
                              
                obj_str.Append(", PROJOB_CODE");     
                obj_str.Append(", PROJECT_CODE");                

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PRO_TR_PROJOBMACHINE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PROJECT_CODE, PROJOB_CODE, PROJOBMACHINE_IP");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRProjobmachine();

                    model.projobmachine_id = Convert.ToInt32(dr["PROJOBMACHINE_ID"]);
                    model.projobmachine_ip = Convert.ToString(dr["PROJOBMACHINE_IP"]);
                    model.projobmachine_port = Convert.ToString(dr["PROJOBMACHINE_PORT"]);
                    model.projobmachine_enable = Convert.ToBoolean(dr["PROJOBMACHINE_ENABLE"]);
                                        
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

        public List<cls_TRProjobmachine> getDataByFillter(string project, string job)
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

                obj_str.Append("SELECT ISNULL(PROJOBMACHINE_ID, 1) ");
                obj_str.Append(" FROM PRO_TR_PROJOBMACHINE");
                obj_str.Append(" ORDER BY PROJOBMACHINE_ID DESC ");

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

        public bool checkDataOld(string project, string job, string ip)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PROJOBMACHINE_IP");
                obj_str.Append(" FROM PRO_TR_PROJOBMACHINE");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROJOB_CODE='" + job + "'");
                obj_str.Append(" AND PROJOBMACHINE_IP='" + ip + "'");

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

        public bool delete(string project, string job, string ip)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_TR_PROJOBMACHINE");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROJOB_CODE='" + job + "'");
                obj_str.Append(" AND PROJOBMACHINE_IP='" + ip + "'");
                
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

                obj_str.Append("DELETE FROM PRO_TR_PROJOBMACHINE");
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

        public bool insert(cls_TRProjobmachine model)
        {
            bool blnResult = false;
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.project_code, model.projob_code, model.projobmachine_ip))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PRO_TR_PROJOBMACHINE");
                obj_str.Append(" (");
                obj_str.Append("PROJOBMACHINE_ID ");
                obj_str.Append(", PROJOBMACHINE_IP ");
                obj_str.Append(", PROJOBMACHINE_PORT ");
                obj_str.Append(", PROJOBMACHINE_ENABLE ");               
                
                obj_str.Append(", PROJOB_CODE ");     
                obj_str.Append(", PROJECT_CODE ");      
         
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@PROJOBMACHINE_ID ");
                obj_str.Append(", @PROJOBMACHINE_IP ");
                obj_str.Append(", @PROJOBMACHINE_PORT ");
                obj_str.Append(", @PROJOBMACHINE_ENABLE ");
              
                obj_str.Append(", @PROJOB_CODE ");
                obj_str.Append(", @PROJECT_CODE ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROJOBMACHINE_ID", SqlDbType.Int); obj_cmd.Parameters["@PROJOBMACHINE_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@PROJOBMACHINE_IP", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBMACHINE_IP"].Value = model.projobmachine_ip;
                obj_cmd.Parameters.Add("@PROJOBMACHINE_PORT", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBMACHINE_PORT"].Value = model.projobmachine_port;
                obj_cmd.Parameters.Add("@PROJOBMACHINE_ENABLE", SqlDbType.Bit); obj_cmd.Parameters["@PROJOBMACHINE_ENABLE"].Value = model.projobmachine_enable;              
                
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

        public bool update(cls_TRProjobmachine model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PRO_TR_PROJOBMACHINE SET ");

                obj_str.Append(" PROJOBMACHINE_IP=@PROJOBMACHINE_IP ");
                obj_str.Append(", PROJOBMACHINE_PORT=@PROJOBMACHINE_PORT ");
                obj_str.Append(", PROJOBMACHINE_ENABLE=@PROJOBMACHINE_ENABLE ");
                                       

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE PROJOBMACHINE_ID=@PROJOBMACHINE_ID ");
               
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROJOBMACHINE_IP", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBMACHINE_IP"].Value = model.projobmachine_ip;
                obj_cmd.Parameters.Add("@PROJOBMACHINE_PORT", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBMACHINE_PORT"].Value = model.projobmachine_port;
                obj_cmd.Parameters.Add("@PROJOBMACHINE_ENABLE", SqlDbType.Bit); obj_cmd.Parameters["@PROJOBMACHINE_ENABLE"].Value = model.projobmachine_enable;               

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@PROJOBMACHINE_ID", SqlDbType.Int); obj_cmd.Parameters["@PROJOBMACHINE_ID"].Value = model.projobmachine_id;

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
