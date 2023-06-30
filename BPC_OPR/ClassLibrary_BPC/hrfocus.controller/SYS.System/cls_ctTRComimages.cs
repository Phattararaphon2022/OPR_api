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
   public class cls_ctTRComimages
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRComimages() { }

        public string getMessage() { return this.Message.Replace("SYS_TR_COMIMAGESLOGO", "").Replace("cls_ctTRComimages", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRComimages> getData(string condition)
        {
            List<cls_TRComimages> list_model = new List<cls_TRComimages>();
            cls_TRComimages model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", COMIMAGES_ID");
                obj_str.Append(", COMIMAGES_IMAGESLOGO");
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM SYS_TR_COMIMAGESLOGO");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, COMIMAGES_ID");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());


                //foreach (DataRow dr in dt.Rows)
                //{
                //    model = new cls_TRComimages();

                //    byte[] binaryData = (byte[])dr["COMIMAGES_IMAGESLOGO"];
                //    model.company_code = dr["COMPANY_CODE"].ToString();
                //    model.comimages_id = Convert.ToInt32(dr["COMIMAGES_ID"]);
                //    model.comimages_imageslogo = binaryData;

                //    model.modified_by = dr["MODIFIED_BY"].ToString();
                //    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                //    list_model.Add(model);
                //}


                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRComimages();

                    object binaryData = dr["COMIMAGES_IMAGESLOGO"];



                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.comimages_id = Convert.ToInt32(dr["COMIMAGES_ID"]);
                    model.comimages_imageslogo = (byte[])binaryData;


                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "COMIMAGES001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRComimages> getDataByFillter(string com)
        {
            string strCondition = " AND COMPANY_CODE='" + com + "'";

            //if (!id.Equals(""))
            //    strCondition += " AND COMIMAGES_ID='" + id + "'";


            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM SYS_TR_COMIMAGESLOGO");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "'");
                obj_str.Append(" AND COMIMAGES_ID ='" + id + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "COMIMAGES002:" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(string com, string id)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SYS_TR_COMIMAGESLOGO");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND COMIMAGES_ID='" + id + "'");
                                          
                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "COMIMAGES003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool clear(string com)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SYS_TR_COMIMAGESLOGO");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                //obj_str.Append(" AND WORKER_CODE='" + worker + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "COMIMAGES004:" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextNo(string com )
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(COMIMAGES_ID) ");
                obj_str.Append(" FROM SYS_TR_COMIMAGESLOGO");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                //obj_str.Append(" AND WORKER_CODE='" + worker + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "COMIMAGES005:" + ex.ToString();
            }

            return intResult;
        }

        public bool insert(cls_TRComimages model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.comimages_id.ToString()))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_TR_COMIMAGESLOGO");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", COMIMAGES_ID ");
                obj_str.Append(", COMIMAGES_IMAGESLOGO ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @COMIMAGES_ID ");
                obj_str.Append(", @COMIMAGES_IMAGESLOGO ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@COMIMAGES_ID", SqlDbType.VarChar); obj_cmd.Parameters["@COMIMAGES_ID"].Value = model.comimages_id;
                obj_cmd.Parameters.Add("@COMIMAGES_IMAGESLOGO", SqlDbType.Image); obj_cmd.Parameters["@COMIMAGES_IMAGESLOGO"].Value = model.comimages_imageslogo;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;
     
                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "COMIMAGES006:" + ex.ToString();
            }

            return blnResult;
        }

        public bool update(cls_TRComimages model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SYS_TR_COMIMAGESLOGO SET ");

                obj_str.Append(" COMIMAGES_IMAGESLOGO=@COMIMAGES_IMAGESLOGO ");


                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND COMIMAGES_ID=@COMIMAGES_ID ");
                               
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMIMAGES_IMAGESLOGO", SqlDbType.Image); obj_cmd.Parameters["@COMIMAGES_IMAGESLOGO"].Value = model.comimages_imageslogo;
  
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@COMIMAGES_ID", SqlDbType.Int); obj_cmd.Parameters["@COMIMAGES_ID"].Value = model.comimages_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "COMIMAGES007:" + ex.ToString();
            }

            return blnResult;
        }
    
    }
}
