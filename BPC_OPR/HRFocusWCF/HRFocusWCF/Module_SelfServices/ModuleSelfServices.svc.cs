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

        #region TRTimeleave
        public string getTRTimeleaveList(InputTRTimeleave input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self007";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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

                cls_ctTRTimeleave objTRTimeleave = new cls_ctTRTimeleave();
                List<cls_TRTimeleave> listTRTimeleave = objTRTimeleave.getDataByFillter(input.timeleave_id,input.status, input.company_code, input.worker_code, datefrom, dateto);

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
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return output.ToString(Formatting.None);
        }
        public string doManageTRTimeleave(InputTRTimeleave input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self007";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteTRTimeleave(InputTRTimeleave input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self007";
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

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        #endregion

        #region TRTimeot
        public string getTRTimeotList(InputTRTimeot input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self008";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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
                DateTime datefrom = Convert.ToDateTime(input.timeot_fromdate);
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
                        json.Add("timeot_break", model.timeot_breakmin);
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
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return output.ToString(Formatting.None);
        }
        public string doManageTRTimeot(InputTRTimeot input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self008";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objTRTime.getMessage();
                }

                objTRTime.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteTRTimeot(InputTRTimeot input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self008";
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

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        #endregion

        #region TRTimeshift
        public string getTRTimeshiftList(InputTRTimeshift input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self009";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return output.ToString(Formatting.None);
        }
        public string doManageTRTimeshift(InputTRTimeshift input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self009";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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

                    model.timeshift_old = shiftdata.timeshift_old;
                    model.timeshift_new = shiftdata.timeshift_new;

                    model.timeshift_note = shiftdata.timeshift_note;

                    model.reason_code = shiftdata.reason_code;
                    model.status = shiftdata.status;

                    model.modified_by = input.username;
                    model.flag = shiftdata.flag;

                    strID = objTRTime.insert(model);
                    if (!strID.Equals(""))
                    {

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
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objTRTime.getMessage();
                }

                objTRTime.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteTRTimeshift(InputTRTimeshift input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self009";
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
                    List<cls_TRTimecard> list_timecard = objTRTimecard.getDataByFillter(model.company_code,"", model.worker_code, model.timeshift_workdate.Date, model.timeshift_workdate.Date);

                    if (list_timecard.Count > 0)
                    {
                        cls_TRTimecard timecard = list_timecard[0];
                        timecard.shift_code = model.timeshift_old;

                        objTRTimecard.update(timecard);
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

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        #endregion

        #region TRTimecheckin
        public string getTRTimecheckinList(InputTRTimecheckin input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self012";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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
                List<cls_TRTimecheckin> listTRTimecheckin = objTRTimecheckin.getDataByFillter(input.company_code, input.timecheckin_id, input.timecheckin_time, input.timecheckin_type, input.location_code, input.worker_code, input.timecheckin_workdate, input.timecheckin_workdate_to);

                JArray array = new JArray();

                if (listTRTimecheckin.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRTimecheckin model in listTRTimecheckin)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("timecheckin_id", model.timecheckin_id);
                        json.Add("timecheckin_workdate", model.timecheckin_workdate.ToString("yyyy-MM-dd"));
                        json.Add("timecheckin_time", model.timecheckin_time);
                        json.Add("timecheckin_type", model.timecheckin_type);
                        json.Add("timecheckin_lat", model.timecheckin_lat);
                        json.Add("timecheckin_long", model.timecheckin_long);
                        json.Add("timecheckin_note", model.timecheckin_note);
                        json.Add("location_code", model.location_code);
                        json.Add("worker_code", model.worker_code);
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
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return output.ToString(Formatting.None);
        }
        public string doManageTRTimecheckin(InputTRTimecheckin input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self012";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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
                foreach (cls_TRTimecheckin otdata in jsonArray)
                {
                    cls_TRTimecheckin model = new cls_TRTimecheckin();

                    model.company_code = otdata.company_code;
                    model.timecheckin_id = otdata.timecheckin_id.Equals("") ? 0 : Convert.ToInt32(otdata.timecheckin_id);
                    model.timecheckin_workdate = Convert.ToDateTime(otdata.timecheckin_workdate);
                    model.timecheckin_time = otdata.timecheckin_time;
                    model.timecheckin_type = otdata.timecheckin_type;
                    model.timecheckin_lat = otdata.timecheckin_lat;
                    model.timecheckin_long = otdata.timecheckin_long;
                    model.timecheckin_note = otdata.timecheckin_note;
                    model.location_code = otdata.location_code;
                    model.worker_code = otdata.worker_code;
                    model.modified_by = input.username;
                    model.flag = otdata.flag;

                    strID = objTRTimecheckin.insert(model);
                    if (!strID.Equals(""))
                    {

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
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objTRTimecheckin.getMessage();
                }

                objTRTimecheckin.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteTRTimecheckin(InputTRTimecheckin input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self012";
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

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        #endregion

        #region TRTimedaytype
        public string getTRTimedaytypeList(InputTRTimedaytype input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self011";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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
                List<cls_TRTimedaytype> listTRTimedaytype = objTRTimedaytype.getDataByFillter(input.company_code,input.timedaytype_id,input.worker_code,input.timedaytype_workdate,input.timedaytype_workdate_to,input.status);

                JArray array = new JArray();

                if (listTRTimedaytype.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRTimedaytype model in listTRTimedaytype)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("timedaytype_id", model.timedaytype_id);
                        json.Add("timedaytype_doc", model.timedaytype_doc);
                        json.Add("timedaytype_workdate", model.timedaytype_workdate.ToString("yyyy-MM-dd"));
                        json.Add("timedaytype_old", model.timedaytype_old);
                        json.Add("timedaytype_new", model.timedaytype_new);
                        json.Add("timedaytype_note", model.timedaytype_note);
                        json.Add("reason_code", model.reason_code);
                        json.Add("status", model.status);

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
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return output.ToString(Formatting.None);
        }
        public string doManageTRTimedaytype(InputTRTimedaytype input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self011";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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
                    model.timedaytype_old = data.timedaytype_old;
                    model.timedaytype_new = data.timedaytype_new;
                    model.timedaytype_note = data.timedaytype_note;
                    model.reason_code = data.reason_code;
                    model.status = data.status;
                    model.modified_by = input.username;
                    model.flag = data.flag;

                    strID = objTRTimedaytype.insert(model);
                    if (!strID.Equals(""))
                    {

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
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objTRTimedaytype.getMessage();
                }

                objTRTimedaytype.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteTRTimedaytype(InputTRTimedaytype input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self011";
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

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        #endregion

        #region TRTimeonsite
        public string getTRTimeonsiteList(InputTRTimeonsite input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self010";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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
                List<cls_TRTimeonsite> listTRTimeonsite = objTRTimeonsite.getDataByFillter(input.company_code,input.timeonsite_id,input.location_code,input.worker_code,input.timeonsite_workdate,input.timeonsite_workdate_to,input.status);

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
                        //cls_ctMTReason objRason = new cls_ctMTReason();
                        //List<cls_MTReason> listReason = objRason.getDataByFillter("ONS", "", model.reason_code, model.company_code);
                        //json.Add("reason_name_th", listReason[0].reason_name_th);
                        //json.Add("reason_name_en", listReason[0].reason_name_en);
                        json.Add("location_code", model.location_code);
                        //cls_ctMTLocation objlocation = new cls_ctMTLocation();
                        //List<cls_MTLocation> listlocation = objlocation.getDataByFillter("",model.location_code,model.company_code);
                        //json.Add("location_name_th", listlocation[0].location_name_th);
                        //json.Add("location_name_en", listlocation[0].location_name_en);
                        json.Add("worker_code", model.worker_code);
                        json.Add("status", model.status);

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
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return output.ToString(Formatting.None);
        }
        public string doManageTRTimeonsite(InputTRTimeonsite input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self010";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objTRTimeonsite.getMessage();
                }

                objTRTimeonsite.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteTRTimeonsite(InputTRTimeonsite input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self010";
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
                bool blnResult = controller.delete(input.company_code,input.timeonsite_id,input.worker_code);

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        #endregion

        #region MTArea
        public string getMTAreaList(InputMTArea input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self002";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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
                List<cls_MTArea> list = objMTArea.getDataByFillter(input.company_code,input.area_id,input.location_code,input.project_code);

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
                        List<cls_TRArea> listTRArea = objTRArea.getDataByFillter(model.company_code,model.location_code,"");
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
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return output.ToString(Formatting.None);
        }
        public string doManageMTArea(InputMTArea input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self002";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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
                            objTRArea.delete(input.company_code,input.location_code,"");
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

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteeMTArea(InputMTArea input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self002";
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
                bool blnResult = controller.delete(input.company_code,input.area_id,input.location_code);
                if (blnResult)
                {
                    try
                    {
                        cls_ctTRArea objTRArea = new cls_ctTRArea();
                        objTRArea.delete(input.company_code,input.location_code,"");
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

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        #endregion

        #region MTWorkflow
        public string getMTWorkflowList(InputMTWorkflow input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self001";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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
                List<cls_MTWorkflow> list = objMT.getDataByFillter(input.company_code, input.workflow_id, input.workflow_code,input.workflow_type);

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
                        List<cls_TRLineapprove> listTRLineapp = objTRLineapp.getDataByFillter(model.company_code, model.workflow_type,model.workflow_code,"");
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
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return output.ToString(Formatting.None);
        }
        public string doManageMTWorkflow(InputMTWorkflow input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self001";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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
                            objTRLineapp.insert(input.lineapprove_data,input.username);
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

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteMTWorkflow(InputMTWorkflow input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self001";
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

                bool blnResult = controller.delete(input.workflow_id,input.company_code);

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public string getPositionLevelList(InputMTWorkflow input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self001";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return output.ToString(Formatting.None);
        }
        #endregion

        #region TRLineapprove
        public string getTRLineapproveList(InputTRLineapprove input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self003";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return output.ToString(Formatting.None);
        }
        public string doManageTRLineapprove(InputTRLineapprove input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self003";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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
                bool strID = objTRTimeleave.insert(input.lineapprove_data,input.username);
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

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteTRLineapprove(InputTRLineapprove input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self003";
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
                bool blnResult = controller.delete(input.company_code,input.workflow_type,input.workflow_code,input.position_level);
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

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        #endregion

        #region MTAccount
        public string getMTAccountList(InputMTAccount input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self002";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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
                        List<cls_TRAccountdep> listTRAccountdep = objTRAccountdep.getDataByFillter(model.company_code, model.account_user, model.account_type, "","");
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
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return output.ToString(Formatting.None);
        }
        public string doManageMTAccount(InputMTAccount input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self002";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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
                            objTRAccoutdep.delete(input.company_code, input.account_user, input.account_type,"","");
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

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteeMTAccount(InputMTAccount input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self002";
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

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        #endregion

        #region TRAccountpos
        public string getTRAccountposList(InputTRAccountpos input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self002";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return output.ToString(Formatting.None);
        }
        public string doManageTRAccountpos(InputTRAccountpos input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self002";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteeTRAccountpos(InputTRAccountpos input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self002";
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

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        #endregion

        #region TRAccountdep
        public string getTRAccountdepList(InputTRAccountdep input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self002";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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
                List<cls_TRAccountdep> list = objMTAccountpost.getDataByFillter(input.company_code, input.account_user, input.account_type, input.level_code,input.dep_code);

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
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return output.ToString(Formatting.None);
        }
        public string doManageTRAccountdep(InputTRAccountdep input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self002";
            log.apilog_by = input.username;
            log.apilog_data = "all";
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

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteeTRAccountdep(InputTRAccountdep input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "Self002";
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
                bool blnResult = controller.delete(input.company_code, input.account_user, input.account_type,input.level_code,input.dep_code);
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

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        #endregion
    }
}
