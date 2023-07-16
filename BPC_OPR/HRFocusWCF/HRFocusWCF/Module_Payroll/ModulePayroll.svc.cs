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
            log.apilog_code = "PAY002.1";
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
            log.apilog_code = "PAY002.2";
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
            log.apilog_code = "PAY002.3";
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
            log.apilog_code = "PAY002.4";
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
            log.apilog_code = "PAY003.1";
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
                List<cls_MTItem> list = contaddresstype.getDataByFillter(req.company_code, "", req.item_code, req.item_type);
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
            log.apilog_code = "PAY003.2";
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
            log.apilog_code = "PAY003.3";
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
            log.apilog_code = "PAY003.4";
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
            log.apilog_code = "PAY004.1";
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
            log.apilog_code = "PAY004.2";
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
            log.apilog_code = "PAY004.3";
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
            log.apilog_code = "PAY004.4";
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
            log.apilog_code = "PAY005.1";
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
            log.apilog_code = "PAY005.2";
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
            log.apilog_code = "PAY005.3";
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
            log.apilog_code = "PAY005.4";
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
            log.apilog_code = "PAY001.1";
            log.apilog_by = input.username;
            log.apilog_data = "all";
            try
            {
  

                //var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                //if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                //{
                //    output["success"] = false;
                //    output["message"] = BpcOpr.MessageNotAuthen;

                //    log.apilog_status = "500";
                //    log.apilog_message = BpcOpr.MessageNotAuthen;
                //    objBpcOpr.doRecordLog(log);

                //    return output.ToString(Formatting.None);
                //}
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
            log.apilog_code = "PAY001.2";
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
            log.apilog_code = "PAY001.3";
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
            log.apilog_code = "PAY001.4";
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
        

        #region batch set bonus
        public string getBatchBonusList(InputTRList input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAY007.1";
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
                cls_ctTRBonus objMTBonus = new cls_ctTRBonus();
                List<cls_TRBonus> listMTBonus = objMTBonus.getDataByFillter("","", input.company_code, input.paypolbonus_code);

                JArray array = new JArray();

                if (listMTBonus.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRBonus model in listMTBonus)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("worker_detail", model.worker_detail);
                        json.Add("paypolbonus_code", model.paypolbonus_code);
                        json.Add("modified_by", model.created_by);
                        json.Add("modified_date", model.created_date);
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
        public string doSetBatchBonus(InputTRList input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAY007.2";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();
            {
                string company_code = input.company_code;
                string bonus_code = input.worker_code;

                //-- Transaction
                string pay_data = input.transaction_data;
                //bool blnResult = true;
                //string strMessage = "";

               try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                    cls_ctTRBonus objPol = new cls_ctTRBonus();
                    List<cls_TRBonus> listPol = new List<cls_TRBonus>();
                    bool strID = false;
                    foreach (cls_MTWorker modelWorker in input.emp_data)
                         {
                      
                    cls_TRBonus model = new cls_TRBonus();

                    model.paypolbonus_code = input.paypolbonus_code;
                    model.company_code = input.company_code;
                    model.worker_code = modelWorker.worker_code;

                    model.flag = input.flag;
                    model.created_by = input.modified_by;

                    listPol.Add(model);
                }
                if (listPol.Count > 0)
                {
                    strID = objPol.insertlist(input.company_code, input.bonus_code, listPol);


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
                    log.apilog_message = objPol.getMessage();
                }
                      
                objPol.dispose();
            }
             catch (Exception ex)
               {
                   output["result"] = "0";
                   output["result_text"] = ex.ToString();
               }


               return output.ToString(Formatting.None);
            }

        }

        public string doDeleteBatchBonus(InputTRPaypolbonus input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAY007.3";
            log.apilog_by = input.worker_code;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctTRBonus controller = new cls_ctTRBonus();

                bool blnResult = controller.delete(input.company_code, input.worker_code,input.paypolbonus_code);

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
        public async Task<string> doUploadBatchBonus(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAY007.4";
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
                    string tmp = srv_import.doImportExcel("SetBonus", fileName, by);


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

        #region batch set PolProvident
        public string getBatchPaypolprovidentList(InputTRList input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAY006.1";
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
                cls_ctTRPolProvident objPolProvident = new cls_ctTRPolProvident();
                List<cls_TRPolProvident> listPolProvident = objPolProvident.getDataByFillter("", "", input.company_code, input.paypolprovident_code);

                JArray array = new JArray();

                if (listPolProvident.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRPolProvident model in listPolProvident)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("worker_detail", model.worker_detail);
                        json.Add("paypolprovident_code", model.paypolprovident_code);
                        json.Add("modified_by", model.created_by);
                        json.Add("modified_date", model.created_date);
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
        public string doSetBatchPaypolprovident(InputTRList input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAY006.2";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();
            {
                string company_code = input.company_code;
                string provident_code = input.worker_code;
                //-- Transaction
                string pay_data = input.transaction_data;
                try
                {
                    var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                    if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                    {
                        output["success"] = false;
                        output["message"] = BpcOpr.MessageNotAuthen;

                        log.apilog_status = "500";
                        log.apilog_message = BpcOpr.MessageNotAuthen;
                        objBpcOpr.doRecordLog(log);

                        return output.ToString(Formatting.None);
                    }
                    cls_ctTRPolProvident objPol = new cls_ctTRPolProvident();
                    List<cls_TRPolProvident> listPol = new List<cls_TRPolProvident>();
                    bool strID = false;
                    foreach (cls_MTWorker modelWorkers in input.emp_data)
                    {

                        cls_TRPolProvident model = new cls_TRPolProvident();

                        model.paypolprovident_code = input.paypolprovident_code;
                        model.company_code = input.company_code;
                        model.worker_code = modelWorkers.worker_code;

                        model.flag = input.flag;
                        model.created_by = input.modified_by;

                        listPol.Add(model);
                    }
                    if (listPol.Count > 0)
                    {
                        strID = objPol.insertlist(input.company_code, input.provident_code, listPol);


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
                        log.apilog_message = objPol.getMessage();
                    }

                    objPol.dispose();
                }
                catch (Exception ex)
                {
                    output["result"] = "0";
                    output["result_text"] = ex.ToString();
                }


                return output.ToString(Formatting.None);
            }

        }

        public string doDeleteBatchPaypolprovident(InputTRList input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAY006.3";
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

                cls_ctTRPolProvident controller = new cls_ctTRPolProvident();
                bool blnResult = controller.delete(input.company_code, input.worker_code, input.paypolprovident_code);

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
        public async Task<string> doUploadBatchPaypolprovident(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAY006.4";
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
                    string tmp = srv_import.doImportExcel("SetProvident", fileName, by);


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

        #region batch set PolItem
        public string getBatchPaypolitemList(InputTRList input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAY008.1";
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
                cls_ctTRPolItem objPolItem = new cls_ctTRPolItem();
                List<cls_TRPolItem> listPolItem = objPolItem.getDataByFillter("", "", input.company_code, input.paypolitem_code);

                JArray array = new JArray();

                if (listPolItem.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRPolItem model in listPolItem)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("worker_detail", model.worker_detail);
                        json.Add("paypolitem_code", model.paypolitem_code);
                        json.Add("modified_by", model.created_by);
                        json.Add("modified_date", model.created_date);
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
        public string doSetBatchPaypolitem(InputTRList input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAY008.2";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();
            {
                string company_code = input.company_code;
                string item_code = input.worker_code;
                //-- Transaction
                string pay_data = input.transaction_data;
                try
                {
                    var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                    if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                    {
                        output["success"] = false;
                        output["message"] = BpcOpr.MessageNotAuthen;

                        log.apilog_status = "500";
                        log.apilog_message = BpcOpr.MessageNotAuthen;
                        objBpcOpr.doRecordLog(log);

                        return output.ToString(Formatting.None);
                    }
                    cls_ctTRPolItem objPol = new cls_ctTRPolItem();
                    List<cls_TRPolItem> listPol = new List<cls_TRPolItem>();
                    bool strID = false;
                    foreach (cls_MTWorker modelWorkers in input.emp_data)
                    {

                        cls_TRPolItem model = new cls_TRPolItem();

                        model.paypolitem_code = input.paypolitem_code;
                        model.company_code = input.company_code;
                        model.worker_code = modelWorkers.worker_code;

                        model.flag = input.flag;
                        model.created_by = input.modified_by;

                        listPol.Add(model);
                    }
                    if (listPol.Count > 0)
                    {
                        strID = objPol.insertlist(input.company_code, input.item_code, listPol);


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
                        log.apilog_message = objPol.getMessage();
                    }

                    objPol.dispose();
                }
                catch (Exception ex)
                {
                    output["result"] = "0";
                    output["result_text"] = ex.ToString();
                }


                return output.ToString(Formatting.None);
            }

        }

        public string doDeleteBatchPaypolitem(InputTRList input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAY008.3";
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

                cls_ctTRPolItem controller = new cls_ctTRPolItem();
                bool blnResult = controller.delete(input.company_code, input.worker_code, input.paypolitem_code);

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
        public async Task<string> doUploadBatchPaypolPaypolitem(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAY008.4";
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
                    string tmp = srv_import.doImportExcel("Set Income / Deduct", fileName, by);


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


        #region  PayItem

        public string getTRPayitemList(InputTRPayitem input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAY011.1";
            log.apilog_by = input.username;
            log.apilog_data = "all";

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["result"] = "0";
                    output["result_text"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctTRPayitem objPolItem = new cls_ctTRPayitem();
                List<cls_TRPayitem> listPolItem = objPolItem.getDataByFillter("", input.company_code, input.worker_code, input.item_code, input.item_code, Convert.ToDateTime(input.payitem_date), Convert.ToDateTime(input.payitem_date));

                JArray array = new JArray();
                if (listPolItem != null)
                {
                    if (listPolItem.Count > 0)
                    {
                        int index = 1;

                        foreach (cls_TRPayitem model in listPolItem)
                        {
                            JObject json = new JObject();

                            // Check if the properties are not null before accessing them
                            if (model != null)
                            {
                                json.Add("company_code", model.company_code);
                                json.Add("worker_code", model.worker_code);
                                json.Add("item_code", model.item_code);
                                json.Add("payitem_date", model.payitem_date);
                                json.Add("payitem_amount", model.payitem_amount);
                                json.Add("payitem_quantity", model.payitem_quantity);
                                json.Add("payitem_paytype", model.payitem_paytype);
                                json.Add("payitem_note", model.payitem_note);
                                json.Add("item_detail", model.item_detail);
                                json.Add("item_type", model.item_type);
                                json.Add("worker_detail", model.worker_detail);
                                json.Add("modified_by", model.modified_by);
                                json.Add("modified_date", model.modified_date);
                                json.Add("flag", model.flag);
                                json.Add("index", index);

                                index++;
                            }

                            array.Add(json);
                        }

                        output["result"] = "1";
                        output["result_text"] = "Data found";
                        output["data"] = array;
                    }
                    else
                    {
                        output["result"] = "0";
                        output["result_text"] = "Data not found";
                        output["data"] = array;
                    }
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "An error occurred";
                    output["data"] = array;
                }
            }
            catch (Exception e)
            {
                output["result"] = "0";
                output["result_text"] = "An error occurred";
                output["error"] = e.Message;
            }

            return output.ToString(Formatting.None);
        }

        //test
        public string doManageTRPayitemList(InputTRPayitem input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAY011.2";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();
            {
                string company_code = input.company_code;
                string item_code = input.worker_code;
                //-- Transaction
                string pay_data = input.transaction_data;
                try
                {
                    var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                    if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                    {
                        output["success"] = false;
                        output["message"] = BpcOpr.MessageNotAuthen;

                        log.apilog_status = "500";
                        log.apilog_message = BpcOpr.MessageNotAuthen;
                        objBpcOpr.doRecordLog(log);

                        return output.ToString(Formatting.None);
                    }
                    cls_ctTRPayitem objPol = new cls_ctTRPayitem();
                    List<cls_TRPayitem> listPol = new List<cls_TRPayitem>();
                    bool strID = false;
                    foreach (cls_MTWorker modelWorkers in input.emp_data)
                    {

                        cls_TRPayitem model = new cls_TRPayitem();

                        model.company_code = input.company_code;
                        model.worker_code = modelWorkers.worker_code;
                        model.item_code = input.item_code;
                        model.payitem_date = Convert.ToDateTime(input.payitem_date);
                        model.payitem_amount = input.payitem_amount;
                        model.payitem_quantity = input.payitem_quantity;
                        model.payitem_paytype = input.payitem_paytype;
                        model.payitem_note = input.payitem_note;



                        model.flag = input.flag;
                        model.created_by = input.modified_by;

                        listPol.Add(model);
                    }
                    if (listPol.Count > 0)
                    {
                        strID = objPol.insertlist(input.company_code, input.item_code, Convert.ToDateTime(input.payitem_date), listPol);


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
                        log.apilog_message = objPol.getMessage();
                    }

                    objPol.dispose();
                }
                catch (Exception ex)
                {
                    output["result"] = "0";
                    output["result_text"] = ex.ToString();
                }


                return output.ToString(Formatting.None);
            }

        }
        //test
        public string doManageTRPayitem(InputTRPayitem input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAY011.1";
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

                cls_ctTRPayitem objPayitem = new cls_ctTRPayitem();
                cls_TRPayitem model = new cls_TRPayitem();

                model.company_code = input.company_code;
                model.worker_code = input.worker_code;
                model.item_code = input.item_code;
                model.payitem_date = Convert.ToDateTime(input.payitem_date);
                model.payitem_amount = input.payitem_amount;
                model.payitem_quantity = input.payitem_quantity;
                model.payitem_paytype = input.payitem_paytype;
                model.payitem_note = input.payitem_note;


                model.modified_by = input.modified_by;
                model.flag = input.flag;
                bool blnResult = objPayitem.insert(model);
                //string strID = objShift.insert(model);
                if (!blnResult.Equals(""))
                {
                    //cls_ctTRShiftbreak objbreak = new cls_ctTRShiftbreak();
                    //cls_ctTRShiftallowance allowance = new cls_ctTRShiftallowance();
                    //bool breaks = objbreak.insert(input.company_code, input.shift_code, input.shift_break);
                    //bool allowances = allowance.insert(input.company_code, input.shift_code, input.shift_allowance);

                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = blnResult;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objPayitem.getMessage();
                }

                objPayitem.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

            }

            return output.ToString(Formatting.None);

        }              
 
        public string doDeleteTRPayitem(InputTRPayitem input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);
            

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAY011.4";
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

                cls_ctTRPayitem controller = new cls_ctTRPayitem();
                bool blnResult = controller.delete(input.company_code, input.worker_code, input.item_code, Convert.ToDateTime(input.payitem_date));

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
        public async Task<string> doUploadTRPayitem(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAY011.5";
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
                    string tmp = srv_import.doImportExcel("Set Income / Deduct", fileName, by);


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

        #region Paytran
        public string getTRPaytranList(FillterPayroll req)
        {

            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PAY012.1";
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

                cls_ctTRPaytran controller = new cls_ctTRPaytran();
                List<cls_TRPaytran> listResult = controller.getDataByFillter(req.language, req.company,datefrom, dateto, req.worker_code);
                JArray array = new JArray();

                if (listResult.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRPaytran model in listResult)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("worker_code", model.worker_code);
                        json.Add("paytran_date", model.paytran_date);

                        json.Add("paytran_ssoemp", model.paytran_ssoemp);
                        json.Add("paytran_ssocom", model.paytran_ssocom);
                        json.Add("paytran_ssorateemp", model.paytran_ssorateemp);
                        json.Add("paytran_ssoratecom", model.paytran_ssoratecom);

                        json.Add("paytran_pfemp", model.paytran_pfemp);
                        json.Add("paytran_pfcom", model.paytran_pfcom);

                        json.Add("paytran_income_401", model.paytran_income_401);
                        json.Add("paytran_deduct_401", model.paytran_deduct_401);
                        json.Add("paytran_tax_401", model.paytran_tax_401);

                        json.Add("paytran_income_4012", model.paytran_income_4012);
                        json.Add("paytran_deduct_4012", model.paytran_deduct_4012);
                        json.Add("paytran_tax_4012", model.paytran_tax_4012);

                        json.Add("paytran_income_4013", model.paytran_income_4013);
                        json.Add("paytran_deduct_4013", model.paytran_deduct_4013);
                        json.Add("paytran_tax_4013", model.paytran_tax_4013);

                        json.Add("paytran_income_402I", model.paytran_income_402I);
                        json.Add("paytran_deduct_402I", model.paytran_deduct_402I);
                        json.Add("paytran_tax_402I", model.paytran_tax_402I);

                        json.Add("paytran_income_402O", model.paytran_income_402O);
                        json.Add("paytran_deduct_402O", model.paytran_deduct_402O);
                        json.Add("paytran_tax_402O", model.paytran_tax_402O);
                        
                        json.Add("paytran_income_notax", model.paytran_income_notax);
                        json.Add("paytran_deduct_notax", model.paytran_deduct_notax);

                        json.Add("paytran_income_total", model.paytran_income_total);
                        json.Add("paytran_deduct_total", model.paytran_deduct_total);

                        json.Add("paytran_netpay_b", model.paytran_netpay_b);
                        json.Add("paytran_netpay_c", model.paytran_netpay_c);
                      
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);

                        json.Add("worker_detail", model.worker_detail);

                        json.Add("paytran_salary", model.paytran_salary);
                        json.Add("paytran_overtime", model.paytran_overtime);
                        json.Add("paytran_diligence", model.paytran_diligence);

                        json.Add("paytran_absent", model.paytran_absent);
                        json.Add("paytran_late", model.paytran_late);
                        json.Add("paytran_leave", model.paytran_leave);
                        
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

    }

}