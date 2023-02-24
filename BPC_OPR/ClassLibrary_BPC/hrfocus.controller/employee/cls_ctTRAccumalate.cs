using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRAccumalate
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRAccumalate() { }

        public string getMessage() { return this.Message.Replace("EMP_TR_ACCUMALATE", "").Replace("cls_ctTRAccumalate", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRAccumalate> getData(string condition)
        {
            List<cls_TRAccumalate> list_model = new List<cls_TRAccumalate>();
            cls_TRAccumalate model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");

                obj_str.Append(", PERIODYEAR");
                obj_str.Append(", PERIODID");
                obj_str.Append(", PAYDATE");
                obj_str.Append(", INCOMEFIX");
                obj_str.Append(", INCOMEVAR");
                obj_str.Append(", TAXFIX");
                obj_str.Append(", TAXVAR");
                obj_str.Append(", INCOMEFORTYONE");
                obj_str.Append(", INCOMEFORTYONEPERTHREE");
                obj_str.Append(", INCOMEFORTYONETWO");
                obj_str.Append(", INCOMEFORTYTWOIN");
                obj_str.Append(", INCOMEFORTYTWOOUT");
                obj_str.Append(", TAXFORTYONE");
                obj_str.Append(", TAXFORTYONEPERTHREE");
                obj_str.Append(", TAXFORTYONETWO");
                obj_str.Append(", TAXFORTYTWOIN");
                obj_str.Append(", TAXFORTYTWOOUT");
                obj_str.Append(", SSOACC_WORKER");
                obj_str.Append(", SSOACC_COMPANY");
                obj_str.Append(", PFACC_WORKER");
                obj_str.Append(", PFACC_COMPANY");
                obj_str.Append(", ALLWANCEINCTAX");
                obj_str.Append(", ALLWANCENOTINCTAX");
                obj_str.Append(", DEDUCTINCTAX");
                obj_str.Append(", DEDUCTNOTINCTAX");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_TR_ACCUMALATE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRAccumalate();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.periodyear = Convert.ToString(dr["PERIODYEAR"]);
                    model.periodid = Convert.ToString(dr["PERIODID"]);
                    model.paydate = Convert.ToDateTime(dr["PAYDATE"]);
                    model.incomefix = Convert.ToDouble(dr["INCOMEFIX"]);
                    model.incomevar = Convert.ToDouble(dr["INCOMEVAR"]);
                    model.taxfix = Convert.ToDouble(dr["TAXFIX"]);
                    model.taxvar = Convert.ToDouble(dr["TAXVAR"]);
                    model.incomefortyone = Convert.ToDouble(dr["INCOMEFORTYONE"]);
                    model.incomefortyoneperthree = Convert.ToDouble(dr["INCOMEFORTYONEPERTHREE"]);
                    model.incomefortyonetwo = Convert.ToDouble(dr["INCOMEFORTYONETWO"]);
                    model.incomefortytwoin = Convert.ToDouble(dr["INCOMEFORTYTWOIN"]);
                    model.incomefortytwoout = Convert.ToDouble(dr["INCOMEFORTYTWOOUT"]);
                    model.taxfortyone = Convert.ToDouble(dr["TAXFORTYONE"]);
                    model.taxfortyoneperthree = Convert.ToDouble(dr["TAXFORTYONEPERTHREE"]);
                    model.taxfortyonetwo = Convert.ToDouble(dr["TAXFORTYONETWO"]);
                    model.taxfortytwoin = Convert.ToDouble(dr["TAXFORTYTWOIN"]);
                    model.taxfortytwoout = Convert.ToDouble(dr["TAXFORTYTWOOUT"]);
                    model.ssoacc_worker = Convert.ToDouble(dr["SSOACC_WORKER"]);
                    model.ssoacc_company = Convert.ToDouble(dr["SSOACC_COMPANY"]);
                    model.pfacc_worker = Convert.ToDouble(dr["PFACC_WORKER"]);
                    model.pfacc_company = Convert.ToDouble(dr["PFACC_COMPANY"]);
                    model.allwanceinctax = Convert.ToDouble(dr["ALLWANCEINCTAX"]);
                    model.allwancenotinctax = Convert.ToDouble(dr["ALLWANCENOTINCTAX"]);
                    model.deductinctax = Convert.ToDouble(dr["DEDUCTINCTAX"]);
                    model.deductnotinctax = Convert.ToDouble(dr["DEDUCTNOTINCTAX"]); 
                    
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "EMPACC001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRAccumalate> getDataByFillter(string com, string emp, DateTime date)
        {
            string strCondition = "";

            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!emp.Equals(""))
                strCondition += " AND WORKER_CODE='" + emp + "'";

            if (!date.Equals(""))
                strCondition += " AND PAYDATE='" + date + "'";

            return this.getData(strCondition);
        }
    }
}
