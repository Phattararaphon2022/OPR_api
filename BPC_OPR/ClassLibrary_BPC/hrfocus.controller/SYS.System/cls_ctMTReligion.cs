﻿using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller
{
   public class cls_ctMTReligion
    {
   string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTReligion() { }

        public string getMessage() { return this.Message.Replace("SYS_MT_RELIGION", "").Replace("cls_ctMTReligion", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTReligion> getData(string condition)
        {
            List<cls_MTReligion> list_model = new List<cls_MTReligion>();
            cls_MTReligion model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");
                obj_str.Append("COMPANY_CODE");

                obj_str.Append(", RELIGION_ID");
                obj_str.Append(", RELIGION_CODE");
                obj_str.Append(", RELIGION_NAME_TH");
                obj_str.Append(", RELIGION_NAME_EN");             
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM SYS_MT_RELIGION");
                obj_str.Append(" WHERE 1=1");
                
                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY RELIGION_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTReligion();
                    model.company_code = dr["COMPANY_CODE"].ToString();

                    model.religion_id = Convert.ToInt32(dr["RELIGION_ID"]);
                    model.religion_code = dr["RELIGION_CODE"].ToString();
                    model.religion_name_th = dr["RELIGION_NAME_TH"].ToString();
                    model.religion_name_en = dr["RELIGION_NAME_EN"].ToString();                    
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                                                                                            
                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "RLG001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTReligion> getDataByFillter(string com, string code)
        {
            string strCondition = "";
            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";
            if (!code.Equals(""))
                strCondition += " AND RELIGION_CODE='" + code + "'";
            
            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(RELIGION_ID, 1) ");
                obj_str.Append(" FROM SYS_MT_RELIGION");
                obj_str.Append(" ORDER BY RELIGION_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "RLG002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string code, string com, string id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT RELIGION_CODE");
                obj_str.Append(" FROM SYS_MT_RELIGION");
                obj_str.Append(" WHERE RELIGION_CODE='" + code + "'");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");

                obj_str.Append(" AND RELIGION_ID='" + id + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "RLG003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM SYS_MT_RELIGION");
                obj_str.Append(" WHERE RELIGION_CODE='" + code + "'");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "RLG004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTReligion model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.religion_code, model.company_code, model.religion_id.ToString()))
                {
                    if (this.update(model))
                        return model.religion_id.ToString();
                    else
                        return "";                    
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_MT_RELIGION");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");

                obj_str.Append(", RELIGION_ID ");
                obj_str.Append(", RELIGION_CODE ");
                obj_str.Append(", RELIGION_NAME_TH ");
                obj_str.Append(", RELIGION_NAME_EN ");               
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");          
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");

                obj_str.Append(", @RELIGION_ID ");
                obj_str.Append(", @RELIGION_CODE ");
                obj_str.Append(", @RELIGION_NAME_TH ");
                obj_str.Append(", @RELIGION_NAME_EN ");      
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");
                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.religion_id = this.getNextID();
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

                obj_cmd.Parameters.Add("@RELIGION_ID", SqlDbType.Int); obj_cmd.Parameters["@RELIGION_ID"].Value = model.religion_id;
                obj_cmd.Parameters.Add("@RELIGION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@RELIGION_CODE"].Value = model.religion_code;
                obj_cmd.Parameters.Add("@RELIGION_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@RELIGION_NAME_TH"].Value = model.religion_name_th;
                obj_cmd.Parameters.Add("@RELIGION_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@RELIGION_NAME_EN"].Value = model.religion_name_en;        
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                                     
                obj_cmd.ExecuteNonQuery();
                                
                obj_conn.doClose();
                strResult = model.religion_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "RLG005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTReligion model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE SYS_MT_RELIGION SET ");
                //obj_str.Append(" RELIGION_CODE=@RELIGION_CODE ");

                obj_str.Append(" RELIGION_NAME_TH=@RELIGION_NAME_TH ");
                obj_str.Append(", RELIGION_NAME_EN=@RELIGION_NAME_EN ");               
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE RELIGION_CODE=@RELIGION_CODE ");            

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                //obj_cmd.Parameters.Add("@RELIGION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@RELIGION_CODE"].Value = model.religion_code;

                obj_cmd.Parameters.Add("@RELIGION_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@RELIGION_NAME_TH"].Value = model.religion_name_th;
                obj_cmd.Parameters.Add("@RELIGION_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@RELIGION_NAME_EN"].Value = model.religion_name_en;        
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                //obj_cmd.Parameters.Add("@RELIGION_ID", SqlDbType.Int); obj_cmd.Parameters["@RELIGION_ID"].Value = model.religion_id;
                obj_cmd.Parameters.Add("@RELIGION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@RELIGION_CODE"].Value = model.religion_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "RLG006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}
