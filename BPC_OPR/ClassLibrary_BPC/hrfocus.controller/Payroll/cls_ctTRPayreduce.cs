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
    public class cls_ctTRPayreduce
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRPayreduce() { }

        public string getMessage() { return this.Message.Replace("PAY_TR_PAYREDUCE", "").Replace("cls_ctTRPayreduce", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRPayreduce> getData(string condition)
        {
            List<cls_TRPayreduce> list_model = new List<cls_TRPayreduce>();
            cls_TRPayreduce model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");
                obj_str.Append(", PAYREDUCE_PAYDATE");
                obj_str.Append(", PAY_TR_PAYREDUCE.REDUCE_CODE AS REDUCE_CODE");
                obj_str.Append(", PAYREDUCE_AMOUNT");

                obj_str.Append(", ISNULL(REDUCE_NAME_TH, '') AS REDUCE_NAME_TH");
                obj_str.Append(", ISNULL(REDUCE_NAME_EN, '') AS REDUCE_NAME_EN");

                obj_str.Append(" FROM PAY_TR_PAYREDUCE");
                obj_str.Append(" LEFT JOIN SYS_MT_REDUCE ON PAY_TR_PAYREDUCE.REDUCE_CODE = SYS_MT_REDUCE.REDUCE_CODE");

                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, WORKER_CODE, PAYREDUCE_PAYDATE,PAY_TR_PAYREDUCE.REDUCE_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRPayreduce();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.payreduce_paydate = Convert.ToDateTime(dr["PAYREDUCE_PAYDATE"]);
                    model.reduce_code = dr["REDUCE_CODE"].ToString();
                    model.payreduce_amount = Convert.ToDouble(dr["PAYREDUCE_AMOUNT"]);

                    model.reduce_name_th = dr["REDUCE_NAME_TH"].ToString();
                    model.reduce_name_en = dr["REDUCE_NAME_EN"].ToString();

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Comcard.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRPayreduce> getDataByFillter(string com, string worker, DateTime paydate)
        {
            string strCondition = " AND COMPANY_CODE='" + com + "'";

            if (!worker.Equals(""))
                strCondition += " AND WORKER_CODE='" + worker + "'";

            strCondition += " AND PAYREDUCE_PAYDATE ='" + paydate.ToString("MM/dd/yyyy") + "'";

            return this.getData(strCondition);
        }
    }
}
