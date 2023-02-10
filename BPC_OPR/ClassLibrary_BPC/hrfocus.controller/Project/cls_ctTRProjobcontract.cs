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
    public class cls_ctTRProjobcontract
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRProjobcontract() { }

        public string getMessage() { return this.Message.Replace("PRO_TR_PROJOBCONTRACT", "").Replace("cls_ctTRProjobcontract", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRProjobcontract> getData(string condition)
        {
            List<cls_TRProjobcontract> list_model = new List<cls_TRProjobcontract>();
            cls_TRProjobcontract model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PROJOBCONTRACT_ID");
                obj_str.Append(", PROJOBCONTRACT_REF");
                obj_str.Append(", PROJOBCONTRACT_DATE");

                obj_str.Append(", ISNULL(PROJOBCONTRACT_EMP, 0) AS PROJOBCONTRACT_EMP");
                obj_str.Append(", ISNULL(PROJOBCONTRACT_AMOUNT, 0) AS PROJOBCONTRACT_AMOUNT");
                obj_str.Append(", ISNULL(PROJOBCONTRACT_FROMDATE, '01/01/1900') AS PROJOBCONTRACT_FROMDATE");
                obj_str.Append(", ISNULL(PROJOBCONTRACT_TODATE, '01/01/1900') AS PROJOBCONTRACT_TODATE");
               
                obj_str.Append(", PROJOB_CODE");     
                obj_str.Append(", PROJECT_CODE");                

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PRO_TR_PROJOBCONTRACT");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PROJECT_CODE, PROJOB_CODE, PROJOBCONTRACT_FROMDATE DESC, PROJOBCONTRACT_REF");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRProjobcontract();

                    model.projobcontract_id = Convert.ToInt32(dr["PROJOBCONTRACT_ID"]);
                    model.projobcontract_ref = Convert.ToString(dr["PROJOBCONTRACT_REF"]);
                    model.projobcontract_date = Convert.ToDateTime(dr["PROJOBCONTRACT_DATE"]);
                    model.projobcontract_emp = Convert.ToInt32(dr["PROJOBCONTRACT_EMP"]);
                    model.projobcontract_amount = Convert.ToDecimal(dr["PROJOBCONTRACT_AMOUNT"]);
                    model.projobcontract_fromdate = Convert.ToDateTime(dr["PROJOBCONTRACT_FROMDATE"]);
                    model.projobcontract_todate = Convert.ToDateTime(dr["PROJOBCONTRACT_TODATE"]);
                    
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

        public List<cls_TRProjobcontract> getDataByFillter(string project, string job)
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

                obj_str.Append("SELECT ISNULL(PROJOBCONTRACT_ID, 1) ");
                obj_str.Append(" FROM PRO_TR_PROJOBCONTRACT");
                obj_str.Append(" ORDER BY PROJOBCONTRACT_ID DESC ");

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

        public bool checkDataOld(string project, string job, string contract_ref)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PROJOBCONTRACT_REF");
                obj_str.Append(" FROM PRO_TR_PROJOBCONTRACT");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROJOB_CODE='" + job + "'");
                obj_str.Append(" AND PROJOBCONTRACT_REF='" + contract_ref + "'");

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

        public bool delete(string project, string job, string contract_ref)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_TR_PROJOBCONTRACT");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROJOB_CODE='" + job + "'");
                obj_str.Append(" AND PROJOBCOST_CODE='" + contract_ref + "'");
                
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

                obj_str.Append("DELETE FROM PRO_TR_PROJOBCONTRACT");
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

        public bool insert(cls_TRProjobcontract model)
        {
            bool blnResult = false;
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.project_code, model.projob_code, model.projobcontract_ref))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PRO_TR_PROJOBCONTRACT");
                obj_str.Append(" (");
                obj_str.Append("PROJOBCONTRACT_ID ");
                obj_str.Append(", PROJOBCONTRACT_REF ");
                obj_str.Append(", PROJOBCONTRACT_DATE ");
                obj_str.Append(", PROJOBCONTRACT_EMP ");
                obj_str.Append(", PROJOBCONTRACT_AMOUNT ");
                obj_str.Append(", PROJOBCONTRACT_FROMDATE ");
                obj_str.Append(", PROJOBCONTRACT_TODATE ");
                
                obj_str.Append(", PROJOB_CODE ");     
                obj_str.Append(", PROJECT_CODE ");      
         
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@PROJOBCONTRACT_ID ");
                obj_str.Append(", @PROJOBCONTRACT_REF ");
                obj_str.Append(", @PROJOBCONTRACT_DATE ");
                obj_str.Append(", @PROJOBCONTRACT_EMP ");
                obj_str.Append(", @PROJOBCONTRACT_AMOUNT ");
                obj_str.Append(", @PROJOBCONTRACT_FROMDATE ");
                obj_str.Append(", @PROJOBCONTRACT_TODATE ");

                obj_str.Append(", @PROJOB_CODE ");
                obj_str.Append(", @PROJECT_CODE ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROJOBCONTRACT_ID", SqlDbType.Int); obj_cmd.Parameters["@PROJOBCONTRACT_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@PROJOBCONTRACT_REF", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBCONTRACT_REF"].Value = model.projobcontract_ref;
                obj_cmd.Parameters.Add("@PROJOBCONTRACT_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROJOBCONTRACT_DATE"].Value = model.projobcontract_date;
                obj_cmd.Parameters.Add("@PROJOBCONTRACT_EMP", SqlDbType.Int); obj_cmd.Parameters["@PROJOBCONTRACT_EMP"].Value = model.projobcontract_emp;
                obj_cmd.Parameters.Add("@PROJOBCONTRACT_AMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@PROJOBCONTRACT_AMOUNT"].Value = model.projobcontract_amount;
                obj_cmd.Parameters.Add("@PROJOBCONTRACT_FROMDATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROJOBCONTRACT_FROMDATE"].Value = model.projobcontract_fromdate;
                obj_cmd.Parameters.Add("@PROJOBCONTRACT_TODATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROJOBCONTRACT_TODATE"].Value = model.projobcontract_todate;
                
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

        public bool update(cls_TRProjobcontract model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PRO_TR_PROJOBCONTRACT SET ");

                obj_str.Append(" PROJOBCONTRACT_REF=@PROJOBCONTRACT_REF ");
                obj_str.Append(", PROJOBCONTRACT_DATE=@PROJOBCONTRACT_DATE ");
                obj_str.Append(", PROJOBCONTRACT_EMP=@PROJOBCONTRACT_EMP ");
                obj_str.Append(", PROJOBCONTRACT_AMOUNT=@PROJOBCONTRACT_AMOUNT ");
                obj_str.Append(", PROJOBCONTRACT_FROMDATE=@PROJOBCONTRACT_FROMDATE ");
                obj_str.Append(", PROJOBCONTRACT_TODATE=@PROJOBCONTRACT_TODATE ");                               

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE PROJOBCONTRACT_ID=@PROJOBCONTRACT_ID ");
               
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROJOBCONTRACT_REF", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBCONTRACT_REF"].Value = model.projobcontract_ref;
                obj_cmd.Parameters.Add("@PROJOBCONTRACT_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROJOBCONTRACT_DATE"].Value = model.projobcontract_date;
                obj_cmd.Parameters.Add("@PROJOBCONTRACT_EMP", SqlDbType.Int); obj_cmd.Parameters["@PROJOBCONTRACT_EMP"].Value = model.projobcontract_emp;
                obj_cmd.Parameters.Add("@PROJOBCONTRACT_AMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@PROJOBCONTRACT_AMOUNT"].Value = model.projobcontract_amount;
                obj_cmd.Parameters.Add("@PROJOBCONTRACT_FROMDATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROJOBCONTRACT_FROMDATE"].Value = model.projobcontract_fromdate;
                obj_cmd.Parameters.Add("@PROJOBCONTRACT_TODATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROJOBCONTRACT_TODATE"].Value = model.projobcontract_todate;    

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@PROJOBCONTRACT_ID", SqlDbType.Int); obj_cmd.Parameters["@PROJOBCONTRACT_ID"].Value = model.projobcontract_id;

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
