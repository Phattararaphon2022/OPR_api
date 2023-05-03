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
   public class cls_ctMTBonus
  {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTBonus() { }

        public string getMessage() { return this.Message.Replace("PAY_MT_BONUS", "").Replace("cls_ctMTBonus", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTBonus> getData(string condition)
        {
            List<cls_MTBonus> list_model = new List<cls_MTBonus>();
            cls_MTBonus model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", BONUS_ID");
                obj_str.Append(", BONUS_CODE");
                obj_str.Append(", ISNULL(BONUS_NAME_TH, '') AS BONUS_NAME_TH");
                obj_str.Append(", ISNULL(BONUS_NAME_EN, '') AS BONUS_NAME_EN");
                obj_str.Append(", ITEM_CODE");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PAY_MT_BONUS");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, BONUS_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTBonus();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                    model.bonus_id = Convert.ToInt32(dr["BONUS_ID"]);
                    model.bonus_code = Convert.ToString(dr["BONUS_CODE"]);
                    model.bonus_name_th = Convert.ToString(dr["BONUS_NAME_TH"]);
                    model.bonus_name_en = Convert.ToString(dr["BONUS_NAME_EN"]);

                    model.item_code = Convert.ToString(dr["ITEM_CODE"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "PAYB001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTBonus> getDataByFillter(string com, string id, string code)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!id.Equals(""))
                strCondition += " AND BONUS_ID='" + id + "'";

            if (!code.Equals(""))
                strCondition += " AND BONUS_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(BONUS_ID) ");
                obj_str.Append(" FROM PAY_MT_BONUS");       

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "PAYB002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT BONUS_ID");
                obj_str.Append(" FROM PAY_MT_BONUS");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND BONUS_CODE='" + code + "'");
                                                

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "PAYB003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string id )
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM PAY_MT_BONUS");
                obj_str.Append(" WHERE BONUS_ID='" + id + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "PAYB004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_MTBonus model)
        {
            bool blnResult = false;
            //string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.company_code, model.bonus_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PAY_MT_BONUS");
                obj_str.Append(" (");
                obj_str.Append("BONUS_ID ");
                obj_str.Append(", BONUS_CODE ");
                obj_str.Append(", BONUS_NAME_TH ");
                obj_str.Append(", BONUS_NAME_EN ");

                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", ITEM_CODE ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@BONUS_ID ");
                obj_str.Append(", @BONUS_CODE ");
                obj_str.Append(", @BONUS_NAME_TH ");
                obj_str.Append(", @BONUS_NAME_EN ");
                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @ITEM_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@BONUS_ID", SqlDbType.Int); obj_cmd.Parameters["@BONUS_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@BONUS_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@BONUS_CODE"].Value = model.bonus_code;
                obj_cmd.Parameters.Add("@BONUS_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@BONUS_NAME_TH"].Value = model.bonus_name_th;
                obj_cmd.Parameters.Add("@BONUS_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@BONUS_NAME_EN"].Value = model.bonus_name_en;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@ITEM_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_CODE"].Value = model.item_code;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;
     
                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                //strResult = model.bonus_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "PAYB005:" + ex.ToString();
                //strResult = "";
            }

            return blnResult;
        }

        public bool update(cls_MTBonus model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PAY_MT_BONUS SET ");

                obj_str.Append(" BONUS_CODE=@BONUS_CODE ");
                obj_str.Append(", BONUS_NAME_TH=@BONUS_NAME_TH ");
                obj_str.Append(", BONUS_NAME_EN=@BONUS_NAME_EN ");

                obj_str.Append(", ITEM_CODE=@ITEM_CODE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");


                obj_str.Append(" WHERE BONUS_ID=@BONUS_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@BONUS_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@BONUS_CODE"].Value = model.bonus_code;
                obj_cmd.Parameters.Add("@BONUS_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@BONUS_NAME_TH"].Value = model.bonus_name_th;
                obj_cmd.Parameters.Add("@BONUS_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@BONUS_NAME_EN"].Value = model.bonus_name_en;

                obj_cmd.Parameters.Add("@ITEM_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_CODE"].Value = model.item_code;


                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@BONUS_ID", SqlDbType.Int); obj_cmd.Parameters["@BONUS_ID"].Value = model.bonus_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "PAYB006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}