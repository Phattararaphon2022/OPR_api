using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRPlanschedule
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRPlanschedule() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRPlanschedule> getData(string condition)
        {
            List<cls_TRPlanschedule> list_model = new List<cls_TRPlanschedule>();
            cls_TRPlanschedule model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", PLANSHIFT_CODE");
                obj_str.Append(", PLANSCHEDULE_FROMDATE");
                obj_str.Append(", PLANSCHEDULE_TODATE");
                obj_str.Append(", SHIFT_CODE");
                obj_str.Append(", PLANSCHEDULE_SUN_OFF");
                obj_str.Append(", PLANSCHEDULE_MON_OFF");
                obj_str.Append(", PLANSCHEDULE_TUE_OFF");
                obj_str.Append(", PLANSCHEDULE_WED_OFF");
                obj_str.Append(", PLANSCHEDULE_THU_OFF");
                obj_str.Append(", PLANSCHEDULE_FRI_OFF");
                obj_str.Append(", PLANSCHEDULE_SAT_OFF");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM ATT_TR_PLANSCHEDULE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, PLANSHIFT_CODE, PLANSCHEDULE_FROMDATE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRPlanschedule();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.planshift_code = dr["PLANSHIFT_CODE"].ToString();

                    model.planschedule_fromdate = Convert.ToDateTime(dr["PLANSCHEDULE_FROMDATE"]).ToString("yyyy/MM/dd");
                    model.planschedule_todate = Convert.ToDateTime(dr["PLANSCHEDULE_TODATE"]).ToString("yyyy/MM/dd");

                    model.shift_code = dr["SHIFT_CODE"].ToString();

                    model.planschedule_sun_off = dr["PLANSCHEDULE_SUN_OFF"].ToString();
                    model.planschedule_mon_off = dr["PLANSCHEDULE_MON_OFF"].ToString();
                    model.planschedule_tue_off = dr["PLANSCHEDULE_TUE_OFF"].ToString();
                    model.planschedule_wed_off = dr["PLANSCHEDULE_WED_OFF"].ToString();
                    model.planschedule_thu_off = dr["PLANSCHEDULE_THU_OFF"].ToString();
                    model.planschedule_fri_off = dr["PLANSCHEDULE_FRI_OFF"].ToString();
                    model.planschedule_sat_off = dr["PLANSCHEDULE_SAT_OFF"].ToString();


                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Planschedule.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRPlanschedule> getDataByFillter(string com, string plan)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!plan.Equals(""))
                strCondition += " AND PLANSHIFT_CODE='" + plan + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string plan, DateTime fromdate)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM ATT_TR_PLANSCHEDULE");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PLANSHIFT_CODE='" + plan + "'");
                obj_str.Append(" AND PLANSCHEDULE_FROMDATE='" + fromdate.ToString("MM/dd/yyyy") + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Planschedule.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string plan, DateTime fromdate)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_PLANSCHEDULE");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PLANSHIFT_CODE='" + plan + "'");
                obj_str.Append(" AND PLANSCHEDULE_FROMDATE='" + fromdate.ToString("MM/dd/yyyy") + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Planschedule.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool clear(string com, string plan)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_PLANSCHEDULE");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PLANSHIFT_CODE='" + plan + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Planschedule.clear)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRPlanschedule model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.planshift_code, Convert.ToDateTime(model.planschedule_fromdate)))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO ATT_TR_PLANSCHEDULE");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", PLANSHIFT_CODE ");
                obj_str.Append(", PLANSCHEDULE_FROMDATE ");
                obj_str.Append(", PLANSCHEDULE_TODATE ");
                obj_str.Append(", SHIFT_CODE ");

                obj_str.Append(", PLANSCHEDULE_SUN_OFF ");
                obj_str.Append(", PLANSCHEDULE_MON_OFF ");
                obj_str.Append(", PLANSCHEDULE_TUE_OFF ");
                obj_str.Append(", PLANSCHEDULE_WED_OFF ");
                obj_str.Append(", PLANSCHEDULE_THU_OFF ");
                obj_str.Append(", PLANSCHEDULE_FRI_OFF ");
                obj_str.Append(", PLANSCHEDULE_SAT_OFF ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @PLANSHIFT_CODE ");
                obj_str.Append(", @PLANSCHEDULE_FROMDATE ");
                obj_str.Append(", @PLANSCHEDULE_TODATE ");
                obj_str.Append(", @SHIFT_CODE ");

                obj_str.Append(", @PLANSCHEDULE_SUN_OFF ");
                obj_str.Append(", @PLANSCHEDULE_MON_OFF ");
                obj_str.Append(", @PLANSCHEDULE_TUE_OFF ");
                obj_str.Append(", @PLANSCHEDULE_WED_OFF ");
                obj_str.Append(", @PLANSCHEDULE_THU_OFF ");
                obj_str.Append(", @PLANSCHEDULE_FRI_OFF ");
                obj_str.Append(", @PLANSCHEDULE_SAT_OFF ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@PLANSHIFT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PLANSHIFT_CODE"].Value = model.planshift_code;

                obj_cmd.Parameters.Add("@PLANSCHEDULE_FROMDATE", SqlDbType.DateTime); obj_cmd.Parameters["@PLANSCHEDULE_FROMDATE"].Value = model.planschedule_fromdate;
                obj_cmd.Parameters.Add("@PLANSCHEDULE_TODATE", SqlDbType.DateTime); obj_cmd.Parameters["@PLANSCHEDULE_TODATE"].Value = model.planschedule_todate;

                obj_cmd.Parameters.Add("@SHIFT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CODE"].Value = model.shift_code;

                obj_cmd.Parameters.Add("@PLANSCHEDULE_SUN_OFF", SqlDbType.VarChar); obj_cmd.Parameters["@PLANSCHEDULE_SUN_OFF"].Value = model.planschedule_sun_off;
                obj_cmd.Parameters.Add("@PLANSCHEDULE_MON_OFF", SqlDbType.VarChar); obj_cmd.Parameters["@PLANSCHEDULE_MON_OFF"].Value = model.planschedule_mon_off;
                obj_cmd.Parameters.Add("@PLANSCHEDULE_TUE_OFF", SqlDbType.VarChar); obj_cmd.Parameters["@PLANSCHEDULE_TUE_OFF"].Value = model.planschedule_tue_off;
                obj_cmd.Parameters.Add("@PLANSCHEDULE_WED_OFF", SqlDbType.VarChar); obj_cmd.Parameters["@PLANSCHEDULE_WED_OFF"].Value = model.planschedule_wed_off;
                obj_cmd.Parameters.Add("@PLANSCHEDULE_THU_OFF", SqlDbType.VarChar); obj_cmd.Parameters["@PLANSCHEDULE_THU_OFF"].Value = model.planschedule_thu_off;
                obj_cmd.Parameters.Add("@PLANSCHEDULE_FRI_OFF", SqlDbType.VarChar); obj_cmd.Parameters["@PLANSCHEDULE_FRI_OFF"].Value = model.planschedule_fri_off;
                obj_cmd.Parameters.Add("@PLANSCHEDULE_SAT_OFF", SqlDbType.VarChar); obj_cmd.Parameters["@PLANSCHEDULE_SAT_OFF"].Value = model.planschedule_sat_off;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Planschedule.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public bool update(cls_TRPlanschedule model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE ATT_TR_PLANSCHEDULE SET ");

                obj_str.Append(" PLANSCHEDULE_TODATE=@PLANSCHEDULE_TODATE ");
                obj_str.Append(", SHIFT_CODE=@SHIFT_CODE ");

                obj_str.Append(", PLANSCHEDULE_SUN_OFF=@PLANSCHEDULE_SUN_OFF ");
                obj_str.Append(", PLANSCHEDULE_MON_OFF=@PLANSCHEDULE_MON_OFF ");
                obj_str.Append(", PLANSCHEDULE_TUE_OFF=@PLANSCHEDULE_TUE_OFF ");
                obj_str.Append(", PLANSCHEDULE_WED_OFF=@PLANSCHEDULE_WED_OFF ");
                obj_str.Append(", PLANSCHEDULE_THU_OFF=@PLANSCHEDULE_THU_OFF ");
                obj_str.Append(", PLANSCHEDULE_FRI_OFF=@PLANSCHEDULE_FRI_OFF ");
                obj_str.Append(", PLANSCHEDULE_SAT_OFF=@PLANSCHEDULE_SAT_OFF ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND PLANSHIFT_CODE=@PLANSHIFT_CODE ");
                obj_str.Append(" AND PLANSCHEDULE_FROMDATE=@PLANSCHEDULE_FROMDATE ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                obj_cmd.Parameters.Add("@PLANSCHEDULE_FROMDATE", SqlDbType.DateTime); obj_cmd.Parameters["@PLANSCHEDULE_FROMDATE"].Value =  Convert.ToDateTime(model.planschedule_fromdate);
                obj_cmd.Parameters.Add("@PLANSCHEDULE_TODATE", SqlDbType.DateTime); obj_cmd.Parameters["@PLANSCHEDULE_TODATE"].Value = Convert.ToDateTime(model.planschedule_todate);

                obj_cmd.Parameters.Add("@SHIFT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CODE"].Value = model.shift_code;

                obj_cmd.Parameters.Add("@PLANSCHEDULE_SUN_OFF", SqlDbType.VarChar); obj_cmd.Parameters["@PLANSCHEDULE_SUN_OFF"].Value = model.planschedule_sun_off;
                obj_cmd.Parameters.Add("@PLANSCHEDULE_MON_OFF", SqlDbType.VarChar); obj_cmd.Parameters["@PLANSCHEDULE_MON_OFF"].Value = model.planschedule_mon_off;
                obj_cmd.Parameters.Add("@PLANSCHEDULE_TUE_OFF", SqlDbType.VarChar); obj_cmd.Parameters["@PLANSCHEDULE_TUE_OFF"].Value = model.planschedule_tue_off;
                obj_cmd.Parameters.Add("@PLANSCHEDULE_WED_OFF", SqlDbType.VarChar); obj_cmd.Parameters["@PLANSCHEDULE_WED_OFF"].Value = model.planschedule_wed_off;
                obj_cmd.Parameters.Add("@PLANSCHEDULE_THU_OFF", SqlDbType.VarChar); obj_cmd.Parameters["@PLANSCHEDULE_THU_OFF"].Value = model.planschedule_thu_off;
                obj_cmd.Parameters.Add("@PLANSCHEDULE_FRI_OFF", SqlDbType.VarChar); obj_cmd.Parameters["@PLANSCHEDULE_FRI_OFF"].Value = model.planschedule_fri_off;
                obj_cmd.Parameters.Add("@PLANSCHEDULE_SAT_OFF", SqlDbType.VarChar); obj_cmd.Parameters["@PLANSCHEDULE_SAT_OFF"].Value = model.planschedule_sat_off;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@PLANSHIFT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PLANSHIFT_CODE"].Value = model.planshift_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Planschedule.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
