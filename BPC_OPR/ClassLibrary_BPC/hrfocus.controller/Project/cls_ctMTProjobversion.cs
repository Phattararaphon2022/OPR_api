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
    public class cls_ctMTProjobversion
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTProjobversion() { }

        public string getMessage() { return this.Message.Replace("PRO_MT_PROJOBVERSION", "").Replace("cls_ctMTProjobversion", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTProjobversion> getData(string condition)
        {
            List<cls_MTProjobversion> list_model = new List<cls_MTProjobversion>();
            cls_MTProjobversion model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PROJOBVERSION_ID");
                obj_str.Append(", TRANSACTION_ID");
                obj_str.Append(", VERSION");
                obj_str.Append(", FROMDATE");
                obj_str.Append(", TODATE");

                obj_str.Append(", ISNULL(TRANSACTION_DATA, '') AS TRANSACTION_DATA");
                obj_str.Append(", ISNULL(TRANSACTION_OLD, '') AS TRANSACTION_OLD");

                obj_str.Append(", ISNULL(REFSO, '') AS REFSO");
                obj_str.Append(", ISNULL(CUSTNO, '') AS CUSTNO");
                obj_str.Append(", ISNULL(REFAPPCOSTID, '') AS REFAPPCOSTID");
                obj_str.Append(", ISNULL(CURRENCY, '') AS CURRENCY");
                
                obj_str.Append(", PROJECT_CODE");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");    

                obj_str.Append(" FROM PRO_MT_PROJOBVERSION");
                obj_str.Append(" WHERE 1=1");
                
                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY CREATED_DATE DESC");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTProjobversion();

                    model.projobversion_id = Convert.ToInt32(dr["PROJOBVERSION_ID"]);
                    model.transaction_id = dr["TRANSACTION_ID"].ToString();
                    model.version = dr["VERSION"].ToString();

                    model.fromdate = Convert.ToDateTime(dr["FROMDATE"]);
                    model.todate = Convert.ToDateTime(dr["TODATE"]);

                    model.transaction_data = dr["TRANSACTION_DATA"].ToString();
                    model.transaction_old = dr["TRANSACTION_OLD"].ToString();

                    model.refso = dr["REFSO"].ToString();
                    model.custno = dr["CUSTNO"].ToString();
                    model.refappcostid = dr["REFAPPCOSTID"].ToString();
                    model.currency = dr["CURRENCY"].ToString();


                    model.project_code = dr["PROJECT_CODE"].ToString();

                    model.version = dr["VERSION"].ToString(); 

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                                                                                            
                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "BNK001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTProjobversion> getDataByFillter(string project)
        {
            string strCondition = "";

            if (!project.Equals(""))
                strCondition += " AND PROJECT_CODE='" + project + "'";

            
            return this.getData(strCondition);
        }
        public cls_MTProjobversion getDataCurrents(string project, DateTime fromdate, DateTime todate)
        {
            string strCondition = " AND PROJECT_CODE='" + project + "'";
            strCondition += " AND ('" + fromdate.ToString("MM/dd/yyyy") + "' BETWEEN FROMDATE AND TODATE)";
            strCondition += " AND ('" + todate.ToString("MM/dd/yyyy") + "' BETWEEN FROMDATE AND TODATE)";

            List<cls_MTProjobversion> list_model = this.getData(strCondition);

            if (list_model.Count > 0)
                return list_model[0];
            else
                return null;
        }


        public cls_MTProjobversion getDataCurrent(string project, DateTime date)
        {
            string strCondition = " AND PROJECT_CODE='" + project + "'";

            strCondition += " AND ( '" + date.ToString("MM/dd/yyyy") + "' BETWEEN FROMDATE AND TODATE)";

            List<cls_MTProjobversion> list_model = this.getData(strCondition);

            if (list_model.Count > 0)
                return list_model[0];
            else
                return null;
        }

        public cls_MTProjobversion getDataCurrentwithdraw(string project, DateTime date, DateTime todate)
        {
            string strCondition = " AND PROJECT_CODE='" + project + "'";

            strCondition += " AND ( '" + date.ToString("MM/dd/yyyy") + "' BETWEEN '" + date.ToString("MM/dd/yyyy") + "' AND '" + todate.ToString("MM/dd/yyyy") + "')";

            List<cls_MTProjobversion> list_model = this.getData(strCondition);

            if (list_model.Count > 0)
                return list_model[0];
            else
                return null;
        }


        public cls_MTProjobversion getDataTransaction(string id)
        {
            string strCondition = " AND TRANSACTION_ID='" + id + "'";

            List<cls_MTProjobversion> list_model = this.getData(strCondition);

            if (list_model.Count > 0)
                return list_model[0];
            else
                return null;
        }

        public string getLastVersion(string project)
        {
            string result = "";
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(VERSION, 1) ");
                obj_str.Append(" FROM PRO_MT_PROJOBVERSION");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" ORDER BY FROMDATE DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    result = dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                Message = "BNK002:" + ex.ToString();
            }

            return result;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(PROJOBVERSION_ID, 1) ");
                obj_str.Append(" FROM PRO_MT_PROJOBVERSION");
                obj_str.Append(" ORDER BY PROJOBVERSION_ID DESC ");

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

        public bool checkDataOld(string transaction_id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT TRANSACTION_ID");
                obj_str.Append(" FROM PRO_MT_PROJOBVERSION");
                obj_str.Append(" WHERE TRANSACTION_ID='" + transaction_id + "'");
               
      
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "BNK003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool checkDataOld(string project, string version)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT TRANSACTION_ID");
                obj_str.Append(" FROM PRO_MT_PROJOBVERSION");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND VERSION='" + version + "'");


                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "BNK003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string transaction_id)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_MT_PROJOBVERSION");
                obj_str.Append(" WHERE TRANSACTION_ID='" + transaction_id + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BNK004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string project, string version)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_MT_PROJOBVERSION");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND VERSION='" + version + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BNK004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool clear(string project)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_MT_PROJOBVERSION");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BNK004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_MTProjobversion model)
        {
            bool blnResult = false;
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.project_code, model.version))
                {
                    return this.update(model);               
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                          
                obj_str.Append("INSERT INTO PRO_MT_PROJOBVERSION");
                obj_str.Append(" (");
                obj_str.Append("PROJOBVERSION_ID ");
                obj_str.Append(", TRANSACTION_ID ");
                obj_str.Append(", VERSION ");
                obj_str.Append(", FROMDATE ");
                obj_str.Append(", TODATE ");


                obj_str.Append(", TRANSACTION_DATA ");
                obj_str.Append(", TRANSACTION_OLD ");

                obj_str.Append(", REFSO ");
                obj_str.Append(", CUSTNO ");
                obj_str.Append(", REFAPPCOSTID ");
                obj_str.Append(", CURRENCY ");               

                obj_str.Append(", PROJECT_CODE ");    

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");          
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@PROJOBVERSION_ID ");
                obj_str.Append(", @TRANSACTION_ID ");
                obj_str.Append(", @VERSION ");
                obj_str.Append(", @FROMDATE ");
                obj_str.Append(", @TODATE ");


                obj_str.Append(", @TRANSACTION_DATA ");
                obj_str.Append(", @TRANSACTION_OLD ");

                obj_str.Append(", @REFSO ");
                obj_str.Append(", @CUSTNO ");
                obj_str.Append(", @REFAPPCOSTID ");
                obj_str.Append(", @CURRENCY ");

                obj_str.Append(", @PROJECT_CODE ");    

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");
                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.projobversion_id = this.getNextID();

                obj_cmd.Parameters.Add("@PROJOBVERSION_ID", SqlDbType.Int); obj_cmd.Parameters["@PROJOBVERSION_ID"].Value = model.projobversion_id;
                obj_cmd.Parameters.Add("@TRANSACTION_ID", SqlDbType.VarChar); obj_cmd.Parameters["@TRANSACTION_ID"].Value = model.transaction_id;
                obj_cmd.Parameters.Add("@VERSION", SqlDbType.VarChar); obj_cmd.Parameters["@VERSION"].Value = model.version;
                obj_cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime); obj_cmd.Parameters["@FROMDATE"].Value = model.fromdate;
                obj_cmd.Parameters.Add("@TODATE", SqlDbType.DateTime); obj_cmd.Parameters["@TODATE"].Value = model.todate;

                obj_cmd.Parameters.Add("@TRANSACTION_DATA", SqlDbType.VarChar); obj_cmd.Parameters["@TRANSACTION_DATA"].Value = model.transaction_data;
                obj_cmd.Parameters.Add("@TRANSACTION_OLD", SqlDbType.VarChar); obj_cmd.Parameters["@TRANSACTION_OLD"].Value = model.transaction_old;
                
                obj_cmd.Parameters.Add("@REFSO", SqlDbType.VarChar); obj_cmd.Parameters["@REFSO"].Value = model.refso;
                obj_cmd.Parameters.Add("@CUSTNO", SqlDbType.VarChar); obj_cmd.Parameters["@CUSTNO"].Value = model.custno;
                obj_cmd.Parameters.Add("@REFAPPCOSTID", SqlDbType.VarChar); obj_cmd.Parameters["@REFAPPCOSTID"].Value = model.refappcostid;
                obj_cmd.Parameters.Add("@CURRENCY", SqlDbType.VarChar); obj_cmd.Parameters["@CURRENCY"].Value = model.currency;                               
                                
                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                                     
                obj_cmd.ExecuteNonQuery();
                                
                obj_conn.doClose();
                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "BNK005:" + ex.ToString();                
            }

            return blnResult;
        }

        public bool update(cls_MTProjobversion model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PRO_MT_PROJOBVERSION SET ");
                obj_str.Append(" FROMDATE=@FROMDATE ");
                obj_str.Append(", TODATE=@TODATE ");
              
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE TRANSACTION_ID=@TRANSACTION_ID ");            

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime); obj_cmd.Parameters["@FROMDATE"].Value = model.fromdate;
                obj_cmd.Parameters.Add("@TODATE", SqlDbType.DateTime); obj_cmd.Parameters["@TODATE"].Value = model.todate;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@TRANSACTION_ID", SqlDbType.Int); obj_cmd.Parameters["@TRANSACTION_ID"].Value = model.transaction_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "BNK006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}
