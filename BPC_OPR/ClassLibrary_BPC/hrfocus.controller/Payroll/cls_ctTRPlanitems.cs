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
   public class cls_ctTRPlanitems
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRPlanitems() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRPlanitems> getData(string condition)
        {
            List<cls_TRPlanitems> list_model = new List<cls_TRPlanitems>();
            cls_TRPlanitems model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", PLANITEMS_CODE");
                obj_str.Append(", ITEM_CODE");

                obj_str.Append(" FROM PAY_TR_PLANITEMS");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, PLANITEMS_CODE, ITEM_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRPlanitems();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.planitems_code = dr["PLANITEMS_CODE"].ToString();
                    model.item_code = dr["ITEM_CODE"].ToString();

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRPlanitems.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRPlanitems> getDataByFillter(string com, string plan)
        {
            string strCondition = " AND COMPANY_CODE='" + com + "'";

            if (!plan.Equals(""))
                strCondition += " AND PLANITEMS_CODE='" + plan + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string code, string item)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM PAY_TR_PLANITEMS");
                obj_str.Append(" WHERE AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PLANITEMS_CODE='" + code + "'");
                obj_str.Append(" AND ITEM_CODE='" + item + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRPlanitems.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }


        public bool delete(string com, string code, string item)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM PAY_TR_PLANITEMS");
                obj_str.Append(" WHERE AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PLANITEMS_CODE='" + code + "'");
                obj_str.Append(" AND ITEM_CODE='" + item + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(TRPlanitems.delete)" + ex.ToString();
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

                obj_str.Append(" DELETE FROM PAY_TR_PLANITEMS");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PLANITEMS_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(TRPlanitems.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(List<cls_TRPlanitems> list_model)
        {
            bool blnResult = false;
            try
            {
                

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PAY_TR_PLANITEMS");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", PLANITEMS_CODE ");
                obj_str.Append(", ITEM_CODE ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @PLANITEMS_CODE ");
                obj_str.Append(", @ITEM_CODE ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                obj_cmd.Transaction = obj_conn.getTransaction();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                obj_cmd.Parameters.Add("@PLANITEMS_CODE", SqlDbType.VarChar);
                obj_cmd.Parameters.Add("@ITEM_CODE", SqlDbType.VarChar);


                foreach (cls_TRPlanitems model in list_model)
                {

                    obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                    obj_cmd.Parameters["@PLANITEMS_CODE"].Value = model.planitems_code;
                    obj_cmd.Parameters["@ITEM_CODE"].Value = model.item_code;

                    obj_cmd.ExecuteNonQuery();

                }

                blnResult = obj_conn.doCommit();
                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRPlanitems.insert)" + ex.ToString();
            }

            return blnResult;
        }

    }
}
