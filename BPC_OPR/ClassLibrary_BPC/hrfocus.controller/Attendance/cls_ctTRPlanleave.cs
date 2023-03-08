using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;


namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRPlanleave
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRPlanleave() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRPlanleave> getData(string condition)
        {
            List<cls_TRPlanleave> list_model = new List<cls_TRPlanleave>();
            cls_TRPlanleave model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", PLANLEAVE_CODE");
                obj_str.Append(", LEAVE_CODE");

                obj_str.Append(" FROM ATT_TR_PLANLEAVE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, PLANLEAVE_CODE, LEAVE_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRPlanleave();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.planleave_code = dr["PLANLEAVE_CODE"].ToString();
                    model.leave_code = dr["LEAVE_CODE"].ToString();

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRPlanleave.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRPlanleave> getDataByFillter(string com, string plan)
        {
            string strCondition = " AND COMPANY_CODE='" + com + "'";

            if (!plan.Equals(""))
                strCondition += " AND PLANLEAVE_CODE='" + plan + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string code, string leave)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM ATT_TR_PLANLEAVE");
                obj_str.Append(" WHERE AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PLANLEAVE_CODE='" + code + "'");
                obj_str.Append(" AND LEAVE_CODE='" + leave + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRPlanleave.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }


        public bool delete(string com, string code, string leave)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_PLANLEAVE");
                obj_str.Append(" WHERE AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PLANLEAVE_CODE='" + code + "'");
                obj_str.Append(" AND LEAVE_CODE='" + leave + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(TRPlanleave.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string code)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_PLANLEAVE");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PLANLEAVE_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(TRPlanleave.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(List<cls_TRPlanleave> list_model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                //if (!this.delete(list_model[0].company_code, list_model[0].planleave_code))
                //{
                //    return false;
                //}

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO ATT_TR_PLANLEAVE");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", PLANLEAVE_CODE ");
                obj_str.Append(", LEAVE_CODE ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @PLANLEAVE_CODE ");
                obj_str.Append(", @LEAVE_CODE ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                obj_cmd.Transaction = obj_conn.getTransaction();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                obj_cmd.Parameters.Add("@PLANLEAVE_CODE", SqlDbType.VarChar);
                obj_cmd.Parameters.Add("@LEAVE_CODE", SqlDbType.VarChar);


                foreach (cls_TRPlanleave model in list_model)
                {

                    obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                    obj_cmd.Parameters["@PLANLEAVE_CODE"].Value = model.planleave_code;
                    obj_cmd.Parameters["@LEAVE_CODE"].Value = model.leave_code;

                    obj_cmd.ExecuteNonQuery();

                }

                blnResult = obj_conn.doCommit();
                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRPlanleave.insert)" + ex.ToString();
            }

            return blnResult;
        }

    }
}
