using ClassLibrary_BPC.hrfocus.model.Recruitment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRApplyaddress
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRApplyaddress() { }

        public string getMessage() { return this.Message.Replace("REQ_TR_APPLYADDRESS", "").Replace("cls_ctTRApplyaddress", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRApplyaddress> getData(string condition)
        {
            List<cls_TRApplyaddress> list_model = new List<cls_TRApplyaddress>();
            cls_TRApplyaddress model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", APPLYWORK_CODE");
                obj_str.Append(", APPLYADDRESS_ID");
                obj_str.Append(", APPLYADDRESS_TYPE");
                obj_str.Append(", ISNULL(APPLYADDRESS_NO, '') AS APPLYADDRESS_NO");
                obj_str.Append(", ISNULL(APPLYADDRESS_MOO, '') AS APPLYADDRESS_MOO");
                obj_str.Append(", ISNULL(APPLYADDRESS_SOI, '') AS APPLYADDRESS_SOI");
                obj_str.Append(", ISNULL(APPLYADDRESS_ROAD, '') AS APPLYADDRESS_ROAD");
                obj_str.Append(", ISNULL(APPLYADDRESS_TAMBON, '') AS APPLYADDRESS_TAMBON");
                obj_str.Append(", ISNULL(APPLYADDRESS_AMPHUR, '') AS APPLYADDRESS_AMPHUR");
                obj_str.Append(", ISNULL(APPLYPROVINCE_CODE, '') AS APPLYPROVINCE_CODE");
                obj_str.Append(", ISNULL(APPLYADDRESS_ZIPCODE, '') AS APPLYADDRESS_ZIPCODE");
                obj_str.Append(", ISNULL(APPLYADDRESS_TEL, '') AS APPLYADDRESS_TEL");
                obj_str.Append(", ISNULL(APPLYADDRESS_EMAIL, '') AS APPLYADDRESS_EMAIL");
                obj_str.Append(", ISNULL(APPLYADDRESS_LINE, '') AS APPLYADDRESS_LINE");
                obj_str.Append(", ISNULL(APPLYADDRESS_FACEBOOK, '') AS APPLYADDRESS_FACEBOOK");
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM REQ_TR_APPLYADDRESS");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE,APPLYWORK_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRApplyaddress();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                    model.applywork_code = Convert.ToString(dr["APPLYWORK_CODE"]);
                    model.applyaddress_id = Convert.ToInt32(dr["APPLYADDRESS_ID"]);
                    model.applyaddress_type = Convert.ToString(dr["APPLYADDRESS_TYPE"]);
                    model.applyaddress_no = Convert.ToString(dr["APPLYADDRESS_NO"]);
                    model.applyaddress_moo = Convert.ToString(dr["APPLYADDRESS_MOO"]);
                    model.applyaddress_soi = Convert.ToString(dr["APPLYADDRESS_SOI"]);
                    model.applyaddress_road = Convert.ToString(dr["APPLYADDRESS_ROAD"]);
                    model.applyaddress_tambon = Convert.ToString(dr["APPLYADDRESS_TAMBON"]);
                    model.applyaddress_amphur = Convert.ToString(dr["APPLYADDRESS_AMPHUR"]);
                    model.applyprovince_code = Convert.ToString(dr["APPLYPROVINCE_CODE"]);
                    model.applyaddress_zipcode = Convert.ToString(dr["APPLYADDRESS_ZIPCODE"]);
                    model.applyaddress_tel = Convert.ToString(dr["APPLYADDRESS_TEL"]);
                    model.applyaddress_email = Convert.ToString(dr["APPLYADDRESS_EMAIL"]);
                    model.applyaddress_line = Convert.ToString(dr["APPLYADDRESS_LINE"]);
                    model.applyaddress_facebook = Convert.ToString(dr["APPLYADDRESS_FACEBOOK"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "REQADD001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRApplyaddress> getDataByFillter(string com, string emp)
        {
            string strCondition = "";

            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!emp.Equals(""))
                strCondition += " AND APPLYWORK_CODE='" + emp + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(APPLYADDRESS_ID, 1) ");
                obj_str.Append(" FROM REQ_TR_APPLYADDRESS");
                obj_str.Append(" ORDER BY APPLYADDRESS_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "REQADD002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT APPLYADDRESS_ID");
                obj_str.Append(" FROM REQ_TR_APPLYADDRESS");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND APPLYWORK_CODE='" + emp + "' ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "REQADD003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string emp)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM REQ_TR_APPLYADDRESS");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND APPLYWORK_CODE='" + emp + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "REQADD004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRApplyaddress model)
        {
            bool blnResult = false;
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.company_code,model.applywork_code))
                {
                    return this.update(model);
                    //if (this.update(model))
                    //    return model.address_id.ToString();
                    //else
                    //    return "";
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO REQ_TR_APPLYADDRESS");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", APPLYWORK_CODE ");
                obj_str.Append(", APPLYADDRESS_ID ");
                obj_str.Append(", APPLYADDRESS_TYPE ");
                obj_str.Append(", APPLYADDRESS_NO ");
                obj_str.Append(", APPLYADDRESS_MOO ");
                obj_str.Append(", APPLYADDRESS_SOI ");
                obj_str.Append(", APPLYADDRESS_ROAD ");
                obj_str.Append(", APPLYADDRESS_TAMBON ");
                obj_str.Append(", APPLYADDRESS_AMPHUR ");
                obj_str.Append(", APPLYPROVINCE_CODE ");
                obj_str.Append(", APPLYADDRESS_ZIPCODE ");
                obj_str.Append(", APPLYADDRESS_TEL ");
                obj_str.Append(", APPLYADDRESS_EMAIL ");
                obj_str.Append(", APPLYADDRESS_LINE ");
                obj_str.Append(", APPLYADDRESS_FACEBOOK ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @APPLYWORK_CODE ");
                obj_str.Append(", @APPLYADDRESS_ID ");
                obj_str.Append(", @APPLYADDRESS_TYPE ");
                obj_str.Append(", @APPLYADDRESS_NO ");
                obj_str.Append(", @APPLYADDRESS_MOO  ");
                obj_str.Append(", @APPLYADDRESS_SOI ");
                obj_str.Append(", @APPLYADDRESS_ROAD ");
                obj_str.Append(", @APPLYADDRESS_TAMBON ");
                obj_str.Append(", @APPLYADDRESS_AMPHUR ");
                obj_str.Append(", @APPLYPROVINCE_CODE ");
                obj_str.Append(", @APPLYADDRESS_ZIPCODE ");
                obj_str.Append(", @APPLYADDRESS_TEL ");
                obj_str.Append(", @APPLYADDRESS_EMAIL ");
                obj_str.Append(", @APPLYADDRESS_LINE ");
                obj_str.Append(", @APPLYADDRESS_FACEBOOK ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.applyaddress_id = this.getNextID();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@APPLYWORK_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYWORK_CODE"].Value = model.applywork_code;
                obj_cmd.Parameters.Add("@APPLYADDRESS_ID", SqlDbType.Int); obj_cmd.Parameters["@APPLYADDRESS_ID"].Value = model.applyaddress_id;
                obj_cmd.Parameters.Add("@APPLYADDRESS_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_TYPE"].Value = model.applyaddress_type;
                obj_cmd.Parameters.Add("@APPLYADDRESS_NO", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_NO"].Value = model.applyaddress_no;
                obj_cmd.Parameters.Add("@APPLYADDRESS_MOO", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_MOO"].Value = model.applyaddress_moo;
                obj_cmd.Parameters.Add("@APPLYADDRESS_SOI", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_SOI"].Value = model.applyaddress_soi;
                obj_cmd.Parameters.Add("@APPLYADDRESS_ROAD", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_ROAD"].Value = model.applyaddress_road;
                obj_cmd.Parameters.Add("@APPLYADDRESS_TAMBON", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_TAMBON"].Value = model.applyaddress_tambon;
                obj_cmd.Parameters.Add("@APPLYADDRESS_AMPHUR", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_AMPHUR"].Value = model.applyaddress_amphur;
                obj_cmd.Parameters.Add("@APPLYPROVINCE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYPROVINCE_CODE"].Value = model.applyprovince_code;
                obj_cmd.Parameters.Add("@APPLYADDRESS_ZIPCODE", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_ZIPCODE"].Value = model.applyaddress_zipcode;
                obj_cmd.Parameters.Add("@APPLYADDRESS_TEL", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_TEL"].Value = model.applyaddress_tel;
                obj_cmd.Parameters.Add("@APPLYADDRESS_EMAIL", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_EMAIL"].Value = model.applyaddress_email;
                obj_cmd.Parameters.Add("@APPLYADDRESS_LINE", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_LINE"].Value = model.applyaddress_line;
                obj_cmd.Parameters.Add("@APPLYADDRESS_FACEBOOK", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_FACEBOOK"].Value = model.applyaddress_facebook;
                
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                strResult = model.applyaddress_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "REQADD005:" + ex.ToString();
                strResult = "";
            }

            return blnResult;
        }

        public bool update(cls_TRApplyaddress model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE REQ_TR_APPLYADDRESS SET ");

                obj_str.Append("APPLYADDRESS_TYPE=@APPLYADDRESS_TYPE ");
                obj_str.Append(", APPLYADDRESS_NO=@APPLYADDRESS_NO ");
                obj_str.Append(", APPLYADDRESS_MOO=@APPLYADDRESS_MOO ");
                obj_str.Append(", APPLYADDRESS_SOI=@APPLYADDRESS_SOI ");
                obj_str.Append(", APPLYADDRESS_ROAD=@APPLYADDRESS_ROAD ");
                obj_str.Append(", APPLYADDRESS_TAMBON=@APPLYADDRESS_TAMBON ");
                obj_str.Append(", APPLYADDRESS_AMPHUR=@APPLYADDRESS_AMPHUR ");
                obj_str.Append(", APPLYPROVINCE_CODE=@APPLYPROVINCE_CODE ");
                obj_str.Append(", APPLYADDRESS_ZIPCODE=@APPLYADDRESS_ZIPCODE ");
                obj_str.Append(", APPLYADDRESS_TEL=@APPLYADDRESS_TEL ");
                obj_str.Append(", APPLYADDRESS_EMAIL=@APPLYADDRESS_EMAIL ");
                obj_str.Append(", APPLYADDRESS_LINE=@APPLYADDRESS_LINE ");
                obj_str.Append(", APPLYADDRESS_FACEBOOK=@APPLYADDRESS_FACEBOOK ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND APPLYWORK_CODE=@APPLYWORK_CODE ");
                obj_str.Append(" AND APPLYADDRESS_ID=@APPLYADDRESS_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@APPLYADDRESS_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_TYPE"].Value = model.applyaddress_type;
                obj_cmd.Parameters.Add("@APPLYADDRESS_NO", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_NO"].Value = model.applyaddress_no;
                obj_cmd.Parameters.Add("@APPLYADDRESS_MOO", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_MOO"].Value = model.applyaddress_moo;
                obj_cmd.Parameters.Add("@APPLYADDRESS_SOI", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_SOI"].Value = model.applyaddress_soi;
                obj_cmd.Parameters.Add("@APPLYADDRESS_ROAD", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_ROAD"].Value = model.applyaddress_road;
                obj_cmd.Parameters.Add("@APPLYADDRESS_TAMBON", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_TAMBON"].Value = model.applyaddress_tambon;
                obj_cmd.Parameters.Add("@APPLYADDRESS_AMPHUR", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_AMPHUR"].Value = model.applyaddress_amphur;
                obj_cmd.Parameters.Add("@APPLYPROVINCE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYPROVINCE_CODE"].Value = model.applyprovince_code;
                obj_cmd.Parameters.Add("@APPLYADDRESS_ZIPCODE", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_ZIPCODE"].Value = model.applyaddress_zipcode;
                obj_cmd.Parameters.Add("@APPLYADDRESS_TEL", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_TEL"].Value = model.applyaddress_tel;
                obj_cmd.Parameters.Add("@APPLYADDRESS_EMAIL", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_EMAIL"].Value = model.applyaddress_email;
                obj_cmd.Parameters.Add("@APPLYADDRESS_LINE", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_LINE"].Value = model.applyaddress_line;
                obj_cmd.Parameters.Add("@APPLYADDRESS_FACEBOOK", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYADDRESS_FACEBOOK"].Value = model.applyaddress_facebook;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@APPLYWORK_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYWORK_CODE"].Value = model.applywork_code;
                obj_cmd.Parameters.Add("@APPLYADDRESS_ID", SqlDbType.Int); obj_cmd.Parameters["@APPLYADDRESS_ID"].Value = model.applyaddress_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "REQADD006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
