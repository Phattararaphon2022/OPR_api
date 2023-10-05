﻿using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTForetype
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTForetype() { }

        public string getMessage() { return this.Message.Replace("EMP_MT_FORETYPE", "").Replace("cls_ctMTForetype", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTForetype> getData(string condition)
        {
            List<cls_MTForetype> list_model = new List<cls_MTForetype>();
            cls_MTForetype model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");
                obj_str.Append("FORETYPE_ID");
                obj_str.Append(", FORETYPE_CODE");
                obj_str.Append(", ISNULL(FORETYPE_NAME_TH, '') AS FORETYPE_NAME_TH");
                obj_str.Append(", ISNULL(FORETYPE_NAME_EN, '') AS FORETYPE_NAME_EN");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");
                obj_str.Append(" FROM EMP_MT_FORETYPE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY FORETYPE_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTForetype();

                    model.foretype_id = Convert.ToInt32(dr["FORETYPE_ID"]);
                    model.foretype_code = dr["FORETYPE_CODE"].ToString();
                    model.foretype_name_th = dr["FORETYPE_NAME_TH"].ToString();
                    model.foretype_name_en = dr["FORETYPE_NAME_EN"].ToString();
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "FTY001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTForetype> getDataByFillter(string code)
        {
            string strCondition = "";
            

            if (!code.Equals(""))
                strCondition += " AND FORETYPE_CODE ='" + code + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(FORETYPE_ID, 1) ");
                obj_str.Append(" FROM EMP_MT_FORETYPE");
                obj_str.Append(" ORDER BY FORETYPE_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "FTY002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string id,string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT FORETYPE_CODE");
                obj_str.Append(" FROM EMP_MT_FORETYPE");
                obj_str.Append(" WHERE FORETYPE_ID='" + id + "'");
                obj_str.Append(" AND FORETYPE_CODE='" + code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "FTY003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM EMP_MT_FORETYPE");
                obj_str.Append(" WHERE FORETYPE_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "FTY004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTForetype model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.foretype_id.ToString(),model.foretype_code))
                {
                    if (this.update(model))
                        return model.foretype_id.ToString();
                    else
                        return "";
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_MT_FORETYPE");
                obj_str.Append(" (");
                obj_str.Append("FORETYPE_ID ");
                obj_str.Append(", FORETYPE_CODE ");
                obj_str.Append(", FORETYPE_NAME_TH ");
                obj_str.Append(", FORETYPE_NAME_EN ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");
                obj_str.Append(" VALUES(");
                obj_str.Append(" @FORETYPE_ID ");
                obj_str.Append(", @FORETYPE_CODE ");
                obj_str.Append(", @FORETYPE_NAME_TH ");
                obj_str.Append(", @FORETYPE_NAME_EN ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", 1 ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.foretype_id = this.getNextID();

                obj_cmd.Parameters.Add("@FORETYPE_ID", SqlDbType.Int); obj_cmd.Parameters["@FORETYPE_ID"].Value = model.foretype_id;
                obj_cmd.Parameters.Add("@FORETYPE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@FORETYPE_CODE"].Value = model.foretype_code;
                obj_cmd.Parameters.Add("@FORETYPE_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@FORETYPE_NAME_TH"].Value = model.foretype_name_th;
                obj_cmd.Parameters.Add("@FORETYPE_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@FORETYPE_NAME_EN"].Value = model.foretype_name_en;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                strResult = model.foretype_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "FTY005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTForetype model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE EMP_MT_FORETYPE SET ");
                obj_str.Append(" FORETYPE_CODE=@FORETYPE_CODE ");
                obj_str.Append(", FORETYPE_NAME_TH=@FORETYPE_NAME_TH ");
                obj_str.Append(", FORETYPE_NAME_EN=@FORETYPE_NAME_EN ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE FORETYPE_ID=@FORETYPE_ID ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@FORETYPE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@FORETYPE_CODE"].Value = model.foretype_code;
                obj_cmd.Parameters.Add("@FORETYPE_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@FORETYPE_NAME_TH"].Value = model.foretype_name_th;
                obj_cmd.Parameters.Add("@FORETYPE_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@FORETYPE_NAME_EN"].Value = model.foretype_name_en;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@FORETYPE_ID", SqlDbType.Int); obj_cmd.Parameters["@FORETYPE_ID"].Value = model.foretype_id;


                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "FTY006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}