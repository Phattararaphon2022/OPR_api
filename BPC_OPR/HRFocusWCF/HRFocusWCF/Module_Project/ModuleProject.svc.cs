using AntsCode.Util;
using ClassLibrary_BPC.hrfocus.controller;
using ClassLibrary_BPC.hrfocus.model;
using ClassLibrary_BPC.hrfocus.service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BPC_OPR
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ModuleProject" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ModuleProject.svc or ModuleProject.svc.cs at the Solution Explorer and start debugging.

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]  
    
    public class ModuleProject : IModuleProject
    {
        BpcOpr objBpcOpr = new BpcOpr();
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

        #region MTProbusiness
        public string getMTProbusinessList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO001.1";
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

                cls_ctMTProbusiness controller = new cls_ctMTProbusiness();
                List<cls_MTProbusiness> list = controller.getDataByFillter("");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTProbusiness model in list)
                    {
                        JObject json = new JObject();
                        json.Add("probusiness_id", model.probusiness_id);
                        json.Add("probusiness_code", model.probusiness_code);
                        json.Add("probusiness_name_th", model.probusiness_name_th);
                        json.Add("probusiness_name_en", model.probusiness_name_en);
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
        public string doManageMTProbusiness(InputMTProbusiness input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO001.2";
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

                cls_ctMTProbusiness controller = new cls_ctMTProbusiness();
                cls_MTProbusiness model = new cls_MTProbusiness();

                model.probusiness_id = Convert.ToInt32(input.probusiness_id);
                model.probusiness_code = input.probusiness_code;
                model.probusiness_name_th = input.probusiness_name_th;
                model.probusiness_name_en = input.probusiness_name_en;
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
        public string doDeleteMTProbusiness(InputMTProbusiness input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO001.3";
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

                cls_ctMTProbusiness controller = new cls_ctMTProbusiness();

                if (controller.checkDataOld(input.probusiness_code))
                {
                    bool blnResult = controller.delete(input.probusiness_code);

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
                    string message = "Not Found Project code : " + input.probusiness_code;
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
        public async Task<string> doUploadMTProbusiness(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO001.4";
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
                    cls_srvProjectImport srv_import = new cls_srvProjectImport();
                    string tmp = srv_import.doImportExcel("PROBUSINESS", fileName, by);

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

        #region MTProtype
        public string getMTProtypeList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO002.1";
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

                cls_ctMTProtype controller = new cls_ctMTProtype();
                List<cls_MTProtype> list = controller.getDataByFillter("");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTProtype model in list)
                    {
                        JObject json = new JObject();
                        json.Add("protype_id", model.protype_id);
                        json.Add("protype_code", model.protype_code);
                        json.Add("protype_name_th", model.protype_name_th);
                        json.Add("protype_name_en", model.protype_name_en);
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
        public string doManageMTProtype(InputMTProtype input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO002.2";
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

                cls_ctMTProtype controller = new cls_ctMTProtype();
                cls_MTProtype model = new cls_MTProtype();

                model.protype_id = Convert.ToInt32(input.protype_id);
                model.protype_code = input.protype_code;
                model.protype_name_th = input.protype_name_th;
                model.protype_name_en = input.protype_name_en;
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
        public string doDeleteMTProtype(InputMTProtype input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO002.3";
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

                cls_ctMTProtype controller = new cls_ctMTProtype();

                if (controller.checkDataOld(input.protype_code))
                {
                    bool blnResult = controller.delete(input.protype_code);

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
                    string message = "Not Found Project code : " + input.protype_code;
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
        public async Task<string> doUploadMTProtype(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO002.4";
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
                    cls_srvProjectImport srv_import = new cls_srvProjectImport();
                    string tmp = srv_import.doImportExcel("PROTYPE", fileName, by);

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

        #region MTProuniform
        public string getMTProuniformList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO003.1";
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

                cls_ctMTProuniform controller = new cls_ctMTProuniform();
                List<cls_MTProuniform> list = controller.getDataByFillter("");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTProuniform model in list)
                    {
                        JObject json = new JObject();
                        json.Add("prouniform_id", model.prouniform_id);
                        json.Add("prouniform_code", model.prouniform_code);
                        json.Add("prouniform_name_th", model.prouniform_name_th);
                        json.Add("prouniform_name_en", model.prouniform_name_en);
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
        public string doManageMTProuniform(InputMTProuniform input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO003.2";
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

                cls_ctMTProuniform controller = new cls_ctMTProuniform();
                cls_MTProuniform model = new cls_MTProuniform();

                model.prouniform_id = Convert.ToInt32(input.prouniform_id);
                model.prouniform_code = input.prouniform_code;
                model.prouniform_name_th = input.prouniform_name_th;
                model.prouniform_name_en = input.prouniform_name_en;
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
        public string doDeleteMTProuniform(InputMTProuniform input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO003.3";
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

                cls_ctMTProuniform controller = new cls_ctMTProuniform();

                if (controller.checkDataOld(input.prouniform_code))
                {
                    bool blnResult = controller.delete(input.prouniform_code);

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
                    string message = "Not Found Project code : " + input.prouniform_code;
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
        public async Task<string> doUploadMTProuniform(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO003.4";
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
                    cls_srvProjectImport srv_import = new cls_srvProjectImport();
                    string tmp = srv_import.doImportExcel("PROUNIFORM", fileName, by);

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

        #region MTProslip
        public string getMTProslipList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO004.1";
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

                cls_ctMTProslip controller = new cls_ctMTProslip();
                List<cls_MTProslip> list = controller.getDataByFillter("");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTProslip model in list)
                    {
                        JObject json = new JObject();
                        json.Add("proslip_id", model.proslip_id);
                        json.Add("proslip_code", model.proslip_code);
                        json.Add("proslip_name_th", model.proslip_name_th);
                        json.Add("proslip_name_en", model.proslip_name_en);
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
        public string doManageMTProslip(InputMTProslip input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO004.2";
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

                cls_ctMTProslip controller = new cls_ctMTProslip();
                cls_MTProslip model = new cls_MTProslip();

                model.proslip_id = Convert.ToInt32(input.proslip_id);
                model.proslip_code = input.proslip_code;
                model.proslip_name_th = input.proslip_name_th;
                model.proslip_name_en = input.proslip_name_en;
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
        public string doDeleteMTProslip(InputMTProslip input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO004.3";
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

                cls_ctMTProslip controller = new cls_ctMTProslip();

                if (controller.checkDataOld(input.proslip_code))
                {
                    bool blnResult = controller.delete(input.proslip_code);

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
                    string message = "Not Found Project code : " + input.proslip_code;
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
        public async Task<string> doUploadMTProslip(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO004.4";
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
                    cls_srvProjectImport srv_import = new cls_srvProjectImport();
                    string tmp = srv_import.doImportExcel("PROSLIP", fileName, by);

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

        #region MTProcost
        public string getMTProcostList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO005.1";
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

                cls_ctMTProcost controller = new cls_ctMTProcost();
                List<cls_MTProcost> list = controller.getDataByFillter("");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTProcost model in list)
                    {
                        JObject json = new JObject();
                        json.Add("procost_id", model.procost_id);
                        json.Add("procost_code", model.procost_code);
                        json.Add("procost_name_th", model.procost_name_th);
                        json.Add("procost_name_en", model.procost_name_en);

                        json.Add("procost_type", model.procost_type);
                        json.Add("procost_auto", model.procost_auto);
                        json.Add("procost_itemcode", model.procost_itemcode);
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
        public string doManageMTProcost(InputMTProcost input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO005.2";
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

                cls_ctMTProcost controller = new cls_ctMTProcost();
                cls_MTProcost model = new cls_MTProcost();

                model.procost_id = Convert.ToInt32(input.procost_id);
                model.procost_code = input.procost_code;
                model.procost_name_th = input.procost_name_th;
                model.procost_name_en = input.procost_name_en;

                model.procost_type = input.procost_type;
                model.procost_auto = input.procost_auto;
                model.procost_itemcode = input.procost_itemcode;
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
        public string doDeleteMTProcost(InputMTProcost input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO005.3";
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

                cls_ctMTProcost controller = new cls_ctMTProcost();

                if (controller.checkDataOld(input.procost_code))
                {
                    bool blnResult = controller.delete(input.procost_code);

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
                    string message = "Not Found Project code : " + input.procost_code;
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
        public async Task<string> doUploadMTProcost(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO005.4";
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
                    cls_srvProjectImport srv_import = new cls_srvProjectImport();
                    string tmp = srv_import.doImportExcel("PROCOST", fileName, by);

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

        #region MTProject
        public string getMTProjectList(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO006.1";
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

                cls_ctMTProject controller = new cls_ctMTProject();
                List<cls_MTProject> list = controller.getDataByFillter(req.project_code, "", "", "");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTProject model in list)
                    {
                        JObject json = new JObject();
                        json.Add("project_id", model.project_id);
                        json.Add("project_code", model.project_code);
                        json.Add("project_name_th", model.project_name_th);
                        json.Add("project_name_en", model.project_name_en);

                        json.Add("project_name_sub", model.project_name_sub);
                        json.Add("project_codecentral", model.project_codecentral);
                        json.Add("project_protype", model.project_protype);
                        json.Add("project_probusiness", model.project_probusiness);
                        json.Add("project_roundtime", model.project_roundtime);
                        json.Add("project_roundmoney", model.project_roundmoney);
                        json.Add("project_status", model.project_status);
                        json.Add("company_code", model.company_code);


                        json.Add("project_emp", 0);
                        json.Add("project_cost", 0);
                        json.Add("project_start", new DateTime());
                        json.Add("project_end", new DateTime());
            

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
        public string doManageMTProject(InputMTProject input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO006.2";
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

                cls_ctMTProject controller = new cls_ctMTProject();
                cls_MTProject model = new cls_MTProject();

                model.project_id = Convert.ToInt32(input.project_id);
                model.project_code = input.project_code;
                model.project_name_th = input.project_name_th;
                model.project_name_en = input.project_name_en;

                model.project_name_sub = input.project_name_sub;
                model.project_codecentral = input.project_codecentral;
                model.project_protype = input.project_protype;
                model.project_probusiness = input.project_probusiness;
                model.project_roundtime = input.project_roundtime;
                model.project_roundmoney = input.project_roundmoney;
                model.project_status = input.project_status;
                model.company_code = input.company_code;               

                model.modified_by = input.modified_by;

                bool blnResult = controller.insert(model);

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
        public string doDeleteMTProject(InputMTProject input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO006.3";
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

                cls_ctMTProtype controller = new cls_ctMTProtype();

                if (controller.checkDataOld(input.project_code))
                {
                    bool blnResult = controller.delete(input.project_code);

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
                    string message = "Not Found Project code : " + input.project_code;
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
        public async Task<string> doUploadMTProject(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO006.4";
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
                    cls_srvProjectImport srv_import = new cls_srvProjectImport();
                    string tmp = srv_import.doImportExcel("PROJECT", fileName, by);

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

        #region TRProaddress
        public string getTRProaddressList(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO007.1";
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

                cls_ctTRProaddress controller = new cls_ctTRProaddress();
                List<cls_TRProaddress> list = controller.getDataByFillter(req.project_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRProaddress model in list)
                    {
                        JObject json = new JObject();
                        json.Add("proaddress_id", model.proaddress_id);
                        json.Add("proaddress_type", model.proaddress_type);
                        json.Add("proaddress_no", model.proaddress_no);
                        json.Add("proaddress_moo", model.proaddress_moo);
                        json.Add("proaddress_soi", model.proaddress_soi);
                        json.Add("proaddress_road", model.proaddress_road);
                        json.Add("proaddress_tambon", model.proaddress_tambon);
                        json.Add("proaddress_amphur", model.proaddress_amphur);
                        json.Add("proaddress_zipcode", model.proaddress_zipcode);
                        json.Add("proaddress_tel", model.proaddress_tel);
                        json.Add("proaddress_email", model.proaddress_email);
                        json.Add("proaddress_line", model.proaddress_line);
                        json.Add("proaddress_facebook", model.proaddress_facebook);
                        json.Add("province_code", model.province_code);
                        json.Add("project_code", model.project_code);                        

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
        public string doManageTRProaddress(InputTRProaddress input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO007.2";
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

                cls_ctTRProaddress controller = new cls_ctTRProaddress();
                cls_TRProaddress model = new cls_TRProaddress();

                model.proaddress_id = Convert.ToInt32(input.proaddress_id);
                model.proaddress_type = input.proaddress_type;
                model.proaddress_no = input.proaddress_no;
                model.proaddress_moo = input.proaddress_moo;
                model.proaddress_soi = input.proaddress_soi;
                model.proaddress_road = input.proaddress_road;
                model.proaddress_tambon = input.proaddress_tambon;
                model.proaddress_amphur = input.proaddress_amphur;
                model.proaddress_zipcode = input.proaddress_zipcode;
                model.proaddress_tel = input.proaddress_tel;
                model.proaddress_email = input.proaddress_email;
                model.proaddress_line = input.proaddress_line;
                model.proaddress_facebook = input.proaddress_facebook;
                model.province_code = input.province_code;
                model.project_code = input.project_code;
                
                model.modified_by = input.modified_by;

                bool blnResult = controller.insert(model);

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
        public string doDeleteTRProaddress(InputTRProaddress input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO007.3";
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

                cls_ctTRProaddress controller = new cls_ctTRProaddress();

                if (controller.checkDataOld(input.project_code, input.proaddress_type))
                {
                    bool blnResult = controller.delete(input.project_code, input.proaddress_type);

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
                    string message = "Not Found Project code : " + input.project_code;
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
        public async Task<string> doUploadTRProaddress(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO007.4";
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
                    cls_srvProjectImport srv_import = new cls_srvProjectImport();
                    string tmp = srv_import.doImportExcel("PROJECT_ADDRESS", fileName, by);

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

        #region TRProcontact
        public string getTRProcontactList(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO008.1";
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

                cls_ctTRProcontact controller = new cls_ctTRProcontact();
                List<cls_TRProcontact> list = controller.getDataByFillter(req.project_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRProcontact model in list)
                    {
                        JObject json = new JObject();
                        json.Add("procontact_id", model.procontact_id);
                        json.Add("procontact_ref", model.procontact_ref);
                        json.Add("procontact_firstname_th", model.procontact_firstname_th);
                        json.Add("procontact_lastname_th", model.procontact_lastname_th);
                        json.Add("procontact_firstname_en", model.procontact_firstname_en);
                        json.Add("procontact_lastname_en", model.procontact_lastname_en);
                        json.Add("procontact_tel", model.procontact_tel);
                        json.Add("procontact_email", model.procontact_email);
                        json.Add("position_code", model.position_code);
                        json.Add("initial_code", model.initial_code);
                        json.Add("project_code", model.project_code);
                      
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
        public string doManageTRProcontact(InputTRProcontact input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO008.2";
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

                cls_ctTRProcontact controller = new cls_ctTRProcontact();
                cls_TRProcontact model = new cls_TRProcontact();

                model.procontact_id = Convert.ToInt32(input.procontact_id);
                model.procontact_ref = input.procontact_ref;
                model.procontact_firstname_th = input.procontact_firstname_th;
                model.procontact_lastname_th = input.procontact_lastname_th;
                model.procontact_firstname_en = input.procontact_firstname_en;
                model.procontact_lastname_en = input.procontact_lastname_en;
                model.procontact_tel = input.procontact_tel;
                model.procontact_email = input.procontact_email;
                model.position_code = input.position_code;
                model.initial_code = input.initial_code;
                model.project_code = input.project_code;
                
                model.modified_by = input.modified_by;

                bool blnResult = controller.insert(model);

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
        public string doManageTRProcontactList(InputProjectTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO008.5";
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

                cls_ctTRProcontact controller = new cls_ctTRProcontact();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRProcontact>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.project_code);

                if (clear)
                {
                    foreach (cls_TRProcontact model in jsonArray)
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
        public string doDeleteTRProcontact(InputTRProcontact input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO008.3";
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

                cls_ctTRProcontact controller = new cls_ctTRProcontact();

                if (controller.checkDataOld(input.project_code, input.procontact_ref))
                {
                    bool blnResult = controller.delete(input.project_code, input.procontact_ref);

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
                    string message = "Not Found Project code : " + input.project_code;
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
        public async Task<string> doUploadTRProcontact(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO008.4";
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
                    cls_srvProjectImport srv_import = new cls_srvProjectImport();
                    string tmp = srv_import.doImportExcel("PROJECT_CONTACT", fileName, by);

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

        #region TRProcontract
        public string getTRProcontractList(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO009.1";
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

                cls_ctTRProcontract controller = new cls_ctTRProcontract();
                List<cls_TRProcontract> list = controller.getDataByFillter(req.project_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRProcontract model in list)
                    {
                        JObject json = new JObject();
                        json.Add("procontract_id", model.procontract_id);
                        json.Add("procontract_ref", model.procontract_ref);
                        json.Add("procontract_date", model.procontract_date);
                        json.Add("procontract_amount", model.procontract_amount);
                        json.Add("procontract_fromdate", model.procontract_fromdate);
                        json.Add("procontract_todate", model.procontract_todate);
                        json.Add("procontract_customer", model.procontract_customer);
                        json.Add("procontract_bidder", model.procontract_bidder);
                        
                        json.Add("project_code", model.project_code);

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
        public string doManageTRProcontractList(InputProjectTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO009.5";
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

                cls_ctTRProcontract controller = new cls_ctTRProcontract();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRProcontract>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.project_code);

                if (clear)
                {
                    foreach (cls_TRProcontract model in jsonArray)
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
        public string doDeleteTRProcontract(InputTRProcontract input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO009.3";
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

                cls_ctTRProcontract controller = new cls_ctTRProcontract();

                if (controller.checkDataOld(input.project_code, input.procontract_ref))
                {
                    bool blnResult = controller.delete(input.project_code, input.procontract_ref);

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
                    string message = "Not Found Project code : " + input.project_code;
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
        public async Task<string> doUploadTRProcontract(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO009.4";
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
                    cls_srvProjectImport srv_import = new cls_srvProjectImport();
                    string tmp = srv_import.doImportExcel("PROJECT_CONTRACT", fileName, by);

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

        #region TRProresponsible
        public string getTRProresponsibleList(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO010.1";
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

                cls_ctTRProresponsible controller = new cls_ctTRProresponsible();
                List<cls_TRProresponsible> list = controller.getDataByFillter(req.project_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRProresponsible model in list)
                    {
                        JObject json = new JObject();
                        json.Add("proresponsible_id", model.proresponsible_id);
                        json.Add("proresponsible_ref", model.proresponsible_ref);
                        json.Add("proresponsible_emp", model.proresponsible_emp);
                        json.Add("proresponsible_position", model.proresponsible_position);
                        json.Add("proresponsible_area", model.proresponsible_area);
                        json.Add("proresponsible_fromdate", model.proresponsible_fromdate);
                        json.Add("proresponsible_todate", model.proresponsible_todate);

                        json.Add("project_code", model.project_code);

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
        public string doManageTRProresponsibleList(InputProjectTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO010.5";
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

                cls_ctTRProresponsible controller = new cls_ctTRProresponsible();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRProresponsible>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.project_code);

                if (clear)
                {
                    foreach (cls_TRProresponsible model in jsonArray)
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
        public string doDeleteTRProresponsible(InputTRProresponsible input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO010.3";
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

                cls_ctTRProresponsible controller = new cls_ctTRProresponsible();

                if (controller.checkDataOld(input.project_code, input.proresponsible_ref))
                {
                    bool blnResult = controller.delete(input.project_code, input.proresponsible_ref);

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
                    string message = "Not Found Project code : " + input.project_code;
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
        public async Task<string> doUploadTRProresponsible(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO010.4";
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
                    cls_srvProjectImport srv_import = new cls_srvProjectImport();
                    string tmp = srv_import.doImportExcel("PROJECT_RESPONSIBLE", fileName, by);

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

        #region MTProtimepol
        public string getMTProtimepolList(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO011.1";
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

                cls_ctMTProtimepol controller = new cls_ctMTProtimepol();
                List<cls_MTProtimepol> list = controller.getDataByFillter(req.project_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTProtimepol model in list)
                    {
                        JObject json = new JObject();
                        json.Add("protimepol_id", model.protimepol_id);
                        json.Add("protimepol_code", model.protimepol_code);
                        json.Add("protimepol_name_th", model.protimepol_name_th);
                        json.Add("protimepol_name_en", model.protimepol_name_en);
                        json.Add("protimepol_ot", model.protimepol_ot);
                        json.Add("protimepol_allw", model.protimepol_allw);
                        json.Add("protimepol_dg", model.protimepol_dg);
                        json.Add("protimepol_lv", model.protimepol_lv);
                        json.Add("protimepol_lt", model.protimepol_lt);

                        json.Add("project_code", model.project_code);

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
        public string doManageMTProtimepolList(InputProjectTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO011.5";
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

                cls_ctMTProtimepol controller = new cls_ctMTProtimepol();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_MTProtimepol>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.project_code);

                if (clear)
                {
                    foreach (cls_MTProtimepol model in jsonArray)
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
        public string doDeleteMTProtimepol(InputMTProtimepol input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO011.3";
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

                cls_ctMTProtimepol controller = new cls_ctMTProtimepol();

                if (controller.checkDataOld(input.project_code, input.protimepol_code))
                {
                    bool blnResult = controller.delete(input.project_code, input.protimepol_code);

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
                    string message = "Not Found Project code : " + input.project_code;
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
        public async Task<string> doUploadMTProtimepol(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO011.4";
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
                    cls_srvProjectImport srv_import = new cls_srvProjectImport();
                    string tmp = srv_import.doImportExcel("PROJECT_TIMEPOL", fileName, by);

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

        #region MTProjobmain
        public string getMTProjobmainList(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO012.1";
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

                //-- Cost master
                //cls_ctMTProcost polcost_controller = new cls_ctMTProcost();
                //List<cls_MTProcost> polcost_list = polcost_controller.getDataByFillter("");
                
                //-- Job contract
                cls_ctTRProjobcontract contract_controller = new cls_ctTRProjobcontract();
                List<cls_TRProjobcontract> contract_list = contract_controller.getDataByFillter(req.project_code, "");

                //-- Job cost
                cls_ctTRProjobcost cost_controller = new cls_ctTRProjobcost();
                

                cls_ctMTProjobmain controller = new cls_ctMTProjobmain();
                List<cls_MTProjobmain> list = controller.getDataByFillter(req.project_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTProjobmain model in list)
                    {
                        JObject json = new JObject();
                        json.Add("projobmain_id", model.projobmain_id);
                        json.Add("projobmain_code", model.projobmain_code);
                        json.Add("projobmain_name_th", model.projobmain_name_th);
                        json.Add("projobmain_name_en", model.projobmain_name_en);
                        json.Add("projobmain_type", model.projobmain_type);
                        json.Add("projobmain_shift", model.projobmain_shift);

                        json.Add("projobmain_sun", model.projobmain_sun);
                        json.Add("projobmain_mon", model.projobmain_mon);
                        json.Add("projobmain_tue", model.projobmain_tue);
                        json.Add("projobmain_wed", model.projobmain_wed);
                        json.Add("projobmain_thu", model.projobmain_thu);
                        json.Add("projobmain_fri", model.projobmain_fri);
                        json.Add("projobmain_sat", model.projobmain_sat);
                        
                        json.Add("projobmain_working", model.projobmain_working);
                        json.Add("projobmain_hrsperday", model.projobmain_hrsperday);
                        json.Add("projobmain_hrsot", model.projobmain_hrsot);
                        json.Add("projobmain_autoot", model.projobmain_autoot);


                        json.Add("projobmain_timepol", model.projobmain_timepol);
                        json.Add("projobmain_slip", model.projobmain_slip);
                        json.Add("projobmain_uniform", model.projobmain_uniform);

                        json.Add("project_code", model.project_code);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);

                        //-- Contract
                        cls_TRProjobcontract contract = null;
                        foreach (cls_TRProjobcontract tmp in contract_list)
                        {
                            if (tmp.projob_code.Equals(model.projobmain_code))
                            {
                                contract = tmp;
                                break;
                            }                                
                        }

                        //-- Allow
                        List<cls_TRProjobcost> cost_list_max = cost_controller.getDataMaxDate(req.company, req.project_code, model.projobmain_code);

                       
                        int i = 1;
                        foreach (cls_TRProjobcost cost in cost_list_max)
                        {
                            switch (i)
                            {
                                case 1: model.allow1 += cost.projobcost_amount; break;
                                case 2: model.allow2 += cost.projobcost_amount; break;
                                case 3: model.allow3 += cost.projobcost_amount; break;
                                case 4: model.allow4 += cost.projobcost_amount; break;
                                case 5: model.allow5 += cost.projobcost_amount; break;
                                case 6: model.allow6 += cost.projobcost_amount; break;
                                case 7: model.allow7 += cost.projobcost_amount; break;
                                case 8: model.allow8 += cost.projobcost_amount; break;
                                case 9: model.allow9 += cost.projobcost_amount; break;
                                case 10: model.allow10 += cost.projobcost_amount; break;
                            }

                            i++;
                        }
                        

                        if (contract != null)
                        {
                            model.emp_total = contract.projobcontract_emp;                            
                        }
                        else
                        {
                            model.emp_total = 0;
                        }

                        model.allow_emp = model.allow1 + model.allow2 + model.allow3 + model.allow4 + model.allow5 + model.allow6 + model.allow7 + model.allow8 + model.allow9 + model.allow10;
                        model.allow_total = model.allow_emp * model.emp_total;

                        json.Add("allow1", model.allow1);
                        json.Add("allow2", model.allow2);
                        json.Add("allow3", model.allow3);
                        json.Add("allow4", model.allow4);
                        json.Add("allow5", model.allow5);
                        json.Add("allow6", model.allow6);
                        json.Add("allow7", model.allow7);
                        json.Add("allow8", model.allow8);
                        json.Add("allow9", model.allow9);
                        json.Add("allow10", model.allow10);

                        json.Add("emp_total", model.emp_total);
                        json.Add("allow_emp", model.allow_emp);
                        json.Add("allow_total", model.allow_total);



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
        public string doManageMTProjobmainList(InputProjectTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO012.5";
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

                cls_ctMTProjobmain controller = new cls_ctMTProjobmain();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_MTProjobmain>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.project_code);

                if (clear)
                {
                    foreach (cls_MTProjobmain model in jsonArray)
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
        public string doDeleteMTProjobmain(InputMTProjobmain input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO012.3";
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

                cls_ctMTProjobmain controller = new cls_ctMTProjobmain();

                if (controller.checkDataOld(input.project_code, input.projobmain_code))
                {
                    bool blnResult = controller.delete(input.project_code, input.projobmain_code);

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
                    string message = "Not Found Project code : " + input.project_code;
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
        public async Task<string> doUploadMTProjobmain(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO012.4";
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
                    cls_srvProjectImport srv_import = new cls_srvProjectImport();
                    string tmp = srv_import.doImportExcel("PROJECT_JOBMAIN", fileName, by);

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
        
        #region TRProjobcontract
        public string getTRProjobcontractList(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO013.1";
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

                cls_ctTRProjobcontract controller = new cls_ctTRProjobcontract();
                List<cls_TRProjobcontract> list = controller.getDataByFillter(req.project_code, req.job_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRProjobcontract model in list)
                    {
                        JObject json = new JObject();
                        json.Add("projobcontract_id", model.projobcontract_id);
                        json.Add("projobcontract_ref", model.projobcontract_ref);
                        json.Add("projobcontract_date", model.projobcontract_date);
                        json.Add("projobcontract_emp", model.projobcontract_emp);
                        json.Add("projobcontract_amount", model.projobcontract_amount);
                        json.Add("projobcontract_fromdate", model.projobcontract_fromdate);
                        json.Add("projobcontract_todate", model.projobcontract_todate);
                        json.Add("projob_code", model.projob_code);
                        json.Add("project_code", model.project_code);
                       
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
        public string doManageTRProjobcontract(InputTRProjobcontract input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO013.2";
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

                cls_ctTRProjobcontract controller = new cls_ctTRProjobcontract();
                cls_TRProjobcontract model = new cls_TRProjobcontract();

                model.projobcontract_id = Convert.ToInt32(input.projobcontract_id);
                model.projobcontract_ref = input.projobcontract_ref;
                model.projobcontract_date = Convert.ToDateTime(input.projobcontract_date);
                model.projobcontract_emp = input.projobcontract_emp;
                model.projobcontract_amount = input.projobcontract_amount;
                model.projobcontract_fromdate = Convert.ToDateTime(input.projobcontract_fromdate);
                model.projobcontract_todate = Convert.ToDateTime(input.projobcontract_todate);            
                model.projob_code = input.projob_code;
                model.project_code = input.project_code;

                model.modified_by = input.modified_by;

                bool blnResult = controller.insert(model);

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
        public string doManageTRProjobcontractList(InputProjectTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO013.5";
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

                cls_ctTRProjobcontract controller = new cls_ctTRProjobcontract();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRProjobcontract>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.project_code, input.job_code);

                if (clear)
                {
                    foreach (cls_TRProjobcontract model in jsonArray)
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
        public string doDeleteTRProjobcontract(InputTRProjobcontract input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO013.3";
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

                cls_ctTRProjobcontract controller = new cls_ctTRProjobcontract();

                if (controller.checkDataOld(input.project_code, input.projob_code, input.projobcontract_ref))
                {
                    bool blnResult = controller.delete(input.project_code, input.projob_code, input.projobcontract_ref);

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
                    string message = "Not Found Project code : " + input.project_code;
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
        public async Task<string> doUploadTRProjobcontract(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO013.4";
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
                    cls_srvProjectImport srv_import = new cls_srvProjectImport();
                    string tmp = srv_import.doImportExcel("PROJECT_JOBCONTRACT", fileName, by);

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

        #region TRProjobcost
        public string getTRProjobcostList(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO014.1";
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

                cls_ctTRProjobcost controller = new cls_ctTRProjobcost();
                List<cls_TRProjobcost> list = controller.getDataByFillter(req.project_code, req.job_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRProjobcost model in list)
                    {
                        JObject json = new JObject();
                        json.Add("projobcost_id", model.projobcost_id);
                        json.Add("projobcost_code", model.projobcost_code);
                        json.Add("projobcost_amount", model.projobcost_amount);
                        json.Add("projobcost_fromdate", model.projobcost_fromdate);
                        json.Add("projobcost_todate", model.projobcost_todate);
                        json.Add("projobcost_version", model.projobcost_version);
                        json.Add("projobcost_status", model.projobcost_status);
                        json.Add("projob_code", model.projob_code);
                        json.Add("project_code", model.project_code);

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
        public string doManageTRProjobcost(InputTRProjobcost input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO014.2";
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

                cls_ctTRProjobcost controller = new cls_ctTRProjobcost();
                cls_TRProjobcost model = new cls_TRProjobcost();

                model.projobcost_id = Convert.ToInt32(input.projobcost_id);
                model.projobcost_code = input.projobcost_code;
                model.projobcost_amount = input.projobcost_amount;     
                model.projobcost_fromdate = Convert.ToDateTime(input.projobcost_fromdate);
                model.projobcost_todate = Convert.ToDateTime(input.projobcost_todate);
                model.projobcost_version = input.projobcost_version;
                model.projobcost_status = input.projobcost_status;
                
                model.projob_code = input.projob_code;
                model.project_code = input.project_code;

                model.modified_by = input.modified_by;

                bool blnResult = controller.insert(model);

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
        public string doManageTRProjobcostList(InputProjectTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO014.5";
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

                cls_ctTRProjobcost controller = new cls_ctTRProjobcost();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRProjobcost>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.project_code, input.job_code);

                if (clear)
                {
                    foreach (cls_TRProjobcost model in jsonArray)
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
        public string doDeleteTRProjobcost(InputTRProjobcost input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO014.3";
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

                cls_ctTRProjobcost controller = new cls_ctTRProjobcost();

                if (controller.checkDataOld(input.project_code, input.projob_code, input.projob_code, Convert.ToDateTime(input.projobcost_fromdate)))
                {
                    bool blnResult = controller.delete(input.project_code, input.projob_code, input.projob_code, Convert.ToDateTime(input.projobcost_fromdate));

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
                    string message = "Not Found Project code : " + input.project_code;
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
        public async Task<string> doUploadTRProjobcost(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO014.4";
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
                    cls_srvProjectImport srv_import = new cls_srvProjectImport();
                    string tmp = srv_import.doImportExcel("PROJECT_JOBCOST", fileName, by);

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

        #region TRProjobmachine
        public string getTRProjobmachineList(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO015.1";
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

                cls_ctTRProjobmachine controller = new cls_ctTRProjobmachine();
                List<cls_TRProjobmachine> list = controller.getDataByFillter(req.project_code, req.job_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRProjobmachine model in list)
                    {
                        JObject json = new JObject();
                        json.Add("projobmachine_id", model.projobmachine_id);
                        json.Add("projobmachine_ip", model.projobmachine_ip);
                        json.Add("projobmachine_port", model.projobmachine_port);
                        json.Add("projobmachine_enable", model.projobmachine_enable);
                        
                        json.Add("projob_code", model.projob_code);
                        json.Add("project_code", model.project_code);

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
        public string doManageTRProjobmachine(InputTRProjobmachine input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO015.2";
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

                cls_ctTRProjobmachine controller = new cls_ctTRProjobmachine();
                cls_TRProjobmachine model = new cls_TRProjobmachine();

                model.projobmachine_id = Convert.ToInt32(input.projobmachine_id);
                model.projobmachine_ip = input.projobmachine_ip;
                model.projobmachine_port = input.projobmachine_port;
                model.projobmachine_enable = input.projobmachine_enable;
               
                model.projob_code = input.projob_code;
                model.project_code = input.project_code;

                model.modified_by = input.modified_by;

                bool blnResult = controller.insert(model);

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
        public string doManageTRProjobmachineList(InputProjectTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO015.5";
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

                cls_ctTRProjobmachine controller = new cls_ctTRProjobmachine();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRProjobmachine>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.project_code, input.job_code);

                if (clear)
                {
                    foreach (cls_TRProjobmachine model in jsonArray)
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
        public string doDeleteTRProjobmachine(InputTRProjobmachine input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO015.3";
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

                cls_ctTRProjobmachine controller = new cls_ctTRProjobmachine();

                if (controller.checkDataOld(input.project_code, input.projob_code, input.projobmachine_ip))
                {
                    bool blnResult = controller.delete(input.project_code, input.projob_code, input.projobmachine_ip);

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
                    string message = "Not Found Project code : " + input.project_code;
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
        public async Task<string> doUploadTRProjobmachine(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO015.4";
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
                    cls_srvProjectImport srv_import = new cls_srvProjectImport();
                    string tmp = srv_import.doImportExcel("PROJECT_JOBMACHINE", fileName, by);

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

        #region MTProjobsub
        public string getMTProjobsubList(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO016.1";
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

                //-- Cost master
                //cls_ctMTProcost polcost_controller = new cls_ctMTProcost();
                //List<cls_MTProcost> polcost_list = polcost_controller.getDataByFillter("");

                //-- Job contract
                cls_ctTRProjobcontract contract_controller = new cls_ctTRProjobcontract();
                List<cls_TRProjobcontract> contract_list = contract_controller.getDataByFillter(req.project_code, "");

                cls_ctMTProjobsub controller = new cls_ctMTProjobsub();
                List<cls_MTProjobsub> list = controller.getDataByFillter(req.project_code);

                cls_ctTRProjobcost cost_controller = new cls_ctTRProjobcost();

                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTProjobsub model in list)
                    {
                        JObject json = new JObject();
                        json.Add("projobsub_id", model.projobsub_id);
                        json.Add("projobsub_code", model.projobsub_code);
                        json.Add("projobsub_name_th", model.projobsub_name_th);
                        json.Add("projobsub_name_en", model.projobsub_name_en);
                      
                        json.Add("project_code", model.project_code);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);

                        //-- Contract
                        cls_TRProjobcontract contract = null;
                        foreach (cls_TRProjobcontract tmp in contract_list)
                        {
                            if (tmp.projob_code.Equals(model.projobsub_code))
                            {
                                contract = tmp;
                                break;
                            }
                        }

                        //-- Allow
                        List<cls_TRProjobcost> cost_list_max = cost_controller.getDataMaxDate(req.company, req.project_code, model.projobsub_code);


                        int i = 1;
                        foreach (cls_TRProjobcost cost in cost_list_max)
                        {
                            switch (i)
                            {
                                case 1: model.allow1 += cost.projobcost_amount; break;
                                case 2: model.allow2 += cost.projobcost_amount; break;
                                case 3: model.allow3 += cost.projobcost_amount; break;
                                case 4: model.allow4 += cost.projobcost_amount; break;
                                case 5: model.allow5 += cost.projobcost_amount; break;
                                case 6: model.allow6 += cost.projobcost_amount; break;
                                case 7: model.allow7 += cost.projobcost_amount; break;
                                case 8: model.allow8 += cost.projobcost_amount; break;
                                case 9: model.allow9 += cost.projobcost_amount; break;
                                case 10: model.allow10 += cost.projobcost_amount; break;
                            }

                            i++;
                        }


                        if (contract != null)
                        {
                            model.emp_total = contract.projobcontract_emp;
                        }
                        else
                        {
                            model.emp_total = 0;
                        }

                        model.allow_emp = model.allow1 + model.allow2 + model.allow3 + model.allow4 + model.allow5 + model.allow6 + model.allow7 + model.allow8 + model.allow9 + model.allow10;
                        model.allow_total = model.allow_emp * model.emp_total;

                        json.Add("allow1", model.allow1);
                        json.Add("allow2", model.allow2);
                        json.Add("allow3", model.allow3);
                        json.Add("allow4", model.allow4);
                        json.Add("allow5", model.allow5);
                        json.Add("allow6", model.allow6);
                        json.Add("allow7", model.allow7);
                        json.Add("allow8", model.allow8);
                        json.Add("allow9", model.allow9);
                        json.Add("allow10", model.allow10);

                        json.Add("emp_total", model.emp_total);
                        json.Add("allow_emp", model.allow_emp);
                        json.Add("allow_total", model.allow_total);



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
        public string doManageMTProjobsubList(InputProjectTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO016.5";
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

                cls_ctMTProjobsub controller = new cls_ctMTProjobsub();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_MTProjobsub>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.project_code);

                if (clear)
                {
                    foreach (cls_MTProjobsub model in jsonArray)
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
        public string doDeleteMTProjobsub(InputMTProjobsub input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO016.3";
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

                cls_ctMTProjobsub controller = new cls_ctMTProjobsub();

                if (controller.checkDataOld(input.project_code, input.projobsub_code))
                {
                    bool blnResult = controller.delete(input.project_code, input.projobsub_code);

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
                    string message = "Not Found Project code : " + input.project_code;
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
        public async Task<string> doUploadMTProjobsub(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO016.4";
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
                    cls_srvProjectImport srv_import = new cls_srvProjectImport();
                    string tmp = srv_import.doImportExcel("PROJECT_JOBSUB", fileName, by);

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

        #region TRProjobemp
        public string getTRProjobempList(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO017.1";
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

                cls_ctTRProjobemp controller = new cls_ctTRProjobemp();
                List<cls_TRProjobemp> list = controller.getDataByFillter(req.project_code, "");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRProjobemp model in list)
                    {
                        JObject json = new JObject();
                        json.Add("projobemp_id", model.projobemp_id);
                        json.Add("projobemp_emp", model.projobemp_emp);
                        json.Add("projobemp_fromdate", model.projobemp_fromdate);
                        json.Add("projobemp_todate", model.projobemp_todate);
                        json.Add("projobemp_status", model.projobemp_status);
                     
                        json.Add("projob_code", model.projob_code);
                        json.Add("project_code", model.project_code);

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
        public string doManageTRProjobemp(InputTRProjobemp input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO017.2";
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

                cls_ctTRProjobemp controller = new cls_ctTRProjobemp();
                cls_TRProjobemp model = new cls_TRProjobemp();

                model.projobemp_id = Convert.ToInt32(input.projobemp_id);
                model.projobemp_emp = input.projobemp_emp;

                model.projobemp_fromdate = Convert.ToDateTime(input.projobemp_fromdate);
                model.projobemp_todate = Convert.ToDateTime(input.projobemp_todate);
                model.projobemp_status = input.projobemp_status;
                
                model.projob_code = input.projob_code;
                model.project_code = input.project_code;

                model.modified_by = input.modified_by;

                bool blnResult = controller.insert(model);

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
        public string doManageTRProjobempList(InputProjectTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO017.5";
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

                cls_ctTRProjobemp controller = new cls_ctTRProjobemp();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRProjobemp>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.project_code, input.job_code);

                if (clear)
                {
                    foreach (cls_TRProjobemp model in jsonArray)
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
        public string doDeleteTRProjobemp(InputTRProjobemp input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO017.3";
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

                cls_ctTRProjobemp controller = new cls_ctTRProjobemp();

                if (controller.checkDataOld(input.project_code, input.projob_code, input.projob_code, Convert.ToDateTime(input.projobemp_fromdate)))
                {
                    bool blnResult = controller.delete(input.project_code, input.projob_code, input.projob_code, Convert.ToDateTime(input.projobemp_fromdate));

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
                    string message = "Not Found Project code : " + input.project_code;
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
        public async Task<string> doUploadTRProjobemp(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO017.4";
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
                    cls_srvProjectImport srv_import = new cls_srvProjectImport();
                    string tmp = srv_import.doImportExcel("PROJECT_JOBEMP", fileName, by);

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

        #region TRProjobworking
        public string getTRProjobworkingList(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO018.1";
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

                cls_ctTRProjobworking controller = new cls_ctTRProjobworking();
                List<cls_TRProjobworking> list = controller.getDataByFillter(req.project_code, req.job_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRProjobworking model in list)
                    {
                        JObject json = new JObject();
                        json.Add("projobworking_id", model.projobworking_id);
                        json.Add("projobworking_emp", model.projobworking_emp);
                        json.Add("projobworking_workdate", model.projobworking_workdate);
                        json.Add("projobworking_in", model.projobworking_in);
                        json.Add("projobworking_out", model.projobworking_out);
                        json.Add("projobworking_status", model.projobworking_status);

                        json.Add("projob_code", model.projob_code);
                        json.Add("project_code", model.project_code);

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
        public string doManageTRProjobworking(InputTRProjobworking input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO018.2";
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

                cls_ctTRProjobworking controller = new cls_ctTRProjobworking();
                cls_TRProjobworking model = new cls_TRProjobworking();

                model.projobworking_id = Convert.ToInt32(input.projobworking_id);
                model.projobworking_emp = input.projobworking_emp;

                model.projobworking_workdate = Convert.ToDateTime(input.projobworking_workdate);
                model.projobworking_in = Convert.ToDateTime(input.projobworking_in);
                model.projobworking_out = Convert.ToDateTime(input.projobworking_out);
                model.projobworking_status = input.projobworking_status;

                model.projob_code = input.projob_code;
                model.project_code = input.project_code;

                model.modified_by = input.modified_by;

                bool blnResult = controller.insert(model);

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
        public string doManageTRProjobworkingList(InputProjectTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO018.5";
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

                cls_ctTRProjobworking controller = new cls_ctTRProjobworking();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRProjobworking>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.project_code, input.job_code);

                if (clear)
                {
                    foreach (cls_TRProjobworking model in jsonArray)
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
        public string doDeleteTRProjobworking(InputTRProjobworking input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO018.3";
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

                cls_ctTRProjobworking controller = new cls_ctTRProjobworking();

                if (controller.checkDataOld(input.project_code, input.projob_code, input.projob_code, Convert.ToDateTime(input.projobworking_workdate)))
                {
                    bool blnResult = controller.delete(input.project_code, input.projob_code, input.projob_code, Convert.ToDateTime(input.projobworking_workdate));

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
                    string message = "Not Found Project code : " + input.project_code;
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
        public async Task<string> doUploadTRProjobworking(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO018.4";
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
                    cls_srvProjectImport srv_import = new cls_srvProjectImport();
                    string tmp = srv_import.doImportExcel("PROJECT_JOBWORKING", fileName, by);

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

        #region Task
        public string getMTTaskList(FillterTask req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SYS903.1";
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

                cls_ctMTTask objTask = new cls_ctMTTask();
                List<cls_MTTask> listTask = objTask.getDataByFillter(req.company, "", req.type, req.status);

                JArray array = new JArray();

                if (listTask.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTTask model in listTask)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("project_code", model.project_code);
                        json.Add("task_id", model.task_id);
                        json.Add("task_type", model.task_type);
                        json.Add("task_status", model.task_status);

                        json.Add("task_start", this.doCheckDateTimeEmpty(model.task_start));
                        json.Add("task_end", this.doCheckDateTimeEmpty(model.task_end));
                        json.Add("task_note", model.task_note);

                        if (model.task_type.Equals("IMP_TIME"))
                        {
                            json.Add("task_detail", model.taskdetail_process);
                        }
                        else if (model.task_type.Equals("IMP_XLS"))
                        {
                            json.Add("task_detail", model.task_note);
                        }
                        else
                        {
                            json.Add("task_detail", model.taskdetail_process + " (" + model.taskdetail_fromdate.ToString("dd/MM/yy") + "-" + model.taskdetail_todate.ToString("dd/MM/yy") + ":" + model.taskdetail_paydate.ToString("dd/MM/yy") + ")");
                        }

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
        public string getTRTaskdetail(InputMTTask input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SYS903.2";
            log.apilog_by = input.modified_by;
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

                cls_ctMTTask objTask = new cls_ctMTTask();
                cls_TRTaskdetail task_detail = objTask.getTaskDetail(input.task_id.ToString());

                JArray array = new JArray();

                if (task_detail != null)
                {

                    JObject json = new JObject();

                    json.Add("task_id", task_detail.task_id);
                    json.Add("taskdetail_process", task_detail.taskdetail_process);
                    json.Add("taskdetail_fromdate", task_detail.taskdetail_fromdate);
                    json.Add("taskdetail_todate", task_detail.taskdetail_todate);
                    json.Add("taskdetail_paydate", task_detail.taskdetail_paydate);

                    array.Add(json);

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
        public string getTRTaskwhose(InputMTTask input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SYS903.3";
            log.apilog_by = input.modified_by; ;
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

                cls_ctMTTask objTask = new cls_ctMTTask();
                List<cls_TRTaskwhose> listWhose = objTask.getTaskWhose(input.task_id.ToString());

                JArray array = new JArray();

                if (listWhose.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRTaskwhose model in listWhose)
                    {
                        JObject json = new JObject();

                        json.Add("task_id", model.task_id);
                        json.Add("worker_code", model.worker_code);

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
        public string doManageTask(InputMTTask input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SYS903.4";
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

                cls_ctMTTask objTask = new cls_ctMTTask();
                cls_MTTask model = new cls_MTTask();

                cls_TRTaskdetail detail = new cls_TRTaskdetail();

                List<cls_TRTaskwhose> list_whose = new List<cls_TRTaskwhose>();

                model.company_code = input.company_code;
                model.project_code = input.project_code;

                model.task_id = input.task_id;
                model.task_type = input.task_type;
                model.task_status = input.task_status;

                model.modified_by = input.modified_by;
                model.flag = model.flag;

                //-- Task detail
                string detail_data = input.detail_data;

                string whose_data = input.whose_data;

                try
                {
                    var jsonArray = JsonConvert.DeserializeObject<List<cls_TRTaskdetail>>(detail_data);
                    List<cls_TRTaskdetail> list_model = new List<cls_TRTaskdetail>();
                    foreach (cls_TRTaskdetail item in jsonArray)
                    {
                        item.task_id = model.task_id;
                        list_model.Add(item);
                    }

                    if (list_model.Count > 0)
                    {
                        detail = list_model[0];
                    }

                    //--
                    var jsonArray2 = JsonConvert.DeserializeObject<List<cls_TRTaskwhose>>(whose_data);
                    foreach (cls_TRTaskwhose item in jsonArray2)
                    {
                        item.task_id = model.task_id;
                        list_whose.Add(item);
                    }

                }
                catch (Exception ex)
                {
                    string str = ex.ToString();
                }

                int intTaskID = objTask.insert(model, detail, list_whose);

                if (intTaskID > 0)
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = intTaskID;

                    log.apilog_status = "200";
                    log.apilog_message = "";

                    //if (input.task_type.Trim().Equals("CAL_TAX"))
                    //{
                    //    cls_srvProcessPayroll srvPay = new cls_srvProcessPayroll();
                    //    srvPay.doCalculateTax(input.company_code, intTaskID.ToString());
                    //}
                    if (input.task_type.Trim().Equals("SUM_TIME"))
                    {
                        cls_srvProcessTime srvTime = new cls_srvProcessTime();
                        srvTime.doSummarizeTime(input.company_code, intTaskID.ToString());
                    }
                    //else if (input.task_type.Trim().Equals("CAL_TIME"))
                    //{
                    //    cls_srvProcessTime srvTime = new cls_srvProcessTime();
                    //    srvTime.doCalculateTime(input.company_code, intTaskID.ToString());
                    //}
                    else if (input.task_type.Trim().Equals("IMP_TIME"))
                    {
                        cls_srvProcessTime srvTime = new cls_srvProcessTime();
                        srvTime.doImportTime(input.company_code, intTaskID.ToString());
                    }
                    //else if (input.task_type.Trim().Equals("CAL_BONUS"))
                    //{
                    //    cls_srvProcessPayroll srvPay = new cls_srvProcessPayroll();
                    //    srvPay.doCalculateBonus(input.company_code, intTaskID.ToString());
                    //}
                    //else if (input.task_type.Trim().Equals("TRN_BANK"))
                    //{
                    //    cls_srvProcessPayroll srvPay = new cls_srvProcessPayroll();
                    //    string link = srvPay.doExportBank(input.company_code, intTaskID.ToString());

                    //    output["result_link"] = link;
                    //}
                    //else if (input.task_type.Trim().Equals("TRN_TAX"))
                    //{
                    //    cls_srvProcessPayroll srvPay = new cls_srvProcessPayroll();
                    //    string link = srvPay.doExportTax(input.company_code, intTaskID.ToString());

                    //    output["result_link"] = link;
                    //}
                    //else if (input.task_type.Trim().Equals("TRN_PF"))
                    //{
                    //    cls_srvProcessPayroll srvPay = new cls_srvProcessPayroll();
                    //    string link = srvPay.doExportPF(input.company_code, intTaskID.ToString());

                    //    output["result_link"] = link;
                    //}
                    //else if (input.task_type.Trim().Equals("IMP_XLS"))
                    //{
                    //    cls_srvImport srvImport = new cls_srvImport();
                    //    string link = srvImport.doImportExcel(input.company_code, intTaskID.ToString());

                    //}
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objTask.getMessage();
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
        public string doDeleteMTTask(InputMTTask input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SYS903.5";
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

                cls_ctMTTask objTask = new cls_ctMTTask();

                bool blnResult = objTask.delete(input.task_id.ToString());

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
                    log.apilog_message = objTask.getMessage();
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
