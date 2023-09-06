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
    public class cls_ctMTBlacklist
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTBlacklist() { }

        public string getMessage() { return this.Message.Replace("REQ_MT_BLACKLIST", "").Replace("cls_ctMTBlacklist", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTBlacklist> getData(string condition)
        {
            List<cls_MTBlacklist> list_model = new List<cls_MTBlacklist>();
            cls_MTBlacklist model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("REQ_MT_BLACKLIST.COMPANY_CODE");
                obj_str.Append(", REQ_MT_BLACKLIST.BLACKLIST_ID");

                obj_str.Append(", ISNULL(REQ_MT_BLACKLIST.WORKER_CODE,'') AS WORKER_CODE");

                obj_str.Append(", REQ_MT_BLACKLIST.CARD_NO");

                obj_str.Append(", REQ_MT_BLACKLIST.BLACKLIST_FNAME_TH");
                obj_str.Append(", REQ_MT_BLACKLIST.BLACKLIST_LNAME_TH");
                obj_str.Append(", REQ_MT_BLACKLIST.BLACKLIST_FNAME_EN");
                obj_str.Append(", REQ_MT_BLACKLIST.BLACKLIST_LNAME_EN");


                obj_str.Append(", REQ_MT_BLACKLIST.REASON_CODE");
                obj_str.Append(", REQ_MT_BLACKLIST.BLACKLIST_NOTE");

                //obj_str.Append(", INITIAL_NAME_TH + WORKER_FNAME_TH + ' ' + WORKER_LNAME_TH AS WORKER_DETAIL_TH");
                //obj_str.Append(", INITIAL_NAME_EN + WORKER_FNAME_EN + ' ' + WORKER_LNAME_EN AS WORKER_DETAIL_EN");


                obj_str.Append(", ISNULL(REQ_MT_BLACKLIST.MODIFIED_BY, REQ_MT_BLACKLIST.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(REQ_MT_BLACKLIST.MODIFIED_DATE, REQ_MT_BLACKLIST.CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM REQ_MT_BLACKLIST");
                //obj_str.Append(" INNER JOIN EMP_MT_WORKER ON EMP_MT_WORKER.COMPANY_CODE=REQ_MT_BLACKLIST.COMPANY_CODE AND EMP_MT_WORKER.WORKER_CODE=REQ_MT_BLACKLIST.WORKER_CODE");
                //obj_str.Append(" INNER JOIN EMP_MT_INITIAL ON EMP_MT_INITIAL.INITIAL_CODE=EMP_MT_WORKER.WORKER_INITIAL ");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY REQ_MT_BLACKLIST.COMPANY_CODE, REQ_MT_BLACKLIST.WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTBlacklist();
                    
                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.blacklist_id = Convert.ToInt32(dr["BLACKLIST_ID"]);
                    model.worker_code = dr["WORKER_CODE"].ToString();

                    model.card_no = dr["CARD_NO"].ToString();

                    model.blacklist_fname_th = dr["BLACKLIST_FNAME_TH"].ToString();
                    model.blacklist_lname_th = dr["BLACKLIST_LNAME_TH"].ToString();
                    model.blacklist_fname_en = dr["BLACKLIST_FNAME_EN"].ToString();
                    model.blacklist_lname_en = dr["BLACKLIST_LNAME_EN"].ToString();

                    model.reason_code = dr["REASON_CODE"].ToString();
                    model.blacklist_note = dr["BLACKLIST_NOTE"].ToString();

                    //model.worker_detail_th = dr["WORKER_DETAIL_TH"].ToString();
                    //model.worker_detail_en = dr["WORKER_DETAIL_EN"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "BLK001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTBlacklist> getDataByFillter(string com, string emp,string card)
        {
            string strCondition = "";

            if (!com.Equals(""))
                strCondition += " AND REQ_MT_BLACKLIST.COMPANY_CODE='" + com + "'";

            if (!emp.Equals(""))
                strCondition += " AND REQ_MT_BLACKLIST.WORKER_CODE='" + emp + "'";
            if (!card.Equals(""))
                strCondition += " AND REQ_MT_BLACKLIST.CARD_NO='" + card + "'";


            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string card)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT WORKER_CODE");
                obj_str.Append(" FROM REQ_MT_BLACKLIST");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND CARD_NO='" + card + "' ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "BLK002:" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(BLACKLIST_ID, 1) ");
                obj_str.Append(" FROM REQ_MT_BLACKLIST");
                obj_str.Append(" ORDER BY BLACKLIST_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "REQST002:" + ex.ToString();
            }

            return intResult;
        }

        public bool delete(string com, string card,string emp)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM REQ_MT_BLACKLIST");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND CARD_NO='" + card + "' ");
                if (!emp.Equals(""))
                {
                    obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                }

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BLK003:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTBlacklist model)
        {
            
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.company_code, model.card_no))
                {
                    if (this.update(model))
                        return model.card_no;
                    else
                        return "";
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO REQ_MT_BLACKLIST");
                obj_str.Append(" (");
                
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", BLACKLIST_ID ");
                if (!model.worker_code.Equals(""))
                {
                    obj_str.Append(", WORKER_CODE ");

                }
                obj_str.Append(", CARD_NO ");

                obj_str.Append(", BLACKLIST_FNAME_TH ");
                obj_str.Append(", BLACKLIST_LNAME_TH ");
                obj_str.Append(", BLACKLIST_FNAME_EN ");
                obj_str.Append(", BLACKLIST_LNAME_EN ");

                obj_str.Append(", REASON_CODE");
                obj_str.Append(", BLACKLIST_NOTE");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @BLACKLIST_ID ");
                if (!model.worker_code.Equals(""))
                {
                    obj_str.Append(", @WORKER_CODE ");
                }
                obj_str.Append(", @CARD_NO ");
                obj_str.Append(", @BLACKLIST_FNAME_TH ");
                obj_str.Append(", @BLACKLIST_LNAME_TH ");
                obj_str.Append(", @BLACKLIST_FNAME_EN ");
                obj_str.Append(", @BLACKLIST_LNAME_EN ");
                obj_str.Append(", @REASON_CODE");
                obj_str.Append(", @BLACKLIST_NOTE");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@BLACKLIST_ID", SqlDbType.Int); obj_cmd.Parameters["@BLACKLIST_ID"].Value = this.getNextID();
                if (!model.worker_code.Equals(""))
                {
                    obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                }

                obj_cmd.Parameters.Add("@CARD_NO", SqlDbType.VarChar); obj_cmd.Parameters["@CARD_NO"].Value = model.card_no;

                obj_cmd.Parameters.Add("@BLACKLIST_FNAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@BLACKLIST_FNAME_TH"].Value = model.blacklist_fname_th;
                obj_cmd.Parameters.Add("@BLACKLIST_LNAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@BLACKLIST_LNAME_TH"].Value = model.blacklist_lname_th;
                obj_cmd.Parameters.Add("@BLACKLIST_FNAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@BLACKLIST_FNAME_EN"].Value = model.blacklist_fname_en;
                obj_cmd.Parameters.Add("@BLACKLIST_LNAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@BLACKLIST_LNAME_EN"].Value = model.blacklist_lname_en;

                obj_cmd.Parameters.Add("@REASON_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@REASON_CODE"].Value = model.reason_code;
                obj_cmd.Parameters.Add("@BLACKLIST_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@BLACKLIST_NOTE"].Value = model.blacklist_note;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                strResult = model.card_no.ToString();
            }
            catch (Exception ex)
            {
                Message = "BLK004:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTBlacklist model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE REQ_MT_BLACKLIST SET ");

                obj_str.Append("BLACKLIST_FNAME_TH=@BLACKLIST_FNAME_TH ");
                obj_str.Append(", BLACKLIST_LNAME_TH=@BLACKLIST_LNAME_TH ");
                obj_str.Append(", BLACKLIST_FNAME_EN=@BLACKLIST_FNAME_EN ");
                obj_str.Append(", BLACKLIST_LNAME_EN=@BLACKLIST_LNAME_EN ");

                obj_str.Append(", REASON_CODE=@REASON_CODE ");
                obj_str.Append(", BLACKLIST_NOTE=@BLACKLIST_NOTE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND CARD_NO=@CARD_NO ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@BLACKLIST_FNAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@BLACKLIST_FNAME_TH"].Value = model.blacklist_fname_th;
                obj_cmd.Parameters.Add("@BLACKLIST_LNAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@BLACKLIST_LNAME_TH"].Value = model.blacklist_lname_th;
                obj_cmd.Parameters.Add("@BLACKLIST_FNAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@BLACKLIST_FNAME_EN"].Value = model.blacklist_fname_en;
                obj_cmd.Parameters.Add("@BLACKLIST_LNAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@BLACKLIST_LNAME_EN"].Value = model.blacklist_lname_en;
                obj_cmd.Parameters.Add("@REASON_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@REASON_CODE"].Value = model.reason_code;
                obj_cmd.Parameters.Add("@BLACKLIST_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@BLACKLIST_NOTE"].Value = model.blacklist_note;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@CARD_NO", SqlDbType.VarChar); obj_cmd.Parameters["@CARD_NO"].Value = model.card_no;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "BLK005:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insertlist(List<cls_MTBlacklist> list_model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO REQ_MT_BLACKLIST");
                obj_str.Append(" (");

                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", BLACKLIST_ID ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", CARD_NO ");
                obj_str.Append(", BLACKLIST_FNAME_TH ");
                obj_str.Append(", BLACKLIST_LNAME_TH ");
                obj_str.Append(", BLACKLIST_FNAME_EN ");
                obj_str.Append(", BLACKLIST_LNAME_EN ");
                obj_str.Append(", REASON_CODE");
                obj_str.Append(", BLACKLIST_NOTE");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @BLACKLIST_ID ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @CARD_NO ");
                obj_str.Append(", @BLACKLIST_FNAME_TH ");
                obj_str.Append(", @BLACKLIST_LNAME_TH ");
                obj_str.Append(", @BLACKLIST_FNAME_EN ");
                obj_str.Append(", @BLACKLIST_LNAME_EN ");
                obj_str.Append(", @REASON_CODE");
                obj_str.Append(", @BLACKLIST_NOTE");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                //-- Step 1 delete data old
                string strWorkerID = "";
                foreach (cls_MTBlacklist model in list_model)
                {
                    strWorkerID += "'" + model.worker_code + "',";
                }
                if (strWorkerID.Length > 0)
                    strWorkerID = strWorkerID.Substring(0, strWorkerID.Length - 1);
                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM REQ_MT_BLACKLIST");
                obj_str2.Append(" WHERE 1=1 ");
                obj_str2.Append(" AND COMPANY_CODE='" + list_model[0].company_code + "'");
                obj_str2.Append(" AND WORKER_CODE IN (" + strWorkerID + ")");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());

                if (blnResult)
                {
                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); 
                    obj_cmd.Parameters.Add("@BLACKLIST_ID", SqlDbType.Int); 
                    obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); 
                    obj_cmd.Parameters.Add("@CARD_NO", SqlDbType.VarChar); 
                    obj_cmd.Parameters.Add("@BLACKLIST_FNAME_TH", SqlDbType.VarChar); 
                    obj_cmd.Parameters.Add("@BLACKLIST_LNAME_TH", SqlDbType.VarChar); 
                    obj_cmd.Parameters.Add("@BLACKLIST_FNAME_EN", SqlDbType.VarChar); 
                    obj_cmd.Parameters.Add("@BLACKLIST_LNAME_EN", SqlDbType.VarChar); 
                    obj_cmd.Parameters.Add("@REASON_CODE", SqlDbType.VarChar); 
                    obj_cmd.Parameters.Add("@BLACKLIST_NOTE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); 
                    obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); 

                    foreach (cls_MTBlacklist model in list_model)
                    {

                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                        obj_cmd.Parameters["@BLACKLIST_ID"].Value = this.getNextID();
                        obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                        obj_cmd.Parameters["@CARD_NO"].Value = model.card_no;
                        obj_cmd.Parameters["@BLACKLIST_FNAME_TH"].Value = model.blacklist_fname_th;
                        obj_cmd.Parameters["@BLACKLIST_LNAME_TH"].Value = model.blacklist_lname_th;
                        obj_cmd.Parameters["@BLACKLIST_FNAME_EN"].Value = model.blacklist_fname_en;
                        obj_cmd.Parameters["@BLACKLIST_LNAME_EN"].Value = model.blacklist_lname_en;
                        obj_cmd.Parameters["@REASON_CODE"].Value = model.reason_code;
                        obj_cmd.Parameters["@BLACKLIST_NOTE"].Value = model.blacklist_note;
                        obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
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
                Message = "BLK006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
