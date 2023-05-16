﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
  public  class cls_ctMTReqdoc
    {
           string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTReqdoc() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTReqdoc> getData(string condition)
        {
            List<cls_MTReqdoc> list_model = new List<cls_MTReqdoc>();
            cls_MTReqdoc model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");
                obj_str.Append(", REQDOC_ID");
                obj_str.Append(", REQDOC_DOC");
                obj_str.Append(", REQDOC_DATE");
                obj_str.Append(", REQDOC_NOTE");
                obj_str.Append(", STATUS");
           
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");
                obj_str.Append(", ISNULL(FLAG, 0) AS FLAG");

                obj_str.Append(" FROM SELF_MT_REQDOC");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY REQDOC_ID");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTReqdoc();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.reqdoc_id = Convert.ToInt32(dr["REQDOC_ID"]);
                    model.reqdoc_doc = dr["REQDOC_DOC"].ToString();
                    model.reqdoc_date = Convert.ToDateTime(dr["REQDOC_DATE"]);
                    model.reqdoc_note = dr["REQDOC_NOTE"].ToString();
                    model.status = Convert.ToInt32(dr["STATUS"]);
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                    model.flag = Convert.ToBoolean(dr["FLAG"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTreqdoc.getData)" + ex.ToString();
            }

            return list_model;
        }
        public List<cls_MTReqdoc> getDataByFillter(string com,int id,string worker_code,string datefrom,string dateto,string status)
        {
            string strCondition = "";
            if(!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!id.Equals(0))
                strCondition += " AND REQDOC_ID='" + id + "'";

            if (!datefrom.Equals("") && !dateto.Equals(""))
                strCondition += " AND (REQDOC_DATE BETWEEN '" + datefrom + "' AND '" + dateto + "'";

            if (!worker_code.Equals(""))
                strCondition += " AND WORKER_CODE='" + worker_code + "'";

            if (!status.Equals(""))
                strCondition += " AND STATUS='" + status + "'";

            return this.getData(strCondition);
        }
        public bool checkDataOld(string com, string date, string worker_code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT WORKER_CODE");
                obj_str.Append(" FROM SELF_MT_REQDOC");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                obj_str.Append(" AND REQDOC_DATE ='" + date + "'");
                obj_str.Append(" AND WORKER_CODE='" + worker_code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTreqdoc.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(string com,int id,string date,string worker_code)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SELF_MT_REQDOC");
                obj_str.Append(" WHERE 1=1 ");
                if (!com.Equals(""))
                    obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                if (!id.Equals(0))
                    obj_str.Append(" AND REQDOC_ID='" + id + "'");
                if (!date.Equals(""))
                    obj_str.Append(" AND REQDOC_DATE='" + date + "'");
                if (!worker_code.Equals(""))
                    obj_str.Append(" AND WORKER_CODE='" + worker_code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(MTreqdoc.delete)" + ex.ToString();
            }

            return blnResult;
        }
        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(WORKER_CODE) ");
                obj_str.Append(" FROM SELF_MT_REQDOC");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTreqdoc.getNextID)" + ex.ToString();
            }

            return intResult;
        }
        public string insert(cls_MTReqdoc model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.reqdoc_date.ToString("MM/dd/yyyy"),model.worker_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO SELF_MT_REQDOC");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", REQDOC_ID ");
                obj_str.Append(", REQDOC_DOC ");
                obj_str.Append(", REQDOC_DATE ");
                obj_str.Append(", REQDOC_NOTE ");
                obj_str.Append(", STATUS ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @REQDOC_ID ");
                obj_str.Append(", @REQDOC_DOC ");
                obj_str.Append(", @REQDOC_DATE ");
                obj_str.Append(", @REQDOC_NOTE ");
                obj_str.Append(", @STATUS ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@REQDOC_ID", SqlDbType.Int); obj_cmd.Parameters["@REQDOC_ID"].Value = id;
                obj_cmd.Parameters.Add("@REQDOC_DOC", SqlDbType.VarChar); obj_cmd.Parameters["@REQDOC_DOC"].Value = model.reqdoc_doc;
                obj_cmd.Parameters.Add("@REQDOC_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@REQDOC_DATE"].Value = model.reqdoc_date;
                obj_cmd.Parameters.Add("@REQDOC_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@REQDOC_NOTE"].Value = model.reqdoc_note;
                obj_cmd.Parameters.Add("@STATUS", SqlDbType.Int); obj_cmd.Parameters["@STATUS"].Value = model.status;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTreqdoc.insert)" + ex.ToString();
            }

            return blnResult;
        }
        public string update(cls_MTReqdoc model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SELF_MT_REQDOC SET ");
                obj_str.Append(" COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(", REQDOC_DOC=@REQDOC_DOC ");
                obj_str.Append(", REQDOC_DATE=@REQDOC_DATE ");
                obj_str.Append(", REQDOC_NOTE=@REQDOC_NOTE ");
                obj_str.Append(", STATUS=@STATUS ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");
                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND REQDOC_ID=@REQDOC_ID ");
      

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());



                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@REQDOC_ID", SqlDbType.Int); obj_cmd.Parameters["@REQDOC_ID"].Value = model.reqdoc_id;
                obj_cmd.Parameters.Add("@REQDOC_DOC", SqlDbType.VarChar); obj_cmd.Parameters["@REQDOC_DOC"].Value = model.reqdoc_doc;
                obj_cmd.Parameters.Add("@REQDOC_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@REQDOC_DATE"].Value = model.reqdoc_date;
                obj_cmd.Parameters.Add("@REQDOC_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@REQDOC_NOTE"].Value = model.reqdoc_note;
                obj_cmd.Parameters.Add("@STATUS", SqlDbType.Int); obj_cmd.Parameters["@STATUS"].Value = model.status;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;
                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.reqdoc_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTreqdoc.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
