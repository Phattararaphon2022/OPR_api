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
    public class cls_ctMTProtype
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTProtype() { }

        public string getMessage() { return this.Message.Replace("PRO_MT_PROTYPE", "").Replace("cls_ctMTProtype", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTProtype> getData(string condition)
        {
            List<cls_MTProtype> list_model = new List<cls_MTProtype>();
            cls_MTProtype model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");
                obj_str.Append("COMPANY_CODE");

                obj_str.Append(", PROTYPE_ID");
                obj_str.Append(", PROTYPE_CODE");
                obj_str.Append(", PROTYPE_NAME_TH");
                obj_str.Append(", PROTYPE_NAME_EN");
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PRO_MT_PROTYPE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY protype_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTProtype();
                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);

                    model.protype_id = Convert.ToInt32(dr["PROTYPE_ID"]);
                    model.protype_code = dr["PROTYPE_CODE"].ToString();
                    model.protype_name_th = dr["PROTYPE_NAME_TH"].ToString();
                    model.protype_name_en = dr["PROTYPE_NAME_EN"].ToString();
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

        public List<cls_MTProtype> getDataByFillter(string com, string code)
        {
            string strCondition = "";
            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!code.Equals(""))
                strCondition += " AND PROTYPE_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(PROTYPE_ID, 1) ");
                obj_str.Append(" FROM PRO_MT_PROTYPE");
                obj_str.Append(" ORDER BY PROTYPE_ID DESC ");

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

        public bool checkDataOld(string code,string com)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PROTYPE_CODE");
                obj_str.Append(" FROM PRO_MT_PROTYPE");
                obj_str.Append(" WHERE PROTYPE_CODE='" + code + "'");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");

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

        public bool delete(string code, string com)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_MT_PROTYPE");
                obj_str.Append(" WHERE PROTYPE_CODE='" + code + "'");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BNK004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTProtype model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.protype_code,model.company_code))
                {
                    if (this.update(model))
                        return model.protype_id.ToString();
                    else
                        return "";
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PRO_MT_PROTYPE");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");

                obj_str.Append(", PROTYPE_ID ");
                obj_str.Append(", PROTYPE_CODE ");
                obj_str.Append(", PROTYPE_NAME_TH ");
                obj_str.Append(", PROTYPE_NAME_EN ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");

                obj_str.Append(", @PROTYPE_ID ");
                obj_str.Append(", @PROTYPE_CODE ");
                obj_str.Append(", @PROTYPE_NAME_TH ");
                obj_str.Append(", @PROTYPE_NAME_EN ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.protype_id = this.getNextID();
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

                obj_cmd.Parameters.Add("@PROTYPE_ID", SqlDbType.Int); obj_cmd.Parameters["@PROTYPE_ID"].Value = model.protype_id;
                obj_cmd.Parameters.Add("@PROTYPE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROTYPE_CODE"].Value = model.protype_code;
                obj_cmd.Parameters.Add("@PROTYPE_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROTYPE_NAME_TH"].Value = model.protype_name_th;
                obj_cmd.Parameters.Add("@PROTYPE_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROTYPE_NAME_EN"].Value = model.protype_name_en;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                strResult = model.protype_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "BNK005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTProtype model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PRO_MT_PROTYPE SET ");
                obj_str.Append(" PROTYPE_NAME_TH=@PROTYPE_NAME_TH ");
                obj_str.Append(", PROTYPE_NAME_EN=@PROTYPE_NAME_EN ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE PROTYPE_ID=@PROTYPE_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROTYPE_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROTYPE_NAME_TH"].Value = model.protype_name_th;
                obj_cmd.Parameters.Add("@PROTYPE_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROTYPE_NAME_EN"].Value = model.protype_name_en;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@PROTYPE_ID", SqlDbType.Int); obj_cmd.Parameters["@PROTYPE_ID"].Value = model.protype_id;

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
