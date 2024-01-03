using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRBenefit
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRBenefit() { }

        public string getMessage() { return this.Message.Replace("EMP_TR_BENEFIT", "").Replace("cls_ctTRBenefit", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRBenefit> getData(string condition)
        {
            List<cls_TRBenefit> list_model = new List<cls_TRBenefit>();
            cls_TRBenefit model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");

                obj_str.Append(", EMPBENEFIT_ID");
                obj_str.Append(", ITEM_CODE");
                obj_str.Append(", EMPBENEFIT_AMOUNT");
                obj_str.Append(", EMPBENEFIT_STARTDATE");
                obj_str.Append(", EMPBENEFIT_ENDDATE");
                obj_str.Append(", EMPBENEFIT_REASON");
                obj_str.Append(", ISNULL(EMPBENEFIT_NOTE, '') AS EMPBENEFIT_NOTE");
                obj_str.Append(", ISNULL(EMPBENEFIT_PAYTYPE, 'A') AS EMPBENEFIT_PAYTYPE");
                obj_str.Append(", ISNULL(EMPBENEFIT_BREAK, 0) AS EMPBENEFIT_BREAK");
                obj_str.Append(", ISNULL(EMPBENEFIT_BREAKREASON, '') AS EMPBENEFIT_BREAKREASON");
                obj_str.Append(", ISNULL(EMPBENEFIT_CONDITIONPAY, 'F') AS EMPBENEFIT_CONDITIONPAY");
                obj_str.Append(", ISNULL(EMPBENEFIT_PAYFIRST, 'Y') AS EMPBENEFIT_PAYFIRST");
                obj_str.Append(", ISNULL(EMPBENEFIT_CAPITALAMOUNT, 0) AS EMPBENEFIT_CAPITALAMOUNT");
                obj_str.Append(", ISNULL(EMPBENEFIT_PERIOD, 0) AS EMPBENEFIT_PERIOD");


                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_TR_BENEFIT");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRBenefit();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.item_code = dr["ITEM_CODE"].ToString();
                    model.empbenefit_id = Convert.ToInt32(dr["EMPBENEFIT_ID"]);
                    model.empbenefit_amount = Convert.ToDouble(dr["EMPBENEFIT_AMOUNT"]);
                    model.empbenefit_startdate = Convert.ToDateTime(dr["EMPBENEFIT_STARTDATE"]);
                    model.empbenefit_enddate = Convert.ToDateTime(dr["EMPBENEFIT_ENDDATE"]);
                    model.empbenefit_reason = dr["EMPBENEFIT_REASON"].ToString();
                    model.empbenefit_note = dr["EMPBENEFIT_NOTE"].ToString();
                    model.empbenefit_paytype = dr["EMPBENEFIT_PAYTYPE"].ToString();
                    model.empbenefit_break = Convert.ToBoolean(dr["EMPBENEFIT_BREAK"]);
                    model.empbenefit_breakreason = dr["EMPBENEFIT_BREAKREASON"].ToString();
                    model.empbenefit_conditionpay = dr["EMPBENEFIT_CONDITIONPAY"].ToString();
                    model.empbenefit_payfirst = dr["EMPBENEFIT_PAYFIRST"].ToString();
                    model.empbenefit_capitalamount = Convert.ToDouble(dr["EMPBENEFIT_CAPITALAMOUNT"]);
                    model.empbenefit_period = Convert.ToDouble(dr["EMPBENEFIT_PERIOD"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "EMPBNF001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRBenefit> getDataByFillter(string com, string emp ,string code)
        {
            string strCondition = "";

            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!emp.Equals(""))
                strCondition += " AND WORKER_CODE='" + emp + "'";

            if (!code.Equals(""))
                strCondition += " AND ITEM_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(EMPBENEFIT_ID, 1) ");
                obj_str.Append(" FROM EMP_TR_BENEFIT");
                obj_str.Append(" ORDER BY EMPBENEFIT_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPBNF002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp,int id,string item ,string date)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT EMPBENEFIT_ID");
                obj_str.Append(" FROM EMP_TR_BENEFIT");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                if(!id.Equals(0)){
                    obj_str.Append(" AND EMPBENEFIT_ID='" + id + "' ");
                }
                if (!item.ToString().Equals(""))
                {
                    obj_str.Append(" AND ITEM_CODE='" + item + "' ");
                }
                if (!date.ToString().Equals(""))
                {
                    obj_str.Append(" AND EMPBENEFIT_STARTDATE='" + date + "' ");
                }

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPBNF003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string emp)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM EMP_TR_BENEFIT");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "EMPBNF004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRBenefit model)
        {
            bool blnResult = false;
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.company_code, model.worker_code, model.empbenefit_id, model.item_code, model.empbenefit_startdate.ToString("yyyy-MM-ddTHH:mm:ss")))
                {
                        return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_TR_BENEFIT");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");

                obj_str.Append(", EMPBENEFIT_ID ");
                obj_str.Append(", ITEM_CODE ");
                obj_str.Append(", EMPBENEFIT_AMOUNT ");
                obj_str.Append(", EMPBENEFIT_STARTDATE ");
                obj_str.Append(", EMPBENEFIT_ENDDATE ");
                obj_str.Append(", EMPBENEFIT_REASON ");
                obj_str.Append(", EMPBENEFIT_NOTE ");
                obj_str.Append(", EMPBENEFIT_PAYTYPE ");
                obj_str.Append(", EMPBENEFIT_BREAK ");

                if (model.empbenefit_break)
                {
                    obj_str.Append(", EMPBENEFIT_BREAKREASON ");
                }
                obj_str.Append(", EMPBENEFIT_CONDITIONPAY ");
                obj_str.Append(", EMPBENEFIT_PAYFIRST ");
                
                obj_str.Append(", EMPBENEFIT_CAPITALAMOUNT ");
                obj_str.Append(", EMPBENEFIT_PERIOD ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");

                obj_str.Append(", @EMPBENEFIT_ID ");
                obj_str.Append(", @ITEM_CODE ");
                obj_str.Append(", @EMPBENEFIT_AMOUNT ");
                obj_str.Append(", @EMPBENEFIT_STARTDATE ");
                obj_str.Append(", @EMPBENEFIT_ENDDATE ");
                obj_str.Append(", @EMPBENEFIT_REASON ");
                obj_str.Append(", @EMPBENEFIT_NOTE ");
                obj_str.Append(", @EMPBENEFIT_PAYTYPE ");
                obj_str.Append(", @EMPBENEFIT_BREAK ");
                if (model.empbenefit_break)
                {
                    obj_str.Append(", @EMPBENEFIT_BREAKREASON ");
                }
                obj_str.Append(", @EMPBENEFIT_CONDITIONPAY ");
                obj_str.Append(", @EMPBENEFIT_PAYFIRST ");

                obj_str.Append(", @EMPBENEFIT_CAPITALAMOUNT ");
                obj_str.Append(", @EMPBENEFIT_PERIOD ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@EMPBENEFIT_ID", SqlDbType.Int); obj_cmd.Parameters["@EMPBENEFIT_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@ITEM_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_CODE"].Value = model.item_code;
                obj_cmd.Parameters.Add("@EMPBENEFIT_AMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@EMPBENEFIT_AMOUNT"].Value = model.empbenefit_amount;
                obj_cmd.Parameters.Add("@EMPBENEFIT_STARTDATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPBENEFIT_STARTDATE"].Value = model.empbenefit_startdate;
                obj_cmd.Parameters.Add("@EMPBENEFIT_ENDDATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPBENEFIT_ENDDATE"].Value = model.empbenefit_enddate;
                obj_cmd.Parameters.Add("@EMPBENEFIT_REASON", SqlDbType.VarChar); obj_cmd.Parameters["@EMPBENEFIT_REASON"].Value = model.empbenefit_reason;
                obj_cmd.Parameters.Add("@EMPBENEFIT_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@EMPBENEFIT_NOTE"].Value = model.empbenefit_note;
                obj_cmd.Parameters.Add("@EMPBENEFIT_PAYTYPE", SqlDbType.VarChar); obj_cmd.Parameters["@EMPBENEFIT_PAYTYPE"].Value = model.empbenefit_paytype;
                obj_cmd.Parameters.Add("@EMPBENEFIT_BREAK", SqlDbType.Bit); obj_cmd.Parameters["@EMPBENEFIT_BREAK"].Value = model.empbenefit_break;
                if (model.empbenefit_break)
                {
                    obj_cmd.Parameters.Add("@EMPBENEFIT_BREAKREASON", SqlDbType.VarChar); obj_cmd.Parameters["@EMPBENEFIT_BREAKREASON"].Value = model.empbenefit_breakreason;
                }
                obj_cmd.Parameters.Add("@EMPBENEFIT_CONDITIONPAY", SqlDbType.VarChar); obj_cmd.Parameters["@EMPBENEFIT_CONDITIONPAY"].Value = model.empbenefit_conditionpay;
                obj_cmd.Parameters.Add("@EMPBENEFIT_PAYFIRST", SqlDbType.VarChar); obj_cmd.Parameters["@EMPBENEFIT_PAYFIRST"].Value = model.empbenefit_payfirst;

                obj_cmd.Parameters.Add("@EMPBENEFIT_CAPITALAMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@EMPBENEFIT_CAPITALAMOUNT"].Value = model.empbenefit_capitalamount;
                obj_cmd.Parameters.Add("@EMPBENEFIT_PERIOD", SqlDbType.Decimal); obj_cmd.Parameters["@EMPBENEFIT_PERIOD"].Value = model.empbenefit_period;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                strResult = model.empbenefit_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "EMPBNF005:" + ex.ToString();
                strResult = "";
            }

            return blnResult;
        }

        public bool update(cls_TRBenefit model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE EMP_TR_BENEFIT SET ");

                obj_str.Append(" EMPBENEFIT_AMOUNT=@EMPBENEFIT_AMOUNT ");
                obj_str.Append(", EMPBENEFIT_ENDDATE=@EMPBENEFIT_ENDDATE ");
                obj_str.Append(", EMPBENEFIT_REASON=@EMPBENEFIT_REASON ");
                obj_str.Append(", EMPBENEFIT_NOTE=@EMPBENEFIT_NOTE ");
                obj_str.Append(", EMPBENEFIT_PAYTYPE=@EMPBENEFIT_PAYTYPE ");
                obj_str.Append(", EMPBENEFIT_BREAK=@EMPBENEFIT_BREAK ");
                if (model.empbenefit_break)
                {
                    obj_str.Append(", EMPBENEFIT_BREAKREASON=@EMPBENEFIT_BREAKREASON ");
                }
                obj_str.Append(", EMPBENEFIT_CONDITIONPAY=@EMPBENEFIT_CONDITIONPAY ");
                obj_str.Append(", EMPBENEFIT_PAYFIRST=@EMPBENEFIT_PAYFIRST ");
                
                obj_str.Append(", EMPBENEFIT_CAPITALAMOUNT=@EMPBENEFIT_CAPITALAMOUNT ");
                obj_str.Append(", EMPBENEFIT_PERIOD=@EMPBENEFIT_PERIOD ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE "); ;

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" AND ITEM_CODE=@ITEM_CODE ");
                obj_str.Append(" AND EMPBENEFIT_STARTDATE=@EMPBENEFIT_STARTDATE ");
                if (!model.empbenefit_id.Equals(0))
                {
                    obj_str.Append(" AND EMPBENEFIT_ID=@EMPBENEFIT_ID ");
                }

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@ITEM_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_CODE"].Value = model.item_code;
                obj_cmd.Parameters.Add("@EMPBENEFIT_AMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@EMPBENEFIT_AMOUNT"].Value = model.empbenefit_amount;
                obj_cmd.Parameters.Add("@EMPBENEFIT_STARTDATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPBENEFIT_STARTDATE"].Value = model.empbenefit_startdate;
                obj_cmd.Parameters.Add("@EMPBENEFIT_ENDDATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPBENEFIT_ENDDATE"].Value = model.empbenefit_enddate;
                obj_cmd.Parameters.Add("@EMPBENEFIT_REASON", SqlDbType.VarChar); obj_cmd.Parameters["@EMPBENEFIT_REASON"].Value = model.empbenefit_reason;
                obj_cmd.Parameters.Add("@EMPBENEFIT_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@EMPBENEFIT_NOTE"].Value = model.empbenefit_note;
                obj_cmd.Parameters.Add("@EMPBENEFIT_PAYTYPE", SqlDbType.VarChar); obj_cmd.Parameters["@EMPBENEFIT_PAYTYPE"].Value = model.empbenefit_paytype;
                obj_cmd.Parameters.Add("@EMPBENEFIT_BREAK", SqlDbType.Bit); obj_cmd.Parameters["@EMPBENEFIT_BREAK"].Value = model.empbenefit_break;
                if (model.empbenefit_break)
                {
                    obj_cmd.Parameters.Add("@EMPBENEFIT_BREAKREASON", SqlDbType.VarChar); obj_cmd.Parameters["@EMPBENEFIT_BREAKREASON"].Value = model.empbenefit_breakreason;
                }
                obj_cmd.Parameters.Add("@EMPBENEFIT_CONDITIONPAY", SqlDbType.VarChar); obj_cmd.Parameters["@EMPBENEFIT_CONDITIONPAY"].Value = model.empbenefit_conditionpay;
                obj_cmd.Parameters.Add("@EMPBENEFIT_PAYFIRST", SqlDbType.VarChar); obj_cmd.Parameters["@EMPBENEFIT_PAYFIRST"].Value = model.empbenefit_payfirst;

                obj_cmd.Parameters.Add("@EMPBENEFIT_CAPITALAMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@EMPBENEFIT_CAPITALAMOUNT"].Value = model.empbenefit_capitalamount;
                obj_cmd.Parameters.Add("@EMPBENEFIT_PERIOD", SqlDbType.Decimal); obj_cmd.Parameters["@EMPBENEFIT_PERIOD"].Value = model.empbenefit_period;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@EMPBENEFIT_ID", SqlDbType.Int); obj_cmd.Parameters["@EMPBENEFIT_ID"].Value = model.empbenefit_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "EMPBNF006:" + ex.ToString();
            }

            return blnResult;
        }

        public List<cls_TRBenefit> getDataBatch(string com ,double amount , string code)
        {
            List<cls_TRBenefit> list_model = new List<cls_TRBenefit>();
            cls_TRBenefit model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("EMP_TR_BENEFIT.COMPANY_CODE");
                obj_str.Append(", EMP_TR_BENEFIT.WORKER_CODE");

                obj_str.Append(", EMP_TR_BENEFIT.EMPBENEFIT_ID");
                obj_str.Append(", EMP_TR_BENEFIT.ITEM_CODE");
                obj_str.Append(", EMP_TR_BENEFIT.EMPBENEFIT_AMOUNT");

                obj_str.Append(", INITIAL_NAME_TH + WORKER_FNAME_TH + ' ' + WORKER_LNAME_TH AS WORKER_DETAIL_TH");
                obj_str.Append(", INITIAL_NAME_EN + WORKER_FNAME_EN + ' ' + WORKER_LNAME_EN AS WORKER_DETAIL_EN");

                obj_str.Append(", ISNULL(EMP_TR_BENEFIT.MODIFIED_BY, EMP_TR_BENEFIT.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(EMP_TR_BENEFIT.MODIFIED_DATE, EMP_TR_BENEFIT.CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_TR_BENEFIT");
                obj_str.Append(" INNER JOIN EMP_MT_WORKER ON EMP_MT_WORKER.COMPANY_CODE=EMP_TR_BENEFIT.COMPANY_CODE AND EMP_MT_WORKER.WORKER_CODE=EMP_TR_BENEFIT.WORKER_CODE");
                obj_str.Append(" INNER JOIN EMP_MT_INITIAL ON EMP_MT_INITIAL.INITIAL_CODE=EMP_MT_WORKER.WORKER_INITIAL ");
                obj_str.Append(" WHERE 1=1");
                obj_str.Append(" AND EMP_TR_BENEFIT.COMPANY_CODE='" + com + "' ");

                if (!code.Equals(""))
                    obj_str.Append(" AND EMP_TR_BENEFIT.ITEM_CODE='" + code + "' ");
                if (!amount.Equals(""))
                    obj_str.Append(" AND EMP_TR_BENEFIT.EMPBENEFIT_AMOUNT='" + amount + "' ");

                obj_str.Append(" ORDER BY WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRBenefit();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.item_code = dr["ITEM_CODE"].ToString();
                    model.empbenefit_id = Convert.ToInt32(dr["EMPBENEFIT_ID"]);
                    model.empbenefit_amount = Convert.ToDouble(dr["EMPBENEFIT_AMOUNT"]);
                    //model.empbenefit_startdate = Convert.ToDateTime(dr["EMPBENEFIT_STARTDATE"]);
                    //model.empbenefit_enddate = Convert.ToDateTime(dr["EMPBENEFIT_ENDDATE"]);
                    //model.empbenefit_reason = dr["EMPBENEFIT_REASON"].ToString();
                    //model.empbenefit_note = dr["EMPBENEFIT_NOTE"].ToString();
                    //model.empbenefit_paytype = dr["EMPBENEFIT_PAYTYPE"].ToString();
                    //model.empbenefit_break = Convert.ToBoolean(dr["EMPBENEFIT_BREAK"]);
                    //model.empbenefit_breakreason = dr["EMPBENEFIT_BREAKREASON"].ToString();
                    //model.empbenefit_conditionpay = dr["EMPBENEFIT_CONDITIONPAY"].ToString();
                    //model.empbenefit_payfirst = dr["EMPBENEFIT_PAYFIRST"].ToString();

                    model.worker_detail_th = dr["WORKER_DETAIL_TH"].ToString();
                    model.worker_detail_en = dr["WORKER_DETAIL_EN"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "EMPBNF001:" + ex.ToString();
            }

            return list_model;
        }

        public bool insertlist(List<cls_TRBenefit> list_model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_TR_BENEFIT");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");

                obj_str.Append(", EMPBENEFIT_ID ");
                obj_str.Append(", ITEM_CODE ");
                obj_str.Append(", EMPBENEFIT_AMOUNT ");
                obj_str.Append(", EMPBENEFIT_STARTDATE ");
                obj_str.Append(", EMPBENEFIT_ENDDATE ");
                obj_str.Append(", EMPBENEFIT_REASON ");
                obj_str.Append(", EMPBENEFIT_NOTE ");
                obj_str.Append(", EMPBENEFIT_PAYTYPE ");
                obj_str.Append(", EMPBENEFIT_BREAK ");

                if (list_model[0].empbenefit_break)
                {
                    obj_str.Append(", EMPBENEFIT_BREAKREASON ");
                }
                obj_str.Append(", EMPBENEFIT_CONDITIONPAY ");
                obj_str.Append(", EMPBENEFIT_PAYFIRST ");
                
                obj_str.Append(", EMPBENEFIT_CAPITALAMOUNT ");
                obj_str.Append(", EMPBENEFIT_PERIOD ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");

                obj_str.Append(", @EMPBENEFIT_ID ");
                obj_str.Append(", @ITEM_CODE ");
                obj_str.Append(", @EMPBENEFIT_AMOUNT ");
                obj_str.Append(", @EMPBENEFIT_STARTDATE ");
                obj_str.Append(", @EMPBENEFIT_ENDDATE ");
                obj_str.Append(", @EMPBENEFIT_REASON ");
                obj_str.Append(", @EMPBENEFIT_NOTE ");
                obj_str.Append(", @EMPBENEFIT_PAYTYPE ");
                obj_str.Append(", @EMPBENEFIT_BREAK ");
                if (list_model[0].empbenefit_break)
                {
                    obj_str.Append(", @EMPBENEFIT_BREAKREASON ");
                }
                obj_str.Append(", @EMPBENEFIT_CONDITIONPAY ");
                obj_str.Append(", @EMPBENEFIT_PAYFIRST ");
                
                obj_str.Append(", @EMPBENEFIT_CAPITALAMOUNT ");
                obj_str.Append(", @EMPBENEFIT_PERIOD ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                //-- Step 1 delete data old
                string strWorkerID = "";
                foreach (cls_TRBenefit model in list_model)
                {
                    strWorkerID += "'" + model.worker_code + "',";
                }
                if (strWorkerID.Length > 0)
                    strWorkerID = strWorkerID.Substring(0, strWorkerID.Length - 1);
                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM EMP_TR_BENEFIT");
                obj_str2.Append(" WHERE 1=1 ");
                obj_str2.Append(" AND COMPANY_CODE='" + list_model[0].company_code + "'");
                obj_str2.Append(" AND WORKER_CODE IN (" + strWorkerID + ")");
                obj_str2.Append(" AND ITEM_CODE='" + list_model[0].item_code + "'");
                obj_str2.Append(" AND EMPBENEFIT_STARTDATE='" + list_model[0].empbenefit_startdate.ToString("yyyy-MM-ddTHH:mm:ss") + "'");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());

                if (blnResult)
                {
                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar);

                    obj_cmd.Parameters.Add("@EMPBENEFIT_ID", SqlDbType.Int);
                    obj_cmd.Parameters.Add("@ITEM_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPBENEFIT_AMOUNT", SqlDbType.Decimal);
                    obj_cmd.Parameters.Add("@EMPBENEFIT_STARTDATE", SqlDbType.DateTime);
                    obj_cmd.Parameters.Add("@EMPBENEFIT_ENDDATE", SqlDbType.DateTime);
                    obj_cmd.Parameters.Add("@EMPBENEFIT_REASON", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPBENEFIT_NOTE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPBENEFIT_PAYTYPE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPBENEFIT_BREAK", SqlDbType.Bit);
                    if (list_model[0].empbenefit_break)
                    {
                        obj_cmd.Parameters.Add("@EMPBENEFIT_BREAKREASON", SqlDbType.VarChar);
                    }
                    obj_cmd.Parameters.Add("@EMPBENEFIT_CONDITIONPAY", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPBENEFIT_PAYFIRST", SqlDbType.VarChar);
                    
                    obj_cmd.Parameters.Add("@EMPBENEFIT_CAPITALAMOUNT", SqlDbType.Decimal);
                    obj_cmd.Parameters.Add("@EMPBENEFIT_PERIOD", SqlDbType.Decimal);

                    obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime);

                    foreach (cls_TRBenefit model in list_model)
                    {
                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                        obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                        obj_cmd.Parameters["@EMPBENEFIT_ID"].Value = this.getNextID();
                        obj_cmd.Parameters["@ITEM_CODE"].Value = model.item_code;
                        obj_cmd.Parameters["@EMPBENEFIT_AMOUNT"].Value = model.empbenefit_amount;
                        obj_cmd.Parameters["@EMPBENEFIT_STARTDATE"].Value = model.empbenefit_startdate;
                        obj_cmd.Parameters["@EMPBENEFIT_ENDDATE"].Value = model.empbenefit_enddate;
                        obj_cmd.Parameters["@EMPBENEFIT_REASON"].Value = model.empbenefit_reason;
                        obj_cmd.Parameters["@EMPBENEFIT_NOTE"].Value = model.empbenefit_note;
                        obj_cmd.Parameters["@EMPBENEFIT_PAYTYPE"].Value = model.empbenefit_paytype;
                        obj_cmd.Parameters["@EMPBENEFIT_BREAK"].Value = model.empbenefit_break;
                        if (model.empbenefit_break)
                        {
                            obj_cmd.Parameters["@EMPBENEFIT_BREAKREASON"].Value = model.empbenefit_breakreason;
                        }
                        obj_cmd.Parameters["@EMPBENEFIT_CONDITIONPAY"].Value = model.empbenefit_conditionpay;
                        obj_cmd.Parameters["@EMPBENEFIT_PAYFIRST"].Value = model.empbenefit_payfirst;
                        
                        obj_cmd.Parameters["@EMPBENEFIT_CAPITALAMOUNT"].Value = model.empbenefit_capitalamount;
                        obj_cmd.Parameters["@EMPBENEFIT_PERIOD"].Value = model.empbenefit_period;

                        obj_cmd.Parameters["@CREATED_BY"].Value = model.created_by;
                        obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                        obj_cmd.ExecuteNonQuery();
                    }

                    blnResult = obj_conn.doCommit();

                    if (!blnResult)
                        obj_conn.doRollback();
                    obj_conn.doClose();

                }
                else
                {
                    obj_conn.doRollback();
                    obj_conn.doClose();
                }
            }
            catch (Exception ex)
            {
                Message = "EMPPVD099:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
