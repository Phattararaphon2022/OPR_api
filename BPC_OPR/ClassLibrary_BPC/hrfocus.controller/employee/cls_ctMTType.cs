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
    public class cls_ctMTType
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTType() { }

        public string getMessage() { return this.Message.Replace("EMP_MT_TYPE", "").Replace("cls_ctMTType", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTType> getData(string condition)
        {
            List<cls_MTType> list_model = new List<cls_MTType>();
            cls_MTType model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("TYPE_ID");
                obj_str.Append(", TYPE_CODE");
                obj_str.Append(", TYPE_NAME_TH");
                obj_str.Append(", TYPE_NAME_EN");
                obj_str.Append(", COMPANY_CODE");
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_MT_TYPE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY TYPE_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTType();

                    model.type_id = Convert.ToInt32(dr["TYPE_ID"]);
                    model.type_code = dr["TYPE_CODE"].ToString();
                    model.type_name_th = dr["TYPE_NAME_TH"].ToString();
                    model.type_name_en = dr["TYPE_NAME_EN"].ToString();
                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "TYP001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTType> getDataByFillter(string code,string com)
        {
            string strCondition = "";
            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";
            if (!code.Equals(""))
                strCondition += " AND TYPE_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(TYPE_ID, 1) ");
                obj_str.Append(" FROM EMP_MT_TYPE");
                obj_str.Append(" ORDER BY TYPE_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "TYP002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string id,string code,string com)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT TYPE_CODE");
                obj_str.Append(" FROM EMP_MT_TYPE");
                obj_str.Append(" WHERE TYPE_ID='" + id + "'");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND TYPE_CODE='" + code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "TYP003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string code , string com)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM EMP_MT_TYPE");
                obj_str.Append(" WHERE TYPE_CODE='" + code + "'");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");


                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "TYP004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTType model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.type_id.ToString(),model.type_code,model.company_code))
                {
                    if (this.update(model))
                        return model.type_id.ToString();
                    else
                        return "";
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_MT_TYPE");
                obj_str.Append(" (");
                obj_str.Append("TYPE_ID ");
                obj_str.Append(", TYPE_CODE ");
                obj_str.Append(", TYPE_NAME_TH ");
                obj_str.Append(", TYPE_NAME_EN ");
                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@TYPE_ID ");
                obj_str.Append(", @TYPE_CODE ");
                obj_str.Append(", @TYPE_NAME_TH ");
                obj_str.Append(", @TYPE_NAME_EN ");
                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.type_id = this.getNextID();

                obj_cmd.Parameters.Add("@TYPE_ID", SqlDbType.Int); obj_cmd.Parameters["@TYPE_ID"].Value = model.type_id;
                obj_cmd.Parameters.Add("@TYPE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@TYPE_CODE"].Value = model.type_code;
                obj_cmd.Parameters.Add("@TYPE_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@TYPE_NAME_TH"].Value = model.type_name_th;
                obj_cmd.Parameters.Add("@TYPE_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@TYPE_NAME_EN"].Value = model.type_name_en;
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                strResult = model.type_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "TYP005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTType model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE EMP_MT_TYPE SET ");
                obj_str.Append(" TYPE_NAME_TH=@TYPE_NAME_TH ");
                obj_str.Append(", TYPE_NAME_EN=@TYPE_NAME_EN ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE TYPE_ID=@TYPE_ID ");
                obj_str.Append(" AND COMPANY_CODE=@COMPANY_CODE ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@TYPE_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@TYPE_NAME_TH"].Value = model.type_name_th;
                obj_cmd.Parameters.Add("@TYPE_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@TYPE_NAME_EN"].Value = model.type_name_en;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@TYPE_ID", SqlDbType.Int); obj_cmd.Parameters["@TYPE_ID"].Value = model.type_id;
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "TYP006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
