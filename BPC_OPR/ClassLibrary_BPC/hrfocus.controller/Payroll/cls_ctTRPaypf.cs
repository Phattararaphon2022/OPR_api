using ClassLibrary_BPC.hrfocus.model.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller.Payroll
{
   public class cls_ctTRPaypf
   {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRPaypf() { }

        public string getMessage() { return this.Message.Replace("PAY_TR_PAYPF", "").Replace("cls_ctTRPaypf", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }
        private List<cls_TRPaypf> getData(string language, string condition)
        {
            List<cls_TRPaypf> list_model = new List<cls_TRPaypf>();
            cls_TRPaypf model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PAY_TR_PAYPF.COMPANY_CODE");
                obj_str.Append(", PAY_TR_PAYPF.WORKER_CODE");
                obj_str.Append(", PAY_TR_PAYPF.PROVIDENT_CODE");
                obj_str.Append(", PAYPF_DATE");

                obj_str.Append(", PAYPF_EMP_RATE");
                obj_str.Append(", PAYPF_EMP_AMOUNT");
                obj_str.Append(", PAYPF_COM_RATE");
                obj_str.Append(", PAYPF_COM_AMOUNT");


                obj_str.Append(", ISNULL(PAY_TR_PAYPF.MODIFIED_BY, PAY_TR_PAYPF.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(PAY_TR_PAYPF.MODIFIED_DATE, PAY_TR_PAYPF.CREATED_DATE) AS MODIFIED_DATE");

                if (language.Equals("TH"))
                {
                    obj_str.Append(", INITIAL_NAME_TH + WORKER_FNAME_TH + ' ' + WORKER_LNAME_TH AS WORKER_DETAIL");
                }
                else
                {
                    obj_str.Append(", INITIAL_NAME_EN + WORKER_FNAME_EN + ' ' + WORKER_LNAME_EN AS WORKER_DETAIL");
                }

                obj_str.Append(" FROM PAY_TR_PAYPF");
                obj_str.Append(" INNER JOIN EMP_MT_WORKER ON EMP_MT_WORKER.COMPANY_CODE=PAY_TR_PAYPF.COMPANY_CODE AND EMP_MT_WORKER.WORKER_CODE=PAY_TR_PAYPF.WORKER_CODE");
                obj_str.Append(" INNER JOIN EMP_MT_INITIAL ON EMP_MT_INITIAL.INITIAL_CODE=EMP_MT_WORKER.WORKER_INITIAL ");

                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PAY_TR_PAYPF.COMPANY_CODE, PAYPF_DATE, PAY_TR_PAYPF.WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRPaypf();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                    model.worker_code = Convert.ToString(dr["WORKER_CODE"]);
                    model.provident_code = Convert.ToString(dr["PROVIDENT_CODE"]);
                    model.paypf_date = Convert.ToDateTime(dr["PAYPF_DATE"]);


                    model.paypf_emp_rate = Convert.ToDouble(dr["PAYPF_EMP_RATE"]);
                    model.paypf_emp_amount = Convert.ToDouble(dr["PAYPF_EMP_AMOUNT"]);
                    model.paypf_com_rate = Convert.ToDouble(dr["PAYPF_COM_RATE"]);
                    model.paypf_com_amount = Convert.ToDouble(dr["PAYPF_COM_AMOUNT"]);

                    model.worker_detail = Convert.ToString(dr["WORKER_DETAIL"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "PAYPF001:" + ex.ToString();
            }

            return list_model;
        }


        public List<cls_TRPaypf> getDataByFillter(string language, string com, DateTime datefrom, DateTime dateto, string emp)
        {
            string strCondition = "";

            strCondition += " AND PAY_TR_PAYPF.COMPANY_CODE='" + com + "'";
            strCondition += " AND (PAY_TR_PAYPF.PAYPF_DATE BETWEEN '" + datefrom.ToString("MM/dd/yyyy") + "' AND '" + dateto.ToString("MM/dd/yyyy") + "')";

            if (!emp.Equals(""))
                strCondition += " AND PAY_TR_PAYPF.WORKER_CODE='" + emp + "'";

            return this.getData(language, strCondition);
        }


        public List<cls_TRPaypf> getDataMultipleEmp(string language, string com, DateTime datefrom, DateTime dateto, string emp)
        {
            string strCondition = "";

            strCondition += " AND PAY_TR_PAYPF.COMPANY_CODE='" + com + "'";
            strCondition += " AND (PAY_TR_PAYPF.PAYPF_DATE BETWEEN '" + datefrom.ToString("MM/dd/yyyy") + "' AND '" + dateto.ToString("MM/dd/yyyy") + "')";
            strCondition += " AND PAY_TR_PAYPF.WORKER_CODE IN (" + emp + ")";

            return this.getData(language, strCondition);
        }

    }
}
