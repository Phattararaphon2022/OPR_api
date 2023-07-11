using ClassLibrary_BPC.hrfocus.model.SYS.System;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller 
{
    public class cls_ctMTPolround
   {
    string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTPolround() { }

        public string getMessage() { return this.Message.Replace("SYS_MT_POLROUND", "").Replace("cls_ctMTPolround", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTPolround> getData(string condition)
        {
            List<cls_MTPolround> list_model = new List<cls_MTPolround>();
            cls_MTPolround model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();


                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", ISNULL(POLROUND_PF, '') AS POLROUND_PF");
                obj_str.Append(", ISNULL(POLROUND_SSO, '') AS POLROUND_SSO");
                obj_str.Append(", ISNULL(POLROUND_TAX, '') AS POLROUND_TAX");
                obj_str.Append(", ISNULL(POLROUND_WAGE_DAY, '') AS POLROUND_WAGE_DAY");
                obj_str.Append(", ISNULL(POLROUND_WAGE_SUMMARY, '') AS POLROUND_WAGE_SUMMARY");
                obj_str.Append(", ISNULL(POLROUND_OT_DAY, '') AS POLROUND_OT_DAY");
                obj_str.Append(", ISNULL(POLROUND_OT_SUMMARY, '') AS POLROUND_OT_SUMMARY");
                obj_str.Append(", ISNULL(POLROUND_ABSENT, '') AS POLROUND_ABSENT");
                obj_str.Append(", ISNULL(POLROUND_LATE, '') AS POLROUND_LATE");
                obj_str.Append(", ISNULL(POLROUND_LEAVE, '') AS POLROUND_LEAVE");
                obj_str.Append(", ISNULL(POLROUND_NETPAY, '') AS POLROUND_NETPAY");
                obj_str.Append(", ISNULL(POLROUND_TIMELATE, '') AS POLROUND_TIMELATE");
                obj_str.Append(", ISNULL(POLROUND_TIMELEAVE, '') AS POLROUND_TIMELEAVE");
                obj_str.Append(", ISNULL(POLROUND_TIMEOT, '') AS POLROUND_TIMEOT");
                obj_str.Append(", ISNULL(POLROUND_TIMEWORKING, '') AS POLROUND_TIMEWORKING");
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM SYS_MT_POLROUND");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTPolround();
                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.polround_pf = dr["POLROUND_PF"].ToString();
                    model.polround_sso = dr["POLROUND_SSO"].ToString();
                    model.polround_tax = dr["POLROUND_TAX"].ToString();
                    model.polround_wage_day = dr["POLROUND_WAGE_DAY"].ToString();
                    model.polround_wage_summary = dr["POLROUND_WAGE_SUMMARY"].ToString();
                    model.polround_ot_day = dr["POLROUND_OT_DAY"].ToString();
                    model.polround_ot_summary = dr["POLROUND_OT_SUMMARY"].ToString();
                    model.polround_absent = dr["POLROUND_ABSENT"].ToString();
                    model.polround_late = dr["POLROUND_LATE"].ToString();
                    model.polround_leave = dr["POLROUND_LEAVE"].ToString();
                    model.polround_netpay = dr["POLROUND_NETPAY"].ToString();
                    model.polround_timelate = dr["POLROUND_TIMELATE"].ToString();
                    model.polround_timeleave = dr["POLROUND_TIMELEAVE"].ToString();
                    model.polround_timeot = dr["POLROUND_TIMEOT"].ToString();
                    model.polround_timeworking = dr["POLROUND_TIMEWORKING"].ToString();
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "POLR001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTPolround> getDataByFillter(string com)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            return this.getData(strCondition);
        }

        //public int getNextID()
        //{
        //    int intResult = 1;
        //    try
        //    {
        //        System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

        //        obj_str.Append("SELECT ISNULL(HOSPITAL_ID, 1) ");
        //        obj_str.Append(" FROM SYS_MT_HOSPITAL");
        //        obj_str.Append(" ORDER BY HOSPITAL_ID DESC ");

        //        DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

        //        if (dt.Rows.Count > 0)
        //        {
        //            intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Message = "HPT002:" + ex.ToString();
        //    }

        //    return intResult;
        //}

        public bool checkDataOld(string com)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM SYS_MT_POLROUND");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
      
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "POLR003:" + ex.ToString();
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


                obj_str.Append(" DELETE FROM SYS_MT_POLROUND");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "POLR004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_MTPolround model)
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

                obj_str.Append("INSERT INTO SYS_MT_POLROUND");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", POLROUND_PF ");
                obj_str.Append(", POLROUND_SSO ");
                obj_str.Append(", POLROUND_TAX ");
                obj_str.Append(", POLROUND_WAGE_DAY ");
                obj_str.Append(", POLROUND_WAGE_SUMMARY ");
                obj_str.Append(", POLROUND_OT_DAY ");
                obj_str.Append(", POLROUND_OT_SUMMARY ");
                obj_str.Append(", POLROUND_ABSENT ");
                obj_str.Append(", POLROUND_LATE ");
                obj_str.Append(", POLROUND_LEAVE ");
                obj_str.Append(", POLROUND_NETPAY ");
                obj_str.Append(", POLROUND_TIMELATE ");
                obj_str.Append(", POLROUND_TIMELEAVE ");
                obj_str.Append(", POLROUND_TIMEOT ");
                obj_str.Append(", POLROUND_TIMEWORKING ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append(" @COMPANY_CODE ");
                obj_str.Append(", @POLROUND_PF ");
                obj_str.Append(", @POLROUND_SSO ");
                obj_str.Append(", @POLROUND_TAX ");
                obj_str.Append(", @POLROUND_WAGE_DAY ");
                obj_str.Append(", @POLROUND_WAGE_SUMMARY ");
                obj_str.Append(", @POLROUND_OT_DAY ");
                obj_str.Append(", @POLROUND_OT_SUMMARY ");
                obj_str.Append(", @POLROUND_ABSENT ");
                obj_str.Append(", @POLROUND_LATE ");
                obj_str.Append(", @POLROUND_LEAVE ");
                obj_str.Append(", @POLROUND_NETPAY ");
                obj_str.Append(", @POLROUND_TIMELATE ");
                obj_str.Append(", @POLROUND_TIMELEAVE ");
                obj_str.Append(", @POLROUND_TIMEOT ");
                obj_str.Append(", @POLROUND_TIMEWORKING ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");
                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());


                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@POLROUND_PF", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_PF"].Value = model.polround_pf;
                obj_cmd.Parameters.Add("@POLROUND_SSO", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_SSO"].Value = model.polround_sso;
                obj_cmd.Parameters.Add("@POLROUND_TAX", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_TAX"].Value = model.polround_tax;
                obj_cmd.Parameters.Add("@POLROUND_WAGE_DAY", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_WAGE_DAY"].Value = model.polround_wage_day;
                obj_cmd.Parameters.Add("@POLROUND_WAGE_SUMMARY", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_WAGE_SUMMARY"].Value = model.polround_wage_summary;
                obj_cmd.Parameters.Add("@POLROUND_OT_DAY", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_OT_DAY"].Value = model.polround_ot_day;
                obj_cmd.Parameters.Add("@POLROUND_OT_SUMMARY", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_OT_SUMMARY"].Value = model.polround_ot_summary;
                obj_cmd.Parameters.Add("@POLROUND_ABSENT", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_ABSENT"].Value = model.polround_absent;
                obj_cmd.Parameters.Add("@POLROUND_LATE", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_LATE"].Value = model.polround_late;
                obj_cmd.Parameters.Add("@POLROUND_LEAVE", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_LEAVE"].Value = model.polround_leave;
                obj_cmd.Parameters.Add("@POLROUND_NETPAY", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_NETPAY"].Value = model.polround_netpay;
                obj_cmd.Parameters.Add("@POLROUND_TIMELATE", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_TIMELATE"].Value = model.polround_timelate;
                obj_cmd.Parameters.Add("@POLROUND_TIMELEAVE", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_TIMELEAVE"].Value = model.polround_timeleave;
                obj_cmd.Parameters.Add("@POLROUND_TIMEOT", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_TIMEOT"].Value = model.polround_timeot;
                obj_cmd.Parameters.Add("@POLROUND_TIMEWORKING", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_TIMEWORKING"].Value = model.polround_timeworking;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;
     
                obj_cmd.ExecuteNonQuery();
                                
                obj_conn.doClose();
                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "POLR005:" + ex.ToString();
                 
            }

            return blnResult;
        }

        public bool update(cls_MTPolround model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SYS_MT_POLROUND SET ");

                obj_str.Append(" COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(", POLROUND_PF=@POLROUND_PF ");
                obj_str.Append(", POLROUND_SSO=@POLROUND_SSO ");
                obj_str.Append(", POLROUND_TAX=@POLROUND_TAX ");
                obj_str.Append(", POLROUND_WAGE_DAY=@POLROUND_WAGE_DAY ");
                obj_str.Append(", POLROUND_WAGE_SUMMARY=@POLROUND_WAGE_SUMMARY ");
                obj_str.Append(", POLROUND_OT_DAY=@POLROUND_OT_DAY ");
                obj_str.Append(", POLROUND_OT_SUMMARY=@POLROUND_OT_SUMMARY ");
                obj_str.Append(", POLROUND_ABSENT=@POLROUND_ABSENT ");
                obj_str.Append(", POLROUND_LATE=@POLROUND_LATE ");
                obj_str.Append(", POLROUND_LEAVE=@POLROUND_LEAVE ");
                obj_str.Append(", POLROUND_NETPAY=@POLROUND_NETPAY ");
                obj_str.Append(", POLROUND_TIMELATE=@POLROUND_TIMELATE ");
                obj_str.Append(", POLROUND_TIMELEAVE=@POLROUND_TIMELEAVE ");
                obj_str.Append(", POLROUND_TIMEOT=@POLROUND_TIMEOT ");
                obj_str.Append(", POLROUND_TIMEWORKING=@POLROUND_TIMEWORKING ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");
                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@POLROUND_PF", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_PF"].Value = model.polround_pf;
                obj_cmd.Parameters.Add("@POLROUND_SSO", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_SSO"].Value = model.polround_sso;
                obj_cmd.Parameters.Add("@POLROUND_TAX", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_TAX"].Value = model.polround_tax;
                obj_cmd.Parameters.Add("@POLROUND_WAGE_DAY", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_WAGE_DAY"].Value = model.polround_wage_day;
                obj_cmd.Parameters.Add("@POLROUND_WAGE_SUMMARY", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_WAGE_SUMMARY"].Value = model.polround_wage_summary;
                obj_cmd.Parameters.Add("@POLROUND_OT_DAY", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_OT_DAY"].Value = model.polround_ot_day;
                obj_cmd.Parameters.Add("@POLROUND_OT_SUMMARY", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_OT_SUMMARY"].Value = model.polround_ot_summary;
                obj_cmd.Parameters.Add("@POLROUND_ABSENT", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_ABSENT"].Value = model.polround_absent;
                obj_cmd.Parameters.Add("@POLROUND_LATE", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_LATE"].Value = model.polround_late;
                obj_cmd.Parameters.Add("@POLROUND_LEAVE", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_LEAVE"].Value = model.polround_leave;
                obj_cmd.Parameters.Add("@POLROUND_NETPAY", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_NETPAY"].Value = model.polround_netpay;
                obj_cmd.Parameters.Add("@POLROUND_TIMELATE", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_TIMELATE"].Value = model.polround_timelate;
                obj_cmd.Parameters.Add("@POLROUND_TIMELEAVE", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_TIMELEAVE"].Value = model.polround_timeleave;
                obj_cmd.Parameters.Add("@POLROUND_TIMEOT", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_TIMEOT"].Value = model.polround_timeot;
                obj_cmd.Parameters.Add("@POLROUND_TIMEWORKING", SqlDbType.VarChar); obj_cmd.Parameters["@POLROUND_TIMEWORKING"].Value = model.polround_timeworking;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "POLR006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}
