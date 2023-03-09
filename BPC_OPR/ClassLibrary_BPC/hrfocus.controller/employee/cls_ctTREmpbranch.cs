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
    public class cls_ctTREmpbranch
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTREmpbranch() { }

        public string getMessage() { return this.Message.Replace("EMP_TR_BRANCH", "").Replace("cls_ctTREmpbranch", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TREmpbranch> getData(string condition)
        {
            List<cls_TREmpbranch> list_model = new List<cls_TREmpbranch>();
            cls_TREmpbranch model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");
                obj_str.Append(", BRANCH_CODE");
                obj_str.Append(", EMPBRANCH_STARTDATE");
                obj_str.Append(", EMPBRANCH_ENDDATE");
                obj_str.Append(", EMPBRANCH_NOTE");


                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_TR_BRANCH");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TREmpbranch();



                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.branch_code = dr["BRANCH_CODE"].ToString();
                    model.empbranch_startdate = Convert.ToDateTime(dr["EMPBRANCH_STARTDATE"]);
                    model.empbranch_enddate = Convert.ToDateTime(dr["EMPBRANCH_ENDDATE"]);
                    model.empbranch_note = dr["EMPBRANCH_NOTE"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "EMPBRN001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TREmpbranch> getDataByFillter(string com, string emp)
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

                obj_str.Append("SELECT ISNULL(EMPBRANCH_ID, 1) ");
                obj_str.Append(" FROM EMP_TR_BRANCH");
                obj_str.Append(" ORDER BY EMPBRANCH_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPBRN002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT BRANCH_CODE");
                obj_str.Append(" FROM EMP_TR_BRANCH");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPBRN003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM EMP_TR_BRANCH");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "EMPBRN004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TREmpbranch model)
        {
            bool blnResult = false;
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.company_code, model.worker_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_TR_BRANCH");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", BRANCH_CODE ");
                obj_str.Append(", EMPBRANCH_STARTDATE ");
                obj_str.Append(", EMPBRANCH_ENDDATE ");
                obj_str.Append(", EMPBRANCH_NOTE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @BRANCH_CODE ");
                obj_str.Append(", @EMPBRANCH_STARTDATE ");
                obj_str.Append(", @EMPBRANCH_ENDDATE ");
                obj_str.Append(", @EMPBRANCH_NOTE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                //model.family_id = this.getNextID();

                //obj_cmd.Parameters.Add("@FAMILY_ID", SqlDbType.Int); obj_cmd.Parameters["@FAMILY_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@BRANCH_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@BRANCH_CODE"].Value = model.branch_code;
                obj_cmd.Parameters.Add("@EMPBRANCH_STARTDATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPBRANCH_STARTDATE"].Value = model.empbranch_startdate;
                obj_cmd.Parameters.Add("@EMPBRANCH_ENDDATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPBRANCH_ENDDATE"].Value = model.empbranch_enddate;
                obj_cmd.Parameters.Add("@EMPBRANCH_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@EMPBRANCH_NOTE"].Value = model.empbranch_note;



                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                strResult = model.branch_code.ToString();
            }
            catch (Exception ex)
            {
                Message = "EMPBRN005:" + ex.ToString();
                strResult = "";
            }

            return blnResult;
        }

        public bool update(cls_TREmpbranch model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE EMP_TR_BRANCH SET ");

                obj_str.Append(" BRANCH_CODE=@BRANCH_CODE ");
                obj_str.Append(", EMPBRANCH_STARTDATE=@EMPBRANCH_STARTDATE ");
                obj_str.Append(", EMPBRANCH_ENDDATE=@EMPBRANCH_ENDDATE ");
                obj_str.Append(", EMPBRANCH_NOTE=@EMPBRANCH_NOTE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@BRANCH_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@BRANCH_CODE"].Value = model.branch_code;
                obj_cmd.Parameters.Add("@EMPBRANCH_STARTDATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPBRANCH_STARTDATE"].Value = model.empbranch_startdate;
                obj_cmd.Parameters.Add("@EMPBRANCH_ENDDATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPBRANCH_ENDDATE"].Value = model.empbranch_enddate;
                obj_cmd.Parameters.Add("@EMPBRANCH_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@EMPBRANCH_NOTE"].Value = model.empbranch_note;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "EMPBRN006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
