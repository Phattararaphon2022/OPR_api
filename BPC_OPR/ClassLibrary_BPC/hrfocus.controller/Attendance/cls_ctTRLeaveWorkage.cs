using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;
namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRLeaveWorkage
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRLeaveWorkage() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRLeaveWorkage> getData(string condition)
        {
            List<cls_TRLeaveWorkage> list_model = new List<cls_TRLeaveWorkage>();
            cls_TRLeaveWorkage model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", LEAVE_CODE");
                obj_str.Append(", WORKAGE_FROM");
                obj_str.Append(", WORKAGE_TO");
                obj_str.Append(", WORKAGE_LEAVEDAY");

                obj_str.Append(" FROM ATT_TR_LEAVE_WORKAGE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, LEAVE_CODE, WORKAGE_FROM");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRLeaveWorkage();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                    model.leave_code = Convert.ToString(dr["LEAVE_CODE"]);
                    model.workage_from = Convert.ToDouble(dr["WORKAGE_FROM"]);
                    model.workage_to = Convert.ToDouble(dr["WORKAGE_TO"]);
                    model.workage_leaveday = Convert.ToDouble(dr["WORKAGE_LEAVEDAY"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(LeaveWorkage.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRLeaveWorkage> getDataByFillter(string com, string code)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!code.Equals(""))
                strCondition += " AND LEAVE_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string code, double workagefrom)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM ATT_TR_LEAVE_WORKAGE");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND LEAVE_CODE='" + code + "'");
                obj_str.Append(" AND WORKAGE_FROM='" + workagefrom + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(LeaveWorkage.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string code)
        {
            bool blnResult = true;
            try
            {
                //cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_LEAVE_WORKAGE");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE ='" + com + "'");
                obj_str.Append(" AND LEAVE_CODE ='" + code + "'");

                blnResult = Obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(LeaveWorkage.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(List<cls_TRLeaveWorkage> list_model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                if (!this.delete(list_model[0].company_code, list_model[0].leave_code))
                {
                    return false;
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO ATT_TR_LEAVE_WORKAGE");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", LEAVE_CODE ");
                obj_str.Append(", WORKAGE_FROM ");
                obj_str.Append(", WORKAGE_TO ");
                obj_str.Append(", WORKAGE_LEAVEDAY ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @LEAVE_CODE ");
                obj_str.Append(", @WORKAGE_FROM ");
                obj_str.Append(", @WORKAGE_TO ");
                obj_str.Append(", @WORKAGE_LEAVEDAY ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                obj_cmd.Transaction = obj_conn.getTransaction();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                obj_cmd.Parameters.Add("@LEAVE_CODE", SqlDbType.VarChar);
                obj_cmd.Parameters.Add("@WORKAGE_FROM", SqlDbType.Decimal);
                obj_cmd.Parameters.Add("@WORKAGE_TO", SqlDbType.Decimal);
                obj_cmd.Parameters.Add("@WORKAGE_LEAVEDAY", SqlDbType.Decimal);

                foreach (cls_TRLeaveWorkage model in list_model)
                {

                    obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                    obj_cmd.Parameters["@LEAVE_CODE"].Value = model.leave_code;
                    obj_cmd.Parameters["@WORKAGE_FROM"].Value = model.workage_from;
                    obj_cmd.Parameters["@WORKAGE_TO"].Value = model.workage_to;
                    obj_cmd.Parameters["@WORKAGE_LEAVEDAY"].Value = model.workage_leaveday;

                    obj_cmd.ExecuteNonQuery();

                }

                blnResult = obj_conn.doCommit();
                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(LeaveWorkage.insert)" + ex.ToString();
            }

            return blnResult;
        }

    }
}
