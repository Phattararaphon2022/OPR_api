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
    public class cls_ctMTProslip
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTProslip() { }

        public string getMessage() { return this.Message.Replace("PRO_MT_PROSLIP", "").Replace("cls_ctMTProslip", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTProslip> getData(string condition)
        {
            List<cls_MTProslip> list_model = new List<cls_MTProslip>();
            cls_MTProslip model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PROSLIP_ID");
                obj_str.Append(", PROSLIP_CODE");
                obj_str.Append(", PROSLIP_NAME_TH");
                obj_str.Append(", PROSLIP_NAME_EN");
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PRO_MT_PROSLIP");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY proslip_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTProslip();

                    model.proslip_id = Convert.ToInt32(dr["PROSLIP_ID"]);
                    model.proslip_code = dr["PROSLIP_CODE"].ToString();
                    model.proslip_name_th = dr["PROSLIP_NAME_TH"].ToString();
                    model.proslip_name_en = dr["PROSLIP_NAME_EN"].ToString();
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

        public List<cls_MTProslip> getDataByFillter(string code)
        {
            string strCondition = "";

            if (!code.Equals(""))
                strCondition += " AND PROSLIP_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(PROSLIP_ID, 1) ");
                obj_str.Append(" FROM PRO_MT_PROSLIP");
                obj_str.Append(" ORDER BY PROSLIP_ID DESC ");

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

                obj_str.Append("SELECT PROSLIP_CODE");
                obj_str.Append(" FROM PRO_MT_PROSLIP");
                obj_str.Append(" WHERE PROSLIP_CODE='" + code + "'");

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

                obj_str.Append("DELETE FROM PRO_MT_PROSLIP");
                obj_str.Append(" WHERE PROSLIP_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BNK004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTProslip model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.proslip_code))
                {
                    if (this.update(model))
                        return model.proslip_id.ToString();
                    else
                        return "";
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PRO_MT_PROSLIP");
                obj_str.Append(" (");
                obj_str.Append("PROSLIP_ID ");
                obj_str.Append(", PROSLIP_CODE ");
                obj_str.Append(", PROSLIP_NAME_TH ");
                obj_str.Append(", PROSLIP_NAME_EN ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@PROSLIP_ID ");
                obj_str.Append(", @PROSLIP_CODE ");
                obj_str.Append(", @PROSLIP_NAME_TH ");
                obj_str.Append(", @PROSLIP_NAME_EN ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.proslip_id = this.getNextID();

                obj_cmd.Parameters.Add("@PROSLIP_ID", SqlDbType.Int); obj_cmd.Parameters["@PROSLIP_ID"].Value = model.proslip_id;
                obj_cmd.Parameters.Add("@PROSLIP_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROSLIP_CODE"].Value = model.proslip_code;
                obj_cmd.Parameters.Add("@PROSLIP_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROSLIP_NAME_TH"].Value = model.proslip_name_th;
                obj_cmd.Parameters.Add("@PROSLIP_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROSLIP_NAME_EN"].Value = model.proslip_name_en;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                strResult = model.proslip_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "BNK005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTProslip model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PRO_MT_PROSLIP SET ");
                obj_str.Append(" PROSLIP_NAME_TH=@PROSLIP_NAME_TH ");
                obj_str.Append(", PROSLIP_NAME_EN=@PROSLIP_NAME_EN ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE PROSLIP_ID=@PROSLIP_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROSLIP_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROSLIP_NAME_TH"].Value = model.proslip_name_th;
                obj_cmd.Parameters.Add("@PROSLIP_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROSLIP_NAME_EN"].Value = model.proslip_name_en;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@PROSLIP_ID", SqlDbType.Int); obj_cmd.Parameters["@PROSLIP_ID"].Value = model.proslip_id;

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
