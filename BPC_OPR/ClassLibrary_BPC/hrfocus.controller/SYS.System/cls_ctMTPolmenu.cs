using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;
namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTPolmenu
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTPolmenu() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTPolmenu> getData(string condition)
        {
            List<cls_MTPolmenu> list_model = new List<cls_MTPolmenu>();
            cls_MTPolmenu model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("POLMENU_ID");
                obj_str.Append(", POLMENU_CODE");
                obj_str.Append(", POLMENU_NAME_TH");
                obj_str.Append(", POLMENU_NAME_EN");
                obj_str.Append(", COMPANY_CODE");
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");
                obj_str.Append(", FLAG");

                obj_str.Append(" FROM SYS_MT_POLMENU");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY POLMENU_ID");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTPolmenu();

                    model.polmenu_id = Convert.ToInt32(dr["POLMENU_ID"]);
                    model.polmenu_code = dr["POLMENU_CODE"].ToString();
                    model.polmenu_name_th = dr["POLMENU_NAME_TH"].ToString();
                    model.polmenu_name_en = dr["POLMENU_NAME_EN"].ToString();
                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                    model.flag = Convert.ToBoolean(dr["FLAG"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Polmenu.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTPolmenu> getDataByFillter(string com,int id,string code)
        {
            string strCondition = "";
            if(!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!id.Equals(0))
                strCondition += " AND POLMENU_ID='" + id + "'";

            if (!code.Equals(""))
                strCondition += " AND POLMENU_CODE='" + code + "'";


            return this.getData(strCondition);
        }

        public bool checkDataOld(string com,int id, string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT POLMENU_ID");
                obj_str.Append(" FROM SYS_MT_POLMENU");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                if (!id.Equals(0))
                    obj_str.Append(" AND POLMENU_ID='" + id + "' ");
                if (!code.Equals(""))
                    obj_str.Append(" AND POLMENU_CODE='" + code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Polmenu.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(string com, int id, string code)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SYS_MT_POLMENU");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                if (!id.Equals(0))
                    obj_str.Append(" AND POLMENU_ID='" + id + "' ");
                if (!code.Equals(""))
                    obj_str.Append(" AND POLMENU_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Polmenu.delete)" + ex.ToString();
            }

            return blnResult;
        }
        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(POLMENU_ID) ");
                obj_str.Append(" FROM SYS_MT_POLMENU");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Polmenu.getNextID)" + ex.ToString();
            }

            return intResult;
        }
        public string insert(cls_MTPolmenu model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code,model.polmenu_id,model.polmenu_code))
                {
                    return this.update(model);
                }
                int id = getNextID();
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("INSERT INTO SYS_MT_POLMENU");
                obj_str.Append(" (");
                obj_str.Append("POLMENU_ID ");
                obj_str.Append(", POLMENU_CODE ");
                obj_str.Append(", POLMENU_NAME_TH ");
                obj_str.Append(", POLMENU_NAME_EN ");
                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@POLMENU_ID ");
                obj_str.Append(", @POLMENU_CODE ");
                obj_str.Append(", @POLMENU_NAME_TH ");
                obj_str.Append(", @POLMENU_NAME_EN ");
                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@POLMENU_ID", SqlDbType.Int); obj_cmd.Parameters["@POLMENU_ID"].Value = id;
                obj_cmd.Parameters.Add("@POLMENU_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@POLMENU_CODE"].Value = model.polmenu_code;
                obj_cmd.Parameters.Add("@POLMENU_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@POLMENU_NAME_TH"].Value = model.polmenu_name_th;
                obj_cmd.Parameters.Add("@POLMENU_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@POLMENU_NAME_EN"].Value = model.polmenu_name_en;
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.Int); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.Int); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = model.polmenu_code;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Polmenu.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_MTPolmenu model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SYS_MT_POLMENU SET ");
                obj_str.Append(" POLMENU_ID=@POLMENU_ID ");
                obj_str.Append(", POLMENU_CODE=@POLMENU_CODE ");
                obj_str.Append(", POLMENU_NAME_TH=@POLMENU_NAME_TH ");
                obj_str.Append(", POLMENU_NAME_EN=@POLMENU_NAME_EN ");
                obj_str.Append(", COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");
                obj_str.Append(" WHERE POLMENU_ID=@POLMENU_ID ");
      

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());


                obj_cmd.Parameters.Add("@POLMENU_ID", SqlDbType.Int); obj_cmd.Parameters["@POLMENU_ID"].Value = model.polmenu_id;
                obj_cmd.Parameters.Add("@POLMENU_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@POLMENU_CODE"].Value = model.polmenu_code;
                obj_cmd.Parameters.Add("@POLMENU_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@POLMENU_NAME_TH"].Value = model.polmenu_name_th;
                obj_cmd.Parameters.Add("@POLMENU_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@POLMENU_NAME_EN"].Value = model.polmenu_name_en;
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now ;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.polmenu_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Polmenu.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
