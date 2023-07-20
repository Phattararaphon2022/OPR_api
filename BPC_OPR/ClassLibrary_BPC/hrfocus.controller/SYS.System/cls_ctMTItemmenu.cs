using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTItemmenu
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTItemmenu() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTItemmenu> getData(string condition)
        {
            List<cls_MTItemmenu> list_model = new List<cls_MTItemmenu>();
            cls_MTItemmenu model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("SUBMENU_CODE");
                obj_str.Append(", ITEMMENU_CODE");
                obj_str.Append(", ITEMMENU_DETAIL_TH");
                obj_str.Append(", ITEMMENU_DETAIL_EN");
                obj_str.Append(", ITEMMENU_ORDER");

                obj_str.Append(" FROM SYS_MT_ITEMMENU");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY ITEMMENU_ORDER");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTItemmenu();

                    model.submenu_code = dr["SUBMENU_CODE"].ToString();
                    model.itemmenu_code = dr["ITEMMENU_CODE"].ToString();
                    model.itemmenu_detail_th = dr["ITEMMENU_DETAIL_TH"].ToString();
                    model.itemmenu_detail_en = dr["ITEMMENU_DETAIL_EN"].ToString();
                    model.itemmenu_order = Convert.ToInt32(dr["ITEMMENU_ORDER"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Itemmenu.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTItemmenu> getDataByFillter(string sub, string item_code)
        {
            string strCondition = "";
            if (!sub.Equals(""))
                strCondition += " AND SUBMENU_CODE='" + sub + "'";

            if (!item_code.Equals(""))
                strCondition += " AND ITEMMENU_CODE='" + item_code + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string sub, string item_code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ITEMMENU_CODE");
                obj_str.Append(" FROM SYS_MT_ITEMMENU");
                obj_str.Append(" WHERE SUBMENU_CODE ='" + sub + "' ");
                obj_str.Append(" AND ITEMMENU_CODE='" + item_code + "'");
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Itemmenu.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(string sub, string item_code)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SYS_MT_ITEMMENU");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND SUBMENU_CODE='" + sub + "'");
                obj_str.Append(" AND ITEMMENU_CODE='" + item_code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Itemmenu.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTItemmenu model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.submenu_code, model.itemmenu_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("INSERT INTO SYS_MT_ITEMMENU");
                obj_str.Append(" (");
                obj_str.Append("SUBMENU_CODE ");
                obj_str.Append(", ITEMMENU_CODE ");
                obj_str.Append(", ITEMMENU_DETAIL_TH ");
                obj_str.Append(", ITEMMENU_DETAIL_EN ");
                obj_str.Append(", ITEMMENU_ORDER ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@SUBMENU_CODE ");
                obj_str.Append(", @ITEMMENU_CODE ");
                obj_str.Append(", @ITEMMENU_DETAIL_TH ");
                obj_str.Append(", @ITEMMENU_DETAIL_EN ");
                obj_str.Append(", @ITEMMENU_ORDER ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@SUBMENU_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@SUBMENU_CODE"].Value = model.submenu_code;
                obj_cmd.Parameters.Add("@ITEMMENU_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@ITEMMENU_CODE"].Value = model.itemmenu_code;
                obj_cmd.Parameters.Add("@ITEMMENU_DETAIL_TH", SqlDbType.VarChar); obj_cmd.Parameters["@ITEMMENU_DETAIL_TH"].Value = model.itemmenu_detail_th;
                obj_cmd.Parameters.Add("@ITEMMENU_DETAIL_EN", SqlDbType.VarChar); obj_cmd.Parameters["@ITEMMENU_DETAIL_EN"].Value = model.itemmenu_detail_en;
                obj_cmd.Parameters.Add("@ITEMMENU_ORDER", SqlDbType.Int); obj_cmd.Parameters["@ITEMMENU_ORDER"].Value = model.itemmenu_order;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = model.itemmenu_code;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Itemmenu.insert)" + ex.ToString();
            }

            return blnResult;
        }
        public bool insert(List<cls_MTItemmenu> list_model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_MT_ITEMMENU");
                obj_str.Append(" (");
                obj_str.Append("SUBMENU_CODE ");
                obj_str.Append(", ITEMMENU_CODE ");
                obj_str.Append(", ITEMMENU_DETAIL_TH ");
                obj_str.Append(", ITEMMENU_DETAIL_EN ");
                obj_str.Append(", ITEMMENU_ORDER ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@SUBMENU_CODE ");
                obj_str.Append(", @ITEMMENU_CODE ");
                obj_str.Append(", @ITEMMENU_DETAIL_TH ");
                obj_str.Append(", @ITEMMENU_DETAIL_EN ");
                obj_str.Append(", @ITEMMENU_ORDER ");
                obj_str.Append(" )");


                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                //-- Step 1 delete data old
                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM SYS_MT_ITEMMENU");
                obj_str2.Append(" WHERE 1=1 ");
                obj_str2.Append(" AND SUBMENU_CODE='" + list_model[0].submenu_code + "'");
                obj_str2.Append(" AND ITEMMENU_CODE='" + list_model[0].itemmenu_code + "' ");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());
                if (blnResult)
                {
                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@SUBMENU_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@ITEMMENU_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@ITEMMENU_DETAIL_TH", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@ITEMMENU_DETAIL_EN", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@ITEMMENU_ORDER", SqlDbType.Int);
                    foreach (cls_MTItemmenu model in list_model)
                    {

                        obj_cmd.Parameters["@SUBMENU_CODE"].Value = model.submenu_code;
                        obj_cmd.Parameters["@ITEMMENU_CODE"].Value = model.itemmenu_code;
                        obj_cmd.Parameters["@ITEMMENU_DETAIL_TH"].Value = model.itemmenu_detail_th;
                        obj_cmd.Parameters["@ITEMMENU_DETAIL_EN"].Value = model.itemmenu_detail_en;
                        obj_cmd.Parameters["@ITEMMENU_ORDER"].Value = model.itemmenu_order;
                        obj_cmd.ExecuteNonQuery();

                    }

                    blnResult = obj_conn.doCommit();

                    if (!blnResult)
                    {
                        obj_conn.doRollback();
                    }
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
                Message = "ERROR::(Itemmenu.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_MTItemmenu model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SYS_MT_ITEMMENU SET ");
                obj_str.Append(" SUBMENU_CODE=@SUBMENU_CODE ");
                obj_str.Append(", ITEMMENU_CODE=@ITEMMENU_CODE ");
                obj_str.Append(", ITEMMENU_DETAIL_TH=@ITEMMENU_DETAIL_TH ");
                obj_str.Append(", ITEMMENU_DETAIL_EN=@ITEMMENU_DETAIL_EN ");
                obj_str.Append(", ITEMMENU_ORDER=@ITEMMENU_ORDER ");
                obj_str.Append(" WHERE SUBMENU_CODE=@SUBMENU_CODE ");
                obj_str.Append(" AND ITEMMENU_CODE=@ITEMMENU_CODE ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());


                obj_cmd.Parameters.Add("@SUBMENU_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@SUBMENU_CODE"].Value = model.submenu_code;
                obj_cmd.Parameters.Add("@ITEMMENU_CODE", SqlDbType.Int); obj_cmd.Parameters["@ITEMMENU_CODE"].Value = model.itemmenu_code;
                obj_cmd.Parameters.Add("@ITEMMENU_DETAIL_TH", SqlDbType.VarChar); obj_cmd.Parameters["@ITEMMENU_DETAIL_TH"].Value = model.itemmenu_detail_th;
                obj_cmd.Parameters.Add("@ITEMMENU_DETAIL_EN", SqlDbType.VarChar); obj_cmd.Parameters["@ITEMMENU_DETAIL_EN"].Value = model.itemmenu_detail_en;
                obj_cmd.Parameters.Add("@ITEMMENU_ORDER", SqlDbType.Int); obj_cmd.Parameters["@ITEMMENU_ORDER"].Value = model.itemmenu_order;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.itemmenu_code;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Itemmenu.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
