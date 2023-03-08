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
    public class cls_ctMTTimeimpformat
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTTimeimpformat() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTTimeimpformat> getData(string condition)
        {
            List<cls_MTTimeimpformat> list_model = new List<cls_MTTimeimpformat>();
            cls_MTTimeimpformat model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", DATE_FORMAT");

                obj_str.Append(", CARD_START");
                obj_str.Append(", CARD_LENGHT");

                obj_str.Append(", DATE_START");
                obj_str.Append(", DATE_LENGHT");

                obj_str.Append(", HOURS_START");
                obj_str.Append(", HOURS_LENGHT");

                obj_str.Append(", MINUTE_START");
                obj_str.Append(", MINUTE_LENGHT");

                obj_str.Append(", ISNULL(FUNCTION_START, 0) AS FUNCTION_START");
                obj_str.Append(", ISNULL(FUNCTION_LENGHT, 0) AS FUNCTION_LENGHT");

                obj_str.Append(", ISNULL(MACHINE_START, 0) AS MACHINE_START");
                obj_str.Append(", ISNULL(MACHINE_LENGHT, 0) AS MACHINE_LENGHT");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");
                
                obj_str.Append(" FROM ATT_MT_TIMEIMPFORMAT");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTTimeimpformat();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.date_format = dr["DATE_FORMAT"].ToString();

                    model.card_start = Convert.ToInt32(dr["CARD_START"]);
                    model.card_lenght = Convert.ToInt32(dr["CARD_LENGHT"]);

                    model.date_start = Convert.ToInt32(dr["DATE_START"]);
                    model.date_lenght = Convert.ToInt32(dr["DATE_LENGHT"]);

                    model.hours_start = Convert.ToInt32(dr["HOURS_START"]);
                    model.hours_lenght = Convert.ToInt32(dr["HOURS_LENGHT"]);

                    model.minute_start = Convert.ToInt32(dr["MINUTE_START"]);
                    model.minute_lenght = Convert.ToInt32(dr["MINUTE_LENGHT"]);

                    model.function_start = Convert.ToInt32(dr["FUNCTION_START"]);
                    model.function_lenght = Convert.ToInt32(dr["FUNCTION_LENGHT"]);

                    model.machine_start = Convert.ToInt32(dr["MACHINE_START"]);
                    model.machine_lenght = Convert.ToInt32(dr["MACHINE_LENGHT"]);
                    

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                                                                                                                      
                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "ERROR::(Timeimpformat.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTTimeimpformat> getDataByFillter(string com)
        {
            string strCondition = "";

            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";
            
            return this.getData(strCondition);
        }

        public bool checkDataOld(string com)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM ATT_MT_TIMEIMPFORMAT");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                                                
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Timeimpformat.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_MT_TIMEIMPFORMAT");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                                              
                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Timeimpformat.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_MTTimeimpformat model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO ATT_MT_TIMEIMPFORMAT");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", DATE_FORMAT ");

                obj_str.Append(", CARD_START ");
                obj_str.Append(", CARD_LENGHT ");
                
                obj_str.Append(", DATE_START ");
                obj_str.Append(", DATE_LENGHT ");

                obj_str.Append(", HOURS_START ");
                obj_str.Append(", HOURS_LENGHT ");

                obj_str.Append(", MINUTE_START ");
                obj_str.Append(", MINUTE_LENGHT ");

                obj_str.Append(", FUNCTION_START ");
                obj_str.Append(", FUNCTION_LENGHT ");

                obj_str.Append(", MACHINE_START ");
                obj_str.Append(", MACHINE_LENGHT ");
                
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @DATE_FORMAT ");

                obj_str.Append(", @CARD_START ");
                obj_str.Append(", @CARD_LENGHT ");

                obj_str.Append(", @DATE_START ");
                obj_str.Append(", @DATE_LENGHT ");

                obj_str.Append(", @HOURS_START ");
                obj_str.Append(", @HOURS_LENGHT ");

                obj_str.Append(", @MINUTE_START ");
                obj_str.Append(", @MINUTE_LENGHT ");

                obj_str.Append(", @FUNCTION_START ");
                obj_str.Append(", @FUNCTION_LENGHT ");

                obj_str.Append(", @MACHINE_START ");
                obj_str.Append(", @MACHINE_LENGHT ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");         
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

                obj_cmd.Parameters.Add("@DATE_FORMAT", SqlDbType.VarChar); obj_cmd.Parameters["@DATE_FORMAT"].Value = model.date_format;

                obj_cmd.Parameters.Add("@CARD_START", SqlDbType.Int); obj_cmd.Parameters["@CARD_START"].Value = model.card_start;
                obj_cmd.Parameters.Add("@CARD_LENGHT", SqlDbType.Int); obj_cmd.Parameters["@CARD_LENGHT"].Value = model.card_lenght;

                obj_cmd.Parameters.Add("@DATE_START", SqlDbType.Int); obj_cmd.Parameters["@DATE_START"].Value = model.date_start;
                obj_cmd.Parameters.Add("@DATE_LENGHT", SqlDbType.Int); obj_cmd.Parameters["@DATE_LENGHT"].Value = model.date_lenght;

                obj_cmd.Parameters.Add("@HOURS_START", SqlDbType.Int); obj_cmd.Parameters["@HOURS_START"].Value = model.hours_start;
                obj_cmd.Parameters.Add("@HOURS_LENGHT", SqlDbType.Int); obj_cmd.Parameters["@HOURS_LENGHT"].Value = model.hours_lenght;

                obj_cmd.Parameters.Add("@MINUTE_START", SqlDbType.Int); obj_cmd.Parameters["@MINUTE_START"].Value = model.minute_start;
                obj_cmd.Parameters.Add("@MINUTE_LENGHT", SqlDbType.Int); obj_cmd.Parameters["@MINUTE_LENGHT"].Value = model.minute_lenght;

                obj_cmd.Parameters.Add("@FUNCTION_START", SqlDbType.Int); obj_cmd.Parameters["@FUNCTION_START"].Value = model.function_start;
                obj_cmd.Parameters.Add("@FUNCTION_LENGHT", SqlDbType.Int); obj_cmd.Parameters["@FUNCTION_LENGHT"].Value = model.function_lenght;

                obj_cmd.Parameters.Add("@MACHINE_START", SqlDbType.Int); obj_cmd.Parameters["@MACHINE_START"].Value = model.machine_start;
                obj_cmd.Parameters.Add("@MACHINE_LENGHT", SqlDbType.Int); obj_cmd.Parameters["@MACHINE_LENGHT"].Value = model.machine_lenght;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;
     
                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Timeimpformat.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public bool update(cls_MTTimeimpformat model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE ATT_MT_TIMEIMPFORMAT SET ");

                obj_str.Append(" DATE_FORMAT=@DATE_FORMAT ");

                obj_str.Append(", CARD_START=@CARD_START ");
                obj_str.Append(", CARD_LENGHT=@CARD_LENGHT ");

                obj_str.Append(", DATE_START=@DATE_START ");
                obj_str.Append(", DATE_LENGHT=@DATE_LENGHT ");

                obj_str.Append(", HOURS_START=@HOURS_START ");
                obj_str.Append(", HOURS_LENGHT=@HOURS_LENGHT ");

                obj_str.Append(", MINUTE_START=@MINUTE_START ");
                obj_str.Append(", MINUTE_LENGHT=@MINUTE_LENGHT ");

                obj_str.Append(", FUNCTION_START=@FUNCTION_START ");
                obj_str.Append(", FUNCTION_LENGHT=@FUNCTION_LENGHT ");

                obj_str.Append(", MACHINE_START=@MACHINE_START ");
                obj_str.Append(", MACHINE_LENGHT=@MACHINE_LENGHT ");
                
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@DATE_FORMAT", SqlDbType.VarChar); obj_cmd.Parameters["@DATE_FORMAT"].Value = model.date_format;

                obj_cmd.Parameters.Add("@CARD_START", SqlDbType.Int); obj_cmd.Parameters["@CARD_START"].Value = model.card_start;
                obj_cmd.Parameters.Add("@CARD_LENGHT", SqlDbType.Int); obj_cmd.Parameters["@CARD_LENGHT"].Value = model.card_lenght;

                obj_cmd.Parameters.Add("@DATE_START", SqlDbType.Int); obj_cmd.Parameters["@DATE_START"].Value = model.date_start;
                obj_cmd.Parameters.Add("@DATE_LENGHT", SqlDbType.Int); obj_cmd.Parameters["@DATE_LENGHT"].Value = model.date_lenght;

                obj_cmd.Parameters.Add("@HOURS_START", SqlDbType.Int); obj_cmd.Parameters["@HOURS_START"].Value = model.hours_start;
                obj_cmd.Parameters.Add("@HOURS_LENGHT", SqlDbType.Int); obj_cmd.Parameters["@HOURS_LENGHT"].Value = model.hours_lenght;

                obj_cmd.Parameters.Add("@MINUTE_START", SqlDbType.Int); obj_cmd.Parameters["@MINUTE_START"].Value = model.minute_start;
                obj_cmd.Parameters.Add("@MINUTE_LENGHT", SqlDbType.Int); obj_cmd.Parameters["@MINUTE_LENGHT"].Value = model.minute_lenght;

                obj_cmd.Parameters.Add("@FUNCTION_START", SqlDbType.Int); obj_cmd.Parameters["@FUNCTION_START"].Value = model.function_start;
                obj_cmd.Parameters.Add("@FUNCTION_LENGHT", SqlDbType.Int); obj_cmd.Parameters["@FUNCTION_LENGHT"].Value = model.function_lenght;

                obj_cmd.Parameters.Add("@MACHINE_START", SqlDbType.Int); obj_cmd.Parameters["@MACHINE_START"].Value = model.machine_start;
                obj_cmd.Parameters.Add("@MACHINE_LENGHT", SqlDbType.Int); obj_cmd.Parameters["@MACHINE_LENGHT"].Value = model.machine_lenght;


                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Timeimpformat.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
