using ClassLibrary_BPC.hrfocus.model;
using ClassLibrary_BPC.hrfocus.model.System;
//using ClassLibrary_BPC.hrfocus.model.System;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTCodestructure
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTCodestructure() { }

        public string getMessage() { return this.Message.Replace("SYS_MT_CODESTRUCTURE", "").Replace("cls_ctMTCodestructure", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTCodestructure> getData(string condition)
        {
            List<cls_MTCodestructure> list_model = new List<cls_MTCodestructure>();
            cls_MTCodestructure model;
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
                    model = new cls_MTCodestructure();


                    model.codestructure_code = Convert.ToString(dr["CODESTRUCTURE_CODE"]);
                    model.codestructure_name_th = Convert.ToString(dr["CODESTRUCTURE_NAME_TH"]);
                    model.codestructure_name_en = Convert.ToString(dr["CODESTRUCTURE_NAME_EN"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "CID001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTCodestructure> getData()
        {
            string strCondition = "";
            return this.getData(strCondition);
        }

        public List<cls_MTCodestructure> getDataByFillter(string code)
        {
            string strCondition = "";

            if (!code.Equals(""))
                strCondition += " AND ADDRESSTYPE_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(ADDRESSTYPE_ID, 1) ");
                obj_str.Append(" FROM SYS_MT_ADDRESSTYPE");
                obj_str.Append(" ORDER BY ADDRESSTYPE_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "CID002:" + ex.ToString();
            }

            return intResult;
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
                Message = "CID003:" + ex.ToString();
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
                Message = "CID004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_MTCodestructure model)
        {
            bool blnResult = false;
            string strResult = "";
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
                strResult = model.codestructure_code.ToString();
            }
            catch (Exception ex)
            {
                Message = "CID005:" + ex.ToString();
                strResult = "";
            }

            return blnResult;

        }

        public bool update(cls_MTCodestructure model)
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
                Message = "CID006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}
