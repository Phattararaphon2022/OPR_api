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
                 List<cls_MTWorker> list = controller.getDataByFillter(req.company_code, req.worker_code,req.status);

                 //-- Workflow
                 cls_ctTRWorkflow workflow = new cls_ctTRWorkflow();
                 List<cls_TRWorkflow> list_workflow = workflow.getDataByFillter(req.company_code, "", "REQ_APY");

                 //-- Approve history
                 cls_ctTRApprove approve = new cls_ctTRApprove();
                 List<cls_TRApprove> list_approve = approve.getDataByFillter(req.company_code, "REQ_APY", "");

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
                         list = new List<cls_MTWorker>();
                 }

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
                         json.Add("worker_status", model.worker_status);
                         json.Add("worker_gender", model.worker_gender);
                         json.Add("worker_birthdate", model.worker_birthdate);
                         json.Add("worker_hiredate", model.worker_hiredate);
                         json.Add("religion_code", model.religion_code);
                         json.Add("blood_code", model.blood_code);
                         json.Add("worker_height", model.worker_height);
                         json.Add("worker_weight", model.worker_weight);
                         //json.Add("worker_age", model.worker_age);

                         json.Add("worker_tel", model.worker_tel);
                         json.Add("worker_email", model.worker_email);
                         json.Add("worker_line", model.worker_line);
                         json.Add("worker_facebook", model.worker_facebook);

                         json.Add("worker_military", model.worker_military);
                         json.Add("nationality_code", model.nationality_code);

                         json.Add("worker_cardno", model.worker_cardno);
                         json.Add("worker_cardnoissuedate", model.worker_cardnoissuedate);
                         json.Add("worker_cardnoexpiredate", model.worker_cardnoexpiredate);


                         json.Add("status", model.status);

                         json.Add("modified_by", model.modified_by);
                         json.Add("modified_date", model.modified_date);

                         json.Add("initial_name_th", model.initial_name_th);
                         json.Add("initial_name_en", model.initial_name_en);

                         json.Add("flag", model.flag);

                         json.Add("checkblacklist", model.checkblacklist);
                         //json.Add("checkhistory", model.checkhistory);
                         json.Add("counthistory", model.counthistory);
                         json.Add("checkcertificate", model.checkcertificate);


                         //-- Approve
                         int count_approve = 0;
                         foreach (cls_TRApprove appr in list_approve)
                         {
                             if (model.worker_code.Equals(appr.approve_code))
                                 count_approve++;
                         }

                         json.Add("approve_status", count_approve.ToString() + "/" + list_workflow.Count.ToString());

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

                 cls_ctMTApplywork controller = new cls_ctMTApplywork();
                 var jsonArray = JsonConvert.DeserializeObject<List<cls_MTWorker>>(input.reqworker_data);
                 foreach (cls_MTWorker data in jsonArray)
                 {
                     cls_MTWorker model = new cls_MTWorker();

                     model.company_code = data.company_code;
                     model.worker_id = data.worker_id;
                     model.worker_code = data.worker_code;
                     model.worker_card = data.worker_card;
                     model.worker_initial = data.worker_initial;
                     model.worker_fname_th = data.worker_fname_th;
                     model.worker_lname_th = data.worker_lname_th;
                     model.worker_fname_en = data.worker_fname_en;
                     model.worker_lname_en = data.worker_lname_en;
                     model.worker_type = data.worker_type;
                     model.worker_status = data.worker_status;
                     model.worker_gender = data.worker_gender;
                     model.worker_birthdate = Convert.ToDateTime(data.worker_birthdate);
                     model.worker_hiredate = Convert.ToDateTime(data.worker_hiredate);
                     model.religion_code = data.religion_code;
                     model.blood_code = data.blood_code;
                     model.worker_height = data.worker_height;
                     model.worker_weight = data.worker_weight;
                     //model.worker_age = data.worker_age;

                     model.worker_tel = data.worker_tel;
                     model.worker_email = data.worker_email;
                     model.worker_line = data.worker_line;
                     model.worker_facebook = data.worker_facebook;

                     model.worker_military = data.worker_military;
                     model.nationality_code = data.nationality_code;

                     model.worker_cardno = data.worker_cardno;
                     model.worker_cardnoissuedate = data.worker_cardnoissuedate;
                     model.worker_cardnoexpiredate = data.worker_cardnoexpiredate;

                     model.status = data.status;

                     model.modified_by = data.modified_by;
                     model.flag = data.flag;
                     strID = controller.insert(model);

                     //if (!strID.Equals(""))
                     //{
                     //    if (data.reqdocatt_data.Count > 0)
                     //    {
                     //        foreach (cls_TRDocatt docatt in data.reqdocatt_data)
                     //        {
                     //            cls_ctTRDocatt objMTDoc = new cls_ctTRDocatt();
                     //            cls_TRDocatt modeldoc = new cls_TRDocatt();
                     //            modeldoc.company_code = docatt.company_code;
                     //            modeldoc.worker_code = docatt.worker_code;
                     //            modeldoc.document_id = docatt.document_id;
                     //            modeldoc.job_type = docatt.job_type;

                     //            modeldoc.document_name = docatt.document_name;
                     //            modeldoc.document_type = docatt.document_type;
                     //            modeldoc.document_path = docatt.document_path;

                     //            modeldoc.created_by = input.username;
                     //            string strIDs = objMTDoc.insert(modeldoc);
                     //        }
                     //    }
                     //}
                     //else { break; }
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
                     bool blnResult = controller.delete(input.worker_id.ToString());

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
         public string doUpdateStatusReq(InputReqWorker input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);


             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ001.5";
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

                 model.status = input.status;

                 model.modified_by = input.modified_by;
                 model.flag = model.flag;

                 string strID = controller.updatestatus(model,input.status);

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

        //Foreigner Card
         //ForeignerCard
         public string getTRForeignercardList(FillterApplywork input)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ005.5";
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

                 cls_ctTRApplyforeignercard controller = new cls_ctTRApplyforeignercard();
                 List<cls_TRForeignercard> list = controller.getDataByFillter(input.company_code, input.worker_code, "");
                 JArray array = new JArray();

                 if (list.Count > 0)
                 {
                     int index = 1;

                     foreach (cls_TRForeignercard model in list)
                     {
                         JObject json = new JObject();
                         json.Add("company_code", model.company_code);
                         json.Add("worker_code", model.worker_code);
                         json.Add("foreignercard_id", model.foreignercard_id);
                         json.Add("foreignercard_code", model.foreignercard_code);
                         json.Add("foreignercard_type", model.foreignercard_type);
                         json.Add("foreignercard_issue", model.foreignercard_issue);
                         json.Add("foreignercard_expire", model.foreignercard_expire);

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
         public string doManageTRForeignercard(InputWorkerTransaction input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);


             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ005.6";
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

                 cls_ctTRApplyforeignercard controller = new cls_ctTRApplyforeignercard();

                 JObject jsonObject = new JObject();
                 var jsonArray = JsonConvert.DeserializeObject<List<cls_TRForeignercard>>(input.transaction_data);

                 int success = 0;
                 int error = 0;
                 StringBuilder obj_error = new StringBuilder();

                 bool clear = controller.delete(input.company_code, input.worker_code, "");

                 if (clear)
                 {
                     foreach (cls_TRForeignercard model in jsonArray)
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
         public string doDeleteTRForeignercard(InputTRReqForeigner input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ005.7";
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

                 cls_ctTRApplyforeignercard controller = new cls_ctTRApplyforeignercard();

                 if (controller.checkDataOld(input.company_code, input.worker_code, input.foreignercard_id.ToString()))
                 {
                     bool blnResult = controller.delete(input.company_code, input.worker_code, input.foreignercard_id.ToString());

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
                     string message = "Not Found Project code : " + input.foreignercard_id;
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
         public async Task<string> doUploadreqForeignercard(string token, string by, string fileName, Stream stream)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ005.8";
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
                     string tmp = srv_import.doImportExcel("REQFOREIGNERCARD", fileName, by);

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

                cls_ctTRApplyeducation controller = new cls_ctTRApplyeducation();

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

                 cls_ctTRApplytraining controller = new cls_ctTRApplytraining();

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

                 cls_ctTRApplySuggest controller = new cls_ctTRApplySuggest();

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

         #region Attachflie(REQ011)
         public string getTReqDocattList(FillterApplywork input)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ011.1";
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

                 cls_ctTRDocatt controller = new cls_ctTRDocatt();
                 List<cls_TRDocatt> list = controller.getDataByFillter(input.company_code,0, input.worker_code,input.job_type);
                 JArray array = new JArray();

                 if (list.Count > 0)
                 {
                     int index = 1;

                     foreach (cls_TRDocatt model in list)
                     {
                         JObject json = new JObject();
                         json.Add("company_code", model.company_code);
                         json.Add("worker_code", model.worker_code);
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

         public string doManageTRReqDocatt(InputApplyTransaction input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);


             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ011.2";
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

                 cls_ctTRDocatt controller = new cls_ctTRDocatt();

                 JObject jsonObject = new JObject();
                 var jsonArray = JsonConvert.DeserializeObject<List<cls_TRDocatt>>(input.transaction_data);

                 int success = 0;
                 int error = 0;
                 StringBuilder obj_error = new StringBuilder();

                 bool clear = controller.delete(input.company_code,"",input.worker_code,input.job_type);

                 if (clear)
                 {
                     foreach (cls_TRDocatt model in jsonArray)
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

         public string doDeleteTRReqDocatt(InputTRDocatt input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ011.3";
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

                 cls_ctTRDocatt controller = new cls_ctTRDocatt();

                 if (controller.checkDataOld(input.company_code,Convert.ToString(input.document_id),input.worker_code))
                 {
                     bool blnResult = controller.delete(input.company_code, Convert.ToString(input.document_id), input.worker_code,input.job_type);

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

         public async Task<string> doUploadReqDocatt(string token, string by, string fileName, Stream stream)
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

         public async Task<string> doUploadMTDocatt(string token, string by, string fileName, Stream stream)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ011.4";
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

         #region Blacklist
         public string getMTBlacklistList(InputBlacklist input)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "BLK01.1";
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

                 cls_ctMTBlacklist controller = new cls_ctMTBlacklist();
                 List<cls_MTBlacklist> list = controller.getDataByFillter(input.company_code, input.worker_code,input.card_no);
                 JArray array = new JArray();

                 if (list.Count > 0)
                 {
                     int index = 1;

                     foreach (cls_MTBlacklist model in list)
                     {
                         JObject json = new JObject();
                         json.Add("company_code", model.company_code);
                         json.Add("blacklist_id", model.blacklist_id);

                         json.Add("worker_code", model.worker_code);
                         json.Add("card_no", model.card_no);

                         json.Add("blacklist_fname_th", model.blacklist_fname_th);
                         json.Add("blacklist_lname_th", model.blacklist_lname_th);
                         json.Add("blacklist_fname_en", model.blacklist_fname_en);
                         json.Add("blacklist_lname_en", model.blacklist_lname_en);

                         json.Add("reason_code", model.reason_code);
                         json.Add("blacklist_note", model.blacklist_note);

                         json.Add("worker_detail_th", model.worker_detail_th);
                         json.Add("worker_detail_en", model.worker_detail_en);

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

         public string doManageMTBlacklistList(InputBlacklist input)
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

                 cls_ctMTBlacklist controller = new cls_ctMTBlacklist();
                 cls_MTBlacklist model = new cls_MTBlacklist();

                 model.company_code = input.company_code;
                 model.blacklist_id = input.blacklist_id;
                 model.worker_code = input.worker_code;
                 model.card_no = input.card_no;
                 model.blacklist_fname_th = input.blacklist_fname_th;
                 model.blacklist_lname_th = input.blacklist_lname_th;
                 model.blacklist_fname_en = input.blacklist_fname_en;
                 model.blacklist_lname_en = input.blacklist_lname_en;
                 model.reason_code = input.reason_code;
                 model.blacklist_note = input.blacklist_note;

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

         public string doDeleteMTBlacklistList(InputBlacklist input)
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

                 cls_ctMTBlacklist controller = new cls_ctMTBlacklist();

                 if (controller.checkDataOld(input.company_code, input.card_no))
                 {
                     bool blnResult = controller.delete(input.company_code, input.card_no);

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

         public async Task<string> doUploadMTBlacklistList(string token, string by, string fileName, Stream stream)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "BLK01.4";
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
                     string tmp = srv_import.doImportExcel("BLACKLIST", fileName, by);

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
         public string doSetBatchBlacklist(InputBlacklist input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);


             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "BLK001.1";
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
                 cls_ctMTBlacklist objPo = new cls_ctMTBlacklist();
                 List<cls_MTBlacklist> listPo = new List<cls_MTBlacklist>();
                 bool strID = false;
                 foreach (cls_MTWorker modelWorker in input.emp_data)
                 {
                     cls_MTBlacklist model = new cls_MTBlacklist();
                     model.company_code = input.company_code;
                     model.worker_code = modelWorker.worker_code;
                     model.card_no = modelWorker.worker_cardno;
                     model.blacklist_fname_th = modelWorker.worker_fname_th;
                     model.blacklist_lname_th = modelWorker.worker_lname_th;
                     model.blacklist_fname_en = modelWorker.worker_fname_en;
                     model.blacklist_lname_en = modelWorker.worker_lname_en;

                     model.reason_code = input.reason_code;
                     model.blacklist_note = input.blacklist_note;
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

         #region Position (REQ012)
         public string getTRApplyPositionList(FillterApplywork input)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ012.1";
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

                 cls_ctTRReqPosition controller = new cls_ctTRReqPosition();
                 List<cls_TRPosition> list = controller.getDataByFillter(input.company_code, input.worker_code, "");
                 JArray array = new JArray();

                 if (list.Count > 0)
                 {
                     int index = 1;

                     foreach (cls_TRPosition model in list)
                     {
                         JObject json = new JObject();
                         json.Add("empposition_id", model.empposition_id);
                         json.Add("empposition_position", model.empposition_position);

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

         public string doManageTRApplyPosition(InputApplyTransaction input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);


             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ012.2";
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

                 cls_ctTRReqPosition controller = new cls_ctTRReqPosition();

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

         public string doDeleteTRApplyPosition(InputTRApplyPosition input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ012.3";
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

                 cls_ctTRReqPosition controller = new cls_ctTRReqPosition();

                 if (controller.checkDataOld(input.company_code, input.worker_code,input.empposition_id.ToString()))
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

         public async Task<string> doUploadApplyPosition(string token, string by, string fileName, Stream stream)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ012.4";
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
                     string tmp = srv_import.doImportExcel("REQPOSITION", fileName, "TEST");

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

         #region Project (REQ013)
         public string getTRApplyProjectList(FillterApplywork input)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ013.1";
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

                 cls_ctTRReqProject controller = new cls_ctTRReqProject();
                 List<cls_TRReqProject> list = controller.getDataByFillter(input.company_code, input.worker_code,"");
                 JArray array = new JArray();

                 if (list.Count > 0)
                 {
                     int index = 1;

                     foreach (cls_TRReqProject model in list)
                     {
                         JObject json = new JObject();
                         json.Add("empproject_id", model.empproject_id);
                         json.Add("project_code", model.project_code);

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

         public string doManageTRApplyProject(InputApplyTransaction input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);


             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ013.2";
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

                 cls_ctTRReqProject controller = new cls_ctTRReqProject();

                 JObject jsonObject = new JObject();
                 var jsonArray = JsonConvert.DeserializeObject<List<cls_TRReqProject>>(input.transaction_data);

                 int success = 0;
                 int error = 0;
                 StringBuilder obj_error = new StringBuilder();

                 bool clear = controller.delete(input.company_code, input.worker_code);

                 if (clear)
                 {
                     foreach (cls_TRReqProject model in jsonArray)
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

         public string doDeleteTRApplyProject(InputTRApplyProject input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ013.3";
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

                 cls_ctTRReqProject controller = new cls_ctTRReqProject();

                 if (controller.checkDataOld(input.company_code, input.worker_code,input.empproject_id.ToString()))
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
                     string message = "Not Found Project code : " + input.empproject_id;
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

         public async Task<string> doUploadApplyProject(string token, string by, string fileName, Stream stream)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ013.4";
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
                     string tmp = srv_import.doImportExcel("REQPROJECT", fileName, "TEST");

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

         #region Salary (REQ014)
         public string getTRApplySalaryList(FillterApplywork input)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ014.1";
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

                 cls_ctTRReqSalary controller = new cls_ctTRReqSalary();
                 List<cls_TRSalary> list = controller.getDataByFillter(input.company_code, input.worker_code);
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

         public string doManageTRApplySalary(InputApplyTransaction input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);


             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ014.2";
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

                 cls_ctTRReqSalary controller = new cls_ctTRReqSalary();

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

         public string doDeleteTRApplySalary(InputTRApplySalary input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ014.3";
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

                 cls_ctTRReqSalary controller = new cls_ctTRReqSalary();

                 if (controller.checkDataOld(input.company_code, input.worker_code,input.empsalary_id.ToString()))
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

         public async Task<string> doUploadApplySalary(string token, string by, string fileName, Stream stream)
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
                     string tmp = srv_import.doImportExcel("REQSALARY", fileName, "TEST");

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

         #region Benefit (REQ015)
         public string getTRApplyBenefitList(FillterApplywork input)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ015.1";
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

                 cls_ctTRReqBenefit controller = new cls_ctTRReqBenefit();
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

         public string doManageTRApplyBenefit(InputApplyTransaction input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);


             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ015.2";
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

                 cls_ctTRReqBenefit controller = new cls_ctTRReqBenefit();

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

         public string doDeleteTRApplyBenefit(InputTRApplyBenefit input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQ015.3";
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

                 cls_ctTRReqSalary controller = new cls_ctTRReqSalary();

                 if (controller.checkDataOld(input.company_code, input.worker_code, input.empbenefit_id.ToString()))
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

         public async Task<string> doUploadApplyBenefit(string token, string by, string fileName, Stream stream)
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
                     string tmp = srv_import.doImportExcel("REQBENEFIT", fileName, "TEST");

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


         #region REQ_REQUEST(REQST)
         public string getMTReqRequest(InputReqRequest input)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQST1.1";
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

                 cls_ctMTReqRequest controller = new cls_ctMTReqRequest();
                 List<cls_MTReqRequest> list = controller.getDataByFillter(input.company_code,input.request_code,input.request_status);
                 JArray array = new JArray();

                 if (list.Count > 0)
                 {
                     int index = 1;

                     foreach (cls_MTReqRequest model in list)
                     {
                         JObject json = new JObject();
                         json.Add("company_code", model.company_code);
                         json.Add("request_id", model.request_id);
                         json.Add("request_code", model.request_code);
                         json.Add("request_date", model.request_date);
                         json.Add("request_startdate", model.request_startdate);
                         json.Add("request_enddate", model.request_enddate);
                         json.Add("request_position", model.request_position);
                         json.Add("request_project", model.request_project);
                         json.Add("request_employee_type", model.request_employee_type);
                         json.Add("request_quantity", model.request_quantity);
                         json.Add("request_urgency", model.request_urgency);

                         json.Add("request_note", model.request_note);

                         json.Add("request_accepted", model.request_accepted);
                         json.Add("request_status", model.request_status);

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

         public string doManageMTReqRequestList(InputReqRequest input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);


             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQST1.2";
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

                 cls_ctMTReqRequest controller = new cls_ctMTReqRequest();
                 cls_MTReqRequest model = new cls_MTReqRequest();

                 model.company_code = input.company_code;
                 model.request_id = Convert.ToInt32(input.request_id);
                 model.request_code = input.request_code;
                 model.request_date = Convert.ToDateTime(input.request_date);
                 model.request_startdate = Convert.ToDateTime(input.request_startdate);
                 model.request_enddate = Convert.ToDateTime(input.request_enddate);
                 model.request_position = input.request_position;
                 model.request_project = input.request_project;

                 model.request_employee_type = input.request_employee_type;
                 model.request_quantity = Convert.ToDouble(input.request_quantity);
                 model.request_urgency = input.request_urgency;
                 model.request_note = input.request_note;

                 model.request_accepted = Convert.ToDouble(input.request_accepted);
                 model.request_status = input.request_status;


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

         public string doDeleteMTReqRequestList(InputReqRequest input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQST1.3";
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

                 cls_ctMTReqRequest controller = new cls_ctMTReqRequest();

                 if (controller.checkDataOld(input.request_code))
                 {
                     bool blnResult = controller.delete(input.request_code);

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

         public async Task<string> doUploadMTReqRequestList(string token, string by, string fileName, Stream stream)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "BLK01.4";
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
                     string tmp = srv_import.doImportExcel("REQUEST", fileName, by);

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

         public string getMTReqRequestPosition(InputReqRequest input)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQST2.1";
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

                 cls_ctMTReqRequest controller = new cls_ctMTReqRequest();
                 List<cls_MTReqRequest> list = controller.getPositionData(input.company_code);
                 JArray array = new JArray();

                 if (list.Count > 0)
                 {
                     int index = 1;

                     foreach (cls_MTReqRequest model in list)
                     {
                         JObject json = new JObject();
                         json.Add("company_code", model.company_code);
                         json.Add("request_id", model.request_id);
                         json.Add("request_code", model.request_code);
                         json.Add("request_position", model.request_position);

                         json.Add("modified_by", model.modified_by);
                         json.Add("modified_date", model.modified_date);

                         json.Add("position_name_th", model.position_name_th);
                         json.Add("position_name_en", model.position_name_en);

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

         public string getMTReqRequestProject(InputReqRequest input)
         {
             JObject output = new JObject();

             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQST2.2";
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

                 cls_ctMTReqRequest controller = new cls_ctMTReqRequest();
                 List<cls_MTReqRequest> list = controller.getProjectData(input.company_code);
                 JArray array = new JArray();

                 if (list.Count > 0)
                 {
                     int index = 1;

                     foreach (cls_MTReqRequest model in list)
                     {
                         JObject json = new JObject();
                         json.Add("company_code", model.company_code);
                         json.Add("request_id", model.request_id);
                         json.Add("request_code", model.request_code);
                         json.Add("request_project", model.request_project);

                         json.Add("modified_by", model.modified_by);
                         json.Add("modified_date", model.modified_date);

                         json.Add("project_name_th", model.project_name_th);
                         json.Add("project_name_en", model.project_name_en);

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

         public string doUpdateStatusRequest(InputReqRequest input)
         {
             JObject output = new JObject();

             var json_data = new JavaScriptSerializer().Serialize(input);
             var tmp = JToken.Parse(json_data);


             cls_SYSApilog log = new cls_SYSApilog();
             log.apilog_code = "REQST3.1";
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

                 cls_ctMTReqRequest controller = new cls_ctMTReqRequest();
                 cls_MTReqRequest model = new cls_MTReqRequest();

                 string strWorkerCode = input.worker_code;
                 string strComCode = input.company_code;

                 model.company_code = strComCode;
                 model.request_id = input.request_id;

                 model.request_status = input.request_status;

                 model.modified_by = input.modified_by;
                 model.flag = model.flag;

                 string strID = controller.updatestatus(model);

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
         #endregion
    }
}
