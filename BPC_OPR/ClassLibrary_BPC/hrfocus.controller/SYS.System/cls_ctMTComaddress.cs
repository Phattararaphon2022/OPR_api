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

        public string getMessage() { return this.Message.Replace("SYS_MT_COMADDRESS", "").Replace("cls_ctMTComaddress", "").Replace("line", ""); }

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
                obj_str.Append(", COMADDRESS_TYPE");

                obj_str.Append(", ISNULL(COMADDRESSTH_NO, '') AS COMADDRESSTH_NO");
                obj_str.Append(", ISNULL(COMADDRESSTH_MOO, '') AS COMADDRESSTH_MOO");
                obj_str.Append(", ISNULL(COMADDRESSTH_SOI, '') AS COMADDRESSTH_SOI");
                obj_str.Append(", ISNULL(COMADDRESSTH_ROAD, '') AS COMADDRESSTH_ROAD");
                obj_str.Append(", ISNULL(COMADDRESSTH_TAMBON, '') AS COMADDRESSTH_TAMBON");
                obj_str.Append(", ISNULL(COMADDRESSTH_AMPHUR, '') AS COMADDRESSTH_AMPHUR");
                obj_str.Append(", ISNULL(PROVINCETH_CODE, '') AS PROVINCETH_CODE");

                obj_str.Append(", ISNULL(COMADDRESSEN_NO, '') AS COMADDRESSEN_NO");
                obj_str.Append(", ISNULL(COMADDRESSEN_MOO, '') AS COMADDRESSEN_MOO");
                obj_str.Append(", ISNULL(COMADDRESSEN_SOI, '') AS COMADDRESSEN_SOI");
                obj_str.Append(", ISNULL(COMADDRESSEN_ROAD, '') AS COMADDRESSEN_ROAD");
                obj_str.Append(", ISNULL(COMADDRESSEN_TAMBON, '') AS COMADDRESSEN_TAMBON");
                obj_str.Append(", ISNULL(COMADDRESSEN_AMPHUR, '') AS COMADDRESSEN_AMPHUR");
                obj_str.Append(", ISNULL(COMADDRESS_ZIPCODE, '') AS COMADDRESS_ZIPCODE");
                obj_str.Append(", ISNULL(PROVINCEEN_CODE, '') AS PROVINCEEN_CODE");

                obj_str.Append(", ISNULL(COMADDRESS_TEL, '') AS COMADDRESS_TEL");
                obj_str.Append(", ISNULL(COMADDRESS_EMAIL, '') AS COMADDRESS_EMAIL");
                obj_str.Append(", ISNULL(COMADDRESS_LINE, '') AS COMADDRESS_LINE");
                obj_str.Append(", ISNULL(COMADDRESS_FACEBOOK, '') AS COMADDRESS_FACEBOOK");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM SYS_MT_COMADDRESS");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, COMBRANCH_CODE, COMADDRESS_TYPE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTComaddress();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                    model.combranch_code = Convert.ToString(dr["COMBRANCH_CODE"]);
                    model.comaddress_type = Convert.ToString(dr["COMADDRESS_TYPE"]);

                    model.comaddressth_no = Convert.ToString(dr["COMADDRESSTH_NO"]);
                    model.comaddressth_moo = Convert.ToString(dr["COMADDRESSTH_MOO"]);
                    model.comaddressth_soi = Convert.ToString(dr["COMADDRESSTH_SOI"]);
                    model.comaddressth_road = Convert.ToString(dr["COMADDRESSTH_ROAD"]);
                    model.comaddressth_tambon = Convert.ToString(dr["COMADDRESSTH_TAMBON"]);
                    model.comaddressth_amphur = Convert.ToString(dr["COMADDRESSTH_AMPHUR"]);
                    model.provinceth_code = Convert.ToString(dr["PROVINCETH_CODE"]);

                    model.comaddressen_no = Convert.ToString(dr["COMADDRESSEN_NO"]);
                    model.comaddressen_moo = Convert.ToString(dr["COMADDRESSEN_MOO"]);
                    model.comaddressen_soi = Convert.ToString(dr["COMADDRESSEN_SOI"]);
                    model.comaddressen_road = Convert.ToString(dr["COMADDRESSEN_ROAD"]);
                    model.comaddressen_tambon = Convert.ToString(dr["COMADDRESSEN_TAMBON"]);
                    model.comaddressen_amphur = Convert.ToString(dr["COMADDRESSEN_AMPHUR"]);
                    model.comaddress_zipcode = Convert.ToString(dr["COMADDRESS_ZIPCODE"]);
                    model.provinceen_code = Convert.ToString(dr["PROVINCEEN_CODE"]);

                    model.comaddress_tel = Convert.ToString(dr["COMADDRESS_TEL"]);
                    model.comaddress_email = Convert.ToString(dr["COMADDRESS_EMAIL"]);
                    model.comaddress_line = Convert.ToString(dr["COMADDRESS_LINE"]);
                    model.comaddress_facebook = Convert.ToString(dr["COMADDRESS_FACEBOOK"]);

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
                strCondition += " AND COMADDRESS_TYPE='" + type + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(ADDRESS_ID, 1) ");
                obj_str.Append(" FROM SYS_MT_COMADDRESS");
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
                obj_str.Append(" FROM SYS_MT_COMADDRESS");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND COMBRANCH_CODE='" + branch + "' ");
                obj_str.Append(" AND COMADDRESS_TYPE='" + type + "' ");

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

                obj_str.Append(" DELETE FROM SYS_MT_COMADDRESS");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND COMBRANCH_CODE='" + branch + "' ");
                obj_str.Append(" AND COMADDRESS_TYPE='" + type + "' ");

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
                if (this.checkDataOld(model.company_code, model.combranch_code, model.comaddress_type))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_MT_COMADDRESS");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", COMBRANCH_CODE ");
                obj_str.Append(", COMADDRESS_TYPE ");

                obj_str.Append(", COMADDRESSTH_NO ");
                obj_str.Append(", COMADDRESSTH_MOO ");
                obj_str.Append(", COMADDRESSTH_SOI ");
                obj_str.Append(", COMADDRESSTH_ROAD ");
                obj_str.Append(", COMADDRESSTH_TAMBON ");
                obj_str.Append(", COMADDRESSTH_AMPHUR ");
                obj_str.Append(", PROVINCETH_CODE ");

                obj_str.Append(", COMADDRESSEN_NO ");
                obj_str.Append(", COMADDRESSEN_MOO ");
                obj_str.Append(", COMADDRESSEN_SOI ");
                obj_str.Append(", COMADDRESSEN_ROAD ");
                obj_str.Append(", COMADDRESSEN_TAMBON ");
                obj_str.Append(", COMADDRESSEN_AMPHUR ");
                obj_str.Append(", COMADDRESS_ZIPCODE ");
                obj_str.Append(", PROVINCEEN_CODE ");


                obj_str.Append(", COMADDRESS_TEL ");
                obj_str.Append(", COMADDRESS_EMAIL ");
                obj_str.Append(", COMADDRESS_LINE ");
                obj_str.Append(", COMADDRESS_FACEBOOK ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @COMBRANCH_CODE ");
                obj_str.Append(", @COMADDRESS_TYPE ");

                obj_str.Append(", @COMADDRESSTH_NO ");
                obj_str.Append(", @COMADDRESSTH_MOO ");
                obj_str.Append(", @COMADDRESSTH_SOI ");
                obj_str.Append(", @COMADDRESSTH_ROAD ");
                obj_str.Append(", @COMADDRESSTH_TAMBON ");
                obj_str.Append(", @COMADDRESSTH_AMPHUR ");
                obj_str.Append(", @PROVINCETH_CODE ");

                obj_str.Append(", @COMADDRESSEN_NO ");
                obj_str.Append(", @COMADDRESSEN_MOO ");
                obj_str.Append(", @COMADDRESSEN_SOI ");
                obj_str.Append(", @COMADDRESSEN_ROAD ");
                obj_str.Append(", @COMADDRESSEN_TAMBON ");
                obj_str.Append(", @COMADDRESSEN_AMPHUR ");
                obj_str.Append(", @COMADDRESS_ZIPCODE ");
                obj_str.Append(", @PROVINCEEN_CODE ");


                obj_str.Append(", @COMADDRESS_TEL ");
                obj_str.Append(", @COMADDRESS_EMAIL ");
                obj_str.Append(", @COMADDRESS_LINE ");
                obj_str.Append(", @COMADDRESS_FACEBOOK ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                //model.address_id = this.getNextID();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@COMBRANCH_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMBRANCH_CODE"].Value = model.combranch_code;
                obj_cmd.Parameters.Add("@COMADDRESS_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESS_TYPE"].Value = model.comaddress_type;
                
                obj_cmd.Parameters.Add("@COMADDRESSTH_NO", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSTH_NO"].Value = model.comaddressth_no;
                obj_cmd.Parameters.Add("@COMADDRESSTH_MOO", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSTH_MOO"].Value = model.comaddressth_moo;
                obj_cmd.Parameters.Add("@COMADDRESSTH_SOI", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSTH_SOI"].Value = model.comaddressth_soi;
                obj_cmd.Parameters.Add("@COMADDRESSTH_ROAD", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSTH_ROAD"].Value = model.comaddressth_road;
                obj_cmd.Parameters.Add("@COMADDRESSTH_TAMBON", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSTH_TAMBON"].Value = model.comaddressth_tambon;
                obj_cmd.Parameters.Add("@COMADDRESSTH_AMPHUR", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSTH_AMPHUR"].Value = model.comaddressth_amphur;
                obj_cmd.Parameters.Add("@PROVINCETH_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROVINCETH_CODE"].Value = model.provinceth_code;

                obj_cmd.Parameters.Add("@COMADDRESSEN_NO", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSEN_NO"].Value = model.comaddressen_no;
                obj_cmd.Parameters.Add("@COMADDRESSEN_MOO", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSEN_MOO"].Value = model.comaddressen_moo;
                obj_cmd.Parameters.Add("@COMADDRESSEN_SOI", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSEN_SOI"].Value = model.comaddressen_soi;
                obj_cmd.Parameters.Add("@COMADDRESSEN_ROAD", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSEN_ROAD"].Value = model.comaddressen_road;
                obj_cmd.Parameters.Add("@COMADDRESSEN_TAMBON", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSEN_TAMBON"].Value = model.comaddressen_tambon;
                obj_cmd.Parameters.Add("@COMADDRESSEN_AMPHUR", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSEN_AMPHUR"].Value = model.comaddressen_amphur;
                obj_cmd.Parameters.Add("@COMADDRESS_ZIPCODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESS_ZIPCODE"].Value = model.comaddress_zipcode;
                obj_cmd.Parameters.Add("@PROVINCEEN_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROVINCEEN_CODE"].Value = model.provinceen_code;

                obj_cmd.Parameters.Add("@COMADDRESS_TEL", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESS_TEL"].Value = model.comaddress_tel;
                obj_cmd.Parameters.Add("@COMADDRESS_EMAIL", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESS_EMAIL"].Value = model.comaddress_email;
                obj_cmd.Parameters.Add("@COMADDRESS_LINE", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESS_LINE"].Value = model.comaddress_line;
                obj_cmd.Parameters.Add("@COMADDRESS_FACEBOOK", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESS_FACEBOOK"].Value = model.comaddress_facebook;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                strResult = model.comaddress_type.ToString();
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
                obj_str.Append("UPDATE SYS_MT_COMADDRESS SET ");


                obj_str.Append(" COMADDRESSTH_NO=@COMADDRESSTH_NO ");
                obj_str.Append(", COMADDRESSTH_MOO=@COMADDRESSTH_MOO ");
                obj_str.Append(", COMADDRESSTH_SOI=@COMADDRESSTH_SOI ");
                obj_str.Append(", COMADDRESSTH_ROAD=@COMADDRESSTH_ROAD ");
                obj_str.Append(", COMADDRESSTH_TAMBON=@COMADDRESSTH_TAMBON ");
                obj_str.Append(", COMADDRESSTH_AMPHUR=@COMADDRESSTH_AMPHUR ");
                obj_str.Append(", PROVINCETH_CODE=@PROVINCETH_CODE ");


                obj_str.Append(", COMADDRESSEN_NO=@COMADDRESSEN_NO ");
                obj_str.Append(", COMADDRESSEN_MOO=@COMADDRESSEN_MOO ");
                obj_str.Append(", COMADDRESSEN_SOI=@COMADDRESSEN_SOI ");
                obj_str.Append(", COMADDRESSEN_ROAD=@COMADDRESSEN_ROAD ");
                obj_str.Append(", COMADDRESSEN_TAMBON=@COMADDRESSEN_TAMBON ");
                obj_str.Append(", COMADDRESSEN_AMPHUR=@COMADDRESSEN_AMPHUR ");
                obj_str.Append(", PROVINCEEN_CODE=@PROVINCEEN_CODE ");


                obj_str.Append(", COMADDRESS_ZIPCODE=@COMADDRESS_ZIPCODE ");
                obj_str.Append(", COMADDRESS_TEL=@COMADDRESS_TEL ");
                obj_str.Append(", COMADDRESS_EMAIL=@COMADDRESS_EMAIL ");
                obj_str.Append(", COMADDRESS_LINE=@COMADDRESS_LINE ");
                obj_str.Append(", COMADDRESS_FACEBOOK=@COMADDRESS_FACEBOOK ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND COMBRANCH_CODE=@COMBRANCH_CODE ");
                obj_str.Append(" AND COMADDRESS_TYPE=@COMADDRESS_TYPE ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMADDRESSTH_NO", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSTH_NO"].Value = model.comaddressth_no;
                obj_cmd.Parameters.Add("@COMADDRESSTH_MOO", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSTH_MOO"].Value = model.comaddressth_moo;
                obj_cmd.Parameters.Add("@COMADDRESSTH_SOI", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSTH_SOI"].Value = model.comaddressth_soi;
                obj_cmd.Parameters.Add("@COMADDRESSTH_ROAD", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSTH_ROAD"].Value = model.comaddressth_road;
                obj_cmd.Parameters.Add("@COMADDRESSTH_TAMBON", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSTH_TAMBON"].Value = model.comaddressth_tambon;
                obj_cmd.Parameters.Add("@COMADDRESSTH_AMPHUR", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSTH_AMPHUR"].Value = model.comaddressth_amphur;
                obj_cmd.Parameters.Add("@PROVINCETH_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROVINCETH_CODE"].Value = model.provinceth_code;


                obj_cmd.Parameters.Add("@COMADDRESSEN_NO", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSEN_NO"].Value = model.comaddressen_no;
                obj_cmd.Parameters.Add("@COMADDRESSEN_MOO", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSEN_MOO"].Value = model.comaddressen_moo;
                obj_cmd.Parameters.Add("@COMADDRESSEN_SOI", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSEN_SOI"].Value = model.comaddressen_soi;
                obj_cmd.Parameters.Add("@COMADDRESSEN_ROAD", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSEN_ROAD"].Value = model.comaddressen_road;
                obj_cmd.Parameters.Add("@COMADDRESSEN_TAMBON", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSEN_TAMBON"].Value = model.comaddressen_tambon;
                obj_cmd.Parameters.Add("@COMADDRESSEN_AMPHUR", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESSEN_AMPHUR"].Value = model.comaddressen_amphur;
                obj_cmd.Parameters.Add("@COMADDRESS_ZIPCODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESS_ZIPCODE"].Value = model.comaddress_zipcode;
                obj_cmd.Parameters.Add("@PROVINCEEN_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROVINCEEN_CODE"].Value = model.provinceen_code;



                obj_cmd.Parameters.Add("@COMADDRESS_TEL", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESS_TEL"].Value = model.comaddress_tel;
                obj_cmd.Parameters.Add("@COMADDRESS_EMAIL", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESS_EMAIL"].Value = model.comaddress_email;
                obj_cmd.Parameters.Add("@COMADDRESS_LINE", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESS_LINE"].Value = model.comaddress_line;
                obj_cmd.Parameters.Add("@COMADDRESS_FACEBOOK", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESS_FACEBOOK"].Value = model.comaddress_facebook;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@COMBRANCH_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMBRANCH_CODE"].Value = model.combranch_code;
                obj_cmd.Parameters.Add("@COMADDRESS_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESS_TYPE"].Value = model.comaddress_type;

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
