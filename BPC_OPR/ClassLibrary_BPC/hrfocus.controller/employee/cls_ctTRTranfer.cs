using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRTranfer
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRTranfer() { }

        public string getMessage() { return this.Message.Replace("EMP_TR_TRANFER", "").Replace("cls_ctTRTranfer", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRTranfer> getData(string condition)
        {
            List<cls_TRTranfer> list_model = new List<cls_TRTranfer>();
            cls_TRTranfer model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");

                obj_str.Append(", EMPTRANFER_ID");
                obj_str.Append(", INSTITUTE_CODE");
                obj_str.Append(", JOB_TYPE");
                obj_str.Append(", EMPTRANFER_FROMDATE");
                obj_str.Append(", EMPTRANFER_TODATE");
                obj_str.Append(", EMPTRANFER_SALARY");
                obj_str.Append(", EMPTRANFER_OT");
                obj_str.Append(", EMPTRANFER_OTHER");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_TR_TRANFER");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRTranfer();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.emptranfer_id = Convert.ToInt32(dr["EMPTRANFER_ID"]);
                    model.institute_code = Convert.ToString(dr["INSTITUTE_CODE"]);
                    model.job_type = Convert.ToString(dr["JOB_TYPE"]);
                    model.emptranfer_fromdate = Convert.ToDateTime(dr["EMPTRANFER_FROMDATE"]);
                    model.emptranfer_todate = Convert.ToDateTime(dr["EMPTRANFER_TODATE"]);
                    model.emptranfer_salary = Convert.ToDouble(dr["EMPTRANFER_SALARY"]);
                    model.emptranfer_ot = Convert.ToDouble(dr["EMPTRANFER_OT"]);
                    model.emptranfer_other = Convert.ToString(dr["EMPTRANFER_OTHER"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "EMPTRF001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRTranfer> getDataByFillter(string com, string emp)
        {
            string strCondition = "";

            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!emp.Equals(""))
                strCondition += " AND WORKER_CODE='" + emp + "'";

            return this.getData(strCondition);
        }
    }
}
