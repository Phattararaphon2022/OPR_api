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
    public class cls_ctTRProjobshift
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRProjobshift() { }

        public string getMessage() { return this.Message.Replace("PRO_TR_PROJOBSHIFT", "").Replace("cls_ctTRProjobshift", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRProjobshift> getData(string condition)
        {
            List<cls_TRProjobshift> list_model = new List<cls_TRProjobshift>();
            cls_TRProjobshift model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PROJOBSHIFT_ID");
                obj_str.Append(", SHIFT_CODE");
                obj_str.Append(", PROJOBSHIFT_EMP");
             
                obj_str.Append(", PROJOBSHIFT_SUN");
                obj_str.Append(", PROJOBSHIFT_MON");
                obj_str.Append(", PROJOBSHIFT_TUE");
                obj_str.Append(", PROJOBSHIFT_WED");
                obj_str.Append(", PROJOBSHIFT_THU");
                obj_str.Append(", PROJOBSHIFT_FRI");
                obj_str.Append(", PROJOBSHIFT_SAT");
                obj_str.Append(", PROJOBSHIFT_PH");

                

                obj_str.Append(", PROJOBSHIFT_WORKING");
                obj_str.Append(", PROJOBSHIFT_HRSPERDAY");
                obj_str.Append(", PROJOBSHIFT_HRSOT");

                obj_str.Append(", PROJOB_CODE");
                obj_str.Append(", PROJECT_CODE");

                obj_str.Append(", VERSION");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");    

                obj_str.Append(" FROM PRO_TR_PROJOBSHIFT");
                obj_str.Append(" WHERE 1=1");
                
                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY SHIFT_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRProjobshift();

                    model.projobshift_id = Convert.ToInt32(dr["PROJOBSHIFT_ID"]);
                    model.shift_code = dr["SHIFT_CODE"].ToString();

                    model.projobshift_emp = Convert.ToInt32(dr["PROJOBSHIFT_EMP"]);

                    model.projobshift_sun = Convert.ToBoolean(dr["PROJOBSHIFT_SUN"]);
                    model.projobshift_mon = Convert.ToBoolean(dr["PROJOBSHIFT_MON"]);
                    model.projobshift_tue = Convert.ToBoolean(dr["PROJOBSHIFT_TUE"]);
                    model.projobshift_wed = Convert.ToBoolean(dr["PROJOBSHIFT_WED"]);
                    model.projobshift_thu = Convert.ToBoolean(dr["PROJOBSHIFT_THU"]);
                    model.projobshift_fri = Convert.ToBoolean(dr["PROJOBSHIFT_FRI"]);
                    model.projobshift_sat = Convert.ToBoolean(dr["PROJOBSHIFT_SAT"]);
                    model.projobshift_ph = Convert.ToBoolean(dr["PROJOBSHIFT_PH"]);

                    
                    model.projobshift_working = Convert.ToInt32(dr["PROJOBSHIFT_WORKING"]);
                    model.projobshift_hrsperday = Convert.ToDouble(dr["PROJOBSHIFT_HRSPERDAY"]);
                    model.projobshift_hrsot = Convert.ToDouble(dr["PROJOBSHIFT_HRSOT"]);

                    model.projob_code = dr["PROJOB_CODE"].ToString(); 
                    model.project_code = dr["PROJECT_CODE"].ToString();

                    model.version = dr["VERSION"].ToString(); 

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                                                                                            
                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "BNK001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRProjobshift> getDataByFillter(string project, string job, string version)
        {
            string strCondition = "";

            if (!project.Equals(""))
                strCondition += " AND PROJECT_CODE='" + project + "'";

            if (!job.Equals(""))
                strCondition += " AND PROJOB_CODE='" + job + "'";

            if (!version.Equals(""))
                strCondition += " AND VERSION='" + version + "'";
            
            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(PROJOBSHIFT_ID, 1) ");
                obj_str.Append(" FROM PRO_TR_PROJOBSHIFT");
                obj_str.Append(" ORDER BY PROJOBSHIFT_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "BNK002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string project, string job, string code, string version)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT SHIFT_CODE");
                obj_str.Append(" FROM PRO_TR_PROJOBSHIFT");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROJOB_CODE='" + job + "'");
                obj_str.Append(" AND SHIFT_CODE='" + code + "'");

                obj_str.Append(" AND VERSION='" + version + "'");
      
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "BNK003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string project, string job, string version)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_TR_PROJOBSHIFT");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROJOB_CODE='" + job + "'");
                obj_str.Append(" AND VERSION='" + version + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BNK004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string project, string job, string shift, string version)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_TR_PROJOBSHIFT");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROJOB_CODE='" + job + "'");
                obj_str.Append(" AND SHIFT_CODE='" + shift + "'");
                obj_str.Append(" AND VERSION='" + version + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BNK004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRProjobshift model)
        {
            bool blnResult = false;
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.project_code, model.projob_code, model.shift_code, model.version))
                {
                    return this.update(model);               
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                          
                obj_str.Append("INSERT INTO PRO_TR_PROJOBSHIFT");
                obj_str.Append(" (");
                obj_str.Append("PROJOBSHIFT_ID ");
                obj_str.Append(", SHIFT_CODE ");
               
                obj_str.Append(", PROJOBSHIFT_SUN ");
                obj_str.Append(", PROJOBSHIFT_MON ");
                obj_str.Append(", PROJOBSHIFT_TUE ");
                obj_str.Append(", PROJOBSHIFT_WED ");
                obj_str.Append(", PROJOBSHIFT_THU ");
                obj_str.Append(", PROJOBSHIFT_FRI ");
                obj_str.Append(", PROJOBSHIFT_SAT ");
                obj_str.Append(", PROJOBSHIFT_PH ");


                obj_str.Append(", PROJOBSHIFT_EMP ");
                obj_str.Append(", PROJOBSHIFT_WORKING ");
                obj_str.Append(", PROJOBSHIFT_HRSPERDAY ");
                obj_str.Append(", PROJOBSHIFT_HRSOT ");

                obj_str.Append(", PROJOB_CODE ");
                obj_str.Append(", PROJECT_CODE ");

                obj_str.Append(", VERSION ");     

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");          
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@PROJOBSHIFT_ID ");
                obj_str.Append(", @SHIFT_CODE ");

                obj_str.Append(", @PROJOBSHIFT_SUN ");
                obj_str.Append(", @PROJOBSHIFT_MON ");
                obj_str.Append(", @PROJOBSHIFT_TUE ");
                obj_str.Append(", @PROJOBSHIFT_WED ");
                obj_str.Append(", @PROJOBSHIFT_THU ");
                obj_str.Append(", @PROJOBSHIFT_FRI ");
                obj_str.Append(", @PROJOBSHIFT_SAT ");
                obj_str.Append(", @PROJOBSHIFT_PH ");

                
                obj_str.Append(", @PROJOBSHIFT_EMP ");
                obj_str.Append(", @PROJOBSHIFT_WORKING ");
                obj_str.Append(", @PROJOBSHIFT_HRSPERDAY ");
                obj_str.Append(", @PROJOBSHIFT_HRSOT ");

                obj_str.Append(", @PROJOB_CODE ");
                obj_str.Append(", @PROJECT_CODE ");

                obj_str.Append(", @VERSION ");     

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");
                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.projobshift_id = this.getNextID();

                obj_cmd.Parameters.Add("@PROJOBSHIFT_ID", SqlDbType.Int); obj_cmd.Parameters["@PROJOBSHIFT_ID"].Value = model.projobshift_id;
                obj_cmd.Parameters.Add("@SHIFT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CODE"].Value = model.shift_code;

                obj_cmd.Parameters.Add("@PROJOBSHIFT_SUN", SqlDbType.Bit); obj_cmd.Parameters["@PROJOBSHIFT_SUN"].Value = model.projobshift_sun;
                obj_cmd.Parameters.Add("@PROJOBSHIFT_MON", SqlDbType.Bit); obj_cmd.Parameters["@PROJOBSHIFT_MON"].Value = model.projobshift_mon;
                obj_cmd.Parameters.Add("@PROJOBSHIFT_TUE", SqlDbType.Bit); obj_cmd.Parameters["@PROJOBSHIFT_TUE"].Value = model.projobshift_tue;
                obj_cmd.Parameters.Add("@PROJOBSHIFT_WED", SqlDbType.Bit); obj_cmd.Parameters["@PROJOBSHIFT_WED"].Value = model.projobshift_wed;
                obj_cmd.Parameters.Add("@PROJOBSHIFT_THU", SqlDbType.Bit); obj_cmd.Parameters["@PROJOBSHIFT_THU"].Value = model.projobshift_thu;
                obj_cmd.Parameters.Add("@PROJOBSHIFT_FRI", SqlDbType.Bit); obj_cmd.Parameters["@PROJOBSHIFT_FRI"].Value = model.projobshift_fri;
                obj_cmd.Parameters.Add("@PROJOBSHIFT_SAT", SqlDbType.Bit); obj_cmd.Parameters["@PROJOBSHIFT_SAT"].Value = model.projobshift_sat;
                obj_cmd.Parameters.Add("@PROJOBSHIFT_PH", SqlDbType.Bit); obj_cmd.Parameters["@PROJOBSHIFT_PH"].Value = model.projobshift_ph;

                  
                obj_cmd.Parameters.Add("@PROJOBSHIFT_EMP", SqlDbType.Int); obj_cmd.Parameters["@PROJOBSHIFT_EMP"].Value = model.projobshift_emp;
                obj_cmd.Parameters.Add("@PROJOBSHIFT_WORKING", SqlDbType.Int); obj_cmd.Parameters["@PROJOBSHIFT_WORKING"].Value = model.projobshift_working;
                obj_cmd.Parameters.Add("@PROJOBSHIFT_HRSPERDAY", SqlDbType.Decimal); obj_cmd.Parameters["@PROJOBSHIFT_HRSPERDAY"].Value = model.projobshift_hrsperday;
                obj_cmd.Parameters.Add("@PROJOBSHIFT_HRSOT", SqlDbType.Decimal); obj_cmd.Parameters["@PROJOBSHIFT_HRSOT"].Value = model.projobshift_hrsot;

                obj_cmd.Parameters.Add("@PROJOB_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOB_CODE"].Value = model.projob_code;                
                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;

                obj_cmd.Parameters.Add("@VERSION", SqlDbType.VarChar); obj_cmd.Parameters["@VERSION"].Value = model.version;
                
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                                     
                obj_cmd.ExecuteNonQuery();
                                
                obj_conn.doClose();
                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "BNK005:" + ex.ToString();                
            }

            return blnResult;
        }

        public bool update(cls_TRProjobshift model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PRO_TR_PROJOBSHIFT SET ");
              
                obj_str.Append(" SHIFT_CODE=@SHIFT_CODE ");
                obj_str.Append(", PROJOBSHIFT_SUN=@PROJOBSHIFT_SUN ");
                obj_str.Append(", PROJOBSHIFT_MON=@PROJOBSHIFT_MON ");
                obj_str.Append(", PROJOBSHIFT_TUE=@PROJOBSHIFT_TUE ");
                obj_str.Append(", PROJOBSHIFT_WED=@PROJOBSHIFT_WED ");
                obj_str.Append(", PROJOBSHIFT_THU=@PROJOBSHIFT_THU ");
                obj_str.Append(", PROJOBSHIFT_FRI=@PROJOBSHIFT_FRI ");
                obj_str.Append(", PROJOBSHIFT_SAT=@PROJOBSHIFT_SAT ");
                obj_str.Append(", PROJOBSHIFT_PH=@PROJOBSHIFT_PH ");

                
                obj_str.Append(", PROJOBSHIFT_EMP=@PROJOBSHIFT_EMP ");
                obj_str.Append(", PROJOBSHIFT_WORKING=@PROJOBSHIFT_WORKING ");
                obj_str.Append(", PROJOBSHIFT_HRSPERDAY=@PROJOBSHIFT_HRSPERDAY ");
                obj_str.Append(", PROJOBSHIFT_HRSOT=@PROJOBSHIFT_HRSOT ");
                
               
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE PROJOBSHIFT_ID=@PROJOBSHIFT_ID ");            

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@SHIFT_CODE", SqlDbType.Bit); obj_cmd.Parameters["@SHIFT_CODE"].Value = model.shift_code;
                obj_cmd.Parameters.Add("@PROJOBSHIFT_SUN", SqlDbType.Bit); obj_cmd.Parameters["@PROJOBSHIFT_SUN"].Value = model.projobshift_sun;
                obj_cmd.Parameters.Add("@PROJOBSHIFT_MON", SqlDbType.Bit); obj_cmd.Parameters["@PROJOBSHIFT_MON"].Value = model.projobshift_mon;
                obj_cmd.Parameters.Add("@PROJOBSHIFT_TUE", SqlDbType.Bit); obj_cmd.Parameters["@PROJOBSHIFT_TUE"].Value = model.projobshift_tue;
                obj_cmd.Parameters.Add("@PROJOBSHIFT_WED", SqlDbType.Bit); obj_cmd.Parameters["@PROJOBSHIFT_WED"].Value = model.projobshift_wed;
                obj_cmd.Parameters.Add("@PROJOBSHIFT_THU", SqlDbType.Bit); obj_cmd.Parameters["@PROJOBSHIFT_THU"].Value = model.projobshift_thu;
                obj_cmd.Parameters.Add("@PROJOBSHIFT_FRI", SqlDbType.Bit); obj_cmd.Parameters["@PROJOBSHIFT_FRI"].Value = model.projobshift_fri;
                obj_cmd.Parameters.Add("@PROJOBSHIFT_SAT", SqlDbType.Bit); obj_cmd.Parameters["@PROJOBSHIFT_SAT"].Value = model.projobshift_ph;

                obj_cmd.Parameters.Add("@PROJOBSHIFT_PH", SqlDbType.Bit); obj_cmd.Parameters["@PROJOBSHIFT_PH"].Value = model.projobshift_ph;

                obj_cmd.Parameters.Add("@PROJOBSHIFT_EMP", SqlDbType.Int); obj_cmd.Parameters["@PROJOBSHIFT_EMP"].Value = model.projobshift_emp;
                obj_cmd.Parameters.Add("@PROJOBSHIFT_WORKING", SqlDbType.Int); obj_cmd.Parameters["@PROJOBSHIFT_WORKING"].Value = model.projobshift_working;
                obj_cmd.Parameters.Add("@PROJOBSHIFT_HRSPERDAY", SqlDbType.Decimal); obj_cmd.Parameters["@PROJOBSHIFT_HRSPERDAY"].Value = model.projobshift_hrsperday;
                obj_cmd.Parameters.Add("@PROJOBSHIFT_HRSOT", SqlDbType.Decimal); obj_cmd.Parameters["@PROJOBSHIFT_HRSOT"].Value = model.projobshift_hrsot;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@PROJOBSHIFT_ID", SqlDbType.Int); obj_cmd.Parameters["@PROJOBSHIFT_ID"].Value = model.projobshift_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "BNK006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}
