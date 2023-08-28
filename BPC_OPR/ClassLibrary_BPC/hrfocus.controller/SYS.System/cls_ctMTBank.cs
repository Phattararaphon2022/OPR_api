using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary_BPC.hrfocus.model;
using System.Data.SqlClient;
using System.Data;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTBank
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTBank() { }

        public string getMessage() { return this.Message.Replace("SYS_MT_BANK", "").Replace("cls_ctMTBank", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTBank> getData(string condition)
        {
            List<cls_MTBank> list_model = new List<cls_MTBank>();
            cls_MTBank model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("BANK_ID");
                obj_str.Append(", BANK_CODE");
                obj_str.Append(", BANK_NAME_TH");
                obj_str.Append(", BANK_NAME_EN");             
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");    

                obj_str.Append(" FROM SYS_MT_BANK");
                obj_str.Append(" WHERE 1=1");
                
                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY BANK_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTBank();

                    model.bank_id = Convert.ToInt32(dr["BANK_ID"]);
                    model.bank_code = dr["BANK_CODE"].ToString();
                    model.bank_name_th = dr["BANK_NAME_TH"].ToString();
                    model.bank_name_en = dr["BANK_NAME_EN"].ToString();                    
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                                                                                            
                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "BNK001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTBank> getDataByFillter(string code)
        {
            string strCondition = "";

            if (!code.Equals(""))
                strCondition += " AND BANK_CODE='" + code + "'";
            
            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(BANK_ID, 1) ");
                obj_str.Append(" FROM SYS_MT_BANK");
                obj_str.Append(" ORDER BY BANK_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "BNK002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string code, string id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT BANK_CODE");
                obj_str.Append(" FROM SYS_MT_BANK");
                obj_str.Append(" WHERE BANK_CODE='" + code + "'");
                obj_str.Append(" AND BANK_ID='" + id + "'");
      
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "BNK003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM SYS_MT_BANK");
                obj_str.Append(" WHERE BANK_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BNK004:" + ex.ToString();
            }

            return blnResult;
        }
        
        public string insert(cls_MTBank model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.bank_code, model.bank_id.ToString()))
                {
                    if (this.update(model))
                        return model.bank_id.ToString();
                    else
                        return "";                    
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                          
                obj_str.Append("INSERT INTO SYS_MT_BANK");
                obj_str.Append(" (");
                obj_str.Append("BANK_ID ");
                obj_str.Append(", BANK_CODE ");
                obj_str.Append(", BANK_NAME_TH ");
                obj_str.Append(", BANK_NAME_EN ");               
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");          
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@BANK_ID ");
                obj_str.Append(", @BANK_CODE ");
                obj_str.Append(", @BANK_NAME_TH ");
                obj_str.Append(", @BANK_NAME_EN ");      
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");
                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.bank_id = this.getNextID();

                obj_cmd.Parameters.Add("@BANK_ID", SqlDbType.Int); obj_cmd.Parameters["@BANK_ID"].Value = model.bank_id;
                obj_cmd.Parameters.Add("@BANK_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@BANK_CODE"].Value = model.bank_code;
                obj_cmd.Parameters.Add("@BANK_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@BANK_NAME_TH"].Value = model.bank_name_th;
                obj_cmd.Parameters.Add("@BANK_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@BANK_NAME_EN"].Value = model.bank_name_en;        
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                                     
                obj_cmd.ExecuteNonQuery();
                                
                obj_conn.doClose();
                strResult = model.bank_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "BNK005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTBank model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE SYS_MT_BANK SET ");


                obj_str.Append(" BANK_NAME_TH=@BANK_NAME_TH ");
                obj_str.Append(", BANK_NAME_EN=@BANK_NAME_EN ");               
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE BANK_CODE=@BANK_CODE ");
                obj_str.Append(" AND BANK_ID=@BANK_ID ");



                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@BANK_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@BANK_NAME_TH"].Value = model.bank_name_th;
                obj_cmd.Parameters.Add("@BANK_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@BANK_NAME_EN"].Value = model.bank_name_en;        
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@BANK_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@BANK_CODE"].Value = model.bank_code;
                obj_cmd.Parameters.Add("@BANK_ID", SqlDbType.Int); obj_cmd.Parameters["@BANK_ID"].Value = model.bank_id;


                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "BNK006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}
