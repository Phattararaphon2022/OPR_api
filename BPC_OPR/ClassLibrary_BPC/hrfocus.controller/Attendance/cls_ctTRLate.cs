using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;
namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRLate
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRLate() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRLate> getData(string condition)
        {
            List<cls_TRLate> list_model = new List<cls_TRLate>();
            cls_TRLate model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", LATE_CODE");
                obj_str.Append(", LATE_FROM");
                obj_str.Append(", LATE_TO");
                obj_str.Append(", LATE_DEDUCT_TYPE");
                obj_str.Append(", LATE_DEDUCT_AMOUNT");

                obj_str.Append(" FROM ATT_TR_LATE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, LATE_CODE, LATE_FROM");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRLate();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                    model.late_code = Convert.ToString(dr["LATE_CODE"]);
                    model.late_from = Convert.ToInt32(dr["LATE_FROM"]);
                    model.late_to = Convert.ToInt32(dr["LATE_TO"]);
                    model.late_deduct_type = Convert.ToString(dr["LATE_DEDUCT_TYPE"]);
                    model.late_deduct_amount = Convert.ToDouble(dr["LATE_DEDUCT_AMOUNT"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Late.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRLate> getDataByFillter(string com, string code)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!code.Equals(""))
                strCondition += " AND LATE_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string code, int latefrom)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT LATE_ID");
                obj_str.Append(" FROM ATT_TR_LATE");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND LATE_CODE='" + code + "'");
                obj_str.Append(" AND LATE_FROM='" + latefrom + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Late.checkDataOld)" + ex.ToString();
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

                obj_str.Append(" DELETE FROM ATT_TR_LATE");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE ='" + com + "'");
                obj_str.Append(" AND LATE_CODE ='" + code + "'");

                blnResult = Obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Late.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(List<cls_TRLate> list_model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                if (!this.delete(list_model[0].company_code, list_model[0].late_code))
                {
                    return false;
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO ATT_TR_LATE");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", LATE_CODE ");
                obj_str.Append(", LATE_FROM ");
                obj_str.Append(", LATE_TO ");
                obj_str.Append(", LATE_DEDUCT_TYPE ");
                obj_str.Append(", LATE_DEDUCT_AMOUNT ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @LATE_CODE ");
                obj_str.Append(", @LATE_FROM ");
                obj_str.Append(", @LATE_TO ");
                obj_str.Append(", @LATE_DEDUCT_TYPE ");
                obj_str.Append(", @LATE_DEDUCT_AMOUNT ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                obj_cmd.Transaction = obj_conn.getTransaction();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                obj_cmd.Parameters.Add("@LATE_CODE", SqlDbType.VarChar);
                obj_cmd.Parameters.Add("@LATE_FROM", SqlDbType.Int);
                obj_cmd.Parameters.Add("@LATE_TO", SqlDbType.Int);
                obj_cmd.Parameters.Add("@LATE_DEDUCT_TYPE", SqlDbType.VarChar);
                obj_cmd.Parameters.Add("@LATE_DEDUCT_AMOUNT", SqlDbType.Decimal);


                foreach (cls_TRLate model in list_model)
                {

                    obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                    obj_cmd.Parameters["@LATE_CODE"].Value = model.late_code;
                    obj_cmd.Parameters["@LATE_FROM"].Value = model.late_from;
                    obj_cmd.Parameters["@LATE_TO"].Value = model.late_to;
                    obj_cmd.Parameters["@LATE_DEDUCT_TYPE"].Value = model.late_deduct_type;
                    obj_cmd.Parameters["@LATE_DEDUCT_AMOUNT"].Value = model.late_deduct_amount;

                    obj_cmd.ExecuteNonQuery();

                }

                blnResult = obj_conn.doCommit();
                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Late.insert)" + ex.ToString();
            }

            return blnResult;
        }

    }
}
