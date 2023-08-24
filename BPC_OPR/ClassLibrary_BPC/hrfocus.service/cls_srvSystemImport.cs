using ClassLibrary_BPC.hrfocus.controller;
using ClassLibrary_BPC.hrfocus.model;
using ClassLibrary_BPC.hrfocus.model.SYS.System;
using ClassLibrary_BPC.hrfocus.model.System;
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
                                model.created_by = by;

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
                                model.level_id = dr["level_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["level_id"].ToString());

                                //model.level_id = Convert.ToInt32(dr["level_id"]);
                                model.level_code = dr["level_code"].ToString();
                                model.level_name_th = dr["level_name_th"].ToString();
                                model.level_name_en = dr["level_name_en"].ToString();
                                model.company_code = dr["company_code"].ToString();
                                //model.modified_date = Convert.ToDateTime(dr["modified_date"]);
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
                                model.company_code = dr["company_code"].ToString();

                                model.family_id = dr["family_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["family_id"].ToString());
                                model.family_code = dr["family_code"].ToString();
                                model.family_name_th = dr["family_name_th"].ToString();
                                model.family_name_en = dr["family_name_en"].ToString();

                                model.created_by = by;
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
                                model.addresstype_id = dr["addresstype_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["addresstype_id"].ToString());

                                //model.addresstype_id = Convert.ToInt32(dr["addresstype_id"]);
                                model.addresstype_code = dr["addresstype_code"].ToString();
                                model.addresstype_name_th = dr["addresstype_name_th"].ToString();
                                model.addresstype_name_en = dr["addresstype_name_en"].ToString();

                                model.created_by = by;
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
                                model.ethnicity_id = dr["ethnicity_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["ethnicity_id"].ToString());

                                //model.ethnicity_id = Convert.ToInt32(dr["ETHNICITY_ID"]);
                                model.ethnicity_code = dr["ethnicity_code"].ToString();
                                model.ethnicity_name_th = dr["ethnicity_name_th"].ToString();
                                model.ethnicity_name_en = dr["ethnicity_name_en"].ToString();         

                                
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
                                model.bloodtype_id = dr["bloodtype_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["bloodtype_id"].ToString());

                                //model.bloodtype_id = Convert.ToInt32(dr["BLOODTYPE_ID"]);
                                model.bloodtype_code = dr["bloodtype_code"].ToString();
                                model.bloodtype_name_th = dr["bloodtype_name_th"].ToString();
                                model.bloodtype_name_en = dr["bloodtype_name_en"].ToString();               
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
                                model.hospital_id = dr["hospital_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["hospital_id"].ToString());

                                //model.hospital_id = Convert.ToInt32(dr["HOSPITAL_ID"]);
                                model.hospital_code = dr["hospital_code"].ToString();
                                model.hospital_name_th = dr["hospital_name_th"].ToString();
                                model.hospital_name_en = dr["hospital_name_en"].ToString();               
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
                                model.province_id = dr["province_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["province_id"].ToString());
                                model.province_code = dr["province_code"].ToString();
                                model.province_name_th = dr["province_name_th"].ToString();
                                model.province_name_en = dr["province_name_en"].ToString();
                                model.created_by = by;
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

                #region  //Course
                switch (type)
                {
                    case "COURSE":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTCcourse objCourse = new cls_ctMTCcourse();
                                cls_MTCcourse model = new cls_MTCcourse();
                                model.course_id = dr["course_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["course_id"].ToString());

                                //model.course_id = Convert.ToInt32(dr["COURSE_ID"]);                                
                                model.course_code = dr["course_code"].ToString();
                                model.course_name_th = dr["course_name_th"].ToString();
                                model.course_name_en = dr["course_name_en"].ToString();
                                model.created_by = by;

                                model.modified_by = by;

                                string strID = objCourse.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.course_code);
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

                #region  //Institute
                switch (type)
                {
                    case "Institute":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTInstitute objInstitute = new cls_ctMTInstitute();
                                cls_MTInstitute model = new cls_MTInstitute();
                                model.institute_id = dr["institute_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["institute_id"].ToString());

                                //model.institute_id = Convert.ToInt32(dr["institute_id"]);
                                model.institute_code = dr["institute_code"].ToString();
                                model.institute_name_th = dr["institute_name_th"].ToString();
                                model.institute_name_en = dr["institute_name_en"].ToString();
                                model.created_by = by;

                                model.modified_by = by;

                                string strID = objInstitute.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.institute_code);
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

                #region  //Faculty
                switch (type)
                {
                    case "FACULTY":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTFaculty objCourse = new cls_ctMTFaculty();
                                cls_MTFaculty model = new cls_MTFaculty();
                                model.faculty_id = dr["faculty_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["faculty_id"].ToString());

                                //model.faculty_id = Convert.ToInt32(dr["faculty_id"]);
                                model.faculty_code = dr["faculty_code"].ToString();
                                model.faculty_name_th = dr["faculty_name_th"].ToString();
                                model.faculty_name_en = dr["faculty_name_en"].ToString();
                                model.created_by = by;

                                model.modified_by = by;

                                string strID = objCourse.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.faculty_code);
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

                #region  //major
                switch (type)
                {
                    case "Major":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTMajorr objCourse = new cls_ctMTMajorr();
                                cls_MTMajorr model = new cls_MTMajorr();
                                model.major_id = dr["major_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["major_id"].ToString());

                                //model.faculty_id = Convert.ToInt32(dr["faculty_id"]);
                                model.major_code = dr["major_code"].ToString();
                                model.major_name_th = dr["major_name_th"].ToString();
                                model.major_name_en = dr["major_name_en"].ToString();
                                model.created_by = by;

                                model.modified_by = by;

                                string strID = objCourse.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.major_code);
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


                #region  //Qualification
                switch (type)
                {
                    case "Qualification":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTQualification objQualification = new cls_ctMTQualification();
                                cls_MTQualification model = new cls_MTQualification();
                                model.qualification_id = dr["qualification_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["qualification_id"].ToString());

                                //model.qualification_id = Convert.ToInt32(dr["qualification_id"]);
                                model.qualification_code = dr["qualification_code"].ToString();
                                model.qualification_name_th = dr["qualification_name_th"].ToString();
                                model.qualification_name_en = dr["qualification_name_en"].ToString();
                                model.created_by = by;

                                model.modified_by = by;

                                string strID = objQualification.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.qualification_code);
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

                #region  //COURSE
                switch (type)
                {
                    case "COURSE":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTCcourse objCourse = new cls_ctMTCcourse();
                                cls_MTCcourse model = new cls_MTCcourse();
                                model.course_id = dr["course_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["course_id"].ToString());

                                //model.course_id = Convert.ToInt32(dr["COURSE_ID"]);
                                model.course_code = dr["course_code"].ToString();
                                model.course_name_th = dr["course_name_th"].ToString();
                                model.course_name_en = dr["course_name_en"].ToString();
                                model.created_by = by;

                                model.modified_by = by;

                                string strID = objCourse.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.course_code);
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
                                model.religion_id = dr["religion_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["religion_id"].ToString());

                                //model.religion_id = Convert.religion_code(dr["RELIGION_ID"]);
                                model.religion_code = dr["religion_code"].ToString();
                                model.religion_name_th = dr["religion_name_th"].ToString();
                                model.religion_name_en = dr["religion_name_en"].ToString();         
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
                                model.reduce_id = dr["reduce_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["reduce_id"].ToString());

                                model.reduce_code = dr["reduce_code"].ToString();
                                model.reduce_name_th = dr["reduce_name_th"].ToString();
                                model.reduce_name_en = dr["reduce_name_en"].ToString();

                                model.reduce_amount = Convert.ToDouble(dr["reduce_amount"]);
                                model.reduce_amount_max = Convert.ToDouble(dr["reduce_amount_max"]);
                                model.reduce_percent = Convert.ToDouble(dr["reduce_percent"]);
                                model.reduce_percent_max = Convert.ToDouble(dr["reduce_percent_max"]);
                                model.modified_by = by;
                                model.created_by = by;

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

                #region //ROUND
                switch (type)
                {
                    case "ROUND":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTRounds objReason = new cls_ctMTRounds();
                                cls_MTRounds model = new cls_MTRounds();
                                model.round_id = dr["round_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["round_id"].ToString());

                                 model.round_code = dr["round_code"].ToString();
                                 model.round_name_th = dr["round_name_th"].ToString();
                                 model.round_name_en = dr["round_name_en"].ToString();
                                 model.round_group = dr["round_group"].ToString();

                                model.modified_by = by;
 
                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.round_code);
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

                #region //SUPPLY
                switch (type)
                {
                    case "SUPPLY":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTSupply objReason = new cls_ctMTSupply();
                                cls_MTSupply model = new cls_MTSupply();
                                model.supply_id = dr["supply_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["supply_id"].ToString());
                                model.supply_code = dr["supply_code"].ToString();
                                model.supply_name_th = dr["supply_name_th"].ToString();
                                model.supply_name_en = dr["supply_name_en"].ToString();

                                model.modified_by = by;
                                model.created_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.supply_code);
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


                #region //company
                switch (type)
                {
                    case "COMPANY":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTCompany objReason = new cls_ctMTCompany();
                                cls_MTCompany model = new cls_MTCompany();

                                 

                                model.company_id = dr["company_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["company_id"].ToString());
                                model.company_code = dr["company_code"].ToString();
                                //model.company_initials = dr["company_initials"].ToString();
                                model.company_name_th = dr["company_name_th"].ToString();
                                model.company_name_en = dr["company_name_en"].ToString();
                                model.sso_tax_no = dr["sso_tax_no"].ToString();
                                model.citizen_no = dr["citizen_no"].ToString();
                                model.provident_fund_no = dr["provident_fund_no"].ToString();

                                model.hrs_perday = Convert.ToDouble(dr["hrs_perday"]);
                                model.sso_com_rate = Convert.ToDouble(dr["sso_com_rate"]);
                                model.sso_emp_rate = Convert.ToDouble(dr["sso_emp_rate"]);
 
                                model.sso_security_no = dr["sso_security_no"].ToString();
                                model.sso_branch_no = dr["sso_branch_no"].ToString();

                                model.sso_min_wage = Convert.ToDouble(dr["sso_min_wage"]);
                                model.sso_max_wage = Convert.ToDouble(dr["sso_max_wage"]);



                                model.sso_min_age = Convert.ToInt32(dr["sso_min_age"]);
                                model.sso_max_age = Convert.ToInt32(dr["sso_max_age"]);
 

                      


                    


                                model.modified_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.company_code);
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

                #region //Branch
                switch (type)
                {
                    case "Combranch":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                cls_ctMTCombranch objReason = new cls_ctMTCombranch();
                                cls_MTCombranch model = new cls_MTCombranch();

                                model.company_code = dr["company_code"].ToString();

                                model.combranch_id = dr["combranch_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["combranch_id"].ToString());
                                model.sso_combranch_no = dr["sso_combranch_no"].ToString();

                                model.combranch_code = dr["combranch_code"].ToString();
                                model.combranch_name_th = dr["combranch_name_th"].ToString();
                                model.combranch_name_en = dr["combranch_name_en"].ToString();

 
                                model.modified_by = by;
                                model.created_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.combranch_code);
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


                #region //Year
                switch (type)
                {
                    case "YEAR":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTYear objYear = new cls_ctMTYear();
                                cls_MTYear model = new cls_MTYear();
                                model.company_code = dr["company_code"].ToString();
                                model.year_id = dr["year_id"].ToString();
                                model.year_code = dr["year_code"].ToString();
                                model.year_fromdate = Convert.ToDateTime(dr["year_fromdate"]);
                                model.year_todate = Convert.ToDateTime(dr["year_todate"]);
                                model.year_name_th = dr["year_name_th"].ToString();
                                model.year_name_en = dr["year_name_en"].ToString();
                                model.year_group = dr["year_group"].ToString();

                                //model.year_id = Convert.ToInt32(dr["year_id"]);
                                //model.year_code = dr["year_code"].ToString();
                                //model.year_name_th = dr["year_name_th"].ToString();
                                //model.year_name_en = dr["year_name_en"].ToString();
                                //model.company_code = dr["company_code"].ToString();

                                //model.year_fromdate = Convert.ToDateTime(dr["year_fromdate"]);

                                //model.year_todate = Convert.ToDateTime(dr["year_todate"]);
                                //model.year_group = dr["year_group"].ToString();
                                model.modified_by = by;
                                model.created_by = by;
                                string strID = objYear.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.year_code);
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


                #region //TRPolcode
                switch (type)
                {
                    case "TRPolcode":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctTRPolcode objReason = new cls_ctTRPolcode();
                                cls_TRPolcode model = new cls_TRPolcode();
                                model.polcode_id = dr["polcode_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["polcode_id"].ToString());

                                 model.codestructure_code = dr["codestructure_code"].ToString();
                                model.polcode_lenght = Convert.ToInt32(dr["polcode_lenght"]);
                                model.polcode_text = dr["polcode_text"].ToString();
                                model.polcode_order = Convert.ToInt32(dr["polcode_order"]);

                                 model.modified_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.codestructure_code);
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
                    case "CARDTYPE":

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