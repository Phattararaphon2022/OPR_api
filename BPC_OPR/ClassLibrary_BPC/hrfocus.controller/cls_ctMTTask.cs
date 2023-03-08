using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary_BPC.hrfocus.model;
using System.Data.SqlClient;
using System.Data;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTTask
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTTask() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTTask> getData(string condition)
        {
            List<cls_MTTask> list_model = new List<cls_MTTask>();
            cls_MTTask model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", PROJECT_CODE");
                obj_str.Append(", SYS_MT_TASK.TASK_ID");
                obj_str.Append(", TASK_TYPE");
                obj_str.Append(", TASK_STATUS");
               
                obj_str.Append(", ISNULL(TASK_START, '01/01/1900') AS TASK_START");
                obj_str.Append(", ISNULL(TASK_END, '01/01/1900') AS TASK_END");
                obj_str.Append(", ISNULL(TASK_NOTE, '') AS TASK_NOTE");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(", ISNULL(TASKDETAIL_PROCESS, '') AS TASKDETAIL_PROCESS");
                obj_str.Append(", ISNULL(TASKDETAIL_FROMDATE, '01/01/1900') AS TASKDETAIL_FROMDATE");
                obj_str.Append(", ISNULL(TASKDETAIL_TODATE, '01/01/1900') AS TASKDETAIL_TODATE");
                obj_str.Append(", ISNULL(TASKDETAIL_PAYDATE, '01/01/1900') AS TASKDETAIL_PAYDATE");
              

                obj_str.Append(" FROM SYS_MT_TASK");

                obj_str.Append(" INNER JOIN SYS_TR_TASKDETAIL ON SYS_MT_TASK.TASK_ID=SYS_TR_TASKDETAIL.TASK_ID");


                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, SYS_MT_TASK.TASK_ID DESC");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTTask();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                    model.project_code = Convert.ToString(dr["PROJECT_CODE"]);
                    model.task_id = Convert.ToInt32(dr["TASK_ID"]);
                    model.task_type = Convert.ToString(dr["TASK_TYPE"]);
                    model.task_status = Convert.ToString(dr["TASK_STATUS"]);
                    model.task_start = Convert.ToDateTime(dr["TASK_START"]);
                    model.task_end = Convert.ToDateTime(dr["TASK_END"]);

                    model.task_note = Convert.ToString(dr["TASK_NOTE"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    model.taskdetail_process = Convert.ToString(dr["TASKDETAIL_PROCESS"]);
                    model.taskdetail_fromdate = Convert.ToDateTime(dr["TASKDETAIL_FROMDATE"]);
                    model.taskdetail_todate = Convert.ToDateTime(dr["TASKDETAIL_TODATE"]);
                    model.taskdetail_paydate = Convert.ToDateTime(dr["TASKDETAIL_PAYDATE"]);

                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "ERROR::(Task.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTTask> getDataByFillter(string com, string id, string type, string status)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!id.Equals(""))
                strCondition += " AND SYS_MT_TASK.TASK_ID='" + id + "'";

            if (!type.Equals(""))
                strCondition += " AND TASK_TYPE='" + type + "'";

            if (!status.Equals(""))
                strCondition += " AND TASK_STATUS='" + status + "'";

            return this.getData(strCondition);
        }

        public cls_TRTaskdetail getTaskDetail(string id)
        {
            
            cls_TRTaskdetail model = null;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("TASK_ID");
                obj_str.Append(", ISNULL(TASKDETAIL_PROCESS, '') AS TASKDETAIL_PROCESS");
                obj_str.Append(", ISNULL(TASKDETAIL_FROMDATE, '01/01/1900') AS TASKDETAIL_FROMDATE");
                obj_str.Append(", ISNULL(TASKDETAIL_TODATE, '01/01/1900') AS TASKDETAIL_TODATE");
                obj_str.Append(", ISNULL(TASKDETAIL_PAYDATE, '01/01/1900') AS TASKDETAIL_PAYDATE");
                
                obj_str.Append(" FROM SYS_TR_TASKDETAIL");
                obj_str.Append(" WHERE TASK_ID='" + id + "'");
                
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRTaskdetail();

                    model.task_id = Convert.ToInt32(dr["TASK_ID"]);           
                    model.taskdetail_fromdate = Convert.ToDateTime(dr["TASKDETAIL_FROMDATE"]);
                    model.taskdetail_todate = Convert.ToDateTime(dr["TASKDETAIL_TODATE"]);
                    model.taskdetail_paydate = Convert.ToDateTime(dr["TASKDETAIL_PAYDATE"]);

                    model.taskdetail_process = dr["TASKDETAIL_PROCESS"].ToString();
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Task.getTaskDetail)" + ex.ToString();
            }

            return model;
        }

        public List<cls_TRTaskwhose> getTaskWhose(string id)
        {
            List<cls_TRTaskwhose> list_model = new List<cls_TRTaskwhose>();
            cls_TRTaskwhose model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");
                obj_str.Append("TASK_ID");
                obj_str.Append(", WORKER_CODE");                
                obj_str.Append(" FROM SYS_TR_TASKWHOSE");
                obj_str.Append(" WHERE TASK_ID='" + id + "'");           
                obj_str.Append(" ORDER BY WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRTaskwhose();
                    
                    model.task_id = Convert.ToInt32(dr["TASK_ID"]);
                    model.worker_code = Convert.ToString(dr["WORKER_CODE"]);
                   
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Task.getTaskWhose)" + ex.ToString();
            }

            return list_model;
        }

        public bool checkDataOld(string com, string id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT TASK_ID");
                obj_str.Append(" FROM SYS_MT_TASK");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND TASK_ID='" + id + "'");
                                                
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Task.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(TASK_ID) ");
                obj_str.Append(" FROM SYS_MT_TASK");                

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Task.getNextID)" + ex.ToString();
            }

            return intResult;
        }

        public bool delete(string id)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SYS_MT_TASK");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND TASK_ID ='" + id + "'");
                                              
                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

                if (blnResult)
                {
                    obj_str.Append(" DELETE FROM SYS_TR_TASKDETAIL");
                    obj_str.Append(" WHERE 1=1 ");
                    obj_str.Append(" AND TASK_ID='" + id + "'");

                    obj_conn.doExecuteSQL(obj_str.ToString());

                    obj_str.Append(" DELETE FROM SYS_TR_TASKWHOSE");
                    obj_str.Append(" WHERE 1=1 ");
                    obj_str.Append(" AND TASK_ID='" + id + "'");

                    obj_conn.doExecuteSQL(obj_str.ToString());

                }

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Task.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public int insert(cls_MTTask model, cls_TRTaskdetail detail, List<cls_TRTaskwhose> list_whose)
        {
            int intResult = 0;
            try
            {                
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_MT_TASK");
                obj_str.Append(" (");
                obj_str.Append("TASK_ID ");
                obj_str.Append(", TASK_TYPE ");
                obj_str.Append(", TASK_STATUS ");                                                
                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", PROJECT_CODE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@TASK_ID ");
                obj_str.Append(", @TASK_TYPE ");
                obj_str.Append(", @TASK_STATUS ");                            
                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @PROJECT_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");              
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                int intTaskID = this.getNextID();

                obj_cmd.Parameters.Add("@TASK_ID", SqlDbType.Int); obj_cmd.Parameters["@TASK_ID"].Value = intTaskID;
                obj_cmd.Parameters.Add("@TASK_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@TASK_TYPE"].Value = model.task_type;
                obj_cmd.Parameters.Add("@TASK_STATUS", SqlDbType.VarChar); obj_cmd.Parameters["@TASK_STATUS"].Value = model.task_status;
                                             
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;
     
                int intCount = obj_cmd.ExecuteNonQuery();

                if(intCount > 0)
                {
                    intResult = intTaskID;
                    
                    if (detail != null)
                    {
                        obj_str = new System.Text.StringBuilder();
                        obj_str.Append("INSERT INTO SYS_TR_TASKDETAIL");
                        obj_str.Append(" (");
                        obj_str.Append("TASK_ID ");
                        obj_str.Append(", TASKDETAIL_PROCESS ");
                        obj_str.Append(", TASKDETAIL_FROMDATE ");
                        obj_str.Append(", TASKDETAIL_TODATE ");
                        obj_str.Append(", TASKDETAIL_PAYDATE ");
                        obj_str.Append(" )");

                        obj_str.Append(" VALUES(");
                        obj_str.Append("@TASK_ID ");
                        obj_str.Append(", @TASKDETAIL_PROCESS ");
                        obj_str.Append(", @TASKDETAIL_FROMDATE ");
                        obj_str.Append(", @TASKDETAIL_TODATE ");
                        obj_str.Append(", @TASKDETAIL_PAYDATE ");
                        obj_str.Append(" )");

                        obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                        obj_cmd.Parameters.Add("@TASK_ID", SqlDbType.Int); obj_cmd.Parameters["@TASK_ID"].Value = intTaskID;
                        obj_cmd.Parameters.Add("@TASKDETAIL_PROCESS", SqlDbType.VarChar); obj_cmd.Parameters["@TASKDETAIL_PROCESS"].Value = detail.taskdetail_process;
                        obj_cmd.Parameters.Add("@TASKDETAIL_FROMDATE", SqlDbType.DateTime); obj_cmd.Parameters["@TASKDETAIL_FROMDATE"].Value = detail.taskdetail_fromdate;
                        obj_cmd.Parameters.Add("@TASKDETAIL_TODATE", SqlDbType.DateTime); obj_cmd.Parameters["@TASKDETAIL_TODATE"].Value = detail.taskdetail_todate;
                        obj_cmd.Parameters.Add("@TASKDETAIL_PAYDATE", SqlDbType.DateTime); obj_cmd.Parameters["@TASKDETAIL_PAYDATE"].Value = detail.taskdetail_paydate;

                        obj_cmd.ExecuteNonQuery();
                    }

                    if(list_whose.Count > 0)
                    {
                        obj_str = new System.Text.StringBuilder();
                        obj_str.Append("INSERT INTO SYS_TR_TASKWHOSE");
                        obj_str.Append(" (");
                        obj_str.Append("TASK_ID ");
                        obj_str.Append(", WORKER_CODE ");                       
                        obj_str.Append(" )");

                        obj_str.Append(" VALUES(");
                        obj_str.Append("@TASK_ID ");
                        obj_str.Append(", @WORKER_CODE ");                       
                        obj_str.Append(" )");

                        obj_conn.doOpenTransaction();

                        obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                        obj_cmd.Transaction = obj_conn.getTransaction();

                        obj_cmd.Parameters.Add("@TASK_ID", SqlDbType.Int);
                        obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar);
                       

                        foreach (cls_TRTaskwhose whose in list_whose)
                        {

                            obj_cmd.Parameters["@TASK_ID"].Value = intTaskID;
                            obj_cmd.Parameters["@WORKER_CODE"].Value = whose.worker_code;
                            
                            obj_cmd.ExecuteNonQuery();

                        }

                        obj_conn.doCommit();
                        
                    }
                }


                obj_conn.doClose();

                
            }
            catch (Exception ex)
            {              
                Message = "ERROR::(Task.insert)" + ex.ToString();
                intResult = 0;
            }

            return intResult;
        }

        public bool update(cls_MTTask model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SYS_MT_TASK SET ");

                obj_str.Append(" TASK_TYPE=@TASK_TYPE ");
                obj_str.Append(", TASK_STATUS=@TASK_STATUS ");
                                                
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");
                
                obj_str.Append(" WHERE TASK_ID=@TASK_ID ");

                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@TASK_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@TASK_TYPE"].Value = model.task_type;
                obj_cmd.Parameters.Add("@TASK_STATUS", SqlDbType.VarChar); obj_cmd.Parameters["@TASK_STATUS"].Value = model.task_status;
                                
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@TASK_ID", SqlDbType.Int); obj_cmd.Parameters["@TASK_ID"].Value = model.task_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Task.update)" + ex.ToString();
            }

            return blnResult;
        }

        public bool updateStatus(cls_MTTask model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SYS_MT_TASK SET ");

                obj_str.Append(" TASK_STATUS=@TASK_STATUS ");
                obj_str.Append(", TASK_START=@TASK_START ");
                obj_str.Append(", TASK_END=@TASK_END ");
                obj_str.Append(", TASK_NOTE=@TASK_NOTE ");
                
                obj_str.Append(" WHERE TASK_ID=@TASK_ID ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@TASK_STATUS", SqlDbType.VarChar); obj_cmd.Parameters["@TASK_STATUS"].Value = model.task_status;
                obj_cmd.Parameters.Add("@TASK_START", SqlDbType.DateTime); obj_cmd.Parameters["@TASK_START"].Value = model.task_start;
                obj_cmd.Parameters.Add("@TASK_END", SqlDbType.DateTime); obj_cmd.Parameters["@TASK_END"].Value = model.task_end;
                obj_cmd.Parameters.Add("@TASK_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@TASK_NOTE"].Value = model.task_note;                

                obj_cmd.Parameters.Add("@TASK_ID", SqlDbType.Int); obj_cmd.Parameters["@TASK_ID"].Value = model.task_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Task.updateStatus)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
