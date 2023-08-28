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
   public class cls_ctTRPlanreduce
     {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRPlanreduce() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRPlanreduce> getData(string condition)
        {
            List<cls_TRPlanreduce> list_model = new List<cls_TRPlanreduce>();
            cls_TRPlanreduce model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", PLANREDUCE_CODE");
                obj_str.Append(", REDUCE_CODE");

                obj_str.Append(" FROM PAY_TR_PLANREDUCE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, PLANREDUCE_CODE, REDUCE_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRPlanreduce();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.planreduce_code = dr["PLANREDUCE_CODE"].ToString();
                    model.reduce_code = dr["REDUCE_CODE"].ToString();

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRPlanreduce.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRPlanreduce> getDataByFillter(string com, string plan)
        {
            string strCondition = " AND COMPANY_CODE='" + com + "'";

            if (!plan.Equals(""))
                strCondition += " AND PLANREDUCE_CODE='" + plan + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string code, string reduce)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM ATT_TR_PLANREDUCE");
                obj_str.Append(" WHERE AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PLANREDUCE_CODE='" + code + "'");
                obj_str.Append(" AND REDUCE_CODE='" + reduce + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRPlanreduce.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }


        public bool delete(string com, string code, string reduce)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM PAY_TR_PLANREDUCE");
                obj_str.Append(" WHERE AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PLANREDUCE_CODE='" + code + "'");
                obj_str.Append(" AND REDUCE_CODE='" + reduce + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(TRPlanreduce.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string code)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM PAY_TR_PLANREDUCE");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PLANREDUCE_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(TRPlanreduce.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(List<cls_TRPlanreduce> list_model)
        {
            bool blnResult = false;
            try
            {
                

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PAY_TR_PLANREDUCE");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", PLANREDUCE_CODE ");
                obj_str.Append(", REDUCE_CODE ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @PLANREDUCE_CODE ");
                obj_str.Append(", @REDUCE_CODE ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                obj_cmd.Transaction = obj_conn.getTransaction();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                obj_cmd.Parameters.Add("@PLANREDUCE_CODE", SqlDbType.VarChar);
                obj_cmd.Parameters.Add("@REDUCE_CODE", SqlDbType.VarChar);


                foreach (cls_TRPlanreduce model in list_model)
                {

                    obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                    obj_cmd.Parameters["@PLANREDUCE_CODE"].Value = model.planreduce_code;
                    obj_cmd.Parameters["@REDUCE_CODE"].Value = model.reduce_code;

                    obj_cmd.ExecuteNonQuery();

                }

                blnResult = obj_conn.doCommit();
                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRPlanreduce.insert)" + ex.ToString();
            }

            return blnResult;
        }

    }
}
