using ClassLibrary_BPC.hrfocus.model;
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
    public class cls_ctTRComimagesMaps
   {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRComimagesMaps() { }

        public string getMessage() { return this.Message.Replace("SYS_TR_COMIMAGESMAPS", "").Replace("cls_ctTRComimagesMaps", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRComimagesMaps> getData(string condition)
        {
            List<cls_TRComimagesMaps> list_model = new List<cls_TRComimagesMaps>();
            cls_TRComimagesMaps model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", COMIMAGESMAPS_ID");
                obj_str.Append(", COMIMAGESMAPS_IMAGESMAPS");
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM SYS_TR_COMIMAGESMAPS");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, COMIMAGESMAPS_ID");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());


                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRComimagesMaps();


                    object binaryDatas = dr["COMIMAGESMAPS_IMAGESMAPS"];


                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.comimagesmaps_id = Convert.ToInt32(dr["COMIMAGESMAPS_ID"]);
                    model.comimagesmaps_imagesmaps = (byte[])binaryDatas;


                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

                //foreach (DataRow dr in dt.Rows)
                //{
                //    model = new cls_TRComimagesMaps();

                //    byte[] binaryData = (byte[])dr["COMIMAGESMAPS_IMAGESMAPS"];
                //    model.company_code = dr["COMPANY_CODE"].ToString();
                //    model.comimagesmaps_id = Convert.ToInt32(dr["COMIMAGESMAPS_ID"]);
                //    model.comimagesmaps_imagesmaps = binaryData;

                //    model.modified_by = dr["MODIFIED_BY"].ToString();
                //    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                //    list_model.Add(model);
                //}


                

            }
            catch (Exception ex)
            {
                Message = "COMIMAGES001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRComimagesMaps> getDataByFillter(string com)
        {
            string strCondition = " AND COMPANY_CODE='" + com + "'";

            //if (!id.Equals(""))
            //    strCondition += " AND COMIMAGESMAPS_ID='" + id + "'";


            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM SYS_TR_COMIMAGESMAPS");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "'");
                obj_str.Append(" AND COMIMAGESMAPS_ID ='" + id + "'");

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

                obj_str.Append(" DELETE FROM SYS_TR_COMIMAGESMAPS");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND COMIMAGESMAPS_ID='" + id + "'");
                                          
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

                obj_str.Append(" DELETE FROM SYS_TR_COMIMAGESMAPS");
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

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(COMIMAGESMAPS_ID, 1) ");
                obj_str.Append(" FROM SYS_TR_COMIMAGESMAPS");
                obj_str.Append(" ORDER BY COMIMAGESMAPS_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "COMIM005:" + ex.ToString();
            }

            return intResult;
        }

        //public int getNextNo(string com )
        //{
        //    int intResult = 1;
        //    try
        //    {
        //        System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

        //        obj_str.Append("SELECT MAX(COMIMAGESMAPS_ID) ");
        //        obj_str.Append(" FROM SYS_TR_COMIMAGESMAPS");
        //        obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
        //        //obj_str.Append(" AND WORKER_CODE='" + worker + "'");

        //        DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

        //        if (dt.Rows.Count > 0)
        //        {
        //            intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Message = "COMIMAGES005:" + ex.ToString();
        //    }

        //    return intResult;
        //}

        public bool insert(cls_TRComimagesMaps model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.comimagesmaps_id.ToString()))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_TR_COMIMAGESMAPS");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", COMIMAGESMAPS_ID ");
                obj_str.Append(", COMIMAGESMAPS_IMAGESMAPS ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @COMIMAGESMAPS_ID ");
                obj_str.Append(", @COMIMAGESMAPS_IMAGESMAPS ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                model.comimagesmaps_id = this.getNextID();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@COMIMAGESMAPS_ID", SqlDbType.Int); obj_cmd.Parameters["@COMIMAGESMAPS_ID"].Value = model.comimagesmaps_id;
                obj_cmd.Parameters.Add("@COMIMAGESMAPS_IMAGESMAPS", SqlDbType.Image); obj_cmd.Parameters["@COMIMAGESMAPS_IMAGESMAPS"].Value = model.comimagesmaps_imagesmaps;

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

        public bool update(cls_TRComimagesMaps model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SYS_TR_COMIMAGESMAPS SET ");

                obj_str.Append(" COMIMAGESMAPS_IMAGESMAPS=@COMIMAGESMAPS_IMAGESMAPS ");


                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND COMIMAGESMAPS_ID=@COMIMAGESMAPS_ID ");
                               
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMIMAGESMAPS_IMAGESMAPS", SqlDbType.Image); obj_cmd.Parameters["@COMIMAGESMAPS_IMAGESMAPS"].Value = model.comimagesmaps_imagesmaps;
  
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@COMIMAGESMAPS_ID", SqlDbType.Int); obj_cmd.Parameters["@COMIMAGESMAPS_ID"].Value = model.comimagesmaps_id;

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
