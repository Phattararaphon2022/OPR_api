using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;
namespace ClassLibrary_BPC.hrfocus.controller
{
 public class cls_ctMTRateot
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTRateot() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTRateot> getData(string condition)
        {
            List<cls_MTRateot> list_model = new List<cls_MTRateot>();
            cls_MTRateot model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("RATEOT_ID");
                obj_str.Append(", RATEOT_CODE");
                obj_str.Append(", ISNULL(RATEOT_NAME_TH, '') AS RATEOT_NAME_TH");
                obj_str.Append(", ISNULL(RATEOT_NAME_EN, '') AS RATEOT_NAME_EN");

                obj_str.Append(", COMPANY_CODE");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");
                
                obj_str.Append(" FROM ATT_MT_RATEOT");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY RATEOT_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTRateot();

                    model.rateot_id = Convert.ToInt32(dr["RATEOT_ID"]);
                    model.rateot_code = dr["RATEOT_CODE"].ToString();
                    model.rateot_name_th = dr["RATEOT_NAME_TH"].ToString();
                    model.rateot_name_en = dr["RATEOT_NAME_EN"].ToString();
                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                                                                                                                      
                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "ERROR::(Rateot.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTRateot> getDataByFillter(string com, string id, string code)
        {
            string strCondition = " AND COMPANY_CODE='" + com + "'";

            if (!id.Equals(""))
                strCondition += " AND RATEOT_ID='" + id + "'";

            if (!code.Equals(""))
                strCondition += " AND RATEOT_CODE='" + code + "'";
            
            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT RATEOT_ID");
                obj_str.Append(" FROM ATT_MT_RATEOT");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND RATEOT_CODE='" + code + "'");
                                                
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Rateot.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(RATEOT_ID) ");
                obj_str.Append(" FROM ATT_MT_RATEOT");             

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Rateot.getNextID)" + ex.ToString();
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

                obj_str.Append(" DELETE FROM ATT_MT_RATEOT");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND RATEOT_ID='" + id + "'");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                                              
                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Rateot.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTRateot model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.rateot_code))
                {
                    if (model.rateot_id.Equals(0))
                    {
                        return "D";
                    }
                    else
                    {
                        return this.update(model);
                    }
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO ATT_MT_RATEOT");
                obj_str.Append(" (");
                obj_str.Append("RATEOT_ID ");
                obj_str.Append(", RATEOT_CODE ");
                obj_str.Append(", RATEOT_NAME_TH ");
                obj_str.Append(", RATEOT_NAME_EN ");
                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append(" @RATEOT_ID ");
                obj_str.Append(", @RATEOT_CODE ");
                obj_str.Append(", @RATEOT_NAME_TH ");
                obj_str.Append(", @RATEOT_NAME_EN ");
                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");              
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@RATEOT_ID", SqlDbType.Int); obj_cmd.Parameters["@RATEOT_ID"].Value = id;
                obj_cmd.Parameters.Add("@RATEOT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@RATEOT_CODE"].Value = model.rateot_code;
                obj_cmd.Parameters.Add("@RATEOT_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@RATEOT_NAME_TH"].Value = model.rateot_name_th;
                obj_cmd.Parameters.Add("@RATEOT_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@RATEOT_NAME_EN"].Value = model.rateot_name_en;
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;
     
                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Rateot.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_MTRateot model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE ATT_MT_RATEOT SET ");

                obj_str.Append(" RATEOT_CODE=@RATEOT_CODE ");
                obj_str.Append(", RATEOT_NAME_TH=@RATEOT_NAME_TH ");
                obj_str.Append(", RATEOT_NAME_EN=@RATEOT_NAME_EN ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");
                
                obj_str.Append(" WHERE RATEOT_ID=@RATEOT_ID ");
                                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@RATEOT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@RATEOT_CODE"].Value = model.rateot_code;
                obj_cmd.Parameters.Add("@RATEOT_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@RATEOT_NAME_TH"].Value = model.rateot_name_th;
                obj_cmd.Parameters.Add("@RATEOT_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@RATEOT_NAME_EN"].Value = model.rateot_name_en;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.Parameters.Add("@RATEOT_ID", SqlDbType.Int); obj_cmd.Parameters["@RATEOT_ID"].Value = model.rateot_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.rateot_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Rateot.update)" + ex.ToString();
            }

            return blnResult;
        }
        public string totalot(string com, string emp, string year)
        {
            string blnResult = "0";
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(SUM(TIMEOT_BEFOREMIN+TIMEOT_NORMALMIN+TIMEOT_AFTERMIN),0) FROM ATT_TR_TIMEOT WHERE WORKER_CODE = '" + emp + "' AND COMPANY_CODE = '" + com + "' AND YEAR(TIMEOT_WORKDATE) ='" + year + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                blnResult = ex.ToString();
                Message = "ERROR::(Level.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
