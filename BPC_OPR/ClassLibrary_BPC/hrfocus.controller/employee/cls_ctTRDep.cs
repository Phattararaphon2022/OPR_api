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
    public class cls_ctTRDep
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRDep() { }

        public string getMessage() { return this.Message.Replace("EMP_TR_DEP", "").Replace("cls_ctTRDep", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }
        private List<cls_TRDep> getData(string condition)
        {
            List<cls_TRDep> list_model = new List<cls_TRDep>();
            cls_TRDep model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");
                obj_str.Append(", EMPDEP_ID");
                obj_str.Append(", EMPDEP_DATE");
                obj_str.Append(", ISNULL(EMPDEP_LEVEL01, '') AS EMPDEP_LEVEL01");
                obj_str.Append(", ISNULL(EMPDEP_LEVEL02, '') AS EMPDEP_LEVEL02");
                obj_str.Append(", ISNULL(EMPDEP_LEVEL03, '') AS EMPDEP_LEVEL03");
                obj_str.Append(", ISNULL(EMPDEP_LEVEL04, '') AS EMPDEP_LEVEL04");
                obj_str.Append(", ISNULL(EMPDEP_LEVEL05, '') AS EMPDEP_LEVEL05");
                obj_str.Append(", ISNULL(EMPDEP_LEVEL06, '') AS EMPDEP_LEVEL06");
                obj_str.Append(", ISNULL(EMPDEP_LEVEL07, '') AS EMPDEP_LEVEL07");
                obj_str.Append(", ISNULL(EMPDEP_LEVEL08, '') AS EMPDEP_LEVEL08");
                obj_str.Append(", ISNULL(EMPDEP_LEVEL09, '') AS EMPDEP_LEVEL09");
                obj_str.Append(", ISNULL(EMPDEP_LEVEL10, '') AS EMPDEP_LEVEL10");
                obj_str.Append(", ISNULL(EMPDEP_REASON, '') AS EMPDEP_REASON");
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_TR_DEP");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRDep();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.empdep_id = Convert.ToInt32(dr["EMPDEP_ID"]);

                    model.empdep_date = Convert.ToDateTime(dr["EMPDEP_DATE"]);

                    model.empdep_level01 = dr["EMPDEP_LEVEL01"].ToString();
                    model.empdep_level02 = dr["EMPDEP_LEVEL02"].ToString();
                    model.empdep_level03 = dr["EMPDEP_LEVEL03"].ToString();
                    model.empdep_level04 = dr["EMPDEP_LEVEL04"].ToString();
                    model.empdep_level05 = dr["EMPDEP_LEVEL05"].ToString();
                    model.empdep_level06 = dr["EMPDEP_LEVEL06"].ToString();
                    model.empdep_level07 = dr["EMPDEP_LEVEL07"].ToString();
                    model.empdep_level08 = dr["EMPDEP_LEVEL08"].ToString();
                    model.empdep_level09 = dr["EMPDEP_LEVEL09"].ToString();
                    model.empdep_level10 = dr["EMPDEP_LEVEL10"].ToString();

                    model.empdep_reason = dr["EMPDEP_REASON"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "EMPDEP001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRDep> getDataByFillter(string com, string emp ,string date)
        {
            string strCondition = "";

            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!emp.Equals(""))
                strCondition += " AND WORKER_CODE='" + emp + "'";

            if(!date.Equals(""))
                strCondition += " AND EMPDEP_DATE='" + Convert.ToDateTime(date) + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(EMPDEP_ID, 1) ");
                obj_str.Append(" FROM EMP_TR_DEP");
                obj_str.Append(" ORDER BY EMPDEP_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPDEP002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp, int id,string date)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT EMPDEP_ID");
                obj_str.Append(" FROM EMP_TR_DEP");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                if(!id.Equals(0)){
                    obj_str.Append(" AND EMPDEP_ID='" + id + "' ");
                }
                if(!date.Equals("")){
                    obj_str.Append(" AND EMPDEP_DATE='" + date + "' ");
                }
                

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPDEP003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM EMP_TR_DEP");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
               

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "EMPDEP004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRDep model)
        {
            bool blnResult = false;
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.company_code, model.worker_code, model.empdep_id, model.empdep_date.ToString("yyyy-MM-ddTHH:mm:ss")))
                {
                   
                        return this.update(model) ;
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_TR_DEP");
                obj_str.Append(" (");
                obj_str.Append("EMPDEP_ID ");
                obj_str.Append(", EMPDEP_DATE ");
                obj_str.Append(", EMPDEP_LEVEL01 ");
                obj_str.Append(", EMPDEP_LEVEL02 ");
                obj_str.Append(", EMPDEP_LEVEL03 ");
                obj_str.Append(", EMPDEP_LEVEL04 ");
                obj_str.Append(", EMPDEP_LEVEL05 ");
                obj_str.Append(", EMPDEP_LEVEL06 ");
                obj_str.Append(", EMPDEP_LEVEL07 ");
                obj_str.Append(", EMPDEP_LEVEL08 ");
                obj_str.Append(", EMPDEP_LEVEL09 ");
                obj_str.Append(", EMPDEP_LEVEL10 ");
                obj_str.Append(", EMPDEP_REASON ");
                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@EMPDEP_ID ");
                obj_str.Append(", @EMPDEP_DATE ");
                obj_str.Append(", @EMPDEP_LEVEL01 ");
                obj_str.Append(", @EMPDEP_LEVEL02 ");
                obj_str.Append(", @EMPDEP_LEVEL03 ");
                obj_str.Append(", @EMPDEP_LEVEL04 ");
                obj_str.Append(", @EMPDEP_LEVEL05 ");
                obj_str.Append(", @EMPDEP_LEVEL06 ");
                obj_str.Append(", @EMPDEP_LEVEL07 ");
                obj_str.Append(", @EMPDEP_LEVEL08 ");
                obj_str.Append(", @EMPDEP_LEVEL09 ");
                obj_str.Append(", @EMPDEP_LEVEL10 ");
                obj_str.Append(", @EMPDEP_REASON ");
                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.empdep_id = this.getNextID();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@EMPDEP_ID", SqlDbType.Int); obj_cmd.Parameters["@EMPDEP_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@EMPDEP_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPDEP_DATE"].Value = model.empdep_date;
                obj_cmd.Parameters.Add("@EMPDEP_LEVEL01", SqlDbType.VarChar); obj_cmd.Parameters["@EMPDEP_LEVEL01"].Value = model.empdep_level01;
                obj_cmd.Parameters.Add("@EMPDEP_LEVEL02", SqlDbType.VarChar); obj_cmd.Parameters["@EMPDEP_LEVEL02"].Value = model.empdep_level02;
                obj_cmd.Parameters.Add("@EMPDEP_LEVEL03", SqlDbType.VarChar); obj_cmd.Parameters["@EMPDEP_LEVEL03"].Value = model.empdep_level03;
                obj_cmd.Parameters.Add("@EMPDEP_LEVEL04", SqlDbType.VarChar); obj_cmd.Parameters["@EMPDEP_LEVEL04"].Value = model.empdep_level04;
                obj_cmd.Parameters.Add("@EMPDEP_LEVEL05", SqlDbType.VarChar); obj_cmd.Parameters["@EMPDEP_LEVEL05"].Value = model.empdep_level05;
                obj_cmd.Parameters.Add("@EMPDEP_LEVEL06", SqlDbType.VarChar); obj_cmd.Parameters["@EMPDEP_LEVEL06"].Value = model.empdep_level06;
                obj_cmd.Parameters.Add("@EMPDEP_LEVEL07", SqlDbType.VarChar); obj_cmd.Parameters["@EMPDEP_LEVEL07"].Value = model.empdep_level07;
                obj_cmd.Parameters.Add("@EMPDEP_LEVEL08", SqlDbType.VarChar); obj_cmd.Parameters["@EMPDEP_LEVEL08"].Value = model.empdep_level08;
                obj_cmd.Parameters.Add("@EMPDEP_LEVEL09", SqlDbType.VarChar); obj_cmd.Parameters["@EMPDEP_LEVEL09"].Value = model.empdep_level09;
                obj_cmd.Parameters.Add("@EMPDEP_LEVEL10", SqlDbType.VarChar); obj_cmd.Parameters["@EMPDEP_LEVEL10"].Value = model.empdep_level10;
                obj_cmd.Parameters.Add("@EMPDEP_REASON", SqlDbType.VarChar); obj_cmd.Parameters["@EMPDEP_REASON"].Value = model.empdep_reason;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                strResult = model.empdep_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "EMPDEP005:" + ex.ToString();
                strResult = "";
            }

            return blnResult;
        }

        public bool update(cls_TRDep model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE EMP_TR_DEP SET ");

                obj_str.Append(" EMPDEP_LEVEL01=@EMPDEP_LEVEL01 ");
                obj_str.Append(", EMPDEP_LEVEL02=@EMPDEP_LEVEL02 ");
                obj_str.Append(", EMPDEP_LEVEL03=@EMPDEP_LEVEL03 ");
                obj_str.Append(", EMPDEP_LEVEL04=@EMPDEP_LEVEL04 ");
                obj_str.Append(", EMPDEP_LEVEL05=@EMPDEP_LEVEL05 ");
                obj_str.Append(", EMPDEP_LEVEL06=@EMPDEP_LEVEL06 ");
                obj_str.Append(", EMPDEP_LEVEL07=@EMPDEP_LEVEL07 ");
                obj_str.Append(", EMPDEP_LEVEL08=@EMPDEP_LEVEL08 ");
                obj_str.Append(", EMPDEP_LEVEL09=@EMPDEP_LEVEL09 ");
                obj_str.Append(", EMPDEP_LEVEL10=@EMPDEP_LEVEL10 ");
                obj_str.Append(", EMPDEP_REASON=@EMPDEP_REASON ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE "); ;

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" AND EMPDEP_DATE=@EMPDEP_DATE ");

                //obj_str.Append(" AND EMPDEP_ID=@EMPDEP_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@EMPDEP_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPDEP_DATE"].Value = model.empdep_date;
                obj_cmd.Parameters.Add("@EMPDEP_LEVEL01", SqlDbType.VarChar); obj_cmd.Parameters["@EMPDEP_LEVEL01"].Value = model.empdep_level01;
                obj_cmd.Parameters.Add("@EMPDEP_LEVEL02", SqlDbType.VarChar); obj_cmd.Parameters["@EMPDEP_LEVEL02"].Value = model.empdep_level02;
                obj_cmd.Parameters.Add("@EMPDEP_LEVEL03", SqlDbType.VarChar); obj_cmd.Parameters["@EMPDEP_LEVEL03"].Value = model.empdep_level03;
                obj_cmd.Parameters.Add("@EMPDEP_LEVEL04", SqlDbType.VarChar); obj_cmd.Parameters["@EMPDEP_LEVEL04"].Value = model.empdep_level04;
                obj_cmd.Parameters.Add("@EMPDEP_LEVEL05", SqlDbType.VarChar); obj_cmd.Parameters["@EMPDEP_LEVEL05"].Value = model.empdep_level05;
                obj_cmd.Parameters.Add("@EMPDEP_LEVEL06", SqlDbType.VarChar); obj_cmd.Parameters["@EMPDEP_LEVEL06"].Value = model.empdep_level06;
                obj_cmd.Parameters.Add("@EMPDEP_LEVEL07", SqlDbType.VarChar); obj_cmd.Parameters["@EMPDEP_LEVEL07"].Value = model.empdep_level07;
                obj_cmd.Parameters.Add("@EMPDEP_LEVEL08", SqlDbType.VarChar); obj_cmd.Parameters["@EMPDEP_LEVEL08"].Value = model.empdep_level08;
                obj_cmd.Parameters.Add("@EMPDEP_LEVEL09", SqlDbType.VarChar); obj_cmd.Parameters["@EMPDEP_LEVEL09"].Value = model.empdep_level09;
                obj_cmd.Parameters.Add("@EMPDEP_LEVEL10", SqlDbType.VarChar); obj_cmd.Parameters["@EMPDEP_LEVEL10"].Value = model.empdep_level10;
                obj_cmd.Parameters.Add("@EMPDEP_REASON", SqlDbType.VarChar); obj_cmd.Parameters["@EMPDEP_REASON"].Value = model.empdep_reason;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@EMPDEP_ID", SqlDbType.Int); obj_cmd.Parameters["@EMPDEP_ID"].Value = model.empdep_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "EMPDEP006:" + ex.ToString();
            }

            return blnResult;
        }

        public List<cls_TRDep> getDataBatch(string com,string code, DateTime date)
        {
            List<cls_TRDep> list_model = new List<cls_TRDep>();
            cls_TRDep model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("EMP_TR_DEP.COMPANY_CODE");
                obj_str.Append(", EMP_TR_DEP.WORKER_CODE");
                obj_str.Append(", EMP_TR_DEP.EMPDEP_ID");
                obj_str.Append(", EMP_TR_DEP.EMPDEP_DATE");
                obj_str.Append(", ISNULL(EMP_TR_DEP.EMPDEP_LEVEL01, '') AS EMPDEP_LEVEL01");
                obj_str.Append(", ISNULL(EMP_TR_DEP.EMPDEP_LEVEL02, '') AS EMPDEP_LEVEL02");
                obj_str.Append(", ISNULL(EMP_TR_DEP.EMPDEP_LEVEL03, '') AS EMPDEP_LEVEL03");
                obj_str.Append(", ISNULL(EMP_TR_DEP.EMPDEP_LEVEL04, '') AS EMPDEP_LEVEL04");
                obj_str.Append(", ISNULL(EMP_TR_DEP.EMPDEP_LEVEL05, '') AS EMPDEP_LEVEL05");
                obj_str.Append(", ISNULL(EMP_TR_DEP.EMPDEP_LEVEL06, '') AS EMPDEP_LEVEL06");
                obj_str.Append(", ISNULL(EMP_TR_DEP.EMPDEP_LEVEL07, '') AS EMPDEP_LEVEL07");
                obj_str.Append(", ISNULL(EMP_TR_DEP.EMPDEP_LEVEL08, '') AS EMPDEP_LEVEL08");
                obj_str.Append(", ISNULL(EMP_TR_DEP.EMPDEP_LEVEL09, '') AS EMPDEP_LEVEL09");
                obj_str.Append(", ISNULL(EMP_TR_DEP.EMPDEP_LEVEL10, '') AS EMPDEP_LEVEL10");

                obj_str.Append(", INITIAL_NAME_TH + WORKER_FNAME_TH + ' ' + WORKER_LNAME_TH AS WORKER_DETAIL_TH");
                obj_str.Append(", INITIAL_NAME_EN + WORKER_FNAME_EN + ' ' + WORKER_LNAME_EN AS WORKER_DETAIL_EN");

                obj_str.Append(", ISNULL(EMP_TR_DEP.MODIFIED_BY, EMP_TR_DEP.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(EMP_TR_DEP.MODIFIED_DATE, EMP_TR_DEP.CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_TR_DEP");
                obj_str.Append(" INNER JOIN EMP_MT_WORKER ON EMP_MT_WORKER.COMPANY_CODE=EMP_TR_DEP.COMPANY_CODE AND EMP_MT_WORKER.WORKER_CODE=EMP_TR_DEP.WORKER_CODE");
                obj_str.Append(" INNER JOIN EMP_MT_INITIAL ON EMP_MT_INITIAL.INITIAL_CODE=EMP_MT_WORKER.WORKER_INITIAL ");
                obj_str.Append(" WHERE 1=1");
                obj_str.Append(" AND EMP_TR_DEP.COMPANY_CODE='" + com + "' ");

                if (!code.Equals(""))
                    obj_str.Append(" AND EMP_TR_DEP.EMPDEP_LEVEL01='" + code + "' ");
                if (!date.Equals(""))
                    obj_str.Append(" AND EMP_TR_DEP.EMPDEP_DATE='" + date.ToString("yyyy-MM-ddTHH:mm:ss") + "' ");

                obj_str.Append(" ORDER BY EMP_TR_DEP.WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRDep();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.empdep_id = Convert.ToInt32(dr["EMPDEP_ID"]);

                    model.empdep_date = Convert.ToDateTime(dr["EMPDEP_DATE"]);

                    model.empdep_level01 = dr["EMPDEP_LEVEL01"].ToString();
                    model.empdep_level02 = dr["EMPDEP_LEVEL02"].ToString();
                    model.empdep_level03 = dr["EMPDEP_LEVEL03"].ToString();
                    model.empdep_level04 = dr["EMPDEP_LEVEL04"].ToString();
                    model.empdep_level05 = dr["EMPDEP_LEVEL05"].ToString();
                    model.empdep_level06 = dr["EMPDEP_LEVEL06"].ToString();
                    model.empdep_level07 = dr["EMPDEP_LEVEL07"].ToString();
                    model.empdep_level08 = dr["EMPDEP_LEVEL08"].ToString();
                    model.empdep_level09 = dr["EMPDEP_LEVEL09"].ToString();
                    model.empdep_level10 = dr["EMPDEP_LEVEL10"].ToString();

                    model.worker_detail_th = dr["WORKER_DETAIL_TH"].ToString();
                    model.worker_detail_en = dr["WORKER_DETAIL_EN"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "EMPDEP007:" + ex.ToString();
            }

            return list_model;
        }

        public bool insertlist(List<cls_TRDep> list_model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_TR_DEP");
                obj_str.Append(" (");
                obj_str.Append("EMPDEP_ID ");
                obj_str.Append(", EMPDEP_DATE ");
                obj_str.Append(", EMPDEP_LEVEL01 ");
                obj_str.Append(", EMPDEP_LEVEL02 ");
                obj_str.Append(", EMPDEP_LEVEL03 ");
                obj_str.Append(", EMPDEP_LEVEL04 ");
                obj_str.Append(", EMPDEP_LEVEL05 ");
                obj_str.Append(", EMPDEP_LEVEL06 ");
                obj_str.Append(", EMPDEP_LEVEL07 ");
                obj_str.Append(", EMPDEP_LEVEL08 ");
                obj_str.Append(", EMPDEP_LEVEL09 ");
                obj_str.Append(", EMPDEP_LEVEL10 ");
                obj_str.Append(", EMPDEP_REASON ");
                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@EMPDEP_ID ");
                obj_str.Append(", @EMPDEP_DATE ");
                obj_str.Append(", @EMPDEP_LEVEL01 ");
                obj_str.Append(", @EMPDEP_LEVEL02 ");
                obj_str.Append(", @EMPDEP_LEVEL03 ");
                obj_str.Append(", @EMPDEP_LEVEL04 ");
                obj_str.Append(", @EMPDEP_LEVEL05 ");
                obj_str.Append(", @EMPDEP_LEVEL06 ");
                obj_str.Append(", @EMPDEP_LEVEL07 ");
                obj_str.Append(", @EMPDEP_LEVEL08 ");
                obj_str.Append(", @EMPDEP_LEVEL09 ");
                obj_str.Append(", @EMPDEP_LEVEL10 ");
                obj_str.Append(", @EMPDEP_REASON ");
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
                foreach (cls_TRDep model in list_model)
                {
                    strWorkerID += "'" + model.worker_code + "',";
                }
                if (strWorkerID.Length > 0)
                    strWorkerID = strWorkerID.Substring(0, strWorkerID.Length - 1);
                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM EMP_TR_DEP");
                obj_str2.Append(" WHERE 1=1 ");
                obj_str2.Append(" AND COMPANY_CODE='" + list_model[0].company_code + "'");
                obj_str2.Append(" AND WORKER_CODE IN (" + strWorkerID + ")");
                obj_str2.Append(" AND EMPDEP_DATE='" + list_model[0].empdep_date.ToString("yyyy-MM-ddTHH:mm:ss") + "'");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());

                if (blnResult)
                {
                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); 
                    obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPDEP_ID", SqlDbType.Int);
                    obj_cmd.Parameters.Add("@EMPDEP_DATE", SqlDbType.DateTime);
                    obj_cmd.Parameters.Add("@EMPDEP_LEVEL01", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPDEP_LEVEL02", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPDEP_LEVEL03", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPDEP_LEVEL04", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPDEP_LEVEL05", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPDEP_LEVEL06", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPDEP_LEVEL07", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPDEP_LEVEL08", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPDEP_LEVEL09", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPDEP_LEVEL10", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPDEP_REASON", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime);

                    foreach (cls_TRDep model in list_model)
                    {
                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                        obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                        obj_cmd.Parameters["@EMPDEP_ID"].Value = this.getNextID();
                        obj_cmd.Parameters["@EMPDEP_DATE"].Value = model.empdep_date;
                        obj_cmd.Parameters["@EMPDEP_LEVEL01"].Value = model.empdep_level01;
                        obj_cmd.Parameters["@EMPDEP_LEVEL02"].Value = model.empdep_level02;
                        obj_cmd.Parameters["@EMPDEP_LEVEL03"].Value = model.empdep_level03;
                        obj_cmd.Parameters["@EMPDEP_LEVEL04"].Value = model.empdep_level04;
                        obj_cmd.Parameters["@EMPDEP_LEVEL05"].Value = model.empdep_level05;
                        obj_cmd.Parameters["@EMPDEP_LEVEL06"].Value = model.empdep_level06;
                        obj_cmd.Parameters["@EMPDEP_LEVEL07"].Value = model.empdep_level07;
                        obj_cmd.Parameters["@EMPDEP_LEVEL08"].Value = model.empdep_level08;
                        obj_cmd.Parameters["@EMPDEP_LEVEL09"].Value = model.empdep_level09;
                        obj_cmd.Parameters["@EMPDEP_LEVEL10"].Value = model.empdep_level10;
                        obj_cmd.Parameters["@EMPDEP_REASON"].Value = model.empdep_reason;
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
                Message = "EMPDEP099:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
