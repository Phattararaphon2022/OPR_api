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
    }
}
