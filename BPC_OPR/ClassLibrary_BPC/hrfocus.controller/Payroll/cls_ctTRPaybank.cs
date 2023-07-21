using ClassLibrary_BPC.hrfocus.model.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller.Payroll
{
  public  class cls_ctTRPaybank
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRPaybank() { }

        public string getMessage() { return this.Message.Replace("PAY_TR_PAYBANK", "").Replace("cls_ctTRPaybank", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }
        private List<cls_TRPaybank> getData(string condition)
        {
            List<cls_TRPaybank> list_model = new List<cls_TRPaybank>();
            cls_TRPaybank model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", PAYBANK_BANKCODE ");
                obj_str.Append(", PAYBANK_BANKACCOUNT ");
                obj_str.Append(", PAYBANK_AMOUNT");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PAY_TR_PAYBANK");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append("ORDER BY COMPANY_CODE,PAYBANK_BANKACCOUNT , PAYBANK_BANKCODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRPaybank();
                    model.paybank_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.paybank_bankcode = dr["PAYBANK_BANKCODE"].ToString();
                    model.paybank_bankaccount = dr["PAYBANK_BANKACCOUNT"].ToString();
                    model.paybank_bankamount = dr["PAYBANK_AMOUNT"].ToString();

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

        public List<cls_TRPaybank> getDataByFillter(string com, string worker)
        {

            string strCondition = " AND COMPANY_CODE='" + com + "'";
            strCondition += " AND WORKER_CODE IN (" + worker + ") ";

            return this.getData(strCondition);
        }

    }
}
