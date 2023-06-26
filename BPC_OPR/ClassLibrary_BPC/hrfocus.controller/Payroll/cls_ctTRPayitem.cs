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
  public class cls_ctTRPayitem
   {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRPayitem() { }

        public string getMessage() { return this.Message.Replace("PAY_TR_PAYITEM", "").Replace("cls_ctTRPayitem", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }
        private List<cls_TRPayitem> getData(string language, string condition)
        {
            List<cls_TRPayitem> list_model = new List<cls_TRPayitem>();
            cls_TRPayitem model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PAY_TR_PAYITEM.COMPANY_CODE");
                obj_str.Append(", PAY_TR_PAYITEM.WORKER_CODE");
                obj_str.Append(", PAY_TR_PAYITEM.ITEM_CODE");
                obj_str.Append(", PAYITEM_DATE");
                obj_str.Append(", PAYITEM_AMOUNT");
                obj_str.Append(", PAYITEM_QUANTITY");
                obj_str.Append(", PAYITEM_PAYTYPE");
                obj_str.Append(", ISNULL(PAYITEM_NOTE, '') AS PAYITEM_NOTE");

                obj_str.Append(", ISNULL(PAY_TR_PAYITEM.MODIFIED_BY, PAY_TR_PAYITEM.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(PAY_TR_PAYITEM.MODIFIED_DATE, PAY_TR_PAYITEM.CREATED_DATE) AS MODIFIED_DATE");


                obj_str.Append(", ITEM_TYPE");

                if (language.Equals("TH"))
                {
                    obj_str.Append(", ITEM_NAME_TH AS ITEM_DETAIL");
                    obj_str.Append(", INITIAL_NAME_TH + WORKER_FNAME_TH + ' ' + WORKER_LNAME_TH AS WORKER_DETAIL");
                }
                else
                {
                    obj_str.Append(", ITEM_NAME_EN AS ITEM_DETAIL");
                    obj_str.Append(", INITIAL_NAME_EN + WORKER_FNAME_EN + ' ' + WORKER_LNAME_EN AS WORKER_DETAIL");
                }


                obj_str.Append(" FROM PAY_TR_PAYITEM");

                obj_str.Append(" INNER JOIN EMP_MT_WORKER ON EMP_MT_WORKER.COMPANY_CODE=PAY_TR_PAYITEM.COMPANY_CODE AND EMP_MT_WORKER.WORKER_CODE=PAY_TR_PAYITEM.WORKER_CODE");
                obj_str.Append(" INNER JOIN EMP_MT_INITIAL ON EMP_MT_INITIAL.INITIAL_CODE=EMP_MT_WORKER.WORKER_INITIAL ");

                obj_str.Append(" INNER JOIN PAY_MT_ITEM ON PAY_MT_ITEM.COMPANY_CODE=PAY_TR_PAYITEM.COMPANY_CODE AND PAY_MT_ITEM.ITEM_CODE=PAY_TR_PAYITEM.ITEM_CODE");


                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PAY_TR_PAYITEM.COMPANY_CODE, PAY_TR_PAYITEM.WORKER_CODE, PAYITEM_DATE DESC, ITEM_TYPE DESC, PAY_TR_PAYITEM.ITEM_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRPayitem();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                    model.worker_code = Convert.ToString(dr["WORKER_CODE"]);
                    model.item_code = Convert.ToString(dr["ITEM_CODE"]);
                    model.payitem_date = Convert.ToDateTime(dr["PAYITEM_DATE"]);
                    model.payitem_amount = Convert.ToDouble(dr["PAYITEM_AMOUNT"]);
                    model.payitem_quantity = Convert.ToDouble(dr["PAYITEM_QUANTITY"]);
                    model.payitem_paytype = Convert.ToString(dr["PAYITEM_PAYTYPE"]);

                    model.payitem_note = Convert.ToString(dr["PAYITEM_NOTE"]);

                    model.item_detail = Convert.ToString(dr["ITEM_DETAIL"]);
                    model.item_type = Convert.ToString(dr["ITEM_TYPE"]);
                    model.worker_detail = Convert.ToString(dr["WORKER_DETAIL"]);


                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);


                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "TRIT001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRPayitem> getDataByFillter(string language, string com, string emp,  string item_type, string item)
        {
            string strCondition = "";

            strCondition += " AND PAY_TR_PAYITEM.COMPANY_CODE='" + com + "'";
            //strCondition += " AND PAY_TR_PAYITEM.PAYITEM_DATE='" + date.ToString("MM/dd/yyyy") + "'";

            if (!emp.Equals(""))
                strCondition += " AND PAY_TR_PAYITEM.WORKER_CODE='" + emp + "'";

            if (!item.Equals(""))
                strCondition += " AND PAY_TR_PAYITEM.ITEM_CODE='" + item + "'";

            if (!item_type.Equals(""))
            {
                strCondition += " AND PAY_TR_PAYITEM.ITEM_CODE IN (SELECT ITEM_CODE FROM PAY_MT_ITEM WHERE COMPANY_CODE='" + com + "' AND ITEM_CODE='" + item_type + "')";
            }

            return this.getData(language, strCondition);
        }
        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" FROM PAY_TR_PAYPOLITEM");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "TRIT002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp, string item, DateTime date)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ITEM_CODE");
                obj_str.Append(" FROM PAY_TR_PAYITEM");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                obj_str.Append(" AND ITEM_CODE='" + item + "' ");
                obj_str.Append(" AND PAYITEM_DATE='" + date.ToString("MM/dd/yyyy") + "' ");


                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "TRIT003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string emp, string item, DateTime date)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append(" DELETE FROM PAY_TR_PAYITEM");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                obj_str.Append(" AND ITEM_CODE='" + item + "' ");
                obj_str.Append(" AND PAYITEM_DATE='" + date.ToString("MM/dd/yyyy") + "' ");
                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "TRIT004:" + ex.ToString();
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

                obj_str.Append(" DELETE FROM PAY_TR_PAYITEM");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());
            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Payitem.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRPayitem model)

        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.worker_code, model.item_code, model.payitem_date))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PAY_TR_PAYITEM");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", ITEM_CODE ");
                obj_str.Append(", PAYITEM_DATE ");
                obj_str.Append(", PAYITEM_AMOUNT ");
                obj_str.Append(", PAYITEM_QUANTITY ");
                obj_str.Append(", PAYITEM_PAYTYPE ");
                obj_str.Append(", PAYITEM_NOTE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @ITEM_CODE ");
                obj_str.Append(", @PAYITEM_DATE ");
                obj_str.Append(", @PAYITEM_AMOUNT ");
                obj_str.Append(", @PAYITEM_QUANTITY ");
                obj_str.Append(", @PAYITEM_PAYTYPE ");
                obj_str.Append(", @PAYITEM_NOTE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");             


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@ITEM_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_CODE"].Value = model.item_code;
                obj_cmd.Parameters.Add("@PAYITEM_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@PAYITEM_DATE"].Value = model.payitem_date;
                obj_cmd.Parameters.Add("@PAYITEM_AMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@PAYITEM_AMOUNT"].Value = model.payitem_amount;
                obj_cmd.Parameters.Add("@PAYITEM_QUANTITY", SqlDbType.Decimal); obj_cmd.Parameters["@PAYITEM_QUANTITY"].Value = model.payitem_quantity;
                obj_cmd.Parameters.Add("@PAYITEM_PAYTYPE", SqlDbType.VarChar); obj_cmd.Parameters["@PAYITEM_PAYTYPE"].Value = model.payitem_paytype;
                obj_cmd.Parameters.Add("@PAYITEM_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@PAYITEM_NOTE"].Value = model.payitem_note;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "TRIT005:" + ex.ToString();
            }

            return blnResult;
        }

        public bool update(cls_TRPayitem model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PAY_TR_PAYITEM SET ");

                obj_str.Append("PAYITEM_AMOUNT=@PAYITEM_AMOUNT ");
                obj_str.Append(", PAYITEM_QUANTITY=@PAYITEM_QUANTITY ");
                obj_str.Append(", PAYITEM_PAYTYPE=@PAYITEM_PAYTYPE ");
                obj_str.Append(", PAYITEM_NOTE=@PAYITEM_NOTE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" AND ITEM_CODE=@ITEM_CODE ");
                obj_str.Append(" AND PAYITEM_DATE=@PAYITEM_DATE ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                obj_cmd.Parameters.Add("@PAYITEM_AMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@PAYITEM_AMOUNT"].Value = model.payitem_amount;
                obj_cmd.Parameters.Add("@PAYITEM_QUANTITY", SqlDbType.Decimal); obj_cmd.Parameters["@PAYITEM_QUANTITY"].Value = model.payitem_quantity;
                obj_cmd.Parameters.Add("@PAYITEM_PAYTYPE", SqlDbType.VarChar); obj_cmd.Parameters["@PAYITEM_PAYTYPE"].Value = model.payitem_paytype;
                obj_cmd.Parameters.Add("@PAYITEM_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@PAYITEM_NOTE"].Value = model.payitem_note;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@ITEM_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_CODE"].Value = model.item_code;
                obj_cmd.Parameters.Add("@PAYITEM_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@PAYITEM_DATE"].Value = model.payitem_date;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "TRIT006:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insertlist(string com, string emp, DateTime date, List<cls_TRPayitem> list_model)
        {
            bool blnResult = false;
            cls_ctConnection obj_conn = new cls_ctConnection();
            try
            {

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PAY_TR_PAYITEM");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", ITEM_CODE ");
                obj_str.Append(", PAYITEM_DATE ");
                obj_str.Append(", PAYITEM_AMOUNT ");
                obj_str.Append(", PAYITEM_QUANTITY ");
                obj_str.Append(", PAYITEM_PAYTYPE ");
                obj_str.Append(", PAYITEM_NOTE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @ITEM_CODE ");
                obj_str.Append(", @PAYITEM_DATE ");
                obj_str.Append(", @PAYITEM_AMOUNT ");
                obj_str.Append(", @PAYITEM_QUANTITY ");
                obj_str.Append(", @PAYITEM_PAYTYPE ");
                obj_str.Append(", @PAYITEM_NOTE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM PAY_TR_PAYITEM");
                obj_str2.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str2.Append(" AND WORKER_CODE='" + emp + "'");
                obj_str2.Append(" AND PAYITEM_DATE='" + date.ToString("MM/dd/yyyy") + "'");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());

                if (blnResult)
                {
                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@ITEM_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@PAYITEM_DATE", SqlDbType.DateTime);
                    obj_cmd.Parameters.Add("@PAYITEM_AMOUNT", SqlDbType.Decimal);
                    obj_cmd.Parameters.Add("@PAYITEM_QUANTITY", SqlDbType.Decimal);
                    obj_cmd.Parameters.Add("@PAYITEM_PAYTYPE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@PAYITEM_NOTE", SqlDbType.VarChar);

                    obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime);
                    obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit);

                    foreach (cls_TRPayitem model in list_model)
                    {

                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                        obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                        obj_cmd.Parameters["@ITEM_CODE"].Value = model.item_code;
                        obj_cmd.Parameters["@PAYITEM_DATE"].Value = model.payitem_date;
                        obj_cmd.Parameters["@PAYITEM_AMOUNT"].Value = model.payitem_amount;
                        obj_cmd.Parameters["@PAYITEM_QUANTITY"].Value = model.payitem_quantity;
                        obj_cmd.Parameters["@PAYITEM_PAYTYPE"].Value = model.payitem_paytype;
                        obj_cmd.Parameters["@PAYITEM_NOTE"].Value = model.payitem_note;

                        obj_cmd.Parameters["@CREATED_BY"].Value = model.created_by;
                        obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                        obj_cmd.Parameters["@FLAG"].Value = false;

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
                Message = "TRIT007:" + ex.ToString();
            }

            return blnResult;
        }
   }
}
