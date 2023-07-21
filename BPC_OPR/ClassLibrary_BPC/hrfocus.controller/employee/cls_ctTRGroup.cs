using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRGroup
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRGroup() { }

        public string getMessage() { return this.Message.Replace("EMP_TR_GROUP", "").Replace("cls_ctTRGroup", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRGroup> getData(string condition)
        {
            List<cls_TRGroup> list_model = new List<cls_TRGroup>();
            cls_TRGroup model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("EMPGROUP_ID");
                obj_str.Append(", EMPGROUP_CODE");
                obj_str.Append(", EMPGROUP_DATE");

                obj_str.Append(", COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");


                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_TR_GROUP");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRGroup();

                    model.empgroup_id = Convert.ToInt32(dr["EMPGROUP_ID"]);
                    model.empgroup_code = dr["EMPGROUP_CODE"].ToString();
                    model.empgroup_date = Convert.ToDateTime(dr["EMPGROUP_DATE"]);

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "EMPGRP001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRGroup> getDataByFillter(string com, string emp,string code)
        {
            string strCondition = "";

            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!emp.Equals(""))
                strCondition += " AND WORKER_CODE='" + emp + "'";

            if (!code.Equals(""))
                strCondition += " AND EMPGROUP_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(EMPGROUP_ID, 1) ");
                obj_str.Append(" FROM EMP_TR_GROUP");
                obj_str.Append(" ORDER BY EMPGROUP_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPGRP002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp,string id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT EMPGROUP_ID");
                obj_str.Append(" FROM EMP_TR_GROUP");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                if(!id.ToString().Equals("")){
                    obj_str.Append(" AND EMPGROUP_ID='" + id + "' ");
                }

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPGRP003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM EMP_TR_GROUP");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "EMPGRP004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRGroup model)
        {
            bool blnResult = false;
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.company_code, model.worker_code,model.empgroup_id.ToString()))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_TR_GROUP");
                obj_str.Append(" (");
                obj_str.Append("EMPGROUP_ID ");
                obj_str.Append(", EMPGROUP_CODE ");
                obj_str.Append(", EMPGROUP_DATE ");
                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@EMPGROUP_ID ");
                obj_str.Append(", @EMPGROUP_CODE ");
                obj_str.Append(", @EMPGROUP_DATE ");
                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.empgroup_id = this.getNextID();

                obj_cmd.Parameters.Add("@EMPGROUP_ID", SqlDbType.Int); obj_cmd.Parameters["@EMPGROUP_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@EMPGROUP_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@EMPGROUP_CODE"].Value = model.empgroup_code;
                obj_cmd.Parameters.Add("@EMPGROUP_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPGROUP_DATE"].Value = model.empgroup_date;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                strResult = model.empgroup_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "EMPGRP005:" + ex.ToString();
                strResult = "";
            }

            return blnResult;
        }

        public bool update(cls_TRGroup model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE EMP_TR_GROUP SET ");

                obj_str.Append(" EMPGROUP_CODE=@EMPGROUP_CODE ");
                obj_str.Append(", EMPGROUP_DATE=@EMPGROUP_DATE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE EMPGROUP_ID=@EMPGROUP_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@EMPGROUP_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@EMPGROUP_CODE"].Value = model.empgroup_code;
                obj_cmd.Parameters.Add("@EMPGROUP_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPGROUP_DATE"].Value = model.empgroup_date;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@EMPGROUP_ID", SqlDbType.Int); obj_cmd.Parameters["@EMPGROUP_ID"].Value = model.empgroup_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "EMPGRP005:" + ex.ToString();
            }

            return blnResult;
        }

        public List<cls_TRGroup> getDataBatch(string com,string code, DateTime date)
        {
            List<cls_TRGroup> list_model = new List<cls_TRGroup>();
            cls_TRGroup model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");
                obj_str.Append("EMP_TR_GROUP.COMPANY_CODE");
                obj_str.Append(", EMP_TR_GROUP.WORKER_CODE");

                obj_str.Append(", EMP_TR_GROUP.EMPGROUP_ID");
                obj_str.Append(", EMP_TR_GROUP.EMPGROUP_CODE");
                obj_str.Append(", EMP_TR_GROUP.EMPGROUP_DATE");

                obj_str.Append(", INITIAL_NAME_TH + WORKER_FNAME_TH + ' ' + WORKER_LNAME_TH AS WORKER_DETAIL_TH");
                obj_str.Append(", INITIAL_NAME_EN + WORKER_FNAME_EN + ' ' + WORKER_LNAME_EN AS WORKER_DETAIL_EN");

                obj_str.Append(", ISNULL(EMP_TR_GROUP.MODIFIED_BY, EMP_TR_GROUP.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(EMP_TR_GROUP.MODIFIED_DATE, EMP_TR_GROUP.CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_TR_GROUP");
                obj_str.Append(" INNER JOIN EMP_MT_WORKER ON EMP_MT_WORKER.COMPANY_CODE=EMP_TR_GROUP.COMPANY_CODE AND EMP_MT_WORKER.WORKER_CODE=EMP_TR_GROUP.WORKER_CODE");
                obj_str.Append(" INNER JOIN EMP_MT_INITIAL ON EMP_MT_INITIAL.INITIAL_CODE=EMP_MT_WORKER.WORKER_INITIAL ");
                obj_str.Append(" WHERE 1=1");
                obj_str.Append(" AND EMP_TR_GROUP.COMPANY_CODE='" + com + "' ");

                if (!code.Equals(""))
                    obj_str.Append(" AND EMP_TR_GROUP.EMPGROUP_CODE='" + code + "' ");
                if (!date.Equals(""))
                    obj_str.Append(" AND EMP_TR_GROUP.EMPGROUP_DATE='" + date.ToString("yyyy-MM-ddTHH:mm:ss") + "' ");

                obj_str.Append(" ORDER BY EMP_TR_GROUP.COMPANY_CODE, EMP_TR_GROUP.WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRGroup();

                    model.empgroup_id = Convert.ToInt32(dr["EMPGROUP_ID"]);
                    model.empgroup_code = dr["EMPGROUP_CODE"].ToString();
                    model.empgroup_date = Convert.ToDateTime(dr["EMPGROUP_DATE"]);

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();

                    model.worker_detail_th = dr["WORKER_DETAIL_TH"].ToString();
                    model.worker_detail_en = dr["WORKER_DETAIL_EN"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "EMPGRP001:" + ex.ToString();
            }

            return list_model;
        }

        public bool insertlist(List<cls_TRGroup> list_model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_TR_GROUP");
                obj_str.Append(" (");
                obj_str.Append("EMPGROUP_ID ");
                obj_str.Append(", EMPGROUP_CODE ");
                obj_str.Append(", EMPGROUP_DATE ");
                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@EMPGROUP_ID ");
                obj_str.Append(", @EMPGROUP_CODE ");
                obj_str.Append(", @EMPGROUP_DATE ");
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
                foreach (cls_TRGroup model in list_model)
                {
                    strWorkerID += "'" + model.worker_code + "',";
                }
                if (strWorkerID.Length > 0)
                    strWorkerID = strWorkerID.Substring(0, strWorkerID.Length - 1);
                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM EMP_TR_GROUP");
                obj_str2.Append(" WHERE 1=1 ");
                obj_str2.Append(" AND COMPANY_CODE='" + list_model[0].company_code + "'");
                obj_str2.Append(" AND WORKER_CODE IN (" + strWorkerID + ")");
                obj_str2.Append(" AND EMPGROUP_DATE='" + list_model[0].empgroup_date.ToString("yyyy-MM-ddTHH:mm:ss") + "'");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());

                if (blnResult)
                {
                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@EMPGROUP_ID", SqlDbType.Int);
                    obj_cmd.Parameters.Add("@EMPGROUP_CODE", SqlDbType.VarChar); 
                    obj_cmd.Parameters.Add("@EMPGROUP_DATE", SqlDbType.DateTime);
                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime);

                    foreach (cls_TRGroup model in list_model)
                    {
                        obj_cmd.Parameters["@EMPGROUP_ID"].Value = this.getNextID();
                        obj_cmd.Parameters["@EMPGROUP_CODE"].Value = model.empgroup_code;
                        obj_cmd.Parameters["@EMPGROUP_DATE"].Value = model.empgroup_date;
                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                        obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
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
                Message = "EMPGRP099:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
