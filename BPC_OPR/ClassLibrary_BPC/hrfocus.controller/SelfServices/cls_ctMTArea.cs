using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;

namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ctMTArea
    {
         string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ctMTArea() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }

        private List<cls_MTArea> getData(string condition)
        {
            List<cls_MTArea> list_model = new List<cls_MTArea>();
            cls_MTArea model;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT ");

                obj_str.Append("COMPANY_CODE");
                obj_str.Append(", AREA_ID");
                obj_str.Append(", AREA_LAT");
                obj_str.Append(", AREA_LONG");
                obj_str.Append(", AREA_DISTANCE");
                obj_str.Append(", LOCATION_CODE");
                obj_str.Append(", PROJECT_CODE");

                obj_str.Append(", ISNULL(MODIFIED_BY, CREATED_BY) AS MODIFIED_BY");
                obj_str.Append(", ISNULL(MODIFIED_DATE, CREATED_DATE) AS MODIFIED_DATE");
                obj_str.Append(", ISNULL(FLAG, 0) AS FLAG");

                obj_str.Append(" FROM SELF_MT_AREA");
                obj_str.Append(" WHERE 1=1");

                if (!condition.Equals(""))
                    obj_str.Append(" " + condition);

                obj_str.Append(" ORDER BY AREA_ID");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    model = new cls_MTArea();

                    model.company_code = dr["COMPANY_CODE"].ToString();
                    model.area_id = Convert.ToInt32(dr["AREA_ID"]);
                    model.area_lat = Convert.ToDouble(dr["AREA_LAT"]);
                    model.area_long = Convert.ToDouble(dr["AREA_LONG"]);
                    model.area_distance = Convert.ToDouble(dr["AREA_DISTANCE"]);
                    model.location_code = dr["LOCATION_CODE"].ToString();
                    model.project_code = dr["PROJECT_CODE"].ToString();
       
                    model.modified_by = dr["MODIFIED_BY"].ToString();
                    model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                    model.flag = Convert.ToBoolean(dr["FLAG"]);

                    list_model.Add(model);
                }

            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTArea.getData)" + ex.ToString();
            }

            return list_model;
        }
        public List<cls_MTArea> getDataByFillter(string com,int id,string location_code,string project_code)
        {
            string strCondition = "";
            if(!com.Equals(""))
                strCondition += " AND COMPANY_CODE='" + com + "'";

            if (!id.Equals(0))
                strCondition += " AND AREA_ID='" + id + "'";

            if (!location_code.Equals(""))
                strCondition += " AND LOCATION_CODE='" + location_code + "'";

            if (!project_code.Equals(""))
                strCondition += " AND PROJECT_CODE='" + project_code + "'";

            return this.getData(strCondition);
        }
        public bool checkDataOld(string com, int id,string location_code)
        {
            bool blnResult = false;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT AREA_ID");
                obj_str.Append(" FROM SELF_MT_AREA");
                obj_str.Append(" WHERE COMPANY_CODE ='" + com + "' ");
                if(!id.Equals(0))
                    obj_str.Append(" AND AREA_ID='" + id + "'");
                if (!location_code.Equals(""))
                    obj_str.Append(" AND LOCATION_CODE='" + location_code + "'");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    blnResult = true;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTArea.checkDataOld)" + ex.ToString();
            }

            return blnResult;
        }
        public bool delete(string com,int id,string location_code)
        {
            bool blnResult = true;
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append(" DELETE FROM SELF_MT_AREA");
                obj_str.Append(" WHERE 1=1 ");
                if (!com.Equals(""))
                    obj_str.Append(" AND COMPANY_CODE='" + com + "'");
                if (!id.Equals(0))
                    obj_str.Append(" AND AREA_ID='" + id + "'");
                if (!location_code.Equals(""))
                    obj_str.Append(" AND LOCATION_CODE='" + location_code + "'");

                blnResult = obj_conn.doExecuteSQL(obj_str.ToString());

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERROR::(MTArea.delete)" + ex.ToString();
            }

            return blnResult;
        }
        public int getNextID()
        {
            int intResult = 1;
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("SELECT MAX(AREA_ID) ");
                obj_str.Append(" FROM SELF_MT_AREA");

                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());

                if (dt.Rows.Count > 0)
                {
                    intResult = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTArea.getNextID)" + ex.ToString();
            }

            return intResult;
        }
        public string insert(cls_MTArea model)
        {
            string blnResult = "";
            try
            {
                //-- Check data old
                if (this.checkDataOld(model.company_code,model.area_id,model.location_code))
                {
                    return this.update(model);
                }

                cls_ctConnection obj_conn = new cls_ctConnection();
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                int id = this.getNextID();
                obj_str.Append("INSERT INTO SELF_MT_AREA");
                obj_str.Append(" (");
                obj_str.Append("COMPANY_CODE ");
                obj_str.Append(", AREA_ID");
                obj_str.Append(", AREA_LAT");
                obj_str.Append(", AREA_LONG");
                obj_str.Append(", AREA_DISTANCE");
                obj_str.Append(", LOCATION_CODE");
                obj_str.Append(", PROJECT_CODE");

                obj_str.Append(", CREATED_BY ");
                obj_str.Append(", CREATED_DATE ");
                obj_str.Append(", FLAG ");
                obj_str.Append(" )");

                obj_str.Append(" VALUES(");
                obj_str.Append("@COMPANY_CODE ");
                obj_str.Append(", @AREA_ID ");
                obj_str.Append(", @AREA_LAT ");
                obj_str.Append(", @AREA_LONG ");
                obj_str.Append(", @AREA_DISTANCE ");
                obj_str.Append(", @LOCATION_CODE ");
                obj_str.Append(", @PROJECT_CODE ");

                obj_str.Append(", @CREATED_BY ");
                obj_str.Append(", @CREATED_DATE ");
                obj_str.Append(", @FLAG ");
                obj_str.Append(" )");

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@AREA_ID", SqlDbType.Int); obj_cmd.Parameters["@AREA_ID"].Value = id; ;
                obj_cmd.Parameters.Add("@AREA_LAT", SqlDbType.Float); obj_cmd.Parameters["@AREA_LAT"].Value = model.area_lat;
                obj_cmd.Parameters.Add("@AREA_LONG", SqlDbType.Float); obj_cmd.Parameters["@AREA_LONG"].Value = model.area_long;
                obj_cmd.Parameters.Add("@AREA_DISTANCE", SqlDbType.Float); obj_cmd.Parameters["@AREA_DISTANCE"].Value = model.area_distance;
                obj_cmd.Parameters.Add("@LOCATION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@LOCATION_CODE"].Value = model.location_code;
                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;

                obj_cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@CREATED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@CREATED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@CREATED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;

                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();
                blnResult = id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTArea.insert)" + ex.ToString();
            }

            return blnResult;
        }
        public string update(cls_MTArea model)
        {
            string blnResult = "";
            try
            {
                cls_ctConnection obj_conn = new cls_ctConnection();

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_str.Append("UPDATE SELF_MT_AREA SET ");
                obj_str.Append(" COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(", AREA_LAT=@AREA_LAT ");
                obj_str.Append(", AREA_LONG=@AREA_LONG ");
                obj_str.Append(", AREA_DISTANCE=@AREA_DISTANCE ");
                obj_str.Append(", LOCATION_CODE=@LOCATION_CODE ");
                obj_str.Append(", PROJECT_CODE=@PROJECT_CODE ");

                obj_str.Append(", MODIFIED_BY=@MODIFIED_BY ");
                obj_str.Append(", MODIFIED_DATE=@MODIFIED_DATE ");
                obj_str.Append(", FLAG=@FLAG ");
                obj_str.Append(" WHERE COMPANY_CODE=@COMPANY_CODE ");
                obj_str.Append(" AND AREA_ID=@AREA_ID ");
                
      

                obj_conn.doConnect();

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());


                obj_cmd.Parameters.Add("@COMPANY_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@COMPANY_CODE"].Value = model.company_code;
                obj_cmd.Parameters.Add("@AREA_ID", SqlDbType.Int); obj_cmd.Parameters["@AREA_ID"].Value = model.area_id; ;
                obj_cmd.Parameters.Add("@AREA_LAT", SqlDbType.Float); obj_cmd.Parameters["@AREA_LAT"].Value = model.area_lat;
                obj_cmd.Parameters.Add("@AREA_LONG", SqlDbType.Float); obj_cmd.Parameters["@AREA_LONG"].Value = model.area_long;
                obj_cmd.Parameters.Add("@AREA_DISTANCE", SqlDbType.Float); obj_cmd.Parameters["@AREA_DISTANCE"].Value = model.area_distance;
                obj_cmd.Parameters.Add("@LOCATION_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@LOCATION_CODE"].Value = model.location_code;
                obj_cmd.Parameters.Add("@PROJECT_CODE", SqlDbType.VarChar); obj_cmd.Parameters["@PROJECT_CODE"].Value = model.project_code;

                obj_cmd.Parameters.Add("@MODIFIED_BY", SqlDbType.VarChar); obj_cmd.Parameters["@MODIFIED_BY"].Value = model.modified_by;
                obj_cmd.Parameters.Add("@MODIFIED_DATE", SqlDbType.DateTime); obj_cmd.Parameters["@MODIFIED_DATE"].Value = DateTime.Now;
                obj_cmd.Parameters.Add("@FLAG", SqlDbType.Bit); obj_cmd.Parameters["@FLAG"].Value = model.flag;
                obj_cmd.ExecuteNonQuery();

                obj_conn.doClose();

                blnResult = model.area_id.ToString();
            }
            catch (Exception ex)
            {
                Message = "ERROR::(MTArea.update)" + ex.ToString();
            }

            return blnResult;
        }

    }
}
