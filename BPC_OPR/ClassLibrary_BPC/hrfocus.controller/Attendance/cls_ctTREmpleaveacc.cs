using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;


namespace ClassLibrary_BPC.hrfocus.controller
{

    public class cls_ctTREmpleaveacc
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTREmpleaveacc() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TREmpleaveacc> getData(string condition)
        {
            List<cls_TREmpleaveacc> list_model = new List<cls_TREmpleaveacc>();
            cls_TREmpleaveacc model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("ATT_TR_EMPLEAVEACC.COMPANY_CODE");
                obj_str.Append(", ATT_TR_EMPLEAVEACC.WORKER_CODE");
                obj_str.Append(", ATT_TR_EMPLEAVEACC.YEAR_CODE");

                obj_str.Append(", ATT_TR_EMPLEAVEACC.LEAVE_CODE");
                obj_str.Append(", EMPLEAVEACC_BF");
                obj_str.Append(", EMPLEAVEACC_ANNUAL");
                obj_str.Append(", EMPLEAVEACC_USED");
                obj_str.Append(", EMPLEAVEACC_REMAIN");

                obj_str.Append(", ISNULL(ATT_TR_EMPLEAVEACC.MODIFIED_BY, ATT_TR_EMPLEAVEACC.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(ATT_TR_EMPLEAVEACC.MODIFIED_DATE, ATT_TR_EMPLEAVEACC.CREATED_DATE) AS MODIFIED_DATE");
                obj_str.Append(", LEAVE_NAME_TH AS LEAVE_DETAIL_TH");
                obj_str.Append(", INITIAL_NAME_TH + WORKER_FNAME_TH + ' ' + WORKER_LNAME_TH AS WORKER_DETAIL_TH");
                obj_str.Append(", LEAVE_NAME_EN AS LEAVE_DETAIL_EN");
                obj_str.Append(", INITIAL_NAME_EN + WORKER_FNAME_EN + ' ' + WORKER_LNAME_EN AS WORKER_DETAIL_EN");
                obj_str.Append(" FROM ATT_TR_EMPLEAVEACC");

                obj_str.Append(" INNER JOIN EMP_MT_WORKER ON EMP_MT_WORKER.COMPANY_CODE=ATT_TR_EMPLEAVEACC.COMPANY_CODE AND EMP_MT_WORKER.WORKER_CODE=ATT_TR_EMPLEAVEACC.WORKER_CODE");
                obj_str.Append(" INNER JOIN EMP_MT_INITIAL ON EMP_MT_INITIAL.INITIAL_CODE=EMP_MT_WORKER.WORKER_INITIAL ");

                obj_str.Append(" INNER JOIN ATT_MT_LEAVE ON ATT_MT_LEAVE.COMPANY_CODE=ATT_TR_EMPLEAVEACC.COMPANY_CODE AND ATT_MT_LEAVE.LEAVE_CODE=ATT_TR_EMPLEAVEACC.LEAVE_CODE");


                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY ATT_TR_EMPLEAVEACC.COMPANY_CODE, ATT_TR_EMPLEAVEACC.WORKER_CODE, ATT_TR_EMPLEAVEACC.YEAR_CODE, ATT_TR_EMPLEAVEACC.LEAVE_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TREmpleaveacc();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                    model.worker_code = Convert.ToString(dr["WORKER_CODE"]);
                    model.year_code = Convert.ToString(dr["YEAR_CODE"]);
                    model.leave_code = Convert.ToString(dr["LEAVE_CODE"]);
                    model.empleaveacc_bf = Convert.ToDouble(dr["EMPLEAVEACC_BF"]);
                    model.empleaveacc_annual = Convert.ToDouble(dr["EMPLEAVEACC_ANNUAL"]);
                    model.empleaveacc_used = Convert.ToDouble(dr["EMPLEAVEACC_USED"]);
                    model.empleaveacc_remain = Convert.ToDouble(dr["EMPLEAVEACC_REMAIN"]);

                    model.worker_detail_th = Convert.ToString(dr["WORKER_DETAIL_TH"]);
                    model.leave_detail_th = Convert.ToString(dr["LEAVE_DETAIL_TH"]);
                    model.worker_detail_en = Convert.ToString(dr["WORKER_DETAIL_EN"]);
                    model.leave_detail_en = Convert.ToString(dr["LEAVE_DETAIL_EN"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Empleaveacc.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TREmpleaveacc> getDataByFillter(string com, string emp, string year)
        {
            string strCondition = "";

            strCondition += " AND ATT_TR_EMPLEAVEACC.COMPANY_CODE='" + com + "'";

            if (!emp.Equals(""))
                strCondition += " AND ATT_TR_EMPLEAVEACC.WORKER_CODE='" + emp + "'";

            if (!year.Equals(""))
                strCondition += " AND ATT_TR_EMPLEAVEACC.YEAR_CODE='" + year + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string emp, string year, string leave)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT WORKER_CODE");
                obj_str.Append(" FROM ATT_TR_EMPLEAVEACC");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                obj_str.Append(" AND YEAR_CODE='" + year + "' ");
                obj_str.Append(" AND LEAVE_CODE='" + leave + "' ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Empleaveacc.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string emp, string year, string leave)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_EMPLEAVEACC");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                obj_str.Append(" AND YEAR_CODE='" + year + "' ");
                obj_str.Append(" AND LEAVE_CODE='" + leave + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Empleaveacc.delete)" + ex.ToString();
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

                obj_str.Append(" DELETE FROM ATT_TR_EMPLEAVEACC");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());
            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Empleaveacc.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TREmpleaveacc model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.worker_code, model.year_code, model.leave_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO ATT_TR_EMPLEAVEACC");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", YEAR_CODE ");
                obj_str.Append(", LEAVE_CODE ");
                obj_str.Append(", EMPLEAVEACC_BF ");
                obj_str.Append(", EMPLEAVEACC_ANNUAL ");
                obj_str.Append(", EMPLEAVEACC_USED ");
                obj_str.Append(", EMPLEAVEACC_REMAIN ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @YEAR_CODE ");
                obj_str.Append(", @LEAVE_CODE ");
                obj_str.Append(", @EMPLEAVEACC_BF ");
                obj_str.Append(", @EMPLEAVEACC_ANNUAL ");
                obj_str.Append(", @EMPLEAVEACC_USED ");
                obj_str.Append(", @EMPLEAVEACC_REMAIN ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@YEAR_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@YEAR_CODE"].Value = model.year_code;
                obj_cmd.Parameters.Add("@LEAVE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@LEAVE_CODE"].Value = model.leave_code;
                obj_cmd.Parameters.Add("@EMPLEAVEACC_BF", SqlDbType.Decimal); obj_cmd.Parameters["@EMPLEAVEACC_BF"].Value = model.empleaveacc_bf;
                obj_cmd.Parameters.Add("@EMPLEAVEACC_ANNUAL", SqlDbType.Decimal); obj_cmd.Parameters["@EMPLEAVEACC_ANNUAL"].Value = model.empleaveacc_annual;
                obj_cmd.Parameters.Add("@EMPLEAVEACC_USED", SqlDbType.Decimal); obj_cmd.Parameters["@EMPLEAVEACC_USED"].Value = model.empleaveacc_used;
                obj_cmd.Parameters.Add("@EMPLEAVEACC_REMAIN", SqlDbType.Decimal); obj_cmd.Parameters["@EMPLEAVEACC_REMAIN"].Value = model.empleaveacc_remain;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Empleaveacc.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(string com, string emp, string year, List<cls_TREmpleaveacc> list_model)
        {
            bool blnResult = false;
            cls_ctConnection obj_conn = new cls_ctConnection();
            try
            {

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO ATT_TR_EMPLEAVEACC");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", YEAR_CODE ");
                obj_str.Append(", LEAVE_CODE ");
                obj_str.Append(", EMPLEAVEACC_BF ");
                obj_str.Append(", EMPLEAVEACC_ANNUAL ");
                obj_str.Append(", EMPLEAVEACC_USED ");
                obj_str.Append(", EMPLEAVEACC_REMAIN ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @YEAR_CODE ");
                obj_str.Append(", @LEAVE_CODE ");
                obj_str.Append(", @EMPLEAVEACC_BF ");
                obj_str.Append(", @EMPLEAVEACC_ANNUAL ");
                obj_str.Append(", @EMPLEAVEACC_USED ");
                obj_str.Append(", @EMPLEAVEACC_REMAIN ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM ATT_TR_EMPLEAVEACC");
                obj_str2.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str2.Append(" AND WORKER_CODE='" + emp + "'");
                obj_str2.Append(" AND YEAR_CODE='" + year + "'");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());

                if (blnResult)
                {
                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@YEAR_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@LEAVE_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@EMPLEAVEACC_BF", SqlDbType.Decimal);
                    obj_cmd.Parameters.Add("@EMPLEAVEACC_ANNUAL", SqlDbType.Decimal);
                    obj_cmd.Parameters.Add("@EMPLEAVEACC_USED", SqlDbType.Decimal);
                    obj_cmd.Parameters.Add("@EMPLEAVEACC_REMAIN", SqlDbType.Decimal);

                    obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime);
                    obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit);

                    foreach (cls_TREmpleaveacc model in list_model)
                    {

                        obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                        obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                        obj_cmd.Parameters["@YEAR_CODE"].Value = model.year_code;
                        obj_cmd.Parameters["@LEAVE_CODE"].Value = model.leave_code;
                        obj_cmd.Parameters["@EMPLEAVEACC_BF"].Value = model.empleaveacc_bf;
                        obj_cmd.Parameters["@EMPLEAVEACC_ANNUAL"].Value = model.empleaveacc_annual;
                        obj_cmd.Parameters["@EMPLEAVEACC_USED"].Value = model.empleaveacc_used;
                        obj_cmd.Parameters["@EMPLEAVEACC_REMAIN"].Value = model.empleaveacc_remain;

                        obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                        obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                        obj_cmd.Parameters["@FLAG"].Value = false;

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


                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Empleaveacc.insert)" + ex.ToString();
                obj_conn.doRollback();
            }
            finally
            {
                obj_conn.doClose();
            }

            return blnResult;
        }

