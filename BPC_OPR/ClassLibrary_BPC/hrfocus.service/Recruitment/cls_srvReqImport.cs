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
    public class cls_srvReqImport
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

        public string doImportExcel(string type, string filename, string by, string com)
        {
            string strResult = "";

            try
            {

                int success = 0;
                StringBuilder objStr = new StringBuilder();

                switch (type)
                {
                    case "REQWORKER":

                        DataTable dtreqworker = doReadExcel(filename);
                        if (dtreqworker.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtreqworker.Rows)
                            {

                                cls_ctMTApplywork objReqworker = new cls_ctMTApplywork();
                                cls_MTWorker model = new cls_MTWorker();
                                if (!com.Equals(dr["company_code"].ToString()))
                                {
                                    continue;
                                }
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
                                if (!dr["worker_weight"].ToString().Equals(""))
                                {
                                    model.worker_weight = Convert.ToDouble(dr["worker_weight"]);
                                }
                                if(!dr["worker_height"].ToString().Equals("")){
                                    model.worker_height = Convert.ToDouble(dr["worker_height"]);
                                }
                                model.worker_tel = dr["worker_tel"].ToString();
                                model.worker_email = dr["worker_email"].ToString();
                                model.worker_line = dr["worker_line"].ToString();
                                model.worker_facebook = dr["worker_facebook"].ToString();
                                model.worker_military = dr["worker_military"].ToString();
                                model.nationality_code = dr["nationality_code"].ToString();

                                model.worker_cardno = dr["worker_cardno"].ToString();
                                model.worker_cardnoissuedate = Convert.ToDateTime(dr["worker_cardnoissuedate"]);
                                model.worker_cardnoexpiredate = Convert.ToDateTime(dr["worker_cardnoexpiredate"]);
                                model.modified_by = by;

                                string strID = objReqworker.insert(model);

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

                    case "REQADDRESS":

                        DataTable dtreqworkeradd = doReadExcel(filename);
                        if (dtreqworkeradd.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtreqworkeradd.Rows)
                            {

                                cls_ctTRApplyaddress objReqworker = new cls_ctTRApplyaddress();
                                cls_TRAddress model = new cls_TRAddress();
                                if (!com.Equals(dr["company_code"].ToString()))
                                {
                                    continue;
                                }
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

                                model.modified_by = by;

                                bool strID = objReqworker.insert(model);

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

                    case "REQCARD":

                        DataTable dtreqworkercard = doReadExcel(filename);
                        if (dtreqworkercard.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtreqworkercard.Rows)
                            {

                                cls_ctTRApplyCard objReqworker = new cls_ctTRApplyCard();
                                cls_TRCard model = new cls_TRCard();
                                if (!com.Equals(dr["company_code"].ToString()))
                                {
                                    continue;
                                }
                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();
                                model.card_code = dr["card_code"].ToString();
                                model.card_type = dr["card_type"].ToString();
                                model.card_issue = Convert.ToDateTime(dr["card_issue"]);
                                model.card_expire = Convert.ToDateTime(dr["card_expire"]);

                                model.modified_by = by;

                                bool strID = objReqworker.insert(model);

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

                    case "REQFOREIGNER":

                        DataTable dtreqworkerfor = doReadExcel(filename);
                        if (dtreqworkerfor.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtreqworkerfor.Rows)
                            {

                                cls_ctTRApplyforeigner objReqworker = new cls_ctTRApplyforeigner();
                                cls_TRForeigner model = new cls_TRForeigner();
                                if (!com.Equals(dr["company_code"].ToString()))
                                {
                                    continue;
                                }
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

                                string strID = objReqworker.insert(model);

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

                    case "REQEDUCATION":

                        DataTable dtreqworkeredu = doReadExcel(filename);
                        if (dtreqworkeredu.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtreqworkeredu.Rows)
                            {

                                cls_ctTRApplyeducation objReqworker = new cls_ctTRApplyeducation();
                                cls_TREducation model = new cls_TREducation();
                                if (!com.Equals(dr["company_code"].ToString()))
                                {
                                    continue;
                                }
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

                                bool strID = objReqworker.insert(model);

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

                    case "REQTRAINING":

                        DataTable dtreqworkertra = doReadExcel(filename);
                        if (dtreqworkertra.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtreqworkertra.Rows)
                            {

                                cls_ctTRApplytraining objReqworker = new cls_ctTRApplytraining();
                                cls_TRTraining model = new cls_TRTraining();
                                if (!com.Equals(dr["company_code"].ToString()))
                                {
                                    continue;
                                }
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

                                bool strID = objReqworker.insert(model);

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

                    case "REQASSESSMENT":

                        DataTable dtreqworkerass = doReadExcel(filename);
                        if (dtreqworkerass.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtreqworkerass.Rows)
                            {

                                cls_ctTRReqAssessment objReqworker = new cls_ctTRReqAssessment();
                                cls_TRAssessment model = new cls_TRAssessment();
                                if (!com.Equals(dr["company_code"].ToString()))
                                {
                                    continue;
                                }
                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.empassessment_location = dr["empassessment_location"].ToString();
                                model.empassessment_topic = dr["empassessment_topic"].ToString();
                                model.empassessment_fromdate = Convert.ToDateTime(dr["empassessment_fromdate"]);
                                model.empassessment_todate = Convert.ToDateTime(dr["empassessment_todate"]);
                                model.empassessment_count = Convert.ToDouble(dr["empassessment_count"]);
                                model.empassessment_result = dr["empassessment_result"].ToString();

                                model.modified_by = by;

                                bool strID = objReqworker.insert(model);

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

                    case "REQCRIMINAL":

                        DataTable dtreqworkercri = doReadExcel(filename);
                        if (dtreqworkercri.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtreqworkercri.Rows)
                            {

                                cls_ctTRReqCriminal objReqworker = new cls_ctTRReqCriminal();
                                cls_TRCriminal model = new cls_TRCriminal();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();
                                if (!com.Equals(dr["company_code"].ToString()))
                                {
                                    continue;
                                }
                                model.empcriminal_location = dr["empcriminal_location"].ToString();
                                model.empcriminal_fromdate = Convert.ToDateTime(dr["empcriminal_fromdate"]);
                                model.empcriminal_todate = Convert.ToDateTime(dr["empcriminal_todate"]);
                                model.empcriminal_count = Convert.ToDouble(dr["empcriminal_count"]);
                                model.empcriminal_result = dr["empcriminal_result"].ToString();

                                model.modified_by = by;

                                bool strID = objReqworker.insert(model);

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

                    case "REQSUGGEST":

                        DataTable dtreqworkersug = doReadExcel(filename);
                        if (dtreqworkersug.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtreqworkersug.Rows)
                            {

                                cls_ctTRApplySuggest objReqworker = new cls_ctTRApplySuggest();
                                cls_TRSuggest model = new cls_TRSuggest();
                                if (!com.Equals(dr["company_code"].ToString()))
                                {
                                    continue;
                                }
                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.empsuggest_code = dr["empsuggest_code"].ToString();
                                model.empsuggest_date = Convert.ToDateTime(dr["empsuggest_date"]);
                                model.empsuggest_note = dr["empsuggest_note"].ToString();

                                model.modified_by = by;

                                bool strID = objReqworker.insert(model);

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

                    case "BLACKLIST":

                        DataTable dtreqblacklist = doReadExcel(filename);
                        if (dtreqblacklist.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtreqblacklist.Rows)
                            {

                                cls_ctMTBlacklist objReqworker = new cls_ctMTBlacklist();
                                cls_MTBlacklist model = new cls_MTBlacklist();
                                if (!com.Equals(dr["company_code"].ToString()))
                                {
                                    continue;
                                }
                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.card_no = dr["card_no"].ToString();
                                model.blacklist_fname_th = dr["blacklist_fname_th"].ToString();
                                model.blacklist_lname_th = dr["blacklist_lname_th"].ToString();
                                model.blacklist_fname_en = dr["blacklist_fname_en"].ToString();
                                model.blacklist_lname_en = dr["blacklist_lname_en"].ToString();

                                model.reason_code = dr["reason_code"].ToString();

                                model.blacklist_note = dr["blacklist_note"].ToString();

                                model.modified_by = by;

                                
                                string strID = objReqworker.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.card_no);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "REQFOREIGNERCARD":

                        DataTable dtworkerforecard = doReadExcel(filename);
                        if (dtworkerforecard.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworkerforecard.Rows)
                            {

                                cls_ctTRApplyforeignercard objAdd = new cls_ctTRApplyforeignercard();
                                cls_TRForeignercard model = new cls_TRForeignercard();
                                if (!com.Equals(dr["company_code"].ToString()))
                                {
                                    continue;
                                }
                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();

                                model.foreignercard_code = dr["foreigner_code"].ToString();
                                model.foreignercard_type = dr["foreigner_type"].ToString();
                                model.foreignercard_issue = Convert.ToDateTime(dr["foreigner_issue"]).ToString("yyyy/MM/dd");
                                model.foreignercard_expire = Convert.ToDateTime(dr["foreigner_expire"]).ToString("yyyy/MM/dd");


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

                    case "REQREQUEST":

                        DataTable dtreqrequest = doReadExcel(filename);
                        if (dtreqrequest.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtreqrequest.Rows)
                            {

                                cls_ctMTReqRequest objReqworker = new cls_ctMTReqRequest();
                                cls_MTReqRequest model = new cls_MTReqRequest();
                                if (!com.Equals(dr["company_code"].ToString()))
                                {
                                    continue;
                                }
                                model.company_code = dr["company_code"].ToString();
                                model.request_code = dr["request_code"].ToString();

                                model.request_date = Convert.ToDateTime(dr["request_date"]);
                                model.request_startdate = Convert.ToDateTime(dr["request_startdate"]);
                                model.request_enddate = Convert.ToDateTime(dr["request_enddate"]);
                                model.request_position = dr["request_position"].ToString();
                                model.request_project = dr["request_project"].ToString();
                                model.request_employee_type = dr["request_employee_type"].ToString();
                                model.request_quantity = Convert.ToDouble(dr["request_quantity"].ToString());
                                model.request_urgency = dr["request_urgency"].ToString();
                                model.request_note = dr["request_note"].ToString();

                                model.request_accepted = 0;
                                model.request_status = '0';

                                model.modified_by = by;


                                string strID = objReqworker.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.request_code);
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
