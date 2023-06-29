using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary_BPC.hrfocus.model;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
namespace ClassLibrary_BPC.hrfocus.controller
{
    public class cls_ApproveJob
    {
        string Message = string.Empty;

        cls_ctConnection Obj_conn = new cls_ctConnection();

        public cls_ApproveJob() { }

        public string getMessage() { return this.Message; }

        public void dispose()
        {
            Obj_conn.doClose();
        }
        public JArray getdocfile(string com, string job_id, string job_type)
        {
            cls_ctMTReqdocument objMTReqdoc = new cls_ctMTReqdocument();
            List<cls_MTReqdocument> listTRReqdoc = objMTReqdoc.getDataByFillter(com, 0, job_id, job_type);
            JArray arrayTRReqdoc = new JArray();
            if (listTRReqdoc.Count > 0)
            {
                int indexTRReqdoc = 1;

                foreach (cls_MTReqdocument modelTRReqdoc in listTRReqdoc)
                {
                    JObject jsonTRReqdoc = new JObject();
                    jsonTRReqdoc.Add("company_code", modelTRReqdoc.company_code);
                    jsonTRReqdoc.Add("document_id", modelTRReqdoc.document_id);
                    jsonTRReqdoc.Add("job_id", modelTRReqdoc.job_id);
                    jsonTRReqdoc.Add("job_type", modelTRReqdoc.job_type);
                    jsonTRReqdoc.Add("document_name", modelTRReqdoc.document_name);
                    jsonTRReqdoc.Add("document_type", modelTRReqdoc.document_type);
                    jsonTRReqdoc.Add("document_path", modelTRReqdoc.document_path);
                    jsonTRReqdoc.Add("created_by", modelTRReqdoc.created_by);
                    jsonTRReqdoc.Add("created_date", modelTRReqdoc.created_date);

                    jsonTRReqdoc.Add("index", indexTRReqdoc);


                    indexTRReqdoc++;

                    arrayTRReqdoc.Add(jsonTRReqdoc);
                }
                return arrayTRReqdoc;
            }
            else
            {
                return arrayTRReqdoc;
            }
        }

