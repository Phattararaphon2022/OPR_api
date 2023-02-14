using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRHoliday
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRHoliday() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRHoliday> getData(string condition)
        {
            List<cls_TRHoliday> list_model = new List<cls_TRHoliday>();
            cls_TRHoliday model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append(" HOLIDAY_DATE");
                obj_str.Append(", ISNULL(HOLIDAY_NAME_TH, '') AS HOLIDAY_NAME_TH");
                obj_str.Append(", ISNULL(HOLIDAY_NAME_EN, '') AS HOLIDAY_NAME_EN");

                obj_str.Append(", PLANHOLIDAY_CODE");
                obj_str.Append(", COMPANY_CODE");

                obj_str.Append(", ISNULL(HOLIDAY_DAYTYPE, 'H') AS HOLIDAY_DAYTYPE");
                obj_str.Append(", ISNULL(HOLIDAY_PAYPER, 100) AS HOLIDAY_PAYPER");
             
                obj_str.Append(" FROM ATT_TR_HOLIDAY");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PLANHOLIDAY_CODE, HOLIDAY_DATE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRHoliday();
                 
                    model.holiday_date =    dr["HOLIDAY_DATE"].ToString();
                    model.holiday_name_th = dr["HOLIDAY_NAME_TH"].ToString();
                    model.holiday_name_en = dr["HOLIDAY_NAME_EN"].ToString();

                    model.planholiday_code = dr["PLANHOLIDAY_CODE"].ToString();
                    model.company_code = dr["COMPANY_CODE"].ToString();

                    model.holiday_daytype = dr["HOLIDAY_DAYTYPE"].ToString();
                    model.holiday_payper = Convert.ToDouble(dr["HOLIDAY_PAYPER"]);           

                                                                                                                      
                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "ERROR::(Holiday.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRHoliday> getDataByFillter(string com, string plan)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!plan.Equals(""))
                strCondition += " AND PLANHOLIDAY_CODE='" + plan + "'";
            
            return this.getData(strCondition);
        }

        public List<cls_TRHoliday> getDataByWorker(string com, string worker)
        {
            string strCondition = "";

            strCondition += " AND ATT_TR_HOLIDAY.COMPANY_CODE='" + com + "'";
            strCondition += " AND PLANHOLIDAY_CODE IN (SELECT EMPPOLATT_POLICY_CODE FROM ATT_TR_EMPPOLATT WHERE COMPANY_CODE='" + com + "' AND EMPPOLATT_POLICY_TYPE='HO' AND WORKER_CODE='" + worker + "')";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string plan, DateTime date)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT HOLIDAY_ID");
                obj_str.Append(" FROM ATT_TR_HOLIDAY");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                obj_str.Append(" AND PLANHOLIDAY_CODE='" + plan + "'");
                obj_str.Append(" AND HOLIDAY_DATE='" + date.ToString("MM/dd/yyyy") + "'");
                                                
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Holiday.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }


        public bool delete(string com, string plan, DateTime date)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_HOLIDAY");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                obj_str.Append(" AND PLANHOLIDAY_CODE='" + plan + "'");
                obj_str.Append(" AND HOLIDAY_DATE='" + date.ToString("MM/dd/yyyy") + "'");
                                              
                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Holiday.delete)" + ex.ToString();
            }

            return blnResult;
        }
        
        public bool insert(string com, string plan, List<cls_TRHoliday> list_model)
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

                obj_str.Append(" DELETE FROM ATT_TR_HOLIDAY");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                obj_str.Append(" AND PLANHOLIDAY_CODE='" + plan + "'");                

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str.ToString());

                //-- Step 2 insert
                if (blnResult)
                {
                    obj_str = new System.Text.StringBuilder();
                    obj_str.Append("INSERT INTO ATT_TR_HOLIDAY");
                    obj_str.Append(" (");
                    obj_str.Append("COMPANY_CODE ");
                    obj_str.Append(", HOLIDAY_DATE ");
                    obj_str.Append(", HOLIDAY_NAME_TH ");
                    obj_str.Append(", HOLIDAY_NAME_EN ");
                    obj_str.Append(", PLANHOLIDAY_CODE ");

                    obj_str.Append(", HOLIDAY_DAYTYPE ");
                    obj_str.Append(", HOLIDAY_PAYPER ");

                    obj_str.Append(" )");

                    obj_str.Append(" VALUES(");
                    obj_str.Append("@COMPANY_CODE ");
                    obj_str.Append(", @HOLIDAY_DATE ");
                    obj_str.Append(", @HOLIDAY_NAME_TH ");
                    obj_str.Append(", @HOLIDAY_NAME_EN ");
                    obj_str.Append(", @PLANHOLIDAY_CODE ");

                    obj_str.Append(", @HOLIDAY_DAYTYPE ");
                    obj_str.Append(", @HOLIDAY_PAYPER ");

                    obj_str.Append(" )");

                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@HOLIDAY_DATE", SqlDbType.DateTime);
                    obj_cmd.Parameters.Add("@HOLIDAY_NAME_TH", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@HOLIDAY_NAME_EN", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@PLANHOLIDAY_CODE", SqlDbType.VarChar);

                    obj_cmd.Parameters.Add("@HOLIDAY_DAYTYPE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@HOLIDAY_PAYPER", SqlDbType.Decimal);
                                        
                    foreach (cls_TRHoliday model in list_model)
                    {

                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                        obj_cmd.Parameters["@HOLIDAY_DATE"].Value = Convert.ToDateTime(model.holiday_date);
                        obj_cmd.Parameters["@HOLIDAY_NAME_TH"].Value = model.holiday_name_th;
                        obj_cmd.Parameters["@HOLIDAY_NAME_EN"].Value = model.holiday_name_en;
                        obj_cmd.Parameters["@PLANHOLIDAY_CODE"].Value = model.planholiday_code;

                        obj_cmd.Parameters["@HOLIDAY_DAYTYPE"].Value = model.holiday_daytype;
                        obj_cmd.Parameters["@HOLIDAY_PAYPER"].Value = model.holiday_payper;
                    
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
                Message = "ERROR::(Holiday.insert)" + ex.ToString();
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
