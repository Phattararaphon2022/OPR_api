﻿using ClassLibrary_BPC.hrfocus.model.Project;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller.Project
{
   public class cls_ctMTResponsiblepos
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTResponsiblepos() { }

        public string getMessage() { return this.Message.Replace("PRO_MT_RESPONSIBLEPOS", "").Replace("cls_ctMTResponsiblepos", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTResponsiblepos> getData(string condition)  
        {
            List<cls_MTResponsiblepos> list_model = new List<cls_MTResponsiblepos>();
            cls_MTResponsiblepos model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");
                obj_str.Append("COMPANY_CODE");

                obj_str.Append(", RESPONSIBLEPOS_ID");
                obj_str.Append(", RESPONSIBLEPOS_CODE");
                obj_str.Append(", RESPONSIBLEPOS_NAME_TH");
                obj_str.Append(", RESPONSIBLEPOS_NAME_EN");             
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PRO_MT_RESPONSIBLEPOS");
                obj_str.Append(" WHERE 1=1");
                
                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY RESPONSIBLEPOS_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTResponsiblepos();
                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);

                    model.responsiblepos_id = Convert.ToInt32(dr["RESPONSIBLEPOS_ID"]);
                    model.responsiblepos_code = dr["RESPONSIBLEPOS_CODE"].ToString();
                    model.responsiblepos_name_th = dr["RESPONSIBLEPOS_NAME_TH"].ToString();
                    model.responsiblepos_name_en = dr["RESPONSIBLEPOS_NAME_EN"].ToString();                    
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                                                                                            
                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "RESPONSIBLEPOS001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTResponsiblepos> getDataByFillter(string com, string code)
        {
            string strCondition = "";
            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!code.Equals(""))
                strCondition += " AND RESPONSIBLEPOS_CODE='" + code + "'";
            
            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(RESPONSIBLEPOS_ID, 1) ");
                obj_str.Append(" FROM PRO_MT_RESPONSIBLEPOS");
                obj_str.Append(" ORDER BY RESPONSIBLEPOS_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "RESPONSIBLEPOS002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string code, string com)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT RESPONSIBLEPOS_CODE");
                obj_str.Append(" FROM PRO_MT_RESPONSIBLEPOS");
                obj_str.Append(" WHERE RESPONSIBLEPOS_CODE='" + code + "'");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "RESPONSIBLEPOS003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM PRO_MT_RESPONSIBLEPOS");
                obj_str.Append(" WHERE RESPONSIBLEPOS_CODE='" + code + "'");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "RESPONSIBLEPOS004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTResponsiblepos model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.responsiblepos_code, model.company_code))
                {
                    if (this.update(model))
                        return model.responsiblepos_id.ToString();
                    else
                        return "";                    
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PRO_MT_RESPONSIBLEPOS");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");

                obj_str.Append(", RESPONSIBLEPOS_ID ");
                obj_str.Append(", RESPONSIBLEPOS_CODE ");
                obj_str.Append(", RESPONSIBLEPOS_NAME_TH ");
                obj_str.Append(", RESPONSIBLEPOS_NAME_EN ");               
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");          
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");

                obj_str.Append(", @RESPONSIBLEPOS_ID ");
                obj_str.Append(", @RESPONSIBLEPOS_CODE ");
                obj_str.Append(", @RESPONSIBLEPOS_NAME_TH ");
                obj_str.Append(", @RESPONSIBLEPOS_NAME_EN ");      
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");
                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.responsiblepos_id = this.getNextID();
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

                obj_cmd.Parameters.Add("@RESPONSIBLEPOS_ID", SqlDbType.Int); obj_cmd.Parameters["@RESPONSIBLEPOS_ID"].Value = model.responsiblepos_id;
                obj_cmd.Parameters.Add("@RESPONSIBLEPOS_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@RESPONSIBLEPOS_CODE"].Value = model.responsiblepos_code;
                obj_cmd.Parameters.Add("@RESPONSIBLEPOS_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@RESPONSIBLEPOS_NAME_TH"].Value = model.responsiblepos_name_th;
                obj_cmd.Parameters.Add("@RESPONSIBLEPOS_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@RESPONSIBLEPOS_NAME_EN"].Value = model.responsiblepos_name_en;        
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                                     
                obj_cmd.ExecuteNonQuery();
                                
                obj_conn.doClose();
                strResult = model.responsiblepos_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "RESPONSIBLEPOS005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTResponsiblepos model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PRO_MT_RESPONSIBLEPOS SET ");
                obj_str.Append(" RESPONSIBLEPOS_NAME_TH=@RESPONSIBLEPOS_NAME_TH ");
                obj_str.Append(", RESPONSIBLEPOS_NAME_EN=@RESPONSIBLEPOS_NAME_EN ");               
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE RESPONSIBLEPOS_ID=@RESPONSIBLEPOS_ID ");            

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@RESPONSIBLEPOS_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@RESPONSIBLEPOS_NAME_TH"].Value = model.responsiblepos_name_th;
                obj_cmd.Parameters.Add("@RESPONSIBLEPOS_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@RESPONSIBLEPOS_NAME_EN"].Value = model.responsiblepos_name_en;        
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@RESPONSIBLEPOS_ID", SqlDbType.Int); obj_cmd.Parameters["@RESPONSIBLEPOS_ID"].Value = model.responsiblepos_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "RESPONSIBLEPOS006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}