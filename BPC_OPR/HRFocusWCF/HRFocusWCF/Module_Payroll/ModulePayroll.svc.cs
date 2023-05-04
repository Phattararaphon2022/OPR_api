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
using ClassLibrary_BPC.hrfocus.model.System;
using ClassLibrary_BPC.hrfocus;
using ClassLibrary_BPC.hrfocus.model.SYS.System;
using ClassLibrary_BPC.hrfocus.model.Payroll;
using ClassLibrary_BPC.hrfocus.controller.Payroll;

namespace BPC_OPR
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ModuleSystem" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ModuleSystem.svc or ModuleSystem.svc.cs at the Solution Explorer and start debugging.

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]

    public class ModulePayroll : IModulePayroll
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

        #region TRTaxrate
        public string getTRTaxrateList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAYT001.1";
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

                cls_ctTRTaxrate contprovince = new cls_ctTRTaxrate();
                List<cls_TRTaxrate> list = contprovince.getDataByFillter("");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRTaxrate model in list)
                    {
                        JObject json = new JObject();
                        json.Add("taxrate_id", model.taxrate_id);
                        json.Add("taxrate_from", model.taxrate_from);
                        json.Add("taxrate_to", model.taxrate_to);
                        json.Add("taxrate_tax", model.taxrate_tax);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        json.Add("index", index);
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

                contprovince.dispose();
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
        public string doManageTRTaxrate(InputTRTaxrate input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAYT001.2";
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

                cls_ctTRTaxrate controller = new cls_ctTRTaxrate();
                cls_TRTaxrate model = new cls_TRTaxrate();


                model.company_code = input.company_code;
                model.taxrate_id = input.taxrate_id;

                //model.taxrate_id = Convert.ToInt32(input.taxrate_id);
                model.taxrate_from = input.taxrate_from;
                model.taxrate_to = input.taxrate_to;
                model.taxrate_tax = input.taxrate_tax;
                model.modified_by = input.modified_by;
                model.flag = model.flag;

                bool strID = controller.insert(model);

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
        public string doDeleteTRTaxrate(InputTRTaxrate input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAYT001.3";
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

                cls_ctTRTaxrate controller = new cls_ctTRTaxrate();

                if (controller.checkDataOld(input.company_code, input.taxrate_from, input.taxrate_to))
                {
                    bool blnResult = controller.delete(input.taxrate_id.ToString());

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
                    string message = "Not Found Project code : " + input.taxrate_id;
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
        public async Task<string> doUploadTRTaxrate(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAYT001.4";
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
                    cls_srvSystemImport srv_import = new cls_srvSystemImport();
                    string tmp = srv_import.doImportExcel("Taxrate", fileName, "TEST");

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

        #region MTItem
        public string getMTItemList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAYI001.1";
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

                cls_ctMTItem contaddresstype = new cls_ctMTItem();
                List<cls_MTItem> list = contaddresstype.getDataByFillter(req.company_code, "", req.item_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTItem model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("item_id", model.item_id);
                        json.Add("item_code", model.item_code);
                        json.Add("item_name_th", model.item_name_th);
                        json.Add("item_name_en", model.item_name_en);
                        json.Add("item_type", model.item_type);
                        json.Add("item_regular", model.item_regular);
                        json.Add("item_caltax", model.item_caltax);
                        json.Add("item_calpf", model.item_calpf);
                        json.Add("item_calsso", model.item_calsso);
                        json.Add("item_calot", model.item_calot);
                        json.Add("item_contax", model.item_contax);
                        json.Add("item_section", model.item_section);
                        json.Add("item_rate", model.item_rate);
                        json.Add("item_account", model.item_account);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);

                        json.Add("index", index);
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

                contaddresstype.dispose();
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
        public string doManageMTItem(InputMTItem input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAYI001.2";
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

                cls_ctMTItem controller = new cls_ctMTItem();
                cls_MTItem model = new cls_MTItem();

                model.item_id = Convert.ToInt32(input.item_id);
                model.company_code = input.company_code;

                //model.item_id = input.item_id;
                model.item_code = input.item_code;
                model.item_name_th = input.item_name_th;
                model.item_name_en = input.item_name_en;
                model.item_type = input.item_type;
                model.item_regular = input.item_regular;
                model.item_caltax = input.item_caltax;
                model.item_calpf = input.item_calpf;
                model.item_calsso = input.item_calsso;
                model.item_calot = input.item_calot;
                model.item_contax = input.item_contax;
                model.item_section = input.item_section;
                model.item_rate = input.item_rate;
                model.item_account = input.item_account;

                model.modified_by = input.modified_by;
                model.flag = model.flag;

                bool strID = controller.insert(model);

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
        public string doDeleteMTItem(InputMTItem input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAYI001.3";
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

                cls_ctMTItem controller = new cls_ctMTItem();

                if (controller.checkDataOld(input.company_code, input.item_code))
                {
                    bool blnResult = controller.delete(input.item_id.ToString());

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
                    string message = "Not Found Project code : " + input.item_code;
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
        public async Task<string> doUploadMTItemr(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAYI001.4";
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
                    cls_srvSystemImport srv_import = new cls_srvSystemImport();
                    string tmp = srv_import.doImportExcel("MTItemr", fileName, "TEST");

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

        #region MTProvident
        public string getMTProvidentList(BasicRequest req)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAYB001.1";
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
                cls_ctMTProvident objMTBonus = new cls_ctMTProvident();
                List<cls_MTProvident> listMTBonus = objMTBonus.getDataByFillter(req.company_code, "", "");

                JArray array = new JArray();

                if (listMTBonus.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTProvident model in listMTBonus)
                    {
                        JObject json = new JObject();

                        json.Add("provident_id", model.provident_id);
                        json.Add("provident_code", model.provident_code);
                        json.Add("provident_name_th", model.provident_name_th);
                        json.Add("provident_name_en", model.provident_name_en);

                        json.Add("company_code", model.company_code);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);

                        //json.Add("index", index);
                        array.Add(json);

                        cls_ctTRProvidentWorkage objBonusrate = new cls_ctTRProvidentWorkage();
                        List<cls_TRProvidentWorkage> listTRBonusrate = objBonusrate.getDataByFillter(model.company_code, model.provident_code);
                        JArray arrayBonusrate = new JArray();

                        if (listTRBonusrate.Count > 0)
                        {
                            int indexBonusrate = 1;

                            foreach (cls_TRProvidentWorkage modelTRBonusrate in listTRBonusrate)
                            {
                                JObject jsonBonusrate = new JObject();

                                jsonBonusrate.Add("leave_code", modelTRBonusrate.provident_code);
                                jsonBonusrate.Add("workage_from", modelTRBonusrate.workage_from);
                                jsonBonusrate.Add("workage_to", modelTRBonusrate.workage_to);
                                jsonBonusrate.Add("rate_emp", modelTRBonusrate.rate_emp);
                                jsonBonusrate.Add("rate_com", modelTRBonusrate.rate_com);
                                

                                jsonBonusrate.Add("index", indexBonusrate);
                                indexBonusrate++;

                                arrayBonusrate.Add(jsonBonusrate);
                            }
                            json.Add("providentWorkage_data", arrayBonusrate);
                        }
                        else
                        {
                            json.Add("providentWorkage_data", arrayBonusrate);
                        }
                        json.Add("index", index);

                        index++;

                        //array.Add(json);
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
        public string doManageMTProvident(InputMTProvident input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAYB001.2";
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
                cls_ctMTProvident objMTBonus = new cls_ctMTProvident();
                cls_MTProvident model = new cls_MTProvident();

                model.company_code = input.company_code;
                model.provident_id = input.provident_id.Equals("") ? 0 : Convert.ToInt32(input.provident_id);
                model.provident_code = input.provident_code;
                model.provident_name_th = input.provident_name_th;
                model.provident_name_en = input.provident_name_en;

                model.modified_by = input.modified_by;
                model.flag = input.flag;

                bool strID = objMTBonus.insert(model);
                if (!strID.Equals(""))
                {
                    try
                    {
                        cls_ctTRProvidentWorkage objBonusrate = new cls_ctTRProvidentWorkage();
                        objBonusrate.delete(input.company_code, input.provident_code);
                        if (input.providentWorkage_data.Count > 0)
                        {
                            objBonusrate.insert(input.providentWorkage_data);
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
                    log.apilog_message = objMTBonus.getMessage();
                }

                objMTBonus.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteMTProvident(InputMTProvident input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAYB001.3";
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

                cls_ctMTProvident controller = new cls_ctMTProvident();

                bool blnResult = controller.delete(input.provident_id.ToString());

                if (blnResult)
                {
                    cls_ctTRProvidentWorkage objBonusrate = new cls_ctTRProvidentWorkage();
                    objBonusrate.delete(input.company_code, input.provident_code);
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
        public async Task<string> doUploadMTProvident(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAYB001.4";
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
                    string tmp = srv_import.doImportExcel("Bonus", fileName, by);


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

        #region bonus
        public string getBonusList(BasicRequest req)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAYB001.1";
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
                cls_ctMTBonus objMTBonus = new cls_ctMTBonus();
                List<cls_MTBonus> listMTBonus = objMTBonus.getDataByFillter(req.company_code, "", req.bonus_code);

                JArray array = new JArray();

                if (listMTBonus.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTBonus model in listMTBonus)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("bonus_id", model.bonus_id);
                        json.Add("bonus_code", model.bonus_code);
                        json.Add("bonus_name_th", model.bonus_name_th);
                        json.Add("bonus_name_en", model.bonus_name_en);
                        json.Add("item_code", model.item_code);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);

                        //json.Add("index", index);
                        array.Add(json);

                        cls_ctTRBonusrate objBonusrate = new cls_ctTRBonusrate();
                        List<cls_TRBonusrate> listTRBonusrate = objBonusrate.getDataByFillter(model.company_code, model.bonus_code);
                        JArray arrayBonusrate = new JArray();

                        if (listTRBonusrate.Count > 0)
                        {
                            int indexBonusrate = 1;

                            foreach (cls_TRBonusrate modelTRBonusrate in listTRBonusrate)
                            {
                                JObject jsonBonusrate = new JObject();

                                //jsonBonusrate.Add("company_code", modelTRBonusrate.company_code);
                                jsonBonusrate.Add("bonus_code", modelTRBonusrate.bonus_code);
                                jsonBonusrate.Add("bonusrate_from", modelTRBonusrate.bonusrate_from);
                                jsonBonusrate.Add("bonusrate_to", modelTRBonusrate.bonusrate_to);
                                jsonBonusrate.Add("bonusrate_rate", modelTRBonusrate.bonusrate_rate);

                                jsonBonusrate.Add("index", indexBonusrate);
                                indexBonusrate++;

                                arrayBonusrate.Add(jsonBonusrate);
                            }
                            json.Add("bonus_data", arrayBonusrate);
                        }
                        else
                        {
                            json.Add("bonus_data", arrayBonusrate);
                        }
                        json.Add("index", index);

                        index++;

                        //array.Add(json);
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
        public string doManageBonus(InputMTBonus input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAYB001.2";
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
                cls_ctMTBonus objMTBonus = new cls_ctMTBonus();
                cls_MTBonus model = new cls_MTBonus();

                model.company_code = input.company_code;
                model.bonus_id = input.bonus_id.Equals("") ? 0 : Convert.ToInt32(input.bonus_id);
                model.bonus_code = input.bonus_code;
                model.bonus_name_th = input.bonus_name_th;
                model.bonus_name_en = input.bonus_name_en;
                model.item_code = input.item_code;
                
                model.modified_by = input.modified_by;
                model.flag = input.flag;

                bool strID = objMTBonus.insert(model);
                if (!strID.Equals(""))
                {
                    try
                    {
                        cls_ctTRBonusrate objBonusrate = new cls_ctTRBonusrate();
                        objBonusrate.delete(input.company_code, input.bonus_code);
                        if (input.bonus_data.Count > 0)
                        {
                            objBonusrate.insert(input.bonus_data);
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
                    log.apilog_message = objMTBonus.getMessage();
                }

                objMTBonus.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteBonus(InputMTBonus input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAYB001.3";
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

                cls_ctMTBonus controller = new cls_ctMTBonus();

                bool blnResult = controller.delete(input.bonus_id.ToString());

                if (blnResult)
                {
                    cls_ctTRBonusrate objBonusrate = new cls_ctTRBonusrate();
                    objBonusrate.delete(input.company_code, input.bonus_code);
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
        public async Task<string> doUploadBonus(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAYB001.4";
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
                    string tmp = srv_import.doImportExcel("Bonus", fileName, by);


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
            log.apilog_code = "PAYPE001.1";
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
                cls_ctMTPeriods controler = new cls_ctMTPeriods();
                List<cls_MTPeriods> listPeriod = controler.getDataByFillter(input.period_id, input.company_code, input.period_type, input.year_code, input.emptype_code);

                JArray array = new JArray();

                if (listPeriod.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTPeriods model in listPeriod)
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
            log.apilog_code = "PAYPE001.2";
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
                cls_ctMTPeriods controler = new cls_ctMTPeriods();
                cls_MTPeriods model = new cls_MTPeriods();

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
            log.apilog_code = "PAYPE001.3";
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

                cls_ctMTPeriods controller = new cls_ctMTPeriods();

                bool blnResult = controller.delete(input.company_code, input.period_id);

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
            log.apilog_code = "PAYPE001.4";
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











        
    }

}