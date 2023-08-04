using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRDocatt
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRDocatt() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRDocatt> getData(string condition)
        {
            List<cls_TRDocatt> list_model = new List<cls_TRDocatt>();
            cls_TRDocatt model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("REQDOC_ID");
                obj_str.Append(", REQDOC_ATT_NO");
                obj_str.Append(", REQDOC_ATT_FILENAME");
                obj_str.Append(", REQDOC_ATT_FILETYPE");
                obj_str.Append(", REQDOC_ATT_PATH");
                obj_str.Append(", REQDOC_ATT_PATH");

                obj_str.Append(", CREATED_BY");
                obj_str.Append(", CREATED_DATE");

                obj_str.Append(" FROM REQ_TR_DOCATT");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY REQDOC_ATT_NO");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRDocatt();

                    model.reqdoc_id = Convert.ToInt32(dr["REQDOC_ID"]);
                    model.reqdoc_att_no = Convert.ToInt32(dr["REQDOC_ATT_NO"]);
                    model.reqdoc_att_file_name = dr["REQDOC_ATT_FILENAME"].ToString();
                    model.reqdoc_att_file_type = dr["REQDOC_ATT_FILETYPE"].ToString();
                    model.reqdoc_att_path = dr["REQDOC_ATT_PATH"].ToString();
                    model.created_by = dr["CREATED_BY"].ToString();
                    model.created_date = Convert.ToDateTime(dr["CREATED_DATE"]).ToString();

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Trreqdocatt.getData)" + ex.ToString();
            }

            return list_model;
        }
        public List<cls_TRDocatt> getDataByFillter(int reqdoc_id, int reqdoc_att_no, string filename, string filetype)
        {
            string strCondition = "";
            if (!reqdoc_id.Equals(0))
                strCondition += " AND REQDOC_ID='" + reqdoc_id + "'";

            if (!reqdoc_att_no.Equals(0))
                strCondition += " AND REQDOC_ATT_NO='" + reqdoc_att_no + "'";

            if (!filename.Equals(""))
                strCondition += " AND REQDOC_ATT_FILENAME='" + filename + "'";

            if (!filetype.Equals(""))
                strCondition += " AND TOPIC_TYPE='" + filetype + "'";

            return this.getData(strCondition);
        }
        public bool checkDataOld(int reqdoc_id,int reqdoc_att_no)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT REQDOC_ATT_NO");
                obj_str.Append(" FROM REQ_TR_DOCATT");
                obj_str.Append(" WHERE REQDOC_ID ='" + reqdoc_id + "' ");
                obj_str.Append(" AND REQDOC_ATT_NO='" + reqdoc_att_no + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Trreqdocatt.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(int reqdoc_id,int reqdoc_att_no)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM REQ_TR_DOCATT");
                if (!reqdoc_id.Equals(0))
                    obj_str.Append(" WHERE REQDOC_ID='" + reqdoc_id + "'");

                if (!reqdoc_att_no.Equals(0))
                    obj_str.Append(" WHERE REQDOC_ATT_NO='" + reqdoc_att_no + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Trreqdocatt.delete)" + ex.ToString();
            }

            return blnResult;
        }
        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(REQDOC_ATT_NO) ");
                obj_str.Append(" FROM REQ_TR_DOCATT");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Trreqdocatt.getNextID)" + ex.ToString();
            }

            return intResult;
        }
        public string insert(cls_TRDocatt model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.reqdoc_id,model.reqdoc_att_no))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO REQ_TR_DOCATT");
                obj_str.Append(" (");
                obj_str.Append("REQDOC_ID ");
                obj_str.Append(", REQDOC_ATT_NO ");
                obj_str.Append(", REQDOC_ATT_FILENAME ");
                obj_str.Append(", REQDOC_ATT_FILETYPE ");
                obj_str.Append(", REQDOC_ATT_PATH ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@REQDOC_ID ");
                obj_str.Append(", @REQDOC_ATT_NO ");
                obj_str.Append(", @REQDOC_ATT_FILENAME ");
                obj_str.Append(", @REQDOC_ATT_FILETYPE ");
                obj_str.Append(", @REQDOC_ATT_PATH ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@REQDOC_ID", SqlDbType.Int); obj_cmd.Parameters["@REQDOC_ID"].Value = model.reqdoc_id;
                obj_cmd.Parameters.Add("@REQDOC_ATT_NO", SqlDbType.Int); obj_cmd.Parameters["@REQDOC_ATT_NO"].Value = id;
                obj_cmd.Parameters.Add("@REQDOC_ATT_FILENAME", SqlDbType.VarChar); obj_cmd.Parameters["@REQDOC_ATT_FILENAME"].Value = model.reqdoc_att_file_name;
                obj_cmd.Parameters.Add("@REQDOC_ATT_FILETYPE", SqlDbType.VarChar); obj_cmd.Parameters["@REQDOC_ATT_FILETYPE"].Value = model.reqdoc_att_file_type;
                obj_cmd.Parameters.Add("@REQDOC_ATT_PATH", SqlDbType.VarChar); obj_cmd.Parameters["@REQDOC_ATT_PATH"].Value = model.reqdoc_att_path;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.created_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Trreqdocatt.insert)" + ex.ToString();
            }

            return blnResult;
        }
        public string update(cls_TRDocatt model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE REQ_TR_DOCATT SET ");
                obj_str.Append(" REQDOC_ATT_FILENAME=@REQDOC_ATT_FILENAME ");
                obj_str.Append(", REQDOC_ATT_FILETYPE=@REQDOC_ATT_FILETYPE ");
                obj_str.Append(", REQDOC_ATT_PATH=@REQDOC_ATT_PATH ");
                obj_str.Append(", CREATED_BY=@CREATED_BY ");
                obj_str.Append(", CREATED_DATE=@CREATED_DATE ");

                obj_str.Append(" WHERE REQDOC_ID=@REQDOC_ID ");
                obj_str.Append(" AND REQDOC_ATT_NO=@REQDOC_ATT_NO ");
      

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());



                obj_cmd.Parameters.Add("@REQDOC_ID", SqlDbType.Int); obj_cmd.Parameters["@REQDOC_ID"].Value = model.reqdoc_id;
                obj_cmd.Parameters.Add("@REQDOC_ATT_NO", SqlDbType.Int); obj_cmd.Parameters["@REQDOC_ATT_NO"].Value = model.reqdoc_att_no;
                obj_cmd.Parameters.Add("@REQDOC_ATT_FILENAME", SqlDbType.VarChar); obj_cmd.Parameters["@REQDOC_ATT_FILENAME"].Value = model.reqdoc_att_file_name;
                obj_cmd.Parameters.Add("@REQDOC_ATT_FILETYPE", SqlDbType.VarChar); obj_cmd.Parameters["@REQDOC_ATT_FILETYPE"].Value = model.reqdoc_att_file_type;
                obj_cmd.Parameters.Add("@REQDOC_ATT_PATH", SqlDbType.VarChar); obj_cmd.Parameters["@REQDOC_ATT_PATH"].Value = model.reqdoc_att_path;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.created_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.reqdoc_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Trreqdocatt.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
