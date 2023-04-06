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
    public class cls_ctMTComaddlocation
    {
   
     string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTComaddlocation() { }

        public string getMessage() { return this.Message.Replace("SYS_MT_COMADDLOCATION", "").Replace("cls_ctMTComaddlocation", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }


        private List<cls_MTComaddlocation> getData(string condition)
        {
            List<cls_MTComaddlocation> list_model = new List<cls_MTComaddlocation>();
            cls_MTComaddlocation model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", COMLOCATION_CODE");
                obj_str.Append(", COMADDRESS_TYPE");

                obj_str.Append(", ISNULL(COMADDLOCATIONTH_NO, '') AS COMADDLOCATIONTH_NO");
                obj_str.Append(", ISNULL(COMADDLOCATIONTH_MOO, '') AS COMADDLOCATIONTH_MOO");
                obj_str.Append(", ISNULL(COMADDLOCATIONTH_SOI, '') AS COMADDLOCATIONTH_SOI");
                obj_str.Append(", ISNULL(COMADDLOCATIONTH_ROAD, '') AS COMADDLOCATIONTH_ROAD");
                obj_str.Append(", ISNULL(COMADDLOCATIONTH_TAMBON, '') AS COMADDLOCATIONTH_TAMBON");
                obj_str.Append(", ISNULL(COMADDLOCATIONTH_AMPHUR, '') AS COMADDLOCATIONTH_AMPHUR");
                obj_str.Append(", ISNULL(PROVINCETH_CODE, '') AS PROVINCETH_CODE");

                obj_str.Append(", ISNULL(COMADDLOCATIONEN_NO, '') AS COMADDLOCATIONEN_NO");
                obj_str.Append(", ISNULL(COMADDLOCATIONEN_MOO, '') AS COMADDLOCATIONEN_MOO");
                obj_str.Append(", ISNULL(COMADDLOCATIONEN_SOI, '') AS COMADDLOCATIONEN_SOI");
                obj_str.Append(", ISNULL(COMADDLOCATIONEN_ROAD, '') AS COMADDLOCATIONEN_ROAD");
                obj_str.Append(", ISNULL(COMADDLOCATIONEN_TAMBON, '') AS COMADDLOCATIONEN_TAMBON");
                obj_str.Append(", ISNULL(COMADDLOCATIONEN_AMPHUR, '') AS COMADDLOCATIONEN_AMPHUR");
                obj_str.Append(", ISNULL(COMADDLOCATION_ZIPCODE, '') AS COMADDLOCATION_ZIPCODE");
                obj_str.Append(", ISNULL(PROVINCEEN_CODE, '') AS PROVINCEEN_CODE");

                obj_str.Append(", ISNULL(COMADDLOCATION_TEL, '') AS COMADDLOCATION_TEL");
                obj_str.Append(", ISNULL(COMADDLOCATION_EMAIL, '') AS COMADDLOCATION_EMAIL");
                obj_str.Append(", ISNULL(COMADDLOCATION_LINE, '') AS COMADDLOCATION_LINE");
                obj_str.Append(", ISNULL(COMADDLOCATION_FACEBOOK, '') AS COMADDLOCATION_FACEBOOK");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM SYS_MT_COMADDLOCATION");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE,COMLOCATION_CODE, COMADDRESS_TYPE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTComaddlocation();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                    model.comlocation_code = Convert.ToString(dr["COMLOCATION_CODE"]);

                    model.comaddress_type = Convert.ToString(dr["COMADDRESS_TYPE"]);

                    model.comaddlocationth_no = Convert.ToString(dr["COMADDLOCATIONTH_NO"]);
                    model.comaddlocationth_moo = Convert.ToString(dr["COMADDLOCATIONTH_MOO"]);
                    model.comaddlocationth_soi = Convert.ToString(dr["COMADDLOCATIONTH_SOI"]);
                    model.comaddlocationth_road = Convert.ToString(dr["COMADDLOCATIONTH_ROAD"]);
                    model.comaddlocationth_tambon = Convert.ToString(dr["COMADDLOCATIONTH_TAMBON"]);
                    model.comaddlocationth_amphur = Convert.ToString(dr["COMADDLOCATIONTH_AMPHUR"]);
                    model.provinceth_code = Convert.ToString(dr["PROVINCETH_CODE"]);

                    model.comaddlocationen_no = Convert.ToString(dr["COMADDLOCATIONEN_NO"]);
                    model.comaddlocationen_moo = Convert.ToString(dr["COMADDLOCATIONEN_MOO"]);
                    model.comaddlocationen_soi = Convert.ToString(dr["COMADDLOCATIONEN_SOI"]);
                    model.comaddlocationen_road = Convert.ToString(dr["COMADDLOCATIONEN_ROAD"]);
                    model.comaddlocationen_tambon = Convert.ToString(dr["COMADDLOCATIONEN_TAMBON"]);
                    model.comaddlocationen_amphur = Convert.ToString(dr["COMADDLOCATIONEN_AMPHUR"]);
                    model.comaddlocation_zipcode = Convert.ToString(dr["COMADDLOCATION_ZIPCODE"]);
                    model.provinceen_code = Convert.ToString(dr["PROVINCEEN_CODE"]);

                    model.comaddlocation_tel = Convert.ToString(dr["COMADDLOCATION_TEL"]);
                    model.comaddlocation_email = Convert.ToString(dr["COMADDLOCATION_EMAIL"]);
                    model.comaddlocation_line = Convert.ToString(dr["COMADDLOCATION_LINE"]);
                    model.comaddlocation_facebook = Convert.ToString(dr["COMADDLOCATION_FACEBOOK"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ADDL001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTComaddlocation> getDataByFillter(string com, string code, string type)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!code.Equals(""))
                strCondition += " AND COMLOCATION_CODE='" + code + "'";

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

                obj_str.Append("SELECT ISNULL(COMADDLOCATION_ID, 1) ");
                obj_str.Append(" FROM SYS_MT_COMADDLOCATION");
                obj_str.Append(" ORDER BY COMADDLOCATION_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ADDL002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string code, string type)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM SYS_MT_COMADDLOCATION");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND COMLOCATION_CODE='" + code + "' ");

                obj_str.Append(" AND COMADDRESS_TYPE='" + type + "' ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ADDL003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string code, string type)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SYS_MT_COMADDLOCATION");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND COMLOCATION_CODE='" + code + "' ");

                obj_str.Append(" AND COMADDRESS_TYPE='" + type + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ADDL004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_MTComaddlocation model)
        {
            bool blnResult = false;
            string strResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.comlocation_code, model.comaddress_type))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_MT_COMADDLOCATION");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", COMLOCATION_CODE ");

                obj_str.Append(", COMADDRESS_TYPE ");

                obj_str.Append(", COMADDLOCATIONTH_NO ");
                obj_str.Append(", COMADDLOCATIONTH_MOO ");
                obj_str.Append(", COMADDLOCATIONTH_SOI ");
                obj_str.Append(", COMADDLOCATIONTH_ROAD ");
                obj_str.Append(", COMADDLOCATIONTH_TAMBON ");
                obj_str.Append(", COMADDLOCATIONTH_AMPHUR ");
                obj_str.Append(", PROVINCETH_CODE ");

                obj_str.Append(", COMADDLOCATIONEN_NO ");
                obj_str.Append(", COMADDLOCATIONEN_MOO ");
                obj_str.Append(", COMADDLOCATIONEN_SOI ");
                obj_str.Append(", COMADDLOCATIONEN_ROAD ");
                obj_str.Append(", COMADDLOCATIONEN_TAMBON ");
                obj_str.Append(", COMADDLOCATIONEN_AMPHUR ");
                obj_str.Append(", COMADDLOCATION_ZIPCODE ");
                obj_str.Append(", PROVINCEEN_CODE ");


                obj_str.Append(", COMADDLOCATION_TEL ");
                obj_str.Append(", COMADDLOCATION_EMAIL ");
                obj_str.Append(", COMADDLOCATION_LINE ");
                obj_str.Append(", COMADDLOCATION_FACEBOOK ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @COMLOCATION_CODE ");
                obj_str.Append(", @COMADDRESS_TYPE ");

                obj_str.Append(", @COMADDLOCATIONTH_NO ");
                obj_str.Append(", @COMADDLOCATIONTH_MOO ");
                obj_str.Append(", @COMADDLOCATIONTH_SOI ");
                obj_str.Append(", @COMADDLOCATIONTH_ROAD ");
                obj_str.Append(", @COMADDLOCATIONTH_TAMBON ");
                obj_str.Append(", @COMADDLOCATIONTH_AMPHUR ");
                obj_str.Append(", @PROVINCETH_CODE ");

                obj_str.Append(", @COMADDLOCATIONEN_NO ");
                obj_str.Append(", @COMADDLOCATIONEN_MOO ");
                obj_str.Append(", @COMADDLOCATIONEN_SOI ");
                obj_str.Append(", @COMADDLOCATIONEN_ROAD ");
                obj_str.Append(", @COMADDLOCATIONEN_TAMBON ");
                obj_str.Append(", @COMADDLOCATIONEN_AMPHUR ");
                obj_str.Append(", @COMADDLOCATION_ZIPCODE ");
                obj_str.Append(", @PROVINCEEN_CODE ");


                obj_str.Append(", @COMADDLOCATION_TEL ");
                obj_str.Append(", @COMADDLOCATION_EMAIL ");
                obj_str.Append(", @COMADDLOCATION_LINE ");
                obj_str.Append(", @COMADDLOCATION_FACEBOOK ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                //model.address_id = this.getNextID();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@COMLOCATION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMLOCATION_CODE"].Value = model.comlocation_code;

                obj_cmd.Parameters.Add("@COMADDRESS_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESS_TYPE"].Value = model.comaddress_type;

                obj_cmd.Parameters.Add("@COMADDLOCATIONTH_NO", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATIONTH_NO"].Value = model.comaddlocationth_no;
                obj_cmd.Parameters.Add("@COMADDLOCATIONTH_MOO", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATIONTH_MOO"].Value = model.comaddlocationth_moo;
                obj_cmd.Parameters.Add("@COMADDLOCATIONTH_SOI", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATIONTH_SOI"].Value = model.comaddlocationth_soi;
                obj_cmd.Parameters.Add("@COMADDLOCATIONTH_ROAD", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATIONTH_ROAD"].Value = model.comaddlocationth_road;
                obj_cmd.Parameters.Add("@COMADDLOCATIONTH_TAMBON", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATIONTH_TAMBON"].Value = model.comaddlocationth_tambon;
                obj_cmd.Parameters.Add("@COMADDLOCATIONTH_AMPHUR", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATIONTH_AMPHUR"].Value = model.comaddlocationth_amphur;
                obj_cmd.Parameters.Add("@PROVINCETH_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROVINCETH_CODE"].Value = model.provinceth_code;

                obj_cmd.Parameters.Add("@COMADDLOCATIONEN_NO", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATIONEN_NO"].Value = model.comaddlocationen_no;
                obj_cmd.Parameters.Add("@COMADDLOCATIONEN_MOO", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATIONEN_MOO"].Value = model.comaddlocationen_moo;
                obj_cmd.Parameters.Add("@COMADDLOCATIONEN_SOI", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATIONEN_SOI"].Value = model.comaddlocationen_soi;
                obj_cmd.Parameters.Add("@COMADDLOCATIONEN_ROAD", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATIONEN_ROAD"].Value = model.comaddlocationen_road;
                obj_cmd.Parameters.Add("@COMADDLOCATIONEN_TAMBON", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATIONEN_TAMBON"].Value = model.comaddlocationen_tambon;
                obj_cmd.Parameters.Add("@COMADDLOCATIONEN_AMPHUR", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATIONEN_AMPHUR"].Value = model.comaddlocationen_amphur;
                obj_cmd.Parameters.Add("@COMADDLOCATION_ZIPCODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATION_ZIPCODE"].Value = model.comaddlocation_zipcode;
                obj_cmd.Parameters.Add("@PROVINCEEN_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROVINCEEN_CODE"].Value = model.provinceen_code;

                obj_cmd.Parameters.Add("@COMADDLOCATION_TEL", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATION_TEL"].Value = model.comaddlocation_tel;
                obj_cmd.Parameters.Add("@COMADDLOCATION_EMAIL", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATION_EMAIL"].Value = model.comaddlocation_email;
                obj_cmd.Parameters.Add("@COMADDLOCATION_LINE", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATION_LINE"].Value = model.comaddlocation_line;
                obj_cmd.Parameters.Add("@COMADDLOCATION_FACEBOOK", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATION_FACEBOOK"].Value = model.comaddlocation_facebook;

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
                Message = "ADDL005:" + ex.ToString();
                strResult = "";
            }

            return blnResult;
        }

        public bool update(cls_MTComaddlocation model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE SYS_MT_COMADDLOCATION SET ");


                obj_str.Append(" COMADDLOCATIONTH_NO=@COMADDLOCATIONTH_NO ");
                obj_str.Append(", COMADDLOCATIONTH_MOO=@COMADDLOCATIONTH_MOO ");
                obj_str.Append(", COMADDLOCATIONTH_SOI=@COMADDLOCATIONTH_SOI ");
                obj_str.Append(", COMADDLOCATIONTH_ROAD=@COMADDLOCATIONTH_ROAD ");
                obj_str.Append(", COMADDLOCATIONTH_TAMBON=@COMADDLOCATIONTH_TAMBON ");
                obj_str.Append(", COMADDLOCATIONTH_AMPHUR=@COMADDLOCATIONTH_AMPHUR ");
                obj_str.Append(", PROVINCETH_CODE=@PROVINCETH_CODE ");


                obj_str.Append(", COMADDLOCATIONEN_NO=@COMADDLOCATIONEN_NO ");
                obj_str.Append(", COMADDLOCATIONEN_MOO=@COMADDLOCATIONEN_MOO ");
                obj_str.Append(", COMADDLOCATIONEN_SOI=@COMADDLOCATIONEN_SOI ");
                obj_str.Append(", COMADDLOCATIONEN_ROAD=@COMADDLOCATIONEN_ROAD ");
                obj_str.Append(", COMADDLOCATIONEN_TAMBON=@COMADDLOCATIONEN_TAMBON ");
                obj_str.Append(", COMADDLOCATIONEN_AMPHUR=@COMADDLOCATIONEN_AMPHUR ");
                obj_str.Append(", PROVINCEEN_CODE=@PROVINCEEN_CODE ");


                obj_str.Append(", COMADDLOCATION_ZIPCODE=@COMADDLOCATION_ZIPCODE ");
                obj_str.Append(", COMADDLOCATION_TEL=@COMADDLOCATION_TEL ");
                obj_str.Append(", COMADDLOCATION_EMAIL=@COMADDLOCATION_EMAIL ");
                obj_str.Append(", COMADDLOCATION_LINE=@COMADDLOCATION_LINE ");
                obj_str.Append(", COMADDLOCATION_FACEBOOK=@COMADDLOCATION_FACEBOOK ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND COMLOCATION_CODE=@COMLOCATION_CODE ");

                obj_str.Append(" AND COMADDRESS_TYPE=@COMADDRESS_TYPE ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMADDLOCATIONTH_NO", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATIONTH_NO"].Value = model.comaddlocationth_no;
                obj_cmd.Parameters.Add("@COMADDLOCATIONTH_MOO", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATIONTH_MOO"].Value = model.comaddlocationth_moo;
                obj_cmd.Parameters.Add("@COMADDLOCATIONTH_SOI", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATIONTH_SOI"].Value = model.comaddlocationth_soi;
                obj_cmd.Parameters.Add("@COMADDLOCATIONTH_ROAD", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATIONTH_ROAD"].Value = model.comaddlocationth_road;
                obj_cmd.Parameters.Add("@COMADDLOCATIONTH_TAMBON", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATIONTH_TAMBON"].Value = model.comaddlocationth_tambon;
                obj_cmd.Parameters.Add("@COMADDLOCATIONTH_AMPHUR", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATIONTH_AMPHUR"].Value = model.comaddlocationth_amphur;
                obj_cmd.Parameters.Add("@PROVINCETH_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROVINCETH_CODE"].Value = model.provinceth_code;


                obj_cmd.Parameters.Add("@COMADDLOCATIONEN_NO", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATIONEN_NO"].Value = model.comaddlocationen_no;
                obj_cmd.Parameters.Add("@COMADDLOCATIONEN_MOO", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATIONEN_MOO"].Value = model.comaddlocationen_moo;
                obj_cmd.Parameters.Add("@COMADDLOCATIONEN_SOI", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATIONEN_SOI"].Value = model.comaddlocationen_soi;
                obj_cmd.Parameters.Add("@COMADDLOCATIONEN_ROAD", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATIONEN_ROAD"].Value = model.comaddlocationen_road;
                obj_cmd.Parameters.Add("@CCOMADDLOCATIONEN_TAMBON", SqlDbType.VarChar); obj_cmd.Parameters["@CCOMADDLOCATIONEN_TAMBON"].Value = model.comaddlocationen_tambon;
                obj_cmd.Parameters.Add("@COMADDLOCATIONEN_AMPHUR", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATIONEN_AMPHUR"].Value = model.comaddlocationen_amphur;
                obj_cmd.Parameters.Add("@COMADDLOCATION_ZIPCODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATION_ZIPCODE"].Value = model.comaddlocation_zipcode;
                obj_cmd.Parameters.Add("@PROVINCEEN_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROVINCEEN_CODE"].Value = model.provinceen_code;



                obj_cmd.Parameters.Add("@COMADDLOCATION_TEL", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATION_TEL"].Value = model.comaddlocation_tel;
                obj_cmd.Parameters.Add("@COMADDLOCATION_EMAIL", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATION_EMAIL"].Value = model.comaddlocation_email;
                obj_cmd.Parameters.Add("@COMADDLOCATION_LINE", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATION_LINE"].Value = model.comaddlocation_line;
                obj_cmd.Parameters.Add("@COMADDLOCATION_FACEBOOK", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDLOCATION_FACEBOOK"].Value = model.comaddlocation_facebook;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@COMLOCATION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMLOCATION_CODE"].Value = model.comlocation_code;

                obj_cmd.Parameters.Add("@COMADDRESS_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@COMADDRESS_TYPE"].Value = model.comaddress_type;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ADDL006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}