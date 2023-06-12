using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTSupply
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTSupply() { }

        public string getMessage() { return this.Message.Replace("SYS_MT_SUPPLY", "").Replace("cls_ctMTSupply", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }
        private List<cls_MTSupply> getData(string condition)
        {
            List<cls_MTSupply> list_model = new List<cls_MTSupply>();
            cls_MTSupply model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("SUPPLY_ID");
                obj_str.Append(", SUPPLY_CODE");
                obj_str.Append(", ISNULL(SUPPLY_NAME_TH, '') AS SUPPLY_NAME_TH");
                obj_str.Append(", ISNULL(SUPPLY_NAME_EN, '') AS SUPPLY_NAME_EN");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM SYS_MT_SUPPLY");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY SUPPLY_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTSupply();

                    model.supply_id = Convert.ToInt32(dr["SUPPLY_ID"]);
                    model.supply_code = dr["SUPPLY_CODE"].ToString();
                    model.supply_name_th = dr["SUPPLY_NAME_TH"].ToString();
                    model.supply_name_en = dr["SUPPLY_NAME_EN"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "SUP001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTSupply> getDataByFillter(string code)
        {
            string strCondition = "";

            if (!code.Equals(""))
                strCondition += " AND SUPPLY_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(SUPPLY_ID, 1) ");
                obj_str.Append(" FROM SYS_MT_SUPPLY");
                obj_str.Append(" ORDER BY SUPPLY_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "SUP002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT SUPPLY_CODE");
                obj_str.Append(" FROM SYS_MT_SUPPLY");
                obj_str.Append(" WHERE SUPPLY_CODE='" + code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "SUP003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM SYS_MT_SUPPLY");
                obj_str.Append(" WHERE SUPPLY_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "SUP004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTSupply model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.supply_code))
                {
                    if (this.update(model))
                        return model.supply_id.ToString();
                    else
                        return "";
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_MT_SUPPLY");
                obj_str.Append(" (");
                obj_str.Append("SUPPLY_ID ");
                obj_str.Append(", SUPPLY_CODE ");
                obj_str.Append(", SUPPLY_NAME_TH ");
                obj_str.Append(", SUPPLY_NAME_EN ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@SUPPLY_ID ");
                obj_str.Append(", @SUPPLY_CODE ");
                obj_str.Append(", @SUPPLY_NAME_TH ");
                obj_str.Append(", @SUPPLY_NAME_EN ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.supply_id = this.getNextID();

                obj_cmd.Parameters.Add("@SUPPLY_ID", SqlDbType.Int); obj_cmd.Parameters["@SUPPLY_ID"].Value = model.supply_id;
                obj_cmd.Parameters.Add("@SUPPLY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@SUPPLY_CODE"].Value = model.supply_code;
                obj_cmd.Parameters.Add("@SUPPLY_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@SUPPLY_NAME_TH"].Value = model.supply_name_th;
                obj_cmd.Parameters.Add("@SUPPLY_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@SUPPLY_NAME_EN"].Value = model.supply_name_en;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                strResult = model.supply_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "SUP005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTSupply model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE SYS_MT_SUPPLY SET ");
                obj_str.Append(" SUPPLY_CODE=@SUPPLY_CODE ");
                obj_str.Append(", SUPPLY_NAME_TH=@SUPPLY_NAME_TH ");
                obj_str.Append(", SUPPLY_NAME_EN=@SUPPLY_NAME_EN ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE SUPPLY_ID=@SUPPLY_ID ");



                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@SUPPLY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@SUPPLY_CODE"].Value = model.supply_code;
                obj_cmd.Parameters.Add("@SUPPLY_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@SUPPLY_NAME_TH"].Value = model.supply_name_th;
                obj_cmd.Parameters.Add("@SUPPLY_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@SUPPLY_NAME_EN"].Value = model.supply_name_en;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@SUPPLY_ID", SqlDbType.Int); obj_cmd.Parameters["@SUPPLY_ID"].Value = model.supply_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "SUP006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
