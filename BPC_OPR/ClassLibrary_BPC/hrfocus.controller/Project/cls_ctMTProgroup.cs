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
    public class cls_ctMTProgroup
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTProgroup() { }

        public string getMessage() { return this.Message.Replace("PRO_MT_PROGROUP", "").Replace("cls_ctMTGroup", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTProgroup> getData(string condition)
        {
            List<cls_MTProgroup> list_model = new List<cls_MTProgroup>();
            cls_MTProgroup model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");
                obj_str.Append("COMPANY_CODE");

                obj_str.Append(", PROGROUP_ID");
                obj_str.Append(", PROGROUP_CODE");
                obj_str.Append(", PROGROUP_NAME_TH");
                obj_str.Append(", PROGROUP_NAME_EN");             
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PRO_MT_PROGROUP");
                obj_str.Append(" WHERE 1=1");
                
                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PROGROUP_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTProgroup();
                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);

                    model.progroup_id = Convert.ToInt32(dr["PROGROUP_ID"]);
                    model.progroup_code = dr["PROGROUP_CODE"].ToString();
                    model.progroup_name_th = dr["PROGROUP_NAME_TH"].ToString();
                    model.progroup_name_en = dr["PROGROUP_NAME_EN"].ToString();                    
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                                                                                            
                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "PROGROUP001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTProgroup> getDataByFillter(string com, string code)
        {
            string strCondition = "";
            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!code.Equals(""))
                strCondition += " AND PROGROUP_CODE='" + code + "'";
            
            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(PROGROUP_ID, 1) ");
                obj_str.Append(" FROM PRO_MT_PROGROUP");
                obj_str.Append(" ORDER BY PROGROUP_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "PROGROUP002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string code, string com)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PROGROUP_CODE");
                obj_str.Append(" FROM PRO_MT_PROGROUP");
                obj_str.Append(" WHERE PROGROUP_CODE='" + code + "'");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "PROGROUP003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string code, string com)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_MT_PROGROUP");
                obj_str.Append(" WHERE PROGROUP_CODE='" + code + "'");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "PROGROUP004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTProgroup model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.progroup_code, model.company_code))
                {
                    if (this.update(model))
                        return model.progroup_id.ToString();
                    else
                        return "";                    
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PRO_MT_PROGROUP");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");

                obj_str.Append(", PROGROUP_ID ");
                obj_str.Append(", PROGROUP_CODE ");
                obj_str.Append(", PROGROUP_NAME_TH ");
                obj_str.Append(", PROGROUP_NAME_EN ");               
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");          
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");

                obj_str.Append(", @PROGROUP_ID ");
                obj_str.Append(", @PROGROUP_CODE ");
                obj_str.Append(", @PROGROUP_NAME_TH ");
                obj_str.Append(", @PROGROUP_NAME_EN ");      
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");
                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.progroup_id = this.getNextID();
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

                obj_cmd.Parameters.Add("@PROGROUP_ID", SqlDbType.Int); obj_cmd.Parameters["@PROGROUP_ID"].Value = model.progroup_id;
                obj_cmd.Parameters.Add("@PROGROUP_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROGROUP_CODE"].Value = model.progroup_code;
                obj_cmd.Parameters.Add("@PROGROUP_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROGROUP_NAME_TH"].Value = model.progroup_name_th;
                obj_cmd.Parameters.Add("@PROGROUP_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROGROUP_NAME_EN"].Value = model.progroup_name_en;        
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                                     
                obj_cmd.ExecuteNonQuery();
                                
                obj_conn.doClose();
                strResult = model.progroup_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "PROGROUP005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTProgroup model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PRO_MT_PROGROUP SET ");
                obj_str.Append(" PROGROUP_NAME_TH=@PROGROUP_NAME_TH ");
                obj_str.Append(", PROGROUP_NAME_EN=@PROGROUP_NAME_EN ");               
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE PROGROUP_ID=@PROGROUP_ID ");            

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROGROUP_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROGROUP_NAME_TH"].Value = model.progroup_name_th;
                obj_cmd.Parameters.Add("@PROGROUP_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROGROUP_NAME_EN"].Value = model.progroup_name_en;        
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@PROGROUP_ID", SqlDbType.Int); obj_cmd.Parameters["@PROGROUP_ID"].Value = model.progroup_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "PROGROUP006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}
