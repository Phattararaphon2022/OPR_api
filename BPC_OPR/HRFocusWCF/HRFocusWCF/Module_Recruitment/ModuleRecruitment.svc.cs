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

using ClassLibrary_BPC.hrfocus.controller;
using ClassLibrary_BPC.hrfocus;
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




         #region Recruit Worker (REQ001)
         public string getReqWorkerList(FillterApplywork req)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ001.1";
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
                 List<cls_MTWorker> list = controller.getDataByFillter(req.company_code, req.worker_code);
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
                         json.Add("worker_gender", model.worker_gender);
                         json.Add("worker_birthdate", model.worker_birthdate);
                         json.Add("worker_hiredate", model.worker_hiredate);
                         json.Add("religion_code", model.religion_code);
                         json.Add("blood_code", model.blood_code);
                         json.Add("worker_height", model.worker_height);
                         json.Add("worker_weight", model.worker_weight);

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
         public string doManageReqWorker(InputReqWorker input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);


             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ001.2";
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
                 model.worker_gender = input.worker_gender;
                 model.worker_birthdate = Convert.ToDateTime(input.worker_birthdate);
                 model.worker_hiredate = Convert.ToDateTime(input.worker_hiredate);
                 model.religion_code = input.religion_code;
                 model.blood_code = input.blood_code;
                 model.worker_height = input.worker_height;
                 model.worker_weight = input.worker_weight;

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
         public string doDeleteReqWorker(InputReqWorker input)
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
         public async Task<string> doUploadReqworker(string token, string by, string fileName, Stream stream)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ001.4";
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
                     cls_srvReqImport srv_import = new cls_srvReqImport();
                     string tmp = srv_import.doImportExcel("REQWORKER", fileName, "TEST");

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

         #region req image (REQ002)
         public string doUploadReqImages(string ref_to, Stream stream)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ002.1";
             log.apilog_by = "";
             log.apilog_data = "Stream";

             try
             {

                 cls_ctTRReqimage ct_empimages = new cls_ctTRReqimage();

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

         public string doGetReqImages(FillterApplywork req)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ002.2";
             log.apilog_by = "";
             log.apilog_data = "Stream";

             try
             {
                 cls_ctTRReqimage ct_empimages = new cls_ctTRReqimage();
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

         #region Address (REQ003)
         public string getTRApplyAddressList(FillterApplywork input)
         {
             JObject output = new JObject();
           
             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ003.1";
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
                 List<cls_TRAddress> list = controller.getDataByFillter(input.company_code, input.worker_code);
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

         public string doManageTRApplyAddress(InputApplyTransaction input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);


             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ003.2";
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
                 var jsonArray = JsonConvert.DeserializeObject<List<cls_TRAddress>>(input.transaction_data);

                 int success = 0;
                 int error = 0;
                 StringBuilder obj_error = new StringBuilder();

                 bool clear = controller.delete(input.company_code, input.worker_code);

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

         public string doDeleteTRApplyAddress(InputTRApplyAddress input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ003.3";
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
             log.apilog_code = "REQ003.4";
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
                     cls_srvReqImport srv_import = new cls_srvReqImport();
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

         #region Card (REQ004)
         public string getTRApplyCardList(FillterApplywork input)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ004.1";
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
                         json.Add("worker_code", model.worker_code);

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
             log.apilog_code = "REQ004.2";
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

         public string doDeleteTRApplyCard(InputTRApplyCard input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ004.3";
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

         public async Task<string> doUploadApplyCard(string token, string by, string fileName, Stream stream)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ004.4";
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
                     cls_srvReqImport srv_import = new cls_srvReqImport();
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

         #region Foreigner(REQ005)
         public string getTRForeignerList(FillterApplywork input)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ005.1";
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

         public string doManageTRreqForeigner(InputTRReqForeigner input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);


             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ005.2";
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

         public string doDeleteTRreqForeigner(InputTRReqForeigner input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ005.3";
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

         public async Task<string> doUploadreqForeigner(string token, string by, string fileName, Stream stream)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ005.4";
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
                     cls_srvReqImport srv_import = new cls_srvReqImport();
                     string tmp = srv_import.doImportExcel("REQFOREIGNER", fileName, "TEST");

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

         #region Education(REQ006)
         public string getTRreqEducationList(FillterApplywork input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "REQ006.1";
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

         public string doManageTRreqEducation(InputApplyTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "REQ006.2";
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

         public string doDeleteTRreqEducation(InputTRReqEducation input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "REQ006.3";
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
            log.apilog_code = "REQ006.4";
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
                    cls_srvReqImport srv_import = new cls_srvReqImport();
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

         #region Assessment(REQ008)
         public string getTRAssessmentList(FillterApplywork input)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ008.1";
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

                 cls_ctTRReqAssessment controller = new cls_ctTRReqAssessment();
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

         public string doManageTRAssessment(InputApplyTransaction input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);


             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ008.2";
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

                 cls_ctTRReqAssessment controller = new cls_ctTRReqAssessment();

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

         public string doDeleteTRAssessment(InputTRReqAssessment input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ008.3";
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

                 cls_ctTRReqAssessment controller = new cls_ctTRReqAssessment();

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
                     string message = "Not Found Project code : " + input.reqassessment_id;
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

         public async Task<string> doUploadReqAssessment(string token, string by, string fileName, Stream stream)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ008.4";
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
                     cls_srvReqImport srv_import = new cls_srvReqImport();
                     string tmp = srv_import.doImportExcel("REQASSESSMENT", fileName, "TEST");

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

         #region TR_Criminal(REQ009)
         public string getTRCriminalList(FillterApplywork input)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ009.1";
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

                 cls_ctTRReqCriminal controller = new cls_ctTRReqCriminal();
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

         public string doManageTRCriminal(InputApplyTransaction input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);


             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ009.2";
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

                 cls_ctTRReqCriminal controller = new cls_ctTRReqCriminal();

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

         public string doDeleteTRCriminal(InputTRReqCriminal input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ009.3";
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

                 cls_ctTRReqCriminal controller = new cls_ctTRReqCriminal();

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
                     string message = "Not Found Project code : " + input.reqcriminal_id;
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

         public async Task<string> doUploadReqCriminal(string token, string by, string fileName, Stream stream)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ009.4";
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
                     cls_srvReqImport srv_import = new cls_srvReqImport();
                     string tmp = srv_import.doImportExcel("REQCRIMINAL", fileName, "TEST");

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

         #region Training(REQ007)
         public string getTRreqTrainingList(FillterApplywork input)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ007.1";
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
                 List<cls_TRTraining> list = controller.getDataByFillter(input.company_code, input.worker_code);
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

         public string doManageTRreqTraining(InputApplyTransaction input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);


             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ007.2";
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

         public string doDeleteTRreqTraining(InputTRReqTraining input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ007.3";
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
             log.apilog_code = "REQ007.4";
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
                     cls_srvReqImport srv_import = new cls_srvReqImport();
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

         #region Suggest(REQ010)
         public string getTReqsuggestList(FillterApplywork input)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ010.1";
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

                 cls_ctTRApplySuggest controller = new cls_ctTRApplySuggest();
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

         public string doManageTRReqsuggest(InputApplyTransaction input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);


             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ010.2";
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

                 cls_ctTRApplySuggest controller = new cls_ctTRApplySuggest();

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

         public string doDeleteTRreqSuggest(InputTRReqSuggest input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ010.3";
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
                     string message = "Not Found Project code : " + input.reqsuggest_id;
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

         public async Task<string> doUploadreqSuggest(string token, string by, string fileName, Stream stream)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ010.4";
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
                     cls_srvReqImport srv_import = new cls_srvReqImport();
                     string tmp = srv_import.doImportExcel("REQSUGGEST", fileName, "TEST");

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
