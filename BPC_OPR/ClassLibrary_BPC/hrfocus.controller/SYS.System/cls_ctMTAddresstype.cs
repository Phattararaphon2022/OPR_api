using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller
{
   public class cls_ctMTAddresstype
     {
     string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTAddresstype() { }

        public string getMessage() { return this.Message.Replace("SYS_MT_ADDRESSTYPE", "").Replace("cls_ctMTAddresstype", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTAddresstype> getData(string condition)
        {
            List<cls_MTAddresstype> list_model = new List<cls_MTAddresstype>();
            cls_MTAddresstype model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                 obj_str.Append("  ADDRESSTYPE_ID");
                obj_str.Append(", ADDRESSTYPE_CODE");
                obj_str.Append(", ISNULL(ADDRESSTYPE_NAME_TH, '') AS ADDRESSTYPE_NAME_TH");
                obj_str.Append(", ISNULL(ADDRESSTYPE_NAME_EN, '') AS ADDRESSTYPE_NAME_EN");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM SYS_MT_ADDRESSTYPE");
                obj_str.Append(" WHERE 1=1");
                
                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY ADDRESSTYPE_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTAddresstype();
 
                    model.addresstype_id = Convert.ToInt32(dr["ADDRESSTYPE_ID"]);
                    model.addresstype_code = dr["ADDRESSTYPE_CODE"].ToString();
                    model.addresstype_name_th = dr["ADDRESSTYPE_NAME_TH"].ToString();
                    model.addresstype_name_en = dr["ADDRESSTYPE_NAME_EN"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "ADD001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTAddresstype> getDataByFillter(  string code)
        {
            string strCondition = "";
 
            if (!code.Equals(""))
                strCondition += " AND ADDRESSTYPE_CODE='" + code + "'";
            
            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(ADDRESSTYPE_ID, 1) ");
                obj_str.Append(" FROM SYS_MT_ADDRESSTYPE");

                obj_str.Append(" ORDER BY ADDRESSTYPE_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ADD002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(  string code, string id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ADDRESSTYPE_CODE");
                obj_str.Append(" FROM SYS_MT_ADDRESSTYPE");
                obj_str.Append(" WHERE 1=1 ");

                obj_str.Append(" AND ADDRESSTYPE_CODE='" + code + "'");

                 obj_str.Append(" AND ADDRESSTYPE_ID='" + id + "'");

       
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ADD003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(  string code)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM SYS_MT_ADDRESSTYPE");

                obj_str.Append(" WHERE ADDRESSTYPE_CODE='" + code + "'");
 
                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ADD004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTAddresstype model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(  model.addresstype_code, model.addresstype_id.ToString()))
                {
                    if (this.update(model))
                        return model.addresstype_id.ToString();
                    else
                        return "";                    
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();

                obj_str.Append("INSERT INTO SYS_MT_ADDRESSTYPE");
                obj_str.Append(" (");
 
                obj_str.Append("  ADDRESSTYPE_ID ");
                obj_str.Append(", ADDRESSTYPE_CODE ");
                obj_str.Append(", ADDRESSTYPE_NAME_TH ");
                obj_str.Append(", ADDRESSTYPE_NAME_EN ");               
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");          
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
 
                obj_str.Append("  @ADDRESSTYPE_ID ");
                obj_str.Append(", @ADDRESSTYPE_CODE ");
                obj_str.Append(", @ADDRESSTYPE_NAME_TH ");
                obj_str.Append(", @ADDRESSTYPE_NAME_EN ");      
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");
                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                //model.addresstype_id = this.getNextID();
 
                obj_cmd.Parameters.Add("@ADDRESSTYPE_ID", SqlDbType.Int); obj_cmd.Parameters["@ADDRESSTYPE_ID"].Value = id;
                obj_cmd.Parameters.Add("@ADDRESSTYPE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@ADDRESSTYPE_CODE"].Value = model.addresstype_code;
                obj_cmd.Parameters.Add("@ADDRESSTYPE_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@ADDRESSTYPE_NAME_TH"].Value = model.addresstype_name_th;
                obj_cmd.Parameters.Add("@ADDRESSTYPE_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@ADDRESSTYPE_NAME_EN"].Value = model.addresstype_name_en;        
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                                     
                obj_cmd.ExecuteNonQuery();
                                
                obj_conn.doClose();
                strResult = model.addresstype_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ADD005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTAddresstype model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE SYS_MT_ADDRESSTYPE SET ");
                //obj_str.Append(" ADDRESSTYPE_CODE=@ADDRESSTYPE_CODE ");
                obj_str.Append("  ADDRESSTYPE_NAME_TH=@ADDRESSTYPE_NAME_TH ");
                obj_str.Append(", ADDRESSTYPE_NAME_EN=@ADDRESSTYPE_NAME_EN ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE ADDRESSTYPE_CODE=@ADDRESSTYPE_CODE ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());


                obj_cmd.Parameters.Add("@ADDRESSTYPE_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@ADDRESSTYPE_NAME_TH"].Value = model.addresstype_name_th;
                obj_cmd.Parameters.Add("@ADDRESSTYPE_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@ADDRESSTYPE_NAME_EN"].Value = model.addresstype_name_en;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                //obj_cmd.Parameters.Add("@ADDRESSTYPE_ID", SqlDbType.Int); obj_cmd.Parameters["@ADDRESSTYPE_ID"].Value = model.addresstype_id;
                obj_cmd.Parameters.Add("@ADDRESSTYPE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@ADDRESSTYPE_CODE"].Value = model.addresstype_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ADD006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}
