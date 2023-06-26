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
    public class cls_ctMTProjobsub
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTProjobsub() { }

        public string getMessage() { return this.Message.Replace("PRO_MT_PROJOBSUB", "").Replace("cls_ctMTProjobsub", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTProjobsub> getData(string condition)
        {
            List<cls_MTProjobsub> list_model = new List<cls_MTProjobsub>();
            cls_MTProjobsub model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PROJOBSUB_ID");
                obj_str.Append(", PROJOBSUB_CODE");
                obj_str.Append(", PROJOBSUB_NAME_TH");
                obj_str.Append(", PROJOBSUB_NAME_EN");

                obj_str.Append(", PROJECT_CODE");
                obj_str.Append(", VERSION");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");    

                obj_str.Append(" FROM PRO_MT_PROJOBSUB");
                obj_str.Append(" WHERE 1=1");
                
                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PROJOBSUB_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTProjobsub();

                    model.projobsub_id = Convert.ToInt32(dr["PROJOBSUB_ID"]);
                    model.projobsub_code = dr["PROJOBSUB_CODE"].ToString();
                    model.projobsub_name_th = dr["PROJOBSUB_NAME_TH"].ToString();
                    model.projobsub_name_en = dr["PROJOBSUB_NAME_EN"].ToString();

                    model.project_code = dr["PROJECT_CODE"].ToString();
                    model.version = dr["VERSION"].ToString();

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

        public List<cls_MTProjobsub> getDataByFillter(string project, string version)
        {
            string strCondition = "";

            if (!project.Equals(""))
                strCondition += " AND PROJECT_CODE='" + project + "'";

            if (!version.Equals(""))
                strCondition += " AND VERSION='" + version + "'";
            
            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(PROJOBSUB_ID, 1) ");
                obj_str.Append(" FROM PRO_MT_PROJOBSUB");
                obj_str.Append(" ORDER BY PROJOBSUB_ID DESC ");

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

        public bool checkDataOld(string version, string project, string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PROJOBSUB_CODE");
                obj_str.Append(" FROM PRO_MT_PROJOBSUB");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROJOBSUB_CODE='" + code + "'");
                obj_str.Append(" AND VERSION='" + version + "'");
      
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

        public bool delete(string version, string project)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_MT_PROJOBSUB");
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

        public bool delete(string version, string project, string code)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_MT_PROJOBSUB");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROJOBSUB_CODE='" + code + "'");
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

        public bool insert(cls_MTProjobsub model)
        {
            bool blnResult = false;
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.version, model.project_code, model.projobsub_code))
                {
                    return this.update(model);               
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                          
                obj_str.Append("INSERT INTO PRO_MT_PROJOBSUB");
                obj_str.Append(" (");
                obj_str.Append("PROJOBSUB_ID ");
                obj_str.Append(", PROJOBSUB_CODE ");
                obj_str.Append(", PROJOBSUB_NAME_TH ");
                obj_str.Append(", PROJOBSUB_NAME_EN ");

                obj_str.Append(", PROJECT_CODE ");
                obj_str.Append(", VERSION ");  

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");          
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@PROJOBSUB_ID ");
                obj_str.Append(", @PROJOBSUB_CODE ");
                obj_str.Append(", @PROJOBSUB_NAME_TH ");
                obj_str.Append(", @PROJOBSUB_NAME_EN ");
                                
                obj_str.Append(", @PROJECT_CODE ");
                obj_str.Append(", @VERSION ");  

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");
                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.projobsub_id = this.getNextID();

                obj_cmd.Parameters.Add("@PROJOBSUB_ID", SqlDbType.Int); obj_cmd.Parameters["@PROJOBSUB_ID"].Value = model.projobsub_id;
                obj_cmd.Parameters.Add("@PROJOBSUB_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBSUB_CODE"].Value = model.projobsub_code;
                obj_cmd.Parameters.Add("@PROJOBSUB_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBSUB_NAME_TH"].Value = model.projobsub_name_th;
                obj_cmd.Parameters.Add("@PROJOBSUB_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBSUB_NAME_EN"].Value = model.projobsub_name_en;

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

        public bool update(cls_MTProjobsub model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PRO_MT_PROJOBSUB SET ");
                obj_str.Append(" PROJOBSUB_NAME_TH=@PROJOBSUB_NAME_TH ");
                obj_str.Append(", PROJOBSUB_NAME_EN=@PROJOBSUB_NAME_EN ");
                               
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE PROJOBSUB_ID=@PROJOBSUB_ID ");            

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROJOBSUB_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBSUB_NAME_TH"].Value = model.projobsub_name_th;
                obj_cmd.Parameters.Add("@PROJOBSUB_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOBSUB_NAME_EN"].Value = model.projobsub_name_en;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@PROJOBSUB_ID", SqlDbType.Int); obj_cmd.Parameters["@PROJOBSUB_ID"].Value = model.projobsub_id;

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