        public int getCountDoc(string com, string job_type, string username, string status, string fromdate, string todate)
        {
            int totalDoc = 0;
            SqlCommand cmd = new SqlCommand();
            Obj_conn.doConnect();
            cmd.Connection = Obj_conn.getConnection();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SELF_MT_APPROVEGETDOC]";
            //Passing the parameters
            cmd.Parameters.AddWithValue("@CompID", com);
            cmd.Parameters.AddWithValue("@JobType", job_type);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@fromDate", fromdate);
            cmd.Parameters.AddWithValue("@toDate", todate);
            /*The output parameters*/
            cmd.Parameters.Add("@doc_count", SqlDbType.Int);
            cmd.Parameters["@doc_count"].Direction = ParameterDirection.Output;
            /*End*/
            try
            {

                int i = cmd.ExecuteNonQuery();
                totalDoc = Convert.ToInt32(cmd.Parameters["@doc_count"].Value);
            }
            catch (Exception ex)
            {
                this.Message = ex.ToString();
            }
            finally
            {
                Obj_conn.doClose();
            }
            return totalDoc;
        }
        public JArray ApproveJob_get(string com, string job_type, string username,int status,string fromdate,string todate)
        {
            JArray result = new JArray();
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("DECLARE @count INT ");
                obj_str.Append("EXEC [dbo].[SELF_MT_APPROVEGETDOC] ");
                obj_str.Append("@CompID = '"+com+"'");
                obj_str.Append(", @JobType = '"+job_type+"'");
                obj_str.Append(", @Username = '" + username + "'");
                obj_str.Append(", @Status = '" + status + "'");
                obj_str.Append(", @fromDate = '" + fromdate + "'");
                obj_str.Append(", @toDate = '" + todate + "'");
                obj_str.Append(", @doc_count = @count OUTPUT");
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());
                int index = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    JObject json = new JObject();
                    if (job_type.Equals("LEA")){
                    json.Add("company_code",dr["COMPANY_CODE"].ToString());
                    json.Add("worker_code", dr["WORKER_CODE"].ToString());
                    json.Add("leave_code", dr["LEAVE_CODE"].ToString());

                    json.Add("worker_detail_th", dr["WORKER_DETAIL_TH"].ToString());
                    json.Add("leave_detail_th", dr["LEAVE_DETAIL_TH"].ToString());
                    json.Add("worker_detail_en", dr["WORKER_DETAIL_EN"].ToString());
                    json.Add("leave_detail_en", dr["LEAVE_DETAIL_EN"].ToString());

                    json.Add("timeleave_id", Convert.ToInt32(dr["TIMELEAVE_ID"]));
                    json.Add("timeleave_doc", dr["TIMELEAVE_DOC"].ToString());

                    json.Add("timeleave_fromdate", Convert.ToDateTime(dr["TIMELEAVE_FROMDATE"]));
                    json.Add("timeleave_todate", Convert.ToDateTime(dr["TIMELEAVE_TODATE"]));

                    json.Add("timeleave_type", dr["TIMELEAVE_TYPE"].ToString());
                    json.Add("timeleave_min", Convert.ToInt32(dr["TIMELEAVE_MIN"]));

                    json.Add("timeleave_actualday", Convert.ToInt32(dr["TIMELEAVE_ACTUALDAY"]));
                    json.Add("timeleave_incholiday", Convert.ToBoolean(dr["TIMELEAVE_INCHOLIDAY"]));
                    json.Add("timeleave_deduct", Convert.ToBoolean(dr["TIMELEAVE_DEDUCT"]));

                    json.Add("timeleave_note", dr["TIMELEAVE_NOTE"].ToString());
                    json.Add("reason_code", dr["REASON_CODE"].ToString());
                    json.Add("reason_th", dr["REASON_NAME_TH"].ToString());
                    json.Add("reason_en", dr["REASON_NAME_EN"].ToString());
                    json.Add("status", Convert.ToInt32(dr["STATUS"]));
                    json.Add("status_job", dr["STATUS_JOB"].ToString());
                    json.Add("jobtable_id", dr["JOBTABLE_ID"].ToString());

                    json.Add("modified_by", dr["MODIFIED_BY"].ToString());
                    json.Add("modified_date", Convert.ToDateTime(dr["MODIFIED_DATE"]));
                    json.Add("flag", false);
                    json.Add("reqdoc_data", this.getdocfile(dr["COMPANY_CODE"].ToString(), dr["TIMELEAVE_ID"].ToString(), job_type));
                    json.Add("index", index);

                    index++;

                    result.Add(json);
                    }
                    if (job_type.Equals("SHT"))
                    {
                        json.Add("company_code", dr["COMPANY_CODE"].ToString());
                        json.Add("worker_code", dr["WORKER_CODE"].ToString());

                        json.Add("worker_detail_th", dr["WORKER_DETAIL_TH"].ToString());
                        json.Add("worker_detail_en", dr["WORKER_DETAIL_EN"].ToString());

                        json.Add("timeshift_id", Convert.ToInt32(dr["TIMESHIFT_ID"]));
                        json.Add("timeshift_doc", dr["TIMESHIFT_DOC"].ToString());
                        json.Add("timeshift_workdate", Convert.ToDateTime(dr["TIMESHIFT_WORKDATE"]));
                        json.Add("timeshift_old", dr["TIMESHIFT_OLD"].ToString());
                        json.Add("shift_old_th", dr["SHIFT_OLD_TH"].ToString());
                        json.Add("shift_old_en", dr["SHIFT_OLD_EN"].ToString());
                        json.Add("timeshift_new", dr["TIMESHIFT_NEW"].ToString());
                        json.Add("shift_new_th", dr["SHIFT_NEW_TH"].ToString());
                        json.Add("shift_new_en", dr["SHIFT_NEW_EN"].ToString());
                        json.Add("timeshift_note", dr["TIMESHIFT_NOTE"].ToString());

                        json.Add("reason_code", dr["REASON_CODE"].ToString());
                        json.Add("reason_detail_th", dr["REASON_DETAIL_TH"].ToString());
                        json.Add("reason_detail_en", dr["REASON_DETAIL_EN"].ToString());

                        json.Add("status", Convert.ToInt32(dr["STATUS"]));
                        json.Add("status_job", dr["STATUS_JOB"].ToString());
                        json.Add("jobtable_id", dr["JOBTABLE_ID"].ToString());
                        json.Add("modified_by", dr["MODIFIED_BY"].ToString());
                        json.Add("modified_date", Convert.ToDateTime(dr["MODIFIED_DATE"]));
                        json.Add("flag", false);
                        json.Add("reqdoc_data", this.getdocfile(dr["COMPANY_CODE"].ToString(), dr["TIMESHIFT_ID"].ToString(), job_type));
                        json.Add("index", index);

                        index++;

                        result.Add(json);
                    }
                    if (job_type.Equals("OT"))
                    {
                        json.Add("company_code", dr["COMPANY_CODE"].ToString());
                        json.Add("worker_code", dr["WORKER_CODE"].ToString());

                        json.Add("worker_detail_th", dr["WORKER_DETAIL_TH"].ToString());
                        json.Add("worker_detail_en", dr["WORKER_DETAIL_EN"].ToString());

                        json.Add("timeot_id", Convert.ToInt32(dr["TIMEOT_ID"]));
                        json.Add("timeot_doc", dr["TIMEOT_DOC"].ToString());

                        json.Add("timeot_workdate", Convert.ToDateTime(dr["TIMEOT_WORKDATE"]));

                        json.Add("timeot_beforemin", Convert.ToInt32(dr["TIMEOT_BEFOREMIN"]));
                        json.Add("timeot_normalmin", Convert.ToInt32(dr["TIMEOT_NORMALMIN"]));
                        json.Add("timeot_breakmin", Convert.ToInt32(dr["TIMEOT_BREAK"]));
                        json.Add("timeot_aftermin", Convert.ToInt32(dr["TIMEOT_AFTERMIN"]));

                        int hrs = (Convert.ToInt32(dr["TIMEOT_BEFOREMIN"])) / 60;
                        int min = (Convert.ToInt32(dr["TIMEOT_BEFOREMIN"])) - (hrs * 60);
                        json.Add("timeot_beforemin_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));

                        hrs = (Convert.ToInt32(dr["TIMEOT_NORMALMIN"])) / 60;
                        min = (Convert.ToInt32(dr["TIMEOT_NORMALMIN"])) - (hrs * 60);
                        json.Add("timeot_normalmin_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));

                        hrs = (Convert.ToInt32(dr["TIMEOT_BREAK"])) / 60;
                        min = (Convert.ToInt32(dr["TIMEOT_BREAK"])) - (hrs * 60);
                        json.Add("timeot_break_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));

                        hrs = (Convert.ToInt32(dr["TIMEOT_AFTERMIN"])) / 60;
                        min = (Convert.ToInt32(dr["TIMEOT_AFTERMIN"])) - (hrs * 60);
                        json.Add("timeot_aftermin_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));


                        json.Add("timeot_note", dr["TIMEOT_NOTE"].ToString());
                        json.Add("location_code", dr["LOCATION_CODE"].ToString());
                        json.Add("location_name_th", dr["LOCATION_NAME_TH"].ToString());
                        json.Add("location_name_en", dr["LOCATION_NAME_EN"].ToString());
                        json.Add("reason_code", dr["REASON_CODE"].ToString());
                        json.Add("reason_name_th", dr["REASON_NAME_TH"].ToString());
                        json.Add("reason_name_en", dr["REASON_NAME_EN"].ToString());

                        json.Add("status", Convert.ToInt32(dr["STATUS"]));
                        json.Add("status_job", dr["STATUS_JOB"].ToString());
                        json.Add("jobtable_id", dr["JOBTABLE_ID"].ToString());
                        json.Add("modified_by", dr["MODIFIED_BY"].ToString());
                        json.Add("modified_date", Convert.ToDateTime(dr["MODIFIED_DATE"]));
                        json.Add("flag", false);
                        json.Add("reqdoc_data", this.getdocfile(dr["COMPANY_CODE"].ToString(), dr["TIMEOT_ID"].ToString(), job_type));
                        json.Add("index", index);

                        index++;

                        result.Add(json);
                    }
                    if (job_type.Equals("DAT"))
                    {
                        json.Add("company_code", dr["COMPANY_CODE"].ToString());
                        json.Add("worker_code", dr["WORKER_CODE"].ToString());
                        json.Add("worker_detail_en", dr["WORKER_DETAIL_EN"].ToString());
                        json.Add("worker_detail_th", dr["WORKER_DETAIL_TH"].ToString());
                        json.Add("timedaytype_id", Convert.ToInt32(dr["TIMEDAYTYPE_ID"]));
                        json.Add("timedaytype_doc", dr["TIMEDAYTYPE_DOC"].ToString());
                        json.Add("timedaytype_workdate", Convert.ToDateTime(dr["TIMEDAYTYPE_WORKDATE"]));
                        json.Add("timedaytype_old", dr["TIMEDAYTYPE_OLD"].ToString());
                        json.Add("timedaytype_new", dr["TIMEDAYTYPE_NEW"].ToString());
                        json.Add("timedaytype_note", dr["TIMEDAYTYPE_NOTE"].ToString());
                        json.Add("reason_code", dr["REASON_CODE"].ToString());
                        json.Add("reason_name_en", dr["REASON_NAME_EN"].ToString());
                        json.Add("reason_name_th", dr["REASON_NAME_TH"].ToString());

                        json.Add("status", Convert.ToInt32(dr["STATUS"]));
                        json.Add("status_job", dr["STATUS_JOB"].ToString());
                        json.Add("jobtable_id", dr["JOBTABLE_ID"].ToString());
                        json.Add("modified_by", dr["MODIFIED_BY"].ToString());
                        json.Add("modified_date", Convert.ToDateTime(dr["MODIFIED_DATE"]));
                        json.Add("flag", false);
                        json.Add("reqdoc_data", this.getdocfile(dr["COMPANY_CODE"].ToString(), dr["TIMEDAYTYPE_ID"].ToString(), job_type));
                        json.Add("index", index);

                        index++;

                        result.Add(json);
                    }
                    if (job_type.Equals("ONS"))
                    {
                        json.Add("company_code", dr["COMPANY_CODE"].ToString());
                        json.Add("timeonsite_id", Convert.ToInt32(dr["TIMEONSITE_ID"]));
                        json.Add("timeonsite_doc", dr["TIMEONSITE_DOC"].ToString());
                        json.Add("timeonsite_workdate", Convert.ToDateTime(dr["TIMEONSITE_WORKDATE"]));
                        json.Add("timeonsite_in", dr["TIMEONSITE_IN"].ToString());
                        json.Add("timeonsite_out", dr["TIMEONSITE_OUT"].ToString());
                        json.Add("timeonsite_note", dr["TIMEONSITE_NOTE"].ToString());
                        json.Add("reason_code", dr["REASON_CODE"].ToString());
                        json.Add("reason_name_en", dr["REASON_NAME_EN"].ToString());
                        json.Add("reason_name_th", dr["REASON_NAME_TH"].ToString());
                        json.Add("location_code", dr["LOCATION_CODE"].ToString());
                        json.Add("location_name_en", dr["LOCATION_NAME_EN"].ToString());
                        json.Add("location_name_th", dr["LOCATION_NAME_TH"].ToString());
                        json.Add("worker_code", dr["WORKER_CODE"].ToString());
                        json.Add("worker_detail_en", dr["WORKER_DETAIL_EN"].ToString());
                        json.Add("worker_detail_th", dr["WORKER_DETAIL_TH"].ToString());

                        json.Add("status", Convert.ToInt32(dr["STATUS"]));
                        json.Add("status_job", dr["STATUS_JOB"].ToString());
                        json.Add("jobtable_id", dr["JOBTABLE_ID"].ToString());
                        json.Add("modified_by", dr["MODIFIED_BY"].ToString());
                        json.Add("modified_date", Convert.ToDateTime(dr["MODIFIED_DATE"]));
                        json.Add("flag", false);
                        json.Add("reqdoc_data", this.getdocfile(dr["COMPANY_CODE"].ToString(), dr["TIMEONSITE_ID"].ToString(), job_type));
                        json.Add("index", index);

                        index++;

                        result.Add(json);
                    }
                    if (job_type.Equals("CI"))
                    {
                        json.Add("company_code", dr["COMPANY_CODE"].ToString());
                        json.Add("worker_code", dr["WORKER_CODE"].ToString());
                        json.Add("worker_detail_en", dr["WORKER_DETAIL_EN"].ToString());
                        json.Add("worker_detail_th", dr["WORKER_DETAIL_TH"].ToString());
                        json.Add("timecheckin_id", Convert.ToInt32(dr["TIMECHECKIN_ID"]));
                        json.Add("timecheckin_doc", dr["TIMECHECKIN_DOC"].ToString());
                        json.Add("timecheckin_workdate",  Convert.ToDateTime(dr["TIMECHECKIN_WORKDATE"]));
                        json.Add("timecheckin_time", dr["TIMECHECKIN_TIME"].ToString());
                        json.Add("timecheckin_type", dr["TIMECHECKIN_TYPE"].ToString());
                        json.Add("timecheckin_lat", Convert.ToDouble(dr["TIMECHECKIN_LAT"]));
                        json.Add("timecheckin_long", Convert.ToDouble(dr["TIMECHECKIN_LONG"]));
                        json.Add("timecheckin_note", dr["TIMECHECKIN_NOTE"].ToString());
                        json.Add("location_code", dr["LOCATION_CODE"].ToString());
                        json.Add("location_name_en", dr["LOCATION_NAME_EN"].ToString());
                        json.Add("location_name_th", dr["LOCATION_NAME_TH"].ToString());

                        json.Add("status", Convert.ToInt32(dr["STATUS"]));
                        json.Add("status_job", dr["STATUS_JOB"].ToString());
                        json.Add("jobtable_id", dr["JOBTABLE_ID"].ToString());
                        json.Add("modified_by", dr["MODIFIED_BY"].ToString());
                        json.Add("modified_date", Convert.ToDateTime(dr["MODIFIED_DATE"]));
                        json.Add("flag", false);
                        json.Add("reqdoc_data", this.getdocfile(dr["COMPANY_CODE"].ToString(), dr["TIMECHECKIN_ID"].ToString(), job_type));
                        json.Add("index", index);

                        index++;

                        result.Add(json);
                    }
                    if (job_type.Equals("REQ"))
                    {
                        json.Add("company_code", dr["COMPANY_CODE"].ToString());
                        json.Add("worker_code", dr["WORKER_CODE"].ToString());
                        json.Add("worker_detail_th", dr["WORKER_DETAIL_TH"].ToString());
                        json.Add("worker_detail_en", dr["WORKER_DETAIL_EN"].ToString());
                        json.Add("reqdoc_id", Convert.ToInt32(dr["REQDOC_ID"]));
                        json.Add("reqdoc_doc", dr["REQDOC_DOC"].ToString());
                        json.Add("reqdoc_date", Convert.ToDateTime(dr["REQDOC_DATE"]));
                        json.Add("reqdoc_note", dr["REQDOC_NOTE"].ToString());

                        json.Add("status", Convert.ToInt32(dr["STATUS"]));
                        json.Add("status_job", dr["STATUS_JOB"].ToString());
                        json.Add("jobtable_id", dr["JOBTABLE_ID"].ToString());
                        json.Add("modified_by", dr["MODIFIED_BY"].ToString());
                        json.Add("modified_date", Convert.ToDateTime(dr["MODIFIED_DATE"]));
                        json.Add("flag", false);
                        cls_ctTRReqempinfo objTRReqempinfo = new cls_ctTRReqempinfo();
                        List<cls_TRReqempinfo> listTRReqempinfo = objTRReqempinfo.getDataByFillter(Convert.ToInt32(dr["REQDOC_ID"]), 0, 0);
                        JArray arrayTRReqempinfo = new JArray();
                        if (listTRReqempinfo.Count > 0)
                        {
                            int indexTR = 1;

                            foreach (cls_TRReqempinfo modelTRReqempinfo in listTRReqempinfo)
                            {
                                JObject jsonTRReqempinfo = new JObject();
                                jsonTRReqempinfo.Add("reqdoc_id", modelTRReqempinfo.reqdoc_id);
                                jsonTRReqempinfo.Add("reqdocempinfo_no", modelTRReqempinfo.reqdocempinfo_no);
                                jsonTRReqempinfo.Add("topic_code", modelTRReqempinfo.topic_code);
                                jsonTRReqempinfo.Add("reqempinfo_detail", modelTRReqempinfo.reqempinfo_detail);

                                jsonTRReqempinfo.Add("index", indexTR);


                                indexTR++;

                                arrayTRReqempinfo.Add(jsonTRReqempinfo);
                            }
                            json.Add("reqempinfo_data", arrayTRReqempinfo);
                        }
                        else
                        {
                            json.Add("reqempinfo_data", arrayTRReqempinfo);
                        }
                        cls_ctTRReqdocatt objTRReqedocatt = new cls_ctTRReqdocatt();
                        List<cls_TRReqdocatt> listTRReqdocatt = objTRReqedocatt.getDataByFillter(Convert.ToInt32(dr["REQDOC_ID"]), 0, "", "");
                        JArray arrayTRReqdocatt = new JArray();
                        if (listTRReqdocatt.Count > 0)
                        {
                            int indexTR = 1;

                            foreach (cls_TRReqdocatt modelTRReqdocatt in listTRReqdocatt)
                            {
                                JObject jsonTRReqdocatt = new JObject();
                                jsonTRReqdocatt.Add("reqdoc_id", modelTRReqdocatt.reqdoc_id);
                                jsonTRReqdocatt.Add("reqdoc_att_no", modelTRReqdocatt.reqdoc_att_no);
                                jsonTRReqdocatt.Add("reqdoc_att_file_name", modelTRReqdocatt.reqdoc_att_file_name);
                                jsonTRReqdocatt.Add("reqdoc_att_file_type", modelTRReqdocatt.reqdoc_att_file_type);
                                jsonTRReqdocatt.Add("reqdoc_att_path", modelTRReqdocatt.reqdoc_att_path);
                                jsonTRReqdocatt.Add("created_by", modelTRReqdocatt.created_by);
                                jsonTRReqdocatt.Add("created_date", modelTRReqdocatt.created_date);

                                jsonTRReqdocatt.Add("index", indexTR);


                                indexTR++;

                                arrayTRReqdocatt.Add(jsonTRReqdocatt);
                            }
                            json.Add("reqdocatt_data", arrayTRReqdocatt);
                        }
                        else
                        {
                            json.Add("reqdocatt_data", arrayTRReqdocatt);
                        }
                        json.Add("index", index);

                        index++;

                        result.Add(json);
                    }
                }

                this.SendEmail(this.getDetailSendMail(com, "21", job_type, true, "EN"));
            }
            catch { }

            return result;
        }

        public string ApproveJob(ref bool Status, string com, string jobtable_Id, string job_type, string username, string statusapprove,string lang)
        {
            string result = "";
            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();
                obj_str.Append("DECLARE	@return_value int ");
                obj_str.Append("EXEC @return_value = [dbo].[SELF_MT_APPROVEDOC] ");
                obj_str.Append("@CompID = '" + com + "'");
                obj_str.Append(", @JobID = '" + jobtable_Id + "'");
                obj_str.Append(", @JobType = '" + job_type + "'");
                obj_str.Append(", @Username = '" + username + "'");
                obj_str.Append(", @ApproveStatus = '" + statusapprove + "' ");
                obj_str.Append(" SELECT	'Return Value' = @return_value");
                DataTable dt = Obj_conn.doGetTable(obj_str.ToString());
                if (dt.Rows.Count > 0)
                {
                    string tmp = dt.Rows[0][0].ToString();
                    Status = false;
                    switch (tmp)
                    {
                        case "101":
                            if(lang.Equals("TH"))
                                result = "เอกสารนี้มีการอนุมัติแล้ว/มีผู้อนุมัติในระดับนี้แล้ว";
                            else
                                result = "You have already Approve.";
                            break;
                        case "102":
                            if (lang.Equals("TH"))
                                result = "เอกสารนี้มีการอนุมัติแล้ว/มีผู้อนุมัติในระดับนี้แล้ว";
                            else
                                result = "This level " + jobtable_Id + " has been approved.";
                            break;
                        case "103":
                            if (lang.Equals("TH"))
                                result = "เอกสารนี้มีการอนุมัติแล้ว";
                            else
                                result = "The document has been approved.";
                            break;
                        case "104":
                            if (lang.Equals("TH"))
                                result = "ต้องผ่านการอนุมัติจากระดับก่อนหน้าก่อนค่ะ";
                            else
                                result = "Must have permission from the previous level first.";
                            break;
                        case "111":
                            if (lang.Equals("TH"))
                                result = "อนุมัติเอกสารเรียบร้อยแล้วค่ะ";
                            else
                                result = "Approve complete!";
                            Status = true;
                            break;
                        case "222":
                            if (lang.Equals("TH"))
                                result = "อนุมัติเอกสารเรียบร้อยแล้วค่ะ";
                            else
                                result = "Approve complete!";
                            Status = true;

                            //-- Success doc
                            //sendMailAlert(UserRole.CompanyID, vJobID, ApproveType);
                            break;

                        case "999":
                            if (lang.Equals("TH"))
                                result = "พบปัญหาในการอนุมัติเอกสาร";
                            else
                                result = "Approve not success!";
                            break;
                    }

                }

            }
            catch { }

            return result;
        }
        public  void SendEmail(string Body)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("opr.test@outlook.com");
            message.To.Add("phattaraphon.p@bhatarapro.com");
            message.IsBodyHtml = true;
            message.Body = Body;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.UseDefaultCredentials = true;

