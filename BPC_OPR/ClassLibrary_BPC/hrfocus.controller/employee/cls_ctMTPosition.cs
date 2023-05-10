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
    public class cls_ctMTPosition
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTPosition() { }

        public string getMessage() { return this.Message.Replace("EMP_MT_POSITION", "").Replace("cls_ctMTPosition", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTPosition> getData(string condition)
        {
            List<cls_MTPosition> list_model = new List<cls_MTPosition>();
            cls_MTPosition model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("POSITION_ID");
                obj_str.Append(", POSITION_CODE");
                obj_str.Append(", ISNULL(POSITION_NAME_TH, '') AS POSITION_NAME_TH");
                obj_str.Append(", ISNULL(POSITION_NAME_EN, '') AS POSITION_NAME_EN");
                obj_str.Append(", COMPANY_CODE");
                obj_str.Append(", ISNULL(POSITION_LEVEL, 0) AS POSITION_LEVEL");
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_MT_POSITION");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY POSITION_LEVEL");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTPosition();

                    model.position_id = Convert.ToInt32(dr["POSITION_ID"]);
                    model.position_code = dr["POSITION_CODE"].ToString();
                    model.position_name_th = dr["POSITION_NAME_TH"].ToString();
                    model.position_name_en = dr["POSITION_NAME_EN"].ToString();
                    model.position_level = Convert.ToInt32(dr["POSITION_LEVEL"]);
                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "PST001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTPosition> getDataByFillter(string code)
        {
            string strCondition = "";

            if (!code.Equals(""))
                strCondition += " AND POSITION_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(POSITION_ID, 1) ");
                obj_str.Append(" FROM EMP_MT_POSITION");
                obj_str.Append(" ORDER BY POSITION_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "PST002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT POSITION_CODE");
                obj_str.Append(" FROM EMP_MT_POSITION");
                obj_str.Append(" WHERE POSITION_CODE='" + code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "PST003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM EMP_MT_POSITION");
                obj_str.Append(" WHERE POSITION_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "PST004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTPosition model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.position_code))
                {
                    if (this.update(model))
                        return model.position_id.ToString();
                    else
                        return "";
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_MT_POSITION");
                obj_str.Append(" (");
                obj_str.Append("POSITION_ID ");
                obj_str.Append(", POSITION_CODE ");
                obj_str.Append(", POSITION_NAME_TH ");
                obj_str.Append(", POSITION_NAME_EN ");
                obj_str.Append(", POSITION_LEVEL ");
                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append(" @POSITION_ID ");
                obj_str.Append(", @POSITION_CODE ");
                obj_str.Append(", @POSITION_NAME_TH ");
                obj_str.Append(", @POSITION_NAME_EN ");
                obj_str.Append(", @POSITION_LEVEL ");
                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", 1 ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.position_id = this.getNextID();

                obj_cmd.Parameters.Add("@POSITION_ID", SqlDbType.Int); obj_cmd.Parameters["@POSITION_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@POSITION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@POSITION_CODE"].Value = model.position_code;
                obj_cmd.Parameters.Add("@POSITION_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@POSITION_NAME_TH"].Value = model.position_name_th;
                obj_cmd.Parameters.Add("@POSITION_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@POSITION_NAME_EN"].Value = model.position_name_en;
                obj_cmd.Parameters.Add("@POSITION_LEVEL", SqlDbType.Int); obj_cmd.Parameters["@POSITION_LEVEL"].Value = model.position_level;
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                strResult = model.position_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "PST005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTPosition model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE EMP_MT_POSITION SET ");
                obj_str.Append(" POSITION_CODE=@POSITION_CODE ");
                obj_str.Append(", POSITION_NAME_TH=@POSITION_NAME_TH ");
                obj_str.Append(", POSITION_NAME_EN=@POSITION_NAME_EN ");
                obj_str.Append(", POSITION_LEVEL=@POSITION_LEVEL ");
                obj_str.Append(", COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE POSITION_ID=@POSITION_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@POSITION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@POSITION_CODE"].Value = model.position_code;
                obj_cmd.Parameters.Add("@POSITION_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@POSITION_NAME_TH"].Value = model.position_name_th;
                obj_cmd.Parameters.Add("@POSITION_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@POSITION_NAME_EN"].Value = model.position_name_en;
                obj_cmd.Parameters.Add("@POSITION_LEVEL", SqlDbType.Int); obj_cmd.Parameters["@POSITION_LEVEL"].Value = model.position_level;
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@POSITION_ID", SqlDbType.Int); obj_cmd.Parameters["@POSITION_ID"].Value = model.position_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "PST006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
