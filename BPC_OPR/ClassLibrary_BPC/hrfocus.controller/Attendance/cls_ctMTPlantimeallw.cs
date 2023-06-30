using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTPlantimeallw
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTPlantimeallw() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTPlantimeallw> getData(string condition)
        {
            List<cls_MTPlantimeallw> list_model = new List<cls_MTPlantimeallw>();
            cls_MTPlantimeallw model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PLANTIMEALLW_ID");
                obj_str.Append(", PLANTIMEALLW_CODE");
                obj_str.Append(", ISNULL(PLANTIMEALLW_NAME_TH, '') AS PLANTIMEALLW_NAME_TH");
                obj_str.Append(", ISNULL(PLANTIMEALLW_NAME_EN, '') AS PLANTIMEALLW_NAME_EN");
                obj_str.Append(", COMPANY_CODE");

                obj_str.Append(", ISNULL(PLANTIMEALLW_PASSPRO, '0') AS PLANTIMEALLW_PASSPRO");
                obj_str.Append(", ISNULL(PLANTIMEALLW_LASTPERIOD, '0') AS PLANTIMEALLW_LASTPERIOD");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM ATT_MT_PLANTIMEALLW");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PLANTIMEALLW_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTPlantimeallw();

                    model.plantimeallw_id = Convert.ToInt32(dr["PLANTIMEALLW_ID"]);
                    model.plantimeallw_code = dr["PLANTIMEALLW_CODE"].ToString();
                    model.plantimeallw_name_th = dr["PLANTIMEALLW_NAME_TH"].ToString();
                    model.plantimeallw_name_en = dr["PLANTIMEALLW_NAME_EN"].ToString();

                    model.plantimeallw_passpro = dr["PLANTIMEALLW_PASSPRO"].ToString();
                    model.plantimeallw_lastperiod = dr["PLANTIMEALLW_LASTPERIOD"].ToString();

                    model.company_code = dr["COMPANY_CODE"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(PLANTIMEALLW.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTPlantimeallw> getDataByFillter(string com, string id, string code)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!id.Equals(""))
                strCondition += " AND PLANTIMEALLW_ID='" + id + "'";

            if (!code.Equals(""))
                strCondition += " AND PLANTIMEALLW_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PLANTIMEALLW_ID");
                obj_str.Append(" FROM ATT_MT_PLANTIMEALLW");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PLANTIMEALLW_CODE='" + code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(PLANTIMEALLW.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(PLANTIMEALLW_ID) ");
                obj_str.Append(" FROM ATT_MT_PLANTIMEALLW");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(PLANTIMEALLW.getNextID)" + ex.ToString();
            }

            return intResult;
        }

        public bool delete(string id ,string com)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_MT_PLANTIMEALLW");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND PLANTIMEALLW_ID='" + id + "'");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(PLANTIMEALLW.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTPlantimeallw model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.plantimeallw_code))
                {
                    if (model.plantimeallw_id.Equals(0))
                    {
                        return "";
                    }
                    else
                    {
                        return this.update(model);
                    }
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO ATT_MT_PLANTIMEALLW");
                obj_str.Append(" (");
                obj_str.Append("PLANTIMEALLW_ID ");
                obj_str.Append(", PLANTIMEALLW_CODE ");
                obj_str.Append(", PLANTIMEALLW_NAME_TH ");
                obj_str.Append(", PLANTIMEALLW_NAME_EN ");

                obj_str.Append(", PLANTIMEALLW_PASSPRO ");
                obj_str.Append(", PLANTIMEALLW_LASTPERIOD ");

                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append(" @PLANTIMEALLW_ID ");
                obj_str.Append(", @PLANTIMEALLW_CODE ");
                obj_str.Append(", @PLANTIMEALLW_NAME_TH ");
                obj_str.Append(", @PLANTIMEALLW_NAME_EN ");

                obj_str.Append(", @PLANTIMEALLW_PASSPRO ");
                obj_str.Append(", @PLANTIMEALLW_LASTPERIOD ");

                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PLANTIMEALLW_ID", SqlDbType.Int); obj_cmd.Parameters["@PLANTIMEALLW_ID"].Value = id;
                obj_cmd.Parameters.Add("@PLANTIMEALLW_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PLANTIMEALLW_CODE"].Value = model.plantimeallw_code;
                obj_cmd.Parameters.Add("@PLANTIMEALLW_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PLANTIMEALLW_NAME_TH"].Value = model.plantimeallw_name_th;
                obj_cmd.Parameters.Add("@PLANTIMEALLW_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PLANTIMEALLW_NAME_EN"].Value = model.plantimeallw_name_en;

                obj_cmd.Parameters.Add("@PLANTIMEALLW_PASSPRO", SqlDbType.VarChar); obj_cmd.Parameters["@PLANTIMEALLW_PASSPRO"].Value = model.plantimeallw_passpro;
                obj_cmd.Parameters.Add("@PLANTIMEALLW_LASTPERIOD", SqlDbType.VarChar); obj_cmd.Parameters["@PLANTIMEALLW_LASTPERIOD"].Value = model.plantimeallw_lastperiod;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = id.ToString(); ;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(PLANTIMEALLW.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_MTPlantimeallw model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE ATT_MT_PLANTIMEALLW SET ");

                obj_str.Append(" PLANTIMEALLW_CODE=@PLANTIMEALLW_CODE ");
                obj_str.Append(", PLANTIMEALLW_NAME_TH=@PLANTIMEALLW_NAME_TH ");
                obj_str.Append(", PLANTIMEALLW_NAME_EN=@PLANTIMEALLW_NAME_EN ");

                obj_str.Append(", PLANTIMEALLW_PASSPRO=@PLANTIMEALLW_PASSPRO ");
                obj_str.Append(", PLANTIMEALLW_LASTPERIOD=@PLANTIMEALLW_LASTPERIOD ");

                obj_str.Append(", COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");


                obj_str.Append(" WHERE PLANTIMEALLW_ID=@PLANTIMEALLW_ID ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PLANTIMEALLW_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PLANTIMEALLW_CODE"].Value = model.plantimeallw_code;
                obj_cmd.Parameters.Add("@PLANTIMEALLW_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PLANTIMEALLW_NAME_TH"].Value = model.plantimeallw_name_th;
                obj_cmd.Parameters.Add("@PLANTIMEALLW_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PLANTIMEALLW_NAME_EN"].Value = model.plantimeallw_name_en;

                obj_cmd.Parameters.Add("@PLANTIMEALLW_PASSPRO", SqlDbType.VarChar); obj_cmd.Parameters["@PLANTIMEALLW_PASSPRO"].Value = model.plantimeallw_passpro;
                obj_cmd.Parameters.Add("@PLANTIMEALLW_LASTPERIOD", SqlDbType.VarChar); obj_cmd.Parameters["@PLANTIMEALLW_LASTPERIOD"].Value = model.plantimeallw_lastperiod;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.Parameters.Add("@PLANTIMEALLW_ID", SqlDbType.Int); obj_cmd.Parameters["@PLANTIMEALLW_ID"].Value = model.plantimeallw_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.plantimeallw_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(PLANTIMEALLW.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
