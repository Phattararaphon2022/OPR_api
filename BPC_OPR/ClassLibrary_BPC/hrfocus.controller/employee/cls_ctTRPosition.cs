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
    public class cls_ctTRPosition
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRPosition() { }

        public string getMessage() { return this.Message.Replace("EMP_TR_POSITION", "").Replace("cls_ctTRPosition", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }
        private List<cls_TRPosition> getData(string condition)
        {
            List<cls_TRPosition> list_model = new List<cls_TRPosition>();
            cls_TRPosition model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("EMP_TR_POSITION.COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");
                obj_str.Append(", EMPPOSITION_ID");
                obj_str.Append(", EMPPOSITION_DATE");
                obj_str.Append(", ISNULL(EMPPOSITION_POSITION, '') AS EMPPOSITION_POSITION");
                obj_str.Append(", ISNULL(EMPPOSITION_REASON, '') AS EMPPOSITION_REASON");
                //obj_str.Append(", ISNULL(EMP_MT_POSITION.POSITION_NAME_TH,'') AS POSITION_NAME_TH");
                //obj_str.Append(", ISNULL(EMP_MT_POSITION.POSITION_NAME_EN,'') AS POSITION_NAME_EN");


                obj_str.Append(", ISNULL(EMP_TR_POSITION.MODIFIED_BY, EMP_TR_POSITION.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(EMP_TR_POSITION.MODIFIED_DATE, EMP_TR_POSITION.CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_TR_POSITION");
                //obj_str.Append(" INNER JOIN EMP_MT_POSITION ON EMP_MT_POSITION.POSITION_CODE=EMP_TR_POSITION.EMPPOSITION_POSITION");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRPosition();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.empposition_id = Convert.ToInt32(dr["EMPPOSITION_ID"]);
                    model.empposition_date = Convert.ToDateTime(dr["EMPPOSITION_DATE"]);
                    model.empposition_position = dr["EMPPOSITION_POSITION"].ToString();
                    model.empposition_reason = dr["EMPPOSITION_REASON"].ToString();

                    //model.position_name_th = dr["POSITION_NAME_TH"].ToString();
                    //model.position_name_en = dr["POSITION_NAME_EN"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "EMPPST001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRPosition> getDataByFillter(string com, string emp,string position)
        {
            string strCondition = "";

            if (!com.Equals(""))
                strCondition += " AND EMP_TR_POSITION.COMPANY_CODE='" + com + "'";

            if (!emp.Equals(""))
                strCondition += " AND WORKER_CODE='" + emp + "'";

            if (!position.Equals(""))
                strCondition += " AND EMPPOSITION_POSITION='"+ position +"'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(EMPPOSITION_ID, 1) ");
                obj_str.Append(" FROM EMP_TR_POSITION");
                obj_str.Append(" ORDER BY EMPPOSITION_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPPST002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp, int id,string date)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT EMPPOSITION_ID");
                obj_str.Append(" FROM EMP_TR_POSITION");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                if (!id.Equals(0))
                {
                    obj_str.Append(" AND EMPPOSITION_ID='" + id + "' ");
                }
                if (!date.Equals(""))
                {
                    obj_str.Append(" AND EMPPOSITION_DATE='" + date + "' ");
                }

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPPST003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM EMP_TR_POSITION");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "EMPPST004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRPosition model)
        {
            bool blnResult = false;
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.company_code, model.worker_code, model.empposition_id, model.empposition_date.ToString("yyyy-MM-ddTHH:mm:ss")))
                {
                    
                        return this.update(model);
                    
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_TR_POSITION");
                obj_str.Append(" (");
                obj_str.Append("EMPPOSITION_ID ");
                obj_str.Append(", EMPPOSITION_DATE ");
                obj_str.Append(", EMPPOSITION_POSITION ");
                obj_str.Append(", EMPPOSITION_REASON ");
                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@EMPPOSITION_ID ");
                obj_str.Append(", @EMPPOSITION_DATE ");
                obj_str.Append(", @EMPPOSITION_POSITION ");
                obj_str.Append(", @EMPPOSITION_REASON ");
                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.empposition_id = this.getNextID();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@EMPPOSITION_ID", SqlDbType.Int); obj_cmd.Parameters["@EMPPOSITION_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@EMPPOSITION_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPPOSITION_DATE"].Value = model.empposition_date;
                obj_cmd.Parameters.Add("@EMPPOSITION_POSITION", SqlDbType.VarChar); obj_cmd.Parameters["@EMPPOSITION_POSITION"].Value = model.empposition_position;
                obj_cmd.Parameters.Add("@EMPPOSITION_REASON", SqlDbType.VarChar); obj_cmd.Parameters["@EMPPOSITION_REASON"].Value = model.empposition_reason;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                strResult = model.empposition_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "EMPPST005:" + ex.ToString();
                strResult = "";
            }

            return blnResult;
        }

        public bool update(cls_TRPosition model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE EMP_TR_POSITION SET ");

                //obj_str.Append(" EMPPOSITION_DATE=@EMPPOSITION_DATE ");
                obj_str.Append(" EMPPOSITION_POSITION=@EMPPOSITION_POSITION ");
                obj_str.Append(", EMPPOSITION_REASON=@EMPPOSITION_REASON ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE "); ;

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" AND EMPPOSITION_DATE=@EMPPOSITION_DATE ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@EMPPOSITION_POSITION", SqlDbType.VarChar); obj_cmd.Parameters["@EMPPOSITION_POSITION"].Value = model.empposition_position;
                obj_cmd.Parameters.Add("@EMPPOSITION_REASON", SqlDbType.VarChar); obj_cmd.Parameters["@EMPPOSITION_REASON"].Value = model.empposition_reason;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@EMPPOSITION_ID", SqlDbType.Int); obj_cmd.Parameters["@EMPPOSITION_ID"].Value = model.empposition_id;
                obj_cmd.Parameters.Add("@EMPPOSITION_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@EMPPOSITION_DATE"].Value = model.empposition_date;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "EMPPST006:" + ex.ToString();
            }

            return blnResult;
        }

        public List<cls_TRPosition> getDataBatch(string com, string code, DateTime date)
        {
            List<cls_TRPosition> list_model = new List<cls_TRPosition>();
            cls_TRPosition model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("EMP_TR_POSITION.COMPANY_CODE");
                obj_str.Append(", EMP_TR_POSITION.WORKER_CODE");
                obj_str.Append(", EMP_TR_POSITION.EMPPOSITION_ID");
                obj_str.Append(", EMP_TR_POSITION.EMPPOSITION_DATE");
                obj_str.Append(", ISNULL(EMP_TR_POSITION.EMPPOSITION_POSITION, '') AS EMPPOSITION_POSITION");

                obj_str.Append(", INITIAL_NAME_TH + WORKER_FNAME_TH + ' ' + WORKER_LNAME_TH AS WORKER_DETAIL_TH");
                obj_str.Append(", INITIAL_NAME_EN + WORKER_FNAME_EN + ' ' + WORKER_LNAME_EN AS WORKER_DETAIL_EN");

                obj_str.Append(", ISNULL(EMP_TR_POSITION.MODIFIED_BY, EMP_TR_POSITION.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(EMP_TR_POSITION.MODIFIED_DATE, EMP_TR_POSITION.CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_TR_POSITION");
                obj_str.Append(" INNER JOIN EMP_MT_WORKER ON EMP_MT_WORKER.COMPANY_CODE=EMP_TR_POSITION.COMPANY_CODE AND EMP_MT_WORKER.WORKER_CODE=EMP_TR_POSITION.WORKER_CODE");
                obj_str.Append(" INNER JOIN EMP_MT_INITIAL ON EMP_MT_INITIAL.INITIAL_CODE=EMP_MT_WORKER.WORKER_INITIAL ");
                obj_str.Append(" WHERE 1=1");
                obj_str.Append(" AND EMP_TR_POSITION.COMPANY_CODE='" + com + "' ");

                if (!code.Equals(""))
                    obj_str.Append(" AND EMP_TR_POSITION.EMPPOSITION_POSITION='" + code + "' ");
                if (!date.Equals(""))
                    obj_str.Append(" AND EMP_TR_POSITION.EMPPOSITION_DATE='" + date.ToString("yyyy-MM-ddTHH:mm:ss") + "' ");

                obj_str.Append(" ORDER BY EMP_TR_POSITION.WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRPosition();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.empposition_id = Convert.ToInt32(dr["EMPPOSITION_ID"]);
                    model.empposition_date = Convert.ToDateTime(dr["EMPPOSITION_DATE"]);
                    model.empposition_position = dr["EMPPOSITION_POSITION"].ToString();

                    model.worker_detail_th = dr["WORKER_DETAIL_TH"].ToString();
                    model.worker_detail_en = dr["WORKER_DETAIL_EN"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "EMPPST007:" + ex.ToString();
            }

            return list_model;
        }

        public bool insertlist(List<cls_TRPosition> list_model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_TR_POSITION");
                obj_str.Append(" (");
                obj_str.Append("EMPPOSITION_ID ");
                obj_str.Append(", EMPPOSITION_DATE ");
                obj_str.Append(", EMPPOSITION_POSITION ");
                obj_str.Append(", EMPPOSITION_REASON ");
                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@EMPPOSITION_ID ");
                obj_str.Append(", @EMPPOSITION_DATE ");
                obj_str.Append(", @EMPPOSITION_POSITION ");
                obj_str.Append(", @EMPPOSITION_REASON ");
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
                foreach (cls_TRPosition model in list_model)
                {
                    strWorkerID += "'" + model.worker_code + "',";
                }
                if (strWorkerID.Length > 0)
                    strWorkerID = strWorkerID.Substring(0, strWorkerID.Length - 1);
                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM EMP_TR_POSITION");
                obj_str2.Append(" WHERE 1=1 ");
                obj_str2.Append(" AND COMPANY_CODE='" + list_model[0].company_code + "'");
                obj_str2.Append(" AND WORKER_CODE IN (" + strWorkerID + ")");
                obj_str2.Append(" AND EMPPOSITION_DATE='" + list_model[0].empposition_date.ToString("yyyy-MM-ddTHH:mm:ss") + "'");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());

                if(blnResult)
                {
                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPPOSITION_ID", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPPOSITION_DATE", SqlDbType.DateTime);
                    obj_cmd.Parameters.Add("@EMPPOSITION_POSITION", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPPOSITION_REASON", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime);

                    foreach (cls_TRPosition model in list_model)
                    {
                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                        obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                        obj_cmd.Parameters["@EMPPOSITION_ID"].Value = this.getNextID();
                        obj_cmd.Parameters["@EMPPOSITION_DATE"].Value = model.empposition_date;
                        obj_cmd.Parameters["@EMPPOSITION_POSITION"].Value = model.empposition_position;
                        obj_cmd.Parameters["@EMPPOSITION_REASON"].Value = model.empposition_reason;
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
                Message = "EMPPST099:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
