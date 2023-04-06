using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;
namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRLineapprove
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRLineapprove() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRLineapprove> getData(string condition)
        {
            List<cls_TRLineapprove> list_model = new List<cls_TRLineapprove>();
            cls_TRLineapprove model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");
                obj_str.Append(", LINEAPPROVE_ID");
                obj_str.Append(", LINEAPPROVE_LEAVE");
                obj_str.Append(", LINEAPPROVE_OT");
                obj_str.Append(", LINEAPPROVE_SHIFT");
                obj_str.Append(", LINEAPPROVE_PUNCHCARD");
                obj_str.Append(", LINEAPPROVE_CHECKIN");
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");
                obj_str.Append(", FLAG ");
                obj_str.Append(" FROM SELF_TR_LINEAPPROVE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, LINEAPPROVE_ID");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRLineapprove();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);

                    model.worker_code = Convert.ToString(dr["WORKER_CODE"]);
                    model.lineapprove_id = Convert.ToInt32(dr["LINEAPPROVE_ID"]);
                    model.lineapprove_leave = Convert.ToString(dr["LINEAPPROVE_LEAVE"]);
                    model.lineapprove_ot = Convert.ToString(dr["LINEAPPROVE_OT"]);
                    model.lineapprove_shift = Convert.ToString(dr["LINEAPPROVE_SHIFT"]);
                    model.lineapprove_punchcard = Convert.ToString(dr["LINEAPPROVE_PUNCHCARD"]);
                    model.lineapprove_checkin = Convert.ToString(dr["LINEAPPROVE_CHECKIN"]);
                    model.modified_by = Convert.ToString(dr["MODIFIED_BY"]);
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                    model.flag = Convert.ToBoolean(dr["FLAG"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Lineapprove.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRLineapprove> getDataByFillter(string com, string lineid, string worker_code, string line_leave, string line_ot, string line_shift, string line_punchcard, string line_checkin)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!lineid.Equals(""))
                strCondition += " AND LINEAPPROVE_ID='" + lineid + "'";
            if (!worker_code.Equals(""))
                strCondition += " AND WORKER_CODE='" + worker_code + "'";
            if (!line_leave.Equals(""))
                strCondition += " AND LINEAPPROVE_LEAVE='" + line_leave + "'";
            if (!line_ot.Equals(""))
                strCondition += " AND LINEAPPROVE_OT='" + line_ot + "'";
            if (!line_shift.Equals(""))
                strCondition += " AND LINEAPPROVE_SHIFT='" + line_shift + "'";
            if (!line_punchcard.Equals(""))
                strCondition += " AND LINEAPPROVE_PUNCHCARD='" + line_punchcard + "'";
            if (!line_checkin.Equals(""))
                strCondition += " AND LINEAPPROVE_CHECKIN='" + line_checkin + "'";
            return this.getData(strCondition);
        }

        public bool checkDataOld(string com,string worker_code )
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM SELF_TR_LINEAPPROVE");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + worker_code + "' ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Lineapprove.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string lineid,string worker_code)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SELF_TR_LINEAPPROVE");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                if (!lineid.Equals(""))
                    obj_str.Append(" AND LINEAPPROVE_ID='" + lineid + "' ");
                if (!worker_code.Equals(""))
                    obj_str.Append(" AND WORKER_CODE='" + worker_code + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());


            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Lineapprove.delete)" + ex.ToString();
            }

            return blnResult;
        }
        public string insert(cls_TRLineapprove model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.worker_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO SELF_TR_LINEAPPROVE");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", LINEAPPROVE_ID ");
                obj_str.Append(", LINEAPPROVE_LEAVE ");
                obj_str.Append(", LINEAPPROVE_OT ");
                obj_str.Append(", LINEAPPROVE_SHIFT ");
                obj_str.Append(", LINEAPPROVE_PUNCHCARD ");
                obj_str.Append(", LINEAPPROVE_CHECKIN ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @LINEAPPROVE_ID ");
                obj_str.Append(", @LINEAPPROVE_LEAVE ");
                obj_str.Append(", @LINEAPPROVE_OT ");
                obj_str.Append(", @LINEAPPROVE_SHIFT ");
                obj_str.Append(", @LINEAPPROVE_PUNCHCARD ");
                obj_str.Append(", @LINEAPPROVE_CHECKIN ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@LINEAPPROVE_ID", SqlDbType.Int); obj_cmd.Parameters["@LINEAPPROVE_ID"].Value = id;
                obj_cmd.Parameters.Add("@LINEAPPROVE_LEAVE", SqlDbType.VarChar); obj_cmd.Parameters["@LINEAPPROVE_LEAVE"].Value = model.lineapprove_leave;
                obj_cmd.Parameters.Add("@LINEAPPROVE_OT", SqlDbType.VarChar); obj_cmd.Parameters["@LINEAPPROVE_OT"].Value = model.lineapprove_ot;
                obj_cmd.Parameters.Add("@LINEAPPROVE_SHIFT", SqlDbType.VarChar); obj_cmd.Parameters["@LINEAPPROVE_SHIFT"].Value = model.lineapprove_shift;
                obj_cmd.Parameters.Add("@LINEAPPROVE_PUNCHCARD", SqlDbType.VarChar); obj_cmd.Parameters["@LINEAPPROVE_PUNCHCARD"].Value = model.lineapprove_punchcard;
                obj_cmd.Parameters.Add("@LINEAPPROVE_CHECKIN", SqlDbType.VarChar); obj_cmd.Parameters["@LINEAPPROVE_CHECKIN"].Value = model.lineapprove_checkin;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;


                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Lineapprove.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(LINEAPPROVE_ID) ");
                obj_str.Append(" FROM SELF_TR_LINEAPPROVE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Lineapprove.getNextID)" + ex.ToString();
            }

            return intResult;
        }
        public bool insert(List<cls_TRLineapprove> list_model,string username)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SELF_TR_LINEAPPROVE");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", LINEAPPROVE_ID ");
                obj_str.Append(", LINEAPPROVE_LEAVE ");
                obj_str.Append(", LINEAPPROVE_OT ");
                obj_str.Append(", LINEAPPROVE_SHIFT ");
                obj_str.Append(", LINEAPPROVE_PUNCHCARD ");
                obj_str.Append(", LINEAPPROVE_CHECKIN ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @LINEAPPROVE_ID ");
                obj_str.Append(", @LINEAPPROVE_LEAVE ");
                obj_str.Append(", @LINEAPPROVE_OT ");
                obj_str.Append(", @LINEAPPROVE_SHIFT ");
                obj_str.Append(", @LINEAPPROVE_PUNCHCARD ");
                obj_str.Append(", @LINEAPPROVE_CHECKIN ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");


                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                //-- Step 1 delete data old
                string strWorkerID = "";
                foreach (cls_TRLineapprove model in list_model)
                {
                    strWorkerID += "'" + model.worker_code + "',";
                }
                if (strWorkerID.Length > 0)
                    strWorkerID = strWorkerID.Substring(0, strWorkerID.Length - 1);

                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM SELF_TR_LINEAPPROVE");
                obj_str2.Append(" WHERE 1=1 ");
                obj_str2.Append(" AND COMPANY_CODE='" + list_model[0].company_code + "'");
                obj_str2.Append(" AND WORKER_CODE IN (" + strWorkerID + ")");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());
                int id = 1;
                try
                {
                    System.Text.StringBuilder obj_str3 = new System.Text.StringBuilder();

                    obj_str3.Append("SELECT MAX(LINEAPPROVE_ID) ");
                    obj_str3.Append(" FROM SELF_TR_LINEAPPROVE");

                    DataTable dt = obj_conn.doGetTableTransaction(obj_str3.ToString());
                    if (dt.Rows.Count > 0)
                    {
                        id = Convert.ToInt32(dt.Rows[0][0]) + 1;
                    }
                }
                catch
                {

                }
                if (blnResult)
                {
                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@LINEAPPROVE_ID", SqlDbType.Int);
                    obj_cmd.Parameters.Add("@LINEAPPROVE_LEAVE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@LINEAPPROVE_OT", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@LINEAPPROVE_SHIFT", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@LINEAPPROVE_PUNCHCARD", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@LINEAPPROVE_CHECKIN", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime);
                    obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit);
                    foreach (cls_TRLineapprove model in list_model)
                    {

                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                        obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                        obj_cmd.Parameters["@LINEAPPROVE_ID"].Value = id;
                        obj_cmd.Parameters["@LINEAPPROVE_LEAVE"].Value = model.lineapprove_leave;
                        obj_cmd.Parameters["@LINEAPPROVE_OT"].Value = model.lineapprove_ot;
                        obj_cmd.Parameters["@LINEAPPROVE_SHIFT"].Value = model.lineapprove_shift;
                        obj_cmd.Parameters["@LINEAPPROVE_PUNCHCARD"].Value = model.lineapprove_punchcard;
                        obj_cmd.Parameters["@LINEAPPROVE_CHECKIN"].Value = model.lineapprove_checkin;
                        obj_cmd.Parameters["@CREATED_BY"].Value = username;
                        obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                        obj_cmd.Parameters["@FLAG"].Value = model.flag;
                        id++;
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
                Message = "ERROR::(Lineapprove.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_TRLineapprove model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SELF_TR_LINEAPPROVE SET ");

                obj_str.Append(" COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(", LINEAPPROVE_LEAVE=@LINEAPPROVE_LEAVE ");
                obj_str.Append(", LINEAPPROVE_OT=@LINEAPPROVE_OT ");
                obj_str.Append(", LINEAPPROVE_SHIFT=@LINEAPPROVE_SHIFT ");
                obj_str.Append(", LINEAPPROVE_PUNCHCARD=@LINEAPPROVE_PUNCHCARD ");
                obj_str.Append(", LINEAPPROVE_CHECKIN=@LINEAPPROVE_CHECKIN ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");


                obj_str.Append(" WHERE LINEAPPROVE_ID=@LINEAPPROVE_ID ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@LINEAPPROVE_ID", SqlDbType.Int); obj_cmd.Parameters["@LINEAPPROVE_ID"].Value = model.lineapprove_id;
                obj_cmd.Parameters.Add("@LINEAPPROVE_LEAVE", SqlDbType.VarChar); obj_cmd.Parameters["@LINEAPPROVE_LEAVE"].Value = model.lineapprove_leave;
                obj_cmd.Parameters.Add("@LINEAPPROVE_OT", SqlDbType.VarChar); obj_cmd.Parameters["@LINEAPPROVE_OT"].Value = model.lineapprove_ot;
                obj_cmd.Parameters.Add("@LINEAPPROVE_SHIFT", SqlDbType.VarChar); obj_cmd.Parameters["@LINEAPPROVE_SHIFT"].Value = model.lineapprove_shift;
                obj_cmd.Parameters.Add("@LINEAPPROVE_PUNCHCARD", SqlDbType.VarChar); obj_cmd.Parameters["@LINEAPPROVE_PUNCHCARD"].Value = model.lineapprove_punchcard;
                obj_cmd.Parameters.Add("@LINEAPPROVE_CHECKIN", SqlDbType.VarChar); obj_cmd.Parameters["@LINEAPPROVE_CHECKIN"].Value = model.lineapprove_checkin;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.lineapprove_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Lineapprove.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
