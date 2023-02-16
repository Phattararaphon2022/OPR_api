using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTShift
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTShift() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTShift> getData(string condition)
        {
            List<cls_MTShift> list_model = new List<cls_MTShift>();
            cls_MTShift model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("SHIFT_ID");
                obj_str.Append(", SHIFT_CODE");
                obj_str.Append(", ISNULL(SHIFT_NAME_TH, '') AS SHIFT_NAME_TH");
                obj_str.Append(", ISNULL(SHIFT_NAME_EN, '') AS SHIFT_NAME_EN");
                obj_str.Append(", COMPANY_CODE");

                obj_str.Append(", ISNULL(SHIFT_CH1, '00:00') AS SHIFT_CH1");
                obj_str.Append(", ISNULL(SHIFT_CH2, '00:00') AS SHIFT_CH2");
                obj_str.Append(", ISNULL(SHIFT_CH3, '00:00') AS SHIFT_CH3");
                obj_str.Append(", ISNULL(SHIFT_CH4, '00:00') AS SHIFT_CH4");
                obj_str.Append(", ISNULL(SHIFT_CH5, '00:00') AS SHIFT_CH5");
                obj_str.Append(", ISNULL(SHIFT_CH6, '00:00') AS SHIFT_CH6");
                obj_str.Append(", ISNULL(SHIFT_CH7, '00:00') AS SHIFT_CH7");
                obj_str.Append(", ISNULL(SHIFT_CH8, '00:00') AS SHIFT_CH8");
                obj_str.Append(", ISNULL(SHIFT_CH9, '00:00') AS SHIFT_CH9");
                obj_str.Append(", ISNULL(SHIFT_CH10, '00:00') AS SHIFT_CH10");

                obj_str.Append(", ISNULL(SHIFT_CH3_FROM, '00:00') AS SHIFT_CH3_FROM");
                obj_str.Append(", ISNULL(SHIFT_CH3_TO, '00:00') AS SHIFT_CH3_TO");
                obj_str.Append(", ISNULL(SHIFT_CH4_FROM, '00:00') AS SHIFT_CH4_FROM");
                obj_str.Append(", ISNULL(SHIFT_CH4_TO, '00:00') AS SHIFT_CH4_TO");

                obj_str.Append(", ISNULL(SHIFT_CH7_FROM, '00:00') AS SHIFT_CH7_FROM");
                obj_str.Append(", ISNULL(SHIFT_CH7_TO, '00:00') AS SHIFT_CH7_TO");
                obj_str.Append(", ISNULL(SHIFT_CH8_FROM, '00:00') AS SHIFT_CH8_FROM");
                obj_str.Append(", ISNULL(SHIFT_CH8_TO, '00:00') AS SHIFT_CH8_TO");

                obj_str.Append(", ISNULL(SHIFT_OTIN_MIN, 0) AS SHIFT_OTIN_MIN");
                obj_str.Append(", ISNULL(SHIFT_OTIN_MAX, 0) AS SHIFT_OTIN_MAX");

                obj_str.Append(", ISNULL(SHIFT_OTOUT_MIN, 0) AS SHIFT_OTOUT_MIN");
                obj_str.Append(", ISNULL(SHIFT_OTOUT_MAX, 0) AS SHIFT_OTOUT_MAX");

                obj_str.Append(", ISNULL(SHIFT_FLEXIBLEBREAK, 0) AS SHIFT_FLEXIBLEBREAK");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");

                obj_str.Append(" FROM ATT_MT_SHIFT");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY COMPANY_CODE, SHIFT_CODE");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTShift();

                    model.shift_id = Convert.ToInt32(dr["SHIFT_ID"]);
                    model.shift_code = dr["SHIFT_CODE"].ToString();
                    model.shift_name_th = dr["SHIFT_NAME_TH"].ToString();
                    model.shift_name_en = dr["SHIFT_NAME_EN"].ToString();

                    model.shift_ch1 = dr["SHIFT_CH1"].ToString();
                    model.shift_ch2 = dr["SHIFT_CH2"].ToString();
                    model.shift_ch3 = dr["SHIFT_CH3"].ToString();
                    model.shift_ch4 = dr["SHIFT_CH4"].ToString();
                    model.shift_ch5 = dr["SHIFT_CH5"].ToString();
                    model.shift_ch6 = dr["SHIFT_CH6"].ToString();
                    model.shift_ch7 = dr["SHIFT_CH7"].ToString();
                    model.shift_ch8 = dr["SHIFT_CH8"].ToString();
                    model.shift_ch9 = dr["SHIFT_CH9"].ToString();
                    model.shift_ch10 = dr["SHIFT_CH10"].ToString();

                    model.shift_ch3_from = dr["SHIFT_CH3_FROM"].ToString();
                    model.shift_ch3_to = dr["SHIFT_CH3_TO"].ToString();
                    model.shift_ch4_from = dr["SHIFT_CH4_FROM"].ToString();
                    model.shift_ch4_to = dr["SHIFT_CH4_TO"].ToString();

                    model.shift_ch7_from = dr["SHIFT_CH7_FROM"].ToString();
                    model.shift_ch7_to = dr["SHIFT_CH7_TO"].ToString();
                    model.shift_ch8_from = dr["SHIFT_CH8_FROM"].ToString();
                    model.shift_ch8_to = dr["SHIFT_CH8_TO"].ToString();

                    model.shift_otin_min = Convert.ToInt32(dr["SHIFT_OTIN_MIN"]);
                    model.shift_otin_max = Convert.ToInt32(dr["SHIFT_OTIN_MAX"]);

                    model.shift_otout_min = Convert.ToInt32(dr["SHIFT_OTOUT_MIN"]);
                    model.shift_otout_max = Convert.ToInt32(dr["SHIFT_OTOUT_MAX"]);

                    model.company_code = dr["COMPANY_CODE"].ToString();

                    model.shift_flexiblebreak = Convert.ToBoolean(dr["SHIFT_FLEXIBLEBREAK"]);

                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);

                    list_model.Add(model);
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Shift.getData)" + ex.ToString();
            }

            return list_model;
        }

        public List<cls_MTShift> getDataByFillter(string com, string id, string code)
        {
            string strCondition = "";

            strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!id.Equals(""))
                strCondition += " AND SHIFT_ID='" + id + "'";

            if (!code.Equals(""))
                strCondition += " AND SHIFT_CODE='" + code + "'";

            return this.getData(strCondition);
        }

        public bool checkDataOld(string com, string code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT SHIFT_ID");
                obj_str.Append(" FROM ATT_MT_SHIFT");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                obj_str.Append(" AND SHIFT_CODE='" + code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Shift.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }

        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(SHIFT_ID) ");
                obj_str.Append(" FROM ATT_MT_SHIFT");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Shift.getNextID)" + ex.ToString();
            }

            return intResult;
        }

        public bool delete(string id,string com)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM ATT_MT_SHIFT");
                obj_str.Append(" WHERE 1=1 ");
                obj_str.Append(" AND SHIFT_ID='" + id + "'");
                obj_str.Append(" AND COMPANY_CODE='" + com + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(Shift.delete)" + ex.ToString();
            }

            return blnResult;
        }

        public string insert(cls_MTShift model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code, model.shift_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO ATT_MT_SHIFT");
                obj_str.Append(" (");

                obj_str.Append("COMPANY_CODE ");

                obj_str.Append(", SHIFT_ID ");
                obj_str.Append(", SHIFT_CODE ");
                obj_str.Append(", SHIFT_NAME_TH ");
                obj_str.Append(", SHIFT_NAME_EN ");

                obj_str.Append(", SHIFT_CH1 ");
                obj_str.Append(", SHIFT_CH2 ");
                obj_str.Append(", SHIFT_CH3 ");
                obj_str.Append(", SHIFT_CH4 ");
                obj_str.Append(", SHIFT_CH5 ");
                obj_str.Append(", SHIFT_CH6 ");
                obj_str.Append(", SHIFT_CH7 ");
                obj_str.Append(", SHIFT_CH8 ");
                obj_str.Append(", SHIFT_CH9 ");
                obj_str.Append(", SHIFT_CH10 ");

                obj_str.Append(", SHIFT_CH3_FROM ");
                obj_str.Append(", SHIFT_CH3_TO ");
                obj_str.Append(", SHIFT_CH4_FROM ");
                obj_str.Append(", SHIFT_CH4_TO ");
                obj_str.Append(", SHIFT_CH7_FROM ");
                obj_str.Append(", SHIFT_CH7_TO ");
                obj_str.Append(", SHIFT_CH8_FROM ");
                obj_str.Append(", SHIFT_CH8_TO ");

                obj_str.Append(", SHIFT_OTIN_MIN ");
                obj_str.Append(", SHIFT_OTIN_MAX ");

                obj_str.Append(", SHIFT_OTOUT_MIN ");
                obj_str.Append(", SHIFT_OTOUT_MAX ");

                obj_str.Append(", SHIFT_FLEXIBLEBREAK ");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");

                obj_str.Append(", @SHIFT_ID ");
                obj_str.Append(", @SHIFT_CODE ");
                obj_str.Append(", @SHIFT_NAME_TH ");
                obj_str.Append(", @SHIFT_NAME_EN ");

                obj_str.Append(", @SHIFT_CH1 ");
                obj_str.Append(", @SHIFT_CH2 ");
                obj_str.Append(", @SHIFT_CH3 ");
                obj_str.Append(", @SHIFT_CH4 ");
                obj_str.Append(", @SHIFT_CH5 ");
                obj_str.Append(", @SHIFT_CH6 ");
                obj_str.Append(", @SHIFT_CH7 ");
                obj_str.Append(", @SHIFT_CH8 ");
                obj_str.Append(", @SHIFT_CH9 ");
                obj_str.Append(", @SHIFT_CH10 ");

                obj_str.Append(", @SHIFT_CH3_FROM ");
                obj_str.Append(", @SHIFT_CH3_TO ");
                obj_str.Append(", @SHIFT_CH4_FROM ");
                obj_str.Append(", @SHIFT_CH4_TO ");
                obj_str.Append(", @SHIFT_CH7_FROM ");
                obj_str.Append(", @SHIFT_CH7_TO ");
                obj_str.Append(", @SHIFT_CH8_FROM ");
                obj_str.Append(", @SHIFT_CH8_TO ");

                obj_str.Append(", @SHIFT_OTIN_MIN ");
                obj_str.Append(", @SHIFT_OTIN_MAX ");

                obj_str.Append(", @SHIFT_OTOUT_MIN ");
                obj_str.Append(", @SHIFT_OTOUT_MAX ");

                obj_str.Append(", @SHIFT_FLEXIBLEBREAK ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;

                obj_cmd.Parameters.Add("@SHIFT_ID", SqlDbType.Int); obj_cmd.Parameters["@SHIFT_ID"].Value = id;
                obj_cmd.Parameters.Add("@SHIFT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CODE"].Value = model.shift_code;
                obj_cmd.Parameters.Add("@SHIFT_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_NAME_TH"].Value = model.shift_name_th;
                obj_cmd.Parameters.Add("@SHIFT_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_NAME_EN"].Value = model.shift_name_en;

                obj_cmd.Parameters.Add("@SHIFT_CH1", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH1"].Value = model.shift_ch1;
                obj_cmd.Parameters.Add("@SHIFT_CH2", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH2"].Value = model.shift_ch2;
                obj_cmd.Parameters.Add("@SHIFT_CH3", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH3"].Value = model.shift_ch3;
                obj_cmd.Parameters.Add("@SHIFT_CH4", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH4"].Value = model.shift_ch4;
                obj_cmd.Parameters.Add("@SHIFT_CH5", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH5"].Value = model.shift_ch5;
                obj_cmd.Parameters.Add("@SHIFT_CH6", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH6"].Value = model.shift_ch6;
                obj_cmd.Parameters.Add("@SHIFT_CH7", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH7"].Value = model.shift_ch7;
                obj_cmd.Parameters.Add("@SHIFT_CH8", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH8"].Value = model.shift_ch8;
                obj_cmd.Parameters.Add("@SHIFT_CH9", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH9"].Value = model.shift_ch9;
                obj_cmd.Parameters.Add("@SHIFT_CH10", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH10"].Value = model.shift_ch10;

                obj_cmd.Parameters.Add("@SHIFT_CH3_FROM", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH3_FROM"].Value = model.shift_ch3_from;
                obj_cmd.Parameters.Add("@SHIFT_CH3_TO", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH3_TO"].Value = model.shift_ch3_to;

                obj_cmd.Parameters.Add("@SHIFT_CH4_FROM", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH4_FROM"].Value = model.shift_ch4_from;
                obj_cmd.Parameters.Add("@SHIFT_CH4_TO", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH4_TO"].Value = model.shift_ch4_to;

                obj_cmd.Parameters.Add("@SHIFT_CH7_FROM", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH7_FROM"].Value = model.shift_ch7_from;
                obj_cmd.Parameters.Add("@SHIFT_CH7_TO", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH7_TO"].Value = model.shift_ch7_to;

                obj_cmd.Parameters.Add("@SHIFT_CH8_FROM", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH8_FROM"].Value = model.shift_ch8_from;
                obj_cmd.Parameters.Add("@SHIFT_CH8_TO", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH8_TO"].Value = model.shift_ch8_to;

                obj_cmd.Parameters.Add("@SHIFT_OTIN_MIN", SqlDbType.Int); obj_cmd.Parameters["@SHIFT_OTIN_MIN"].Value = model.shift_otin_min;
                obj_cmd.Parameters.Add("@SHIFT_OTIN_MAX", SqlDbType.Int); obj_cmd.Parameters["@SHIFT_OTIN_MAX"].Value = model.shift_otin_max;

                obj_cmd.Parameters.Add("@SHIFT_OTOUT_MIN", SqlDbType.Int); obj_cmd.Parameters["@SHIFT_OTOUT_MIN"].Value = model.shift_otout_min;
                obj_cmd.Parameters.Add("@SHIFT_OTOUT_MAX", SqlDbType.Int); obj_cmd.Parameters["@SHIFT_OTOUT_MAX"].Value = model.shift_otout_max;

                obj_cmd.Parameters.Add("@SHIFT_FLEXIBLEBREAK", SqlDbType.Bit); obj_cmd.Parameters["@SHIFT_FLEXIBLEBREAK"].Value = model.shift_flexiblebreak;


                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Shift.insert)" + ex.ToString();
            }

            return blnResult;
        }

        public string update(cls_MTShift model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE ATT_MT_SHIFT SET ");

                obj_str.Append(" SHIFT_CODE=@SHIFT_CODE ");
                obj_str.Append(", SHIFT_NAME_TH=@SHIFT_NAME_TH ");
                obj_str.Append(", SHIFT_NAME_EN=@SHIFT_NAME_EN ");

                obj_str.Append(", SHIFT_Ch1=@SHIFT_Ch1 ");
                obj_str.Append(", SHIFT_Ch2=@SHIFT_Ch2 ");
                obj_str.Append(", SHIFT_Ch3=@SHIFT_Ch3 ");
                obj_str.Append(", SHIFT_Ch4=@SHIFT_Ch4 ");
                obj_str.Append(", SHIFT_Ch5=@SHIFT_Ch5 ");
                obj_str.Append(", SHIFT_Ch6=@SHIFT_Ch6 ");
                obj_str.Append(", SHIFT_Ch7=@SHIFT_Ch7 ");
                obj_str.Append(", SHIFT_Ch8=@SHIFT_Ch8 ");
                obj_str.Append(", SHIFT_Ch9=@SHIFT_Ch9 ");
                obj_str.Append(", SHIFT_Ch10=@SHIFT_Ch10 ");

                obj_str.Append(", SHIFT_CH3_FROM=@SHIFT_CH3_FROM ");
                obj_str.Append(", SHIFT_CH3_TO=@SHIFT_CH3_TO ");

                obj_str.Append(", SHIFT_CH4_FROM=@SHIFT_CH4_FROM ");
                obj_str.Append(", SHIFT_CH4_TO=@SHIFT_CH4_TO ");

                obj_str.Append(", SHIFT_CH7_FROM=@SHIFT_CH7_FROM ");
                obj_str.Append(", SHIFT_CH7_TO=@SHIFT_CH7_TO ");

                obj_str.Append(", SHIFT_CH8_FROM=@SHIFT_CH8_FROM ");
                obj_str.Append(", SHIFT_CH8_TO=@SHIFT_CH8_TO ");

                obj_str.Append(", SHIFT_OTIN_MIN=@SHIFT_OTIN_MIN ");
                obj_str.Append(", SHIFT_OTIN_MAX=@SHIFT_OTIN_MAX ");

                obj_str.Append(", SHIFT_OTOUT_MIN=@SHIFT_OTOUT_MIN ");
                obj_str.Append(", SHIFT_OTOUT_MAX=@SHIFT_OTOUT_MAX ");

                obj_str.Append(", SHIFT_FLEXIBLEBREAK=@SHIFT_FLEXIBLEBREAK ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");
                if (!model.shift_id.Equals(0))
                {
                obj_str.Append(" WHERE SHIFT_ID=@SHIFT_ID ");
                }
                else
                {
                    obj_str.Append(" WHERE SHIFT_CODE=@SHIFT_CODE ");
                    obj_str.Append(" AND COMPANY_CODE=@COMPANY_CODE ");
                }
                

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@SHIFT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CODE"].Value = model.shift_code;
                obj_cmd.Parameters.Add("@SHIFT_NAME_TH", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_NAME_TH"].Value = model.shift_name_th;
                obj_cmd.Parameters.Add("@SHIFT_NAME_EN", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_NAME_EN"].Value = model.shift_name_en;

                obj_cmd.Parameters.Add("@SHIFT_CH1", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH1"].Value = model.shift_ch1;
                obj_cmd.Parameters.Add("@SHIFT_CH2", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH2"].Value = model.shift_ch2;
                obj_cmd.Parameters.Add("@SHIFT_CH3", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH3"].Value = model.shift_ch3;
                obj_cmd.Parameters.Add("@SHIFT_CH4", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH4"].Value = model.shift_ch4;
                obj_cmd.Parameters.Add("@SHIFT_CH5", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH5"].Value = model.shift_ch5;
                obj_cmd.Parameters.Add("@SHIFT_CH6", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH6"].Value = model.shift_ch6;
                obj_cmd.Parameters.Add("@SHIFT_CH7", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH7"].Value = model.shift_ch7;
                obj_cmd.Parameters.Add("@SHIFT_CH8", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH8"].Value = model.shift_ch8;
                obj_cmd.Parameters.Add("@SHIFT_CH9", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH9"].Value = model.shift_ch9;
                obj_cmd.Parameters.Add("@SHIFT_CH10", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH10"].Value = model.shift_ch10;

                obj_cmd.Parameters.Add("@SHIFT_CH3_FROM", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH3_FROM"].Value = model.shift_ch3_from;
                obj_cmd.Parameters.Add("@SHIFT_CH3_TO", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH3_TO"].Value = model.shift_ch3_to;

                obj_cmd.Parameters.Add("@SHIFT_CH4_FROM", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH4_FROM"].Value = model.shift_ch4_from;
                obj_cmd.Parameters.Add("@SHIFT_CH4_TO", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH4_TO"].Value = model.shift_ch4_to;

                obj_cmd.Parameters.Add("@SHIFT_CH7_FROM", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH7_FROM"].Value = model.shift_ch7_from;
                obj_cmd.Parameters.Add("@SHIFT_CH7_TO", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH7_TO"].Value = model.shift_ch7_to;

                obj_cmd.Parameters.Add("@SHIFT_CH8_FROM", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH8_FROM"].Value = model.shift_ch8_from;
                obj_cmd.Parameters.Add("@SHIFT_CH8_TO", SqlDbType.VarChar); obj_cmd.Parameters["@SHIFT_CH8_TO"].Value = model.shift_ch8_to;

                obj_cmd.Parameters.Add("@SHIFT_OTIN_MIN", SqlDbType.Int); obj_cmd.Parameters["@SHIFT_OTIN_MIN"].Value = model.shift_otin_min;
                obj_cmd.Parameters.Add("@SHIFT_OTIN_MAX", SqlDbType.Int); obj_cmd.Parameters["@SHIFT_OTIN_MAX"].Value = model.shift_otin_max;

                obj_cmd.Parameters.Add("@SHIFT_OTOUT_MIN", SqlDbType.Int); obj_cmd.Parameters["@SHIFT_OTOUT_MIN"].Value = model.shift_otout_min;
                obj_cmd.Parameters.Add("@SHIFT_OTOUT_MAX", SqlDbType.Int); obj_cmd.Parameters["@SHIFT_OTOUT_MAX"].Value = model.shift_otout_max;

                obj_cmd.Parameters.Add("@SHIFT_FLEXIBLEBREAK", SqlDbType.Bit); obj_cmd.Parameters["@SHIFT_FLEXIBLEBREAK"].Value = model.shift_flexiblebreak;


                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.Parameters.Add("@SHIFT_ID", SqlDbType.Int); obj_cmd.Parameters["@SHIFT_ID"].Value = model.shift_id;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.shift_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(Shift.update)" + ex.ToString();
            }

            return blnResult;
        }
    }
}
