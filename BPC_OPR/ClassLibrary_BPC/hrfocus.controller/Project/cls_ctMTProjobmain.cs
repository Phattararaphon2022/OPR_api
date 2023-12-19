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
    public class cls_ctMTProjobmain
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTProjobmain() { }

        public string getMessage() { return this.Message.Replace("PRO_MT_PROJOBMAIN", "").Replace("cls_ctMTProjobmain", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTProjobmain> getData(string language, string condition)
        {
            List<cls_MTProjobmain> list_model = new List<cls_MTProjobmain>();
            cls_MTProjobmain model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PROJOBMAIN_ID");
                obj_str.Append(", PROJOBMAIN_CODE");
                obj_str.Append(", PROJOBMAIN_NAME_TH");
                obj_str.Append(", PROJOBMAIN_NAME_EN");

                obj_str.Append(", PROJOBMAIN_JOBTYPE");
                obj_str.Append(", ISNULL(PROJOBMAIN_FROMDATE, '01/01/1900') AS PROJOBMAIN_FROMDATE");
                obj_str.Append(", ISNULL(PROJOBMAIN_TODATE, '01/01/1900') AS PROJOBMAIN_TODATE");

                obj_str.Append(", PROJOBMAIN_TYPE");
                obj_str.Append(", ISNULL(PROJOBMAIN_TIMEPOL, '') AS PROJOBMAIN_TIMEPOL");
                obj_str.Append(", ISNULL(PROJOBMAIN_SLIP, '') AS PROJOBMAIN_SLIP");
                obj_str.Append(", ISNULL(PROJOBMAIN_UNIFORM, '') AS PROJOBMAIN_UNIFORM");

                obj_str.Append(", PROJECT_CODE");
                obj_str.Append(", VERSION");

                if (language.Equals("TH"))
                {
                    obj_str.Append(", PRO_MT_PROTYPE.PROTYPE_NAME_TH    AS PROTYPE_NAME");
                }
                else
                {
                     obj_str.Append(", PRO_MT_PROTYPE.PROTYPE_NAME_EN    AS PROTYPE_NAME");
                }

                obj_str.Append(", ISNULL(PRO_MT_PROJOBMAIN.MODIFIED_BY, PRO_MT_PROJOBMAIN.CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(PRO_MT_PROJOBMAIN.MODIFIED_DATE, PRO_MT_PROJOBMAIN.CREATED_DATE) AS MODIFIED_DATE");    

                obj_str.Append(" FROM PRO_MT_PROJOBMAIN");
                obj_str.Append("  INNER JOIN PRO_MT_PROTYPE ON PRO_MT_PROTYPE.PROTYPE_CODE=PRO_MT_PROJOBMAIN.PROJOBMAIN_JOBTYPE");

                obj_str.Append(" WHERE 1=1");
                
                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PROJOBMAIN_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTProjobmain();

                    model.projobmain_id = Convert.ToInt32(dr["PROJOBMAIN_ID"]);
                    model.projobmain_code = dr["PROJOBMAIN_CODE"].ToString();
                    model.projobmain_name_th = dr["PROJOBMAIN_NAME_TH"].ToString();
                    model.projobmain_name_en = dr["PROJOBMAIN_NAME_EN"].ToString();

                    model.projobmain_jobtype = dr["PROJOBMAIN_JOBTYPE"].ToString();
                    model.projobmain_fromdate = Convert.ToDateTime(dr["PROJOBMAIN_FROMDATE"]);
                    model.projobmain_todate = Convert.ToDateTime(dr["PROJOBMAIN_TODATE"]);

                    model.projobmain_type = dr["PROJOBMAIN_TYPE"].ToString();
                    model.projobmain_timepol = dr["PROJOBMAIN_TIMEPOL"].ToString();
                    model.projobmain_slip = dr["PROJOBMAIN_SLIP"].ToString();
                    model.projobmain_uniform = dr["PROJOBMAIN_UNIFORM"].ToString();
                    model.project_code = dr["PROJECT_CODE"].ToString();

                    model.version = dr["VERSION"].ToString();
                    model.protype_name = dr["PROTYPE_NAME"].ToString();

                    
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                                                                                            
                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "BNK001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTProjobmain> getDataByFillter(string language, string project, string version, string jobcode)
        {
            string strCondition = "";

            if (!project.Equals(""))
                strCondition += " AND PROJECT_CODE='" + project + "'";

            if (!version.Equals(""))
                strCondition += " AND VERSION='" + version + "'";

            if (!jobcode.Equals(""))
                strCondition += " AND PROJOBMAIN_CODE='" + jobcode + "'";



            return this.getData(language,strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(PROJOBMAIN_ID, 1) ");
                obj_str.Append(" FROM PRO_MT_PROJOBMAIN");
                obj_str.Append(" ORDER BY PROJOBMAIN_ID DESC ");

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

        public bool checkDataOld(string version, string project, string code ,int id )
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PROJOBMAIN_CODE");
                obj_str.Append(" FROM PRO_MT_PROJOBMAIN");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROJOBMAIN_CODE='" + code + "'");
                if (!version.Equals(""))
                {
                    obj_str.Append(" AND VERSION='" + version + "'");
                }
                if (!id.Equals(0))
                {
                    obj_str.Append(" AND PROJOBMAIN_ID='" + id + "'");
                }
                
 
      
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

        public bool delete2(string version, string project, string projobmin)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_MT_PROJOBMAIN");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                if (!projobmin.Equals(""))
                {
                    obj_str.Append(" AND PROJOBMAIN_CODE='" + projobmin + "'");
                }
                if (!version.Equals(""))
                {
                    obj_str.Append(" AND VERSION='" + version + "'");
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

        public bool delete(string version, string project )
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_MT_PROJOBMAIN");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");                
                obj_str.Append(" AND VERSION='" + version + "'");
 
                
                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BNK004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string version, string project, string code )
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_MT_PROJOBMAIN");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                if (!code.Equals(""))
                    obj_str.Append(" AND PROJOBMAIN_CODE='" + code + "'");
                obj_str.Append(" AND VERSION='" + version + "'");
 
                
                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BNK004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_MTProjobmain model)
        {
            bool blnResult = false;
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.version, model.project_code, model.projobmain_code ,0 ))
                {
                    if (model.projobmain_id.Equals(0))
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
                          
                obj_str.Append("INSERT INTO PRO_MT_PROJOBMAIN");
                obj_str.Append(" (");
                obj_str.Append("PROJOBMAIN_ID ");
                obj_str.Append(", PROJOBMAIN_CODE ");
                obj_str.Append(", PROJOBMAIN_NAME_TH ");
                obj_str.Append(", PROJOBMAIN_NAME_EN ");
                obj_str.Append(", PROJOBMAIN_JOBTYPE ");
                obj_str.Append(", PROJOBMAIN_FROMDATE ");
                obj_str.Append(", PROJOBMAIN_TODATE ");

                

                obj_str.Append(", PROJOBMAIN_TYPE ");
               

                obj_str.Append(", PROJOBMAIN_TIMEPOL ");
                obj_str.Append(", PROJOBMAIN_SLIP ");
                obj_str.Append(", PROJOBMAIN_UNIFORM ");
                obj_str.Append(", PROJECT_CODE ");

                obj_str.Append(", VERSION ");
 
                

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");          
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@PROJOBMAIN_ID ");
                obj_str.Append(", @PROJOBMAIN_CODE ");
                obj_str.Append(", @PROJOBMAIN_NAME_TH ");
                obj_str.Append(", @PROJOBMAIN_NAME_EN ");
                obj_str.Append(", @PROJOBMAIN_JOBTYPE ");
                obj_str.Append(", @PROJOBMAIN_FROMDATE ");
                obj_str.Append(", @PROJOBMAIN_TODATE ");

                

                
                obj_str.Append(", @PROJOBMAIN_TYPE ");
              

                obj_str.Append(", @PROJOBMAIN_TIMEPOL ");
                obj_str.Append(", @PROJOBMAIN_SLIP ");
                obj_str.Append(", @PROJOBMAIN_UNIFORM ");
                obj_str.Append(", @PROJECT_CODE ");

                obj_str.Append(", @VERSION ");
 
                

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");
                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.projobmain_id = this.getNextID();

                obj_cmd.Parameters.Add("@PROJOBMAIN_ID", SqlDbType.Int); obj_cmd.Parameters["@PROJOBMAIN_ID"].Value = model.projobmain_id;
                obj_cmd.Parameters.Add("@PROJOBMAIN_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBMAIN_CODE"].Value = model.projobmain_code;
                obj_cmd.Parameters.Add("@PROJOBMAIN_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBMAIN_NAME_TH"].Value = model.projobmain_name_th;
                obj_cmd.Parameters.Add("@PROJOBMAIN_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBMAIN_NAME_EN"].Value = model.projobmain_name_en;
                

                obj_cmd.Parameters.Add("@PROJOBMAIN_JOBTYPE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBMAIN_JOBTYPE"].Value = model.projobmain_jobtype;
                obj_cmd.Parameters.Add("@PROJOBMAIN_FROMDATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROJOBMAIN_FROMDATE"].Value = model.projobmain_fromdate;
                obj_cmd.Parameters.Add("@PROJOBMAIN_TODATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROJOBMAIN_TODATE"].Value = model.projobmain_todate;


                obj_cmd.Parameters.Add("@PROJOBMAIN_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBMAIN_TYPE"].Value = model.projobmain_type;
                obj_cmd.Parameters.Add("@PROJOBMAIN_TIMEPOL", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBMAIN_TIMEPOL"].Value = model.projobmain_timepol;
                obj_cmd.Parameters.Add("@PROJOBMAIN_SLIP", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBMAIN_SLIP"].Value = model.projobmain_slip;
                obj_cmd.Parameters.Add("@PROJOBMAIN_UNIFORM", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBMAIN_UNIFORM"].Value = model.projobmain_uniform;
                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;

                obj_cmd.Parameters.Add("@VERSION", SqlDbType.VarChar); obj_cmd.Parameters["@VERSION"].Value = model.version;
 
                
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                                     
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

        public bool update(cls_MTProjobmain model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PRO_MT_PROJOBMAIN SET ");
                obj_str.Append(" PROJOBMAIN_NAME_TH=@PROJOBMAIN_NAME_TH ");
                obj_str.Append(", PROJOBMAIN_NAME_EN=@PROJOBMAIN_NAME_EN ");
                obj_str.Append(", PROJOBMAIN_JOBTYPE=@PROJOBMAIN_JOBTYPE ");
                obj_str.Append(", PROJOBMAIN_FROMDATE=@PROJOBMAIN_FROMDATE ");
                obj_str.Append(", PROJOBMAIN_TODATE=@PROJOBMAIN_TODATE ");


                
                obj_str.Append(", PROJOBMAIN_TYPE=@PROJOBMAIN_TYPE ");
                obj_str.Append(", PROJOBMAIN_TIMEPOL=@PROJOBMAIN_TIMEPOL ");
                obj_str.Append(", PROJOBMAIN_SLIP=@PROJOBMAIN_SLIP ");
                obj_str.Append(", PROJOBMAIN_UNIFORM=@PROJOBMAIN_UNIFORM ");
               
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE PROJOBMAIN_ID=@PROJOBMAIN_ID ");            

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROJOBMAIN_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBMAIN_NAME_TH"].Value = model.projobmain_name_th;
                obj_cmd.Parameters.Add("@PROJOBMAIN_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBMAIN_NAME_EN"].Value = model.projobmain_name_en;
                obj_cmd.Parameters.Add("@PROJOBMAIN_JOBTYPE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBMAIN_JOBTYPE"].Value = model.projobmain_jobtype;
                obj_cmd.Parameters.Add("@PROJOBMAIN_FROMDATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROJOBMAIN_FROMDATE"].Value = model.projobmain_fromdate;
                obj_cmd.Parameters.Add("@PROJOBMAIN_TODATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROJOBMAIN_TODATE"].Value = model.projobmain_todate;

               
                obj_cmd.Parameters.Add("@PROJOBMAIN_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBMAIN_TYPE"].Value = model.projobmain_type;
               

                obj_cmd.Parameters.Add("@PROJOBMAIN_TIMEPOL", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBMAIN_TIMEPOL"].Value = model.projobmain_timepol;
                obj_cmd.Parameters.Add("@PROJOBMAIN_SLIP", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBMAIN_SLIP"].Value = model.projobmain_slip;
                obj_cmd.Parameters.Add("@PROJOBMAIN_UNIFORM", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBMAIN_UNIFORM"].Value = model.projobmain_uniform;      

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@PROJOBMAIN_ID", SqlDbType.Int); obj_cmd.Parameters["@PROJOBMAIN_ID"].Value = model.projobmain_id;

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
