using ClassLibrary_BPC.hrfocus.model.Project;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller.Project
{
    public class cls_ctTRProequipmentreq
     {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRProequipmentreq() { }

        public string getMessage() { return this.Message.Replace("PRO_TR_PROEQUIPMENTREQ", "").Replace("cls_ctTRProjobmachine", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRProequipmentreq> getData(string condition)
        {
            List<cls_TRProequipmentreq> list_model = new List<cls_TRProequipmentreq>();
            cls_TRProequipmentreq model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PROEQUIPMENTREQ_ID");
                obj_str.Append(", PROUNIFORM_CODE");

                obj_str.Append(", PROEQUIPMENTREQ_DATE");
                //obj_str.Append(", ISNULL(PROEQUIPMENTREQ_DATE, '01/01/2999') AS PROEQUIPMENTREQ_DATE");


                obj_str.Append(", PROEQUIPMENTREQ_QTY");
                obj_str.Append(", PROEQUIPMENTREQ_NOTE");

                obj_str.Append(", PROEQUIPMENTREQ_BY");
                obj_str.Append(", PROEQUIPMENTTYPE_CODE");
                               
                obj_str.Append(", PROJOB_CODE");     
                obj_str.Append(", PROJECT_CODE");                

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PRO_TR_PROEQUIPMENTREQ");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PROJECT_CODE, PROJOB_CODE ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRProequipmentreq();

                    model.proequipmentreq_id = Convert.ToInt32(dr["PROEQUIPMENTREQ_ID"]);
                    model.prouniform_code = Convert.ToString(dr["PROUNIFORM_CODE"]);


                    model.proequipmentreq_date = Convert.ToDateTime(dr["PROEQUIPMENTREQ_DATE"]);
                    model.proequipmentreq_qty = Convert.ToInt32(dr["PROEQUIPMENTREQ_QTY"]);

                    model.proequipmentreq_note = Convert.ToString(dr["PROEQUIPMENTREQ_NOTE"]);
                    model.proequipmentreq_by = Convert.ToString(dr["PROEQUIPMENTREQ_BY"]);
                    model.proequipmenttype_code = Convert.ToString(dr["PROEQUIPMENTTYPE_CODE"]);

                    model.projob_code = Convert.ToString(dr["PROJOB_CODE"]);                                        
                    model.project_code = Convert.ToString(dr["PROJECT_CODE"]);
                   
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "QTY001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRProequipmentreq> getDataByFillter(string project, string job, string uniform, string code)
        {
            string strCondition = "";

            if (!project.Equals(""))
                strCondition += " AND PROJECT_CODE='" + project + "'";

            if (!job.Equals(""))
                strCondition += " AND PROJOB_CODE='" + job + "'";

            if (!uniform.Equals(""))
                strCondition += " AND PROUNIFORM_CODE='" + uniform + "'";

            if (!code.Equals(""))
                strCondition += " AND PROEQUIPMENTTYPE_CODE='" + code + "'";
            


            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(PROEQUIPMENTREQ_ID, 1) ");
                obj_str.Append(" FROM PRO_TR_PROEQUIPMENTREQ");
                obj_str.Append(" ORDER BY PROEQUIPMENTREQ_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "QTY002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string project, string job, string uniform, string code,int id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PROEQUIPMENTREQ_ID");
                obj_str.Append(" FROM PRO_TR_PROEQUIPMENTREQ");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                if (!job.Equals(""))
                {
                    obj_str.Append(" AND PROJOB_CODE='" + job + "'");
                }
                if (!uniform.Equals(""))
                {
                    obj_str.Append(" AND PROUNIFORM_CODE='" + uniform + "'");
                }
                if (!code.Equals(""))
                {
                    obj_str.Append(" AND PROEQUIPMENTTYPE_CODE='" + code + "'");
                }
                if (!id.Equals(0))
                {
                    obj_str.Append(" AND PROEQUIPMENTREQ_ID='" + id + "'");
                }
                

 
                 DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "QTY003:" + ex.ToString();
            }

            return blnResult;
        }

       

        public bool delete(string project,string job,string uniform )
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_TR_PROEQUIPMENTREQ");
                
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                if (!job.Equals(""))
                {
                    obj_str.Append(" AND PROJOB_CODE='" + job + "'");
                }
                if (!uniform.Equals(""))
                {
                    obj_str.Append(" AND PROUNIFORM_CODE='" + uniform + "'");
                }
               
                
                //obj_str.Append(" AND PROEQUIPMENTTYPE_CODE='" + code + "'");


                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "QTY004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRProequipmentreq model)
        {
            bool blnResult = false;
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.project_code, model.projob_code, model.prouniform_code, model.proequipmenttype_code,model.proequipmentreq_id))
                {
                    if (model.proequipmentreq_id.Equals(0))
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

                obj_str.Append("INSERT INTO PRO_TR_PROEQUIPMENTREQ");
                obj_str.Append(" (");
                obj_str.Append("PROEQUIPMENTREQ_ID ");
                obj_str.Append(", PROUNIFORM_CODE ");
                obj_str.Append(", PROEQUIPMENTREQ_DATE ");
                obj_str.Append(", PROEQUIPMENTREQ_QTY ");

                obj_str.Append(", PROEQUIPMENTREQ_NOTE ");
                obj_str.Append(", PROEQUIPMENTREQ_BY ");
                obj_str.Append(", PROEQUIPMENTTYPE_CODE ");  
                
                obj_str.Append(", PROJOB_CODE ");     
                obj_str.Append(", PROJECT_CODE ");      
         
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@PROEQUIPMENTREQ_ID ");
                obj_str.Append(", @PROUNIFORM_CODE ");
                obj_str.Append(", @PROEQUIPMENTREQ_DATE ");
                obj_str.Append(", @PROEQUIPMENTREQ_QTY ");

                obj_str.Append(", @PROEQUIPMENTREQ_NOTE ");
                obj_str.Append(", @PROEQUIPMENTREQ_BY ");
                obj_str.Append(", @PROEQUIPMENTTYPE_CODE ");
              
                obj_str.Append(", @PROJOB_CODE ");
                obj_str.Append(", @PROJECT_CODE ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROEQUIPMENTREQ_ID", SqlDbType.Int); obj_cmd.Parameters["@PROEQUIPMENTREQ_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@PROUNIFORM_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROUNIFORM_CODE"].Value = model.prouniform_code;


                obj_cmd.Parameters.Add("@PROEQUIPMENTREQ_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROEQUIPMENTREQ_DATE"].Value = model.proequipmentreq_date;
                 obj_cmd.Parameters.Add("@PROEQUIPMENTREQ_QTY", SqlDbType.Int); obj_cmd.Parameters["@PROEQUIPMENTREQ_QTY"].Value = model.proequipmentreq_qty;

                 obj_cmd.Parameters.Add("@PROEQUIPMENTREQ_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@PROEQUIPMENTREQ_NOTE"].Value = model.proequipmentreq_note;
                 obj_cmd.Parameters.Add("@PROEQUIPMENTREQ_BY", SqlDbType.VarChar); obj_cmd.Parameters["@PROEQUIPMENTREQ_BY"].Value = model.proequipmentreq_by;
                 obj_cmd.Parameters.Add("@PROEQUIPMENTTYPE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROEQUIPMENTTYPE_CODE"].Value = model.proequipmenttype_code;               


                obj_cmd.Parameters.Add("@PROJOB_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOB_CODE"].Value = model.projob_code;               
                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
                
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
               
                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "QTY005:" + ex.ToString();               
            }

            return blnResult;
        }

        public bool update(cls_TRProequipmentreq model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PRO_TR_PROEQUIPMENTREQ SET ");

                obj_str.Append(" PROUNIFORM_CODE=@PROUNIFORM_CODE ");
                obj_str.Append(", PROEQUIPMENTREQ_DATE=@PROEQUIPMENTREQ_DATE ");
                obj_str.Append(", PROEQUIPMENTREQ_QTY=@PROEQUIPMENTREQ_QTY ");
                obj_str.Append(", PROEQUIPMENTREQ_NOTE=@PROEQUIPMENTREQ_NOTE ");
                obj_str.Append(", PROEQUIPMENTREQ_BY=@PROEQUIPMENTREQ_BY ");
                obj_str.Append(", PROEQUIPMENTTYPE_CODE=@PROEQUIPMENTTYPE_CODE ");
                obj_str.Append(", PROJOB_CODE=@PROJOB_CODE ");
                obj_str.Append(", PROJECT_CODE=@PROJECT_CODE ");


                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE PROEQUIPMENTREQ_ID=@PROEQUIPMENTREQ_ID ");
               
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROUNIFORM_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROUNIFORM_CODE"].Value = model.prouniform_code;
                obj_cmd.Parameters.Add("@PROEQUIPMENTREQ_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROEQUIPMENTREQ_DATE"].Value = model.proequipmentreq_date;
                obj_cmd.Parameters.Add("@PROEQUIPMENTREQ_QTY", SqlDbType.Int); obj_cmd.Parameters["@PROEQUIPMENTREQ_QTY"].Value = model.proequipmentreq_qty;

                obj_cmd.Parameters.Add("@PROEQUIPMENTREQ_NOTE", SqlDbType.VarChar); obj_cmd.Parameters["@PROEQUIPMENTREQ_NOTE"].Value = model.proequipmentreq_note;
                obj_cmd.Parameters.Add("@PROEQUIPMENTREQ_BY", SqlDbType.VarChar); obj_cmd.Parameters["@PROEQUIPMENTREQ_BY"].Value = model.proequipmentreq_by;
                obj_cmd.Parameters.Add("@PROEQUIPMENTTYPE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROEQUIPMENTTYPE_CODE"].Value = model.proequipmenttype_code;


                obj_cmd.Parameters.Add("@PROJOB_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJOB_CODE"].Value = model.projob_code;
                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@PROEQUIPMENTREQ_ID", SqlDbType.Int); obj_cmd.Parameters["@PROEQUIPMENTREQ_ID"].Value = model.proequipmentreq_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "QTY006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}