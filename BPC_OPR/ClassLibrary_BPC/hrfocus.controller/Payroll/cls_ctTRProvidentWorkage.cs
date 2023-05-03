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
   public class cls_ctTRProvidentWorkage
   {
       string Message = string.Empty;

       cls_ctConnection Obj_conn = new cls_ctConnection();

       public cls_ctTRProvidentWorkage() { }

       public string getMessage() { return this.Message.Replace("PAY_TR_PROVIDENT_WORKAGE", "").Replace("cls_ctTRProvidentWorkage", "").Replace("line", ""); }

       public void dispose()
       {
           Obj_conn.doClose();
       }

       private List<cls_TRProvidentWorkage> getData(string condition)
       {
           List<cls_TRProvidentWorkage> list_model = new List<cls_TRProvidentWorkage>();
           cls_TRProvidentWorkage model;
           try
           {
               System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

               obj_str.Append("SELECT ");

               obj_str.Append("COMPANY_CODE");
               obj_str.Append(", PROVIDENT_CODE");
               obj_str.Append(", WORKAGE_FROM");
               obj_str.Append(", WORKAGE_TO");
               obj_str.Append(", RATE_EMP");
               obj_str.Append(", RATE_COM");

               obj_str.Append(" FROM PAY_TR_PROVIDENT_WORKAGE");
               obj_str.Append(" WHERE 1=1");

               if (!condition.Equals(""))
                   obj_str.Append(" " + condition);

               obj_str.Append(" ORDER BY COMPANY_CODE, PROVIDENT_CODE, WORKAGE_FROM");

               DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

               foreach (DataRow dr in dt.Rows)
               {
                   model = new cls_TRProvidentWorkage();

                   model.company_code = Convert.ToString(dr["COMPANY_CODE"]);
                   model.provident_code = Convert.ToString(dr["PROVIDENT_CODE"]);
                   model.workage_from = Convert.ToDouble(dr["WORKAGE_FROM"]);
                   model.workage_to = Convert.ToDouble(dr["WORKAGE_TO"]);
                   model.rate_emp = Convert.ToDouble(dr["RATE_EMP"]);
                   model.rate_com = Convert.ToDouble(dr["RATE_COM"]);

                   list_model.Add(model);
               }

           }
           catch (Exception ex)
           {
               Message = "PAYPW001:" + ex.ToString();
           }

           return list_model;
       }

       public List<cls_TRProvidentWorkage> getDataByFillter(string com, string code)
       {
           string strCondition = "";

           strCondition += " AND COMPANY_CODE='" + com + "'";

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

               obj_str.Append("SELECT ISNULL(PROVIDENT_CODE, 1) ");
               obj_str.Append(" FROM EMP_TR_PROVIDENT");
               obj_str.Append(" ORDER BY PROVIDENT_CODE DESC ");

               DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

               if (dt.Rows.Count > 0)
               {
                   intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
               }
           }
           catch (Exception ex)
           {
               Message = "PAYPW002:" + ex.ToString();
           }

           return intResult;
       }

       public bool checkDataOld(string com, string code, double workagefrom)
       {
           bool blnResult = false;
           try
           {
               System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

               obj_str.Append("SELECT PROVIDENT_ID");
               obj_str.Append(" FROM PAY_TR_PROVIDENT_WORKAGE");
               obj_str.Append(" WHERE 1=1 ");
               obj_str.Append(" AND COMPANY_CODE='" + com + "'");
               obj_str.Append(" AND PROVIDENT_CODE='" + code + "'");
               obj_str.Append(" AND WORKAGE_FROM='" + workagefrom + "'");


               DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

               if (dt.Rows.Count > 0)
               {
                   blnResult = true;
               }
           }
           catch (Exception ex)
           {
               Message = "PAYPW003:" + ex.ToString();
           }

           return blnResult;
       }

       public bool delete(string com, string code)
       {
           bool blnResult = true;
           try
           {
               cls_ctConnection obj_conn = new cls_ctConnection();

               System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

               obj_str.Append(" DELETE FROM PAY_TR_PROVIDENT_WORKAGE");
               obj_str.Append(" WHERE 1=1 ");
               obj_str.Append(" AND COMPANY_CODE ='" + com + "'");
               obj_str.Append(" AND PROVIDENT_CODE ='" + code + "'");


               blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

           }
           catch (Exception ex)
           {
               blnResult = false;
               Message = "PAYPW004:" + ex.ToString();
           }

           return blnResult;
       }

       public bool insert(List<cls_TRProvidentWorkage> list_model)
       {
           bool blnResult = false;
           //string strResult = "";
           try
           {
               cls_ctConnection obj_conn = new cls_ctConnection();
               System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

               obj_str.Append("INSERT INTO PAY_TR_PROVIDENT_WORKAGE");
               obj_str.Append(" (");
               obj_str.Append("COMPANY_CODE ");
               obj_str.Append(", PROVIDENT_CODE ");
               obj_str.Append(", WORKAGE_FROM ");
               obj_str.Append(", WORKAGE_TO ");
               obj_str.Append(", RATE_EMP ");
               obj_str.Append(", RATE_COM ");
               obj_str.Append(" )");

               obj_str.Append(" VALUES(");
               obj_str.Append("@COMPANY_CODE ");
               obj_str.Append(", @PROVIDENT_CODE ");
               obj_str.Append(", @WORKAGE_FROM ");
               obj_str.Append(", @WORKAGE_TO ");
               obj_str.Append(", @RATE_EMP ");
               obj_str.Append(", @RATE_COM ");
               obj_str.Append(" )");

               obj_conn.doConnect();

               obj_conn.doOpenTransaction();

               System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

               obj_str2.Append(" DELETE FROM PAY_TR_PROVIDENT_WORKAGE");
               obj_str2.Append(" WHERE 1=1 ");
               obj_str2.Append(" AND COMPANY_CODE='" + list_model[0].company_code + "'");
               obj_str2.Append(" AND PROVIDENT_CODE='" + list_model[0].provident_code + "'");

               blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());

               if (blnResult)
               {
                   SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                   obj_cmd.Transaction = obj_conn.getTransaction();

                   obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                   obj_cmd.Parameters.Add("@PROVIDENT_CODE", SqlDbType.VarChar);
                   obj_cmd.Parameters.Add("@WORKAGE_FROM", SqlDbType.Decimal);
                   obj_cmd.Parameters.Add("@WORKAGE_TO", SqlDbType.Decimal);
                   obj_cmd.Parameters.Add("@RATE_EMP", SqlDbType.Decimal);
                   obj_cmd.Parameters.Add("@RATE_COM", SqlDbType.Decimal);

                   foreach (cls_TRProvidentWorkage model in list_model)
                   {

                       obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                       obj_cmd.Parameters["@PROVIDENT_CODE"].Value = model.provident_code;
                       obj_cmd.Parameters["@WORKAGE_FROM"].Value = model.workage_from;
                       obj_cmd.Parameters["@WORKAGE_TO"].Value = model.workage_to;
                       obj_cmd.Parameters["@RATE_EMP"].Value = model.rate_emp;
                       obj_cmd.Parameters["@RATE_COM"].Value = model.rate_com;

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
               Message = "ERROR::(ProvidentWorkage.insert)" + ex.ToString();
           }

           return blnResult;
       }
    }
}
