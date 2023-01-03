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
    public class cls_ctSYSApilog
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctSYSApilog() { }

        public string getMessage() { return this.Message.Replace("OPR_SYS_APILOG", "").Replace("cls_ctSYSApilog", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_SYSApilog> getData(string condition)
        {
            List<cls_SYSApilog> list_model = new List<cls_SYSApilog>();
            cls_SYSApilog model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("APILOG_ID");
                obj_str.Append(", APILOG_CODE");
                obj_str.Append(", APILOG_DATA");
                obj_str.Append(", APILOG_STATUS");
                obj_str.Append(", ISNULL(APILOG_MESSAGE, '') AS APILOG_MESSAGE");
                obj_str.Append(", APILOG_BY");
                obj_str.Append(", APILOG_DATE"); 

                obj_str.Append(" FROM OPR_SYS_APILOG");
                obj_str.Append(" WHERE 1=1");
                
                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY APILOG_DATE DESC");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_SYSApilog();

                    model.apilog_id = Convert.ToDouble(dr["APILOG_ID"]);
                    model.apilog_code = dr["APILOG_CODE"].ToString();
                    model.apilog_data = dr["APILOG_DATA"].ToString();
                    model.apilog_status = dr["APILOG_STATUS"].ToString();
                    model.apilog_message = dr["APILOG_MESSAGE"].ToString();
                    model.apilog_by = dr["APILOG_BY"].ToString();
                    model.apilog_date = Convert.ToDateTime(dr["APILOG_DATE"]);
                                                                                            
                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "LAPI001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_SYSApilog> getDataByFillter(string code, DateTime start, DateTime end)
        {
            string strCondition = "";

            if (!code.Equals(""))
                strCondition += " AND APILOG_CODE='" + code + "'";

            strCondition += " AND (APILOG_DATE BETWEEN '" + start.ToString("MM/dd/yyyy") + "' AND '" + end.ToString("MM/dd/yyyy") + "' )";

            return this.getData(strCondition);
        }

        public double getNextID()
        {
            double douResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(APILOG_ID, 1) ");
                obj_str.Append(" FROM OPR_SYS_APILOG");
                obj_str.Append(" ORDER BY APILOG_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    douResult = Convert.ToDouble(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "LAPI002:" + ex.ToString();
            }

            return douResult;
        }
                   
        public string insert(cls_SYSApilog model)
        {
            string strResult = "";
            try
            {
                
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                          
                obj_str.Append("INSERT INTO OPR_SYS_APILOG");
                obj_str.Append(" (");
                obj_str.Append("APILOG_ID ");
                obj_str.Append(", APILOG_CODE ");
                obj_str.Append(", APILOG_DATA ");
                obj_str.Append(", APILOG_STATUS ");
                obj_str.Append(", APILOG_MESSAGE ");
                obj_str.Append(", APILOG_BY ");
                obj_str.Append(", APILOG_DATE ");                  
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@APILOG_ID ");
                obj_str.Append(", @APILOG_CODE ");
                obj_str.Append(", @APILOG_DATA ");
                obj_str.Append(", @APILOG_STATUS ");
                obj_str.Append(", @APILOG_MESSAGE ");
                obj_str.Append(", @APILOG_BY ");
                obj_str.Append(", @APILOG_DATE ");
                obj_str.Append(" )");
                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.apilog_id = this.getNextID();

                obj_cmd.Parameters.Add("@APILOG_ID", SqlDbType.BigInt); obj_cmd.Parameters["@APILOG_ID"].Value = model.apilog_id;
                obj_cmd.Parameters.Add("@APILOG_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@APILOG_CODE"].Value = model.apilog_code;
                obj_cmd.Parameters.Add("@APILOG_DATA", SqlDbType.VarChar); obj_cmd.Parameters["@APILOG_DATA"].Value = model.apilog_data;
                obj_cmd.Parameters.Add("@APILOG_STATUS", SqlDbType.VarChar); obj_cmd.Parameters["@APILOG_STATUS"].Value = model.apilog_status;
                obj_cmd.Parameters.Add("@APILOG_MESSAGE", SqlDbType.VarChar); obj_cmd.Parameters["@APILOG_MESSAGE"].Value = model.apilog_message;
                obj_cmd.Parameters.Add("@APILOG_BY", SqlDbType.VarChar); obj_cmd.Parameters["@APILOG_BY"].Value = model.apilog_by;
                obj_cmd.Parameters.Add("@APILOG_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@APILOG_DATE"].Value = DateTime.Now;
                                     
                obj_cmd.ExecuteNonQuery();
                                
                obj_conn.doClose();
                strResult = model.apilog_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "LAPI005:" + ex.ToString();
                strResult = "";
            }

            return strResult;
        }

        

    }
}
