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
    public class cls_ctTRPolcode
   {
        public string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRPolcode() { }

        public string getMessage() { return this.Message.Replace("SYS_TR_POLCODE", "").Replace("cls_ctTRPolcode", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRPolcode> getData(string condition)
        {
            List<cls_TRPolcode> list_model = new List<cls_TRPolcode>();
            cls_TRPolcode model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();


                obj_str.Append("SELECT ");

                obj_str.Append("POLCODE_ID");
                obj_str.Append(", CODESTRUCTURE_CODE");
                obj_str.Append(", POLCODE_LENGHT");
                obj_str.Append(", ISNULL(POLCODE_TEXT, '') AS POLCODE_TEXT");
                obj_str.Append(", POLCODE_ORDER");

                obj_str.Append(" FROM SYS_TR_POLCODE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY POLCODE_ID, POLCODE_ORDER");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
               {
                    model = new cls_TRPolcode();
                                        
                    model.polcode_id = Convert.ToInt32(dr["POLCODE_ID"]);
                    model.codestructure_code = Convert.ToString(dr["CODESTRUCTURE_CODE"]);
                    model.polcode_lenght = Convert.ToInt32(dr["POLCODE_LENGHT"]);
                    model.polcode_text = Convert.ToString(dr["POLCODE_TEXT"]);
                    model.polcode_order = Convert.ToInt32(dr["POLCODE_ORDER"]);

                    list_model.Add(model);
                }

            

            }
            catch(Exception ex)
            {
                Message = "PID001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRPolcode> getDataByFillter(string id)
        {
            string strCondition = "";

            if (!id.Equals(""))
                strCondition += " AND POLCODE_ID='" + id + "'";

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(POLCODE_ID) ");
                obj_str.Append(" FROM SYS_TR_POLCODE");                


                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "PID002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string polid, string struccode)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT POLCODE_ID");
                obj_str.Append(" FROM SYS_TR_POLCODE");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND POLCODE_ID='" + polid + "'");
                obj_str.Append(" AND CODESTRUCTURE_CODE='" + struccode + "'");
      
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "PID003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string polid, string struccode)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SYS_TR_POLCODE");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND POLCODE_ID='" + polid + "'");
                obj_str.Append(" AND CODESTRUCTURE_CODE='" + struccode + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "PID004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(List<cls_TRPolcode> list_model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_TR_POLCODE");
                obj_str.Append(" (");
                obj_str.Append("POLCODE_ID ");
                obj_str.Append(", CODESTRUCTURE_CODE ");
                obj_str.Append(", POLCODE_LENGHT ");
                obj_str.Append(", POLCODE_TEXT ");
                obj_str.Append(", POLCODE_ORDER ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@POLCODE_ID ");
                obj_str.Append(", @CODESTRUCTURE_CODE ");
                obj_str.Append(", @POLCODE_LENGHT ");
                obj_str.Append(", @POLCODE_TEXT ");
                obj_str.Append(", @POLCODE_ORDER ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM SYS_TR_POLCODE");
                obj_str2.Append(" WHERE POLCODE_ID='" + list_model[0].polcode_id + "'");
                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());

                if (blnResult)
                {
                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@POLCODE_ID", SqlDbType.Int);
                    obj_cmd.Parameters.Add("@CODESTRUCTURE_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@POLCODE_LENGHT", SqlDbType.Int);
                    obj_cmd.Parameters.Add("@POLCODE_TEXT", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@POLCODE_ORDER", SqlDbType.Int);

                    foreach (cls_TRPolcode model in list_model)
                    {

                        obj_cmd.Parameters["@POLCODE_ID"].Value = model.polcode_id;
                        obj_cmd.Parameters["@CODESTRUCTURE_CODE"].Value = model.codestructure_code;
                        obj_cmd.Parameters["@POLCODE_LENGHT"].Value = model.polcode_lenght;
                        obj_cmd.Parameters["@POLCODE_TEXT"].Value = model.polcode_text;
                        obj_cmd.Parameters["@POLCODE_ORDER"].Value = model.polcode_order;

                        obj_cmd.ExecuteNonQuery();
                    }

                    blnResult = obj_conn.doCommit();

                    if (!blnResult)
                        obj_conn.doRollback();

                }
                else
                {
                    obj_conn.doRollback();
                }

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "PID005:" + ex.ToString();
                //strResult = "";
            }

            return blnResult;
        }

       //public bool update(cls_MTPolcode model)
        //{
        //    bool blnResult = false;
        //    try
        //    {
        //        cls_ctConnection obj_conn = new cls_ctConnection();
        //        System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
        //        obj_str.Append("UPDATE SYS_MT_POLCODE SET ");

        //        obj_str.Append(" POLCODE_TYPE=@POLCODE_TYPE ");

        //        obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
        //        obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
        //        obj_str.Append(", FLAG=@FLAG ");


        //        obj_str.Append(" WHERE POLCODE_ID=@POLCODE_ID ");


        //        obj_conn.doConnect();

        //        SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

        //        obj_cmd.Parameters.Add("@POLCODE_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@POLCODE_TYPE"].Value = model.polcode_type;

        //        obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
        //        obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
        //        obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

        //        obj_cmd.Parameters.Add("@POLCODE_ID", SqlDbType.Int); obj_cmd.Parameters["@POLCODE_ID"].Value = model.polcode_id;

        //        obj_cmd.ExecuteNonQuery();

        //        obj_conn.doClose();

        //        blnResult = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Message = "EID006:" + ex.ToString();
        //    }
            
        //    return blnResult;
        //}

    }
}
