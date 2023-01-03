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
    public class cls_ctTRPCost
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRPCost() { }

        public string getMessage() { return this.Message.Replace("OPR_TR_PCOST", "").Replace("cls_ctTRPCost", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRPCost> getData(string condition)
        {
            List<cls_TRPCost> list_model = new List<cls_TRPCost>();
            cls_TRPCost model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PCOST_ID");
                obj_str.Append(", PCOST_CODE");
                obj_str.Append(", PCOST_AMOUNT");
                obj_str.Append(", PCOST_TYPE");
                obj_str.Append(", PCOST_START");
                obj_str.Append(", PCOST_END");

                obj_str.Append(", PROJECT_CODE");
                obj_str.Append(", PCOST_VERSION");         

                obj_str.Append(", ISNULL(PCOST_ALLWCODE, '') AS PCOST_ALLWCODE");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");    

                obj_str.Append(" FROM OPR_TR_PCOST");
                obj_str.Append(" WHERE 1=1");
                
                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY CREATED_DATE DESC");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRPCost();

                    model.pcost_id = Convert.ToInt32(dr["PCOST_ID"]);
                    model.pcost_code = dr["PCOST_CODE"].ToString();
                    model.pcost_amount = dr["PCOST_AMOUNT"].ToString();
                    model.pcost_type = dr["PCOST_TYPE"].ToString();
                    model.pcost_start = Convert.ToDateTime(dr["PCOST_START"]);
                    model.pcost_end = Convert.ToDateTime(dr["PCOST_END"]);
                    model.pcost_allwcode = dr["PCOST_ALLWCODE"].ToString();
                    model.project_code = dr["PROJECT_CODE"].ToString();
                    model.pcost_version = dr["PCOST_VERSION"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                                                                                            
                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "PCS001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRPCost> getDataByFillter(string project, string type)
        {
            string strCondition = "";

            if (!project.Equals(""))
                strCondition += " AND PROJECT_CODE='" + project + "'";

            if (!type.Equals(""))
                strCondition += " AND PCOST_TYPE='" + type + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(PCOST_ID, 1) ");
                obj_str.Append(" FROM OPR_TR_PCOST");
                obj_str.Append(" ORDER BY PCOST_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "PCS002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string project, string code, DateTime start)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PROJECT_CODE");
                obj_str.Append(" FROM OPR_TR_PCOST");
                obj_str.Append(" WHERE PROJECT_CODE='" + code + "'");
                obj_str.Append(" AND PCOST_CODE='" + code + "'");
                obj_str.Append(" AND PCOST_START='" + start.ToString("MM/dd/yyyy") + "'");
      
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "PCS003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string id)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM OPR_TR_PCOST");
                obj_str.Append(" WHERE PCOST_ID='" + id + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "PCS004:" + ex.ToString();
            }

            return blnResult;
        }
        
        public string insert(cls_TRPCost model)
        {
            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.project_code, model.pcost_code, model.pcost_start))
                {
                    if (this.update(model))
                        return model.pcost_id.ToString();
                    else
                        return "";                    
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                          
                obj_str.Append("INSERT INTO OPR_TR_PCOST");
                obj_str.Append(" (");
                obj_str.Append("PCOST_ID ");
                obj_str.Append(", PCOST_CODE ");
                obj_str.Append(", PCOST_AMOUNT ");
                obj_str.Append(", PCOST_TYPE ");
                obj_str.Append(", PCOST_START ");
                obj_str.Append(", PCOST_END ");
                obj_str.Append(", PCOST_ALLWCODE ");
                obj_str.Append(", PROJECT_CODE ");
                obj_str.Append(", PCOST_VERSION ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");          
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@PCOST_ID ");
                obj_str.Append(", @PCOST_CODE ");
                obj_str.Append(", @PCOST_AMOUNT ");
                obj_str.Append(", @PCOST_TYPE ");
                obj_str.Append(", @PCOST_START ");
                obj_str.Append(", @PCOST_END ");
                obj_str.Append(", @PCOST_ALLWCODE ");
                obj_str.Append(", @PROJECT_CODE ");
                obj_str.Append(", @PCOST_VERSION ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");
                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.pcost_id = this.getNextID();

                obj_cmd.Parameters.Add("@PCOST_ID", SqlDbType.Int); obj_cmd.Parameters["@PCOST_ID"].Value = model.pcost_id;
                obj_cmd.Parameters.Add("@PCOST_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PCOST_CODE"].Value = model.pcost_code;
                obj_cmd.Parameters.Add("@PCOST_AMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@PCOST_AMOUNT"].Value = model.pcost_amount;
                obj_cmd.Parameters.Add("@PCOST_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@PCOST_TYPE"].Value = model.pcost_type;
                obj_cmd.Parameters.Add("@PCOST_START", SqlDbType.DateTime); obj_cmd.Parameters["@PCOST_START"].Value = model.pcost_start;
                obj_cmd.Parameters.Add("@PCOST_END", SqlDbType.DateTime); obj_cmd.Parameters["@PCOST_END"].Value = model.pcost_end;
                obj_cmd.Parameters.Add("@PCOST_ALLWCODE", SqlDbType.VarChar); obj_cmd.Parameters["@PCOST_ALLWCODE"].Value = model.pcost_allwcode;
                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
                obj_cmd.Parameters.Add("@PCOST_VERSION", SqlDbType.VarChar); obj_cmd.Parameters["@PCOST_VERSION"].Value = model.pcost_version;


                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                                     
                obj_cmd.ExecuteNonQuery();
                                
                obj_conn.doClose();
                strResult = model.pcost_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "PCS005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        public bool update(cls_TRPCost model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE OPR_TR_PCOST SET ");
                obj_str.Append(" PCOST_AMOUNT=@PCOST_AMOUNT ");
                obj_str.Append(", PCOST_TYPE=@PCOST_TYPE ");
                obj_str.Append(", PCOST_START=@PCOST_START ");
                obj_str.Append(", PCOST_END=@PCOST_END ");
                obj_str.Append(", PCOST_ALLWCODE=@PCOST_ALLWCODE ");
                obj_str.Append(", PROJECT_CODE=@PROJECT_CODE ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(" WHERE PROJECT_ID=@PROJECT_ID ");            

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PCOST_AMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@PCOST_AMOUNT"].Value = model.pcost_amount;
                obj_cmd.Parameters.Add("@PCOST_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@PCOST_TYPE"].Value = model.pcost_type;
                obj_cmd.Parameters.Add("@PCOST_START", SqlDbType.DateTime); obj_cmd.Parameters["@PCOST_START"].Value = model.pcost_start;
                obj_cmd.Parameters.Add("@PCOST_END", SqlDbType.DateTime); obj_cmd.Parameters["@PCOST_END"].Value = model.pcost_end;
                obj_cmd.Parameters.Add("@PCOST_ALLWCODE", SqlDbType.VarChar); obj_cmd.Parameters["@PCOST_ALLWCODE"].Value = model.pcost_allwcode;
                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;

                obj_cmd.Parameters.Add("@PCOST_ID", SqlDbType.Int); obj_cmd.Parameters["@PCOST_ID"].Value = model.pcost_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "PCS006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}
