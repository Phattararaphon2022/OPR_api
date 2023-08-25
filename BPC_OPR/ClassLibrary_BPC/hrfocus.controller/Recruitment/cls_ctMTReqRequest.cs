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
    public class cls_ctMTReqRequest
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTReqRequest() { }

        public string getMessage() { return this.Message.Replace("REQ_TR_REQUEST", "").Replace("cls_ctMTReqRequest", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }
        private List<cls_MTReqRequest> getData(string condition)
        {
            List<cls_MTReqRequest> list_model = new List<cls_MTReqRequest>();
            cls_MTReqRequest model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");
                obj_str.Append("COMPANY_CODE");

                obj_str.Append(", REQUEST_ID");
                obj_str.Append(", REQUEST_CODE");
                obj_str.Append(", ISNULL(REQUEST_DATE, '') AS REQUEST_DATE");
                obj_str.Append(", ISNULL(REQUEST_STARTDATE, '') AS REQUEST_STARTDATE");
                obj_str.Append(", ISNULL(REQUEST_ENDDATE, '') AS REQUEST_ENDDATE");
                obj_str.Append(", ISNULL(REQUEST_POSITION, '') AS REQUEST_POSITION");
                obj_str.Append(", ISNULL(REQUEST_PROJECT, '') AS REQUEST_PROJECT");

                obj_str.Append(", ISNULL(REQUEST_EMPLOYEE_TYPE, '') AS REQUEST_EMPLOYEE_TYPE");
                obj_str.Append(", ISNULL(REQUEST_QUANTITY, 0) AS REQUEST_QUANTITY");
                obj_str.Append(", ISNULL(REQUEST_URGENCY, '') AS REQUEST_URGENCY");
                obj_str.Append(", ISNULL(REQUEST_NOTE, '') AS REQUEST_NOTE");

                obj_str.Append(", ISNULL(REQUEST_ACCEPTED, 0) AS REQUEST_ACCEPTED");
                obj_str.Append(", REQUEST_STATUS");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM REQ_MT_REQUEST");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY REQUEST_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTReqRequest();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.request_id = Convert.ToInt32(dr["REQUEST_ID"]);
                    model.request_code = dr["REQUEST_CODE"].ToString();
                    model.request_date = Convert.ToDateTime(dr["REQUEST_DATE"]);
                    model.request_startdate = Convert.ToDateTime(dr["REQUEST_STARTDATE"]);
                    model.request_enddate = Convert.ToDateTime(dr["REQUEST_ENDDATE"]);
                    model.request_position = dr["REQUEST_POSITION"].ToString();
                    model.request_project = dr["REQUEST_PROJECT"].ToString();

                    model.request_employee_type = dr["REQUEST_EMPLOYEE_TYPE"].ToString();
                    model.request_quantity = Convert.ToDouble(dr["REQUEST_QUANTITY"]);
                    model.request_urgency = dr["REQUEST_URGENCY"].ToString();
                    model.request_note = dr["REQUEST_NOTE"].ToString();

                    model.request_accepted = Convert.ToDouble(dr["REQUEST_ACCEPTED"]);
                    model.request_status = Convert.ToInt32(dr["REQUEST_STATUS"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "REQST001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTReqRequest> getDataByFillter(string com, string code,int status)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!code.ToString().Equals(""))
                strCondition += " AND REQUEST_CODE='" + code + "'";
            if (!status.Equals(""))
                strCondition += " AND REQUEST_STATUS  ='" + status + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(REQUEST_ID, 1) ");
                obj_str.Append(" FROM REQ_MT_REQUEST");
                obj_str.Append(" ORDER BY REQUEST_ID DESC ");

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

        public bool checkDataOld(string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT REQUEST_ID");
                obj_str.Append(" FROM REQ_MT_REQUEST");
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
                Message = "REQST003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM REQ_MT_REQUEST");
                obj_str.Append(" WHERE REQUEST_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "REQST004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTReqRequest model)
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

                obj_str.Append("INSERT INTO REQ_MT_REQUEST");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");

                obj_str.Append(", REQUEST_ID ");
                obj_str.Append(", REQUEST_CODE ");
                obj_str.Append(", REQUEST_DATE ");
                obj_str.Append(", REQUEST_STARTDATE ");
                obj_str.Append(", REQUEST_ENDDATE ");
                obj_str.Append(", REQUEST_POSITION ");
                obj_str.Append(", REQUEST_PROJECT ");


                obj_str.Append(", REQUEST_EMPLOYEE_TYPE ");
                obj_str.Append(", REQUEST_QUANTITY ");
                obj_str.Append(", REQUEST_URGENCY ");

                obj_str.Append(", REQUEST_NOTE ");

                obj_str.Append(", REQUEST_ACCEPTED ");
                obj_str.Append(", REQUEST_STATUS ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");

                obj_str.Append(", @REQUEST_ID ");
                obj_str.Append(", @REQUEST_CODE ");
                obj_str.Append(", @REQUEST_DATE ");
                obj_str.Append(", @REQUEST_STARTDATE ");
                obj_str.Append(", @REQUEST_ENDDATE ");
                obj_str.Append(", @REQUEST_POSITION ");
                obj_str.Append(", @REQUEST_PROJECT ");


                obj_str.Append(", @REQUEST_EMPLOYEE_TYPE ");
                obj_str.Append(", @REQUEST_QUANTITY ");
                obj_str.Append(", @REQUEST_URGENCY ");

                obj_str.Append(", @REQUEST_NOTE ");
                obj_str.Append(", @REQUEST_ACCEPTED ");
                obj_str.Append(", @REQUEST_STATUS ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.request_id = this.getNextID();
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

                obj_cmd.Parameters.Add("@REQUEST_ID", SqlDbType.Int); obj_cmd.Parameters["@REQUEST_ID"].Value = model.request_id;
                obj_cmd.Parameters.Add("@REQUEST_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@REQUEST_CODE"].Value = model.request_code;
                obj_cmd.Parameters.Add("@REQUEST_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@REQUEST_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@REQUEST_STARTDATE", SqlDbType.DateTime); obj_cmd.Parameters["@REQUEST_STARTDATE"].Value = model.request_startdate;
                obj_cmd.Parameters.Add("@REQUEST_ENDDATE", SqlDbType.DateTime); obj_cmd.Parameters["@REQUEST_ENDDATE"].Value = model.request_enddate;

                obj_cmd.Parameters.Add("@REQUEST_POSITION", SqlDbType.VarChar); obj_cmd.Parameters["@REQUEST_POSITION"].Value = model.request_position;
                obj_cmd.Parameters.Add("@REQUEST_PROJECT", SqlDbType.VarChar); obj_cmd.Parameters["@REQUEST_PROJECT"].Value = model.request_project;


                obj_cmd.Parameters.Add("@REQUEST_EMPLOYEE_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@REQUEST_EMPLOYEE_TYPE"].Value = model.request_employee_type;

                obj_cmd.Parameters.Add("@REQUEST_QUANTITY", SqlDbType.Decimal); obj_cmd.Parameters["@REQUEST_QUANTITY"].Value = model.request_quantity;

                obj_cmd.Parameters.Add("@REQUEST_URGENCY", SqlDbType.VarChar); obj_cmd.Parameters["@REQUEST_URGENCY"].Value = model.request_urgency;

                obj_cmd.Parameters.Add("@REQUEST_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@REQUEST_NOTE"].Value = model.request_note;

                obj_cmd.Parameters.Add("@REQUEST_ACCEPTED", SqlDbType.Decimal); obj_cmd.Parameters["@REQUEST_ACCEPTED"].Value = 0;
                obj_cmd.Parameters.Add("@REQUEST_STATUS", SqlDbType.Int); obj_cmd.Parameters["@REQUEST_STATUS"].Value = 0;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                strResult = model.request_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "REQST005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTReqRequest model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE REQ_MT_REQUEST SET ");

                obj_str.Append("REQUEST_CODE=@REQUEST_CODE ");
                obj_str.Append(", REQUEST_DATE=@REQUEST_DATE ");
                obj_str.Append(", REQUEST_STARTDATE=@REQUEST_STARTDATE ");
                obj_str.Append(", REQUEST_ENDDATE=@REQUEST_ENDDATE ");

                obj_str.Append(", REQUEST_POSITION=@REQUEST_POSITION ");
                obj_str.Append(", REQUEST_PROJECT=@REQUEST_PROJECT ");

                obj_str.Append(", REQUEST_EMPLOYEE_TYPE=@REQUEST_EMPLOYEE_TYPE ");
                obj_str.Append(", REQUEST_QUANTITY=@REQUEST_QUANTITY ");
                obj_str.Append(", REQUEST_URGENCY=@REQUEST_URGENCY ");
                obj_str.Append(", REQUEST_NOTE=@REQUEST_NOTE ");

                obj_str.Append(", REQUEST_ACCEPTED=@REQUEST_ACCEPTED ");
                obj_str.Append(", REQUEST_STATUS=@REQUEST_STATUS ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE REQUEST_ID=@REQUEST_ID ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@REQUEST_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@REQUEST_CODE"].Value = model.request_code;
                obj_cmd.Parameters.Add("@REQUEST_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@REQUEST_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@REQUEST_STARTDATE", SqlDbType.DateTime); obj_cmd.Parameters["@REQUEST_STARTDATE"].Value = model.request_startdate;
                obj_cmd.Parameters.Add("@REQUEST_ENDDATE", SqlDbType.DateTime); obj_cmd.Parameters["@REQUEST_ENDDATE"].Value = model.request_enddate;

                obj_cmd.Parameters.Add("@REQUEST_POSITION", SqlDbType.VarChar); obj_cmd.Parameters["@REQUEST_POSITION"].Value = model.request_position;
                obj_cmd.Parameters.Add("@REQUEST_PROJECT", SqlDbType.VarChar); obj_cmd.Parameters["@REQUEST_PROJECT"].Value = model.request_project;


                obj_cmd.Parameters.Add("@REQUEST_EMPLOYEE_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@REQUEST_EMPLOYEE_TYPE"].Value = model.request_employee_type;

                obj_cmd.Parameters.Add("@REQUEST_QUANTITY", SqlDbType.Decimal); obj_cmd.Parameters["@REQUEST_QUANTITY"].Value = model.request_quantity;

                obj_cmd.Parameters.Add("@REQUEST_URGENCY", SqlDbType.VarChar); obj_cmd.Parameters["@REQUEST_URGENCY"].Value = model.request_urgency;
                obj_cmd.Parameters.Add("@REQUEST_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@REQUEST_NOTE"].Value = model.request_note;

                obj_cmd.Parameters.Add("@REQUEST_ACCEPTED", SqlDbType.VarChar); obj_cmd.Parameters["@REQUEST_ACCEPTED"].Value = model.request_accepted;
                obj_cmd.Parameters.Add("@REQUEST_STATUS", SqlDbType.VarChar); obj_cmd.Parameters["@REQUEST_STATUS"].Value = model.request_status;

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
                Message = "REQST006:" + ex.ToString();
            }

            return blnResult;
        }

        public List<cls_MTReqRequest> getPositionData(string com)
        {
            List<cls_MTReqRequest> list_model = new List<cls_MTReqRequest>();
            cls_MTReqRequest model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");
                obj_str.Append("REQ_MT_REQUEST.COMPANY_CODE");

                obj_str.Append(", REQ_MT_REQUEST.REQUEST_ID");
                obj_str.Append(", REQ_MT_REQUEST.REQUEST_CODE");
                obj_str.Append(", ISNULL(REQ_MT_REQUEST.REQUEST_POSITION, '') AS REQUEST_POSITION");

                obj_str.Append(", ISNULL(REQ_MT_REQUEST.MODIFIED_BY, REQ_MT_REQUEST.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(REQ_MT_REQUEST.MODIFIED_DATE, REQ_MT_REQUEST.CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(", ISNULL(EMP_MT_POSITION.POSITION_NAME_TH,'') AS POSITION_NAME_TH");
                obj_str.Append(", ISNULL(EMP_MT_POSITION.POSITION_NAME_EN,'') AS POSITION_NAME_EN");

                obj_str.Append(" FROM REQ_MT_REQUEST");
                obj_str.Append(" INNER JOIN EMP_MT_POSITION ON EMP_MT_POSITION.POSITION_CODE=REQ_MT_REQUEST.REQUEST_POSITION");

                obj_str.Append(" WHERE 1=1");
                obj_str.Append(" AND REQ_MT_REQUEST.COMPANY_CODE ='" + com + "'");
                obj_str.Append(" AND REQ_MT_REQUEST.REQUEST_STATUS = 0 ");

                

                obj_str.Append(" ORDER BY REQUEST_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTReqRequest();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.request_id = Convert.ToInt32(dr["REQUEST_ID"]);
                    model.request_code = dr["REQUEST_CODE"].ToString();
                    model.request_position = dr["REQUEST_POSITION"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    model.position_name_th = dr["POSITION_NAME_TH"].ToString();
                    model.position_name_en = dr["POSITION_NAME_EN"].ToString();
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "REQSTPST:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTReqRequest> getProjectData(string com)
        {
            List<cls_MTReqRequest> list_model = new List<cls_MTReqRequest>();
            cls_MTReqRequest model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");
                obj_str.Append("REQ_MT_REQUEST.COMPANY_CODE");

                obj_str.Append(", REQ_MT_REQUEST.REQUEST_ID");
                obj_str.Append(", REQ_MT_REQUEST.REQUEST_CODE");
                obj_str.Append(", ISNULL(REQ_MT_REQUEST.REQUEST_PROJECT, '') AS REQUEST_PROJECT");

                obj_str.Append(", ISNULL(REQ_MT_REQUEST.MODIFIED_BY, REQ_MT_REQUEST.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(REQ_MT_REQUEST.MODIFIED_DATE, REQ_MT_REQUEST.CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(", ISNULL(PRO_MT_PROJECT.PROJECT_NAME_TH,'') AS PROJECT_NAME_TH");
                obj_str.Append(", ISNULL(PRO_MT_PROJECT.PROJECT_NAME_EN,'') AS PROJECT_NAME_EN");

                obj_str.Append(" FROM REQ_MT_REQUEST");
                obj_str.Append(" INNER JOIN PRO_MT_PROJECT ON PRO_MT_PROJECT.PROJECT_CODE=REQ_MT_REQUEST.REQUEST_PROJECT");

                obj_str.Append(" WHERE 1=1");
                obj_str.Append(" AND REQ_MT_REQUEST.COMPANY_CODE ='" + com + "'");
                obj_str.Append(" AND REQ_MT_REQUEST.REQUEST_STATUS = 0 ");



                obj_str.Append(" ORDER BY REQUEST_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTReqRequest();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.request_id = Convert.ToInt32(dr["REQUEST_ID"]);
                    model.request_code = dr["REQUEST_CODE"].ToString();
                    model.request_position = dr["REQUEST_PROJECT"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    model.position_name_th = dr["PROJECT_NAME_TH"].ToString();
                    model.position_name_en = dr["PROJECT_NAME_EN"].ToString();
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "REQSTPRO:" + ex.ToString();
            }

            return list_model;
        }

        public string updatestatus(cls_MTReqRequest model)
        {

            string strResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE REQ_MT_REQUEST SET ");

                obj_str.Append(" REQUEST_STATUS=@REQUEST_STATUS ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");

                obj_str.Append(" WHERE REQUEST_ID=@REQUEST_ID ");
                obj_str.Append(" AND COMPANY_CODE=@COMPANY_CODE ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());


                strResult = model.request_id.ToString();

                obj_cmd.Parameters.Add("@REQUEST_STATUS", SqlDbType.Int); obj_cmd.Parameters["@REQUEST_STATUS"].Value = model.request_status;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@REQUEST_ID", SqlDbType.Int); obj_cmd.Parameters["@REQUEST_ID"].Value = strResult;
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

            }
            catch (Exception ex)
            {
                strResult = "";
                Message = "REQST007:" + ex.ToString();
            }

            return strResult;
        }
    }
}
