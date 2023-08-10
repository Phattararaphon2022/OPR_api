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
   public class cls_ctMTComaddress
   {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTComaddress() { }

        public string getMessage() { return this.Message.Replace("SYS_MT_COMADDRES", "").Replace("cls_ctMTComaddress", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTComaddress> getData(string condition)
        {
            List<cls_MTComaddress> list_model = new List<cls_MTComaddress>();
            cls_MTComaddress model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", COMBRANCH_CODE");
                obj_str.Append(", COMADDRES_TYPE");

                obj_str.Append(", ISNULL(COMADDRES_NOTH, '') AS COMADDRES_NOTH");
                obj_str.Append(", ISNULL(COMADDRES_MOOTH, '') AS COMADDRES_MOOTH");
                obj_str.Append(", ISNULL(COMADDRES_SOITH, '') AS COMADDRES_SOITH");
                obj_str.Append(", ISNULL(COMADDRES_ROADTH, '') AS COMADDRES_ROADTH");
                obj_str.Append(", ISNULL(COMADDRES_TAMBONTH, '') AS COMADDRES_TAMBONTH");
                obj_str.Append(", ISNULL(COMADDRES_AMPHURTH, '') AS COMADDRES_AMPHURTH");


                obj_str.Append(", ISNULL(COMADDRES_NOEN, '') AS COMADDRES_NOEN");
                obj_str.Append(", ISNULL(COMADDRES_MOOEN, '') AS COMADDRES_MOOEN");
                obj_str.Append(", ISNULL(COMADDRES_SOIEN, '') AS COMADDRES_SOIEN");
                obj_str.Append(", ISNULL(COMADDRES_ROADEN, '') AS COMADDRES_ROADEN");
                obj_str.Append(", ISNULL(COMADDRES_TAMBONEN, '') AS COMADDRES_TAMBONEN");
                obj_str.Append(", ISNULL(COMADDRES_AMPHUREN, '') AS COMADDRES_AMPHUREN");

                obj_str.Append(", ISNULL(COMADDRES_ZIPCODE, '') AS COMADDRES_ZIPCODE");

                obj_str.Append(", ISNULL(PROVINCE_CODE, '') AS PROVINCE_CODE");

                obj_str.Append(", ISNULL(COMADDRES_TEL, '') AS COMADDRES_TEL");

                obj_str.Append(", ISNULL(COMADDRES_FAX, '') AS COMADDRES_FAX");
                obj_str.Append(", ISNULL(COMADDRES_URL, '') AS COMADDRES_URL");


                obj_str.Append(", ISNULL(COMADDRES_EMAIL, '') AS COMADDRES_EMAIL");
                obj_str.Append(", ISNULL(COMADDRES_LINE, '') AS COMADDRES_LINE");
                obj_str.Append(", ISNULL(COMADDRES_FACEBOOK, '') AS COMADDRES_FACEBOOK");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM SYS_MT_COMADDRES");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, COMBRANCH_CODE, COMADDRES_TYPE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTComaddress();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                    model.combranch_code = Convert.ToString(dr["COMBRANCH_CODE"]);
                    model.comaddres_type = Convert.ToString(dr["COMADDRES_TYPE"]);

                    model.comaddres_noth = Convert.ToString(dr["COMADDRES_NOTH"]);
                    model.comaddres_mooth = Convert.ToString(dr["COMADDRES_MOOTH"]);
                    model.comaddres_soith = Convert.ToString(dr["COMADDRES_SOITH"]);
                    model.comaddres_roadth = Convert.ToString(dr["COMADDRES_ROADTH"]);
                    model.comaddres_tambonth = Convert.ToString(dr["COMADDRES_TAMBONTH"]);
                    model.comaddres_amphurth = Convert.ToString(dr["COMADDRES_AMPHURTH"]);


                    model.comaddres_noen = Convert.ToString(dr["COMADDRES_NOEN"]);
                    model.comaddres_mooen = Convert.ToString(dr["COMADDRES_MOOEN"]);
                    model.comaddres_soien = Convert.ToString(dr["COMADDRES_SOIEN"]);
                    model.comaddres_roaden = Convert.ToString(dr["COMADDRES_ROADEN"]);
                    model.comaddres_tambonen = Convert.ToString(dr["COMADDRES_TAMBONEN"]);
                    model.comaddres_amphuren = Convert.ToString(dr["COMADDRES_AMPHUREN"]);

                    model.comaddres_zipcode = Convert.ToString(dr["COMADDRES_ZIPCODE"]);
                    model.province_code = Convert.ToString(dr["PROVINCE_CODE"]);

                    model.comaddres_tel = Convert.ToString(dr["COMADDRES_TEL"]);

                    model.comaddres_fax = Convert.ToString(dr["COMADDRES_FAX"]);
                    model.comaddres_url = Convert.ToString(dr["COMADDRES_URL"]);


                    model.comaddres_email = Convert.ToString(dr["COMADDRES_EMAIL"]);
                    model.comaddres_line = Convert.ToString(dr["COMADDRES_LINE"]);
                    model.comaddres_facebook = Convert.ToString(dr["COMADDRES_FACEBOOK"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "CCAD001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTComaddress> getDataByFillter(string com, string branch, string type)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!branch.Equals(""))
                strCondition += " AND COMBRANCH_CODE='" + branch + "'";

            if (!type.Equals(""))
                strCondition += " AND COMADDRES_TYPE='" + type + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(ADDRESS_ID, 1) ");
                obj_str.Append(" FROM SYS_MT_COMADDRES");
                obj_str.Append(" ORDER BY ADDRESS_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "CCAD002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string branch, string type)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM SYS_MT_COMADDRES");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND COMBRANCH_CODE='" + branch + "' ");
                obj_str.Append(" AND COMADDRES_TYPE='" + type + "' ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "CCAD003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string branch, string type)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SYS_MT_COMADDRES");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND COMBRANCH_CODE='" + branch + "' ");
                obj_str.Append(" AND COMADDRES_TYPE='" + type + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "CCAD004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_MTComaddress model)
        {
            bool blnResult = false;
            string strResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.combranch_code, model.comaddres_type))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_MT_COMADDRES");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", COMBRANCH_CODE ");
                obj_str.Append(", COMADDRES_TYPE ");

                obj_str.Append(", COMADDRES_NOTH ");
                obj_str.Append(", COMADDRES_MOOTH ");
                obj_str.Append(", COMADDRES_SOITH ");
                obj_str.Append(", COMADDRES_ROADTH ");
                obj_str.Append(", COMADDRES_TAMBONTH ");
                obj_str.Append(", COMADDRES_AMPHURTH ");
 
                obj_str.Append(", COMADDRES_NOEN ");
                obj_str.Append(", COMADDRES_MOOEN ");
                obj_str.Append(", COMADDRES_SOIEN ");
                obj_str.Append(", COMADDRES_ROADEN ");
                obj_str.Append(", COMADDRES_TAMBONEN ");
                obj_str.Append(", COMADDRES_AMPHUREN ");


                obj_str.Append(", COMADDRES_ZIPCODE ");
                obj_str.Append(", PROVINCE_CODE ");
                obj_str.Append(", COMADDRES_TEL ");

                obj_str.Append(", COMADDRES_FAX ");
                obj_str.Append(", COMADDRES_URL ");



                obj_str.Append(", COMADDRES_EMAIL ");
                obj_str.Append(", COMADDRES_LINE ");
                obj_str.Append(", COMADDRES_FACEBOOK ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @COMBRANCH_CODE ");
                obj_str.Append(", @COMADDRES_TYPE ");

                obj_str.Append(", @COMADDRES_NOTH ");
                obj_str.Append(", @COMADDRES_MOOTH ");
                obj_str.Append(", @COMADDRES_SOITH ");
                obj_str.Append(", @COMADDRES_ROADTH ");
                obj_str.Append(", @COMADDRES_TAMBONTH ");
                obj_str.Append(", @COMADDRES_AMPHURTH ");
               

                obj_str.Append(", @COMADDRES_NOEN ");
                obj_str.Append(", @COMADDRES_MOOEN ");
                obj_str.Append(", @COMADDRES_SOIEN ");
                obj_str.Append(", @COMADDRES_ROADEN ");
                obj_str.Append(", @COMADDRES_TAMBONEN ");
                obj_str.Append(", @COMADDRES_AMPHUREN ");

                obj_str.Append(", @COMADDRES_ZIPCODE ");

                obj_str.Append(", @PROVINCE_CODE ");


                obj_str.Append(", @COMADDRES_TEL ");

                obj_str.Append(", @COMADDRES_FAX ");
                obj_str.Append(", @COMADDRES_URL ");

                obj_str.Append(", @COMADDRES_EMAIL ");
                obj_str.Append(", @COMADDRES_LINE ");
                obj_str.Append(", @COMADDRES_FACEBOOK ");
                obj_str.Append(", @CREATED_BY ");

                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                //model.address_id = this.getNextID();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@COMBRANCH_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMBRANCH_CODE"].Value = model.combranch_code;
                obj_cmd.Parameters.Add("@COMADDRES_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_TYPE"].Value = model.comaddres_type;
                
                obj_cmd.Parameters.Add("@COMADDRES_NOTH", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_NOTH"].Value = model.comaddres_noth;
                obj_cmd.Parameters.Add("@COMADDRES_MOOTH", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_MOOTH"].Value = model.comaddres_mooth;
                obj_cmd.Parameters.Add("@COMADDRES_SOITH", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_SOITH"].Value = model.comaddres_soith;
                obj_cmd.Parameters.Add("@COMADDRES_ROADTH", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_ROADTH"].Value = model.comaddres_roadth;
                obj_cmd.Parameters.Add("@COMADDRES_TAMBONTH", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_TAMBONTH"].Value = model.comaddres_tambonth;
                obj_cmd.Parameters.Add("@COMADDRES_AMPHURTH", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_AMPHURTH"].Value = model.comaddres_amphurth;
 
                obj_cmd.Parameters.Add("@COMADDRES_NOEN", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_NOEN"].Value = model.comaddres_noen;
                obj_cmd.Parameters.Add("@COMADDRES_MOOEN", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_MOOEN"].Value = model.comaddres_mooen;
                obj_cmd.Parameters.Add("@COMADDRES_SOIEN", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_SOIEN"].Value = model.comaddres_soien;
                obj_cmd.Parameters.Add("@COMADDRES_ROADEN", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_ROADEN"].Value = model.comaddres_roaden;
                obj_cmd.Parameters.Add("@COMADDRES_TAMBONEN", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_TAMBONEN"].Value = model.comaddres_tambonen;
                obj_cmd.Parameters.Add("@COMADDRES_AMPHUREN", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_AMPHUREN"].Value = model.comaddres_amphuren;

                obj_cmd.Parameters.Add("@COMADDRES_ZIPCODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_ZIPCODE"].Value = model.comaddres_zipcode;
                obj_cmd.Parameters.Add("@PROVINCE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROVINCE_CODE"].Value = model.province_code;

                obj_cmd.Parameters.Add("@COMADDRES_TEL", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_TEL"].Value = model.comaddres_tel;

                obj_cmd.Parameters.Add("@COMADDRES_FAX", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_FAX"].Value = model.comaddres_fax;
                obj_cmd.Parameters.Add("@COMADDRES_URL", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_URL"].Value = model.comaddres_url;

                
                obj_cmd.Parameters.Add("@COMADDRES_EMAIL", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_EMAIL"].Value = model.comaddres_email;
                obj_cmd.Parameters.Add("@COMADDRES_LINE", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_LINE"].Value = model.comaddres_line;
                obj_cmd.Parameters.Add("@COMADDRES_FACEBOOK", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_FACEBOOK"].Value = model.comaddres_facebook;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                strResult = model.comaddres_type.ToString();
            }
            catch (Exception ex)
            {
                Message = "CCAD005:" + ex.ToString();
                strResult = "";
            }

            return blnResult;
        }

        public bool update(cls_MTComaddress model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE SYS_MT_COMADDRES SET ");


                obj_str.Append(" COMADDRES_NOTH=@COMADDRES_NOTH ");
                obj_str.Append(", COMADDRES_MOOTH=@COMADDRES_MOOTH ");
                obj_str.Append(", COMADDRES_SOITH=@COMADDRES_SOITH ");
                obj_str.Append(", COMADDRES_ROADTH=@COMADDRES_ROADTH ");
                obj_str.Append(", COMADDRES_TAMBONTH=@COMADDRES_TAMBONTH ");
                obj_str.Append(", COMADDRES_AMPHURTH=@COMADDRES_AMPHURTH ");

 

                obj_str.Append(", COMADDRES_NOEN=@COMADDRES_NOEN ");
                obj_str.Append(", COMADDRES_MOOEN=@COMADDRES_MOOEN ");
                obj_str.Append(", COMADDRES_SOIEN=@COMADDRES_SOIEN ");
                obj_str.Append(", COMADDRES_ROADEN=@COMADDRES_ROADEN ");
                obj_str.Append(", COMADDRES_TAMBONEN=@COMADDRES_TAMBONEN ");
                obj_str.Append(", COMADDRES_AMPHUREN=@COMADDRES_AMPHUREN ");

                obj_str.Append(", PROVINCE_CODE=@PROVINCE_CODE ");


                obj_str.Append(", COMADDRES_ZIPCODE=@COMADDRES_ZIPCODE ");
                obj_str.Append(", COMADDRES_TEL=@COMADDRES_TEL ");

                obj_str.Append(", COMADDRES_FAX=@COMADDRES_FAX ");
                obj_str.Append(", COMADDRES_URL=@COMADDRES_URL ");

                obj_str.Append(", COMADDRES_EMAIL=@COMADDRES_EMAIL ");
                obj_str.Append(", COMADDRES_LINE=@COMADDRES_LINE ");
                obj_str.Append(", COMADDRES_FACEBOOK=@COMADDRES_FACEBOOK ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND COMBRANCH_CODE=@COMBRANCH_CODE ");
                obj_str.Append(" AND COMADDRES_TYPE=@COMADDRES_TYPE ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMADDRES_NOTH", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_NOTH"].Value = model.comaddres_noth;
                obj_cmd.Parameters.Add("@COMADDRES_MOOTH", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_MOOTH"].Value = model.comaddres_mooth;
                obj_cmd.Parameters.Add("@COMADDRES_SOITH", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_SOITH"].Value = model.comaddres_soith;
                obj_cmd.Parameters.Add("@COMADDRES_ROADTH", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_ROADTH"].Value = model.comaddres_roadth;
                obj_cmd.Parameters.Add("@COMADDRES_TAMBONTH", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_TAMBONTH"].Value = model.comaddres_tambonth;
                obj_cmd.Parameters.Add("@COMADDRES_AMPHURTH", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_AMPHURTH"].Value = model.comaddres_amphurth;
 

                obj_cmd.Parameters.Add("@COMADDRES_NOEN", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_NOEN"].Value = model.comaddres_noen;
                obj_cmd.Parameters.Add("@COMADDRES_MOOEN", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_MOOEN"].Value = model.comaddres_mooen;
                obj_cmd.Parameters.Add("@COMADDRES_SOIEN", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_SOIEN"].Value = model.comaddres_soien;
                obj_cmd.Parameters.Add("@COMADDRES_ROADEN", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_ROADEN"].Value = model.comaddres_roaden;
                obj_cmd.Parameters.Add("@COMADDRES_TAMBONEN", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_TAMBONEN"].Value = model.comaddres_tambonen;
                obj_cmd.Parameters.Add("@COMADDRES_AMPHUREN", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_AMPHUREN"].Value = model.comaddres_amphuren;

                obj_cmd.Parameters.Add("@COMADDRES_ZIPCODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_ZIPCODE"].Value = model.comaddres_zipcode;
                obj_cmd.Parameters.Add("@PROVINCE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROVINCE_CODE"].Value = model.province_code;



                obj_cmd.Parameters.Add("@COMADDRES_TEL", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_TEL"].Value = model.comaddres_tel;

                obj_cmd.Parameters.Add("@COMADDRES_FAX", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_FAX"].Value = model.comaddres_fax;
                obj_cmd.Parameters.Add("@COMADDRES_URL", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_URL"].Value = model.comaddres_url;

                
                obj_cmd.Parameters.Add("@COMADDRES_EMAIL", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_EMAIL"].Value = model.comaddres_email;
                obj_cmd.Parameters.Add("@COMADDRES_LINE", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_LINE"].Value = model.comaddres_line;
                obj_cmd.Parameters.Add("@COMADDRES_FACEBOOK", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_FACEBOOK"].Value = model.comaddres_facebook;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@COMBRANCH_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMBRANCH_CODE"].Value = model.combranch_code;
                obj_cmd.Parameters.Add("@COMADDRES_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRES_TYPE"].Value = model.comaddres_type;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "CCAD006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
