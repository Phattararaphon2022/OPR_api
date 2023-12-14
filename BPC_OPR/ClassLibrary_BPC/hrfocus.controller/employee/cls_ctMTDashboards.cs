using ClassLibrary_BPC.hrfocus.model.employee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTDashboards
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTDashboards() { }
        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }
        private List<cls_MTDashboards> getDataLO(string condition)
        {
            List<cls_MTDashboards> list_model = new List<cls_MTDashboards>();
            cls_MTDashboards model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT");
                obj_str.Append(" COUNT(DISTINCT EMP_MT_WORKER.WORKER_CODE) AS WORKER_CODE");
                obj_str.Append(", SYS_MT_LOCATION.LOCATION_NAME_TH");
                obj_str.Append(", SYS_MT_LOCATION.LOCATION_NAME_EN");

                obj_str.Append(", MAX(EMP_TR_LOCATION.EMPLOCATION_STARTDATE) AS EMPLOCATION_STARTDATE");
                obj_str.Append(", MAX(EMP_TR_LOCATION.EMPLOCATION_ENDDATE) AS EMPLOCATION_ENDDATE");

                obj_str.Append(" FROM EMP_TR_LOCATION");
                obj_str.Append(" INNER JOIN EMP_MT_WORKER ON EMP_TR_LOCATION.WORKER_CODE = EMP_MT_WORKER.WORKER_CODE");
                obj_str.Append(" INNER JOIN SYS_MT_LOCATION ON EMP_TR_LOCATION.LOCATION_CODE = SYS_MT_LOCATION.LOCATION_CODE");

                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" GROUP BY SYS_MT_LOCATION.LOCATION_NAME_TH, SYS_MT_LOCATION.LOCATION_NAME_EN");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTDashboards();

                     model.worker_code = dr["WORKER_CODE"].ToString();

                    model.location_name_th = dr["LOCATION_NAME_TH"].ToString();
                    model.location_name_en = dr["LOCATION_NAME_EN"].ToString();


                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTDashboard.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTDashboards> getDataByFillter(string code)
        {
            string strCondition = "";

            if (!code.Equals(""))
                strCondition += " AND WORKER_CODE='" + code + "'";



            return this.getDataLO(strCondition);
        }

         private List<cls_MTDashboards> getDataType(string condition)
        {
            List<cls_MTDashboards> list_model = new List<cls_MTDashboards>();
            cls_MTDashboards model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT");
                 obj_str.Append("  EMP_MT_WORKER.WORKER_CODE");
                obj_str.Append(", EMP_MT_WORKER.WORKER_TYPE");

                obj_str.Append(", EMP_MT_TYPE.TYPE_NAME_TH");
                obj_str.Append(",  EMP_MT_TYPE.TYPE_NAME_EN");
                obj_str.Append(" FROM EMP_MT_WORKER");
                obj_str.Append(" INNER JOIN EMP_MT_TYPE ON EMP_MT_WORKER.WORKER_TYPE = EMP_MT_TYPE.TYPE_CODE");

                obj_str.Append(" WHERE 1=1");
                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);
                //obj_str.Append("GROUP BY EMP_MT_TYPE.TYPE_CODE, EMP_MT_TYPE.TYPE_NAME_TH, EMP_MT_TYPE.TYPE_NAME_EN");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTDashboards();

                     model.worker_code = dr["WORKER_CODE"].ToString();
                    model.worker_type = dr["WORKER_TYPE"].ToString();


                    model.type_name_th = dr["TYPE_NAME_TH"].ToString();
                    model.type_name_en = dr["TYPE_NAME_EN"].ToString();


                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTDashboard.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTDashboards> getDataByFillterType(string code)
        {
            string strCondition = "";

            if (!code.Equals(""))

                strCondition += " AND WORKER_CODE='" + code;
 


            return this.getDataType(strCondition);
        }


        /// /////////////
 
        private List<cls_MTDashboards> getDataGender(string condition)
        {
            List<cls_MTDashboards> list_model = new List<cls_MTDashboards>();
            cls_MTDashboards model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("SELECT");

                obj_str.Append(" COUNT(WORKER_CODE)as WORKER_CODE");
                obj_str.Append(", EMP_MT_WORKER.COMPANY_CODE");
                obj_str.Append(", EMP_MT_WORKER.WORKER_GENDER");
                obj_str.Append(", EMP_MT_WORKER.WORKER_RESIGNSTATUS");

                
                  
                 
                obj_str.Append(", (CASE ISNULL(WORKER_GENDER, '') WHEN 'M' THEN 'Male' WHEN 'F' THEN 'Female' END) AS WORKER_GENDER_EN");
                obj_str.Append(", (CASE ISNULL(WORKER_GENDER, '') WHEN 'M' THEN 'เพศชาย' WHEN 'F' THEN 'เพศหญิง' END) AS WORKER_GENDER_TH");
                obj_str.Append(" FROM EMP_MT_WORKER");

                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" GROUP BY EMP_MT_WORKER.WORKER_GENDER,EMP_MT_WORKER.COMPANY_CODE,EMP_MT_WORKER.WORKER_CODE,EMP_MT_WORKER.WORKER_RESIGNSTATUS");


                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTDashboards();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    
                    model.worker_gender_en = dr["WORKER_GENDER_EN"].ToString();
                    model.worker_gender_th = dr["WORKER_GENDER_TH"].ToString();
                    model.worker_gender = dr["WORKER_GENDER"].ToString();
                    model.worker_resignstatus = dr["WORKER_RESIGNSTATUS"].ToString();


                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTDashboard.getData)" + ex.ToString();
            }

            return list_model;
        }
        public List<cls_MTDashboards> getDataByFillterDataGender(string com, string code, string status)
        {
            string strCondition = "";

            if (!com.Equals(""))

                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!code.Equals(""))

                strCondition += " AND WORKER_CODE='" + code + "'";
            if (!status.Equals(""))

                strCondition += " AND WORKER_RESIGNSTATUS='" + status + "'";

            return this.getDataGender(strCondition);
        }
        ////
        /// /////////////

        private List<cls_MTDashboards> getDataEmpAge(string condition)
        {
            List<cls_MTDashboards> list_model = new List<cls_MTDashboards>();
            cls_MTDashboards model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
               
                obj_str.Append("SELECT");
                
                obj_str.Append(" COUNT(WORKER_CODE)as WORKER_CODE");
                obj_str.Append(" ,COMPANY_CODE");
                obj_str.Append(" ,WORKER_RESIGNSTATUS");

                
                obj_str.Append(", (case when (DATEDIFF(YY,WORKER_BIRTHDATE,GETDATE())) between 18 and 30 then '18-30'");
                obj_str.Append(" WHEN (DATEDIFF(YY,WORKER_BIRTHDATE,GETDATE())) between 31 and 40 then '31-40'");
                obj_str.Append(" WHEN (DATEDIFF(YY,WORKER_BIRTHDATE,GETDATE())) between 41 and 55 then '41-55'");
                obj_str.Append(" ELSE '55+' END)AS AGE_CODE");
                obj_str.Append(" FROM EMP_MT_WORKER");

                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" GROUP BY  WORKER_CODE, COMPANY_CODE,WORKER_RESIGNSTATUS, (case when (DATEDIFF(YY,WORKER_BIRTHDATE,GETDATE())) between 18 and 30 then '18-30'");
                obj_str.Append(" WHEN (DATEDIFF(YY,WORKER_BIRTHDATE,GETDATE())) between 31 and 40 then '31-40'");
                obj_str.Append(" WHEN (DATEDIFF(YY,WORKER_BIRTHDATE,GETDATE())) between 41 and 55 then '41-55' else '55+' END)");


                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTDashboards();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.age_code = dr["AGE_CODE"].ToString();
                    model.worker_resignstatus = dr["WORKER_RESIGNSTATUS"].ToString();

 
 

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTDashboard.getData)" + ex.ToString();
            }

            return list_model;
        }
        public List<cls_MTDashboards> getDataEmpWorkAgeByFillter(string com, string code, string status)
        {
            string strCondition = "";

            if (!com.Equals(""))

                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!code.Equals(""))

                strCondition += " AND WORKER_CODE='" + code + "'";
            if (!status.Equals(""))

                strCondition += " AND WORKER_RESIGNSTATUS='" + status + "'";

            return this.getDataEmpAge(strCondition);
        }
        ///

        /// /////////////
        //อายุงาน Length of Employment
        private List<cls_MTDashboards> getDataworkAge(string condition)
        {
            List<cls_MTDashboards> list_model = new List<cls_MTDashboards>();
            cls_MTDashboards model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT");

                obj_str.Append(" COUNT(WORKER_CODE)as WORKER_CODE");
                obj_str.Append(" ,COMPANY_CODE");
                obj_str.Append(" ,WORKER_RESIGNSTATUS");


                obj_str.Append(", (case when (DATEDIFF(YY,WORKER_HIREDATE,GETDATE())) between 0 and 2 then '0-2'");
                obj_str.Append(" WHEN (DATEDIFF(YY,WORKER_HIREDATE,GETDATE())) between 3 and 5 then '3-5'");
                obj_str.Append(" WHEN (DATEDIFF(YY,WORKER_HIREDATE,GETDATE())) between 6 and 10 then '6-10'");
 
                obj_str.Append(" WHEN (DATEDIFF(YY,WORKER_HIREDATE,GETDATE())) between 11 and 15 then '11-15'");
                obj_str.Append(" ELSE '16+' END)AS WORK_AGE");
                obj_str.Append(" FROM EMP_MT_WORKER");

                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" GROUP BY  WORKER_CODE, COMPANY_CODE,WORKER_RESIGNSTATUS, (case when (DATEDIFF(YY,WORKER_HIREDATE,GETDATE())) BETWEEN 0 AND 2 THEN '0-2'");
                obj_str.Append(" WHEN (DATEDIFF(YY,WORKER_HIREDATE,GETDATE())) BETWEEN 3 AND 5 THEN '3-5'");
                obj_str.Append(" WHEN (DATEDIFF(YY,WORKER_HIREDATE,GETDATE()))BETWEEN 6 AND 10 THEN '6-10'");
                obj_str.Append(" WHEN (DATEDIFF(YY,WORKER_HIREDATE,GETDATE())) BETWEEN 11 AND 15 THEN '11-15' else '16+' END)");


                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTDashboards();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.work_age = dr["WORK_AGE"].ToString();
                    model.worker_resignstatus = dr["WORKER_RESIGNSTATUS"].ToString();




                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTDashboard.getData)" + ex.ToString();
            }

            return list_model;
        }
        public List<cls_MTDashboards> getDataWorkAgeByFillter(string com, string code, string status)
        {
            string strCondition = "";

            if (!com.Equals(""))

                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!code.Equals(""))

                strCondition += " AND WORKER_CODE='" + code + "'";
            if (!status.Equals(""))

                strCondition += " AND WORKER_RESIGNSTATUS='" + status + "'";

            return this.getDataworkAge(strCondition);
        }
        ///
        //ตำแหน่งงาน
        private List<cls_MTDashboards> getDataPosition(string language, string condition)
        {
            List<cls_MTDashboards> list_model = new List<cls_MTDashboards>();
            cls_MTDashboards model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COUNT( EMP_MT_WORKER.WORKER_CODE)as WORKER_CODE");
                obj_str.Append(", EMPPOSITION_POSITION");

                if (language.Equals("EN"))
                {

                    obj_str.Append(", EMP_MT_POSITION.POSITION_NAME_EN AS POSITION_NAME ");
                }
                else
                {
                    obj_str.Append(", EMP_MT_POSITION.POSITION_NAME_TH AS POSITION_NAME ");
                }


 
                obj_str.Append(", EMP_MT_WORKER.WORKER_RESIGNSTATUS");
                obj_str.Append(", EMP_TR_POSITION.COMPANY_CODE");

                obj_str.Append(" FROM EMP_TR_POSITION");

                obj_str.Append(" INNER JOIN EMP_MT_POSITION ON EMP_TR_POSITION.COMPANY_CODE=EMP_MT_POSITION.COMPANY_CODE");
                obj_str.Append(" AND EMP_TR_POSITION.EMPPOSITION_POSITION=EMP_MT_POSITION.POSITION_CODE");
                obj_str.Append(" INNER JOIN EMP_MT_WORKER ON EMP_TR_POSITION.COMPANY_CODE=EMP_MT_WORKER.COMPANY_CODE");
                obj_str.Append(" AND EMP_TR_POSITION.WORKER_CODE=EMP_MT_WORKER.WORKER_CODE");

                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" GROUP BY EMP_MT_WORKER.WORKER_CODE, EMP_TR_POSITION.COMPANY_CODE,  EMP_MT_WORKER.WORKER_RESIGNSTATUS, EMPPOSITION_POSITION, EMP_MT_POSITION.POSITION_NAME_TH, EMP_MT_POSITION.POSITION_NAME_EN");


                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTDashboards();

                    model.empposition_position = dr["EMPPOSITION_POSITION"].ToString();
                    //model.position_name_en = dr["POSITION_NAME_EN"].ToString();
                    model.position_name  = dr["POSITION_NAME"].ToString();
                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.worker_resignstatus = dr["WORKER_RESIGNSTATUS"].ToString();
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTDashboard.getData)" + ex.ToString();
            }

            return list_model;
        }
        public List<cls_MTDashboards> getDataPositionByFillter(string language, string com, string code, string status)
        {
            string strCondition = "";

            if (!com.Equals(""))

                strCondition += " AND EMP_TR_POSITION.COMPANY_CODE='" + com + "'";

            if (!code.Equals(""))

                strCondition += " AND WORKER_CODE='" + code + "'";
            if (!status.Equals(""))

                strCondition += " AND WORKER_RESIGNSTATUS='" + status + "'";

            return this.getDataPosition(language, strCondition);
        }
        ///

    }

}
