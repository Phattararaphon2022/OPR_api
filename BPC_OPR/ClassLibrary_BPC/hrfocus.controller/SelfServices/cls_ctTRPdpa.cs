using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRPdpa
    {
                 string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRPdpa() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRPdpa> getData(string condition)
        {
            List<cls_TRPdpa> list_model = new List<cls_TRPdpa>();
            cls_TRPdpa model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("SELF_TR_PDPA.COMPANY_CODE");
                obj_str.Append(", SELF_TR_PDPA.WORKER_CODE");
                obj_str.Append(", INITIAL_NAME_TH + WORKER_FNAME_TH + ' ' + WORKER_LNAME_TH AS WORKER_DETAIL_TH");
                obj_str.Append(", INITIAL_NAME_EN + WORKER_FNAME_EN + ' ' + WORKER_LNAME_EN AS WORKER_DETAIL_EN");
                obj_str.Append(", STATUS");
                obj_str.Append(", ISNULL(SELF_TR_PDPA.MODIFIED_BY, SELF_TR_PDPA.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(SELF_TR_PDPA.MODIFIED_DATE, SELF_TR_PDPA.CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM SELF_TR_PDPA");
                obj_str.Append(" INNER JOIN EMP_MT_WORKER ON EMP_MT_WORKER.COMPANY_CODE=SELF_TR_PDPA.COMPANY_CODE ");
                obj_str.Append(" AND EMP_MT_WORKER.WORKER_CODE=SELF_TR_PDPA.WORKER_CODE ");
                obj_str.Append(" INNER JOIN EMP_MT_INITIAL ON EMP_MT_INITIAL.INITIAL_CODE=EMP_MT_WORKER.WORKER_INITIAL ");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRPdpa();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.worker_detail_en = dr["WORKER_DETAIL_EN"].ToString();
                    model.worker_detail_th = dr["WORKER_DETAIL_TH"].ToString();
                    model.status = Convert.ToBoolean(dr["STATUS"]);
                    model.created_by = dr["MODIFIED_BY"].ToString();
                    model.created_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRTRAccountpos.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRPdpa> getDataByFillter(string com,string worker,bool status)
        {
            string strCondition = "";
            if(!com.Equals(""))
                strCondition += " AND SELF_TR_PDPA.COMPANY_CODE='" + com + "'";

            if (!worker.Equals(""))
                strCondition += " AND SELF_TR_PDPA.WORKER_CODE='" + worker + "'";

            //strCondition += " AND SELF_TR_PDPA.STATUS='" + status + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com,string worker)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT WORKER_CODE");
                obj_str.Append(" FROM SELF_TR_PDPA");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
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
        public bool delete(string com, string worker)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SELF_TR_PDPA");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
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

        public string insert(cls_TRPdpa model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code,model.worker_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("INSERT INTO SELF_TR_PDPA");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", STATUS ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @STATUS ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@STATUS", SqlDbType.Bit); obj_cmd.Parameters["@STATUS"].Value = model.status;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.created_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = model.worker_code;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRAccountpos.insert)" + ex.ToString();
            }

            return blnResult;
        }
        public string update(cls_TRPdpa model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SELF_TR_PDPA SET ");
                obj_str.Append(" COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(", STATUS=@STATUS ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
      

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());


                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@STATUS", SqlDbType.Bit); obj_cmd.Parameters["@STATUS"].Value = model.status;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.created_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.worker_code;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRAccountpos.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
