using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRAddress
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRAddress() { }

        public string getMessage() { return this.Message.Replace("EMP_TR_ADDRESS", "").Replace("cls_ctTRAddress", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRAddress> getData(string condition)
        {
            List<cls_TRAddress> list_model = new List<cls_TRAddress>();
            cls_TRAddress model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");
                obj_str.Append(", ADDRESS_ID");
                obj_str.Append(", ADDRESS_TYPE");
                obj_str.Append(", ISNULL(ADDRESS_NO, '') AS ADDRESS_NO");
                obj_str.Append(", ISNULL(ADDRESS_MOO, '') AS ADDRESS_MOO");
                obj_str.Append(", ISNULL(ADDRESS_SOI, '') AS ADDRESS_SOI");
                obj_str.Append(", ISNULL(ADDRESS_ROAD, '') AS ADDRESS_ROAD");
                obj_str.Append(", ISNULL(ADDRESS_TAMBON, '') AS ADDRESS_TAMBON");
                obj_str.Append(", ISNULL(ADDRESS_AMPHUR, '') AS ADDRESS_AMPHUR");
                obj_str.Append(", ISNULL(PROVINCE_CODE, '') AS PROVINCE_CODE");
                obj_str.Append(", ISNULL(ADDRESS_ZIPCODE, '') AS ADDRESS_ZIPCODE");
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM EMP_TR_ADDRESS");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRAddress();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                    model.worker_code = Convert.ToString(dr["WORKER_CODE"]);
                    model.address_id = Convert.ToInt32(dr["ADDRESS_ID"]);
                    model.address_type = Convert.ToString(dr["ADDRESS_TYPE"]);
                    model.address_no = Convert.ToString(dr["ADDRESS_NO"]);
                    model.address_moo = Convert.ToString(dr["ADDRESS_MOO"]);
                    model.address_soi = Convert.ToString(dr["ADDRESS_SOI"]);
                    model.address_road = Convert.ToString(dr["ADDRESS_ROAD"]);
                    model.address_tambon = Convert.ToString(dr["ADDRESS_TAMBON"]);
                    model.address_amphur = Convert.ToString(dr["ADDRESS_AMPHUR"]);
                    model.province_code = Convert.ToString(dr["PROVINCE_CODE"]);
                    model.address_zipcode = Convert.ToString(dr["ADDRESS_ZIPCODE"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "EMPADD001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRAddress> getDataByFillter(string com ,string emp)
        {
            string strCondition = "";

            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!emp.Equals(""))
                strCondition += " AND WORKER_CODE=" + emp ;

 
            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(ADDRESS_ID, 1) ");
                obj_str.Append(" FROM EMP_TR_ADDRESS");
                obj_str.Append(" ORDER BY ADDRESS_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPADD002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string emp,string id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ADDRESS_ID");
                obj_str.Append(" FROM EMP_TR_ADDRESS");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");
                if (!id.ToString().Equals(""))
                {
                    obj_str.Append(" AND ADDRESS_ID='" + id + "' ");
                }

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "EMPADD003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM EMP_TR_ADDRESS");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "' ");
                obj_str.Append(" AND WORKER_CODE='" + emp + "' ");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "EMPADD004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRAddress model)
        {
            bool blnResult = false;
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.company_code,model.worker_code,model.address_id.ToString()))
                {
                    return this.update(model);
                    //if (this.update(model))
                    //    return model.address_id.ToString();
                    //else
                    //    return "";
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO EMP_TR_ADDRESS");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", ADDRESS_ID ");
                obj_str.Append(", ADDRESS_TYPE ");
                obj_str.Append(", ADDRESS_NO ");
                obj_str.Append(", ADDRESS_MOO ");
                obj_str.Append(", ADDRESS_SOI ");
                obj_str.Append(", ADDRESS_ROAD ");
                obj_str.Append(", ADDRESS_TAMBON ");
                obj_str.Append(", ADDRESS_AMPHUR ");
                obj_str.Append(", PROVINCE_CODE ");
                obj_str.Append(", ADDRESS_ZIPCODE ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @ADDRESS_ID ");
                obj_str.Append(", @ADDRESS_TYPE ");
                obj_str.Append(", @ADDRESS_NO ");
                obj_str.Append(", @ADDRESS_MOO  ");
                obj_str.Append(", @ADDRESS_SOI ");
                obj_str.Append(", @ADDRESS_ROAD ");
                obj_str.Append(", @ADDRESS_TAMBON ");
                obj_str.Append(", @ADDRESS_AMPHUR ");
                obj_str.Append(", @PROVINCE_CODE ");
                obj_str.Append(", @ADDRESS_ZIPCODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.address_id = this.getNextID();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@ADDRESS_ID", SqlDbType.Int); obj_cmd.Parameters["@ADDRESS_ID"].Value = model.address_id;
                obj_cmd.Parameters.Add("@ADDRESS_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@ADDRESS_TYPE"].Value = model.address_type;
                obj_cmd.Parameters.Add("@ADDRESS_NO", SqlDbType.VarChar); obj_cmd.Parameters["@ADDRESS_NO"].Value = model.address_no;
                obj_cmd.Parameters.Add("@ADDRESS_MOO", SqlDbType.VarChar); obj_cmd.Parameters["@ADDRESS_MOO"].Value = model.address_moo;
                obj_cmd.Parameters.Add("@ADDRESS_SOI", SqlDbType.VarChar); obj_cmd.Parameters["@ADDRESS_SOI"].Value = model.address_soi;
                obj_cmd.Parameters.Add("@ADDRESS_ROAD", SqlDbType.VarChar); obj_cmd.Parameters["@ADDRESS_ROAD"].Value = model.address_road;
                obj_cmd.Parameters.Add("@ADDRESS_TAMBON", SqlDbType.VarChar); obj_cmd.Parameters["@ADDRESS_TAMBON"].Value = model.address_tambon;
                obj_cmd.Parameters.Add("@ADDRESS_AMPHUR", SqlDbType.VarChar); obj_cmd.Parameters["@ADDRESS_AMPHUR"].Value = model.address_amphur;
                obj_cmd.Parameters.Add("@PROVINCE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROVINCE_CODE"].Value = model.province_code;
                obj_cmd.Parameters.Add("@ADDRESS_ZIPCODE", SqlDbType.VarChar); obj_cmd.Parameters["@ADDRESS_ZIPCODE"].Value = model.address_zipcode;
                
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
                strResult = model.address_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "EMPADD005:" + ex.ToString();
                strResult = "";
            }

            return blnResult;
        }

        public bool update(cls_TRAddress model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE EMP_TR_ADDRESS SET ");

                obj_str.Append("ADDRESS_TYPE=@ADDRESS_TYPE ");
                obj_str.Append(", ADDRESS_NO=@ADDRESS_NO ");
                obj_str.Append(", ADDRESS_MOO=@ADDRESS_MOO ");
                obj_str.Append(", ADDRESS_SOI=@ADDRESS_SOI ");
                obj_str.Append(", ADDRESS_ROAD=@ADDRESS_ROAD ");
                obj_str.Append(", ADDRESS_TAMBON=@ADDRESS_TAMBON ");
                obj_str.Append(", ADDRESS_AMPHUR=@ADDRESS_AMPHUR ");
                obj_str.Append(", PROVINCE_CODE=@PROVINCE_CODE ");
                obj_str.Append(", ADDRESS_ZIPCODE=@ADDRESS_ZIPCODE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");
                obj_str.Append(" AND ADDRESS_ID=@ADDRESS_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("ADDRESS_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@ADDRESS_TYPE"].Value = model.address_type;
                obj_cmd.Parameters.Add("@ADDRESS_NO", SqlDbType.VarChar); obj_cmd.Parameters["@ADDRESS_NO"].Value = model.address_no;
                obj_cmd.Parameters.Add("@ADDRESS_MOO", SqlDbType.VarChar); obj_cmd.Parameters["@ADDRESS_MOO"].Value = model.address_moo;
                obj_cmd.Parameters.Add("@ADDRESS_SOI", SqlDbType.VarChar); obj_cmd.Parameters["@ADDRESS_SOI"].Value = model.address_soi;
                obj_cmd.Parameters.Add("@ADDRESS_ROAD", SqlDbType.VarChar); obj_cmd.Parameters["@ADDRESS_ROAD"].Value = model.address_road;
                obj_cmd.Parameters.Add("@ADDRESS_TAMBON", SqlDbType.VarChar); obj_cmd.Parameters["@ADDRESS_TAMBON"].Value = model.address_tambon;
                obj_cmd.Parameters.Add("@ADDRESS_AMPHUR", SqlDbType.VarChar); obj_cmd.Parameters["@ADDRESS_AMPHUR"].Value = model.address_amphur;
                obj_cmd.Parameters.Add("@PROVINCE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROVINCE_CODE"].Value = model.province_code;
                obj_cmd.Parameters.Add("@ADDRESS_ZIPCODE", SqlDbType.VarChar); obj_cmd.Parameters["@ADDRESS_ZIPCODE"].Value = model.address_zipcode;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@ADDRESS_ID", SqlDbType.Int); obj_cmd.Parameters["@ADDRESS_ID"].Value = model.address_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "EMPADD006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
