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
    public class cls_srvEmpImport
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
            catch (Exception ex)
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

                switch (type)
                {
                    case "LOCATION":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTLocation objWorker = new cls_ctMTLocation();
                                cls_MTLocation model = new cls_MTLocation();

                                model.location_code = dr["location_code"].ToString();
                                model.location_name_th = dr["location_name_th"].ToString();
                                model.location_name_en = dr["location_name_en"].ToString();
                                model.location_detail = dr["location_detail"].ToString();
                                model.modified_by = by;

                                string strID = objWorker.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.location_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "DEP":
                        DataTable dtdep = doReadExcel(filename);
                        if (dtdep.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtdep.Rows)
                            {

                                cls_ctMTDep objWorker = new cls_ctMTDep();
                                cls_MTDep model = new cls_MTDep();

                                model.dep_code = dr["dep_code"].ToString();
                                model.dep_name_th = dr["dep_name_th"].ToString();
                                model.dep_name_en = dr["dep_name_en"].ToString();
                                model.dep_parent = dr["dep_parent"].ToString();
                                model.dep_level = dr["dep_level"].ToString();
                                model.company_code = dr["company_code"].ToString();
                                model.modified_by = by;

                                string strID = objWorker.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.dep_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;

                    case "POSITION":
                        DataTable dtpo = doReadExcel(filename);
                        if (dtpo.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtpo.Rows)
                            {

                                cls_ctMTPosition objWorker = new cls_ctMTPosition();
                                cls_MTPosition model = new cls_MTPosition();

                                model.position_code = dr["position_code"].ToString();
                                model.position_name_th = dr["position_name_th"].ToString();
                                model.position_name_en = dr["position_name_en"].ToString();
                                model.company_code = dr["company_code"].ToString();
                                model.modified_by = by;

                                string strID = objWorker.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.position_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;

                    case "GROUP":
                        DataTable dtgr = doReadExcel(filename);
                        if (dtgr.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtgr.Rows)
                            {

                                cls_ctMTGroup objWorker = new cls_ctMTGroup();
                                cls_MTGroup model = new cls_MTGroup();

                                model.group_code = dr["group_code"].ToString();
                                model.group_name_th = dr["group_name_th"].ToString();
                                model.group_name_en = dr["group_name_en"].ToString();
                                model.company_code = dr["company_code"].ToString();
                                model.modified_by = by;

                                string strID = objWorker.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.group_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;

                    case "INITIAL":
                        DataTable dtin = doReadExcel(filename);
                        if (dtin.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtin.Rows)
                            {

                                cls_ctMTInitial objWorker = new cls_ctMTInitial();
                                cls_MTInitial model = new cls_MTInitial();

                                model.initial_code = dr["initial_code"].ToString();
                                model.initial_name_th = dr["initial_name_th"].ToString();
                                model.initial_name_en = dr["initial_name_en"].ToString();
                                model.modified_by = by;

                                string strID = objWorker.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.initial_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;

                    case "TYPE":
                        DataTable dtty = doReadExcel(filename);
                        if (dtty.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtty.Rows)
                            {

                                cls_ctMTType objWorker = new cls_ctMTType();
                                cls_MTType model = new cls_MTType();

                                model.type_code = dr["type_code"].ToString();
                                model.type_name_th = dr["type_name_th"].ToString();
                                model.type_name_en = dr["type_name_en"].ToString();
                                model.modified_by = by;

                                string strID = objWorker.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.type_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;

                    case "STATUS":
                        DataTable dtst = doReadExcel(filename);
                        if (dtst.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtst.Rows)
                            {

                                cls_ctMTStatus objWorker = new cls_ctMTStatus();
                                cls_MTStatus model = new cls_MTStatus();

                                model.status_code = dr["status_code"].ToString();
                                model.status_name_th = dr["status_name_th"].ToString();
                                model.status_name_en = dr["status_name_en"].ToString();
                                model.modified_by = by;

                                string strID = objWorker.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.status_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;

                    case "WORKER":

                        DataTable dtworker = doReadExcel(filename);
                        if (dtworker.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworker.Rows)
                            {

                                cls_ctMTWorker objWorker = new cls_ctMTWorker();
                                cls_MTWorker model = new cls_MTWorker();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();
                                model.worker_card = dr["worker_code"].ToString();
                                model.worker_initial = dr["worker_initial"].ToString();
                                model.worker_fname_th = dr["worker_fname_th"].ToString();
                                model.worker_lname_th = dr["worker_lname_th"].ToString();
                                model.worker_fname_en = dr["worker_fname_en"].ToString();
                                model.worker_lname_en = dr["worker_lname_en"].ToString();
                                model.worker_type = dr["worker_type"].ToString();
                                model.worker_gender = dr["worker_gender"].ToString();
                                model.worker_birthdate = Convert.ToDateTime(dr["worker_birthdate"]);
                                model.worker_hiredate = Convert.ToDateTime(dr["worker_hiredate"]);
                                model.worker_status = dr["worker_status"].ToString();
                                model.religion_code = dr["religion_code"].ToString();
                                model.blood_code = dr["blood_code"].ToString();
                                if (dr["worker_resignstatus"].ToString().Equals("1"))
                                {
                                    model.worker_resigndate = Convert.ToDateTime(dr["worker_resigndate"]);
                                    model.worker_resignstatus = true;
                                    model.worker_resignreason = dr["worker_resignreason"].ToString();
                                }
                                else
                                {
                                    model.worker_resignstatus = false;
                                }
                                model.worker_probationday = Convert.ToDouble(dr["worker_probationday"]);
                                model.worker_probationdate = Convert.ToDateTime(dr["worker_probationdate"]);
                                model.worker_probationenddate = Convert.ToDateTime(dr["worker_probationenddate"]);
                                model.hrs_perday = Convert.ToDouble(dr["hrs_perday"]);
                                model.worker_taxmethod = "1";
                                model.self_admin = false;

                                model.modified_by = by;

                                string strID = objWorker.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "EMPADDRESS":

                        DataTable dtworkeradd = doReadExcel(filename);
                        if (dtworkeradd.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworkeradd.Rows)
                            {

                                cls_ctTRAddress objAdd = new cls_ctTRAddress();
                                cls_TRAddress model = new cls_TRAddress();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();
                                model.address_type = dr["address_type"].ToString();
                                model.address_no = dr["address_no"].ToString();
                                model.address_moo = dr["address_moo"].ToString();
                                model.address_soi = dr["address_soi"].ToString();
                                model.address_road = dr["address_road"].ToString();
                                model.address_tambon = dr["address_tambon"].ToString();
                                model.address_amphur = dr["address_amphur"].ToString();
                                model.province_code = dr["province_code"].ToString();
                                model.address_zipcode = dr["address_zipcode"].ToString();
                                model.address_tel = dr["address_tel"].ToString();
                                model.address_email = dr["address_email"].ToString();
                                model.address_line = dr["address_line"].ToString();
                                model.address_facebook = dr["address_facebook"].ToString();

                                model.modified_by = by;

                                bool strID = objAdd.insert(model);

                                if (strID)
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "EMPCARD":

                        DataTable dtworkercard = doReadExcel(filename);
                        if (dtworkercard.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworkercard.Rows)
                            {

                                cls_ctTRCard objAdd = new cls_ctTRCard();
                                cls_TRCard model = new cls_TRCard();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();
                                model.card_code = dr["card_code"].ToString();
                                model.card_type = dr["card_type"].ToString();
                                model.card_issue = Convert.ToDateTime(dr["card_issue"]);
                                model.card_expire = Convert.ToDateTime(dr["card_expire"]);

                                model.modified_by = by;

                                bool strID = objAdd.insert(model);

                                if (strID)
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "EMPBANK":

                        DataTable dtworkerbank = doReadExcel(filename);
                        if (dtworkerbank.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworkerbank.Rows)
                            {

                                cls_ctTRBank objAdd = new cls_ctTRBank();
                                cls_TRBank model = new cls_TRBank();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.bank_code = dr["bank_code"].ToString();
                                model.bank_account = dr["bank_account"].ToString();
                                model.bank_percent = Convert.ToDouble(dr["bank_percent"]);
                                model.bank_cashpercent = Convert.ToDouble(dr["bank_cashpercent"]);
                                model.bank_bankname = dr["bank_bankname"].ToString();

                                model.modified_by = by;

                                bool strID = objAdd.insert(model);

                                if (strID)
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "EMPFAMILY":

                        DataTable dtworkerfam = doReadExcel(filename);
                        if (dtworkerfam.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworkerfam.Rows)
                            {

                                cls_ctTRFamily objAdd = new cls_ctTRFamily();
                                cls_TRFamily model = new cls_TRFamily();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.family_code = dr["family_code"].ToString();
                                model.family_type = dr["family_type"].ToString();
                                model.family_fname_th = dr["family_fname_th"].ToString();
                                model.family_lname_th = dr["family_lname_th"].ToString();
                                model.family_fname_en = dr["family_fname_en"].ToString();
                                model.family_lname_en = dr["family_lname_en"].ToString();
                                model.family_birthdate = Convert.ToDateTime(dr["family_birthdate"]);

                                model.modified_by = by;

                                bool strID = objAdd.insert(model);

                                if (strID)
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "EMPHOSPITAL":

                        DataTable dtworkerhosp = doReadExcel(filename);
                        if (dtworkerhosp.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworkerhosp.Rows)
                            {

                                cls_ctTRHospital objAdd = new cls_ctTRHospital();
                                cls_TRHospital model = new cls_TRHospital();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.emphospital_code = dr["emphospital_code"].ToString();
                                model.emphospital_date = Convert.ToDateTime(dr["emphospital_date"]);

                                model.modified_by = by;

                                bool strID = objAdd.insert(model);

                                if (strID)
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "EMPFOREIGNER":

                        DataTable dtworkerfore = doReadExcel(filename);
                        if (dtworkerfore.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworkerfore.Rows)
                            {

                                cls_ctTRForeigner objAdd = new cls_ctTRForeigner();
                                cls_TRForeigner model = new cls_TRForeigner();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.passport_no = dr["passport_no"].ToString();
                                model.passport_issue = Convert.ToDateTime(dr["passport_issue"]);
                                model.passport_expire = Convert.ToDateTime(dr["passport_expire"]);
                                model.visa_no = dr["visa_no"].ToString();
                                model.visa_issue = Convert.ToDateTime(dr["visa_issue"]);
                                model.visa_expire = Convert.ToDateTime(dr["visa_expire"]);
                                model.workpermit_no = dr["workpermit_no"].ToString();
                                model.workpermit_by = dr["workpermit_by"].ToString();
                                model.workpermit_issue = Convert.ToDateTime(dr["workpermit_issue"]);
                                model.workpermit_expire = Convert.ToDateTime(dr["workpermit_expire"]);
                                model.entry_date = Convert.ToDateTime(dr["entry_date"]);
                                model.certificate_no = dr["certificate_no"].ToString();
                                model.certificate_expire = Convert.ToDateTime(dr["certificate_expire"]);
                                if (!dr["otherdoc_no"].ToString().Equals(""))
                                {
                                    model.otherdoc_no = dr["otherdoc_no"].ToString();
                                    model.otherdoc_expire = Convert.ToDateTime(dr["otherdoc_expire"]);
                                }
                                else
                                {
                                    model.otherdoc_no = "";
                                }
                                

                                model.modified_by = by;

                                string strID = objAdd.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "EMPDEP":

                        DataTable dtworkerdep = doReadExcel(filename);
                        if (dtworkerdep.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworkerdep.Rows)
                            {

                                cls_ctTRDep objAdd = new cls_ctTRDep();
                                cls_TRDep model = new cls_TRDep();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.empdep_date = Convert.ToDateTime(dr["empdep_date"]);

                                model.empdep_level01 = dr["empdep_level01"].ToString();
                                model.empdep_level02 = dr["empdep_level02"].ToString();
                                model.empdep_level03 = dr["empdep_level03"].ToString();
                                model.empdep_level04 = dr["empdep_level04"].ToString();
                                model.empdep_level05 = dr["empdep_level05"].ToString();
                                model.empdep_level06 = dr["empdep_level06"].ToString();
                                model.empdep_level07 = dr["empdep_level07"].ToString();
                                model.empdep_level08 = dr["empdep_level08"].ToString();
                                model.empdep_level09 = dr["empdep_level09"].ToString();
                                model.empdep_level10 = dr["empdep_level10"].ToString();

                                model.empdep_reason = dr["empdep_reason"].ToString();

                                model.modified_by = by;

                                bool strID = objAdd.insert(model);

                                if (strID)
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "EMPPOSITION":

                        DataTable dtworkerpos = doReadExcel(filename);
                        if (dtworkerpos.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworkerpos.Rows)
                            {

                                cls_ctTRPosition objAdd = new cls_ctTRPosition();
                                cls_TRPosition model = new cls_TRPosition();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.empposition_date = Convert.ToDateTime(dr["empposition_date"]);
                                model.empposition_position = dr["empposition_position"].ToString();
                                model.empposition_reason = dr["empposition_reason"].ToString();

                                model.modified_by = by;

                                bool strID = objAdd.insert(model);

                                if (strID)
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "EMPEDUCATION":

                        DataTable dtworkeredu = doReadExcel(filename);
                        if (dtworkeredu.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworkeredu.Rows)
                            {

                                cls_ctTREducation objAdd = new cls_ctTREducation();
                                cls_TREducation model = new cls_TREducation();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.institute_code = dr["institute_code"].ToString();
                                model.faculty_code = dr["faculty_code"].ToString();
                                model.major_code = dr["major_code"].ToString();
                                model.qualification_code = dr["qualification_code"].ToString();
                                model.empeducation_gpa = dr["empeducation_gpa"].ToString();
                                model.empeducation_start = Convert.ToDateTime(dr["empeducation_start"]);
                                model.empeducation_finish = Convert.ToDateTime(dr["empeducation_finish"]);

                                model.modified_by = by;

                                bool strID = objAdd.insert(model);

                                if (strID)
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "EMPTRAINING":

                        DataTable dtworkertra = doReadExcel(filename);
                        if (dtworkertra.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworkertra.Rows)
                            {

                                cls_ctTRTraining objAdd = new cls_ctTRTraining();
                                cls_TRTraining model = new cls_TRTraining();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.institute_code = dr["institute_code"].ToString();
                                model.course_code = dr["course_code"].ToString();
                                model.institute_other = "";
                                model.course_other = "";

                                model.emptraining_start = Convert.ToDateTime(dr["emptraining_start"]);
                                model.emptraining_finish = Convert.ToDateTime(dr["emptraining_finish"]);

                                model.emptraining_status = dr["emptraining_status"].ToString();
                                model.emptraining_hours = Convert.ToDouble(dr["emptraining_hours"]);
                                model.emptraining_cost = Convert.ToDouble(dr["emptraining_cost"]);
                                model.emptraining_note = dr["emptraining_note"].ToString();

                                model.modified_by = by;

                                bool strID = objAdd.insert(model);

                                if (strID)
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "EMPASSESSMENT":

                        DataTable dtworkerass = doReadExcel(filename);
                        if (dtworkerass.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworkerass.Rows)
                            {

                                cls_ctTRAssessment objAdd = new cls_ctTRAssessment();
                                cls_TRAssessment model = new cls_TRAssessment();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.empassessment_location = dr["empassessment_location"].ToString();
                                model.empassessment_topic = dr["empassessment_topic"].ToString();
                                model.empassessment_fromdate = Convert.ToDateTime(dr["empassessment_fromdate"]);
                                model.empassessment_todate = Convert.ToDateTime(dr["empassessment_todate"]);
                                model.empassessment_count = Convert.ToDouble(dr["empassessment_count"]);
                                model.empassessment_result = dr["empassessment_result"].ToString();

                                model.modified_by = by;

                                bool strID = objAdd.insert(model);

                                if (strID)
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "EMPCRIMINAL":

                        DataTable dtworkercri = doReadExcel(filename);
                        if (dtworkercri.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworkercri.Rows)
                            {

                                cls_ctTRCriminal objAdd = new cls_ctTRCriminal();
                                cls_TRCriminal model = new cls_TRCriminal();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.empcriminal_location = dr["empcriminal_location"].ToString();
                                model.empcriminal_fromdate = Convert.ToDateTime(dr["empcriminal_fromdate"]);
                                model.empcriminal_todate = Convert.ToDateTime(dr["empcriminal_todate"]);
                                model.empcriminal_count = Convert.ToDouble(dr["empcriminal_count"]);
                                model.empcriminal_result = dr["empcriminal_result"].ToString();

                                model.modified_by = by;

                                bool strID = objAdd.insert(model);

                                if (strID)
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "EMPSALARY":

                        DataTable dtworkersala = doReadExcel(filename);
                        if (dtworkersala.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworkersala.Rows)
                            {

                                cls_ctTRSalary objAdd = new cls_ctTRSalary();
                                cls_TRSalary model = new cls_TRSalary();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.empsalary_amount = Convert.ToDouble(dr["empsalary_amount"]);
                                model.empsalary_date = Convert.ToDateTime(dr["empsalary_date"]);
                                model.empsalary_reason = dr["empsalary_reason"].ToString();

                                model.empsalary_incamount = Convert.ToDouble(dr["empsalary_incamount"]);
                                model.empsalary_incpercent = Convert.ToDouble(dr["empsalary_incpercent"]);

                                model.modified_by = by;

                                bool strID = objAdd.insert(model);

                                if (strID)
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "EMPPROVIDENT":

                        DataTable dtworkerprov = doReadExcel(filename);
                        if (dtworkerprov.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworkerprov.Rows)
                            {

                                cls_ctTRProvident objAdd = new cls_ctTRProvident();
                                cls_TRProvident model = new cls_TRProvident();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.provident_code = dr["provident_code"].ToString();
                                model.empprovident_card = dr["empprovident_card"].ToString();
                                model.empprovident_entry = Convert.ToDateTime(dr["empprovident_entry"]);
                                model.empprovident_start = Convert.ToDateTime(dr["empprovident_start"]);
                                if (!dr["empprovident_end"].ToString().Equals(""))
                                {
                                    model.empprovident_end = Convert.ToDateTime(dr["empprovident_end"]);
                                }
                                

                                model.modified_by = by;

                                bool strID = objAdd.insert(model);

                                if (strID)
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "EMPBENEFIT":

                        DataTable dtworkerbene = doReadExcel(filename);
                        if (dtworkerbene.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworkerbene.Rows)
                            {

                                cls_ctTRBenefit objAdd = new cls_ctTRBenefit();
                                cls_TRBenefit model = new cls_TRBenefit();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.item_code = dr["item_code"].ToString();
                                model.empbenefit_amount = Convert.ToDouble(dr["empbenefit_amount"]);
                                model.empbenefit_startdate = Convert.ToDateTime(dr["empbenefit_startdate"]);
                                model.empbenefit_enddate = Convert.ToDateTime(dr["empbenefit_enddate"]);
                                model.empbenefit_reason = dr["empbenefit_reason"].ToString();
                                model.empbenefit_note = dr["empbenefit_note"].ToString();
                                model.empbenefit_paytype = dr["empbenefit_paytype"].ToString();
                                //model.empbenefit_break = Convert.ToBoolean(dr["empbenefit_break"]);
                                if (dr["empbenefit_break"].ToString().Equals("1"))
                                {
                                    model.empbenefit_break = true;
                                    model.empbenefit_breakreason = dr["empbenefit_breakreason"].ToString();
                                }
                                else
                                {
                                    model.empbenefit_break = false;
                                }
                                model.empbenefit_conditionpay = dr["empbenefit_conditionpay"].ToString();
                                model.empbenefit_payfirst = dr["empbenefit_payfirst"].ToString();


                                model.modified_by = by;

                                bool strID = objAdd.insert(model);

                                if (strID)
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "EMPREDUCE":

                        DataTable dtworkerredu = doReadExcel(filename);
                        if (dtworkerredu.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworkerredu.Rows)
                            {

                                cls_ctTRReduce objAdd = new cls_ctTRReduce();
                                cls_TRReduce model = new cls_TRReduce();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.reduce_type = dr["reduce_type"].ToString();
                                model.empreduce_amount = Convert.ToDouble(dr["empreduce_amount"]);


                                model.modified_by = by;

                                bool strID = objAdd.insert(model);

                                if (strID)
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "EMPLOCATION":

                        DataTable dtworkerloca = doReadExcel(filename);
                        if (dtworkerloca.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworkerloca.Rows)
                            {

                                cls_ctTREmplocation objAdd = new cls_ctTREmplocation();
                                cls_TREmplocation model = new cls_TREmplocation();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.location_code = dr["location_code"].ToString();
                                model.emplocation_startdate = Convert.ToDateTime(dr["emplocation_startdate"]);
                                model.emplocation_enddate = Convert.ToDateTime(dr["emplocation_enddate"]);
                                model.emplocation_note = dr["emplocation_note"].ToString();


                                model.modified_by = by;

                                bool strID = objAdd.insert(model);

                                if (strID)
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "EMPGROUP":

                        DataTable dtworkergrou = doReadExcel(filename);
                        if (dtworkergrou.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworkergrou.Rows)
                            {

                                cls_ctTRGroup objAdd = new cls_ctTRGroup();
                                cls_TRGroup model = new cls_TRGroup();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.empgroup_code = dr["empgroup_code"].ToString();
                                model.empgroup_date = Convert.ToDateTime(dr["empgroup_date"]);


                                model.modified_by = by;

                                bool strID = objAdd.insert(model);

                                if (strID)
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "EMPBRANCH":

                        DataTable dtworkerbran = doReadExcel(filename);
                        if (dtworkerbran.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworkerbran.Rows)
                            {

                                cls_ctTREmpbranch objAdd = new cls_ctTREmpbranch();
                                cls_TREmpbranch model = new cls_TREmpbranch();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.branch_code = dr["branch_code"].ToString();
                                model.empbranch_startdate = Convert.ToDateTime(dr["empbranch_startdate"]);
                                model.empbranch_enddate = Convert.ToDateTime(dr["empbranch_enddate"]);
                                model.empbranch_note = dr["empbranch_note"].ToString();


                                model.modified_by = by;

                                bool strID = objAdd.insert(model);

                                if (strID)
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "EMPSUPPLY":

                        DataTable dtworkersupp = doReadExcel(filename);
                        if (dtworkersupp.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworkersupp.Rows)
                            {

                                cls_ctTRSupply objAdd = new cls_ctTRSupply();
                                cls_TRSupply model = new cls_TRSupply();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.empsupply_qauntity = Convert.ToDouble(dr["empsupply_qauntity"]);
                                model.empsupply_code = dr["empsupply_code"].ToString();
                                model.empsupply_issuedate = Convert.ToDateTime(dr["empsupply_issuedate"]);
                                model.empsupply_note = dr["empsupply_note"].ToString();
                                if (!dr["empsupply_returndate"].ToString().Equals(""))
                                {
                                    model.empsupply_returndate = Convert.ToDateTime(dr["empsupply_returndate"]);
                                }
                                if (dr["empsupply_returnstatus"].ToString().Equals("1"))
                                {
                                    model.empsupply_returnstatus = true;
                                }
                                else
                                {
                                    model.empsupply_returnstatus = false;
                                }
                                


                                model.modified_by = by;

                                bool strID = objAdd.insert(model);

                                if (strID)
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "EMPUNIFORM":

                        DataTable dtworkerunif = doReadExcel(filename);
                        if (dtworkerunif.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworkerunif.Rows)
                            {

                                cls_ctTRUniform objAdd = new cls_ctTRUniform();
                                cls_TRUniform model = new cls_TRUniform();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.empuniform_code = dr["empuniform_code"].ToString();
                                model.empuniform_qauntity = Convert.ToDouble(dr["empuniform_qauntity"]);
                                //model.empuniform_amount = Convert.ToDouble(dr["empuniform_amount"]);
                                model.empuniform_issuedate = Convert.ToDateTime(dr["empuniform_issuedate"]);
                                model.empuniform_note = dr["empuniform_note"].ToString();


                                model.modified_by = by;

                                bool strID = objAdd.insert(model);

                                if (strID)
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "EMPSUGGEST":

                        DataTable dtworkersugg = doReadExcel(filename);
                        if (dtworkersugg.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworkersugg.Rows)
                            {

                                cls_ctTRSuggest objAdd = new cls_ctTRSuggest();
                                cls_TRSuggest model = new cls_TRSuggest();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.empsuggest_code = dr["empsuggest_code"].ToString();
                                model.empsuggest_date = Convert.ToDateTime(dr["empsuggest_date"]);
                                model.empsuggest_note = dr["empsuggest_note"].ToString();


                                model.modified_by = by;

                                bool strID = objAdd.insert(model);

                                if (strID)
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
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
