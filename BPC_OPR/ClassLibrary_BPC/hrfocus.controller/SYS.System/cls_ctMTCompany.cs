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
     public class cls_ctMTCompany
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTCompany() { }

        public string getMessage() { return this.Message.Replace("SYS_MT_COMPANYS", "").Replace("cls_ctMTCompany", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }
        private List<cls_MTCompany> getData(string condition)
        {
            List<cls_MTCompany> list_model = new List<cls_MTCompany>();
            cls_MTCompany model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_ID");
                obj_str.Append(", COMPANY_CODE");
                obj_str.Append(", ISNULL(COMPANY_INITIALS, '') AS COMPANY_INITIALS");

                obj_str.Append(", ISNULL(COMPANY_NAME_TH, '') AS COMPANY_NAME_TH");
                obj_str.Append(", ISNULL(COMPANY_NAME_EN, '') AS COMPANY_NAME_EN");

                obj_str.Append(", ISNULL(SSO_TAX_NO, '') AS SSO_TAX_NO");
                obj_str.Append(", ISNULL(CITIZEN_NO, '') AS CITIZEN_NO");
                obj_str.Append(", ISNULL(PROVIDENT_FUND_NO, '') AS PROVIDENT_FUND_NO");

                obj_str.Append(", ISNULL(HRS_PERDAY, 0) AS HRS_PERDAY");

                obj_str.Append(", ISNULL(SSO_COM_RATE, 0) AS SSO_COM_RATE");
                obj_str.Append(", ISNULL(SSO_EMP_RATE, 0) AS SSO_EMP_RATE");

                obj_str.Append(", ISNULL(SSO_SECURITY_NO, '') AS SSO_SECURITY_NO");
                obj_str.Append(", ISNULL(SSO_BRANCH_NO, '') AS SSO_BRANCH_NO");

                obj_str.Append(", ISNULL(SSO_MIN_WAGE, 0) AS SSO_MIN_WAGE");
                obj_str.Append(", ISNULL(SSO_MAX_WAGE, 0) AS SSO_MAX_WAGE");
                obj_str.Append(", ISNULL(SSO_MIN_AGE, 0) AS SSO_MIN_AGE");
                obj_str.Append(", ISNULL(SSO_MAX_AGE, 0) AS SSO_MAX_AGE");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM SYS_MT_COMPANYS");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTCompany();

                    model.company_id = Convert.ToInt32(dr["COMPANY_ID"]);
                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.company_initials = dr["COMPANY_INITIALS"].ToString();
                    model.company_name_th = dr["COMPANY_NAME_TH"].ToString();
                    model.company_name_en = dr["COMPANY_NAME_EN"].ToString();

                    model.sso_tax_no = dr["SSO_TAX_NO"].ToString();
                    model.citizen_no = dr["CITIZEN_NO"].ToString();
                    model.provident_fund_no = dr["PROVIDENT_FUND_NO"].ToString();

                    model.hrs_perday = Convert.ToDouble(dr["HRS_PERDAY"]);
                    model.sso_com_rate = Convert.ToDouble(dr["SSO_COM_RATE"]);
                    model.sso_emp_rate = Convert.ToDouble(dr["SSO_EMP_RATE"]);

                    model.sso_security_no = dr["SSO_SECURITY_NO"].ToString();
                    model.sso_branch_no = dr["SSO_BRANCH_NO"].ToString();

                    model.sso_min_wage = Convert.ToDouble(dr["SSO_MIN_WAGE"]);
                    model.sso_max_wage = Convert.ToDouble(dr["SSO_MAX_WAGE"]);

                    model.sso_min_age = Convert.ToInt32(dr["SSO_MIN_AGE"]);
                    model.sso_max_age = Convert.ToInt32(dr["SSO_MAX_AGE"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Company.getData)" + ex.ToString();
            }

            return list_model;
        }

        private DataTable doGetTable(string p)
        {
            throw new NotImplementedException();
        }

        public List<cls_MTCompany> getDataByFillter(string id, string code)
        {
            string strCondition = "";

            if (!id.Equals(""))
                strCondition += " AND COMPANY_ID='" + id + "'";

            if (!code.Equals(""))
                strCondition += " AND COMPANY_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_ID");
                obj_str.Append(" FROM SYS_MT_COMPANYS");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Company.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(COMPANY_ID) ");
                obj_str.Append(" FROM SYS_MT_COMPANYS");    

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Company.getNextID)" + ex.ToString();
            }

            return intResult;
        }

        public bool delete(string id)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SYS_MT_COMPANYS");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_ID='" + id + "'");
                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Company.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTCompany model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO SYS_MT_COMPANYS");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_ID ");
                obj_str.Append(", COMPANY_CODE ");
                obj_str.Append(", COMPANY_INITIALS ");
                obj_str.Append(", COMPANY_NAME_TH ");
                obj_str.Append(", COMPANY_NAME_EN ");

                obj_str.Append(", SSO_TAX_NO ");
                obj_str.Append(", CITIZEN_NO ");
                obj_str.Append(", PROVIDENT_FUND_NO ");

                obj_str.Append(", HRS_PERDAY ");
                obj_str.Append(", SSO_COM_RATE ");
                obj_str.Append(", SSO_EMP_RATE ");

                obj_str.Append(", SSO_SECURITY_NO ");
                obj_str.Append(", SSO_BRANCH_NO ");

                obj_str.Append(", SSO_MIN_WAGE ");
                obj_str.Append(", SSO_MAX_WAGE ");
                obj_str.Append(", SSO_MIN_AGE ");
                obj_str.Append(", SSO_MAX_AGE ");


                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_ID ");
                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @COMPANY_INITIALS ");
                obj_str.Append(", @COMPANY_NAME_TH ");
                obj_str.Append(", @COMPANY_NAME_EN ");

                obj_str.Append(", @SSO_TAX_NO ");
                obj_str.Append(", @CITIZEN_NO ");
                obj_str.Append(", @PROVIDENT_FUND_NO ");

                obj_str.Append(", @HRS_PERDAY ");
                obj_str.Append(", @SSO_COM_RATE ");
                obj_str.Append(", @SSO_EMP_RATE ");

                obj_str.Append(", @SSO_SECURITY_NO ");
                obj_str.Append(", @SSO_BRANCH_NO ");

                obj_str.Append(", @SSO_MIN_WAGE ");
                obj_str.Append(", @SSO_MAX_WAGE ");
                obj_str.Append(", @SSO_MIN_AGE ");
                obj_str.Append(", @SSO_MAX_AGE ");


                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_ID", SqlDbType.Int); obj_cmd.Parameters["@COMPANY_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@COMPANY_INITIALS", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_INITIALS"].Value = model.company_initials;

                obj_cmd.Parameters.Add("@COMPANY_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_NAME_TH"].Value = model.company_name_th;
                obj_cmd.Parameters.Add("@COMPANY_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_NAME_EN"].Value = model.company_name_en;

                obj_cmd.Parameters.Add("@SSO_TAX_NO", SqlDbType.VarChar); obj_cmd.Parameters["@SSO_TAX_NO"].Value = model.sso_tax_no;
                obj_cmd.Parameters.Add("@CITIZEN_NO", SqlDbType.VarChar); obj_cmd.Parameters["@CITIZEN_NO"].Value = model.citizen_no;
                obj_cmd.Parameters.Add("@PROVIDENT_FUND_NO", SqlDbType.VarChar); obj_cmd.Parameters["@PROVIDENT_FUND_NO"].Value = model.provident_fund_no;



                obj_cmd.Parameters.Add("@HRS_PERDAY", SqlDbType.Decimal); obj_cmd.Parameters["@HRS_PERDAY"].Value = model.hrs_perday;
                obj_cmd.Parameters.Add("@SSO_COM_RATE", SqlDbType.Decimal); obj_cmd.Parameters["@SSO_COM_RATE"].Value = model.sso_com_rate;
                obj_cmd.Parameters.Add("@SSO_EMP_RATE", SqlDbType.Decimal); obj_cmd.Parameters["@SSO_EMP_RATE"].Value = model.sso_emp_rate;

                obj_cmd.Parameters.Add("@SSO_SECURITY_NO", SqlDbType.VarChar); obj_cmd.Parameters["@SSO_SECURITY_NO"].Value = model.sso_security_no;
                obj_cmd.Parameters.Add("@SSO_BRANCH_NO", SqlDbType.VarChar); obj_cmd.Parameters["@SSO_BRANCH_NO"].Value = model.sso_branch_no;

                
                
                obj_cmd.Parameters.Add("@SSO_MIN_WAGE", SqlDbType.Decimal); obj_cmd.Parameters["@SSO_MIN_WAGE"].Value = model.sso_min_wage;
                obj_cmd.Parameters.Add("@SSO_MAX_WAGE", SqlDbType.Decimal); obj_cmd.Parameters["@SSO_MAX_WAGE"].Value = model.sso_max_wage;
                obj_cmd.Parameters.Add("@SSO_MIN_AGE", SqlDbType.Int); obj_cmd.Parameters["@SSO_MIN_AGE"].Value = model.sso_min_age;
                obj_cmd.Parameters.Add("@SSO_MAX_AGE", SqlDbType.Int); obj_cmd.Parameters["@SSO_MAX_AGE"].Value = model.sso_max_age;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Company.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_MTCompany model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();


                obj_str.Append("UPDATE SYS_MT_COMPANYS SET ");

                obj_str.Append(" COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(", COMPANY_INITIALS=@COMPANY_INITIALS ");
                obj_str.Append(", COMPANY_NAME_TH=@COMPANY_NAME_TH ");
                obj_str.Append(", COMPANY_NAME_EN=@COMPANY_NAME_EN ");

                obj_str.Append(", SSO_TAX_NO=@SSO_TAX_NO ");
                obj_str.Append(", CITIZEN_NO=@CITIZEN_NO ");
                obj_str.Append(", PROVIDENT_FUND_NO=@PROVIDENT_FUND_NO ");


                obj_str.Append(", HRS_PERDAY=@HRS_PERDAY ");
                obj_str.Append(", SSO_COM_RATE=@SSO_COM_RATE ");
                obj_str.Append(", SSO_EMP_RATE=@SSO_EMP_RATE ");

                obj_str.Append(", SSO_SECURITY_NO=@SSO_SECURITY_NO ");
                obj_str.Append(", SSO_BRANCH_NO=@SSO_BRANCH_NO ");


                obj_str.Append(", SSO_MIN_WAGE=@SSO_MIN_WAGE ");
                obj_str.Append(", SSO_MAX_WAGE=@SSO_MAX_WAGE ");
                obj_str.Append(", SSO_MIN_AGE=@SSO_MIN_AGE ");
                obj_str.Append(", SSO_MAX_AGE=@SSO_MAX_AGE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");

                obj_str.Append(" WHERE COMPANY_ID=@COMPANY_ID ");


                obj_conn.doConnect();
                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@COMPANY_INITIALS", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_INITIALS"].Value = model.company_initials;

                obj_cmd.Parameters.Add("@COMPANY_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_NAME_TH"].Value = model.company_name_th;
                obj_cmd.Parameters.Add("@COMPANY_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_NAME_EN"].Value = model.company_name_en;


                obj_cmd.Parameters.Add("@SSO_TAX_NO", SqlDbType.VarChar); obj_cmd.Parameters["@SSO_TAX_NO"].Value = model.sso_tax_no;
                obj_cmd.Parameters.Add("@CITIZEN_NO", SqlDbType.VarChar); obj_cmd.Parameters["@CITIZEN_NO"].Value = model.citizen_no;
                obj_cmd.Parameters.Add("@PROVIDENT_FUND_NO", SqlDbType.VarChar); obj_cmd.Parameters["@PROVIDENT_FUND_NO"].Value = model.provident_fund_no;


                obj_cmd.Parameters.Add("@HRS_PERDAY", SqlDbType.Decimal); obj_cmd.Parameters["@HRS_PERDAY"].Value = model.hrs_perday;
                obj_cmd.Parameters.Add("@SSO_COM_RATE", SqlDbType.Decimal); obj_cmd.Parameters["@SSO_COM_RATE"].Value = model.sso_com_rate;
                obj_cmd.Parameters.Add("@SSO_EMP_RATE", SqlDbType.Decimal); obj_cmd.Parameters["@SSO_EMP_RATE"].Value = model.sso_emp_rate;


                obj_cmd.Parameters.Add("@SSO_SECURITY_NO", SqlDbType.VarChar); obj_cmd.Parameters["@SSO_SECURITY_NO"].Value = model.sso_security_no;
                obj_cmd.Parameters.Add("@SSO_BRANCH_NO", SqlDbType.VarChar); obj_cmd.Parameters["@SSO_BRANCH_NO"].Value = model.sso_branch_no;

                
                
                obj_cmd.Parameters.Add("@SSO_MIN_WAGE", SqlDbType.Decimal); obj_cmd.Parameters["@SSO_MIN_WAGE"].Value = model.sso_min_wage;
                obj_cmd.Parameters.Add("@SSO_MAX_WAGE", SqlDbType.Decimal); obj_cmd.Parameters["@SSO_MAX_WAGE"].Value = model.sso_max_wage;
                obj_cmd.Parameters.Add("@SSO_MIN_AGE", SqlDbType.Int); obj_cmd.Parameters["@SSO_MIN_AGE"].Value = model.sso_min_age;
                obj_cmd.Parameters.Add("@SSO_MAX_AGE", SqlDbType.Int); obj_cmd.Parameters["@SSO_MAX_AGE"].Value = model.sso_max_age;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@COMPANY_ID", SqlDbType.Int); obj_cmd.Parameters["@COMPANY_ID"].Value = model.company_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.company_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Company.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
 }
