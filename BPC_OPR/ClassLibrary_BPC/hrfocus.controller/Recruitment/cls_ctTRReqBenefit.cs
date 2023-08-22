using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRReqBenefit
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRReqBenefit() { }

        public string getMessage() { return this.Message.Replace("REQ_TR_BENEFIT", "").Replace("cls_ctTRReqBenefit", "").Replace("line", ""); }

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

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM REQ_TR_BENEFIT");
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
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "REQBNF001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRBenefit> getDataByFillter(string com, string emp, string code)
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
                obj_str.Append(" FROM REQ_TR_BENEFIT");
                obj_str.Append(" ORDER BY EMPBENEFIT_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "REQBNF002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp, string id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT EMPBENEFIT_ID");
                obj_str.Append(" FROM REQ_TR_BENEFIT");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                if (!id.ToString().Equals(""))
                {
                    obj_str.Append(" AND EMPBENEFIT_ID='" + id + "' ");
                }

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "REQBNF003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM REQ_TR_BENEFIT");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "REQBNF004:" + ex.ToString();
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
                if (this.checkDataOld(model.company_code, model.worker_code, model.empbenefit_id.ToString()))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO REQ_TR_BENEFIT");
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

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                strResult = model.empbenefit_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "REQBNF005:" + ex.ToString();
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
                obj_str.Append("UPDATE REQ_TR_BENEFIT SET ");

                obj_str.Append(" ITEM_CODE=@ITEM_CODE ");
                obj_str.Append(", EMPBENEFIT_AMOUNT=@EMPBENEFIT_AMOUNT ");
                obj_str.Append(", EMPBENEFIT_STARTDATE=@EMPBENEFIT_STARTDATE ");
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

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE "); ;

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" AND EMPBENEFIT_ID=@EMPBENEFIT_ID ");

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
                Message = "REQBNF006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
