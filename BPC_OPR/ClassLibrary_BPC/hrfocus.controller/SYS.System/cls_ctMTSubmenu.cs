using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;
namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTSubmenu
    {
        
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTSubmenu() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTSubmenu> getData(string condition)
        {
            List<cls_MTSubmenu> list_model = new List<cls_MTSubmenu>();
            cls_MTSubmenu model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("MAINMENU_CODE");
                obj_str.Append(", SUBMENU_CODE");
                obj_str.Append(", SUBMENU_DETAIL_TH");
                obj_str.Append(", SUBMENU_DETAIL_EN");
                obj_str.Append(", SUBMENU_ORDER");

                obj_str.Append(" FROM SYS_MT_SUBMENU");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY SUBMENU_ORDER");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTSubmenu();

                    model.mainmenu_code = dr["MAINMENU_CODE"].ToString();
                    model.submenu_code = dr["SUBMENU_CODE"].ToString();
                    model.submenu_detail_th = dr["SUBMENU_DETAIL_TH"].ToString();
                    model.submenu_detail_en = dr["SUBMENU_DETAIL_EN"].ToString();
                    model.submenu_order = Convert.ToInt32(dr["SUBMENU_ORDER"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Submenu.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTSubmenu> getDataByFillter(string main_code, string sub_code)
        {
            string strCondition = "";
            if (!main_code.Equals(""))
                strCondition += " AND MAINMENU_CODE='" + main_code + "'";

            if (!sub_code.Equals(""))
                strCondition += " AND SUBMENU_CODE='" + sub_code + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string main_code, string sub_code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT SUBMENU_CODE");
                obj_str.Append(" FROM SYS_MT_SUBMENU");
                obj_str.Append(" WHERE MAINMENU_CODE ='" + main_code + "' ");
                obj_str.Append(" AND SUBMENU_CODE='" + sub_code + "'");
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Submenu.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(string main_code, string sub_code)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SYS_MT_SUBMENU");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND MAINMENU_CODE='" + main_code + "'");
                obj_str.Append(" AND SUBMENU_CODE='" + sub_code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Submenu.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTSubmenu model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.mainmenu_code, model.submenu_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("INSERT INTO SYS_MT_SUBMENU");
                obj_str.Append(" (");
                obj_str.Append("MAINMENU_CODE ");
                obj_str.Append(", SUBMENU_CODE ");
                obj_str.Append(", SUBMENU_DETAIL_TH ");
                obj_str.Append(", SUBMENU_DETAIL_EN ");
                obj_str.Append(", SUBMENU_ORDER ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@MAINMENU_CODE ");
                obj_str.Append(", @SUBMENU_CODE ");
                obj_str.Append(", @SUBMENU_DETAIL_TH ");
                obj_str.Append(", @SUBMENU_DETAIL_EN ");
                obj_str.Append(", @SUBMENU_ORDER ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@MAINMENU_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@MAINMENU_CODE"].Value = model.mainmenu_code;
                obj_cmd.Parameters.Add("@SUBMENU_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@SUBMENU_CODE"].Value = model.submenu_code;
                obj_cmd.Parameters.Add("@SUBMENU_DETAIL_TH", SqlDbType.VarChar); obj_cmd.Parameters["@SUBMENU_DETAIL_TH"].Value = model.submenu_detail_th;
                obj_cmd.Parameters.Add("@SUBMENU_DETAIL_EN", SqlDbType.VarChar); obj_cmd.Parameters["@SUBMENU_DETAIL_EN"].Value = model.submenu_detail_en;
                obj_cmd.Parameters.Add("@SUBMENU_ORDER", SqlDbType.Int); obj_cmd.Parameters["@SUBMENU_ORDER"].Value = model.submenu_order;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = model.submenu_code;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Submenu.insert)" + ex.ToString();
            }

            return blnResult;
        }
        public bool insert(List<cls_MTSubmenu> list_model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_MT_SUBMENU");
                obj_str.Append(" (");
                obj_str.Append("MAINMENU_CODE ");
                obj_str.Append(", SUBMENU_CODE ");
                obj_str.Append(", SUBMENU_DETAIL_TH ");
                obj_str.Append(", SUBMENU_DETAIL_EN ");
                obj_str.Append(", SUBMENU_ORDER ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@MAINMENU_CODE ");
                obj_str.Append(", @SUBMENU_CODE ");
                obj_str.Append(", @SUBMENU_DETAIL_TH ");
                obj_str.Append(", @SUBMENU_DETAIL_EN ");
                obj_str.Append(", @SUBMENU_ORDER ");
                obj_str.Append(" )");


                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                //-- Step 1 delete data old
                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM SYS_MT_SUBMENU");
                obj_str2.Append(" WHERE 1=1 ");
                obj_str2.Append(" AND MAINMENU_CODE='" + list_model[0].mainmenu_code + "'");
                obj_str2.Append(" AND SUBMENU_CODE='" + list_model[0].submenu_code + "' ");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());
                if (blnResult)
                {
                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@MAINMENU_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@SUBMENU_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@SUBMENU_DETAIL_TH", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@SUBMENU_DETAIL_EN", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@SUBMENU_ORDER", SqlDbType.Int);
                    foreach (cls_MTSubmenu model in list_model)
                    {

                        obj_cmd.Parameters["@MAINMENU_CODE"].Value = model.mainmenu_code;
                        obj_cmd.Parameters["@SUBMENU_CODE"].Value = model.submenu_code;
                        obj_cmd.Parameters["@SUBMENU_DETAIL_TH"].Value = model.submenu_detail_th;
                        obj_cmd.Parameters["@SUBMENU_DETAIL_EN"].Value = model.submenu_detail_en;
                        obj_cmd.Parameters["@SUBMENU_ORDER"].Value = model.submenu_order;
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
                Message = "ERROR::(Submenu.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_MTSubmenu model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SYS_MT_SUBMENU SET ");
                obj_str.Append(" MAINMENU_CODE=@MAINMENU_CODE ");
                obj_str.Append(", SUBMENU_CODE=@SUBMENU_CODE ");
                obj_str.Append(", SUBMENU_DETAIL_TH=@SUBMENU_DETAIL_TH ");
                obj_str.Append(", SUBMENU_DETAIL_EN=@SUBMENU_DETAIL_EN ");
                obj_str.Append(", SUBMENU_ORDER=@SUBMENU_ORDER ");
                obj_str.Append(" WHERE MAINMENU_CODE=@MAINMENU_CODE ");
                obj_str.Append(" AND SUBMENU_CODE=@SUBMENU_CODE ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());


                obj_cmd.Parameters.Add("@MAINMENU_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@MAINMENU_CODE"].Value = model.mainmenu_code;
                obj_cmd.Parameters.Add("@SUBMENU_CODE", SqlDbType.Int); obj_cmd.Parameters["@SUBMENU_CODE"].Value = model.submenu_code;
                obj_cmd.Parameters.Add("@SUBMENU_DETAIL_TH", SqlDbType.VarChar); obj_cmd.Parameters["@SUBMENU_DETAIL_TH"].Value = model.submenu_detail_th;
                obj_cmd.Parameters.Add("@SUBMENU_DETAIL_EN", SqlDbType.VarChar); obj_cmd.Parameters["@SUBMENU_DETAIL_EN"].Value = model.submenu_detail_en;
                obj_cmd.Parameters.Add("@SUBMENU_ORDER", SqlDbType.Int); obj_cmd.Parameters["@SUBMENU_ORDER"].Value = model.submenu_order;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.submenu_code;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Submenu.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
