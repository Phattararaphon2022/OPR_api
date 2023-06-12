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
                obj_str.Append(", ISNULL(PROJOBCONTRACT_REF, '') AS PROJOBCONTRACT_REF");

                obj_str.Append(", PROJOBCONTRACT_WORKING");
                obj_str.Append(", PROJOBCONTRACT_HRSPERDAY");
                obj_str.Append(", PROJOBCONTRACT_HRSOT");                
               
                obj_str.Append(", PROJOB_CODE");     
                obj_str.Append(", PROJECT_CODE");
                obj_str.Append(", VERSION");       

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PRO_TR_PROJOBCONTRACT");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PROJECT_CODE, PROJOB_CODE, VERSION DESC");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRProjobcontract();

                    model.projobcontract_id = Convert.ToInt32(dr["PROJOBCONTRACT_ID"]);
                    model.projobcontract_ref = Convert.ToString(dr["PROJOBCONTRACT_REF"]);
                    model.projobcontract_working = Convert.ToInt32(dr["PROJOBCONTRACT_WORKING"]);
                    model.projobcontract_hrsperday = Convert.ToDouble(dr["PROJOBCONTRACT_HRSPERDAY"]);
                    model.projobcontract_hrsot = Convert.ToDouble(dr["PROJOBCONTRACT_HRSOT"]);
                                        
                    model.projob_code = Convert.ToString(dr["PROJOB_CODE"]);                                        
                    model.project_code = Convert.ToString(dr["PROJECT_CODE"]);
                    model.version = Convert.ToString(dr["VERSION"]);
                   
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

        public List<cls_TRProjobcontract> getDataByFillter(string version, string project, string job)
        {
            string strCondition = "";

            if (!project.Equals(""))
                strCondition += " AND PROJECT_CODE='" + project + "'";

            if (!job.Equals(""))
                strCondition += " AND PROJOB_CODE='" + job + "'";

            if (!version.Equals(""))
                strCondition += " AND VERSION='" + version + "'";

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

        public bool checkDataOld(string version, string project, string job)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PROJOBCONTRACT_REF");
                obj_str.Append(" FROM PRO_TR_PROJOBCONTRACT");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROJOB_CODE='" + job + "'");
                obj_str.Append(" AND VERSION='" + version + "'");

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

        public bool delete(string version, string project, string job)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_TR_PROJOBCONTRACT");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROJOB_CODE='" + job + "'");
                obj_str.Append(" AND VERSION='" + version + "'");
                
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
                if (this.checkDataOld(model.version, model.project_code, model.projob_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PRO_TR_PROJOBCONTRACT");
                obj_str.Append(" (");
                obj_str.Append("PROJOBCONTRACT_ID ");
                obj_str.Append(", PROJOBCONTRACT_REF ");
                obj_str.Append(", PROJOBCONTRACT_WORKING ");
                obj_str.Append(", PROJOBCONTRACT_HRSPERDAY ");
                obj_str.Append(", PROJOBCONTRACT_HRSOT "); 
                obj_str.Append(", PROJOB_CODE ");     
                obj_str.Append(", PROJECT_CODE ");
                obj_str.Append(", VERSION ");            
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@PROJOBCONTRACT_ID ");
                obj_str.Append(", @PROJOBCONTRACT_REF ");
                obj_str.Append(", @PROJOBCONTRACT_WORKING ");
                obj_str.Append(", @PROJOBCONTRACT_HRSPERDAY ");
                obj_str.Append(", @PROJOBCONTRACT_HRSOT ");
                obj_str.Append(", @PROJOB_CODE ");
                obj_str.Append(", @PROJECT_CODE ");
                obj_str.Append(", @VERSION ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROJOBCONTRACT_ID", SqlDbType.Int); obj_cmd.Parameters["@PROJOBCONTRACT_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@PROJOBCONTRACT_REF", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBCONTRACT_REF"].Value = model.projobcontract_ref;
                obj_cmd.Parameters.Add("@PROJOBCONTRACT_WORKING", SqlDbType.Int); obj_cmd.Parameters["@PROJOBCONTRACT_WORKING"].Value = model.projobcontract_working;
                obj_cmd.Parameters.Add("@PROJOBCONTRACT_HRSPERDAY", SqlDbType.Decimal); obj_cmd.Parameters["@PROJOBCONTRACT_HRSPERDAY"].Value = model.projobcontract_hrsperday;
                obj_cmd.Parameters.Add("@PROJOBCONTRACT_HRSOT", SqlDbType.Decimal); obj_cmd.Parameters["@PROJOBCONTRACT_HRSOT"].Value = model.projobcontract_hrsot;
                        
                obj_cmd.Parameters.Add("@PROJOB_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOB_CODE"].Value = model.projob_code;               
                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
                obj_cmd.Parameters.Add("@VERSION", SqlDbType.VarChar); obj_cmd.Parameters["@VERSION"].Value = model.version;
                
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
                obj_str.Append(", PROJOBCONTRACT_WORKING=@PROJOBCONTRACT_WORKING ");
                obj_str.Append(", PROJOBCONTRACT_HRSPERDAY=@PROJOBCONTRACT_HRSPERDAY ");
                obj_str.Append(", PROJOBCONTRACT_HRSOT=@PROJOBCONTRACT_HRSOT ");                                      

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE PROJOBCONTRACT_ID=@PROJOBCONTRACT_ID ");
               
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROJOBCONTRACT_REF", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBCONTRACT_REF"].Value = model.projobcontract_ref;
                obj_cmd.Parameters.Add("@PROJOBCONTRACT_WORKING", SqlDbType.Int); obj_cmd.Parameters["@PROJOBCONTRACT_WORKING"].Value = model.projobcontract_working;
                obj_cmd.Parameters.Add("@PROJOBCONTRACT_HRSPERDAY", SqlDbType.Decimal); obj_cmd.Parameters["@PROJOBCONTRACT_HRSPERDAY"].Value = model.projobcontract_hrsperday;
                obj_cmd.Parameters.Add("@PROJOBCONTRACT_HRSOT", SqlDbType.Decimal); obj_cmd.Parameters["@PROJOBCONTRACT_HRSOT"].Value = model.projobcontract_hrsot;  

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
