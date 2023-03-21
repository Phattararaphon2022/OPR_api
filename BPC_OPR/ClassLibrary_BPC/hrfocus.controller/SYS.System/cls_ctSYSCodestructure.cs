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
    public class cls_ctSYSCodestructure
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctSYSCodestructure() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_SYSCodestructure> getData(string condition)
        {
            List<cls_SYSCodestructure> list_model = new List<cls_SYSCodestructure>();
            cls_SYSCodestructure model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("CODESTRUCTURE_CODE");
                obj_str.Append(", CODESTRUCTURE_NAME_TH");
                obj_str.Append(", CODESTRUCTURE_NAME_EN");
                obj_str.Append(" FROM SYS_MT_CODESTRUCTURE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY CODESTRUCTURE_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_SYSCodestructure();

                    model.codestructure_code = Convert.ToString(dr["CODESTRUCTURE_CODE"]);
                    model.codestructure_name_th = Convert.ToString(dr["CODESTRUCTURE_NAME_TH"]);
                    model.codestructure_name_en = Convert.ToString(dr["CODESTRUCTURE_NAME_EN"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(SYSCodestructure.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_SYSCodestructure> getData()
        {
            string strCondition = "";
            return this.getData(strCondition);
        }

        public bool checkDataOld(string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT CODESTRUCTURE_CODE");
                obj_str.Append(" FROM SYS_MT_CODESTRUCTURE");
                obj_str.Append(" WHERE CODESTRUCTURE_CODE='" + code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(SYSCodestructure.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string code)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SYS_MT_CODESTRUCTURE");
                obj_str.Append(" WHERE CODESTRUCTURE_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());
            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(SYSCodestructure.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_SYSCodestructure model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.codestructure_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_MT_CODESTRUCTURE");
                obj_str.Append(" (");
                obj_str.Append("CODESTRUCTURE_CODE ");
                obj_str.Append(", CODESTRUCTURE_NAME_TH ");
                obj_str.Append(", CODESTRUCTURE_NAME_EN ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@CODESTRUCTURE_CODE ");
                obj_str.Append(", @CODESTRUCTURE_NAME_TH ");
                obj_str.Append(", @CODESTRUCTURE_NAME_EN ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@CODESTRUCTURE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@CODESTRUCTURE_CODE"].Value = model.codestructure_code;
                obj_cmd.Parameters.Add("@CODESTRUCTURE_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@CODESTRUCTURE_NAME_TH"].Value = model.codestructure_name_th;
                obj_cmd.Parameters.Add("@CODESTRUCTURE_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@CODESTRUCTURE_NAME_EN"].Value = model.codestructure_name_en;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(SYSCodestructure.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public bool update(cls_SYSCodestructure model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SYS_MT_CODESTRUCTURE SET ");
                obj_str.Append(" CODESTRUCTURE_NAME_TH=@CODESTRUCTURE_NAME_TH ");
                obj_str.Append(", CODESTRUCTURE_NAME_EN=@CODESTRUCTURE_NAME_EN ");

                obj_str.Append(" WHERE CODESTRUCTURE_CODE=@CODESTRUCTURE_CODE ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@CODESTRUCTURE_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@CODESTRUCTURE_NAME_TH"].Value = model.codestructure_name_th;
                obj_cmd.Parameters.Add("@CODESTRUCTURE_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@CODESTRUCTURE_NAME_EN"].Value = model.codestructure_name_en;

                obj_cmd.Parameters.Add("@CODESTRUCTURE_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@CODESTRUCTURE_CODE"].Value = model.codestructure_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(SYSCodestructure.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
