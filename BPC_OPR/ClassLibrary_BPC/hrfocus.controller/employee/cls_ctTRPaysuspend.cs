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
    public class cls_ctTRPaysuspend
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRPaysuspend() { }

        public string getMessage() { return this.Message.Replace("EMP_TR_PAYSUSPEND", "").Replace("cls_ctTRPaysuspend", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRPaysuspend> getData(string condition)
        {
            List<cls_TRPaysuspend> list_model = new List<cls_TRPaysuspend>();
            cls_TRPaysuspend model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("EMP_TR_PAYSUSPEND.COMPANY_CODE");
                obj_str.Append(", EMP_TR_PAYSUSPEND.WORKER_CODE");
                obj_str.Append(", PAYSUSPEND_ID");
                obj_str.Append(", PAYITEM_DATE ");
                obj_str.Append(", ISNULL(PAYSUSPEND_NOTE, '') AS PAYSUSPEND_NOTE");
                obj_str.Append(", ISNULL(REASON_CODE, '') AS REASON_CODE");
                obj_str.Append(", PAYSUSPEND_TYPE");
                obj_str.Append(", PAYSUSPEND_PAYMENT");

                obj_str.Append(", INITIAL_NAME_TH + WORKER_FNAME_TH + ' ' + WORKER_LNAME_TH AS WORKER_DETAIL_TH");
                obj_str.Append(", INITIAL_NAME_EN + WORKER_FNAME_EN + ' ' + WORKER_LNAME_EN AS WORKER_DETAIL_EN");

                obj_str.Append(", ISNULL(EMP_TR_PAYSUSPEND.MODIFIED_BY, EMP_TR_PAYSUSPEND.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(EMP_TR_PAYSUSPEND.MODIFIED_DATE, EMP_TR_PAYSUSPEND.CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_TR_PAYSUSPEND");
                obj_str.Append(" INNER JOIN EMP_MT_WORKER ON EMP_MT_WORKER.COMPANY_CODE=EMP_TR_PAYSUSPEND.COMPANY_CODE AND EMP_MT_WORKER.WORKER_CODE=EMP_TR_PAYSUSPEND.WORKER_CODE");
                obj_str.Append(" INNER JOIN EMP_MT_INITIAL ON EMP_MT_INITIAL.INITIAL_CODE=EMP_MT_WORKER.WORKER_INITIAL ");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY EMP_TR_PAYSUSPEND.WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRPaysuspend();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.paysuspend_id = Convert.ToInt32(dr["PAYSUSPEND_ID"]);
                    model.payitem_date = Convert.ToDateTime(dr["PAYITEM_DATE"]);
                    model.paysuspend_note = dr["PAYSUSPEND_NOTE"].ToString();

                    model.reason_code = dr["REASON_CODE"].ToString();
                    model.paysuspend_type = dr["PAYSUSPEND_TYPE"].ToString();
                    model.paysuspend_payment = dr["PAYSUSPEND_PAYMENT"].ToString();

                    model.worker_detail_th = dr["WORKER_DETAIL_TH"].ToString();
                    model.worker_detail_en = dr["WORKER_DETAIL_EN"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "EMPPAY001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRPaysuspend> getDataByFillter(string com, string emp, string date)
        {
            string strCondition = "";

            if (!com.Equals(""))
                strCondition += " AND EMP_TR_PAYSUSPEND.COMPANY_CODE='" + com + "'";

            if (!emp.Equals(""))
                strCondition += " AND EMP_TR_PAYSUSPEND.WORKER_CODE='" + emp + "'";

            if (!date.Equals(""))
                strCondition += " AND PAYITEM_DATE='" + date + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(PAYSUSPEND_ID, 1) ");
                obj_str.Append(" FROM EMP_TR_PAYSUSPEND");
                obj_str.Append(" ORDER BY PAYSUSPEND_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPPAY002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp, int id, string date)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PAYSUSPEND_ID");
                obj_str.Append(" FROM EMP_TR_PAYSUSPEND");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                if (!id.Equals(0))
                {
                    obj_str.Append(" AND PAYSUSPEND_ID='" + id + "' ");
                }
                if (!date.Equals(""))
                {
                    obj_str.Append(" AND PAYITEM_DATE='" + date + "' ");
                }


                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPPAY003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM EMP_TR_PAYSUSPEND");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");


                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "EMPPAY004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRPaysuspend model)
        {
            bool blnResult = false;
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.company_code, model.worker_code, model.paysuspend_id, model.payitem_date.ToString("yyyy-MM-ddTHH:mm:ss")))
                {

                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_TR_PAYSUSPEND");
                obj_str.Append(" (");
                obj_str.Append("PAYSUSPEND_ID ");
                obj_str.Append(", PAYITEM_DATE ");
                obj_str.Append(", PAYSUSPEND_NOTE ");
                obj_str.Append(", REASON_CODE ");
                obj_str.Append(", PAYSUSPEND_TYPE ");
                obj_str.Append(", PAYSUSPEND_PAYMENT ");

                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@PAYSUSPEND_ID ");
                obj_str.Append(", @PAYITEM_DATE ");
                obj_str.Append(", @PAYSUSPEND_NOTE ");
                obj_str.Append(", @REASON_CODE ");
                obj_str.Append(", @PAYSUSPEND_TYPE ");
                obj_str.Append(", @PAYSUSPEND_PAYMENT ");

                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.paysuspend_id = this.getNextID();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@PAYSUSPEND_ID", SqlDbType.Int); obj_cmd.Parameters["@PAYSUSPEND_ID"].Value = model.paysuspend_id;
                obj_cmd.Parameters.Add("@PAYITEM_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@PAYITEM_DATE"].Value = model.payitem_date;
                obj_cmd.Parameters.Add("@PAYSUSPEND_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@PAYSUSPEND_NOTE"].Value = model.paysuspend_note;
                obj_cmd.Parameters.Add("@REASON_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@REASON_CODE"].Value = model.reason_code;
                obj_cmd.Parameters.Add("@PAYSUSPEND_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@PAYSUSPEND_TYPE"].Value = model.paysuspend_type;
                obj_cmd.Parameters.Add("@PAYSUSPEND_PAYMENT", SqlDbType.VarChar); obj_cmd.Parameters["@PAYSUSPEND_PAYMENT"].Value = model.paysuspend_payment;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                strResult = model.paysuspend_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "EMPPAY005:" + ex.ToString();
                strResult = "";
            }

            return blnResult;
        }

        public bool update(cls_TRPaysuspend model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE EMP_TR_PAYSUSPEND SET ");

                obj_str.Append(" PAYSUSPEND_NOTE=@PAYSUSPEND_NOTE ");
                obj_str.Append(", REASON_CODE=@REASON_CODE ");
                obj_str.Append(", PAYSUSPEND_TYPE=@PAYSUSPEND_TYPE ");
                obj_str.Append(", PAYSUSPEND_PAYMENT=@PAYSUSPEND_PAYMENT ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE "); ;

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" AND PAYITEM_DATE=@PAYITEM_DATE ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                

                obj_cmd.Parameters.Add("@PAYITEM_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@PAYITEM_DATE"].Value = model.payitem_date;
                obj_cmd.Parameters.Add("@PAYSUSPEND_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@PAYSUSPEND_NOTE"].Value = model.paysuspend_note;
                obj_cmd.Parameters.Add("@REASON_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@REASON_CODE"].Value = model.reason_code;
                obj_cmd.Parameters.Add("@PAYSUSPEND_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@PAYSUSPEND_TYPE"].Value = model.paysuspend_type;
                obj_cmd.Parameters.Add("@PAYSUSPEND_PAYMENT", SqlDbType.VarChar); obj_cmd.Parameters["@PAYSUSPEND_PAYMENT"].Value = model.paysuspend_payment;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "EMPPAY006:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insertlist(List<cls_TRPaysuspend> list_model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_TR_PAYSUSPEND");
                obj_str.Append(" (");
                obj_str.Append("PAYSUSPEND_ID ");
                obj_str.Append(", PAYITEM_DATE ");
                obj_str.Append(", PAYSUSPEND_NOTE ");
                obj_str.Append(", REASON_CODE ");
                obj_str.Append(", PAYSUSPEND_TYPE ");
                obj_str.Append(", PAYSUSPEND_PAYMENT ");

                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@PAYSUSPEND_ID ");
                obj_str.Append(", @PAYITEM_DATE ");
                obj_str.Append(", @PAYSUSPEND_NOTE ");
                obj_str.Append(", @REASON_CODE ");
                obj_str.Append(", @PAYSUSPEND_TYPE ");
                obj_str.Append(", @PAYSUSPEND_PAYMENT ");

                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                //-- Step 1 delete data old
                string strWorkerID = "";
                foreach (cls_TRPaysuspend model in list_model)
                {
                    strWorkerID += "'" + model.worker_code + "',";
                }
                if (strWorkerID.Length > 0)
                    strWorkerID = strWorkerID.Substring(0, strWorkerID.Length - 1);
                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM EMP_TR_PAYSUSPEND");
                obj_str2.Append(" WHERE 1=1 ");
                obj_str2.Append(" AND COMPANY_CODE='" + list_model[0].company_code + "'");
                obj_str2.Append(" AND WORKER_CODE IN (" + strWorkerID + ")");
                obj_str2.Append(" AND PAYITEM_DATE='" + list_model[0].payitem_date.ToString("yyyy-MM-ddTHH:mm:ss") + "'");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());

                if (blnResult)
                {
                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); 
                    obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); 

                    obj_cmd.Parameters.Add("@PAYSUSPEND_ID", SqlDbType.Int); 
                    obj_cmd.Parameters.Add("@PAYITEM_DATE", SqlDbType.DateTime); 
                    obj_cmd.Parameters.Add("@PAYSUSPEND_NOTE", SqlDbType.VarChar); 
                    obj_cmd.Parameters.Add("@REASON_CODE", SqlDbType.VarChar); 
                    obj_cmd.Parameters.Add("@PAYSUSPEND_TYPE", SqlDbType.VarChar); 
                    obj_cmd.Parameters.Add("@PAYSUSPEND_PAYMENT", SqlDbType.VarChar); 

                    obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); 
                    obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); 

                    foreach (cls_TRPaysuspend model in list_model)
                    {
                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                        obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                        obj_cmd.Parameters["@PAYSUSPEND_ID"].Value = model.paysuspend_id;
                        obj_cmd.Parameters["@PAYITEM_DATE"].Value = model.payitem_date;
                        obj_cmd.Parameters["@PAYSUSPEND_NOTE"].Value = model.paysuspend_note;
                        obj_cmd.Parameters["@REASON_CODE"].Value = model.reason_code;
                        obj_cmd.Parameters["@PAYSUSPEND_TYPE"].Value = model.paysuspend_type;
                        obj_cmd.Parameters["@PAYSUSPEND_PAYMENT"].Value = model.paysuspend_payment;
                        obj_cmd.Parameters["@CREATED_BY"].Value = model.created_by;
                        obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                        obj_cmd.ExecuteNonQuery();
                    }

                    blnResult = obj_conn.doCommit();

                    if (!blnResult)
                        obj_conn.doRollback();
                    obj_conn.doClose();

                }
                else
                {
                    obj_conn.doRollback();
                    obj_conn.doClose();
                }
            }
            catch (Exception ex)
            {
                Message = "EMPPAY099:" + ex.ToString();
            }

            return blnResult;
        }
    }
}