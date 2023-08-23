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
    public class cls_ctMTLevel
    {
      string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTLevel() { }

        public string getMessage() { return this.Message.Replace("SYS_MT_LEVEL", "").Replace("cls_ctMTLevel", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTLevel> getData(string condition)
        {
            List<cls_MTLevel> list_model = new List<cls_MTLevel>();
            cls_MTLevel model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("LEVEL_ID");
                obj_str.Append(", LEVEL_CODE");
                obj_str.Append(", LEVEL_NAME_TH");
                obj_str.Append(", LEVEL_NAME_EN");
                obj_str.Append(", COMPANY_CODE"); 
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM SYS_MT_LEVEL");
                obj_str.Append(" WHERE 1=1");
                
                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY LEVEL_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTLevel();

                    model.level_id = Convert.ToInt32(dr["LEVEL_ID"]);
                    model.level_code = dr["LEVEL_CODE"].ToString();
                    model.level_name_th = dr["LEVEL_NAME_TH"].ToString();
                    model.level_name_en = dr["LEVEL_NAME_EN"].ToString();
 

                    model.company_code = dr["COMPANY_CODE"].ToString(); 
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                                                                                            
                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "LVL001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTLevel> getDataByFillter(string code, string com)
        {
            string strCondition = "";


            if (!code.Equals(""))
                strCondition += " AND LEVEL_CODE='" + code + "'";

            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            
            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(LEVEL_ID, 1) ");
                obj_str.Append(" FROM SYS_MT_LEVEL");
                obj_str.Append(" ORDER BY LEVEL_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "LVL002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT LEVEL_CODE");
                obj_str.Append(" FROM SYS_MT_LEVEL");
                obj_str.Append(" WHERE LEVEL_CODE='" + com + "'");
                obj_str.Append(" AND LEVEL_ID='" + id + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "LVL003:" + ex.ToString();
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

                obj_str.Append("DELETE FROM SYS_MT_LEVEL");
                obj_str.Append(" WHERE LEVEL_CODE='" + code + "'");


                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "LVL004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTLevel model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.level_code, model.level_id.ToString()))
                {
                    if (this.update(model))
                        return model.level_id.ToString();
                    else
                        return "";                    
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_MT_LEVEL");
                obj_str.Append(" (");
                obj_str.Append("LEVEL_ID ");
                obj_str.Append(", LEVEL_CODE ");
                obj_str.Append(", LEVEL_NAME_TH ");
                obj_str.Append(", LEVEL_NAME_EN ");
                
                obj_str.Append(", COMPANY_CODE "); 
                
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");          
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@LEVEL_ID ");
                obj_str.Append(", @LEVEL_CODE ");
                obj_str.Append(", @LEVEL_NAME_TH ");
                obj_str.Append(", @LEVEL_NAME_EN ");


                obj_str.Append(", @COMPANY_CODE ");      

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");
                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.level_id = this.getNextID();

                obj_cmd.Parameters.Add("@LEVEL_ID", SqlDbType.Int); obj_cmd.Parameters["@LEVEL_ID"].Value = model.level_id;
                obj_cmd.Parameters.Add("@LEVEL_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@LEVEL_CODE"].Value = model.level_code;
                obj_cmd.Parameters.Add("@LEVEL_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@LEVEL_NAME_TH"].Value = model.level_name_th;
                obj_cmd.Parameters.Add("@LEVEL_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@LEVEL_NAME_EN"].Value = model.level_name_en;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                                     
                obj_cmd.ExecuteNonQuery();
                                
                obj_conn.doClose();
                strResult = model.level_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "LVL005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }


        public bool update(cls_MTLevel model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE SYS_MT_LEVEL SET ");

                obj_str.Append(" LEVEL_NAME_TH=@LEVEL_NAME_TH ");
                obj_str.Append(", LEVEL_NAME_EN=@LEVEL_NAME_EN ");


                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE LEVEL_CODE=@LEVEL_CODE ");
                obj_str.Append(" AND COMPANY_CODE=@COMPANY_CODE ");
               

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());


                obj_cmd.Parameters.Add("@LEVEL_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@LEVEL_NAME_TH"].Value = model.level_name_th;
                obj_cmd.Parameters.Add("@LEVEL_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@LEVEL_NAME_EN"].Value = model.level_name_en;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@LEVEL_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@LEVEL_CODE"].Value = model.level_code;
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "LVL006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}
 

//{
//        string Message = string.Empty;

//        cls_ctConnection Obj_conn = new cls_ctConnection();

//        public cls_ctMTLevel() { }

//        public string getMessage() { return this.Message; }

//        public void dispose()
//        {
//            Obj_conn.doClose();
//        }

//        private List<cls_MTLevel> getData(string condition)
//        {
//            List<cls_MTLevel> list_model = new List<cls_MTLevel>();
//            cls_MTLevel model;
//            try
//            {
//                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

//                obj_str.Append("SELECT ");

//                obj_str.Append("LEVEL_ID");
//                obj_str.Append(", LEVEL_CODE");
//                obj_str.Append(", LEVEL_NAME_TH");
//                obj_str.Append(", LEVEL_NAME_EN");
//                obj_str.Append(", LEVEL_GROUP");
//                obj_str.Append(", COMPANY_CODE"); 
//                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
//                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

//                obj_str.Append(" FROM SYS_MT_LEVEL");
//                obj_str.Append(" WHERE 1=1");
//                if (!condition.Equals(""))
//                    obj_str.Append(" " + condition);

//                 obj_str.Append(" ORDER BY LEVEL_CODE");

//                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

//                foreach (DataRow dr in dt.Rows)
//                {
//                   model = new cls_MTLevel();

//                    model.level_id = Convert.ToInt32(dr["LEVEL_ID"]);
//                    model.level_code = dr["LEVEL_CODE"].ToString();
//                    model.level_name_th = dr["LEVEL_NAME_TH"].ToString();
//                    model.level_name_en = dr["LEVEL_NAME_EN"].ToString();
//                    model.level_group = dr["LEVEL_GROUP"].ToString();
 

//                    model.company_code = dr["COMPANY_CODE"].ToString(); 
//                    model.modified_by = dr["MODIFIED_BY"].ToString();
//                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

//                    list_model.Add(model);
//                }

//            }
//            catch (Exception ex)
//            {
//                Message = "ERROR::(level.getData)" + ex.ToString();
//            }

//            return list_model;
//        }

//        public List<cls_MTLevel> getDataByFillter(string group, string id, string code, string com)
//        {
//            string strCondition = "";

//            if (!group.Equals(""))
//                strCondition += " AND LEVEL_GROUP='" + group + "'";

//            if (!id.Equals(""))
//                strCondition += " AND LEVEL_ID='" + id + "'";

//            if (!code.Equals(""))
//                strCondition += " AND LEVEL_CODE='" + code + "'";
//            if (!com.Equals(""))
//                strCondition += " AND COMPANY_CODE='" + com + "'";

//            return this.getData(strCondition);
//        }

//        public bool checkDataOld(string group, string code, string com, string id)
//        {
//            bool blnResult = false;
//            try
//            {
//                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

//                obj_str.Append("SELECT LEVEL_ID");
//                obj_str.Append(" FROM SYS_MT_LEVEL");
//                obj_str.Append(" WHERE 1=1 ");
//                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
//                obj_str.Append(" AND LEVEL_GROUP='" + group + "'");
//                obj_str.Append(" AND LEVEL_CODE='" + code + "'");
//                obj_str.Append(" AND LEVEL_ID='" + id + "'");

//                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

//                if (dt.Rows.Count > 0)
//                {
//                    blnResult = true;
//                }
//            }
//            catch (Exception ex)
//            {
//                Message = "ERROR::(level.checkDataOld)" + ex.ToString();
//            }

//            return blnResult;
//        }

//        public int getNextID()
//        {
//            int intResult = 1;
//            try
//            {
//                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

//                obj_str.Append("SELECT MAX(LEVEL_ID) ");
//                obj_str.Append(" FROM SYS_MT_LEVEL");

//                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

//                if (dt.Rows.Count > 0)
//                {
//                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
//                }
//            }
//            catch (Exception ex)
//            {
//                Message = "ERROR::(level.getNextID)" + ex.ToString();
//            }

//            return intResult;
//        }

//        public bool delete(string id, string com)
//        {
//            bool blnResult = true;
//            try
//            {
//                cls_ctConnection obj_conn = new cls_ctConnection();

//                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

//                obj_str.Append(" DELETE FROM SYS_MT_LEVEL");
//                obj_str.Append(" WHERE 1=1 ");
//                obj_str.Append(" AND LEVEL_ID='" + id + "'");
//                obj_str.Append(" AND COMPANY_CODE='" + com + "'");

//                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

//            }
//            catch (Exception ex)
//            {
//                blnResult = false;
//                Message = "ERROR::(level.delete)" + ex.ToString();
//            }

//            return blnResult;
//        }

//        public string insert(cls_MTLevel model)
//        {
//            string blnResult = "";
//            try
//            {
//                //-- Check data old
//                if (this.checkDataOld(model.level_group, model.level_code, model.company_code, model.level_id.ToString()))
//                {
//                    return this.update(model);
//                }

//                cls_ctConnection obj_conn = new cls_ctConnection();
//                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
//                int id = this.getNextID();
//                obj_str.Append("INSERT INTO SYS_MT_LEVEL");
//                obj_str.Append(" (");
//                obj_str.Append("LEVEL_ID ");
//                obj_str.Append(", COMPANY_CODE ");
//                obj_str.Append(", LEVEL_CODE ");
//                obj_str.Append(", LEVEL_NAME_TH ");
//                obj_str.Append(", LEVEL_NAME_EN ");
//                obj_str.Append(", LEVEL_GROUP ");
//                obj_str.Append(", CREATED_BY ");
//                obj_str.Append(", CREATED_DATE ");
//                obj_str.Append(", FLAG ");
//                obj_str.Append(" )");

//                obj_str.Append(" VALUES(");
//                obj_str.Append(" @LEVEL_ID ");
//                obj_str.Append(", @COMPANY_CODE ");
//                obj_str.Append(", @LEVEL_CODE ");
//                obj_str.Append(", @LEVEL_NAME_TH ");
//                obj_str.Append(", @LEVEL_NAME_EN ");
//                obj_str.Append(", @LEVEL_GROUP ");
//                obj_str.Append(", @CREATED_BY ");
//                obj_str.Append(", @CREATED_DATE ");
//                obj_str.Append(", @FLAG ");
//                obj_str.Append(" )");

//                obj_conn.doConnect();

//                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

//                obj_cmd.Parameters.Add("@LEVEL_ID", SqlDbType.Int); obj_cmd.Parameters["@LEVEL_ID"].Value = id;
//                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
//                obj_cmd.Parameters.Add("@LEVEL_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@LEVEL_CODE"].Value = model.level_code;
//                obj_cmd.Parameters.Add("@LEVEL_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@LEVEL_NAME_TH"].Value = model.level_name_th;
//                obj_cmd.Parameters.Add("@LEVEL_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@LEVEL_NAME_EN"].Value = model.level_name_en;
//                obj_cmd.Parameters.Add("@LEVEL_GROUP", SqlDbType.VarChar); obj_cmd.Parameters["@LEVEL_GROUP"].Value = model.level_group;
//                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
//                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
//                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

//                obj_cmd.ExecuteNonQuery();

//                obj_conn.doClose();
//                blnResult = id.ToString();
//            }
//            catch (Exception ex)
//            {
//                Message = "ERROR::(Round.insert)" + ex.ToString();
//                blnResult = "";
//            }

//            return blnResult;
//        }

//        public string update(cls_MTLevel model)
//        {
//            string blnResult = "";
//            try
//            {
//                cls_ctConnection obj_conn = new cls_ctConnection();

//                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

//                obj_str.Append("UPDATE SYS_MT_LEVEL SET ");

//                obj_str.Append(" COMPANY_CODE=@COMPANY_CODE ");
  
//                obj_str.Append(", LEVEL_NAME_TH=@LEVEL_NAME_TH ");
//                obj_str.Append(", LEVEL_NAME_EN=@LEVEL_NAME_EN ");
//                obj_str.Append(", LEVEL_GROUP=@LEVEL_GROUP ");
//                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
//                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
//                obj_str.Append(", FLAG=@FLAG ");

//                obj_str.Append(" WHERE LEVEL_ID=@LEVEL_ID ");
//                obj_str.Append(" AND LEVEL_CODE=@LEVEL_CODE ");

//                obj_conn.doConnect();

//                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
//                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
//                obj_cmd.Parameters.Add("@LEVEL_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@LEVEL_NAME_TH"].Value = model.level_name_th;
//                obj_cmd.Parameters.Add("@LEVEL_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@LEVEL_NAME_EN"].Value = model.level_name_en;
//                obj_cmd.Parameters.Add("@LEVEL_GROUP", SqlDbType.VarChar); obj_cmd.Parameters["@LEVEL_GROUP"].Value = model.level_group;
//                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
//                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
//                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

//                obj_cmd.Parameters.Add("@LEVEL_ID", SqlDbType.Int); obj_cmd.Parameters["@LEVEL_ID"].Value = model.level_id;
//                obj_cmd.Parameters.Add("@LEVEL_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@LEVEL_CODE"].Value = model.level_code;

//                obj_cmd.ExecuteNonQuery();

//                obj_conn.doClose();

//                blnResult = model.level_id.ToString();
//            }
//            catch (Exception ex)
//            {
//                Message = "ERROR::(level.update)" + ex.ToString();
//                blnResult = "ERROR::(level.update)" + ex.ToString();
//            }

//            return blnResult;
//        }
//    }
//}