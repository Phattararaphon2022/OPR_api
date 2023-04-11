using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTWorkflow
    {
         string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTWorkflow() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTWorkflow> getData(string condition)
        {
            List<cls_MTWorkflow> list_model = new List<cls_MTWorkflow>();
            cls_MTWorkflow model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("WORKFLOW_ID");
                obj_str.Append(", COMPANY_CODE");
                obj_str.Append(", WORKFLOW_CODE");
                obj_str.Append(", ISNULL(WORKFLOW_NAME_TH, '') AS WORKFLOW_NAME_TH");
                obj_str.Append(", ISNULL(WORKFLOW_NAME_EN, '') AS WORKFLOW_NAME_EN");
                obj_str.Append(", ISNULL(WORKFLOW_TYPE, '') AS WORKFLOW_TYPE");

                obj_str.Append(", ISNULL(STEP1, 0) AS STEP1");
                obj_str.Append(", ISNULL(STEP2, 0) AS STEP2");
                obj_str.Append(", ISNULL(STEP3, 0) AS STEP3");
                obj_str.Append(", ISNULL(STEP4, 0) AS STEP4");
                obj_str.Append(", ISNULL(STEP5, 0) AS STEP5");

                obj_str.Append(", ISNULL(TOTALAPPROVE, 2) AS TOTALAPPROVE"); 

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");
                obj_str.Append(", ISNULL(FLAG, 0) AS FLAG");

                obj_str.Append(" FROM SELF_MT_WORKFLOW");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY WORKFLOW_ID");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTWorkflow();

                    model.workflow_id = Convert.ToInt32(dr["WORKFLOW_ID"]);
                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.workflow_code = dr["WORKFLOW_CODE"].ToString();
                    model.workflow_name_th = dr["WORKFLOW_NAME_TH"].ToString();
                    model.workflow_name_en = dr["WORKFLOW_NAME_EN"].ToString();
                    model.workflow_type = dr["WORKFLOW_TYPE"].ToString();

                    model.step1 = Convert.ToInt32(dr["STEP1"]);
                    model.step2 = Convert.ToInt32(dr["STEP2"]);
                    model.step3 = Convert.ToInt32(dr["STEP3"]);
                    model.step4 = Convert.ToInt32(dr["STEP4"]);
                    model.step5 = Convert.ToInt32(dr["STEP5"]);

                    model.totalapprove = Convert.ToInt32(dr["TOTALAPPROVE"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    model.flag = Convert.ToBoolean(dr["FLAG"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Workflw.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTWorkflow> getDataByFillter(string com,string id,string code, string type)
        {
            string strCondition = "";
            if(!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!id.Equals(""))
                strCondition += " AND WORKFLOW_ID='" + id + "'";

            if (!code.Equals(""))
                strCondition += " AND WORKFLOW_CODE='" + code + "'";

            if (!type.Equals(""))
                strCondition += " AND WORKFLOW_TYPE='" + type + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT WORKFLOW_ID");
                obj_str.Append(" FROM SELF_MT_WORKFLOW");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                obj_str.Append(" AND WORKFLOW_CODE='" + code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Workflow.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(WORKFLOW_ID) ");
                obj_str.Append(" FROM SELF_MT_WORKFLOW");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Workflow.getNextID)" + ex.ToString();
            }

            return intResult;
        }

        public bool delete(string id,string com)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SELF_MT_WORKFLOW");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND WORKFLOW_ID='" + id + "'");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Workflow.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTWorkflow model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.workflow_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO SELF_MT_WORKFLOW");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKFLOW_ID ");
                obj_str.Append(", WORKFLOW_CODE ");
                obj_str.Append(", WORKFLOW_NAME_TH ");
                obj_str.Append(", WORKFLOW_NAME_EN ");
                obj_str.Append(", WORKFLOW_TYPE ");

                obj_str.Append(", STEP1 ");
                obj_str.Append(", STEP2 ");
                obj_str.Append(", STEP3 ");
                obj_str.Append(", STEP4 ");
                obj_str.Append(", STEP5 ");

                obj_str.Append(", TOTALAPPROVE ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKFLOW_ID ");
                obj_str.Append(", @WORKFLOW_CODE ");
                obj_str.Append(", @WORKFLOW_NAME_TH ");
                obj_str.Append(", @WORKFLOW_NAME_EN ");
                obj_str.Append(", @WORKFLOW_TYPE ");

                obj_str.Append(", @STEP1 ");
                obj_str.Append(", @STEP2 ");
                obj_str.Append(", @STEP3 ");
                obj_str.Append(", @STEP4 ");
                obj_str.Append(", @STEP5 ");

                obj_str.Append(", @TOTALAPPROVE ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKFLOW_ID", SqlDbType.Int); obj_cmd.Parameters["@WORKFLOW_ID"].Value = id;
                obj_cmd.Parameters.Add("@WORKFLOW_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKFLOW_CODE"].Value = model.workflow_code;
                obj_cmd.Parameters.Add("@WORKFLOW_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@WORKFLOW_NAME_TH"].Value = model.workflow_name_th;
                obj_cmd.Parameters.Add("@WORKFLOW_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@WORKFLOW_NAME_EN"].Value = model.workflow_name_en;
                obj_cmd.Parameters.Add("@WORKFLOW_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKFLOW_TYPE"].Value = model.workflow_type;

                obj_cmd.Parameters.Add("@STEP1", SqlDbType.Int); obj_cmd.Parameters["@STEP1"].Value = model.step1;
                obj_cmd.Parameters.Add("@STEP2", SqlDbType.Int); obj_cmd.Parameters["@STEP2"].Value = model.step2;
                obj_cmd.Parameters.Add("@STEP3", SqlDbType.Int); obj_cmd.Parameters["@STEP3"].Value = model.step3;
                obj_cmd.Parameters.Add("@STEP4", SqlDbType.Int); obj_cmd.Parameters["@STEP4"].Value = model.step4;
                obj_cmd.Parameters.Add("@STEP5", SqlDbType.Int); obj_cmd.Parameters["@STEP5"].Value = model.step5;

                obj_cmd.Parameters.Add("@TOTALAPPROVE", SqlDbType.Int); obj_cmd.Parameters["@TOTALAPPROVE"].Value = model.totalapprove;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = id.ToString() ;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Workflow.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_MTWorkflow model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SELF_MT_WORKFLOW SET ");
                obj_str.Append(" WORKFLOW_NAME_TH=@WORKFLOW_NAME_TH ");
                obj_str.Append(", WORKFLOW_NAME_EN=@WORKFLOW_NAME_EN ");
                obj_str.Append(", WORKFLOW_TYPE=@WORKFLOW_TYPE ");

                obj_str.Append(", STEP1=@STEP1 ");
                obj_str.Append(", STEP2=@STEP2 ");
                obj_str.Append(", STEP3=@STEP3 ");
                obj_str.Append(", STEP4=@STEP4 ");
                obj_str.Append(", STEP5=@STEP5 ");

                obj_str.Append(", TOTALAPPROVE=@TOTALAPPROVE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");

                if (!model.workflow_id.Equals(0))
                {
                    obj_str.Append(" WHERE WORKFLOW_ID=@WORKFLOW_ID ");
                }
                else
                {
                    obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                    obj_str.Append(" AND WORKFLOW_CODE=@WORKFLOW_CODE ");
                }

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());


                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKFLOW_ID", SqlDbType.Int); obj_cmd.Parameters["@WORKFLOW_ID"].Value = model.workflow_id;
                obj_cmd.Parameters.Add("@WORKFLOW_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKFLOW_CODE"].Value = model.workflow_code;
                obj_cmd.Parameters.Add("@WORKFLOW_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@WORKFLOW_NAME_TH"].Value = model.workflow_name_th;
                obj_cmd.Parameters.Add("@WORKFLOW_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@WORKFLOW_NAME_EN"].Value = model.workflow_name_en;
                obj_cmd.Parameters.Add("@WORKFLOW_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKFLOW_TYPE"].Value = model.workflow_type;

                obj_cmd.Parameters.Add("@STEP1", SqlDbType.Int); obj_cmd.Parameters["@STEP1"].Value = model.step1;
                obj_cmd.Parameters.Add("@STEP2", SqlDbType.Int); obj_cmd.Parameters["@STEP2"].Value = model.step2;
                obj_cmd.Parameters.Add("@STEP3", SqlDbType.Int); obj_cmd.Parameters["@STEP3"].Value = model.step3;
                obj_cmd.Parameters.Add("@STEP4", SqlDbType.Int); obj_cmd.Parameters["@STEP4"].Value = model.step4;
                obj_cmd.Parameters.Add("@STEP5", SqlDbType.Int); obj_cmd.Parameters["@STEP5"].Value = model.step5;

                obj_cmd.Parameters.Add("@TOTALAPPROVE", SqlDbType.Int); obj_cmd.Parameters["@TOTALAPPROVE"].Value = model.totalapprove;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;


                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.workflow_id.ToString() ;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Workflow.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
