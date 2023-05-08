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
   public class cls_ctTRRound
    {
   
     string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRRound() { }

        public string getMessage() { return this.Message.Replace("SYS_TR_ROUND", "").Replace("cls_ctTRRound", "").Replace("line", ""); }

        public void dispose()
        {
            Obj_conn.doClose();
        }


        private List<cls_TRRound> getData(string condition)
        {
            List<cls_TRRound> list_model = new List<cls_TRRound>();
            cls_TRRound model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("ROUND_ID");
                obj_str.Append(", ROUND_FROM");
                obj_str.Append(", ROUND_TO");
                obj_str.Append(", ROUND_RESULT");

                obj_str.Append(" FROM SYS_TR_ROUND");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY ROUND_ID, ROUND_FROM");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRRound();

                    model.round_id = dr["ROUND_ID"].ToString();

                    model.round_from = Convert.ToDouble(dr["ROUND_FROM"]);
                    model.round_to = Convert.ToDouble(dr["ROUND_TO"]);
                    model.round_result = Convert.ToDouble(dr["ROUND_RESULT"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Round.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRRound> getDataByFillter(string id)
        {
            string strCondition = " AND ROUND_ID='" + id + "'";

            return this.getData(strCondition);
        }

        public bool delete(string id)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SYS_TR_ROUND");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND ROUND_ID='" + id + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Round.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(cls_TRRound model)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SYS_TR_ROUND");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND ROUND_ID='" + model.round_id + "'");
                obj_str.Append(" AND ROUND_FROM='" + model.round_from + "'");
                obj_str.Append(" AND ROUND_TO='" + model.round_to + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Round.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(List<cls_TRRound> list_model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                if (!this.delete(list_model[0].round_id))
                {
                    return false;
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_TR_ROUND");
                obj_str.Append(" (");
                obj_str.Append("ROUND_ID ");
                obj_str.Append(", ROUND_FROM ");
                obj_str.Append(", ROUND_TO ");
                obj_str.Append(", ROUND_RESULT ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append(" @ROUND_ID ");
                obj_str.Append(", @ROUND_FROM ");
                obj_str.Append(", @ROUND_TO ");
                obj_str.Append(", @ROUND_RESULT ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                obj_cmd.Transaction = obj_conn.getTransaction();

                obj_cmd.Parameters.Add("@ROUND_ID", SqlDbType.Int);
                obj_cmd.Parameters.Add("@ROUND_FROM", SqlDbType.Decimal);
                obj_cmd.Parameters.Add("@ROUND_TO", SqlDbType.Decimal);
                obj_cmd.Parameters.Add("@ROUND_RESULT", SqlDbType.Decimal);


                foreach (cls_TRRound model in list_model)
                {

                    obj_cmd.Parameters["@ROUND_ID"].Value = model.round_id;
                    obj_cmd.Parameters["@ROUND_FROM"].Value = model.round_from;
                    obj_cmd.Parameters["@ROUND_TO"].Value = model.round_to;
                    obj_cmd.Parameters["@ROUND_RESULT"].Value = model.round_result;

                    obj_cmd.ExecuteNonQuery();

                }

                blnResult = obj_conn.doCommit();
                obj_conn.doClose();

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Round.insert)" + ex.ToString();
            }

            return blnResult;
        }


        public bool checkDataOld(string p)
        {
            throw new NotImplementedException();
        }
    }
}
