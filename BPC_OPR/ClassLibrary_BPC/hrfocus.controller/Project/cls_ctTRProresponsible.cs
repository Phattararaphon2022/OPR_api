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
    public class cls_ctTRProresponsible
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRProresponsible() { }

        public string getMessage() { return this.Message.Replace("PRO_TR_PRORESPONSIBLE", "").Replace("cls_ctTRProresponsible", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRProresponsible> getData(string condition)
        {
            List<cls_TRProresponsible> list_model = new List<cls_TRProresponsible>();
            cls_TRProresponsible model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PRORESPONSIBLE_ID");
                obj_str.Append(", PRORESPONSIBLE_REF");
                obj_str.Append(", PRORESPONSIBLE_EMP");
                obj_str.Append(", PRORESPONSIBLE_POSITION");
                obj_str.Append(", ISNULL(PRORESPONSIBLE_AREA, '') AS PRORESPONSIBLE_AREA");
                obj_str.Append(", PRORESPONSIBLE_FROMDATE");
                obj_str.Append(", PRORESPONSIBLE_TODATE");
                           
                obj_str.Append(", PROJECT_CODE");                

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PRO_TR_PRORESPONSIBLE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PROJECT_CODE, PRORESPONSIBLE_REF");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRProresponsible();

                    model.proresponsible_id = Convert.ToInt32(dr["PRORESPONSIBLE_ID"]);

                    model.proresponsible_ref = Convert.ToString(dr["PRORESPONSIBLE_REF"]);
                    model.proresponsible_emp = Convert.ToString(dr["PRORESPONSIBLE_EMP"]);
                    model.proresponsible_position = Convert.ToString(dr["PRORESPONSIBLE_POSITION"]);
                    model.proresponsible_area = Convert.ToString(dr["PRORESPONSIBLE_AREA"]);

                    model.proresponsible_fromdate = Convert.ToDateTime(dr["PRORESPONSIBLE_FROMDATE"]);
                    model.proresponsible_todate = Convert.ToDateTime(dr["PRORESPONSIBLE_TODATE"]);
                                                           
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

        public List<cls_TRProresponsible> getDataByFillter(string project)
        {
            string strCondition = "";

            if (!project.Equals(""))
                strCondition += " AND PROJECT_CODE='" + project + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(PRORESPONSIBLE_ID, 1) ");
                obj_str.Append(" FROM PRO_TR_PRORESPONSIBLE");
                obj_str.Append(" ORDER BY PRORESPONSIBLE_ID DESC ");

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

        public bool checkDataOld(string project, string contract_ref)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PRORESPONSIBLE_REF");
                obj_str.Append(" FROM PRO_TR_PRORESPONSIBLE");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PRORESPONSIBLE_REF='" + contract_ref + "'");

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

                obj_str.Append("DELETE FROM PRO_TR_PRORESPONSIBLE");
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

        public bool delete(string project, string contract_ref)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_TR_PRORESPONSIBLE");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PRORESPONSIBLE_REF='" + contract_ref + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BNK004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRProresponsible model)
        {
            bool blnResult = false;
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.project_code, model.proresponsible_ref))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PRO_TR_PRORESPONSIBLE");
                obj_str.Append(" (");
                obj_str.Append("PRORESPONSIBLE_ID ");
                obj_str.Append(", PRORESPONSIBLE_REF ");
                obj_str.Append(", PRORESPONSIBLE_EMP ");
                obj_str.Append(", PRORESPONSIBLE_POSITION ");
                obj_str.Append(", PRORESPONSIBLE_AREA ");
                obj_str.Append(", PRORESPONSIBLE_FROMDATE ");
                obj_str.Append(", PRORESPONSIBLE_TODATE ");     
                obj_str.Append(", PROJECT_CODE ");               
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@PRORESPONSIBLE_ID ");
                obj_str.Append(", @PRORESPONSIBLE_REF ");
                obj_str.Append(", @PRORESPONSIBLE_EMP ");
                obj_str.Append(", @PRORESPONSIBLE_POSITION ");
                obj_str.Append(", @PRORESPONSIBLE_AREA ");
                obj_str.Append(", @PRORESPONSIBLE_FROMDATE ");
                obj_str.Append(", @PRORESPONSIBLE_TODATE ");
                obj_str.Append(", @PROJECT_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PRORESPONSIBLE_ID", SqlDbType.Int); obj_cmd.Parameters["@PRORESPONSIBLE_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@PRORESPONSIBLE_REF", SqlDbType.VarChar); obj_cmd.Parameters["@PRORESPONSIBLE_REF"].Value = model.proresponsible_ref;
                obj_cmd.Parameters.Add("@PRORESPONSIBLE_EMP", SqlDbType.VarChar); obj_cmd.Parameters["@PRORESPONSIBLE_EMP"].Value = model.proresponsible_emp;
                obj_cmd.Parameters.Add("@PRORESPONSIBLE_POSITION", SqlDbType.VarChar); obj_cmd.Parameters["@PRORESPONSIBLE_POSITION"].Value = model.proresponsible_position;
                obj_cmd.Parameters.Add("@PRORESPONSIBLE_AREA", SqlDbType.VarChar); obj_cmd.Parameters["@PRORESPONSIBLE_AREA"].Value = model.proresponsible_area;
                obj_cmd.Parameters.Add("@PRORESPONSIBLE_FROMDATE", SqlDbType.DateTime); obj_cmd.Parameters["@PRORESPONSIBLE_FROMDATE"].Value = model.proresponsible_fromdate;
                obj_cmd.Parameters.Add("@PRORESPONSIBLE_TODATE", SqlDbType.DateTime); obj_cmd.Parameters["@PRORESPONSIBLE_TODATE"].Value = model.proresponsible_todate;               
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

        public bool update(cls_TRProresponsible model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PRO_TR_PRORESPONSIBLE SET ");

                obj_str.Append(" PRORESPONSIBLE_EMP=@PRORESPONSIBLE_EMP ");
                obj_str.Append(", PRORESPONSIBLE_POSITION=@PRORESPONSIBLE_POSITION ");
                obj_str.Append(", PRORESPONSIBLE_AREA=@PRORESPONSIBLE_AREA ");
                obj_str.Append(", PRORESPONSIBLE_FROMDATE=@PRORESPONSIBLE_FROMDATE ");
                obj_str.Append(", PRORESPONSIBLE_TODATE=@PRORESPONSIBLE_TODATE ");
                              
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE PRORESPONSIBLE_ID=@PRORESPONSIBLE_ID ");
               
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PRORESPONSIBLE_EMP", SqlDbType.Decimal); obj_cmd.Parameters["@PRORESPONSIBLE_EMP"].Value = model.proresponsible_emp;
                obj_cmd.Parameters.Add("@PRORESPONSIBLE_POSITION", SqlDbType.DateTime); obj_cmd.Parameters["@PRORESPONSIBLE_POSITION"].Value = model.proresponsible_position;
                obj_cmd.Parameters.Add("@PRORESPONSIBLE_AREA", SqlDbType.DateTime); obj_cmd.Parameters["@PRORESPONSIBLE_AREA"].Value = model.proresponsible_area;
                obj_cmd.Parameters.Add("@PRORESPONSIBLE_FROMDATE", SqlDbType.VarChar); obj_cmd.Parameters["@PRORESPONSIBLE_FROMDATE"].Value = model.proresponsible_fromdate;
                obj_cmd.Parameters.Add("@PRORESPONSIBLE_TODATE", SqlDbType.VarChar); obj_cmd.Parameters["@PRORESPONSIBLE_TODATE"].Value = model.proresponsible_todate;     

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;


                obj_cmd.Parameters.Add("@PRORESPONSIBLE_ID", SqlDbType.Int); obj_cmd.Parameters["@PRORESPONSIBLE_ID"].Value = model.proresponsible_id;

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
