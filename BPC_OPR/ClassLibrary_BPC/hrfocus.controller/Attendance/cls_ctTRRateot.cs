using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRRateot
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRRateot() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRRateot> getData(string condition)
        {
            List<cls_TRRateot> list_model = new List<cls_TRRateot>();
            cls_TRRateot model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", RATEOT_CODE");
                obj_str.Append(", RATEOT_DAYTYPE");
                obj_str.Append(", ISNULL(RATEOT_BEFORE, 0) AS RATEOT_BEFORE");
                obj_str.Append(", ISNULL(RATEOT_NORMAL, 0) AS RATEOT_NORMAL");
                obj_str.Append(", ISNULL(RATEOT_BREAK, 0) AS RATEOT_BREAK");
                obj_str.Append(", ISNULL(RATEOT_AFTER, 0) AS RATEOT_AFTER");

                obj_str.Append(" FROM ATT_TR_RATEOT");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, RATEOT_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRRateot();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.rateot_code = dr["RATEOT_CODE"].ToString();
                    model.rateot_daytype = dr["RATEOT_DAYTYPE"].ToString();
                    model.rateot_before = Convert.ToDouble(dr["RATEOT_BEFORE"]);
                    model.rateot_normal = Convert.ToDouble(dr["RATEOT_NORMAL"]);
                    model.rateot_break = Convert.ToDouble(dr["RATEOT_BREAK"]);
                    model.rateot_after = Convert.ToDouble(dr["RATEOT_AFTER"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRRateot.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRRateot> getDataByFillter(string com, string code)
        {
            string strCondition = " AND COMPANY_CODE='" + com + "'";

            if (!code.Equals(""))
                strCondition += " AND RATEOT_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string code, string daytype)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM ATT_TR_RATEOT");
                obj_str.Append(" WHERE AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND RATEOT_CODE='" + code + "'");
                obj_str.Append(" AND RATEOT_DAYTYPE='" + daytype + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRRateot.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }


        public bool delete(string com, string code, string daytype)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_RATEOT");
                obj_str.Append(" WHERE AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND RATEOT_CODE='" + code + "'");
                obj_str.Append(" AND RATEOT_DAYTYPE='" + daytype + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(TRRateot.delete)" + ex.ToString();
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

                obj_str.Append(" DELETE FROM ATT_TR_RATEOT");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND RATEOT_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(TRRateot.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(List<cls_TRRateot> list_model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                //if (!this.delete(list_model[0].company_code, list_model[0].rateot_code))
                //{
                //    return false;
                //}

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO ATT_TR_RATEOT");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", RATEOT_CODE ");
                obj_str.Append(", RATEOT_DAYTYPE ");
                obj_str.Append(", RATEOT_BEFORE ");
                obj_str.Append(", RATEOT_NORMAL ");
                obj_str.Append(", RATEOT_BREAK ");
                obj_str.Append(", RATEOT_AFTER ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @RATEOT_CODE ");
                obj_str.Append(", @RATEOT_DAYTYPE ");
                obj_str.Append(", @RATEOT_BEFORE ");
                obj_str.Append(", @RATEOT_NORMAL ");
                obj_str.Append(", @RATEOT_BREAK ");
                obj_str.Append(", @RATEOT_AFTER ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                obj_cmd.Transaction = obj_conn.getTransaction();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                obj_cmd.Parameters.Add("@RATEOT_CODE", SqlDbType.VarChar);
                obj_cmd.Parameters.Add("@RATEOT_DAYTYPE", SqlDbType.VarChar);
                obj_cmd.Parameters.Add("@RATEOT_BEFORE", SqlDbType.Decimal);
                obj_cmd.Parameters.Add("@RATEOT_NORMAL", SqlDbType.Decimal);
                obj_cmd.Parameters.Add("@RATEOT_BREAK", SqlDbType.Decimal);
                obj_cmd.Parameters.Add("@RATEOT_AFTER", SqlDbType.Decimal);

                foreach (cls_TRRateot model in list_model)
                {

                    obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                    obj_cmd.Parameters["@RATEOT_CODE"].Value = model.rateot_code;
                    obj_cmd.Parameters["@RATEOT_DAYTYPE"].Value = model.rateot_daytype;
                    obj_cmd.Parameters["@RATEOT_BEFORE"].Value = model.rateot_before;
                    obj_cmd.Parameters["@RATEOT_NORMAL"].Value = model.rateot_normal;
                    obj_cmd.Parameters["@RATEOT_BREAK"].Value = model.rateot_break;
                    obj_cmd.Parameters["@RATEOT_AFTER"].Value = model.rateot_after;

                    obj_cmd.ExecuteNonQuery();

                }

                blnResult = obj_conn.doCommit();
                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRRateot.insert)" + ex.ToString();
            }

            return blnResult;
        }

    }
}
