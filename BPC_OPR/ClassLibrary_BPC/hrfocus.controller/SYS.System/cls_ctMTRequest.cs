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
   public class cls_ctMTRequest
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTRequest() { }

        public string getMessage() { return this.Message.Replace("OPR_MT_REQUESTS", "").Replace("cls_ctMTRequest", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTRequest> getData(string condition)
        {
            List<cls_MTRequest> list_model = new List<cls_MTRequest>();
            cls_MTRequest model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("REQUEST_ID");
                obj_str.Append(", REQUEST_CODE");
                obj_str.Append(", ISNULL(REQUEST_DATE, '') AS REQUEST_DATE");
                obj_str.Append(", ISNULL(REQUEST_AGENCY, '') AS REQUEST_AGENCY");
                obj_str.Append(", ISNULL(REQUEST_WORK, '') AS REQUEST_WORK");

                obj_str.Append(", ISNULL(REQUEST_JOB_TYPE, '') AS REQUEST_JOB_TYPE");

                obj_str.Append(", ISNULL(REQUEST_EMPLOYEE_TYPE, '') AS REQUEST_EMPLOYEE_TYPE");
                obj_str.Append(", ISNULL(REQUEST_QUANTITY, 0) AS REQUEST_QUANTITY");
                obj_str.Append(", ISNULL(REQUEST_URGENCY, '') AS REQUEST_URGENCY");
                obj_str.Append(", ISNULL(REQUEST_WAGE_RATE, 0) AS REQUEST_WAGE_RATE");
                obj_str.Append(", ISNULL(REQUEST_OVERTIME, 0) AS REQUEST_OVERTIME");
                obj_str.Append(", ISNULL(REQUEST_ANOTHER, '') AS REQUEST_ANOTHER");

              

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM OPR_MT_REQUESTS");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY REQUEST_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTRequest();

                    model.request_id = Convert.ToInt32(dr["REQUEST_ID"]);
                    model.request_code = dr["REQUEST_CODE"].ToString();
                    model.request_date = Convert.ToDateTime(dr["REQUEST_DATE"]);
                    model.request_agency = dr["REQUEST_AGENCY"].ToString();
                    model.request_work = dr["REQUEST_WORK"].ToString();
                    model.request_job_type = dr["REQUEST_JOB_TYPE"].ToString();
                    model.request_employee_type = dr["REQUEST_EMPLOYEE_TYPE"].ToString();
                    model.request_quantity = Convert.ToDouble(dr["REQUEST_QUANTITY"]);
                    model.request_urgency = dr["REQUEST_URGENCY"].ToString();
                    model.request_wage_rate = Convert.ToDouble(dr["REQUEST_WAGE_RATE"]);
                    model.request_overtime = Convert.ToDouble(dr["REQUEST_OVERTIME"]);

                    //model.request_overtime = Convert.ToInt32(dr["REQUEST_OVERTIME"]);
                    model.request_another = dr["REQUEST_ANOTHER"].ToString();

                    //model.created_by = dr["CREATED_BY"].ToString();
                    //model.created_date = Convert.ToDateTime(dr["CREATED_DATE"]);
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "REQ001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTRequest> getDataByFillter(string id, string code)
        {
            string strCondition = "";

            if (!id.Equals(""))
                strCondition += " AND REQUEST_ID='" + id + "'";

            if (!code.Equals(""))
                strCondition += " AND REQUEST_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(REQUEST_ID, 1) ");
                obj_str.Append(" FROM OPR_MT_REQUESTS");
                obj_str.Append(" ORDER BY REQUEST_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "REQ002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT REQUEST_ID");
                obj_str.Append(" FROM OPR_MT_REQUESTS");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND REQUEST_CODE='" + code + "'");


                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "REQ003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string code)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM OPR_MT_REQUESTS");
                obj_str.Append(" WHERE REQUEST_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "REQ004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTRequest model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.request_code))
                {
                    if (this.update(model))
                        return model.request_id.ToString();
                    else
                        return "";
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO OPR_MT_REQUESTS");
                obj_str.Append(" (");
                obj_str.Append("REQUEST_ID ");
                obj_str.Append(", REQUEST_CODE ");
                obj_str.Append(", REQUEST_DATE ");
                obj_str.Append(", REQUEST_AGENCY ");
                obj_str.Append(", REQUEST_WORK ");

                obj_str.Append(", REQUEST_JOB_TYPE ");

                obj_str.Append(", REQUEST_EMPLOYEE_TYPE ");
                obj_str.Append(", REQUEST_QUANTITY ");
                obj_str.Append(", REQUEST_URGENCY ");
                obj_str.Append(", REQUEST_WAGE_RATE ");
                obj_str.Append(", REQUEST_OVERTIME ");
                obj_str.Append(", REQUEST_ANOTHER ");


                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@REQUEST_ID ");
                obj_str.Append(", @REQUEST_CODE");
                obj_str.Append(", @REQUEST_DATE ");
                obj_str.Append(", @REQUEST_AGENCY ");
                obj_str.Append(", @REQUEST_WORK ");

                obj_str.Append(", @REQUEST_JOB_TYPE ");

                obj_str.Append(", @REQUEST_EMPLOYEE_TYPE ");
                obj_str.Append(", @REQUEST_QUANTITY ");
                obj_str.Append(", @REQUEST_URGENCY ");
                obj_str.Append(", @REQUEST_WAGE_RATE ");
                obj_str.Append(", @REQUEST_OVERTIME ");
                obj_str.Append(", @REQUEST_ANOTHER ");


                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.request_id = this.getNextID();

                obj_cmd.Parameters.Add("@REQUEST_ID", SqlDbType.Int); obj_cmd.Parameters["@REQUEST_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@REQUEST_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@REQUEST_CODE"].Value = model.request_code;
                //obj_cmd.Parameters.Add("@REQUEST_DATE", SqlDbType.VarChar); obj_cmd.Parameters["@REQUEST_DATE"].Value = model.request_date;
                obj_cmd.Parameters.Add("@REQUEST_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@REQUEST_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@REQUEST_AGENCY", SqlDbType.VarChar); obj_cmd.Parameters["@REQUEST_AGENCY"].Value = model.request_agency;
                obj_cmd.Parameters.Add("@REQUEST_WORK", SqlDbType.VarChar); obj_cmd.Parameters["@REQUEST_WORK"].Value = model.request_work;
                obj_cmd.Parameters.Add("@REQUEST_JOB_TYPE", SqlDbType.Decimal); obj_cmd.Parameters["@REQUEST_JOB_TYPE"].Value = model.request_job_type;

                obj_cmd.Parameters.Add("@REQUEST_EMPLOYEE_TYPE", SqlDbType.Decimal); obj_cmd.Parameters["@REQUEST_EMPLOYEE_TYPE"].Value = model.request_employee_type;
                
                //obj_cmd.Parameters.Add("@REQUEST_QUANTITY", SqlDbType.Decimal); obj_cmd.Parameters["@REQUEST_QUANTITY"].Value = model.request_quantity;
                obj_cmd.Parameters.Add("@REQUEST_QUANTITY", SqlDbType.Decimal); obj_cmd.Parameters["@REQUEST_QUANTITY"].Value = model.request_quantity;

                obj_cmd.Parameters.Add("@REQUEST_URGENCY", SqlDbType.Decimal); obj_cmd.Parameters["@REQUEST_URGENCY"].Value = model.request_urgency;
                
                //obj_cmd.Parameters.Add("@REQUEST_WAGE_RATE", SqlDbType.Decimal); obj_cmd.Parameters["@REQUEST_WAGE_RATE"].Value = model.request_wage_rate;
                obj_cmd.Parameters.Add("@REQUEST_WAGE_RATE", SqlDbType.Decimal); obj_cmd.Parameters["@REQUEST_WAGE_RATE"].Value = model.request_wage_rate;

                obj_cmd.Parameters.Add("@REQUEST_OVERTIME", SqlDbType.Decimal); obj_cmd.Parameters["@REQUEST_OVERTIME"].Value = model.request_overtime;

                //obj_cmd.Parameters.Add("@REQUEST_OVERTIME", SqlDbType.Int); obj_cmd.Parameters["@REQUEST_OVERTIME"].Value = model.request_overtime;
                obj_cmd.Parameters.Add("@REQUEST_ANOTHER", SqlDbType.Int); obj_cmd.Parameters["@REQUEST_ANOTHER"].Value = model.request_another;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                //obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                strResult = model.request_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "REQ005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTRequest model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE OPR_MT_REQUESTS SET ");
                obj_str.Append(" REQUEST_CODE=@REQUEST_CODE ");
                obj_str.Append(", REQUEST_DATE=@REQUEST_DATE ");
                obj_str.Append(", REQUEST_AGENCY=@REQUEST_AGENCY ");
                obj_str.Append(", REQUEST_WORK=@REQUEST_WORK ");
                obj_str.Append(", REQUEST_JOB_TYPE=@REQUEST_JOB_TYPE ");

                obj_str.Append(", REQUEST_EMPLOYEE_TYPE=@REQUEST_EMPLOYEE_TYPE ");
                obj_str.Append(", REQUEST_QUANTITY=@REQUEST_QUANTITY ");
                obj_str.Append(", REQUEST_URGENCY=@REQUEST_URGENCY ");
                obj_str.Append(", REQUEST_WAGE_RATE=@REQUEST_WAGE_RATE ");
                obj_str.Append(", REQUEST_OVERTIME=@REQUEST_OVERTIME ");
                obj_str.Append(", REQUEST_ANOTHER=@REQUEST_ANOTHER ");

                obj_str.Append(", CREATED_BY=@CREATED_BY ");
                obj_str.Append(", CREATED_DATE=@CREATED_DATE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                //obj_str.Append(", FLAG=@FLAG ");

                obj_str.Append(" WHERE REQUEST_ID=@REQUEST_ID ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@REQUEST_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@REQUEST_CODE"].Value = model.request_code;
                obj_cmd.Parameters.Add("@REQUEST_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@REQUEST_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@REQUEST_AGENCY", SqlDbType.VarChar); obj_cmd.Parameters["@REQUEST_AGENCY"].Value = model.request_agency;
                obj_cmd.Parameters.Add("@REQUEST_WORK", SqlDbType.VarChar); obj_cmd.Parameters["@REQUEST_WORK"].Value = model.request_work;

                obj_cmd.Parameters.Add("@REQUEST_JOB_TYPE", SqlDbType.Decimal); obj_cmd.Parameters["@REQUEST_JOB_TYPE"].Value = model.request_job_type;

                obj_cmd.Parameters.Add("@REQUEST_EMPLOYEE_TYPE", SqlDbType.Decimal); obj_cmd.Parameters["@REQUEST_EMPLOYEE_TYPE"].Value = model.request_employee_type;
                obj_cmd.Parameters.Add("@REQUEST_QUANTITY", SqlDbType.Decimal); obj_cmd.Parameters["@REQUEST_QUANTITY"].Value = model.request_quantity;
                obj_cmd.Parameters.Add("@REQUEST_URGENCY", SqlDbType.Decimal); obj_cmd.Parameters["@REQUEST_URGENCY"].Value = model.request_urgency;
                obj_cmd.Parameters.Add("@REQUEST_WAGE_RATE", SqlDbType.Decimal); obj_cmd.Parameters["@REQUEST_WAGE_RATE"].Value = model.request_wage_rate;
                obj_cmd.Parameters.Add("@REQUEST_OVERTIME", SqlDbType.Decimal); obj_cmd.Parameters["@REQUEST_OVERTIME"].Value = model.request_overtime;
                obj_cmd.Parameters.Add("@REQUEST_ANOTHER", SqlDbType.Int); obj_cmd.Parameters["@REQUEST_ANOTHER"].Value = model.request_another;

                //obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.created_by;
                //obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                //obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@REQUEST_ID", SqlDbType.Int); obj_cmd.Parameters["@REQUEST_ID"].Value = model.request_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "REQ006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}
