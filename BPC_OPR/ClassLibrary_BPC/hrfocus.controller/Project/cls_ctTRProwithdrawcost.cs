using ClassLibrary_BPC.hrfocus.model.Project;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller.Project
{
   public class cls_ctTRProwithdrawcost
    {
       string Message = string.Empty;

       cls_ctConnection Obj_conn = new cls_ctConnection();

       public cls_ctTRProwithdrawcost() { }

       public string getMessage() { return this.Message.Replace("PRO_TR_PROWITHDRAWCOST", "").Replace("cls_ctTRProwithdrawcost", "").Replace("line", ""); }

       public void dispose()
       {
           Obj_conn.doClose();
       }

       private List<cls_TRProwithdrawcost> getData(string condition)
       {
           List<cls_TRProwithdrawcost> list_model = new List<cls_TRProwithdrawcost>();
           cls_TRProwithdrawcost model;
           try
           {
               System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

               obj_str.Append("SELECT ");

               obj_str.Append("PROWITHDRAW_ID");
               obj_str.Append(", PROCOST_CODE");
               obj_str.Append(", PROWITHDRAWCOST_AMOUNT");
               
               obj_str.Append(" FROM PRO_TR_PROWITHDRAWCOST");
               obj_str.Append(" WHERE 1=1");

               if (!condition.Equals(""))
                   obj_str.Append(" " + condition);

               obj_str.Append(" ORDER BY PROCOST_CODE ");

               DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

               foreach (DataRow dr in dt.Rows)
               {
                   model = new cls_TRProwithdrawcost();

                   model.prowithdraw_id = Convert.ToString(dr["PROWITHDRAW_ID"]);
                   model.procost_code = Convert.ToString(dr["PROCOST_CODE"]);
                   model.prowithdrawcost_amount = Convert.ToDateTime(dr["PROWITHDRAWCOST_AMOUNT"]);
 

                   list_model.Add(model);
               }

           }
           catch (Exception ex)
           {
               Message = "PAYPW001:" + ex.ToString();
           }

           return list_model;
       }

       public List<cls_TRProwithdrawcost> getDataByFillter( string id, string code)
       {
           string strCondition = "";

           strCondition += " AND PROWITHDRAW_ID='" + id + "'";

           if (!code.Equals(""))
               strCondition += " AND PROCOST_CODE='" + code + "'";
           return this.getData(strCondition);
       }

        

       public bool checkDataOld(string com, string code)
       {
           bool blnResult = false;
           try
           {
               System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

               obj_str.Append("SELECT PROCOST_CODE");
               obj_str.Append(" FROM PRO_TR_PROWITHDRAWCOST");
               obj_str.Append(" WHERE 1=1 ");
 

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

       public bool delete(string code,string id)
       {
           bool blnResult = true;
           try
           {
               cls_ctConnection obj_conn = new cls_ctConnection();

               System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

               obj_str.Append(" DELETE FROM PRO_TR_PROWITHDRAWCOST");
               obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND PROCOST_CODE ='" + code + "'");
                obj_str.Append(" AND PROWITHDRAW_ID ='" + id + "'");

                
               blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

           }
           catch (Exception ex)
           {
               blnResult = false;
               Message = "PAYPW004:" + ex.ToString();
           }

           return blnResult;
       }



       public bool insert(List<cls_TRProwithdrawcost> list_model)
       {
           bool blnResult = false;
           //string strResult = "";
           try
         
               {
                //-- Check data old
                   if (!this.delete(list_model[0].procost_code, list_model[0].prowithdraw_id.ToString()))
                {
                    return false;
                }

               cls_ctConnection obj_conn = new cls_ctConnection();
               System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

               obj_str.Append("INSERT INTO PRO_TR_PROWITHDRAWCOST");
               obj_str.Append(" (");
               obj_str.Append("PROWITHDRAW_ID ");
               obj_str.Append(", PROCOST_CODE ");
               obj_str.Append(", PROWITHDRAWCOST_AMOUNT ");
               
               obj_str.Append(" )");

               obj_str.Append(" VALUES(");
               obj_str.Append("@PROWITHDRAW_ID ");
               obj_str.Append(", @PROCOST_CODE ");
               obj_str.Append(", @PROWITHDRAWCOST_AMOUNT ");
               
               obj_str.Append(" )");

               obj_conn.doConnect();

               obj_conn.doOpenTransaction();

               System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();
 

               obj_str2.Append(" DELETE FROM PRO_TR_PROWITHDRAWCOST");
               obj_str2.Append(" WHERE 1=1 ");
               obj_str2.Append(" AND PROCOST_CODE='" + list_model[0].procost_code + "'");
               obj_str2.Append(" AND PROWITHDRAWCOST_AMOUNT='" + list_model[0].prowithdrawcost_amount + "'");

               blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());

               if (blnResult)
               {
                   SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                   obj_cmd.Transaction = obj_conn.getTransaction();

                   obj_cmd.Parameters.Add("@PROWITHDRAW_ID", SqlDbType.VarChar);
                   obj_cmd.Parameters.Add("@PROCOST_CODE", SqlDbType.VarChar);
                   obj_cmd.Parameters.Add("@PROWITHDRAWCOST_AMOUNT", SqlDbType.Decimal);
   
                   foreach (cls_TRProwithdrawcost model in list_model)
                   {

                       obj_cmd.Parameters["@PROWITHDRAW_ID"].Value = model.prowithdraw_id;
                       obj_cmd.Parameters["@PROCOST_CODE"].Value = model.procost_code;
                       obj_cmd.Parameters["@PROWITHDRAWCOST_AMOUNT"].Value = model.prowithdrawcost_amount;
   

                       obj_cmd.ExecuteNonQuery();

                   }

                   blnResult = obj_conn.doCommit();
                   obj_conn.doClose();

                   blnResult = true;

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
