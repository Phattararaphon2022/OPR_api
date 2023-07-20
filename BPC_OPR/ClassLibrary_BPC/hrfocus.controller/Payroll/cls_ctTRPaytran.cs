using ClassLibrary_BPC.hrfocus.model.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller.Payroll
{
   public class cls_ctTRPaytran
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRPaytran() { }

        public string getMessage() { return this.Message.Replace("PAY_TR_PAYTRAN", "").Replace("cls_ctTRPaytran", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }
        private List<cls_TRPaytran> getData(string language, string condition)
        {
            List<cls_TRPaytran> list_model = new List<cls_TRPaytran>();
            cls_TRPaytran model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PAY_TR_PAYTRAN.COMPANY_CODE");
                obj_str.Append(", PAY_TR_PAYTRAN.WORKER_CODE");
                obj_str.Append(", PAYTRAN_PAYDATE");

                obj_str.Append(", PAYTRAN_SSOEMP");
                obj_str.Append(", PAYTRAN_SSOCOM");
                obj_str.Append(", PAYTRAN_SSORATEEMP");
                obj_str.Append(", PAYTRAN_SSORATECOM");

                obj_str.Append(", PAYTRAN_PFEMP");
                obj_str.Append(", PAYTRAN_PFCOM");

                obj_str.Append(", ISNULL(PAYTRAN_INCOME_401, 0) AS PAYTRAN_INCOME_401");
                obj_str.Append(", ISNULL(PAYTRAN_DEDUCT_401, 0) AS PAYTRAN_DEDUCT_401");
                obj_str.Append(", ISNULL(PAYTRAN_TAX_401, 0) AS PAYTRAN_TAX_401");

                obj_str.Append(", ISNULL(PAYTRAN_INCOME_4012, 0) AS PAYTRAN_INCOME_4012");
                obj_str.Append(", ISNULL(PAYTRAN_DEDUCT_4012, 0) AS PAYTRAN_DEDUCT_4012");
                obj_str.Append(", ISNULL(PAYTRAN_TAX_4012, 0) AS PAYTRAN_TAX_4012");

                obj_str.Append(", ISNULL(PAYTRAN_INCOME_4013, 0) AS PAYTRAN_INCOME_4013");
                obj_str.Append(", ISNULL(PAYTRAN_DEDUCT_4013, 0) AS PAYTRAN_DEDUCT_4013");
                obj_str.Append(", ISNULL(PAYTRAN_TAX_4013, 0) AS PAYTRAN_TAX_4013");

                obj_str.Append(", ISNULL(PAYTRAN_INCOME_402I, 0) AS PAYTRAN_INCOME_402I");
                obj_str.Append(", ISNULL(PAYTRAN_DEDUCT_402I, 0) AS PAYTRAN_DEDUCT_402I");
                obj_str.Append(", ISNULL(PAYTRAN_TAX_402I, 0) AS PAYTRAN_TAX_402I");

                obj_str.Append(", ISNULL(PAYTRAN_INCOME_402O, 0) AS PAYTRAN_INCOME_402O");
                obj_str.Append(", ISNULL(PAYTRAN_DEDUCT_402O, 0) AS PAYTRAN_DEDUCT_402O");
                obj_str.Append(", ISNULL(PAYTRAN_TAX_402O, 0) AS PAYTRAN_TAX_402O");

                obj_str.Append(", ISNULL(PAYTRAN_INCOME_NOTAX, 0) AS PAYTRAN_INCOME_NOTAX");
                obj_str.Append(", ISNULL(PAYTRAN_DEDUCT_NOTAX, 0) AS PAYTRAN_DEDUCT_NOTAX");

                obj_str.Append(", ISNULL(PAYTRAN_INCOME_TOTAL, 0) AS PAYTRAN_INCOME_TOTAL");
                obj_str.Append(", ISNULL(PAYTRAN_DEDUCT_TOTAL, 0) AS PAYTRAN_DEDUCT_TOTAL");

                obj_str.Append(", ISNULL(PAYTRAN_NETPAY_B, 0) AS PAYTRAN_NETPAY_B");
                obj_str.Append(", ISNULL(PAYTRAN_NEYPAY_C, 0) AS PAYTRAN_NEYPAY_C");

                obj_str.Append(", ISNULL(PAY_TR_PAYTRAN.MODIFIED_BY, PAY_TR_PAYTRAN.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(PAY_TR_PAYTRAN.MODIFIED_DATE, PAY_TR_PAYTRAN.CREATED_DATE) AS MODIFIED_DATE");

                if (language.Equals("TH"))
                {
                    obj_str.Append(", INITIAL_NAME_TH + WORKER_FNAME_TH + ' ' + WORKER_LNAME_TH AS WORKER_DETAIL");
                }
                else
                {
                    obj_str.Append(", INITIAL_NAME_EN + WORKER_FNAME_EN + ' ' + WORKER_LNAME_EN AS WORKER_DETAIL");
                }

                obj_str.Append(" ,ISNULL((SELECT SUM(PAYITEM_AMOUNT) FROM PAY_TR_PAYITEM WHERE COMPANY_CODE=PAY_TR_PAYTRAN.COMPANY_CODE AND WORKER_CODE=PAY_TR_PAYTRAN.WORKER_CODE AND PAYITEM_DATE=PAY_TR_PAYTRAN.PAYTRAN_PAYDATE AND ITEM_CODE LIKE 'SA%'), 0) AS SALARY ");
                obj_str.Append(" ,ISNULL((SELECT SUM(PAYITEM_AMOUNT) FROM PAY_TR_PAYITEM WHERE COMPANY_CODE=PAY_TR_PAYTRAN.COMPANY_CODE AND WORKER_CODE=PAY_TR_PAYTRAN.WORKER_CODE AND PAYITEM_DATE=PAY_TR_PAYTRAN.PAYTRAN_PAYDATE AND ITEM_CODE LIKE 'OT%'), 0) AS OVERTIME ");
                obj_str.Append(" ,ISNULL((SELECT SUM(PAYITEM_AMOUNT) FROM PAY_TR_PAYITEM WHERE COMPANY_CODE=PAY_TR_PAYTRAN.COMPANY_CODE AND WORKER_CODE=PAY_TR_PAYTRAN.WORKER_CODE AND PAYITEM_DATE=PAY_TR_PAYTRAN.PAYTRAN_PAYDATE AND ITEM_CODE LIKE 'DG%'), 0) AS DILIGENCE ");

                obj_str.Append(" ,ISNULL((SELECT SUM(PAYITEM_AMOUNT) FROM PAY_TR_PAYITEM WHERE COMPANY_CODE=PAY_TR_PAYTRAN.COMPANY_CODE AND WORKER_CODE=PAY_TR_PAYTRAN.WORKER_CODE AND PAYITEM_DATE=PAY_TR_PAYTRAN.PAYTRAN_PAYDATE AND ITEM_CODE LIKE 'AB%'), 0) AS ABSENT ");
                obj_str.Append(" ,ISNULL((SELECT SUM(PAYITEM_AMOUNT) FROM PAY_TR_PAYITEM WHERE COMPANY_CODE=PAY_TR_PAYTRAN.COMPANY_CODE AND WORKER_CODE=PAY_TR_PAYTRAN.WORKER_CODE AND PAYITEM_DATE=PAY_TR_PAYTRAN.PAYTRAN_PAYDATE AND ITEM_CODE LIKE 'LT%'), 0) AS LATE ");
                obj_str.Append(" ,ISNULL((SELECT SUM(PAYITEM_AMOUNT) FROM PAY_TR_PAYITEM WHERE COMPANY_CODE=PAY_TR_PAYTRAN.COMPANY_CODE AND WORKER_CODE=PAY_TR_PAYTRAN.WORKER_CODE AND PAYITEM_DATE=PAY_TR_PAYTRAN.PAYTRAN_PAYDATE AND ITEM_CODE LIKE 'LV%'), 0) AS LEAVE ");
                                
                obj_str.Append(" FROM PAY_TR_PAYTRAN");
                obj_str.Append(" INNER JOIN EMP_MT_WORKER ON EMP_MT_WORKER.COMPANY_CODE=PAY_TR_PAYTRAN.COMPANY_CODE AND EMP_MT_WORKER.WORKER_CODE=PAY_TR_PAYTRAN.WORKER_CODE");
                obj_str.Append(" INNER JOIN EMP_MT_INITIAL ON EMP_MT_INITIAL.INITIAL_CODE=EMP_MT_WORKER.WORKER_INITIAL ");

                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PAY_TR_PAYTRAN.COMPANY_CODE, PAYTRAN_PAYDATE, PAY_TR_PAYTRAN.WORKER_CODE");
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRPaytran();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                    model.worker_code = Convert.ToString(dr["WORKER_CODE"]);
                    model.paytran_date = Convert.ToDateTime(dr["PAYTRAN_PAYDATE"]);


                    model.paytran_ssoemp = Convert.ToDouble(dr["PAYTRAN_SSOEMP"]);
                    model.paytran_ssocom = Convert.ToDouble(dr["PAYTRAN_SSOCOM"]);
                    model.paytran_ssorateemp = Convert.ToDouble(dr["PAYTRAN_SSORATEEMP"]);
                    model.paytran_ssoratecom = Convert.ToDouble(dr["PAYTRAN_SSORATECOM"]);

                    model.paytran_pfemp = Convert.ToDouble(dr["PAYTRAN_PFEMP"]);
                    model.paytran_pfcom = Convert.ToDouble(dr["PAYTRAN_PFCOM"]);

                    model.paytran_income_401 = Convert.ToDouble(dr["PAYTRAN_INCOME_401"]);
                    model.paytran_deduct_401 = Convert.ToDouble(dr["PAYTRAN_DEDUCT_401"]);
                    model.paytran_tax_401 = Convert.ToDouble(dr["PAYTRAN_TAX_401"]);

                    model.paytran_income_4012 = Convert.ToDouble(dr["PAYTRAN_INCOME_4012"]);
                    model.paytran_deduct_4012 = Convert.ToDouble(dr["PAYTRAN_DEDUCT_4012"]);
                    model.paytran_tax_4012 = Convert.ToDouble(dr["PAYTRAN_TAX_4012"]);

                    model.paytran_income_4013 = Convert.ToDouble(dr["PAYTRAN_INCOME_4013"]);
                    model.paytran_deduct_4013 = Convert.ToDouble(dr["PAYTRAN_DEDUCT_4013"]);
                    model.paytran_tax_4013 = Convert.ToDouble(dr["PAYTRAN_TAX_4013"]);

                    model.paytran_income_402I = Convert.ToDouble(dr["PAYTRAN_INCOME_402I"]);
                    model.paytran_deduct_402I = Convert.ToDouble(dr["PAYTRAN_DEDUCT_402I"]);
                    model.paytran_tax_402I = Convert.ToDouble(dr["PAYTRAN_TAX_402I"]);

                    model.paytran_income_402O = Convert.ToDouble(dr["PAYTRAN_INCOME_402O"]);
                    model.paytran_deduct_402O = Convert.ToDouble(dr["PAYTRAN_DEDUCT_402O"]);
                    model.paytran_tax_402O = Convert.ToDouble(dr["PAYTRAN_TAX_402O"]);

                    model.paytran_income_notax = Convert.ToDouble(dr["PAYTRAN_INCOME_NOTAX"]);
                    model.paytran_deduct_notax = Convert.ToDouble(dr["PAYTRAN_DEDUCT_NOTAX"]);

                    model.paytran_income_total = Convert.ToDouble(dr["PAYTRAN_INCOME_TOTAL"]);
                    model.paytran_deduct_total = Convert.ToDouble(dr["PAYTRAN_DEDUCT_TOTAL"]);

                    model.paytran_netpay_b = Convert.ToDouble(dr["PAYTRAN_NETPAY_B"]);
                    model.paytran_netpay_c = Convert.ToDouble(dr["PAYTRAN_NEYPAY_C"]);

                    model.worker_detail = Convert.ToString(dr["WORKER_DETAIL"]);

                    model.paytran_salary = Convert.ToDouble(dr["SALARY"]);
                    model.paytran_overtime = Convert.ToDouble(dr["OVERTIME"]);
                    model.paytran_diligence = Convert.ToDouble(dr["DILIGENCE"]);

                    model.paytran_absent = Convert.ToDouble(dr["ABSENT"]);
                    model.paytran_late = Convert.ToDouble(dr["LATE"]);
                    model.paytran_leave = Convert.ToDouble(dr["LEAVE"]);


                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "PAYTRAN001:" + ex.ToString();
            }

            return list_model;
        }

        //public List<cls_TRPayitem> getDataByFillter(string language, string com, string emp,  string item_type, string item)
        public List<cls_TRPaytran> getDataByFillter(string language, string com, DateTime datefrom, DateTime dateto, string emp)
        {
            string strCondition = "";

            strCondition += " AND PAY_TR_PAYTRAN.COMPANY_CODE='" + com + "'";
            strCondition += " AND (PAY_TR_PAYTRAN.PAYTRAN_PAYDATE BETWEEN '" + datefrom.ToString("MM/dd/yyyy") + "' AND '" + dateto.ToString("MM/dd/yyyy") + "')";

            if (!emp.Equals(""))
                strCondition += " AND PAY_TR_PAYTRAN.WORKER_CODE='" + emp + "'";

            return this.getData(language, strCondition);
        }
        public List<cls_TRPaytran> getDataByYear(string language, string com, string year, string emp)
        {
            string strCondition = "";

            strCondition += " AND PAY_TR_PAYTRAN.COMPANY_CODE='" + com + "'";

            //strCondition += " AND PAY_TR_PAYTRAN.PAYTRAN_PAYDATE IN (SELECT PERIOD_PAYMENT FROM PAY_MT_PERIOD WHERE COMPANY_CODE='" + com + "' AND YEAR_CODE='" + year + "')";

            if (!emp.Equals(""))
                strCondition += " AND PAY_TR_PAYTRAN.WORKER_CODE='" + emp + "'";

            return this.getData(language, strCondition);
        }
        public List<cls_TRPaytran> getDataMultipleEmp(string language, string com, DateTime datefrom, DateTime dateto, string emp)
        {
            string strCondition = "";

            strCondition += " AND PAY_TR_PAYTRAN.COMPANY_CODE='" + com + "'";
            strCondition += " AND (PAY_TR_PAYTRAN.PAYTRAN_PAYDATE BETWEEN '" + datefrom.ToString("MM/dd/yyyy") + "' AND '" + dateto.ToString("MM/dd/yyyy") + "')";
            strCondition += " AND PAY_TR_PAYTRAN.WORKER_CODE IN (" + emp + ")";

            return this.getData(language, strCondition);
        }



        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" FROM PAY_TR_PAYTRAN");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "PAYTRAN002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp, DateTime date)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PAYTRAN_PAYDATE");
                obj_str.Append(" FROM PAY_TR_PAYTRAN");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                obj_str.Append(" AND PAYTRAN_PAYDATE='" + date.ToString("MM/dd/yyyy") + "' ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "PAYTRAN003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string emp, DateTime date)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM PAY_TR_PAYTRAN");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                obj_str.Append(" AND PAYTRAN_PAYDATE='" + date.ToString("MM/dd/yyyy") + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "PAYTRAN004:" + ex.ToString();
            }

            return blnResult;
        }

        //public bool delete(string com, string emp)
        //{
        //    bool blnResult = true;
        //    try
        //    {
        //        cls_ctConnection obj_conn = new cls_ctConnection();

        //        System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

        //        obj_str.Append(" DELETE FROM PAY_TR_PAYITEM");
        //        obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
        //        obj_str.Append(" AND WORKER_CODE='" + emp + "' ");

        //        blnResult = obj_conn.doExecuteSQL(obj_str.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        blnResult = false;
        //        Message = "ERROR::(Payitem.delete)" + ex.ToString();
        //    }

        //    return blnResult;
        //}

        public bool insert(cls_TRPaytran model)

        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.worker_code, model.paytran_date))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PAY_TR_PAYTRAN");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");
                obj_str.Append(", PAYTRAN_PAYDATE");

                obj_str.Append(", PAYTRAN_SSOEMP");
                obj_str.Append(", PAYTRAN_SSOCOM");
                obj_str.Append(", PAYTRAN_SSORATEEMP");
                obj_str.Append(", PAYTRAN_SSORATECOM");

                obj_str.Append(", PAYTRAN_PFEMP");
                obj_str.Append(", PAYTRAN_PFCOM");

                obj_str.Append(", PAYTRAN_INCOME_401");
                obj_str.Append(", PAYTRAN_DEDUCT_401");
                obj_str.Append(", PAYTRAN_TAX_401");

                obj_str.Append(", PAYTRAN_INCOME_4012");
                obj_str.Append(", PAYTRAN_DEDUCT_4012");
                obj_str.Append(", PAYTRAN_TAX_4012");

                obj_str.Append(", PAYTRAN_INCOME_4013");
                obj_str.Append(", PAYTRAN_DEDUCT_4013");
                obj_str.Append(", PAYTRAN_TAX_4013");

                obj_str.Append(", PAYTRAN_INCOME_402I");
                obj_str.Append(", PAYTRAN_DEDUCT_402I");
                obj_str.Append(", PAYTRAN_TAX_402I");

                obj_str.Append(", PAYTRAN_INCOME_402O");
                obj_str.Append(", PAYTRAN_DEDUCT_402O");
                obj_str.Append(", PAYTRAN_TAX_402O");

                obj_str.Append(", PAYTRAN_INCOME_NOTAX");
                obj_str.Append(", PAYTRAN_DEDUCT_NOTAX");

                obj_str.Append(", PAYTRAN_INCOME_TOTAL");
                obj_str.Append(", PAYTRAN_DEDUCT_TOTAL");

                obj_str.Append(", PAYTRAN_NETPAY_B");
                obj_str.Append(", PAYTRAN_NEYPAY_C");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE");
                obj_str.Append(", @WORKER_CODE");
                obj_str.Append(", @PAYTRAN_PAYDATE");

                obj_str.Append(", @PAYTRAN_SSOEMP");
                obj_str.Append(", @PAYTRAN_SSOCOM");
                obj_str.Append(", @PAYTRAN_SSORATEEMP");
                obj_str.Append(", @PAYTRAN_SSORATECOM");

                obj_str.Append(", @PAYTRAN_PFEMP");
                obj_str.Append(", @PAYTRAN_PFCOM");

                obj_str.Append(", @PAYTRAN_INCOME_401");
                obj_str.Append(", @PAYTRAN_DEDUCT_401");
                obj_str.Append(", @PAYTRAN_TAX_401");

                obj_str.Append(", @PAYTRAN_INCOME_4012");
                obj_str.Append(", @PAYTRAN_DEDUCT_4012");
                obj_str.Append(", @PAYTRAN_TAX_4012");

                obj_str.Append(", @PAYTRAN_INCOME_4013");
                obj_str.Append(", @PAYTRAN_DEDUCT_4013");
                obj_str.Append(", @PAYTRAN_TAX_4013");

                obj_str.Append(", @PAYTRAN_INCOME_402I");
                obj_str.Append(", @PAYTRAN_DEDUCT_402I");
                obj_str.Append(", @PAYTRAN_TAX_402I");

                obj_str.Append(", @PAYTRAN_INCOME_402O");
                obj_str.Append(", @PAYTRAN_DEDUCT_402O");
                obj_str.Append(", @PAYTRAN_TAX_402O");

                obj_str.Append(", @PAYTRAN_INCOME_NOTAX");
                obj_str.Append(", @PAYTRAN_DEDUCT_NOTAX");

                obj_str.Append(", @PAYTRAN_INCOME_TOTAL");
                obj_str.Append(", @PAYTRAN_DEDUCT_TOTAL");

                obj_str.Append(", @PAYTRAN_NETPAY_B");
                obj_str.Append(", @PAYTRAN_NEYPAY_C");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@PAYTRAN_PAYDATE", SqlDbType.Date); obj_cmd.Parameters["@PAYTRAN_PAYDATE"].Value = model.paytran_date;

                obj_cmd.Parameters.Add("@PAYTRAN_SSOEMP", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_SSOEMP"].Value = model.paytran_ssoemp;
                obj_cmd.Parameters.Add("@PAYTRAN_SSOCOM", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_SSOCOM"].Value = model.paytran_ssocom;
                obj_cmd.Parameters.Add("@PAYTRAN_SSORATEEMP", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_SSORATEEMP"].Value = model.paytran_ssoratecom;
                obj_cmd.Parameters.Add("@PAYTRAN_SSORATECOM", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_SSORATECOM"].Value = model.paytran_ssoratecom;

                obj_cmd.Parameters.Add("@PAYTRAN_PFEMP", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_PFEMP"].Value = model.paytran_pfemp;
                obj_cmd.Parameters.Add("@PAYTRAN_PFCOM", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_PFCOM"].Value = model.paytran_pfcom;

                obj_cmd.Parameters.Add("@PAYTRAN_INCOME_401", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_INCOME_401"].Value = model.paytran_income_401;
                obj_cmd.Parameters.Add("@PAYTRAN_DEDUCT_401", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_DEDUCT_401"].Value = model.paytran_deduct_401;
                obj_cmd.Parameters.Add("@PAYTRAN_TAX_401", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_TAX_401"].Value = model.paytran_tax_401;

                obj_cmd.Parameters.Add("@PAYTRAN_INCOME_4012", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_INCOME_4012"].Value = model.paytran_income_4012;
                obj_cmd.Parameters.Add("@PAYTRAN_DEDUCT_4012", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_DEDUCT_4012"].Value = model.paytran_deduct_4012;
                obj_cmd.Parameters.Add("@PAYTRAN_TAX_4012", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_TAX_4012"].Value = model.paytran_tax_4012;


                obj_cmd.Parameters.Add("@PAYTRAN_INCOME_4013", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_INCOME_4013"].Value = model.paytran_income_4013;
                obj_cmd.Parameters.Add("@PAYTRAN_DEDUCT_4013", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_DEDUCT_4013"].Value = model.paytran_deduct_4013;
                obj_cmd.Parameters.Add("@PAYTRAN_TAX_4013", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_TAX_4013"].Value = model.paytran_tax_4013;

                obj_cmd.Parameters.Add("@PAYTRAN_INCOME_402I", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_INCOME_402I"].Value = model.paytran_income_402I;
                obj_cmd.Parameters.Add("@PAYTRAN_DEDUCT_402I", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_DEDUCT_402I"].Value = model.paytran_deduct_402I;
                obj_cmd.Parameters.Add("@PAYTRAN_TAX_402I", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_TAX_402I"].Value = model.paytran_tax_402I;

                obj_cmd.Parameters.Add("@PAYTRAN_INCOME_402O", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_INCOME_402O"].Value = model.paytran_income_402O;
                obj_cmd.Parameters.Add("@PAYTRAN_DEDUCT_402O", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_DEDUCT_402O"].Value = model.paytran_deduct_402O;
                obj_cmd.Parameters.Add("@PAYTRAN_TAX_402O", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_TAX_402O"].Value = model.paytran_tax_402O;

                obj_cmd.Parameters.Add("@PAYTRAN_INCOME_NOTAX", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_INCOME_NOTAX"].Value = model.paytran_income_notax;
                obj_cmd.Parameters.Add("@PAYTRAN_DEDUCT_NOTAX", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_DEDUCT_NOTAX"].Value = model.paytran_deduct_notax;

                obj_cmd.Parameters.Add("@PAYTRAN_INCOME_TOTAL", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_INCOME_TOTAL"].Value = model.paytran_income_total;
                obj_cmd.Parameters.Add("@PAYTRAN_DEDUCT_TOTAL", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_DEDUCT_TOTAL"].Value = model.paytran_deduct_total;

                obj_cmd.Parameters.Add("@PAYTRAN_NETPAY_B", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_NETPAY_B"].Value = model.paytran_netpay_b;
                obj_cmd.Parameters.Add("@PAYTRAN_NEYPAY_C", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_NEYPAY_C"].Value = model.paytran_netpay_c;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "PAYTRAN005:" + ex.ToString();
            }

            return blnResult;
        }

        public bool update(cls_TRPaytran model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE PAY_TR_PAYTRAN SET ");

                obj_str.Append(" PAYTRAN_SSOEMP=@PAYTRAN_SSOEMP");
                obj_str.Append(", PAYTRAN_SSOCOM=@PAYTRAN_SSOCOM");
                obj_str.Append(", PAYTRAN_SSORATEEMP=@PAYTRAN_SSORATEEMP");
                obj_str.Append(", PAYTRAN_SSORATECOM=@PAYTRAN_SSORATECOM");

                obj_str.Append(", PAYTRAN_PFEMP=@PAYTRAN_PFEMP");
                obj_str.Append(", PAYTRAN_PFCOM=@PAYTRAN_PFCOM");

                obj_str.Append(", PAYTRAN_INCOME_401=@PAYTRAN_INCOME_401");
                obj_str.Append(", PAYTRAN_DEDUCT_401=@PAYTRAN_DEDUCT_401");
                obj_str.Append(", PAYTRAN_TAX_401=@PAYTRAN_TAX_401");

                obj_str.Append(", PAYTRAN_INCOME_4012=@PAYTRAN_INCOME_4012");
                obj_str.Append(", PAYTRAN_DEDUCT_4012=@PAYTRAN_DEDUCT_4012");
                obj_str.Append(", PAYTRAN_TAX_4012=@PAYTRAN_TAX_4012");

                obj_str.Append(", PAYTRAN_INCOME_4013=@PAYTRAN_INCOME_4013");
                obj_str.Append(", PAYTRAN_DEDUCT_4013=@PAYTRAN_DEDUCT_4013");
                obj_str.Append(", PAYTRAN_TAX_4013=@PAYTRAN_TAX_4013");

                obj_str.Append(", PAYTRAN_INCOME_402I=@PAYTRAN_INCOME_402I");
                obj_str.Append(", PAYTRAN_DEDUCT_402I=@PAYTRAN_DEDUCT_402I");
                obj_str.Append(", PAYTRAN_TAX_402I=@PAYTRAN_TAX_402I");

                obj_str.Append(", PAYTRAN_INCOME_402O=@PAYTRAN_INCOME_402O");
                obj_str.Append(", PAYTRAN_DEDUCT_402O=@PAYTRAN_DEDUCT_402O");
                obj_str.Append(", PAYTRAN_TAX_402O=@PAYTRAN_TAX_402O");

                obj_str.Append(", PAYTRAN_INCOME_NOTAX=@PAYTRAN_INCOME_NOTAX");
                obj_str.Append(", PAYTRAN_DEDUCT_NOTAX=@PAYTRAN_DEDUCT_NOTAX");

                obj_str.Append(", PAYTRAN_INCOME_TOTAL=@PAYTRAN_INCOME_TOTAL");
                obj_str.Append(", PAYTRAN_DEDUCT_TOTAL=@PAYTRAN_DEDUCT_TOTAL");

                obj_str.Append(", PAYTRAN_NETPAY_B=@PAYTRAN_NETPAY_B");
                obj_str.Append(", PAYTRAN_NEYPAY_C=@PAYTRAN_NEYPAY_C");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" AND PAYTRAN_PAYDATE=@WAGEDAY_DATE ");

                obj_conn.doConnect();


                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                obj_cmd.Parameters.Add("@PAYTRAN_SSOEMP", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_SSOEMP"].Value = model.paytran_ssoemp;
                obj_cmd.Parameters.Add("@PAYTRAN_SSOCOM", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_SSOCOM"].Value = model.paytran_ssocom;
                obj_cmd.Parameters.Add("@PAYTRAN_SSORATEEMP", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_SSORATEEMP"].Value = model.paytran_ssoratecom;
                obj_cmd.Parameters.Add("@PAYTRAN_SSORATECOM", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_SSORATECOM"].Value = model.paytran_ssoratecom;

                obj_cmd.Parameters.Add("@PAYTRAN_PFEMP", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_PFEMP"].Value = model.paytran_pfemp;
                obj_cmd.Parameters.Add("@PAYTRAN_PFCOM", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_PFCOM"].Value = model.paytran_pfcom;

                obj_cmd.Parameters.Add("@PAYTRAN_INCOME_401", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_INCOME_401"].Value = model.paytran_income_401;
                obj_cmd.Parameters.Add("@PAYTRAN_DEDUCT_401", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_DEDUCT_401"].Value = model.paytran_deduct_401;
                obj_cmd.Parameters.Add("@PAYTRAN_TAX_401", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_TAX_401"].Value = model.paytran_tax_401;

                obj_cmd.Parameters.Add("@PAYTRAN_INCOME_4012", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_INCOME_4012"].Value = model.paytran_income_4012;
                obj_cmd.Parameters.Add("@PAYTRAN_DEDUCT_4012", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_DEDUCT_4012"].Value = model.paytran_deduct_4012;
                obj_cmd.Parameters.Add("@PAYTRAN_TAX_4012", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_TAX_4012"].Value = model.paytran_tax_4012;

                obj_cmd.Parameters.Add("@PAYTRAN_INCOME_4013", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_INCOME_4013"].Value = model.paytran_income_4013;
                obj_cmd.Parameters.Add("@PAYTRAN_DEDUCT_4013", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_DEDUCT_4013"].Value = model.paytran_deduct_4013;
                obj_cmd.Parameters.Add("@PAYTRAN_TAX_4013", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_TAX_4013"].Value = model.paytran_tax_4013;

                obj_cmd.Parameters.Add("@PAYTRAN_INCOME_402I", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_INCOME_402I"].Value = model.paytran_income_402I;
                obj_cmd.Parameters.Add("@PAYTRAN_DEDUCT_402I", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_DEDUCT_402I"].Value = model.paytran_deduct_402I;
                obj_cmd.Parameters.Add("@PAYTRAN_TAX_402I", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_TAX_402I"].Value = model.paytran_tax_402I;

                obj_cmd.Parameters.Add("@PAYTRAN_INCOME_402O", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_INCOME_402O"].Value = model.paytran_income_402O;
                obj_cmd.Parameters.Add("@PAYTRAN_DEDUCT_402O", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_DEDUCT_402O"].Value = model.paytran_deduct_402O;
                obj_cmd.Parameters.Add("@PAYTRAN_TAX_402O", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_TAX_402O"].Value = model.paytran_tax_402O;

                obj_cmd.Parameters.Add("@PAYTRAN_INCOME_NOTAX", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_INCOME_NOTAX"].Value = model.paytran_income_notax;
                obj_cmd.Parameters.Add("@PAYTRAN_DEDUCT_NOTAX", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_DEDUCT_NOTAX"].Value = model.paytran_deduct_notax;

                obj_cmd.Parameters.Add("@PAYTRAN_INCOME_TOTAL", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_INCOME_TOTAL"].Value = model.paytran_income_total;
                obj_cmd.Parameters.Add("@PAYTRAN_DEDUCT_TOTAL", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_DEDUCT_TOTAL"].Value = model.paytran_deduct_total;

                obj_cmd.Parameters.Add("@PAYTRAN_NETPAY_B", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_NETPAY_B"].Value = model.paytran_netpay_b;
                obj_cmd.Parameters.Add("@PAYTRAN_NEYPAY_C", SqlDbType.Decimal); obj_cmd.Parameters["@PAYTRAN_NEYPAY_C"].Value = model.paytran_netpay_c;


                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@PAYTRAN_PAYDATE", SqlDbType.Date); obj_cmd.Parameters["@PAYTRAN_PAYDATE"].Value = model.paytran_date;

                obj_cmd.ExecuteNonQuery();


                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "PAYTRAN006:" + ex.ToString();
            }

            return blnResult;
        }

        
   }
}
