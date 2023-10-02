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
    public class cls_ctMTProject
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTProject() { }

        public string getMessage() { return this.Message.Replace("PRO_MT_PROJECT", "").Replace("cls_ctMTProject", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTProject> getData(string condition)
        {
            List<cls_MTProject> list_model = new List<cls_MTProject>();
            cls_MTProject model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PROJECT_ID");
                obj_str.Append(", PROJECT_CODE");
                obj_str.Append(", PROJECT_NAME_TH");
                obj_str.Append(", PROJECT_NAME_EN");

                obj_str.Append(", ISNULL(PROJECT_NAME_SUB, '') AS PROJECT_NAME_SUB");
                obj_str.Append(", ISNULL(PROJECT_CODECENTRAL, '') AS PROJECT_CODECENTRAL");
                obj_str.Append(", ISNULL(PROJECT_PROTYPE, '') AS PROJECT_PROTYPE");

                obj_str.Append(", ISNULL(PROJECT_PROAREA, '') AS PROJECT_PROAREA");
                obj_str.Append(", ISNULL(PROJECT_PROGROUP, '') AS PROJECT_PROGROUP");
                obj_str.Append(", ISNULL(PROJECT_PROBUSINESS, '') AS PROJECT_PROBUSINESS");

                //
                obj_str.Append(", ISNULL(PROJECT_ROUNDTIME, '') AS PROJECT_ROUNDTIME");
                obj_str.Append(", ISNULL(PROJECT_ROUNDMONEY, '') AS PROJECT_ROUNDMONEY");
                obj_str.Append(", ISNULL(PROJECT_PROHOLIDAY, '') AS PROJECT_PROHOLIDAY");
                //

                obj_str.Append(", ISNULL(PROJECT_STATUS, '') AS PROJECT_STATUS");
                obj_str.Append(", ISNULL(COMPANY_CODE, '') AS COMPANY_CODE");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PRO_MT_PROJECT");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PROJECT_CODE, COMPANY_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTProject();

                    model.project_id = Convert.ToInt32(dr["PROJECT_ID"]);
                    model.project_code = dr["PROJECT_CODE"].ToString();
                    model.project_name_th = dr["PROJECT_NAME_TH"].ToString();
                    model.project_name_en = dr["PROJECT_NAME_EN"].ToString();

                    model.project_name_sub = dr["PROJECT_NAME_SUB"].ToString();
                    model.project_codecentral = dr["PROJECT_CODECENTRAL"].ToString();
                    model.project_protype = dr["PROJECT_PROTYPE"].ToString();

                    model.project_proarea = dr["PROJECT_PROAREA"].ToString();
                    model.project_progroup = dr["PROJECT_PROGROUP"].ToString();

                    model.project_probusiness = dr["PROJECT_PROBUSINESS"].ToString();

                    //
                    model.project_roundtime = dr["PROJECT_ROUNDTIME"].ToString();
                    model.project_roundmoney = dr["PROJECT_ROUNDMONEY"].ToString();
                    model.project_proholiday = dr["PROJECT_PROHOLIDAY"].ToString();
                    //

                    model.project_status = dr["PROJECT_STATUS"].ToString();
                    model.company_code = dr["COMPANY_CODE"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "BNK001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTProject> getDataByFillter(string com, string code, string codecentral, string type, string business, string area, string group)
        {
            string strCondition = "";
            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!code.Equals(""))
                strCondition += " AND PROJECT_CODE='" + code + "'";

            if (!codecentral.Equals(""))
                strCondition += " AND PROJECT_CODECENTRAL='" + codecentral + "'";

            if (!type.Equals(""))
                strCondition += " AND PROJECT_PROTYPE='" + type + "'";

            if (!business.Equals(""))
                strCondition += " AND PROJECT_PROBUSINESS='" + business + "'";

            ////
            if (!area.Equals(""))
                strCondition += " AND PROJECT_PROAREA='" + area + "'";
            if (!group.Equals(""))
                strCondition += " AND PROJECT_PROGROUP='" + group + "'";
            ///

            strCondition += " AND PROJECT_STATUS='F'";

            return this.getData(strCondition);
        }

        public List<cls_MTProject> getDataByFillter(string com, string code, string codecentral, string type, string business, string area, string group, string status)
        {
            string strCondition = "";

            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!code.Equals(""))
                strCondition += " AND PROJECT_CODE='" + code + "'";

            if (!codecentral.Equals(""))
                strCondition += " AND PROJECT_CODECENTRAL='" + codecentral + "'";

            if (!type.Equals(""))
                strCondition += " AND PROJECT_PROTYPE='" + type + "'";

            if (!business.Equals(""))
                strCondition += " AND PROJECT_PROBUSINESS='" + business + "'";

            ////
            if (!area.Equals(""))
                strCondition += " AND PROJECT_PROAREA='" + area + "'";
            if (!group.Equals(""))
                strCondition += " AND PROJECT_PROGROUP='" + group + "'";
            ///

            if (!status.Equals(""))
            {
                strCondition += " AND PROJECT_STATUS='" + status + "'";
            }

            return this.getData(strCondition);
        }






        public List<cls_MTProject> getDataCurrents(string code, DateTime fromdate, DateTime todate)
        {
            string strCondition = "";

            if (!code.Equals(""))
                strCondition += " AND PROJECT_CODE='" + code + "'";

            //strCondition += "AND ('" + fromdate.ToString("MM/dd/yyyy") + "' BETWEEN PROCONTRACT_FROMDATE AND PROCONTRACT_TODATE) or (('" + todate.ToString("MM/dd/yyyy") + "' BETWEEN PROCONTRACT_FROMDATE AND PROCONTRACT_TODATE)))";
            strCondition += "AND PROJECT_CODE IN ((SELECT PROJECT_CODE FROM PRO_TR_PROCONTRACT WHERE ('" + fromdate.ToString("MM/dd/yyyy") + "'BETWEEN PROCONTRACT_FROMDATE AND PROCONTRACT_TODATE) OR ('" + todate.ToString("MM/dd/yyyy") + "'  BETWEEN PROCONTRACT_FROMDATE AND PROCONTRACT_TODATE)))";


            return this.getData(strCondition);
        }
        //


        //
        public List<cls_MTProject> getDataByFillterAll(string com, string code, string business, string area, string status )
        {
            string strCondition = "";

            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!code.Equals(""))
                strCondition += " AND PROJECT_CODE='" + code + "'";
            if (!business.Equals(""))
                strCondition += " AND PROJECT_PROBUSINESS='" + business + "'";
            if (!area.Equals(""))
                strCondition += " AND PROJECT_PROAREA='" + area + "'";
            if (!status.Equals(""))
            {
                strCondition += " AND PROJECT_STATUS='" + status + "'";
            }




            return this.getData(strCondition);
        }
        //



        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(PROJECT_ID, 1) ");
                obj_str.Append(" FROM PRO_MT_PROJECT");
                obj_str.Append(" ORDER BY PROJECT_ID DESC ");

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

        public bool checkDataOld(string code, string com, string id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PROJECT_ID");
                obj_str.Append(" FROM PRO_MT_PROJECT");
                obj_str.Append(" WHERE PROJECT_CODE='" + code + "'");
                obj_str.Append(" AND COMPANY_CODE ='" + com + "'");
                obj_str.Append(" AND PROJECT_ID ='" + id + "'");

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

        public bool delete(string code, string com)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_MT_PROJECT");
                obj_str.Append(" WHERE PROJECT_CODE='" + code + "'");
                obj_str.Append(" AND COMPANY_CODE ='" + com + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BNK004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTProject model)
        {

            string strResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.project_code, model.company_code, model.project_id.ToString()))
                {
                    if (this.update(model))
                        return model.project_id.ToString();
                    else
                        return "";                    
                }


            //bool blnResult = false;
            //try
            //{

            //    //-- Check data old
            //    if (this.checkDataOld(model.project_code, model.company_code, model.project_id.ToString()))
            //    {
            //        if (this.update(model))
            //            return this.update(model);
            //        else
            //            return false;
            //    }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();

                obj_str.Append("INSERT INTO PRO_MT_PROJECT");
                obj_str.Append(" (");
                obj_str.Append("PROJECT_ID ");
                obj_str.Append(", PROJECT_CODE ");
                obj_str.Append(", PROJECT_NAME_TH ");
                obj_str.Append(", PROJECT_NAME_EN ");

                obj_str.Append(", PROJECT_NAME_SUB ");
                obj_str.Append(", PROJECT_CODECENTRAL ");
                obj_str.Append(", PROJECT_PROTYPE ");

                obj_str.Append(", PROJECT_PROAREA ");
                obj_str.Append(", PROJECT_PROGROUP ");

                obj_str.Append(", PROJECT_PROBUSINESS ");
                obj_str.Append(", PROJECT_ROUNDTIME ");
                obj_str.Append(", PROJECT_ROUNDMONEY ");
                obj_str.Append(", PROJECT_PROHOLIDAY ");
                obj_str.Append(", PROJECT_STATUS ");
                obj_str.Append(", COMPANY_CODE ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@PROJECT_ID ");
                obj_str.Append(", @PROJECT_CODE ");
                obj_str.Append(", @PROJECT_NAME_TH ");
                obj_str.Append(", @PROJECT_NAME_EN ");

                obj_str.Append(", @PROJECT_NAME_SUB ");
                obj_str.Append(", @PROJECT_CODECENTRAL ");
                obj_str.Append(", @PROJECT_PROTYPE ");

                obj_str.Append(", @PROJECT_PROAREA ");
                obj_str.Append(", @PROJECT_PROGROUP ");


                obj_str.Append(", @PROJECT_PROBUSINESS ");
                obj_str.Append(", @PROJECT_ROUNDTIME ");
                obj_str.Append(", @PROJECT_ROUNDMONEY ");
                obj_str.Append(", @PROJECT_PROHOLIDAY ");

                obj_str.Append(", @PROJECT_STATUS ");
                obj_str.Append(", @COMPANY_CODE ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", '1' ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROJECT_ID", SqlDbType.Int); obj_cmd.Parameters["@PROJECT_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
                obj_cmd.Parameters.Add("@PROJECT_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_NAME_TH"].Value = model.project_name_th;
                obj_cmd.Parameters.Add("@PROJECT_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_NAME_EN"].Value = model.project_name_en;

                obj_cmd.Parameters.Add("@PROJECT_NAME_SUB", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_NAME_SUB"].Value = model.project_name_sub;
                obj_cmd.Parameters.Add("@PROJECT_CODECENTRAL", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODECENTRAL"].Value = model.project_codecentral;
                obj_cmd.Parameters.Add("@PROJECT_PROTYPE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_PROTYPE"].Value = model.project_protype;

                obj_cmd.Parameters.Add("@PROJECT_PROAREA", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_PROAREA"].Value = model.project_proarea;
                obj_cmd.Parameters.Add("@PROJECT_PROGROUP", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_PROGROUP"].Value = model.project_progroup;

                
                obj_cmd.Parameters.Add("@PROJECT_PROBUSINESS", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_PROBUSINESS"].Value = model.project_probusiness;
                obj_cmd.Parameters.Add("@PROJECT_ROUNDTIME", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_ROUNDTIME"].Value = model.project_roundtime;
                obj_cmd.Parameters.Add("@PROJECT_ROUNDMONEY", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_ROUNDMONEY"].Value = model.project_roundmoney;
                obj_cmd.Parameters.Add("@PROJECT_PROHOLIDAY", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_PROHOLIDAY"].Value = model.project_proholiday;

                obj_cmd.Parameters.Add("@PROJECT_STATUS", SqlDbType.Char); obj_cmd.Parameters["@PROJECT_STATUS"].Value = "W";
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
 
                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                strResult = model.project_id.ToString();
                //blnResult = true;

            }
            catch (Exception ex)
            {

                Message = "BNK005:" + ex.ToString();
                strResult = "";

            }

            return strResult;
        }

        public bool update(cls_MTProject model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PRO_MT_PROJECT SET ");
                obj_str.Append(" PROJECT_NAME_TH=@PROJECT_NAME_TH ");
                obj_str.Append(", PROJECT_NAME_EN=@PROJECT_NAME_EN ");

                obj_str.Append(", PROJECT_NAME_SUB=@PROJECT_NAME_SUB ");
                obj_str.Append(", PROJECT_CODECENTRAL=@PROJECT_CODECENTRAL ");
                obj_str.Append(", PROJECT_PROTYPE=@PROJECT_PROTYPE ");

                obj_str.Append(", PROJECT_PROAREA=@PROJECT_PROAREA ");
                obj_str.Append(", PROJECT_PROGROUP=@PROJECT_PROGROUP ");


                obj_str.Append(", PROJECT_PROBUSINESS=@PROJECT_PROBUSINESS ");
                obj_str.Append(", PROJECT_ROUNDTIME=@PROJECT_ROUNDTIME ");
                obj_str.Append(", PROJECT_ROUNDMONEY=@PROJECT_ROUNDMONEY ");
                obj_str.Append(", PROJECT_PROHOLIDAY=@PROJECT_PROHOLIDAY ");
                
                obj_str.Append(", PROJECT_STATUS=@PROJECT_STATUS ");
               
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");

                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND PROJECT_CODE=@PROJECT_CODE ");
                obj_str.Append(" AND PROJECT_ID=@PROJECT_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROJECT_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_NAME_TH"].Value = model.project_name_th;
                obj_cmd.Parameters.Add("@PROJECT_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_NAME_EN"].Value = model.project_name_en;

                obj_cmd.Parameters.Add("@PROJECT_NAME_SUB", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_NAME_SUB"].Value = model.project_name_sub;
                obj_cmd.Parameters.Add("@PROJECT_CODECENTRAL", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODECENTRAL"].Value = model.project_codecentral;
                obj_cmd.Parameters.Add("@PROJECT_PROTYPE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_PROTYPE"].Value = model.project_protype;

                obj_cmd.Parameters.Add("@PROJECT_PROAREA", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_PROAREA"].Value = model.project_proarea;
                obj_cmd.Parameters.Add("@PROJECT_PROGROUP", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_PROGROUP"].Value = model.project_progroup;

                
                
                obj_cmd.Parameters.Add("@PROJECT_PROBUSINESS", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_PROBUSINESS"].Value = model.project_probusiness;
                obj_cmd.Parameters.Add("@PROJECT_ROUNDTIME", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_ROUNDTIME"].Value = model.project_roundtime;
                obj_cmd.Parameters.Add("@PROJECT_ROUNDMONEY", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_ROUNDMONEY"].Value = model.project_roundmoney;
                obj_cmd.Parameters.Add("@PROJECT_PROHOLIDAY", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_PROHOLIDAY"].Value = model.project_proholiday;

                obj_cmd.Parameters.Add("@PROJECT_STATUS", SqlDbType.Char); obj_cmd.Parameters["@PROJECT_STATUS"].Value = model.project_status;
                               
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
                obj_cmd.Parameters.Add("@PROJECT_ID", SqlDbType.Int); obj_cmd.Parameters["@PROJECT_ID"].Value = model.project_id;

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

        public bool update_status(cls_MTProject model, string status)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PRO_MT_PROJECT SET ");
              
                obj_str.Append(" PROJECT_STATUS=@PROJECT_STATUS ");

                obj_str.Append(" WHERE PROJECT_CODE=@PROJECT_CODE ");
                obj_str.Append(" AND COMPANY_CODE=@COMPANY_CODE ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROJECT_STATUS", SqlDbType.Char); obj_cmd.Parameters["@PROJECT_STATUS"].Value = status;

                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "BNK008:" + ex.ToString();
            }

            return blnResult;
        }

    }
}
