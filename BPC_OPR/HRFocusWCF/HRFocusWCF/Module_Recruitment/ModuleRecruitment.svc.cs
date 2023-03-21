﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
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

using ClassLibrary_BPC.hrfocus.model.Recruitment;
using ClassLibrary_BPC.hrfocus.controller;
namespace BPC_OPR
{

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]

    public class ModuleRecruitment : IModuleRecruitment
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




         #region Applywork
         public string getApplyworkList(FillterApplywork req)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "APW001.1";
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

                 cls_ctMTApplywork controller = new cls_ctMTApplywork();
                 List<cls_MTApplywork> list = controller.getDataByFillter(req.company_code, req.applywork_code);
                 JArray array = new JArray();

                 if (list.Count > 0)
                 {
                     int index = 1;

                     foreach (cls_MTApplywork model in list)
                     {
                         JObject json = new JObject();
                         json.Add("company_code", model.company_code);
                         json.Add("applywork_id", model.applywork_id);
                         json.Add("applywork_code", model.applywork_code);
                         json.Add("applywork_initial", model.applywork_initial);

                         json.Add("applywork_fname_th", model.applywork_fname_th);
                         json.Add("applywork_lname_th", model.applywork_lname_th);
                         json.Add("applywork_fname_en", model.applywork_fname_en);
                         json.Add("applywork_lname_en", model.applywork_lname_en);


                         json.Add("applywork_birthdate", model.applywork_birthdate);
                         json.Add("applywork_startdate", model.applywork_startdate);


                         json.Add("province_code", model.province_code);
                         json.Add("bloodtype_code", model.bloodtype_code);

                         json.Add("applywork_height", model.applywork_height);
                         json.Add("applywork_weight", model.applywork_weight);


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
         public string doManageMTApplywork(InputMTApplywork input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);


             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "APW001.2";
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

                 cls_ctMTApplywork controller = new cls_ctMTApplywork();
                 cls_MTApplywork model = new cls_MTApplywork();

                 string strApplyworkCode = input.applywork_code;
                 string strComCode = input.company_code;

                 model.company_code = strComCode;
                 model.applywork_id = input.applywork_id;
                 model.applywork_code = strApplyworkCode;
                 model.applywork_initial = input.applywork_initial;
                 model.applywork_fname_th = input.applywork_fname_th;
                 model.applywork_lname_th = input.applywork_lname_th;
                 model.applywork_fname_en = input.applywork_fname_en;
                 model.applywork_lname_en = input.applywork_lname_en;
                 model.applywork_birthdate = Convert.ToDateTime(input.applywork_birthdate);
                 model.applywork_startdate = Convert.ToDateTime(input.applywork_startdate);
                 model.province_code = input.province_code;
                 model.bloodtype_code = input.bloodtype_code;
                 model.applywork_height = input.applywork_height;
                 model.applywork_weight = input.applywork_weight;


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
         public string doDeleteMTApplywork(InputMTApplywork input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "APW001.3";
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

                 cls_ctMTApplywork controller = new cls_ctMTApplywork();

                 if (controller.checkDataOld(input.applywork_code))
                 {
                     bool blnResult = controller.delete(input.applywork_code);

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
                     string message = "Not Found Project code : " + input.applywork_code;
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
         public async Task<string> doUploadApplywork(string token, string by, string fileName, Stream stream)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "APW001.4";
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
                     string tmp = srv_import.doImportExcel("Applywork", fileName, "TEST");

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

         #region Address
         public string getTRApplyAddressList(FillterApplywork input)
         {
             JObject output = new JObject();
           
             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQADD001.1";
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

                 cls_ctTRApplyaddress controller = new cls_ctTRApplyaddress();
                 List<cls_TRApplyaddress> list = controller.getDataByFillter(input.company_code, input.applywork_code);
                 JArray array = new JArray();

                 if (list.Count > 0)
                 {
                     int index = 1;

                     foreach (cls_TRApplyaddress model in list)
                     {
                         JObject json = new JObject();
                         json.Add("company_code", model.company_code);
                         json.Add("applywork_code", model.applywork_code);
                         json.Add("applyaddress_id", model.applyaddress_id);
                         json.Add("applyaddress_type", model.applyaddress_type);
                         json.Add("applyaddress_no", model.applyaddress_no);
                         json.Add("applyaddress_moo", model.applyaddress_moo);
                         json.Add("applyaddress_soi", model.applyaddress_soi);
                         json.Add("applyaddress_road", model.applyaddress_road);
                         json.Add("applyaddress_tambon", model.applyaddress_tambon);
                         json.Add("applyaddress_amphur", model.applyaddress_amphur);
                         json.Add("applyprovince_code", model.applyprovince_code);
                         json.Add("applyaddress_zipcode", model.applyaddress_zipcode);
                         json.Add("applyaddress_tel", model.applyaddress_tel);
                         json.Add("applyaddress_email", model.applyaddress_email);
                         json.Add("applyaddress_line", model.applyaddress_line);
                         json.Add("applyaddress_facebook", model.applyaddress_facebook);
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

         public string doManageTRApplyAddress(InputApplyTransaction input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);


             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQADD001.2";
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

                 cls_ctTRApplyaddress controller = new cls_ctTRApplyaddress();

                 JObject jsonObject = new JObject();
                 var jsonArray = JsonConvert.DeserializeObject<List<cls_TRApplyaddress>>(input.transaction_data);

                 int success = 0;
                 int error = 0;
                 StringBuilder obj_error = new StringBuilder();

                 bool clear = controller.delete(input.company_code, input.applywork_code);

                 if (clear)
                 {
                     foreach (cls_TRApplyaddress model in jsonArray)
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

         public string doDeleteTRApplyAddress(InputTRApplyAddress input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQADD001.3";
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

                 cls_ctTRApplyaddress controller = new cls_ctTRApplyaddress();

                 if (controller.checkDataOld(input.company_code, input.applywork_code))
                 {
                     bool blnResult = controller.delete(input.company_code, input.applywork_code);

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
                     string message = "Not Found Project code : " + input.applyaddress_id;
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

         public async Task<string> doUploadApplyAddress(string token, string by, string fileName, Stream stream)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQADD001.4";
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
                     string tmp = srv_import.doImportExcel("REQADDRESS", fileName, "TEST");

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

         #region Card
         public string getTRApplyCardList(FillterApplywork input)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQCRD001.1";
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

                 cls_ctTRApplyCard controller = new cls_ctTRApplyCard();
                 List<cls_TRApplyCard> list = controller.getDataByFillter(input.company_code, input.applywork_code);
                 JArray array = new JArray();

                 if (list.Count > 0)
                 {
                     int index = 1;

                     foreach (cls_TRApplyCard model in list)
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

         public string doManageTRApplyCard(InputApplyTransaction input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);


             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQCRD001.2";
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

                 cls_ctTRApplyCard controller = new cls_ctTRApplyCard();

                 JObject jsonObject = new JObject();
                 var jsonArray = JsonConvert.DeserializeObject<List<cls_TRApplyCard>>(input.transaction_data);

                 int success = 0;
                 int error = 0;
                 StringBuilder obj_error = new StringBuilder();

                 bool clear = controller.delete(input.company_code, input.applywork_code);

                 if (clear)
                 {
                     foreach (cls_TRApplyCard model in jsonArray)
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

         public string doDeleteTRApplyCard(InputTRApplyCard input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQCRD001.3";
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

                 cls_ctTRApplyCard controller = new cls_ctTRApplyCard();

                 if (controller.checkDataOld(input.company_code, input.applywork_code))
                 {
                     bool blnResult = controller.delete(input.company_code, input.applywork_code);

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

         public async Task<string> doUploadApplyCard(string token, string by, string fileName, Stream stream)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQCRD001.4";
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
                     string tmp = srv_import.doImportExcel("REQCARD", fileName, "TEST");

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

         #region Foreigner
         public string getTRForeignerList(FillterApplywork input)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQFGR001.1";
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

                 cls_ctTRApplyforeigner controller = new cls_ctTRApplyforeigner();
                 List<cls_TRApplyforeigner> list = controller.getDataByFillter(input.company_code, input.applywork_code);
                 JArray array = new JArray();

                 if (list.Count > 0)
                 {
                     int index = 1;

                     foreach (cls_TRApplyforeigner model in list)
                     {
                         JObject json = new JObject();
                         json.Add("company_code", model.company_code);
                         json.Add("applywork_code", model.applywork_code);
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

         public string doManageTRreqForeigner(InputTRReqForeigner input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);


             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQFGR001.2";
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

                 cls_ctTRApplyforeigner controller = new cls_ctTRApplyforeigner();
                 cls_TRApplyforeigner model = new cls_TRApplyforeigner();

                 model.company_code = input.company_code;
                 model.applywork_code = input.applywork_code;
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

         public string doDeleteTRreqForeigner(InputTRReqForeigner input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQFGR001.3";
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

                 cls_ctTRApplyforeigner controller = new cls_ctTRApplyforeigner();

                 if (controller.checkDataOld(input.company_code, input.applywork_code, input.foreigner_id.ToString()))
                 {
                     bool blnResult = controller.delete(input.company_code, input.applywork_code, input.foreigner_id.ToString());

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

         public async Task<string> doUploadreqForeigner(string token, string by, string fileName, Stream stream)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQFGR001.4";
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
                     string tmp = srv_import.doImportExcel("REQHOSPITAL", fileName, "TEST");

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

         #region Education
         public string getTRreqEducationList(FillterApplywork input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "REQEDT001.1";
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

                cls_ctTRApplyeducation controller = new cls_ctTRApplyeducation();
                List<cls_TRApplyeducation> list = controller.getDataByFillter(input.company_code, input.applywork_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_TRApplyeducation model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("applywork_code", model.applywork_code);
                        json.Add("reqeducation_no", model.reqeducation_no);
                        json.Add("reqeducation_gpa", model.reqeducation_gpa);
                        json.Add("reqeducation_start", model.reqeducation_start);
                        json.Add("reqeducation_finish", model.reqeducation_finish);
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

         public string doManageTRreqEducation(InputApplyTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "REQEDT001.2";
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

                cls_ctTRApplyeducation controller = new cls_ctTRApplyeducation();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_TRApplyeducation>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code, input.applywork_code);

                if (clear)
                {
                    foreach (cls_TRApplyeducation model in jsonArray)
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

         public string doDeleteTRreqEducation(InputTRReqEducation input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "REQEDT001.3";
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

                if (controller.checkDataOld(input.company_code, input.applywork_code))
                {
                    bool blnResult = controller.delete(input.company_code, input.applywork_code);

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
                    string message = "Not Found Project code : " + input.reqeducation_no;
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

         public async Task<string> doUploadreqEducation(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "REQEDT001.4";
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
                    string tmp = srv_import.doImportExcel("REQEDUCATION", fileName, "TEST");

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

         #region Training
         public string getTRreqTrainingList(FillterApplywork input)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQTRN001.1";
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

                 cls_ctTRApplytraining controller = new cls_ctTRApplytraining();
                 List<cls_TRApplytraining> list = controller.getDataByFillter(input.company_code, input.applywork_code);
                 JArray array = new JArray();

                 if (list.Count > 0)
                 {
                     int index = 1;

                     foreach (cls_TRApplytraining model in list)
                     {
                         JObject json = new JObject();
                         json.Add("company_code", model.company_code);
                         json.Add("applywork_code", model.applywork_code);
                         json.Add("reqtraining_no", model.reqtraining_no);
                         json.Add("reqtraining_start", model.reqtraining_start);
                         json.Add("reqtraining_finish", model.reqtraining_finish);
                         json.Add("reqtraining_status", model.reqtraining_status);
                         json.Add("reqtraining_hours", model.reqtraining_hours);
                         json.Add("reqtraining_cost", model.reqtraining_cost);
                         json.Add("reqtraining_note", model.reqtraining_note);
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

         public string doManageTRreqTraining(InputApplyTransaction input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);


             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQTRN001.2";
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

                 cls_ctTRApplytraining controller = new cls_ctTRApplytraining();

                 JObject jsonObject = new JObject();
                 var jsonArray = JsonConvert.DeserializeObject<List<cls_TRApplytraining>>(input.transaction_data);

                 int success = 0;
                 int error = 0;
                 StringBuilder obj_error = new StringBuilder();

                 bool clear = controller.delete(input.company_code, input.applywork_code);

                 if (clear)
                 {
                     foreach (cls_TRApplytraining model in jsonArray)
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

         public string doDeleteTRreqTraining(InputTRReqTraining input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQTRN001.3";
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

                 cls_ctTRApplytraining controller = new cls_ctTRApplytraining();

                 if (controller.checkDataOld(input.company_code, input.applywork_code))
                 {
                     bool blnResult = controller.delete(input.company_code, input.applywork_code);

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
                     string message = "Not Found Project code : " + input.reqtraining_no;
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

         public async Task<string> doUploadreqTraining(string token, string by, string fileName, Stream stream)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQTRN001.4";
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
                     string tmp = srv_import.doImportExcel("REQTRAINING", fileName, "TEST");

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




    //    public void DoWork()
    //    {
    //    }
    }
}