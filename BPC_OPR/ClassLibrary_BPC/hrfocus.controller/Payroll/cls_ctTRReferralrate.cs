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
    public class cls_ctTRReferralrate
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRReferralrate() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRReferralrate> getData(string condition)
        {
            List<cls_TRReferralrate> list_model = new List<cls_TRReferralrate>();
            cls_TRReferralrate model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", REFERRAL_CODE");
                obj_str.Append(", REFERRALRATE_MONTH");
                obj_str.Append(", REFERRALRATE_RATE");

                obj_str.Append(" FROM PAY_TR_REFERRALRATE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, REFERRAL_CODE, REFERRALRATE_MONTH");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRReferralrate();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                    model.referral_code = Convert.ToString(dr["REFERRAL_CODE"]);
                    model.referralrate_month = Convert.ToDouble(dr["REFERRALRATE_MONTH"]);
                    model.referralrate_rate = Convert.ToDouble(dr["REFERRALRATE_RATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Referralrate.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRReferralrate> getDataByFillter(string com, string code)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!code.Equals(""))
                strCondition += " AND REFERRAL_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string code, double workagefrom)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT REFERRAL_CODE");
                obj_str.Append(" FROM PAY_TR_REFERRALRATE");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND REFERRAL_CODE='" + code + "'");
                obj_str.Append(" AND REFERRALRATE_MONTH='" + workagefrom + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Referralrate.checkDataOld)" + ex.ToString();
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
                obj_str.Append(" DELETE FROM PAY_TR_REFERRALRATE");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "'");
                obj_str.Append(" AND REFERRAL_CODE ='" + code + "'");

                blnResult = Obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Referralrate.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(List<cls_TRReferralrate> list_model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                if (!this.delete(list_model[0].company_code, list_model[0].referral_code))
                {
                    return false;
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PAY_TR_REFERRALRATE");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", REFERRAL_CODE ");
                obj_str.Append(", REFERRALRATE_MONTH ");
                obj_str.Append(", REFERRALRATE_RATE ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @REFERRAL_CODE ");
                obj_str.Append(", @REFERRALRATE_MONTH ");
                obj_str.Append(", @REFERRALRATE_RATE ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Transaction = obj_conn.getTransaction();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                obj_cmd.Parameters.Add("@REFERRAL_CODE", SqlDbType.VarChar);
                obj_cmd.Parameters.Add("@REFERRALRATE_MONTH", SqlDbType.Decimal);
                obj_cmd.Parameters.Add("@REFERRALRATE_RATE", SqlDbType.Decimal);

                foreach (cls_TRReferralrate model in list_model)
                {

                    obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                    obj_cmd.Parameters["@REFERRAL_CODE"].Value = model.referral_code;
                    obj_cmd.Parameters["@REFERRALRATE_MONTH"].Value = model.referralrate_month;
                    obj_cmd.Parameters["@REFERRALRATE_RATE"].Value = model.referralrate_rate;

                    obj_cmd.ExecuteNonQuery();

                }


                blnResult = obj_conn.doCommit();
                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Referralrate.insert)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
