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
    public class cls_ctMTProtimepol
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTProtimepol() { }

        public string getMessage() { return this.Message.Replace("PRO_MT_PROTIMEPOL", "").Replace("cls_ctMTProtimepol", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTProtimepol> getData(string condition)
        {
            List<cls_MTProtimepol> list_model = new List<cls_MTProtimepol>();
            cls_MTProtimepol model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PROTIMEPOL_ID");
                obj_str.Append(", PROTIMEPOL_CODE");
                obj_str.Append(", PROTIMEPOL_NAME_TH");
                obj_str.Append(", PROTIMEPOL_NAME_EN");

                obj_str.Append(", ISNULL(PROTIMEPOL_OT, '') AS PROTIMEPOL_OT");
                obj_str.Append(", ISNULL(PROTIMEPOL_ALLW, '') AS PROTIMEPOL_ALLW");
                obj_str.Append(", ISNULL(PROTIMEPOL_DG, '') AS PROTIMEPOL_DG");
                obj_str.Append(", ISNULL(PROTIMEPOL_LV, '') AS PROTIMEPOL_LV");
                obj_str.Append(", ISNULL(PROTIMEPOL_LT, '') AS PROTIMEPOL_LT");

                obj_str.Append(", PROJECT_CODE");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");    

                obj_str.Append(" FROM PRO_MT_PROTIMEPOL");
                obj_str.Append(" WHERE 1=1");
                
                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PROJECT_CODE, PROTIMEPOL_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTProtimepol();

                    model.protimepol_id = Convert.ToInt32(dr["PROTIMEPOL_ID"]);
                    model.protimepol_code = dr["PROTIMEPOL_CODE"].ToString();
                    model.protimepol_name_th = dr["PROTIMEPOL_NAME_TH"].ToString();
                    model.protimepol_name_en = dr["PROTIMEPOL_NAME_EN"].ToString();

                    model.protimepol_ot = dr["PROTIMEPOL_OT"].ToString();
                    model.protimepol_allw = dr["PROTIMEPOL_ALLW"].ToString();
                    model.protimepol_dg = dr["PROTIMEPOL_DG"].ToString();
                    model.protimepol_lv = dr["PROTIMEPOL_LV"].ToString();
                    model.protimepol_lt = dr["PROTIMEPOL_LT"].ToString();

                    model.project_code = dr["PROJECT_CODE"].ToString();

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

        public List<cls_MTProtimepol> getDataByFillter(string project)
        {
            string strCondition = "";

            if (!project.Equals(""))
                strCondition += " AND PROJECT_CODE='" + project + "'";
            
            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(PROTIMEPOL_ID, 1) ");
                obj_str.Append(" FROM PRO_MT_PROTIMEPOL");
                obj_str.Append(" ORDER BY PROTIMEPOL_ID DESC ");

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

        public bool checkDataOld(string project, string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PROTIMEPOL_CODE");
                obj_str.Append(" FROM PRO_MT_PROTIMEPOL");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROTIMEPOL_CODE='" + code + "'");
      
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

        public bool delete(string project, string code)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_MT_PROTIMEPOL");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROTIMEPOL_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BNK004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string project)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_MT_PROTIMEPOL");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");                

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BNK004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_MTProtimepol model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.project_code, model.protimepol_code))
                {
                    return this.update(model);                    
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                          
                obj_str.Append("INSERT INTO PRO_MT_PROTIMEPOL");
                obj_str.Append(" (");
                obj_str.Append("PROTIMEPOL_ID ");
                obj_str.Append(", PROTIMEPOL_CODE ");
                obj_str.Append(", PROTIMEPOL_NAME_TH ");
                obj_str.Append(", PROTIMEPOL_NAME_EN ");

                obj_str.Append(", PROTIMEPOL_OT ");
                obj_str.Append(", PROTIMEPOL_ALLW ");
                obj_str.Append(", PROTIMEPOL_DG ");
                obj_str.Append(", PROTIMEPOL_LV ");
                obj_str.Append(", PROTIMEPOL_LT ");

                obj_str.Append(", PROJECT_CODE ");  

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");          
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@PROTIMEPOL_ID ");
                obj_str.Append(", @PROTIMEPOL_CODE ");
                obj_str.Append(", @PROTIMEPOL_NAME_TH ");
                obj_str.Append(", @PROTIMEPOL_NAME_EN ");

                obj_str.Append(", @PROTIMEPOL_OT ");
                obj_str.Append(", @PROTIMEPOL_ALLW ");
                obj_str.Append(", @PROTIMEPOL_DG ");
                obj_str.Append(", @PROTIMEPOL_LV ");
                obj_str.Append(", @PROTIMEPOL_LT ");

                obj_str.Append(", @PROJECT_CODE ");  

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                     
                obj_str.Append(", '1' ");
                obj_str.Append(" )");
                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.protimepol_id = this.getNextID();

                obj_cmd.Parameters.Add("@PROTIMEPOL_ID", SqlDbType.Int); obj_cmd.Parameters["@PROTIMEPOL_ID"].Value = model.protimepol_id;
                obj_cmd.Parameters.Add("@PROTIMEPOL_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROTIMEPOL_CODE"].Value = model.protimepol_code;
                obj_cmd.Parameters.Add("@PROTIMEPOL_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROTIMEPOL_NAME_TH"].Value = model.protimepol_name_th;
                obj_cmd.Parameters.Add("@PROTIMEPOL_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROTIMEPOL_NAME_EN"].Value = model.protimepol_name_en;

                obj_cmd.Parameters.Add("@PROTIMEPOL_OT", SqlDbType.VarChar); obj_cmd.Parameters["@PROTIMEPOL_OT"].Value = model.protimepol_ot;
                obj_cmd.Parameters.Add("@PROTIMEPOL_ALLW", SqlDbType.VarChar); obj_cmd.Parameters["@PROTIMEPOL_ALLW"].Value = model.protimepol_allw;
                obj_cmd.Parameters.Add("@PROTIMEPOL_DG", SqlDbType.VarChar); obj_cmd.Parameters["@PROTIMEPOL_DG"].Value = model.protimepol_dg;
                obj_cmd.Parameters.Add("@PROTIMEPOL_LV", SqlDbType.VarChar); obj_cmd.Parameters["@PROTIMEPOL_LV"].Value = model.protimepol_lv;
                obj_cmd.Parameters.Add("@PROTIMEPOL_LT", SqlDbType.VarChar); obj_cmd.Parameters["@PROTIMEPOL_LT"].Value = model.protimepol_lt;

                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;

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

        public bool update(cls_MTProtimepol model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PRO_MT_PROTIMEPOL SET ");
                obj_str.Append(" PROTIMEPOL_NAME_TH=@PROTIMEPOL_NAME_TH ");
                obj_str.Append(", PROTIMEPOL_NAME_EN=@PROTIMEPOL_NAME_EN ");
                obj_str.Append(", PROTIMEPOL_OT=@PROTIMEPOL_OT ");
                obj_str.Append(", PROTIMEPOL_ALLW=@PROTIMEPOL_ALLW ");
                obj_str.Append(", PROTIMEPOL_DG=@PROTIMEPOL_DG ");
                obj_str.Append(", PROTIMEPOL_LV=@PROTIMEPOL_LV ");
                obj_str.Append(", PROTIMEPOL_LT=@PROTIMEPOL_LT ");
                
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE PROTIMEPOL_ID=@PROTIMEPOL_ID ");            

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROTIMEPOL_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROTIMEPOL_NAME_TH"].Value = model.protimepol_name_th;
                obj_cmd.Parameters.Add("@PROTIMEPOL_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROTIMEPOL_NAME_EN"].Value = model.protimepol_name_en;

                obj_cmd.Parameters.Add("@PROTIMEPOL_OT", SqlDbType.VarChar); obj_cmd.Parameters["@PROTIMEPOL_OT"].Value = model.protimepol_ot;
                obj_cmd.Parameters.Add("@PROTIMEPOL_ALLW", SqlDbType.VarChar); obj_cmd.Parameters["@PROTIMEPOL_ALLW"].Value = model.protimepol_allw;
                obj_cmd.Parameters.Add("@PROTIMEPOL_DG", SqlDbType.VarChar); obj_cmd.Parameters["@PROTIMEPOL_DG"].Value = model.protimepol_dg;
                obj_cmd.Parameters.Add("@PROTIMEPOL_LV", SqlDbType.VarChar); obj_cmd.Parameters["@PROTIMEPOL_LV"].Value = model.protimepol_lv;
                obj_cmd.Parameters.Add("@PROTIMEPOL_LT", SqlDbType.VarChar); obj_cmd.Parameters["@PROTIMEPOL_LT"].Value = model.protimepol_lt;
      
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@PROTIMEPOL_ID", SqlDbType.Int); obj_cmd.Parameters["@PROTIMEPOL_ID"].Value = model.protimepol_id;

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
