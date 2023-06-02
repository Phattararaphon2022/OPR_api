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
    public class cls_ctMTUniform
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTUniform() { }

        public string getMessage() { return this.Message.Replace("SYS_MT_UNIFORM", "").Replace("cls_ctMTUniform", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }
        private List<cls_MTUniform> getData(string condition)
        {
            List<cls_MTUniform> list_model = new List<cls_MTUniform>();
            cls_MTUniform model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("UNIFORM_ID");
                obj_str.Append(", UNIFORM_CODE");
                obj_str.Append(", ISNULL(UNIFORM_NAME_TH, '') AS UNIFORM_NAME_TH");
                obj_str.Append(", ISNULL(UNIFORM_NAME_EN, '') AS UNIFORM_NAME_EN");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM SYS_MT_UNIFORM");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY UNIFORM_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTUniform();

                    model.uniform_id = Convert.ToInt32(dr["UNIFORM_ID"]);
                    model.uniform_code = dr["UNIFORM_CODE"].ToString();
                    model.uniform_name_th = dr["UNIFORM_NAME_TH"].ToString();
                    model.uniform_name_en = dr["UNIFORM_NAME_EN"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "UNI001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTUniform> getDataByFillter(string code)
        {
            string strCondition = "";

            if (!code.Equals(""))
                strCondition += " AND UNIFORM_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(UNIFORM_ID, 1) ");
                obj_str.Append(" FROM SYS_MT_UNIFORM");
                obj_str.Append(" ORDER BY UNIFORM_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "UNI002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT UNIFORM_CODE");
                obj_str.Append(" FROM SYS_MT_UNIFORM");
                obj_str.Append(" WHERE UNIFORM_CODE='" + code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "UNI003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM SYS_MT_UNIFORM");
                obj_str.Append(" WHERE UNIFORM_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "UNI004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTUniform model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.uniform_code))
                {
                    if (this.update(model))
                        return model.uniform_id.ToString();
                    else
                        return "";
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_MT_UNIFORM");
                obj_str.Append(" (");
                obj_str.Append("UNIFORM_ID ");
                obj_str.Append(", UNIFORM_CODE ");
                obj_str.Append(", UNIFORM_NAME_TH ");
                obj_str.Append(", UNIFORM_NAME_EN ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@UNIFORM_ID ");
                obj_str.Append(", @UNIFORM_CODE ");
                obj_str.Append(", @UNIFORM_NAME_TH ");
                obj_str.Append(", @UNIFORM_NAME_EN ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.uniform_id = this.getNextID();

                obj_cmd.Parameters.Add("@UNIFORM_ID", SqlDbType.Int); obj_cmd.Parameters["@UNIFORM_ID"].Value = model.uniform_id;
                obj_cmd.Parameters.Add("@UNIFORM_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@UNIFORM_CODE"].Value = model.uniform_code;
                obj_cmd.Parameters.Add("@UNIFORM_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@UNIFORM_NAME_TH"].Value = model.uniform_name_th;
                obj_cmd.Parameters.Add("@UNIFORM_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@UNIFORM_NAME_EN"].Value = model.uniform_name_en;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                strResult = model.uniform_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "UNI005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTUniform model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE SYS_MT_UNIFORM SET ");
                obj_str.Append(" UNIFORM_CODE=@UNIFORM_CODE ");
                obj_str.Append(", UNIFORM_NAME_TH=@UNIFORM_NAME_TH ");
                obj_str.Append(", UNIFORM_NAME_EN=@UNIFORM_NAME_EN ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE UNIFORM_ID=@UNIFORM_ID ");



                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@UNIFORM_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@UNIFORM_CODE"].Value = model.uniform_code;
                obj_cmd.Parameters.Add("@UNIFORM_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@UNIFORM_NAME_TH"].Value = model.uniform_name_th;
                obj_cmd.Parameters.Add("@UNIFORM_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@UNIFORM_NAME_EN"].Value = model.uniform_name_en;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@UNIFORM_ID", SqlDbType.Int); obj_cmd.Parameters["@UNIFORM_ID"].Value = model.uniform_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "UNI006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
