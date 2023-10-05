using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary_BPC.hrfocus.model;
using System.Data.SqlClient;
using System.Data;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRProcontract
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRProcontract() { }

        public string getMessage() { return this.Message.Replace("PRO_TR_PROCONTRACT", "").Replace("cls_ctTRProcontract", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRProcontract> getData(string condition)
        {
            List<cls_TRProcontract> list_model = new List<cls_TRProcontract>();
            cls_TRProcontract model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("PROCONTRACT_ID");
                obj_str.Append(", PROCONTRACT_REF");
                obj_str.Append(", PROCONTRACT_DATE");

                obj_str.Append(", ISNULL(PROCONTRACT_AMOUNT, 0) AS PROCONTRACT_AMOUNT");
                obj_str.Append(", ISNULL(PROCONTRACT_FROMDATE, '01/01/1900') AS PROCONTRACT_FROMDATE");
                obj_str.Append(", ISNULL(PROCONTRACT_TODATE, '01/01/1900') AS PROCONTRACT_TODATE");
                obj_str.Append(", ISNULL(PROCONTRACT_CUSTOMER, '') AS PROCONTRACT_CUSTOMER");
                obj_str.Append(", ISNULL(PROCONTRACT_BIDDER, '') AS PROCONTRACT_BIDDER");
             
                obj_str.Append(", PROJECT_CODE");
                obj_str.Append(", PROCONTRACT_TYPE");                

                
                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM PRO_TR_PROCONTRACT");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY PROJECT_CODE, PROCONTRACT_REF");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRProcontract();

                    model.procontract_id = Convert.ToInt32(dr["PROCONTRACT_ID"]);
                    model.procontract_ref = Convert.ToString(dr["PROCONTRACT_REF"]);
                    model.procontract_date = Convert.ToDateTime(dr["PROCONTRACT_DATE"]);
                    model.procontract_amount = Convert.ToDecimal(dr["PROCONTRACT_AMOUNT"]);
                    model.procontract_fromdate = Convert.ToDateTime(dr["PROCONTRACT_FROMDATE"]);
                    model.procontract_todate = Convert.ToDateTime(dr["PROCONTRACT_TODATE"]);
                    model.procontract_customer = Convert.ToString(dr["PROCONTRACT_CUSTOMER"]);
                    model.procontract_bidder = Convert.ToString(dr["PROCONTRACT_BIDDER"]);                                        
                    model.project_code = Convert.ToString(dr["PROJECT_CODE"]);
                    model.procontract_type = Convert.ToString(dr["PROCONTRACT_TYPE"]);

                   
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "BNK001:" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRProcontract> getDataByFillter(string project)
        {
            string strCondition = "";

            if (!project.Equals(""))
                strCondition += " AND PROJECT_CODE='" + project + "'";

            return this.getData(strCondition);
        }

        public List<cls_TRProcontract> getDataCurrents(string project, DateTime procontract_fromdate, DateTime procontract_todate)
        {
            string strCondition = "";

            if (!project.Equals(""))
                strCondition += " AND PROJECT_CODE='" + project + "'";

            strCondition += "AND PROCONTRACT_FROMDATE in( (select PROCONTRACT_FROMDATE from PRO_TR_PROCONTRACT where PROCONTRACT_FROMDATE between'" + procontract_fromdate.ToString("MM/dd/yyyy") + "' and '" + procontract_todate.ToString("MM/dd/yyyy") + "'))";
            return this.getData(strCondition);
        }

        public List<cls_TRProcontract> getDataCurrents2(string project, DateTime procontract_fromdate, DateTime procontract_todate)
        {
            string strCondition = "";

            if (!project.Equals(""))
                strCondition += " AND PROJECT_CODE='" + project + "'";

            strCondition += "AND PROCONTRACT_TODATE in( (select PROCONTRACT_TODATE from PRO_TR_PROCONTRACT where PROCONTRACT_TODATE between'" + procontract_fromdate.ToString("MM/dd/yyyy") + "' and '" + procontract_todate.ToString("MM/dd/yyyy") + "'))";
            return this.getData(strCondition);
        }


        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ISNULL(PROCONTRACT_ID, 1) ");
                obj_str.Append(" FROM PRO_MT_PROCONTACT");
                obj_str.Append(" ORDER BY PROCONTRACT_ID DESC ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "BNK002:" + ex.ToString();
            }

            return intResult;
        }

        public bool checkDataOld(string project, string contract_ref)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT PROCONTRACT_REF");
                obj_str.Append(" FROM PRO_TR_PROCONTRACT");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROCONTRACT_REF='" + contract_ref + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "BNK003:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string project)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_TR_PROCONTRACT");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                
                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BNK004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string project, string contract_ref)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("DELETE FROM PRO_TR_PROCONTRACT");
                obj_str.Append(" WHERE PROJECT_CODE='" + project + "'");
                obj_str.Append(" AND PROCONTRACT_REF='" + contract_ref + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "BNK004:" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TRProcontract model)
        {
            bool blnResult = false;
            try
            {

                //-- Check data old
                if (this.checkDataOld(model.project_code, model.procontract_ref))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO PRO_TR_PROCONTRACT");
                obj_str.Append(" (");
                obj_str.Append("PROCONTRACT_ID ");
                obj_str.Append(", PROCONTRACT_REF ");
                obj_str.Append(", PROCONTRACT_DATE ");
                obj_str.Append(", PROCONTRACT_AMOUNT ");
                obj_str.Append(", PROCONTRACT_FROMDATE ");
                obj_str.Append(", PROCONTRACT_TODATE ");
                obj_str.Append(", PROCONTRACT_CUSTOMER ");
                obj_str.Append(", PROCONTRACT_BIDDER ");     
                obj_str.Append(", PROJECT_CODE ");
                obj_str.Append(", PROCONTRACT_TYPE ");

                
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@PROCONTRACT_ID ");
                obj_str.Append(", @PROCONTRACT_REF ");
                obj_str.Append(", @PROCONTRACT_DATE ");
                obj_str.Append(", @PROCONTRACT_AMOUNT ");
                obj_str.Append(", @PROCONTRACT_FROMDATE ");
                obj_str.Append(", @PROCONTRACT_TODATE ");
                obj_str.Append(", @PROCONTRACT_CUSTOMER ");
                obj_str.Append(", @PROCONTRACT_BIDDER ");                
                obj_str.Append(", @PROJECT_CODE ");
                obj_str.Append(", @PROCONTRACT_TYPE ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROCONTRACT_ID", SqlDbType.Int); obj_cmd.Parameters["@PROCONTRACT_ID"].Value = this.getNextID();
                obj_cmd.Parameters.Add("@PROCONTRACT_REF", SqlDbType.VarChar); obj_cmd.Parameters["@PROCONTRACT_REF"].Value = model.procontract_ref;
                obj_cmd.Parameters.Add("@PROCONTRACT_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROCONTRACT_DATE"].Value = model.procontract_date;
                obj_cmd.Parameters.Add("@PROCONTRACT_AMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@PROCONTRACT_AMOUNT"].Value = model.procontract_amount;
                obj_cmd.Parameters.Add("@PROCONTRACT_FROMDATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROCONTRACT_FROMDATE"].Value = model.procontract_fromdate;
                obj_cmd.Parameters.Add("@PROCONTRACT_TODATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROCONTRACT_TODATE"].Value = model.procontract_todate;
                obj_cmd.Parameters.Add("@PROCONTRACT_CUSTOMER", SqlDbType.VarChar); obj_cmd.Parameters["@PROCONTRACT_CUSTOMER"].Value = model.procontract_customer;
                obj_cmd.Parameters.Add("@PROCONTRACT_BIDDER", SqlDbType.VarChar); obj_cmd.Parameters["@PROCONTRACT_BIDDER"].Value = model.procontract_bidder;               
                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;
                obj_cmd.Parameters.Add("@PROCONTRACT_TYPE", SqlDbType.VarChar); obj_cmd.Parameters["@PROCONTRACT_TYPE"].Value = model.procontract_type;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
               
                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "BNK005:" + ex.ToString();               
            }

            return blnResult;
        }

        public bool update(cls_TRProcontract model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("UPDATE PRO_TR_PROCONTRACT SET ");

                obj_str.Append(" PROCONTRACT_DATE=@PROCONTRACT_DATE ");
                obj_str.Append(", PROCONTRACT_AMOUNT=@PROCONTRACT_AMOUNT ");
                obj_str.Append(", PROCONTRACT_FROMDATE=@PROCONTRACT_FROMDATE ");
                obj_str.Append(", PROCONTRACT_TODATE=@PROCONTRACT_TODATE ");
                obj_str.Append(", PROCONTRACT_CUSTOMER=@PROCONTRACT_CUSTOMER ");
                obj_str.Append(", PROCONTRACT_BIDDER=@PROCONTRACT_BIDDER ");
                

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                obj_str.Append(" WHERE PROCONTRACT_ID=@PROCONTRACT_ID ");
               
                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@PROCONTRACT_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROCONTRACT_DATE"].Value = model.procontract_date;
                obj_cmd.Parameters.Add("@PROCONTRACT_AMOUNT", SqlDbType.Decimal); obj_cmd.Parameters["@PROCONTRACT_AMOUNT"].Value = model.procontract_amount;
                obj_cmd.Parameters.Add("@PROCONTRACT_FROMDATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROCONTRACT_FROMDATE"].Value = model.procontract_fromdate;
                obj_cmd.Parameters.Add("@PROCONTRACT_TODATE", SqlDbType.DateTime); obj_cmd.Parameters["@PROCONTRACT_TODATE"].Value = model.procontract_todate;
                obj_cmd.Parameters.Add("@PROCONTRACT_CUSTOMER", SqlDbType.VarChar); obj_cmd.Parameters["@PROCONTRACT_CUSTOMER"].Value = model.procontract_customer;
                obj_cmd.Parameters.Add("@PROCONTRACT_BIDDER", SqlDbType.VarChar); obj_cmd.Parameters["@PROCONTRACT_BIDDER"].Value = model.procontract_bidder;     

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;


                obj_cmd.Parameters.Add("@PROCONTRACT_ID", SqlDbType.Int); obj_cmd.Parameters["@PROCONTRACT_ID"].Value = model.procontract_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "BNK006:" + ex.ToString();
            }

            return blnResult;
        }

    }
}
