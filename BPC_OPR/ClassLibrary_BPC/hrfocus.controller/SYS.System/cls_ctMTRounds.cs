using ClassLibrary_BPC.hrfocus.model;
using ClassLibrary_BPC.hrfocus.model.SYS.System;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTRounds
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTRounds() { }

        public string getMessage() { return this.Message.Replace("SYS_MT_ROUNDS", "").Replace("cls_ctMTRounds", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTRounds> getData(string condition)
        {
            List<cls_MTRounds> list_model = new List<cls_MTRounds>();
            cls_MTRounds model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("ROUNDS_ID");
                obj_str.Append(", ROUNDS_CODE");
                obj_str.Append(", ISNULL(ROUNDS_NAME_TH, '') AS ROUNDS_NAME_TH");
                obj_str.Append(", ISNULL(ROUNDS_NAME_EN, '') AS ROUNDS_NAME_EN");

                obj_str.Append(", ROUNDS_FROM");
                obj_str.Append(", ROUNDS_TO");
                obj_str.Append(", ROUNDS_RESULT");
                obj_str.Append(", ROUNDS_GROUP");


                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");
                obj_str.Append(", FLAG");
                obj_str.Append(" FROM SYS_MT_ROUNDS");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY ROUNDS_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTRounds();
                    model.rounds_id = Convert.ToInt32(dr["ROUNDS_ID"]);
                    model.rounds_code = dr["ROUNDS_CODE"].ToString();
                    model.rounds_from = dr["ROUNDS_FROM"].ToString();
                    model.rounds_to = dr["ROUNDS_TO"].ToString();
                    model.rounds_result = dr["ROUNDS_RESULT"].ToString();
                    model.rounds_name_th = dr["ROUNDS_NAME_TH"].ToString();
                    model.rounds_name_en = dr["ROUNDS_NAME_EN"].ToString();
                    model.rounds_group = dr["ROUNDS_GROUP"].ToString();

                    if (dr["MODIFIED_BY"].ToString().Equals(""))
                    {
                        model.modified_by = dr["CREATED_BY"].ToString();
                        model.modified_date = Convert.ToDateTime(dr["CREATED_DATE"]);
                    }
                    else
                    {
                        model.modified_by = dr["MODIFIED_BY"].ToString();
                        model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                    }
                    model.flag = Convert.ToBoolean(dr["FLAG"]);
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Rounds.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTRounds> getDataByFillter(string group, string id, string code)
        {
            string strCondition = "";

            if (!group.Equals(""))
                strCondition += " AND ROUNDS_GROUP='" + group + "'";

            if (!id.Equals(""))
                strCondition += " AND ROUNDS_ID='" + id + "'";

            if (!code.Equals(""))
                strCondition += " AND ROUNDS_CODE='" + code + "'";

            return this.getData(strCondition);




            //string strCondition = "";

            //strCondition += " AND ROUNDS_GROUP='" + group + "'";

            //if (!id.Equals(""))
            //    strCondition += " AND ROUNDS_ID='" + id + "'";

            //if (!code.Equals(""))
            //    strCondition += " AND ROUNDS_CODE='" + code + "'";
           

            //return this.getData(strCondition);
        }

        public bool checkDataOld(string group, string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ROUNDS_ID");
                obj_str.Append(" FROM SYS_MT_ROUNDS");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND ROUNDS_GROUP='" + group + "'");
                obj_str.Append(" AND ROUNDS_CODE='" + code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(ROUNDS.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(ROUNDS_ID) ");
                obj_str.Append(" FROM SYS_MT_ROUNDS");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(ROUNDS.getNextID)" + ex.ToString();
            }

            return intResult;
        }

        public bool delete(string id)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SYS_MT_ROUNDS");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND ROUNDS_ID='" + id + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(ROUNDS.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTRounds model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.rounds_group, model.rounds_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();

                obj_str.Append("INSERT INTO SYS_MT_ROUNDS");
                obj_str.Append(" (");
                obj_str.Append(" ROUNDS_ID ");
                obj_str.Append(", ROUNDS_CODE ");

                obj_str.Append(", ROUNDS_NAME_TH ");
                obj_str.Append(", ROUNDS_NAME_EN ");

                obj_str.Append(", ROUNDS_FROM ");
                obj_str.Append(", ROUNDS_TO ");
                obj_str.Append(", ROUNDS_RESULT ");

                obj_str.Append(", ROUNDS_GROUP ");
                
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append(" @ROUNDS_ID ");
                obj_str.Append(", @ROUNDS_CODE ");
                obj_str.Append(", @ROUNDS_NAME_TH ");
                obj_str.Append(", @ROUNDS_NAME_EN ");

                obj_str.Append(", @ROUNDS_FROM ");
                obj_str.Append(", @ROUNDS_TO ");
                obj_str.Append(", @ROUNDS_RESULT ");

                obj_str.Append(", @ROUNDS_GROUP ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@ROUNDS_ID", SqlDbType.Int); obj_cmd.Parameters["@ROUNDS_ID"].Value = this.getNextID();

                obj_cmd.Parameters.Add("@ROUNDS_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@ROUNDS_CODE"].Value = model.rounds_code;
                obj_cmd.Parameters.Add("@ROUNDS_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@ROUNDS_NAME_TH"].Value = model.rounds_name_th;
                obj_cmd.Parameters.Add("@ROUNDS_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@ROUNDS_NAME_EN"].Value = model.rounds_name_en;

                obj_cmd.Parameters.Add("@ROUNDS_FROM", SqlDbType.VarChar); obj_cmd.Parameters["@ROUNDS_FROM"].Value = model.rounds_from;
                obj_cmd.Parameters.Add("@ROUNDS_TO", SqlDbType.VarChar); obj_cmd.Parameters["@ROUNDS_TO"].Value = model.rounds_to;
                obj_cmd.Parameters.Add("@ROUNDS_RESULT", SqlDbType.VarChar); obj_cmd.Parameters["@ROUNDS_RESULT"].Value = model.rounds_result;

                obj_cmd.Parameters.Add("@ROUNDS_GROUP", SqlDbType.VarChar); obj_cmd.Parameters["@ROUNDS_GROUP"].Value = model.rounds_group;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = id.ToString();
            }
            catch (Exception ex)
            {
                blnResult = "ERROR::(ROUNDS.insert)" + ex.ToString();
                Message = "ERROR::(ROUNDS.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_MTRounds model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SYS_MT_ROUNDS SET ");

                obj_str.Append(" ROUNDS_NAME_TH=@ROUNDS_NAME_TH ");
                obj_str.Append(", ROUNDS_NAME_EN=@ROUNDS_NAME_EN ");

                obj_str.Append(", ROUNDS_FROM=@ROUNDS_FROM ");
                obj_str.Append(", ROUNDS_TO=@ROUNDS_TO ");
                obj_str.Append(", ROUNDS_RESULT=@ROUNDS_RESULT ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");
                obj_str.Append(" WHERE ROUNDS_ID=@ROUNDS_ID ");
                obj_str.Append(" AND ROUNDS_CODE=@ROUNDS_CODE ");
                obj_str.Append(" AND ROUNDS_GROUP=@ROUNDS_GROUP ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@ROUNDS_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@ROUNDS_NAME_TH"].Value = model.rounds_name_th;
                obj_cmd.Parameters.Add("@ROUNDS_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@ROUNDS_NAME_EN"].Value = model.rounds_name_en;
                obj_cmd.Parameters.Add("@ROUNDS_FROM", SqlDbType.VarChar); obj_cmd.Parameters["@ROUNDS_FROM"].Value = model.rounds_from;
                obj_cmd.Parameters.Add("@ROUNDS_TO", SqlDbType.VarChar); obj_cmd.Parameters["@ROUNDS_TO"].Value = model.rounds_to;
                obj_cmd.Parameters.Add("@ROUNDS_RESULT", SqlDbType.VarChar); obj_cmd.Parameters["@ROUNDS_RESULT"].Value = model.rounds_result;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.Parameters.Add("@ROUNDS_ID", SqlDbType.Int); obj_cmd.Parameters["@ROUNDS_ID"].Value = model.rounds_id;

                obj_cmd.Parameters.Add("@ROUNDS_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@ROUNDS_CODE"].Value = model.rounds_code;
                obj_cmd.Parameters.Add("@ROUNDS_GROUP", SqlDbType.VarChar); obj_cmd.Parameters["@ROUNDS_GROUP"].Value = model.rounds_group;


                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = model.rounds_id.ToString();
            }
            catch (Exception ex)
            {
                blnResult = "ERROR::(Year.update)" + ex.ToString();
                Message = "ERROR::(Year.update)" + ex.ToString();
            }

            return blnResult;
        }

    }
}