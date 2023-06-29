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
    public class cls_ctMTReduce
    {
    string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTReduce() { }

        public string getMessage() { return this.Message.Replace("SYS_MT_REDUCE", "").Replace("cls_ctMTReduce", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTReduce> getData(string condition)
        {
            List<cls_MTReduce> list_model = new List<cls_MTReduce>();
            cls_MTReduce model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("REDUCE_ID");
                obj_str.Append(", REDUCE_CODE");
                obj_str.Append(", ISNULL(REDUCE_NAME_TH, '') AS REDUCE_NAME_TH");
                obj_str.Append(", ISNULL(REDUCE_NAME_EN, '') AS REDUCE_NAME_EN");

                obj_str.Append(", ISNULL(REDUCE_AMOUNT, '') AS REDUCE_AMOUNT");
                obj_str.Append(", ISNULL(REDUCE_PERCENT, '') AS REDUCE_PERCENT");
                obj_str.Append(", ISNULL(REDUCE_PERCENT_MAX, '') AS REDUCE_PERCENT_MAX");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM SYS_MT_REDUCE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY REDUCE_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTReduce();

                    model.reduce_id = Convert.ToInt32(dr["REDUCE_ID"]);
                    model.reduce_code = dr["REDUCE_CODE"].ToString();
                    model.reduce_name_th = dr["REDUCE_NAME_TH"].ToString();
                    model.reduce_name_en = dr["REDUCE_NAME_EN"].ToString();

                    model.reduce_amount = Convert.ToDouble(dr["REDUCE_AMOUNT"]);
                    model.reduce_percent = Convert.ToDouble(dr["REDUCE_PERCENT"]);
                    model.reduce_percent_max = Convert.ToDouble(dr["REDUCE_PERCENT_MAX"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                                                                                            
                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "RED001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTReduce> getDataByFillter(string id, string code)
        {
            string strCondition = "";

            if (!id.Equals(""))
                strCondition += " AND REDUCE_ID='" + id + "'";

            if (!code.Equals(""))
                strCondition += " AND REDUCE_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(REDUCE_ID, 1) ");
                obj_str.Append(" FROM SYS_MT_REDUCE");
                obj_str.Append(" ORDER BY REDUCE_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "RED002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string code, string id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT REDUCE_ID");
                obj_str.Append(" FROM SYS_MT_REDUCE");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND REDUCE_CODE='" + code + "'");
                obj_str.Append(" AND REDUCE_ID='" + id + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "RED003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string id)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SYS_MT_REDUCE");
                obj_str.Append(" WHERE 1=1 ");
                //obj_str.Append(" AND REDUCE_ID='" + id + "'");
                obj_str.Append(" AND REDUCE_CODE='" + id + "'");


                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "RED004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTReduce model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.reduce_code, model.reduce_id.ToString()))
                {
                    if (this.update(model))
                        return model.reduce_id.ToString();
                    else
                        return "";                    
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_MT_REDUCE");
                obj_str.Append(" (");
                obj_str.Append("REDUCE_ID ");
                obj_str.Append(", REDUCE_CODE ");
                obj_str.Append(", REDUCE_NAME_TH ");
                obj_str.Append(", REDUCE_NAME_EN ");

                obj_str.Append(", REDUCE_AMOUNT ");
                obj_str.Append(", REDUCE_PERCENT ");
                obj_str.Append(", REDUCE_PERCENT_MAX ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append(" @REDUCE_ID ");
                obj_str.Append(", @REDUCE_CODE ");
                obj_str.Append(", @REDUCE_NAME_TH ");
                obj_str.Append(", @REDUCE_NAME_EN ");

                obj_str.Append(", @REDUCE_AMOUNT ");
                obj_str.Append(", @REDUCE_PERCENT ");
                obj_str.Append(", @REDUCE_PERCENT_MAX ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@REDUCE_ID", SqlDbType.Int); obj_cmd.Parameters["@REDUCE_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@REDUCE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@REDUCE_CODE"].Value = model.reduce_code;
                obj_cmd.Parameters.Add("@REDUCE_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@REDUCE_NAME_TH"].Value = model.reduce_name_th;
                obj_cmd.Parameters.Add("@REDUCE_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@REDUCE_NAME_EN"].Value = model.reduce_name_en;

                obj_cmd.Parameters.Add("@REDUCE_AMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@REDUCE_AMOUNT"].Value = model.reduce_amount;
                obj_cmd.Parameters.Add("@REDUCE_PERCENT", SqlDbType.Decimal); obj_cmd.Parameters["@REDUCE_PERCENT"].Value = model.reduce_percent;
                obj_cmd.Parameters.Add("@REDUCE_PERCENT_MAX", SqlDbType.Decimal); obj_cmd.Parameters["@REDUCE_PERCENT_MAX"].Value = model.reduce_percent_max;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;
     
                obj_cmd.ExecuteNonQuery();
                                
                obj_conn.doClose();
                strResult = model.reduce_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "RED005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTReduce model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE SYS_MT_REDUCE SET ");
                //obj_str.Append(" REDUCE_CODE=@REDUCE_CODE ");
                obj_str.Append("  REDUCE_NAME_TH=@REDUCE_NAME_TH ");
                obj_str.Append(", REDUCE_NAME_EN=@REDUCE_NAME_EN ");

                obj_str.Append(", REDUCE_AMOUNT=@REDUCE_AMOUNT ");
                obj_str.Append(", REDUCE_PERCENT=@REDUCE_PERCENT ");
                obj_str.Append(", REDUCE_PERCENT_MAX=@REDUCE_PERCENT_MAX ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");

                obj_str.Append(" WHERE REDUCE_CODE=@REDUCE_CODE ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@REDUCE_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@REDUCE_NAME_TH"].Value = model.reduce_name_th;
                obj_cmd.Parameters.Add("@REDUCE_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@REDUCE_NAME_EN"].Value = model.reduce_name_en;

                obj_cmd.Parameters.Add("@REDUCE_AMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@REDUCE_AMOUNT"].Value = model.reduce_amount;
                obj_cmd.Parameters.Add("@REDUCE_PERCENT", SqlDbType.Decimal); obj_cmd.Parameters["@REDUCE_PERCENT"].Value = model.reduce_percent;
                obj_cmd.Parameters.Add("@REDUCE_PERCENT_MAX", SqlDbType.Decimal); obj_cmd.Parameters["@REDUCE_PERCENT_MAX"].Value = model.reduce_percent_max;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                //obj_cmd.Parameters.Add("@REDUCE_ID", SqlDbType.Int); obj_cmd.Parameters["@REDUCE_ID"].Value = model.reduce_id;
                obj_cmd.Parameters.Add("@REDUCE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@REDUCE_CODE"].Value = model.reduce_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "RED006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}
 