        public bool update(cls_TREmpleaveacc model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE ATT_TR_EMPLEAVEACC SET ");

                obj_str.Append("EMPLEAVEACC_BF=@EMPLEAVEACC_BF ");
                obj_str.Append(", EMPLEAVEACC_ANNUAL=@EMPLEAVEACC_ANNUAL ");
                obj_str.Append(", EMPLEAVEACC_USED=@EMPLEAVEACC_USED ");
                obj_str.Append(", EMPLEAVEACC_REMAIN=@EMPLEAVEACC_REMAIN ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" AND YEAR_CODE=@YEAR_CODE ");
                obj_str.Append(" AND LEAVE_CODE=@LEAVE_CODE ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@EMPLEAVEACC_BF", SqlDbType.Decimal); obj_cmd.Parameters["@EMPLEAVEACC_BF"].Value = model.empleaveacc_bf;
                obj_cmd.Parameters.Add("@EMPLEAVEACC_ANNUAL", SqlDbType.Decimal); obj_cmd.Parameters["@EMPLEAVEACC_ANNUAL"].Value = model.empleaveacc_annual;
                obj_cmd.Parameters.Add("@EMPLEAVEACC_USED", SqlDbType.Decimal); obj_cmd.Parameters["@EMPLEAVEACC_USED"].Value = model.empleaveacc_used;
                obj_cmd.Parameters.Add("@EMPLEAVEACC_REMAIN", SqlDbType.Decimal); obj_cmd.Parameters["@EMPLEAVEACC_REMAIN"].Value = model.empleaveacc_remain;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@YEAR_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@YEAR_CODE"].Value = model.year_code;
                obj_cmd.Parameters.Add("@LEAVE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@LEAVE_CODE"].Value = model.leave_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Empleaveacc.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
