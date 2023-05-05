using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;
namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRLineapprove
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRLineapprove() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRLineapprove> getData(string condition)
        {
            List<cls_TRLineapprove> list_model = new List<cls_TRLineapprove>();
            cls_TRLineapprove model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKFLOW_TYPE");
                obj_str.Append(", WORKFLOW_CODE");
                obj_str.Append(", POSITION_LEVEL");
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");
                obj_str.Append(", FLAG ");
                obj_str.Append(" FROM SELF_TR_LINEAPPROVE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRLineapprove();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);

                    model.workflow_type = Convert.ToString(dr["WORKFLOW_TYPE"]);
                    model.workflow_code = Convert.ToString(dr["WORKFLOW_CODE"]);
                    model.position_level = Convert.ToString(dr["POSITION_LEVEL"]);
                    model.modified_by = Convert.ToString(dr["MODIFIED_BY"]);
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                    model.flag = Convert.ToBoolean(dr["FLAG"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Lineapprove.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRLineapprove> getDataByFillter(string com, string workflow_type, string workflow_code, string position_level)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!workflow_type.Equals(""))
                strCondition += " AND WORKFLOW_TYPE='" + workflow_type + "'";
            if (!workflow_code.Equals(""))
                strCondition += " AND WORKFLOW_CODE='" + workflow_code + "'";
            if (!position_level.Equals(""))
                strCondition += " AND POSITION_LEVEL='" + position_level + "'";
            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string workflow_type, string position_level)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM SELF_TR_LINEAPPROVE");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKFLOW_TYPE='" + workflow_type + "' ");
                obj_str.Append(" AND POSITION_LEVEL='" + position_level + "' ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Lineapprove.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string workflow_type, string workflow_code, string position_level)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SELF_TR_LINEAPPROVE");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                if (!workflow_type.Equals(""))
                    obj_str.Append(" AND WORKFLOW_TYPE='" + workflow_type + "' ");
                if (!workflow_code.Equals(""))
                    obj_str.Append(" AND WORKFLOW_CODE='" + workflow_code + "' ");
                if (!position_level.Equals(""))
                    obj_str.Append(" AND POSITION_LEVEL='" + position_level + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());


            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Lineapprove.delete)" + ex.ToString();
            }

            return blnResult;
        }
        public string insert(cls_TRLineapprove model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.workflow_type,model.position_level))
                {
                    return "";
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("INSERT INTO SELF_TR_LINEAPPROVE");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKFLOW_TYPE ");
                obj_str.Append(", WORKFLOW_CODE ");
                obj_str.Append(", POSITION_LEVEL ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKFLOW_TYPE ");
                obj_str.Append(", @WORKFLOW_CODE ");
                obj_str.Append(", @POSITION_LEVEL ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKFLOW_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKFLOW_TYPE"].Value = model.workflow_type;
                obj_cmd.Parameters.Add("@WORKFLOW_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKFLOW_CODE"].Value = model.workflow_code;
                obj_cmd.Parameters.Add("@POSITION_LEVEL", SqlDbType.VarChar); obj_cmd.Parameters["@POSITION_LEVEL"].Value = model.position_level;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;


                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.workflow_code.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Lineapprove.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(List<cls_TRLineapprove> list_model,string username)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SELF_TR_LINEAPPROVE");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKFLOW_TYPE ");
                obj_str.Append(", WORKFLOW_CODE ");
                obj_str.Append(", POSITION_LEVEL ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKFLOW_TYPE ");
                obj_str.Append(", @WORKFLOW_CODE ");
                obj_str.Append(", @POSITION_LEVEL ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");


                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                //-- Step 1 delete data old
                string posilevel = "";
                foreach (cls_TRLineapprove model in list_model)
                {
                    posilevel += "'" + model.position_level + "',";
                }
                if (posilevel.Length > 0)
                    posilevel = posilevel.Substring(0, posilevel.Length - 1);

                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM SELF_TR_LINEAPPROVE");
                obj_str2.Append(" WHERE 1=1 ");
                obj_str2.Append(" AND COMPANY_CODE='" + list_model[0].company_code + "'");
                obj_str2.Append(" AND WORKFLOW_TYPE='" + list_model[0].workflow_type + "' ");
                obj_str2.Append(" AND POSITION_LEVEL IN (" + posilevel + ")");
                //obj_str2.Append(" AND WORKFLOW_CODE='" + list_model[0].workflow_code + "' ");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());
                //int id = 1;
                //try
                //{
                //    System.Text.StringBuilder obj_str3 = new System.Text.StringBuilder();

                //    obj_str3.Append("SELECT MAX(LINEAPPROVE_ID) ");
                //    obj_str3.Append(" FROM SELF_TR_LINEAPPROVE");

                //    DataTable dt = obj_conn.doGetTableTransaction(obj_str3.ToString());
                //    if (dt.Rows.Count > 0)
                //    {
                //        id = Convert.ToInt32(dt.Rows[0][0]) + 1;
                //    }
                //}
                //catch
                //{

                //}
                if (blnResult)
                {
                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@WORKFLOW_TYPE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@WORKFLOW_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@POSITION_LEVEL", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime);
                    obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit);
                    foreach (cls_TRLineapprove model in list_model)
                    {

                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                        obj_cmd.Parameters["@WORKFLOW_TYPE"].Value = model.workflow_type;
                        obj_cmd.Parameters["@WORKFLOW_CODE"].Value = model.workflow_code;
                        obj_cmd.Parameters["@POSITION_LEVEL"].Value = model.position_level;
                        obj_cmd.Parameters["@CREATED_BY"].Value = username;
                        obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                        obj_cmd.Parameters["@FLAG"].Value = model.flag;
                        obj_cmd.ExecuteNonQuery();

                    }

                    blnResult = obj_conn.doCommit();

                    if (!blnResult)
                    {
                        obj_conn.doRollback();
                    }
                }
                else
                {
                    obj_conn.doRollback();
                }
                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Lineapprove.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_TRLineapprove model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SELF_TR_LINEAPPROVE SET ");

                obj_str.Append(" WORKFLOW_TYPE=@WORKFLOW_TYPE ");
                obj_str.Append(", WORKFLOW_CODE=@WORKFLOW_CODE ");
                obj_str.Append(", POSITION_LEVEL=@POSITION_LEVEL ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");


                obj_str.Append(" WHERE WORKFLOW_CODE=@WORKFLOW_CODE ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKFLOW_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKFLOW_TYPE"].Value = model.workflow_type;
                obj_cmd.Parameters.Add("@WORKFLOW_CODE", SqlDbType.Int); obj_cmd.Parameters["@WORKFLOW_CODE"].Value = model.workflow_code;
                obj_cmd.Parameters.Add("@POSITION_LEVEL", SqlDbType.VarChar); obj_cmd.Parameters["@POSITION_LEVEL"].Value = model.position_level;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.workflow_code;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Lineapprove.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
