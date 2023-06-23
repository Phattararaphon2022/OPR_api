using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;
namespace ClassLibrary_BPC.hrfocus.controller
{
  public  class cls_ctTRAccount
    {
             string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRAccount() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRAccount> getData(string condition)
        {
            List<cls_TRAccount> list_model = new List<cls_TRAccount>();
            cls_TRAccount model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("SELF_TR_ACCOUNT.COMPANY_CODE");
                obj_str.Append(", ACCOUNT_USER");
                obj_str.Append(", ACCOUNT_TYPE");
                obj_str.Append(", SELF_TR_ACCOUNT.WORKER_CODE");
                obj_str.Append(", INITIAL_NAME_TH + WORKER_FNAME_TH + ' ' + WORKER_LNAME_TH AS WORKER_DETAIL_TH");
                obj_str.Append(", INITIAL_NAME_EN + WORKER_FNAME_EN + ' ' + WORKER_LNAME_EN AS WORKER_DETAIL_EN");

                obj_str.Append(" FROM SELF_TR_ACCOUNT");
                obj_str.Append(" INNER JOIN EMP_MT_WORKER ON EMP_MT_WORKER.COMPANY_CODE=SELF_TR_ACCOUNT.COMPANY_CODE ");
                obj_str.Append(" AND EMP_MT_WORKER.WORKER_CODE=SELF_TR_ACCOUNT.WORKER_CODE ");
                obj_str.Append(" INNER JOIN EMP_MT_INITIAL ON EMP_MT_INITIAL.INITIAL_CODE=EMP_MT_WORKER.WORKER_INITIAL ");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY ACCOUNT_USER");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRAccount();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.account_user = dr["ACCOUNT_USER"].ToString();
                    model.account_type = dr["ACCOUNT_TYPE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.worker_detail_en = dr["WORKER_DETAIL_EN"].ToString();
                    model.worker_detail_th = dr["WORKER_DETAIL_TH"].ToString();

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRTRAccountpos.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRAccount> getDataByFillter(string com,string user,string type,string worker)
        {
            string strCondition = "";
            if(!com.Equals(""))
                strCondition += " AND SELF_TR_ACCOUNT.COMPANY_CODE='" + com + "'";

            if (!user.Equals(""))
                strCondition += " AND SELF_TR_ACCOUNT.ACCOUNT_USER='" + user + "'";

            if (!type.Equals(""))
                strCondition += " AND SELF_TR_ACCOUNT.ACCOUNT_TYPE='" + type + "'";

            if (!worker.Equals(""))
                strCondition += " AND SELF_TR_ACCOUNT.WORKER_CODE='" + worker + "'";

            return this.getData(strCondition);
        }
        private List<cls_TRAccount> getDataworkflow(string condition)
        {
            List<cls_TRAccount> list_model = new List<cls_TRAccount>();
            cls_TRAccount model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("SELF_TR_ACCOUNT.COMPANY_CODE ");
                obj_str.Append(", SELF_TR_ACCOUNT.ACCOUNT_USER ");
                obj_str.Append(", SELF_TR_ACCOUNT.ACCOUNT_TYPE ");
                obj_str.Append(", SELF_TR_ACCOUNT.WORKER_CODE ");
                obj_str.Append(", EMP_TR_POSITION.EMPPOSITION_POSITION ");
                obj_str.Append(", EMP_MT_POSITION.POSITION_LEVEL ");
                obj_str.Append(", ISNULL(SELF_TR_LINEAPPROVE.WORKFLOW_CODE,'') AS WORKFLOW_CODE ");
                obj_str.Append(", ISNULL(SELF_TR_LINEAPPROVE.WORKFLOW_TYPE,'') AS WORKFLOW_TYPE ");
                obj_str.Append(", SELF_MT_WORKFLOW.TOTALAPPROVE ");

                obj_str.Append(" FROM SELF_TR_ACCOUNT");
                obj_str.Append(" JOIN EMP_TR_POSITION ON EMP_TR_POSITION.WORKER_CODE = SELF_TR_ACCOUNT.WORKER_CODE AND EMP_TR_POSITION.COMPANY_CODE = SELF_TR_ACCOUNT.COMPANY_CODE");
                obj_str.Append(" AND EMP_TR_POSITION.EMPPOSITION_DATE = (SELECT MAX(EMPPOSITION_DATE) FROM EMP_TR_POSITION WHERE WORKER_CODE=SELF_TR_ACCOUNT.WORKER_CODE)");
                obj_str.Append(" JOIN EMP_MT_POSITION ON EMP_MT_POSITION.POSITION_CODE = EMP_TR_POSITION.EMPPOSITION_POSITION");
                obj_str.Append(" AND EMP_MT_POSITION.COMPANY_CODE = SELF_TR_ACCOUNT.COMPANY_CODE");
                obj_str.Append(" LEFT JOIN SELF_TR_LINEAPPROVE ON SELF_TR_LINEAPPROVE.POSITION_LEVEL = EMP_MT_POSITION.POSITION_LEVEL");
                obj_str.Append(" AND SELF_TR_LINEAPPROVE.COMPANY_CODE = SELF_TR_ACCOUNT.COMPANY_CODE");
                obj_str.Append(" LEFT JOIN SELF_MT_WORKFLOW ON SELF_MT_WORKFLOW.WORKFLOW_CODE = SELF_TR_LINEAPPROVE.WORKFLOW_CODE");
                obj_str.Append(" AND SELF_MT_WORKFLOW.COMPANY_CODE = SELF_TR_ACCOUNT.COMPANY_CODE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRAccount();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.account_user = dr["ACCOUNT_USER"].ToString();
                    model.account_type = dr["ACCOUNT_TYPE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.empposition_position = dr["EMPPOSITION_POSITION"].ToString();
                    model.position_level = Convert.ToInt32(dr["POSITION_LEVEL"].ToString());
                    model.workflow_code = dr["WORKFLOW_CODE"].ToString();
                    model.workflow_type = dr["WORKFLOW_TYPE"].ToString();
                    model.totalapprove = Convert.ToInt32(dr["TOTALAPPROVE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRTRAccountpos.getDataworkflow)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRAccount> getDataworkflowByFillter(string com, string user,string worker_code, string type, string workflow_type)
        {
            string strCondition = "";
            if (!com.Equals(""))
                strCondition += " AND SELF_TR_ACCOUNT.COMPANY_CODE='" + com + "'";

            if (!user.Equals(""))
                strCondition += " AND SELF_TR_ACCOUNT.ACCOUNT_USER='" + user + "'";

            if (!worker_code.Equals(""))
                strCondition += " AND SELF_TR_ACCOUNT.WORKER_CODE='" + worker_code + "'";

            if (!type.Equals(""))
                strCondition += " AND SELF_TR_ACCOUNT.ACCOUNT_TYPE='" + type + "'";

            if (!workflow_type.Equals(""))
                strCondition += " AND SELF_TR_LINEAPPROVE.WORKFLOW_TYPE='" + workflow_type + "'";

            return this.getDataworkflow(strCondition);
        }

        public bool checkDataOld(string com,string user,string type,string worker)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ACCOUNT_USER");
                obj_str.Append(" FROM SELF_TR_ACCOUNT");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                obj_str.Append(" AND ACCOUNT_USER='" + user + "'");
                obj_str.Append(" AND ACCOUNT_TYPE='" + type + "'");
                if (!worker.Equals(""))
                {
                    obj_str.Append(" AND WORKER_CODE='" + worker + "'");
                }

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRAccountpos.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(string com, string user, string type, string worker)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SELF_TR_ACCOUNT");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND ACCOUNT_USER='" + user + "'");
                obj_str.Append(" AND ACCOUNT_TYPE='" + type + "'");
                if (!worker.Equals(""))
                {
                    obj_str.Append(" AND WORKER_CODE='" + worker + "'");
                }

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(TRAccountpos.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_TRAccount model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.account_user,model.account_type,model.worker_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("INSERT INTO SELF_TR_ACCOUNT");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", ACCOUNT_USER ");
                obj_str.Append(", ACCOUNT_TYPE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @ACCOUNT_USER ");
                obj_str.Append(", @ACCOUNT_TYPE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@ACCOUNT_USER", SqlDbType.VarChar); obj_cmd.Parameters["@ACCOUNT_USER"].Value = model.account_user;
                obj_cmd.Parameters.Add("@ACCOUNT_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@ACCOUNT_TYPE"].Value = model.account_type;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = model.account_user;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRAccountpos.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(List<cls_TRAccount> list_model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SELF_TR_ACCOUNT");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", ACCOUNT_USER ");
                obj_str.Append(", ACCOUNT_TYPE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @ACCOUNT_USER ");
                obj_str.Append(", @ACCOUNT_TYPE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(" )");


                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                //-- Step 1 delete data old
                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM SELF_TR_ACCOUNT");
                obj_str2.Append(" WHERE 1=1 ");
                obj_str2.Append(" AND COMPANY_CODE='" + list_model[0].company_code + "'");
                obj_str2.Append(" AND ACCOUNT_USER='" + list_model[0].account_user + "' ");
                obj_str2.Append(" AND ACCOUNT_TYPE='" + list_model[0].account_type + "' ");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());
                if (blnResult)
                {
                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@ACCOUNT_USER", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@ACCOUNT_TYPE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar);
                    foreach (cls_TRAccount model in list_model)
                    {

                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                        obj_cmd.Parameters["@ACCOUNT_USER"].Value = model.account_user;
                        obj_cmd.Parameters["@ACCOUNT_TYPE"].Value = model.account_type;
                        obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                        obj_cmd.ExecuteNonQuery();

                    }

                    blnResult = obj_conn.doCommit();

                    if (!blnResult)
                    {
                        obj_conn.doRollback();
                    }
                }
                else
                {
                    obj_conn.doRollback();
                }
                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRAccountpos.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_TRAccount model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SELF_TR_ACCOUNT SET ");
                obj_str.Append(" COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(", ACCOUNT_USER=@ACCOUNT_USER ");
                obj_str.Append(", ACCOUNT_TYPE=@ACCOUNT_TYPE ");
                obj_str.Append(", WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND ACCOUNT_USER=@ACCOUNT_USER ");
                obj_str.Append(" AND ACCOUNT_TYPE=@ACCOUNT_TYPE ");
      

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());


                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@ACCOUNT_USER", SqlDbType.Int); obj_cmd.Parameters["@ACCOUNT_USER"].Value = model.account_user;
                obj_cmd.Parameters.Add("@ACCOUNT_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@ACCOUNT_TYPE"].Value = model.account_type;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.account_user;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRAccountpos.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
