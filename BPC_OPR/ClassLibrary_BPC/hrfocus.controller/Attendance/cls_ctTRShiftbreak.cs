using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;
namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRShiftbreak
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRShiftbreak() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRShiftbreak> getData(string condition)
        {
            List<cls_TRShiftbreak> list_model = new List<cls_TRShiftbreak>();
            cls_TRShiftbreak model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", SHIFT_CODE");
                obj_str.Append(", SHIFTBREAK_NO");
                obj_str.Append(", SHIFTBREAK_FROM");
                obj_str.Append(", SHIFTBREAK_TO");
                obj_str.Append(", SHIFTBREAK_BREAK");
                obj_str.Append(" FROM ATT_TR_SHIFTBREAK");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, SHIFT_CODE, SHIFTBREAK_NO");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRShiftbreak();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);

                    model.shift_code = Convert.ToString(dr["SHIFT_CODE"]);
                    model.shiftbreak_no = Convert.ToInt32(dr["SHIFTBREAK_NO"]);
                    model.shiftbreak_from = Convert.ToString(dr["SHIFTBREAK_FROM"]);
                    model.shiftbreak_to = Convert.ToString(dr["SHIFTBREAK_TO"]);
                    model.shiftbreak_break = Convert.ToInt32(dr["SHIFTBREAK_BREAK"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Shiftbreak.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRShiftbreak> getDataByFillter(string com, string shift)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!shift.Equals(""))
                strCondition += " AND SHIFT_CODE='" + shift + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string shift, string no)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM ATT_TR_SHIFTBREAK");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND SHIFT_CODE='" + shift + "' ");
                obj_str.Append(" AND SHIFTBREAK_NO='" + no + "' ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Shiftbreak.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string shift, string no)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_SHIFTBREAK");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND SHIFT_CODE='" + shift + "' ");
                obj_str.Append(" AND SHIFTBREAK_NO='" + no + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());


            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Shiftbreak.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string shift)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_SHIFTBREAK");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND SHIFT_CODE='" + shift + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());


            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Shiftbreak.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRShiftbreak model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                if (!this.delete(model.company_code, model.shift_code, model.shiftbreak_no.ToString()))
                {
                    return false;
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO ATT_TR_SHIFTBREAK");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", SHIFT_CODE ");
                obj_str.Append(", SHIFTBREAK_NO ");
                obj_str.Append(", SHIFTBREAK_FROM ");
                obj_str.Append(", SHIFTBREAK_TO ");
                obj_str.Append(", SHIFTBREAK_BREAK ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @SHIFT_CODE ");
                obj_str.Append(", @SHIFTBREAK_NO ");
                obj_str.Append(", @SHIFTBREAK_FROM ");
                obj_str.Append(", @SHIFTBREAK_TO ");
                obj_str.Append(", @SHIFTBREAK_BREAK ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@SHIFT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CODE"].Value = model.shift_code;
                obj_cmd.Parameters.Add("@SHIFTBREAK_NO", SqlDbType.Int); obj_cmd.Parameters["@SHIFTBREAK_NO"].Value = model.shiftbreak_no;
                obj_cmd.Parameters.Add("@SHIFTBREAK_FROM", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFTBREAK_FROM"].Value = model.shiftbreak_from;
                obj_cmd.Parameters.Add("@SHIFTBREAK_TO", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFTBREAK_TO"].Value = model.shiftbreak_to;
                obj_cmd.Parameters.Add("@SHIFTBREAK_BREAK", SqlDbType.Int); obj_cmd.Parameters["@SHIFTBREAK_BREAK"].Value = model.shiftbreak_break;


                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Shiftbreak.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(SHIFTBREAK_NO) ");
                obj_str.Append(" FROM ATT_TR_SHIFTBREAK");

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
        public bool insert(string com, string shift, List<cls_TRShiftbreak> list_model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO ATT_TR_SHIFTBREAK");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", SHIFT_CODE ");
                obj_str.Append(", SHIFTBREAK_NO ");
                obj_str.Append(", SHIFTBREAK_FROM ");
                obj_str.Append(", SHIFTBREAK_TO ");
                obj_str.Append(", SHIFTBREAK_BREAK ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @SHIFT_CODE ");
                obj_str.Append(", @SHIFTBREAK_NO ");
                obj_str.Append(", @SHIFTBREAK_FROM ");
                obj_str.Append(", @SHIFTBREAK_TO ");
                obj_str.Append(", @SHIFTBREAK_BREAK ");
                obj_str.Append(" )");


                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM ATT_TR_SHIFTBREAK");
                //obj_str2.Append(" WHERE 1=1 ");
                obj_str2.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str2.Append(" AND SHIFT_CODE='" + shift + "'");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());

                if (blnResult)
                {
                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@SHIFT_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@SHIFTBREAK_NO", SqlDbType.Int);
                    obj_cmd.Parameters.Add("@SHIFTBREAK_FROM", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@SHIFTBREAK_TO", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@SHIFTBREAK_BREAK", SqlDbType.Int);
                    int id = 1;
                    foreach (cls_TRShiftbreak model in list_model)
                    {

                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                        obj_cmd.Parameters["@SHIFT_CODE"].Value = shift;
                        obj_cmd.Parameters["@SHIFTBREAK_NO"].Value = id;
                        obj_cmd.Parameters["@SHIFTBREAK_FROM"].Value = model.shiftbreak_from;
                        obj_cmd.Parameters["@SHIFTBREAK_TO"].Value = model.shiftbreak_to;
                        obj_cmd.Parameters["@SHIFTBREAK_BREAK"].Value = model.shiftbreak_break;
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
                Message = "ERROR::(Shiftbreak.insert)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
