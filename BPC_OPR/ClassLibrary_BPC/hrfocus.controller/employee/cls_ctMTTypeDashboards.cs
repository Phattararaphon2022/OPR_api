using ClassLibrary_BPC.hrfocus.model.employee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller 
{
 
   public class cls_ctMTTypeDashboards
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTTypeDashboards() { }
        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }
         
        private List<cls_MTDashboards> getDataType(string condition)
        {
            List<cls_MTDashboards> list_model = new List<cls_MTDashboards>();
            cls_MTDashboards model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT");

                obj_str.Append(" COUNT(EMP_MT_WORKER.WORKER_TYPE)as WORKER_CODE");
                obj_str.Append(", EMP_MT_TYPE.TYPE_CODE");
                obj_str.Append(",  EMP_MT_TYPE.TYPE_NAME_TH");
                obj_str.Append(", EMP_MT_TYPE.TYPE_NAME_EN");
                obj_str.Append(",  EMP_MT_WORKER.WORKER_CODE");
                obj_str.Append(", EMP_MT_WORKER.WORKER_FNAME_TH");
                obj_str.Append(", EMP_MT_WORKER.WORKER_FNAME_EN");

                obj_str.Append(" FROM EMP_MT_WORKER");
                obj_str.Append(" INNER JOIN EMP_MT_TYPE ON EMP_MT_TYPE.TYPE_CODE = EMP_MT_WORKER.WORKER_TYPE");
 
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);
                //obj_str.Append(" AND EMP_MT_WORKER.WORKER_TYPE IN ('M', 'D')");
                obj_str.Append(" GROUP BY EMP_MT_TYPE.TYPE_CODE, EMP_MT_TYPE.TYPE_NAME_TH, EMP_MT_TYPE.TYPE_NAME_EN, EMP_MT_WORKER.WORKER_CODE, EMP_MT_WORKER.WORKER_FNAME_TH, EMP_MT_WORKER.WORKER_FNAME_EN");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTDashboards();

                    model.worker_code = Convert.ToInt32(dr["WORKER_CODE"]);
                    model.type_name_th = dr["TYPE_NAME_TH"].ToString();
                    model.type_name_en = dr["TYPE_NAME_EN"].ToString();


                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTDashboard.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTDashboards> getDataByFillterType(string type)
        {
            string strCondition = "";

            if (!type.Equals("")) 

                strCondition += " AND EMP_MT_WORKER='" + type + "('M', 'D')";



            return this.getDataType(strCondition);
        }


    }
}
