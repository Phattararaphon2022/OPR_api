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
    public class cls_ctTRProaddress
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRProaddress() { }

        public string getMessage() { return this.Message.Replace("PRO_TR_PROADDRESS", "").Replace("cls_ctTRProaddress", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRProaddress> getData(string condition)
        {
            List<cls_TRProaddress> list_model = new List<cls_TRProaddress>();
            cls_TRProaddress model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PROADDRESS_ID");
                obj_str.Append(", PROJECT_CODE");
                obj_str.Append(", PROADDRESS_TYPE");
                obj_str.Append(", ISNULL(PROADDRESS_NO, '') AS PROADDRESS_NO");
                obj_str.Append(", ISNULL(PROADDRESS_MOO, '') AS PROADDRESS_MOO");
                obj_str.Append(", ISNULL(PROADDRESS_SOI, '') AS PROADDRESS_SOI");
                obj_str.Append(", ISNULL(PROADDRESS_ROAD, '') AS PROADDRESS_ROAD");
                obj_str.Append(", ISNULL(PROADDRESS_TAMBON, '') AS PROADDRESS_TAMBON");
                obj_str.Append(", ISNULL(PROADDRESS_AMPHUR, '') AS PROADDRESS_AMPHUR");
                obj_str.Append(", ISNULL(PROADDRESS_ZIPCODE, '') AS PROADDRESS_ZIPCODE");
                obj_str.Append(", ISNULL(PROADDRESS_TEL, '') AS PROADDRESS_TEL");
                obj_str.Append(", ISNULL(PROADDRESS_EMAIL, '') AS PROADDRESS_EMAIL");
                obj_str.Append(", ISNULL(PROADDRESS_LINE, '') AS PROADDRESS_LINE");
                obj_str.Append(", ISNULL(PROADDRESS_FACEBOOK, '') AS PROADDRESS_FACEBOOK");
                obj_str.Append(", ISNULL(PROVINCE_CODE, '') AS PROVINCE_CODE");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PRO_TR_PROADDRESS");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PROJECT_CODE, PROADDRESS_TYPE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRProaddress();

                    model.proaddress_id = Convert.ToInt32(dr["PROADDRESS_ID"]);

                    model.project_code = Convert.ToString(dr["PROJECT_CODE"]);
                    model.proaddress_type = Convert.ToString(dr["PROADDRESS_TYPE"]);
                    model.proaddress_no = Convert.ToString(dr["PROADDRESS_NO"]);
                    model.proaddress_moo = Convert.ToString(dr["PROADDRESS_MOO"]);
                    model.proaddress_soi = Convert.ToString(dr["PROADDRESS_SOI"]);
                    model.proaddress_road = Convert.ToString(dr["PROADDRESS_ROAD"]);
                    model.proaddress_tambon = Convert.ToString(dr["PROADDRESS_TAMBON"]);
                    model.proaddress_amphur = Convert.ToString(dr["PROADDRESS_AMPHUR"]);
                    model.proaddress_zipcode = Convert.ToString(dr["PROADDRESS_ZIPCODE"]);
                    model.proaddress_tel = Convert.ToString(dr["PROADDRESS_TEL"]);
                    model.proaddress_email = Convert.ToString(dr["PROADDRESS_EMAIL"]);
                    model.proaddress_line = Convert.ToString(dr["PROADDRESS_LINE"]);
                    model.proaddress_facebook = Convert.ToString(dr["PROADDRESS_FACEBOOK"]);
                    model.province_code = Convert.ToString(dr["PROVINCE_CODE"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "BNK001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRProaddress> getDataByFillter(string project)
        {
            string strCondition = "";

            if (!project.Equals(""))
                strCondition += " AND PROJECT_CODE='" + project + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(PROADDRESS_ID, 1) ");
                obj_str.Append(" FROM PRO_MT_PROADDRESS");
                obj_str.Append(" ORDER BY PROADDRESS_ID DESC ");

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

        public bool checkDataOld(string project, string type)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PROJECT_CODE");
                obj_str.Append(" FROM PRO_TR_PROADDRESS");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROADDRESS_TYPE='" + type + "'");

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

        public bool delete(string project, string type)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_TR_PROADDRESS");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                if (!type.Equals(""))
                {
                    obj_str.Append(" AND PROADDRESS_TYPE='" + type + "'");
                }

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BNK004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRProaddress model)
        {
            bool blnResult = false;
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.project_code, model.proaddress_type))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PRO_TR_PROADDRESS");
                obj_str.Append(" (");                
                obj_str.Append("PROADDRESS_ID ");
                obj_str.Append(", PROADDRESS_TYPE ");
                obj_str.Append(", PROADDRESS_NO ");
                obj_str.Append(", PROADDRESS_MOO ");
                obj_str.Append(", PROADDRESS_SOI ");
                obj_str.Append(", PROADDRESS_ROAD ");
                obj_str.Append(", PROADDRESS_TAMBON ");
                obj_str.Append(", PROADDRESS_AMPHUR ");
                obj_str.Append(", PROADDRESS_ZIPCODE ");
                obj_str.Append(", PROADDRESS_TEL ");
                obj_str.Append(", PROADDRESS_EMAIL ");
                obj_str.Append(", PROADDRESS_LINE ");
                obj_str.Append(", PROADDRESS_FACEBOOK ");
                obj_str.Append(", PROVINCE_CODE ");
                obj_str.Append(", PROJECT_CODE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
           
                obj_str.Append("@PROADDRESS_ID ");
                obj_str.Append(", @PROADDRESS_TYPE ");
                obj_str.Append(", @PROADDRESS_NO ");
                obj_str.Append(", @PROADDRESS_MOO ");
                obj_str.Append(", @PROADDRESS_SOI ");
                obj_str.Append(", @PROADDRESS_ROAD ");
                obj_str.Append(", @PROADDRESS_TAMBON ");
                obj_str.Append(", @PROADDRESS_AMPHUR ");
                obj_str.Append(", @PROADDRESS_ZIPCODE ");
                obj_str.Append(", @PROADDRESS_TEL ");
                obj_str.Append(", @PROADDRESS_EMAIL ");
                obj_str.Append(", @PROADDRESS_LINE ");
                obj_str.Append(", @PROADDRESS_FACEBOOK ");
                obj_str.Append(", @PROVINCE_CODE ");
                obj_str.Append(", @PROJECT_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROADDRESS_ID", SqlDbType.Int); obj_cmd.Parameters["@PROADDRESS_ID"].Value = this.getNextID();              
                obj_cmd.Parameters.Add("@PROADDRESS_TYPE", SqlDbType.Char); obj_cmd.Parameters["@PROADDRESS_TYPE"].Value = Convert.ToChar(model.proaddress_type);
                obj_cmd.Parameters.Add("@PROADDRESS_NO", SqlDbType.VarChar); obj_cmd.Parameters["@PROADDRESS_NO"].Value = model.proaddress_no;
                obj_cmd.Parameters.Add("@PROADDRESS_MOO", SqlDbType.VarChar); obj_cmd.Parameters["@PROADDRESS_MOO"].Value = model.proaddress_moo;
                obj_cmd.Parameters.Add("@PROADDRESS_SOI", SqlDbType.VarChar); obj_cmd.Parameters["@PROADDRESS_SOI"].Value = model.proaddress_soi;
                obj_cmd.Parameters.Add("@PROADDRESS_ROAD", SqlDbType.VarChar); obj_cmd.Parameters["@PROADDRESS_ROAD"].Value = model.proaddress_road;
                obj_cmd.Parameters.Add("@PROADDRESS_TAMBON", SqlDbType.VarChar); obj_cmd.Parameters["@PROADDRESS_TAMBON"].Value = model.proaddress_tambon;
                obj_cmd.Parameters.Add("@PROADDRESS_AMPHUR", SqlDbType.VarChar); obj_cmd.Parameters["@PROADDRESS_AMPHUR"].Value = model.proaddress_amphur;
                obj_cmd.Parameters.Add("@PROADDRESS_ZIPCODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROADDRESS_ZIPCODE"].Value = model.proaddress_zipcode;
                obj_cmd.Parameters.Add("@PROADDRESS_TEL", SqlDbType.VarChar); obj_cmd.Parameters["@PROADDRESS_TEL"].Value = model.proaddress_tel;
                obj_cmd.Parameters.Add("@PROADDRESS_EMAIL", SqlDbType.VarChar); obj_cmd.Parameters["@PROADDRESS_EMAIL"].Value = model.proaddress_email;
                obj_cmd.Parameters.Add("@PROADDRESS_LINE", SqlDbType.VarChar); obj_cmd.Parameters["@PROADDRESS_LINE"].Value = model.proaddress_line;
                obj_cmd.Parameters.Add("@PROADDRESS_FACEBOOK", SqlDbType.VarChar); obj_cmd.Parameters["@PROADDRESS_FACEBOOK"].Value = model.proaddress_facebook;
                obj_cmd.Parameters.Add("@PROVINCE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROVINCE_CODE"].Value = model.province_code;
                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

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

        public bool update(cls_TRProaddress model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PRO_TR_PROADDRESS SET ");

                obj_str.Append(" PROADDRESS_NO=@PROADDRESS_NO ");
                obj_str.Append(", PROADDRESS_MOO=@PROADDRESS_MOO ");
                obj_str.Append(", PROADDRESS_SOI=@PROADDRESS_SOI ");
                obj_str.Append(", PROADDRESS_ROAD=@PROADDRESS_ROAD ");
                obj_str.Append(", PROADDRESS_TAMBON=@PROADDRESS_TAMBON ");
                obj_str.Append(", PROADDRESS_AMPHUR=@PROADDRESS_AMPHUR ");
                obj_str.Append(", PROADDRESS_ZIPCODE=@PROADDRESS_ZIPCODE ");
                obj_str.Append(", PROADDRESS_TEL=@PROADDRESS_TEL ");
                obj_str.Append(", PROADDRESS_EMAIL=@PROADDRESS_EMAIL ");
                obj_str.Append(", PROADDRESS_LINE=@PROADDRESS_LINE ");
                obj_str.Append(", PROADDRESS_FACEBOOK=@PROADDRESS_FACEBOOK ");
                obj_str.Append(", PROVINCE_CODE=@PROVINCE_CODE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE PROADDRESS_ID=@PROADDRESS_ID ");
              

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROADDRESS_NO", SqlDbType.VarChar); obj_cmd.Parameters["@PROADDRESS_NO"].Value = model.proaddress_no;
                obj_cmd.Parameters.Add("@PROADDRESS_MOO", SqlDbType.VarChar); obj_cmd.Parameters["@PROADDRESS_MOO"].Value = model.proaddress_moo;
                obj_cmd.Parameters.Add("@PROADDRESS_SOI", SqlDbType.VarChar); obj_cmd.Parameters["@PROADDRESS_SOI"].Value = model.proaddress_soi;
                obj_cmd.Parameters.Add("@PROADDRESS_ROAD", SqlDbType.VarChar); obj_cmd.Parameters["@PROADDRESS_ROAD"].Value = model.proaddress_road;
                obj_cmd.Parameters.Add("@PROADDRESS_TAMBON", SqlDbType.VarChar); obj_cmd.Parameters["@PROADDRESS_TAMBON"].Value = model.proaddress_tambon;
                obj_cmd.Parameters.Add("@PROADDRESS_AMPHUR", SqlDbType.VarChar); obj_cmd.Parameters["@PROADDRESS_AMPHUR"].Value = model.proaddress_amphur;
                obj_cmd.Parameters.Add("@PROADDRESS_ZIPCODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROADDRESS_ZIPCODE"].Value = model.proaddress_zipcode;
                obj_cmd.Parameters.Add("@PROADDRESS_TEL", SqlDbType.VarChar); obj_cmd.Parameters["@PROADDRESS_TEL"].Value = model.proaddress_tel;
                obj_cmd.Parameters.Add("@PROADDRESS_EMAIL", SqlDbType.VarChar); obj_cmd.Parameters["@PROADDRESS_EMAIL"].Value = model.proaddress_email;
                obj_cmd.Parameters.Add("@PROADDRESS_LINE", SqlDbType.VarChar); obj_cmd.Parameters["@PROADDRESS_LINE"].Value = model.proaddress_line;
                obj_cmd.Parameters.Add("@PROADDRESS_FACEBOOK", SqlDbType.VarChar); obj_cmd.Parameters["@PROADDRESS_FACEBOOK"].Value = model.proaddress_facebook;
                obj_cmd.Parameters.Add("@PROVINCE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROVINCE_CODE"].Value = model.province_code;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@PROADDRESS_ID", SqlDbType.Int); obj_cmd.Parameters["@PROADDRESS_ID"].Value = model.proaddress_id;

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
