using ClassLibrary_BPC.hrfocus.model.Attendance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.controller.Attendance
{
    public class cls_ctTREmpattitem
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctTREmpattitem() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_TREmpattitem> getData(string condition)
        {
            List<cls_TREmpattitem> list_model = new List<cls_TREmpattitem>();
            cls_TREmpattitem model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", WORKER_CODE");

                obj_str.Append(", ISNULL(EMPATTITEM_SA, '') AS EMPATTITEM_SA");
                obj_str.Append(", ISNULL(EMPATTITEM_OT, '') AS EMPATTITEM_OT");
                obj_str.Append(", ISNULL(EMPATTITEM_AW, '') AS EMPATTITEM_AW");
                obj_str.Append(", ISNULL(EMPATTITEM_DG, '') AS EMPATTITEM_DG");
                obj_str.Append(", ISNULL(EMPATTITEM_LV, '') AS EMPATTITEM_LV");
                obj_str.Append(", ISNULL(EMPATTITEM_AB, '') AS EMPATTITEM_AB");
                obj_str.Append(", ISNULL(EMPATTITEM_LT, '') AS EMPATTITEM_LT");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM ATT_TR_EMPATTITEM");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, WORKER_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_TREmpattitem();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.worker_code = dr["WORKER_CODE"].ToString();

                    model.empattitem_sa = dr["EMPATTITEM_SA"].ToString();
                    model.empattitem_ot = dr["EMPATTITEM_OT"].ToString();
                    model.empattitem_aw = dr["EMPATTITEM_AW"].ToString();
                    model.empattitem_dg = dr["EMPATTITEM_DG"].ToString();
                    model.empattitem_lv = dr["EMPATTITEM_LV"].ToString();
                    model.empattitem_ab = dr["EMPATTITEM_AB"].ToString();
                    model.empattitem_lt = dr["EMPATTITEM_LT"].ToString();

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(Empcard.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_TREmpattitem> getDataByFillter(string com, string emp)
        {
            string strCondition = " AND COMPANY_CODE='" + com + "'";

            if (!emp.Equals(""))
                strCondition += " AND WORKER_CODE='" + emp + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string emp)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT WORKER_CODE");
                obj_str.Append(" FROM ATT_TR_EMPATTITEM");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND WORKER_CODE='" + emp + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Empattitem.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public bool delete(string com, string emp)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_TR_EMPATTITEM");
                obj_str.Append(" WHERE COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND WORKER_CODE='" + emp + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Empattitem.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public bool insert(cls_TREmpattitem model)
        {
            bool blnResult = false;
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.worker_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("INSERT INTO ATT_TR_EMPATTITEM");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", WORKER_CODE ");
                obj_str.Append(", EMPATTITEM_SA ");
                obj_str.Append(", EMPATTITEM_OT ");
                obj_str.Append(", EMPATTITEM_AW ");
                obj_str.Append(", EMPATTITEM_DG ");
                obj_str.Append(", EMPATTITEM_LV ");
                obj_str.Append(", EMPATTITEM_AB ");
                obj_str.Append(", EMPATTITEM_LT ");
                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @WORKER_CODE ");
                obj_str.Append(", @EMPATTITEM_SA ");
                obj_str.Append(", @EMPATTITEM_OT ");
                obj_str.Append(", @EMPATTITEM_AW ");
                obj_str.Append(", @EMPATTITEM_DG ");
                obj_str.Append(", @EMPATTITEM_LV ");
                obj_str.Append(", @EMPATTITEM_AB ");
                obj_str.Append(", @EMPATTITEM_LT ");
                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.Parameters.Add("@EMPATTITEM_SA", SqlDbType.VarChar); obj_cmd.Parameters["@EMPATTITEM_SA"].Value = model.empattitem_sa;
                obj_cmd.Parameters.Add("@EMPATTITEM_OT", SqlDbType.VarChar); obj_cmd.Parameters["@EMPATTITEM_OT"].Value = model.empattitem_ot;
                obj_cmd.Parameters.Add("@EMPATTITEM_AW", SqlDbType.VarChar); obj_cmd.Parameters["@EMPATTITEM_AW"].Value = model.empattitem_aw;
                obj_cmd.Parameters.Add("@EMPATTITEM_DG", SqlDbType.VarChar); obj_cmd.Parameters["@EMPATTITEM_DG"].Value = model.empattitem_dg;
                obj_cmd.Parameters.Add("@EMPATTITEM_LV", SqlDbType.VarChar); obj_cmd.Parameters["@EMPATTITEM_LV"].Value = model.empattitem_lv;
                obj_cmd.Parameters.Add("@EMPATTITEM_AB", SqlDbType.VarChar); obj_cmd.Parameters["@EMPATTITEM_AB"].Value = model.empattitem_ab;
                obj_cmd.Parameters.Add("@EMPATTITEM_LT", SqlDbType.VarChar); obj_cmd.Parameters["@EMPATTITEM_LT"].Value = model.empattitem_lt;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = false;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Empattitem.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public bool update(cls_TREmpattitem model)
        {
            bool blnResult = false;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE ATT_TR_EMPATTITEM SET ");

                obj_str.Append(" MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");

                if (!model.empattitem_sa.Equals(""))
                    obj_str.Append(", EMPATTITEM_SA=@EMPATTITEM_SA ");

                if (!model.empattitem_ot.Equals(""))
                    obj_str.Append(", EMPATTITEM_OT=@EMPATTITEM_OT ");

                if (!model.empattitem_aw.Equals(""))
                    obj_str.Append(", EMPATTITEM_AW=@EMPATTITEM_AW ");

                if (!model.empattitem_dg.Equals(""))
                    obj_str.Append(", EMPATTITEM_DG=@EMPATTITEM_DG ");

                if (!model.empattitem_lv.Equals(""))
                    obj_str.Append(", EMPATTITEM_LV=@EMPATTITEM_LV ");

                if (!model.empattitem_ab.Equals(""))
                    obj_str.Append(", EMPATTITEM_AB=@EMPATTITEM_AB ");

                if (!model.empattitem_lt.Equals(""))
                    obj_str.Append(", EMPATTITEM_LT=@EMPATTITEM_LT ");




                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND WORKER_CODE=@WORKER_CODE ");


                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;


                if (!model.empattitem_sa.Equals(""))
                {
                    obj_cmd.Parameters.Add("@EMPATTITEM_SA", SqlDbType.VarChar); obj_cmd.Parameters["@EMPATTITEM_SA"].Value = model.empattitem_sa;
                }

                if (!model.empattitem_ot.Equals(""))
                {
                    obj_cmd.Parameters.Add("@EMPATTITEM_OT", SqlDbType.VarChar); obj_cmd.Parameters["@EMPATTITEM_OT"].Value = model.empattitem_ot;
                }

                if (!model.empattitem_aw.Equals(""))
                {
                    obj_cmd.Parameters.Add("@EMPATTITEM_AW", SqlDbType.VarChar); obj_cmd.Parameters["@EMPATTITEM_AW"].Value = model.empattitem_aw;
                }

                if (!model.empattitem_dg.Equals(""))
                {
                    obj_cmd.Parameters.Add("@EMPATTITEM_DG", SqlDbType.VarChar); obj_cmd.Parameters["@EMPATTITEM_DG"].Value = model.empattitem_dg;
                }

                if (!model.empattitem_lv.Equals(""))
                {
                    obj_cmd.Parameters.Add("@EMPATTITEM_LV", SqlDbType.VarChar); obj_cmd.Parameters["@EMPATTITEM_LV"].Value = model.empattitem_lv;
                }

                if (!model.empattitem_ab.Equals(""))
                {
                    obj_cmd.Parameters.Add("@EMPATTITEM_AB", SqlDbType.VarChar); obj_cmd.Parameters["@EMPATTITEM_AB"].Value = model.empattitem_ab;
                }

                if (!model.empattitem_lt.Equals(""))
                {
                    obj_cmd.Parameters.Add("@EMPATTITEM_LT", SqlDbType.VarChar); obj_cmd.Parameters["@EMPATTITEM_LT"].Value = model.empattitem_lt;
                }

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@WORKER_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@WORKER_CODE"].Value = model.worker_code;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = true;
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Empattitem.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