            smtpClient.Host = "smtp.office365.com";
            smtpClient.Port = Convert.ToInt32("587");
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new System.Net.NetworkCredential("opr.test@outlook.com", "OPR@2023");
            smtpClient.Send(message);
        }
        private string getDetailSendMail(string com, string job_id, string job_type, bool Approve, string _language)
        {
            string strResult = string.Empty;
            try
            {
                string strHTML = string.Empty;
                string strSubject = string.Empty;
                string JobType = "-";
                string strDescription = "-";
                string Doc = "-";
                if (job_type.Equals("LEA"))
                {
                    cls_ctMTJobtable controller1 = new cls_ctMTJobtable();
                    cls_ctTRTimeleave controller2 = new cls_ctTRTimeleave();
                    List<cls_MTJobtable> listjob =  controller1.getDataByFillter(com,Convert.ToInt32(job_id),"",job_type,"","","","");
                    List<cls_TRTimeleave> listdoc = controller2.getDataByFillter(Convert.ToInt32(listjob[0].job_id), 1, com, "", "", "");
                    if (listdoc.Count > 0)
                    {
                        Doc = listdoc[0].timeleave_doc;
                        if (_language == "EN")
                        {
                            strDescription += "(" + listdoc[0].worker_code + ") " + listdoc[0].worker_detail_en;
                            strDescription += "<br />";
                            strDescription += "Date From : " + Convert.ToDateTime(listdoc[0].timeleave_fromdate).ToString("yyyy/MM/dd") + " To " + Convert.ToDateTime(listdoc[0].timeleave_todate).ToString("yyyy/MM/dd");
                            strDescription += "<br />";
                            strDescription += "Reason : " + listdoc[0].reason_en;
                            JobType = "Leave";
                        }
                        else
                        {
                            strDescription += "(" + listdoc[0].worker_code + ") " + listdoc[0].worker_detail_th;
                            strDescription += "<br />";
                            strDescription += "วันที่ : " + Convert.ToDateTime(listdoc[0].timeleave_fromdate).ToString("dd/MM/yyyy") + " ถึงวันที่ " + Convert.ToDateTime(listdoc[0].timeleave_todate).ToString("dd/MM/yyyy");
                            strDescription += "<br />";
                            strDescription += "เหตุผล : " + listdoc[0].reason_th;
                            JobType = "ลา";
                        }
                    }
                }

                if (Approve)
                {
                    if (_language == "EN")
                        strSubject = "Your " + JobType + " request was approved. <br /> DocRef:" + Doc;
                    else
                        strSubject = "ระบบได้ทำการอนุมัติเอกสาร เลขที่เอกสาร:" + Doc + " (" + JobType + ")";
                }
                else
                {
                    if (_language == "EN")
                        strSubject = "Your " + JobType + " request was (not) approved. <br /> DocRef:" + Doc;
                    else
                        strSubject = "เอกสารของท่านไม่ผ่านการอนุมัติ เลขที่เอกสาร:" + Doc + " (" + JobType + ")";
                }
                #region HTML
                strHTML = " <html xmlns=\"http://www.w3.org/1999/xhtml\">";
                strHTML += " <head runat=\"server\">";
                strHTML += "     <title>Untitled Page</title>";
                strHTML += "     <style type=\"text/css\">";
                strHTML += "         .style1";
                strHTML += "         {";
                strHTML += "             width: 150px;font-size:14px;font-weight:bold;padding:3px;";
                strHTML += "         }";
                strHTML += "         .style2";
                strHTML += "         {";
                strHTML += "             font-size:14px;padding:3px;";
                strHTML += "         }";
                strHTML += "         .styleHeader";
                strHTML += "         {";
                strHTML += "             height: 25px;font-size:17px;padding:3px;";
                strHTML += "         }";
                strHTML += "     </style>";
                strHTML += " </head>";
                strHTML += " <body>";
                strHTML += "     <form id=\"form1\" runat=\"server\">";
                strHTML += "     <div>";
                strHTML += "     ";
                strHTML += "         <table cellpadding=\"0\" cellspacing=\"1\" ";
                strHTML += "             style=\"width: 80%; font-family: Tahoma, Geneva, sans-serif; font-size: 16px;\">";
                strHTML += "             <tr>";
                strHTML += "                 <td align=\"center\" bgcolor=\"#3399FF\" class=\"styleHeader\" colspan=\"2\" ";
                strHTML += "                     style=\"font-weight: bold\">";
                strHTML += "                     HRFocus On-Web Notification</td>";
                strHTML += "             </tr>";
                //--
                strHTML += "             <tr>";
                strHTML += "                 <td align=\"right\" bgcolor=\"#99FF99\" class=\"style1\">";

                if (_language == "EN")
                    strHTML += "                     Subject :</td>";
                else
                    strHTML += "                     เรื่อง :</td>";

                strHTML += "                 <td bgcolor=\"#FFFF99\" class=\"style2\" >";
                strHTML += "                     &nbsp;" + strSubject + "</td>";
                strHTML += "             </tr>";
                //--
                strHTML += "             <tr>";
                strHTML += "                 <td align=\"right\" bgcolor=\"#99FF99\" class=\"style1\">";

                if (_language == "EN")
                    strHTML += "                     Description :</td>";
                else
                    strHTML += "                     รายละเอียด :</td>";

                strHTML += "                 <td bgcolor=\"#FFFF99\" class=\"style2\" >";
                strHTML += "                     &nbsp;" + strDescription + "</td>";
                strHTML += "             </tr>";
                //--

                string strRemark = "-";

                strHTML += "             <tr>";
                strHTML += "                 <td align=\"right\" bgcolor=\"#99FF99\" class=\"style1\">";

                if (_language == "EN")
                    strHTML += "                     Remark :</td>";
                else
                    strHTML += "                     หมายเหตุ :</td>";

                strHTML += "                 <td bgcolor=\"#FFFF99\" class=\"style2\" >";
                strHTML += "                     &nbsp;" + strRemark + "</td>";
                strHTML += "             </tr>";
                //--

                strHTML += "             <tr bgcolor=\"#CCCCCC\">";
                strHTML += "                 <td class=\"style1\">";
                strHTML += "                     &nbsp;</td>";
                strHTML += "                 <td>";
                strHTML += "                     &nbsp;</td>";
                strHTML += "             </tr>";


                strHTML += "         </table>";
                strHTML += "     ";
                strHTML += "     </div>";
                strHTML += "     </form>";
                strHTML += " </body>";
                strHTML += " </html>";

                #endregion

                strResult = strHTML;

            }
            catch { }
            return strResult;
        }
    }



}

