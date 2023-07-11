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
   public class cls_ctTRTaxrate
   {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRTaxrate() { }

        public string getMessage() { return this.Message.Replace("PAY_TR_TAXRATE", "").Replace("cls_ctTRTaxrate", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }
        private List<cls_TRTaxrate> getData(string condition)
        {
            List<cls_TRTaxrate> list_model = new List<cls_TRTaxrate>();
            cls_TRTaxrate model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", TAXRATE_ID");
                obj_str.Append(", TAXRATE_FROM");
                obj_str.Append(", TAXRATE_TO");
                obj_str.Append(", TAXRATE_TAX");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PAY_TR_TAXRATE");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, TAXRATE_FROM");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRTaxrate();

                    model.taxrate_id = Convert.ToInt32(dr["TAXRATE_ID"]);
                    model.taxrate_from = Convert.ToDouble(dr["TAXRATE_FROM"]);
                    model.taxrate_to = Convert.ToDouble(dr["TAXRATE_TO"]);
                    model.taxrate_tax = Convert.ToDouble(dr["TAXRATE_TAX"]);

                    model.company_code = dr["COMPANY_CODE"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                   

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Taxrate.getData)" + ex.ToString();
            }

            return list_model;
        }

        private DataTable doGetTable(string p)
        {
            throw new NotImplementedException();
        }

        public List<cls_TRTaxrate> getDataByFillter(string com)
        {
            string strCondition = "";

            if (!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";       

            

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, double from, double to)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT COMPANY_CODE");
                obj_str.Append(" FROM PAY_TR_TAXRATE");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND TAXRATE_FROM='" + from + "'");
                obj_str.Append(" AND TAXRATE_TO='" + to + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(TAXRATE.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(TAXRATE_ID) ");
                obj_str.Append(" FROM PAY_TR_TAXRATE");     

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(TAXRATE.getNextID)" + ex.ToString();
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

                obj_str.Append(" DELETE FROM PAY_TR_TAXRATE");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND TAXRATE_ID='" + id + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(TAXRATE.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_TRTaxrate model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.taxrate_from, model.taxrate_to))
                {
                    if (model.taxrate_id.Equals(0))
                    {
                        return "";
                    }
                    else
                    {
                        return this.update(model);
                    }
                }

                //if (this.checkDataOld(model.company_code, model.taxrate_from, model.taxrate_to))
                //{
                //    return this.update(model);
                //}

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO PAY_TR_TAXRATE");
                obj_str.Append(" (");
                obj_str.Append("TAXRATE_ID");
                obj_str.Append(", TAXRATE_FROM");
                obj_str.Append(", TAXRATE_TO");
                obj_str.Append(", TAXRATE_TAX");
                obj_str.Append(", COMPANY_CODE");
                obj_str.Append(", CREATED_BY");
                obj_str.Append(", CREATED_DATE");
                obj_str.Append(", FLAG");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append(" @TAXRATE_ID");
                obj_str.Append(", @TAXRATE_FROM");
                obj_str.Append(", @TAXRATE_TO");
                obj_str.Append(", @TAXRATE_TAX");
                obj_str.Append(", @COMPANY_CODE");
                obj_str.Append(", @CREATED_BY");
                obj_str.Append(", @CREATED_DATE");
                obj_str.Append(", @FLAG");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@TAXRATE_ID", SqlDbType.Int); obj_cmd.Parameters["@TAXRATE_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@TAXRATE_FROM", SqlDbType.VarChar); obj_cmd.Parameters["@TAXRATE_FROM"].Value = model.taxrate_from;
                obj_cmd.Parameters.Add("@TAXRATE_TO", SqlDbType.VarChar); obj_cmd.Parameters["@TAXRATE_TO"].Value = model.taxrate_to;
                obj_cmd.Parameters.Add("@TAXRATE_TAX", SqlDbType.VarChar); obj_cmd.Parameters["@TAXRATE_TAX"].Value = model.taxrate_tax;
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;
     
                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = model.taxrate_id.ToString();

                //blnResult = id.ToString();
            }
            catch (Exception ex)
            {
                blnResult ="";
                Message = "ERROR::(TAXRATE.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_TRTaxrate model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();


                obj_str.Append("UPDATE PAY_TR_TAXRATE SET ");

                obj_str.Append(" TAXRATE_FROM=@TAXRATE_FROM ");
                obj_str.Append(", TAXRATE_TO=@TAXRATE_TO ");
                obj_str.Append(", TAXRATE_TAX=@TAXRATE_TAX ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");



                //obj_str.Append(" WHERE TAXRATE_ID=@TAXRATE_ID ");


                obj_conn.doConnect();
                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@TAXRATE_FROM", SqlDbType.VarChar); obj_cmd.Parameters["@TAXRATE_FROM"].Value = model.taxrate_from;
                obj_cmd.Parameters.Add("@TAXRATE_TO", SqlDbType.VarChar); obj_cmd.Parameters["@TAXRATE_TO"].Value = model.taxrate_to;
                obj_cmd.Parameters.Add("@TAXRATE_TAX", SqlDbType.VarChar); obj_cmd.Parameters["@TAXRATE_TAX"].Value = model.taxrate_tax;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                //obj_cmd.Parameters.Add("@TAXRATE_ID", SqlDbType.Int); obj_cmd.Parameters["@TAXRATE_ID"].Value = model.taxrate_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.taxrate_id.ToString();


                //blnResult = model.taxrate_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(TAXRATE.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
 }
