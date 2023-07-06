using ClassLibrary_BPC.hrfocus.model.SYS.System;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTQualification
  {
     string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTQualification() { }

        public string getMessage() { return this.Message.Replace("SYS_MT_QUALIFICATION", "").Replace("cls_ctMTQualification", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTQualification> getData(string condition)
        {
            List<cls_MTQualification> list_model = new List<cls_MTQualification>();
            cls_MTQualification model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("QUALIFICATION_ID");
                obj_str.Append(", QUALIFICATION_CODE");
                obj_str.Append(", ISNULL(QUALIFICATION_NAME_TH, '') AS QUALIFICATION_NAME_TH");
                obj_str.Append(", ISNULL(QUALIFICATION_NAME_EN, '') AS QUALIFICATION_NAME_EN");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM SYS_MT_QUALIFICATION");
                obj_str.Append(" WHERE 1=1");
                
                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY QUALIFICATION_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTQualification();

                    model.qualification_id = Convert.ToInt32(dr["QUALIFICATION_ID"]);
                    model.qualification_code = dr["QUALIFICATION_CODE"].ToString();
                    model.qualification_name_th = dr["QUALIFICATION_NAME_TH"].ToString();
                    model.qualification_name_en = dr["QUALIFICATION_NAME_EN"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "QUA001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTQualification> getDataByFillter(string code)
        {
            string strCondition = "";

            if (!code.Equals(""))
                strCondition += " AND QUALIFICATION_CODE='" + code + "'";
            
            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(QUALIFICATION_ID, 1) ");
                obj_str.Append(" FROM SYS_MT_QUALIFICATION");
                obj_str.Append(" ORDER BY QUALIFICATION_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "QUA002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string code, string id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT QUALIFICATION_CODE");
                obj_str.Append(" FROM SYS_MT_QUALIFICATION");
                obj_str.Append(" WHERE QUALIFICATION_CODE='" + code + "'");
                obj_str.Append(" AND QUALIFICATION_ID='" + id + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "QUA003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM SYS_MT_QUALIFICATION");
                obj_str.Append(" WHERE QUALIFICATION_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "QUA004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTQualification model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.qualification_code, model.qualification_id.ToString()))
                {
                    if (this.update(model))
                        return model.qualification_id.ToString();
                    else
                        return "";                    
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_MT_QUALIFICATION");
                obj_str.Append(" (");
                obj_str.Append("QUALIFICATION_ID ");
                obj_str.Append(", QUALIFICATION_CODE ");
                obj_str.Append(", QUALIFICATION_NAME_TH ");
                obj_str.Append(", QUALIFICATION_NAME_EN ");               
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");          
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@QUALIFICATION_ID ");
                obj_str.Append(", @QUALIFICATION_CODE ");
                obj_str.Append(", @QUALIFICATION_NAME_TH ");
                obj_str.Append(", @QUALIFICATION_NAME_EN ");      
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");
                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.qualification_id = this.getNextID();

                obj_cmd.Parameters.Add("@QUALIFICATION_ID", SqlDbType.Int); obj_cmd.Parameters["@QUALIFICATION_ID"].Value = model.qualification_id;
                obj_cmd.Parameters.Add("@QUALIFICATION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@QUALIFICATION_CODE"].Value = model.qualification_code;
                obj_cmd.Parameters.Add("@QUALIFICATION_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@QUALIFICATION_NAME_TH"].Value = model.qualification_name_th;
                obj_cmd.Parameters.Add("@QUALIFICATION_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@QUALIFICATION_NAME_EN"].Value = model.qualification_name_en;        
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                                     
                obj_cmd.ExecuteNonQuery();
                                
                obj_conn.doClose();
                strResult = model.qualification_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "QUA005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTQualification model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE SYS_MT_QUALIFICATION SET ");
                //obj_str.Append(" QUALIFICATION_CODE=@QUALIFICATION_CODE ");

                obj_str.Append(" QUALIFICATION_NAME_TH=@QUALIFICATION_NAME_TH ");
                obj_str.Append(", QUALIFICATION_NAME_EN=@QUALIFICATION_NAME_EN ");               
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE QUALIFICATION_CODE=@QUALIFICATION_CODE ");            

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                //obj_cmd.Parameters.Add("@QUALIFICATION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@QUALIFICATION_CODE"].Value = model.qualification_code;

                obj_cmd.Parameters.Add("@QUALIFICATION_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@QUALIFICATION_NAME_TH"].Value = model.qualification_name_th;
                obj_cmd.Parameters.Add("@QUALIFICATION_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@QUALIFICATION_NAME_EN"].Value = model.qualification_name_en;        
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@QUALIFICATION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@QUALIFICATION_CODE"].Value = model.qualification_code;

                //obj_cmd.Parameters.Add("@QUALIFICATION_ID", SqlDbType.Int); obj_cmd.Parameters["@QUALIFICATION_ID"].Value = model.qualification_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "QUA006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}
