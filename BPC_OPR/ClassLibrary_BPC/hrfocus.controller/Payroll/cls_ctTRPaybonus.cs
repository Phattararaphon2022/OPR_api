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
    public class cls_ctTRPaybonus
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRPaybonus() { }

        public string getMessage() { return this.Message.Replace("PAY_TR_PAYBONUS", "").Replace("cls_ctTRPaybonus", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }
        private List<cls_TRPaybonus> getData(string language, string condition)
        {
            List<cls_TRPaybonus> list_model = new List<cls_TRPaybonus>();
            cls_TRPaybonus model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();


                obj_str.Append("SELECT ");

                obj_str.Append("PAY_TR_PAYBONUS.COMPANY_CODE");
                obj_str.Append(", PAY_TR_PAYBONUS.WORKER_CODE");

                obj_str.Append(", PAYBONUS_DATE");
                obj_str.Append(", PAYBONUS_AMOUNT");
                obj_str.Append(", PAYBONUS_QUANTITY");
                obj_str.Append(", PAYBONUS_RATE");
                obj_str.Append(", PAYBONUS_TAX");
                obj_str.Append(", ISNULL(PAYBONUS_NOTE, '') AS PAYBONUS_NOTE");

                obj_str.Append(", ISNULL(PAY_TR_PAYBONUS.MODIFIED_BY, PAY_TR_PAYBONUS.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(PAY_TR_PAYBONUS.MODIFIED_DATE, PAY_TR_PAYBONUS.CREATED_DATE) AS MODIFIED_DATE");



                if (language.Equals("TH"))
                {
                    obj_str.Append(", INITIAL_NAME_TH + WORKER_FNAME_TH + ' ' + WORKER_LNAME_TH AS WORKER_DETAIL");
                }
                else
                {
                    obj_str.Append(", INITIAL_NAME_EN + WORKER_FNAME_EN + ' ' + WORKER_LNAME_EN AS WORKER_DETAIL");
                }


                obj_str.Append(" FROM PAY_TR_PAYBONUS");

                obj_str.Append(" INNER JOIN EMP_MT_WORKER ON EMP_MT_WORKER.COMPANY_CODE=PAY_TR_PAYBONUS.COMPANY_CODE AND EMP_MT_WORKER.WORKER_CODE=PAY_TR_PAYBONUS.WORKER_CODE");
                obj_str.Append(" INNER JOIN EMP_MT_Initial ON EMP_MT_Initial.INITIAL_CODE=EMP_MT_WORKER.WORKER_INITIAL ");


                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PAY_TR_PAYBONUS.COMPANY_CODE, PAY_TR_PAYBONUS.WORKER_CODE, PAYBONUS_DATE DESC");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRPaybonus();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                    model.worker_code = Convert.ToString(dr["WORKER_CODE"]);
                    model.paybonus_date = Convert.ToDateTime(dr["PAYBONUS_DATE"]);
                    model.paybonus_amount = Convert.ToDouble(dr["PAYBONUS_AMOUNT"]);
                    model.paybonus_quantity = Convert.ToDouble(dr["PAYBONUS_QUANTITY"]);
                    model.paybonus_rate = Convert.ToDouble(dr["PAYBONUS_RATE"]);
                    model.paybonus_tax = Convert.ToDouble(dr["PAYBONUS_TAX"]);

                    model.paybonus_note = Convert.ToString(dr["PAYBONUS_NOTE"]);

                    model.worker_detail = Convert.ToString(dr["WORKER_DETAIL"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "PAYBANK001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRPaybonus> getDataByFillter(string language, string com, DateTime date, string access_emp)
        {
            string strCondition = "";

            strCondition += " AND PAY_TR_PAYBONUS.COMPANY_CODE='" + com + "'";
            strCondition += " AND PAY_TR_PAYBONUS.PAYBONUS_DATE='" + date.ToString("MM/dd/yyyy") + "'";

            if (!access_emp.Equals(""))
                strCondition += " AND PAY_TR_PAYBONUS.WORKER_CODE IN (" + access_emp + ") ";

            return this.getData(language, strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(POSITION_ID, 1) ");
                obj_str.Append(" FROM EMP_MT_POSITION");
                obj_str.Append(" ORDER BY POSITION_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "PST002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp, DateTime date)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM PAY_TR_PAYBONUS");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                obj_str.Append(" AND PAYBONUS_DATE='" + date.ToString("MM/dd/yyyy") + "' ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "PST003:" + ex.ToString();
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

                obj_str.Append(" DELETE FROM PAY_TR_PAYBONUS");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                obj_str.Append(" AND PAYBONUS_DATE='" + date.ToString("MM/dd/yyyy") + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "PST004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRPaybonus model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.worker_code, model.paybonus_date))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PAY_TR_PAYBONUS");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", ITEM_CODE ");
                obj_str.Append(", PAYBONUS_DATE ");
                obj_str.Append(", PAYBONUS_AMOUNT ");
                obj_str.Append(", PAYBONUS_QUANTITY ");
                obj_str.Append(", PAYBONUS_RATE ");
                obj_str.Append(", PAYBONUS_TAX ");
                obj_str.Append(", PAYBONUS_NOTE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @ITEM_CODE ");
                obj_str.Append(", @PAYBONUS_DATE ");
                obj_str.Append(", @PAYBONUS_AMOUNT ");
                obj_str.Append(", @PAYBONUS_QUANTITY ");
                obj_str.Append(", @PAYBONUS_RATE ");
                obj_str.Append(", @PAYBONUS_TAX ");
                obj_str.Append(", @PAYBONUS_NOTE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@PAYBONUS_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@PAYBONUS_DATE"].Value = model.paybonus_date;
                obj_cmd.Parameters.Add("@PAYBONUS_AMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@PAYBONUS_AMOUNT"].Value = model.paybonus_amount;
                obj_cmd.Parameters.Add("@PAYBONUS_QUANTITY", SqlDbType.Decimal); obj_cmd.Parameters["@PAYBONUS_QUANTITY"].Value = model.paybonus_quantity;
                obj_cmd.Parameters.Add("@PAYBONUS_RATE", SqlDbType.VarChar); obj_cmd.Parameters["@PAYBONUS_RATE"].Value = model.paybonus_rate;
                obj_cmd.Parameters.Add("@PAYBONUS_TAX", SqlDbType.VarChar); obj_cmd.Parameters["@PAYBONUS_TAX"].Value = model.paybonus_tax;
                obj_cmd.Parameters.Add("@PAYBONUS_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@PAYBONUS_NOTE"].Value = model.paybonus_note;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;
                obj_cmd.ExecuteNonQuery();

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "PST005:" + ex.ToString();
             }

            return blnResult;
        }

        public bool update(cls_TRPaybonus model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE PAY_TR_PAYBONUS SET ");

                obj_str.Append("PAYBONUS_AMOUNT=@PAYBONUS_AMOUNT ");
                obj_str.Append(", PAYBONUS_QUANTITY=@PAYBONUS_QUANTITY ");
                obj_str.Append(", PAYBONUS_RATE=@PAYBONUS_RATE ");
                obj_str.Append(", PAYBONUS_TAX=@PAYBONUS_TAX ");
                obj_str.Append(", PAYBONUS_NOTE=@PAYBONUS_NOTE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" AND PAYBONUS_DATE=@PAYBONUS_DATE ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PAYBONUS_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@PAYBONUS_DATE"].Value = model.paybonus_date;
                obj_cmd.Parameters.Add("@PAYBONUS_AMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@PAYBONUS_AMOUNT"].Value = model.paybonus_amount;
                obj_cmd.Parameters.Add("@PAYBONUS_QUANTITY", SqlDbType.Decimal); obj_cmd.Parameters["@PAYBONUS_QUANTITY"].Value = model.paybonus_quantity;
                obj_cmd.Parameters.Add("@PAYBONUS_RATE", SqlDbType.VarChar); obj_cmd.Parameters["@PAYBONUS_RATE"].Value = model.paybonus_rate;
                obj_cmd.Parameters.Add("@PAYBONUS_TAX", SqlDbType.VarChar); obj_cmd.Parameters["@PAYBONUS_TAX"].Value = model.paybonus_tax;
                obj_cmd.Parameters.Add("@PAYBONUS_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@PAYBONUS_NOTE"].Value = model.paybonus_note;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@PAYBONUS_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@PAYBONUS_DATE"].Value = model.paybonus_date;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "PST006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}