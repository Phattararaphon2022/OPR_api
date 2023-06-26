using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTFaculty
         {
     string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTFaculty() { }

        public string getMessage() { return this.Message.Replace("SYS_MT_FACULTY", "").Replace("cls_ctMTFaculty", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTFaculty> getData(string condition)
        {
            List<cls_MTFaculty> list_model = new List<cls_MTFaculty>();
            cls_MTFaculty model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("FACULTY_ID");
                obj_str.Append(", FACULTY_CODE");
                obj_str.Append(", ISNULL(FACULTY_NAME_TH, '') AS FACULTY_NAME_TH");
                obj_str.Append(", ISNULL(FACULTY_NAME_EN, '') AS FACULTY_NAME_EN");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM SYS_MT_FACULTY");
                obj_str.Append(" WHERE 1=1");
                
                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY FACULTY_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTFaculty();

                    model.faculty_id = Convert.ToInt32(dr["FACULTY_ID"]);
                    model.faculty_code = dr["FACULTY_CODE"].ToString();
                    model.faculty_name_th = dr["FACULTY_NAME_TH"].ToString();
                    model.faculty_name_en = dr["FACULTY_NAME_EN"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "FAC001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTFaculty> getDataByFillter(string code)
        {
            string strCondition = "";

            if (!code.Equals(""))
                strCondition += " AND FACULTY_CODE='" + code + "'";
            
            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(FACULTY_ID, 1) ");
                obj_str.Append(" FROM SYS_MT_FACULTY");
                obj_str.Append(" ORDER BY FACULTY_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "FAC002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT FACULTY_CODE");
                obj_str.Append(" FROM SYS_MT_FACULTY");
                obj_str.Append(" WHERE FACULTY_CODE='" + code + "'");
      
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "FAC003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string code)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM SYS_MT_FACULTY");
                obj_str.Append(" WHERE FACULTY_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "FAC004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTFaculty model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.faculty_code))
                {
                    if (this.update(model))
                        return model.faculty_id.ToString();
                    else
                        return "";                    
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_MT_FACULTY");
                obj_str.Append(" (");
                obj_str.Append("FACULTY_ID ");
                obj_str.Append(", FACULTY_CODE ");
                obj_str.Append(", FACULTY_NAME_TH ");
                obj_str.Append(", FACULTY_NAME_EN ");               
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");          
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@FACULTY_ID ");
                obj_str.Append(", @FACULTY_CODE ");
                obj_str.Append(", @FACULTY_NAME_TH ");
                obj_str.Append(", @FACULTY_NAME_EN ");      
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");
                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.faculty_id = this.getNextID();

                obj_cmd.Parameters.Add("@FACULTY_ID", SqlDbType.Int); obj_cmd.Parameters["@FACULTY_ID"].Value = model.faculty_id;
                obj_cmd.Parameters.Add("@FACULTY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@FACULTY_CODE"].Value = model.faculty_code;
                obj_cmd.Parameters.Add("@FACULTY_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@FACULTY_NAME_TH"].Value = model.faculty_name_th;
                obj_cmd.Parameters.Add("@FACULTY_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@FACULTY_NAME_EN"].Value = model.faculty_name_en;        
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                                     
                obj_cmd.ExecuteNonQuery();
                                
                obj_conn.doClose();
                strResult = model.faculty_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "FAC005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_MTFaculty model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE SYS_MT_FACULTY SET ");
                obj_str.Append("  FACULTY_NAME_TH=@FACULTY_NAME_TH ");
                obj_str.Append(", FACULTY_NAME_EN=@FACULTY_NAME_EN ");               
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE FACULTY_ID=@FACULTY_ID ");            



                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@FACULTY_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@FACULTY_NAME_TH"].Value = model.faculty_name_th;
                obj_cmd.Parameters.Add("@FACULTY_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@FACULTY_NAME_EN"].Value = model.faculty_name_en;        
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@FACULTY_ID", SqlDbType.Int); obj_cmd.Parameters["@FACULTY_ID"].Value = model.faculty_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "FAC006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}

//    {
   
//     string Message = string.Empty;

//        cls_ctConnection Obj_conn = new cls_ctConnection();

//        public cls_ctMTFaculty() { }

//        public string getMessage() { return this.Message.Replace("SYS_MT_FACULTY", "").Replace("cls_ctMTFaculty", "").Replace("line", ""); }

//        public void dispose()
//        {
//            Obj_conn.doClose();
//        }

//        private List<cls_MTFaculty> getData(string condition)
//        {
//            List<cls_MTFaculty> list_model = new List<cls_MTFaculty>();
//            cls_MTFaculty model;
//            try
//            {
//                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

//                obj_str.Append("SELECT ");

//                obj_str.Append("COMPANY_CODE");
//                obj_str.Append(", FACULTY_ID");
//                obj_str.Append(", FACULTY_CODE");
//                obj_str.Append(", ISNULL(FACULTY_NAME_TH, '') AS FACULTY_NAME_TH");
//                obj_str.Append(", ISNULL(FACULTY_NAME_EN, '') AS FACULTY_NAME_EN");

//                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
//                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

//                obj_str.Append(" FROM SYS_MT_FACULTY");
//                obj_str.Append(" WHERE 1=1");

//                if (!condition.Equals(""))
//                    obj_str.Append(" " + condition);

//                obj_str.Append(" ORDER BY COMPANY_CODE, FACULTY_CODE");

//                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

//                foreach (DataRow dr in dt.Rows)
//                {
//                    model = new cls_MTFaculty();

//                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
//                    model.faculty_id = Convert.ToInt32(dr["FACULTY_ID"]);
//                    model.faculty_code = Convert.ToString(dr["FACULTY_CODE"]);
//                    model.faculty_name_th = Convert.ToString(dr["FACULTY_NAME_TH"]);
//                    model.faculty_name_en = Convert.ToString(dr["FACULTY_NAME_EN"]);

//                    model.modified_by = dr["MODIFIED_BY"].ToString();
//                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

//                    list_model.Add(model);
//                }

//            }
//            catch(Exception ex)
//            {
//                Message = "FACULTY001:" + ex.ToString();
//            }

//            return list_model;
//        }

//        public List<cls_MTFaculty> getDataByFillter(string com, string id, string code)
//        {
//            string strCondition = "";

//            strCondition += " AND COMPANY_CODE='" + com + "'";

//            if (!id.Equals(""))
//                strCondition += " AND FACULTY_ID='" + id + "'";

//            if (!code.Equals(""))
//                strCondition += " AND FACULTY_CODE='" + code + "'";

//            return this.getData(strCondition);
//        }
//        public int getNextID()
//        {
//            int intResult = 1;
//            try
//            {
//                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();


//                obj_str.Append("SELECT MAX(FACULTY_ID) ");
//                obj_str.Append(" FROM SYS_MT_FACULTY"); 

//                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

//                if (dt.Rows.Count > 0)
//                {
//                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
//                }
//            }
//            catch (Exception ex)
//            {
//                Message = "FACULTY002:" + ex.ToString();
//            }

//            return intResult;
//        }

//        public bool checkDataOld(string com, string code)
//        {
//            bool blnResult = false;
//            try
//            {
//                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();




//                obj_str.Append("SELECT FACULTY_ID");
//                obj_str.Append(" FROM SYS_MT_FACULTY");
//                obj_str.Append(" WHERE 1=1 ");
//                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
//                obj_str.Append(" AND FACULTY_CODE='" + code + "'");
      
//                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

//                if (dt.Rows.Count > 0)
//                {
//                    blnResult = true;
//                }
//            }
//            catch (Exception ex)
//            {
//                Message = "FACULTY003:" + ex.ToString();
//            }

//            return blnResult;
//        }

//        public bool delete(string id)
//        {
//            bool blnResult = true;
//            try
//            {
//                cls_ctConnection obj_conn = new cls_ctConnection();

//                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

//                obj_str.Append(" DELETE FROM SYS_MT_FACULTY");
//                obj_str.Append(" WHERE 1=1 ");
//                obj_str.Append(" AND FACULTY_ID='" + id + "'");


//                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

//            }
//            catch (Exception ex)
//            {
//                blnResult = false;
//                Message = "FACULTY004:" + ex.ToString();
//            }

//            return blnResult;
//        }

//        public bool insert(cls_MTFaculty model)
//        {
//            bool blnResult = false;
//            string strResult = "";
//            try
//            {

//                //-- Check data old


               
//                //-- Check data old
//                if (this.checkDataOld(model.company_code, model.faculty_code))
//                {
//                    return this.update(model);
//                }

//                cls_ctConnection obj_conn = new cls_ctConnection();
//                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();


//                obj_str.Append("INSERT INTO SYS_MT_FACULTY");
//                obj_str.Append(" (");
//                obj_str.Append("FACULTY_ID ");
//                obj_str.Append(", FACULTY_CODE ");
//                obj_str.Append(", FACULTY_NAME_TH ");
//                obj_str.Append(", FACULTY_NAME_EN ");
                                
//                obj_str.Append(", COMPANY_CODE ");

//                obj_str.Append(", CREATED_BY ");
//                obj_str.Append(", CREATED_DATE ");
//                obj_str.Append(", FLAG ");
//                obj_str.Append(" )");

//                obj_str.Append(" VALUES(");
//                obj_str.Append("@FACULTY_ID ");
//                obj_str.Append(", @FACULTY_CODE ");
//                obj_str.Append(", @FACULTY_NAME_TH ");
//                obj_str.Append(", @FACULTY_NAME_EN ");                
//                obj_str.Append(", @COMPANY_CODE ");
//                obj_str.Append(", @CREATED_BY ");
//                obj_str.Append(", @CREATED_DATE ");
//                obj_str.Append(", @FLAG ");              
//                obj_str.Append(" )");
                
//                obj_conn.doConnect();

//                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

//                model.faculty_id = this.getNextID();

//                obj_cmd.Parameters.Add("@FACULTY_ID", SqlDbType.Int); obj_cmd.Parameters["@FACULTY_ID"].Value = this.getNextID();
//                obj_cmd.Parameters.Add("@FACULTY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@FACULTY_CODE"].Value = model.faculty_code;
//                obj_cmd.Parameters.Add("@FACULTY_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@FACULTY_NAME_TH"].Value = model.faculty_name_th;
//                obj_cmd.Parameters.Add("@FACULTY_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@FACULTY_NAME_EN"].Value = model.faculty_name_en;

//                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

//                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
//                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
//                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

//                obj_cmd.ExecuteNonQuery();

                                
//                obj_conn.doClose();
//                strResult = model.faculty_id.ToString();
//                blnResult = true;

//            }
//            catch (Exception ex)
//            {
//                Message = "FACULTY005:" + ex.ToString();
//                strResult = "";
//                blnResult = false;
//            }

//            return blnResult;
//        }

//        public bool update(cls_MTFaculty model)
//        {
//            bool blnResult = false;
//            try
//            {
//                cls_ctConnection obj_conn = new cls_ctConnection();
//                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

//                obj_str.Append("UPDATE SYS_MT_FACULTY SET ");

//                obj_str.Append(" FACULTY_CODE=@FACULTY_CODE ");
//                obj_str.Append(", FACULTY_NAME_TH=@FACULTY_NAME_TH ");
//                obj_str.Append(", FACULTY_NAME_EN=@FACULTY_NAME_EN ");

//                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
//                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
//                obj_str.Append(", FLAG=@FLAG ");


//                obj_str.Append(" WHERE FACULTY_ID=@FACULTY_ID ");

//                obj_conn.doConnect();

//                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

//                obj_cmd.Parameters.Add("@FACULTY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@FACULTY_CODE"].Value = model.faculty_code;
//                obj_cmd.Parameters.Add("@FACULTY_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@FACULTY_NAME_TH"].Value = model.faculty_name_th;
//                obj_cmd.Parameters.Add("@FACULTY_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@FACULTY_NAME_EN"].Value = model.faculty_name_en;

//                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
//                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
//                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

//                obj_cmd.Parameters.Add("@FACULTY_ID", SqlDbType.Int); obj_cmd.Parameters["@FACULTY_ID"].Value = model.faculty_id;

//                obj_cmd.ExecuteNonQuery();


//                obj_conn.doClose();

//                blnResult = true;
//            }
//            catch (Exception ex)
//            {
//                Message = "FACULTY006:" + ex.ToString();
//            }

//            return blnResult;
//        }

//    }
//}
