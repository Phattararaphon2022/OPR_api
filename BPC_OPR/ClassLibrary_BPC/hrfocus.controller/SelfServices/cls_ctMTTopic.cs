using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTTopic
    {
                   string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTTopic() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTTopic> getData(string condition)
        {
            List<cls_MTTopic> list_model = new List<cls_MTTopic>();
            cls_MTTopic model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", TOPIC_ID");
                obj_str.Append(", TOPIC_CODE");
                obj_str.Append(", TOPIC_NAME_TH");
                obj_str.Append(", TOPIC_NAME_EN");
                obj_str.Append(", TOPIC_TYPE");
           
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");
                obj_str.Append(", ISNULL(FLAG, 0) AS FLAG");

                obj_str.Append(" FROM SELF_MT_TOPIC");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY TOPIC_ID");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTTopic();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.topic_id = Convert.ToInt32(dr["TOPIC_ID"]);
                    model.topic_code = dr["TOPIC_CODE"].ToString();
                    model.topic_name_th = dr["TOPIC_NAME_TH"].ToString();
                    model.topic_name_en = dr["TOPIC_NAME_EN"].ToString();
                    model.topic_type = dr["TOPIC_TYPE"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                    model.flag = Convert.ToBoolean(dr["FLAG"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Mttopic.getData)" + ex.ToString();
            }

            return list_model;
        }
        public List<cls_MTTopic> getDataByFillter(string com, int id, string code, string type)
        {
            string strCondition = "";
            if(!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!id.Equals(0))
                strCondition += " AND TOPIC_ID='" + id + "'";

            if (!code.Equals(""))
                strCondition += " AND TOPIC_CODE='" + code + "'";

            if (!type.Equals(""))
                strCondition += " AND TOPIC_TYPE='" + type + "'";

            return this.getData(strCondition);
        }
        public bool checkDataOld(string com,int id,string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT TOPIC_ID");
                obj_str.Append(" FROM SELF_MT_TOPIC");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                if (!id.Equals(0))
                    obj_str.Append(" AND TOPIC_ID='" + id + "'");
                if (!code.Equals(""))
                    obj_str.Append(" AND TOPIC_CODE='" + code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Mttopic.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(string com,int id,string code)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SELF_MT_TOPIC");
                obj_str.Append(" WHERE 1=1 ");
                if (!com.Equals(""))
                    obj_str.Append(" AND COMPANY_CODE='" + com + "'");

                if (!id.Equals(0))
                    obj_str.Append(" AND TOPIC_ID='" + id + "'");

                if (!code.Equals(""))
                    obj_str.Append(" AND TOPIC_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Mttopic.delete)" + ex.ToString();
            }

            return blnResult;
        }
        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(TOPIC_ID) ");
                obj_str.Append(" FROM SELF_MT_TOPIC");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Mttopic.getNextID)" + ex.ToString();
            }

            return intResult;
        }
        public string insert(cls_MTTopic model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code,model.topic_id,model.topic_code))
                {
                    if (model.topic_id.Equals(0))
                    {
                        return "D";
                    }
                    else
                    {
                        return this.update(model);
                    }
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO SELF_MT_TOPIC");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", TOPIC_ID ");
                obj_str.Append(", TOPIC_CODE ");
                obj_str.Append(", TOPIC_NAME_TH ");
                obj_str.Append(", TOPIC_NAME_EN ");
                obj_str.Append(", TOPIC_TYPE ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @TOPIC_ID ");
                obj_str.Append(", @TOPIC_CODE ");
                obj_str.Append(", @TOPIC_NAME_TH ");
                obj_str.Append(", @TOPIC_NAME_EN ");
                obj_str.Append(", @TOPIC_TYPE ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@TOPIC_ID", SqlDbType.Int); obj_cmd.Parameters["@TOPIC_ID"].Value = id;
                obj_cmd.Parameters.Add("@TOPIC_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@TOPIC_CODE"].Value = model.topic_code;
                obj_cmd.Parameters.Add("@TOPIC_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@TOPIC_NAME_TH"].Value = model.topic_name_th;
                obj_cmd.Parameters.Add("@TOPIC_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@TOPIC_NAME_EN"].Value = model.topic_name_en;
                obj_cmd.Parameters.Add("@TOPIC_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@TOPIC_TYPE"].Value = model.topic_type;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Mttopic.insert)" + ex.ToString();
            }

            return blnResult;
        }
        public string update(cls_MTTopic model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SELF_MT_TOPIC SET ");
                obj_str.Append(" COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(", TOPIC_NAME_TH=@TOPIC_NAME_TH ");
                obj_str.Append(", TOPIC_NAME_EN=@TOPIC_NAME_EN ");
                obj_str.Append(", TOPIC_TYPE=@TOPIC_TYPE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");
                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND TOPIC_CODE=@TOPIC_CODE ");
      

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());



                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@TOPIC_ID", SqlDbType.Int); obj_cmd.Parameters["@TOPIC_ID"].Value = model.topic_id;
                obj_cmd.Parameters.Add("@TOPIC_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@TOPIC_CODE"].Value = model.topic_code;
                obj_cmd.Parameters.Add("@TOPIC_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@TOPIC_NAME_TH"].Value = model.topic_name_th;
                obj_cmd.Parameters.Add("@TOPIC_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@TOPIC_NAME_EN"].Value = model.topic_name_en;
                obj_cmd.Parameters.Add("@TOPIC_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@TOPIC_TYPE"].Value = model.topic_type;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;
                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.topic_code.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Mttopic.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
