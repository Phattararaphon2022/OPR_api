using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRTimeallw
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRTimeallw() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRTimeallw> getData(string condition)
        {
            List<cls_TRTimeallw> list_model = new List<cls_TRTimeallw>();
            cls_TRTimeallw model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append(" TIMEALLW_NO");

                obj_str.Append(", TIMEALLW_TYPE");
                obj_str.Append(", TIMEALLW_METHOD");

                obj_str.Append(", TIMEALLW_TIME");
                obj_str.Append(", ISNULL(TIMEALLW_TIMEIN, '00:00') AS TIMEALLW_TIMEIN");
                obj_str.Append(", ISNULL(TIMEALLW_TIMEOUT, '00:00') AS TIMEALLW_TIMEOUT");

                obj_str.Append(", ISNULL(TIMEALLW_NORMALDAY, 0) AS TIMEALLW_NORMALDAY");
                obj_str.Append(", ISNULL(TIMEALLW_OFFDAY, 0) AS TIMEALLW_OFFDAY");
                obj_str.Append(", ISNULL(TIMEALLW_COMPANYDAY, 0) AS TIMEALLW_COMPANYDAY");
                obj_str.Append(", ISNULL(TIMEALLW_HOLIDAY, 0) AS TIMEALLW_HOLIDAY");
                obj_str.Append(", ISNULL(TIMEALLW_LEAVEDAY, 0) AS TIMEALLW_LEAVEDAY");

                obj_str.Append(", PLANTIMEALLW_CODE");
                obj_str.Append(", COMPANY_CODE");

                obj_str.Append(" FROM ATT_TR_TIMEALLW");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, PLANTIMEALLW_CODE, TIMEALLW_TIME");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRTimeallw();

                    model.timeallw_no = Convert.ToInt32(dr["TIMEALLW_NO"]);

                    model.timeallw_type = dr["TIMEALLW_TYPE"].ToString();
                    model.timeallw_method = dr["TIMEALLW_METHOD"].ToString();

                    model.timeallw_time = Convert.ToInt32(dr["TIMEALLW_TIME"]);
                    model.timeallw_timein = dr["TIMEALLW_TIMEIN"].ToString();
                    model.timeallw_timeout = dr["TIMEALLW_TIMEOUT"].ToString();

                    model.timeallw_normalday = Convert.ToDouble(dr["TIMEALLW_NORMALDAY"]);
                    model.timeallw_offday = Convert.ToDouble(dr["TIMEALLW_OFFDAY"]);
                    model.timeallw_companyday = Convert.ToDouble(dr["TIMEALLW_COMPANYDAY"]);
                    model.timeallw_holiday = Convert.ToDouble(dr["TIMEALLW_HOLIDAY"]);
                    model.timeallw_leaveday = Convert.ToDouble(dr["TIMEALLW_LEAVEDAY"]);

                    model.plantimeallw_code = dr["PLANTIMEALLW_CODE"].ToString();
                    model.company_code = dr["COMPANY_CODE"].ToString();

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Timeallw.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRTimeallw> getDataByFillter(string com, string plan)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!plan.Equals(""))
                strCondition += " AND PLANTIMEALLW_CODE='" + plan + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string plan, string no)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT HOLIDAY_ID");
                obj_str.Append(" FROM ATT_TR_TIMEALLW");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                obj_str.Append(" AND PLANTIMEALLW_CODE='" + plan + "'");
                obj_str.Append(" AND TIMEALLW_NO='" + no + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Timeallw.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }


        public bool delete(string com, string plan, string no)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_TIMEALLW");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                obj_str.Append(" AND PLANTIMEALLW_CODE='" + plan + "'");
                obj_str.Append(" AND TIMEALLW_NO='" + no + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Timeallw.delete)" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(string com, string plan)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_TIMEALLW");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                obj_str.Append(" AND PLANTIMEALLW_CODE='" + plan + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Timeallw.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(string com, string plan, List<cls_TRTimeallw> list_model)
        {
            bool blnResult = false;
            cls_ctConnection obj_conn = new cls_ctConnection();
            try
            {

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                //-- Step 1 delete data old
                obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_TIMEALLW");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                obj_str.Append(" AND PLANTIMEALLW_CODE='" + plan + "'");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str.ToString());

                //-- Step 2 insert
                if (blnResult)
                {
                    obj_str = new System.Text.StringBuilder();
                    obj_str.Append("INSERT INTO ATT_TR_TIMEALLW");
                    obj_str.Append(" (");
                    obj_str.Append("TIMEALLW_NO ");
                    obj_str.Append(", TIMEALLW_TIME ");
                    obj_str.Append(", TIMEALLW_TYPE ");
                    obj_str.Append(", TIMEALLW_METHOD ");

                    obj_str.Append(", TIMEALLW_TIMEIN ");
                    obj_str.Append(", TIMEALLW_TIMEOUT ");

                    obj_str.Append(", TIMEALLW_NORMALDAY ");
                    obj_str.Append(", TIMEALLW_OFFDAY ");
                    obj_str.Append(", TIMEALLW_COMPANYDAY ");
                    obj_str.Append(", TIMEALLW_HOLIDAY ");
                    obj_str.Append(", TIMEALLW_LEAVEDAY ");

                    obj_str.Append(", PLANTIMEALLW_CODE ");
                    obj_str.Append(", COMPANY_CODE ");

                    obj_str.Append(" )");

                    obj_str.Append(" VALUES(");
                    obj_str.Append("@TIMEALLW_NO ");
                    obj_str.Append(", @TIMEALLW_TIME ");
                    obj_str.Append(", @TIMEALLW_TYPE ");
                    obj_str.Append(", @TIMEALLW_METHOD ");

                    obj_str.Append(", @TIMEALLW_TIMEIN ");
                    obj_str.Append(", @TIMEALLW_TIMEOUT ");

                    obj_str.Append(", @TIMEALLW_NORMALDAY ");
                    obj_str.Append(", @TIMEALLW_OFFDAY ");
                    obj_str.Append(", @TIMEALLW_COMPANYDAY ");
                    obj_str.Append(", @TIMEALLW_HOLIDAY ");
                    obj_str.Append(", @TIMEALLW_LEAVEDAY ");

                    obj_str.Append(", @PLANTIMEALLW_CODE ");
                    obj_str.Append(", @COMPANY_CODE ");

                    obj_str.Append(" )");

                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@TIMEALLW_NO", SqlDbType.Int);
                    obj_cmd.Parameters.Add("@TIMEALLW_TIME", SqlDbType.Int);
                    obj_cmd.Parameters.Add("@TIMEALLW_TYPE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@TIMEALLW_METHOD", SqlDbType.VarChar);

                    obj_cmd.Parameters.Add("@TIMEALLW_TIMEIN", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@TIMEALLW_TIMEOUT", SqlDbType.VarChar);

                    obj_cmd.Parameters.Add("@TIMEALLW_NORMALDAY", SqlDbType.Decimal);
                    obj_cmd.Parameters.Add("@TIMEALLW_OFFDAY", SqlDbType.Decimal);
                    obj_cmd.Parameters.Add("@TIMEALLW_COMPANYDAY", SqlDbType.Decimal);
                    obj_cmd.Parameters.Add("@TIMEALLW_HOLIDAY", SqlDbType.Decimal);
                    obj_cmd.Parameters.Add("@TIMEALLW_LEAVEDAY", SqlDbType.Decimal);

                    obj_cmd.Parameters.Add("@PLANTIMEALLW_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);

                    foreach (cls_TRTimeallw model in list_model)
                    {

                        obj_cmd.Parameters["@TIMEALLW_NO"].Value = model.timeallw_no;
                        obj_cmd.Parameters["@TIMEALLW_TIME"].Value = model.timeallw_time;
                        obj_cmd.Parameters["@TIMEALLW_TYPE"].Value = model.timeallw_type;
                        obj_cmd.Parameters["@TIMEALLW_METHOD"].Value = model.timeallw_method;

                        obj_cmd.Parameters["@TIMEALLW_TIMEIN"].Value = model.timeallw_timein;
                        obj_cmd.Parameters["@TIMEALLW_TIMEOUT"].Value = model.timeallw_timeout;

                        obj_cmd.Parameters["@TIMEALLW_NORMALDAY"].Value = model.timeallw_normalday;
                        obj_cmd.Parameters["@TIMEALLW_OFFDAY"].Value = model.timeallw_offday;
                        obj_cmd.Parameters["@TIMEALLW_COMPANYDAY"].Value = model.timeallw_companyday;
                        obj_cmd.Parameters["@TIMEALLW_HOLIDAY"].Value = model.timeallw_holiday;
                        obj_cmd.Parameters["@TIMEALLW_LEAVEDAY"].Value = model.timeallw_leaveday;

                        obj_cmd.Parameters["@PLANTIMEALLW_CODE"].Value = model.plantimeallw_code;
                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

                        obj_cmd.ExecuteNonQuery();

                    }

                    blnResult = obj_conn.doCommit();

                }
                else
                {
                    obj_conn.doRollback();
                }

            }


            catch (Exception ex)
            {
                Message = "ERROR::(Timeallw.insert)" + ex.ToString();
                obj_conn.doRollback();
            }
            finally
            {
                obj_conn.doClose();
            }

            return blnResult;
        }

    }
}
