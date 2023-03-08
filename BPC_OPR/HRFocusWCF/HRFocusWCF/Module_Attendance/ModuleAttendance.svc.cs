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
using ClassLibrary_BPC.hrfocus.service;
using System.Runtime.Serialization.Json;

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

        #region MTYear
        public string getMTYearList(InputMTYear input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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
                cls_ctMTYear objYear = new cls_ctMTYear();
                List<cls_MTYear> listYear = objYear.getDataByFillter(input.company_code, input.year_group, input.year_id, input.year_code);

                JArray array = new JArray();

                if (listYear.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTYear model in listYear)
                    {
                        JObject json = new JObject();

                        json.Add("year_id", model.year_id);
                        json.Add("year_code", model.year_code);
                        json.Add("year_name_th", model.year_name_th);
                        json.Add("year_name_en", model.year_name_en);

                        json.Add("year_fromdate", model.year_fromdate);
                        json.Add("year_todate", model.year_todate);
                        json.Add("year_group", model.year_group);

                        json.Add("company_code", model.company_code);

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
            catch(Exception e) {
                return e.ToString();
            }
            return output.ToString(Formatting.None);
        }
        public string doManageMTYear(InputMTYear input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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
                cls_ctMTYear objYear = new cls_ctMTYear();
                cls_MTYear model = new cls_MTYear();

                model.company_code = input.company_code;

                model.year_id = input.year_id;
                model.year_code = input.year_code;
                model.year_name_th = input.year_name_th;
                model.year_name_en = input.year_name_en;
                model.year_fromdate = Convert.ToDateTime(input.year_fromdate);
                model.year_todate = Convert.ToDateTime(input.year_todate);
                model.year_group = input.year_group;
                model.company_code = input.company_code;

                model.modified_by = input.modified_by;
                model.flag = input.flag;
                string strID = objYear.insert(model);
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
                    log.apilog_message = objYear.getMessage();
                }

                objYear.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteMTYear(InputMTYear input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

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

                cls_ctMTYear controller = new cls_ctMTYear();

                    bool blnResult = controller.delete(input.year_id);

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
        public async Task<string> doUploadYear(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.4";
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
                    cls_srvAttendanceImport srv_import = new cls_srvAttendanceImport();
                    string tmp = srv_import.doImportExcel("YEAR", fileName, by);


                    output["success"] = true;
                    output["message"] = tmp;

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

        #region MTPeriod
        public string getMTPeriodList(InputMTPeriod input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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
                cls_ctMTPeriod controler = new cls_ctMTPeriod();
                List<cls_MTPeriod> listPeriod = controler.getDataByFillter(input.period_id,input.company_code,input.period_type,input.year_code,input.emptype_code);

                JArray array = new JArray();

                if (listPeriod.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTPeriod model in listPeriod)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("period_id", model.period_id);
                        json.Add("period_type", model.period_type);
                        json.Add("emptype_code", model.emptype_code);
                        json.Add("year_code", model.year_code);
                        json.Add("period_no", model.period_no);

                        json.Add("period_name_th", model.period_name_th);
                        json.Add("period_name_en", model.period_name_en);

                        json.Add("period_from", model.period_from);
                        json.Add("period_to", model.period_to);
                        json.Add("period_payment", model.period_payment);
                        json.Add("period_dayonperiod", model.period_dayonperiod);

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
        public string doManageMTPeriod(InputMTPeriod input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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
                cls_ctMTPeriod controler = new cls_ctMTPeriod();
                cls_MTPeriod model = new cls_MTPeriod();

                model.company_code = input.company_code;


                model.period_id = input.period_id.Equals("") ? 0 : Convert.ToInt32(input.period_id);
                model.period_type = input.period_type;
                model.emptype_code = input.emptype_code;
                model.year_code = input.year_code;
                model.period_no = input.period_no;

                model.period_name_th = input.period_name_th;
                model.period_name_en = input.period_name_en;

                model.period_from = Convert.ToDateTime(input.period_from);
                model.period_to = Convert.ToDateTime(input.period_to);
                model.period_payment = Convert.ToDateTime(input.period_payment);

                model.period_dayonperiod = input.period_dayonperiod;

                model.modified_by = input.modified_by;
                model.flag = input.flag;
                string strID = controler.insert(model);
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
                    log.apilog_message = controler.getMessage();
                }

                controler.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteMTPeriod(InputMTPeriod input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

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

                cls_ctMTPeriod controller = new cls_ctMTPeriod();

                bool blnResult = controller.delete(input.company_code,input.period_id);

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
        public async Task<string> doUploadMTPeriod(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.4";
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
                    cls_srvAttendanceImport srv_import = new cls_srvAttendanceImport();
                    string tmp = srv_import.doImportExcel("PERIOD", fileName, by);


                    output["success"] = true;
                    output["message"] = tmp;

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

        #region MTReason
        public string getMTReasonList(InputMTReason input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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

            cls_ctMTReason objReason = new cls_ctMTReason();
            List<cls_MTReason> listReason = objReason.getDataByFillter(input.reason_group, input.reason_id, input.reason_code,input.company_code);
            JArray array = new JArray();

            if (listReason.Count > 0)
            {

                int index = 1;

                foreach (cls_MTReason model in listReason)
                {
                    JObject json = new JObject();
                    json.Add("company_code", model.company_code);
                    json.Add("reason_id", model.reason_id);
                    json.Add("reason_code", model.reason_code);
                    json.Add("reason_name_th", model.reason_name_th);
                    json.Add("reason_name_en", model.reason_name_en);
                    json.Add("reason_group", model.reason_group);
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
        public string doManageMTReason(InputMTReason input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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
                cls_ctMTReason objReason = new cls_ctMTReason();
                cls_MTReason model = new cls_MTReason();

                model.reason_id = input.reason_id.Equals("") ? 0 : Convert.ToInt32(input.reason_id);
                model.company_code = input.company_code;
                model.reason_code = input.reason_code;
                model.reason_name_th = input.reason_name_th;
                model.reason_name_en = input.reason_name_en;
                model.reason_group = input.reason_group;
                model.modified_by = input.modified_by;
                model.flag = input.flag;
                string strID = objReason.insert(model);
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
                    log.apilog_message = objReason.getMessage();
                }

                objReason.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

            }


            return output.ToString(Formatting.None);

        }
        public string doDeleteMTReason(InputMTReason input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

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

                cls_ctMTReason objReason = new cls_ctMTReason();

                bool blnResult = objReason.delete(input.reason_id,input.company_code);

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
                    log.apilog_message = objReason.getMessage();
                }
                objReason.dispose();
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
        public async Task<string> doUploadMTReason(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.4";
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
                    cls_srvAttendanceImport srv_import = new cls_srvAttendanceImport();
                    string tmp = srv_import.doImportExcel("REASON", fileName, by);


                    output["success"] = true;
                    output["message"] = tmp;

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

        #region MTLocation
        public string getMTLocationList(InputMTLocation input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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
                cls_ctMTLocation objLocation = new cls_ctMTLocation();
                List<cls_MTLocation> listLocation = objLocation.getDataByFillter(input.location_id,input.location_code,input.company_code);

                JArray array = new JArray();

                if (listLocation.Count > 0)
                {

                    int index = 1;

                    foreach (cls_MTLocation model in listLocation)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("location_id", model.location_id);
                        json.Add("location_code", model.location_code);
                        json.Add("location_name_th", model.location_name_th);
                        json.Add("location_name_en", model.location_name_en);
                        json.Add("location_detail", model.location_detail);
                        json.Add("location_lat", model.location_lat);
                        json.Add("location_long", model.location_long);
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
        public string doManageMTLocation(InputMTLocation input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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
                cls_ctMTLocation objLocation = new cls_ctMTLocation();
                cls_MTLocation model = new cls_MTLocation();
                model.company_code = input.company_code;
                model.location_id = input.location_id.Equals("") ? 0 : Convert.ToInt32(input.location_id);
                model.location_code = input.location_code;
                model.location_name_th = input.location_name_th;
                model.location_name_en = input.location_name_en;
                model.location_detail = input.location_detail;
                model.location_lat = input.location_lat;
                model.location_long = input.location_long;
                model.modified_by = input.modified_by;
                model.flag = input.flag;
                string strID = objLocation.insert(model);
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
                    log.apilog_message = objLocation.getMessage();
                }

                objLocation.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteMTLocation(InputMTLocation input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

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

                cls_ctMTLocation controller = new cls_ctMTLocation();

                bool blnResult = controller.delete(input.location_id,input.company_code);

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
        public async Task<string> doUploadMTLocation(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.4";
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
                    cls_srvAttendanceImport srv_import = new cls_srvAttendanceImport();
                    string tmp = srv_import.doImportExcel("LOCATION", fileName, by);


                    output["success"] = true;
                    output["message"] = tmp;

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

        #region MTPlanholiday
        public string getMTPlanholidayList(InputMTPlanholiday input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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
                cls_ctMTPlanholiday objPlanholiday = new cls_ctMTPlanholiday();
                List<cls_MTPlanholiday> listPlanholiday = objPlanholiday.getDataByFillter(input.company_code,input.planholiday_id,input.planholiday_code,input.year_code);

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
        public string doManageMTPlanholiday(InputMTPlanholiday input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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
                if (!strID.Equals(""))
                {
                    cls_ctTRHoliday objHoliday = new cls_ctTRHoliday();
                    bool trholiday = objHoliday.insert(input.company_code,input.planholiday_code,input.holiday_list);
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

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteMTPlanholiday(InputMTPlanholiday input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

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

                bool blnResult = controller.delete(input.planholiday_id,input.company_code);

                if (blnResult)
                {
                    cls_ctTRHoliday objHoliday = new cls_ctTRHoliday();
                    bool trholiday = objHoliday.delete(input.company_code, input.planholiday_code,input.year_code);
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
        public async Task<string> doUploadMTPlanholiday(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.4";
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
                    cls_srvAttendanceImport srv_import = new cls_srvAttendanceImport();
                    string tmp = srv_import.doImportExcel("HOLIDAY", fileName, by);


                    output["success"] = true;
                    output["message"] = tmp;

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
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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
                cls_ctMTShift objShift = new cls_ctMTShift();
                List<cls_MTShift> listShift = objShift.getDataByFillter(input.company_code,input.shift_id, input.shift_code);

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
        public string doManageMTShift(InputMTShift input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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

                string strID = objShift.insert(model);
                if (!strID.Equals(""))
                {
                    cls_ctTRShiftbreak objbreak = new cls_ctTRShiftbreak();
                    cls_ctTRShiftallowance allowance = new cls_ctTRShiftallowance();
                    bool breaks = objbreak.insert(input.company_code,input.shift_code,input.shift_break);
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

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteMTShift(InputMTShift input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadMTShift(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.4";
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
                    cls_srvAttendanceImport srv_import = new cls_srvAttendanceImport();
                    string tmp = srv_import.doImportExcel("SHIFT", fileName, by);


                    output["success"] = true;
                    output["message"] = tmp;

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
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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
        public string doManageMTPlanshift(InputMTPlanshift input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteMTPlanshift(InputMTPlanshift input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

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

                cls_ctMTPlanshift controller = new cls_ctMTPlanshift();

                bool blnResult = controller.delete(input.planshift_id,input.company_code);

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadMTPlanshift(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.4";
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
                    cls_srvAttendanceImport srv_import = new cls_srvAttendanceImport();
                    string tmp = srv_import.doImportExcel("PLANSHIFT", fileName, by);


                    output["success"] = true;
                    output["message"] = tmp;

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
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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
                cls_ctMTLeave objLeave = new cls_ctMTLeave();
                List<cls_MTLeave> listLeave = objLeave.getDataByFillter(input.company_code,input.leave_id, input.leave_code);

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
        public string doManageMTLeave(InputMTLeave input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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
                if (!strID.Equals(""))
                {
                    try
                    {
                        cls_ctTRLeaveWorkage objTRWorkage = new cls_ctTRLeaveWorkage();
                        objTRWorkage.delete(input.company_code, input.leave_code);
                        if (input.leave_workage.Count > 0){
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

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteMTLeave(InputMTLeave input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

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

                cls_ctMTLeave controller = new cls_ctMTLeave();

                bool blnResult = controller.delete(input.leave_id,input.company_code); 

                if (blnResult)
                {
                    cls_ctTRLeaveWorkage objTRWorkage = new cls_ctTRLeaveWorkage();
                    objTRWorkage.delete(input.company_code,input.leave_code);
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
        public async Task<string> doUploadMTLeave(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.4";
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
                    cls_srvAttendanceImport srv_import = new cls_srvAttendanceImport();
                    string tmp = srv_import.doImportExcel("LEAVE", fileName, by);


                    output["success"] = true;
                    output["message"] = tmp;

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

        #region MTPlanleave
        public string getMTPlanleaveList(InputMTPlanleave input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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
        public string doManageMTPlanleave(InputMTPlanleave input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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
                if (!strID.Equals(""))
                {
                    try{
                       cls_ctTRPlanleave objTRPlan = new cls_ctTRPlanleave();
                       objTRPlan.delete(input.company_code,input.planleave_code);
                       if (input.leavelists.Count > 0) {
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

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteMTPlanleave(InputMTPlanleave input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

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

                cls_ctMTPlanleave controller = new cls_ctMTPlanleave();

                bool blnResult = controller.delete(input.planleave_id,input.company_code);

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadMTPlanleave(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.4";
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
                    cls_srvAttendanceImport srv_import = new cls_srvAttendanceImport();
                    string tmp = srv_import.doImportExcel("PLANLEAVE", fileName, by);


                    output["success"] = true;
                    output["message"] = tmp;

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
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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
        public string doManageMTRateot(InputMTRateot input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteMTRateot(InputMTRateot input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadMTRateot(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.4";
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
                    cls_srvAttendanceImport srv_import = new cls_srvAttendanceImport();
                    string tmp = srv_import.doImportExcel("RATEOT", fileName, by);


                    output["success"] = true;
                    output["message"] = tmp;

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
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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
        public string doManageMTDiligence(InputMTDiligence input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteMTDiligence(InputMTDiligence input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

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

                cls_ctMTDiligence controller = new cls_ctMTDiligence();

                bool blnResult = controller.delete(input.diligence_id,input.company_code);

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadMTDiligence(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.4";
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
                    cls_srvAttendanceImport srv_import = new cls_srvAttendanceImport();
                    string tmp = srv_import.doImportExcel("DILIGENCE", fileName, by);


                    output["success"] = true;
                    output["message"] = tmp;

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
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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
        public string doManageMTLate(InputMTLate input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
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

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteMTLate(InputMTLate input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadMTLate(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.4";
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
                    cls_srvAttendanceImport srv_import = new cls_srvAttendanceImport();
                    string tmp = srv_import.doImportExcel("LATE", fileName, by);


                    output["success"] = true;
                    output["message"] = tmp;

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

        #region Timecard
        public string getTRTimecardList(FillterAttendance req)
        {

            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT901.1";
            log.apilog_by = req.username;
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


                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);

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
                model.worker_code = input.worker_code;

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


        #endregion

        #region MTTimeimpformat
        public string getMTTimeimpformatList(FillterAttendance req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT902.1";
            log.apilog_by = req.username;
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

            }

            return output.ToString(Formatting.None);

        }

        #endregion

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
    }
}
