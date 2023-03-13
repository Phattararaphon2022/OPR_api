using ClassLibrary_BPC.hrfocus.controller;
using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.service
{
    public class cls_srvSystemImport
    {
        public DataTable doReadExcel(string fileName)
        {
            DataTable dt = new DataTable();

            string filePath = Path.Combine(ClassLibrary_BPC.Config.PathFileImport + "\\Imports\\", fileName);
            string xlConnStr = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=Yes;';";
            var xlConn = new OleDbConnection(xlConnStr);

            try
            {

                var da = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", xlConn);
                da.Fill(dt);

            }
            catch (Exception)
            {

            }
            finally
            {
                xlConn.Close();
            }

            return dt;
        }

        public string doImportExcel(string type, string filename, string by)
        {
            string strResult = "";
            
            try
            {
              
                int success = 0;
                StringBuilder objStr = new StringBuilder();

                #region BANK
                switch (type)
                {
                    case "BANK":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                
                                cls_ctMTBank objWorker = new cls_ctMTBank();
                                cls_MTBank model = new cls_MTBank();
                                
                                model.bank_code = dr["bank_code"].ToString();
                                model.bank_name_th = dr["bank_name_th"].ToString();
                                model.bank_name_en = dr["bank_name_en"].ToString();
                                model.modified_by = by;
                                
                                string strID = objWorker.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.bank_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;
                }
                #endregion

                #region PERIOD //REASON
                switch (type)
                {
                    case "REASON":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTReason controller = new cls_ctMTReason();
                                cls_MTReason model = new cls_MTReason();

                                model.reason_id = dr["reason_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["reason_id"].ToString());
                                model.company_code = dr["company_code"].ToString();
                                model.reason_code = dr["reason_code"].ToString();
                                model.reason_name_th = dr["reason_name_th"].ToString();
                                model.reason_name_en = dr["reason_name_en"].ToString();
                                model.reason_group = dr["reason_group"].ToString();
                                model.modified_by = by;
                                model.flag = false;
                                string strID = controller.insert(model);
                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.reason_id + " " + model.reason_group);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;
                }
                #endregion

                #region //Level
                switch (type)
                {
                    case "LEVEL":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTLevel objReason = new cls_ctMTLevel();
                                cls_MTLevel model = new cls_MTLevel();

                                model.level_id = Convert.ToInt32(dr["LEVEL_ID"]);
                                model.level_code = dr["LEVEL_CODE"].ToString();
                                model.level_name_th = dr["LEVEL_NAME_TH"].ToString();
                                model.level_name_en = dr["LEVEL_NAME_EN"].ToString();
                                model.company_code = dr["COMPANY_CODE"].ToString();
                                model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);
                                model.modified_by = by;
                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.level_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;
                }
                #endregion

                #region //Fmily

                switch (type)
                {
                    case "FAMILY":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTFamily objReason = new cls_ctMTFamily();
                                cls_MTFamily model = new cls_MTFamily();
                                model = new cls_MTFamily();

                                model.family_id = Convert.ToInt32(dr["FAMILY_ID"]);
                                model.family_code = dr["FAMILY_CODE"].ToString();
                                model.family_name_th = dr["FAMILY_NAME_TH"].ToString();
                                model.family_name_en = dr["FAMILY_NAME_EN"].ToString();

                                model.modified_date = Convert.ToDateTime(dr["MODIFIED_DATE"]);   

                             
                                model.modified_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.family_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;
                }
                #endregion

                #region //Addresstype
                switch (type)
                {
                    case "ADDRESSTYPE":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTAddresstype objReason = new cls_ctMTAddresstype();
                                cls_MTAddresstype model = new cls_MTAddresstype();
                                model.addresstype_id = Convert.ToInt32(dr["ADDRESSTYPE_ID"]);
                                model.addresstype_code = dr["ADDRESSTYPE_CODE"].ToString();
                                model.addresstype_name_th = dr["ADDRESSTYPE_NAME_TH"].ToString();
                                model.addresstype_name_en = dr["ADDRESSTYPE_NAME_EN"].ToString();

                               
                                model.modified_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.addresstype_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;
                }
                #endregion

                #region//Ethnicity

                switch (type)
                {
                    case "ETHNICITY":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTEthnicity objReason = new cls_ctMTEthnicity();
                                cls_MTEthnicity model = new cls_MTEthnicity();

                                model.ethnicity_id = Convert.ToInt32(dr["ETHNICITY_ID"]);
                                model.ethnicity_code = dr["ETHNICITY_CODE"].ToString();
                                model.ethnicity_name_th = dr["ETHNICITY_NAME_TH"].ToString();
                                model.ethnicity_name_en = dr["ETHNICITY_NAME_EN"].ToString();         

                                
                                model.modified_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.ethnicity_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;
                }
                #endregion

                #region//Bloodtype
                switch (type)
                {
                    case "BLOODTYPE":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTBloodtype objReason = new cls_ctMTBloodtype();
                                cls_MTBloodtype model = new cls_MTBloodtype();

                                model.bloodtype_id = Convert.ToInt32(dr["BLOODTYPE_ID"]);
                                model.bloodtype_code = dr["BLOODTYPE_CODE"].ToString();
                                model.bloodtype_name_th = dr["BLOODTYPE_NAME_TH"].ToString();
                                model.bloodtype_name_en = dr["BLOODTYPE_NAME_EN"].ToString();               
                                model.modified_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.bloodtype_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;
                }
                #endregion

                #region //Hospital

                switch (type)
                {
                    case "HOSPITAL":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTHospital objReason = new cls_ctMTHospital();
                                cls_MTHospital model = new cls_MTHospital();

                                model.hospital_id = Convert.ToInt32(dr["HOSPITAL_ID"]);
                                model.hospital_code = dr["HOSPITAL_CODE"].ToString();
                                model.hospital_name_th = dr["HOSPITAL_NAME_TH"].ToString();
                                model.hospital_name_en = dr["HOSPITAL_NAME_EN"].ToString();               
                                model.modified_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.hospital_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;
                }
                #endregion

                #region //Location


                switch (type)
                {
                    case "LOCATION":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTLocation controller = new cls_ctMTLocation();
                                cls_MTLocation model = new cls_MTLocation();

                                model.company_code = dr["company_code"].ToString();
                                model.location_id = dr["location_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["location_id"].ToString());
                                model.location_code = dr["location_code"].ToString();
                                model.location_name_th = dr["location_name_th"].ToString();
                                model.location_name_en = dr["location_name_en"].ToString();
                                model.location_detail = dr["location_detail"].ToString();
                                model.location_lat = dr["location_lat"].ToString();
                                model.location_long = dr["location_long"].ToString();
                                model.modified_by = by;
                                model.flag = false;
                                string strID = controller.insert(model);
                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.location_id + " " + model.location_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;
                }
                #endregion

                #region  //Province
                switch (type)
                {
                    case "PROVINCE":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTProvince objReason = new cls_ctMTProvince();
                                cls_MTProvince model = new cls_MTProvince();

                                model.province_id = Convert.ToInt32(dr["PROVINCE_ID"]);
                                model.province_code = dr["PROVINCE_CODE"].ToString();
                                model.province_name_th = dr["PROVINCE_NAME_TH"].ToString();
                                model.province_name_en = dr["PROVINCE_NAME_EN"].ToString();
                                model.modified_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.province_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;
                }
                #endregion

                #region  //Religion
                switch (type)
                {
                    case "RELIGION":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTReligion objReason = new cls_ctMTReligion();
                                cls_MTReligion model = new cls_MTReligion();

                                model.religion_id = Convert.ToInt32(dr["RELIGION_ID"]);
                                model.religion_code = dr["RELIGION_CODE"].ToString();
                                model.religion_name_th = dr["RELIGION_NAME_TH"].ToString();
                                model.religion_name_en = dr["RELIGION_NAME_EN"].ToString();         
                                model.modified_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.religion_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;
                }
                #endregion

                #region //REDUCE
                switch (type)
                {
                    case "REDUCE":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTReduce objReason = new cls_ctMTReduce();
                                cls_MTReduce model = new cls_MTReduce();

                                model.reduce_id = Convert.ToInt32(dr["REDUCE_ID"]);
                                model.reduce_code = dr["REDUCE_CODE"].ToString();
                                model.reduce_name_th = dr["REDUCE_NAME_TH"].ToString();
                                model.reduce_name_en = dr["REDUCE_NAME_EN"].ToString();

                                model.reduce_amount = Convert.ToDouble(dr["REDUCE_AMOUNT"]);
                                model.reduce_percent = Convert.ToDouble(dr["REDUCE_PERCENT"]);
                                model.reduce_percent_max = Convert.ToDouble(dr["REDUCE_PERCENT_MAX"]);
                                model.modified_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.reduce_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;
                }
                #endregion

                #region //EmpID Structure
                switch (type)
                {
                    case "EmpID":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTEmpID objReason = new cls_ctMTEmpID();
                                cls_MTEmpID model = new cls_MTEmpID();

                                model.empid_id = Convert.ToInt32(dr["EMPID_ID"]);
                                model.empid_code = dr["EMPID_CODE"].ToString();
                                model.empid_name_th = dr["EMPID_NAME_TH"].ToString();
                                model.empid_name_en = dr["EMPID_NAME_EN"].ToString();             
                                model.modified_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.empid_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;
                }
                #endregion


                #region //cartype
                switch (type)
                {
                    case "CARTYPE":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTCardtype objCardtype = new cls_ctMTCardtype();
                                cls_MTCardtype model = new cls_MTCardtype();

                                model.cardtype_code = dr["cardtype_code"].ToString();
                                model.cardtype_name_th = dr["cardtype_name_th"].ToString();
                                model.cardtype_name_en = dr["cardtype_name_en"].ToString();
                                model.modified_by = by;

                                string strID = objCardtype.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.cardtype_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;
                }







            }
            catch (Exception ex)
            {
                strResult = ex.ToString();
            }
               
            return strResult;
        }
    }
}

                #endregion