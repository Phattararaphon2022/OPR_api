using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary_BPC.hrfocus.model;
using System.Data.SqlClient;
using System.Data;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTProcost
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTProcost() { }

        public string getMessage() { return this.Message.Replace("PRO_MT_PROCOST", "").Replace("cls_ctMTProcost", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTProcost> getData(string condition)
        {
            List<cls_MTProcost> list_model = new List<cls_MTProcost>();
            cls_MTProcost model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PROCOST_ID");
                obj_str.Append(", PROCOST_CODE");
                obj_str.Append(", PROCOST_NAME_TH");
                obj_str.Append(", PROCOST_NAME_EN");

                obj_str.Append(", ISNULL(PROCOST_TYPE, '') AS PROCOST_TYPE");
                obj_str.Append(", ISNULL(PROCOST_AUTO, '0') AS PROCOST_AUTO");
                obj_str.Append(", ISNULL(PROCOST_ITEMCODE, '') AS PROCOST_ITEMCODE");
                obj_str.Append(", COMPANY_CODE");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");



                obj_str.Append(" FROM PRO_MT_PROCOST");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PROCOST_ID");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTProcost();

                    model.procost_id = Convert.ToInt32(dr["PROCOST_ID"]);
                    model.procost_code = dr["PROCOST_CODE"].ToString();
                    model.procost_name_th = dr["PROCOST_NAME_TH"].ToString();
                    model.procost_name_en = dr["PROCOST_NAME_EN"].ToString();

                    model.procost_type = dr["PROCOST_TYPE"].ToString();
                    model.procost_auto = Convert.ToBoolean(dr["PROCOST_AUTO"]);
                    model.procost_itemcode = dr["PROCOST_ITEMCODE"].ToString();
                    model.company_code = dr["COMPANY_CODE"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "BNK001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTProcost> getDataByFillter(string com, string code)
        {
            string strCondition = " AND COMPANY_CODE='" + com + "'";

            if (!code.Equals(""))
                strCondition += " AND PROCOST_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(PROCOST_ID, 1) ");
                obj_str.Append(" FROM PRO_MT_PROCOST");
                obj_str.Append(" ORDER BY PROCOST_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "BNK002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PROCOST_CODE");
                obj_str.Append(" FROM PRO_MT_PROCOST");
                obj_str.Append(" WHERE PROCOST_CODE='" + code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "BNK003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string code)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_MT_PROCOST");
                obj_str.Append(" WHERE PROCOST_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BNK004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTProcost model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.procost_code))
                {
                    if (this.update(model))
                        return model.procost_id.ToString();
                    else
                        return "";
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PRO_MT_PROCOST");
                obj_str.Append(" (");
                obj_str.Append("PROCOST_ID ");
                obj_str.Append(", PROCOST_CODE ");
                obj_str.Append(", PROCOST_NAME_TH ");
                obj_str.Append(", PROCOST_NAME_EN ");

                obj_str.Append(", PROCOST_TYPE ");
                obj_str.Append(", PROCOST_AUTO ");
                obj_str.Append(", PROCOST_ITEMCODE ");
                obj_str.Append(", COMPANY_CODE ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@PROCOST_ID ");
                obj_str.Append(", @PROCOST_CODE ");
                obj_str.Append(", @PROCOST_NAME_TH ");
                obj_str.Append(", @PROCOST_NAME_EN ");

                obj_str.Append(", @PROCOST_TYPE ");
                obj_str.Append(", @PROCOST_AUTO ");
                obj_str.Append(", @PROCOST_ITEMCODE ");
                obj_str.Append(", @COMPANY_CODE ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.procost_id = this.getNextID();

                obj_cmd.Parameters.Add("@PROCOST_ID", SqlDbType.Int); obj_cmd.Parameters["@PROCOST_ID"].Value = model.procost_id;
                obj_cmd.Parameters.Add("@PROCOST_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROCOST_CODE"].Value = model.procost_code;
                obj_cmd.Parameters.Add("@PROCOST_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROCOST_NAME_TH"].Value = model.procost_name_th;
                obj_cmd.Parameters.Add("@PROCOST_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROCOST_NAME_EN"].Value = model.procost_name_en;

                obj_cmd.Parameters.Add("@PROCOST_TYPE", SqlDbType.Char); obj_cmd.Parameters["@PROCOST_TYPE"].Value = model.procost_type;
                obj_cmd.Parameters.Add("@PROCOST_AUTO", SqlDbType.Bit); obj_cmd.Parameters["@PROCOST_AUTO"].Value = model.procost_auto;
                obj_cmd.Parameters.Add("@PROCOST_ITEMCODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROCOST_ITEMCODE"].Value = model.procost_itemcode;
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                strResult = model.procost_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "BNK005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTProcost model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PRO_MT_PROCOST SET ");
                obj_str.Append(" PROCOST_NAME_TH=@PROCOST_NAME_TH ");
                obj_str.Append(", PROCOST_NAME_EN=@PROCOST_NAME_EN ");

                obj_str.Append(", PROCOST_TYPE=@PROCOST_TYPE ");
                obj_str.Append(", PROCOST_AUTO=@PROCOST_AUTO ");
                obj_str.Append(", PROCOST_ITEMCODE=@PROCOST_ITEMCODE ");
                obj_str.Append(", COMPANY_CODE=@COMPANY_CODE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE PROCOST_ID=@PROCOST_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROCOST_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROCOST_NAME_TH"].Value = model.procost_name_th;
                obj_cmd.Parameters.Add("@PROCOST_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROCOST_NAME_EN"].Value = model.procost_name_en;

                obj_cmd.Parameters.Add("@PROCOST_TYPE", SqlDbType.Char); obj_cmd.Parameters["@PROCOST_TYPE"].Value = model.procost_type;
                obj_cmd.Parameters.Add("@PROCOST_AUTO", SqlDbType.Bit); obj_cmd.Parameters["@PROCOST_AUTO"].Value = model.procost_auto;
                obj_cmd.Parameters.Add("@PROCOST_ITEMCODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROCOST_ITEMCODE"].Value = model.procost_itemcode;
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

                
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@PROCOST_ID", SqlDbType.Int); obj_cmd.Parameters["@PROCOST_ID"].Value = model.procost_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "BNK006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}
