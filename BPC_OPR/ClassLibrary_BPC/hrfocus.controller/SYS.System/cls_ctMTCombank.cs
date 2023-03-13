using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTCombank
    {
   
     string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTCombank() { }

        public string getMessage() { return this.Message.Replace("SYS_MT_COMBANkK", "").Replace("cls_ctMTCombank", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTCombank> getData(string condition)
        {
            List<cls_MTCombank> list_model = new List<cls_MTCombank>();
            cls_MTCombank model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", COMBANK_ID");
                obj_str.Append(", ISNULL(COMBANK_BANKCODE, '') AS COMBANK_BANKCODE");
                obj_str.Append(", ISNULL(COMBANK_BANKACCOUNT, '') AS COMBANK_BANKACCOUNT");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM SYS_MT_COMBANkK");
                obj_str.Append(" WHERE 1=1");
                
                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTCombank();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.combank_id = Convert.ToInt32(dr["COMBANK_ID"]);
                    model.combank_bankcode = dr["COMBANK_BANKCODE"].ToString();
                    model.combank_bankaccount = dr["COMBANK_BANKACCOUNT"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "CBK001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTCombank> getDataByFillter(string com)
        {
            string strCondition = "";
            strCondition += " AND COMPANY_CODE='" + com + "'";
            //strCondition += " AND COMBANK_BANKCODE IN (" + bankcode + ") ";

            return this.getData(strCondition);
        }
        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

            

                obj_str.Append("SELECT MAX(COMBANK_ID) ");
                obj_str.Append(" FROM SYS_MT_COMBANKk");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "CBK002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string bankcode)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();



                obj_str.Append("SELECT COMBANK_BANKCODE");
                obj_str.Append(" FROM SYS_MT_COMBANkK");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND COMBANK_BANKCODE='" + bankcode + "'");
      
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "CBK003:" + ex.ToString();
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

                obj_str.Append(" DELETE FROM SYS_MT_COMBANkK");
                obj_str.Append(" WHERE COMBANK_ID='" + id + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "CBK004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_MTCombank model)
        {
            bool blnResult = false;
            string strResult = "";
            try
            {

                //-- Check data old
               

                if (this.checkDataOld(model.company_code, model.combank_bankcode))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_MT_COMBANkK");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", COMBANK_ID ");
                obj_str.Append(", COMBANK_BANKCODE ");
                obj_str.Append(", COMBANK_BANKACCOUNT ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @COMBANK_ID ");
                obj_str.Append(", @COMBANK_BANKCODE ");
                obj_str.Append(", @COMBANK_BANKACCOUNT ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");
                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.combank_id = this.getNextID();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

                obj_cmd.Parameters.Add("@COMBANK_ID", SqlDbType.Int); obj_cmd.Parameters["@COMBANK_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@COMBANK_BANKCODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMBANK_BANKCODE"].Value = model.combank_bankcode;
                obj_cmd.Parameters.Add("@COMBANK_BANKACCOUNT", SqlDbType.VarChar); obj_cmd.Parameters["@COMBANK_BANKACCOUNT"].Value = model.combank_bankaccount;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.ExecuteNonQuery();
                                
                obj_conn.doClose();
                strResult = model.combank_id.ToString();
                blnResult = true;

            }
            catch (Exception ex)
            {
                Message = "CBK005:" + ex.ToString();
                strResult = "";
                blnResult = false;
            }

            return blnResult;
        }

        public bool update(cls_MTCombank model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE SYS_MT_COMBANkK SET ");
                obj_str.Append(" COMBANK_BANKCODE=@COMBANK_BANKCODE ");
                obj_str.Append(", COMBANK_BANKACCOUNT=@COMBANK_BANKACCOUNT ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");

                obj_str.Append(" WHERE COMBANK_ID=@COMBANK_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMBANK_BANKCODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMBANK_BANKCODE"].Value = model.combank_bankcode;
                obj_cmd.Parameters.Add("@COMBANK_BANKACCOUNT", SqlDbType.VarChar); obj_cmd.Parameters["@COMBANK_BANKACCOUNT"].Value = model.combank_bankaccount;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@COMBANK_ID", SqlDbType.Int); obj_cmd.Parameters["@COMBANK_ID"].Value = model.combank_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "CBK006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}

