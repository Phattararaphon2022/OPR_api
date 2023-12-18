using AntsCode.Util;
using ClassLibrary_BPC.hrfocus.controller;
using ClassLibrary_BPC.hrfocus.controller.Project;
using ClassLibrary_BPC.hrfocus.model;
using ClassLibrary_BPC.hrfocus.model.Project;
using ClassLibrary_BPC.hrfocus.service;
using ClassLibrary_BPC.hrfocus.service.Payroll;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
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
                List<cls_MTProbusiness> list = controller.getDataByFillter(req.company_code,"");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTProbusiness model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);

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
                model.company_code = input.company_code;

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

                if (controller.checkDataOld(input.probusiness_code,input.company_code))
                {
                    bool blnResult = controller.delete(input.probusiness_code, input.company_code);

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
        public async Task<string> doUploadMTProbusiness(string token, string by, string fileName, Stream stream, string com)
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
                    string tmp = srv_import.doImportExcel("PROBUSINESS", fileName, by, com);

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
                List<cls_MTProtype> list = controller.getDataByFillter(req.company_code, req.protype_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTProtype model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);

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
                model.company_code = input.company_code;

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

                if (controller.checkDataOld(input.protype_code, input.company_code))
                {
                    bool blnResult = controller.delete(input.protype_code, input.company_code);

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
        public async Task<string> doUploadMTProtype(string token, string by, string fileName, Stream stream, string com)
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
                    string tmp = srv_import.doImportExcel("PROTYPE", fileName, by, com);

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
                List<cls_MTProuniform> list = controller.getDataByFillter(req.company_code,"");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTProuniform model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);

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
                model.company_code = input.company_code;

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

                if (controller.checkDataOld(input.prouniform_code,input.company_code))
                {
                    bool blnResult = controller.delete(input.prouniform_code,input.company_code);

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
        public async Task<string> doUploadMTProuniform(string token, string by, string fileName, Stream stream, string com)
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
                    string tmp = srv_import.doImportExcel("PROUNIFORM", fileName, by, com);

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
                List<cls_MTProslip> list = controller.getDataByFillter(req.company_code,"");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTProslip model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);

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
                model.company_code = input.company_code;

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

                if (controller.checkDataOld(input.proslip_code,input.company_code))
                {
                    bool blnResult = controller.delete(input.proslip_code, input.company_code);

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
        public async Task<string> doUploadMTProslip(string token, string by, string fileName, Stream stream, string com)
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
                    string tmp = srv_import.doImportExcel("PROSLIP", fileName, by, com);

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
                List<cls_MTProcost> list = controller.getDataByFillter(req.company, "");
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

                if (controller.checkDataOld(input.procost_code, input.company_code))
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
        public async Task<string> doUploadMTProcost(string token, string by, string fileName, Stream stream, string com)
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
                    string tmp = srv_import.doImportExcel("PROCOST", fileName, by, com);

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
                List<cls_MTProject> list = controller.getDataByFillter(req.company, req.project_code, "", "", "", "", "", req.status);

                //-- F add 23/08/2023
                //-- Workflow
                cls_ctTRWorkflow workflow = new cls_ctTRWorkflow();
                List<cls_TRWorkflow> list_workflow = workflow.getDataByFillter(req.company, "", "PRO_NEW");

                //-- Approve history
                cls_ctTRApprove approve = new cls_ctTRApprove();
                List<cls_TRApprove> list_approve = approve.getDataByFillter(req.company, "PRO_NEW", "");

                if (req.status.Equals("W"))
                {
                    bool find_approve = false;
                    foreach (cls_TRWorkflow model in list_workflow)
                    {
                        if (req.username.Equals(model.account_user))
                        {
                            find_approve = true;
                            break;
                        }
                    }

                    if (!find_approve)
                        list = new List<cls_MTProject>();
                }
                
                
                
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    cls_ctTRProjobshift shift_controller = new cls_ctTRProjobshift();
                    cls_ctTRProjobcost cost_controller = new cls_ctTRProjobcost();

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

                        json.Add("project_proarea", model.project_proarea);
                        json.Add("project_progroup", model.project_progroup);

                        json.Add("project_probusiness", model.project_probusiness);
                        json.Add("project_roundtime", model.project_roundtime);
                        json.Add("project_roundmoney", model.project_roundmoney);
                        json.Add("project_proholiday", model.project_proholiday);

                        json.Add("project_status", model.project_status);
                        json.Add("company_code", model.company_code);
                                                
                        int manpower = 0;    
                        double sum_cost = 0;


                        cls_ctMTProjobversion jobversion_controller = new cls_ctMTProjobversion();
                        string lastversion = jobversion_controller.getLastVersion(model.project_code);

                        cls_ctMTProjobmain job_controller = new cls_ctMTProjobmain();
                        List<cls_MTProjobmain> list_job = job_controller.getDataByFillter(model.language,model.project_code, lastversion, "");

                        //-- Contract
                        cls_ctTRProcontract contract = new cls_ctTRProcontract();
                        List<cls_TRProcontract> list_contract = contract.getDataByFillter(model.project_code);

                        //-- Approve
                        int count_approve = 0;
                        foreach (cls_TRApprove appr in list_approve)
                        {
                            if (model.project_code.Equals(appr.approve_code))
                                count_approve++;
                        }
                        //json.Add("approve_status", count_approve.ToString() + "/" + list_workflow.Count.ToString());
                        json.Add("approve_status", count_approve.ToString()  );

                        foreach (cls_MTProjobmain jobmain in list_job)
                        {
                            
                            int count_emp = 0;
                            int count_working = 0;
                            List<cls_TRProjobshift> shift_list = shift_controller.getDataByFillter(jobmain.project_code, jobmain.projobmain_code, lastversion);
                            foreach (cls_TRProjobshift tmp in shift_list)
                            {
                                count_emp += tmp.projobshift_emp;
                                count_working += tmp.projobshift_working;
                            }

                            double cost = 0;
                            List<cls_TRProjobcost> cost_list_max = cost_controller.getDataByFillter(model.project_code, jobmain.projobmain_code, lastversion,req.company);
                            foreach (cls_TRProjobcost jobcost in cost_list_max)
                            {
                                if (jobcost.procost_type.Equals("D"))
                                    cost += jobcost.projobcost_amount * count_working;
                                else
                                    cost += jobcost.projobcost_amount;

                            }


                            //-- Total
                            cost *= count_emp;

                            sum_cost += cost;
                            manpower += count_emp;
                        }

                        json.Add("project_emp", manpower);
                        json.Add("project_cost", sum_cost);


                        DateTime project_start = new DateTime();
                        DateTime project_end = new DateTime();
                        foreach (cls_TRProcontract model_ in list_contract)
                        {
                            project_start = model_.procontract_fromdate;
                            project_end = model_.procontract_todate;


                        }

                        json.Add("project_start", project_start);
                        json.Add("project_end", project_end);
            

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
                //model.project_id = input.project_id;

                model.project_id = Convert.ToInt32(input.project_id);
                model.project_code = input.project_code;
                model.project_name_th = input.project_name_th;
                model.project_name_en = input.project_name_en;

                model.project_name_sub = input.project_name_sub;
                model.project_codecentral = input.project_codecentral;
                model.project_protype = input.project_protype;

                model.project_proarea = input.project_proarea;
                model.project_progroup = input.project_progroup;
                

                model.project_probusiness = input.project_probusiness;
                //
                model.project_roundtime = input.project_roundtime;
                model.project_roundmoney = input.project_roundmoney;
                model.project_proholiday = input.project_proholiday;
                //

                model.project_status = input.project_status;
                model.company_code = input.company_code;               

                model.modified_by = input.modified_by;

                string strID = controller.insert(model);
                //
                cls_ctTRApprove controllers = new cls_ctTRApprove();
                cls_TRApprove modell = new cls_TRApprove();
                if (controllers.checkDataOlds(input.company_code, "PRO_NEW", input.project_code, input.modified_by))
                {
                    bool blnResult = controllers.delete(input.company_code, "PRO_NEW", input.project_code, input.modified_by);
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
                    output["message"] = "Code Format is incorrect";

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

                cls_ctMTProject controller = new cls_ctMTProject();

                if (controller.checkDataOld(input.project_code, input.company_code, input.project_id.ToString()))
                {
                    bool blnResult = controller.delete(input.project_code, input.company_code);

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
                    string message = "Not Found Project code : " + input.project_id;
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
        public async Task<string> doUploadMTProject(string token, string by, string fileName, Stream stream, string com)
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
                    string tmp = srv_import.doImportExcel("PROJECT", fileName, by, com);

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


        #region MTProjectFillterList
        public string getMTProjectFillterList(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO028.1";
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

                DateTime fromdate = Convert.ToDateTime(req.fromdate);
                DateTime todate = Convert.ToDateTime(req.todate);


                cls_ctMTProject controller = new cls_ctMTProject();
                List<cls_MTProject> list = controller.getDataByFillterAll(req.company_code, req.project_code, req.project_probusiness, req.project_proarea, req.status, req.searchemp, req.proresponsible_position, req.proresponsible_area);
 

                //-- Approve history
                cls_ctTRApprove approve = new cls_ctTRApprove();
                List<cls_TRApprove> list_approve = approve.getDataByFillter(req.company_code, "PRO_NEW", "");

               
                
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    cls_ctTRProjobshift shift_controller = new cls_ctTRProjobshift();
                    cls_ctTRProjobcost cost_controller = new cls_ctTRProjobcost();

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

                        json.Add("project_proarea", model.project_proarea);
                        json.Add("project_progroup", model.project_progroup);

                        json.Add("project_probusiness", model.project_probusiness);
                        json.Add("project_roundtime", model.project_roundtime);
                        json.Add("project_roundmoney", model.project_roundmoney);
                        json.Add("project_proholiday", model.project_proholiday);

                        json.Add("project_status", model.project_status);
                        json.Add("company_code", model.company_code);
                                                
                        int manpower = 0;    
                        double sum_cost = 0;


                        cls_ctMTProjobversion jobversion_controller = new cls_ctMTProjobversion();
                        cls_MTProjobversion singleContract = jobversion_controller.getDataCurrents(model.project_code, fromdate, todate);

                        string lastversion = jobversion_controller.getLastVersion(model.project_code);

                        //cls_ctMTProjobversion jobversion_controllers = new cls_ctMTProjobversion();

                        cls_ctMTProjobmain job_controller = new cls_ctMTProjobmain();
                        List<cls_MTProjobmain> list_job = job_controller.getDataByFillter(model.language,model.project_code, lastversion, "");

                        //-- Contract
                        cls_ctTRProcontract contract = new cls_ctTRProcontract();
                        //List<cls_TRProcontract> list_contract = contract.getDataCurrents(model.project_code, fromdate, todate);
                        List<cls_TRProcontract> list_contract = contract.getDataByFillter(model.project_code);

                        //-- Approve
                        int count_approve = 0;
                        foreach (cls_TRApprove appr in list_approve)
                        {
                            if (model.project_code.Equals(appr.approve_code))
                                count_approve++;
                        }

                        json.Add("approve_status", count_approve.ToString());

                        foreach (cls_MTProjobmain jobmain in list_job)
                        {
                            
                            int count_emp = 0;
                            int count_working = 0;
                            List<cls_TRProjobshift> shift_list = shift_controller.getDataByFillter(jobmain.project_code, jobmain.projobmain_code, lastversion);
                            foreach (cls_TRProjobshift tmp in shift_list)
                            {
                                count_emp += tmp.projobshift_emp;
                                count_working += tmp.projobshift_working;
                            }

                            double cost = 0;
                            List<cls_TRProjobcost> cost_list_max = cost_controller.getDataByFillter(model.project_code, jobmain.projobmain_code, lastversion, model.company_code);
                            foreach (cls_TRProjobcost jobcost in cost_list_max)
                            {
                                if (jobcost.procost_type.Equals("D"))
                                    cost += jobcost.projobcost_amount * count_working;
                                else
                                    cost += jobcost.projobcost_amount;

                            }


                            //-- Total
                            cost *= count_emp;

                            sum_cost += cost;
                            manpower += count_emp;
                        }

                        json.Add("project_emp", manpower);
                        json.Add("project_cost", sum_cost);


                        DateTime project_start = new DateTime();
                        DateTime project_end = new DateTime();
                        foreach (cls_TRProcontract model_ in list_contract)
                        {
                            project_start = model_.procontract_fromdate;
                            project_end = model_.procontract_todate;


                        }

                        json.Add("project_start", project_start);
                        json.Add("project_end", project_end);
            

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

        #region MTProjectFillterdate2 กรองวันที่fromdate
        public string getMTProjectFillterList2(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO028.1";
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

                DateTime fromdate = Convert.ToDateTime(req.fromdate);
                DateTime todate = Convert.ToDateTime(req.todate);


                cls_ctMTProject controller = new cls_ctMTProject();
                List<cls_MTProject> list = controller.getDataCurrents(req.project_code, fromdate, todate);
                //List<cls_MTProject> lists = controller.getDataByFillterAll(req.company_code, req.project_code, req.project_probusiness, req.project_proarea, req.status);

               

                //-- Approve history
                cls_ctTRApprove approve = new cls_ctTRApprove();
                List<cls_TRApprove> list_approve = approve.getDataByFillter(req.company_code, "PRO_NEW", "");


                JArray array = new JArray();

                if (list.Count > 0)
                {
                    cls_ctTRProjobshift shift_controller = new cls_ctTRProjobshift();
                    cls_ctTRProjobcost cost_controller = new cls_ctTRProjobcost();

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

                        json.Add("project_proarea", model.project_proarea);
                        json.Add("project_progroup", model.project_progroup);

                        json.Add("project_probusiness", model.project_probusiness);
                        json.Add("project_roundtime", model.project_roundtime);
                        json.Add("project_roundmoney", model.project_roundmoney);
                        json.Add("project_proholiday", model.project_proholiday);

                        json.Add("project_status", model.project_status);
                        json.Add("company_code", model.company_code);

                        int manpower = 0;
                        double sum_cost = 0;


                        cls_ctMTProjobversion jobversion_controller = new cls_ctMTProjobversion();
                        cls_MTProjobversion singleContract = jobversion_controller.getDataCurrents(model.project_code, fromdate, todate);

                        string lastversion = jobversion_controller.getLastVersion(model.project_code);

                        //cls_ctMTProjobversion jobversion_controllers = new cls_ctMTProjobversion();

                        cls_ctMTProjobmain job_controller = new cls_ctMTProjobmain();
                        List<cls_MTProjobmain> list_job = job_controller.getDataByFillter(model.language,model.project_code, lastversion, "");

                        //-- Contract
                        cls_ctTRProcontract contract = new cls_ctTRProcontract();
                        List<cls_TRProcontract> list_contract = contract.getDataCurrents(model.project_code, fromdate, todate);
                        //List<cls_TRProcontract> list_contract = contract.getDataByFillter(model.project_code);

                        //-- Approve
                        int count_approve = 0;
                        foreach (cls_TRApprove appr in list_approve)
                        {
                            if (model.project_code.Equals(appr.approve_code))
                                count_approve++;
                        }

                        json.Add("approve_status", count_approve.ToString() + "/");

                        foreach (cls_MTProjobmain jobmain in list_job)
                        {

                            int count_emp = 0;
                            int count_working = 0;
                            List<cls_TRProjobshift> shift_list = shift_controller.getDataByFillter(jobmain.project_code, jobmain.projobmain_code, lastversion);
                            foreach (cls_TRProjobshift tmp in shift_list)
                            {
                                count_emp += tmp.projobshift_emp;
                                count_working += tmp.projobshift_working;
                            }

                            double cost = 0;
                            List<cls_TRProjobcost> cost_list_max = cost_controller.getDataByFillter(model.project_code, jobmain.projobmain_code, lastversion, model.company_code);
                            foreach (cls_TRProjobcost jobcost in cost_list_max)
                            {
                                if (jobcost.procost_type.Equals("D"))
                                    cost += jobcost.projobcost_amount * count_working;
                                else
                                    cost += jobcost.projobcost_amount;

                            }


                            //-- Total
                            cost *= count_emp;

                            sum_cost += cost;
                            manpower += count_emp;
                        }

                        json.Add("project_emp", manpower);
                        json.Add("project_cost", sum_cost);


                        DateTime project_start = new DateTime();
                        DateTime project_end = new DateTime();
                        foreach (cls_TRProcontract model_ in list_contract)
                        {
                            project_start = model_.procontract_fromdate;
                            project_end = model_.procontract_todate;


                        }

                        json.Add("project_start", project_start);
                        json.Add("project_end", project_end);


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

   
         // กรองวันที่todate
        public string getMTProjectFillterList3(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO028.1";
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

                DateTime fromdate = Convert.ToDateTime(req.fromdate);
                DateTime todate = Convert.ToDateTime(req.todate);


                cls_ctMTProject controller = new cls_ctMTProject();
                List<cls_MTProject> list = controller.getDataCurrents(req.project_code, fromdate, todate);
                //List<cls_MTProject> lists = controller.getDataByFillterAll(req.company_code, req.project_code, req.project_probusiness, req.project_proarea, req.status);


                //-- Approve history
                cls_ctTRApprove approve = new cls_ctTRApprove();
                List<cls_TRApprove> list_approve = approve.getDataByFillter(req.company_code, "PRO_NEW", "");


                JArray array = new JArray();

                if (list.Count > 0)
                {
                    cls_ctTRProjobshift shift_controller = new cls_ctTRProjobshift();
                    cls_ctTRProjobcost cost_controller = new cls_ctTRProjobcost();

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

                        json.Add("project_proarea", model.project_proarea);
                        json.Add("project_progroup", model.project_progroup);

                        json.Add("project_probusiness", model.project_probusiness);
                        json.Add("project_roundtime", model.project_roundtime);
                        json.Add("project_roundmoney", model.project_roundmoney);
                        json.Add("project_proholiday", model.project_proholiday);

                        json.Add("project_status", model.project_status);
                        json.Add("company_code", model.company_code);

                        int manpower = 0;
                        double sum_cost = 0;


                        cls_ctMTProjobversion jobversion_controller = new cls_ctMTProjobversion();
                        cls_MTProjobversion singleContract = jobversion_controller.getDataCurrents(model.project_code, fromdate, todate);

                        string lastversion = jobversion_controller.getLastVersion(model.project_code);

                        //cls_ctMTProjobversion jobversion_controllers = new cls_ctMTProjobversion();

                        cls_ctMTProjobmain job_controller = new cls_ctMTProjobmain();
                        List<cls_MTProjobmain> list_job = job_controller.getDataByFillter(model.language,model.project_code, lastversion, "");

                        //-- Contract
                        cls_ctTRProcontract contract = new cls_ctTRProcontract();
                        List<cls_TRProcontract> list_contract = contract.getDataCurrents2(model.project_code, fromdate, todate);
                        //List<cls_TRProcontract> list_contract = contract.getDataByFillter(model.project_code);

                        //-- Approve
                        int count_approve = 0;
                        foreach (cls_TRApprove appr in list_approve)
                        {
                            if (model.project_code.Equals(appr.approve_code))
                                count_approve++;
                        }

                        json.Add("approve_status", count_approve.ToString() );

                        foreach (cls_MTProjobmain jobmain in list_job)
                        {

                            int count_emp = 0;
                            int count_working = 0;
                            List<cls_TRProjobshift> shift_list = shift_controller.getDataByFillter(jobmain.project_code, jobmain.projobmain_code, lastversion);
                            foreach (cls_TRProjobshift tmp in shift_list)
                            {
                                count_emp += tmp.projobshift_emp;
                                count_working += tmp.projobshift_working;
                            }

                            double cost = 0;
                            List<cls_TRProjobcost> cost_list_max = cost_controller.getDataByFillter(model.project_code, jobmain.projobmain_code, lastversion, model.company_code);
                            foreach (cls_TRProjobcost jobcost in cost_list_max)
                            {
                                if (jobcost.procost_type.Equals("D"))
                                    cost += jobcost.projobcost_amount * count_working;
                                else
                                    cost += jobcost.projobcost_amount;

                            }


                            //-- Total
                            cost *= count_emp;

                            sum_cost += cost;
                            manpower += count_emp;
                        }

                        json.Add("project_emp", manpower);
                        json.Add("project_cost", sum_cost);


                        DateTime project_start = new DateTime();
                        DateTime project_end = new DateTime();
                        foreach (cls_TRProcontract model_ in list_contract)
                        {
                            project_start = model_.procontract_fromdate;
                            project_end = model_.procontract_todate;


                        }

                        json.Add("project_start", project_start);
                        json.Add("project_end", project_end);


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
        public async Task<string> doUploadTRProaddress(string token, string by, string fileName, Stream stream, string com)
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
                    string tmp = srv_import.doImportExcel("PROJECT_ADDRESS", fileName, by, com);

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
        public async Task<string> doUploadTRProcontact(string token, string by, string fileName, Stream stream, string com)
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
                    string tmp = srv_import.doImportExcel("PROJECT_CONTACT", fileName, by, com);

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
                        json.Add("procontract_type", model.procontract_type);

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
                    bool blnResult = controller.delete(input.project_code, input.procontract_ref,"");

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
        public async Task<string> doUploadTRProcontract(string token, string by, string fileName, Stream stream, string com)
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
                    string tmp = srv_import.doImportExcel("PROJECT_CONTRACT", fileName, by, com);

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
        public async Task<string> doUploadTRProresponsible(string token, string by, string fileName, Stream stream, string com)
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
                    string tmp = srv_import.doImportExcel("PROJECT_RESPONSIBLE", fileName, by, com);

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
        public async Task<string> doUploadMTProtimepol(string token, string by, string fileName, Stream stream, string com)
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
                    string tmp = srv_import.doImportExcel("PROJECT_TIMEPOL", fileName, by, com);

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

                          
                //-- Job contract
                //cls_ctTRProjobcontract contract_controller = new cls_ctTRProjobcontract();
                //List<cls_TRProjobcontract> contract_list = contract_controller.getDataByFillter(req.project_code, "");

                //-- Job shift
                cls_ctTRProjobshift shift_controller = new cls_ctTRProjobshift();
                List<cls_TRProjobshift> shift_list = shift_controller.getDataByFillter(req.project_code, "", req.version);


                //-- Job cost
                cls_ctTRProjobcost cost_controller = new cls_ctTRProjobcost();
                cls_ctMTProcost cost_controllerall = new cls_ctMTProcost();
                

                cls_ctMTProjobmain controller = new cls_ctMTProjobmain();
                List<cls_MTProjobmain> list = controller.getDataByFillter(req.language,req.project_code, req.version, "");
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
                        json.Add("projobmain_jobtype", model.projobmain_jobtype);

                        json.Add("projobmain_fromdate", model.projobmain_fromdate);
                        json.Add("projobmain_todate", model.projobmain_todate);

                        


                        json.Add("projobmain_type", model.projobmain_type);
                        

                        json.Add("projobmain_timepol", model.projobmain_timepol);
                        json.Add("projobmain_slip", model.projobmain_slip);
                        json.Add("projobmain_uniform", model.projobmain_uniform);

                        json.Add("project_code", model.project_code);
 

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
                        json.Add("protype_name", model.protype_name);
                        json.Add("version", model.version);

                        //-- Contract
                        //cls_TRProjobcontract contract = null;
                        //foreach (cls_TRProjobcontract tmp in contract_list)
                        //{
                        //    if (tmp.projob_code.Equals(model.projobmain_code))
                        //    {
                        //        contract = tmp;
                        //        break;
                        //    }                                
                        //}
                        
                        //-- Job shift
                        int manpower = 0;
                        int working = 0;
                        foreach (cls_TRProjobshift tmp in shift_list)
                        {
                            if (tmp.projob_code.Equals(model.projobmain_code))
                            {
                                manpower += tmp.projobshift_emp;
                                working += tmp.projobshift_working;
                            }
                        }


                        //-- Allow
                        List<cls_TRProjobcost> cost_list_max = cost_controller.getDataByFillter(req.project_code, model.projobmain_code, req.version,req.company);

                        List<cls_MTProcost> cost_list = cost_controllerall.getDataByFillter(req.company, "");
                        int i = 1;
                        foreach (cls_MTProcost cost in cost_list)
                        {
                             bool containsTarget = cost_list_max.Any(item => item.projobcost_code == cost.procost_code);
                             switch (i)
                             {
                                 case 1: model.allow1 += containsTarget?doGetAmountPerday(cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount, cost.procost_type) * working:0; break;
                                 case 2: model.allow2 += containsTarget ? doGetAmountPerday(cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount, cost.procost_type) * working : 0; break;
                                 case 3: model.allow3 += containsTarget ? doGetAmountPerday(cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount, cost.procost_type) * working : 0; break;
                                 case 4: model.allow4 += containsTarget ? doGetAmountPerday(cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount, cost.procost_type) * working : 0; break;
                                 case 5: model.allow5 += containsTarget ? doGetAmountPerday(cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount, cost.procost_type) * working : 0; break;
                                 case 6: model.allow6 += containsTarget ? doGetAmountPerday(cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount, cost.procost_type) * working : 0; break;
                                 case 7: model.allow7 += containsTarget ? doGetAmountPerday(cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount, cost.procost_type) * working : 0; break;
                                 case 8: model.allow8 += containsTarget ? doGetAmountPerday(cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount, cost.procost_type) * working : 0; break;
                                 case 9: model.allow9 += containsTarget ? doGetAmountPerday(cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount, cost.procost_type) * working : 0; break;
                                 case 10: model.allow10 += containsTarget ? doGetAmountPerday(cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount, cost.procost_type) * working : 0; break;
                             }

                            i++;
                                json.Add(cost.procost_code, containsTarget ? doGetAmountPerday(cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount, cost.procost_type) * working : 0);
                        }
                        model.emp_total = manpower;
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

        private double doGetAmountPerday(double amount, string type)
        {
            double result = amount;

            if (type.Equals("M"))
                result = amount / 30;

            return result;
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

                bool clear = controller.delete(input.version, input.project_code, "");

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


                if (success > 0)
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

                if (controller.checkDataOld(input.version, input.project_code, input.projobmain_code ,input.projobmain_id))
                {
                    bool blnResult = controller.delete(input.version, input.project_code, input.projobmain_code);

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
        public async Task<string> doUploadMTProjobmain(string token, string by, string fileName, Stream stream, string com)
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
                    string tmp = srv_import.doImportExcel("PROJECT_JOBMAIN", fileName, by, com);

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
                List<cls_TRProjobcontract> list = controller.getDataByFillter(req.version, req.project_code, req.job_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRProjobcontract model in list)
                    {
                        JObject json = new JObject();
                        json.Add("projobcontract_id", model.projobcontract_id);
                        json.Add("projobcontract_ref", model.projobcontract_ref);
                        json.Add("projobcontract_working", model.projobcontract_working);
                        json.Add("projobcontract_hrsperday", model.projobcontract_hrsperday);
                        json.Add("projobcontract_hrsot", model.projobcontract_hrsot);
                        
                        json.Add("projob_code", model.projob_code);
                        json.Add("project_code", model.project_code);
                        json.Add("version", model.version);
                       
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

                model.projobcontract_working = input.projobcontract_working;
                model.projobcontract_hrsperday = input.projobcontract_hrsperday;
                model.projobcontract_hrsot = input.projobcontract_hrsot;
                  
                model.projob_code = input.projob_code;
                model.project_code = input.project_code;
                model.version = input.version;

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

                bool clear = controller.delete(input.version, input.project_code, input.job_code);

                if (clear)
                {
                    foreach (cls_TRProjobcontract model in jsonArray)
                    {

                        model.version = input.version;
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
        public async Task<string> doUploadTRProjobcontract(string token, string by, string fileName, Stream stream, string com)
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
                    string tmp = srv_import.doImportExcel("PROJECT_JOBCONTRACT", fileName, by, com);

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
                List<cls_TRProjobcost> list = controller.getDataByFillter(req.project_code, req.job_code, req.version,req.company_code);
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
                        json.Add("version", model.version);
                        json.Add("projobcost_status", model.projobcost_status);

                        json.Add("projobcost_auto", model.projobcost_auto);

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
                model.version = input.version;
                model.projobcost_status = input.projobcost_status;
                model.projobcost_auto = input.projobcost_auto;
                
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

                bool clear = controller.delete(input.version, input.project_code, input.job_code);

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

                if (controller.checkDataOld(input.project_code, input.projob_code, input.projobcost_code, input.version,input.projobcost_id))
                {
                    bool blnResult = controller.delete(input.project_code, input.projob_code, input.projobcost_code, input.version);

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
        public async Task<string> doUploadTRProjobcost(string token, string by, string fileName, Stream stream, string com)
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
                    string tmp = srv_import.doImportExcel("PROJECT_JOBCOST", fileName, by, com);

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

                if (controller.checkDataOld(input.project_code, input.projob_code, input.projobmachine_ip,input.projobmachine_id))
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
        public async Task<string> doUploadTRProjobmachine(string token, string by, string fileName, Stream stream, string com)
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
                    string tmp = srv_import.doImportExcel("PROJECT_JOBMACHINE", fileName, by, com);

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
                List<cls_TRProjobcontract> contract_list = contract_controller.getDataByFillter(req.version, req.project_code, req.procontract_type);
                cls_ctMTProcost cost_controllerall = new cls_ctMTProcost();
                cls_ctMTProjobsub controller = new cls_ctMTProjobsub();
                List<cls_MTProjobsub> list = controller.getDataByFillter(req.project_code, req.version);

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
                        json.Add("version", model.version);

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
                        List<cls_TRProjobcost> cost_list_max = cost_controller.getDataByFillter(req.project_code, model.projobsub_code, req.version,req.company);

                        List<cls_MTProcost> cost_list = cost_controllerall.getDataByFillter(req.company, "");
                        int i = 1;
                        foreach (cls_MTProcost cost in cost_list)
                        {
                            bool containsTarget = cost_list_max.Any(item => item.projobcost_code == cost.procost_code);
                            switch (i)
                            {
                                case 1: model.allow1 += containsTarget? cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount:0; break;
                                case 2: model.allow2 += containsTarget ? cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount : 0; break;
                                case 3: model.allow3 += containsTarget ? cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount : 0; break;
                                case 4: model.allow4 += containsTarget ? cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount : 0; break;
                                case 5: model.allow5 += containsTarget ? cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount : 0; break;
                                case 6: model.allow6 += containsTarget ? cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount : 0; break;
                                case 7: model.allow7 += containsTarget ? cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount : 0; break;
                                case 8: model.allow8 += containsTarget ? cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount : 0; break;
                                case 9: model.allow9 += containsTarget ? cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount : 0; break;
                                case 10: model.allow10 += containsTarget ? cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount : 0; break;
                            }
                            json.Add(cost.procost_code, containsTarget ? cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount : 0);
                            i++;
                        }


                        if (contract != null)
                        {
                            model.emp_total = contract.projobcontract_working;
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

                bool clear = controller.delete(input.version, input.project_code);

                if (clear)
                {
                    foreach (cls_MTProjobsub model in jsonArray)
                    {
                        model.version = input.version;
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

                if (controller.checkDataOld(input.version, input.project_code, input.projobsub_code))
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
        public async Task<string> doUploadMTProjobsub(string token, string by, string fileName, Stream stream, string com)
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
                    string tmp = srv_import.doImportExcel("PROJECT_JOBSUB", fileName, by, com);

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

        #region แสดงข้อมูลหน้า จัดการข้อมูลโครงการ  
        public string getProjobempFillterList(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO027.1";
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
                List<cls_TRProjobemp> list = controller.getDataByFillterAll(req.project_code, req.projob_code, req.projobemp_emp, req.projobemp_type, req.searchemp, req.projobemp_status);
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
                        json.Add("projobemp_type", model.projobemp_type);
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
        #endregion


        #region แสดงข้อมูลหน้ารายการอนุมัติโครงการ
        public string getTRProjobempList4(FillterProject req)
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

                //cls_ctMTProject controller = new cls_ctMTProject();
                //List<cls_MTProject> list = controller.getDataByFillter(req.company, req.project_code, "", "", "", "", "", req.status);



                cls_ctTRProjobemp controller = new cls_ctTRProjobemp();
                List<cls_TRProjobemp> list = controller.getDataByFillterAll(req.project_code, "", "", "", "", req.status);

                //List<cls_TRProjobemp> list = controller.getDataByFillter(req.project_code, "");
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
                        json.Add("projobemp_type", model.projobemp_type);
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
        #endregion


        #region TRProjobemp //กรองวันที่fromdate
        public string getTRProjobempList2(FillterProject req)
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
                DateTime fromdate = Convert.ToDateTime(req.fromdate);
                DateTime todate = Convert.ToDateTime(req.todate);

                cls_ctTRProjobemp controller = new cls_ctTRProjobemp();
                List<cls_TRProjobemp> list = controller.getDataByFillter2(req.project_code, todate, todate);
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
                        json.Add("projobemp_type", model.projobemp_type);
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

        //กรองวันที่todate
        public string getTRProjobempList3(FillterProject req)
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
                DateTime fromdate = Convert.ToDateTime(req.fromdate);
                DateTime todate = Convert.ToDateTime(req.todate);

                cls_ctTRProjobemp controller = new cls_ctTRProjobemp();
                List<cls_TRProjobemp> list = controller.getDataByFillter3(req.project_code, fromdate, todate);
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
                        json.Add("projobemp_type", model.projobemp_type);
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
        //
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
                        json.Add("projobemp_type", model.projobemp_type);
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
                model.projobemp_type = input.projobemp_type;
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
        //
        //public string doManageTRProjobempList(InputProjectTransaction input)
        //{
        //    JObject output = new JObject();

        //    var json_data = new JavaScriptSerializer().Serialize(input);
        //    var tmp = JToken.Parse(json_data);


        //    cls_SYSApilog log = new cls_SYSApilog();
        //    log.apilog_code = "PRO017.5";
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

        //        cls_ctMTProject controller = new cls_ctMTProject();
        //        cls_MTProject model = new cls_MTProject();
        //        //model.project_id = input.project_id;

        //        model.project_id = Convert.ToInt32(input.project_id);
        //        model.project_code = input.project_code;
        //        model.project_name_th = input.project_name_th;
        //        model.project_name_en = input.project_name_en;

        //        model.project_name_sub = input.project_name_sub;
        //        model.project_codecentral = input.project_codecentral;
        //        model.project_protype = input.project_protype;

        //        model.project_proarea = input.project_proarea;
        //        model.project_progroup = input.project_progroup;


        //        model.project_probusiness = input.project_probusiness;
        //        //
        //        model.project_roundtime = input.project_roundtime;
        //        model.project_roundmoney = input.project_roundmoney;
        //        model.project_proholiday = input.project_proholiday;
        //        //

        //        model.project_status = input.project_status;
        //        model.company_code = input.company_code;

        //        model.modified_by = input.modified_by;

        //        string strID = controller.insert(model);
        //        //
        //        cls_ctTRApprove controllers = new cls_ctTRApprove();
        //        cls_TRApprove modell = new cls_TRApprove();
        //        if (controllers.checkDataOlds(input.company_code, "PRO_NEW", input.project_code, input.modified_by))
        //        {
        //            bool blnResult = controllers.delete(input.company_code, "PRO_NEW", input.project_code, input.modified_by);
        //        }

        //        if (!strID.Equals(""))
        //        {
        //            output["success"] = true;
        //            output["message"] = "Retrieved data successfully";
        //            output["record_id"] = strID;

        //            log.apilog_status = "200";
        //            log.apilog_message = "";
        //        }
        //        else
        //        {
        //            output["success"] = false;
        //            output["message"] = "Code Format is incorrect";

        //            log.apilog_status = "500";
        //            log.apilog_message = controller.getMessage();
        //        }

        //        controller.dispose();

        //    }
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

        //    output["data"] = tmp;

        //    return output.ToString(Formatting.None);
        //}
        //
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
                 
                //bool clear = controller.delete(input.project_code, input.job_code);

                //if (clear)
                //{
                    foreach (cls_TRProjobemp model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);
                        cls_ctTRApprove controllers = new cls_ctTRApprove();
                        cls_TRApprove modell = new cls_TRApprove();
                        if (controllers.checkDataOlds(input.company_code, "PRO_EMP", input.project_code, input.modified_by))
                        {
                            bool blnResult1 = controllers.delete(input.company_code, "PRO_EMP", input.project_code, input.modified_by);
                        }
                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);

                            error++;
                        }

                    }
                //}
                //else
                //{
                //    error = 1;
                //}

                    
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

                if (controller.checkDataOld(input.project_code, input.projob_code, input.projobemp_emp, Convert.ToDateTime(input.projobemp_fromdate)))
                {
                    bool blnResult = controller.delete(input.project_code, input.projob_code, input.projobemp_emp, Convert.ToDateTime(input.projobemp_fromdate));

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
                    string message = "Not Found Employee code : " + input.project_code;
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
        public async Task<string> doUploadTRProjobemp(string token, string by, string fileName, Stream stream, string com)
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
                    string tmp = srv_import.doImportExcel("PROJECT_JOBEMP", fileName, by, com);

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
        public async Task<string> doUploadTRProjobworking(string token, string by, string fileName, Stream stream, string com)
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
                    string tmp = srv_import.doImportExcel("PROJECT_JOBWORKING", fileName, by, com);

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

        #region TRProjobshift
        public string getTRProjobshiftList(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO020.1";
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


                cls_ctTRProjobshift controller = new cls_ctTRProjobshift();
                List<cls_TRProjobshift> list = controller.getDataByFillter(req.project_code, req.job_code, req.version);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    cls_ctTRProjobcost cost_controller = new cls_ctTRProjobcost();

                    cls_ctMTProcost cost_controllerall = new cls_ctMTProcost();
                    List<cls_MTProcost> cost_list = cost_controllerall.getDataByFillter(req.company_code, "");

                    //-- Allow
                    List<cls_TRProjobcost> cost_list_max = cost_controller.getDataByFillter(req.project_code, req.job_code, req.version,req.company_code);

                    foreach (cls_TRProjobshift model in list)
                    {
                        JObject json = new JObject();
                        json.Add("projobshift_id", model.projobshift_id);
                        json.Add("shift_code", model.shift_code);

                        json.Add("projobshift_sun", model.projobshift_sun);
                        json.Add("projobshift_mon", model.projobshift_mon);
                        json.Add("projobshift_tue", model.projobshift_tue);
                        json.Add("projobshift_wed", model.projobshift_wed);
                        json.Add("projobshift_thu", model.projobshift_thu);
                        json.Add("projobshift_fri", model.projobshift_fri);
                        json.Add("projobshift_sat", model.projobshift_sat);
                        json.Add("projobshift_ph", model.projobshift_ph);

                        json.Add("projobshift_emp", model.projobshift_emp);
                        json.Add("projobshift_working", model.projobshift_working);
                        json.Add("projobshift_hrsperday", model.projobshift_hrsperday);
                        json.Add("projobshift_hrsot", model.projobshift_hrsot);

                        json.Add("projob_code", model.projob_code);
                        json.Add("project_code", model.project_code);
                        json.Add("version", model.version);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
                                               

                        int working = model.projobshift_working;
                        double allow1 = 0; double allow2 = 0; double allow3 = 0; double allow4 = 0; double allow5 = 0;
                        double allow6 = 0; double allow7 = 0; double allow8 = 0; double allow9 = 0; double allow10 = 0;
                        int i = 1;
                        //foreach (cls_TRProjobcost cost in cost_list_max)
                        //{                            
                        //    switch (i)
                        //    {
                        //        case 1: allow1 += doGetAmountPerday(cost.projobcost_amount, cost.procost_type) * working; break;
                        //        case 2: allow2 += doGetAmountPerday(cost.projobcost_amount, cost.procost_type) * working; break;
                        //        case 3: allow3 += doGetAmountPerday(cost.projobcost_amount, cost.procost_type) * working; break;
                        //        case 4: allow4 += doGetAmountPerday(cost.projobcost_amount, cost.procost_type) * working; break;
                        //        case 5: allow5 += doGetAmountPerday(cost.projobcost_amount, cost.procost_type) * working; break;
                        //        case 6: allow6 += doGetAmountPerday(cost.projobcost_amount, cost.procost_type) * working; break;
                        //        case 7: allow7 += doGetAmountPerday(cost.projobcost_amount, cost.procost_type) * working; break;
                        //        case 8: allow8 += doGetAmountPerday(cost.projobcost_amount, cost.procost_type) * working; break;
                        //        case 9: allow9 += doGetAmountPerday(cost.projobcost_amount, cost.procost_type) * working; break;
                        //        case 10: allow10 += doGetAmountPerday(cost.projobcost_amount, cost.procost_type) * working; break;
                        //    }

                        //    i++;
                        //}

                        foreach (cls_MTProcost cost in cost_list)
                        {
                            bool containsTarget = cost_list_max.Any(item => item.projobcost_code == cost.procost_code);
                            switch (i)
                            {
                                case 1: allow1 += containsTarget ? doGetAmountPerday(cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount, cost.procost_type) * working : 0; break;
                                case 2: allow2 += containsTarget ? doGetAmountPerday(cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount, cost.procost_type) * working : 0; break;
                                case 3: allow3 += containsTarget ? doGetAmountPerday(cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount, cost.procost_type) * working : 0; break;
                                case 4: allow4 += containsTarget ? doGetAmountPerday(cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount, cost.procost_type) * working : 0; break;
                                case 5: allow5 += containsTarget ? doGetAmountPerday(cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount, cost.procost_type) * working : 0; break;
                                case 6: allow6 += containsTarget ? doGetAmountPerday(cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount, cost.procost_type) * working : 0; break;
                                case 7: allow7 += containsTarget ? doGetAmountPerday(cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount, cost.procost_type) * working : 0; break;
                                case 8: allow8 += containsTarget ? doGetAmountPerday(cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount, cost.procost_type) * working : 0; break;
                                case 9: allow9 += containsTarget ? doGetAmountPerday(cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount, cost.procost_type) * working : 0; break;
                                case 10: allow10 += containsTarget ? doGetAmountPerday(cost_list_max.Find(c => c.projobcost_code == cost.procost_code).projobcost_amount, cost.procost_type) * working : 0; break;
                            }

                            i++;
                           
                        }



                        double allow_emp = allow1 + allow2 + allow3 + allow4 + allow5 + allow6 + allow7 + allow8 + allow9 + allow10;
                        double allow_total = allow_emp * model.projobshift_emp;

                        json.Add("allow1", allow1);
                        json.Add("allow2", allow2);
                        json.Add("allow3", allow3);
                        json.Add("allow4", allow4);
                        json.Add("allow5", allow5);
                        json.Add("allow6", allow6);
                        json.Add("allow7", allow7);
                        json.Add("allow8", allow8);
                        json.Add("allow9", allow9);
                        json.Add("allow10", allow10);                        
                        json.Add("allow_emp", allow_emp);
                        json.Add("allow_total", allow_total);

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
        public string doManageTRProjobshiftList(InputProjectTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO020.5";
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

                cls_ctTRProjobshift controller = new cls_ctTRProjobshift();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRProjobshift>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.project_code, input.job_code, input.version);

                if (clear)
                {
                    foreach (cls_TRProjobshift model in jsonArray)
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
        public string doDeleteTRProjobshift(InputTRProjobshift input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO020.3";
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

                cls_ctTRProjobshift controller = new cls_ctTRProjobshift();

                if (controller.checkDataOld(input.project_code, input.projob_code, input.shift_code, input.version,input.projobshift_id))
                {
                    bool blnResult = controller.delete(input.project_code, input.projob_code, input.version);

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
                    string message = "Not Found Project code : " + input.project_code + " | Job : " + input.projob_code;
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
        public async Task<string> doUploadTRProjobshift(string token, string by, string fileName, Stream stream, string com)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO020.4";
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
                    string tmp = srv_import.doImportExcel("PROJECT_JOBSHIFT", fileName, by, com);

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

                    if (input.task_type.Trim().Equals("CAL_TAX"))
                    {
                        cls_srvProcessPayroll srvPay = new cls_srvProcessPayroll();
                        srvPay.doCalculateTax(input.company_code, intTaskID.ToString());
                    }
                    else if (input.task_type.Trim().Equals("CAL_INDE"))
                    {
                        cls_srvProcessPayroll srvPay = new cls_srvProcessPayroll();
                        srvPay.doCalculateIncomeDeduct(input.company_code, intTaskID.ToString());
                    }
                    else if (input.task_type.Trim().Equals("SUM_TIME"))
                    {
                        cls_srvProcessTime srvTime = new cls_srvProcessTime();
                        srvTime.doSummarizeTime(input.company_code, intTaskID.ToString());
                    }
                    else if (input.task_type.Trim().Equals("CAL_TIME"))
                    {
                        cls_srvProcessTime srvTime = new cls_srvProcessTime();
                        srvTime.doCalculateTime(input.company_code, intTaskID.ToString());
                    }
                    else if (input.task_type.Trim().Equals("IMP_TIME"))
                    {
                        cls_srvProcessTime srvTime = new cls_srvProcessTime();
                        srvTime.doImportTime(input.company_code, intTaskID.ToString());
                    }
                    else if (input.task_type.Trim().Equals("CAL_BONUS"))
                    {
                        cls_srvProcessPayroll srvPay = new cls_srvProcessPayroll();
                        srvPay.doCalculateBonus(input.company_code, intTaskID.ToString());
                    }
                    else if (input.task_type.Trim().Equals("TRN_BANK"))
                    {
                        cls_srvProcessPayroll srvPay = new cls_srvProcessPayroll();
                        string link = srvPay.doExportBank(input.company_code, intTaskID.ToString());

                        output["result_link"] = link;
                    }
                    else if (input.task_type.Trim().Equals("TRN_SSO"))
                    {
                        cls_srvProcessPayroll srvPay = new cls_srvProcessPayroll();
                        string link = srvPay.doExportSso(input.company_code, intTaskID.ToString());

                        output["result_link"] = link;
                    }
                    else if (input.task_type.Trim().Equals("TRN_TAX"))
                    {
                        cls_srvProcessPayroll srvPay = new cls_srvProcessPayroll();
                        string link = srvPay.doExportTax(input.company_code, intTaskID.ToString());

                        output["result_link"] = link;
                    }
                    else if (input.task_type.Trim().Equals("TRN_BONUS"))
                    {
                        cls_srvProcessPayroll srvPay = new cls_srvProcessPayroll();
                        string link = srvPay.doExportBonus(input.company_code, intTaskID.ToString());

                        output["result_link"] = link;
                    }
                    else if (input.task_type.Trim().Equals("TRN_PF"))
                    {
                        cls_srvProcessPayroll srvPay = new cls_srvProcessPayroll();
                        string link = srvPay.doExportPF(input.company_code, intTaskID.ToString());

                        output["result_link"] = link;
                    }
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

        #region TRProjobversion
        public string getMTProjobversionList(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO022.1";
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

                cls_ctMTProjobversion controller = new cls_ctMTProjobversion();
                List<cls_MTProjobversion> list = new List<cls_MTProjobversion>();

                if (!req.fromdate.Equals(""))
                {
                    cls_MTProjobversion proversion = controller.getDataCurrent(req.project_code, Convert.ToDateTime(req.fromdate));

                    if (proversion != null)
                        list.Add(proversion);

                }
                else
                {
                    list = controller.getDataByFillter(req.project_code);
                }


                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTProjobversion model in list)
                    {
                        JObject json = new JObject();
                        json.Add("projobversion_id", model.projobversion_id);
                        json.Add("transaction_id", model.transaction_id);
                        json.Add("version", model.version);
                        json.Add("fromdate", model.fromdate);
                        json.Add("todate", model.todate);
                        json.Add("transaction_data", model.transaction_data);
                        json.Add("transaction_old", model.transaction_old);
                        json.Add("refso", model.refso);
                        json.Add("custno", model.custno);
                        json.Add("refappcostid", model.refappcostid);
                        json.Add("currency", model.currency);
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
        public string doManageMTProjobversion(InputMTProjobversion input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO022.2";
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

                cls_ctMTProjobversion controller = new cls_ctMTProjobversion();
                cls_MTProjobversion model = new cls_MTProjobversion();

                Random rnd = new Random();
                int genarate  = rnd.Next(1, 500);

                model.projobversion_id = Convert.ToInt32(input.projobversion_id);
                model.transaction_id = "TRN" + DateTime.Now.ToString("yyyyMMddHHmmss") + genarate.ToString();
                model.version = input.version;               
                model.fromdate = Convert.ToDateTime(input.fromdate);
                model.todate = Convert.ToDateTime(input.todate);
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
        public string doDeleteMTProjobversion(InputMTProjobversion input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO022.3";
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

                cls_ctMTProjobversion controller = new cls_ctMTProjobversion();

                if (controller.checkDataOld(input.project_code, input.version))
                {
                    bool blnResult = controller.delete(input.project_code, input.version);

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
                    string message = "Not Found Version : " + input.project_code;
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
        #endregion

        #region TRProjobpol
        public string getTRProjobpolList(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO023.1";
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

                cls_ctTRProjobpol controller = new cls_ctTRProjobpol();
                List<cls_TRProjobpol> list = controller.getDataByFillter(req.project_code, req.job_code);
                
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRProjobpol model in list)
                    {
                        JObject json = new JObject();
                        json.Add("projobpol_id", model.projobpol_id);
                        json.Add("projobpol_type", model.projobpol_type);
                        json.Add("projobpol_timepol", model.projobpol_timepol);
                        json.Add("projobpol_slip", model.projobpol_slip);
                        json.Add("projobpol_uniform", model.projobpol_uniform);
                        json.Add("project_code", model.project_code);
                        json.Add("projobmain_code", model.projobmain_code);
                       
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
        public string doManageTRProjobpol(InputTRProjobpol input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO023.2";
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

                cls_ctTRProjobpol controller = new cls_ctTRProjobpol();
                cls_TRProjobpol model = new cls_TRProjobpol();

                Random rnd = new Random();
                int genarate = rnd.Next(1, 500);

                model.projobpol_id = input.projobpol_id;
                model.projobpol_type = input.projobpol_type;
                model.projobpol_timepol = input.projobpol_timepol;
                model.projobpol_slip = input.projobpol_slip;
                model.projobpol_uniform = input.projobpol_uniform;
                model.project_code = input.project_code;
                model.projobmain_code = input.projobmain_code;
                
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
        public string doDeleteTRProjobpol(InputTRProjobpol input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO023.3";
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

                cls_ctTRProjobpol controller = new cls_ctTRProjobpol();

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
                    string message = "Not Found Version : " + input.project_code;
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

        #region Monitior

        public string getProjectMonitor(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO019.1";
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

                DateTime fromdate = Convert.ToDateTime(req.fromdate);
                DateTime todate = Convert.ToDateTime(req.todate);

                JArray array = new JArray();

                cls_ctTRProjobcost cost_controller = new cls_ctTRProjobcost();
                cls_ctMTProject project_controller = new cls_ctMTProject();
                List<cls_MTProject> list_project = project_controller.getDataMonitiorByFillter(req.company_code, "", "", req.project_protype, req.project_probusiness, req.project_proarea, req.project_progroup, req.proresponsible_position, req.proresponsible_area);

                JObject json;

                foreach (cls_MTProject project in list_project)
                {

                    cls_ctMTProjobversion jobversion_controller = new cls_ctMTProjobversion();
                    cls_MTProjobversion jobversion = jobversion_controller.getDataCurrent(project.project_code, fromdate );

                    if (jobversion == null)
                        continue;

                    //-- Job main
                    cls_ctMTProjobmain controller = new cls_ctMTProjobmain();
                    List<cls_MTProjobmain> list_jobmain = controller.getDataByFillter(project.language, project.project_code, jobversion.version, "");

                    //-- Job shift
                    cls_ctTRProjobshift shift_controller = new cls_ctTRProjobshift();
                    List<cls_TRProjobshift> shift_list = shift_controller.getDataByFillter(project.project_code, "", jobversion.version);

                    //-- Time card
                    cls_ctTRTimecard time_controller = new cls_ctTRTimecard();
                    List<cls_TRTimecard> time_list = time_controller.getDataByFillter(project.company_code, project.project_code, "", fromdate, todate);

                    //-- Time wage
                    cls_ctTRWageday wage_controller = new cls_ctTRWageday();
                    List<cls_TRWageday> wage_list = wage_controller.getDataByFillter("EN", project.company_code, project.project_code, "", fromdate, todate, "");


                    cls_ctTRProjobemp proemp_controller = new cls_ctTRProjobemp();
                    

                    int count_emp_project = 0;
                    int count_working_project = 0;
                    int count_leave_project = 0;
                    int count_absent_project = 0;

                    int count_staff_project_regular = 0;
                    int count_staff_project_temp = 0;
                    int count_staff_project_resign = 0;


                    double sum_cost_project = 0;
                    double sum_pay_project = 0;

                    JArray array_job = new JArray();


                    foreach (cls_MTProjobmain jobmain in list_jobmain)
                    {
                        int count_emp = 0;
                        int count_working = 0;
                        int count_leave = 0;
                        int count_absent = 0;

                        int count_staff_regular = 0;
                        int count_staff_temp = 0;
                        int count_staff_resign = 0;

                        double sum_cost = 0;
                        double sum_pay = 0;
                        
                        //-- Job shift
                        int manpower = 0;                        
                        foreach (cls_TRProjobshift tmp in shift_list)
                        {
                            if (tmp.projob_code.Equals(jobmain.projobmain_code))
                            {
                                manpower += tmp.projobshift_emp;                                
                            }
                        }

                        //-- Staff
                        List<cls_TRProjobemp> proemp_list = proemp_controller.getDataByFillter(project.project_code, jobmain.projobmain_code);
                        foreach (cls_TRProjobemp tmp in proemp_list)
                        {
                            if (tmp.projobemp_todate < fromdate)
                                continue;

                            if (tmp.projobemp_fromdate > todate)
                                continue;

                            if (tmp.projobemp_todate == todate)
                                count_staff_resign++;

                            if (tmp.projobemp_type.Equals("R"))
                                count_staff_regular++;
                            else
                                count_staff_temp++;

                        }

                        //-- Working      
                        foreach (cls_TRTimecard tmp in time_list)
                        {
                            if (tmp.projob_code.Equals(jobmain.projobmain_code))
                            {
                                if (tmp.timecard_work1_min_app > 0)
                                    count_working++;

                                if (tmp.timecard_daytype.Equals("A"))
                                    count_absent++;
                            }
                        }

                        //-- Allow
                        List<cls_TRProjobcost> cost_list_max = cost_controller.getDataByFillter(jobmain.project_code, jobmain.projobmain_code, jobversion.version, project.company_code);                                                
                        foreach (cls_TRProjobcost cost in cost_list_max)
                        {
                            sum_cost += doGetAmountPerday(cost.projobcost_amount, cost.procost_type);
                        }
                        sum_cost *= manpower;

                        //-- Wage      
                        foreach (cls_TRWageday tmp in wage_list)
                        {
                            if (tmp.projob_code.Equals(jobmain.projobmain_code))
                            {
                                sum_pay += tmp.wageday_wage;
                            }
                        }


                        json = new JObject();

                        json.Add("projobmain_id", jobmain.projobmain_id);
                        json.Add("projobmain_code", jobmain.projobmain_code);
                        json.Add("projobmain_name_th", jobmain.projobmain_name_th);
                        json.Add("projobmain_name_en", jobmain.projobmain_name_en);
                        json.Add("projobmain_type", jobmain.projobmain_type);
                        json.Add("projobmain_manpower", manpower);
                        json.Add("projobmain_working", count_working);
                        json.Add("projobmain_leave", count_leave);
                        json.Add("projobmain_absent", count_absent);
                        json.Add("projobmain_cost", sum_cost);
                        json.Add("projobmain_pay", sum_pay);

                        json.Add("projobmain_staff_regular", count_staff_regular);
                        json.Add("projobmain_staff_temp", count_staff_temp);
                        json.Add("projobmain_resign", count_staff_resign);

                        array_job.Add(json);

                        //-- Summary
                        count_emp_project += manpower;
                        count_working_project += count_working;
                        count_leave_project += count_leave;
                        count_absent_project += count_absent;

                        count_staff_project_regular += count_staff_regular;
                        count_staff_project_temp += count_staff_temp;
                        count_staff_project_resign += count_staff_resign;

                        sum_cost_project += sum_cost;
                        sum_pay_project += sum_pay;
                        

                    }

                    //-- Summary by project
                    json = new JObject();
                    json.Add("project_id", project.project_id);
                    json.Add("project_code", project.project_code);
                    json.Add("project_name_th", project.project_name_th);
                    json.Add("project_name_en", project.project_name_en);
                    json.Add("project_type", project.project_protype);

                    json.Add("project_proarea", project.project_proarea);
                    json.Add("project_progroup", project.project_progroup);


                    
                    json.Add("project_business", project.project_probusiness);     
                    json.Add("project_manpower", count_emp_project);
                    json.Add("project_working", count_working_project);
                    json.Add("project_leave", count_leave_project);
                    json.Add("project_absent", count_absent_project);

                    json.Add("project_staff_regular", count_staff_project_regular);
                    json.Add("project_staff_temp", count_staff_project_temp);
                    json.Add("project_staff_resign", count_staff_project_resign);

                    json.Add("projobmain_data", array_job);
                    
                    
                    json.Add("project_cost", sum_cost_project);
                    json.Add("project_pay", sum_pay_project);
                    json.Add("root", true);

                    array.Add(json);



                } //-- Next project


                output["success"] = true;
                output["message"] = "";
                output["data"] = array;

                log.apilog_status = "200";
                log.apilog_message = "";
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

        public string getJobMonitor(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO021.1";
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

                DateTime fromdate = Convert.ToDateTime(req.fromdate);
                DateTime todate = Convert.ToDateTime(req.todate);

                JArray array = new JArray();

                cls_ctTRProjobcost cost_controller = new cls_ctTRProjobcost();
                cls_ctMTProject project_controller = new cls_ctMTProject();
                List<cls_MTProject> list_project = project_controller.getDataByFillter(req.company_code, "", "", req.project_protype, req.project_probusiness, req.project_proarea, req.project_progroup);

                JObject json;

                cls_ctMTProjobversion jobversion_controller = new cls_ctMTProjobversion();
                cls_MTProjobversion jobversion = jobversion_controller.getDataCurrent(req.project_code, fromdate);

                //-- Job main
                cls_ctMTProjobmain controller = new cls_ctMTProjobmain();
                List<cls_MTProjobmain> list_jobmain = controller.getDataByFillter(req.language,req.project_code, jobversion.version, "");

                //-- Job shift
                cls_ctTRProjobshift shift_controller = new cls_ctTRProjobshift();
                List<cls_TRProjobshift> shift_list = shift_controller.getDataByFillter(req.project_code, "", jobversion.version);

                //-- Time card
                cls_ctTRTimecard time_controller = new cls_ctTRTimecard();
                List<cls_TRTimecard> time_list;

                //-- Time wage
                cls_ctTRWageday wage_controller = new cls_ctTRWageday();
                List<cls_TRWageday> wage_list = wage_controller.getDataByFillter("EN", req.company_code, req.project_code, "", fromdate, todate, "");

                cls_ctTRProjobemp proemp_controller = new cls_ctTRProjobemp();
                

                foreach (cls_MTProjobmain jobmain in list_jobmain)
                {
                    
                    int count_working = 0;
                    int count_leave = 0;
                    int count_absent = 0;

                    int count_staff_regular = 0;
                    int count_staff_temp = 0;
                    int count_staff_resign = 0;

                    double sum_cost = 0;
                    double sum_pay = 0;

                    //-- Staff
                    List<cls_TRProjobemp> proemp_list = proemp_controller.getDataByFillter(req.project_code, jobmain.projobmain_code);
                    foreach (cls_TRProjobemp tmp in proemp_list)
                    {
                        if (tmp.projobemp_todate < fromdate)
                            continue;

                        if (tmp.projobemp_fromdate > todate)
                            continue;

                        if (tmp.projobemp_todate == todate)
                            count_staff_resign++;

                        if (tmp.projobemp_type.Equals("R"))
                            count_staff_regular++;
                        else
                            count_staff_temp++;

                    }

                    //-- Cost by job
                    List<cls_TRProjobcost> cost_list_max = cost_controller.getDataByFillter(jobmain.project_code, jobmain.projobmain_code, jobversion.version,req.company_code);
                    foreach (cls_TRProjobcost cost in cost_list_max)
                    {
                        sum_cost += doGetAmountPerday(cost.projobcost_amount, cost.procost_type);
                    }

                    //-- Job shift
                    int manpower = 0;
                    foreach (cls_TRProjobshift tmp in shift_list)
                    {
                        if (tmp.projob_code.Equals(jobmain.projobmain_code))
                        {
                            manpower += tmp.projobshift_emp;
                        }
                    }

                    //-- Working   
                    JArray array_timecard = new JArray();
                    time_list = time_controller.getDataByJob(req.company_code, jobmain.project_code, jobmain.projobmain_code,jobmain.projobsub_code, fromdate, todate);

                    foreach (cls_TRTimecard timecard in time_list)
                    {
                        #region timecard
                        if (timecard.timecard_work1_min_app > 0)
                            count_working++;

                        if (timecard.timecard_daytype.Equals("A"))
                            count_absent++;

                        json = new JObject();
                        json.Add("worker_code", timecard.worker_code);
                        json.Add("shift_code", timecard.shift_code);
                        json.Add("timecard_workdate", timecard.timecard_workdate);
                        json.Add("timecard_daytype", timecard.timecard_daytype);
                        json.Add("timecard_color", timecard.timecard_color);
                        json.Add("timecard_lock", timecard.timecard_lock);

                        json.Add("timecard_ch1", timecard.timecard_ch1);
                        json.Add("timecard_ch2", timecard.timecard_ch2);
                        json.Add("timecard_ch3", timecard.timecard_ch3);
                        json.Add("timecard_ch4", timecard.timecard_ch4);
                        json.Add("timecard_ch5", timecard.timecard_ch5);
                        json.Add("timecard_ch6", timecard.timecard_ch6);
                        json.Add("timecard_ch7", timecard.timecard_ch7);
                        json.Add("timecard_ch8", timecard.timecard_ch8);
                        json.Add("timecard_ch9", timecard.timecard_ch9);
                        json.Add("timecard_ch10", timecard.timecard_ch10);

                        //-- Time in
                        if (!timecard.timecard_ch1.ToString("HH:mm").Equals("00:00"))
                        {
                            json.Add("timecard_in", timecard.timecard_ch1.ToString("dd/MM/yyyy HH:mm"));
                        }
                        else if (!timecard.timecard_ch3.ToString("HH:mm").Equals("00:00"))
                        {
                            json.Add("timecard_in", timecard.timecard_ch3.ToString("dd/MM/yyyy HH:mm"));
                        }
                        else
                        {
                            json.Add("timecard_in", "-");
                        }

                        //-- Time out
                        if (!timecard.timecard_ch10.ToString("HH:mm").Equals("00:00"))
                        {
                            json.Add("timecard_out", timecard.timecard_ch10.ToString("dd/MM/yyyy HH:mm"));
                        }
                        else if (!timecard.timecard_ch8.ToString("HH:mm").Equals("00:00"))
                        {
                            json.Add("timecard_out", timecard.timecard_ch8.ToString("dd/MM/yyyy HH:mm"));
                        }
                        else if (!timecard.timecard_ch4.ToString("HH:mm").Equals("00:00"))
                        {
                            json.Add("timecard_out", timecard.timecard_ch4.ToString("dd/MM/yyyy HH:mm"));
                        }
                        else
                        {
                            json.Add("timecard_out", "-");
                        }


                        json.Add("timecard_before_min", timecard.timecard_before_min);
                        json.Add("timecard_work1_min", timecard.timecard_work1_min);
                        json.Add("timecard_work2_min", timecard.timecard_work2_min);
                        json.Add("timecard_break_min", timecard.timecard_break_min);
                        json.Add("timecard_after_min", timecard.timecard_after_min);
                        json.Add("timecard_late_min", timecard.timecard_late_min);

                        json.Add("timecard_before_min_app", timecard.timecard_before_min_app);
                        json.Add("timecard_work1_min_app", timecard.timecard_work1_min_app);
                        json.Add("timecard_work2_min_app", timecard.timecard_work2_min_app);
                        json.Add("timecard_break_min_app", timecard.timecard_break_min_app);
                        json.Add("timecard_after_min_app", timecard.timecard_after_min_app);
                        json.Add("timecard_late_min_app", timecard.timecard_late_min_app);

                        int hrs = (timecard.timecard_work1_min_app + timecard.timecard_work2_min_app) / 60;
                        int min = (timecard.timecard_work1_min_app + timecard.timecard_work2_min_app) - (hrs * 60);
                        json.Add("work_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));

                        hrs = (timecard.timecard_before_min_app + timecard.timecard_after_min_app) / 60;
                        min = (timecard.timecard_before_min_app + timecard.timecard_after_min_app) - (hrs * 60);
                        json.Add("ot_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));

                        hrs = (timecard.timecard_late_min_app) / 60;
                        min = (timecard.timecard_late_min_app) - (hrs * 60);
                        json.Add("late_hrs", hrs.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0'));


                        json.Add("modified_by", timecard.modified_by);
                        json.Add("modified_date", timecard.modified_date);
                        json.Add("flag", timecard.flag);

                        json.Add("worker_name_th", timecard.worker_name_th);
                        json.Add("worker_name_en", timecard.worker_name_en);
                        json.Add("projob_code", timecard.projob_code);

                        //-- Wage      
                        cls_TRWageday wage = new cls_TRWageday();
                        foreach (cls_TRWageday wageday in wage_list)
                        {
                            if (timecard.worker_code.Equals(wageday.worker_code) && timecard.timecard_workdate.Equals(wageday.wageday_date))
                            {
                                wage = wageday;
                                break;
                            }
                        }

                        json.Add("wageday_wage", wage.wageday_wage);

                        json.Add("wageday_before_amount", wage.wageday_before_amount);
                        json.Add("wageday_normal_amount", wage.wageday_normal_amount);
                        json.Add("wageday_break_amount", wage.wageday_break_amount);
                        json.Add("wageday_after_amount", wage.wageday_after_amount);
                        json.Add("ot1_amount", wage.ot1_amount);
                        json.Add("ot15_amount", wage.ot15_amount);
                        json.Add("ot2_amount", wage.ot2_amount);
                        json.Add("ot3_amount", wage.ot3_amount);
                        json.Add("late_amount", wage.late_amount);
                        json.Add("leave_amount", wage.leave_amount);
                        json.Add("absent_amount", wage.absent_amount);
                        json.Add("allowance_amount", wage.allowance_amount);

                        array_timecard.Add(json);
                        #endregion
                    }                    


                    json = new JObject();

                    json.Add("projobmain_id", jobmain.projobmain_id);
                    json.Add("projobmain_code", jobmain.projobmain_code);
                    json.Add("projobmain_name_th", jobmain.projobmain_name_th);
                    json.Add("projobmain_name_en", jobmain.projobmain_name_en);
                    json.Add("projobmain_type", jobmain.projobmain_type);
                    json.Add("projobmain_manpower", manpower);
                    json.Add("projobmain_working", count_working);
                    json.Add("projobmain_leave", count_leave);
                    json.Add("projobmain_absent", count_absent);
                    json.Add("projobmain_cost", sum_cost);
                    json.Add("projobmain_pay", sum_pay);

                    json.Add("projobmain_staff_regular", count_staff_regular);
                    json.Add("projobmain_staff_temp", count_staff_temp);
                    json.Add("projobmain_resign", count_staff_resign);

                    json.Add("timecard_data", array_timecard);

                    array.Add(json);                   

                }    

                output["success"] = true;
                output["message"] = "";
                output["data"] = array;

                log.apilog_status = "200";
                log.apilog_message = "";
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

        #region MTProarea
        public string getMTProareaList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO024.1";
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

                cls_ctMTProarea controller = new cls_ctMTProarea();
                List<cls_MTProarea> list = controller.getDataByFillter(req.company_code,"");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTProarea model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);

                        json.Add("proarea_id", model.proarea_id);
                        json.Add("proarea_code", model.proarea_code);
                        json.Add("proarea_name_th", model.proarea_name_th);
                        json.Add("proarea_name_en", model.proarea_name_en);
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
        public string doManageMTProarea(InputMTProarea input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO024.2";
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

                cls_ctMTProarea controller = new cls_ctMTProarea();
                cls_MTProarea model = new cls_MTProarea();
                model.company_code = input.company_code;

                model.proarea_id = Convert.ToInt32(input.proarea_id);
                model.proarea_code = input.proarea_code;
                model.proarea_name_th = input.proarea_name_th;
                model.proarea_name_en = input.proarea_name_en;
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
        public string doDeleteMTProarea(InputMTProarea input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO024.3";
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

                cls_ctMTProarea controller = new cls_ctMTProarea();

                if (controller.checkDataOld(input.proarea_code,input.company_code))
                {
                    bool blnResult = controller.delete(input.proarea_code, input.company_code);

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
                    string message = "Not Found Project code : " + input.proarea_code;
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
        public async Task<string> doUploadMTProarea(string token, string by, string fileName, Stream stream, string com)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO024.4";
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
                    string tmp = srv_import.doImportExcel("PROAREA", fileName, by, com);

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

        #region MTProgroup
        public string getMTProgroup(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO025.1";
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

                cls_ctMTProgroup controller = new cls_ctMTProgroup();
                List<cls_MTProgroup> list = controller.getDataByFillter(req.company_code,"");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTProgroup model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);

                        json.Add("progroup_id", model.progroup_id);
                        json.Add("progroup_code", model.progroup_code);
                        json.Add("progroup_name_th", model.progroup_name_th);
                        json.Add("progroup_name_en", model.progroup_name_en);
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
        public string doManageMTProgroup(InputMTProgroup input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO025.2";
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

                cls_ctMTProgroup controller = new cls_ctMTProgroup();
                cls_MTProgroup model = new cls_MTProgroup();
                model.company_code = input.company_code;

                model.progroup_id = Convert.ToInt32(input.progroup_id);
                model.progroup_code = input.progroup_code;
                model.progroup_name_th = input.progroup_name_th;
                model.progroup_name_en = input.progroup_name_en;
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
        public string doDeleteMTProgroup(InputMTProgroup input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO025.3";
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

                cls_ctMTProgroup controller = new cls_ctMTProgroup();

                if (controller.checkDataOld(input.progroup_code,input.company_code))
                {
                    bool blnResult = controller.delete(input.progroup_code, input.company_code);

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
                    string message = "Not Found Project code : " + input.progroup_code;
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
        public async Task<string> doUploadMTProgroup(string token, string by, string fileName, Stream stream, string com)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO025.4";
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
                    string tmp = srv_import.doImportExcel("PROGROUP", fileName, by, com);

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

        #region MTProequipmenttype
        public string getMTProequipmenttype(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO025.1";
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

                cls_ctMTProequipmenttype controller = new cls_ctMTProequipmenttype();
                List<cls_MTProequipmenttype> list = controller.getDataByFillter(req.proequipmenttype_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTProequipmenttype model in list)
                    {
                        JObject json = new JObject();

                        json.Add("proequipmenttype_id", model.proequipmenttype_id);
                        json.Add("proequipmenttype_code", model.proequipmenttype_code);
                        json.Add("proequipmenttype_name_th", model.proequipmenttype_name_th);
                        json.Add("proequipmenttype_name_en", model.proequipmenttype_name_en);
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
        public string doManageMTProequipmenttype(InputMTProequipmenttype input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO025.2";
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

                cls_ctMTProequipmenttype controller = new cls_ctMTProequipmenttype();
                cls_MTProequipmenttype model = new cls_MTProequipmenttype();

                model.proequipmenttype_id = Convert.ToInt32(input.proequipmenttype_id);
                model.proequipmenttype_code = input.proequipmenttype_code;
                model.proequipmenttype_name_th = input.proequipmenttype_name_th;
                model.proequipmenttype_name_en = input.proequipmenttype_name_en;
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
        public string doDeleteMTProequipmenttype(InputMTProequipmenttype input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO025.3";
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

                cls_ctMTProequipmenttype controller = new cls_ctMTProequipmenttype();

                if (controller.checkDataOld(input.proequipmenttype_code  ))
                {
                    bool blnResult = controller.delete(input.proequipmenttype_code );

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
                    string message = "Not Found Project code : " + input.proequipmenttype_code;
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
        public async Task<string> doUploadMTProequipmenttype(string token, string by, string fileName, Stream stream, string com)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO025.4";
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
                    string tmp = srv_import.doImportExcel("PROEQUIPMENTTYPE", fileName, by, com);

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

        #region Compare cost

        public string getCostCompare(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO026.1";
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

                DateTime fromdate = Convert.ToDateTime(req.fromdate);
                DateTime todate = Convert.ToDateTime(req.todate);

                JArray array = new JArray();

                cls_ctTRProjobcost cost_controller = new cls_ctTRProjobcost();
                cls_ctMTProject project_controller = new cls_ctMTProject();
                List<cls_MTProject> list_project = project_controller.getDataByFillter(req.company, "", "", req.project_protype, req.project_probusiness, req.project_proarea, req.project_progroup);

                JObject json;
               

                cls_ctMTProcost mtcost_controller = new cls_ctMTProcost();
                List<cls_MTProcost> list_mtcost = mtcost_controller.getDataByFillter(req.company_code, "");

                foreach (cls_MTProject project in list_project)
                {
                    cls_ctMTProjobversion jobversion_controller = new cls_ctMTProjobversion();
                    List<cls_MTProjobversion> list_jobversion = jobversion_controller.getDataByFillter(project.project_code );
 

                    JArray array_job = new JArray();

                    foreach (cls_MTProjobversion version in list_jobversion)
                    {

                        cls_ctMTProjobversion jobversioncontroller = new cls_ctMTProjobversion();
                        cls_MTProjobversion jobversion = jobversioncontroller.getDataCurrents(project.project_code, fromdate, todate);

                        if (jobversion == null)
                            continue;
                        //-- Job main
                        cls_ctMTProjobmain controller = new cls_ctMTProjobmain();
                        List<cls_MTProjobmain> list_jobmain = controller.getDataByFillter(project.language,project.project_code, version.version ,"");

                        //-- Job shift
                        cls_ctTRProjobshift shift_controller = new cls_ctTRProjobshift();
                        List<cls_TRProjobshift> shift_list = shift_controller.getDataByFillter(req.project_code, "", version.version);

                        foreach (cls_MTProjobmain jobmain in list_jobmain)
                        {

                            //-- Cost 
                            List<cls_TRProjobcost> list_cost = cost_controller.getDataByFillter(jobmain.project_code, jobmain.projobmain_code, version.version,req.company);

                            foreach (cls_MTProcost mtcost in list_mtcost)
                            {
                                //-- Clear amount
                                mtcost.procost_amount = 0;

                                foreach (cls_TRProjobcost cost in list_cost)
                                {
                                    if (mtcost.procost_code.Equals(cost.projobcost_code))
                                    {
                                        mtcost.procost_amount = cost.projobcost_amount;
                                        break;
                                    }
                                }

                            }

                            //-- Job shift
                            int manpower = 0;
                            foreach (cls_TRProjobshift tmp in shift_list)
                            {
                                if (tmp.projob_code.Equals(jobmain.projobmain_code))
                                {
                                    manpower += tmp.projobshift_emp;
                                }
                            }

                            json = new JObject();

                            json.Add("project_id", project.project_id);
                            json.Add("project_code", project.project_code);
                            json.Add("project_name_th", project.project_name_th);
                            json.Add("project_name_en", project.project_name_en);
                            json.Add("project_type", project.project_protype);
                            json.Add("project_proarea", project.project_proarea);
                            json.Add("project_progroup", project.project_progroup);

                            json.Add("version", version.version);
                            json.Add("fromdate", version.fromdate);
                            json.Add("todate", version.todate);
                            
                            json.Add("projobmain_id", jobmain.projobmain_id);
                            json.Add("projobmain_code", jobmain.projobmain_code);
                            json.Add("projobmain_name_th", jobmain.projobmain_name_th);
                            json.Add("projobmain_name_en", jobmain.projobmain_name_en);
                            json.Add("projobmain_type", jobmain.projobmain_type);

                            
                            int i = 1;
                            double sum_cost = 0;
                            foreach (cls_MTProcost mtcost in list_mtcost)
                            {
                                json.Add("allow" + i.ToString(), mtcost.procost_amount);
                                i++;

                                sum_cost += doGetAmountPerday(mtcost.procost_amount, mtcost.procost_type);
                            }

                            sum_cost *= manpower;

                            json.Add("projobmain_manpower", manpower);
                            json.Add("projobmain_cost", sum_cost);


                            array.Add(json);


                        } //-- Next job

                    } //-- Next version

                } //-- Next project


                output["success"] = true;
                output["message"] = "";
                output["data"] = array;

                log.apilog_status = "200";
                log.apilog_message = "";
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
 

        #region TRProequipmenttype
        public string getTRProequipmenttype(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO029.1";
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

                cls_ctTRProequipmentreq controller = new cls_ctTRProequipmentreq();
                List<cls_TRProequipmentreq> list = controller.getDataByFillter(req.project_code, "", "", ""  );
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRProequipmentreq model in list)
                    {
                        JObject json = new JObject();
                        json.Add("proequipmentreq_id", model.proequipmentreq_id);
                        json.Add("prouniform_code", model.prouniform_code);
                        json.Add("proequipmentreq_date", model.proequipmentreq_date);
                        json.Add("proequipmentreq_qty", model.proequipmentreq_qty);
                        json.Add("proequipmentreq_note", model.proequipmentreq_note);
                        json.Add("proequipmentreq_by", model.proequipmentreq_by);
                        json.Add("proequipmenttype_code", model.proequipmenttype_code);
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
       
        public string doManageTRProequipmentreqList(InputProjectTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO029.2";
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

                cls_ctTRProequipmentreq controller = new cls_ctTRProequipmentreq();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRProequipmentreq>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.project_code,"","" );

                if (clear)
                {
                    foreach (cls_TRProequipmentreq model in jsonArray)
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
        

        public string doDeleteTRProequipmenttype(InputTRProequipmentreq input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO029.3";
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

                cls_ctTRProequipmentreq controller = new cls_ctTRProequipmentreq();

                if (controller.checkDataOld(input.project_code, input.projob_code, input.prouniform_code, input.proequipmenttype_code,input.proequipmentreq_id))
                {
                    bool blnResult = controller.delete(input.project_code,"","" );

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
                    string message = "Not Found Employee code : " + input.project_code;
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
        public async Task<string> doUploadTRProequipmenttype(string token, string by, string fileName, Stream stream, string com)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO029.4";
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
                    string tmp = srv_import.doImportExcel("PROJECT_CONTRACT", fileName, by, com);

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


        #region Pro Image 
        public string doUploadProImages(string ref_to, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO030.1";
            log.apilog_by = "";
            log.apilog_data = "Stream";

            try
            {

                cls_ctTRProimages ct_proimages = new cls_ctTRProimages();

                string[] temp = ref_to.Split('.');

                MultipartParser parser = new MultipartParser(stream);

                if (parser.Success)
                {

                    cls_TRProimages proimages = new cls_TRProimages();
                    proimages.company_code = temp[0];
                    proimages.project_code = temp[1];
                    proimages.proimages_images = parser.FileContents;
                    proimages.modified_by = temp[2];

                    proimages.proimages_no = 1;

                    ct_proimages.insert(proimages);

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

        public string doGetProImages(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO030.2";
            log.apilog_by = "";
            log.apilog_data = "Stream";

            try
            {
                cls_ctTRProimages ct_proimages = new cls_ctTRProimages();
                List<cls_TRProimages> list_proimages = ct_proimages.getDataByFillter(req.company_code, req.project_code);

                if (list_proimages.Count > 0)
                {
                    cls_TRProimages md_image = list_proimages[0];

                    bool bln = this.IsValidImage(md_image.proimages_images);

                    output["result"] = "1";
                    output["result_text"] = "";
                    output["data"] = "data:image/png;base64," + System.Convert.ToBase64String(md_image.proimages_images);
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

        public string getImageList(FillterProject req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO030.4";
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

                cls_ctTRProimages controller = new cls_ctTRProimages();
                List<cls_TRProimages> list_proimages = controller.getDataByFillter(req.company_code, req.project_code);
                JArray array = new JArray();

                if (list_proimages.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRProimages model in list_proimages)
                    {
                        JObject json = new JObject();
                        json.Add("proimages_no", model.proimages_no);
                        json.Add("proimages_images", model.proimages_images);
                        json.Add("project_code", model.project_code);
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

        public string doManageProImage(InputTRProImage input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO030.5";
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

                cls_ctTRProimages controller = new cls_ctTRProimages();
                cls_TRProimages model = new cls_TRProimages();

                byte[] bytes = Encoding.ASCII.GetBytes(input.proimages_images);

                model.proimages_no = Convert.ToInt32(input.proimages_no);
                model.project_code = input.project_code;
                model.company_code = input.company_code;
                model.proimages_images = bytes;
                model.company_code = input.company_code;
                model.modified_by = input.modified_by;

                bool blnID = controller.insert(model);

                if (blnID)
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
        #endregion


        #region Attachfile 
        public string getTRProDocattList(FillterProject input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO031.1";
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

                cls_ctTRProDocatt controller = new cls_ctTRProDocatt();
                List<cls_TRProDocatt> list = controller.getDataByFillter(input.company_code, 0, input.project_code, input.job_type);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRProDocatt model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("project_code", model.project_code);
                        json.Add("document_id", model.document_id);
                        json.Add("job_type", model.job_type);
                        json.Add("document_name", model.document_name);
                        json.Add("document_type", model.document_type);
                        json.Add("document_path", model.document_path);

                        json.Add("created_by", model.created_by);
                        json.Add("created_date", model.created_date);
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

        public string doManageTRProDocatt(InputProjectTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO031.2";
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

                cls_ctTRProDocatt controller = new cls_ctTRProDocatt();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRProDocatt>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code, 0, input.project_code, "");

                if (clear)
                {
                    foreach (cls_TRProDocatt model in jsonArray)
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

        public string doDeleteTRProDocatt(InputProTRDocatt input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO031.3";
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

                cls_ctTRProDocatt controller = new cls_ctTRProDocatt();

                if (controller.checkDataOld(input.company_code, Convert.ToString(input.document_id), input.project_code))
                {
                    bool blnResult = controller.delete(input.company_code, Convert.ToInt32(input.document_id), input.project_code, input.job_type);

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
                    string message = "Not Found Project code : " + input.document_id;
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

        public async Task<string> doUploadProDocatt(string token, string by, string fileName, Stream stream, string com)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO031.4";
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
                    string tmp = srv_import.doImportExcel("PROJECT_DOCATT", fileName, by, com);

                    if (tmp.Equals(""))
                    {
                        output["success"] = false;
                        output["message"] = "Company incorrect";
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

        public async Task<string> doUploadMTProDocatt(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO031.4";
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

        #region  MTResponsiblepos
        public string getMTResponsiblepos(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO025.1";
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

                cls_ctMTResponsiblepos controller = new cls_ctMTResponsiblepos();
                List<cls_MTResponsiblepos> list = controller.getDataByFillter(req.company_code, "");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTResponsiblepos model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);

                        json.Add("responsiblepos_id", model.responsiblepos_id);
                        json.Add("responsiblepos_code", model.responsiblepos_code);
                        json.Add("responsiblepos_name_th", model.responsiblepos_name_th);
                        json.Add("responsiblepos_name_en", model.responsiblepos_name_en);
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
        public string doManageMTResponsiblepos(InputMTResponsiblepos input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO025.2";
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

                cls_ctMTResponsiblepos controller = new cls_ctMTResponsiblepos();
                cls_MTResponsiblepos model = new cls_MTResponsiblepos();
                model.company_code = input.company_code;

                model.responsiblepos_id = Convert.ToInt32(input.responsiblepos_id);
                model.responsiblepos_code = input.responsiblepos_code;
                model.responsiblepos_name_th = input.responsiblepos_name_th;
                model.responsiblepos_name_en = input.responsiblepos_name_en;
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
        public string doDeleteMTResponsiblepos(InputMTResponsiblepos input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO025.3";
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

                cls_ctMTResponsiblepos controller = new cls_ctMTResponsiblepos();

                if (controller.checkDataOld(input.responsiblepos_code, input.company_code))
                {
                    bool blnResult = controller.delete(input.responsiblepos_code, input.company_code);

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
                    string message = "Not Found Project code : " + input.responsiblepos_code;
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
        public async Task<string> doUploadMTResponsiblepos(string token, string by, string fileName, Stream stream, string com)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO025.4";
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
                    string tmp = srv_import.doImportExcel("RESPONSIBLEPOS", fileName, by, com);

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



        #region  MTResponsiblearea
        public string getMTResponsiblearea(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO025.1";
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

                cls_ctMTResponsiblearea controller = new cls_ctMTResponsiblearea();
                List<cls_MTResponsiblearea> list = controller.getDataByFillter(req.company_code, "");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTResponsiblearea model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);

                        json.Add("responsiblearea_id", model.responsiblearea_id);
                        json.Add("responsiblearea_code", model.responsiblearea_code);
                        json.Add("responsiblearea_name_th", model.responsiblearea_name_th);
                        json.Add("responsiblearea_name_en", model.responsiblearea_name_en);
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
        public string doManageMTResponsiblearea(InputMTResponsiblearea input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO025.2";
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

                cls_ctMTResponsiblearea controller = new cls_ctMTResponsiblearea();
                cls_MTResponsiblearea model = new cls_MTResponsiblearea();
                model.company_code = input.company_code;

                model.responsiblearea_id = Convert.ToInt32(input.responsiblearea_id);
                model.responsiblearea_code = input.responsiblearea_code;
                model.responsiblearea_name_th = input.responsiblearea_name_th;
                model.responsiblearea_name_en = input.responsiblearea_name_en;
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
        public string doDeleteMTResponsiblearea(InputMTResponsiblearea input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO025.3";
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

                cls_ctMTResponsiblearea controller = new cls_ctMTResponsiblearea();

                if (controller.checkDataOld(input.responsiblearea_code, input.company_code))
                {
                    bool blnResult = controller.delete(input.responsiblearea_code, input.company_code);

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
                    string message = "Not Found Project code : " + input.responsiblearea_code;
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
        public async Task<string> doUploadMTResponsiblearea(string token, string by, string fileName, Stream stream, string com)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRO025.4";
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
                    string tmp = srv_import.doImportExcel("RESPONSIBLEAREA", fileName, by, com);

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
    }
}
