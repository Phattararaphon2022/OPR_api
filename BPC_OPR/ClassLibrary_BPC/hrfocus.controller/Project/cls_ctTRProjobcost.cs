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
    public class cls_ctTRProjobcost
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRProjobcost() { }

        public string getMessage() { return this.Message.Replace("PRO_TR_PROJOBCOST", "").Replace("cls_ctTRProjobcost", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRProjobcost> getData(string condition)
        {
            List<cls_TRProjobcost> list_model = new List<cls_TRProjobcost>();
            cls_TRProjobcost model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PROJOBCOST_ID");
                obj_str.Append(", PROJOBCOST_CODE");                
                
                obj_str.Append(", ISNULL(PROJOBCOST_AMOUNT, 0) AS PROJOBCOST_AMOUNT");
                obj_str.Append(", ISNULL(PROJOBCOST_FROMDATE, '01/01/1900') AS PROJOBCOST_FROMDATE");
                obj_str.Append(", ISNULL(PROJOBCOST_TODATE, '01/01/1900') AS PROJOBCOST_TODATE");

                obj_str.Append(", PROJOBCOST_VERSION");
                obj_str.Append(", PROJOBCOST_STATUS");

                obj_str.Append(", ISNULL(PROJOBCOST_AUTO, 0) AS PROJOBCOST_AUTO");

                obj_str.Append(", PROJOB_CODE");     
                obj_str.Append(", PROJECT_CODE");                

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PRO_TR_PROJOBCOST");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PROJECT_CODE, PROJOB_CODE, PROJOBCOST_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRProjobcost();

                    model.projobcost_id = Convert.ToInt32(dr["PROJOBCOST_ID"]);
                    model.projobcost_code = Convert.ToString(dr["PROJOBCOST_CODE"]);
                    
                    model.projobcost_amount = Convert.ToDouble(dr["PROJOBCOST_AMOUNT"]);
                    model.projobcost_fromdate = Convert.ToDateTime(dr["PROJOBCOST_FROMDATE"]);
                    model.projobcost_todate = Convert.ToDateTime(dr["PROJOBCOST_TODATE"]);

                    model.projobcost_version = Convert.ToString(dr["PROJOBCOST_VERSION"]);
                    model.projobcost_status = Convert.ToString(dr["PROJOBCOST_STATUS"]);

                    model.projobcost_auto = Convert.ToBoolean(dr["PROJOBCOST_AUTO"]); 
                    
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

        public List<cls_TRProjobcost> getDataByFillter(string project, string job)
        {
            string strCondition = "";

            if (!project.Equals(""))
                strCondition += " AND PROJECT_CODE='" + project + "'";

            if (!job.Equals(""))
                strCondition += " AND PROJOB_CODE='" + job + "'";

            return this.getData(strCondition);
        }

        public List<cls_TRProjobcost> getDataMaxDate(string company, string project, string job)
        {
            List<cls_TRProjobcost> list_model = new List<cls_TRProjobcost>();
            cls_TRProjobcost model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" SELECT PRO_MT_PROCOST.PROCOST_CODE");
                obj_str.Append(" , PRO_MT_PROCOST.PROCOST_NAME_TH");
                obj_str.Append(" , PRO_MT_PROCOST.PROCOST_NAME_EN");
                obj_str.Append(" , PRO_MT_PROCOST.PROCOST_TYPE");
                obj_str.Append(" , ISNULL((SELECT TOP 1 PROJOBCOST_AMOUNT FROM PRO_TR_PROJOBCOST WHERE PROJECT_CODE='" + project + "' AND PROJOB_CODE= '" + job + "' AND PROJOBCOST_CODE=PRO_MT_PROCOST.PROCOST_CODE ORDER BY PROJOBCOST_TODATE DESC), 0) AS ALLOW");
                                
                obj_str.Append(" FROM PRO_MT_PROCOST");
                obj_str.Append(" WHERE COMPANY_CODE='" + company + "'");                
                obj_str.Append(" ORDER BY PROCOST_CODE");               
                

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRProjobcost();

                    model.projobcost_id = 1;
                    model.projobcost_code = Convert.ToString(dr["PROCOST_CODE"]);
                    model.procost_type = Convert.ToString(dr["PROCOST_TYPE"]);
                    model.projobcost_amount = Convert.ToDouble(dr["ALLOW"]);                   
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "BNK010:" + ex.ToString();
            }

            return list_model;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(PROJOBCOST_ID, 1) ");
                obj_str.Append(" FROM PRO_TR_PROJOBCOST");
                obj_str.Append(" ORDER BY PROJOBCOST_ID DESC ");

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

        public bool checkDataOld(string project, string job, string cost, DateTime fromdate)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PROJOBCOST_CODE");
                obj_str.Append(" FROM PRO_TR_PROJOBCOST");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROJOB_CODE='" + job + "'");
                obj_str.Append(" AND PROJOBCOST_CODE='" + cost + "'");
                obj_str.Append(" AND PROJOBCOST_FROMDATE='" + fromdate.ToString("MM/dd/yyyy") + "'");

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

        public bool delete(string project, string job, string cost, DateTime fromdate)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_TR_PROJOBCOST");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROJOB_CODE='" + job + "'");
                obj_str.Append(" AND PROJOBCOST_CODE='" + cost + "'");
                obj_str.Append(" AND PROJOBCOST_FROMDATE='" + fromdate.ToString("MM/dd/yyyy") + "'");
                
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

                obj_str.Append("DELETE FROM PRO_TR_PROJOBCOST");
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

        public bool insert(cls_TRProjobcost model)
        {
            bool blnResult = false;
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.project_code, model.projob_code, model.projobcost_code, model.projobcost_fromdate))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PRO_TR_PROJOBCOST");
                obj_str.Append(" (");
                obj_str.Append("PROJOBCOST_ID ");
                obj_str.Append(", PROJOBCOST_CODE ");
                
                obj_str.Append(", PROJOBCOST_AMOUNT ");
                obj_str.Append(", PROJOBCOST_FROMDATE ");
                obj_str.Append(", PROJOBCOST_TODATE ");

                obj_str.Append(", PROJOBCOST_VERSION ");
                obj_str.Append(", PROJOBCOST_STATUS ");

                obj_str.Append(", PROJOBCOST_AUTO ");
                
                obj_str.Append(", PROJOB_CODE ");     
                obj_str.Append(", PROJECT_CODE ");      
         
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@PROJOBCOST_ID ");
                obj_str.Append(", @PROJOBCOST_CODE ");
                
                obj_str.Append(", @PROJOBCOST_AMOUNT ");
                obj_str.Append(", @PROJOBCOST_FROMDATE ");
                obj_str.Append(", @PROJOBCOST_TODATE ");

                obj_str.Append(", @PROJOBCOST_VERSION ");
                obj_str.Append(", @PROJOBCOST_STATUS ");

                obj_str.Append(", @PROJOBCOST_AUTO ");

                obj_str.Append(", @PROJOB_CODE ");
                obj_str.Append(", @PROJECT_CODE ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROJOBCOST_ID", SqlDbType.Int); obj_cmd.Parameters["@PROJOBCOST_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@PROJOBCOST_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBCOST_CODE"].Value = model.projobcost_code;

                obj_cmd.Parameters.Add("@PROJOBCOST_AMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@PROJOBCOST_AMOUNT"].Value = model.projobcost_amount;
                obj_cmd.Parameters.Add("@PROJOBCOST_FROMDATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROJOBCOST_FROMDATE"].Value = model.projobcost_fromdate;
                obj_cmd.Parameters.Add("@PROJOBCOST_TODATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROJOBCOST_TODATE"].Value = model.projobcost_todate;

                obj_cmd.Parameters.Add("@PROJOBCOST_VERSION", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBCOST_VERSION"].Value = model.projobcost_version;
                obj_cmd.Parameters.Add("@PROJOBCOST_STATUS", SqlDbType.Char); obj_cmd.Parameters["@PROJOBCOST_STATUS"].Value = model.projobcost_status;

                obj_cmd.Parameters.Add("@PROJOBCOST_AUTO", SqlDbType.Bit); obj_cmd.Parameters["@PROJOBCOST_AUTO"].Value = model.projobcost_auto;
                
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

        public bool update(cls_TRProjobcost model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PRO_TR_PROJOBCOST SET ");

                obj_str.Append(" PROJOBCOST_AMOUNT=@PROJOBCOST_AMOUNT ");
                obj_str.Append(", PROJOBCOST_FROMDATE=@PROJOBCOST_FROMDATE ");
                obj_str.Append(", PROJOBCOST_TODATE=@PROJOBCOST_TODATE ");
                obj_str.Append(", PROJOBCOST_VERSION=@PROJOBCOST_VERSION ");
                obj_str.Append(", PROJOBCOST_STATUS=@PROJOBCOST_STATUS ");

                obj_str.Append(", PROJOBCOST_AUTO=@PROJOBCOST_AUTO ");      

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE PROJOBCOST_ID=@PROJOBCOST_ID ");
               
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROJOBCOST_AMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@PROJOBCOST_AMOUNT"].Value = model.projobcost_amount;
                obj_cmd.Parameters.Add("@PROJOBCOST_FROMDATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROJOBCOST_FROMDATE"].Value = model.projobcost_fromdate;
                obj_cmd.Parameters.Add("@PROJOBCOST_TODATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROJOBCOST_TODATE"].Value = model.projobcost_todate;

                obj_cmd.Parameters.Add("@PROJOBCOST_VERSION", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBCOST_VERSION"].Value = model.projobcost_version;
                obj_cmd.Parameters.Add("@PROJOBCOST_STATUS", SqlDbType.Char); obj_cmd.Parameters["@PROJOBCOST_STATUS"].Value = model.projobcost_status;

                obj_cmd.Parameters.Add("@PROJOBCOST_AUTO", SqlDbType.Bit); obj_cmd.Parameters["@PROJOBCOST_AUTO"].Value = model.projobcost_auto;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@PROJOBCOST_ID", SqlDbType.Int); obj_cmd.Parameters["@PROJOBCOST_ID"].Value = model.projobcost_id;

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
