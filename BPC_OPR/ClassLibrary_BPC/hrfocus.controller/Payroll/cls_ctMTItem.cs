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
   public class cls_ctMTItem
    {
     string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTItem() { }

        public string getMessage() { return this.Message.Replace("PAY_MT_ITEM", "").Replace("cls_ctMTItem", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTItem> getData(string condition)
        {
            List<cls_MTItem> list_model = new List<cls_MTItem>();
            cls_MTItem model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", ITEM_ID");
                obj_str.Append(", ITEM_CODE");
                obj_str.Append(", ISNULL(ITEM_NAME_TH, '') AS ITEM_NAME_TH");
                obj_str.Append(", ISNULL(ITEM_NAME_EN, '') AS ITEM_NAME_EN");

                obj_str.Append(", ITEM_TYPE");
                obj_str.Append(", ITEM_REGULAR");
                obj_str.Append(", ITEM_CALTAX");
                obj_str.Append(", ITEM_CALPF");
                obj_str.Append(", ITEM_CALSSO");
                obj_str.Append(", ITEM_CALOT");
                obj_str.Append(", ITEM_ALLOWANCE");


                obj_str.Append(", ITEM_CONTAX");
                obj_str.Append(", ITEM_SECTION");

                obj_str.Append(", ISNULL(ITEM_RATE, 0) AS ITEM_RATE");
 
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PAY_MT_ITEM");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY ITEM_TYPE DESC, ITEM_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTItem();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.item_id = Convert.ToInt32(dr["ITEM_ID"]);
                    model.item_code = dr["ITEM_CODE"].ToString();
                    model.item_name_th = dr["ITEM_NAME_TH"].ToString();
                    model.item_name_en = dr["ITEM_NAME_EN"].ToString();

                    model.item_type = dr["ITEM_TYPE"].ToString();
                    model.item_regular = dr["ITEM_REGULAR"].ToString();
                    model.item_caltax = dr["ITEM_CALTAX"].ToString();
                    model.item_calpf = dr["ITEM_CALPF"].ToString();
                    model.item_calsso = dr["ITEM_CALSSO"].ToString();
                    model.item_calot = dr["ITEM_CALOT"].ToString();
                    model.item_allowance = dr["ITEM_ALLOWANCE"].ToString();
                    
                    model.item_contax = dr["ITEM_CONTAX"].ToString();

                    model.item_section = dr["ITEM_SECTION"].ToString();
                    model.item_rate = Convert.ToDouble(dr["ITEM_RATE"]);
 
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch(Exception ex)
            {
                Message = "PAYI001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTItem> getDataByFillter(string com, string id, string code)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!id.Equals(""))
                strCondition += " AND ITEM_ID='" + id + "'";

            if (!code.Equals(""))
                strCondition += " AND ITEM_CODE='" + code + "'";

            return this.getData(strCondition);
        }
        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("SELECT MAX(ITEM_ID) ");
                obj_str.Append(" FROM PAY_MT_ITEM");      

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "PAYI002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string com, string code )
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ITEM_ID");
                obj_str.Append(" FROM PAY_MT_ITEM");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND ITEM_CODE='" + code + "'");
                
      
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "PAYI003:" + ex.ToString();
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

                obj_str.Append(" DELETE FROM PAY_MT_ITEM");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND ITEM_ID='" + id + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "PAYI004:" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTItem model)
        {
            string blnResult = "";
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.company_code, model.item_code ))
                {
                    if (model.item_id.Equals(0))
                    {
                        return "";
                    }
                    else
                    {
                        return this.update(model);
                    }
                }

                //-- Check data old
                //if (this.checkDataOld(model.company_code, model.item_code))
                //{
                //    return this.update(model);
                //}
                //if (this.checkDataOld(model.company_code, model.item_code))
                //{
                //    if (this.update(model))
                //        return model.item_id.ToString();
                //    else
                //        return "";                    
                //}

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PAY_MT_ITEM");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", ITEM_ID ");
                obj_str.Append(", ITEM_CODE ");
                obj_str.Append(", ITEM_NAME_TH ");
                obj_str.Append(", ITEM_NAME_EN ");
                obj_str.Append(", ITEM_TYPE ");
                obj_str.Append(", ITEM_REGULAR ");
                obj_str.Append(", ITEM_CALTAX ");
                obj_str.Append(", ITEM_CALPF ");
                obj_str.Append(", ITEM_CALSSO ");
                obj_str.Append(", ITEM_CALOT ");
                obj_str.Append(", ITEM_ALLOWANCE ");
                
                obj_str.Append(", ITEM_CONTAX ");
                obj_str.Append(", ITEM_SECTION ");
                obj_str.Append(", ITEM_RATE ");
 
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @ITEM_ID ");
                obj_str.Append(", @ITEM_CODE ");
                obj_str.Append(", @ITEM_NAME_TH ");
                obj_str.Append(", @ITEM_NAME_EN ");
                obj_str.Append(", @ITEM_TYPE ");
                obj_str.Append(", @ITEM_REGULAR ");
                obj_str.Append(", @ITEM_CALTAX ");
                obj_str.Append(", @ITEM_CALPF ");
                obj_str.Append(", @ITEM_CALSSO ");
                obj_str.Append(", @ITEM_CALOT ");
                obj_str.Append(", @ITEM_ALLOWANCE ");
                
                obj_str.Append(", @ITEM_CONTAX ");
                obj_str.Append(", @ITEM_SECTION ");
                obj_str.Append(", @ITEM_RATE ");
 
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");
                
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                model.item_id = this.getNextID();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@ITEM_ID", SqlDbType.Int); obj_cmd.Parameters["@ITEM_ID"].Value = this.getNextID();

                obj_cmd.Parameters.Add("@ITEM_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_CODE"].Value = model.item_code;
                obj_cmd.Parameters.Add("@ITEM_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_NAME_TH"].Value = model.item_name_th;
                obj_cmd.Parameters.Add("@ITEM_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_NAME_EN"].Value = model.item_name_en;
                obj_cmd.Parameters.Add("@ITEM_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_TYPE"].Value = model.item_type;
                obj_cmd.Parameters.Add("@ITEM_REGULAR", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_REGULAR"].Value = model.item_regular;
                obj_cmd.Parameters.Add("@ITEM_CALTAX", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_CALTAX"].Value = model.item_caltax;
                obj_cmd.Parameters.Add("@ITEM_CALPF", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_CALPF"].Value = model.item_calpf;
                obj_cmd.Parameters.Add("@ITEM_CALSSO", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_CALSSO"].Value = model.item_calsso;
                obj_cmd.Parameters.Add("@ITEM_CALOT", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_CALOT"].Value = model.item_calot;
                obj_cmd.Parameters.Add("@ITEM_ALLOWANCE", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_ALLOWANCE"].Value = model.item_allowance;

                
                obj_cmd.Parameters.Add("@ITEM_CONTAX", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_CONTAX"].Value = model.item_contax;
                obj_cmd.Parameters.Add("@ITEM_SECTION", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_SECTION"].Value = model.item_section;
                obj_cmd.Parameters.Add("@ITEM_RATE", SqlDbType.Decimal); obj_cmd.Parameters["@ITEM_RATE"].Value = model.item_rate;
 
                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;
               
                obj_cmd.ExecuteNonQuery();
                                
                obj_conn.doClose();
                //blnResult = true;
                blnResult = model.item_id.ToString();
 
            }
            catch (Exception ex)
            {
                Message = "PAYI005:" + ex.ToString();
             }


            return blnResult;

        }

        public string update(cls_MTItem model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PAY_MT_ITEM SET ");

                //obj_str.Append(" ITEM_CODE=@ITEM_CODE ");
                obj_str.Append(" ITEM_NAME_TH=@ITEM_NAME_TH ");
                obj_str.Append(", ITEM_NAME_EN=@ITEM_NAME_EN ");
                obj_str.Append(", ITEM_TYPE=@ITEM_TYPE ");
                obj_str.Append(", ITEM_REGULAR=@ITEM_REGULAR ");
                obj_str.Append(", ITEM_CALTAX=@ITEM_CALTAX ");
                obj_str.Append(", ITEM_CALPF=@ITEM_CALPF ");
                obj_str.Append(", ITEM_CALSSO=@ITEM_CALSSO ");
                obj_str.Append(", ITEM_CALOT=@ITEM_CALOT ");
                obj_str.Append(", ITEM_ALLOWANCE=@ITEM_ALLOWANCE ");
                
                obj_str.Append(", ITEM_CONTAX=@ITEM_CONTAX ");
                obj_str.Append(", ITEM_SECTION=@ITEM_SECTION ");
                obj_str.Append(", ITEM_RATE=@ITEM_RATE ");
 
                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");

                //obj_str.Append(" WHERE ITEM_CODE=@ITEM_CODE ");
                obj_str.Append(" WHERE 1=1 ");
                if (!model.item_id.Equals(0))
                {
                    obj_str.Append(" AND ITEM_ID=@ITEM_ID ");
                }
                else
                {
                    obj_str.Append(" AND ITEM_CODE='" + model.item_code + "'");

                }
 
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@ITEM_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_NAME_TH"].Value = model.item_name_th;
                obj_cmd.Parameters.Add("@ITEM_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_NAME_EN"].Value = model.item_name_en;
                obj_cmd.Parameters.Add("@ITEM_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_TYPE"].Value = model.item_type;
                obj_cmd.Parameters.Add("@ITEM_REGULAR", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_REGULAR"].Value = model.item_regular;
                obj_cmd.Parameters.Add("@ITEM_CALTAX", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_CALTAX"].Value = model.item_caltax;
                obj_cmd.Parameters.Add("@ITEM_CALPF", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_CALPF"].Value = model.item_calpf;
                obj_cmd.Parameters.Add("@ITEM_CALSSO", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_CALSSO"].Value = model.item_calsso;
                obj_cmd.Parameters.Add("@ITEM_CALOT", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_CALOT"].Value = model.item_calot;
                obj_cmd.Parameters.Add("@ITEM_ALLOWANCE", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_ALLOWANCE"].Value = model.item_allowance;

                obj_cmd.Parameters.Add("@ITEM_CONTAX", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_CONTAX"].Value = model.item_contax;
                obj_cmd.Parameters.Add("@ITEM_SECTION", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_SECTION"].Value = model.item_section;
                obj_cmd.Parameters.Add("@ITEM_RATE", SqlDbType.Decimal); obj_cmd.Parameters["@ITEM_RATE"].Value = model.item_rate;
 
                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;
                obj_cmd.Parameters.Add("@ITEM_ID", SqlDbType.Int); obj_cmd.Parameters["@ITEM_ID"].Value = model.item_id;

                obj_cmd.Parameters.Add("@ITEM_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@ITEM_CODE"].Value = model.item_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = model.item_id.ToString();
                //blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "PAYIM006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}
