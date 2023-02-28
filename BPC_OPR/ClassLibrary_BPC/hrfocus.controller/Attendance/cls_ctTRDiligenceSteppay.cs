using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctTRDiligenceSteppay
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTRDiligenceSteppay() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TRDiligenceSteppay> getData(string condition)
        {
            List<cls_TRDiligenceSteppay> list_model = new List<cls_TRDiligenceSteppay>();
            cls_TRDiligenceSteppay model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", DILIGENCE_CODE");
                obj_str.Append(", STEPPAY_STEP");
                obj_str.Append(", STEPPAY_TYPE");
                obj_str.Append(", STEPPAY_AMOUNT");

                obj_str.Append(" FROM ATT_TR_DILIGENCESTEPPAY");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, DILIGENCE_CODE, STEPPAY_STEP");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TRDiligenceSteppay();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.diligence_code = dr["DILIGENCE_CODE"].ToString();
                    model.steppay_step = Convert.ToInt32(dr["STEPPAY_STEP"]);
                    model.steppay_type = dr["STEPPAY_TYPE"].ToString();
                    model.steppay_amount = Convert.ToDouble(dr["STEPPAY_AMOUNT"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRDiligenceSteppay.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TRDiligenceSteppay> getDataByFillter(string com, string code)
        {
            string strCondition = " AND COMPANY_CODE='" + com + "'";

            if (!code.Equals(""))
                strCondition += " AND DILIGENCE_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string code, int step)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT DILIGENCE_CODE");
                obj_str.Append(" FROM ATT_TR_DILIGENCESTEPPAY");
                obj_str.Append(" WHERE AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND DILIGENCE_CODE='" + code + "'");
                obj_str.Append(" AND STEPPAY_STEP='" + step + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRDiligenceSteppay.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }


        public bool delete(string com, string code, int step)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_DILIGENCESTEPPAY");
                obj_str.Append(" WHERE AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND DILIGENCE_CODE='" + code + "'");
                obj_str.Append(" AND STEPPAY_STEP='" + step + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(TRDiligenceSteppay.delete)" + ex.ToString();
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

                obj_str.Append(" DELETE FROM ATT_TR_DILIGENCESTEPPAY");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND DILIGENCE_CODE='" + code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(TRDiligenceSteppay.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(List<cls_TRDiligenceSteppay> list_model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                //if (!this.delete(list_model[0].company_code, list_model[0].DILIGENCE_CODE))
                //{
                //    return false;
                //}

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO ATT_TR_DILIGENCESTEPPAY");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", DILIGENCE_CODE ");
                obj_str.Append(", STEPPAY_STEP ");
                obj_str.Append(", STEPPAY_TYPE ");
                obj_str.Append(", STEPPAY_AMOUNT ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @DILIGENCE_CODE ");
                obj_str.Append(", @STEPPAY_STEP ");
                obj_str.Append(", @STEPPAY_TYPE ");
                obj_str.Append(", @STEPPAY_AMOUNT ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                obj_conn.doOpenTransaction();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                obj_cmd.Transaction = obj_conn.getTransaction();

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar);
                obj_cmd.Parameters.Add("@DILIGENCE_CODE", SqlDbType.VarChar);
                obj_cmd.Parameters.Add("@STEPPAY_STEP", SqlDbType.Int);
                obj_cmd.Parameters.Add("@STEPPAY_TYPE", SqlDbType.Char);
                obj_cmd.Parameters.Add("@STEPPAY_AMOUNT", SqlDbType.Decimal);


                foreach (cls_TRDiligenceSteppay model in list_model)
                {

                    obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                    obj_cmd.Parameters["@DILIGENCE_CODE"].Value = model.diligence_code;
                    obj_cmd.Parameters["@STEPPAY_STEP"].Value = model.steppay_step;
                    obj_cmd.Parameters["@STEPPAY_TYPE"].Value = model.steppay_type;
                    obj_cmd.Parameters["@STEPPAY_AMOUNT"].Value = model.steppay_amount;

                    obj_cmd.ExecuteNonQuery();

                }

                blnResult = obj_conn.doCommit();
                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(TRDiligenceSteppay.insert)" + ex.ToString();
            }

            return blnResult;
        }

    }
}
