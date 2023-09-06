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
    public class cls_ctMTGroup
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTGroup() { }

        public string getMessage() { return this.Message.Replace("EMP_MT_GROUP", "").Replace("cls_ctMTGroup", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTGroup> getData(string condition)
        {
            List<cls_MTGroup> list_model = new List<cls_MTGroup>();
            cls_MTGroup model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("GROUP_ID");
                obj_str.Append(", GROUP_CODE");
                obj_str.Append(", GROUP_NAME_TH");
                obj_str.Append(", GROUP_NAME_EN");
                obj_str.Append(", COMPANY_CODE");
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_MT_GROUP");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY GROUP_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTGroup();

                    model.group_id = Convert.ToInt32(dr["GROUP_ID"]);
                    model.group_code = dr["GROUP_CODE"].ToString();
                    model.group_name_th = dr["GROUP_NAME_TH"].ToString();
                    model.group_name_en = dr["GROUP_NAME_EN"].ToString();
                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "GRP001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTGroup> getDataByFillter(string code, string com)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";
            if (!code.Equals(""))
                strCondition += " AND GROUP_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(GROUP_ID, 1) ");
                obj_str.Append(" FROM EMP_MT_GROUP");
                obj_str.Append(" ORDER BY GROUP_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "GRP002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string id,string code,string com)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT GROUP_CODE");
                obj_str.Append(" FROM EMP_MT_GROUP");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND GROUP_CODE='" + code + "'");
                if (!id.ToString().Equals(""))
                {
                    obj_str.Append(" AND GROUP_ID='" + id + "'");
                }

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "GRP003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM EMP_MT_GROUP");
                obj_str.Append(" WHERE GROUP_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "GRP004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTGroup model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.group_id.ToString(),model.group_code,model.company_code))
                {
                    if (this.update(model))
                        return model.group_id.ToString();
                    else
                        return "";
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_MT_GROUP");
                obj_str.Append(" (");
                obj_str.Append("GROUP_ID ");
                obj_str.Append(", GROUP_CODE ");
                obj_str.Append(", GROUP_NAME_TH ");
                obj_str.Append(", GROUP_NAME_EN ");
                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@GROUP_ID ");
                obj_str.Append(", @GROUP_CODE ");
                obj_str.Append(", @GROUP_NAME_TH ");
                obj_str.Append(", @GROUP_NAME_EN ");
                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.group_id = this.getNextID();

                obj_cmd.Parameters.Add("@GROUP_ID", SqlDbType.Int); obj_cmd.Parameters["@GROUP_ID"].Value = model.group_id;
                obj_cmd.Parameters.Add("@GROUP_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@GROUP_CODE"].Value = model.group_code;
                obj_cmd.Parameters.Add("@GROUP_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@GROUP_NAME_TH"].Value = model.group_name_th;
                obj_cmd.Parameters.Add("@GROUP_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@GROUP_NAME_EN"].Value = model.group_name_en;
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                strResult = model.group_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "GRP005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTGroup model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE EMP_MT_GROUP SET ");
                obj_str.Append(" GROUP_NAME_TH=@GROUP_NAME_TH ");
                obj_str.Append(", GROUP_NAME_EN=@GROUP_NAME_EN ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE GROUP_ID=@GROUP_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@GROUP_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@GROUP_NAME_TH"].Value = model.group_name_th;
                obj_cmd.Parameters.Add("@GROUP_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@GROUP_NAME_EN"].Value = model.group_name_en;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@GROUP_ID", SqlDbType.Int); obj_cmd.Parameters["@GROUP_ID"].Value = model.group_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "GRP006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
