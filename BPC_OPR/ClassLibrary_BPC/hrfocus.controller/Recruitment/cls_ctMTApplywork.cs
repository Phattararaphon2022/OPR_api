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
    public class cls_ctMTApplywork
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTApplywork() { }

        public string getMessage() { return this.Message.Replace("REQ_MT_APPLYWORK", "").Replace("cls_ctMTApplywork", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTApplywork> getData(string condition)
        {
            List<cls_MTApplywork> list_model = new List<cls_MTApplywork>();
            cls_MTApplywork model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", APPLYWORK_ID");
                obj_str.Append(", APPLYWORK_CODE");
                obj_str.Append(", APPLYWORK_INITIAL");
                obj_str.Append(", ISNULL(APPLYWORK_FNAME_TH, '') AS APPLYWORK_FNAME_TH");
                obj_str.Append(", ISNULL(APPLYWORK_LNAME_TH, '') AS APPLYWORK_LNAME_TH");
                obj_str.Append(", ISNULL(APPLYWORK_FNAME_EN, '') AS APPLYWORK_FNAME_EN");
                obj_str.Append(", ISNULL(APPLYWORK_LNAME_EN, '') AS APPLYWORK_LNAME_EN");
                obj_str.Append(", APPLYWORK_BIRTHDATE");
                obj_str.Append(", PROVINCE_CODE");
                obj_str.Append(", BLOODTYPE_CODE");
                obj_str.Append(", APPLYWORK_HEIGHT");
                obj_str.Append(", APPLYWORK_WEIGHT");
                obj_str.Append(", ISNULL(APPLYWORK_STARTDATE, '01/01/2999') AS APPLYWORK_STARTDATE");

               
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");


                obj_str.Append(" FROM REQ_MT_APPLYWORK");

                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, APPLYWORK_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTApplywork();

                    model.company_code = dr["COMPANY_CODE"].ToString();

                    model.applywork_id = Convert.ToInt32(dr["APPLYWORK_ID"]);
                    model.applywork_code = dr["APPLYWORK_CODE"].ToString();
                    model.applywork_initial = dr["APPLYWORK_INITIAL"].ToString();
                    model.applywork_fname_th = dr["APPLYWORK_FNAME_TH"].ToString();
                    model.applywork_lname_th = dr["APPLYWORK_LNAME_TH"].ToString();
                    model.applywork_fname_en = dr["APPLYWORK_FNAME_EN"].ToString();
                    model.applywork_lname_en = dr["APPLYWORK_LNAME_EN"].ToString();


                    model.applywork_birthdate = Convert.ToDateTime(dr["APPLYWORK_BIRTHDATE"]);
                    model.applywork_startdate = Convert.ToDateTime(dr["APPLYWORK_STARTDATE"]);
                    model.province_code = dr["PROVINCE_CODE"].ToString();
                    model.bloodtype_code = dr["BLOODTYPE_CODE"].ToString();
                    model.applywork_height = Convert.ToDouble(dr["APPLYWORK_HEIGHT"]);
                    model.applywork_weight = Convert.ToDouble(dr["APPLYWORK_WEIGHT"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);


                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "APW001:" + ex.ToString();
            }

            return list_model;
        }
        public List<cls_MTApplywork> getDataByFillter(string com, string code)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE= '" + com  + "'";

            if (!code.Equals(""))
                strCondition += " AND APPLYWORK_CODE  ='" + code + "'";

            return this.getData(strCondition);
        }

        public List<cls_MTApplywork> getDataByApplywork(string com, string applywork_code)
        {
            string strCondition = "";


            strCondition += " AND COMPANY_CODE='" + com + "'";
            strCondition += " AND APPLYWORK_CODE='" + applywork_code + "'";

            return this.getData(strCondition);
        }
        public List<cls_MTApplywork> getDataByApplywork(string com)
        {
            return this.getData(" AND COMPANY_CODE='" + com + "'");
        }
        public List<cls_MTApplywork> getDataMultiplereq(string com, string applywork)
        {
            string strCondition = " AND COMPANY_CODE='" + com + "'";
            strCondition += " AND APPLYWORK_CODE IN (" + applywork + ") ";

            return this.getData(strCondition);
        }
        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(APPLYWORK_ID, 1) ");
                obj_str.Append(" FROM REQ_MT_APPLYWORK");
                obj_str.Append(" ORDER BY APPLYWORK_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "APW002:" + ex.ToString();
            }

            return intResult;
        }
        public bool checkDataOld(string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT APPLYWORK_CODE");
                obj_str.Append(" FROM REQ_MT_APPLYWORK");
                obj_str.Append(" WHERE APPLYWORK_CODE='" + code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "APW003:" + ex.ToString();
            }

            return blnResult;
        }

        public int getID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(APPLYWORK_ID, 1) ");
                obj_str.Append(" FROM REQ_MT_APPLYWORK");
                obj_str.Append(" ORDER BY APPLYWORK_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]);
                }
            }
            catch (Exception ex)
            {
                Message = "APW007:" + ex.ToString();
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
                obj_str.Append("DELETE FROM REQ_MT_APPLYWORK");

                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND APPLYWORK_ID='" + id + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "APW004:" + ex.ToString();
            }

            return blnResult;
        }
        public string insert(cls_MTApplywork model)
        {
            string strResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.applywork_code))
                {
                    if (this.update(model))
                        return model.applywork_id.ToString();
                    else
                        return "";
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO REQ_MT_APPLYWORK");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", APPLYWORK_ID ");
                obj_str.Append(", APPLYWORK_CODE ");

                obj_str.Append(", APPLYWORK_INITIAL ");

                obj_str.Append(", APPLYWORK_FNAME_TH ");
                obj_str.Append(", APPLYWORK_LNAME_TH ");

                obj_str.Append(", APPLYWORK_FNAME_EN ");
                obj_str.Append(", APPLYWORK_LNAME_EN ");

                obj_str.Append(", APPLYWORK_BIRTHDATE ");

                obj_str.Append(", APPLYWORK_STARTDATE ");
                obj_str.Append(", PROVINCE_CODE ");
                obj_str.Append(", BLOODTYPE_CODE ");

                obj_str.Append(", APPLYWORK_HEIGHT ");
                obj_str.Append(", APPLYWORK_WEIGHT ");
                //if (model.worker_resignstatus)
                //{
                //    obj_str.Append(", WORKER_RESIGNDATE ");
                //    obj_str.Append(", WORKER_RESIGNREASON ");
                //}
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @APPLYWORK_ID ");
                obj_str.Append(", @APPLYWORK_CODE ");

                obj_str.Append(", @APPLYWORK_INITIAL ");

                obj_str.Append(", @APPLYWORK_FNAME_TH ");
                obj_str.Append(", @APPLYWORK_LNAME_TH ");

                obj_str.Append(", @APPLYWORK_FNAME_EN ");
                obj_str.Append(", @APPLYWORK_LNAME_EN ");

                obj_str.Append(", @APPLYWORK_BIRTHDATE ");

                obj_str.Append(", @APPLYWORK_STARTDATE ");
                obj_str.Append(", @PROVINCE_CODE ");
                obj_str.Append(", @BLOODTYPE_CODE ");

                obj_str.Append(", @APPLYWORK_HEIGHT ");
                obj_str.Append(", @APPLYWORK_WEIGHT ");
                //if (model.worker_resignstatus)
                //{
                //    obj_str.Append(", @WORKER_RESIGNDATE ");
                //    obj_str.Append(", @WORKER_RESIGNREASON ");
                //}
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection()); 

                strResult = this.getNextID().ToString();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

                obj_cmd.Parameters.Add("@APPLYWORK_ID", SqlDbType.Int); obj_cmd.Parameters["@APPLYWORK_ID"].Value = strResult;
                obj_cmd.Parameters.Add("@APPLYWORK_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYWORK_CODE"].Value = model.applywork_code;

                obj_cmd.Parameters.Add("@APPLYWORK_INITIAL", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYWORK_INITIAL"].Value = model.applywork_initial;

                obj_cmd.Parameters.Add("@APPLYWORK_FNAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYWORK_FNAME_TH"].Value = model.applywork_fname_th;
                obj_cmd.Parameters.Add("@APPLYWORK_LNAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYWORK_LNAME_TH"].Value = model.applywork_lname_th;
                obj_cmd.Parameters.Add("@APPLYWORK_FNAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYWORK_FNAME_EN"].Value = model.applywork_fname_en;
                obj_cmd.Parameters.Add("@APPLYWORK_LNAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYWORK_LNAME_EN"].Value = model.applywork_lname_en;


                obj_cmd.Parameters.Add("@APPLYWORK_BIRTHDATE", SqlDbType.DateTime); obj_cmd.Parameters["@APPLYWORK_BIRTHDATE"].Value = model.applywork_birthdate;
                obj_cmd.Parameters.Add("@APPLYWORK_STARTDATE", SqlDbType.DateTime); obj_cmd.Parameters["@APPLYWORK_STARTDATE"].Value = model.applywork_startdate;

                obj_cmd.Parameters.Add("@PROVINCE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROVINCE_CODE"].Value = model.province_code;
                obj_cmd.Parameters.Add("@BLOODTYPE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@BLOODTYPE_CODE"].Value = model.bloodtype_code;
                obj_cmd.Parameters.Add("@APPLYWORK_HEIGHT", SqlDbType.Decimal); obj_cmd.Parameters["@APPLYWORK_HEIGHT"].Value = model.applywork_height;
                obj_cmd.Parameters.Add("@APPLYWORK_WEIGHT", SqlDbType.Decimal); obj_cmd.Parameters["@APPLYWORK_WEIGHT"].Value = model.applywork_weight;
                //if (model.worker_resignstatus)
                //{
                //    obj_cmd.Parameters.Add("@WORKER_RESIGNDATE", SqlDbType.DateTime); obj_cmd.Parameters["@WORKER_RESIGNDATE"].Value = model.worker_resigndate;
                //    obj_cmd.Parameters.Add("@WORKER_RESIGNREASON", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_RESIGNREASON"].Value = model.worker_resignreason;
                //}

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

            }
            catch (Exception ex)
            {
                strResult = "";
                Message = "APW005:" + ex.ToString();
            }

            return strResult;
        }
        public bool update(cls_MTApplywork model)
        {
            string strResult = model.applywork_id.ToString();
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE REQ_MT_APPLYWORK SET ");

                obj_str.Append(" APPLYWORK_CODE=@APPLYWORK_CODE ");
                obj_str.Append(", APPLYWORK_INITIAL=@APPLYWORK_INITIAL ");
                obj_str.Append(", APPLYWORK_FNAME_TH=@APPLYWORK_FNAME_TH ");
                obj_str.Append(", APPLYWORK_LNAME_TH=@APPLYWORK_LNAME_TH ");

                obj_str.Append(", APPLYWORK_FNAME_EN=@APPLYWORK_FNAME_EN ");
                obj_str.Append(", APPLYWORK_LNAME_EN=@APPLYWORK_LNAME_EN ");


                obj_str.Append(", APPLYWORK_BIRTHDATE=@APPLYWORK_BIRTHDATE ");
                obj_str.Append(", APPLYWORK_STARTDATE=@APPLYWORK_STARTDATE ");

                obj_str.Append(", PROVINCE_CODE=@PROVINCE_CODE ");
                obj_str.Append(", BLOODTYPE_CODE=@BLOODTYPE_CODE ");

                obj_str.Append(", APPLYWORK_HEIGHT=@APPLYWORK_HEIGHT ");
                obj_str.Append(", APPLYWORK_WEIGHT=@APPLYWORK_WEIGHT ");
   
                //if (model.worker_resignstatus)
                //{
                //    obj_str.Append(", WORKER_RESIGNDATE=@WORKER_RESIGNDATE ");
                //    obj_str.Append(", WORKER_RESIGNREASON=@WORKER_RESIGNREASON ");
                //}
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");

                obj_str.Append(" WHERE APPLYWORK_ID=@APPLYWORK_ID ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                if (model.applywork_id.ToString().Equals("0"))
                {
                    strResult = this.getID().ToString();
                }

                obj_cmd.Parameters.Add("@APPLYWORK_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYWORK_CODE"].Value = model.applywork_code;

                obj_cmd.Parameters.Add("@APPLYWORK_INITIAL", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYWORK_INITIAL"].Value = model.applywork_initial;

                obj_cmd.Parameters.Add("@APPLYWORK_FNAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYWORK_FNAME_TH"].Value = model.applywork_fname_th;
                obj_cmd.Parameters.Add("@APPLYWORK_LNAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYWORK_LNAME_TH"].Value = model.applywork_lname_th;
                obj_cmd.Parameters.Add("@APPLYWORK_FNAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYWORK_FNAME_EN"].Value = model.applywork_fname_en;
                obj_cmd.Parameters.Add("@APPLYWORK_LNAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYWORK_LNAME_EN"].Value = model.applywork_lname_en;


                obj_cmd.Parameters.Add("@APPLYWORK_BIRTHDATE", SqlDbType.DateTime); obj_cmd.Parameters["@APPLYWORK_BIRTHDATE"].Value = model.applywork_birthdate;
                obj_cmd.Parameters.Add("@APPLYWORK_STARTDATE", SqlDbType.DateTime); obj_cmd.Parameters["@APPLYWORK_STARTDATE"].Value = model.applywork_startdate;

                obj_cmd.Parameters.Add("@PROVINCE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROVINCE_CODE"].Value = model.province_code;
                obj_cmd.Parameters.Add("@BLOODTYPE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@BLOODTYPE_CODE"].Value = model.bloodtype_code;

                obj_cmd.Parameters.Add("@APPLYWORK_HEIGHT", SqlDbType.VarChar); obj_cmd.Parameters["@APPLYWORK_HEIGHT"].Value = model.applywork_code;
                obj_cmd.Parameters.Add("@APPLYWORK_WEIGHT", SqlDbType.Decimal); obj_cmd.Parameters["@APPLYWORK_WEIGHT"].Value = model.applywork_height;
               
                
                //if (model.worker_resignstatus)
                //{
                //    obj_cmd.Parameters.Add("@WORKER_RESIGNDATE", SqlDbType.DateTime); obj_cmd.Parameters["@WORKER_RESIGNDATE"].Value = model.worker_resigndate;
                //    obj_cmd.Parameters.Add("@WORKER_RESIGNREASON", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_RESIGNREASON"].Value = model.worker_resignreason;
                //}

                

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@APPLYWORK_ID", SqlDbType.Int); obj_cmd.Parameters["@APPLYWORK_ID"].Value = strResult;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "APW006:" + ex.ToString();
            }

            return blnResult;
        }
    }
}
