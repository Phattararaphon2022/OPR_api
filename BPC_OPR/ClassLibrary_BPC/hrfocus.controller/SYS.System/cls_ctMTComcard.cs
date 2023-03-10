using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller
{
   public class cls_ctMTComcard
      {
   
     string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTComcard() { }

        public string getMessage() { return this.Message.Replace("SYS_MT_COMBANkK", "").Replace("cls_ctMTComcard", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }
        private List<cls_MTComcard> getData(string condition)
        {
            List<cls_MTComcard> list_model = new List<cls_MTComcard>();
            cls_MTComcard model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMCARD_ID");
                obj_str.Append(", COMCARD_CODE");
                obj_str.Append(", CARD_TYPE");
                obj_str.Append(", ISNULL(COMCARD_ISSUE, '') AS COMCARD_ISSUE");
                obj_str.Append(", ISNULL(COMCARD_EXPIRE, '') AS COMCARD_EXPIRE");

                obj_str.Append(", COMPANY_CODE");
                obj_str.Append(", ISNULL(COMBRANCH_CODE, '') AS COMBRANCH_CODE");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM SYS_MT_COMCARD");
                obj_str.Append(" WHERE 1=1");
                
                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTComcard();

                    model.comcard_id = Convert.ToInt32(dr["COMCARD_ID"]);
                    model.comcard_code = dr["COMCARD_CODE"].ToString();
                    model.card_type = dr["CARD_TYPE"].ToString();
                    model.comcard_issue = Convert.ToDateTime(dr["COMCARD_ISSUE"]);
                    model.comcard_expire = Convert.ToDateTime(dr["COMCARD_EXPIRE"]);

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.combranch_code = dr["COMBRANCH_CODE"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "CDD001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTComcard> getDataByFillter(string com , string type, string id,  string code, string branch)
        {
            string strCondition = " AND COMBRANCH_CODE='" + branch + "'";

            if (!type.Equals(""))
                strCondition += " AND CARD_TYPE='" + type + "'";

            if (!id.Equals(""))
                strCondition += " AND COMCARD_ID='" + id + "'";

            if (!code.Equals(""))
                strCondition += " AND COMCARD_CODE='" + code + "'";

            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

               
                obj_str.Append("SELECT MAX(COMCARD_ID) ");
                obj_str.Append(" FROM SYS_MT_COMCARD");       

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "CDD002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string type, string code, string branch)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMCARD_ID");
                obj_str.Append(" FROM SYS_MT_COMCARD");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND COMCARD_CODE='" + code + "'");
                obj_str.Append(" AND CARD_TYPE='" + type + "'");
                obj_str.Append(" AND COMBRANCH_CODE='" + branch + "'");
      
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "CDD003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string id)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SYS_MT_COMCARD");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMCARD_ID='" + id + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "CDD004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_MTComcard model)
        {
            bool blnResult = false;
            string strResult = "";
            try
            {

               
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.comcard_code, model.card_type, model.combranch_code))
                {
                    return this.update(model);
                }
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_MT_COMCARD");
                obj_str.Append(" (");
                obj_str.Append("COMCARD_ID ");
                obj_str.Append(", COMCARD_CODE ");
                obj_str.Append(", CARD_TYPE ");
                obj_str.Append(", COMCARD_ISSUE ");
                obj_str.Append(", COMCARD_EXPIRE ");

                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", COMBRANCH_CODE ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMCARD_ID ");
                obj_str.Append(", @COMCARD_CODE ");
                obj_str.Append(", @CARD_TYPE ");
                obj_str.Append(", @COMCARD_ISSUE ");
                obj_str.Append(", @COMCARD_EXPIRE ");

                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @COMBRANCH_CODE ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");
                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.comcard_id = this.getNextID();

                obj_cmd.Parameters.Add("@COMCARD_ID", SqlDbType.Int); obj_cmd.Parameters["@COMCARD_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@COMCARD_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMCARD_CODE"].Value = model.comcard_code;
                obj_cmd.Parameters.Add("@CARD_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@CARD_TYPE"].Value = model.card_type;
                obj_cmd.Parameters.Add("@COMCARD_ISSUE", SqlDbType.DateTime); obj_cmd.Parameters["@COMCARD_ISSUE"].Value = model.comcard_issue;
                obj_cmd.Parameters.Add("@COMCARD_EXPIRE", SqlDbType.DateTime); obj_cmd.Parameters["@COMCARD_EXPIRE"].Value = model.comcard_expire;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@COMBRANCH_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMBRANCH_CODE"].Value = model.combranch_code;


                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.ExecuteNonQuery();
                                
                obj_conn.doClose();
                strResult = model.comcard_id.ToString();
                blnResult = true;

            }
            catch (Exception ex)
            {
                Message = "CDD005:" + ex.ToString();
                strResult = "";
                blnResult = false;

            }

            return blnResult;
        }

        public bool update(cls_MTComcard model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE SYS_MT_COMCARD SET ");
                obj_str.Append(" COMCARD_CODE=@COMCARD_CODE ");
                obj_str.Append(", COMCARD_ISSUE=@COMCARD_ISSUE ");
                obj_str.Append(", COMCARD_EXPIRE=@COMCARD_EXPIRE ");
                obj_str.Append(", CARD_TYPE=@CARD_TYPE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");

                obj_str.Append(" WHERE COMCARD_ID=@COMCARD_ID ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMCARD_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMCARD_CODE"].Value = model.comcard_code;

                obj_cmd.Parameters.Add("@COMCARD_ISSUE", SqlDbType.DateTime); obj_cmd.Parameters["@COMCARD_ISSUE"].Value = model.comcard_issue;
                obj_cmd.Parameters.Add("@COMCARD_EXPIRE", SqlDbType.DateTime); obj_cmd.Parameters["@COMCARD_EXPIRE"].Value = model.comcard_expire;
                obj_cmd.Parameters.Add("@CARD_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@CARD_TYPE"].Value = model.card_type;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@COMCARD_ID", SqlDbType.Int); obj_cmd.Parameters["@COMCARD_ID"].Value = model.comcard_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "CDD006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}

