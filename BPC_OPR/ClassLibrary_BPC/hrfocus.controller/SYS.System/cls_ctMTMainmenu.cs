using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTMainmenu
    {
         string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTMainmenu() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTMainmenu> getData(string condition)
        {
            List<cls_MTMainmenu> list_model = new List<cls_MTMainmenu>();
            cls_MTMainmenu model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("MAINMENU_CODE");
                obj_str.Append(", MAINMENU_DETAIL_TH");
                obj_str.Append(", MAINMENU_DETAIL_EN");
                obj_str.Append(", MAINMENU_ORDER");

                obj_str.Append(" FROM SYS_MT_MAINMENU");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY MAINMENU_ORDER");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTMainmenu();

                    model.mainmenu_code = dr["MAINMENU_CODE"].ToString();
                    model.mainmenu_detail_th = dr["MAINMENU_DETAIL_TH"].ToString();
                    model.mainmenu_detail_en = dr["MAINMENU_DETAIL_EN"].ToString();
                    model.mainmenu_order = Convert.ToInt32(dr["MAINMENU_ORDER"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Mainmenu.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTMainmenu> getDataByFillter(string com)
        {
            string strCondition = "";
            if(!com.Equals(""))
                strCondition += " AND MAINMENU_CODE='" + com + "'";


            return this.getData(strCondition);
        }

        public bool checkDataOld(string com)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAINMENU_DETAIL_TH");
                obj_str.Append(" FROM SYS_MT_MAINMENU");
                obj_str.Append(" WHERE MAINMENU_CODE ='" + com + "' ");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Mainmenu.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(string com)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SYS_MT_MAINMENU");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND MAINMENU_CODE='" + com + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Mainmenu.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTMainmenu model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.mainmenu_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("INSERT INTO SYS_MT_MAINMENU");
                obj_str.Append(" (");
                obj_str.Append("MAINMENU_CODE ");
                obj_str.Append(", MAINMENU_DETAIL_TH ");
                obj_str.Append(", MAINMENU_DETAIL_EN ");
                obj_str.Append(", MAINMENU_ORDER ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@MAINMENU_CODE ");
                obj_str.Append(", @MAINMENU_DETAIL_TH ");
                obj_str.Append(", @MAINMENU_DETAIL_EN ");
                obj_str.Append(", @MAINMENU_ORDER ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@MAINMENU_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@MAINMENU_CODE"].Value = model.mainmenu_code;
                obj_cmd.Parameters.Add("@MAINMENU_DETAIL_TH", SqlDbType.VarChar); obj_cmd.Parameters["@MAINMENU_DETAIL_TH"].Value = model.mainmenu_detail_th;
                obj_cmd.Parameters.Add("@MAINMENU_DETAIL_EN", SqlDbType.VarChar); obj_cmd.Parameters["@MAINMENU_DETAIL_EN"].Value = model.mainmenu_detail_en;
                obj_cmd.Parameters.Add("@MAINMENU_ORDER", SqlDbType.Int); obj_cmd.Parameters["@MAINMENU_ORDER"].Value = model.mainmenu_order;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = model.mainmenu_code;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Mainmenu.insert)" + ex.ToString();
            }

            return blnResult;
        }
        public bool insert(List<cls_MTMainmenu> list_model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO SYS_MT_MAINMENU");
                obj_str.Append(" (");
                obj_str.Append("MAINMENU_CODE ");
                obj_str.Append(", MAINMENU_DETAIL_TH ");
                obj_str.Append(", MAINMENU_DETAIL_EN ");
                obj_str.Append(", MAINMENU_ORDER ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@MAINMENU_CODE ");
                obj_str.Append(", @MAINMENU_DETAIL_TH ");
                obj_str.Append(", @MAINMENU_DETAIL_EN ");
                obj_str.Append(", @MAINMENU_ORDER ");
                obj_str.Append(" )");


                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                //-- Step 1 delete data old
                System.Text.StringBuilder obj_str2 = new System.Text.StringBuilder();

                obj_str2.Append(" DELETE FROM SYS_MT_MAINMENU");
                obj_str2.Append(" WHERE 1=1 ");
                obj_str2.Append(" AND MAINMENU_CODE='" + list_model[0].mainmenu_code + "'");

                blnResult = obj_conn.doExecuteSQL_transaction(obj_str2.ToString());
                if (blnResult)
                {
                    SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                    obj_cmd.Transaction = obj_conn.getTransaction();

                    obj_cmd.Parameters.Add("@MAINMENU_CODE", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@MAINMENU_DETAIL_TH", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@MAINMENU_DETAIL_EN", SqlDbType.VarChar);
                    obj_cmd.Parameters.Add("@MAINMENU_ORDER", SqlDbType.Int);
                    foreach (cls_MTMainmenu model in list_model)
                    {

                        obj_cmd.Parameters["@MAINMENU_CODE"].Value = model.mainmenu_code;
                        obj_cmd.Parameters["@MAINMENU_DETAIL_TH"].Value = model.mainmenu_detail_th;
                        obj_cmd.Parameters["@MAINMENU_DETAIL_EN"].Value = model.mainmenu_detail_en;
                        obj_cmd.Parameters["@MAINMENU_ORDER"].Value = model.mainmenu_order;
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
                Message = "ERROR::(Mainmenu.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_MTMainmenu model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SYS_MT_MAINMENU SET ");
                obj_str.Append(" MAINMENU_CODE=@MAINMENU_CODE ");
                obj_str.Append(", MAINMENU_DETAIL_TH=@MAINMENU_DETAIL_TH ");
                obj_str.Append(", MAINMENU_DETAIL_EN=@MAINMENU_DETAIL_EN ");
                obj_str.Append(", MAINMENU_ORDER=@MAINMENU_ORDER ");
                obj_str.Append(" WHERE MAINMENU_CODE=@MAINMENU_CODE ");
      

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());


                obj_cmd.Parameters.Add("@MAINMENU_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@MAINMENU_CODE"].Value = model.mainmenu_code;
                obj_cmd.Parameters.Add("@MAINMENU_DETAIL_TH", SqlDbType.Int); obj_cmd.Parameters["@MAINMENU_DETAIL_TH"].Value = model.mainmenu_detail_th;
                obj_cmd.Parameters.Add("@MAINMENU_DETAIL_EN", SqlDbType.VarChar); obj_cmd.Parameters["@MAINMENU_DETAIL_EN"].Value = model.mainmenu_detail_en;
                obj_cmd.Parameters.Add("@MAINMENU_ORDER", SqlDbType.Int); obj_cmd.Parameters["@MAINMENU_ORDER"].Value = model.mainmenu_order;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.mainmenu_code;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Mainmenu.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
