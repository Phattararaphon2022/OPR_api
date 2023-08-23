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
    public class cls_ctMTPlanitems
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTPlanitems() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTPlanitems> getData(string condition)
        {
            List<cls_MTPlanitems> list_model = new List<cls_MTPlanitems>();
            cls_MTPlanitems model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PLANITEMS_ID");
                obj_str.Append(", PLANITEMS_CODE");
                obj_str.Append(", ISNULL(PLANITEMS_NAME_TH, '') AS PLANITEMS_NAME_TH");
                obj_str.Append(", ISNULL(PLANITEMS_NAME_EN, '') AS PLANITEMS_NAME_EN");

                obj_str.Append(", COMPANY_CODE");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PAY_MT_PLANITEMS");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PLANITEMS_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTPlanitems();

                    model.planitems_id = Convert.ToInt32(dr["PLANITEMS_ID"]);
                    model.planitems_code = dr["PLANITEMS_CODE"].ToString();
                    model.planitems_name_th = dr["PLANITEMS_NAME_TH"].ToString();
                    model.planitems_name_en = dr["PLANITEMS_NAME_EN"].ToString();
                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(PLANITEMS.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTPlanitems> getDataByFillter(string com, string id, string code)
        {
            string strCondition = " AND COMPANY_CODE='" + com + "'";

            if (!id.Equals(""))
                strCondition += " AND PLANITEMS_ID='" + id + "'";

            if (!code.Equals(""))
                strCondition += " AND PLANITEMS_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PLANITEMS_ID");
                obj_str.Append(" FROM PAY_MT_PLANITEMS");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND PLANITEMS_CODE='" + code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(PLANITEMS.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(PLANITEMS_ID) ");
                obj_str.Append(" FROM PAY_MT_PLANITEMS");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(PLANITEMS.getNextID)" + ex.ToString();
            }

            return intResult;
        }

        public bool delete(string id, string com)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM PAY_MT_PLANITEMS");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND PLANITEMS_ID='" + id + "'");
                obj_str.Append(" AND COMPANY_CODE='" + com + "' ");
                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(PLANITEMS.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTPlanitems model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.planitems_code))
                {
                    if (model.planitems_id.Equals(0))
                    {
                        return "D";
                    }
                    else
                    {
                        return this.update(model);
                    }
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO PAY_MT_PLANITEMS");
                obj_str.Append(" (");
                obj_str.Append("PLANITEMS_ID ");
                obj_str.Append(", PLANITEMS_CODE ");
                obj_str.Append(", PLANITEMS_NAME_TH ");
                obj_str.Append(", PLANITEMS_NAME_EN ");
                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append(" @PLANITEMS_ID ");
                obj_str.Append(", @PLANITEMS_CODE ");
                obj_str.Append(", @PLANITEMS_NAME_TH ");
                obj_str.Append(", @PLANITEMS_NAME_EN ");
                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PLANITEMS_ID", SqlDbType.Int); obj_cmd.Parameters["@PLANITEMS_ID"].Value = id;
                obj_cmd.Parameters.Add("@PLANITEMS_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PLANITEMS_CODE"].Value = model.planitems_code;
                obj_cmd.Parameters.Add("@PLANITEMS_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PLANITEMS_NAME_TH"].Value = model.planitems_name_th;
                obj_cmd.Parameters.Add("@PLANITEMS_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PLANITEMS_NAME_EN"].Value = model.planitems_name_en;
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(PLANITEMS.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_MTPlanitems model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE PAY_MT_PLANITEMS SET ");

                obj_str.Append(" PLANITEMS_CODE=@PLANITEMS_CODE ");
                obj_str.Append(", PLANITEMS_NAME_TH=@PLANITEMS_NAME_TH ");
                obj_str.Append(", PLANITEMS_NAME_EN=@PLANITEMS_NAME_EN ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");
                obj_str.Append(" WHERE 1=1 ");
                if (!model.planitems_id.Equals(0))
                {
                    obj_str.Append(" AND PLANITEMS_ID=@PLANITEMS_ID ");
                }
                else
                {
                    obj_str.Append(" AND COMPANY_CODE='" + model.company_code + "' ");
                    obj_str.Append(" AND PLANITEMS_CODE='" + model.planitems_code + "'");
                }

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PLANITEMS_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PLANITEMS_CODE"].Value = model.planitems_code;
                obj_cmd.Parameters.Add("@PLANITEMS_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PLANITEMS_NAME_TH"].Value = model.planitems_name_th;
                obj_cmd.Parameters.Add("@PLANITEMS_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PLANITEMS_NAME_EN"].Value = model.planitems_name_en;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.Parameters.Add("@PLANITEMS_ID", SqlDbType.Int); obj_cmd.Parameters["@PLANITEMS_ID"].Value = model.planitems_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.planitems_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(PLANITEMS.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}