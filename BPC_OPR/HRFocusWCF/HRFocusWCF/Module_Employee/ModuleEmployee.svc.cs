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

namespace BPC_OPR
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ModuleEmployee" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ModuleEmployee.svc or ModuleEmployee.svc.cs at the Solution Explorer and start debugging.

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]

    public class ModuleEmployee : IModuleEmployee
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

        #region Worker(EMP001)
        public string getWorkerList(FillterWorker req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP001.1";
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

                cls_ctMTWorker controller = new cls_ctMTWorker();
                List<cls_MTWorker> list = controller.getDataByFillter(req.company_code,req.worker_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTWorker model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_id", model.worker_id);
                        json.Add("worker_code", model.worker_code);
                        json.Add("worker_card", model.worker_card);
                        json.Add("worker_initial", model.worker_initial);

                        json.Add("worker_fname_th", model.worker_fname_th);
                        json.Add("worker_lname_th", model.worker_lname_th);
                        json.Add("worker_fname_en", model.worker_fname_en);
                        json.Add("worker_lname_en", model.worker_lname_en);

                        json.Add("worker_type", model.worker_type);
                        json.Add("worker_gender", model.worker_gender);
                        json.Add("worker_birthdate", model.worker_birthdate);
                        json.Add("worker_hiredate", model.worker_hiredate);
                        json.Add("worker_status", model.worker_status);
                        json.Add("religion_code", model.religion_code);
                        json.Add("blood_code", model.blood_code);
                        json.Add("worker_height", model.worker_height);
                        json.Add("worker_weight", model.worker_weight);

                        json.Add("worker_resigndate", model.worker_resigndate);
                        json.Add("worker_resignstatus", model.worker_resignstatus);
                        json.Add("worker_resignreason", model.worker_resignreason);

                        json.Add("worker_probationdate", model.worker_probationdate);
                        json.Add("worker_probationenddate", model.worker_probationenddate);
                        json.Add("worker_probationday", model.worker_probationday);

                        json.Add("worker_taxmethod", model.worker_taxmethod);

                        json.Add("hrs_perday", model.hrs_perday);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);

                        json.Add("self_admin", model.self_admin);

                        json.Add("flag", model.flag);

                        json.Add("initial_name_th", model.initial_name_th);
                        json.Add("initial_name_en", model.initial_name_en);

                        json.Add("position_name_th", model.position_name_th);
                        json.Add("position_name_en", model.position_name_en);

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

                controller.dispose();
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
        public string doManageMTWorker(InputMTWorker input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP001.2";
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

                cls_ctMTWorker controller = new cls_ctMTWorker();
                cls_MTWorker model = new cls_MTWorker();

                string strWorkerCode = input.worker_code;
                string strComCode = input.company_code;

                model.company_code = strComCode;
                model.worker_id = input.worker_id;
                model.worker_code = strWorkerCode;
                model.worker_card = input.worker_card;
                model.worker_initial = input.worker_initial;
                model.worker_fname_th = input.worker_fname_th;
                model.worker_lname_th = input.worker_lname_th;
                model.worker_fname_en = input.worker_fname_en;
                model.worker_lname_en = input.worker_lname_en;
                model.worker_type = input.worker_type;
                model.worker_gender = input.worker_gender;
                model.worker_birthdate = Convert.ToDateTime(input.worker_birthdate);
                model.worker_hiredate = Convert.ToDateTime(input.worker_hiredate);
                model.worker_status = input.worker_status;
                model.religion_code = input.religion_code;
                model.blood_code = input.blood_code;
                model.worker_height = input.worker_height;
                model.worker_weight = input.worker_weight;

                model.worker_resignstatus = input.worker_resignstatus;
                if (input.worker_resignstatus.ToString().Equals("")){
                    model.worker_resigndate = Convert.ToDateTime(input.worker_resigndate);
                    model.worker_resignreason = input.worker_resignreason;
                }
                
                model.worker_probationdate = Convert.ToDateTime(input.worker_probationdate);
                model.worker_probationenddate = Convert.ToDateTime(input.worker_probationenddate);
                model.worker_probationday = input.worker_probationday;

                model.hrs_perday = input.hrs_perday;

                model.worker_taxmethod = input.worker_taxmethod;

                model.worker_pwd = "";

                model.self_admin = input.self_admin;

                model.modified_by = input.modified_by;
                model.flag = model.flag;

                string strID = controller.insert(model);

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
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }
        public string doDeleteMTWorker(InputMTWorker input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP001.3";
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

                cls_ctMTWorker controller = new cls_ctMTWorker();

                if (controller.checkDataOld(input.worker_code))
                {
                    bool blnResult = controller.delete(input.worker_code);

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
                    string message = "Not Found Project code : " + input.worker_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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
        public async Task<string> doUploadWorker(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP001.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("WORKER", fileName, "TEST");

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
        public string getWorkerFillterList(FillterSearch req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP001.5";
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

                cls_ctMTWorker controller = new cls_ctMTWorker();
                List<cls_MTWorker> list = controller.getDataByFillterAll(req.company_code, req.worker_code,req.worker_emptype,req.searchemp,req.level_code,req.dep_code,req.position_code,"",req.worker_resignstatus,req.location_code,req.date_fill);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTWorker model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_id", model.worker_id);
                        json.Add("worker_code", model.worker_code);
                        json.Add("worker_card", model.worker_card);
                        json.Add("worker_initial", model.worker_initial);

                        json.Add("worker_fname_th", model.worker_fname_th);
                        json.Add("worker_lname_th", model.worker_lname_th);
                        json.Add("worker_fname_en", model.worker_fname_en);
                        json.Add("worker_lname_en", model.worker_lname_en);

                        json.Add("worker_type", model.worker_type);
                        json.Add("worker_gender", model.worker_gender);
                        json.Add("worker_birthdate", model.worker_birthdate);
                        json.Add("worker_hiredate", model.worker_hiredate);
                        json.Add("worker_status", model.worker_status);
                        json.Add("religion_code", model.religion_code);
                        json.Add("blood_code", model.blood_code);
                        json.Add("worker_height", model.worker_height);
                        json.Add("worker_weight", model.worker_weight);

                        json.Add("worker_resigndate", model.worker_resigndate);
                        json.Add("worker_resignstatus", model.worker_resignstatus);
                        json.Add("worker_resignreason", model.worker_resignreason);

                        json.Add("worker_probationdate", model.worker_probationdate);
                        json.Add("worker_probationenddate", model.worker_probationenddate);
                        json.Add("worker_probationday", model.worker_probationday);

                        json.Add("worker_taxmethod", model.worker_taxmethod);

                        json.Add("hrs_perday", model.hrs_perday);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);

                        json.Add("self_admin", model.self_admin);

                        json.Add("flag", model.flag);

                        json.Add("initial_name_th", model.initial_name_th);
                        json.Add("initial_name_en", model.initial_name_en);

                        json.Add("position_name_th", model.position_name_th);
                        json.Add("position_name_en", model.position_name_en);

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

                controller.dispose();
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

        #region Image worker(EMP002)
        public string doUploadWorkerImages(string ref_to, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP002.1";
            log.apilog_by = "";
            log.apilog_data = "Stream";

            try
            {
                
                cls_ctTREmpimages ct_empimages = new cls_ctTREmpimages();

                string[] temp = ref_to.Split('.');

                MultipartParser parser = new MultipartParser(stream);

                if (parser.Success)
                {

                    cls_TREmpimages empimages = new cls_TREmpimages();
                    empimages.company_code = temp[0];
                    empimages.worker_code = temp[1];
                    empimages.empimages_images = parser.FileContents;
                    empimages.modified_by = temp[2];

                    empimages.empimages_no = 1;

                    ct_empimages.insert(empimages);

                    output["result"] = "1";
                    output["result_text"] = "0";

                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "0";
                }
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();
            }

            return output.ToString(Formatting.None);
        }

        public bool IsValidImage(byte[] bytes)
        {

            try
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                    Image.FromStream(ms);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }

        public string doGetWorkerImages(FillterWorker req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP002.2";
            log.apilog_by = "";
            log.apilog_data = "Stream";

            try
            {
                cls_ctTREmpimages ct_empimages = new cls_ctTREmpimages();
                List<cls_TREmpimages> list_empimages = ct_empimages.getDataByFillter(req.company_code, req.worker_code);

                if (list_empimages.Count > 0)
                {
                    cls_TREmpimages md_image = list_empimages[0];

                    bool bln = this.IsValidImage(md_image.empimages_images);

                    output["result"] = "1";
                    output["result_text"] = "";
                    output["data"] = "data:image/png;base64," + System.Convert.ToBase64String(md_image.empimages_images);
                }
                else
                {
                    output["result"] = "2";
                    output["result_text"] = "Data not found";
                    output["data"] = "";
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

        #region Dep(DEP001)

        public string getDepList(FillterWorker req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "DEP001.1";
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

                cls_ctMTDep controller = new cls_ctMTDep();
                List<cls_MTDep> list = controller.getDataByFillter("", req.level_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTDep model in list)
                    {
                        JObject json = new JObject();
                        json.Add("dep_id", model.dep_id);
                        json.Add("dep_code", model.dep_code);
                        json.Add("dep_name_th", model.dep_name_th);
                        json.Add("dep_name_en", model.dep_name_en);
                        json.Add("dep_parent", model.dep_parent);
                        json.Add("dep_level", model.dep_level);
                        json.Add("company_code", model.company_code);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageMTDep(InputMTDep input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "DEP001.2";
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

                cls_ctMTDep controller = new cls_ctMTDep();
                cls_MTDep model = new cls_MTDep();

                model.dep_id = Convert.ToInt32(input.dep_id);
                model.dep_code = input.dep_code;
                model.dep_name_th = input.dep_name_th;
                model.dep_name_en = input.dep_name_en;
                model.dep_parent = input.dep_parent;
                model.dep_level = input.dep_level;
                model.company_code = input.company_code;
                model.modified_by = input.modified_by;

                string strID = controller.insert(model);

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
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteMTDep(InputMTDep input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "DEP001.3";
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

                cls_ctMTDep controller = new cls_ctMTDep();

                if (controller.checkDataOld(input.dep_id, input.dep_code))
                {
                    bool blnResult = controller.delete(input.dep_code);

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
                    string message = "Not Found Project code : " + input.dep_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadDep(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "DEP001.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("DEP", fileName, "TEST");

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

        #region Position(PST001)
        public string getPositionList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PST001.1";
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

                cls_ctMTPosition controller = new cls_ctMTPosition();
                List<cls_MTPosition> list = controller.getDataByFillter("");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTPosition model in list)
                    {
                        JObject json = new JObject();
                        json.Add("position_id", model.position_id);
                        json.Add("position_code", model.position_code);
                        json.Add("position_name_th", model.position_name_th);
                        json.Add("position_name_en", model.position_name_en);
                        json.Add("position_level", model.position_level);
                        json.Add("company_code", model.company_code);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageMTPosition(InputMTPosition input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PST001.2";
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

                cls_ctMTPosition controller = new cls_ctMTPosition();
                cls_MTPosition model = new cls_MTPosition();

                model.position_id = Convert.ToInt32(input.position_id);
                model.position_code = input.position_code;
                model.position_name_th = input.position_name_th;
                model.position_name_en = input.position_name_en;
                model.position_level = input.position_level;
                model.company_code = input.company_code;
                model.modified_by = input.modified_by;

                string strID = controller.insert(model);

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
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteMTPosition(InputMTPosition input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PST001.3";
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

                cls_ctMTPosition controller = new cls_ctMTPosition();

                if (controller.checkDataOld(input.position_id,input.position_code))
                {
                    bool blnResult = controller.delete(input.position_code);

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
                    string message = "Not Found Project code : " + input.position_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadPosition(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PST001.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("POSITION", fileName, "TEST");

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

        #region Group(GRP001)
        public string getGroupList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "GRP001.1";
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

                cls_ctMTGroup controller = new cls_ctMTGroup();
                List<cls_MTGroup> list = controller.getDataByFillter("");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTGroup model in list)
                    {
                        JObject json = new JObject();
                        json.Add("group_id", model.group_id);
                        json.Add("group_code", model.group_code);
                        json.Add("group_name_th", model.group_name_th);
                        json.Add("group_name_en", model.group_name_en);
                        json.Add("company_code", model.company_code);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageMTGroup(InputMTGroup input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "GRP001.2";
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

                cls_ctMTGroup controller = new cls_ctMTGroup();
                cls_MTGroup model = new cls_MTGroup();

                model.group_id = Convert.ToInt32(input.group_id);
                model.group_code = input.group_code;
                model.group_name_th = input.group_name_th;
                model.group_name_en = input.group_name_en;
                model.company_code = input.company_code;
                model.modified_by = input.modified_by;

                string strID = controller.insert(model);

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
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteMTGroup(InputMTGroup input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "GRP001.3";
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

                cls_ctMTGroup controller = new cls_ctMTGroup();

                if (controller.checkDataOld(input.group_id,input.group_code))
                {
                    bool blnResult = controller.delete(input.group_code);

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
                    string message = "Not Found Project code : " + input.group_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadGroup(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "GRP001.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("GROUP", fileName, "TEST");

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

        #region Initial(INI001)
        public string getInitialList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "INI001.1";
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

                cls_ctMTInitial controller = new cls_ctMTInitial();
                List<cls_MTInitial> list = controller.getDataByFillter("");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTInitial model in list)
                    {
                        JObject json = new JObject();
                        json.Add("initial_id", model.initial_id);
                        json.Add("initial_code", model.initial_code);
                        json.Add("initial_name_th", model.initial_name_th);
                        json.Add("initial_name_en", model.initial_name_en);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageMTInitial(InputMTInitial input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "INI001.2";
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

                cls_ctMTInitial controller = new cls_ctMTInitial();
                cls_MTInitial model = new cls_MTInitial();

                model.initial_id = Convert.ToInt32(input.initial_id);
                model.initial_code = input.initial_code;
                model.initial_name_th = input.initial_name_th;
                model.initial_name_en = input.initial_name_en;
                model.modified_by = input.modified_by;

                string strID = controller.insert(model);

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
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteMTInitial(InputMTInitial input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "INI001.3";
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

                cls_ctMTInitial controller = new cls_ctMTInitial();

                if (controller.checkDataOld(input.initial_id,input.initial_code))
                {
                    bool blnResult = controller.delete(input.initial_code);

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
                    string message = "Not Found Project code : " + input.initial_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadInitial(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "INI001.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("INITIAL", fileName, "TEST");

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

        #region Type(TYP001)
        public string getTypeList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "TYP001.1";
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

                cls_ctMTType controller = new cls_ctMTType();
                List<cls_MTType> list = controller.getDataByFillter("");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTType model in list)
                    {
                        JObject json = new JObject();
                        json.Add("type_id", model.type_id);
                        json.Add("type_code", model.type_code);
                        json.Add("type_name_th", model.type_name_th);
                        json.Add("type_name_en", model.type_name_en);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageMTType(InputMTType input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "TYP001.2";
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

                cls_ctMTType controller = new cls_ctMTType();
                cls_MTType model = new cls_MTType();

                model.type_id = Convert.ToInt32(input.type_id);
                model.type_code = input.type_code;
                model.type_name_th = input.type_name_th;
                model.type_name_en = input.type_name_en;
                model.modified_by = input.modified_by;

                string strID = controller.insert(model);

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
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteMTType(InputMTType input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "TYP001.3";
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

                cls_ctMTType controller = new cls_ctMTType();

                if (controller.checkDataOld(input.type_id, input.type_code))
                {
                    bool blnResult = controller.delete(input.type_code);

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
                    string message = "Not Found Project code : " + input.type_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadType(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "TYP001.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("TYPE", fileName, "TEST");

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

        #region Status(STT001)
        public string getStatusList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "STT001.1";
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

                cls_ctMTStatus controller = new cls_ctMTStatus();
                List<cls_MTStatus> list = controller.getDataByFillter("");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTStatus model in list)
                    {
                        JObject json = new JObject();
                        json.Add("status_id", model.status_id);
                        json.Add("status_code", model.status_code);
                        json.Add("status_name_th", model.status_name_th);
                        json.Add("status_name_en", model.status_name_en);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageMTStatus(InputMTStatus input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "STT001.2";
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

                cls_ctMTStatus controller = new cls_ctMTStatus();
                cls_MTStatus model = new cls_MTStatus();

                model.status_id = Convert.ToInt32(input.status_id);
                model.status_code = input.status_code;
                model.status_name_th = input.status_name_th;
                model.status_name_en = input.status_name_en;
                model.modified_by = input.modified_by;

                string strID = controller.insert(model);

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
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteMTStatus(InputMTStatus input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "STT001.3";
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

                cls_ctMTStatus controller = new cls_ctMTStatus();

                if (controller.checkDataOld(input.status_id, input.status_code))
                {
                    bool blnResult = controller.delete(input.status_code);

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
                    string message = "Not Found Project code : " + input.status_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadStatus(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "STT001.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("STATUS", fileName, "TEST");

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

        #region TR_Location(EMP019)
        public string getTREmpLocationList(FillterWorker input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP019.1";
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

                cls_ctTREmplocation controller = new cls_ctTREmplocation();
                List<cls_TREmplocation> list = controller.getDataByFillter(input.company_code, input.worker_code,"");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TREmplocation model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);

                        json.Add("location_code", model.location_code);
                        json.Add("emplocation_startdate", model.emplocation_startdate);
                        json.Add("emplocation_enddate", model.emplocation_enddate);
                        json.Add("emplocation_note", model.emplocation_note);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageTREmpLocation(InputWorkerTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP019.2";
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

                cls_ctTREmplocation controller = new cls_ctTREmplocation();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TREmplocation>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code, input.worker_code);

                if (clear)
                {
                    foreach (cls_TREmplocation model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
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

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteTREmpLocation(InputTREmpLocation input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP019.3";
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

                cls_ctTREmplocation controller = new cls_ctTREmplocation();

                if (controller.checkDataOld(input.company_code, input.worker_code))
                {
                    bool blnResult = controller.delete(input.company_code, input.worker_code);

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
                    string message = "Not Found Project code : " + input.location_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadEmpLocation(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP019.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("EMPLOCATION", fileName, "TEST");

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

        #region TR_Branch(EMP021)
        public string getTREmpBranchList(FillterWorker input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP021.1";
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

                cls_ctTREmpbranch controller = new cls_ctTREmpbranch();
                List<cls_TREmpbranch> list = controller.getDataByFillter(input.company_code, input.worker_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TREmpbranch model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);

                        json.Add("branch_code", model.branch_code);
                        json.Add("empbranch_startdate", model.empbranch_startdate);
                        json.Add("empbranch_enddate", model.empbranch_enddate);
                        json.Add("empbranch_note", model.empbranch_note);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageTREmpBranch(InputWorkerTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP021.2";
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

                cls_ctTREmpbranch controller = new cls_ctTREmpbranch();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TREmpbranch>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code, input.worker_code);

                if (clear)
                {
                    foreach (cls_TREmpbranch model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
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

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteTREmpBranch(InputTREmpBranch input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP021.3";
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

                cls_ctTREmpbranch controller = new cls_ctTREmpbranch();

                if (controller.checkDataOld(input.company_code, input.worker_code))
                {
                    bool blnResult = controller.delete(input.company_code, input.worker_code);

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
                    string message = "Not Found Project code : " + input.branch_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadEmpBranch(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP021.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("EMPBRANCH", fileName, "TEST");

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

        #region TR_Address(EMP003)
        public string getTRAddressList(FillterWorker input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP003.1";
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

                cls_ctTRAddress controller = new cls_ctTRAddress();
                List<cls_TRAddress> list = controller.getDataByFillter(input.company_code,input.worker_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRAddress model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("address_id", model.address_id);
                        json.Add("address_type", model.address_type);
                        json.Add("address_no", model.address_no);
                        json.Add("address_moo", model.address_moo);
                        json.Add("address_soi", model.address_soi);
                        json.Add("address_road", model.address_road);
                        json.Add("address_tambon", model.address_tambon);
                        json.Add("address_amphur", model.address_amphur);
                        json.Add("province_code", model.province_code);
                        json.Add("address_zipcode", model.address_zipcode);
                        json.Add("address_tel", model.address_tel);
                        json.Add("address_email", model.address_email);
                        json.Add("address_line", model.address_line);
                        json.Add("address_facebook", model.address_facebook);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageTRAddress(InputWorkerTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP003.2";
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

                cls_ctTRAddress controller = new cls_ctTRAddress();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRAddress>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code,input.worker_code);

                if (clear)
                {
                    foreach (cls_TRAddress model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
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

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteTRAddress(InputTRAddress input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP003.3";
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

                cls_ctTRAddress controller = new cls_ctTRAddress();

                if (controller.checkDataOld(input.company_code,input.worker_code))
                {
                    bool blnResult = controller.delete(input.company_code, input.worker_code);

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
                    string message = "Not Found Project code : " + input.address_id;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadAddress(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP003.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("EMPADDRESS", fileName, "TEST");

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

        #region TR_Card(EMP004)
        public string getTRCardList(FillterWorker input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP004.1";
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

                cls_ctTRCard controller = new cls_ctTRCard();
                List<cls_TRCard> list = controller.getDataByFillter(input.company_code, input.worker_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRCard model in list)
                    {
                        JObject json = new JObject();
                        json.Add("card_id", model.card_id);
                        json.Add("card_code", model.card_code);
                        json.Add("card_type", model.card_type);
                        json.Add("card_issue", model.card_issue);
                        json.Add("card_expire", model.card_expire);

                        json.Add("company_code", model.company_code);


                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageTRCard(InputWorkerTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP004.2";
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

                cls_ctTRCard controller = new cls_ctTRCard();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRCard>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code, input.worker_code);

                if (clear)
                {
                    foreach (cls_TRCard model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
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

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteTRCard(InputTRCard input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP004.3";
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

                cls_ctTRCard controller = new cls_ctTRCard();

                if (controller.checkDataOld(input.company_code, input.worker_code))
                {
                    bool blnResult = controller.delete(input.company_code, input.worker_code);

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
                    string message = "Not Found Project code : " + input.card_id;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadCard(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP004.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("EMPCARD", fileName, "TEST");

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

        #region TR_Bank(EMP005)
        public string getTRBankList(FillterWorker input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP005.1";
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

                cls_ctTRBank controller = new cls_ctTRBank();
                List<cls_TRBank> list = controller.getDataByFillter(input.company_code, input.worker_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRBank model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("bank_id", model.bank_id);
                        json.Add("bank_code", model.bank_code);
                        json.Add("bank_account", model.bank_account);
                        json.Add("bank_percent", model.bank_percent);
                        json.Add("bank_cashpercent", model.bank_cashpercent);

                        json.Add("bank_bankname", model.bank_bankname);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageTRBank(InputWorkerTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP005.2";
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

                cls_ctTRBank controller = new cls_ctTRBank();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRBank>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code, input.worker_code);

                if (clear)
                {
                    foreach (cls_TRBank model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
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

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }


                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteTRBank(InputTRBank input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP005.3";
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

                cls_ctTRBank controller = new cls_ctTRBank();

                if (controller.checkDataOld(input.company_code, input.worker_code))
                {
                    bool blnResult = controller.delete(input.company_code, input.worker_code);

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
                    string message = "Not Found Project code : " + input.bank_id;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadBank(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP005.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("EMPBANK", fileName, "TEST");

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

        #region TR_Family(EMP006)
        public string getTRFamilyList(FillterWorker input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP006.1";
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

                cls_ctTRFamily controller = new cls_ctTRFamily();
                List<cls_TRFamily> list = controller.getDataByFillter(input.company_code, input.worker_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRFamily model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("family_id", model.family_id);
                        json.Add("family_code", model.family_code);
                        json.Add("family_type", model.family_type);
                        json.Add("family_fname_th", model.family_fname_th);
                        json.Add("family_lname_th", model.family_lname_th);
                        json.Add("family_fname_en", model.family_fname_en);
                        json.Add("family_lname_en", model.family_lname_en);
                        json.Add("family_birthdate", model.family_birthdate);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageTRFamily(InputWorkerTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP006.2";
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

                cls_ctTRFamily controller = new cls_ctTRFamily();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRFamily>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code, input.worker_code);

                if (clear)
                {
                    foreach (cls_TRFamily model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
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

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteTRFamily(InputTREmpfamily input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP006.3";
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

                cls_ctTRFamily controller = new cls_ctTRFamily();

                if (controller.checkDataOld(input.company_code, input.worker_code))
                {
                    bool blnResult = controller.delete(input.company_code, input.worker_code);

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
                    string message = "Not Found Project code : " + input.family_id;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadFamily(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP006.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("EMPFAMILY", fileName, "TEST");

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

        #region TR_Hospital(EMP007)
        public string getTRHospitalList(FillterWorker input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP007.1";
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

                cls_ctTRHospital controller = new cls_ctTRHospital();
                List<cls_TRHospital> list = controller.getDataByFillter(input.company_code, input.worker_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRHospital model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("emphospital_id", model.emphospital_id);
                        json.Add("emphospital_code", model.emphospital_code);
                        json.Add("emphospital_date", model.emphospital_date);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageTRHospital(InputWorkerTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP007.2";
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

                cls_ctTRHospital controller = new cls_ctTRHospital();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRHospital>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code, input.worker_code);

                if (clear)
                {
                    foreach (cls_TRHospital model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
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

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteTRHospital(InputTREmpHospital input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP007.3";
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

                cls_ctTRHospital controller = new cls_ctTRHospital();

                if (controller.checkDataOld(input.company_code, input.worker_code))
                {
                    bool blnResult = controller.delete(input.company_code, input.worker_code);

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
                    string message = "Not Found Project code : " + input.emphospital_id;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadHospital(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP007.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("EMPHOSPITAL", fileName, "TEST");

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

        #region TR_Foreigner(EMP008)
        public string getTRForeignerList(FillterWorker input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP008.1";
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

                cls_ctTRForeigner controller = new cls_ctTRForeigner();
                List<cls_TRForeigner> list = controller.getDataByFillter(input.company_code, input.worker_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRForeigner model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("foreigner_id", model.foreigner_id);
                        json.Add("passport_no", model.passport_no);
                        json.Add("passport_issue", model.passport_issue);
                        json.Add("passport_expire", model.passport_expire);
                        json.Add("visa_no", model.visa_no);
                        json.Add("visa_issue", model.visa_issue);
                        json.Add("visa_expire", model.visa_expire);
                        json.Add("workpermit_no", model.workpermit_no);
                        json.Add("workpermit_by", model.workpermit_by);
                        json.Add("workpermit_issue", model.workpermit_issue);
                        json.Add("workpermit_expire", model.workpermit_expire);
                        json.Add("entry_date", model.entry_date);
                        json.Add("certificate_no", model.certificate_no);
                        json.Add("certificate_expire", model.certificate_expire);
                        json.Add("otherdoc_no", model.otherdoc_no);
                        json.Add("otherdoc_expire", model.otherdoc_expire);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageTRForeigner(InputTREmpForeigner input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP008.2";
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

                cls_ctTRForeigner controller = new cls_ctTRForeigner();
                cls_TRForeigner model = new cls_TRForeigner();

                model.company_code = input.company_code;
                model.worker_code = input.worker_code;
                model.foreigner_id = Convert.ToInt32(input.foreigner_id);
                model.passport_no = input.passport_no;
                model.passport_issue = Convert.ToDateTime(input.passport_issue);
                model.passport_expire = Convert.ToDateTime(input.passport_expire);
                model.visa_no = input.visa_no;
                model.visa_issue = Convert.ToDateTime(input.visa_issue);
                model.visa_expire = Convert.ToDateTime(input.visa_expire);
                model.workpermit_no = input.workpermit_no;
                model.workpermit_by = input.workpermit_by;
                model.workpermit_issue = Convert.ToDateTime(input.workpermit_issue);
                model.workpermit_expire = Convert.ToDateTime(input.workpermit_expire);
                model.entry_date = Convert.ToDateTime(input.entry_date);
                model.certificate_no = input.certificate_no;
                model.certificate_expire = Convert.ToDateTime(input.certificate_expire);
                model.otherdoc_no = input.otherdoc_no;
                model.otherdoc_expire = Convert.ToDateTime(input.otherdoc_expire);

                model.modified_by = input.modified_by;

                string strID = controller.insert(model);

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
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteTRForeigner(InputTREmpForeigner input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP008.3";
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

                cls_ctTRForeigner controller = new cls_ctTRForeigner();

                if (controller.checkDataOld(input.company_code, input.worker_code, input.foreigner_id.ToString()))
                {
                    bool blnResult = controller.delete(input.company_code, input.worker_code, input.foreigner_id.ToString());

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
                    string message = "Not Found Project code : " + input.foreigner_id;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadForeigner(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP008.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("EMPFOREIGNER", fileName, "TEST");

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

        #region TR_DEP(EMP009)
        public string getTRDepList(FillterWorker input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP009.1";
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

                cls_ctTRDep controller = new cls_ctTRDep();
                List<cls_TRDep> list = controller.getDataByFillter(input.company_code, input.worker_code,"");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRDep model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("empdep_id", model.empdep_id);
                        json.Add("empdep_date", model.empdep_date);
                        json.Add("empdep_level01", model.empdep_level01);
                        json.Add("empdep_level02", model.empdep_level02);
                        json.Add("empdep_level03", model.empdep_level03);
                        json.Add("empdep_level04", model.empdep_level04);
                        json.Add("empdep_level05", model.empdep_level05);
                        json.Add("empdep_level06", model.empdep_level06);
                        json.Add("empdep_level07", model.empdep_level07);
                        json.Add("empdep_level08", model.empdep_level08);
                        json.Add("empdep_level09", model.empdep_level09);
                        json.Add("empdep_level10", model.empdep_level10);
                        json.Add("empdep_reason", model.empdep_reason);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageTRDep(InputWorkerTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP009.2";
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

                cls_ctTRDep controller = new cls_ctTRDep();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRDep>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code, input.worker_code);

                if (clear)
                {
                    foreach (cls_TRDep model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
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

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteTRDep(InputTREmpDep input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP009.3";
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

                cls_ctTRDep controller = new cls_ctTRDep();

                if (controller.checkDataOld(input.company_code, input.worker_code))
                {
                    bool blnResult = controller.delete(input.company_code, input.worker_code);

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
                    string message = "Not Found Project code : " + input.empdep_id;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadEmpDep(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP009.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("EMPDEP", fileName, "TEST");

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

        #region TR_Position(EMP010)
        public string getTRPositionList(FillterWorker input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP010.1";
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

                cls_ctTRPosition controller = new cls_ctTRPosition();
                List<cls_TRPosition> list = controller.getDataByFillter(input.company_code, input.worker_code,"");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRPosition model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("empposition_id", model.empposition_id);
                        json.Add("empposition_date", model.empposition_date);
                        json.Add("empposition_position", model.empposition_position);
                        json.Add("empposition_reason", model.empposition_reason);
                        json.Add("position_name_th", model.position_name_th);
                        json.Add("position_name_en", model.position_name_en);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageTRPosition(InputWorkerTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP010.2";
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

                cls_ctTRPosition controller = new cls_ctTRPosition();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRPosition>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code, input.worker_code);

                if (clear)
                {
                    foreach (cls_TRPosition model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
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

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteTRPosition(InputTREmpPosition input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP010.3";
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

                cls_ctTRPosition controller = new cls_ctTRPosition();

                if (controller.checkDataOld(input.company_code, input.worker_code))
                {
                    bool blnResult = controller.delete(input.company_code, input.worker_code);

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
                    string message = "Not Found Project code : " + input.empposition_id;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadEmpPosition(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP010.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("EMPPOSITION", fileName, "TEST");

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

        #region TR_Group(EMP020)
        public string getTRGroupList(FillterWorker input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP020.1";
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

                cls_ctTRGroup controller = new cls_ctTRGroup();
                List<cls_TRGroup> list = controller.getDataByFillter(input.company_code, input.worker_code,"");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRGroup model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("empgroup_id", model.empgroup_id);
                        json.Add("empgroup_code", model.empgroup_code);
                        json.Add("empgroup_date", model.empgroup_date);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageTRGroup(InputWorkerTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP020.2";
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

                cls_ctTRGroup controller = new cls_ctTRGroup();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRGroup>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code, input.worker_code);

                if (clear)
                {
                    foreach (cls_TRGroup model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
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

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteTRGroup(InputTREmpGroup input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP020.3";
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

                cls_ctTRGroup controller = new cls_ctTRGroup();

                if (controller.checkDataOld(input.company_code, input.worker_code))
                {
                    bool blnResult = controller.delete(input.company_code, input.worker_code);

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
                    string message = "Not Found Project code : " + input.empgroup_id;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadEmpGroup(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP020.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("EMPGROUP", fileName, "TEST");

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

        #region TR_Education(EMP011)
        public string getTREducationList(FillterWorker input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP011.1";
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

                cls_ctTREducation controller = new cls_ctTREducation();
                List<cls_TREducation> list = controller.getDataByFillter(input.company_code, input.worker_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TREducation model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("empeducation_no", model.empeducation_no);
                        json.Add("empeducation_gpa", model.empeducation_gpa);
                        json.Add("empeducation_start", model.empeducation_start);
                        json.Add("empeducation_finish", model.empeducation_finish);
                        json.Add("institute_code", model.institute_code);
                        json.Add("faculty_code", model.faculty_code);
                        json.Add("major_code", model.major_code);
                        json.Add("qualification_code", model.qualification_code);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageTREducation(InputWorkerTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP011.2";
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

                cls_ctTREducation controller = new cls_ctTREducation();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TREducation>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code, input.worker_code);

                if (clear)
                {
                    foreach (cls_TREducation model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
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

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteTREducation(InputTREmpEducation input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP011.3";
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

                cls_ctTREducation controller = new cls_ctTREducation();

                if (controller.checkDataOld(input.company_code, input.worker_code))
                {
                    bool blnResult = controller.delete(input.company_code, input.worker_code);

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
                    string message = "Not Found Project code : " + input.empeducation_no;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadEmpEducation(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP011.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("EMPEDUCATION", fileName, "TEST");

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

        #region TR_Training(EMP012)
        public string getTRTrainingList(FillterWorker input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP012.1";
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

                cls_ctTRTraining controller = new cls_ctTRTraining();
                List<cls_TRTraining> list = controller.getDataByFillter(input.company_code, input.worker_code,"");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRTraining model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("emptraining_no", model.emptraining_no);
                        json.Add("emptraining_start", model.emptraining_start);
                        json.Add("emptraining_finish", model.emptraining_finish);
                        json.Add("emptraining_status", model.emptraining_status);
                        json.Add("emptraining_hours", model.emptraining_hours);
                        json.Add("emptraining_cost", model.emptraining_cost);
                        json.Add("emptraining_note", model.emptraining_note);
                        json.Add("institute_code", model.institute_code);
                        json.Add("institute_other", model.institute_other);
                        json.Add("course_code", model.course_code);
                        json.Add("course_other", model.course_other);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageTRTraining(InputWorkerTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP012.2";
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

                cls_ctTRTraining controller = new cls_ctTRTraining();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRTraining>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code, input.worker_code);

                if (clear)
                {
                    foreach (cls_TRTraining model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
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

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteTRTraining(InputTREmpTraining input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP012.3";
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

                cls_ctTRTraining controller = new cls_ctTRTraining();

                if (controller.checkDataOld(input.company_code, input.worker_code))
                {
                    bool blnResult = controller.delete(input.company_code, input.worker_code);

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
                    string message = "Not Found Project code : " + input.emptraining_no;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadEmpTraining(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP012.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("EMPTRAINING", fileName, "TEST");

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

        #region TR_Assessment(EMP013)
        public string getTRAssessmentList(FillterWorker input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP013.1";
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

                cls_ctTRAssessment controller = new cls_ctTRAssessment();
                List<cls_TRAssessment> list = controller.getDataByFillter(input.company_code, input.worker_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRAssessment model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("empassessment_id", model.empassessment_id);
                        json.Add("empassessment_location", model.empassessment_location);
                        json.Add("empassessment_topic", model.empassessment_topic);
                        json.Add("empassessment_fromdate", model.empassessment_fromdate);
                        json.Add("empassessment_todate", model.empassessment_todate);
                        json.Add("empassessment_count", model.empassessment_count);
                        json.Add("empassessment_result", model.empassessment_result);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageTRAssessment(InputWorkerTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP013.2";
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

                cls_ctTRAssessment controller = new cls_ctTRAssessment();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRAssessment>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code, input.worker_code);

                if (clear)
                {
                    foreach (cls_TRAssessment model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
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

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteTRAssessment(InputTREmpAssessment input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP013.3";
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

                cls_ctTRAssessment controller = new cls_ctTRAssessment();

                if (controller.checkDataOld(input.company_code, input.worker_code))
                {
                    bool blnResult = controller.delete(input.company_code, input.worker_code);

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
                    string message = "Not Found Project code : " + input.empassessment_id;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadEmpAssessment(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP013.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("EMPASSESSMENT", fileName, "TEST");

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

        #region TR_Criminal(EMP014)
        public string getTRCriminalList(FillterWorker input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP014.1";
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

                cls_ctTRCriminal controller = new cls_ctTRCriminal();
                List<cls_TRCriminal> list = controller.getDataByFillter(input.company_code, input.worker_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRCriminal model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("empcriminal_id", model.empcriminal_id);
                        json.Add("empcriminal_location", model.empcriminal_location);
                        json.Add("empcriminal_fromdate", model.empcriminal_fromdate);
                        json.Add("empcriminal_todate", model.empcriminal_todate);
                        json.Add("empcriminal_count", model.empcriminal_count);
                        json.Add("empcriminal_result", model.empcriminal_result);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageTRCriminal(InputWorkerTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP014.2";
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

                cls_ctTRCriminal controller = new cls_ctTRCriminal();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRCriminal>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code, input.worker_code);

                if (clear)
                {
                    foreach (cls_TRCriminal model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
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

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteTRCriminal(InputTREmpCriminal input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP014.3";
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

                cls_ctTRCriminal controller = new cls_ctTRCriminal();

                if (controller.checkDataOld(input.company_code, input.worker_code))
                {
                    bool blnResult = controller.delete(input.company_code, input.worker_code);

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
                    string message = "Not Found Project code : " + input.empcriminal_id;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadEmpCriminal(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP014.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("EMPCRIMINAL", fileName, "TEST");

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

        #region TR_Salary(EMP015)
        public string getTRSalaryList(FillterWorker input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP015.1";
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

                cls_ctTRSalary controller = new cls_ctTRSalary();
                List<cls_TRSalary> list = controller.getDataByFillter(input.company_code, input.worker_code,"");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRSalary model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("empsalary_id", model.empsalary_id);
                        json.Add("empsalary_amount", model.empsalary_amount);
                        json.Add("empsalary_date", model.empsalary_date);
                        json.Add("empsalary_reason", model.empsalary_reason);

                        json.Add("empsalary_incamount", model.empsalary_incamount);
                        json.Add("empsalary_incpercent", model.empsalary_incpercent);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageTRSalary(InputWorkerTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP015.2";
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

                cls_ctTRSalary controller = new cls_ctTRSalary();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRSalary>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code, input.worker_code);

                if (clear)
                {
                    foreach (cls_TRSalary model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
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

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteTRSalary(InputTREmpSalary input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP015.3";
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

                cls_ctTRSalary controller = new cls_ctTRSalary();

                if (controller.checkDataOld(input.company_code, input.worker_code))
                {
                    bool blnResult = controller.delete(input.company_code, input.worker_code);

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
                    string message = "Not Found Project code : " + input.empsalary_id;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadEmpSalary(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP015.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("EMPSALARY", fileName, "TEST");

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

        #region TR_Provident(EMP016)
        public string getTRProvidentList(FillterWorker input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP016.1";
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

                cls_ctTRProvident controller = new cls_ctTRProvident();
                List<cls_TRProvident> list = controller.getDataByFillter(input.company_code, input.worker_code,"");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRProvident model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("provident_code", model.provident_code);
                        json.Add("empprovident_card", model.empprovident_card);
                        json.Add("empprovident_entry", model.empprovident_entry);
                        json.Add("empprovident_start", model.empprovident_start);
                        json.Add("empprovident_end", model.empprovident_end);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageTRProvident(InputWorkerTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP016.2";
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

                cls_ctTRProvident controller = new cls_ctTRProvident();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRProvident>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code, input.worker_code);

                if (clear)
                {
                    foreach (cls_TRProvident model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
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

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteTRProvident(InputTREmpProvident input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP016.3";
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

                cls_ctTRProvident controller = new cls_ctTRProvident();

                if (controller.checkDataOld(input.company_code, input.worker_code))
                {
                    bool blnResult = controller.delete(input.company_code, input.worker_code);

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
                    string message = "Not Found Project code : " + input.provident_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadEmpProvident(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP016.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("EMPPROVIDENT", fileName, "TEST");

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

        #region TR_Benefit(EMP017)
        public string getTRBenefitList(FillterWorker input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP017.1";
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

                cls_ctTRBenefit controller = new cls_ctTRBenefit();
                List<cls_TRBenefit> list = controller.getDataByFillter(input.company_code, input.worker_code,"");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRBenefit model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);

                        json.Add("empbenefit_id", model.empbenefit_id);
                        json.Add("item_code", model.item_code);
                        json.Add("empbenefit_amount", model.empbenefit_amount);
                        json.Add("empbenefit_startdate", model.empbenefit_startdate);
                        json.Add("empbenefit_enddate", model.empbenefit_enddate);
                        json.Add("empbenefit_reason", model.empbenefit_reason);
                        json.Add("empbenefit_note", model.empbenefit_note);

                        json.Add("empbenefit_paytype", model.empbenefit_paytype);
                        json.Add("empbenefit_break", model.empbenefit_break);
                        json.Add("empbenefit_breakreason", model.empbenefit_breakreason);

                        json.Add("empbenefit_conditionpay", model.empbenefit_conditionpay);
                        json.Add("empbenefit_payfirst", model.empbenefit_payfirst);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageTRBenefit(InputWorkerTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP017.2";
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

                cls_ctTRBenefit controller = new cls_ctTRBenefit();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRBenefit>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code, input.worker_code);

                if (clear)
                {
                    foreach (cls_TRBenefit model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
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

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteTRBenefit(InputTREmpBenefit input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP017.3";
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

                cls_ctTRBenefit controller = new cls_ctTRBenefit();

                if (controller.checkDataOld(input.company_code, input.worker_code))
                {
                    bool blnResult = controller.delete(input.company_code, input.worker_code);

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
                    string message = "Not Found Project code : " + input.empbenefit_id;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadEmpBenefit(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP017.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("EMPBENEFIT", fileName, "TEST");

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

        #region TR_Reduce(EMP018)
        public string getTRReduceList(FillterWorker input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP018.1";
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

                cls_ctTRReduce controller = new cls_ctTRReduce();
                List<cls_TRReduce> list = controller.getDataByFillter(input.company_code, input.worker_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRReduce model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);

                        json.Add("empreduce_id", model.empreduce_id);
                        json.Add("reduce_type", model.reduce_type);
                        json.Add("empreduce_amount", model.empreduce_amount);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageTRReduce(InputWorkerTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP018.2";
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

                cls_ctTRReduce controller = new cls_ctTRReduce();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRReduce>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code, input.worker_code);

                if (clear)
                {
                    foreach (cls_TRReduce model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
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

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteTRReduce(InputTREmpReduce input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP018.3";
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

                cls_ctTRReduce controller = new cls_ctTRReduce();

                if (controller.checkDataOld(input.company_code, input.worker_code))
                {
                    bool blnResult = controller.delete(input.company_code, input.worker_code);

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
                    string message = "Not Found Project code : " + input.empreduce_id;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadEmpReduce(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP018.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("EMPREDUCE", fileName, "TEST");

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

        #region TR_Accumalate()
        public string getTRAccumalateList(FillterWorker input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP025.1";
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

                cls_ctTRAccumalate controller = new cls_ctTRAccumalate();
                List<cls_TRAccumalate> list = controller.getDataByFillter(input.company_code, input.worker_code,input.paydate);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRAccumalate model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);

                        json.Add("periodyear", model.periodyear);
                        json.Add("periodid", model.periodid);
                        json.Add("paydate", model.paydate);
                        json.Add("incomefix", model.incomefix);
                        json.Add("incomevar", model.incomevar);
                        json.Add("taxfix", model.taxfix);
                        json.Add("taxvar", model.taxvar);
                        json.Add("incomefortyone", model.incomefortyone);
                        json.Add("incomefortyoneperthree", model.incomefortyoneperthree);
                        json.Add("incomefortyonetwo", model.incomefortyonetwo);
                        json.Add("incomefortytwoin", model.incomefortytwoin);
                        json.Add("incomefortytwoout", model.incomefortytwoout);
                        json.Add("taxfortyone", model.taxfortyone);
                        json.Add("taxfortyoneperthree", model.taxfortyoneperthree);
                        json.Add("taxfortyonetwo", model.taxfortyonetwo);
                        json.Add("taxfortytwoin", model.taxfortytwoin);
                        json.Add("taxfortytwoout", model.taxfortytwoout);
                        json.Add("ssoacc_worker", model.ssoacc_worker);
                        json.Add("ssoacc_company", model.ssoacc_company);
                        json.Add("pfacc_worker", model.pfacc_worker);
                        json.Add("pfacc_company", model.pfacc_company);
                        json.Add("allwanceinctax", model.allwanceinctax);
                        json.Add("allwancenotinctax", model.allwancenotinctax);
                        json.Add("deductinctax", model.deductinctax);
                        json.Add("deductnotinctax", model.deductnotinctax);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        #region TR_Supply(EMP022)
        public string getTRSupplyList(FillterWorker input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP022.1";
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

                cls_ctTRSupply controller = new cls_ctTRSupply();
                List<cls_TRSupply> list = controller.getDataByFillter(input.company_code, input.worker_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRSupply model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("empsupply_id", model.empsupply_id);
                        json.Add("empsupply_code", model.empsupply_code);
                        json.Add("empsupply_qauntity", model.empsupply_qauntity);
                        json.Add("empsupply_issuedate", model.empsupply_issuedate);
                        json.Add("empsupply_note", model.empsupply_note);
                        json.Add("empsupply_returndate", model.empsupply_returndate);
                        json.Add("empsupply_returnstatus", model.empsupply_returnstatus);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageTRSupply(InputWorkerTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP022.2";
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

                cls_ctTRSupply controller = new cls_ctTRSupply();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRSupply>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code, input.worker_code);

                if (clear)
                {
                    foreach (cls_TRSupply model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
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

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteTRSupply(InputTREmpSupply input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP022.3";
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

                cls_ctTRSupply controller = new cls_ctTRSupply();

                if (controller.checkDataOld(input.company_code, input.worker_code))
                {
                    bool blnResult = controller.delete(input.company_code, input.worker_code);

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
                    string message = "Not Found Project code : " + input.empsupply_id;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadEmpSupply(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP022.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("EMPSUPPLY", fileName, "TEST");

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

        #region TR_Uniform(EMP023)
        public string getTRUniformList(FillterWorker input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP023.1";
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

                cls_ctTRUniform controller = new cls_ctTRUniform();
                List<cls_TRUniform> list = controller.getDataByFillter(input.company_code, input.worker_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRUniform model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("empuniform_id", model.empuniform_id);
                        json.Add("empuniform_code", model.empuniform_code);
                        json.Add("empuniform_qauntity", model.empuniform_qauntity);
                        json.Add("empuniform_amount", model.empuniform_amount);
                        json.Add("empuniform_issuedate", model.empuniform_issuedate);
                        json.Add("empuniform_note", model.empuniform_note);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageTRUniform(InputWorkerTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP023.2";
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

                cls_ctTRUniform controller = new cls_ctTRUniform();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRUniform>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code, input.worker_code);

                if (clear)
                {
                    foreach (cls_TRUniform model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
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

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteTRUniform(InputTREmpUniform input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP023.3";
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

                cls_ctTRUniform controller = new cls_ctTRUniform();

                if (controller.checkDataOld(input.company_code, input.worker_code))
                {
                    bool blnResult = controller.delete(input.company_code, input.worker_code);

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
                    string message = "Not Found Project code : " + input.empuniform_id;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadEmpUnifrom(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP023.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("EMPUNIFORM", fileName, "TEST");

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

        #region TR_Suggest(EMP024)
        public string getTRSuggestList(FillterWorker input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP024.1";
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

                cls_ctTRSuggest controller = new cls_ctTRSuggest();
                List<cls_TRSuggest> list = controller.getDataByFillter(input.company_code, input.worker_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRSuggest model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("empsuggest_id", model.empsuggest_id);
                        json.Add("empsuggest_code", model.empsuggest_code);
                        json.Add("empsuggest_date", model.empsuggest_date);
                        json.Add("empsuggest_note", model.empsuggest_note);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
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

                controller.dispose();
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

        public string doManageTRSuggest(InputWorkerTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP024.2";
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

                cls_ctTRSuggest controller = new cls_ctTRSuggest();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRSuggest>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code, input.worker_code);

                if (clear)
                {
                    foreach (cls_TRSuggest model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
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

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

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

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteTRSuggest(InputTREmpSuggest input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP024.3";
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

                cls_ctTRSuggest controller = new cls_ctTRSuggest();

                if (controller.checkDataOld(input.company_code, input.worker_code))
                {
                    bool blnResult = controller.delete(input.company_code, input.worker_code);

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
                    string message = "Not Found Project code : " + input.empsuggest_id;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
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

        public async Task<string> doUploadEmpSuggest(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMP024.4";
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
                    cls_srvEmpImport srv_import = new cls_srvEmpImport();
                    string tmp = srv_import.doImportExcel("EMPSUGGEST", fileName, "TEST");

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

        #region Set batch

        //Set Position (EMBT001)
        public string getBatchPositionList(InputSetPosition input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMBT001.1";
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
                cls_ctTRPosition objPolItem = new cls_ctTRPosition();
                List<cls_TRPosition> listPolItem = objPolItem.getDataBatch(input.company_code,input.empposition_position,Convert.ToDateTime(input.empposition_date));

                JArray array = new JArray();

                if (listPolItem.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRPosition model in listPolItem)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("empposition_id", model.empposition_id);
                        json.Add("empposition_date", model.empposition_date);
                        json.Add("empposition_position", model.empposition_position);

                        json.Add("worker_detail_th", model.worker_detail_th);
                        json.Add("worker_detail_en", model.worker_detail_en);

                        json.Add("modified_by", model.created_by);
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
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return output.ToString(Formatting.None);
        }
        public string doSetBatchPosition(InputSetPosition input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMBT001.2";
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
                cls_ctTRPosition objPo = new cls_ctTRPosition();
                List<cls_TRPosition> listPo = new List<cls_TRPosition>();
                bool strID = false;
                foreach(cls_MTWorker modelWorker in input.emp_data)
                {
                    cls_TRPosition model = new cls_TRPosition();
                    model.empposition_position = input.empposition_position;
                    model.empposition_reason = input.empposition_reason;
                    model.empposition_date = Convert.ToDateTime(input.empposition_date);
                    model.company_code = input.company_code;
                    model.worker_code = modelWorker.worker_code;
                    model.flag = input.flag;
                    model.created_by = input.modified_by;

                    listPo.Add(model);
                }
                if (listPo.Count > 0)
                {
                    strID = objPo.insertlist(listPo);
                }
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
                    log.apilog_message = objPo.getMessage();
                }
                objPo.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();
            }


            return output.ToString(Formatting.None);
        }

        //Set Dep (EMBT002)
        public string getBatchDepList(InputSetDep input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMBT002.2";
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
                cls_ctTRDep objPolItem = new cls_ctTRDep();
                List<cls_TRDep> listPolItem = objPolItem.getDataBatch(input.company_code, input.empdep_level01, Convert.ToDateTime(input.empdep_date));

                JArray array = new JArray();

                if (listPolItem.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRDep model in listPolItem)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("empdep_id", model.empdep_id);
                        json.Add("empdep_date", model.empdep_date);
                        json.Add("empdep_level01", model.empdep_level01);
                        json.Add("empdep_level02", model.empdep_level02);
                        json.Add("empdep_level03", model.empdep_level03);
                        json.Add("empdep_level04", model.empdep_level04);
                        json.Add("empdep_level05", model.empdep_level05);
                        json.Add("empdep_level06", model.empdep_level06);
                        json.Add("empdep_level07", model.empdep_level07);
                        json.Add("empdep_level08", model.empdep_level08);
                        json.Add("empdep_level09", model.empdep_level09);
                        json.Add("empdep_level10", model.empdep_level10);

                        json.Add("worker_detail_th", model.worker_detail_th);
                        json.Add("worker_detail_en", model.worker_detail_en);

                        json.Add("modified_by", model.created_by);
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
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return output.ToString(Formatting.None);
        }
        public string doSetBatchDep(InputSetDep input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMBT002.2";
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
                cls_ctTRDep objPo = new cls_ctTRDep();
                List<cls_TRDep> listPo = new List<cls_TRDep>();
                bool strID = false;
                foreach (cls_MTWorker modelWorker in input.emp_data)
                {
                    cls_TRDep model = new cls_TRDep();
                    model.empdep_level01 = input.empdep_level01;
                    model.empdep_level02 = input.empdep_level02;
                    model.empdep_level03 = input.empdep_level03;
                    model.empdep_level04 = input.empdep_level04;
                    model.empdep_level05 = input.empdep_level05;
                    model.empdep_level06 = input.empdep_level06;
                    model.empdep_level07 = input.empdep_level07;
                    model.empdep_level08 = input.empdep_level08;
                    model.empdep_level09 = input.empdep_level09;
                    model.empdep_level10 = input.empdep_level10;
                    model.empdep_reason = input.empdep_reason;
                    model.empdep_date = Convert.ToDateTime(input.empdep_date);
                    model.company_code = input.company_code;
                    model.worker_code = modelWorker.worker_code;
                    model.flag = input.flag;
                    model.created_by = input.modified_by;

                    listPo.Add(model);
                }
                if (listPo.Count > 0)
                {
                    strID = objPo.insertlist(listPo);
                }
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
                    log.apilog_message = objPo.getMessage();
                }
                objPo.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();
            }


            return output.ToString(Formatting.None);
        }


        //Set Group (EMBT008)
        public string getBatchGroupList(InputSetGroup input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMBT008.1";
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
                cls_ctTRGroup objPolItem = new cls_ctTRGroup();
                List<cls_TRGroup> listPolItem = objPolItem.getDataBatch(input.company_code,input.empgroup_code,Convert.ToDateTime(input.empgroup_date));

                JArray array = new JArray();

                if (listPolItem.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRGroup model in listPolItem)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("empgroup_id", model.empgroup_id);
                        json.Add("empgroup_code", model.empgroup_code);
                        json.Add("empgroup_date", model.empgroup_date);

                        json.Add("worker_detail_th", model.worker_detail_th);
                        json.Add("worker_detail_en", model.worker_detail_en);

                        json.Add("modified_by", model.created_by);
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
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return output.ToString(Formatting.None);
        }

        public string doSetBatchGroup(InputSetGroup input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMBT008.2";
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
                cls_ctTRGroup objPo = new cls_ctTRGroup();
                List<cls_TRGroup> listPo = new List<cls_TRGroup>();
                bool strID = false;
                foreach (cls_MTWorker modelWorker in input.emp_data)
                {
                    cls_TRGroup model = new cls_TRGroup();
                    model.empgroup_code = input.empgroup_code;
                    model.empgroup_date = Convert.ToDateTime(input.empgroup_date);
                    model.company_code = input.company_code;
                    model.worker_code = modelWorker.worker_code;
                    model.flag = input.flag;
                    model.created_by = input.modified_by;

                    listPo.Add(model);
                }
                if (listPo.Count > 0)
                {
                    strID = objPo.insertlist(listPo);
                }
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
                    log.apilog_message = objPo.getMessage();
                }
                objPo.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();
            }


            return output.ToString(Formatting.None);
        }

        //Set Location (EMBT009)
        public string getBatchLocationList(InputSetLocation input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMBT009.1";
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
                cls_ctTREmplocation objPolItem = new cls_ctTREmplocation();
                List<cls_TREmplocation> listPolItem = objPolItem.getDataBatch(input.company_code, input.location_code , Convert.ToDateTime(input.emplocation_startdate));

                JArray array = new JArray();

                if (listPolItem.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TREmplocation model in listPolItem)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("location_code", model.location_code);
                        json.Add("emplocation_startdate", model.emplocation_startdate);

                        json.Add("worker_detail_th", model.worker_detail_th);
                        json.Add("worker_detail_en", model.worker_detail_en);

                        json.Add("modified_by", model.created_by);
                        json.Add("modified_date", model.modified_date);

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
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return output.ToString(Formatting.None);
        }
        public string doSetBatchLocation(InputSetLocation input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMBT009.2";
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
                cls_ctTREmplocation objPo = new cls_ctTREmplocation();
                List<cls_TREmplocation> listPo = new List<cls_TREmplocation>();
                bool strID = false;
                foreach (cls_MTWorker modelWorker in input.emp_data)
                {
                    cls_TREmplocation model = new cls_TREmplocation();
                    model.location_code = input.location_code;
                    model.emplocation_startdate = Convert.ToDateTime(input.emplocation_startdate);
                    model.emplocation_enddate = Convert.ToDateTime(input.emplocation_enddate);
                    model.emplocation_note = input.emplocation_note;
                    model.company_code = input.company_code;
                    model.worker_code = modelWorker.worker_code;
                    model.created_by = input.modified_by;

                    listPo.Add(model);
                }
                if (listPo.Count > 0)
                {
                    strID = objPo.insertlist(listPo);
                }
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
                    log.apilog_message = objPo.getMessage();
                }
                objPo.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();
            }


            return output.ToString(Formatting.None);
        }

        //Set Salary (EMBT005)
        public string getBatchSalaryList(InputSetSalary input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMBT005.1";
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
                cls_ctTRSalary objPolItem = new cls_ctTRSalary();
                List<cls_TRSalary> listPolItem = objPolItem.getDataBatch(input.company_code, Convert.ToDateTime(input.empsalary_date), input.empsalary_amount);

                JArray array = new JArray();

                if (listPolItem.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRSalary model in listPolItem)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);

                        json.Add("empsalary_id", model.empsalary_id);
                        json.Add("empsalary_amount", model.empsalary_amount);
                        json.Add("empsalary_date", model.empsalary_date);

                        json.Add("worker_detail_th", model.worker_detail_th);
                        json.Add("worker_detail_en", model.worker_detail_en);

                        json.Add("modified_by", model.created_by);
                        json.Add("modified_date", model.modified_date);

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
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return output.ToString(Formatting.None);
        }
        public string doSetBatchSalary(InputSetSalary input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMBT005.2";
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
                cls_ctTRSalary objPo = new cls_ctTRSalary();
                List<cls_TRSalary> listPo = new List<cls_TRSalary>();
                bool strID = false;
                foreach (cls_MTWorker modelWorker in input.emp_data)
                {
                    cls_TRSalary model = new cls_TRSalary();
                    model.empsalary_amount = input.empsalary_amount;
                    model.empsalary_incamount = input.empsalary_incamount;
                    model.empsalary_incpercent = input.empsalary_incpercent;
                    model.empsalary_date = Convert.ToDateTime(input.empsalary_date);
                    model.empsalary_reason = input.empsalary_reason;
                    model.company_code = input.company_code;
                    model.worker_code = modelWorker.worker_code;
                    model.created_by = input.modified_by;

                    listPo.Add(model);
                }
                if (listPo.Count > 0)
                {
                    strID = objPo.insertlist(listPo);
                }
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
                    log.apilog_message = objPo.getMessage();
                }
                objPo.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();
            }


            return output.ToString(Formatting.None);
        }

        //Set Provident (EMBT007)
        public string getBatchProvidentList(InputSetProvident input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMBT007.1";
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
                cls_ctTRProvident objPolItem = new cls_ctTRProvident();
                List<cls_TRProvident> listPolItem = objPolItem.getDataBatch(input.company_code,input.provident_code,input.empprovident_card);

                JArray array = new JArray();

                if (listPolItem.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRProvident model in listPolItem)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);

                        json.Add("provident_code", model.provident_code);
                        json.Add("empprovident_card", model.empprovident_card);

                        json.Add("worker_detail_th", model.worker_detail_th);
                        json.Add("worker_detail_en", model.worker_detail_en);

                        json.Add("modified_by", model.created_by);
                        json.Add("modified_date", model.modified_date);

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
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return output.ToString(Formatting.None);
        }
        public string doSetBatchProvident(InputSetProvident input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMBT007.2";
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
                cls_ctTRProvident objPo = new cls_ctTRProvident();
                List<cls_TRProvident> listPo = new List<cls_TRProvident>();
                bool strID = false;
                foreach (cls_MTWorker modelWorker in input.emp_data)
                {
                    cls_TRProvident model = new cls_TRProvident();
                    model.provident_code = input.provident_code;
                    model.empprovident_card = input.empprovident_card;
                    model.empprovident_entry = Convert.ToDateTime(input.empprovident_entry);
                    model.empprovident_start = Convert.ToDateTime(input.empprovident_start);
                    model.empprovident_end = Convert.ToDateTime(input.empprovident_end);
                    model.company_code = input.company_code;
                    model.worker_code = modelWorker.worker_code;
                    model.created_by = input.modified_by;

                    listPo.Add(model);
                }
                if (listPo.Count > 0)
                {
                    strID = objPo.insertlist(listPo);
                }
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
                    log.apilog_message = objPo.getMessage();
                }
                objPo.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();
            }


            return output.ToString(Formatting.None);
        }

        //Set Benefits (EMBT006)
        public string getBatchBenefitsList(InputSetBenefits input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMBT006.1";
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
                cls_ctTRBenefit objPolItem = new cls_ctTRBenefit();
                List<cls_TRBenefit> listPolItem = objPolItem.getDataBatch(input.company_code,input.empbenefit_amount ,input.item_code);

                JArray array = new JArray();

                if (listPolItem.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRBenefit model in listPolItem)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);

                        json.Add("empbenefit_id", model.empbenefit_id);
                        json.Add("item_code", model.item_code);
                        json.Add("empbenefit_amount", model.empbenefit_amount);
                        json.Add("empbenefit_startdate", model.empbenefit_startdate);
                        json.Add("empbenefit_enddate", model.empbenefit_enddate);
                        json.Add("empbenefit_reason", model.empbenefit_reason);
                        json.Add("empbenefit_note", model.empbenefit_note);

                        json.Add("empbenefit_paytype", model.empbenefit_paytype);
                        json.Add("empbenefit_break", model.empbenefit_break);
                        json.Add("empbenefit_breakreason", model.empbenefit_breakreason);

                        json.Add("empbenefit_conditionpay", model.empbenefit_conditionpay);
                        json.Add("empbenefit_payfirst", model.empbenefit_payfirst);

                        json.Add("modified_by", model.created_by);
                        json.Add("modified_date", model.modified_date);

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
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return output.ToString(Formatting.None);
        }
        public string doSetBatchBenefits(InputSetBenefits input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMBT006.2";
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
                cls_ctTRBenefit objPo = new cls_ctTRBenefit();
                List<cls_TRBenefit> listPo = new List<cls_TRBenefit>();
                bool strID = false;
                foreach (cls_MTWorker modelWorker in input.emp_data)
                {
                    cls_TRBenefit model = new cls_TRBenefit();
                    model.item_code = input.item_code;
                    model.empbenefit_amount = input.empbenefit_amount;
                    model.empbenefit_startdate = Convert.ToDateTime(input.empbenefit_startdate);
                    model.empbenefit_enddate = Convert.ToDateTime(input.empbenefit_enddate);
                    model.empbenefit_reason = input.empbenefit_reason;
                    model.empbenefit_note = input.empbenefit_note;
                    model.empbenefit_paytype = input.empbenefit_paytype;
                    model.empbenefit_break = input.empbenefit_break;
                    model.empbenefit_breakreason = input.empbenefit_breakreason;
                    model.empbenefit_conditionpay = input.empbenefit_conditionpay;
                    model.empbenefit_payfirst = input.empbenefit_payfirst;
                    model.company_code = input.company_code;
                    model.worker_code = modelWorker.worker_code;
                    model.created_by = input.modified_by;

                    listPo.Add(model);
                }
                if (listPo.Count > 0)
                {
                    strID = objPo.insertlist(listPo);
                }
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
                    log.apilog_message = objPo.getMessage();
                }
                objPo.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();
            }


            return output.ToString(Formatting.None);
        }

        //Set Training (EMBT004)
        public string getBatchTrainingList(InputSetTraining input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMBT004.1";
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
                cls_ctTRTraining objPolItem = new cls_ctTRTraining();
                List<cls_TRTraining> listPolItem = objPolItem.getDataBatch(input.company_code, input.course_code);

                JArray array = new JArray();

                if (listPolItem.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRTraining model in listPolItem)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);

                        json.Add("emptraining_no", model.emptraining_no);
                        json.Add("institute_code", model.institute_code);
                        json.Add("course_code", model.course_code);

                        json.Add("worker_detail_th", model.worker_detail_th);
                        json.Add("worker_detail_en", model.worker_detail_en);

                        json.Add("modified_by", model.created_by);
                        json.Add("modified_date", model.modified_date);

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
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return output.ToString(Formatting.None);
        }
        public string doSetBatchTraining(InputSetTraining input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMBT004.2";
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
                cls_ctTRTraining objPo = new cls_ctTRTraining();
                List<cls_TRTraining> listPo = new List<cls_TRTraining>();
                bool strID = false;
                foreach (cls_MTWorker modelWorker in input.emp_data)
                {
                    cls_TRTraining model = new cls_TRTraining();
                    model.emptraining_start = Convert.ToDateTime(input.emptraining_start);
                    model.emptraining_finish = Convert.ToDateTime(input.emptraining_finish);
                    model.emptraining_status = input.emptraining_status;
                    model.emptraining_hours = input.emptraining_hours;
                    model.emptraining_cost = input.emptraining_cost;
                    model.emptraining_note = input.emptraining_note;
                    model.institute_code = input.institute_code;
                    model.institute_other = input.institute_other;
                    model.course_code = input.course_code;
                    model.course_other = input.course_other;
                    model.company_code = input.company_code;
                    model.worker_code = modelWorker.worker_code;
                    model.created_by = input.modified_by;

                    listPo.Add(model);
                }
                if (listPo.Count > 0)
                {
                    strID = objPo.insertlist(listPo);
                }
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
                    log.apilog_message = objPo.getMessage();
                }
                objPo.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();
            }


            return output.ToString(Formatting.None);
        }

        //Set Assessment (EMBT003)
        public string getBatchTrainingList(InputSetAssessment input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMBT003.1";
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
                cls_ctTRAssessment objPolItem = new cls_ctTRAssessment();
                List<cls_TRAssessment> listPolItem = objPolItem.getDataBatch(input.company_code, input.empassessment_location, Convert.ToDateTime(input.empassessment_fromdate));

                JArray array = new JArray();

                if (listPolItem.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRAssessment model in listPolItem)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);

                        json.Add("empassessment_id", model.empassessment_id);
                        json.Add("empassessment_location", model.empassessment_location);
                        json.Add("empassessment_topic", model.empassessment_topic);
                        json.Add("empassessment_fromdate", model.empassessment_fromdate);

                        json.Add("worker_detail_th", model.worker_detail_th);
                        json.Add("worker_detail_en", model.worker_detail_en);

                        json.Add("modified_by", model.created_by);
                        json.Add("modified_date", model.modified_date);

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
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return output.ToString(Formatting.None);
        }
        public string doSetBatchAssessment(InputSetAssessment input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "EMBT003.2";
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
                cls_ctTRAssessment objPo = new cls_ctTRAssessment();
                List<cls_TRAssessment> listPo = new List<cls_TRAssessment>();
                bool strID = false;
                foreach (cls_MTWorker modelWorker in input.emp_data)
                {
                    cls_TRAssessment model = new cls_TRAssessment();
                    model.empassessment_id = Convert.ToInt32(input.empassessment_id);
                    model.empassessment_location = input.empassessment_location;
                    model.empassessment_topic = input.empassessment_topic;
                    model.empassessment_fromdate = Convert.ToDateTime(input.empassessment_fromdate);
                    model.empassessment_todate = Convert.ToDateTime(input.empassessment_todate);
                    model.empassessment_count = Convert.ToDouble(input.empassessment_count);
                    model.empassessment_result = input.empassessment_result;
                    model.company_code = input.company_code;
                    model.worker_code = modelWorker.worker_code;
                    model.created_by = input.modified_by;

                    listPo.Add(model);
                }
                if (listPo.Count > 0)
                {
                    strID = objPo.insertlist(listPo);
                }
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
                    log.apilog_message = objPo.getMessage();
                }
                objPo.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();
            }


            return output.ToString(Formatting.None);
        }

        #endregion


    }
}
