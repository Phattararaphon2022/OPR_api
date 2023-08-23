﻿using ClassLibrary_BPC.hrfocus.model.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller.Payroll
{
   public class cls_ctMTPlanreduce
   {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTPlanreduce() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTPlanreduce> getData(string condition)
        {
            List<cls_MTPlanreduce> list_model = new List<cls_MTPlanreduce>();
            cls_MTPlanreduce model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PLANREDUCE_ID");
                obj_str.Append(", PLANREDUCE_CODE");
                obj_str.Append(", ISNULL(PLANREDUCE_NAME_TH, '') AS PLANREDUCE_NAME_TH");
                obj_str.Append(", ISNULL(PLANREDUCE_NAME_EN, '') AS PLANREDUCE_NAME_EN");

                obj_str.Append(", COMPANY_CODE");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PAY_MT_PLANREDUCE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PLANREDUCE_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTPlanreduce();

                    model.planreduce_id = Convert.ToInt32(dr["PLANREDUCE_ID"]);
                    model.planreduce_code = dr["PLANREDUCE_CODE"].ToString();
                    model.planreduce_name_th = dr["PLANREDUCE_NAME_TH"].ToString();
                    model.planreduce_name_en = dr["PLANREDUCE_NAME_EN"].ToString();
                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(PLANREDUCE.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTPlanreduce> getDataByFillter(string com, string id, string code)
        {
            string strCondition = " AND COMPANY_CODE='" + com + "'";

            if (!id.Equals(""))
                strCondition += " AND PLANREDUCE_ID='" + id + "'";

            if (!code.Equals(""))
                strCondition += " AND PLANREDUCE_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PLANREDUCE_ID");
                obj_str.Append(" FROM PAY_MT_PLANREDUCE");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND PLANREDUCE_CODE='" + code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(PLANREDUCE.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(PLANREDUCE_ID) ");
                obj_str.Append(" FROM PAY_MT_PLANREDUCE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(PLANREDUCE.getNextID)" + ex.ToString();
            }

            return intResult;
        }

        public bool delete(string id, string com)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM PAY_MT_PLANREDUCE");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND PLANREDUCE_ID='" + id + "'");
                obj_str.Append(" AND COMPANY_CODE='" + com + "' ");
                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(PLANREDUCE.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTPlanreduce model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.planreduce_code))
                {
                    if (model.planreduce_id.Equals(0))
                    {
                        return "D";
                    }
                    else
                    {
                        return this.update(model);
                    }
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO PAY_MT_PLANREDUCE");
                obj_str.Append(" (");
                obj_str.Append("PLANREDUCE_ID ");
                obj_str.Append(", PLANREDUCE_CODE ");
                obj_str.Append(", PLANREDUCE_NAME_TH ");
                obj_str.Append(", PLANREDUCE_NAME_EN ");
                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append(" @PLANREDUCE_ID ");
                obj_str.Append(", @PLANREDUCE_CODE ");
                obj_str.Append(", @PLANREDUCE_NAME_TH ");
                obj_str.Append(", @PLANREDUCE_NAME_EN ");
                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PLANREDUCE_ID", SqlDbType.Int); obj_cmd.Parameters["@PLANREDUCE_ID"].Value = id;
                obj_cmd.Parameters.Add("@PLANREDUCE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PLANREDUCE_CODE"].Value = model.planreduce_code;
                obj_cmd.Parameters.Add("@PLANREDUCE_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PLANREDUCE_NAME_TH"].Value = model.planreduce_name_th;
                obj_cmd.Parameters.Add("@PLANREDUCE_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PLANREDUCE_NAME_EN"].Value = model.planreduce_name_en;
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Planreduce.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_MTPlanreduce model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE PAY_MT_PLANREDUCE SET ");

                obj_str.Append(" PLANREDUCE_CODE=@PLANREDUCE_CODE ");
                obj_str.Append(", PLANREDUCE_NAME_TH=@PLANREDUCE_NAME_TH ");
                obj_str.Append(", PLANREDUCE_NAME_EN=@PLANREDUCE_NAME_EN ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");
                obj_str.Append(" WHERE 1=1 ");
                if (!model.planreduce_id.Equals(0))
                {
                    obj_str.Append(" AND PLANREDUCE_ID=@PLANREDUCE_ID ");
                }
                else
                {
                    obj_str.Append(" AND COMPANY_CODE='" + model.company_code + "' ");
                    obj_str.Append(" AND PLANREDUCE_CODE='" + model.planreduce_code + "'");
                }

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PLANREDUCE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PLANREDUCE_CODE"].Value = model.planreduce_code;
                obj_cmd.Parameters.Add("@PLANREDUCE_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PLANREDUCE_NAME_TH"].Value = model.planreduce_name_th;
                obj_cmd.Parameters.Add("@PLANREDUCE_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PLANREDUCE_NAME_EN"].Value = model.planreduce_name_en;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.Parameters.Add("@PLANREDUCE_ID", SqlDbType.Int); obj_cmd.Parameters["@PLANREDUCE_ID"].Value = model.planreduce_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.planreduce_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(PLANREDUCE.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}