using ClassLibrary_BPC.hrfocus.model.Project;
using ClassLibrary_BPC.hrfocus.model.SYS.System;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller.Project
{
   public class cls_ctTRProwithdraw
 
    {
   string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRProwithdraw() { }

        public string getMessage() { return this.Message.Replace("PRO_TR_PROWITHDRAW", "").Replace("cls_ctMTProwithdraw", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRProwithdraw> getData(string condition)
        {
            List<cls_TRProwithdraw> list_model = new List<cls_TRProwithdraw>();
            cls_TRProwithdraw model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");
                obj_str.Append("COMPANY_CODE");

                obj_str.Append(", PROWITHDRAW_ID");
                obj_str.Append(", ISNULL(PROWITHDRAW_WORKDATE, 0) AS PROWITHDRAW_WORKDATE");

                obj_str.Append(", WORKER_CODE");
                obj_str.Append(", PROJECT_CODE");
                obj_str.Append(", PROJOB_TYPE");
                obj_str.Append(", PROJOB_CODE");
  
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PRO_TR_PROWITHDRAW");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRProwithdraw();
                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);

                    model.prowithdraw_id = Convert.ToInt32(dr["PROWITHDRAW_ID"]);
                    model.prowithdraw_workdate = Convert.ToDateTime(dr["PROWITHDRAW_WORKDATE"]);

                    model.worker_code = dr["WORKER_CODE"].ToString();
                    model.project_code = dr["PROJECT_CODE"].ToString();
                    model.projob_type = dr["PROJOB_TYPE"].ToString();
                    model.projob_code = dr["PROJOB_CODE"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                                                                                                                      
                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "ERROR::(PROWITHDRAW.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRProwithdraw> getDataByFillter(string id, string com, string emp, string code, string type, string job)
        {
            string strCondition = "";

            strCondition += " AND PROWITHDRAW_ID='" + id + "'";

            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!code.Equals(""))
                strCondition += " AND WORKER_CODE='" + emp + "'";

            if (!code.Equals(""))
                strCondition += " AND PROJECT_CODE='" + code + "'";


            if (!code.Equals(""))
                strCondition += " AND PROJOB_TYPE='" + type + "'";


            if (!code.Equals(""))
                strCondition += " AND PROJOB_CODE='" + job + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string id, string com, string emp, string code, string type, string job)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PROWITHDRAW_ID");
                obj_str.Append(" FROM PRO_TR_PROWITHDRAW");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND PROWITHDRAW_ID='" + id + "'");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PROJECT_CODE='" + code + "'");
                obj_str.Append(" AND WORKER_CODE='" + emp + "'");
                obj_str.Append(" AND PROJOB_TYPE='" + type + "'");
                obj_str.Append(" AND PROJOB_CODE='" + job + "'");

                                                
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(PROWITHDRAW.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(PROWITHDRAW_ID) ");
                obj_str.Append(" FROM PRO_TR_PROWITHDRAW");                

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(PROWITHDRAW.getNextID)" + ex.ToString();
            }

            return intResult;
        }

        public bool delete(string id,string com)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM PRO_TR_PROWITHDRAW");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND PROWITHDRAW_ID='" + id + "'");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");

                                              
                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(PROWITHDRAW.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRProwithdraw model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old


                if (this.checkDataOld(model.prowithdraw_id.ToString(), model.company_code, model.project_code, model.worker_code, model.projob_type, model.projob_code))
                {
                    if (model.prowithdraw_id.Equals(0))
                    {
                        return false;
                    }
                    else
                    {
                        return this.update(model);
                    }
                }

 

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PRO_TR_PROWITHDRAW");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", PROWITHDRAW_ID ");
                obj_str.Append(", PROWITHDRAW_WORKDATE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", PROJECT_CODE ");
                obj_str.Append(", PROJOB_TYPE ");
                obj_str.Append(", PROJOB_CODE ");
                 obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @PROWITHDRAW_ID ");
                obj_str.Append(", @PROWITHDRAW_WORKDATE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @PROJECT_CODE ");
                obj_str.Append(", @PROJOB_TYPE ");
                obj_str.Append(", @PROJOB_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");              
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

               
              obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
 
                obj_cmd.Parameters.Add("@PROWITHDRAW_ID", SqlDbType.Int); obj_cmd.Parameters["@PROWITHDRAW_ID"].Value  = this.getNextID();
                obj_cmd.Parameters.Add("@PROWITHDRAW_WORKDATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROWITHDRAW_WORKDATE"].Value = model.prowithdraw_workdate;

                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
                obj_cmd.Parameters.Add("@PROJOB_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOB_TYPE"].Value = model.projob_type;
                obj_cmd.Parameters.Add("@PROJOB_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOB_CODE"].Value = model.projob_code;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;
     
                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(PAYP006.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public bool update(cls_TRProwithdraw model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                 obj_str.Append("UPDATE PRO_TR_PROWITHDRAW SET ");

                 obj_str.Append(" WORKER_CODE=@WORKER_CODE ");
                 obj_str.Append(", PROWITHDRAW_WORKDATE=@PROWITHDRAW_WORKDATE ");

                 obj_str.Append(", PROJECT_CODE=@PROJECT_CODE ");
                 obj_str.Append(", PROJOB_TYPE=@PROJOB_TYPE ");
                 obj_str.Append(", PROJOB_CODE=@PROJOB_CODE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");


                obj_str.Append(" WHERE PROWITHDRAW_ID=@PROWITHDRAW_ID ");
 

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                 obj_cmd.Parameters.Add("@PROWITHDRAW_WORKDATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROWITHDRAW_WORKDATE"].Value = model.prowithdraw_workdate;
                 obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
                 obj_cmd.Parameters.Add("@PROJOB_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOB_TYPE"].Value = model.projob_type;
                 obj_cmd.Parameters.Add("@PROJOB_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOB_CODE"].Value = model.projob_code;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;
                obj_cmd.Parameters.Add("@PROWITHDRAW_ID", SqlDbType.Int); obj_cmd.Parameters["@PROWITHDRAW_ID"].Value = model.prowithdraw_id;

 

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(PAYP006.update)" + ex.ToString();
            }

            return blnResult;
        }
    }



 

        //public string insert(cls_TRProwithdraw model)
        //{
        //    string strResult = "";
        //    try
        //    {
        //        //-- Check data old
        //        if (this.checkDataOld(model.prowithdraw_id.ToString(), model.company_code, model.project_code, model.worker_code, model.projob_type, model.projob_code))
        //        {
        //            bool blnResult =  this.update(model);

        //            if (blnResult)
        //                return model.prowithdraw_id.ToString();
        //        }

        //        cls_ctConnection obj_conn = new cls_ctConnection();
        //        System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
 
        //        obj_str.Append("INSERT INTO PRO_TR_PROWITHDRAW");
        //        obj_str.Append(" (");
        //        obj_str.Append("COMPANY_CODE ");
        //        obj_str.Append(", PROWITHDRAW_ID ");
        //        obj_str.Append(", PROWITHDRAW_WORKDATE ");
        //        obj_str.Append(", WORKER_CODE ");
        //        obj_str.Append(", PROJECT_CODE ");
        //        obj_str.Append(", PROJOB_TYPE ");
        //        obj_str.Append(", PROJOB_CODE ");
        //         obj_str.Append(", CREATED_BY ");
        //        obj_str.Append(", CREATED_DATE ");
        //        obj_str.Append(", FLAG ");
        //        obj_str.Append(" )");

        //        obj_str.Append(" VALUES(");
        //        obj_str.Append("@COMPANY_CODE ");
        //        obj_str.Append(", @PROWITHDRAW_ID ");
        //        obj_str.Append(", @PROWITHDRAW_WORKDATE ");
        //        obj_str.Append(", @WORKER_CODE ");
        //        obj_str.Append(", @PROJECT_CODE ");
        //        obj_str.Append(", @PROJOB_TYPE ");
        //        obj_str.Append(", @PROJOB_CODE ");
        //        obj_str.Append(", @CREATED_BY ");
        //        obj_str.Append(", @CREATED_DATE ");
        //        obj_str.Append(", @FLAG ");              
        //        obj_str.Append(" )");

        //        obj_conn.doConnect();

        //        SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

        //        strResult = this.getNextID().ToString();
        //        obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

        //        obj_cmd.Parameters.Add("@PROWITHDRAW_ID", SqlDbType.Int); obj_cmd.Parameters["@PROWITHDRAW_ID"].Value = strResult;
        //        obj_cmd.Parameters.Add("@PROWITHDRAW_WORKDATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROWITHDRAW_WORKDATE"].Value = model.prowithdraw_workdate;

        //        obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;
        //        obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
        //        obj_cmd.Parameters.Add("@PROJOB_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOB_TYPE"].Value = model.projob_type;
        //        obj_cmd.Parameters.Add("@PROJOB_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOB_CODE"].Value = model.projob_code;

        //        obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
        //        obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
        //        obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;
     
        //        obj_cmd.ExecuteNonQuery();

        //        obj_conn.doClose();
                
        //    }
        //    catch (Exception ex)
        //    {
        //        strResult = "";
        //        Message = "ERROR::(PROWITHDRAW.insert)" + ex.ToString();
        //    }

        //    return strResult;
        //}

        //public bool update(cls_TRProwithdraw model)
        //{
        //    bool blnResult = false;
        //    try
        //    {
        //        cls_ctConnection obj_conn = new cls_ctConnection();

        //        System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

        //        obj_str.Append("UPDATE PRO_TR_PROWITHDRAW SET ");

        //         obj_str.Append(" PROWITHDRAW_WORKDATE=@PROWITHDRAW_WORKDATE ");
        //         obj_str.Append(", PROJECT_CODE=@PROJECT_CODE ");
        //         obj_str.Append(", PROJOB_TYPE=@PROJOB_TYPE ");
        //         obj_str.Append(", PROJOB_CODE=@PROJOB_CODE ");

        //        obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
        //        obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
        //        obj_str.Append(", FLAG=@FLAG ");


        //        obj_str.Append(" WHERE WORKER_CODE=@WORKER_CODE ");

                
        //        obj_conn.doConnect();

        //        SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

        //        obj_cmd.Parameters.Add("@PROWITHDRAW_WORKDATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROWITHDRAW_WORKDATE"].Value = model.prowithdraw_workdate;
        //         obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
        //         obj_cmd.Parameters.Add("@PROJOB_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOB_TYPE"].Value = model.projob_type;
        //         obj_cmd.Parameters.Add("@PROJOB_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOB_CODE"].Value = model.projob_code;

        //        obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
        //        obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
        //        obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;
        //         obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

        //        obj_cmd.ExecuteNonQuery();

        //        obj_conn.doClose();

        //        blnResult = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Message = "ERROR::(PROWITHDRAW.update)" + ex.ToString();
        //    }

        //    return blnResult;
        //}
 
}
