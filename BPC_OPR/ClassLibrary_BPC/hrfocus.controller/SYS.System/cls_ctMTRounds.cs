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

        public string getMessage() { return this.Message.Replace("SYS_MT_ROUND", "").Replace("cls_ctMTRounds", "").Replace("line", ""); }

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

                obj_str.Append("ROUND_ID");
                obj_str.Append(", ROUND_CODE");
                obj_str.Append(", ISNULL(ROUND_NAME_TH, '') AS ROUND_NAME_TH");
                obj_str.Append(", ISNULL(ROUND_NAME_EN, '') AS ROUND_NAME_EN");

                obj_str.Append(", ROUND_GROUP");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM SYS_MT_ROUND");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY ROUND_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTRounds();

                    model.round_id = Convert.ToInt32(dr["ROUND_ID"]);
                    model.round_code = dr["ROUND_CODE"].ToString();
                    model.round_name_th = dr["ROUND_NAME_TH"].ToString();
                    model.round_name_en = dr["ROUND_NAME_EN"].ToString();

                    model.round_group = dr["ROUND_GROUP"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Round.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTRounds> getDataByFillter(string group, string id, string code)
        {
            string strCondition = "";

            if (!group.Equals(""))
                strCondition += " AND ROUND_GROUP='" + group + "'";

            if (!id.Equals(""))
                strCondition += " AND ROUND_ID='" + id + "'";

            if (!code.Equals(""))
                strCondition += " AND ROUND_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string group, string code, string id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ROUND_ID");
                obj_str.Append(" FROM SYS_MT_ROUND");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND ROUND_GROUP='" + group + "'");
                obj_str.Append(" AND ROUND_CODE='" + code + "'");
                obj_str.Append(" AND ROUND_ID='" + id + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(ROUND.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(ROUND_ID) ");
                obj_str.Append(" FROM SYS_MT_ROUND");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(ROUND.getNextID)" + ex.ToString();
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

                obj_str.Append(" DELETE FROM SYS_MT_ROUND");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND ROUND_ID='" + id + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(ROUND.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTRounds model)
        {
            string strResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.round_group, model.round_code,model.round_id.ToString()))
                {
                    bool blnResult = this.update(model);

                    if (blnResult)
                        return model.round_id.ToString();
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();

                obj_str.Append("INSERT INTO SYS_MT_ROUND");
                obj_str.Append(" (");
                obj_str.Append("ROUND_ID ");
                obj_str.Append(", ROUND_CODE ");
                obj_str.Append(", ROUND_NAME_TH ");
                obj_str.Append(", ROUND_NAME_EN ");
                obj_str.Append(", ROUND_GROUP ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append(" @ROUND_ID ");
                obj_str.Append(", @ROUND_CODE ");
                obj_str.Append(", @ROUND_NAME_TH ");
                obj_str.Append(", @ROUND_NAME_EN ");
                obj_str.Append(", @ROUND_GROUP ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

 
                obj_cmd.Parameters.Add("@ROUND_ID", SqlDbType.Int); obj_cmd.Parameters["@ROUND_ID"].Value = id;
                obj_cmd.Parameters.Add("@ROUND_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@ROUND_CODE"].Value = model.round_code;
                obj_cmd.Parameters.Add("@ROUND_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@ROUND_NAME_TH"].Value = model.round_name_th;
                obj_cmd.Parameters.Add("@ROUND_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@ROUND_NAME_EN"].Value = model.round_name_en;
                obj_cmd.Parameters.Add("@ROUND_GROUP", SqlDbType.VarChar); obj_cmd.Parameters["@ROUND_GROUP"].Value = model.round_group;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
            
            }
            catch (Exception ex)
            {
                strResult = "";
                Message = "ERROR::(ROUNDS.insert)" + ex.ToString();
            }

            return strResult;
        }

        public bool update(cls_MTRounds model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SYS_MT_ROUND SET ");

                obj_str.Append(" ROUND_CODE=@ROUND_CODE ");
                obj_str.Append(", ROUND_NAME_TH=@ROUND_NAME_TH ");
                obj_str.Append(", ROUND_NAME_EN=@ROUND_NAME_EN ");
                obj_str.Append(", ROUND_GROUP=@ROUND_GROUP ");
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");


                obj_str.Append(" WHERE ROUND_ID=@ROUND_ID ");

                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@ROUND_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@ROUND_CODE"].Value = model.round_code;
                obj_cmd.Parameters.Add("@ROUND_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@ROUND_NAME_TH"].Value = model.round_name_th;
                obj_cmd.Parameters.Add("@ROUND_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@ROUND_NAME_EN"].Value = model.round_name_en;
                obj_cmd.Parameters.Add("@ROUND_GROUP", SqlDbType.VarChar); obj_cmd.Parameters["@ROUND_GROUP"].Value = model.round_group;
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@ROUND_ID", SqlDbType.Int); obj_cmd.Parameters["@ROUND_ID"].Value = model.round_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Round.update)" + ex.ToString();
            }

            return blnResult;
      
        }

    }
}