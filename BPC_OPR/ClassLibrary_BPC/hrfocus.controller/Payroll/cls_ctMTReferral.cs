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
    public class cls_ctMTReferral
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTReferral() { }

        public string getMessage() { return this.Message.Replace("PAY_MT_REFERRAL", "").Replace("cls_ctMTReferral", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTReferral> getData(string condition)
        {
            List<cls_MTReferral> list_model = new List<cls_MTReferral>();
            cls_MTReferral model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", REFERRAL_ID");
                obj_str.Append(", REFERRAL_CODE");
                obj_str.Append(", ISNULL(REFERRAL_NAME_TH, '') AS REFERRAL_NAME_TH");
                obj_str.Append(", ISNULL(REFERRAL_NAME_EN, '') AS REFERRAL_NAME_EN");
                obj_str.Append(", ITEM_CODE");
                obj_str.Append(", NOTUSED");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PAY_MT_REFERRAL");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, REFERRAL_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTReferral();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                    model.referral_id = Convert.ToInt32(dr["REFERRAL_ID"]);
                    model.referral_code = Convert.ToString(dr["REFERRAL_CODE"]);
                    model.referral_name_th = Convert.ToString(dr["REFERRAL_NAME_TH"]);
                    model.referral_name_en = Convert.ToString(dr["REFERRAL_NAME_EN"]);

                    model.item_code = Convert.ToString(dr["ITEM_CODE"]);

                    model.notused = Convert.ToBoolean(dr["NOTUSED"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "PAYR001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTReferral> getDataByFillter(string com, string id, string code)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!id.Equals(""))
                strCondition += " AND REFERRAL_ID='" + id + "'";

            if (!code.Equals(""))
                strCondition += " AND REFERRAL_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(REFERRAL_ID) ");
                obj_str.Append(" FROM PAY_MT_REFERRAL");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "PAYR002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string code, string id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT REFERRAL_ID");
                obj_str.Append(" FROM PAY_MT_REFERRAL");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND REFERRAL_CODE='" + code + "'");
                obj_str.Append(" AND REFERRAL_ID='" + id + "'");


                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "PAYR003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string id)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM PAY_MT_REFERRAL");
                obj_str.Append(" WHERE REFERRAL_ID='" + id + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "PAYR004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTReferral model)
        {
            string strResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.referral_code, model.referral_id.ToString()))
                {
                    if (this.update(model))
                        return model.referral_id.ToString();
                    else
                        return "";
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PAY_MT_REFERRAL");
                obj_str.Append(" (");
                obj_str.Append("REFERRAL_ID ");
                obj_str.Append(", REFERRAL_CODE ");
                obj_str.Append(", REFERRAL_NAME_TH ");
                obj_str.Append(", REFERRAL_NAME_EN ");

                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", ITEM_CODE ");
                obj_str.Append(", NOTUSED ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@REFERRAL_ID ");
                obj_str.Append(", @REFERRAL_CODE ");
                obj_str.Append(", @REFERRAL_NAME_TH ");
                obj_str.Append(", @REFERRAL_NAME_EN ");

                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @ITEM_CODE ");
                obj_str.Append(", @NOTUSED ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                model.referral_id = this.getNextID();

                obj_cmd.Parameters.Add("@REFERRAL_ID", SqlDbType.Int); obj_cmd.Parameters["@REFERRAL_ID"].Value = model.referral_id;
                obj_cmd.Parameters.Add("@REFERRAL_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@REFERRAL_CODE"].Value = model.referral_code;
                obj_cmd.Parameters.Add("@REFERRAL_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@REFERRAL_NAME_TH"].Value = model.referral_name_th;
                obj_cmd.Parameters.Add("@REFERRAL_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@REFERRAL_NAME_EN"].Value = model.referral_name_en;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@ITEM_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_CODE"].Value = model.item_code;
                obj_cmd.Parameters.Add("@NOTUSED", SqlDbType.Bit); obj_cmd.Parameters["@NOTUSED"].Value = model.notused;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                strResult = model.referral_id.ToString();
            }
            catch (Exception ex)
            {
                strResult = "";
                Message = "PAYR005:" + ex.ToString();


            }
            return strResult;
        }

        public bool update(cls_MTReferral model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PAY_MT_REFERRAL SET ");

                obj_str.Append("  REFERRAL_NAME_TH=@REFERRAL_NAME_TH ");
                obj_str.Append(", REFERRAL_NAME_EN=@REFERRAL_NAME_EN ");

                obj_str.Append(", ITEM_CODE=@ITEM_CODE ");
                obj_str.Append(", NOTUSED=@NOTUSED ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");
                obj_str.Append(" WHERE REFERRAL_CODE=@REFERRAL_CODE ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@REFERRAL_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@REFERRAL_NAME_TH"].Value = model.referral_name_th;
                obj_cmd.Parameters.Add("@REFERRAL_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@REFERRAL_NAME_EN"].Value = model.referral_name_en;
                obj_cmd.Parameters.Add("@NOTUSED", SqlDbType.Bit); obj_cmd.Parameters["@NOTUSED"].Value = model.notused;

                obj_cmd.Parameters.Add("@ITEM_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_CODE"].Value = model.item_code;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@REFERRAL_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@REFERRAL_CODE"].Value = model.referral_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "PAYR006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
