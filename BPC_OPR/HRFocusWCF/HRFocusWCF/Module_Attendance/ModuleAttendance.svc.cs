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
using ClassLibrary_BPC.hrfocus.controller.Attendance;
using ClassLibrary_BPC.hrfocus.model.Attendance;

namespace BPC_OPR
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ModuleAttendance : IModuleAttendance
    {
        BpcOpr objBpcOpr = new BpcOpr();

        private async Task<bool> doUploadFile(string fileName, Stream stream)
        {
            bool result = false;

            try
            {
                string FilePath = Path.Combine
                  (ClassLibrary_BPC.Config.PathFileImport + "\\Imports", fileName);

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

        private DateTime doConvertDate(string date)
        {
            DateTime result = DateTime.Now;
            try
            {
                result = Convert.ToDateTime(date);
            }
            catch { }

            return result;
        }


        
        private string doCheckDateTimeEmpty(DateTime date)
        {
            if (date.Date.ToString("dd/MM/yyyy").Equals("01/01/1900"))
                return "-";
            else
                return date.ToString("dd/MM/yyyy HH:mm:ss");
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
       
        #region MTPlanholiday
        public string getMTPlanholidayList(InputMTPlanholiday input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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
                cls_ctMTPlanholiday objPlanholiday = new cls_ctMTPlanholiday();
                List<cls_MTPlanholiday> listPlanholiday = objPlanholiday.getDataByFillter(input.company_code, input.planholiday_id, input.planholiday_code, input.year_code);

                JArray array = new JArray();

                if (listPlanholiday.Count > 0)
                {
                    int index = 1;
                    cls_ctTRHoliday objHoliday = new cls_ctTRHoliday();
                    foreach (cls_MTPlanholiday model in listPlanholiday)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("planholiday_id", model.planholiday_id);
                        json.Add("planholiday_code", model.planholiday_code);
                        json.Add("planholiday_name_th", model.planholiday_name_th);
                        json.Add("planholiday_name_en", model.planholiday_name_en);
                        json.Add("year_code", model.year_code);
                        List<cls_TRHoliday> listHoliday = objHoliday.getDataByFillter(model.company_code, model.planholiday_code);
                        JArray holidayarray = new JArray();
                        if (listHoliday.Count > 0)
                        {
                            int indexholiday = 1;
                            foreach (cls_TRHoliday modelholiday in listHoliday)
                            {
                                JObject jsonholiday = new JObject();
                                jsonholiday.Add("company_code", modelholiday.company_code);
                                jsonholiday.Add("holiday_date", modelholiday.holiday_date);
                                jsonholiday.Add("holiday_name_th", modelholiday.holiday_name_th);
                                jsonholiday.Add("holiday_name_en", modelholiday.holiday_name_en);
                                jsonholiday.Add("planholiday_code", modelholiday.planholiday_code);
                                jsonholiday.Add("holiday_daytype", modelholiday.holiday_daytype);
                                jsonholiday.Add("holiday_payper", modelholiday.holiday_payper);
                                jsonholiday.Add("index", indexholiday);
                                indexholiday++;
                                holidayarray.Add(jsonholiday);
                            }
                            json.Add("holiday_list", holidayarray);
                        }
                        else
                        {
                            json.Add("holiday_list", holidayarray);
                        }
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
        public string doManageMTPlanholiday(InputMTPlanholiday input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.2";
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
                cls_ctMTPlanholiday objPlanholiday = new cls_ctMTPlanholiday();
                cls_MTPlanholiday model = new cls_MTPlanholiday();
                model.company_code = input.company_code;
                model.planholiday_id = input.planholiday_id.Equals("") ? 0 : Convert.ToInt32(input.planholiday_id);
                model.planholiday_code = input.planholiday_code;
                model.planholiday_name_th = input.planholiday_name_th;
                model.planholiday_name_en = input.planholiday_name_en;
                model.year_code = input.year_code;
                model.modified_by = input.modified_by;
                model.flag = input.flag;
                string strID = objPlanholiday.insert(model);
                if (strID.Equals("D"))
                {
                    output["success"] = false;
                    output["message"] = "Duplicate Code";

                    log.apilog_status = "500";
                    log.apilog_message = "Duplicate Code";
                    objBpcOpr.doRecordLog(log);
                    return output.ToString(Formatting.None);
                }
                if (!strID.Equals(""))
                {
                    cls_ctTRHoliday objHoliday = new cls_ctTRHoliday();
                    bool trholiday = objHoliday.insert(input.company_code, input.planholiday_code, input.holiday_list);
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
                    log.apilog_message = objPlanholiday.getMessage();
                }

                objPlanholiday.dispose();
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
        public string doDeleteMTPlanholiday(InputMTPlanholiday input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.3";
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

                cls_ctMTPlanholiday controller = new cls_ctMTPlanholiday();

                bool blnResult = controller.delete(input.planholiday_id, input.company_code);

                if (blnResult)
                {
                    cls_ctTRHoliday objHoliday = new cls_ctTRHoliday();
                    bool trholiday = objHoliday.delete(input.company_code, input.planholiday_code, input.year_code);
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
        public async Task<string> doUploadMTPlanholiday(string token, string by, string fileName, Stream stream,string com)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.4";
            log.apilog_by = by;
            log.apilog_data = "Stream : " + fileName;

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
                    cls_srvAttendanceImport srv_import = new cls_srvAttendanceImport();
                    string tmp = srv_import.doImportExcel("HOLIDAY", fileName, by,com);

                    if (tmp.Equals(""))
                    {
                        output["success"] = false;
                        output["message"] = "company incorrect";
                    }
                    else
                    {
                        output["success"] = true;
                        output["message"] = tmp;
                    }

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

        #region MTShift
        public string getMTShiftList(InputMTShift input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT002.1";
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
                cls_ctMTShift objShift = new cls_ctMTShift();
                List<cls_MTShift> listShift = objShift.getDataByFillter(input.company_code, input.shift_id, input.shift_code,input.project);

                JArray array = new JArray();

                if (listShift.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTShift model in listShift)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("shift_id", model.shift_id);
                        json.Add("shift_code", model.shift_code);
                        json.Add("shift_name_th", model.shift_name_th);
                        json.Add("shift_name_en", model.shift_name_en);

                        json.Add("shift_ch1", model.shift_ch1);
                        json.Add("shift_ch2", model.shift_ch2);
                        json.Add("shift_ch3", model.shift_ch3);
                        json.Add("shift_ch4", model.shift_ch4);
                        json.Add("shift_ch5", model.shift_ch5);
                        json.Add("shift_ch6", model.shift_ch6);
                        json.Add("shift_ch7", model.shift_ch7);
                        json.Add("shift_ch8", model.shift_ch8);
                        json.Add("shift_ch9", model.shift_ch9);
                        json.Add("shift_ch10", model.shift_ch10);

                        json.Add("shift_ch3_from", model.shift_ch3_from);
                        json.Add("shift_ch3_to", model.shift_ch3_to);
                        json.Add("shift_ch4_from", model.shift_ch4_from);
                        json.Add("shift_ch4_to", model.shift_ch4_to);

                        json.Add("shift_ch7_from", model.shift_ch7_from);
                        json.Add("shift_ch7_to", model.shift_ch7_to);
                        json.Add("shift_ch8_from", model.shift_ch8_from);
                        json.Add("shift_ch8_to", model.shift_ch8_to);

                        json.Add("shift_otin_min", model.shift_otin_min);
                        json.Add("shift_otin_max", model.shift_otin_max);

                        json.Add("shift_otout_min", model.shift_otout_min);
                        json.Add("shift_otout_max", model.shift_otout_max);

                        json.Add("shift_flexiblebreak", model.shift_flexiblebreak);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        json.Add("project", model.project);
                        cls_ctTRShiftbreak objBreak = new cls_ctTRShiftbreak();
                        List<cls_TRShiftbreak> listBreak = objBreak.getDataByFillter(model.company_code, model.shift_code);
                        JArray arrayBreak = new JArray();
                        if (listBreak.Count > 0)
                        {
                            int indexBreak = 1;

                            foreach (cls_TRShiftbreak modelBreak in listBreak)
                            {
                                JObject jsonBreak = new JObject();

                                jsonBreak.Add("company_code", modelBreak.company_code);
                                jsonBreak.Add("shift_code", modelBreak.shift_code);
                                jsonBreak.Add("shiftbreak_no", modelBreak.shiftbreak_no);
                                jsonBreak.Add("shiftbreak_from", modelBreak.shiftbreak_from);
                                jsonBreak.Add("shiftbreak_to", modelBreak.shiftbreak_to);
                                jsonBreak.Add("shiftbreak_break", modelBreak.shiftbreak_break);

                                jsonBreak.Add("index", indexBreak);

                                indexBreak++;

                                arrayBreak.Add(jsonBreak);
                            }
                            json.Add("shift_break", arrayBreak);
                        }
                        else
                        {
                            json.Add("shift_break", arrayBreak);
                        }
                        cls_ctTRShiftallowance objAllowance = new cls_ctTRShiftallowance();
                        List<cls_TRShiftallowance> listAllowance = objAllowance.getDataByFillter(model.company_code, model.shift_code);
                        JArray arrayallowance = new JArray();
                        if (listAllowance.Count > 0)
                        {

                            int indexallowance = 1;

                            foreach (cls_TRShiftallowance modelallowance in listAllowance)
                            {
                                JObject jsonallowance = new JObject();

                                jsonallowance.Add("company_code", modelallowance.company_code);
                                jsonallowance.Add("shift_code", modelallowance.shift_code);
                                jsonallowance.Add("shiftallowance_no", modelallowance.shiftallowance_no);
                                jsonallowance.Add("shiftallowance_name_th", modelallowance.shiftallowance_name_th);
                                jsonallowance.Add("shiftallowance_name_en", modelallowance.shiftallowance_name_en);
                                jsonallowance.Add("shiftallowance_hhmm", modelallowance.shiftallowance_hhmm);
                                jsonallowance.Add("shiftallowance_amount", modelallowance.shiftallowance_amount);

                                jsonallowance.Add("index", indexallowance);

                                indexallowance++;

                                arrayallowance.Add(jsonallowance);
                            }
                            json.Add("shift_allowance", arrayallowance);
                        }
                        else
                        {
                            json.Add("shift_allowance", arrayallowance);
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
        public string doManageMTShift(InputMTShift input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT002.2";
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

                cls_ctMTShift objShift = new cls_ctMTShift();
                cls_MTShift model = new cls_MTShift();

                model.company_code = input.company_code;

                model.shift_id = input.shift_id.Equals("") ? 0 : Convert.ToInt32(input.shift_id);
                model.shift_code = input.shift_code;
                model.shift_name_th = input.shift_name_th;
                model.shift_name_en = input.shift_name_en;
                model.shift_ch1 = input.shift_ch1;
                model.shift_ch2 = input.shift_ch2;
                model.shift_ch3 = input.shift_ch3;
                model.shift_ch4 = input.shift_ch4;
                model.shift_ch5 = input.shift_ch5;
                model.shift_ch6 = input.shift_ch6;
                model.shift_ch7 = input.shift_ch7;
                model.shift_ch8 = input.shift_ch8;
                model.shift_ch9 = input.shift_ch9;
                model.shift_ch10 = input.shift_ch10;

                model.shift_ch3_from = input.shift_ch3_from;
                model.shift_ch3_to = input.shift_ch3_to;
                model.shift_ch4_from = input.shift_ch4_from;
                model.shift_ch4_to = input.shift_ch4_to;

                model.shift_ch7_from = input.shift_ch7_from;
                model.shift_ch7_to = input.shift_ch7_to;
                model.shift_ch8_from = input.shift_ch8_from;
                model.shift_ch8_to = input.shift_ch8_to;

                model.shift_otin_min = input.shift_otin_min;
                model.shift_otin_max = input.shift_otin_max;
                model.shift_otout_min = input.shift_otout_min;
                model.shift_otout_max = input.shift_otout_max;

                model.shift_flexiblebreak = input.shift_flexiblebreak;

                model.modified_by = input.modified_by;
                model.flag = input.flag;
                model.project = input.project;

                string strID = objShift.insert(model);
                if (strID.Equals("D"))
                {
                    output["success"] = false;
                    output["message"] = "Duplicate Code";

                    log.apilog_status = "500";
                    log.apilog_message = "Duplicate Code";
                    objBpcOpr.doRecordLog(log);
                    return output.ToString(Formatting.None);
                }
                if (!strID.Equals(""))
                {
                    cls_ctTRShiftbreak objbreak = new cls_ctTRShiftbreak();
                    cls_ctTRShiftallowance allowance = new cls_ctTRShiftallowance();
                    bool breaks = objbreak.insert(input.company_code, input.shift_code, input.shift_break);
                    bool allowances = allowance.insert(input.company_code, input.shift_code, input.shift_allowance);

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
                    log.apilog_message = objShift.getMessage();
                }

                objShift.dispose();
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
        public string doDeleteMTShift(InputMTShift input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT002.3";
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

                cls_ctMTShift controller = new cls_ctMTShift();

                bool blnResult = controller.delete(input.shift_id, input.company_code);

                if (blnResult)
                {
                    cls_ctTRShiftbreak objbreak = new cls_ctTRShiftbreak();
                    cls_ctTRShiftallowance allowance = new cls_ctTRShiftallowance();
                    bool breaks = objbreak.insert(input.company_code, input.shift_code, new List<cls_TRShiftbreak>());
                    bool allowances = allowance.delete(input.company_code, input.shift_code);
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
        public async Task<string> doUploadMTShift(string token, string by, string fileName, Stream stream,string com)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT002.4";
            log.apilog_by = by;
            log.apilog_data = "Stream : " + fileName; ;

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
                    cls_srvAttendanceImport srv_import = new cls_srvAttendanceImport();
                    string tmp = srv_import.doImportExcel("SHIFT", fileName, by,com);


                    if (tmp.Equals(""))
                    {
                        output["success"] = false;
                        output["message"] = "company incorrect";
                    }
                    else
                    {
                        output["success"] = true;
                        output["message"] = tmp;
                    }

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

        #region MTPlanshift
        public string getMTPlanshiftList(InputMTPlanshift input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT003.1";
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
                cls_ctMTPlanshift objPlanshift = new cls_ctMTPlanshift();
                List<cls_MTPlanshift> listPlanshift = objPlanshift.getDataByFillter(input.company_code, input.planshift_id, input.planshift_code);

                JArray array = new JArray();

                if (listPlanshift.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTPlanshift model in listPlanshift)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("planshift_id", model.planshift_id);
                        json.Add("planshift_code", model.planshift_code);
                        json.Add("planshift_name_th", model.planshift_name_th);
                        json.Add("planshift_name_en", model.planshift_name_en);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        cls_ctTRPlanschedule objPlanschedule = new cls_ctTRPlanschedule();
                        List<cls_TRPlanschedule> listPlanschedule = objPlanschedule.getDataByFillter(model.company_code, model.planshift_code);
                        JArray arrayPlanschedule = new JArray();
                        if (listPlanschedule.Count > 0)
                        {
                            int indexPlanschedule = 1;

                            foreach (cls_TRPlanschedule modelPlanschedule in listPlanschedule)
                            {
                                JObject jsonBreak = new JObject();

                                jsonBreak.Add("company_code", modelPlanschedule.company_code);
                                jsonBreak.Add("planshift_code", modelPlanschedule.planshift_code);
                                jsonBreak.Add("planschedule_fromdate", modelPlanschedule.planschedule_fromdate);
                                jsonBreak.Add("planschedule_todate", modelPlanschedule.planschedule_todate);
                                jsonBreak.Add("shift_code", modelPlanschedule.shift_code);
                                jsonBreak.Add("planschedule_sun_off", modelPlanschedule.planschedule_sun_off);
                                jsonBreak.Add("planschedule_mon_off", modelPlanschedule.planschedule_mon_off);
                                jsonBreak.Add("planschedule_tue_off", modelPlanschedule.planschedule_tue_off);
                                jsonBreak.Add("planschedule_wed_off", modelPlanschedule.planschedule_wed_off);
                                jsonBreak.Add("planschedule_thu_off", modelPlanschedule.planschedule_thu_off);
                                jsonBreak.Add("planschedule_fri_off", modelPlanschedule.planschedule_fri_off);
                                jsonBreak.Add("planschedule_sat_off", modelPlanschedule.planschedule_sat_off);

                                jsonBreak.Add("modified_by", modelPlanschedule.modified_by);
                                jsonBreak.Add("modified_date", modelPlanschedule.modified_date);
                                jsonBreak.Add("flag", modelPlanschedule.flag);

                                jsonBreak.Add("index", index);

                                indexPlanschedule++;

                                arrayPlanschedule.Add(jsonBreak);
                            }
                            json.Add("planschedule", arrayPlanschedule);
                        }
                        else
                        {
                            json.Add("planschedule", arrayPlanschedule);
                        }
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
        public string doManageMTPlanshift(InputMTPlanshift input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT003.2";
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
                cls_ctMTPlanshift objPlanshift = new cls_ctMTPlanshift();
                cls_MTPlanshift model = new cls_MTPlanshift();

                model.company_code = input.company_code;
                model.planshift_id = input.planshift_id.Equals("") ? 0 : Convert.ToInt32(input.planshift_id);
                model.planshift_code = input.planshift_code;

                model.planshift_name_th = input.planshift_name_th;
                model.planshift_name_en = input.planshift_name_en;
                model.modified_by = input.modified_by;
                model.flag = model.flag;

                string strID = objPlanshift.insert(model);

                if (strID.Equals("D"))
                {
                    output["success"] = false;
                    output["message"] = "Duplicate Code";

                    log.apilog_status = "500";
                    log.apilog_message = "Duplicate Code";
                    objBpcOpr.doRecordLog(log);
                    return output.ToString(Formatting.None);
                }
                if (!strID.Equals(""))
                {
                    cls_ctTRPlanschedule objPlanschedule = new cls_ctTRPlanschedule();
                    bool clear = objPlanschedule.clear(input.company_code, input.planshift_code);
                    if (input.planschedule.Count > 0)
                    {
                        foreach (cls_TRPlanschedule modelPlanschedule in input.planschedule)
                        {
                            bool res = objPlanschedule.insert(modelPlanschedule);
                        }
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
                    log.apilog_message = objPlanshift.getMessage();
                }

                objPlanshift.dispose();
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
        public string doDeleteMTPlanshift(InputMTPlanshift input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT003.3";
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

                cls_ctMTPlanshift controller = new cls_ctMTPlanshift();

                bool blnResult = controller.delete(input.planshift_id, input.company_code);

                if (blnResult)
                {
                    cls_ctTRPlanschedule objPlanschedule = new cls_ctTRPlanschedule();
                    bool res = objPlanschedule.clear(input.company_code, input.planshift_code);
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
        public async Task<string> doUploadMTPlanshift(string token, string by, string fileName, Stream stream,string com)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT003.4";
            log.apilog_by = by;
            log.apilog_data = "Stream : " + fileName; ;

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
                    cls_srvAttendanceImport srv_import = new cls_srvAttendanceImport();
                    string tmp = srv_import.doImportExcel("PLANSHIFT", fileName, by,com);


                    if (tmp.Equals(""))
                    {
                        output["success"] = false;
                        output["message"] = "company incorrect";
                    }
                    else
                    {
                        output["success"] = true;
                        output["message"] = tmp;
                    }
                    

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

        #region MTLeave
        public string getMTLeaveList(InputMTLeave input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT008.1";
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
                cls_ctMTLeave objLeave = new cls_ctMTLeave();
                List<cls_MTLeave> listLeave = objLeave.getDataByFillter(input.company_code, input.leave_id, input.leave_code);

                JArray array = new JArray();

                if (listLeave.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTLeave model in listLeave)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("leave_id", model.leave_id);
                        json.Add("leave_code", model.leave_code);
                        json.Add("leave_name_th", model.leave_name_th);
                        json.Add("leave_name_en", model.leave_name_en);
                        json.Add("leave_day_peryear", model.leave_day_peryear);
                        json.Add("leave_day_acc", model.leave_day_acc);
                        json.Add("leave_day_accexpire", model.leave_day_accexpire);
                        json.Add("leave_incholiday", model.leave_incholiday);
                        json.Add("leave_passpro", model.leave_passpro);
                        json.Add("leave_deduct", model.leave_deduct);
                        json.Add("leave_caldiligence", model.leave_caldiligence);
                        json.Add("leave_agework", model.leave_agework);
                        json.Add("leave_ahead", model.leave_ahead);
                        json.Add("leave_min_hrs", model.leave_min_hrs);
                        json.Add("leave_max_day", model.leave_max_day);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        cls_ctTRLeaveWorkage objWorkage = new cls_ctTRLeaveWorkage();
                        List<cls_TRLeaveWorkage> listWorkage = objWorkage.getDataByFillter(model.company_code, model.leave_code);
                        JArray arrayWorkage = new JArray();
                        if (listWorkage.Count > 0)
                        {
                            int indexWorkage = 1;

                            foreach (cls_TRLeaveWorkage modelWorkage in listWorkage)
                            {
                                JObject jsonWorkage = new JObject();
                                jsonWorkage.Add("leave_code", modelWorkage.leave_code);
                                jsonWorkage.Add("workage_from", modelWorkage.workage_from);
                                jsonWorkage.Add("workage_to", modelWorkage.workage_to);
                                jsonWorkage.Add("workage_leaveday", modelWorkage.workage_leaveday);

                                jsonWorkage.Add("index", indexWorkage);

                                indexWorkage++;

                                arrayWorkage.Add(jsonWorkage);
                            }
                            json.Add("leave_workage", arrayWorkage);
                        }
                        else
                        {
                            json.Add("leave_workage", arrayWorkage);
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
        public string doManageMTLeave(InputMTLeave input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT008.2";
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
                cls_ctMTLeave objLeave = new cls_ctMTLeave();
                cls_MTLeave model = new cls_MTLeave();
                model.company_code = input.company_code;
                model.leave_id = input.leave_id.Equals("") ? 0 : Convert.ToInt32(input.leave_id);
                model.leave_code = input.leave_code;
                model.leave_name_th = input.leave_name_th;
                model.leave_name_en = input.leave_name_en;
                model.leave_day_peryear = Convert.ToDouble(input.leave_day_peryear);
                model.leave_day_acc = Convert.ToDouble(input.leave_day_acc);

                string strExpire = "9999-12-31";
                try
                {
                    if (input.leave_day_accexpire != "")
                        strExpire = input.leave_day_accexpire;
                }
                catch { }

                model.leave_day_accexpire = Convert.ToDateTime(strExpire);
                model.leave_incholiday = input.leave_incholiday;
                model.leave_passpro = input.leave_passpro;
                model.leave_deduct = input.leave_deduct;
                model.leave_caldiligence = input.leave_caldiligence;
                model.leave_agework = input.leave_agework;
                model.leave_ahead = Convert.ToInt32(input.leave_ahead);
                model.leave_min_hrs = Convert.ToString(input.leave_min_hrs);
                model.leave_max_day = Convert.ToDouble(input.leave_max_day);

                model.modified_by = input.modified_by;
                model.flag = input.flag;

                string strID = objLeave.insert(model);
                if (strID.Equals("D"))
                {
                    output["success"] = false;
                    output["message"] = "Duplicate Code";

                    log.apilog_status = "500";
                    log.apilog_message = "Duplicate Code";
                    objBpcOpr.doRecordLog(log);
                    return output.ToString(Formatting.None);
                }
                if (!strID.Equals(""))
                {
                    try
                    {
                        cls_ctTRLeaveWorkage objTRWorkage = new cls_ctTRLeaveWorkage();
                        objTRWorkage.delete(input.company_code, input.leave_code);
                        if (input.leave_workage.Count > 0)
                        {
                            objTRWorkage.insert(input.leave_workage);
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
                    log.apilog_message = objLeave.getMessage();
                }

                objLeave.dispose();
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
        public string doDeleteMTLeave(InputMTLeave input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT008.3";
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

                cls_ctMTLeave controller = new cls_ctMTLeave();

                bool blnResult = controller.delete(input.leave_id, input.company_code);

                if (blnResult)
                {
                    cls_ctTRLeaveWorkage objTRWorkage = new cls_ctTRLeaveWorkage();
                    objTRWorkage.delete(input.company_code, input.leave_code);
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
        public async Task<string> doUploadMTLeave(string token, string by, string fileName, Stream stream,string com)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT008.4";
            log.apilog_by = by;
            log.apilog_data = "Stream : " + fileName; ;

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
                    cls_srvAttendanceImport srv_import = new cls_srvAttendanceImport();
                    string tmp = srv_import.doImportExcel("LEAVE", fileName, by,com);


                    if (tmp.Equals(""))
                    {
                        output["success"] = false;
                        output["message"] = "company incorrect";
                    }
                    else
                    {
                        output["success"] = true;
                        output["message"] = tmp;
                    }
                    

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

        #region TRLeave
        public string getTReaveList(InputTRLeave input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT010.1";
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
                cls_ctTREmpleaveacc objLeave = new cls_ctTREmpleaveacc();
                List<cls_TREmpleaveacc> listLeave = objLeave.getDataByFillter(input.company_code, input.worker_code, input.year_code);

                JArray array = new JArray();

                if (listLeave.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TREmpleaveacc model in listLeave)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("year_code", model.year_code);
                        json.Add("leave_code", model.leave_code);
                        cls_ctMTLeave objMTLeave = new cls_ctMTLeave();
                        List<cls_MTLeave> listMTLeave = objMTLeave.getDataByFillter(model.company_code, "", model.leave_code);
                        json.Add("leave_name_th", listMTLeave[0].leave_name_th);
                        json.Add("leave_name_en", listMTLeave[0].leave_name_en);
                        json.Add("empleaveacc_bf", model.empleaveacc_bf);
                        json.Add("empleaveacc_annual", model.empleaveacc_annual);
                        json.Add("empleaveacc_used", model.empleaveacc_used);
                        json.Add("empleaveacc_remain", model.empleaveacc_remain);
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
        public string doManageTReave(InputTRLeave input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT010.2";
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
                cls_ctTREmpleaveacc objLeave = new cls_ctTREmpleaveacc();
                cls_TREmpleaveacc model = new cls_TREmpleaveacc();
                model.company_code = input.company_code;
                model.leave_code = input.worker_code;
                model.year_code = input.year_code;
                model.leave_code = input.leave_code;
                model.empleaveacc_bf = input.empleaveacc_bf;
                model.empleaveacc_annual = input.empleaveacc_annual;
                model.empleaveacc_used = input.empleaveacc_used;
                model.empleaveacc_remain = input.empleaveacc_remain;

                model.modified_by = input.username;
                model.flag = input.flag;

                bool strID = objLeave.insert(model);
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
                    log.apilog_message = objLeave.getMessage();
                }

                objLeave.dispose();
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
        public string doDeleteTReave(InputTRLeave input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT010.3";
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

                cls_ctTREmpleaveacc controller = new cls_ctTREmpleaveacc();

                bool blnResult = controller.delete(input.company_code, input.worker_code, input.year_code);

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

        #region MTPlanleave
        public string getMTPlanleaveList(InputMTPlanleave input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT009.1";
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
                cls_ctMTPlanleave objPlan = new cls_ctMTPlanleave();
                List<cls_MTPlanleave> listPlan = objPlan.getDataByFillter(input.company_code, input.planleave_id, input.planleave_code);

                JArray array = new JArray();

                if (listPlan.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTPlanleave model in listPlan)
                    {
                        JObject json = new JObject();

                        json.Add("planleave_id", model.planleave_id);
                        json.Add("planleave_code", model.planleave_code);
                        json.Add("planleave_name_th", model.planleave_name_th);
                        json.Add("planleave_name_en", model.planleave_name_en);
                        json.Add("company_code", model.company_code);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        cls_ctTRPlanleave objTRPlan = new cls_ctTRPlanleave();
                        List<cls_TRPlanleave> listTRPlan = objTRPlan.getDataByFillter(model.company_code, model.planleave_code);
                        JArray arrayTRPlan = new JArray();
                        if (listTRPlan.Count > 0)
                        {
                            int indexTRPlan = 1;

                            foreach (cls_TRPlanleave modelTRPlan in listTRPlan)
                            {
                                JObject jsonTRPlan = new JObject();
                                jsonTRPlan.Add("company_code", modelTRPlan.company_code);
                                jsonTRPlan.Add("planleave_code", modelTRPlan.planleave_code);
                                jsonTRPlan.Add("leave_code", modelTRPlan.leave_code);

                                jsonTRPlan.Add("index", indexTRPlan);


                                indexTRPlan++;

                                arrayTRPlan.Add(jsonTRPlan);
                            }
                            json.Add("leavelists", arrayTRPlan);
                        }
                        else
                        {
                            json.Add("leavelists", arrayTRPlan);
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
        public string doManageMTPlanleave(InputMTPlanleave input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT009.2";
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
                cls_ctMTPlanleave objPlan = new cls_ctMTPlanleave();
                cls_MTPlanleave model = new cls_MTPlanleave();

                model.company_code = input.company_code;
                model.planleave_id = input.planleave_id.Equals("") ? 0 : Convert.ToInt32(input.planleave_id);
                model.planleave_code = input.planleave_code;
                model.planleave_name_th = input.planleave_name_th;
                model.planleave_name_en = input.planleave_name_en;
                model.modified_by = input.modified_by;
                model.flag = input.flag;
                string strID = objPlan.insert(model);
                if (strID.Equals("D"))
                {
                    output["success"] = false;
                    output["message"] = "Duplicate Code";

                    log.apilog_status = "500";
                    log.apilog_message = "Duplicate Code";
                    objBpcOpr.doRecordLog(log);
                    return output.ToString(Formatting.None);
                }
                if (!strID.Equals(""))
                {
                    try
                    {
                        cls_ctTRPlanleave objTRPlan = new cls_ctTRPlanleave();
                        objTRPlan.delete(input.company_code, input.planleave_code);
                        if (input.leavelists.Count > 0)
                        {
                            objTRPlan.insert(input.leavelists);
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
                    log.apilog_message = objPlan.getMessage();
                }

                objPlan.dispose();
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
        public string doDeleteMTPlanleave(InputMTPlanleave input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT009.3";
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

                cls_ctMTPlanleave controller = new cls_ctMTPlanleave();

                bool blnResult = controller.delete(input.planleave_id, input.company_code);

                if (blnResult)
                {
                    cls_ctTRPlanleave objTRPlan = new cls_ctTRPlanleave();
                    objTRPlan.delete(input.company_code, input.planleave_code);
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
        public async Task<string> doUploadMTPlanleave(string token, string by, string fileName, Stream stream,string com)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT009.4";
            log.apilog_by = by;
            log.apilog_data = "Stream : " + fileName; ;

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
                    cls_srvAttendanceImport srv_import = new cls_srvAttendanceImport();
                    string tmp = srv_import.doImportExcel("PLANLEAVE", fileName, by,com);


                    if (tmp.Equals(""))
                    {
                        output["success"] = false;
                        output["message"] = "company incorrect";
                    }
                    else
                    {
                        output["success"] = true;
                        output["message"] = tmp;
                    }
                    

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

        #region MTRateot
        public string getMTRateotList(InputMTRateot input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT004.1";
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
                cls_ctMTRateot objRate = new cls_ctMTRateot();
                List<cls_MTRateot> listLate = objRate.getDataByFillter(input.company_code, input.rateot_id, input.rateot_code);

                JArray array = new JArray();

                if (listLate.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTRateot model in listLate)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("rateot_id", model.rateot_id);
                        json.Add("rateot_code", model.rateot_code);
                        json.Add("rateot_name_th", model.rateot_name_th);
                        json.Add("rateot_name_en", model.rateot_name_en);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        cls_ctTRRateot objTRRate = new cls_ctTRRateot();
                        List<cls_TRRateot> listTRRate = objTRRate.getDataByFillter(model.company_code, model.rateot_code);
                        JArray arrayTRRate = new JArray();
                        if (listTRRate.Count > 0)
                        {
                            int indexTRRate = 1;

                            foreach (cls_TRRateot modelTRRate in listTRRate)
                            {
                                JObject jsonTRRate = new JObject();
                                jsonTRRate.Add("company_code", modelTRRate.company_code);
                                jsonTRRate.Add("rateot_daytype", modelTRRate.rateot_daytype);
                                jsonTRRate.Add("rateot_code", modelTRRate.rateot_code);
                                jsonTRRate.Add("rateot_before", modelTRRate.rateot_before);
                                jsonTRRate.Add("rateot_normal", modelTRRate.rateot_normal);
                                jsonTRRate.Add("rateot_break", modelTRRate.rateot_break);
                                jsonTRRate.Add("rateot_after", modelTRRate.rateot_after);

                                jsonTRRate.Add("index", indexTRRate);


                                indexTRRate++;

                                arrayTRRate.Add(jsonTRRate);
                            }
                            json.Add("rateot_data", arrayTRRate);
                        }
                        else
                        {
                            json.Add("rateot_data", arrayTRRate);
                        }
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
        public string doManageMTRateot(InputMTRateot input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT004.2";
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
                cls_ctMTRateot objRate = new cls_ctMTRateot();
                cls_MTRateot model = new cls_MTRateot();

                model.company_code = input.company_code;
                model.rateot_id = input.rateot_id.Equals("") ? 0 : Convert.ToInt32(input.rateot_id);
                model.rateot_code = input.rateot_code;
                model.rateot_name_th = input.rateot_name_th;
                model.rateot_name_en = input.rateot_name_en;
                model.modified_by = input.modified_by;
                model.flag = input.flag;
                string strID = objRate.insert(model);
                if (strID.Equals("D"))
                {
                    output["success"] = false;
                    output["message"] = "Duplicate Code";

                    log.apilog_status = "500";
                    log.apilog_message = "Duplicate Code";
                    objBpcOpr.doRecordLog(log);
                    return output.ToString(Formatting.None);
                }
                if (!strID.Equals(""))
                {
                    try
                    {
                        cls_ctTRRateot objTRRate = new cls_ctTRRateot();
                        objTRRate.delete(input.company_code, input.rateot_code);
                        if (input.rateot_data.Count > 0)
                        {
                            objTRRate.insert(input.rateot_data);
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
                    log.apilog_message = objRate.getMessage();
                }

                objRate.dispose();
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
        public string doDeleteMTRateot(InputMTRateot input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT004.3";
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

                cls_ctMTRateot controller = new cls_ctMTRateot();

                bool blnResult = controller.delete(input.rateot_id, input.company_code);

                if (blnResult)
                {
                    cls_ctTRRateot objTRRate = new cls_ctTRRateot();
                    objTRRate.delete(input.company_code, input.rateot_code);
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
        public async Task<string> doUploadMTRateot(string token, string by, string fileName, Stream stream,string com)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT004.4";
            log.apilog_by = by;
            log.apilog_data = "Stream : " + fileName; ;

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
                    cls_srvAttendanceImport srv_import = new cls_srvAttendanceImport();
                    string tmp = srv_import.doImportExcel("RATEOT", fileName, by,com);


                    if (tmp.Equals(""))
                    {
                        output["success"] = false;
                        output["message"] = "company incorrect";
                    }
                    else
                    {
                        output["success"] = true;
                        output["message"] = tmp;
                    }
                    

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

        #region MTDiligence
        public string getMTDiligenceList(InputMTDiligence input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT005.1";
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
                cls_ctMTDiligence objDiligence = new cls_ctMTDiligence();
                List<cls_MTDiligence> listDiligence = objDiligence.getDataByFillter(input.company_code, input.diligence_id, input.diligence_code);

                JArray array = new JArray();

                if (listDiligence.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTDiligence model in listDiligence)
                    {
                        JObject json = new JObject();

                        json.Add("diligence_id", model.diligence_id);
                        json.Add("diligence_code", model.diligence_code);
                        json.Add("diligence_name_th", model.diligence_name_th);
                        json.Add("diligence_name_en", model.diligence_name_en);

                        json.Add("diligence_punchcard", model.diligence_punchcard);
                        json.Add("diligence_punchcard_times", model.diligence_punchcard_times);
                        json.Add("diligence_punchcard_timespermonth", model.diligence_punchcard_timespermonth);

                        json.Add("diligence_late", model.diligence_late);
                        json.Add("diligence_late_times", model.diligence_late_times);
                        json.Add("diligence_late_timespermonth", model.diligence_late_timespermonth);
                        json.Add("diligence_late_acc", model.diligence_late_acc);

                        json.Add("diligence_ba", model.diligence_ba);
                        json.Add("diligence_before_min", model.diligence_before_min);
                        json.Add("diligence_after_min", model.diligence_after_min);

                        json.Add("diligence_passpro", model.diligence_passpro);
                        json.Add("diligence_wrongcondition", model.diligence_wrongcondition);
                        json.Add("diligence_someperiod", model.diligence_someperiod);
                        json.Add("diligence_someperiod_first", model.diligence_someperiod_first);

                        json.Add("company_code", model.company_code);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        cls_ctTRDiligenceSteppay objStep = new cls_ctTRDiligenceSteppay();
                        List<cls_TRDiligenceSteppay> listStep = objStep.getDataByFillter(model.company_code, model.diligence_code);
                        JArray arrayStep = new JArray();
                        if (listStep.Count > 0)
                        {
                            int indexStep = 1;

                            foreach (cls_TRDiligenceSteppay modelStep in listStep)
                            {
                                JObject jsonStep = new JObject();

                                jsonStep.Add("company_code", modelStep.company_code);
                                jsonStep.Add("diligence_code", modelStep.diligence_code);
                                jsonStep.Add("steppay_step", modelStep.steppay_step);
                                jsonStep.Add("steppay_type", modelStep.steppay_type);
                                jsonStep.Add("steppay_amount", modelStep.steppay_amount);
                                jsonStep.Add("index", indexStep);
                                indexStep++;

                                arrayStep.Add(jsonStep);
                            }
                            json.Add("steppay_data", arrayStep);
                        }
                        else
                        {
                            json.Add("steppay_data", arrayStep);
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
        public string doManageMTDiligence(InputMTDiligence input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT005.2";
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
                cls_ctMTDiligence objDiligence = new cls_ctMTDiligence();
                cls_MTDiligence model = new cls_MTDiligence();

                model.company_code = input.company_code;
                model.diligence_id = input.diligence_id.Equals("") ? 0 : Convert.ToInt32(input.diligence_id);
                model.diligence_code = input.diligence_code;
                model.diligence_name_th = input.diligence_name_th;
                model.diligence_name_en = input.diligence_name_en;

                model.diligence_punchcard = input.diligence_punchcard;
                model.diligence_punchcard_times = input.diligence_punchcard_times;
                model.diligence_punchcard_timespermonth = input.diligence_punchcard_timespermonth;

                model.diligence_late = input.diligence_late;
                model.diligence_late_times = input.diligence_late_times;
                model.diligence_late_timespermonth = input.diligence_late_timespermonth;
                model.diligence_late_acc = input.diligence_late_acc;

                model.diligence_ba = input.diligence_ba;
                model.diligence_before_min = input.diligence_before_min;
                model.diligence_after_min = input.diligence_after_min;

                model.diligence_passpro = input.diligence_passpro;
                model.diligence_wrongcondition = input.diligence_wrongcondition;
                model.diligence_someperiod = input.diligence_someperiod;
                model.diligence_someperiod_first = input.diligence_someperiod_first;

                model.company_code = input.company_code;
                model.modified_by = input.modified_by;
                model.flag = model.flag;
                string strID = objDiligence.insert(model);
                if (strID.Equals("D"))
                {
                    output["success"] = false;
                    output["message"] = "Duplicate Code";

                    log.apilog_status = "500";
                    log.apilog_message = "Duplicate Code";
                    objBpcOpr.doRecordLog(log);
                    return output.ToString(Formatting.None);
                }
                if (!strID.Equals(""))
                {
                    try
                    {
                        cls_ctTRDiligenceSteppay objTRStep = new cls_ctTRDiligenceSteppay();
                        objTRStep.delete(input.company_code, input.diligence_code);
                        if (input.steppay_data.Count > 0)
                        {
                            objTRStep.insert(input.steppay_data);
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
                    log.apilog_message = objDiligence.getMessage();
                }

                objDiligence.dispose();
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
        public string doDeleteMTDiligence(InputMTDiligence input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT005.3";
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

                cls_ctMTDiligence controller = new cls_ctMTDiligence();

                bool blnResult = controller.delete(input.diligence_id, input.company_code);

                if (blnResult)
                {
                    cls_ctTRDiligenceSteppay objTRStep = new cls_ctTRDiligenceSteppay();
                    objTRStep.delete(input.company_code, input.diligence_code);
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
        public async Task<string> doUploadMTDiligence(string token, string by, string fileName, Stream stream,string com)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT005.4";
            log.apilog_by = by;
            log.apilog_data = "Stream : " + fileName; ;

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
                    cls_srvAttendanceImport srv_import = new cls_srvAttendanceImport();
                    string tmp = srv_import.doImportExcel("DILIGENCE", fileName, by,com);


                    if (tmp.Equals(""))
                    {
                        output["success"] = false;
                        output["message"] = "company incorrect";
                    }
                    else
                    {
                        output["success"] = true;
                        output["message"] = tmp;
                    }
                    

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

        #region MTLate
        public string getMTLateList(InputMTLate input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT007.1";
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
                cls_ctMTLate objLate = new cls_ctMTLate();
                List<cls_MTLate> listLate = objLate.getDataByFillter(input.company_code, input.late_id, input.late_code);

                JArray array = new JArray();

                if (listLate.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTLate model in listLate)
                    {
                        JObject json = new JObject();

                        json.Add("late_id", model.late_id);
                        json.Add("late_code", model.late_code);
                        json.Add("late_name_th", model.late_name_th);
                        json.Add("late_name_en", model.late_name_en);
                        json.Add("company_code", model.company_code);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        cls_ctTRLate objTRLate = new cls_ctTRLate();
                        List<cls_TRLate> listTRLate = objTRLate.getDataByFillter(model.company_code, model.late_code);
                        JArray arrayTRLate = new JArray();
                        if (listTRLate.Count > 0)
                        {
                            int indexTRLate = 1;

                            foreach (cls_TRLate modelTRLate in listTRLate)
                            {
                                JObject jsonTRLate = new JObject();

                                jsonTRLate.Add("late_code", modelTRLate.late_code);
                                jsonTRLate.Add("late_from", modelTRLate.late_from);
                                jsonTRLate.Add("late_to", modelTRLate.late_to);
                                jsonTRLate.Add("late_deduct_type", modelTRLate.late_deduct_type);
                                jsonTRLate.Add("late_deduct_amount", modelTRLate.late_deduct_amount);
                                jsonTRLate.Add("index", indexTRLate);
                                indexTRLate++;

                                arrayTRLate.Add(jsonTRLate);
                            }
                            json.Add("late_data", arrayTRLate);
                        }
                        else
                        {
                            json.Add("late_data", arrayTRLate);
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
        public string doManageMTLate(InputMTLate input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT007.2";
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
                cls_ctMTLate objLate = new cls_ctMTLate();
                cls_MTLate model = new cls_MTLate();
                model.company_code = input.company_code;
                model.late_id = input.late_id.Equals("") ? 0 : Convert.ToInt32(input.late_id);
                model.late_code = input.late_code;
                model.late_name_th = input.late_name_th;
                model.late_name_en = input.late_name_en;
                model.modified_by = input.modified_by;
                model.flag = input.flag;
                string strID = objLate.insert(model);
                if (strID.Equals("D"))
                {
                    output["success"] = false;
                    output["message"] = "Duplicate Code";

                    log.apilog_status = "500";
                    log.apilog_message = "Duplicate Code";
                    objBpcOpr.doRecordLog(log);
                    return output.ToString(Formatting.None);
                }
                if (!strID.Equals(""))
                {
                    try
                    {
                        cls_ctTRLate objTRLate = new cls_ctTRLate();
                        objTRLate.delete(input.company_code, input.late_code);
                        if (input.late_data.Count > 0)
                        {
                            objTRLate.insert(input.late_data);
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
                    log.apilog_message = objLate.getMessage();
                }

                objLate.dispose();
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
        public string doDeleteMTLate(InputMTLate input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT007.3";
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

                cls_ctMTLate controller = new cls_ctMTLate();

                bool blnResult = controller.delete(input.late_id);

                if (blnResult)
                {
                    cls_ctTRLate objTRLate = new cls_ctTRLate();
                    objTRLate.delete(input.company_code, input.late_code);
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
        public async Task<string> doUploadMTLate(string token, string by, string fileName, Stream stream,string com)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT007.4";
            log.apilog_by = by;
            log.apilog_data = "Stream : " + fileName; ;

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
                    cls_srvAttendanceImport srv_import = new cls_srvAttendanceImport();
                    string tmp = srv_import.doImportExcel("LATE", fileName, by,com);


                    if (tmp.Equals(""))
                    {
                        output["success"] = false;
                        output["message"] = "company incorrect";
                    }
                    else
                    {
                        output["success"] = true;
                        output["message"] = tmp;
                    }
                    

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

        #region MTPlantimeallw
        public string getMTPlantimeallwList(InputMTPlantimeallw input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT006.1";
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
                cls_ctMTPlantimeallw objPlantimeallw = new cls_ctMTPlantimeallw();
                List<cls_MTPlantimeallw> listPlantimeallw = objPlantimeallw.getDataByFillter(input.company_code, input.plantimeallw_id, input.plantimeallw_code);

                JArray array = new JArray();

                if (listPlantimeallw.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTPlantimeallw model in listPlantimeallw)
                    {
                        JObject json = new JObject();


                        json.Add("plantimeallw_id", model.plantimeallw_id);
                        json.Add("plantimeallw_code", model.plantimeallw_code);
                        json.Add("plantimeallw_name_th", model.plantimeallw_name_th);
                        json.Add("plantimeallw_name_en", model.plantimeallw_name_en);
                        json.Add("company_code", model.company_code);
                        json.Add("plantimeallw_passpro", model.plantimeallw_passpro);
                        json.Add("plantimeallw_lastperiod", model.plantimeallw_lastperiod);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        cls_ctTRTimeallw objTimeallw = new cls_ctTRTimeallw();
                        List<cls_TRTimeallw> listTR = objTimeallw.getDataByFillter(model.company_code, model.plantimeallw_code);
                        JArray arrayTR = new JArray();
                        if (listTR.Count > 0)
                        {
                            int indexTR = 1;

                            foreach (cls_TRTimeallw modelTR in listTR)
                            {
                                JObject jsonTR = new JObject();

                                jsonTR.Add("company_code", modelTR.company_code);
                                jsonTR.Add("plantimeallw_code", modelTR.plantimeallw_code);
                                jsonTR.Add("timeallw_no", modelTR.timeallw_no);
                                jsonTR.Add("timeallw_time", modelTR.timeallw_time);
                                jsonTR.Add("timeallw_type", modelTR.timeallw_type);
                                jsonTR.Add("timeallw_method", modelTR.timeallw_method);
                                jsonTR.Add("timeallw_timein", modelTR.timeallw_timein);
                                jsonTR.Add("timeallw_timeout", modelTR.timeallw_timeout);
                                jsonTR.Add("timeallw_normalday", modelTR.timeallw_normalday);
                                jsonTR.Add("timeallw_offday", modelTR.timeallw_offday);
                                jsonTR.Add("timeallw_companyday", modelTR.timeallw_companyday);
                                jsonTR.Add("timeallw_holiday", modelTR.timeallw_holiday);
                                jsonTR.Add("timeallw_leaveday", modelTR.timeallw_leaveday);

                                jsonTR.Add("index", indexTR);
                                indexTR++;

                                arrayTR.Add(jsonTR);
                            }
                            json.Add("timeallw_data", arrayTR);
                        }
                        else
                        {
                            json.Add("timeallw_data", arrayTR);
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
        public string doManageMTPlantimeallw(InputMTPlantimeallw input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT006.2";
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
                cls_ctMTPlantimeallw objPlantimeallw = new cls_ctMTPlantimeallw();
                cls_MTPlantimeallw model = new cls_MTPlantimeallw();
                model.company_code = input.company_code;
                model.plantimeallw_id = input.plantimeallw_id.Equals("") ? 0 : Convert.ToInt32(input.plantimeallw_id);
                model.plantimeallw_code = input.plantimeallw_code;
                model.plantimeallw_name_th = input.plantimeallw_name_th;
                model.plantimeallw_name_en = input.plantimeallw_name_en;
                model.plantimeallw_passpro = input.plantimeallw_passpro;
                model.plantimeallw_lastperiod = input.plantimeallw_lastperiod;
                model.modified_by = input.modified_by;
                model.flag = input.flag;
                string strID = objPlantimeallw.insert(model);
                if (strID.Equals("D"))
                {
                    output["success"] = false;
                    output["message"] = "Duplicate Code";

                    log.apilog_status = "500";
                    log.apilog_message = "Duplicate Code";
                    objBpcOpr.doRecordLog(log);
                    return output.ToString(Formatting.None);
                }
                if (!strID.Equals(""))
                {
                    try
                    {
                        cls_ctTRTimeallw objTimeallw = new cls_ctTRTimeallw();
                        bool blnResult = objTimeallw.insert(input.company_code, input.plantimeallw_code, input.timeallw_data);

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
                    log.apilog_message = objPlantimeallw.getMessage();
                }

                objPlantimeallw.dispose();
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
        public string doDeleteMTPlantimeallw(InputMTPlantimeallw input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT006.3";
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

                cls_ctMTPlantimeallw controller = new cls_ctMTPlantimeallw();

                bool blnResult = controller.delete(input.plantimeallw_id, input.company_code);

                if (blnResult)
                {
                    cls_ctTRTimeallw objTR = new cls_ctTRTimeallw();
                    objTR.delete(input.company_code, input.plantimeallw_code);
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
        public async Task<string> doUploadMTPlantimeallw(string token, string by, string fileName, Stream stream,string com)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT006.4";
            log.apilog_by = by;
            log.apilog_data = "Stream : " + fileName; ;

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
                    cls_srvAttendanceImport srv_import = new cls_srvAttendanceImport();
                    string tmp = srv_import.doImportExcel("ALLOWANCE", fileName, by,com);


                    if (tmp.Equals(""))
                    {
                        output["success"] = false;
                        output["message"] = "company incorrect";
                    }
                    else
                    {
                        output["success"] = true;
                        output["message"] = tmp;
                    }
                    

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

        #region Batch policy
        public string getPolicyAttendance(InputSetPolicyAtt input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT011.1";
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
                string worker_code = "";
                cls_ctTREmppolatt objPol = new cls_ctTREmppolatt();
                if (input.emp_data.Count > 0)
                {
                    worker_code = input.emp_data[0].worker_code;
                }
                List<cls_TREmppolatt> listPol = objPol.getDataByFillter(input.company_code, worker_code, input.pol_type);

                JArray array = new JArray();

                if (listPol.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TREmppolatt model in listPol)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("emppolatt_policy_code", model.emppolatt_policy_code);
                        json.Add("emppolatt_policy_type", model.emppolatt_policy_type);
                        json.Add("emppolatt_policy_note", model.emppolatt_policy_note);
                        json.Add("modified_by", model.created_by);
                        json.Add("modified_date", model.created_date);
                        json.Add("flag", model.flag);
                        cls_ctMTWorker controller = new cls_ctMTWorker();
                        List<cls_MTWorker> list = controller.getDataByFillter(model.company_code, model.worker_code);
                        JArray arrayWorker = new JArray();
                        if (list.Count > 0)
                        {
                            int indexWorker = 1;

                            foreach (cls_MTWorker modelWorker in list)
                            {
                                JObject jsonWokker = new JObject();

                                jsonWokker.Add("company_code", modelWorker.company_code);
                                jsonWokker.Add("worker_id", modelWorker.worker_id);
                                jsonWokker.Add("worker_code", modelWorker.worker_code);
                                jsonWokker.Add("worker_card", modelWorker.worker_card);
                                jsonWokker.Add("worker_initial", modelWorker.worker_initial);

                                jsonWokker.Add("worker_fname_th", modelWorker.worker_fname_th);
                                jsonWokker.Add("worker_lname_th", modelWorker.worker_lname_th);
                                jsonWokker.Add("worker_fname_en", modelWorker.worker_fname_en);
                                jsonWokker.Add("worker_lname_en", modelWorker.worker_lname_en);

                                jsonWokker.Add("worker_type", modelWorker.worker_type);
                                jsonWokker.Add("worker_gender", modelWorker.worker_gender);
                                jsonWokker.Add("worker_birthdate", modelWorker.worker_birthdate);
                                jsonWokker.Add("worker_hiredate", modelWorker.worker_hiredate);
                                jsonWokker.Add("worker_status", modelWorker.worker_status);
                                jsonWokker.Add("religion_code", modelWorker.religion_code);
                                jsonWokker.Add("blood_code", modelWorker.blood_code);
                                jsonWokker.Add("worker_height", modelWorker.worker_height);
                                jsonWokker.Add("worker_weight", modelWorker.worker_weight);

                                jsonWokker.Add("worker_resigndate", modelWorker.worker_resigndate);
                                jsonWokker.Add("worker_resignstatus", modelWorker.worker_resignstatus);
                                jsonWokker.Add("worker_resignreason", modelWorker.worker_resignreason);

                                jsonWokker.Add("worker_probationdate", modelWorker.worker_probationdate);
                                jsonWokker.Add("worker_probationenddate", modelWorker.worker_probationenddate);
                                jsonWokker.Add("worker_probationday", modelWorker.worker_probationday);

                                jsonWokker.Add("worker_taxmethod", modelWorker.worker_taxmethod);

                                jsonWokker.Add("hrs_perday", modelWorker.hrs_perday);

                                jsonWokker.Add("modified_by", modelWorker.modified_by);
                                jsonWokker.Add("modified_date", modelWorker.modified_date);

                                jsonWokker.Add("self_admin", modelWorker.self_admin);

                                jsonWokker.Add("flag", modelWorker.flag);

                                jsonWokker.Add("initial_name_th", modelWorker.initial_name_th);
                                jsonWokker.Add("initial_name_en", modelWorker.initial_name_en);

                                jsonWokker.Add("position_name_th", modelWorker.position_name_th);
                                jsonWokker.Add("position_name_en", modelWorker.position_name_en);
                                indexWorker++;

                                arrayWorker.Add(jsonWokker);
                            }
                            json.Add("emp_data", arrayWorker);
                        }
                        else
                        {
                            json.Add("emp_data", arrayWorker);
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
        public string doSetPolicyAttendance(InputSetPolicyAtt input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT011.2";
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
                cls_ctTREmppolatt objPol = new cls_ctTREmppolatt();
                List<cls_TREmppolatt> listPol = new List<cls_TREmppolatt>();
                bool strID = false;
                foreach (cls_MTWorker modelWorker in input.emp_data)
                {
                    cls_TREmppolatt model = new cls_TREmppolatt();
                    model.emppolatt_policy_code = input.pol_code;
                    model.emppolatt_policy_type = input.pol_type;
                    model.emppolatt_policy_note = input.pol_note;
                    model.company_code = input.company_code;
                    model.worker_code = modelWorker.worker_code;
                    model.flag = input.flag;
                    model.created_by = input.modified_by;

                    listPol.Add(model);
                }
                if (listPol.Count > 0)
                {
                    strID = objPol.insert(listPol);
                }
                if (strID)
                {
                    try
                    {
                        if (input.pol_type.Equals("LV"))
                        {
                            string year = DateTime.Now.Year.ToString();

                            foreach (cls_TREmppolatt pol in listPol)
                            {
                                cls_srvProcessTime srvTime = new cls_srvProcessTime();
                                srvTime.doSetEmpleaveacc(input.year_code, pol.company_code, pol.worker_code, input.modified_by);
                            }

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
                    log.apilog_message = objPol.getMessage();
                }

                objPol.dispose();
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
        public string doDeletePolicyAttendance(InputSetPolicyAtt input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT011.3";
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

                cls_ctTREmppolatt controller = new cls_ctTREmppolatt();

                bool blnResult = controller.delete(input.company_code, input.emp_data[0].worker_code, input.pol_type);

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

        #region SetPlanShift
        public string doSetBatchPlanshift(InputBatchPlanshift input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT012";
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
                bool blnResult = true;
                string strMessage = "";

                try
                {
                    int intCountSuccss = 0;
                    int intCountFail = 0;
                    System.Text.StringBuilder obj_fail = new System.Text.StringBuilder();

                    //-- Step 1 Get Plan Year

                    cls_ctMTYear ctYear = new cls_ctMTYear();
                    List<cls_MTYear> listYear = ctYear.getDataByFillter(input.company_code, "LEAVE", "", input.year_code);

                    if (listYear.Count > 0)
                    {
                        DateTime dateFrom = listYear[0].year_fromdate;
                        DateTime dateTo = listYear[0].year_todate;


                        //-- Step 2 get plan schedule
                        cls_ctTRPlanschedule ctSchedule = new cls_ctTRPlanschedule();
                        List<cls_TRPlanschedule> listPlanschedule = ctSchedule.getDataByFillter(input.company_code, input.planshift_code);


                        foreach (cls_MTWorker worker in input.transaction_data)
                        {
                            //-- Step 3 get holiday
                            cls_ctTRHoliday ctTRHoliday = new cls_ctTRHoliday();
                            List<cls_TRHoliday> listHoliday = ctTRHoliday.getDataByWorker(input.company_code, worker.worker_code);

                            List<cls_TRTimecard> listTimecard = new List<cls_TRTimecard>();
                            foreach (cls_TRPlanschedule schedule in listPlanschedule)
                            {
                                dateFrom = Convert.ToDateTime(schedule.planschedule_fromdate).Date;
                                dateTo = Convert.ToDateTime(schedule.planschedule_todate).Date;

                                //-- Loop date
                                for (DateTime dateStart = dateFrom; dateStart <= dateTo; dateStart = dateStart.AddDays(1))
                                {
                                    string daytype = "N";
                                    string dateName = dateStart.ToString("ddd");

                                    //-- Check holiday
                                    bool blnHoliday = false;
                                    foreach (cls_TRHoliday holiday in listHoliday)
                                    {
                                        if (Convert.ToDateTime(holiday.holiday_date) == dateStart)
                                        {
                                            daytype = holiday.holiday_daytype;
                                            blnHoliday = true;
                                            break;
                                        }
                                    }

                                    if (!blnHoliday)
                                    {
                                        if (dateName.Equals("Sun") && schedule.planschedule_sun_off.Equals("Y"))
                                            daytype = "O";
                                        else if (dateName.Equals("Mon") && schedule.planschedule_mon_off.Equals("Y"))
                                            daytype = "O";
                                        else if (dateName.Equals("Tue") && schedule.planschedule_tue_off.Equals("Y"))
                                            daytype = "O";
                                        else if (dateName.Equals("Wed") && schedule.planschedule_wed_off.Equals("Y"))
                                            daytype = "O";
                                        else if (dateName.Equals("Thu") && schedule.planschedule_thu_off.Equals("Y"))
                                            daytype = "O";
                                        else if (dateName.Equals("Fri") && schedule.planschedule_fri_off.Equals("Y"))
                                            daytype = "O";
                                        else if (dateName.Equals("Sat") && schedule.planschedule_sat_off.Equals("Y"))
                                            daytype = "O";
                                    }

                                    cls_TRTimecard timecard = new cls_TRTimecard();
                                    timecard.company_code = input.company_code;
                                    timecard.timecard_workdate = dateStart.Date;
                                    timecard.timecard_daytype = daytype;
                                    timecard.shift_code = schedule.shift_code;

                                    timecard.timecard_color = "0";

                                    timecard.modified_by = input.username;


                                    //-- Add to timecard
                                    listTimecard.Add(timecard);
                                }
                            }


                            if (listTimecard.Count > 0)
                            {
                                cls_ctTRTimecard ctTimecard = new cls_ctTRTimecard();

                                bool blnRecord = ctTimecard.insert_plantime(input.company_code, "", worker.worker_code, listTimecard[0].timecard_workdate, listTimecard[listTimecard.Count - 1].timecard_workdate, listTimecard);

                                if (blnRecord)
                                {
                                    intCountSuccss++;
                                    blnResult = true;

                                }
                                else
                                {
                                    intCountFail++;
                                    obj_fail.Append(worker.worker_code + " --> " + ctTimecard.getMessage() + "|");
                                    blnResult = false;
                                }
                            }

                        } //-- foreach (JObject json in jsonArray.Children<JObject>())
                        strMessage = "Success: " + intCountSuccss + " | Fail: " + intCountFail.ToString();
                        output["result_fail"] = obj_fail.ToString();


                    }
                    else
                    {
                        strMessage = "Check the year policy";
                        blnResult = false;
                    }

                }
                catch
                {
                    blnResult = false;
                }

                if (blnResult)
                {
                    output["result"] = "1";
                    output["result_text"] = strMessage;
                }
                else
                {
                    output["result"] = "2";
                    output["result_text"] = strMessage;
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

        #region Timecard
        public string getTRTimecardList(FillterAttendance req)
        {

            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT901.1";
            log.apilog_by = req.username;


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

                DateTime datefrom = Convert.ToDateTime(req.fromdate);
                DateTime dateto = Convert.ToDateTime(req.todate);

                cls_ctTRTimecard objTimecard = new cls_ctTRTimecard();
                List<cls_TRTimecard> listTimecard = objTimecard.getDataByFillter(req.company, req.project_code, req.worker_code, datefrom, dateto);
                JArray array = new JArray();

                if (listTimecard.Count > 0)
                {
                    int index = 1;

                    int intRow = 1;

                    foreach (cls_TRTimecard model in listTimecard)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("project_code", model.project_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("shift_code", model.shift_code);
                        json.Add("timecard_workdate", model.timecard_workdate);
                        json.Add("timecard_daytype", model.timecard_daytype);
                        json.Add("timecard_color", model.timecard_color);
                        json.Add("timecard_lock", model.timecard_lock);

                        json.Add("timecard_ch1", model.timecard_ch1);
                        json.Add("timecard_ch2", model.timecard_ch2);
                        json.Add("timecard_ch3", model.timecard_ch3);
                        json.Add("timecard_ch4", model.timecard_ch4);
                        json.Add("timecard_ch5", model.timecard_ch5);
                        json.Add("timecard_ch6", model.timecard_ch6);
                        json.Add("timecard_ch7", model.timecard_ch7);
                        json.Add("timecard_ch8", model.timecard_ch8);
                        json.Add("timecard_ch9", model.timecard_ch9);
                        json.Add("timecard_ch10", model.timecard_ch10);

                        //-- Time in
                        if (!model.timecard_ch1.ToString("HH:mm").Equals("00:00"))
                        {
                            json.Add("timecard_in", model.timecard_ch1.ToString("dd/MM/yyyy HH:mm"));
                        }
                        else if (!model.timecard_ch3.ToString("HH:mm").Equals("00:00"))
                        {
                            json.Add("timecard_in", model.timecard_ch3.ToString("dd/MM/yyyy HH:mm"));
                        }
                        else
                        {
                            json.Add("timecard_in", "-");
                        }

                        //-- Time out
                        if (!model.timecard_ch10.ToString("HH:mm").Equals("00:00"))
                        {
                            json.Add("timecard_out", model.timecard_ch10.ToString("dd/MM/yyyy HH:mm"));
                        }
                        else if (!model.timecard_ch8.ToString("HH:mm").Equals("00:00"))
                        {
                            json.Add("timecard_out", model.timecard_ch8.ToString("dd/MM/yyyy HH:mm"));
                        }
                        else if (!model.timecard_ch4.ToString("HH:mm").Equals("00:00"))
                        {
                            json.Add("timecard_out", model.timecard_ch4.ToString("dd/MM/yyyy HH:mm"));
                        }
                        else
                        {
                            json.Add("timecard_out", "-");
                        }


                        json.Add("timecard_before_min", model.timecard_before_min);
                        json.Add("timecard_work1_min", model.timecard_work1_min);
                        json.Add("timecard_work2_min", model.timecard_work2_min);
                        json.Add("timecard_break_min", model.timecard_break_min);
                        json.Add("timecard_after_min", model.timecard_after_min);
                        json.Add("timecard_late_min", model.timecard_late_min);

                        json.Add("timecard_before_min_app", model.timecard_before_min_app);
                        json.Add("timecard_work1_min_app", model.timecard_work1_min_app);
                        json.Add("timecard_work2_min_app", model.timecard_work2_min_app);
                        json.Add("timecard_break_min_app", model.timecard_break_min_app);
                        json.Add("timecard_after_min_app", model.timecard_after_min_app);
                        json.Add("timecard_late_min_app", model.timecard_late_min_app);

                        int hrs = (model.timecard_work1_min_app + model.timecard_work2_min_app) / 60;
                        int min = (model.timecard_work1_min_app + model.timecard_work2_min_app) - (hrs * 60);
                        json.Add("work_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));

                        hrs = (model.timecard_before_min_app + model.timecard_after_min_app) / 60;
                        min = (model.timecard_before_min_app + model.timecard_after_min_app) - (hrs * 60);
                        json.Add("ot_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));

                        hrs = (model.timecard_late_min_app) / 60;
                        min = (model.timecard_late_min_app) - (hrs * 60);
                        json.Add("late_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));


                        //-- F add 06/01/2024
                        json.Add("timecard_leavepay_min", model.timecard_leavepay_min);
                        json.Add("timecard_leavededuct_min", model.timecard_leavededuct_min);
                        //--

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);

                        json.Add("worker_name_th", model.worker_name_th);
                        json.Add("worker_name_en", model.worker_name_en);
                        json.Add("projob_code", model.projob_code);
                        json.Add("projobsub_code", model.projobsub_code);

                        json.Add("change", false);

                        json.Add("index", index);

                        json.Add("row", intRow);

                        switch (model.timecard_workdate.DayOfWeek)
                        {
                            case DayOfWeek.Sunday: json.Add("col", 1); break;
                            case DayOfWeek.Monday: json.Add("col", 2); break;
                            case DayOfWeek.Tuesday: json.Add("col", 3); break;
                            case DayOfWeek.Wednesday: json.Add("col", 4); break;
                            case DayOfWeek.Thursday: json.Add("col", 5); break;
                            case DayOfWeek.Friday: json.Add("col", 6); break;
                            case DayOfWeek.Saturday:
                                json.Add("col", 7);
                                intRow++;
                                break;
                        }

                        index++;


                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }


            return output.ToString(Formatting.None);
        }

        public string doManageTRTimecard(InputTRTimecard input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT901.2";
            log.apilog_by = input.modified_by;
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

                cls_ctTRTimecard objTime = new cls_ctTRTimecard();
                cls_TRTimecard model = new cls_TRTimecard();

                model.company_code = input.company_code;
                model.project_code = input.project_code;
                model.projob_code = input.projob_code;
                model.worker_code = input.worker_code;
                model.projobsub_code = input.projobsub_code == null ? "" : input.projobsub_code;

                

                model.timecard_workdate = Convert.ToDateTime(input.timecard_workdate);
                model.timecard_daytype = input.timecard_daytype;
                model.shift_code = input.shift_code;
                model.timecard_color = input.timecard_color;

                model.timecard_lock = input.timecard_lock;

                if (input.timecard_ch1.Equals("") || input.timecard_ch2.Equals(""))
                {
                    model.before_scan = false;
                }
                else
                {
                    model.before_scan = true;
                    model.timecard_ch1 = this.doConvertDate(input.timecard_ch1);
                    model.timecard_ch2 = this.doConvertDate(input.timecard_ch2);
                }

                if (input.timecard_ch3.Equals("") || input.timecard_ch4.Equals(""))
                {
                    model.work1_scan = false;
                }
                else
                {
                    model.work1_scan = true;
                    model.timecard_ch3 = this.doConvertDate(input.timecard_ch3);
                    model.timecard_ch4 = this.doConvertDate(input.timecard_ch4);
                }

                if (input.timecard_ch7.Equals("") || input.timecard_ch8.Equals(""))
                {
                    model.work2_scan = false;
                }
                else
                {
                    model.work2_scan = true;
                    model.timecard_ch7 = this.doConvertDate(input.timecard_ch7);
                    model.timecard_ch8 = this.doConvertDate(input.timecard_ch8);
                }

                if (input.timecard_ch5.Equals("") || input.timecard_ch6.Equals(""))
                {
                    model.break_scan = false;
                }
                else
                {
                    model.break_scan = true;
                    model.timecard_ch5 = this.doConvertDate(input.timecard_ch5);
                    model.timecard_ch6 = this.doConvertDate(input.timecard_ch6);
                }

                if (input.timecard_ch9.Equals("") || input.timecard_ch10.Equals(""))
                {
                    model.after_scan = false;
                }
                else
                {
                    model.after_scan = true;
                    model.timecard_ch9 = this.doConvertDate(input.timecard_ch9);
                    model.timecard_ch10 = this.doConvertDate(input.timecard_ch10);
                }


                model.timecard_before_min = input.timecard_before_min;
                model.timecard_work1_min = input.timecard_work1_min;
                model.timecard_work2_min = input.timecard_work2_min;
                model.timecard_break_min = input.timecard_break_min;
                model.timecard_after_min = input.timecard_after_min;

                model.timecard_late_min = input.timecard_late_min;

                model.timecard_before_min_app = input.timecard_before_min_app;
                model.timecard_work1_min_app = input.timecard_work1_min_app;
                model.timecard_work2_min_app = input.timecard_work2_min_app;
                model.timecard_break_min_app = input.timecard_break_min_app;
                model.timecard_after_min_app = input.timecard_after_min_app;
                model.timecard_late_min_app = input.timecard_late_min_app;

                model.modified_by = input.modified_by;
                model.flag = model.flag;

                bool blnResult = objTime.update(model);

                if (blnResult)
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    //output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objTime.getMessage();
                }

                objTime.dispose();

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);

        }

        public string doManageTRTimesheet_BK(InputTRTimecard input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT901.9";
            log.apilog_by = input.modified_by;
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

               



                cls_ctTRTimecard objTimecard = new cls_ctTRTimecard();
                List<cls_TRTimecard> listPol = new List<cls_TRTimecard>();
                bool strID = false;
                foreach (cls_MTWorker modelWorker in input.emp_data)
                {

                    //-- Step 1 Get Emp detail
                    cls_ctMTWorker objWorker = new cls_ctMTWorker();
                    List<cls_MTWorker> listWorker = objWorker.getDataByFillter(input.company_code, modelWorker.worker_code);

                    if (listWorker.Count == 0)
                    {
                        output["success"] = false;
                        output["message"] = "Not found employee " + input.worker_code;

                        log.apilog_status = "500";
                        log.apilog_message = BpcOpr.MessageNotAuthen;
                        objBpcOpr.doRecordLog(log);

                        return output.ToString(Formatting.None);
                    }


                    cls_TRTimecard timecard = new cls_TRTimecard();

                    //cls_ctTRTimecard objTimecard = new cls_ctTRTimecard();
                    //cls_TRTimecard timecard = new cls_TRTimecard();
                    timecard.company_code = input.company_code;
                    timecard.worker_code = modelWorker.worker_code;
                    timecard.project_code = input.project_code;
                    timecard.projob_code = input.projob_code;
                    timecard.projobsub_code = input.projobsub_code; 

                    
                    timecard.timecard_workdate = Convert.ToDateTime(input.timecard_workdate);
                    timecard.timecard_daytype = input.timecard_daytype;
                    timecard.shift_code = input.shift_code;
                    timecard.timecard_color = "0";
                    timecard.modified_by = input.modified_by;
                    listPol.Add(timecard);
                    bool blnTimecard = objTimecard.insert(timecard);

                

                    if (listPol.Count > 0)
                    {
                        strID = objTimecard.insert(timecard);
                    }
                


                    //cls_ctTRTimecard objTimecard = new cls_ctTRTimecard();
                    //cls_TRTimecard timecard = new cls_TRTimecard();
                    //timecard.company_code = input.company_code;
                    //timecard.worker_code = input.worker_code;
                    //timecard.project_code = input.project_code;
                    //timecard.projob_code = input.projob_code;
                    //timecard.timecard_workdate = Convert.ToDateTime(input.timecard_workdate);
                    //timecard.timecard_daytype = input.timecard_daytype;
                    //timecard.shift_code = input.shift_code;
                    //timecard.timecard_color = "0";
                    //timecard.modified_by = input.modified_by;
                    //bool blnTimecard = objTimecard.insert(timecard);

                    cls_ctTRProjobmachine controller = new cls_ctTRProjobmachine();
                    List<cls_TRProjobmachine> list = controller.getDataByFillter(input.project_code, input.projob_code);

                    string terminal = "MANUAL";

                    //-- F edit 06/01/2024
                    terminal = input.project_code+input.projob_code;
                    //--

                    if (list.Count > 0)
                    {
                        terminal = list[0].projobmachine_ip;
                    }

                    cls_ctTRTimeinput objTime = new cls_ctTRTimeinput();
                    cls_TRTimeinput model = new cls_TRTimeinput();

                    //-- In
                    model.timeinput_card = listWorker[0].worker_card;
                    model.timeinput_date = Convert.ToDateTime(input.timecard_workdate);
                    model.timeinput_hhmm = input.timecard_in;
                    model.timeinput_terminal = terminal;
                    model.timeinput_function = "";
                    model.timeinput_compare = "N";

                    bool blnIn = objTime.insert(model);

                    model = new cls_TRTimeinput();
                    model.timeinput_card = listWorker[0].worker_card;
                    model.timeinput_date = Convert.ToDateTime(input.timecard_workdate);
                    model.timeinput_hhmm = input.timecard_out;
                    model.timeinput_terminal = terminal;
                    model.timeinput_function = "";
                    model.timeinput_compare = "N";

                    int tmp_in = Convert.ToInt32(input.timecard_in.Replace(":", ""));
                    int tmp_out = Convert.ToInt32(input.timecard_out.Replace(":", ""));

                    if (tmp_out < tmp_in)
                        model.timeinput_date = model.timeinput_date.AddDays(1);

                    bool blnOut = objTime.insert(model);

                    if (blnTimecard && blnIn && blnOut)
                    {
                        cls_ctMTTask objTask = new cls_ctMTTask();
                        cls_MTTask task = new cls_MTTask();


                        task.company_code = input.company_code;
                        task.project_code = input.project_code;

                        //int taskid = Convert.ToInt32( DateTime.Now.ToString("yyMMddHHmm"));
                        int taskid = 0;

                        task.task_id = taskid;
                        task.task_type = "SUM_TIME";
                        task.task_status = "W";
                        task.modified_by = "TIMESHEET";
                        task.flag = false;

                        cls_TRTaskdetail task_detail = new cls_TRTaskdetail();
                        task_detail.task_id = taskid;
                        task_detail.taskdetail_fromdate = Convert.ToDateTime(input.timecard_workdate);
                        task_detail.taskdetail_todate = Convert.ToDateTime(input.timecard_workdate);
                        task_detail.taskdetail_paydate = Convert.ToDateTime(input.timecard_workdate);
                        task_detail.taskdetail_process = "";

                        List<cls_TRTaskwhose> list_whose = new List<cls_TRTaskwhose>();
                        cls_TRTaskwhose task_whose = new cls_TRTaskwhose();
                        task_whose.task_id = taskid;
                        task_whose.worker_code = modelWorker.worker_code;
                        list_whose.Add(task_whose);

                        int intTaskID = objTask.insert(task, task_detail, list_whose);

                        if (intTaskID > 0)
                        {
                            output["success"] = true;
                            output["message"] = "Retrieved data successfully";
                            output["record_id"] = intTaskID;

                            log.apilog_status = "200";
                            log.apilog_message = "";

                            cls_srvProcessTime srvTime = new cls_srvProcessTime();
                            srvTime.doSummarizeTime(input.company_code, intTaskID.ToString());

                            //-- Delete task
                            objTask.delete(intTaskID.ToString());
                        }
                        else
                        {
                            output["success"] = false;
                            output["message"] = "Retrieved data not successfully";

                            log.apilog_status = "500";
                            log.apilog_message = objTask.getMessage();
                        }

                    }
                    else
                    {
                        output["success"] = false;
                        output["message"] = "Retrieved data not successfully";

                        log.apilog_status = "500";
                        log.apilog_message = "Record Time input fail";
                    }
                }
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);

        }

        public string doManageTRTimesheet(InputTRTimecard input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT901.9";
            log.apilog_by = input.modified_by;
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
                                
                cls_ctTRTimecard objTimecard = new cls_ctTRTimecard();
                              
                List<cls_TRTaskwhose> list_whose = new List<cls_TRTaskwhose>();
                
                foreach (cls_MTWorker modelWorker in input.emp_data)
                {

                    //-- Step 1 Get Emp detail
                    cls_ctMTWorker objWorker = new cls_ctMTWorker();
                    List<cls_MTWorker> listWorker = objWorker.getDataByFillter(input.company_code, modelWorker.worker_code);

                    if (listWorker.Count == 0)
                    {
                        continue;
                    }

                    cls_TRTimecard timecard = new cls_TRTimecard();              
                    timecard.company_code = input.company_code;
                    timecard.worker_code = modelWorker.worker_code;
                    timecard.project_code = input.project_code;
                    timecard.projob_code = input.projob_code;
                    timecard.projobsub_code = input.projobsub_code;
                    timecard.timecard_workdate = Convert.ToDateTime(input.timecard_workdate);
                    timecard.timecard_daytype = input.timecard_daytype;
                    timecard.shift_code = input.shift_code;
                    timecard.timecard_color = "0";
                    timecard.modified_by = input.modified_by;
                   
                    //-- New timecard
                    bool blnTimecard = objTimecard.insert(timecard);
                    
                    string terminal = input.project_code + input.projob_code;                    
                    cls_ctTRTimeinput objTime = new cls_ctTRTimeinput();
                    cls_TRTimeinput model = new cls_TRTimeinput();

                    //-- In
                    model.timeinput_card = listWorker[0].worker_card;
                    model.timeinput_date = Convert.ToDateTime(input.timecard_workdate);
                    model.timeinput_hhmm = input.timecard_in;
                    model.timeinput_terminal = terminal;
                    model.timeinput_function = "";
                    model.timeinput_compare = "N";
                    bool blnIn = objTime.insert(model);

                    model = new cls_TRTimeinput();
                    model.timeinput_card = listWorker[0].worker_card;
                    model.timeinput_date = Convert.ToDateTime(input.timecard_workdate);
                    model.timeinput_hhmm = input.timecard_out;
                    model.timeinput_terminal = terminal;
                    model.timeinput_function = "";
                    model.timeinput_compare = "N";

                    int tmp_in = Convert.ToInt32(input.timecard_in.Replace(":", ""));
                    int tmp_out = Convert.ToInt32(input.timecard_out.Replace(":", ""));

                    if (tmp_out < tmp_in)
                        model.timeinput_date = model.timeinput_date.AddDays(1);

                    bool blnOut = objTime.insert(model);

                    if (blnTimecard && blnIn && blnOut)
                    {
                        cls_TRTaskwhose task_whose = new cls_TRTaskwhose();
                        task_whose.task_id = 0;
                        task_whose.worker_code = modelWorker.worker_code;
                        list_whose.Add(task_whose);
                    }
                }

                if (list_whose.Count > 0)
                {
                    cls_ctMTTask objTask = new cls_ctMTTask();

                    //-- Calculate
                    cls_MTTask task = new cls_MTTask();
                    task.company_code = input.company_code;
                    task.project_code = input.project_code;
                    int taskid = 0;

                    task.task_id = taskid;
                    task.task_type = "SUM_TIME";
                    task.task_status = "W";
                    task.modified_by = "TIMESHEET";
                    task.flag = false;

                    cls_TRTaskdetail task_detail = new cls_TRTaskdetail();
                    task_detail.task_id = taskid;
                    task_detail.taskdetail_fromdate = Convert.ToDateTime(input.timecard_workdate).Date;
                    task_detail.taskdetail_todate = Convert.ToDateTime(input.timecard_workdate).Date;
                    task_detail.taskdetail_paydate = Convert.ToDateTime(input.timecard_workdate).Date;
                    task_detail.taskdetail_process = "";

                    int intTaskID = objTask.insert(task, task_detail, list_whose);
                    
                    if (intTaskID > 0)
                    {
                        //-- Summarize time
                        cls_srvProcessTime srvTime = new cls_srvProcessTime();
                        srvTime.doSummarizeTime(input.company_code, intTaskID.ToString());

                        //-- Delete task
                        objTask.delete(intTaskID.ToString());

                        //-- Calculate time
                        task.task_id = taskid;
                        task.task_type = "CAL_TIME";
                        task.task_status = "W";
                        task.modified_by = "TIMESHEET";
                        task.flag = false;

                        task_detail.taskdetail_process = "SA,OT,AW";

                        intTaskID = objTask.insert(task, task_detail, list_whose);

                        if (intTaskID > 0)
                        {
                            srvTime.doCalculateTime(input.company_code, intTaskID.ToString());
                            //-- Delete task
                            objTask.delete(intTaskID.ToString());
                        }
                    }

                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = 0;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = "Record Time input fail";
                }

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);

        }

        public string getDaytype()
        {

            JObject output = new JObject();

            try
            {

                JArray array = new JArray();
                JObject json = new JObject();
                json.Add("daytype_code", "N");
                json.Add("daytype_name_th", "วันทำงาน");
                json.Add("daytype_name_en", "Normal day");
                array.Add(json);
                json = new JObject();
                json.Add("daytype_code", "O");
                json.Add("daytype_name_th", "วันหยุด");
                json.Add("daytype_name_en", "Off day");
                array.Add(json);
                json = new JObject();
                json.Add("daytype_code", "H");
                json.Add("daytype_name_th", "วันหยุดประเพณี");
                json.Add("daytype_name_en", "Holiday day");
                array.Add(json);
                json = new JObject();
                json.Add("daytype_code", "C");
                json.Add("daytype_name_th", "วันหยุดบริษัท");
                json.Add("daytype_name_en", "Company day");
                array.Add(json);
                json = new JObject();
                json.Add("daytype_code", "L");
                json.Add("daytype_name_th", "วันลา");
                json.Add("daytype_name_en", "Leave day");
                array.Add(json);
                json = new JObject();
                json.Add("daytype_code", "A");
                json.Add("daytype_name_th", "ขาดงาน");
                json.Add("daytype_name_en", "Absent day");
                array.Add(json);

                output["success"] = true;
                output["message"] = "";
                output["data"] = array;


            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

            }


            return output.ToString(Formatting.None);
        }


        public string doDeleteTRTimecard(InputTRTimecard input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT901.3";
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
                DateTime datefrom = Convert.ToDateTime(input.timecard_workdate);

                cls_ctTRTimecard controller = new cls_ctTRTimecard();
                List<cls_TRTimecard> listTimecard = new List<cls_TRTimecard>();

                bool blnResult = controller.delete(input.company_code, input.project_code, input.worker_code );

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
        //{
        //    JObject output = new JObject();

        //    var json_data = new JavaScriptSerializer().Serialize(input);
        //    var tmp = JToken.Parse(json_data);

        //    cls_SYSApilog log = new cls_SYSApilog();
        //    log.apilog_code = "ATT905.3";
        //    log.apilog_by = input.modified_by;
        //    log.apilog_data = tmp.ToString();

        //    try
        //    {
        //        var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
        //        if (authHeader == null || !objBpcOpr.doVerify(authHeader))
        //        {
        //            output["success"] = false;
        //            output["message"] = BpcOpr.MessageNotAuthen;

        //            log.apilog_status = "500";
        //            log.apilog_message = BpcOpr.MessageNotAuthen;
        //            objBpcOpr.doRecordLog(log);

        //            return output.ToString(Formatting.None);
        //        }
        //        //DateTime datefrom = Convert.ToDateTime(input.timecard_workdate);

        //        cls_ctTRTimecard controller = new cls_ctTRTimecard();

        //        if (controller.checkDataOld(input.company_code, input.project_code, input.projob_code, input.worker_code, Convert.ToDateTime(input.timecard_workdate)))
        //        {
        //            bool blnResult = controller.delete(input.company_code, input.project_code, input.projob_code, input.worker_code, Convert.ToDateTime(input.timecard_workdate));

        //            if (blnResult)
        //            {
        //                output["success"] = true;
        //                output["message"] = "Remove data successfully";

        //                log.apilog_status = "200";
        //                log.apilog_message = "";
        //            }
        //            else
        //            {
        //                output["success"] = false;
        //                output["message"] = "Remove data not successfully";

        //                log.apilog_status = "500";
        //                log.apilog_message = controller.getMessage();
        //            }

        //        }
        //        else
        //        {
        //            string message = "Not Found Data : " + input.company_code;
        //            output["success"] = false;
        //            output["message"] = message;

        //            log.apilog_status = "404";
        //            log.apilog_message = message;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        output["result"] = "0";
        //        output["result_text"] = ex.ToString();

        //        log.apilog_status = "500";
        //        log.apilog_message = ex.ToString();

        //    }
        //    finally
        //    {
        //        objBpcOpr.doRecordLog(log);
        //    }

        //    return output.ToString(Formatting.None);

        //}


        // {
        //    JObject output = new JObject();

        //    var json_data = new JavaScriptSerializer().Serialize(input);
        //    var tmp = JToken.Parse(json_data);

        //    cls_SYSApilog log = new cls_SYSApilog();
        //    log.apilog_code = "ATT901.3";
        //    log.apilog_by = input.username;
        //    log.apilog_data = tmp.ToString();

        //    try
        //    {
        //        var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
        //        if (authHeader == null || !objBpcOpr.doVerify(authHeader))
        //        {
        //            output["success"] = false;
        //            output["message"] = BpcOpr.MessageNotAuthen;
        //            log.apilog_status = "500";
        //            log.apilog_message = BpcOpr.MessageNotAuthen;
        //            objBpcOpr.doRecordLog(log);

        //            return output.ToString(Formatting.None);
        //        }
        //        DateTime datefrom = Convert.ToDateTime(input.timecard_workdate);
          
        //        cls_ctTRTimecard controller = new cls_ctTRTimecard();
        //        List<cls_TRTimecard> listTimecard = new List<cls_TRTimecard>();

        //        bool blnResult = controller.delete(input.company_code, input.project_code, input.projob_code, input.worker_code);

        //        if (blnResult)
        //        {
        //            output["success"] = true;
        //            output["message"] = "Remove data successfully";

        //            log.apilog_status = "200";
        //            log.apilog_message = "";
        //        }
        //        else
        //        {
        //            output["success"] = false;
        //            output["message"] = "Remove data not successfully";

        //            log.apilog_status = "500";
        //            log.apilog_message = controller.getMessage();
        //        }
        //        controller.dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        output["success"] = false;
        //        output["message"] = "(C)Remove data not successfully";

        //        log.apilog_status = "500";
        //        log.apilog_message = ex.ToString();
        //    }
        //    finally
        //    {
        //        objBpcOpr.doRecordLog(log);
        //    }
        //    return output.ToString(Formatting.None);

        //}


        #endregion

        #region MTTimeimpformat
        public string getMTTimeimpformatList(FillterAttendance req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT902.1";
            log.apilog_by = req.username;


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

                cls_ctMTTimeimpformat objMTTimeimpformat = new cls_ctMTTimeimpformat();
                List<cls_MTTimeimpformat> listMTTimeimpformat = objMTTimeimpformat.getDataByFillter(req.company);

                JArray array = new JArray();

                if (listMTTimeimpformat.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTTimeimpformat model in listMTTimeimpformat)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);

                        json.Add("date_format", model.date_format);

                        json.Add("card_start", model.card_start);
                        json.Add("card_lenght", model.card_lenght);

                        json.Add("date_start", model.date_start);
                        json.Add("date_lenght", model.date_lenght);

                        json.Add("hours_start", model.hours_start);
                        json.Add("hours_lenght", model.hours_lenght);

                        json.Add("minute_start", model.minute_start);
                        json.Add("minute_lenght", model.minute_lenght);

                        json.Add("function_start", model.function_start);
                        json.Add("function_lenght", model.function_lenght);

                        json.Add("machine_start", model.machine_start);
                        json.Add("machine_lenght", model.machine_lenght);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);

                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }


            return output.ToString(Formatting.None);
        }
        public string doManageMTTimeimpformat(InputMTTimeimpformat input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT902.2";
            log.apilog_by = input.modified_by;
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

                cls_ctMTTimeimpformat objMTTimeimpformat = new cls_ctMTTimeimpformat();
                cls_MTTimeimpformat model = new cls_MTTimeimpformat();

                model.company_code = input.company_code;
                model.date_format = input.date_format;

                model.card_start = input.card_start;
                model.card_lenght = input.card_lenght;

                model.date_start = input.date_start;
                model.date_lenght = input.date_lenght;

                model.hours_start = input.hours_start;
                model.hours_lenght = input.hours_lenght;

                model.minute_start = input.minute_start;
                model.minute_lenght = input.minute_lenght;

                model.function_start = input.function_start;
                model.function_lenght = input.function_lenght;

                model.machine_start = input.machine_start;
                model.machine_lenght = input.machine_lenght;

                model.modified_by = input.modified_by;
                model.flag = model.flag;

                bool blnResult = objMTTimeimpformat.insert(model);

                if (blnResult)
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objMTTimeimpformat.getMessage();
                }

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteMTTimeimpformat(InputMTTimeimpformat input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT902.3";
            log.apilog_by = input.modified_by;
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

                cls_ctMTTimeimpformat controller = new cls_ctMTTimeimpformat();

                if (controller.checkDataOld(input.company_code))
                {
                    bool blnResult = controller.delete(input.company_code);

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

                }
                else
                {
                    string message = "Not Found Data : " + input.company_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        #region TimeInput
        public string doUploadTimeInput(string fileName, Stream stream)
        {
            JObject output = new JObject();

            try
            {
                Regex regex = new Regex("(^-+)|(^content-)|(^$)|(^submit)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

                string FilePath = Path.Combine
                   (ClassLibrary_BPC.Config.PathFileImport + "\\Att\\Import", fileName);

                using (FileStream writer = new FileStream(FilePath, FileMode.Create))
                {
                    TextReader textReader = new StreamReader(stream);
                    string sLine = textReader.ReadLine();

                    while (sLine != null)
                    {
                        if (!regex.Match(sLine).Success)
                        {
                            byte[] bytes = Encoding.UTF8.GetBytes(sLine);

                            writer.Write(bytes, 0, bytes.Length);

                            byte[] newline = Encoding.ASCII.GetBytes(Environment.NewLine);
                            writer.Write(newline, 0, newline.Length);
                        }
                        sLine = textReader.ReadLine();
                    }
                }

                output["success"] = true;
                output["message"] = "0";
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = ex.ToString();
            }

            return output.ToString(Formatting.None);
        }

        public string doReadSimpleTimeInput(string fileName, Stream stream)
        {
            JObject output = new JObject();

            try
            {
                Regex regex = new Regex("(^-+)|(^content-)|(^$)|(^submit)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

                string FilePath = Path.Combine
                   (ClassLibrary_BPC.Config.PathFileImport + "\\Att\\Import", fileName);

                TextReader textReader = new StreamReader(stream);
                string sLine = textReader.ReadLine();

                string firstRow = "";

                while (sLine != null)
                {
                    if (!regex.Match(sLine).Success)
                    {
                        firstRow = sLine;
                        break;
                    }
                    sLine = textReader.ReadLine();
                }

                output["success"] = true;
                output["message"] = "0";
                output["data"] = firstRow;
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = ex.ToString();
            }

            return output.ToString(Formatting.None);
        }

        public string getTRTimeinputList(FillterAttendance req)
        {

            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT909.1";
            log.apilog_by = req.username;
            
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

                DateTime datefrom = Convert.ToDateTime(req.fromdate);
                DateTime dateto = Convert.ToDateTime(req.todate);

                cls_ctTRTimeinput objTimeinput = new cls_ctTRTimeinput();
                List<cls_TRTimeinput> listTimeinput = objTimeinput.getDataByFillter(req.company, req.worker_code, datefrom, dateto, false);
                JArray array = new JArray();

                if (listTimeinput.Count > 0)
                {
                    int index = 1;
                    
                    foreach (cls_TRTimeinput model in listTimeinput)
                    {
                        JObject json = new JObject();

                        json.Add("timeinput_card", model.timeinput_card);
                        json.Add("timeinput_date", model.timeinput_date);
                        json.Add("timeinput_hhmm", model.timeinput_hhmm);
                        json.Add("timeinput_terminal", model.timeinput_terminal);
                        json.Add("timeinput_function", model.timeinput_function);
                        json.Add("index", index);
                        
                        index++;
                        
                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

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

        #region Batch policy allowance item
        public string getPolicyAttendanceItem(InputSetPolicyAttItem input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT903.1";
            log.apilog_by = input.modified_by;


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

                cls_ctTREmpattitem objPol = new cls_ctTREmpattitem();
                List<cls_TREmpattitem> listPol = objPol.getDataByFillter(input.company_code, "");

                JArray array = new JArray();

                if (listPol.Count > 0)
                {

                    int index = 1;

                    foreach (cls_TREmpattitem model in listPol)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("item_sa", model.empattitem_sa);
                        json.Add("item_ot", model.empattitem_ot);
                        json.Add("item_aw", model.empattitem_aw);
                        json.Add("item_dg", model.empattitem_dg);
                        json.Add("item_lv", model.empattitem_lv);
                        json.Add("item_ab", model.empattitem_ab);
                        json.Add("item_lt", model.empattitem_lt);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);

                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }


            return output.ToString(Formatting.None);
        }
        public string doSetPolicyAttendanceItem(InputSetPolicyAttItem input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT903.2";
            log.apilog_by = input.modified_by;
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

                string company_code = input.company_code;
                string item_sa = input.item_sa;
                string item_ot = input.item_ot;
                string item_aw = input.item_aw;
                string item_dg = input.item_dg;
                string item_lv = input.item_lv;
                string item_ab = input.item_ab;
                string item_lt = input.item_lt;


                string modified_by = input.modified_by;


                System.Text.StringBuilder obj_fail = new System.Text.StringBuilder();
                List<cls_TREmpattitem> listPol = new List<cls_TREmpattitem>();

                foreach (cls_MTWorker modelWorker in input.emp_data)
                {

                    cls_TREmpattitem model = new cls_TREmpattitem();
                    model.empattitem_sa = item_sa;
                    model.empattitem_ot = item_ot;
                    model.empattitem_aw = item_aw;
                    model.empattitem_dg = item_dg;
                    model.empattitem_lv = item_lv;
                    model.empattitem_ab = item_ab;
                    model.empattitem_lt = item_lt;

                    model.company_code = company_code;
                    model.worker_code = modelWorker.worker_code;
                    model.modified_by = modified_by;

                    listPol.Add(model);
                }

                bool blnResult = false;
                if (listPol.Count > 0)
                {
                    cls_ctTREmpattitem objPol = new cls_ctTREmpattitem();
                    //blnResult = objPol.insert(listPol);

                    foreach (cls_TREmpattitem model in listPol)
                    {
                        if (!objPol.insert(model))
                            obj_fail.Append(model.worker_code);
                    }

                    if (obj_fail.Length == 0)
                    {
                        blnResult = true;
                    }


                    if (blnResult)
                    {
                        output["success"] = true;
                        output["message"] = "";

                        log.apilog_status = "200";
                        log.apilog_message = "";
                    }
                    else
                    {
                        output["success"] = false;
                        output["message"] = "Retrieved data not successfully";

                        log.apilog_status = "500";
                        log.apilog_message = objPol.getMessage();
                    }
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                }
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteTREmppolattItem(InputSetPolicyAttItem input)
        {
            JObject output = new JObject();
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT903.3";
            log.apilog_by = input.modified_by;
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

                cls_ctTREmpattitem objPol = new cls_ctTREmpattitem();

                if (objPol.checkDataOld(input.company_code, input.worker_code))
                {
                    bool blnResult = objPol.delete(input.company_code, input.worker_code);

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
                        log.apilog_message = objPol.getMessage();
                    }
                }
                else
                {
                    string message = "Not Found : " + input.worker_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
                }

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

        #region Wageday
        public string getTRWagedayList(FillterAttendance req)
        {

            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT904.1";
            log.apilog_by = req.username;


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

                DateTime datefrom = Convert.ToDateTime(req.fromdate);
                DateTime dateto = Convert.ToDateTime(req.todate);

                cls_ctTRWageday controller = new cls_ctTRWageday();
                List<cls_TRWageday> listResult = controller.getDataByFillter(req.language, req.company, req.project_code, "", datefrom, dateto, req.worker_code);
                JArray array = new JArray();

                if (listResult.Count > 0)
                {
                    int index = 1;
                    
                    foreach (cls_TRWageday model in listResult)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);                                               
                        json.Add("worker_code", model.worker_code);
                        json.Add("project_code", model.project_code);
                        json.Add("projob_code", model.projob_code);
                        json.Add("wageday_date", model.wageday_date);
                        json.Add("wageday_wage", model.wageday_wage);

                        json.Add("wageday_before_rate", model.wageday_before_rate);
                        json.Add("wageday_normal_rate", model.wageday_normal_rate);
                        json.Add("wageday_break_rate", model.wageday_break_rate);
                        json.Add("wageday_after_rate", model.wageday_after_rate);

                        json.Add("wageday_before_min", model.wageday_before_min);
                        json.Add("wageday_normal_min", model.wageday_normal_min);
                        json.Add("wageday_break_min", model.wageday_break_min);
                        json.Add("wageday_after_min", model.wageday_after_min);

                        json.Add("wageday_before_amount", model.wageday_before_amount);
                        json.Add("wageday_normal_amount", model.wageday_normal_amount);
                        json.Add("wageday_break_amount", model.wageday_break_amount);
                        json.Add("wageday_after_amount", model.wageday_after_amount);

                        json.Add("ot1_min", model.ot1_min);
                        json.Add("ot15_min", model.ot15_min);
                        json.Add("ot2_min", model.ot2_min);
                        json.Add("ot3_min", model.ot3_min);

                        json.Add("ot1_amount", model.ot1_amount);
                        json.Add("ot15_amount", model.ot15_amount);
                        json.Add("ot2_amount", model.ot2_amount);
                        json.Add("ot3_amount", model.ot3_amount);
                        
                        json.Add("late_min", model.late_min);
                        json.Add("late_amount", model.late_amount);
                        json.Add("leave_min", model.leave_min);
                        json.Add("leave_amount", model.leave_amount);
                        json.Add("absent_min", model.absent_min);
                        json.Add("absent_amount", model.absent_amount);

                        json.Add("allowance_amount", model.allowance_amount);                      

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);

                        json.Add("worker_detail", model.worker_detail);
                      

                        json.Add("change", false);
                        json.Add("index", index);
                                              
                        index++;


                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

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
        
        //-- F add 06/01/2024
        #region Doc Approve
        public string getTRTimeleaveApproveList(InputTRTimeleave input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATTS01.1";
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

                cls_ctTRTimeleave objTRTimeleave = new cls_ctTRTimeleave();
                List<cls_TRTimeleave> listTRTimeleave = objTRTimeleave.getDocApprove(input.company_code, input.worker_code, datefrom, dateto);

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
                        json.Add("reqdoc_data", null);

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

        public string getTRTimeotApproveList(InputTRTimeot input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATTS02.1";
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
                List<cls_TRTimeot> listTRTime = objTRTime.getDocApprove(input.company_code, input.worker_code, datefrom, dateto);

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
                        json.Add("reqdoc_data", null);

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

        public string getTRTimeshiftApproveList(InputTRTimeshift input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATTS03.1";
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
                List<cls_TRTimeshift> listTRTime = objTRTime.getDocApprove(input.company_code, input.worker_code, datefrom, dateto);

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
                        json.Add("reqdoc_data", null);

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
        
        public string getTRTimeonsiteApproveList(InputTRTimeonsite input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATTS04.1";
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


                DateTime datefrom = Convert.ToDateTime(input.timeonsite_workdate);
                DateTime dateto = Convert.ToDateTime(input.timeonstie_todate);

                cls_ctTRTimeonsite objTRTimeonsite = new cls_ctTRTimeonsite();
                List<cls_TRTimeonsite> listTRTimeonsite = objTRTimeonsite.getDocApprove(input.company_code, input.worker_code, datefrom, dateto);

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
                        json.Add("location_code", model.location_code);
                        json.Add("location_name_en", model.location_name_en);
                        json.Add("location_name_th", model.location_name_th);                       
                        json.Add("worker_code", model.worker_code);
                        json.Add("worker_detail_en", model.worker_detail_en);
                        json.Add("worker_detail_th", model.worker_detail_th);
                        json.Add("status", model.status);
                        json.Add("status_job", model.status_job);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        json.Add("reqdoc_data", null);

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

        public string getTRTimedaytypeApproveList(InputTRTimedaytype input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATTS05.1";
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

                DateTime datefrom = Convert.ToDateTime(input.timedaytype_workdate);
                DateTime dateto = Convert.ToDateTime(input.timedaytype_todate);

                cls_ctTRTimedaytype objTRTimedaytype = new cls_ctTRTimedaytype();
                List<cls_TRTimedaytype> listTRTimedaytype = objTRTimedaytype.getDocApprove(input.company_code, input.worker_code, datefrom, dateto);

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
                        json.Add("reqdoc_data", null);
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
        //--

        #region Lost Wages
        public string getTRLostwagesList(FillterAttendance req)
        {

            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT906.1";
            log.apilog_by = req.username;


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

                DateTime datefrom = Convert.ToDateTime(req.fromdate);
                DateTime dateto = Convert.ToDateTime(req.todate);

                cls_ctTRLostwages objLostwages = new cls_ctTRLostwages();
                List<cls_TRLostwages> listLostwage = objLostwages.getDataByFillter(req.company, req.project_code, req.worker_code, req.lostwages_cardno, datefrom, dateto);
                JArray array = new JArray();

                if (listLostwage.Count > 0)
                {
                    int index = 1;

                    int intRow = 1;

                    foreach (cls_TRLostwages model in listLostwage)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("project_code", model.project_code);
                        //
                        json.Add("lostwages_status", model.lostwages_status);

                        json.Add("lostwages_type", model.lostwages_type);

                        json.Add("lostwages_salary", model.lostwages_salary);
                        json.Add("lostwages_diligence", model.lostwages_diligence);
                        json.Add("lostwages_travelexpenses", model.lostwages_travelexpenses);
                        json.Add("lostwages_other", model.lostwages_other);
                        //
                        json.Add("lostwages_initial", model.lostwages_initial);
                        json.Add("lostwages_cardno", model.lostwages_cardno);
                        json.Add("lostwages_gender", model.lostwages_gender);
                        json.Add("lostwages_fname_th", model.lostwages_fname_th);
                        json.Add("lostwages_laname_th", model.lostwages_laname_th);

                        //
                        //
                        json.Add("worker_code", model.worker_code);
                        json.Add("shift_code", model.shift_code);
                        json.Add("lostwages_workdate", model.lostwages_workdate);
                        json.Add("lostwages_daytype", model.lostwages_daytype);
                        json.Add("lostwages_color", model.lostwages_color);
                        json.Add("lostwages_lock", model.lostwages_lock);

                        json.Add("lostwages_ch1", model.lostwages_ch1);
                        json.Add("lostwages_ch2", model.lostwages_ch2);
                        json.Add("lostwages_ch3", model.lostwages_ch3);
                        json.Add("lostwages_ch4", model.lostwages_ch4);
                        json.Add("lostwages_ch5", model.lostwages_ch5);
                        json.Add("lostwages_ch6", model.lostwages_ch6);
                        json.Add("lostwages_ch7", model.lostwages_ch7);
                        json.Add("lostwages_ch8", model.lostwages_ch8);
                        json.Add("lostwages_ch9", model.lostwages_ch9);
                        json.Add("lostwages_ch10", model.lostwages_ch10);

                        //-- Time in
                        if (!model.lostwages_ch1.ToString("HH:mm").Equals("00:00"))
                        {
                            json.Add("lostwages_in", model.lostwages_ch1.ToString("dd/MM/yyyy HH:mm"));
                        }
                        else if (!model.lostwages_ch3.ToString("HH:mm").Equals("00:00"))
                        {
                            json.Add("lostwages_in", model.lostwages_ch3.ToString("dd/MM/yyyy HH:mm"));
                        }
                        else
                        {
                            json.Add("lostwages_in", "-");
                        }

                        //-- Time out
                        if (!model.lostwages_ch10.ToString("HH:mm").Equals("00:00"))
                        {
                            json.Add("lostwages_out", model.lostwages_ch10.ToString("dd/MM/yyyy HH:mm"));
                        }
                        else if (!model.lostwages_ch8.ToString("HH:mm").Equals("00:00"))
                        {
                            json.Add("lostwages_out", model.lostwages_ch8.ToString("dd/MM/yyyy HH:mm"));
                        }
                        else if (!model.lostwages_ch4.ToString("HH:mm").Equals("00:00"))
                        {
                            json.Add("lostwages_out", model.lostwages_ch4.ToString("dd/MM/yyyy HH:mm"));
                        }
                        else
                        {
                            json.Add("lostwages_out", "-");
                        }


                        json.Add("lostwages_before_min", model.lostwages_before_min);
                        json.Add("lostwages_work1_min", model.lostwages_work1_min);
                        json.Add("lostwages_work2_min", model.lostwages_work2_min);
                        json.Add("lostwages_break_min", model.lostwages_break_min);
                        json.Add("lostwages_after_min", model.lostwages_after_min);
                        json.Add("lostwages_late_min", model.lostwages_late_min);

                        json.Add("lostwages_before_min_app", model.lostwages_before_min_app);
                        json.Add("lostwages_work1_min_app", model.lostwages_work1_min_app);
                        json.Add("lostwages_work2_min_app", model.lostwages_work2_min_app);
                        json.Add("lostwages_break_min_app", model.lostwages_break_min_app);
                        json.Add("lostwages_after_min_app", model.lostwages_after_min_app);
                        json.Add("lostwages_late_min_app", model.lostwages_late_min_app);

                        int hrs = (model.lostwages_work1_min_app + model.lostwages_work2_min_app) / 60;
                        int min = (model.lostwages_work1_min_app + model.lostwages_work2_min_app) - (hrs * 60);
                        json.Add("work_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));

                        hrs = (model.lostwages_before_min_app + model.lostwages_after_min_app) / 60;
                        min = (model.lostwages_before_min_app + model.lostwages_after_min_app) - (hrs * 60);
                        json.Add("ot_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));

                        hrs = (model.lostwages_late_min_app) / 60;
                        min = (model.lostwages_late_min_app) - (hrs * 60);
                        json.Add("late_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));


                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);


                        json.Add("worker_name_th", model.worker_name_th);
                        json.Add("worker_name_en", model.worker_name_en);
                        json.Add("projob_code", model.projob_code);
 
                        json.Add("change", false);

                        json.Add("index", index);

                        json.Add("row", intRow);

                        switch (model.lostwages_workdate.DayOfWeek)
                        {
                            case DayOfWeek.Sunday: json.Add("col", 1); break;
                            case DayOfWeek.Monday: json.Add("col", 2); break;
                            case DayOfWeek.Tuesday: json.Add("col", 3); break;
                            case DayOfWeek.Wednesday: json.Add("col", 4); break;
                            case DayOfWeek.Thursday: json.Add("col", 5); break;
                            case DayOfWeek.Friday: json.Add("col", 6); break;
                            case DayOfWeek.Saturday:
                                json.Add("col", 7);
                                intRow++;
                                break;
                        }

                        index++;


                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }


            return output.ToString(Formatting.None);
        }

        public string doManageTRLostwages(InputTRLostwages input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT906.2";
            log.apilog_by = input.modified_by;
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

                cls_ctTRLostwages objTime = new cls_ctTRLostwages();
                cls_TRLostwages model = new cls_TRLostwages();

                model.company_code = input.company_code;
                model.project_code = input.project_code;
                //
                model.lostwages_status = input.lostwages_status;
                model.lostwages_type = input.lostwages_type;

                model.lostwages_salary = input.lostwages_salary;
                model.lostwages_diligence = input.lostwages_diligence;
                model.lostwages_travelexpenses = input.lostwages_travelexpenses;
                model.lostwages_other = input.lostwages_other;
                //
                //
                model.lostwages_initial = input.lostwages_initial;

                model.lostwages_cardno = input.lostwages_cardno;
                model.lostwages_gender = input.lostwages_gender;
                model.lostwages_fname_th = input.lostwages_fname_th;
                model.lostwages_laname_th = input.lostwages_laname_th;

               

                //
                model.projob_code = input.projob_code;
                model.worker_code = input.worker_code;
 


                model.lostwages_workdate = Convert.ToDateTime(input.lostwages_workdate);
                model.lostwages_daytype = input.lostwages_daytype;
                model.shift_code = input.shift_code;
                model.lostwages_color = input.lostwages_color;

                model.lostwages_lock = input.lostwages_lock;

                if (input.lostwages_ch1.Equals("") || input.lostwages_ch2.Equals(""))
                {
                    model.before_scan = false;
                }
                else
                {
                    model.before_scan = true;
                    model.lostwages_ch1 = this.doConvertDate(input.lostwages_ch1);
                    model.lostwages_ch2 = this.doConvertDate(input.lostwages_ch2);
                }

                if (input.lostwages_ch3.Equals("") || input.lostwages_ch4.Equals(""))
                {
                    model.work1_scan = false;
                }
                else
                {
                    model.work1_scan = true;
                    model.lostwages_ch3 = this.doConvertDate(input.lostwages_ch3);
                    model.lostwages_ch4 = this.doConvertDate(input.lostwages_ch4);
                }

                if (input.lostwages_ch7.Equals("") || input.lostwages_ch8.Equals(""))
                {
                    model.work2_scan = false;
                }
                else
                {
                    model.work2_scan = true;
                    model.lostwages_ch7 = this.doConvertDate(input.lostwages_ch7);
                    model.lostwages_ch8 = this.doConvertDate(input.lostwages_ch8);
                }

                if (input.lostwages_ch5.Equals("") || input.lostwages_ch6.Equals(""))
                {
                    model.break_scan = false;
                }
                else
                {
                    model.break_scan = true;
                    model.lostwages_ch5 = this.doConvertDate(input.lostwages_ch5);
                    model.lostwages_ch6 = this.doConvertDate(input.lostwages_ch6);
                }

                if (input.lostwages_ch9.Equals("") || input.lostwages_ch10.Equals(""))
                {
                    model.after_scan = false;
                }
                else
                {
                    model.after_scan = true;
                    model.lostwages_ch9 = this.doConvertDate(input.lostwages_ch9);
                    model.lostwages_ch10 = this.doConvertDate(input.lostwages_ch10);
                }


                model.lostwages_before_min = input.lostwages_before_min;
                model.lostwages_work1_min = input.lostwages_work1_min;
                model.lostwages_work2_min = input.lostwages_work2_min;
                model.lostwages_break_min = input.lostwages_break_min;
                model.lostwages_after_min = input.lostwages_after_min;

                model.lostwages_late_min = input.lostwages_late_min;

                model.lostwages_before_min_app = input.lostwages_before_min_app;
                model.lostwages_work1_min_app = input.lostwages_work1_min_app;
                model.lostwages_work2_min_app = input.lostwages_work2_min_app;
                model.lostwages_break_min_app = input.lostwages_break_min_app;
                model.lostwages_after_min_app = input.lostwages_after_min_app;

                model.lostwages_late_min_app = input.lostwages_late_min_app;

                model.modified_by = input.modified_by;
                model.flag = model.flag;

                bool blnResult = objTime.update(model);

                if (blnResult)
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";


                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objTime.getMessage();
                }

                objTime.dispose();

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);

        }

        public string doManageTRTimesheetLostwages(InputTRLostwages input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT906.9";
            log.apilog_by = input.modified_by;
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




                cls_ctTRLostwages objLostwages = new cls_ctTRLostwages();
                List<cls_TRLostwages> listPol = new List<cls_TRLostwages>();
                bool strID = false;

                foreach (cls_MTWorker modelWorker in input.emp_data)
                {

                    //-- Step 1 Get Emp detail
                    cls_ctMTWorker objWorker = new cls_ctMTWorker();
                    List<cls_MTWorker> listWorker = objWorker.getDataByFilltercardno(input.company_code, modelWorker.worker_code, input.lostwages_cardno);

                    if (listWorker.Count == 0)
                    {

                        if (input.lostwages_cardno != null && input.lostwages_cardno.Any())
                        {

                            DateTime datefrom = Convert.ToDateTime(input.fromdate);
                            DateTime dateto = Convert.ToDateTime(input.todate);

                            cls_ctTRLostwages objTRLostwages = new cls_ctTRLostwages();
                            List<cls_TRLostwages> listTRLostwages = objTRLostwages.getDataByFilltercardno(input.company_code,  input.lostwages_cardno);

                            //List<cls_TRLostwages> listTRLostwages = objTRLostwages.getDataByFillter(input.company_code, modelWorker.worker_code, input.lostwages_cardno,'','','');

                            ////
                            cls_TRLostwages lostwages1 = new cls_TRLostwages();

                            lostwages1.company_code = input.company_code;
                            lostwages1.worker_code = input.worker_code;
                            lostwages1.project_code = input.project_code;
                            lostwages1.projob_code = input.projob_code;
                            //
                            lostwages1.lostwages_status = input.lostwages_status;
                            lostwages1.lostwages_type = input.lostwages_type;

                            lostwages1.lostwages_salary = input.lostwages_salary;
                            lostwages1.lostwages_diligence = input.lostwages_diligence;
                            lostwages1.lostwages_travelexpenses = input.lostwages_travelexpenses;
                            lostwages1.lostwages_other = input.lostwages_other;
                            //
                            //
                            lostwages1.lostwages_initial = input.lostwages_initial;

                            lostwages1.lostwages_cardno = input.lostwages_cardno;
                            lostwages1.lostwages_gender = input.lostwages_gender;
                            lostwages1.lostwages_fname_th = input.lostwages_fname_th;
                            lostwages1.lostwages_laname_th = input.lostwages_laname_th;



                            //

                            lostwages1.lostwages_workdate = Convert.ToDateTime(input.lostwages_workdate);
                            lostwages1.lostwages_daytype = input.lostwages_daytype;
                            lostwages1.shift_code = input.shift_code;
                            lostwages1.lostwages_color = "0";
                            lostwages1.modified_by = input.modified_by;
                            listPol.Add(lostwages1);
                            bool blnLostwages= objLostwages.insert(lostwages1);



                            if (listPol.Count > 0)
                            {
                                strID = objLostwages.insert(lostwages1);
                            }


                            cls_ctTRProjobmachine controller = new cls_ctTRProjobmachine();
                            List<cls_TRProjobmachine> list = controller.getDataByFillter(input.project_code, input.projob_code);

                            string terminal = "MANUAL";

                            if (list.Count > 0)
                            {
                                terminal = list[0].projobmachine_ip;
                            }

                            cls_ctTRTimeinput objTime1 = new cls_ctTRTimeinput();
                            cls_TRTimeinput model = new cls_TRTimeinput();

                            //-- In
                            model.timeinput_card = input.lostwages_cardno;
                            model.timeinput_date = Convert.ToDateTime(input.lostwages_workdate);
                            model.timeinput_hhmm = input.lostwages_in;
                            model.timeinput_terminal = terminal;
                            model.timeinput_function = "";
                            model.timeinput_compare = "N";

                            bool blnIn = objTime1.insert(model);

                            model = new cls_TRTimeinput();
                            model.timeinput_card = input.lostwages_cardno;
                            model.timeinput_date = Convert.ToDateTime(input.lostwages_workdate);
                            model.timeinput_hhmm = input.lostwages_out;
                            model.timeinput_terminal = terminal;
                            model.timeinput_function = "";
                            model.timeinput_compare = "N";

                            int tmp_in = Convert.ToInt32(input.lostwages_in.Replace(":", ""));
                            int tmp_out = Convert.ToInt32(input.lostwages_out.Replace(":", ""));

                            if (tmp_out < tmp_in)
                                model.timeinput_date = model.timeinput_date.AddDays(1);

                            bool blnOut = objTime1.insert(model);

                            if (blnLostwages && blnIn && blnOut)
                            {
                                cls_ctMTTask objTask = new cls_ctMTTask();
                                cls_MTTask task = new cls_MTTask();


                                task.company_code = input.company_code;
                                task.project_code = input.project_code;

                                //int taskid = Convert.ToInt32( DateTime.Now.ToString("yyMMddHHmm"));
                                int taskid = 0;

                                task.task_id = taskid;
                                task.task_type = "SUM_TIME";
                                task.task_status = "W";
                                task.modified_by = "TIMESHEET";
                                task.flag = false;

                                cls_TRTaskdetail task_detail = new cls_TRTaskdetail();
                                task_detail.task_id = taskid;
                                task_detail.taskdetail_fromdate = Convert.ToDateTime(input.lostwages_workdate);
                                task_detail.taskdetail_todate = Convert.ToDateTime(input.lostwages_workdate);
                                task_detail.taskdetail_paydate = Convert.ToDateTime(input.lostwages_workdate);
                                task_detail.taskdetail_process = "";

                                List<cls_TRTaskwhose> list_whose = new List<cls_TRTaskwhose>();
                                cls_TRTaskwhose task_whose = new cls_TRTaskwhose();
                                task_whose.task_id = taskid;
                                task_whose.lostwages_cardno = input.lostwages_cardno;

                                task_whose.worker_code = modelWorker.worker_code;
                                list_whose.Add(task_whose);

                                int intTaskID = objTask.insert(task, task_detail, list_whose);

                                if (intTaskID > 0)
                                {
                                    output["success"] = true;
                                    output["message"] = "Retrieved data successfully";
                                    output["record_id"] = intTaskID;

                                    log.apilog_status = "200";
                                    log.apilog_message = "";

                                    cls_srvProcessTime srvTime = new cls_srvProcessTime();
                                    srvTime.doSummarizeTimelostwagesno(input.company_code, intTaskID.ToString());

                                    //-- Delete task
                                    objTask.delete(intTaskID.ToString());
                                }
                                else
                                {
                                    output["success"] = false;
                                    output["message"] = "Retrieved data not successfully";

                                    log.apilog_status = "500";
                                    log.apilog_message = objTask.getMessage();
                                }

                            }
                            //
                        }
                    }
                        else
                        {



                            cls_TRLostwages lostwages = new cls_TRLostwages();

                            lostwages.company_code = input.company_code;
                            lostwages.worker_code = modelWorker.worker_code;
                            lostwages.project_code = input.project_code;
                            lostwages.projob_code = input.projob_code;
                            //
                            lostwages.lostwages_status = input.lostwages_status;
                            lostwages.lostwages_type = input.lostwages_type;

                            lostwages.lostwages_salary = input.lostwages_salary;
                            lostwages.lostwages_diligence = input.lostwages_diligence;
                            lostwages.lostwages_travelexpenses = input.lostwages_travelexpenses;
                            lostwages.lostwages_other = input.lostwages_other;
                            //
                            //
                            lostwages.lostwages_initial = input.lostwages_initial;

                            lostwages.lostwages_cardno = input.lostwages_cardno;
                            lostwages.lostwages_gender = input.lostwages_gender;
                            lostwages.lostwages_fname_th = input.lostwages_fname_th;
                            lostwages.lostwages_laname_th = input.lostwages_laname_th;
                            lostwages.worker_cardno = input.worker_cardno;

                        


                            //

                            lostwages.lostwages_workdate = Convert.ToDateTime(input.lostwages_workdate);
                            lostwages.lostwages_daytype = input.lostwages_daytype;
                            lostwages.shift_code = input.shift_code;
                            lostwages.lostwages_color = "0";
                            lostwages.modified_by = input.modified_by;
                            listPol.Add(lostwages);
                            bool blnLostwages = objLostwages.insert(lostwages);



                            if (listPol.Count > 0)
                            {
                                strID = objLostwages.insert(lostwages);
                            }


                            cls_ctTRProjobmachine controller = new cls_ctTRProjobmachine();
                            List<cls_TRProjobmachine> list = controller.getDataByFillter(input.project_code, input.projob_code);

                            string terminal = "MANUAL";

                            if (list.Count > 0)
                            {
                                terminal = list[0].projobmachine_ip;
                            }

                            cls_ctTRTimeinput objTime = new cls_ctTRTimeinput();
                            cls_TRTimeinput model = new cls_TRTimeinput();

                            //-- In
                            model.timeinput_card = listWorker[0].worker_card;
                            model.timeinput_date = Convert.ToDateTime(input.lostwages_workdate);
                            model.timeinput_hhmm = input.lostwages_in;
                            model.timeinput_terminal = terminal;
                            model.timeinput_function = "";
                            model.timeinput_compare = "N";

                            bool blnIn = objTime.insert(model);

                            model = new cls_TRTimeinput();
                            model.timeinput_card = listWorker[0].worker_card;
                            model.timeinput_date = Convert.ToDateTime(input.lostwages_workdate);
                            model.timeinput_hhmm = input.lostwages_out;
                            model.timeinput_terminal = terminal;
                            model.timeinput_function = "";
                            model.timeinput_compare = "N";

                            int tmp_in = Convert.ToInt32(input.lostwages_in.Replace(":", ""));
                            int tmp_out = Convert.ToInt32(input.lostwages_out.Replace(":", ""));

                            if (tmp_out < tmp_in)
                                model.timeinput_date = model.timeinput_date.AddDays(1);

                            bool blnOut = objTime.insert(model);

                            if (blnLostwages && blnIn && blnOut)
                            {
                                cls_ctMTTask objTask = new cls_ctMTTask();
                                cls_MTTask task = new cls_MTTask();


                                task.company_code = input.company_code;
                                task.project_code = input.project_code;

                                //int taskid = Convert.ToInt32( DateTime.Now.ToString("yyMMddHHmm"));
                                int taskid = 0;

                                task.task_id = taskid;
                                task.task_type = "SUM_TIME";
                                task.task_status = "W";
                                task.modified_by =  "";
                                task.flag = false;

                                cls_TRTaskdetail task_detail = new cls_TRTaskdetail();
                                task_detail.task_id = taskid;
                                task_detail.taskdetail_fromdate = Convert.ToDateTime(input.lostwages_workdate);
                                task_detail.taskdetail_todate = Convert.ToDateTime(input.lostwages_workdate);
                                task_detail.taskdetail_paydate = Convert.ToDateTime(input.lostwages_workdate);
                                task_detail.taskdetail_process = "";

                                List<cls_TRTaskwhose> list_whose = new List<cls_TRTaskwhose>();
                                cls_TRTaskwhose task_whose = new cls_TRTaskwhose();
                                task_whose.task_id = taskid;
                                task_whose.worker_code = modelWorker.worker_code;
                                list_whose.Add(task_whose);

                                int intTaskID = objTask.insert(task, task_detail, list_whose);

                                if (intTaskID > 0)
                                {
                                    output["success"] = true;
                                    output["message"] = "Retrieved data successfully";
                                    output["record_id"] = intTaskID;

                                    log.apilog_status = "200";
                                    log.apilog_message = "";

                                    cls_srvProcessTime srvTime = new cls_srvProcessTime();
                                    srvTime.doSummarizeTimelostwages(input.company_code, intTaskID.ToString());

                                    //-- Delete task
                                    objTask.delete(intTaskID.ToString());
                                }
                                else
                                {
                                    output["success"] = false;
                                    output["message"] = "Retrieved data not successfully";

                                    log.apilog_status = "500";
                                    log.apilog_message = objTask.getMessage();
                                }

                            }
                            else
                            {
                      

                                output["success"] = false;
                                output["message"] = "Retrieved data not successfully";

                                log.apilog_status = "500";
                                log.apilog_message = "Record Time input fail";
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);

        }

        public string getDaytype1()
        {

            JObject output = new JObject();

            try
            {

                JArray array = new JArray();
                JObject json = new JObject();
                json.Add("daytype_code", "N");
                json.Add("daytype_name_th", "วันทำงาน");
                json.Add("daytype_name_en", "Normal day");
                array.Add(json);
                json = new JObject();
                json.Add("daytype_code", "O");
                json.Add("daytype_name_th", "วันหยุด");
                json.Add("daytype_name_en", "Off day");
                array.Add(json);
                json = new JObject();
                json.Add("daytype_code", "H");
                json.Add("daytype_name_th", "วันหยุดประเพณี");
                json.Add("daytype_name_en", "Holiday day");
                array.Add(json);
                json = new JObject();
                json.Add("daytype_code", "C");
                json.Add("daytype_name_th", "วันหยุดบริษัท");
                json.Add("daytype_name_en", "Company day");
                array.Add(json);
                json = new JObject();
                json.Add("daytype_code", "L");
                json.Add("daytype_name_th", "วันลา");
                json.Add("daytype_name_en", "Leave day");
                array.Add(json);
                json = new JObject();
                json.Add("daytype_code", "A");
                json.Add("daytype_name_th", "ขาดงาน");
                json.Add("daytype_name_en", "Absent day");
                array.Add(json);

                output["success"] = true;
                output["message"] = "";
                output["data"] = array;


            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

            }


            return output.ToString(Formatting.None);
        }


        public string doDeleteTRLostwages(InputTRLostwages input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT906.3";
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
                DateTime datefrom = Convert.ToDateTime(input.lostwages_workdate);

                cls_ctTRLostwages controller = new cls_ctTRLostwages();
                List<cls_TRLostwages> listLostwage = new List<cls_TRLostwages>();

                bool blnResult = controller.delete(input.company_code, input.project_code, input.worker_code, input.lostwages_cardno);

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




        ///อนุมัติ
        public string getTRLostwagesApprove(FillterLostwagesApprove req)
        {

            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT906.1";
            log.apilog_by = req.username;


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

                DateTime datefrom = Convert.ToDateTime(req.fromdate);
                DateTime dateto = Convert.ToDateTime(req.todate);

                cls_ctTRLostwages objLostwages = new cls_ctTRLostwages();
                List<cls_TRLostwages> listLostwage = objLostwages.getDataByFillterAll(req.company, req.project_code, req.worker_code, req.lostwages_cardno, req.lostwages_status);

                //List<cls_TRLostwages> listLostwage = objLostwages.getDataByFillterAll(req.project_code, "", "", req.status);
                JArray array = new JArray();

                if (listLostwage.Count > 0)
                {
                    int index = 1;

                    int intRow = 1;

                    foreach (cls_TRLostwages model in listLostwage)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("project_code", model.project_code);
                        //
                        json.Add("lostwages_status", model.lostwages_status);

                        json.Add("lostwages_type", model.lostwages_type);

                        json.Add("lostwages_salary", model.lostwages_salary);
                        json.Add("lostwages_diligence", model.lostwages_diligence);
                        json.Add("lostwages_travelexpenses", model.lostwages_travelexpenses);
                        json.Add("lostwages_other", model.lostwages_other);
                        //
                        json.Add("lostwages_initial", model.lostwages_initial);
                        json.Add("lostwages_cardno", model.lostwages_cardno);
                        json.Add("lostwages_gender", model.lostwages_gender);
                        json.Add("lostwages_fname_th", model.lostwages_fname_th);
                        json.Add("lostwages_laname_th", model.lostwages_laname_th);

                        //
                        //
                        json.Add("worker_code", model.worker_code);
                        json.Add("shift_code", model.shift_code);
                        json.Add("lostwages_workdate", model.lostwages_workdate);
                        json.Add("lostwages_daytype", model.lostwages_daytype);
                        json.Add("lostwages_color", model.lostwages_color);
                        json.Add("lostwages_lock", model.lostwages_lock);

                        json.Add("lostwages_ch1", model.lostwages_ch1);
                        json.Add("lostwages_ch2", model.lostwages_ch2);
                        json.Add("lostwages_ch3", model.lostwages_ch3);
                        json.Add("lostwages_ch4", model.lostwages_ch4);
                        json.Add("lostwages_ch5", model.lostwages_ch5);
                        json.Add("lostwages_ch6", model.lostwages_ch6);
                        json.Add("lostwages_ch7", model.lostwages_ch7);
                        json.Add("lostwages_ch8", model.lostwages_ch8);
                        json.Add("lostwages_ch9", model.lostwages_ch9);
                        json.Add("lostwages_ch10", model.lostwages_ch10);

                        //-- Time in
                        if (!model.lostwages_ch1.ToString("HH:mm").Equals("00:00"))
                        {
                            json.Add("lostwages_in", model.lostwages_ch1.ToString("dd/MM/yyyy HH:mm"));
                        }
                        else if (!model.lostwages_ch3.ToString("HH:mm").Equals("00:00"))
                        {
                            json.Add("lostwages_in", model.lostwages_ch3.ToString("dd/MM/yyyy HH:mm"));
                        }
                        else
                        {
                            json.Add("lostwages_in", "-");
                        }

                        //-- Time out
                        if (!model.lostwages_ch10.ToString("HH:mm").Equals("00:00"))
                        {
                            json.Add("lostwages_out", model.lostwages_ch10.ToString("dd/MM/yyyy HH:mm"));
                        }
                        else if (!model.lostwages_ch8.ToString("HH:mm").Equals("00:00"))
                        {
                            json.Add("lostwages_out", model.lostwages_ch8.ToString("dd/MM/yyyy HH:mm"));
                        }
                        else if (!model.lostwages_ch4.ToString("HH:mm").Equals("00:00"))
                        {
                            json.Add("lostwages_out", model.lostwages_ch4.ToString("dd/MM/yyyy HH:mm"));
                        }
                        else
                        {
                            json.Add("lostwages_out", "-");
                        }


                        json.Add("lostwages_before_min", model.lostwages_before_min);
                        json.Add("lostwages_work1_min", model.lostwages_work1_min);
                        json.Add("lostwages_work2_min", model.lostwages_work2_min);
                        json.Add("lostwages_break_min", model.lostwages_break_min);
                        json.Add("lostwages_after_min", model.lostwages_after_min);
                        json.Add("lostwages_late_min", model.lostwages_late_min);

                        json.Add("lostwages_before_min_app", model.lostwages_before_min_app);
                        json.Add("lostwages_work1_min_app", model.lostwages_work1_min_app);
                        json.Add("lostwages_work2_min_app", model.lostwages_work2_min_app);
                        json.Add("lostwages_break_min_app", model.lostwages_break_min_app);
                        json.Add("lostwages_after_min_app", model.lostwages_after_min_app);
                        json.Add("lostwages_late_min_app", model.lostwages_late_min_app);

                        int hrs = (model.lostwages_work1_min_app + model.lostwages_work2_min_app) / 60;
                        int min = (model.lostwages_work1_min_app + model.lostwages_work2_min_app) - (hrs * 60);
                        json.Add("work_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));

                        hrs = (model.lostwages_before_min_app + model.lostwages_after_min_app) / 60;
                        min = (model.lostwages_before_min_app + model.lostwages_after_min_app) - (hrs * 60);
                        json.Add("ot_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));

                        hrs = (model.lostwages_late_min_app) / 60;
                        min = (model.lostwages_late_min_app) - (hrs * 60);
                        json.Add("late_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));


                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);


                        json.Add("worker_name_th", model.worker_name_th);
                        json.Add("worker_name_en", model.worker_name_en);
                        json.Add("projob_code", model.projob_code);

                        json.Add("change", false);

                        json.Add("index", index);

                        json.Add("row", intRow);

                        switch (model.lostwages_workdate.DayOfWeek)
                        {
                            case DayOfWeek.Sunday: json.Add("col", 1); break;
                            case DayOfWeek.Monday: json.Add("col", 2); break;
                            case DayOfWeek.Tuesday: json.Add("col", 3); break;
                            case DayOfWeek.Wednesday: json.Add("col", 4); break;
                            case DayOfWeek.Thursday: json.Add("col", 5); break;
                            case DayOfWeek.Friday: json.Add("col", 6); break;
                            case DayOfWeek.Saturday:
                                json.Add("col", 7);
                                intRow++;
                                break;
                        }

                        index++;


                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

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

//G 29/02/2024
        #region TRTimeot
        public string getTRATTTimeotList(InputTRATTTimeot input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT88.1";
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
                DateTime dateto = Convert.ToDateTime(input.timeot_worktodate);

                cls_ctTRATTTimeot objTRTime = new cls_ctTRATTTimeot();
                List<cls_TRATTTimeot> listTRTime = objTRTime.getDataByFillter(input.language,input.company_code, input.worker_code, datefrom, dateto);

                JArray array = new JArray();

                if (listTRTime.Count > 0)
                {
                    int index = 1;
                    foreach (cls_TRATTTimeot model in listTRTime)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);

                        json.Add("worker_detail", model.worker_detail);

                        json.Add("timeot_id", model.timeot_id);
                        json.Add("timeot_doc", model.timeot_doc);

                        json.Add("timeot_workdate", model.timeot_workdate);
                        json.Add("timeot_worktodate", model.timeot_worktodate);

                        json.Add("timeot_beforemin", model.timeot_beforemin);
                        json.Add("timeot_normalmin", model.timeot_normalmin);
                        json.Add("timeot_aftermin", model.timeot_aftermin);
                        json.Add("timeot_breakmin", model.timeot_break);

                        int hrs = (model.timeot_beforemin) / 60;
                        int min = (model.timeot_beforemin) - (hrs * 60);
                        json.Add("timeot_beforemin_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));

                        hrs = (model.timeot_normalmin) / 60;
                        min = (model.timeot_normalmin) - (hrs * 60);
                        json.Add("timeot_normalmin_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));

                        hrs = (model.timeot_aftermin) / 60;
                        min = (model.timeot_aftermin) - (hrs * 60);
                        json.Add("timeot_aftermin_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));

                        hrs = (model.timeot_break) / 60;
                        min = (model.timeot_break) - (hrs * 60);
                        json.Add("timeot_breakmin_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));


                        json.Add("timeot_note", model.timeot_note);
                        json.Add("location_code", model.location_code);
                        json.Add("reason_code", model.reason_code);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        cls_ctMTATTReqdocument objMTReqdoc = new cls_ctMTATTReqdocument();
                        List<cls_MTATTReqdocument> listTRReqdoc = objMTReqdoc.getDataByFillter(model.company_code, 0, model.timeot_id.ToString(), "OT");
                        JArray arrayTRReqdoc = new JArray();
                        if (listTRReqdoc.Count > 0)
                        {
                            int indexTRReqdoc = 1;
                            foreach (cls_MTATTReqdocument modelTRReqdoc in listTRReqdoc)
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

                //
        public string doManageTRATTTimeot(InputTRATTTimeot input)

    {
            JObject output = new JObject();
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT911.2";
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
                cls_ctTRATTTimeot objTRTime = new cls_ctTRATTTimeot();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRATTTimeot>>(input.ot_data);
                foreach (cls_TRATTTimeot data in jsonArray)
                {
                    cls_TRATTTimeot model = new cls_TRATTTimeot();
                    model.company_code = data.company_code;
                    model.worker_code = data.worker_code;
                    model.timeot_id = data.timeot_id.Equals("") ? 0 : Convert.ToInt32(data.timeot_id);
                    model.timeot_doc = data.timeot_doc;

                    model.timeot_workdate = Convert.ToDateTime(data.timeot_workdate);
                    model.timeot_worktodate = Convert.ToDateTime(data.timeot_worktodate);

                    model.timeot_beforemin = data.timeot_beforemin;
                    model.timeot_normalmin = data.timeot_normalmin;
                    model.timeot_aftermin = data.timeot_aftermin;
                    model.timeot_break = data.timeot_break;

                    model.timeot_note = data.timeot_note;
                    model.location_code = data.location_code;
                    model.reason_code = data.reason_code;

                
                    model.modified_by = data.modified_by;
                    model.flag = data.flag;

                    strID = objTRTime.insert(model);
                    if (!strID.Equals(""))
                    {
                    if (data.reqdoc_data.Count > 0)
                    {
                        foreach (cls_MTATTReqdocument reqdoc in data.reqdoc_data)
                            {
                                cls_ctMTATTReqdocument objMTReqdocu = new cls_ctMTATTReqdocument();
                                cls_MTATTReqdocument modelreqdoc = new cls_MTATTReqdocument();


                                modelreqdoc.company_code = reqdoc.company_code;
                                modelreqdoc.document_id = reqdoc.document_id;
                                modelreqdoc.job_id = strID;
                                modelreqdoc.job_type = reqdoc.job_type;
                                modelreqdoc.document_name = reqdoc.document_name;
                                modelreqdoc.document_type = reqdoc.document_type;
                                modelreqdoc.document_path = reqdoc.document_path;

                                modelreqdoc.created_by = data.modified_by;
                                string strIDs = objMTReqdocu.insert(modelreqdoc);
                            }
                        }
                        else { }
                    }
                if (!strID.Equals(""))
                    {
                        output["success"] = true;
                        output["message"] = "Retrieved data successfully";

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
              
             }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }
 
            return output.ToString(Formatting.None);

        }

        //
        //public string doManageTRATTTimeot(InputTRATTTimeot input)
        //{
        //    JObject output = new JObject();
        //    var json_data = new JavaScriptSerializer().Serialize(input);
        //    var tmp = JToken.Parse(json_data);
        //    cls_SYSApilog log = new cls_SYSApilog();
        //    log.apilog_code = "ATT907.2";
        //    log.apilog_by = input.modified_by;
        //    log.apilog_data = tmp.ToString();



        //    try
        //    {

        //        var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
        //        if (authHeader == null || !objBpcOpr.doVerify(authHeader))
        //        {
        //            output["success"] = false;
        //            output["message"] = BpcOpr.MessageNotAuthen;

        //            log.apilog_status = "500";
        //            log.apilog_message = BpcOpr.MessageNotAuthen;
        //            objBpcOpr.doRecordLog(log);

        //            return output.ToString(Formatting.None);
        //        }
        //        cls_ctTRATTTimeot objTRTime = new cls_ctTRATTTimeot();
        //        var jsonArray = JsonConvert.DeserializeObject<List<cls_TRATTTimeot>>(input.ot_data);
        //        foreach (cls_TRATTTimeot otdata in jsonArray)
        //        {
        //            cls_TRATTTimeot model = new cls_TRATTTimeot();

        //            model.company_code = input.company_code;
        //            model.worker_code = input.worker_code;
        //            model.timeot_id = otdata.timeot_id.Equals("") ? 0 : Convert.ToInt32(otdata.timeot_id);
        //            model.timeot_doc = input.timeot_doc;

        //            model.timeot_workdate = Convert.ToDateTime(input.timeot_workdate);
        //            model.timeot_worktodate = Convert.ToDateTime(input.timeot_worktodate);

        //            model.timeot_beforemin = input.timeot_beforemin;
        //            model.timeot_normalmin = input.timeot_normalmin;
        //            model.timeot_aftermin = input.timeot_aftermin;
        //            model.timeot_break = input.timeot_break;

        //            model.timeot_note = input.timeot_note;
        //            model.location_code = input.location_code;
        //            model.reason_code = input.reason_code;

        //            model.modified_by = input.modified_by;
        //            model.flag = model.flag;
        //            bool blnResult = objTRTime.insert(model);
        //            if (blnResult)
        //            {
        //                if (otdata.reqdoc_data.Count > 0)
        //                {
        //                    foreach (cls_MTATTReqdocument reqdoc in otdata.reqdoc_data)
        //                    {
        //                        cls_ctMTATTReqdocument objMTReqdocu = new cls_ctMTATTReqdocument();
        //                        cls_MTATTReqdocument modelreqdoc = new cls_MTATTReqdocument();

                                 
        //                        modelreqdoc.company_code = reqdoc.company_code;
        //                        modelreqdoc.document_id = reqdoc.document_id;
        //                        modelreqdoc.job_id = reqdoc.job_id;
        //                        modelreqdoc.job_type = reqdoc.job_type;
        //                        modelreqdoc.document_name = reqdoc.document_name;
        //                        modelreqdoc.document_type = reqdoc.document_type;
        //                        modelreqdoc.document_path = reqdoc.document_path;

        //                        modelreqdoc.created_by = input.username;
        //                        string strIDs = objMTReqdocu.insert(modelreqdoc);
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                break;
        //            }
        //            if (blnResult)
        //            {
        //                output["success"] = true;
        //                output["message"] = "Retrieved data successfully";

        //                log.apilog_status = "200";
        //                log.apilog_message = "";
        //            }
        //            else
        //            {
        //                output["success"] = false;
        //                output["message"] = "Retrieved data not successfully";

        //                log.apilog_status = "500";
        //                log.apilog_message = objTRTime.getMessage();
        //            }

        //            objTRTime.dispose();

        //        }
        //        }
        //    catch (Exception ex)
        //    {
        //        output["success"] = false;
        //        output["message"] = "(C)Retrieved data not successfully";

        //        log.apilog_status = "500";
        //        log.apilog_message = ex.ToString();
        //    }
        //    finally
        //    {
        //        objBpcOpr.doRecordLog(log);
        //    }

        //    return output.ToString(Formatting.None);

        //}



        public string doDeleteTRATTTimeot(InputTRATTTimeot input)
         {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT907.3";
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

                cls_ctTRATTTimeot controller = new cls_ctTRATTTimeot();
                bool blnResult = controller.delete(input.timeot_id);

                if (blnResult)
                {
                    cls_ctMTJobtable MTJob = new cls_ctMTJobtable();
                    MTJob.delete(input.company_code, 0, input.timeot_id.ToString(), "OT");
                    cls_ctMTATTReqdocument MTReqdoc = new cls_ctMTATTReqdocument();
                    List<cls_MTATTReqdocument> filelist = MTReqdoc.getDataByFillter(input.company_code, 0, input.timeot_id.ToString(), "OT");
                    if (filelist.Count > 0)
                    {
                        foreach (cls_MTATTReqdocument filedata in filelist)
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


        #region TRTimeleave
        public string getTRATTTimeleaveList(InputTRATTTimeleave input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT908.1";
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
                //DateTime datefrom = Convert.ToDateTime(input.timeleave_fromdate);
                //DateTime dateto = Convert.ToDateTime(input.timeleave_todate);

                cls_ctTRATTTimeleave objTRTimeleave = new cls_ctTRATTTimeleave();
                List<cls_TRATTTimeleave> listTRTimeleave = objTRTimeleave.getDataByFillter(input.language,  input.company_code, input.worker_code,  input.timeleave_fromdate, input.timeleave_todate);

                JArray array = new JArray();

                if (listTRTimeleave.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRATTTimeleave model in listTRTimeleave)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);

                        json.Add("worker_detail", model.worker_detail);
                        json.Add("leave_detail", model.leave_detail);

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
                        json.Add("leave_code", model.leave_code);
                        json.Add("reason_code", model.reason_code);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        cls_ctMTATTReqdocument objMTReqdoc = new cls_ctMTATTReqdocument();
                        List<cls_MTATTReqdocument> listTRReqdoc = objMTReqdoc.getDataByFillter(model.company_code, 0, model.timeleave_id.ToString(), "LEA");
                        JArray arrayTRReqdoc = new JArray();
                        if (listTRReqdoc.Count > 0)
                        {
                            int indexTRReqdoc = 1;

                            foreach (cls_MTATTReqdocument modelTRReqdoc in listTRReqdoc)
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


        //{
        //    var json_data = new JavaScriptSerializer().Serialize(input);
        //    var tmp = JToken.Parse(json_data);
        //    JObject output = new JObject();
        //    cls_SYSApilog log = new cls_SYSApilog();
        //    log.apilog_code = "ATT908.1";
        //    log.apilog_by = input.username;
        //    log.apilog_data = tmp.ToString();
        //    try
        //    {

        //        var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
        //        if (authHeader == null || !objBpcOpr.doVerify(authHeader))
        //        {
        //            output["success"] = false;
        //            output["message"] = BpcOpr.MessageNotAuthen;

        //            log.apilog_status = "500";
        //            log.apilog_message = BpcOpr.MessageNotAuthen;
        //            objBpcOpr.doRecordLog(log);

        //            return output.ToString(Formatting.None);
        //        }
        //        DateTime datefrom = Convert.ToDateTime(input.timeleave_fromdate);
        //        DateTime dateto = Convert.ToDateTime(input.timeleave_todate);

        //        cls_ctTRATTTimeleave objTRTimeleave = new cls_ctTRATTTimeleave();
        //        List<cls_TRATTTimeleave> listTRTimeleave = objTRTimeleave.getDataByFillter(input.language, input.company_code, input.worker_code, datefrom, dateto);
                 
        //        JArray array = new JArray();

        //        if (listTRTimeleave.Count > 0)
        //        {
        //            int index = 1;

        //            foreach (cls_TRATTTimeleave model in listTRTimeleave)
        //            {
        //                JObject json = new JObject();

        //                json.Add("company_code", model.company_code);
        //                json.Add("worker_code", model.worker_code);

        //                json.Add("worker_detail", model.worker_detail);
        //                json.Add("leave_detail", model.leave_detail);

        //                json.Add("timeleave_id", model.timeleave_id);
        //                json.Add("timeleave_doc", model.timeleave_doc);

        //                json.Add("timeleave_fromdate", model.timeleave_fromdate);
        //                json.Add("timeleave_todate", model.timeleave_todate);

        //                json.Add("timeleave_type", model.timeleave_type);
        //                json.Add("timeleave_min", model.timeleave_min);

        //                json.Add("timeleave_actualday", model.timeleave_actualday);
        //                json.Add("timeleave_incholiday", model.timeleave_incholiday);
        //                json.Add("timeleave_deduct", model.timeleave_deduct);

        //                json.Add("timeleave_note", model.timeleave_note);
        //                json.Add("leave_code", model.leave_code);
        //                json.Add("reason_code", model.reason_code);

        //                json.Add("modified_by", model.modified_by);
        //                json.Add("modified_date", model.modified_date);
        //                json.Add("flag", model.flag);

        //                json.Add("index", index);

        //                index++;

        //                array.Add(json);
        //            }

        //            output["result"] = "1";
        //            output["result_text"] = "1";
        //            output["data"] = array;

        //            log.apilog_status = "200";
        //            log.apilog_message = "";
        //        }
        //        else
        //        {
        //            output["result"] = "0";
        //            output["result_text"] = "Data not Found";
        //            output["data"] = array;

        //            log.apilog_status = "404";
        //            log.apilog_message = "Data not Found";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        output["result"] = "0";
        //        output["result_text"] = ex.ToString();

        //        log.apilog_status = "500";
        //        log.apilog_message = ex.ToString();

        //    }
        //    finally
        //    {
        //        objBpcOpr.doRecordLog(log);
        //    }

        //    return output.ToString(Formatting.None);
        //}
        public string doManageTRATTTimeleave(InputTRATTTimeleave input)
         {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT908.2";
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
                cls_ctTRATTTimeleave objTRTimeleave = new cls_ctTRATTTimeleave();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRATTTimeleave>>(input.leave_data);
                foreach (cls_TRATTTimeleave leavedata in jsonArray)
                {
                    cls_TRATTTimeleave model = new cls_TRATTTimeleave();

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
 
                    model.modified_by = input.username;
                    model.flag = leavedata.flag;

                    strID = objTRTimeleave.insert(model);
                    if (!strID.Equals(""))
                     {
                        if (leavedata.reqdoc_data.Count > 0)
                        {
                            foreach (cls_MTATTReqdocument reqdoc in leavedata.reqdoc_data)
                            {
                                cls_ctMTATTReqdocument objMTReqdocu = new cls_ctMTATTReqdocument();
                                cls_MTATTReqdocument modelreqdoc = new cls_MTATTReqdocument();
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




        //{
        //    JObject output = new JObject();
        //    var json_data = new JavaScriptSerializer().Serialize(input);
        //    var tmp = JToken.Parse(json_data);
        //    cls_SYSApilog log = new cls_SYSApilog();
        //    log.apilog_code = "ATT908.2";
        //    log.apilog_by = input.modified_by;
        //    log.apilog_data = tmp.ToString();
        //    string message = "Retrieved data not successfully";

        //    try
        //    {
        //        cls_ctTRATTTimeleave objTRTimeleave = new cls_ctTRATTTimeleave();
        //        cls_TRATTTimeleave model = new cls_TRATTTimeleave();

        //        model.company_code = input.company_code;
        //        model.worker_code = input.worker_code;
        //        model.timeleave_id = input.timeleave_id;
        //        model.timeleave_doc = input.timeleave_doc;

        //        model.timeleave_fromdate = Convert.ToDateTime(input.timeleave_fromdate);
        //        model.timeleave_todate = Convert.ToDateTime(input.timeleave_todate);

        //        model.timeleave_type = input.timeleave_type;
        //        model.timeleave_min = input.timeleave_min;

        //        model.timeleave_actualday = input.timeleave_actualday;
        //        model.timeleave_incholiday = input.timeleave_incholiday;
        //        model.timeleave_deduct = input.timeleave_deduct;

        //        model.timeleave_note = input.timeleave_note;
        //        model.leave_code = input.leave_code;
        //        model.reason_code = input.reason_code;

        //        model.modified_by = input.modified_by;
        //        model.flag = model.flag;

        //        bool blnResult = objTRTimeleave.insert(model);

        //        if (blnResult)
        //        {
        //            cls_srvProcessTime srv_time = new cls_srvProcessTime();
        //            srv_time.doCalleaveacc(model.timeleave_fromdate.Year.ToString(), model.company_code, model.worker_code, model.modified_by);

        //            output["success"] = true;
        //            output["message"] = "Retrieved data successfully";
        //            output["record_id"] = blnResult;

        //            log.apilog_status = "200";
        //            log.apilog_message = "";
        //        }
        //        else
        //        {
        //            output["success"] = false;
        //            output["message"] = message;

        //            log.apilog_status = "500";
        //            log.apilog_message = objTRTimeleave.getMessage();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        output["result"] = "0";
        //        output["result_text"] = ex.ToString();

        //        log.apilog_status = "500";
        //        log.apilog_message = ex.ToString();

        //    }
        //    finally
        //    {
        //        objBpcOpr.doRecordLog(log);
        //    }

        //    return output.ToString(Formatting.None);

        //}
        public string doDeleteTRATTTimeleave(InputTRATTTimeleave input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT908.3";
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

                cls_ctTRATTTimeleave controller = new cls_ctTRATTTimeleave();
                cls_TRATTTimeleave model = controller.getDataByID(input.timeleave_id);
                bool blnResult = controller.delete(input.timeleave_id);

                if (blnResult)
                {
                    cls_srvProcessTime srv_time = new cls_srvProcessTime();
                    srv_time.doCalleaveacc(model.timeleave_fromdate.Year.ToString(), model.company_code, model.worker_code, model.modified_by);
                    cls_ctMTJobtable MTJob = new cls_ctMTJobtable();
                    MTJob.delete(model.company_code, 0, model.timeleave_id.ToString(), "LEA");
                    cls_ctMTATTReqdocument MTReqdoc = new cls_ctMTATTReqdocument();
                    List<cls_MTATTReqdocument> filelist = MTReqdoc.getDataByFillter(model.company_code, 0, model.timeleave_id.ToString(), "LEA");
                    if (filelist.Count > 0)
                    {
                        foreach (cls_MTATTReqdocument filedata in filelist)
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
            log.apilog_code = "ATT908.5";
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


        #region TRTimeonsite
        public string getTRATTTimeonsiteList(InputTRATTTimeonsite input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT908.1";
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
                DateTime datefrom = Convert.ToDateTime(input.timeonsite_workdate);
                 //DateTime dateto = Convert.ToDateTime(input.todate);

                cls_ctTRATTTimeonsite objTRTime = new cls_ctTRATTTimeonsite();
                List<cls_TRATTTimeonsite> listTRTime = objTRTime.getDataByFillter(input.language, input.company_code, input.worker_code, datefrom, datefrom);


                JArray array = new JArray();

                if (listTRTime.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRATTTimeonsite model in listTRTime)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);

                        json.Add("worker_detail", model.worker_detail);
                        json.Add("reason_detail", model.reason_detail);
                        json.Add("location_detail", model.location_detail);

                        json.Add("timeonsite_id", model.timeonsite_id);
                        json.Add("timeonsite_doc", model.timeonsite_doc);

                        json.Add("timeonsite_workdate", model.timeonsite_workdate);
                        json.Add("timeonsite_in", model.timeonsite_in);

                        json.Add("timeonsite_out", model.timeonsite_out);
                        json.Add("timeonsite_note", model.timeonsite_note);
 
                        json.Add("location_code", model.location_code);
                        json.Add("reason_code", model.reason_code);
          
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);





                        cls_ctMTATTReqdocument objMTReqdoc = new cls_ctMTATTReqdocument();
                        List<cls_MTATTReqdocument> listTRReqdoc = objMTReqdoc.getDataByFillter(model.company_code, 0, model.timeonsite_id.ToString(), "ONS");
                        JArray arrayTRReqdoc = new JArray();
                        if (listTRReqdoc.Count > 0)
                        {
                            int indexTRReqdoc = 1;

                            foreach (cls_MTATTReqdocument modelTRReqdoc in listTRReqdoc)
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
        //

        //
        public string doManageTRATTTimeonsite(InputTRATTTimeonsite input)
        {
            JObject output = new JObject();
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT910.2";
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
                cls_ctTRATTTimeonsite objTRTime = new cls_ctTRATTTimeonsite();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRATTTimeonsite>>(input.timeonsite_data);
                foreach (cls_TRATTTimeonsite data in jsonArray)
                {
                    cls_TRATTTimeonsite model = new cls_TRATTTimeonsite();

                    



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
                    model.modified_by = data.modified_by;
                    model.flag = data.flag;

                    strID = objTRTime.insert(model);
                    if (!strID.Equals(""))
                    {
                        if (data.reqdoc_data.Count > 0)
                        {
                            foreach (cls_MTATTReqdocument reqdoc in data.reqdoc_data)
                            {
                                cls_ctMTATTReqdocument objMTReqdocu = new cls_ctMTATTReqdocument();
                                cls_MTATTReqdocument modelreqdoc = new cls_MTATTReqdocument();
                                modelreqdoc.company_code = reqdoc.company_code;
                                modelreqdoc.document_id = reqdoc.document_id;
                                modelreqdoc.job_id = strID;
                                modelreqdoc.job_type = reqdoc.job_type;
                                modelreqdoc.document_name = reqdoc.document_name;
                                modelreqdoc.document_type = reqdoc.document_type;
                                modelreqdoc.document_path = reqdoc.document_path;

                                modelreqdoc.created_by = data.modified_by;
                                string strIDs = objMTReqdocu.insert(modelreqdoc);
                            }
                        }
                        else {
                            break;
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

        public string doDeleteTRATTTimeonsite(InputTRATTTimeonsite input)
         {
            JObject output = new JObject();
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT910.3";
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
                cls_ctTRATTTimeonsite controller = new cls_ctTRATTTimeonsite();
                bool blnResult = controller.delete(input.company_code, input.timeonsite_id, input.worker_code);

                if (blnResult)
                {
                    cls_ctMTJobtable MTJob = new cls_ctMTJobtable();
                    MTJob.delete(input.company_code, 0, input.timeonsite_id.ToString(), "ONS");
                    cls_ctMTATTReqdocument MTReqdoc = new cls_ctMTATTReqdocument();
                    List<cls_MTATTReqdocument> filelist = MTReqdoc.getDataByFillter(input.company_code, 0, input.timeonsite_id.ToString(), "ONS");
                    if (filelist.Count > 0)
                    {
                        foreach (cls_MTATTReqdocument filedata in filelist)
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


        #region MTATTReqdoc
        public string getMTATTReqdocList(InputMTATTReqdoc input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT912.1";
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
                cls_ctMTATTReqdoc objMTReqdoc = new cls_ctMTATTReqdoc();
                List<cls_MTATTReqdoc> list = objMTReqdoc.getDataByFillter(input.company_code, input.reqdoc_id, input.worker_code, input.reqdoc_date, input.reqdoc_date_to );

                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTATTReqdoc model in list)
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
        public string doManageMTATTReqdoc(InputMTATTReqdoc input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT912.2";
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
                cls_ctMTATTReqdoc objMTReqdoc = new cls_ctMTATTReqdoc();
                cls_MTATTReqdoc model = new cls_MTATTReqdoc();
                model.company_code = input.company_code;
                model.reqdoc_id = input.reqdoc_id;
                model.worker_code = input.worker_code;
                model.reqdoc_doc = input.reqdoc_doc;
                model.reqdoc_date = Convert.ToDateTime(input.reqdoc_date);
                model.reqdoc_note = input.reqdoc_note;
 
                model.modified_by = input.username;
                model.flag = input.flag;
                string strID = objMTReqdoc.insert(model);
                if (!strID.Equals(""))
                {
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
        public string doDeleteeMTATTReqdoc(InputMTATTReqdoc input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT912.3";
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
        public async Task<string> doUploadMTATTReqdoc(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT912.4";
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
        //

        #region MTATTReqdocument
        public string getMTATTReqdocumentList(InputMTATTReqdocument input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT913.1";
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
                cls_ctMTATTReqdocument objMTReqdocument = new cls_ctMTATTReqdocument();
                List<cls_MTATTReqdocument> list = objMTReqdocument.getDataByFillter(input.company_code, input.document_id, input.job_id, input.job_type);

                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTATTReqdocument model in list)
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
        public string doManageMTATTReqdocument(InputMTATTReqdocument input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT913.2";
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
                cls_ctMTATTReqdocument objMTReqdocu = new cls_ctMTATTReqdocument();
                cls_MTATTReqdocument model = new cls_MTATTReqdocument();
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
        public string doDeleteeMTATTReqdocument(InputMTATTReqdocument input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT913.3";
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

                cls_ctMTATTReqdocument controller = new cls_ctMTATTReqdocument();
                bool blnResult = controller.delete(input.company_code, input.document_id, input.job_id, input.job_type);
                if (blnResult)
                {
                    cls_ctMTATTReqdocument MTReqdoc = new cls_ctMTATTReqdocument();
                    List<cls_MTATTReqdocument> filelist = MTReqdoc.getDataByFillter(input.company_code, input.document_id, input.job_id, input.job_type);
                    if (filelist.Count > 0)
                    {
                        foreach (cls_MTATTReqdocument filedata in filelist)
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


        #region TRATTTimecheckin
        public string getTRATTTimecheckinList(InputTRATTTimecheckin input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT914.1";
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
               

                cls_ctTRATTTimecheckin objTRTimecheckin = new cls_ctTRATTTimecheckin();
                List<cls_TRATTTimecheckin> listTRTimecheckin = objTRTimecheckin.getDataByFillter(input.company_code, input.worker_code, input.timecheckin_workdate, input.timecheckin_todate);

                JArray array = new JArray();

                if (listTRTimecheckin.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRATTTimecheckin model in listTRTimecheckin)
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
                         
                         json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        cls_ctMTATTReqdocument objMTReqdoc = new cls_ctMTATTReqdocument();
                        List<cls_MTATTReqdocument> listTRReqdoc = objMTReqdoc.getDataByFillter(model.company_code, 0, model.timecheckin_id.ToString(), "CI");
                        JArray arrayTRReqdoc = new JArray();
                        if (listTRReqdoc.Count > 0)
                        {
                            int indexTRReqdoc = 1;
                            foreach (cls_MTATTReqdocument modelTRReqdoc in listTRReqdoc)
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
        public string doManageTRATTTimecheckin(InputTRATTTimecheckin input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT914.2";
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
                cls_ctTRATTTimecheckin objTRTimecheckin = new cls_ctTRATTTimecheckin();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRATTTimecheckin>>(input.timecheckin_data);
                foreach (cls_TRATTTimecheckin cidata in jsonArray)
                {
                    cls_TRATTTimecheckin model = new cls_TRATTTimecheckin();

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
                     model.modified_by = input.username;
                    model.flag = cidata.flag;

                    strID = objTRTimecheckin.insert(model);
                    if (!strID.Equals(""))
                    {
                       
                        if (cidata.reqdoc_data.Count > 0)
                        {
                            foreach (cls_MTATTReqdocument reqdoc in cidata.reqdoc_data)
                            {
                                cls_ctMTATTReqdocument objMTReqdocu = new cls_ctMTATTReqdocument();
                                cls_MTATTReqdocument modelreqdoc = new cls_MTATTReqdocument();
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
        public string doDeleteTRATTTimecheckin(InputTRATTTimecheckin input)
        {
            JObject output = new JObject();
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT914.3";
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
                cls_ctTRATTTimecheckin controller = new cls_ctTRATTTimecheckin();
                bool blnResult = controller.delete(input.company_code, input.timecheckin_id.ToString(), input.timecheckin_time, input.timecheckin_type, input.timecheckin_workdate, input.worker_code);

                if (blnResult)
                {
                    cls_ctMTJobtable MTJob = new cls_ctMTJobtable();
                    MTJob.delete(input.company_code, 0, input.timecheckin_id.ToString(), "CI");
                    cls_ctMTATTReqdocument MTReqdoc = new cls_ctMTATTReqdocument();
                    List<cls_MTATTReqdocument> filelist = MTReqdoc.getDataByFillter(input.company_code, 0, input.timecheckin_id.ToString(), "CI");
                    if (filelist.Count > 0)
                    {
                        foreach (cls_MTATTReqdocument filedata in filelist)
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

        #region TRAttTimeshift
        public string getTRAttTimeshiftList(InputTRAttTimeshift input)
      
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT911.1";
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

                cls_ctTRAttTimeshift objTRTime = new cls_ctTRAttTimeshift();
                List<cls_TRTimeshift> listTRTime = objTRTime.getDataByFillter(input.timeshift_id, input.company_code, input.worker_code, datefrom, dateto);

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
        public string doManageTRAttTimeshift(InputTRAttTimeshift input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT88.2";
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
                cls_ctTRAttTimeshift objTRTime = new cls_ctTRAttTimeshift();
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
        public string doDeleteTRAttTimeshift(InputTRAttTimeshift input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT88.3";
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

                cls_ctTRAttTimeshift controller = new cls_ctTRAttTimeshift();

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

        #region TRAttTimedaytype
        public string getTRAttTimedaytypeList(InputTRAttTimedaytype input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT89.1";
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
                cls_ctTRAttTimeDaytype objTRTimedaytype = new cls_ctTRAttTimeDaytype();
                List<cls_TRTimedaytype> listTRTimedaytype = objTRTimedaytype.getDataByFillter(input.company_code, input.timedaytype_id, input.worker_code, input.timedaytype_workdate, input.timedaytype_todate);

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
        public string doManageTRAttTimedaytype(InputTRAttTimedaytype input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT89.2";
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
                cls_ctTRAttTimeDaytype objTRTimedaytype = new cls_ctTRAttTimeDaytype();
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
        public string doDeleteTRAttTimedaytype(InputTRAttTimedaytype input)
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
                cls_ctTRAttTimeDaytype controller = new cls_ctTRAttTimeDaytype();
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
    }
}
