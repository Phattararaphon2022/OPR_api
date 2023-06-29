using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using ClassLibrary_BPC.hrfocus.controller;
using ClassLibrary_BPC.hrfocus.service;
using ClassLibrary_BPC.hrfocus.model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Drawing;
using AntsCode.Util;
using System.Web.Security;
using HRFocusWCF;
using System.Security.Permissions;
using System.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;

namespace BPC_OPR
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ModuleSelfServices : IModuleSelfServices
    {
        BpcOpr objBpcOpr = new BpcOpr();

        private async Task<bool> doUploadFile(string fileName, Stream stream)
        {
            bool result = false;

            try
            {
                string FilePath = Path.Combine
  (ClassLibrary_BPC.Config.PathFileImport + "\\Imports", fileName);
                //string FilePath = Path.Combine
                //  (HostingEnvironment.MapPath("~/Uploads"), fileName);

                MultipartParser parser = new MultipartParser(stream);

                if (parser.Success)
                {
                    //absolute filename, extension included.
                    var filename = parser.Filename;
                    var filetype = parser.ContentType;
                    var ext = Path.GetExtension(filename);

                    using (var file = File.Create(FilePath))
                    {
                        await file.WriteAsync(parser.FileContents, 0, parser.FileContents.Length);
                        result = true;

                    }
                }

            }
            catch { }

            return result;

        }
        public byte[] DownloadFile(string filePath)
        {
            byte[] data = { };
            try
            {
                data = File.ReadAllBytes(filePath);
            }
            catch
            {
            }
            return data;
        }
        public string DeleteFile(string filePath)
        {
            JObject output = new JObject();
            try
            {
                File.Delete(filePath);
                output["success"] = true;
                output["message"] = filePath;
            }
            catch
            {
                output["success"] = false;
                output["message"] = filePath;
            }
            return output.ToString(Formatting.None);
        }


        #region TRTimeleave
        public string getTRTimeleaveList(InputTRTimeleave input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF005.1";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                //DateTime datefrom = Convert.ToDateTime(input.timeleave_fromdate);
                //DateTime dateto = Convert.ToDateTime(input.timeleave_todate);

                cls_ctTRTimeleave objTRTimeleave = new cls_ctTRTimeleave();
                List<cls_TRTimeleave> listTRTimeleave = objTRTimeleave.getDataByFillter(input.timeleave_id, input.status, input.company_code, input.worker_code, input.timeleave_fromdate, input.timeleave_todate);

                JArray array = new JArray();

                if (listTRTimeleave.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRTimeleave model in listTRTimeleave)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("leave_code", model.leave_code);

                        json.Add("worker_detail_th", model.worker_detail_th);
                        json.Add("leave_detail_th", model.leave_detail_th);
                        json.Add("worker_detail_en", model.worker_detail_en);
                        json.Add("leave_detail_en", model.leave_detail_en);

                        json.Add("timeleave_id", model.timeleave_id);
                        json.Add("timeleave_doc", model.timeleave_doc);

                        json.Add("timeleave_fromdate", model.timeleave_fromdate);
                        json.Add("timeleave_todate", model.timeleave_todate);

                        json.Add("timeleave_type", model.timeleave_type);
                        json.Add("timeleave_min", model.timeleave_min);

                        json.Add("timeleave_actualday", model.timeleave_actualday);
                        json.Add("timeleave_incholiday", model.timeleave_incholiday);
                        json.Add("timeleave_deduct", model.timeleave_deduct);

                        json.Add("timeleave_note", model.timeleave_note);
                        json.Add("reason_code", model.reason_code);
                        json.Add("reason_th", model.reason_th);
                        json.Add("reason_en", model.reason_en);
                        json.Add("status", model.status);
                        json.Add("status_job", model.status_job);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        cls_ctMTReqdocument objMTReqdoc = new cls_ctMTReqdocument();
                        List<cls_MTReqdocument> listTRReqdoc = objMTReqdoc.getDataByFillter(model.company_code, 0, model.timeleave_id.ToString(), "LEA");
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
                            json.Add("reqdoc_data", arrayTRReqdoc);
                        }
                        else
                        {
                            json.Add("reqdoc_data", arrayTRReqdoc);
                        }

                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }

                    output["result"] = "1";
                    output["result_text"] = "1";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageTRTimeleave(InputTRTimeleave input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF005.2";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            string message = "Retrieved data not successfully";
            string strID = "";
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctTRTimeleave objTRTimeleave = new cls_ctTRTimeleave();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRTimeleave>>(input.leave_data);
                foreach (cls_TRTimeleave leavedata in jsonArray)
                {
                    cls_TRTimeleave model = new cls_TRTimeleave();

                    model.company_code = leavedata.company_code;
                    model.worker_code = leavedata.worker_code;
                    model.timeleave_id = leavedata.timeleave_id.Equals("") ? 0 : Convert.ToInt32(leavedata.timeleave_id);
                    model.timeleave_doc = leavedata.timeleave_doc;

                    model.timeleave_fromdate = Convert.ToDateTime(leavedata.timeleave_fromdate);
                    model.timeleave_todate = Convert.ToDateTime(leavedata.timeleave_todate);

                    model.timeleave_type = leavedata.timeleave_type;
                    model.timeleave_min = leavedata.timeleave_min;

                    model.timeleave_actualday = leavedata.timeleave_actualday;
                    model.timeleave_incholiday = leavedata.timeleave_incholiday;
                    model.timeleave_deduct = leavedata.timeleave_deduct;

                    model.timeleave_note = leavedata.timeleave_note;
                    model.leave_code = leavedata.leave_code;
                    model.reason_code = leavedata.reason_code;
                    model.status = leavedata.status;

                    model.modified_by = input.username;
                    model.flag = leavedata.flag;

                    strID = objTRTimeleave.insert(model);
                    if (!strID.Equals(""))
                    {
                        cls_ctTRAccount objTRAccount = new cls_ctTRAccount();
                        List<cls_TRAccount> listTRAccount = objTRAccount.getDataworkflowByFillter(model.company_code, "", model.worker_code, "", "LEA");
                        if (listTRAccount.Count > 0)
                        {
                            cls_ctMTJobtable objMTJob = new cls_ctMTJobtable();
                            cls_MTJobtable modeljob = new cls_MTJobtable();
                            modeljob.company_code = model.company_code;
                            modeljob.jobtable_id = 0;
                            modeljob.job_id = strID;
                            modeljob.job_type = "LEA";
                            modeljob.status_job = "W";
                            modeljob.job_date = Convert.ToDateTime(leavedata.timeleave_fromdate);
                            modeljob.job_nextstep = listTRAccount[0].totalapprove;
                            modeljob.workflow_code = listTRAccount[0].workflow_code;
                            modeljob.created_by = input.username;
                            string strID1 = objMTJob.insert(modeljob);
                        }
                        else
                        {
                            objTRTimeleave.delete(Convert.ToInt32(strID));
                            strID = "";
                            message = "There are no workflow contexts for this worker_code :" + leavedata.worker_code;
                            break;
                        }
                        if (leavedata.reqdoc_data.Count > 0)
                        {
                            foreach (cls_MTReqdocument reqdoc in leavedata.reqdoc_data)
                            {
                                cls_ctMTReqdocument objMTReqdocu = new cls_ctMTReqdocument();
                                cls_MTReqdocument modelreqdoc = new cls_MTReqdocument();
                                modelreqdoc.company_code = reqdoc.company_code;
                                modelreqdoc.document_id = reqdoc.document_id;
                                modelreqdoc.job_id = strID;
                                modelreqdoc.job_type = reqdoc.job_type;
                                modelreqdoc.document_name = reqdoc.document_name;
                                modelreqdoc.document_type = reqdoc.document_type;
                                modelreqdoc.document_path = reqdoc.document_path;

                                modelreqdoc.created_by = input.username;
                                string strIDs = objMTReqdocu.insert(modelreqdoc);
                            }
                        }
                        cls_srvProcessTime srv_time = new cls_srvProcessTime();
                        srv_time.doCalleaveacc(model.timeleave_fromdate.Year.ToString(), model.company_code, model.worker_code, model.modified_by);
                    }
                    else
                    {
                        break;
                    }
                }
                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "500";
                    log.apilog_message = objTRTimeleave.getMessage();
                }

                objTRTimeleave.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteTRTimeleave(InputTRTimeleave input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF005.3";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctTRTimeleave controller = new cls_ctTRTimeleave();
                cls_TRTimeleave model = controller.getDataByID(input.timeleave_id);
                bool blnResult = controller.delete(input.timeleave_id);

                if (blnResult)
                {
                    cls_srvProcessTime srv_time = new cls_srvProcessTime();
                    srv_time.doCalleaveacc(model.timeleave_fromdate.Year.ToString(), model.company_code, model.worker_code, model.modified_by);
                    cls_ctMTJobtable MTJob = new cls_ctMTJobtable();
                    MTJob.delete(model.company_code, 0, model.timeleave_id.ToString(), "LEA");
                    cls_ctMTReqdocument MTReqdoc = new cls_ctMTReqdocument();
                    List<cls_MTReqdocument> filelist = MTReqdoc.getDataByFillter(model.company_code, 0, model.timeleave_id.ToString(), "LEA");
                    if (filelist.Count > 0)
                    {
                        foreach (cls_MTReqdocument filedata in filelist)
                        {
                            File.Delete(filedata.document_path);
                        }
                    }
                    MTReqdoc.delete(model.company_code, 0, model.timeleave_id.ToString(), "LEA");
                    output["success"] = true;
                    output["message"] = "Remove data successfully";

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Remove data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }



            return output.ToString(Formatting.None);

        }
        public string doGetLeaveActualDay(InputTRTimeleave input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF005.5";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                DateTime datefrom = Convert.ToDateTime(input.timeleave_fromdate);
                DateTime dateto = Convert.ToDateTime(input.timeleave_todate);

                cls_ctTRTimecard objTRTimecard = new cls_ctTRTimecard();
                List<cls_TRTimecard> listTRTimecard = objTRTimecard.getDataByFillter(input.company_code, input.project_code, input.worker_code, datefrom, dateto);

                int intDays = 0;

                if (listTRTimecard.Count > 0)
                {
                    foreach (cls_TRTimecard model in listTRTimecard)
                    {
                        if (model.timecard_daytype.Equals("O") || model.timecard_daytype.Equals("H") || model.timecard_daytype.Equals("C"))
                        {

                        }
                        else
                        {
                            intDays++;
                        }

                    }

                    output["result"] = "1";
                    output["result_text"] = "1";
                    output["data"] = intDays;
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = intDays;
                }
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        #endregion

        #region TRTimeot
        public string getTRTimeotList(InputTRTimeot input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF006.1";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                DateTime datefrom = Convert.ToDateTime(input.timeot_workdate);
                DateTime dateto = Convert.ToDateTime(input.timeot_todate);

                cls_ctTRTimeot objTRTime = new cls_ctTRTimeot();
                List<cls_TRTimeot> listTRTime = objTRTime.getDataByFillter(input.timeot_id, input.status, input.company_code, input.worker_code, datefrom, dateto);

                JArray array = new JArray();

                if (listTRTime.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRTimeot model in listTRTime)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);

                        json.Add("worker_detail_th", model.worker_detail_th);
                        json.Add("worker_detail_en", model.worker_detail_en);

                        json.Add("timeot_id", model.timeot_id);
                        json.Add("timeot_doc", model.timeot_doc);

                        json.Add("timeot_workdate", model.timeot_workdate);

                        json.Add("timeot_beforemin", model.timeot_beforemin);
                        json.Add("timeot_normalmin", model.timeot_normalmin);
                        json.Add("timeot_breakmin", model.timeot_breakmin);
                        json.Add("timeot_aftermin", model.timeot_aftermin);

                        int hrs = (model.timeot_beforemin) / 60;
                        int min = (model.timeot_beforemin) - (hrs * 60);
                        json.Add("timeot_beforemin_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));

                        hrs = (model.timeot_normalmin) / 60;
                        min = (model.timeot_normalmin) - (hrs * 60);
                        json.Add("timeot_normalmin_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));

                        hrs = (model.timeot_breakmin) / 60;
                        min = (model.timeot_breakmin) - (hrs * 60);
                        json.Add("timeot_break_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));

                        hrs = (model.timeot_aftermin) / 60;
                        min = (model.timeot_aftermin) - (hrs * 60);
                        json.Add("timeot_aftermin_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));


                        json.Add("timeot_note", model.timeot_note);
                        json.Add("location_code", model.location_code);
                        json.Add("location_name_th", model.location_name_th);
                        json.Add("location_name_en", model.location_name_en);
                        json.Add("reason_code", model.reason_code);
                        json.Add("reason_name_th", model.reason_name_th);
                        json.Add("reason_name_en", model.reason_name_en);
                        json.Add("status", model.status);
                        json.Add("status_job", model.status_job);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        cls_ctMTReqdocument objMTReqdoc = new cls_ctMTReqdocument();
                        List<cls_MTReqdocument> listTRReqdoc = objMTReqdoc.getDataByFillter(model.company_code, 0, model.timeot_id.ToString(), "OT");
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
                            json.Add("reqdoc_data", arrayTRReqdoc);
                        }
                        else
                        {
                            json.Add("reqdoc_data", arrayTRReqdoc);
                        }

                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }

                    output["result"] = "1";
                    output["result_text"] = "1";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageTRTimeot(InputTRTimeot input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF006.2";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            string message = "Retrieved data not successfully";
            string strID = "";
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctTRTimeot objTRTime = new cls_ctTRTimeot();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRTimeot>>(input.ot_data);
                foreach (cls_TRTimeot otdata in jsonArray)
                {
                    cls_TRTimeot model = new cls_TRTimeot();

                    model.company_code = otdata.company_code;
                    model.worker_code = otdata.worker_code;
                    model.timeot_id = otdata.timeot_id.Equals("") ? 0 : Convert.ToInt32(otdata.timeot_id);
                    model.timeot_doc = otdata.timeot_doc;

                    model.timeot_workdate = Convert.ToDateTime(otdata.timeot_workdate);

                    model.timeot_beforemin = otdata.timeot_beforemin;
                    model.timeot_normalmin = otdata.timeot_normalmin;
                    model.timeot_breakmin = otdata.timeot_breakmin;
                    model.timeot_aftermin = otdata.timeot_aftermin;

                    model.timeot_note = otdata.timeot_note;
                    model.location_code = otdata.location_code;
                    model.reason_code = otdata.reason_code;
                    model.status = otdata.status;

                    model.modified_by = input.username;
                    model.flag = otdata.flag;

                    strID = objTRTime.insert(model);
                    if (!strID.Equals(""))
                    {
                        cls_ctTRAccount objTRAccount = new cls_ctTRAccount();
                        List<cls_TRAccount> listTRAccount = objTRAccount.getDataworkflowByFillter(model.company_code, "", model.worker_code, "", "OT");
                        if (listTRAccount.Count > 0)
                        {
                            cls_ctMTJobtable objMTJob = new cls_ctMTJobtable();
                            cls_MTJobtable modeljob = new cls_MTJobtable();
                            modeljob.company_code = model.company_code;
                            modeljob.jobtable_id = 0;
                            modeljob.job_id = strID;
                            modeljob.job_type = "OT";
                            modeljob.status_job = "W";
                            modeljob.job_date = Convert.ToDateTime(otdata.timeot_workdate);
                            modeljob.job_nextstep = listTRAccount[0].totalapprove;
                            modeljob.workflow_code = listTRAccount[0].workflow_code;
                            modeljob.created_by = input.username;
                            string strID1 = objMTJob.insert(modeljob);
                        }
                        else
                        {
                            objTRTime.delete(Convert.ToInt32(strID));
                            strID = "";
                            message = "There are no workflow contexts for this worker_code :" + otdata.worker_code;
                            break;
                        }
                        if (otdata.reqdoc_data.Count > 0)
                        {
                            foreach (cls_MTReqdocument reqdoc in otdata.reqdoc_data)
                            {
                                cls_ctMTReqdocument objMTReqdocu = new cls_ctMTReqdocument();
                                cls_MTReqdocument modelreqdoc = new cls_MTReqdocument();
                                modelreqdoc.company_code = reqdoc.company_code;
                                modelreqdoc.document_id = reqdoc.document_id;
                                modelreqdoc.job_id = strID;
                                modelreqdoc.job_type = reqdoc.job_type;
                                modelreqdoc.document_name = reqdoc.document_name;
                                modelreqdoc.document_type = reqdoc.document_type;
                                modelreqdoc.document_path = reqdoc.document_path;

                                modelreqdoc.created_by = input.username;
                                string strIDs = objMTReqdocu.insert(modelreqdoc);
                            }
                        }

                    }
                    else
                    {
                        break;
                    }
                }
                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "500";
                    log.apilog_message = objTRTime.getMessage();
                }

                objTRTime.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteTRTimeot(InputTRTimeot input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF006.3";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctTRTimeot controller = new cls_ctTRTimeot();
                bool blnResult = controller.delete(input.timeot_id);

                if (blnResult)
                {
                    cls_ctMTJobtable MTJob = new cls_ctMTJobtable();
                    MTJob.delete(input.company_code, 0, input.timeot_id.ToString(), "OT");
                    cls_ctMTReqdocument MTReqdoc = new cls_ctMTReqdocument();
                    List<cls_MTReqdocument> filelist = MTReqdoc.getDataByFillter(input.company_code, 0, input.timeot_id.ToString(), "OT");
                    if (filelist.Count > 0)
                    {
                        foreach (cls_MTReqdocument filedata in filelist)
                        {
                            File.Delete(filedata.document_path);
                        }
                    }
                    MTReqdoc.delete(input.company_code, 0, input.timeot_id.ToString(), "OT");

                    output["success"] = true;
                    output["message"] = "Remove data successfully";

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Remove data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }



            return output.ToString(Formatting.None);

        }
        #endregion

        #region TRTimeshift
        public string getTRTimeshiftList(InputTRTimeshift input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF007.1";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                DateTime datefrom = Convert.ToDateTime(input.timeshift_fromdate);
                DateTime dateto = Convert.ToDateTime(input.timeshift_todate);

                cls_ctTRTimeshift objTRTime = new cls_ctTRTimeshift();
                List<cls_TRTimeshift> listTRTime = objTRTime.getDataByFillter(input.timeshift_id, input.status, input.company_code, input.worker_code, datefrom, dateto);

                JArray array = new JArray();

                if (listTRTime.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRTimeshift model in listTRTime)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);

                        json.Add("worker_detail_th", model.worker_detail_th);
                        json.Add("worker_detail_en", model.worker_detail_en);

                        json.Add("timeshift_id", model.timeshift_id);
                        json.Add("timeshift_doc", model.timeshift_doc);
                        json.Add("timeshift_workdate", model.timeshift_workdate);
                        json.Add("timeshift_old", model.timeshift_old);
                        json.Add("shift_old_th", model.shift_old_th);
                        json.Add("shift_old_en", model.shift_old_en);
                        json.Add("timeshift_new", model.timeshift_new);
                        json.Add("shift_new_th", model.shift_new_th);
                        json.Add("shift_new_en", model.shift_new_en);
                        json.Add("timeshift_note", model.timeshift_note);

                        json.Add("reason_code", model.reason_code);
                        json.Add("reason_detail_th", model.reason_detail_th);
                        json.Add("reason_detail_en", model.reason_detail_en);
                        json.Add("status", model.status);
                        json.Add("status_job", model.status_job);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        cls_ctMTReqdocument objMTReqdoc = new cls_ctMTReqdocument();
                        List<cls_MTReqdocument> listTRReqdoc = objMTReqdoc.getDataByFillter(model.company_code, 0, model.timeshift_id.ToString(), "SHT");
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
                            json.Add("reqdoc_data", arrayTRReqdoc);
                        }
                        else
                        {
                            json.Add("reqdoc_data", arrayTRReqdoc);
                        }

                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }

                    output["result"] = "1";
                    output["result_text"] = "1";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageTRTimeshift(InputTRTimeshift input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF007.2";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            string message = "Retrieved data not successfully";
            string strID = "";
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctTRTimeshift objTRTime = new cls_ctTRTimeshift();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRTimeshift>>(input.timeshift_data);
                foreach (cls_TRTimeshift shiftdata in jsonArray)
                {
                    cls_TRTimeshift model = new cls_TRTimeshift();

                    model.company_code = shiftdata.company_code;
                    model.worker_code = shiftdata.worker_code;

                    model.timeshift_id = shiftdata.timeshift_id;
                    model.timeshift_doc = shiftdata.timeshift_doc;

                    model.timeshift_workdate = Convert.ToDateTime(shiftdata.timeshift_workdate);
                    cls_ctTRTimecard objTimecard = new cls_ctTRTimecard();
                    List<cls_TRTimecard> listTimecard = objTimecard.getDataByFillter(shiftdata.company_code, "", shiftdata.worker_code, model.timeshift_workdate, model.timeshift_workdate);
                    if (listTimecard.Count > 0)
                    {
                        model.timeshift_old = listTimecard[0].shift_code;
                    }
                    model.timeshift_new = shiftdata.timeshift_new;

                    model.timeshift_note = shiftdata.timeshift_note;

                    model.reason_code = shiftdata.reason_code;
                    model.status = shiftdata.status;

                    model.modified_by = input.username;
                    model.flag = shiftdata.flag;

                    strID = objTRTime.insert(model);
                    if (!strID.Equals(""))
                    {
                        cls_ctTRAccount objTRAccount = new cls_ctTRAccount();
                        List<cls_TRAccount> listTRAccount = objTRAccount.getDataworkflowByFillter(model.company_code, "", model.worker_code, "", "SHT");
                        if (listTRAccount.Count > 0)
                        {
                            cls_ctMTJobtable objMTJob = new cls_ctMTJobtable();
                            cls_MTJobtable modeljob = new cls_MTJobtable();
                            modeljob.company_code = model.company_code;
                            modeljob.jobtable_id = 0;
                            modeljob.job_id = strID;
                            modeljob.job_type = "SHT";
                            modeljob.status_job = "W";
                            modeljob.job_date = Convert.ToDateTime(shiftdata.timeshift_workdate);
                            modeljob.job_nextstep = listTRAccount[0].totalapprove;
                            modeljob.workflow_code = listTRAccount[0].workflow_code;
                            modeljob.created_by = input.username;
                            string strID1 = objMTJob.insert(modeljob);
                        }
                        else
                        {
                            objTRTime.delete(Convert.ToInt32(strID));
                            strID = "";
                            message = "There are no workflow contexts for this worker_code :" + shiftdata.worker_code;
                            break;
                        }
                        if (shiftdata.reqdoc_data.Count > 0)
                        {
                            foreach (cls_MTReqdocument reqdoc in shiftdata.reqdoc_data)
                            {
                                cls_ctMTReqdocument objMTReqdocu = new cls_ctMTReqdocument();
                                cls_MTReqdocument modelreqdoc = new cls_MTReqdocument();
                                modelreqdoc.company_code = reqdoc.company_code;
                                modelreqdoc.document_id = reqdoc.document_id;
                                modelreqdoc.job_id = strID;
                                modelreqdoc.job_type = reqdoc.job_type;
                                modelreqdoc.document_name = reqdoc.document_name;
                                modelreqdoc.document_type = reqdoc.document_type;
                                modelreqdoc.document_path = reqdoc.document_path;

                                modelreqdoc.created_by = input.username;
                                string strIDs = objMTReqdocu.insert(modelreqdoc);
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "500";
                    log.apilog_message = objTRTime.getMessage();
                }

                objTRTime.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteTRTimeshift(InputTRTimeshift input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF007.3";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctTRTimeshift controller = new cls_ctTRTimeshift();

                cls_TRTimeshift model = controller.getDataByID(input.timeshift_id);

                bool blnResult = controller.delete(input.timeshift_id);

                if (blnResult)
                {
                    cls_ctTRTimecard objTRTimecard = new cls_ctTRTimecard();
                    List<cls_TRTimecard> list_timecard = objTRTimecard.getDataByFillter(model.company_code, "", model.worker_code, model.timeshift_workdate.Date, model.timeshift_workdate.Date);

                    if (list_timecard.Count > 0)
                    {
                        cls_TRTimecard timecard = list_timecard[0];
                        timecard.shift_code = model.timeshift_old;

                        objTRTimecard.update(timecard);
                    }
                    cls_ctMTJobtable MTJob = new cls_ctMTJobtable();
                    MTJob.delete(model.company_code, 0, model.timeshift_id.ToString(), "SHT");
                    cls_ctMTReqdocument MTReqdoc = new cls_ctMTReqdocument();
                    List<cls_MTReqdocument> filelist = MTReqdoc.getDataByFillter(model.company_code, 0, model.timeshift_id.ToString(), "SHT");
                    if (filelist.Count > 0)
                    {
                        foreach (cls_MTReqdocument filedata in filelist)
                        {
                            File.Delete(filedata.document_path);
                        }
                    }
                    MTReqdoc.delete(model.company_code, 0, model.timeshift_id.ToString(), "SHT");
                    output["success"] = true;
                    output["message"] = "Remove data successfully";

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Remove data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }



            return output.ToString(Formatting.None);

        }
        #endregion

        #region TRTimecheckin
        public string getTRTimecheckinList(InputTRTimecheckin input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF010.1";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctTRTimecheckin objTRTimecheckin = new cls_ctTRTimecheckin();
                List<cls_TRTimecheckin> listTRTimecheckin = objTRTimecheckin.getDataByFillter(input.company_code, input.timecheckin_id, input.timecheckin_time, input.timecheckin_type, input.location_code, input.worker_code, input.timecheckin_workdate, input.timecheckin_todate, input.status);

                JArray array = new JArray();

                if (listTRTimecheckin.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRTimecheckin model in listTRTimecheckin)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("worker_detail_en", model.worker_detail_en);
                        json.Add("worker_detail_th", model.worker_detail_th);
                        json.Add("timecheckin_id", model.timecheckin_id);
                        json.Add("timecheckin_doc", model.timecheckin_doc);
                        json.Add("timecheckin_workdate", model.timecheckin_workdate.ToString("yyyy-MM-dd"));
                        json.Add("timecheckin_time", model.timecheckin_time);
                        json.Add("timecheckin_type", model.timecheckin_type);
                        json.Add("timecheckin_lat", model.timecheckin_lat);
                        json.Add("timecheckin_long", model.timecheckin_long);
                        json.Add("timecheckin_note", model.timecheckin_note);
                        json.Add("location_code", model.location_code);
                        json.Add("location_name_en", model.location_name_en);
                        json.Add("location_name_th", model.location_name_th);
                        json.Add("status", model.status);
                        json.Add("status_job", model.status_job);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        cls_ctMTReqdocument objMTReqdoc = new cls_ctMTReqdocument();
                        List<cls_MTReqdocument> listTRReqdoc = objMTReqdoc.getDataByFillter(model.company_code, 0, model.timecheckin_id.ToString(), "CI");
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
                            json.Add("reqdoc_data", arrayTRReqdoc);
                        }
                        else
                        {
                            json.Add("reqdoc_data", arrayTRReqdoc);
                        }
                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }

                    output["result"] = "1";
                    output["result_text"] = "1";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageTRTimecheckin(InputTRTimecheckin input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF010.2";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            string message = "Retrieved data not successfully";
            string strID = "";
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctTRTimecheckin objTRTimecheckin = new cls_ctTRTimecheckin();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRTimecheckin>>(input.timecheckin_data);
                foreach (cls_TRTimecheckin cidata in jsonArray)
                {
                    cls_TRTimecheckin model = new cls_TRTimecheckin();

                    model.company_code = cidata.company_code;
                    model.worker_code = cidata.worker_code;
                    model.timecheckin_id = cidata.timecheckin_id.Equals("") ? 0 : Convert.ToInt32(cidata.timecheckin_id);
                    model.timecheckin_doc = cidata.timecheckin_doc;
                    model.timecheckin_workdate = Convert.ToDateTime(cidata.timecheckin_workdate);
                    model.timecheckin_time = cidata.timecheckin_time;
                    model.timecheckin_type = cidata.timecheckin_type;
                    model.timecheckin_lat = cidata.timecheckin_lat;
                    model.timecheckin_long = cidata.timecheckin_long;
                    model.timecheckin_note = cidata.timecheckin_note;
                    model.location_code = cidata.location_code;
                    model.status = cidata.status;
                    model.modified_by = input.username;
                    model.flag = cidata.flag;

                    strID = objTRTimecheckin.insert(model);
                    if (!strID.Equals(""))
                    {
                        cls_ctTRAccount objTRAccount = new cls_ctTRAccount();
                        List<cls_TRAccount> listTRAccount = objTRAccount.getDataworkflowByFillter(model.company_code, "", model.worker_code, "", "CI");
                        if (listTRAccount.Count > 0)
                        {
                            cls_ctMTJobtable objMTJob = new cls_ctMTJobtable();
                            cls_MTJobtable modeljob = new cls_MTJobtable();
                            modeljob.company_code = model.company_code;
                            modeljob.jobtable_id = 0;
                            modeljob.job_id = strID;
                            modeljob.job_type = "CI";
                            modeljob.status_job = "W";
                            modeljob.job_date = Convert.ToDateTime(cidata.timecheckin_workdate);
                            modeljob.job_nextstep = listTRAccount[0].totalapprove;
                            modeljob.workflow_code = listTRAccount[0].workflow_code;
                            modeljob.created_by = input.username;
                            string strID1 = objMTJob.insert(modeljob);
                        }
                        else
                        {
                            objTRTimecheckin.delete(cidata.company_code, strID, "", cidata.timecheckin_type, "", cidata.worker_code);
                            strID = "";
                            message = "There are no workflow contexts for this worker_code :" + cidata.worker_code;
                            break;
                        }
                        if (cidata.reqdoc_data.Count > 0)
                        {
                            foreach (cls_MTReqdocument reqdoc in cidata.reqdoc_data)
                            {
                                cls_ctMTReqdocument objMTReqdocu = new cls_ctMTReqdocument();
                                cls_MTReqdocument modelreqdoc = new cls_MTReqdocument();
                                modelreqdoc.company_code = reqdoc.company_code;
                                modelreqdoc.document_id = reqdoc.document_id;
                                modelreqdoc.job_id = strID;
                                modelreqdoc.job_type = reqdoc.job_type;
                                modelreqdoc.document_name = reqdoc.document_name;
                                modelreqdoc.document_type = reqdoc.document_type;
                                modelreqdoc.document_path = reqdoc.document_path;

                                modelreqdoc.created_by = input.username;
                                string strIDs = objMTReqdocu.insert(modelreqdoc);
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "500";
                    log.apilog_message = objTRTimecheckin.getMessage();
                }

                objTRTimecheckin.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteTRTimecheckin(InputTRTimecheckin input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF010.3";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                if (input.timecheckin_workdate.Equals(null))
                {
                    input.timecheckin_workdate = "";
                }
                cls_ctTRTimecheckin controller = new cls_ctTRTimecheckin();
                bool blnResult = controller.delete(input.company_code, input.timecheckin_id.ToString(), input.timecheckin_time, input.timecheckin_type, input.timecheckin_workdate, input.worker_code);

                if (blnResult)
                {
                    cls_ctMTJobtable MTJob = new cls_ctMTJobtable();
                    MTJob.delete(input.company_code, 0, input.timecheckin_id.ToString(), "CI");
                    cls_ctMTReqdocument MTReqdoc = new cls_ctMTReqdocument();
                    List<cls_MTReqdocument> filelist = MTReqdoc.getDataByFillter(input.company_code, 0, input.timecheckin_id.ToString(), "CI");
                    if (filelist.Count > 0)
                    {
                        foreach (cls_MTReqdocument filedata in filelist)
                        {
                            File.Delete(filedata.document_path);
                        }
                    }
                    MTReqdoc.delete(input.company_code, 0, input.timecheckin_id.ToString(), "CI");
                    output["success"] = true;
                    output["message"] = "Remove data successfully";

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Remove data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }



            return output.ToString(Formatting.None);

        }
        #endregion

        #region TRTimedaytype
        public string getTRTimedaytypeList(InputTRTimedaytype input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF009.1";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctTRTimedaytype objTRTimedaytype = new cls_ctTRTimedaytype();
                List<cls_TRTimedaytype> listTRTimedaytype = objTRTimedaytype.getDataByFillter(input.company_code, input.timedaytype_id, input.worker_code, input.timedaytype_workdate, input.timedaytype_todate, input.status);

                JArray array = new JArray();

                if (listTRTimedaytype.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRTimedaytype model in listTRTimedaytype)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("worker_detail_en", model.worker_detail_en);
                        json.Add("worker_detail_th", model.worker_detail_th);
                        json.Add("timedaytype_id", model.timedaytype_id);
                        json.Add("timedaytype_doc", model.timedaytype_doc);
                        json.Add("timedaytype_workdate", model.timedaytype_workdate.ToString("yyyy-MM-dd"));
                        json.Add("timedaytype_old", model.timedaytype_old);
                        json.Add("timedaytype_new", model.timedaytype_new);
                        json.Add("timedaytype_note", model.timedaytype_note);
                        json.Add("reason_code", model.reason_code);
                        json.Add("reason_name_en", model.reason_name_en);
                        json.Add("reason_name_th", model.reason_name_th);
                        json.Add("status", model.status);
                        json.Add("status_job", model.status_job);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);

                        cls_ctMTReqdocument objMTReqdoc = new cls_ctMTReqdocument();
                        List<cls_MTReqdocument> listTRReqdoc = objMTReqdoc.getDataByFillter(model.company_code, 0, model.timedaytype_id.ToString(), "DAT");
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
                            json.Add("reqdoc_data", arrayTRReqdoc);
                        }
                        else
                        {
                            json.Add("reqdoc_data", arrayTRReqdoc);
                        }
                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }

                    output["result"] = "1";
                    output["result_text"] = "1";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageTRTimedaytype(InputTRTimedaytype input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF009.2";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            string message = "Retrieved data not successfully";
            string strID = "";
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctTRTimedaytype objTRTimedaytype = new cls_ctTRTimedaytype();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRTimedaytype>>(input.timedaytype_data);
                foreach (cls_TRTimedaytype data in jsonArray)
                {
                    cls_TRTimedaytype model = new cls_TRTimedaytype();

                    model.company_code = data.company_code;
                    model.worker_code = data.worker_code;
                    model.timedaytype_id = data.timedaytype_id.Equals("") ? 0 : Convert.ToInt32(data.timedaytype_id);
                    model.timedaytype_doc = data.timedaytype_doc;
                    model.timedaytype_workdate = Convert.ToDateTime(data.timedaytype_workdate);
                    cls_ctTRTimecard objTimecard = new cls_ctTRTimecard();
                    List<cls_TRTimecard> listTimecard = objTimecard.getDataByFillter(data.company_code, "", data.worker_code, model.timedaytype_workdate, model.timedaytype_workdate);
                    if (listTimecard.Count > 0)
                    {
                        model.timedaytype_old = listTimecard[0].timecard_daytype;
                    }
                    model.timedaytype_new = data.timedaytype_new;
                    model.timedaytype_note = data.timedaytype_note;
                    model.reason_code = data.reason_code;
                    model.status = data.status;
                    model.modified_by = input.username;
                    model.flag = data.flag;

                    strID = objTRTimedaytype.insert(model);
                    if (!strID.Equals(""))
                    {
                        cls_ctTRAccount objTRAccount = new cls_ctTRAccount();
                        List<cls_TRAccount> listTRAccount = objTRAccount.getDataworkflowByFillter(model.company_code, "", model.worker_code, "", "DAT");
                        if (listTRAccount.Count > 0)
                        {
                            cls_ctMTJobtable objMTJob = new cls_ctMTJobtable();
                            cls_MTJobtable modeljob = new cls_MTJobtable();
                            modeljob.company_code = model.company_code;
                            modeljob.jobtable_id = 0;
                            modeljob.job_id = strID;
                            modeljob.job_type = "DAT";
                            modeljob.status_job = "W";
                            modeljob.job_date = Convert.ToDateTime(data.timedaytype_workdate);
                            modeljob.job_nextstep = listTRAccount[0].totalapprove;
                            modeljob.workflow_code = listTRAccount[0].workflow_code;
                            modeljob.created_by = input.username;
                            string strID1 = objMTJob.insert(modeljob);
                        }
                        else
                        {
                            objTRTimedaytype.delete(data.company_code, Convert.ToInt32(strID), data.worker_code);
                            strID = "";
                            message = "There are no workflow contexts for this worker_code :" + data.worker_code;
                            break;
                        }
                        if (data.reqdoc_data.Count > 0)
                        {
                            foreach (cls_MTReqdocument reqdoc in data.reqdoc_data)
                            {
                                cls_ctMTReqdocument objMTReqdocu = new cls_ctMTReqdocument();
                                cls_MTReqdocument modelreqdoc = new cls_MTReqdocument();
                                modelreqdoc.company_code = reqdoc.company_code;
                                modelreqdoc.document_id = reqdoc.document_id;
                                modelreqdoc.job_id = strID;
                                modelreqdoc.job_type = reqdoc.job_type;
                                modelreqdoc.document_name = reqdoc.document_name;
                                modelreqdoc.document_type = reqdoc.document_type;
                                modelreqdoc.document_path = reqdoc.document_path;

                                modelreqdoc.created_by = input.username;
                                string strIDs = objMTReqdocu.insert(modelreqdoc);
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "500";
                    log.apilog_message = objTRTimedaytype.getMessage();
                }

                objTRTimedaytype.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteTRTimedaytype(InputTRTimedaytype input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF009.3";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctTRTimedaytype controller = new cls_ctTRTimedaytype();
                bool blnResult = controller.delete(input.company_code, input.timedaytype_id, input.worker_code);

                if (blnResult)
                {

                    cls_ctMTJobtable MTJob = new cls_ctMTJobtable();
                    MTJob.delete(input.company_code, 0, input.timedaytype_id.ToString(), "DAT");
                    cls_ctMTReqdocument MTReqdoc = new cls_ctMTReqdocument();
                    List<cls_MTReqdocument> filelist = MTReqdoc.getDataByFillter(input.company_code, 0, input.timedaytype_id.ToString(), "DAT");
                    if (filelist.Count > 0)
                    {
                        foreach (cls_MTReqdocument filedata in filelist)
                        {
                            File.Delete(filedata.document_path);
                        }
                    }
                    MTReqdoc.delete(input.company_code, 0, input.timedaytype_id.ToString(), "DAT");
                    output["success"] = true;
                    output["message"] = "Remove data successfully";

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Remove data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }



            return output.ToString(Formatting.None);

        }
        #endregion

        #region TRTimeonsite
        public string getTRTimeonsiteList(InputTRTimeonsite input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF008.1";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctTRTimeonsite objTRTimeonsite = new cls_ctTRTimeonsite();
                List<cls_TRTimeonsite> listTRTimeonsite = objTRTimeonsite.getDataByFillter(input.company_code, input.timeonsite_id, input.location_code, input.worker_code, input.timeonsite_workdate, input.timeonstie_todate, input.status);

                JArray array = new JArray();

                if (listTRTimeonsite.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRTimeonsite model in listTRTimeonsite)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("timeonsite_id", model.timeonsite_id);
                        json.Add("timeonsite_doc", model.timeonsite_doc);
                        json.Add("timeonsite_workdate", model.timeonsite_workdate.ToString("yyyy-MM-dd"));
                        json.Add("timeonsite_in", model.timeonsite_in);
                        json.Add("timeonsite_out", model.timeonsite_out);
                        json.Add("timeonsite_note", model.timeonsite_note);
                        json.Add("reason_code", model.reason_code);
                        json.Add("reason_name_en", model.reason_name_en);
                        json.Add("reason_name_th", model.reason_name_th);
                        //cls_ctMTReason objRason = new cls_ctMTReason();
                        //List<cls_MTReason> listReason = objRason.getDataByFillter("ONS", "", model.reason_code, model.company_code);
                        //json.Add("reason_name_th", listReason[0].reason_name_th);
                        //json.Add("reason_name_en", listReason[0].reason_name_en);
                        json.Add("location_code", model.location_code);
                        json.Add("location_name_en", model.location_name_en);
                        json.Add("location_name_th", model.location_name_th);
                        //cls_ctMTLocation objlocation = new cls_ctMTLocation();
                        //List<cls_MTLocation> listlocation = objlocation.getDataByFillter("",model.location_code,model.company_code);
                        //json.Add("location_name_th", listlocation[0].location_name_th);
                        //json.Add("location_name_en", listlocation[0].location_name_en);
                        json.Add("worker_code", model.worker_code);
                        json.Add("worker_detail_en", model.worker_detail_en);
                        json.Add("worker_detail_th", model.worker_detail_th);
                        json.Add("status", model.status);
                        json.Add("status_job", model.status_job);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        cls_ctMTReqdocument objMTReqdoc = new cls_ctMTReqdocument();
                        List<cls_MTReqdocument> listTRReqdoc = objMTReqdoc.getDataByFillter(model.company_code, 0, model.timeonsite_id.ToString(), "ONS");
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
                            json.Add("reqdoc_data", arrayTRReqdoc);
                        }
                        else
                        {
                            json.Add("reqdoc_data", arrayTRReqdoc);
                        }

                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }

                    output["result"] = "1";
                    output["result_text"] = "1";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageTRTimeonsite(InputTRTimeonsite input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF008.2";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            string message = "Retrieved data not successfully";
            string strID = "";
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctTRTimeonsite objTRTimeonsite = new cls_ctTRTimeonsite();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRTimeonsite>>(input.timeonsite_data);
                foreach (cls_TRTimeonsite data in jsonArray)
                {
                    cls_TRTimeonsite model = new cls_TRTimeonsite();

                    model.company_code = data.company_code;
                    model.timeonsite_id = data.timeonsite_id.Equals("") ? 0 : Convert.ToInt32(data.timeonsite_id);
                    model.timeonsite_doc = data.timeonsite_doc;
                    model.timeonsite_workdate = Convert.ToDateTime(data.timeonsite_workdate);
                    model.timeonsite_in = data.timeonsite_in;
                    model.timeonsite_out = data.timeonsite_out;
                    model.timeonsite_note = data.timeonsite_note;
                    model.reason_code = data.reason_code;
                    model.location_code = data.location_code;
                    model.worker_code = data.worker_code;
                    model.status = data.status;
                    model.modified_by = input.username;
                    model.flag = data.flag;

                    strID = objTRTimeonsite.insert(model);
                    if (!strID.Equals(""))
                    {
                        cls_ctTRAccount objTRAccount = new cls_ctTRAccount();
                        List<cls_TRAccount> listTRAccount = objTRAccount.getDataworkflowByFillter(model.company_code, "", model.worker_code, "", "ONS");
                        if (listTRAccount.Count > 0)
                        {
                            cls_ctMTJobtable objMTJob = new cls_ctMTJobtable();
                            cls_MTJobtable modeljob = new cls_MTJobtable();
                            modeljob.company_code = model.company_code;
                            modeljob.jobtable_id = 0;
                            modeljob.job_id = strID;
                            modeljob.job_type = "ONS";
                            modeljob.status_job = "W";
                            modeljob.job_date = Convert.ToDateTime(data.timeonsite_workdate);
                            modeljob.job_nextstep = listTRAccount[0].totalapprove;
                            modeljob.workflow_code = listTRAccount[0].workflow_code;
                            modeljob.created_by = input.username;
                            string strID1 = objMTJob.insert(modeljob);
                        }
                        else
                        {
                            objTRTimeonsite.delete(data.company_code, Convert.ToInt32(strID), data.worker_code);
                            strID = "";
                            message = "There are no workflow contexts for this worker_code :" + data.worker_code;
                            break;
                        }
                        if (data.reqdoc_data.Count > 0)
                        {
                            foreach (cls_MTReqdocument reqdoc in data.reqdoc_data)
                            {
                                cls_ctMTReqdocument objMTReqdocu = new cls_ctMTReqdocument();
                                cls_MTReqdocument modelreqdoc = new cls_MTReqdocument();
                                modelreqdoc.company_code = reqdoc.company_code;
                                modelreqdoc.document_id = reqdoc.document_id;
                                modelreqdoc.job_id = strID;
                                modelreqdoc.job_type = reqdoc.job_type;
                                modelreqdoc.document_name = reqdoc.document_name;
                                modelreqdoc.document_type = reqdoc.document_type;
                                modelreqdoc.document_path = reqdoc.document_path;

                                modelreqdoc.created_by = input.username;
                                string strIDs = objMTReqdocu.insert(modelreqdoc);
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "500";
                    log.apilog_message = objTRTimeonsite.getMessage();
                }

                objTRTimeonsite.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteTRTimeonsite(InputTRTimeonsite input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF008.3";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctTRTimeonsite controller = new cls_ctTRTimeonsite();
                bool blnResult = controller.delete(input.company_code, input.timeonsite_id, input.worker_code);

                if (blnResult)
                {
                    cls_ctMTJobtable MTJob = new cls_ctMTJobtable();
                    MTJob.delete(input.company_code, 0, input.timeonsite_id.ToString(), "ONS");
                    cls_ctMTReqdocument MTReqdoc = new cls_ctMTReqdocument();
                    List<cls_MTReqdocument> filelist = MTReqdoc.getDataByFillter(input.company_code, 0, input.timeonsite_id.ToString(), "ONS");
                    if (filelist.Count > 0)
                    {
                        foreach (cls_MTReqdocument filedata in filelist)
                        {
                            File.Delete(filedata.document_path);
                        }
                    }
                    MTReqdoc.delete(input.company_code, 0, input.timeonsite_id.ToString(), "ONS");
                    output["success"] = true;
                    output["message"] = "Remove data successfully";

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Remove data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }



            return output.ToString(Formatting.None);

        }
        #endregion

        #region MTArea
        public string getMTAreaList(InputMTArea input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF004.1";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctMTArea objMTArea = new cls_ctMTArea();
                List<cls_MTArea> list = objMTArea.getDataByFillter(input.company_code, input.area_id, input.location_code, input.project_code);

                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTArea model in list)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("area_id", model.area_id);
                        json.Add("area_lat", model.area_lat);
                        json.Add("area_long", model.area_long);
                        json.Add("area_distance", model.area_distance);
                        json.Add("location_code", model.location_code);
                        json.Add("project_code", model.project_code);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        cls_ctTRArea objTRArea = new cls_ctTRArea();
                        List<cls_TRArea> listTRArea = objTRArea.getDataByFillter(model.company_code, model.location_code, input.worker_code);
                        JArray arrayTRArea = new JArray();
                        if (listTRArea.Count > 0)
                        {
                            int indexTRAccount = 1;

                            foreach (cls_TRArea modelTRArea in listTRArea)
                            {
                                JObject jsonTRPlan = new JObject();
                                jsonTRPlan.Add("company_code", modelTRArea.company_code);
                                jsonTRPlan.Add("location_code", modelTRArea.location_code);
                                jsonTRPlan.Add("worker_code", modelTRArea.worker_code);

                                jsonTRPlan.Add("index", indexTRAccount);


                                indexTRAccount++;

                                arrayTRArea.Add(jsonTRPlan);
                            }
                            json.Add("area_data", arrayTRArea);
                        }
                        else
                        {
                            json.Add("area_data", arrayTRArea);
                        }

                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }

                    output["result"] = "1";
                    output["result_text"] = "1";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageMTArea(InputMTArea input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF004.2";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctMTArea objMTArea = new cls_ctMTArea();
                cls_MTArea model = new cls_MTArea();
                model.company_code = input.company_code;
                model.area_id = input.area_id;
                model.area_lat = input.area_lat;
                model.area_long = input.area_long;
                model.area_distance = input.area_distance;
                model.location_code = input.location_code;
                model.project_code = input.project_code;

                model.modified_by = input.username;
                model.flag = input.flag;
                string strID = objMTArea.insert(model);
                if (!strID.Equals(""))
                {
                    try
                    {
                        cls_ctTRArea objTRArea = new cls_ctTRArea();
                        if (input.area_data.Count > 0)
                        {
                            objTRArea.insert(input.area_data);
                        }
                        else
                        {
                            objTRArea.delete(input.company_code, input.location_code, "");
                        }

                    }
                    catch (Exception ex)
                    {
                        string str = ex.ToString();
                    }
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objMTArea.getMessage();
                }

                objMTArea.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteeMTArea(InputMTArea input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF004.3";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTArea controller = new cls_ctMTArea();
                bool blnResult = controller.delete(input.company_code, input.area_id, input.location_code);
                if (blnResult)
                {
                    try
                    {
                        cls_ctTRArea objTRArea = new cls_ctTRArea();
                        objTRArea.delete(input.company_code, input.location_code, "");
                    }
                    catch (Exception ex)
                    {
                        string str = ex.ToString();
                    }
                    output["success"] = true;
                    output["message"] = "Remove data successfully";

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Remove data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }



            return output.ToString(Formatting.None);

        }
        #endregion

        #region MTTopic
        public string getMTTopicList(InputMTTopic input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF012.1";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctMTTopic objMTTopic = new cls_ctMTTopic();
                List<cls_MTTopic> list = objMTTopic.getDataByFillter(input.company_code, input.topic_id, input.topic_code, input.topic_type);

                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTTopic model in list)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("topic_id", model.topic_id);
                        json.Add("topic_code", model.topic_code);
                        json.Add("topic_name_th", model.topic_name_th);
                        json.Add("topic_name_en", model.topic_name_en);
                        json.Add("topic_type", model.topic_type);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);

                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }

                    output["result"] = "1";
                    output["result_text"] = "1";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageMTTopic(InputMTTopic input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF012.2";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctMTTopic objMTTopic = new cls_ctMTTopic();
                cls_MTTopic model = new cls_MTTopic();
                model.company_code = input.company_code;
                model.topic_id = input.topic_id;
                model.topic_code = input.topic_code;
                model.topic_name_th = input.topic_name_th;
                model.topic_name_en = input.topic_name_en;
                model.topic_type = input.topic_type;

                model.modified_by = input.username;
                model.flag = input.flag;
                string strID = objMTTopic.insert(model);
                if (!strID.Equals(""))
                {

                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objMTTopic.getMessage();
                }

                objMTTopic.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteeMTTopic(InputMTTopic input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF012.3";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTTopic controller = new cls_ctMTTopic();
                bool blnResult = controller.delete(input.company_code, input.topic_id, input.topic_code);
                if (blnResult)
                {
                    output["success"] = true;
                    output["message"] = "Remove data successfully";

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Remove data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }



            return output.ToString(Formatting.None);

        }
        #endregion

        #region MTReqdoc
        public string getMTReqdocList(InputMTReqdoc input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF011.1";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctMTReqdoc objMTReqdoc = new cls_ctMTReqdoc();
                List<cls_MTReqdoc> list = objMTReqdoc.getDataByFillter(input.company_code, input.reqdoc_id, input.worker_code, input.reqdoc_date, input.reqdoc_date_to, input.status);

                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTReqdoc model in list)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("worker_detail_th", model.worker_detail_th);
                        json.Add("worker_detail_en", model.worker_detail_en);
                        json.Add("reqdoc_id", model.reqdoc_id);
                        json.Add("reqdoc_doc", model.reqdoc_doc);
                        json.Add("reqdoc_date", model.reqdoc_date);
                        json.Add("reqdoc_note", model.reqdoc_note);
                        json.Add("status", model.status);
                        json.Add("status_job", model.status_job);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        cls_ctTRReqempinfo objTRReqempinfo = new cls_ctTRReqempinfo();
                        List<cls_TRReqempinfo> listTRReqempinfo = objTRReqempinfo.getDataByFillter(model.reqdoc_id, 0, 0);
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
                        List<cls_TRReqdocatt> listTRReqdocatt = objTRReqedocatt.getDataByFillter(model.reqdoc_id, 0, "", "");
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
                                jsonTRReqdocatt.Add("created_date", Convert.ToDateTime(modelTRReqdocatt.created_date));

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

                        array.Add(json);
                    }

                    output["result"] = "1";
                    output["result_text"] = "1";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageMTReqdoc(InputMTReqdoc input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF011.2";
            log.apilog_by = input.username;
            string message = "Retrieved data not successfully";
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctMTReqdoc objMTReqdoc = new cls_ctMTReqdoc();
                cls_MTReqdoc model = new cls_MTReqdoc();
                model.company_code = input.company_code;
                model.reqdoc_id = input.reqdoc_id;
                model.worker_code = input.worker_code;
                model.reqdoc_doc = input.reqdoc_doc;
                model.reqdoc_date = Convert.ToDateTime(input.reqdoc_date);
                model.reqdoc_note = input.reqdoc_note;
                model.status = Convert.ToInt32(input.status);

                model.modified_by = input.username;
                model.flag = input.flag;
                string strID = objMTReqdoc.insert(model);
                if (!strID.Equals(""))
                {

                    cls_ctTRAccount objTRAccount = new cls_ctTRAccount();
                    List<cls_TRAccount> listTRAccount = objTRAccount.getDataworkflowByFillter(model.company_code, "", model.worker_code, "", "REQ");
                    if (listTRAccount.Count > 0)
                    {
                        cls_ctMTJobtable objMTJob = new cls_ctMTJobtable();
                        cls_MTJobtable modeljob = new cls_MTJobtable();
                        modeljob.company_code = model.company_code;
                        modeljob.jobtable_id = 0;
                        modeljob.job_id = strID;
                        modeljob.job_type = "REQ";
                        modeljob.status_job = "W";
                        modeljob.job_date = Convert.ToDateTime(input.reqdoc_date);
                        modeljob.job_nextstep = listTRAccount[0].totalapprove;
                        modeljob.workflow_code = listTRAccount[0].workflow_code;
                        modeljob.created_by = input.username;
                        string strID1 = objMTJob.insert(modeljob);
                    }
                    else
                    {
                        objMTReqdoc.delete(input.company_code, Convert.ToInt32(strID), "", input.worker_code);
                        strID = "";
                        message = "There are no workflow contexts for this worker_code :" + input.worker_code;
                        input.reqempinfo_data.Clear();
                        input.reqdocatt_data.Clear();
                    }
                    cls_ctTRReqempinfo objTRReqempinfo = new cls_ctTRReqempinfo();
                    objTRReqempinfo.delete(Convert.ToInt32(strID), 0);
                    if (input.reqempinfo_data.Count > 0)
                    {
                        foreach (cls_TRReqempinfo data in input.reqempinfo_data)
                        {
                            cls_TRReqempinfo modelempinfo = new cls_TRReqempinfo();

                            modelempinfo.reqdoc_id = Convert.ToInt32(strID);
                            modelempinfo.reqdocempinfo_no = data.reqdocempinfo_no;
                            modelempinfo.topic_code = data.topic_code;
                            modelempinfo.reqempinfo_detail = data.reqempinfo_detail;

                            string strIDinfo = objTRReqempinfo.insert(modelempinfo);
                            if (!strIDinfo.Equals(""))
                            {

                            }
                            else
                            {
                                break;
                            }
                        }

                    }
                    cls_ctTRReqdocatt objTRReqdocatt = new cls_ctTRReqdocatt();
                    objTRReqdocatt.delete(Convert.ToInt32(strID), 0);
                    if (input.reqdocatt_data.Count > 0)
                    {
                        foreach (cls_TRReqdocatt data in input.reqdocatt_data)
                        {
                            cls_TRReqdocatt modeldocatt = new cls_TRReqdocatt();

                            modeldocatt.reqdoc_id = Convert.ToInt32(strID);
                            modeldocatt.reqdoc_att_no = data.reqdoc_att_no;
                            modeldocatt.reqdoc_att_file_name = data.reqdoc_att_file_name;
                            modeldocatt.reqdoc_att_file_type = data.reqdoc_att_file_type;
                            modeldocatt.reqdoc_att_path = data.reqdoc_att_path;
                            modeldocatt.created_by = input.username;

                            string strIDdocatt = objTRReqdocatt.insert(modeldocatt);
                            if (!strIDdocatt.Equals(""))
                            {

                            }
                            else
                            {
                                break;
                            }
                        }

                    }
                }
                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "500";
                    log.apilog_message = objMTReqdoc.getMessage();
                }

                objMTReqdoc.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteeMTReqdoc(InputMTReqdoc input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF011.3";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTReqdoc controller = new cls_ctMTReqdoc();
                bool blnResult = controller.delete(input.company_code, input.reqdoc_id, "", "");
                if (blnResult)
                {
                    cls_ctMTJobtable MTJob = new cls_ctMTJobtable();
                    MTJob.delete(input.company_code, 0, input.reqdoc_id.ToString(), "REQ");
                    cls_ctTRReqdocatt MTReqdoc = new cls_ctTRReqdocatt();
                    List<cls_TRReqdocatt> filelist = MTReqdoc.getDataByFillter(input.reqdoc_id, 0, "", "");
                    if (filelist.Count > 0)
                    {
                        foreach (cls_TRReqdocatt filedata in filelist)
                        {
                            File.Delete(filedata.reqdoc_att_path);
                        }
                    }
                    MTReqdoc.delete(input.reqdoc_id, 0);
                    try
                    {
                        cls_ctTRReqempinfo objTRReqempinfo = new cls_ctTRReqempinfo();
                        objTRReqempinfo.delete(input.reqdoc_id, 0);
                    }
                    catch (Exception ex)
                    {
                        string str = ex.ToString();
                    }
                    try
                    {
                        cls_ctTRReqdocatt objTRReqdocatt = new cls_ctTRReqdocatt();
                        objTRReqdocatt.delete(input.reqdoc_id, 0);
                    }
                    catch (Exception ex)
                    {
                        string str = ex.ToString();
                    }
                    output["success"] = true;
                    output["message"] = "Remove data successfully";

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Remove data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }



            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadMTReqdoc(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF011.4";
            log.apilog_by = by;
            log.apilog_data = "Stream";

            try
            {
                if (!objBpcOpr.doVerify(token))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }


                bool upload = await this.doUploadFile(fileName, stream);

                if (upload)
                {
                    string FilePath = Path.Combine
              (ClassLibrary_BPC.Config.PathFileImport + "\\Imports", fileName);
                    output["success"] = true;
                    output["message"] = FilePath;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Upload data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = "Upload data not successfully";
                }

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Upload data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        #endregion

        #region TRReqdocatt
        public string doDeleteeTRReqdocatt(InputTRReqdocatt input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF013.1";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctTRReqdocatt controller = new cls_ctTRReqdocatt();
                bool blnResult = controller.delete(input.reqdoc_id, input.reqdoc_att_no);
                if (blnResult)
                {
                    output["success"] = true;
                    output["message"] = "Remove data successfully";

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Remove data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }



            return output.ToString(Formatting.None);

        }
        #endregion

        #region MTJobtable
        public string getMTJobtableList(InputMTJobtable input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF014.1";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctMTJobtable objMTJob = new cls_ctMTJobtable();
                List<cls_MTJobtable> list = objMTJob.getDataByFillter(input.company_code, input.jobtable_id, input.job_id, input.job_type, input.workflow_code, input.status_job, input.job_date, input.job_date_to);

                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTJobtable model in list)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("jobtable_id", model.jobtable_id);
                        json.Add("job_id", model.job_id);
                        json.Add("job_type", model.job_type);
                        json.Add("status_job", model.status_job);
                        json.Add("job_nextstep", model.job_nextstep);
                        json.Add("job_date", model.job_date);
                        json.Add("job_finishdate", model.job_finishdate);
                        json.Add("workflow_code", model.workflow_code);

                        json.Add("created_by", model.created_by);
                        json.Add("created_date", model.created_date);

                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }

                    output["result"] = "1";
                    output["result_text"] = "1";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageMTJobtable(InputMTJobtable input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF014.2";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctMTJobtable objMTJob = new cls_ctMTJobtable();
                cls_MTJobtable model = new cls_MTJobtable();
                model.company_code = input.company_code;
                model.jobtable_id = input.jobtable_id;
                model.job_id = input.job_id;
                model.job_type = input.job_type;
                model.status_job = input.status_job;
                model.job_nextstep = input.job_nextstep;
                model.job_date = Convert.ToDateTime(input.job_date);
                model.job_finishdate = Convert.ToDateTime(input.job_finishdate);
                model.workflow_code = input.workflow_code;

                model.created_by = input.username;
                string strID = objMTJob.insert(model);
                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objMTJob.getMessage();
                }

                objMTJob.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteeMTJobtable(InputMTJobtable input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF014.3";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTJobtable controller = new cls_ctMTJobtable();
                bool blnResult = controller.delete(input.company_code, input.jobtable_id, input.job_id, input.job_type);
                if (blnResult)
                {
                    output["success"] = true;
                    output["message"] = "Remove data successfully";

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Remove data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }



            return output.ToString(Formatting.None);

        }
        #endregion

        #region MTReqdocument
        public string getMTReqdocumentList(InputMTReqdocument input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF015.1";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctMTReqdocument objMTReqdocument = new cls_ctMTReqdocument();
                List<cls_MTReqdocument> list = objMTReqdocument.getDataByFillter(input.company_code, input.document_id, input.job_id, input.job_type);

                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTReqdocument model in list)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("jobtable_id", model.document_id);
                        json.Add("job_id", model.job_id);
                        json.Add("job_type", model.job_type);
                        json.Add("status_job", model.document_name);
                        json.Add("job_nextstep", model.document_type);
                        json.Add("job_date", model.document_path);

                        json.Add("created_by", model.created_by);
                        json.Add("created_date", model.created_date);

                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }

                    output["result"] = "1";
                    output["result_text"] = "1";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageMTReqdocument(InputMTReqdocument input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF015.2";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctMTReqdocument objMTReqdocu = new cls_ctMTReqdocument();
                cls_MTReqdocument model = new cls_MTReqdocument();
                model.company_code = input.company_code;
                model.document_id = input.document_id;
                model.job_id = input.job_id;
                model.job_type = input.job_type;
                model.document_name = input.document_name;
                model.document_type = input.document_type;
                model.document_path = input.document_path;

                model.created_by = input.username;
                string strID = objMTReqdocu.insert(model);
                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objMTReqdocu.getMessage();
                }

                objMTReqdocu.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteeMTReqdocument(InputMTReqdocument input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF015.3";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTReqdocument controller = new cls_ctMTReqdocument();
                bool blnResult = controller.delete(input.company_code, input.document_id, input.job_id, input.job_type);
                if (blnResult)
                {
                    cls_ctMTReqdocument MTReqdoc = new cls_ctMTReqdocument();
                    List<cls_MTReqdocument> filelist = MTReqdoc.getDataByFillter(input.company_code, input.document_id, input.job_id, input.job_type);
                    if (filelist.Count > 0)
                    {
                        foreach (cls_MTReqdocument filedata in filelist)
                        {
                            File.Delete(filedata.document_path);
                        }
                    }
                    output["success"] = true;
                    output["message"] = "Remove data successfully";

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Remove data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }
            return output.ToString(Formatting.None);

        }
        #endregion

        #region MTWorkflow
        public string getMTWorkflowList(InputMTWorkflow input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF001.1";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctMTWorkflow objMT = new cls_ctMTWorkflow();
                List<cls_MTWorkflow> list = objMT.getDataByFillter(input.company_code, input.workflow_id, input.workflow_code, input.workflow_type);

                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTWorkflow model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("workflow_id", model.workflow_id);
                        json.Add("workflow_code", model.workflow_code);
                        json.Add("workflow_name_th", model.workflow_name_th);
                        json.Add("workflow_name_en", model.workflow_name_en);
                        json.Add("workflow_type", model.workflow_type);

                        json.Add("step1", model.step1);
                        json.Add("step2", model.step2);
                        json.Add("step3", model.step3);
                        json.Add("step4", model.step4);
                        json.Add("step5", model.step5);

                        json.Add("totalapprove", model.totalapprove);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        cls_ctTRLineapprove objTRLineapp = new cls_ctTRLineapprove();
                        List<cls_TRLineapprove> listTRLineapp = objTRLineapp.getDataByFillter(model.company_code, model.workflow_type, model.workflow_code, "");
                        JArray arrayTRLineapp = new JArray();
                        if (listTRLineapp.Count > 0)
                        {
                            int indexTRLineapp = 1;

                            foreach (cls_TRLineapprove modelTRLineapp in listTRLineapp)
                            {
                                JObject jsonTRPlan = new JObject();
                                jsonTRPlan.Add("company_code", modelTRLineapp.company_code);
                                jsonTRPlan.Add("workflow_type", modelTRLineapp.workflow_type);
                                jsonTRPlan.Add("workflow_code", modelTRLineapp.workflow_code);
                                jsonTRPlan.Add("position_level", modelTRLineapp.position_level);

                                jsonTRPlan.Add("index", indexTRLineapp);


                                indexTRLineapp++;

                                arrayTRLineapp.Add(jsonTRPlan);
                            }
                            json.Add("lineapprove_data", arrayTRLineapp);
                        }
                        else
                        {
                            json.Add("lineapprove_data", arrayTRLineapp);
                        }
                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }

                    output["result"] = "1";
                    output["result_text"] = "1";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageMTWorkflow(InputMTWorkflow input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF001.2";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctMTWorkflow objMT = new cls_ctMTWorkflow();
                cls_MTWorkflow model = new cls_MTWorkflow();
                model.company_code = input.company_code;
                model.workflow_id = input.workflow_id.Equals("") ? 0 : Convert.ToInt32(input.workflow_id);
                model.workflow_code = input.workflow_code;
                model.workflow_name_th = input.workflow_name_th;
                model.workflow_name_en = input.workflow_name_en;
                model.workflow_type = input.workflow_type;

                model.step1 = input.step1;
                model.step2 = input.step2;
                model.step3 = input.step3;
                model.step4 = input.step4;
                model.step5 = input.step5;

                model.totalapprove = input.totalapprove;

                model.modified_by = input.username;
                model.flag = input.flag;

                string strID = objMT.insert(model);
                if (!strID.Equals(""))
                {
                    try
                    {
                        cls_ctTRLineapprove objTRLineapp = new cls_ctTRLineapprove();
                        //objTRLineapp.delete(input.company_code, input.workflow_type,input.workflow_code,"");
                        if (input.lineapprove_data.Count > 0)
                        {
                            objTRLineapp.delete(input.company_code, input.workflow_type, input.workflow_code, "");
                            objTRLineapp.insert(input.lineapprove_data, input.username);
                        }
                        else
                        {
                            objTRLineapp.delete(input.company_code, input.workflow_type, input.workflow_code, "");
                        }
                    }
                    catch (Exception ex)
                    {
                        string str = ex.ToString();
                    }
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objMT.getMessage();
                }

                objMT.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteMTWorkflow(InputMTWorkflow input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF001.3";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTWorkflow controller = new cls_ctMTWorkflow();

                bool blnResult = controller.delete(input.workflow_id, input.company_code);

                if (blnResult)
                {
                    try
                    {
                        cls_ctTRLineapprove objTRLineapp = new cls_ctTRLineapprove();
                        objTRLineapp.delete(input.company_code, input.workflow_type, input.workflow_code, "");
                    }
                    catch (Exception ex)
                    {
                        string str = ex.ToString();
                    }
                    output["success"] = true;
                    output["message"] = "Remove data successfully";

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Remove data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }



            return output.ToString(Formatting.None);

        }
        public string getPositionLevelList(InputMTWorkflow input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF001.5";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctMTWorkflow objMT = new cls_ctMTWorkflow();
                List<cls_MTWorkflow> list = objMT.getpositionlevel(input.company_code);

                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTWorkflow model in list)
                    {
                        JObject json = new JObject();
                        json.Add("position_level", model.position_level);
                        json.Add("index", index);

                        index++;

                        array.Add(model.position_level);
                    }

                    output["result"] = "1";
                    output["result_text"] = "1";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        #endregion

        #region TRLineapprove
        public string getTRLineapproveList(InputTRLineapprove input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF003.1";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctTRLineapprove objTRLineapprove = new cls_ctTRLineapprove();
                List<cls_TRLineapprove> list = objTRLineapprove.getDataByFillter(input.company_code, input.workflow_type, input.workflow_code, input.position_level);

                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRLineapprove model in list)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("workflow_type", model.workflow_type);
                        json.Add("workflow_code", model.workflow_code);
                        json.Add("position_level", model.position_level);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);

                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }

                    output["result"] = "1";
                    output["result_text"] = "1";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageTRLineapprove(InputTRLineapprove input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF003.2";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctTRLineapprove objTRTimeleave = new cls_ctTRLineapprove();
                bool strID = objTRTimeleave.insert(input.lineapprove_data, input.username);
                if (strID)
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objTRTimeleave.getMessage();
                }

                objTRTimeleave.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteTRLineapprove(InputTRLineapprove input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF003.3";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctTRLineapprove controller = new cls_ctTRLineapprove();
                bool blnResult = controller.delete(input.company_code, input.workflow_type, input.workflow_code, input.position_level);
                if (blnResult)
                {
                    output["success"] = true;
                    output["message"] = "Remove data successfully";

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Remove data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }



            return output.ToString(Formatting.None);

        }
        #endregion

        #region MTAccount
        public string getMTAccountList(InputMTAccount input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF002.1";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctMTAccount objMTAccount = new cls_ctMTAccount();
                List<cls_MTAccount> list = objMTAccount.getDataByFillter(input.company_code, input.account_user, input.account_type);

                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTAccount model in list)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("account_user", model.account_user);
                        //json.Add("account_pwd", model.account_pwd);
                        json.Add("account_pwd", "");
                        json.Add("account_type", model.account_type);
                        json.Add("account_level", model.account_level);
                        json.Add("account_email", model.account_email);
                        json.Add("account_email_alert", model.account_email_alert);
                        json.Add("account_line", model.account_line);
                        json.Add("account_line_alert", model.account_line_alert);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        cls_ctTRAccountpos objTRAccountpos = new cls_ctTRAccountpos();
                        List<cls_TRAccountpos> listTRAccountpos = objTRAccountpos.getDataByFillter(model.company_code, model.account_user, model.account_type, "");
                        JArray arrayTRAccountpos = new JArray();
                        if (listTRAccountpos.Count > 0)
                        {
                            int indexTRAccount = 1;

                            foreach (cls_TRAccountpos modelTRAccount in listTRAccountpos)
                            {
                                JObject jsonTRPlan = new JObject();
                                jsonTRPlan.Add("company_code", modelTRAccount.company_code);
                                jsonTRPlan.Add("account_user", modelTRAccount.account_user);
                                jsonTRPlan.Add("account_type", modelTRAccount.account_type);
                                jsonTRPlan.Add("position_code", modelTRAccount.position_code);

                                jsonTRPlan.Add("index", indexTRAccount);


                                indexTRAccount++;

                                arrayTRAccountpos.Add(jsonTRPlan);
                            }
                            json.Add("position_data", arrayTRAccountpos);
                        }
                        else
                        {
                            json.Add("position_data", arrayTRAccountpos);
                        }
                        cls_ctTRAccountdep objTRAccountdep = new cls_ctTRAccountdep();
                        List<cls_TRAccountdep> listTRAccountdep = objTRAccountdep.getDataByFillter(model.company_code, model.account_user, model.account_type, "", "");
                        JArray arrayTRAccountdep = new JArray();
                        if (listTRAccountdep.Count > 0)
                        {
                            int indexTRAccountdep = 1;

                            foreach (cls_TRAccountdep modelTRAccountdep in listTRAccountdep)
                            {
                                JObject jsonTRdep = new JObject();
                                jsonTRdep.Add("company_code", modelTRAccountdep.company_code);
                                jsonTRdep.Add("account_user", modelTRAccountdep.account_user);
                                jsonTRdep.Add("account_type", modelTRAccountdep.account_type);
                                jsonTRdep.Add("level_code", modelTRAccountdep.level_code);
                                jsonTRdep.Add("dep_code", modelTRAccountdep.dep_code);

                                jsonTRdep.Add("index", indexTRAccountdep);


                                indexTRAccountdep++;

                                arrayTRAccountdep.Add(jsonTRdep);
                            }
                            json.Add("dep_data", arrayTRAccountdep);
                        }
                        else
                        {
                            json.Add("dep_data", arrayTRAccountdep);
                        }

                        cls_ctTRAccount objTRAccount = new cls_ctTRAccount();
                        List<cls_TRAccount> listTRAccount = objTRAccount.getDataByFillter(model.company_code, model.account_user, model.account_type, "");
                        JArray arrayTRAccount = new JArray();
                        if (listTRAccount.Count > 0)
                        {
                            int indexTRAccount = 1;

                            foreach (cls_TRAccount modelTRAccount in listTRAccount)
                            {
                                JObject jsonTRdep = new JObject();
                                jsonTRdep.Add("company_code", modelTRAccount.company_code);
                                jsonTRdep.Add("account_user", modelTRAccount.account_user);
                                jsonTRdep.Add("account_type", modelTRAccount.account_type);
                                jsonTRdep.Add("worker_code", modelTRAccount.worker_code);
                                jsonTRdep.Add("worker_detail_en", modelTRAccount.worker_detail_en);
                                jsonTRdep.Add("worker_detail_th", modelTRAccount.worker_detail_th);

                                jsonTRdep.Add("index", indexTRAccount);


                                indexTRAccount++;

                                arrayTRAccount.Add(jsonTRdep);
                            }
                            json.Add("worker_data", arrayTRAccount);
                        }
                        else
                        {
                            json.Add("worker_data", arrayTRAccount);
                        }

                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }

                    output["result"] = "1";
                    output["result_text"] = "1";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageMTAccount(InputMTAccount input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF002.2";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctMTAccount objMTAccount = new cls_ctMTAccount();
                cls_MTAccount model = new cls_MTAccount();
                Authen objAuthen = new Authen();
                model.company_code = input.company_code;
                model.account_user = input.account_user;
                //model.account_pwd = objAuthen.Encrypt(input.account_pwd);
                model.account_pwd = input.account_pwd;
                model.account_type = input.account_type;
                model.account_level = input.account_level;
                model.account_email = input.account_email;
                model.account_email_alert = input.account_email_alert;
                model.account_line = input.account_line;
                model.account_line_alert = input.account_line_alert;

                model.modified_by = input.username;
                model.flag = input.flag;
                string strID = objMTAccount.insert(model);
                if (!strID.Equals(""))
                {
                    try
                    {
                        cls_ctTRAccountpos objTRAccoutpos = new cls_ctTRAccountpos();
                        cls_ctTRAccountdep objTRAccoutdep = new cls_ctTRAccountdep();
                        cls_ctTRAccount objTRAccout = new cls_ctTRAccount();
                        //objTRLineapp.delete(input.company_code, input.workflow_type,input.workflow_code,"");
                        if (input.positonn_data.Count > 0)
                        {
                            objTRAccoutpos.insert(input.positonn_data);
                        }
                        else
                        {
                            objTRAccoutpos.delete(input.company_code, input.account_user, input.account_type);
                        }
                        if (input.dep_data.Count > 0)
                        {
                            objTRAccoutdep.insert(input.dep_data);
                        }
                        else
                        {
                            objTRAccoutdep.delete(input.company_code, input.account_user, input.account_type, "", "");
                        }
                        if (input.worker_data.Count > 0)
                        {
                            objTRAccout.insert(input.worker_data);
                        }
                        else
                        {
                            objTRAccout.delete(input.company_code, input.account_user, input.account_type, "");
                        }

                    }
                    catch (Exception ex)
                    {
                        string str = ex.ToString();
                    }
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objMTAccount.getMessage();
                }

                objMTAccount.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteeMTAccount(InputMTAccount input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF002.3";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTAccount controller = new cls_ctMTAccount();
                bool blnResult = controller.delete(input.company_code, input.account_user, input.account_type);
                if (blnResult)
                {
                    try
                    {
                        cls_ctTRAccountpos objTRpos = new cls_ctTRAccountpos();
                        cls_ctTRAccountdep objTRdep = new cls_ctTRAccountdep();
                        cls_ctTRAccount objTRAcount = new cls_ctTRAccount();
                        objTRpos.delete(input.company_code, input.account_user, input.account_type);
                        objTRdep.delete(input.company_code, input.account_user, input.account_type, "", "");
                        objTRAcount.delete(input.company_code, input.account_user, input.account_type, "");
                    }
                    catch (Exception ex)
                    {
                        string str = ex.ToString();
                    }
                    output["success"] = true;
                    output["message"] = "Remove data successfully";

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Remove data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }



            return output.ToString(Formatting.None);

        }
        public string getMTAccountworkflowList(InputMTAccount input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF002.5";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctTRAccount objTRAccount = new cls_ctTRAccount();
                List<cls_TRAccount> listTRAccount = objTRAccount.getDataworkflowByFillter(input.company_code, input.account_user, input.worker_code, input.account_type, input.workflow_type);

                JArray array = new JArray();

                if (listTRAccount.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRAccount model in listTRAccount)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("account_user", model.account_user);
                        json.Add("account_type", model.account_type);
                        json.Add("worker_code", model.worker_code);
                        json.Add("empposition_position", model.empposition_position);
                        json.Add("position_level", model.position_level);
                        json.Add("workflow_code", model.workflow_code);
                        json.Add("workflow_type", model.workflow_type);
                        json.Add("totalapprove", model.totalapprove);
                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }

                    output["result"] = "1";
                    output["result_text"] = "1";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        #endregion

        #region TRAccountpos
        public string getTRAccountposList(InputTRAccountpos input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF016.1";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctTRAccountpos objMTAccountpost = new cls_ctTRAccountpos();
                List<cls_TRAccountpos> list = objMTAccountpost.getDataByFillter(input.company_code, input.account_user, input.account_type, input.position_code);

                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRAccountpos model in list)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("account_user", model.account_user);
                        json.Add("account_type", model.account_type);
                        json.Add("position_code", model.position_code);
                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }

                    output["result"] = "1";
                    output["result_text"] = "1";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageTRAccountpos(InputTRAccountpos input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF016.2";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctTRAccountpos objTRAccountpos = new cls_ctTRAccountpos();
                cls_TRAccountpos model = new cls_TRAccountpos();
                model.company_code = input.company_code;
                model.account_user = input.account_user;
                model.account_type = input.account_type;
                model.position_code = input.position_code;
                string strID = objTRAccountpos.insert(model);
                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objTRAccountpos.getMessage();
                }

                objTRAccountpos.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteeTRAccountpos(InputTRAccountpos input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF016.3";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctTRAccountpos controller = new cls_ctTRAccountpos();
                bool blnResult = controller.delete(input.company_code, input.account_user, input.account_type);
                if (blnResult)
                {
                    output["success"] = true;
                    output["message"] = "Remove data successfully";

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Remove data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }



            return output.ToString(Formatting.None);

        }
        #endregion

        #region TRAccountdep
        public string getTRAccountdepList(InputTRAccountdep input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF017.1";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctTRAccountdep objMTAccountpost = new cls_ctTRAccountdep();
                List<cls_TRAccountdep> list = objMTAccountpost.getDataByFillter(input.company_code, input.account_user, input.account_type, input.level_code, input.dep_code);

                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRAccountdep model in list)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("account_user", model.account_user);
                        json.Add("account_type", model.account_type);
                        json.Add("level_code", model.level_code);
                        json.Add("dep_code", model.dep_code);
                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }

                    output["result"] = "1";
                    output["result_text"] = "1";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageTRAccountdep(InputTRAccountdep input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF017.2";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctTRAccountdep objTRAccountdep = new cls_ctTRAccountdep();
                cls_TRAccountdep model = new cls_TRAccountdep();
                model.company_code = input.company_code;
                model.account_user = input.account_user;
                model.account_type = input.account_type;
                model.level_code = input.level_code;
                model.dep_code = input.dep_code;
                string strID = objTRAccountdep.insert(model);
                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objTRAccountdep.getMessage();
                }

                objTRAccountdep.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteeTRAccountdep(InputTRAccountdep input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF017.3";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctTRAccountdep controller = new cls_ctTRAccountdep();
                bool blnResult = controller.delete(input.company_code, input.account_user, input.account_type, input.level_code, input.dep_code);
                if (blnResult)
                {
                    output["success"] = true;
                    output["message"] = "Remove data successfully";

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Remove data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }



            return output.ToString(Formatting.None);

        }
        #endregion


        public string Approvegetdoc(InputApprovedoc input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF018.1";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ApproveJob controller = new cls_ApproveJob();
                JArray countdoc = new JArray();
                JArray list = controller.ApproveJob_get(input.company_code, input.job_type, input.username,input.status,input.fromdate,input.todate);
                JObject jsonCount = new JObject();
                jsonCount.Add("docapprove_wait", controller.getCountDoc(input.company_code, input.job_type, input.username, "0", DateTime.Now.Year.ToString() + "-01-01", DateTime.Now.Year.ToString() + "-12-31"));
                jsonCount.Add("docapprove_all", controller.getCountDoc(input.company_code, input.job_type, input.username, "1", DateTime.Now.Year.ToString() + "-01-01", DateTime.Now.Year.ToString() + "-12-31"));
                jsonCount.Add("docapprove_approve", controller.getCountDoc(input.company_code, input.job_type, input.username, "3", DateTime.Now.Year.ToString() + "-01-01", DateTime.Now.Year.ToString() + "-12-31"));
                jsonCount.Add("docapprove_reject", controller.getCountDoc(input.company_code, input.job_type, input.username, "4", DateTime.Now.Year.ToString() + "-01-01", DateTime.Now.Year.ToString() + "-12-31"));
                countdoc.Add(jsonCount);
                output["result"] = "1";
                output["result_text"] = "1";
                output["data"] = list;
                output["total"] = countdoc;
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string Approve(InputApprovedoc input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SELF018.2";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ApproveJob controller = new cls_ApproveJob();
                int success = 0;
                int fail = 0;
                JArray array = new JArray();
                foreach (string id in input.job_id)
                {
                    JObject json = new JObject();
                    bool Status = false;
                    string result = controller.ApproveJob(ref Status, input.company_code, id, input.job_type, input.username, input.approve_status, input.lang);
                    if (Status)
                    {
                        success++;
                    }
                    else
                    {
                        fail++;
                        json.Add("job_id", id);
                        json.Add("job_type", input.job_type);
                        json.Add("file_detail", result);
                        array.Add(json);
                    }
                    Status = false;
                }
                output["result"] = "1";
                output["result_text"] = "1";
                output["data"] = array;
                output["success"] = success;
                output["fail"] = fail;
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();

            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
    }
}
