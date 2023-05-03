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
   public class cls_ctTRBonusrate
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRBonusrate() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRBonusrate> getData(string condition)
        {
            List<cls_TRBonusrate> list_model = new List<cls_TRBonusrate>();
            cls_TRBonusrate model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", BONUS_CODE");
                obj_str.Append(", BONUSRATE_FROM");
                obj_str.Append(", BONUSRATE_TO");
                obj_str.Append(", BONUSRATE_RATE");

                obj_str.Append(" FROM PAY_TR_BONUSRATE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, BONUS_CODE, BONUSRATE_FROM");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRBonusrate();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                    model.bonus_code = Convert.ToString(dr["BONUS_CODE"]);
                    model.bonusrate_from = Convert.ToDouble(dr["BONUSRATE_FROM"]);
                    model.bonusrate_to = Convert.ToDouble(dr["BONUSRATE_TO"]);
                    model.bonusrate_rate = Convert.ToDouble(dr["BONUSRATE_RATE"]);     
    
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Bonusrate.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRBonusrate> getDataByFillter(string com, string code)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!code.Equals(""))
                strCondition += " AND BONUS_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string code, double workagefrom)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT BONUS_CODE");
                obj_str.Append(" FROM PAY_TR_BONUSRATE");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND BONUS_CODE='" + code + "'");
                obj_str.Append(" AND BONUSRATE_FROM='" + workagefrom + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Bonusrate.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string code)
        {
            bool blnResult = true;
            try
            {
                //cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append(" DELETE FROM PAY_TR_BONUSRATE");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "'");
                obj_str.Append(" AND BONUS_CODE ='" + code + "'");

                blnResult = Obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Bonusrate.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(List<cls_TRBonusrate> list_model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                if (!this.delete(list_model[0].company_code, list_model[0].bonus_code))
                {
                    return false;
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PAY_TR_BONUSRATE");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", BONUS_CODE ");
                obj_str.Append(", BONUSRATE_FROM ");
                obj_str.Append(", BONUSRATE_TO ");
                obj_str.Append(", BONUSRATE_RATE ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @BONUS_CODE ");
                obj_str.Append(", @BONUSRATE_FROM ");
                obj_str.Append(", @BONUSRATE_TO ");
                obj_str.Append(", @BONUSRATE_RATE ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Transaction = obj_conn.getTransaction();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                obj_cmd.Parameters.Add("@BONUS_CODE", SqlDbType.VarChar);
                obj_cmd.Parameters.Add("@BONUSRATE_FROM", SqlDbType.Decimal);
                obj_cmd.Parameters.Add("@BONUSRATE_TO", SqlDbType.Decimal);
                obj_cmd.Parameters.Add("@BONUSRATE_RATE", SqlDbType.Decimal);

                foreach (cls_TRBonusrate model in list_model)
                {

                    obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                    obj_cmd.Parameters["@BONUS_CODE"].Value = model.bonus_code;
                    obj_cmd.Parameters["@BONUSRATE_FROM"].Value = model.bonusrate_from;
                    obj_cmd.Parameters["@BONUSRATE_TO"].Value = model.bonusrate_to;
                    obj_cmd.Parameters["@BONUSRATE_RATE"].Value = model.bonusrate_rate;

                    obj_cmd.ExecuteNonQuery();

                }


                blnResult = obj_conn.doCommit();
                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Bonusrate.insert)" + ex.ToString();
            }

            return blnResult;
        }

    }
}
