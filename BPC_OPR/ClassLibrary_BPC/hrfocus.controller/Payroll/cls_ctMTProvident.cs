using ClassLibrary_BPC.hrfocus.model.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller.Payroll
{
   public class cls_ctMTProvident
   {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTProvident() { }

        public string getMessage() { return this.Message.Replace("PAY_MT_PROVIDENT", "").Replace("cls_ctMTProvident", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTProvident> getData(string condition)
        {
            List<cls_MTProvident> list_model = new List<cls_MTProvident>();
            cls_MTProvident model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", PROVIDENT_ID");
                obj_str.Append(", PROVIDENT_CODE");
                obj_str.Append(", ISNULL(PROVIDENT_NAME_TH, '') AS PROVIDENT_NAME_TH");
                obj_str.Append(", ISNULL(PROVIDENT_NAME_EN, '') AS PROVIDENT_NAME_EN");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PAY_MT_PROVIDENT");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, PROVIDENT_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTProvident();

                    model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                    model.provident_id = Convert.ToInt32(dr["PROVIDENT_ID"]);
                    model.provident_code = Convert.ToString(dr["PROVIDENT_CODE"]);
                    model.provident_name_th = Convert.ToString(dr["PROVIDENT_NAME_TH"]);
                    model.provident_name_en = Convert.ToString(dr["PROVIDENT_NAME_EN"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                                                                                        
                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "PAYP001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTProvident> getDataByFillter(string com, string id, string code)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!id.Equals(""))
                strCondition += " AND PROVIDENT_ID='" + id + "'";

            if (!code.Equals(""))
                strCondition += " AND PROVIDENT_CODE='" + code + "'";
            

            return this.getData(strCondition);
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(PROVIDENT_ID) ");
                obj_str.Append(" FROM PAY_MT_PROVIDENT");          

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "PAYP002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string code, string id)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PROVIDENT_ID");
                obj_str.Append(" FROM PAY_MT_PROVIDENT");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND PROVIDENT_CODE='" + code + "'");
                obj_str.Append(" AND PROVIDENT_ID='" + id + "'");


                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "PAYP003:" + ex.ToString();
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
                obj_str.Append(" DELETE FROM PAY_TR_PROVIDENT_WORKAGE");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND PROVIDENT_CODE IN (SELECT PROVIDENT_CODE FROM PAY_MT_PROVIDENT WHERE PROVIDENT_ID='" + id + "')");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());
                if (blnResult)
                {
                    obj_str.Append(" DELETE FROM PAY_MT_PROVIDENT");
                    obj_str.Append(" WHERE 1=1 ");
                    obj_str.Append(" AND PROVIDENT_ID='" + id + "'");

                    blnResult = obj_conn.doExecuteSQL(obj_str.ToString());
                }

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "PAYP004:" + ex.ToString();
            }

            return blnResult;
        }
        public bool insert(cls_MTProvident model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
 

                if (this.checkDataOld(model.company_code, model.provident_code, model.provident_id.ToString()))
                {
                    if (model.provident_id.Equals(0))
                    {
                        return false;
                    }
                    else
                    {
                        return this.update(model);
                    }
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PAY_MT_PROVIDENT");
                obj_str.Append(" (");
                obj_str.Append("PROVIDENT_ID ");
                obj_str.Append(", PROVIDENT_CODE ");
                obj_str.Append(", PROVIDENT_NAME_TH ");
                obj_str.Append(", PROVIDENT_NAME_EN ");

                obj_str.Append(", COMPANY_CODE ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@PROVIDENT_ID ");
                obj_str.Append(", @PROVIDENT_CODE ");
                obj_str.Append(", @PROVIDENT_NAME_TH ");
                obj_str.Append(", @PROVIDENT_NAME_EN ");
                obj_str.Append(", @COMPANY_CODE ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

               
                obj_cmd.Parameters.Add("@PROVIDENT_ID", SqlDbType.Int); obj_cmd.Parameters["@PROVIDENT_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@PROVIDENT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROVIDENT_CODE"].Value = model.provident_code;
                obj_cmd.Parameters.Add("@PROVIDENT_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROVIDENT_NAME_TH"].Value = model.provident_name_th;
                obj_cmd.Parameters.Add("@PROVIDENT_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROVIDENT_NAME_EN"].Value = model.provident_name_en;

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;
     
                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(PAYP006.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public bool update(cls_MTProvident model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                  obj_str.Append("UPDATE PAY_MT_PROVIDENT SET ");

                obj_str.Append(" PROVIDENT_CODE=@PROVIDENT_CODE ");
                obj_str.Append(", PROVIDENT_NAME_TH=@PROVIDENT_NAME_TH ");
                obj_str.Append(", PROVIDENT_NAME_EN=@PROVIDENT_NAME_EN ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");


                obj_str.Append(" WHERE PROVIDENT_ID=@PROVIDENT_ID ");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROVIDENT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROVIDENT_CODE"].Value = model.provident_code;
                obj_cmd.Parameters.Add("@PROVIDENT_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROVIDENT_NAME_TH"].Value = model.provident_name_th;
                obj_cmd.Parameters.Add("@PROVIDENT_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROVIDENT_NAME_EN"].Value = model.provident_name_en;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.Parameters.Add("@PROVIDENT_ID", SqlDbType.Int); obj_cmd.Parameters["@PROVIDENT_ID"].Value = model.provident_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(PAYP006.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
//

//        public bool insert(cls_MTProvident model)
//        {
//            bool blnResult = false;
//            string strResult = "";
//            try
//            {

//                //-- Check data old
//                if (this.checkDataOld(model.company_code, model.provident_code, model.provident_id.ToString()))
//                {
//                    return this.update(model);
//                }

//                cls_ctConnection obj_conn = new cls_ctConnection();
//                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
//                //int id = this.getNextID();
//                obj_str.Append("INSERT INTO PAY_MT_PROVIDENT");
//                obj_str.Append(" (");
//                obj_str.Append("PROVIDENT_ID ");
//                obj_str.Append(", PROVIDENT_CODE ");
//                obj_str.Append(", PROVIDENT_NAME_TH ");
//                obj_str.Append(", PROVIDENT_NAME_EN ");

//                obj_str.Append(", COMPANY_CODE ");

//                obj_str.Append(", CREATED_BY ");
//                obj_str.Append(", CREATED_DATE ");
//                obj_str.Append(", FLAG ");
//                obj_str.Append(" )");

//                obj_str.Append(" VALUES(");
//                obj_str.Append("@PROVIDENT_ID ");
//                obj_str.Append(", @PROVIDENT_CODE ");
//                obj_str.Append(", @PROVIDENT_NAME_TH ");
//                obj_str.Append(", @PROVIDENT_NAME_EN ");
//                obj_str.Append(", @COMPANY_CODE ");
//                obj_str.Append(", @CREATED_BY ");
//                obj_str.Append(", @CREATED_DATE ");
//                obj_str.Append(", @FLAG ");
//                obj_str.Append(" )");

//                obj_conn.doConnect();

//                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
//                model.provident_id = this.getNextID();

//                obj_cmd.Parameters.Add("@PROVIDENT_ID", SqlDbType.Int); obj_cmd.Parameters["@PROVIDENT_ID"].Value = this.getNextID();
//                obj_cmd.Parameters.Add("@PROVIDENT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROVIDENT_CODE"].Value = model.provident_code;
//                obj_cmd.Parameters.Add("@PROVIDENT_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROVIDENT_NAME_TH"].Value = model.provident_name_th;
//                obj_cmd.Parameters.Add("@PROVIDENT_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROVIDENT_NAME_EN"].Value = model.provident_name_en;

//                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

//                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
//                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
//                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;
     
//                obj_cmd.ExecuteNonQuery();

//                obj_conn.doClose();
//                blnResult = true;
//                strResult = model.provident_id.ToString();
//            }
//            catch (Exception ex)
//            {
//                Message = "PAYP005:" + ex.ToString();
//                strResult = "";

//            }

//            return blnResult;
//        }

//        public bool update(cls_MTProvident model)
//        {
//            bool blnResult = false;
//            try
//            {
//                cls_ctConnection obj_conn = new cls_ctConnection();
//                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
//                obj_str.Append("UPDATE PAY_MT_PROVIDENT SET ");

//                obj_str.Append(" PROVIDENT_CODE=@PROVIDENT_CODE ");
//                obj_str.Append(", PROVIDENT_NAME_TH=@PROVIDENT_NAME_TH ");
//                obj_str.Append(", PROVIDENT_NAME_EN=@PROVIDENT_NAME_EN ");

//                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
//                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
//                obj_str.Append(", FLAG=@FLAG ");


//                obj_str.Append(" WHERE PROVIDENT_ID=@PROVIDENT_ID ");

//                obj_conn.doConnect();

//                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

//                obj_cmd.Parameters.Add("@PROVIDENT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROVIDENT_CODE"].Value = model.provident_code;
//                obj_cmd.Parameters.Add("@PROVIDENT_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@PROVIDENT_NAME_TH"].Value = model.provident_name_th;
//                obj_cmd.Parameters.Add("@PROVIDENT_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@PROVIDENT_NAME_EN"].Value = model.provident_name_en;

//                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
//                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
//                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

//                obj_cmd.Parameters.Add("@PROVIDENT_ID", SqlDbType.Int); obj_cmd.Parameters["@PROVIDENT_ID"].Value = model.provident_id;

//                obj_cmd.ExecuteNonQuery();

//                obj_conn.doClose();

//                blnResult = true;
//            }
//            catch (Exception ex)
//            {
//                Message = "PAYP006:" + ex.ToString();
//            }

//            return blnResult;
//        }
//    }
//}