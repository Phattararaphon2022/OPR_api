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
    public class cls_ctTRHospital
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRHospital() { }

        public string getMessage() { return this.Message.Replace("EMP_TR_HOSPITAL", "").Replace("cls_ctTRHospital", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRHospital> getData(string condition)
        {
            List<cls_TRHospital> list_model = new List<cls_TRHospital>();
            cls_TRHospital model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");
                obj_str.Append(", EMPHOSPITAL_ID");
                obj_str.Append(", ISNULL(EMPHOSPITAL_CODE, '') AS EMPHOSPITAL_CODE");
                obj_str.Append(", EMPHOSPITAL_DATE");

                obj_str.Append(", EMPHOSPITAL_ORDER");
                obj_str.Append(", EMPHOSPITAL_ACTIVATE");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_TR_HOSPITAL");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY WORKER_CODE,EMPHOSPITAL_ORDER");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRHospital();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.emphospital_id = Convert.ToInt32(dr["EMPHOSPITAL_ID"]);
                    model.emphospital_code = dr["EMPHOSPITAL_CODE"].ToString();
                    model.emphospital_date = Convert.ToDateTime(dr["EMPHOSPITAL_DATE"]);

                    model.emphospital_order = dr["EMPHOSPITAL_ORDER"].ToString();
                    model.emphospital_activate = Convert.ToBoolean(dr["EMPHOSPITAL_ACTIVATE"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "EMPHPT001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRHospital> getDataByFillter(string com, string emp)
        {
            string strCondition = "";

            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!emp.Equals(""))
                strCondition += " AND WORKER_CODE='" + emp + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(EMPHOSPITAL_ID, 1) ");
                obj_str.Append(" FROM EMP_TR_HOSPITAL");
                obj_str.Append(" ORDER BY EMPHOSPITAL_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPHPT002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp,string id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT EMPHOSPITAL_ID");
                obj_str.Append(" FROM EMP_TR_HOSPITAL");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                if(!id.ToString().Equals("")){
                    obj_str.Append(" AND EMPHOSPITAL_ID='" + id + "' ");

                }

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPHPT003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string emp)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM EMP_TR_HOSPITAL");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "EMPHPT004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRHospital model)
        {
            bool blnResult = false;
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.company_code, model.worker_code,model.emphospital_id.ToString()))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_TR_HOSPITAL");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", EMPHOSPITAL_ID ");
                obj_str.Append(", EMPHOSPITAL_CODE ");
                obj_str.Append(", EMPHOSPITAL_DATE ");

                obj_str.Append(", EMPHOSPITAL_ORDER ");
                obj_str.Append(", EMPHOSPITAL_ACTIVATE ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @EMPHOSPITAL_ID ");
                obj_str.Append(", @EMPHOSPITAL_CODE ");
                obj_str.Append(", @EMPHOSPITAL_DATE ");

                obj_str.Append(", @EMPHOSPITAL_ORDER ");
                obj_str.Append(", @EMPHOSPITAL_ACTIVATE ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.emphospital_id = this.getNextID();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@EMPHOSPITAL_ID", SqlDbType.Int); obj_cmd.Parameters["@EMPHOSPITAL_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@EMPHOSPITAL_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@EMPHOSPITAL_CODE"].Value = model.emphospital_code;
                obj_cmd.Parameters.Add("@EMPHOSPITAL_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPHOSPITAL_DATE"].Value = model.emphospital_date;

                obj_cmd.Parameters.Add("@EMPHOSPITAL_ORDER", SqlDbType.VarChar); obj_cmd.Parameters["@EMPHOSPITAL_ORDER"].Value = model.emphospital_order;
                obj_cmd.Parameters.Add("@EMPHOSPITAL_ACTIVATE", SqlDbType.Bit); obj_cmd.Parameters["@EMPHOSPITAL_ACTIVATE"].Value = model.emphospital_activate;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                strResult = model.emphospital_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "EMPHPT005:" + ex.ToString();
                strResult = "";
            }

            return blnResult;
        }

        public bool update(cls_TRHospital model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE EMP_TR_HOSPITAL SET ");

                obj_str.Append(" EMPHOSPITAL_CODE=@EMPHOSPITAL_CODE ");
                obj_str.Append(", EMPHOSPITAL_DATE=@EMPHOSPITAL_DATE ");

                obj_str.Append(", EMPHOSPITAL_ORDER=@EMPHOSPITAL_ORDER ");
                obj_str.Append(", EMPHOSPITAL_ACTIVATE=@EMPHOSPITAL_ACTIVATE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE "); ;

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" AND EMPHOSPITAL_ID=@EMPHOSPITAL_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@EMPHOSPITAL_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@EMPHOSPITAL_CODE"].Value = model.emphospital_code;
                obj_cmd.Parameters.Add("@EMPHOSPITAL_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPHOSPITAL_DATE"].Value = model.emphospital_date;

                obj_cmd.Parameters.Add("@EMPHOSPITAL_ORDER", SqlDbType.VarChar); obj_cmd.Parameters["@EMPHOSPITAL_ORDER"].Value = model.emphospital_order;
                obj_cmd.Parameters.Add("@EMPHOSPITAL_ACTIVATE", SqlDbType.Bit); obj_cmd.Parameters["@EMPHOSPITAL_ACTIVATE"].Value = model.emphospital_activate;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@EMPHOSPITAL_ID", SqlDbType.Int); obj_cmd.Parameters["@EMPHOSPITAL_ID"].Value = model.emphospital_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "EMPHPT006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
