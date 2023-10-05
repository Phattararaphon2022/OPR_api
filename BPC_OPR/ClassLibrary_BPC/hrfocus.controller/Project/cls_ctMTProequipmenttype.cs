using ClassLibrary_BPC.hrfocus.model.Project;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller.Project
{
    public class cls_ctMTProequipmenttype
     {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTProequipmenttype() { }

        public string getMessage() { return this.Message.Replace("PRO_MT_PROEQUIPMENTTYPE", "").Replace("cls_ctMTProequipmenttype", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTProequipmenttype> getData(string condition)
        {
            List<cls_MTProequipmenttype> list_model = new List<cls_MTProequipmenttype>();
            cls_MTProequipmenttype model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("  PROEQUIPMENTTYPE_ID");
                obj_str.Append(", PROEQUIPMENTTYPE_CODE");
                obj_str.Append(", PROEQUIPMENTTYPE_NAME_TH");
                obj_str.Append(", PROEQUIPMENTTYPE_NAME_EN");             
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PRO_MT_PROEQUIPMENTTYPE");
                obj_str.Append(" WHERE 1=1");
                
                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PROEQUIPMENTTYPE_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTProequipmenttype();

                    model.proequipmenttype_id = Convert.ToInt32(dr["PROEQUIPMENTTYPE_ID"]);
                    model.proequipmenttype_code = dr["PROEQUIPMENTTYPE_CODE"].ToString();
                    model.proequipmenttype_name_th = dr["PROEQUIPMENTTYPE_NAME_TH"].ToString();
                    model.proequipmenttype_name_en = dr["PROEQUIPMENTTYPE_NAME_EN"].ToString();                    
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                                                                                            
                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "PROEQUIPMENTTYPE001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTProequipmenttype> getDataByFillter(  string code)
        {
            string strCondition = "";
 
            if (!code.Equals(""))
                strCondition += " AND PROEQUIPMENTTYPE_CODE='" + code + "'";
            
            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(PROEQUIPMENTTYPE_ID, 1) ");
                obj_str.Append(" FROM PRO_MT_PROEQUIPMENTTYPE");
                obj_str.Append(" ORDER BY PROEQUIPMENTTYPE_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "PROEQUIPMENTTYPE002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string code )
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PROEQUIPMENTTYPE_CODE");
                obj_str.Append(" FROM PRO_MT_PROEQUIPMENTTYPE");
                obj_str.Append(" WHERE PROEQUIPMENTTYPE_CODE='" + code + "'");
 
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "PROEQUIPMENTTYPE003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string code )
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_MT_PROEQUIPMENTTYPE");
                obj_str.Append(" WHERE PROEQUIPMENTTYPE_CODE='" + code + "'");
 
                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "PROEQUIPMENTTYPE004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTProequipmenttype model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.proequipmenttype_code))
                {
                    if (this.update(model))
                        return model.proequipmenttype_id.ToString();
                    else
                        return "";                    
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PRO_MT_PROEQUIPMENTTYPE");
                obj_str.Append(" (");

                obj_str.Append("  PROEQUIPMENTTYPE_ID ");
                obj_str.Append(", PROEQUIPMENTTYPE_CODE ");
                obj_str.Append(", PROEQUIPMENTTYPE_NAME_TH ");
                obj_str.Append(", PROEQUIPMENTTYPE_NAME_EN");               
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");          
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
 
                obj_str.Append("  @PROEQUIPMENTTYPE_ID ");
                obj_str.Append(", @PROEQUIPMENTTYPE_CODE ");
                obj_str.Append(", @PROEQUIPMENTTYPE_NAME_TH ");
                obj_str.Append(", @PROEQUIPMENTTYPE_NAME_EN ");      
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");
                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.proequipmenttype_id = this.getNextID();

                obj_cmd.Parameters.Add("@PROEQUIPMENTTYPE_ID", SqlDbType.Int); obj_cmd.Parameters["@PROEQUIPMENTTYPE_ID"].Value = model.proequipmenttype_id;
                obj_cmd.Parameters.Add("@PROEQUIPMENTTYPE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROEQUIPMENTTYPE_CODE"].Value = model.proequipmenttype_code;
                obj_cmd.Parameters.Add("@PROEQUIPMENTTYPE_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROEQUIPMENTTYPE_NAME_TH"].Value = model.proequipmenttype_name_th;
                obj_cmd.Parameters.Add("@PROEQUIPMENTTYPE_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROEQUIPMENTTYPE_NAME_EN"].Value = model.proequipmenttype_name_en;        
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                                     
                obj_cmd.ExecuteNonQuery();
                                
                obj_conn.doClose();
                strResult = model.proequipmenttype_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "PROEQUIPMENTTYPE005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTProequipmenttype model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PRO_MT_PROEQUIPMENTTYPE SET ");
                obj_str.Append(" PROEQUIPMENTTYPE_NAME_TH=@PROEQUIPMENTTYPE_NAME_TH ");
                obj_str.Append(", PROEQUIPMENTTYPE_NAME_EN=@PROEQUIPMENTTYPE_NAME_EN ");               
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE PROEQUIPMENTTYPE_ID=@PROEQUIPMENTTYPE_ID ");            

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROEQUIPMENTTYPE_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROEQUIPMENTTYPE_NAME_TH"].Value = model.proequipmenttype_name_th;
                obj_cmd.Parameters.Add("@PROEQUIPMENTTYPE_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROEQUIPMENTTYPE_NAME_EN"].Value = model.proequipmenttype_name_en;        
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@PROEQUIPMENTTYPE_ID", SqlDbType.Int); obj_cmd.Parameters["@PROEQUIPMENTTYPE_ID"].Value = model.proequipmenttype_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "PROEQUIPMENTTYPE006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}
