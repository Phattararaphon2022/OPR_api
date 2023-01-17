using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTInitial
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTInitial() { }

        public string getMessage() { return this.Message.Replace("EMP_MT_Initial", "").Replace("cls_ctMTInitial", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTInitial> getData(string condition)
        {
            List<cls_MTInitial> list_model = new List<cls_MTInitial>();
            cls_MTInitial model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");
                obj_str.Append("INITIAL_ID");
                obj_str.Append(", INITIAL_CODE");
                obj_str.Append(", ISNULL(INITIAL_NAME_TH, '') AS INITIAL_NAME_TH");
                obj_str.Append(", ISNULL(INITIAL_NAME_EN, '') AS INITIAL_NAME_EN");
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");
                obj_str.Append(" FROM EMP_MT_INITIAL");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY INITIAL_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTInitial();

                    model.initial_id = Convert.ToInt32(dr["INITIAL_ID"]);
                    model.initial_code = dr["INITIAL_CODE"].ToString();
                    model.initial_name_th = dr["INITIAL_NAME_TH"].ToString();
                    model.initial_name_en = dr["INITIAL_NAME_EN"].ToString();
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "INI001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTInitial> getDataByFillter(string code)
        {
            string strCondition = "";

            if (!code.Equals(""))
                strCondition += " AND INITIAL_CODE ='" + code + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(INITIAL_ID, 1) ");
                obj_str.Append(" FROM EMP_MT_INITIAL");
                obj_str.Append(" ORDER BY INITIAL_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "INI002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT INITIAL_CODE");
                obj_str.Append(" FROM EMP_MT_INITIAL");
                obj_str.Append(" WHERE INITIAL_CODE='" + code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "INI003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM EMP_MT_INITIAL");
                obj_str.Append(" WHERE INITIAL_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "INI004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTInitial model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.initial_code))
                {
                    if (this.update(model))
                        return model.initial_id.ToString();
                    else
                        return "";
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_MT_INITIAL");
                obj_str.Append(" (");
                obj_str.Append("INITIAL_ID ");
                obj_str.Append(", INITIAL_CODE ");
                obj_str.Append(", INITIAL_NAME_TH ");
                obj_str.Append(", INITIAL_NAME_EN ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");
                obj_str.Append(" VALUES(");
                obj_str.Append(" @INITIAL_ID ");
                obj_str.Append(", @INITIAL_CODE ");
                obj_str.Append(", @INITIAL_NAME_TH ");
                obj_str.Append(", @INITIAL_NAME_EN ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", 1 ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.initial_id = this.getNextID();

                obj_cmd.Parameters.Add("@INITIAL_ID", SqlDbType.Int); obj_cmd.Parameters["@INITIAL_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@INITIAL_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@INITIAL_CODE"].Value = model.initial_code;
                obj_cmd.Parameters.Add("@INITIAL_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@INITIAL_NAME_TH"].Value = model.initial_name_th;
                obj_cmd.Parameters.Add("@INITIAL_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@INITIAL_NAME_EN"].Value = model.initial_name_en;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                strResult = model.initial_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "INI005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTInitial model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE EMP_MT_INITIAL SET ");
                obj_str.Append(" INITIAL_CODE=@INITIAL_CODE ");
                obj_str.Append(", INITIAL_NAME_TH=@INITIAL_NAME_TH ");
                obj_str.Append(", INITIAL_NAME_EN=@INITIAL_NAME_EN ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE INITIAL_ID=@INITIAL_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@INITIAL_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@INITIAL_CODE"].Value = model.initial_code;
                obj_cmd.Parameters.Add("@INITIAL_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@INITIAL_NAME_TH"].Value = model.initial_name_th;
                obj_cmd.Parameters.Add("@INITIAL_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@INITIAL_NAME_EN"].Value = model.initial_name_en;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@INITIAL_ID", SqlDbType.Int); obj_cmd.Parameters["@INITIAL_ID"].Value = model.initial_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "INI006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}


