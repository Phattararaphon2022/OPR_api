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
    public class cls_ctTRProcontact
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRProcontact() { }

        public string getMessage() { return this.Message.Replace("PRO_TR_PROCONTACT", "").Replace("cls_ctTRProcontact", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRProcontact> getData(string condition)
        {
            List<cls_TRProcontact> list_model = new List<cls_TRProcontact>();
            cls_TRProcontact model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PROCONTACT_ID");
                obj_str.Append(", PROCONTACT_REF");

                obj_str.Append(", ISNULL(PROCONTACT_FIRSTNAME_TH, '') AS PROCONTACT_FIRSTNAME_TH");
                obj_str.Append(", ISNULL(PROCONTACT_LASTNAME_TH, '') AS PROCONTACT_LASTNAME_TH");
                obj_str.Append(", ISNULL(PROCONTACT_FIRSTNAME_EN, '') AS PROCONTACT_FIRSTNAME_EN");
                obj_str.Append(", ISNULL(PROCONTACT_LASTNAME_EN, '') AS PROCONTACT_LASTNAME_EN");

                obj_str.Append(", ISNULL(PROCONTACT_TEL, '') AS PROCONTACT_TEL");
                obj_str.Append(", ISNULL(PROCONTACT_EMAIL, '') AS PROCONTACT_EMAIL");
                obj_str.Append(", ISNULL(POSITION_CODE, '') AS POSITION_CODE");
                obj_str.Append(", ISNULL(INITIAL_CODE, '') AS INITIAL_CODE");
                obj_str.Append(", PROJECT_CODE");
                

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PRO_TR_PROCONTACT");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PROJECT_CODE, PROCONTACT_REF");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRProcontact();

                    model.procontact_id = Convert.ToInt32(dr["PROCONTACT_ID"]);

                    model.procontact_ref = Convert.ToString(dr["PROCONTACT_REF"]);
                    model.procontact_firstname_th = Convert.ToString(dr["PROCONTACT_FIRSTNAME_TH"]);
                    model.procontact_lastname_th = Convert.ToString(dr["PROCONTACT_LASTNAME_TH"]);
                    model.procontact_firstname_en = Convert.ToString(dr["PROCONTACT_FIRSTNAME_EN"]);
                    model.procontact_lastname_en = Convert.ToString(dr["PROCONTACT_LASTNAME_EN"]);
                    model.procontact_tel = Convert.ToString(dr["PROCONTACT_TEL"]);
                    model.procontact_email = Convert.ToString(dr["PROCONTACT_EMAIL"]);
                    model.position_code = Convert.ToString(dr["POSITION_CODE"]);
                    model.initial_code = Convert.ToString(dr["INITIAL_CODE"]);
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

        public List<cls_TRProcontact> getDataByFillter(string project)
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

                obj_str.Append("SELECT ISNULL(PROCONTACT_ID, 1) ");
                obj_str.Append(" FROM PRO_MT_PROCONTACT");
                obj_str.Append(" ORDER BY PROCONTACT_ID DESC ");

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

        public bool checkDataOld(string project, string contact_ref)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PROCONTACT_REF");
                obj_str.Append(" FROM PRO_TR_PROCONTACT");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROCONTACT_REF='" + contact_ref + "'");

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

                obj_str.Append("DELETE FROM PRO_TR_PROCONTACT");
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

        public bool delete(string project, string contact_ref)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_TR_PROCONTACT");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROCONTACT_REF='" + contact_ref + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BNK004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRProcontact model)
        {
            bool blnResult = false;
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.project_code, model.procontact_ref))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PRO_TR_PROCONTACT");
                obj_str.Append(" (");
                obj_str.Append("PROCONTACT_ID ");
                obj_str.Append(", PROCONTACT_REF ");
                obj_str.Append(", PROCONTACT_FIRSTNAME_TH ");
                obj_str.Append(", PROCONTACT_LASTNAME_TH ");
                obj_str.Append(", PROCONTACT_FIRSTNAME_EN ");
                obj_str.Append(", PROCONTACT_LASTNAME_EN ");
                obj_str.Append(", PROCONTACT_TEL ");
                obj_str.Append(", PROCONTACT_EMAIL ");
                obj_str.Append(", POSITION_CODE ");
                obj_str.Append(", INITIAL_CODE ");
                obj_str.Append(", PROJECT_CODE ");               
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");

                obj_str.Append("@PROCONTACT_ID ");
                obj_str.Append(", @PROCONTACT_REF ");
                obj_str.Append(", @PROCONTACT_FIRSTNAME_TH ");
                obj_str.Append(", @PROCONTACT_LASTNAME_TH ");
                obj_str.Append(", @PROCONTACT_FIRSTNAME_EN ");
                obj_str.Append(", @PROCONTACT_LASTNAME_EN ");
                obj_str.Append(", @PROCONTACT_TEL ");
                obj_str.Append(", @PROCONTACT_EMAIL ");
                obj_str.Append(", @POSITION_CODE ");
                obj_str.Append(", @INITIAL_CODE ");
                obj_str.Append(", @PROJECT_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROCONTACT_ID", SqlDbType.Int); obj_cmd.Parameters["@PROCONTACT_ID"].Value = this.getNextID();              
                obj_cmd.Parameters.Add("@PROCONTACT_REF", SqlDbType.VarChar); obj_cmd.Parameters["@PROCONTACT_REF"].Value = model.procontact_ref;
                obj_cmd.Parameters.Add("@PROCONTACT_FIRSTNAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROCONTACT_FIRSTNAME_TH"].Value = model.procontact_firstname_th;
                obj_cmd.Parameters.Add("@PROCONTACT_LASTNAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROCONTACT_LASTNAME_TH"].Value = model.procontact_lastname_th;
                obj_cmd.Parameters.Add("@PROCONTACT_FIRSTNAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROCONTACT_FIRSTNAME_EN"].Value = model.procontact_firstname_en;
                obj_cmd.Parameters.Add("@PROCONTACT_LASTNAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROCONTACT_LASTNAME_EN"].Value = model.procontact_lastname_en;
                obj_cmd.Parameters.Add("@PROCONTACT_TEL", SqlDbType.VarChar); obj_cmd.Parameters["@PROCONTACT_TEL"].Value = model.procontact_tel;
                obj_cmd.Parameters.Add("@PROCONTACT_EMAIL", SqlDbType.VarChar); obj_cmd.Parameters["@PROCONTACT_EMAIL"].Value = model.procontact_email;
                obj_cmd.Parameters.Add("@POSITION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@POSITION_CODE"].Value = model.position_code;
                obj_cmd.Parameters.Add("@INITIAL_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@INITIAL_CODE"].Value = model.initial_code;
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

        public bool update(cls_TRProcontact model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PRO_TR_PROCONTACT SET ");

                obj_str.Append(" PROCONTACT_FIRSTNAME_TH=@PROCONTACT_FIRSTNAME_TH ");
                obj_str.Append(", PROCONTACT_LASTNAME_TH=@PROCONTACT_LASTNAME_TH ");
                obj_str.Append(", PROCONTACT_FIRSTNAME_EN=@PROCONTACT_FIRSTNAME_EN ");
                obj_str.Append(", PROCONTACT_LASTNAME_EN=@PROCONTACT_LASTNAME_EN ");
                obj_str.Append(", PROCONTACT_TEL=@PROCONTACT_TEL ");
                obj_str.Append(", PROCONTACT_EMAIL=@PROCONTACT_EMAIL ");
                obj_str.Append(", POSITION_CODE=@POSITION_CODE ");
                obj_str.Append(", INITIAL_CODE=@INITIAL_CODE ");              

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE PROCONTACT_ID=@PROCONTACT_ID ");
               
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROCONTACT_FIRSTNAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROCONTACT_FIRSTNAME_TH"].Value = model.procontact_firstname_th;
                obj_cmd.Parameters.Add("@PROCONTACT_LASTNAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROCONTACT_LASTNAME_TH"].Value = model.procontact_lastname_th;
                obj_cmd.Parameters.Add("@PROCONTACT_FIRSTNAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROCONTACT_FIRSTNAME_EN"].Value = model.procontact_firstname_en;
                obj_cmd.Parameters.Add("@PROCONTACT_LASTNAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROCONTACT_LASTNAME_EN"].Value = model.procontact_lastname_en;
                obj_cmd.Parameters.Add("@PROCONTACT_TEL", SqlDbType.VarChar); obj_cmd.Parameters["@PROCONTACT_TEL"].Value = model.procontact_tel;
                obj_cmd.Parameters.Add("@PROCONTACT_EMAIL", SqlDbType.VarChar); obj_cmd.Parameters["@PROCONTACT_EMAIL"].Value = model.procontact_email;
                obj_cmd.Parameters.Add("@POSITION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@POSITION_CODE"].Value = model.position_code;
                obj_cmd.Parameters.Add("@INITIAL_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@INITIAL_CODE"].Value = model.initial_code;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;


                obj_cmd.Parameters.Add("@PROCONTACT_ID", SqlDbType.Int); obj_cmd.Parameters["@PROCONTACT_ID"].Value = model.procontact_id;

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
