using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;
namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRShiftallowance
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRShiftallowance() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRShiftallowance> getData(string condition)
        {
            List<cls_TRShiftallowance> list_model = new List<cls_TRShiftallowance>();
            cls_TRShiftallowance model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", SHIFT_CODE");
                obj_str.Append(", SHIFTALLOWANCE_NO");
                obj_str.Append(", ISNULL(SHIFTALLOWANCE_NAME_TH, '') AS SHIFTALLOWANCE_NAME_TH");
                obj_str.Append(", ISNULL(SHIFTALLOWANCE_NAME_EN, '') AS SHIFTALLOWANCE_NAME_EN");
                obj_str.Append(", ISNULL(SHIFTALLOWANCE_HHMM, '00:00') AS SHIFTALLOWANCE_HHMM");
                obj_str.Append(", ISNULL(SHIFTALLOWANCE_AMOUNT, 0) AS SHIFTALLOWANCE_AMOUNT");

                obj_str.Append(" FROM ATT_TR_SHIFTALLOWANCE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, SHIFT_CODE, SHIFTALLOWANCE_NO");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRShiftallowance();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.shift_code = dr["SHIFT_CODE"].ToString();
                    model.shiftallowance_no = Convert.ToInt32(dr["SHIFTALLOWANCE_NO"]);
                    model.shiftallowance_name_th = dr["SHIFTALLOWANCE_NAME_TH"].ToString();
                    model.shiftallowance_name_en = dr["SHIFTALLOWANCE_NAME_EN"].ToString();
                    model.shiftallowance_hhmm = dr["SHIFTALLOWANCE_HHMM"].ToString();
                    model.shiftallowance_amount = Convert.ToDouble(dr["SHIFTALLOWANCE_AMOUNT"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRShiftallowance.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRShiftallowance> getDataByFillter(string com, string code)
        {
            string strCondition = " AND COMPANY_CODE='" + com + "'";

            if (!code.Equals(""))
                strCondition += " AND SHIFT_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string code, int no)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM ATT_TR_SHIFTALLOWANCE");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND SHIFT_CODE='" + code + "'");
                obj_str.Append(" AND SHIFTALLOWANCE_NO='" + no + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRShiftallowance.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }


        public bool delete(string com, string code, int no)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_SHIFTALLOWANCE");
                obj_str.Append(" WHERE AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND SHIFT_CODE='" + code + "'");
                obj_str.Append(" AND SHIFTALLOWANCE_NO='" + no + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(TRShiftallowance.delete)" + ex.ToString();
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

                obj_str.Append(" DELETE FROM ATT_TR_SHIFTALLOWANCE");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND SHIFT_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(TRShiftallowance.delete)" + ex.ToString();
            }

            return blnResult;
        }
        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(SHIFTALLOWANCE_NO) ");
                obj_str.Append(" FROM ATT_TR_SHIFTALLOWANCE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(ATT_TR_SHIFTBREAK.getNextID)" + ex.ToString();
            }

            return intResult;
        }
        public bool insert(string com, string shift, List<cls_TRShiftallowance> list_model)
        {
            bool blnResult = false;
            try
            {

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO ATT_TR_SHIFTALLOWANCE");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", SHIFT_CODE ");
                obj_str.Append(", SHIFTALLOWANCE_NO ");
                obj_str.Append(", SHIFTALLOWANCE_NAME_TH ");
                obj_str.Append(", SHIFTALLOWANCE_NAME_EN ");
                obj_str.Append(", SHIFTALLOWANCE_HHMM ");
                obj_str.Append(", SHIFTALLOWANCE_AMOUNT ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @SHIFT_CODE ");
                obj_str.Append(", @SHIFTALLOWANCE_NO ");
                obj_str.Append(", @SHIFTALLOWANCE_NAME_TH ");
                obj_str.Append(", @SHIFTALLOWANCE_NAME_EN ");
                obj_str.Append(", @SHIFTALLOWANCE_HHMM ");
                obj_str.Append(", @SHIFTALLOWANCE_AMOUNT ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM ATT_TR_SHIFTALLOWANCE");
                obj_str2.Append(" WHERE 1=1 ");
                obj_str2.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str2.Append(" AND SHIFT_CODE='" + shift + "'");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());

                if (blnResult)
                {

                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@SHIFT_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@SHIFTALLOWANCE_NO", SqlDbType.Int);
                    obj_cmd.Parameters.Add("@SHIFTALLOWANCE_NAME_TH", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@SHIFTALLOWANCE_NAME_EN", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@SHIFTALLOWANCE_HHMM", SqlDbType.Char);
                    obj_cmd.Parameters.Add("@SHIFTALLOWANCE_AMOUNT", SqlDbType.Decimal);
                    int id = 1;
                    foreach (cls_TRShiftallowance model in list_model)
                    {

                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                        obj_cmd.Parameters["@SHIFT_CODE"].Value = shift;
                        obj_cmd.Parameters["@SHIFTALLOWANCE_NO"].Value = id;
                        obj_cmd.Parameters["@SHIFTALLOWANCE_NAME_TH"].Value = model.shiftallowance_name_th;
                        obj_cmd.Parameters["@SHIFTALLOWANCE_NAME_EN"].Value = model.shiftallowance_name_en;
                        obj_cmd.Parameters["@SHIFTALLOWANCE_HHMM"].Value = model.shiftallowance_hhmm;
                        obj_cmd.Parameters["@SHIFTALLOWANCE_AMOUNT"].Value = model.shiftallowance_amount;
                        id++;
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
                Message = "ERROR::(TRShiftallowance.insert)" + ex.ToString();
            }

            return blnResult;
        }

    }
}
