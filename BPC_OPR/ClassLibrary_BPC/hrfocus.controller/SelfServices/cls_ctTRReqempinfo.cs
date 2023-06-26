using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRReqempinfo
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRReqempinfo() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRReqempinfo> getData(string condition)
        {
            List<cls_TRReqempinfo> list_model = new List<cls_TRReqempinfo>();
            cls_TRReqempinfo model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("REQDOC_ID");
                obj_str.Append(", REQEMPINFO_NO");
                obj_str.Append(", TOPIC_CODE");
                obj_str.Append(", REQEMPINFO_DETAIL");

                obj_str.Append(" FROM SELF_TR_REQEMPINFO");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY REQEMPINFO_NO");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRReqempinfo();

                    model.reqdoc_id = Convert.ToInt32(dr["REQDOC_ID"]);
                    model.reqdocempinfo_no = Convert.ToInt32(dr["REQEMPINFO_NO"]);
                    model.topic_code = dr["TOPIC_CODE"].ToString();
                    model.reqempinfo_detail = dr["REQEMPINFO_DETAIL"].ToString();

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Trreqempinfo.getData)" + ex.ToString();
            }

            return list_model;
        }
        public List<cls_TRReqempinfo> getDataByFillter(int reqdoc_id, int reqempinfo_no, int topic_code)
        {
            string strCondition = "";
            if (!reqdoc_id.Equals(0))
                strCondition += " AND REQDOC_ID='" + reqdoc_id + "'";

            if (!reqempinfo_no.Equals(0))
                strCondition += " AND REQEMPINFO_NO='" + reqempinfo_no + "'";

            if (!topic_code.Equals(0))
                strCondition += " AND TOPIC_CODE='" + topic_code + "'";

            return this.getData(strCondition);
        }
        public bool checkDataOld(int reqdoc_id,int reqempinfo_no)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT REQEMPINFO_NO");
                obj_str.Append(" FROM SELF_TR_REQEMPINFO");
                obj_str.Append(" WHERE REQDOC_ID ='" + reqdoc_id + "' ");
                obj_str.Append(" AND REQEMPINFO_NO='" + reqempinfo_no + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Trreqempinfo.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(int reqdoc_id,int reqempinfo_no)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SELF_TR_REQEMPINFO");
                if (!reqdoc_id.Equals(0))
                    obj_str.Append(" WHERE REQDOC_ID='" + reqdoc_id + "'");

                if (!reqempinfo_no.Equals(0))
                    obj_str.Append(" WHERE REQEMPINFO_NO='" + reqempinfo_no + "'");


                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Trreqempinfo.delete)" + ex.ToString();
            }

            return blnResult;
        }
        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(REQEMPINFO_NO) ");
                obj_str.Append(" FROM SELF_TR_REQEMPINFO");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Trreqempinfo.getNextID)" + ex.ToString();
            }

            return intResult;
        }
        public string insert(cls_TRReqempinfo model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.reqdoc_id,model.reqdocempinfo_no))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO SELF_TR_REQEMPINFO");
                obj_str.Append(" (");
                obj_str.Append("REQDOC_ID ");
                obj_str.Append(", REQEMPINFO_NO ");
                obj_str.Append(", TOPIC_CODE ");
                obj_str.Append(", REQEMPINFO_DETAIL ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@REQDOC_ID ");
                obj_str.Append(", @REQEMPINFO_NO ");
                obj_str.Append(", @TOPIC_CODE ");
                obj_str.Append(", @REQEMPINFO_DETAIL ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@REQDOC_ID", SqlDbType.Int); obj_cmd.Parameters["@REQDOC_ID"].Value = model.reqdoc_id;
                obj_cmd.Parameters.Add("@REQEMPINFO_NO", SqlDbType.Int); obj_cmd.Parameters["@REQEMPINFO_NO"].Value = id;
                obj_cmd.Parameters.Add("@TOPIC_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@TOPIC_CODE"].Value = model.topic_code;
                obj_cmd.Parameters.Add("@REQEMPINFO_DETAIL", SqlDbType.VarChar); obj_cmd.Parameters["@REQEMPINFO_DETAIL"].Value = model.reqempinfo_detail;
     
                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Trreqempinfo.insert)" + ex.ToString();
            }

            return blnResult;
        }
        public string update(cls_TRReqempinfo model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SELF_TR_REQEMPINFO SET ");
                obj_str.Append(" TOPIC_CODE=@TOPIC_CODE ");
                obj_str.Append(", REQEMPINFO_DETAIL=@REQEMPINFO_DETAIL ");
                obj_str.Append(" WHERE REQDOC_ID=@REQDOC_ID ");
                obj_str.Append(" AND REQEMPINFO_NO=@REQEMPINFO_NO ");
      

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());



                obj_cmd.Parameters.Add("@REQDOC_ID", SqlDbType.VarChar); obj_cmd.Parameters["@REQDOC_ID"].Value = model.reqdoc_id;
                obj_cmd.Parameters.Add("@REQEMPINFO_NO", SqlDbType.Int); obj_cmd.Parameters["@REQEMPINFO_NO"].Value = model.reqdocempinfo_no;
                obj_cmd.Parameters.Add("@TOPIC_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@TOPIC_CODE"].Value = model.topic_code;
                obj_cmd.Parameters.Add("@REQEMPINFO_DETAIL", SqlDbType.VarChar); obj_cmd.Parameters["@REQEMPINFO_DETAIL"].Value = model.reqempinfo_detail;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.reqdocempinfo_no.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Trreqempinfo.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
