using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRApprove
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRApprove() { }

        public string getMessage() { return this.Message.Replace("SYS_TR_APPROVE", "").Replace("cls_ctTRApprove", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRApprove> getData(string condition)
        {
            List<cls_TRApprove> list_model = new List<cls_TRApprove>();
            cls_TRApprove model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", APPROVE_CODE");
                obj_str.Append(", WORKFLOW_TYPE");
                obj_str.Append(", APPROVE_BY");
                obj_str.Append(", APPROVE_STATUS");
                obj_str.Append(", APPROVE_DATE");
                obj_str.Append(", WORKFLOW_TYPE");

                obj_str.Append(", ISNULL(APPROVE_NOTE, '') AS APPROVE_NOTE");

                obj_str.Append(" FROM SYS_TR_APPROVE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRApprove();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.approve_code = dr["APPROVE_CODE"].ToString();
                    model.workflow_type = dr["WORKFLOW_TYPE"].ToString();
                    model.approve_by = dr["APPROVE_BY"].ToString();
                    model.approve_status = dr["APPROVE_STATUS"].ToString();

                    model.approve_date = Convert.ToDateTime(dr["APPROVE_DATE"]);
                    model.approve_status = dr["APPROVE_STATUS"].ToString();

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(SysApprove.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRApprove> getDataByFillter(string com, string workflow, string subject_code)
        {
            string strCondition = "";
            if(!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";
            if (!workflow.Equals(""))
                strCondition += " AND WORKFLOW_TYPE='" + workflow + "'";
            if (!subject_code.Equals(""))
                strCondition += " AND APPROVE_CODE='" + subject_code + "'";


            return this.getData(strCondition);
        }

        public List<cls_TRApprove> getDataAllApprove(string com, string workflow, string subject_code)
        {
            List<cls_TRApprove> list_model = new List<cls_TRApprove>();
            cls_TRApprove model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                
                obj_str.Append("SELECT SYS_TR_WORKFLOW.COMPANY_CODE AS  COMPANY_CODE");
                obj_str.Append(", SYS_TR_WORKFLOW.WORKFLOW_TYPE AS WORKFLOW_TYPE");
                obj_str.Append(", SYS_TR_WORKFLOW.ACCOUNT_USER AS ACCOUNT_USER");
                obj_str.Append(", ISNULL(APPR.APPROVE_DATE, '01/01/1900') AS APPROVE_DATE");
                obj_str.Append(", ISNULL(APPR.APPROVE_STATUS, '-') AS APPROVE_STATUS");
                obj_str.Append(", ISNULL(APPR.APPROVE_NOTE, '') AS APPROVE_NOTE");
                obj_str.Append(" FROM SYS_TR_WORKFLOW");
                obj_str.Append(" LEFT JOIN (");
                obj_str.Append(" SELECT COMPANY_CODE, WORKFLOW_TYPE, APPROVE_DATE, APPROVE_STATUS, APPROVE_BY, APPROVE_NOTE");
                obj_str.Append(" FROM SYS_TR_APPROVE");
                obj_str.Append(" WHERE COMPANY_CODE='"+ com + "'");
                obj_str.Append(" AND APPROVE_CODE='" + subject_code + "'");
                obj_str.Append(" AND WORKFLOW_TYPE='" + workflow + "'");
                obj_str.Append(" ) APPR ON SYS_TR_WORKFLOW.COMPANY_CODE = APPR.COMPANY_CODE AND SYS_TR_WORKFLOW.WORKFLOW_TYPE = APPR.WORKFLOW_TYPE AND SYS_TR_WORKFLOW.ACCOUNT_USER = APPR.APPROVE_BY");
                obj_str.Append(" WHERE SYS_TR_WORKFLOW.COMPANY_CODE = '"+ com + "'");
                obj_str.Append(" AND SYS_TR_WORKFLOW.WORKFLOW_TYPE = '" + workflow + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRApprove();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.approve_code = subject_code;
                    model.workflow_type = dr["WORKFLOW_TYPE"].ToString();
                    model.approve_by = dr["ACCOUNT_USER"].ToString();
                    model.approve_status = dr["APPROVE_STATUS"].ToString();

                    model.approve_date = Convert.ToDateTime(dr["APPROVE_DATE"]);
                    model.approve_status = dr["APPROVE_STATUS"].ToString();

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(SysApprove.getData)" + ex.ToString();
            }

            return list_model;
        }

        private bool checkDataOld(string com, string workflow, string subject_code, string username)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM SYS_TR_APPROVE");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                obj_str.Append(" AND WORKFLOW_TYPE='" + workflow + "'");
                obj_str.Append(" AND APPROVE_CODE='" + subject_code + "'");
                obj_str.Append(" AND APPROVE_BY='" + username + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(SysApprove.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public bool checkDataOld(cls_TRApprove model)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM SYS_TR_APPROVE");
                obj_str.Append(" WHERE COMPANY_CODE ='" + model.company_code + "' ");
                obj_str.Append(" AND WORKFLOW_TYPE='" + model.workflow_type + "'");
                obj_str.Append(" AND APPROVE_CODE='" + model.approve_code + "'");
                obj_str.Append(" AND APPROVE_BY='" + model.approve_by + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(SysApprove.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(string com, string workflow, string subject_code, string username)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SYS_TR_APPROVE");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                obj_str.Append(" AND WORKFLOW_TYPE='" + workflow + "'");
                obj_str.Append(" AND APPROVE_CODE='" + subject_code + "'");
                obj_str.Append(" AND APPROVE_BY='" + username + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(SysApprove.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(APPROVE_ID, 1) ");
                obj_str.Append(" FROM SYS_TR_APPROVE");
                obj_str.Append(" ORDER BY APPROVE_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "BNK002:" + ex.ToString();
            }

            return intResult;
        }

        public bool insert(cls_TRApprove model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.workflow_type, model.approve_code, model.approve_by))
                {
                    return false;
                }
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("INSERT INTO SYS_TR_APPROVE");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                //obj_str.Append(", APPROVE_ID ");
                obj_str.Append(", APPROVE_CODE ");
                obj_str.Append(", WORKFLOW_TYPE ");
                obj_str.Append(", APPROVE_BY ");
                obj_str.Append(", APPROVE_STATUS ");
                obj_str.Append(", APPROVE_DATE ");
                obj_str.Append(", APPROVE_NOTE ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                //obj_str.Append(", @APPROVE_ID ");
                obj_str.Append(", @APPROVE_CODE ");
                obj_str.Append(", @WORKFLOW_TYPE ");
                obj_str.Append(", @APPROVE_BY ");
                obj_str.Append(", @APPROVE_STATUS ");
                obj_str.Append(", @APPROVE_DATE ");
                obj_str.Append(", @APPROVE_NOTE ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                //obj_cmd.Parameters.Add("@APPROVE_ID", SqlDbType.Int); obj_cmd.Parameters["@APPROVE_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@APPROVE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@APPROVE_CODE"].Value = model.approve_code;
                obj_cmd.Parameters.Add("@WORKFLOW_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKFLOW_TYPE"].Value = model.workflow_type;
                obj_cmd.Parameters.Add("@APPROVE_BY", SqlDbType.VarChar); obj_cmd.Parameters["@APPROVE_BY"].Value = model.approve_by;
                obj_cmd.Parameters.Add("@APPROVE_STATUS", SqlDbType.VarChar); obj_cmd.Parameters["@APPROVE_STATUS"].Value = model.approve_status;
                obj_cmd.Parameters.Add("@APPROVE_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@APPROVE_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@APPROVE_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@APPROVE_NOTE"].Value = model.approve_note;
                

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(SysApprove.insert)" + ex.ToString();
            }

            return blnResult;
        }
        public bool insert(List<cls_TRApprove> list_model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_TR_APPROVE");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                //obj_str.Append(", APPROVE_ID ");
                obj_str.Append(", APPROVE_CODE ");
                obj_str.Append(", WORKFLOW_TYPE ");
                obj_str.Append(", APPROVE_BY ");
                obj_str.Append(", APPROVE_STATUS ");
                obj_str.Append(", APPROVE_DATE ");
                obj_str.Append(", APPROVE_NOTE ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                //obj_str.Append(", @APPROVE_ID ");
                obj_str.Append(", @APPROVE_CODE ");
                obj_str.Append(", @WORKFLOW_TYPE ");
                obj_str.Append(", @APPROVE_BY ");
                obj_str.Append(", @APPROVE_STATUS ");
                obj_str.Append(", @APPROVE_DATE ");
                obj_str.Append(", @APPROVE_NOTE ");
                obj_str.Append(" )");


                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                
                if (true)
                {
                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); 
                    //obj_cmd.Parameters.Add("@APPROVE_ID", SqlDbType.Int); 
                    obj_cmd.Parameters.Add("@APPROVE_CODE", SqlDbType.VarChar); 
                    obj_cmd.Parameters.Add("@WORKFLOW_TYPE", SqlDbType.VarChar); 
                    obj_cmd.Parameters.Add("@APPROVE_BY", SqlDbType.VarChar); 
                    obj_cmd.Parameters.Add("@APPROVE_STATUS", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@APPROVE_DATE", SqlDbType.DateTime); 
                    obj_cmd.Parameters.Add("@APPROVE_NOTE", SqlDbType.VarChar);
                    


                    foreach (cls_TRApprove model in list_model)
                    {
                        //-- Check data old
                        if (this.checkDataOld(model.company_code, model.workflow_type, model.approve_code, model.approve_by))
                        {
                            continue;
                        }
                        
                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                        //obj_cmd.Parameters["@APPROVE_ID"].Value = this.getNextID();
                        obj_cmd.Parameters["@APPROVE_CODE"].Value = model.approve_code;
                        obj_cmd.Parameters["@WORKFLOW_TYPE"].Value = model.workflow_type;
                        obj_cmd.Parameters["@APPROVE_BY"].Value = model.approve_by;
                        obj_cmd.Parameters["@APPROVE_STATUS"].Value = model.approve_status;
                        obj_cmd.Parameters["@APPROVE_DATE"].Value = DateTime.Now;
                        obj_cmd.Parameters["@APPROVE_NOTE"].Value = model.approve_note;
                        
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
                Message = "ERROR::(SysApprove.insert)" + ex.ToString();
            }

            return blnResult;
        }

    }
}
