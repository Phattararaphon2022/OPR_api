using AntsCode.Util;
using ClassLibrary_BPC.hrfocus.controller;
using ClassLibrary_BPC.hrfocus.controller.Project;
using ClassLibrary_BPC.hrfocus.model;
using ClassLibrary_BPC.hrfocus.model.Project;
using ClassLibrary_BPC.hrfocus.service;
using ClassLibrary_BPC.hrfocus.service.Payroll;
using HRFocusWCF;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
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

    public class BCInterface : IBCInterface
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

        #region APIHRJob

        public string doManageAPIHRJob(APIHRJob input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BCO001.2";
            log.apilog_by = input.PostBy;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                var TransactionId = WebOperationContext.Current.IncomingRequest.Headers["TransactionId"];
                var RequestDate = WebOperationContext.Current.IncomingRequest.Headers["RequestDate"];
                var OldTransactionId = WebOperationContext.Current.IncomingRequest.Headers["OldTransactionId"];
                
                //if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                //{
                //    output["success"] = false;
                //    output["message"] = BpcOpr.MessageNotAuthen;

                //    log.apilog_status = "500";
                //    log.apilog_message = BpcOpr.MessageNotAuthen;
                //    objBpcOpr.doRecordLog(log);

                //    return output.ToString(Formatting.None);
                //}

                //-- Step 1 Validate
                //-- Shift


                cls_ctMTProjobversion ctVersion = new cls_ctMTProjobversion();
                cls_ctMTShift ctShift = new cls_ctMTShift();

                //-- Get shift master
                List<cls_MTShift> shift_list = ctShift.getDataByFillter("", "", "", true);

                List<JobPlaningLines> JobPlaningLines = input.JobPlaningLines;
                StringBuilder sbNotfound = new StringBuilder();

                foreach (JobPlaningLines job in JobPlaningLines)
                {

                    foreach (JobTaskShift job_shift in job.JobTaskShift){

                        bool found = false;
                        foreach (cls_MTShift shift in shift_list)
                        {
                            if (job_shift.TimeIN.Equals(shift.shift_ch3) && job_shift.TimeOUT.Equals(shift.shift_ch4))
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                            sbNotfound.Append(job.JabTaskNo + " not found shift " + job_shift.TimeIN + "-" + job_shift.TimeOUT);
                    }
                }

                if (sbNotfound.Length > 0)
                {
                    output["success"] = false;
                    output["message"] = sbNotfound.ToString();                    
                    output["data"] = tmp;

                    return output.ToString(Formatting.None);                    
                }

                //-- Transaction
                cls_MTProjobversion version = ctVersion.getDataTransaction(TransactionId);
                if (version != null)
                {
                    output["success"] = false;
                    output["message"] = "Transaction id is dupilcate";
                    output["data"] = tmp;

                    return output.ToString(Formatting.None);    
                }

                //-- Step 2 Record version
                
                version = new cls_MTProjobversion();
                version.projobversion_id = 0;
                version.transaction_id = TransactionId;
                version.version = input.Version;
                version.fromdate = Convert.ToDateTime(input.StartDate);
                version.todate = Convert.ToDateTime(input.EndDate);
                version.project_code = input.JobNo;
                version.modified_by = input.PostBy;

                bool blnVersion = ctVersion.insert(version);

                //-- Step 3 Record Job




                bool blnResult = true;

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
                    //log.apilog_message = controller.getMessage();
                    log.apilog_message = "";
                }

                //controller.dispose();

            }
            catch (Exception ex)
            {
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

        public ApiResponse<APIHRProject> APIHRProjectCreate(APIHRProject input, string TransactionId)
        {
            ApiResponse<APIHRProject> response = new ApiResponse<APIHRProject>();
            response.data = new List<APIHRProject>();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BCO001.1";
            log.apilog_by = input.ModifiedBy;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    response.message = "indicates that the requested resource requires authentication.";

                    log.apilog_status = "401";
                    log.apilog_message = BpcOpr.MessageNotAuthen;

                    return response;
                }

                cls_ctMTProject controller = new cls_ctMTProject();
                cls_ctTRProaddress controlleraddres = new cls_ctTRProaddress();
                cls_ctTRProcontact controllercontact = new cls_ctTRProcontact();
                cls_MTProject model = new cls_MTProject();

                model.project_code = input.ProjectCode;
                model.project_name_th = input.ProjectNameTh;
                model.project_name_en = input.ProjectNameEn;

                model.project_name_sub = input.ProjectNameSub;
                model.project_codecentral = input.ProjectCodeCentral;
                model.project_protype = input.ProjectProType;

                model.project_proarea = input.ProjectProArea;
                model.project_progroup = input.ProGroupCode;


                model.project_probusiness = input.ProjectProBusiness;
                //
                model.project_roundtime = input.ProjectRoundTime;
                model.project_roundmoney = input.ProjectRoundMoney;
                model.project_proholiday = input.ProjectProHoliday;
                //

                model.project_status = input.ProjectStatus.ToString();
                model.company_code = input.CompanyCode;

                model.modified_by = input.ModifiedBy;

                string strID = controller.insert(model);
                if (!strID.Equals(""))
                {
                    input.ProjectId = Convert.ToInt32(strID);
                    cls_TRProaddress modeladdress = new cls_TRProaddress();
                    modeladdress.proaddress_type = "1";
                    modeladdress.proaddress_no = input.ProAddressNo;
                    modeladdress.proaddress_moo = input.ProAddressMoo;
                    modeladdress.proaddress_soi = input.ProAddressSoi;
                    modeladdress.proaddress_road = input.ProAddressRoad;
                    modeladdress.proaddress_tambon = input.ProAddressTambon;
                    modeladdress.proaddress_amphur = input.ProAddressAmphur;
                    modeladdress.proaddress_zipcode = input.ProAddressZipCode;
                    modeladdress.proaddress_tel = input.ProAddressTel;
                    modeladdress.proaddress_email = input.ProAddressEmail;
                    modeladdress.proaddress_line = input.ProAddressLine;
                    modeladdress.proaddress_facebook = input.ProAddressFacebook;
                    modeladdress.province_code = input.ProvinceCode;
                    modeladdress.project_code = input.ProjectCode;

                    modeladdress.modified_by = input.ModifiedBy;
                    bool blnResultaddres = controlleraddres.insert(modeladdress);
                    if (!blnResultaddres)
                    {
                        controller.delete(input.ProjectCode, input.CompanyCode);
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                        response.success = false;
                        response.message = "indicates that the request could not be understood by the server. | " + controlleraddres.getMessage();
                        response.data.Add(input);

                        log.apilog_status = "500";
                        log.apilog_message = controlleraddres.getMessage();
                        return response;
                    }
                    List<ProContact> dataContact = new List<ProContact>();
                    foreach (ProContact data in input.Contact)
                    {
                        data.ModifiedBy = input.ModifiedBy;
                        data.ModifiedDate = DateTime.Now.ToString("dd/MM/yyyy");
                        data.ProjectCode = input.ProjectCode;
                        dataContact.Add(data);
                        cls_TRProcontact modelcontact = new cls_TRProcontact();
                        modelcontact.procontact_ref = data.ProContactRef;
                        modelcontact.procontact_firstname_th = data.ProContactFirstNameTh;
                        modelcontact.procontact_lastname_th = data.ProContactLastNameTh;
                        modelcontact.procontact_firstname_en = data.ProContactFirstNameEn;
                        modelcontact.procontact_lastname_en = data.ProContactLastNameEn;
                        modelcontact.procontact_tel = data.ProContactTel;
                        modelcontact.procontact_email = data.ProContactEmail;
                        modelcontact.position_code = data.PositionCode;
                        modelcontact.initial_code = data.InitialCode;
                        modelcontact.project_code = input.ProjectCode;
                        modelcontact.modified_by = input.ModifiedBy;

                        bool blnResultcontact = controllercontact.insert(modelcontact);
                        if (!blnResultcontact)
                        {
                            controller.delete(input.ProjectCode, input.CompanyCode);
                            controlleraddres.delete(input.ProjectCode, "");
                            controllercontact.delete(input.ProjectCode, "");
                            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                            response.success = false;
                            response.message = "indicates that the request could not be understood by the server. | " + controllercontact.getMessage();
                            response.data.Add(input);

                            log.apilog_status = "500";
                            log.apilog_message = controllercontact.getMessage();
                            return response;
                        }
                    }
                    input.Contact = dataContact;
                    input.ModifiedDate = DateTime.Now.ToString("dd/MM/yyyy");
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Created;
                    response.success = true;
                    response.message = "indicates that the request resulted in a new resource created before the response was sent.";
                    response.data.Add(input);

                    log.apilog_status = "201";
                    log.apilog_message = "indicates that the request resulted in a new resource created before the response was sent.";
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    response.success = false;
                    response.message = "indicates that the request could not be understood by the server.";
                    response.data.Add(input);

                    log.apilog_status = "400";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.ToString();
                log.apilog_message = ex.ToString();
                log.apilog_status = "500";
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return response;
        }

        public ApiResponse<APIHRProject> APIHRProjectUpdate(APIHRProject input, string TransactionId, string OldTransactionId)
        {
            ApiResponse<APIHRProject> response = new ApiResponse<APIHRProject>();
            response.data = new List<APIHRProject>();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BCO001.2";
            log.apilog_by = input.ModifiedBy;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    response.message = "indicates that the requested resource requires authentication.";

                    log.apilog_status = "401";
                    log.apilog_message = BpcOpr.MessageNotAuthen;

                    return response;
                }

                cls_ctMTProject controller = new cls_ctMTProject();
                cls_ctTRProaddress controlleraddres = new cls_ctTRProaddress();
                cls_ctTRProcontact controllercontact = new cls_ctTRProcontact();
                cls_MTProject model = new cls_MTProject();

                model.project_code = input.ProjectCode;
                model.project_name_th = input.ProjectNameTh;
                model.project_name_en = input.ProjectNameEn;

                model.project_name_sub = input.ProjectNameSub;
                model.project_codecentral = input.ProjectCodeCentral;
                model.project_protype = input.ProjectProType;

                model.project_proarea = input.ProjectProArea;
                model.project_progroup = input.ProGroupCode;


                model.project_probusiness = input.ProjectProBusiness;
                //
                model.project_roundtime = input.ProjectRoundTime;
                model.project_roundmoney = input.ProjectRoundMoney;
                model.project_proholiday = input.ProjectProHoliday;
                //

                model.project_status = input.ProjectStatus.ToString();
                model.company_code = input.CompanyCode;

                model.modified_by = input.ModifiedBy;
                bool strID = false;
                if (controller.checkDataOld(input.ProjectCode, input.CompanyCode, input.ProjectId.ToString()))
                {
                    strID = controller.update(model);
                  }
                if (strID)
                {
                    cls_TRProaddress modeladdress = new cls_TRProaddress();
                    modeladdress.proaddress_type = "1";
                    modeladdress.proaddress_no = input.ProAddressNo;
                    modeladdress.proaddress_moo = input.ProAddressMoo;
                    modeladdress.proaddress_soi = input.ProAddressSoi;
                    modeladdress.proaddress_road = input.ProAddressRoad;
                    modeladdress.proaddress_tambon = input.ProAddressTambon;
                    modeladdress.proaddress_amphur = input.ProAddressAmphur;
                    modeladdress.proaddress_zipcode = input.ProAddressZipCode;
                    modeladdress.proaddress_tel = input.ProAddressTel;
                    modeladdress.proaddress_email = input.ProAddressEmail;
                    modeladdress.proaddress_line = input.ProAddressLine;
                    modeladdress.proaddress_facebook = input.ProAddressFacebook;
                    modeladdress.province_code = input.ProvinceCode;
                    modeladdress.project_code = input.ProjectCode;

                    modeladdress.modified_by = input.ModifiedBy;
                    bool blnResultaddres = false;
                    if (controlleraddres.checkDataOld(input.ProjectCode, modeladdress.proaddress_type))
                    {
                        blnResultaddres = controlleraddres.update(modeladdress);
                    }
                    if (!blnResultaddres)
                    {
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                        response.success = false;
                        response.message = "indicates that the request could not be understood by the server. | " + controlleraddres.getMessage();
                        response.data.Add(input);

                        log.apilog_status = "400";
                        log.apilog_message = controlleraddres.getMessage();
                        return response;
                    }
                    List<ProContact> dataContact = new List<ProContact>();
                    foreach (ProContact data in input.Contact)
                    {
                        data.ModifiedBy = input.ModifiedBy;
                        data.ModifiedDate = DateTime.Now.ToString("dd/MM/yyyy");
                        data.ProjectCode = input.ProjectCode;
                        dataContact.Add(data);
                        cls_TRProcontact modelcontact = new cls_TRProcontact();
                        modelcontact.procontact_id = data.ProContactId;
                        modelcontact.procontact_ref = data.ProContactRef;
                        modelcontact.procontact_firstname_th = data.ProContactFirstNameTh;
                        modelcontact.procontact_lastname_th = data.ProContactLastNameTh;
                        modelcontact.procontact_firstname_en = data.ProContactFirstNameEn;
                        modelcontact.procontact_lastname_en = data.ProContactLastNameEn;
                        modelcontact.procontact_tel = data.ProContactTel;
                        modelcontact.procontact_email = data.ProContactEmail;
                        modelcontact.position_code = data.PositionCode;
                        modelcontact.initial_code = data.InitialCode;
                        modelcontact.project_code = input.ProjectCode;
                        modelcontact.modified_by = input.ModifiedBy;
                        bool blnResultcontact = false;
                        if (controllercontact.checkDataOld(input.ProjectCode, ""))
                        {
                            blnResultcontact = controllercontact.update(modelcontact);
                        }
                        if (!blnResultcontact)
                        {
                            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                            response.success = false;
                            response.message = "indicates that the request could not be understood by the server. | " + controllercontact.getMessage();
                            response.data.Add(input);

                            log.apilog_status = "400";
                            log.apilog_message = controllercontact.getMessage();
                            return response;
                        }
                    }
                    input.Contact = dataContact;
                    input.ModifiedDate = DateTime.Now.ToString("dd/MM/yyyy");
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                    response.success = true;
                    response.message = "indicates that the request succeeded and that the requested information is in the response.";
                    response.data.Add(input);

                    log.apilog_status = "200";
                    log.apilog_message = "indicates that the request succeeded and that the requested information is in the response.";
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    response.success = false;
                    response.message = "indicates that the request could not be understood by the server.";
                    response.data.Add(input);

                    log.apilog_status = "400";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

            }
            catch (Exception ex)
            {
                log.apilog_message = ex.ToString();
                log.apilog_status = "500";
                response.success = false;
                response.message = ex.ToString();
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return response;
        }

        public ApiResponse<APIHRProject> APIHRProjectList(string CompanyCode, string ProjectCode, string ProjectStatus)
        {
            ApiResponse<APIHRProject> response = new ApiResponse<APIHRProject>();
            response.data = new List<APIHRProject>();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BCO001.3";
            log.apilog_data = "all";
            log.apilog_by = "";

            try
            {
                string url = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.ToString();
                log.apilog_data = url;
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    response.message = "indicates that the requested resource requires authentication.";

                    log.apilog_status = "401";
                    log.apilog_message = BpcOpr.MessageNotAuthen;

                    return response;
                }
                Authen objAuthen = new Authen();
                string tmp = authHeader.Substring(7);
                var handler = new JwtSecurityTokenHandler();
                var decodedValue = handler.ReadJwtToken(tmp);
                var usr = decodedValue.Claims.Single(claim => claim.Type == "user_aabbcc");
                log.apilog_by = usr.Value;

                cls_ctMTProject controller = new cls_ctMTProject();
                List<cls_MTProject> list = controller.getDataByFillter(CompanyCode == null ? "" : CompanyCode, ProjectCode == null ? "" : ProjectCode, "", "", "", "", "", ProjectStatus == null ? "" : ProjectStatus);

                JArray array = new JArray();

                if (list.Count > 0)
                {
                    foreach (cls_MTProject data in list)
                    {
                        APIHRProject apihrproject = new APIHRProject();
                        apihrproject.ProjectId = data.project_id;
                        apihrproject.CompanyCode = data.company_code;
                        apihrproject.ProjectCode = data.project_code;
                        apihrproject.ProjectNameTh = data.project_name_th;
                        apihrproject.ProjectNameEn = data.project_name_en;
                        apihrproject.ProjectNameSub = data.project_name_sub;
                        apihrproject.ProjectCodeCentral = data.project_codecentral;
                        apihrproject.ProjectProType = data.project_protype;
                        apihrproject.ProjectProArea = data.project_proarea;
                        apihrproject.ProjectProGroup = data.project_progroup;
                        apihrproject.ProjectProBusiness = data.project_probusiness;
                        apihrproject.ProjectRoundTime = data.project_roundtime;
                        apihrproject.ProjectRoundMoney = data.project_roundmoney;
                        apihrproject.ProjectProHoliday = data.project_proholiday;
                        apihrproject.ProjectStatus = Convert.ToChar(data.project_status);
                        apihrproject.ProGroupCode = "";
                        apihrproject.ModifiedBy = data.modified_by;
                        apihrproject.ModifiedDate = data.modified_date.ToString("dd/MM/yy");
                        // ProAddress
                        cls_ctTRProaddress controlleraddres = new cls_ctTRProaddress();
                        List<cls_TRProaddress> listaddres = controlleraddres.getDataByFillter(data.project_code);
                        if (listaddres.Count > 0)
                        {
                            cls_TRProaddress dataaddres = listaddres[0];
                            apihrproject.ProAddressId = dataaddres.proaddress_id;
                            apihrproject.ProAddressType = Convert.ToChar(dataaddres.proaddress_type);
                            apihrproject.ProAddressNo = dataaddres.proaddress_no;
                            apihrproject.ProAddressMoo = dataaddres.proaddress_moo;
                            apihrproject.ProAddressSoi = dataaddres.proaddress_soi;
                            apihrproject.ProAddressRoad = dataaddres.proaddress_road;
                            apihrproject.ProAddressTambon = dataaddres.proaddress_tambon;
                            apihrproject.ProAddressAmphur = dataaddres.proaddress_amphur;
                            apihrproject.ProvinceCode = dataaddres.province_code;
                            apihrproject.ProAddressZipCode = dataaddres.proaddress_zipcode;
                            apihrproject.ProAddressTel = dataaddres.proaddress_tel;
                            apihrproject.ProAddressEmail = dataaddres.proaddress_email;
                            apihrproject.ProAddressLine = dataaddres.proaddress_line;
                            apihrproject.ProAddressFacebook = dataaddres.proaddress_facebook;
                            apihrproject.ProjectCode = dataaddres.project_code;
                            apihrproject.ModifiedBy = dataaddres.modified_by;
                            apihrproject.ModifiedDate = dataaddres.modified_date.ToString("dd/MM/yyyy");

                        }
                        // ProContact

                        apihrproject.Contact = new List<ProContact>();
                        cls_ctTRProcontact controllercontact = new cls_ctTRProcontact();
                        List<cls_TRProcontact> listcontact = controllercontact.getDataByFillter(data.project_code);
                        if (listcontact.Count > 0)
                        {
                            foreach (cls_TRProcontact datacontact in listcontact)
                            {
                                ProContact modelcontact = new ProContact();
                                modelcontact.ProContactId = datacontact.procontact_id;
                                modelcontact.ProContactRef = datacontact.procontact_ref;
                                modelcontact.ProContactFirstNameTh = datacontact.procontact_firstname_th;
                                modelcontact.ProContactLastNameTh = datacontact.procontact_lastname_th;
                                modelcontact.ProContactFirstNameEn = datacontact.procontact_firstname_en;
                                modelcontact.ProContactLastNameEn = datacontact.procontact_lastname_en;
                                modelcontact.ProContactTel = datacontact.procontact_tel;
                                modelcontact.ProContactEmail = datacontact.procontact_email;
                                modelcontact.PositionCode = datacontact.position_code;
                                modelcontact.InitialCode = datacontact.initial_code;
                                modelcontact.ProjectCode = datacontact.project_code;
                                modelcontact.ModifiedBy = datacontact.modified_by;
                                modelcontact.ModifiedDate = datacontact.modified_date.ToString("dd/MM/yyyy");
                                apihrproject.Contact.Add(modelcontact);
                            }
                        }
                        response.data.Add(apihrproject);
                    }

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                response.success = true;
                response.message = "indicates that the request succeeded and that the requested information is in the response.";
                log.apilog_status = "200";
                log.apilog_message = "indicates that the request succeeded and that the requested information is in the response.";
                controller.dispose();
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.ToString();
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return response;
        }

        public ApiResponse<APIHRProject> APIHRProjectDelete(string CompanyCode, string ProjectCode)
        {
            ApiResponse<APIHRProject> response = new ApiResponse<APIHRProject>();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BCO001.4";
            log.apilog_by = "";

            try
            {
                string url = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.ToString();
                log.apilog_data = url;
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    response.message = "indicates that the requested resource requires authentication.";

                    log.apilog_status = "401";
                    log.apilog_message = BpcOpr.MessageNotAuthen;

                    return response;
                }
                Authen objAuthen = new Authen();
                string tmp = authHeader.Substring(7);
                var handler = new JwtSecurityTokenHandler();
                var decodedValue = handler.ReadJwtToken(tmp);
                var usr = decodedValue.Claims.Single(claim => claim.Type == "user_aabbcc");
                log.apilog_by = usr.Value;
                cls_ctMTProject controller = new cls_ctMTProject();
                cls_ctTRProaddress controlleraddres = new cls_ctTRProaddress();
                cls_ctTRProcontact controllercontact = new cls_ctTRProcontact();
                bool blnResult = controller.delete(ProjectCode,CompanyCode);

                if (blnResult)
                {
                    controlleraddres.delete(ProjectCode, "");
                    controllercontact.delete(ProjectCode, "");
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                    response.success = true;
                    response.message = "indicates that the request succeeded and that the requested information is in the response.";
                    log.apilog_status = "200";
                    log.apilog_message = "indicates that the request succeeded and that the requested information is in the response.";
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    response.success = false;
                    response.message = "indicates that the request could not be understood by the server.";
                    log.apilog_status = "400";
                    log.apilog_message = "indicates that the request could not be understood by the server.";
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                log.apilog_message = ex.ToString();
                response.success = false;
                response.message = ex.ToString();
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return response;

        }


        public ApiResponse<ProContract> APIHRProjectContractCreate(ProContract input, string TransactionId)
        {
            ApiResponse<ProContract> response = new ApiResponse<ProContract>();
            response.data = new List<ProContract>();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BCO002.1";
            log.apilog_by = input.ModifiedBy;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    response.message = "indicates that the requested resource requires authentication.";

                    log.apilog_status = "401";
                    log.apilog_message = BpcOpr.MessageNotAuthen;

                    return response;
                }

                cls_ctTRProcontract controller = new cls_ctTRProcontract();
                cls_TRProcontract model = new cls_TRProcontract();

                model.project_code = input.ProjectCode;
                model.procontract_ref = input.ProContractRef;
                if (!input.ProContractDate.Equals(""))
                {
                    model.procontract_date = Convert.ToDateTime(input.ProContractDate);
                }
                model.procontract_amount = input.ProContractAmount;
                model.procontract_fromdate = Convert.ToDateTime(input.ProContractFromDate);
                model.procontract_todate = Convert.ToDateTime(input.ProContractToDate);
                model.procontract_customer = input.ProContractCustomer;
                model.procontract_bidder = input.ProContractBidder;
                model.procontract_type = input.ProContractType;
                model.procontract_type = input.ProContractType;

                model.modified_by = input.ModifiedBy;

                bool strID = controller.insert(model);
                if (strID)
                {
                    input.ModifiedDate = DateTime.Now.ToString("dd/MM/yyyy");
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Created;
                    response.success = true;
                    response.message = "indicates that the request resulted in a new resource created before the response was sent.";
                    response.data.Add(input);

                    log.apilog_status = "201";
                    log.apilog_message = "indicates that the request resulted in a new resource created before the response was sent.";
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    response.success = false;
                    response.message = "indicates that the request could not be understood by the server.";
                    response.data.Add(input);

                    log.apilog_status = "400";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.ToString();
                log.apilog_message = ex.ToString();
                log.apilog_status = "500";
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return response;
        }

        public ApiResponse<ProContract> APIHRProjectContractUpdate(ProContract input ,string TransactionId, string OldTransactionId)
        {
            ApiResponse<ProContract> response = new ApiResponse<ProContract>();
            response.data = new List<ProContract>();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BCO002.2";
            log.apilog_by = input.ModifiedBy;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    response.message = "indicates that the requested resource requires authentication.";

                    log.apilog_status = "401";
                    log.apilog_message = BpcOpr.MessageNotAuthen;

                    return response;
                }
                cls_ctTRProcontract controller = new cls_ctTRProcontract();
                cls_TRProcontract model = new cls_TRProcontract();

                model.procontract_id = input.ProContractId;
                model.project_code = input.ProjectCode;
                model.procontract_ref = input.ProContractRef;
                if (!input.ProContractDate.Equals(""))
                {
                    model.procontract_date = Convert.ToDateTime(input.ProContractDate);
                }
                model.procontract_amount = input.ProContractAmount;
                model.procontract_fromdate = Convert.ToDateTime(input.ProContractFromDate);
                model.procontract_todate = Convert.ToDateTime(input.ProContractToDate);
                model.procontract_customer = input.ProContractCustomer;
                model.procontract_bidder = input.ProContractBidder;
                model.procontract_type = input.ProContractType;
                model.procontract_type = input.ProContractType;

                model.modified_by = input.ModifiedBy;
                bool strID = false;
                if (controller.checkDataOld(input.ProjectCode,""))
                {
                    if (model.procontract_id.Equals(0))
                    {
                        strID = false;
                    }
                    else
                    {
                        strID = controller.update(model);
                    }
                }
                if (strID)
                {
                    input.ModifiedDate = DateTime.Now.ToString("dd/MM/yyyy");
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                    response.success = true;
                    response.message = "indicates that the request succeeded and that the requested information is in the response.";
                    response.data.Add(input);

                    log.apilog_status = "200";
                    log.apilog_message = "indicates that the request succeeded and that the requested information is in the response.";
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    response.success = false;
                    response.message = "indicates that the request could not be understood by the server.";
                    response.data.Add(input);

                    log.apilog_status = "400";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

            }
            catch (Exception ex)
            {
                log.apilog_message = ex.ToString();
                log.apilog_status = "500";
                response.success = false;
                response.message = ex.ToString();
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return response;
        }

        public ApiResponse<ProContract> APIHRProjectContractList(string ProjectCode)
        {
            ApiResponse<ProContract> response = new ApiResponse<ProContract>();
            response.data = new List<ProContract>();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BCO002.3";
            log.apilog_data = "all";
            log.apilog_by = "";

            try
            {
                string url = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.ToString();
                log.apilog_data = url;
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    response.message = "indicates that the requested resource requires authentication.";

                    log.apilog_status = "401";
                    log.apilog_message = BpcOpr.MessageNotAuthen;

                    return response;
                }
                Authen objAuthen = new Authen();
                string tmp = authHeader.Substring(7);
                var handler = new JwtSecurityTokenHandler();
                var decodedValue = handler.ReadJwtToken(tmp);
                var usr = decodedValue.Claims.Single(claim => claim.Type == "user_aabbcc");
                log.apilog_by = usr.Value;

                cls_ctTRProcontract controller = new cls_ctTRProcontract();
                List<cls_TRProcontract> list = controller.getDataByFillter(ProjectCode == null ? "" : ProjectCode);

                JArray array = new JArray();

                if (list.Count > 0)
                {
                    foreach (cls_TRProcontract data in list)
                    {
                        ProContract ProContract = new ProContract();
                        ProContract.ProContractId = data.procontract_id;
                        ProContract.ProContractRef = data.procontract_ref;
                        ProContract.ProContractDate = data.procontract_date.ToString("dd/MM/yyyy");
                        ProContract.ProContractAmount = data.procontract_amount;
                        ProContract.ProContractFromDate = data.procontract_fromdate.ToString("dd/MM/yyyy");
                        ProContract.ProContractToDate = data.procontract_todate.ToString("dd/MM/yyyy");
                        ProContract.ProContractCustomer = data.procontract_customer;
                        ProContract.ProContractBidder = data.procontract_bidder;
                        ProContract.ProjectCode = data.project_code;
                        ProContract.ProContractType = data.procontract_type;
                        ProContract.ModifiedBy = data.modified_by;
                        ProContract.ModifiedDate = data.modified_date.ToString("dd/MM/yyyy");

                        response.data.Add(ProContract);
                    }

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                response.success = true;
                response.message = "indicates that the request succeeded and that the requested information is in the response.";
                log.apilog_status = "200";
                log.apilog_message = "indicates that the request succeeded and that the requested information is in the response.";
                controller.dispose();
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.ToString();
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return response;
        }

        public ApiResponse<ProContract> APIHRProjectContractDelete(string ProjectCode, string ProContractId)
        {
            ApiResponse<ProContract> response = new ApiResponse<ProContract>();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BCO002.4";
            log.apilog_by = "";

            try
            {
                string url = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.ToString();
                log.apilog_data = url;
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    response.message = "indicates that the requested resource requires authentication.";

                    log.apilog_status = "401";
                    log.apilog_message = BpcOpr.MessageNotAuthen;

                    return response;
                }
                Authen objAuthen = new Authen();
                string tmp = authHeader.Substring(7);
                var handler = new JwtSecurityTokenHandler();
                var decodedValue = handler.ReadJwtToken(tmp);
                var usr = decodedValue.Claims.Single(claim => claim.Type == "user_aabbcc");
                log.apilog_by = usr.Value;
                cls_ctTRProcontract controller = new cls_ctTRProcontract();
                bool blnResult = controller.delete(ProjectCode, "",ProContractId);

                if (blnResult)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                    response.success = true;
                    response.message = "indicates that the request succeeded and that the requested information is in the response.";
                    log.apilog_status = "200";
                    log.apilog_message = "indicates that the request succeeded and that the requested information is in the response.";
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    response.success = false;
                    response.message = "indicates that the request could not be understood by the server.";
                    log.apilog_status = "400";
                    log.apilog_message = "indicates that the request could not be understood by the server.";
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                log.apilog_message = ex.ToString();
                response.success = false;
                response.message = ex.ToString();
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return response;

        }


        public ApiResponse<APIHRJobmain> APIHRJobCreate(APIHRJobmain input, string TransactionId)
        {
            ApiResponse<APIHRJobmain> response = new ApiResponse<APIHRJobmain>();
            response.data = new List<APIHRJobmain>();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BCO003.1";
            log.apilog_by = input.ModifiedBy;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    response.message = "indicates that the requested resource requires authentication.";

                    log.apilog_status = "401";
                    log.apilog_message = BpcOpr.MessageNotAuthen;

                    return response;
                }

                cls_ctMTProjobmain controller = new cls_ctMTProjobmain();
                cls_ctTRProjobshift controllerjobshift = new cls_ctTRProjobshift();
                cls_ctTRProjobcost controllerjobcost = new cls_ctTRProjobcost();
                cls_ctTRProjobmachine controllerjobmachine = new cls_ctTRProjobmachine();
                if(input.JobPlaningLines.Count>0){
                foreach (ProJobMain data in input.JobPlaningLines)
                {
                    cls_MTProjobmain model = new cls_MTProjobmain();
                    model.projobmain_id = data.ProJobMainId;
                    model.projobmain_code = data.ProJobMainCode;
                    model.projobmain_name_th = data.ProJobMainNameTh;
                    model.projobmain_name_en = data.ProJobMainNameEn;
                    model.projobmain_jobtype = data.ProJobMainJobType.ToString();
                    model.projobmain_fromdate = Convert.ToDateTime(data.ProJobMainFromDate);
                    model.projobmain_todate = Convert.ToDateTime(data.ProJobMainToDate);
                    model.projobmain_type = data.ProJobMainType.ToString();
                    model.projobmain_timepol = data.ProJobMainTimePol;
                    model.projobmain_slip = data.ProJobMainSlip;
                    model.projobmain_uniform = data.ProJobMainUniform;
                    model.project_code = input.ProjectCode;
                    model.version = input.Version;
                    model.modified_by = input.ModifiedBy;
                    bool strID = controller.insert(model);
                    if (strID)
                    {
                        foreach (ProJobShift datashift in data.JobTaskShift)
                        {
                            cls_TRProjobshift modeljobshift = new cls_TRProjobshift();

                            modeljobshift.projobshift_id = datashift.ProJobShiftId;
                            modeljobshift.shift_code = datashift.ShiftCode;
                            modeljobshift.projobshift_sun = datashift.ProJobShiftSun;
                            modeljobshift.projobshift_mon = datashift.ProJobShiftMon;
                            modeljobshift.projobshift_tue = datashift.ProJobShiftTue;
                            modeljobshift.projobshift_wed = datashift.ProJobShiftWed;
                            modeljobshift.projobshift_thu = datashift.ProJobShiftThu;
                            modeljobshift.projobshift_fri = datashift.ProJobShiftFri;
                            modeljobshift.projobshift_sat = datashift.ProJobShiftSat;
                            modeljobshift.projobshift_emp = datashift.ProJobShiftEmp;
                            modeljobshift.projobshift_ph = datashift.ProJobShiftPh;
                            modeljobshift.projobshift_working = datashift.ProJobShiftWorking;
                            modeljobshift.projobshift_hrsperday = datashift.ProJobShiftHrsPerDay;
                            modeljobshift.projobshift_hrsot = datashift.ProJobShiftHrsOT;
                            modeljobshift.projob_code = model.projobmain_code;
                            modeljobshift.project_code = model.project_code;
                            modeljobshift.version = model.version;
                            modeljobshift.modified_by = input.ModifiedBy;

                            if (!controllerjobshift.insert(modeljobshift))
                            {
                                controller.delete2(model.version,model.project_code,model.projobmain_code);
                                controllerjobshift.delete(model.project_code, model.projobmain_code, model.version);
                                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                                response.success = false;
                                response.message = "indicates that the request could not be understood by the server. | " + controllerjobshift.getMessage();
                                response.data.Add(input);

                                log.apilog_status = "500";
                                log.apilog_message = controllerjobshift.getMessage();
                                return response;
                            }
                        }
                        foreach (ProJobCost datacost in data.JobTaskCost)
                        {
                            cls_TRProjobcost modeljobcost = new cls_TRProjobcost();
                            modeljobcost.projobcost_id = datacost.ProJobCostId;
                            modeljobcost.projobcost_code = datacost.ProJobCostCode;
                            modeljobcost.projobcost_amount = datacost.ProJobCostAmount;
                            modeljobcost.projobcost_auto = datacost.ProJobCostAuto;
                            modeljobcost.projobcost_status = datacost.ProJobCostStatus.ToString();
                            modeljobcost.projob_code = model.projobmain_code;
                            modeljobcost.project_code = model.project_code;
                            modeljobcost.version = model.version;
                            modeljobcost.modified_by = input.ModifiedBy;


                            if (!controllerjobcost.insert(modeljobcost))
                            {
                                controller.delete2(model.version, model.project_code, model.projobmain_code);
                                controllerjobshift.delete(model.project_code, model.projobmain_code, model.version);
                                controllerjobcost.delete(model.version,model.project_code,model.projobmain_code);
                                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                                response.success = false;
                                response.message = "indicates that the request could not be understood by the server. | " + controllerjobcost.getMessage();
                                response.data.Add(input);

                                log.apilog_status = "500";
                                log.apilog_message = controllerjobcost.getMessage();
                                return response;
                            }
                        }
                        foreach (ProJobMachine datamac in data.JobTaskMachine)
                        {
                            cls_TRProjobmachine modeljobmac = new cls_TRProjobmachine();
                            modeljobmac.projobmachine_id = datamac.ProJobMachineId;
                            modeljobmac.projobmachine_ip = datamac.ProJobMachineIp;
                            modeljobmac.projobmachine_port = datamac.ProJobMachinePort;
                            modeljobmac.projobmachine_enable = datamac.ProJobMachineEnable;
                            modeljobmac.projob_code = model.projobmain_code;
                            modeljobmac.project_code = model.project_code;
                            modeljobmac.modified_by = input.ModifiedBy;



                            if (!controllerjobmachine.insert(modeljobmac))
                            {
                                controller.delete2(model.version, model.project_code, model.projobmain_code);
                                controllerjobshift.delete(model.project_code, model.projobmain_code, model.version);
                                controllerjobcost.delete(model.version, model.project_code, model.projobmain_code);
                                controllerjobmachine.delete(model.project_code, model.projobmain_code,"");
                                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                                response.success = false;
                                response.message = "indicates that the request could not be understood by the server. | " + controllerjobmachine.getMessage();
                                response.data.Add(input);

                                log.apilog_status = "500";
                                log.apilog_message = controllerjobmachine.getMessage();
                                return response;
                            }
                        }
                    }
                }

                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Created;
                    response.success = true;
                    response.message = "indicates that the request resulted in a new resource created before the response was sent.";
                    response.data.Add(input);

                    log.apilog_status = "201";
                    log.apilog_message = "indicates that the request resulted in a new resource created before the response was sent.";
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    response.success = false;
                    response.message = "indicates that the request could not be understood by the server.";
                    response.data.Add(input);

                    log.apilog_status = "400";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.ToString();
                log.apilog_message = ex.ToString();
                log.apilog_status = "500";
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return response;
        }

        public ApiResponse<APIHRJobmain> APIHRJobUpdate(APIHRJobmain input,string TransactionId, string OldTransactionId)
        {
            ApiResponse<APIHRJobmain> response = new ApiResponse<APIHRJobmain>();
            response.data = new List<APIHRJobmain>();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BCO003.2";
            log.apilog_by = input.ModifiedBy;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    response.message = "indicates that the requested resource requires authentication.";

                    log.apilog_status = "401";
                    log.apilog_message = BpcOpr.MessageNotAuthen;

                    return response;
                }

                cls_ctMTProjobmain controller = new cls_ctMTProjobmain();
                cls_ctTRProjobshift controllerjobshift = new cls_ctTRProjobshift();
                cls_ctTRProjobcost controllerjobcost = new cls_ctTRProjobcost();
                cls_ctTRProjobmachine controllerjobmachine = new cls_ctTRProjobmachine();
                if (input.JobPlaningLines.Count > 0)
                {
                    foreach (ProJobMain data in input.JobPlaningLines)
                    {
                        cls_MTProjobmain model = new cls_MTProjobmain();
                        model.projobmain_id = data.ProJobMainId;
                        model.projobmain_code = data.ProJobMainCode;
                        model.projobmain_name_th = data.ProJobMainNameTh;
                        model.projobmain_name_en = data.ProJobMainNameEn;
                        model.projobmain_jobtype = data.ProJobMainJobType.ToString();
                        model.projobmain_fromdate = Convert.ToDateTime(data.ProJobMainFromDate);
                        model.projobmain_todate = Convert.ToDateTime(data.ProJobMainToDate);
                        model.projobmain_type = data.ProJobMainType.ToString();
                        model.projobmain_timepol = data.ProJobMainTimePol;
                        model.projobmain_slip = data.ProJobMainSlip;
                        model.projobmain_uniform = data.ProJobMainUniform;
                        model.project_code = input.ProjectCode;
                        model.version = input.Version;
                        model.modified_by = input.ModifiedBy;
                        bool strID = false;
                        if (controller.checkDataOld("",model.project_code,model.projobmain_code,model.projobmain_id))
                        {
                            strID = controller.update(model);
                        }
                        if (strID)
                        {
                            foreach (ProJobShift datashift in data.JobTaskShift)
                            {
                                cls_TRProjobshift modeljobshift = new cls_TRProjobshift();

                                modeljobshift.projobshift_id = datashift.ProJobShiftId;
                                modeljobshift.shift_code = datashift.ShiftCode;
                                modeljobshift.projobshift_sun = datashift.ProJobShiftSun;
                                modeljobshift.projobshift_mon = datashift.ProJobShiftMon;
                                modeljobshift.projobshift_tue = datashift.ProJobShiftTue;
                                modeljobshift.projobshift_wed = datashift.ProJobShiftWed;
                                modeljobshift.projobshift_thu = datashift.ProJobShiftThu;
                                modeljobshift.projobshift_fri = datashift.ProJobShiftFri;
                                modeljobshift.projobshift_sat = datashift.ProJobShiftSat;
                                modeljobshift.projobshift_emp = datashift.ProJobShiftEmp;
                                modeljobshift.projobshift_ph = datashift.ProJobShiftPh;
                                modeljobshift.projobshift_working = datashift.ProJobShiftWorking;
                                modeljobshift.projobshift_hrsperday = datashift.ProJobShiftHrsPerDay;
                                modeljobshift.projobshift_hrsot = datashift.ProJobShiftHrsOT;
                                modeljobshift.projob_code = model.projobmain_code;
                                modeljobshift.project_code = model.project_code;
                                modeljobshift.version = model.version;
                                modeljobshift.modified_by = input.ModifiedBy;
                                bool jobshift = false;
                                if (controllerjobshift.checkDataOld(modeljobshift.project_code,modeljobshift.projob_code, "", "", modeljobshift.projobshift_id))
                                {
                                    jobshift = controllerjobshift.update(modeljobshift);
                                }
                                if (!jobshift)
                                {
                                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                                    response.success = false;
                                    response.message = "indicates that the request could not be understood by the server. | " + controllerjobshift.getMessage();
                                    response.data.Add(input);

                                    log.apilog_status = "500";
                                    log.apilog_message = controllerjobshift.getMessage();
                                    return response;
                                }
                            }
                            foreach (ProJobCost datacost in data.JobTaskCost)
                            {
                                cls_TRProjobcost modeljobcost = new cls_TRProjobcost();
                                modeljobcost.projobcost_id = datacost.ProJobCostId;
                                modeljobcost.projobcost_code = datacost.ProJobCostCode;
                                modeljobcost.projobcost_amount = datacost.ProJobCostAmount;
                                modeljobcost.projobcost_auto = datacost.ProJobCostAuto;
                                modeljobcost.projobcost_status = datacost.ProJobCostStatus.ToString();
                                modeljobcost.projob_code = model.projobmain_code;
                                modeljobcost.project_code = model.project_code;
                                modeljobcost.version = model.version;
                                modeljobcost.modified_by = input.ModifiedBy;

                                bool jobcost = false;
                                if (controllerjobcost.checkDataOld(modeljobcost.project_code,"","","",modeljobcost.projobcost_id))
                                {
                                    jobcost = controllerjobcost.update(modeljobcost);
                                }
                                if (!jobcost)
                                {
                                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                                    response.success = false;
                                    response.message = "indicates that the request could not be understood by the server. | " + controllerjobcost.getMessage();
                                    response.data.Add(input);

                                    log.apilog_status = "500";
                                    log.apilog_message = controllerjobcost.getMessage();
                                    return response;
                                }
                            }
                            foreach (ProJobMachine datamac in data.JobTaskMachine)
                            {
                                cls_TRProjobmachine modeljobmac = new cls_TRProjobmachine();
                                modeljobmac.projobmachine_id = datamac.ProJobMachineId;
                                modeljobmac.projobmachine_ip = datamac.ProJobMachineIp;
                                modeljobmac.projobmachine_port = datamac.ProJobMachinePort;
                                modeljobmac.projobmachine_enable = datamac.ProJobMachineEnable;
                                modeljobmac.projob_code = model.projobmain_code;
                                modeljobmac.project_code = model.project_code;
                                modeljobmac.modified_by = input.ModifiedBy;

                                bool jobmac = false;
                                if (controllerjobmachine.checkDataOld(modeljobmac.project_code, model.projobmain_code, "", modeljobmac.projobmachine_id))
                                {
                                    jobmac = controllerjobmachine.update(modeljobmac);
                                }
                                if (!jobmac)
                                {
                                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                                    response.success = false;
                                    response.message = "indicates that the request could not be understood by the server. | " + controllerjobmachine.getMessage();
                                    response.data.Add(input);

                                    log.apilog_status = "500";
                                    log.apilog_message = controllerjobmachine.getMessage();
                                    return response;
                                }
                            }
                        }
                    }

                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                    response.success = true;
                    response.message = "indicates that the request succeeded and that the requested information is in the response.";
                    response.data.Add(input);

                    log.apilog_status = "200";
                    log.apilog_message = "indicates that the request succeeded and that the requested information is in the response.";
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    response.success = false;
                    response.message = "indicates that the request could not be understood by the server.";
                    response.data.Add(input);

                    log.apilog_status = "400";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

            }
            catch (Exception ex)
            {
                log.apilog_message = ex.ToString();
                log.apilog_status = "500";
                response.success = false;
                response.message = ex.ToString();
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return response;
        }

        public ApiResponse<ProJobMain> APIHRJobList(string ProjectCode, string ProJobMainCode,string Version)
        {
            ApiResponse<ProJobMain> response = new ApiResponse<ProJobMain>();
            response.data = new List<ProJobMain>();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BCO003.3";
            log.apilog_data = "all";
            log.apilog_by = "";

            try
            {
                string url = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.ToString();
                log.apilog_data = url;
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    response.message = "indicates that the requested resource requires authentication.";

                    log.apilog_status = "401";
                    log.apilog_message = BpcOpr.MessageNotAuthen;

                    return response;
                }
                Authen objAuthen = new Authen();
                string tmp = authHeader.Substring(7);
                var handler = new JwtSecurityTokenHandler();
                var decodedValue = handler.ReadJwtToken(tmp);
                var usr = decodedValue.Claims.Single(claim => claim.Type == "user_aabbcc");
                log.apilog_by = usr.Value;

                cls_ctMTProjobmain controller = new cls_ctMTProjobmain();
                List<cls_MTProjobmain> list = controller.getDataByFillter("",ProjectCode == null ? "" : ProjectCode, Version == null ? "" : Version,ProJobMainCode == null ? "" :ProJobMainCode);

                JArray array = new JArray();

                if (list.Count > 0)
                {
                    foreach (cls_MTProjobmain data in list)
                    {
                        ProJobMain apihrpromain = new ProJobMain();
                        apihrpromain.ProJobMainId = data.projobmain_id;
                        apihrpromain.ProJobMainCode = data.projobmain_code;
                        apihrpromain.ProJobMainNameTh = data.projobmain_name_th;
                        apihrpromain.ProJobMainNameEn = data.projobmain_name_en;
                        apihrpromain.ProJobMainJobType = Convert.ToChar(data.projobmain_jobtype);
                        apihrpromain.ProJobMainFromDate = data.projobmain_fromdate.ToString("dd/MM/yyyy");
                        apihrpromain.ProJobMainToDate = data.projobmain_todate.ToString("dd/MM/yyyy");
                        apihrpromain.ProJobMainType = Convert.ToChar(data.projobmain_type);
                        apihrpromain.ProJobMainTimePol = data.projobmain_timepol;
                        apihrpromain.ProJobMainSlip = data.projobmain_slip;
                        apihrpromain.ProJobMainUniform = data.projobmain_uniform;
                        apihrpromain.ProjectCode = data.project_code;
                        apihrpromain.Version = data.version;

                        apihrpromain.ModifiedBy = data.modified_by;
                        apihrpromain.ModifiedDate = data.modified_date.ToString("dd/MM/yy");
                        // Projobshift
                        apihrpromain.JobTaskShift = new List<ProJobShift>();
                        cls_ctTRProjobshift controllerjobshift = new cls_ctTRProjobshift();
                        List<cls_TRProjobshift> projobshift = controllerjobshift.getDataByFillter(data.project_code,data.projobmain_code,data.version);
                        if (projobshift.Count > 0)
                        {
                            foreach (cls_TRProjobshift datashift in projobshift)
                            {
                                ProJobShift jobShift = new ProJobShift();
                                jobShift.ProJobShiftId = datashift.projobshift_id;
                                jobShift.ShiftCode = datashift.shift_code;
                                jobShift.ProJobShiftSun = datashift.projobshift_sun;
                                jobShift.ProJobShiftMon = datashift.projobshift_mon;
                                jobShift.ProJobShiftTue = datashift.projobshift_tue;
                                jobShift.ProJobShiftWed = datashift.projobshift_wed;
                                jobShift.ProJobShiftThu = datashift.projobshift_thu;
                                jobShift.ProJobShiftFri = datashift.projobshift_fri;
                                jobShift.ProJobShiftSat = datashift.projobshift_sat;
                                jobShift.ProJobShiftEmp = datashift.projobshift_emp;
                                jobShift.ProJobShiftPh = datashift.projobshift_ph;
                                jobShift.ProJobShiftWorking = datashift.projobshift_working;
                                jobShift.ProJobShiftHrsPerDay = datashift.projobshift_hrsperday;
                                jobShift.ProJobShiftHrsOT = datashift.projobshift_hrsot;
                                jobShift.ProJobCode = datashift.projob_code;
                                jobShift.ProjectCode = datashift.project_code;
                                jobShift.Version = datashift.version;
                                jobShift.ModifiedBy = datashift.modified_by;
                                jobShift.ModifiedDate = datashift.modified_date.ToString("dd/MM/yyyy");
                                apihrpromain.JobTaskShift.Add(jobShift);
                            }
                        }
                        // Projobcost
                        apihrpromain.JobTaskCost = new List<ProJobCost>();
                        cls_ctTRProjobcost controllerprojobcost = new cls_ctTRProjobcost();
                        List<cls_TRProjobcost> listcost = controllerprojobcost.getDataByFillter(data.project_code,data.projobmain_code,data.version,"PSG");
                        if (listcost.Count > 0)
                        {
                            foreach (cls_TRProjobcost datacost in listcost)
                            {
                                ProJobCost modelcost = new ProJobCost();
                                modelcost.ProJobCostId = datacost.projobcost_id;
                                modelcost.ProJobCostCode = datacost.projobcost_code;
                                modelcost.ProJobCostAmount = datacost.projobcost_amount;
                                modelcost.ProJobCostAuto = datacost.projobcost_auto;
                                modelcost.ProJobCostStatus = Convert.ToChar(datacost.projobcost_status);
                                modelcost.ProJobCode = datacost.projob_code;
                                modelcost.ProjectCode = datacost.project_code;
                                modelcost.Version = datacost.version;
                                modelcost.ModifiedBy = datacost.modified_by;
                                modelcost.ModifiedDate = datacost.modified_date.ToString("dd/MM/yyyy");
                                apihrpromain.JobTaskCost.Add(modelcost);
                            }
                        }
                        // Projobmachine
                        apihrpromain.JobTaskMachine = new List<ProJobMachine>();
                        cls_ctTRProjobmachine controllerjobmachine = new cls_ctTRProjobmachine();
                        List<cls_TRProjobmachine> jobmac = controllerjobmachine.getDataByFillter(data.project_code,data.projobmain_code);
                        if (jobmac.Count > 0)
                        {
                            foreach (cls_TRProjobmachine datamac in jobmac)
                            {
                                ProJobMachine modelmac = new ProJobMachine();
                                modelmac.ProJobMachineId = datamac.projobmachine_id;
                                modelmac.ProJobMachineIp = datamac.projobmachine_ip;
                                modelmac.ProJobMachinePort = datamac.projobmachine_port;
                                modelmac.ProJobMachineEnable = datamac.projobmachine_enable;
                                modelmac.ProJobCode = datamac.projob_code;
                                modelmac.ProjectCode = datamac.project_code;
                                modelmac.ModifiedBy = datamac.modified_by;
                                modelmac.ModifiedDate = datamac.modified_date.ToString("dd/MM/yyyy");
                                apihrpromain.JobTaskMachine.Add(modelmac);
                      
                            }
                        }
                        response.data.Add(apihrpromain);
                    }

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                response.success = true;
                response.message = "indicates that the request succeeded and that the requested information is in the response.";
                log.apilog_status = "200";
                log.apilog_message = "indicates that the request succeeded and that the requested information is in the response.";
                controller.dispose();
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.ToString();
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return response;
        }

        public ApiResponse<ProJobMain> APIHRJobDelete(string ProjectCode, string ProJobMainCode, string Version)
        {
            ApiResponse<ProJobMain> response = new ApiResponse<ProJobMain>();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BCO003.4";
            log.apilog_by = "";

            try
            {
                string url = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.ToString();
                log.apilog_data = url;
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    response.message = "indicates that the requested resource requires authentication.";

                    log.apilog_status = "401";
                    log.apilog_message = BpcOpr.MessageNotAuthen;

                    return response;
                }
                Authen objAuthen = new Authen();
                string tmp = authHeader.Substring(7);
                var handler = new JwtSecurityTokenHandler();
                var decodedValue = handler.ReadJwtToken(tmp);
                var usr = decodedValue.Claims.Single(claim => claim.Type == "user_aabbcc");
                log.apilog_by = usr.Value;
                cls_ctMTProjobmain controller = new cls_ctMTProjobmain();
                cls_ctTRProjobshift controllerjobshift = new cls_ctTRProjobshift();
                cls_ctTRProjobcost controllerjobcost = new cls_ctTRProjobcost();
                cls_ctTRProjobmachine controllerjobmachine = new cls_ctTRProjobmachine();
                bool blnResult = controller.delete(Version, ProjectCode, ProJobMainCode);

                if (blnResult)
                {
                    controllerjobshift.delete(ProjectCode, ProJobMainCode, Version);
                    controllerjobcost.delete(Version, ProjectCode, ProJobMainCode);
                    controllerjobmachine.delete(ProjectCode, ProJobMainCode, "");
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                    response.success = true;
                    response.message = "indicates that the request succeeded and that the requested information is in the response.";
                    log.apilog_status = "200";
                    log.apilog_message = "indicates that the request succeeded and that the requested information is in the response.";
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    response.success = false;
                    response.message = "indicates that the request could not be understood by the server.";
                    log.apilog_status = "400";
                    log.apilog_message = "indicates that the request could not be understood by the server.";
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                log.apilog_message = ex.ToString();
                response.success = false;
                response.message = ex.ToString();
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return response;

        }


        public ApiResponse<ProUniform> APIHRUniformCreate(ProUniform input, string TransactionId)
        {
            ApiResponse<ProUniform> response = new ApiResponse<ProUniform>();
            response.data = new List<ProUniform>();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BCO004.1";
            log.apilog_by = input.ModifiedBy;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    response.message = "indicates that the requested resource requires authentication.";

                    log.apilog_status = "401";
                    log.apilog_message = BpcOpr.MessageNotAuthen;

                    return response;
                }

                cls_ctMTProuniform controller = new cls_ctMTProuniform();
                cls_MTProuniform model = new cls_MTProuniform();

                model.company_code = input.CompanyCode;
                model.prouniform_id = input.ProUniformId;
                model.prouniform_code = input.ProUniformCode;
                model.prouniform_name_th = input.ProUniformNameTh;
                model.prouniform_name_en = input.ProUniformNameEn;
 
                model.modified_by = input.ModifiedBy;

                string strID = controller.insert(model);
                if (!strID.Equals(""))
                {
                    input.ProUniformId = Convert.ToInt32(strID);
                    input.ModifiedDate = DateTime.Now.ToString("dd/MM/yyyy");
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Created;
                    response.success = true;
                    response.message = "indicates that the request resulted in a new resource created before the response was sent.";
                    response.data.Add(input);

                    log.apilog_status = "201";
                    log.apilog_message = "indicates that the request resulted in a new resource created before the response was sent.";
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    response.success = false;
                    response.message = "indicates that the request could not be understood by the server.";
                    response.data.Add(input);

                    log.apilog_status = "400";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.ToString();
                log.apilog_message = ex.ToString();
                log.apilog_status = "500";
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return response;
        }

        public ApiResponse<ProUniform> APIHRUniformUpdate(ProUniform input, string TransactionId, string OldTransactionId)
        {
            ApiResponse<ProUniform> response = new ApiResponse<ProUniform>();
            response.data = new List<ProUniform>();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BCO004.2";
            log.apilog_by = input.ModifiedBy;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    response.message = "indicates that the requested resource requires authentication.";

                    log.apilog_status = "401";
                    log.apilog_message = BpcOpr.MessageNotAuthen;

                    return response;
                }
                cls_ctMTProuniform controller = new cls_ctMTProuniform();
                cls_MTProuniform model = new cls_MTProuniform();

                model.company_code = input.CompanyCode;
                model.prouniform_id = input.ProUniformId;
                model.prouniform_code = input.ProUniformCode;
                model.prouniform_name_th = input.ProUniformNameTh;
                model.prouniform_name_en = input.ProUniformNameEn;

                model.modified_by = input.ModifiedBy;
                bool strID = false;
                if (controller.checkDataOld(model.prouniform_code, model.company_code))
                {
                    if (model.prouniform_id.Equals(0))
                    {
                        strID = false;
                    }
                    else
                    {
                        strID = controller.update(model);
                    }
                }
                if (strID)
                {
                    input.ModifiedDate = DateTime.Now.ToString("dd/MM/yyyy");
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                    response.success = true;
                    response.message = "indicates that the request succeeded and that the requested information is in the response.";
                    response.data.Add(input);

                    log.apilog_status = "200";
                    log.apilog_message = "indicates that the request succeeded and that the requested information is in the response.";
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    response.success = false;
                    response.message = "indicates that the request could not be understood by the server.";
                    response.data.Add(input);

                    log.apilog_status = "400";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

            }
            catch (Exception ex)
            {
                log.apilog_message = ex.ToString();
                log.apilog_status = "500";
                response.success = false;
                response.message = ex.ToString();
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return response;
        }

        public ApiResponse<ProUniform> APIHRUniformList(string CompanyCode, string ProUniformCode)
        {
            ApiResponse<ProUniform> response = new ApiResponse<ProUniform>();
            response.data = new List<ProUniform>();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BCO004.3";
            log.apilog_data = "all";
            log.apilog_by = "";

            try
            {
                string url = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.ToString();
                log.apilog_data = url;
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    response.message = "indicates that the requested resource requires authentication.";

                    log.apilog_status = "401";
                    log.apilog_message = BpcOpr.MessageNotAuthen;

                    return response;
                }
                Authen objAuthen = new Authen();
                string tmp = authHeader.Substring(7);
                var handler = new JwtSecurityTokenHandler();
                var decodedValue = handler.ReadJwtToken(tmp);
                var usr = decodedValue.Claims.Single(claim => claim.Type == "user_aabbcc");
                log.apilog_by = usr.Value;

                cls_ctMTProuniform controller = new cls_ctMTProuniform();
                List<cls_MTProuniform> list = controller.getDataByFillter(CompanyCode == null ? "" : CompanyCode, ProUniformCode == null ? "" : ProUniformCode);

                JArray array = new JArray();

                if (list.Count > 0)
                {
                    foreach (cls_MTProuniform data in list)
                    {
                        ProUniform proUniform = new ProUniform();
                        proUniform.CompanyCode = data.company_code;
                        proUniform.ProUniformId = data.prouniform_id;
                        proUniform.ProUniformCode = data.prouniform_code;
                        proUniform.ProUniformNameTh = data.prouniform_name_th;
                        proUniform.ProUniformNameEn = data.prouniform_name_en;
                        proUniform.ModifiedBy = data.modified_by;
                        proUniform.ModifiedDate = data.modified_date.ToString("dd/MM/yyyy");

                        response.data.Add(proUniform);
                    }

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                response.success = true;
                response.message = "indicates that the request succeeded and that the requested information is in the response.";
                log.apilog_status = "200";
                log.apilog_message = "indicates that the request succeeded and that the requested information is in the response.";
                controller.dispose();
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.ToString();
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return response;
        }

        public ApiResponse<ProUniform> APIHRUniformDelete(string CompanyCode, string ProUniformCode)
        {
            ApiResponse<ProUniform> response = new ApiResponse<ProUniform>();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BCO004.4";
            log.apilog_by = "";

            try
            {
                string url = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.ToString();
                log.apilog_data = url;
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    response.message = "indicates that the requested resource requires authentication.";

                    log.apilog_status = "401";
                    log.apilog_message = BpcOpr.MessageNotAuthen;

                    return response;
                }
                Authen objAuthen = new Authen();
                string tmp = authHeader.Substring(7);
                var handler = new JwtSecurityTokenHandler();
                var decodedValue = handler.ReadJwtToken(tmp);
                var usr = decodedValue.Claims.Single(claim => claim.Type == "user_aabbcc");
                log.apilog_by = usr.Value;
                cls_ctMTProuniform controller = new cls_ctMTProuniform();
                bool blnResult = controller.delete(ProUniformCode,CompanyCode);

                if (blnResult)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                    response.success = true;
                    response.message = "indicates that the request succeeded and that the requested information is in the response.";
                    log.apilog_status = "200";
                    log.apilog_message = "indicates that the request succeeded and that the requested information is in the response.";
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    response.success = false;
                    response.message = "indicates that the request could not be understood by the server.";
                    log.apilog_status = "400";
                    log.apilog_message = "indicates that the request could not be understood by the server.";
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                log.apilog_message = ex.ToString();
                response.success = false;
                response.message = ex.ToString();
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return response;

        }


        public ApiResponse<ProEquipmentReq> APIHRUniformSummaryCreate(ProEquipmentReq input,string TransactionId)
        {
            ApiResponse<ProEquipmentReq> response = new ApiResponse<ProEquipmentReq>();
            response.data = new List<ProEquipmentReq>();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BCO005.1";
            log.apilog_by = input.ModifiedBy;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    response.message = "indicates that the requested resource requires authentication.";

                    log.apilog_status = "401";
                    log.apilog_message = BpcOpr.MessageNotAuthen;

                    return response;
                }

                cls_ctTRProequipmentreq controller = new cls_ctTRProequipmentreq();
                cls_TRProequipmentreq model = new cls_TRProequipmentreq();

                model.proequipmentreq_id = input.ProEquipmentReqId;
                model.prouniform_code = input.ProUniformCode;
                model.proequipmentreq_date = Convert.ToDateTime(input.ProEquipmentReqDate);
                model.proequipmentreq_qty = input.ProEquipmentReqQty;
                model.proequipmentreq_note = input.ProEquipmentReqNote;
                model.proequipmentreq_by = input.ProEquipmentReqBy;
                model.proequipmenttype_code = input.ProEquipmentTypeCode;
                model.projob_code = input.ProJobCode;
                model.project_code = input.ProjectCode;

                model.modified_by = input.ModifiedBy;

                bool strID = controller.insert(model);
                if (strID)
                {
                    input.ModifiedDate = DateTime.Now.ToString("dd/MM/yyyy");
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Created;
                    response.success = true;
                    response.message = "indicates that the request resulted in a new resource created before the response was sent.";
                    response.data.Add(input);

                    log.apilog_status = "201";
                    log.apilog_message = "indicates that the request resulted in a new resource created before the response was sent.";
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    response.success = false;
                    response.message = "indicates that the request could not be understood by the server.";
                    response.data.Add(input);

                    log.apilog_status = "400";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.ToString();
                log.apilog_message = ex.ToString();
                log.apilog_status = "500";
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return response;
        }

        public ApiResponse<ProEquipmentReq> APIHRUniformSummaryUpdate(ProEquipmentReq input,string TransactionId, string OldTransactionId)
        {
            ApiResponse<ProEquipmentReq> response = new ApiResponse<ProEquipmentReq>();
            response.data = new List<ProEquipmentReq>();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BCO005.2";
            log.apilog_by = input.ModifiedBy;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    response.message = "indicates that the requested resource requires authentication.";

                    log.apilog_status = "401";
                    log.apilog_message = BpcOpr.MessageNotAuthen;

                    return response;
                }
                cls_ctTRProequipmentreq controller = new cls_ctTRProequipmentreq();
                cls_TRProequipmentreq model = new cls_TRProequipmentreq();

                model.proequipmentreq_id = input.ProEquipmentReqId;
                model.prouniform_code = input.ProUniformCode;
                model.proequipmentreq_date = Convert.ToDateTime(input.ProEquipmentReqDate);
                model.proequipmentreq_qty = input.ProEquipmentReqQty;
                model.proequipmentreq_note = input.ProEquipmentReqNote;
                model.proequipmentreq_by = input.ProEquipmentReqBy;
                model.proequipmenttype_code = input.ProEquipmentTypeCode;
                model.projob_code = input.ProJobCode;
                model.project_code = input.ProjectCode;

                model.modified_by = input.ModifiedBy;
                bool strID = false;
                if (controller.checkDataOld(model.project_code,"","","",model.proequipmentreq_id))
                {
                    if (model.proequipmentreq_id.Equals(0))
                    {
                        strID = false;
                    }
                    else
                    {
                        strID = controller.update(model);
                    }
                }
                if (strID)
                {
                    input.ModifiedDate = DateTime.Now.ToString("dd/MM/yyyy");
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                    response.success = true;
                    response.message = "indicates that the request succeeded and that the requested information is in the response.";
                    response.data.Add(input);

                    log.apilog_status = "200";
                    log.apilog_message = "indicates that the request succeeded and that the requested information is in the response.";
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    response.success = false;
                    response.message = "indicates that the request could not be understood by the server.";
                    response.data.Add(input);

                    log.apilog_status = "400";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

            }
            catch (Exception ex)
            {
                log.apilog_message = ex.ToString();
                log.apilog_status = "500";
                response.success = false;
                response.message = ex.ToString();
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return response;
        }

        public ApiResponse<ProEquipmentReq> APIHRUniformSummaryList(string ProjectCode, string ProJobCode,string ProUniformCode)
        {
            ApiResponse<ProEquipmentReq> response = new ApiResponse<ProEquipmentReq>();
            response.data = new List<ProEquipmentReq>();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BCO005.3";
            log.apilog_data = "all";
            log.apilog_by = "";

            try
            {
                string url = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.ToString();
                log.apilog_data = url;
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    response.message = "indicates that the requested resource requires authentication.";

                    log.apilog_status = "401";
                    log.apilog_message = BpcOpr.MessageNotAuthen;

                    return response;
                }
                Authen objAuthen = new Authen();
                string tmp = authHeader.Substring(7);
                var handler = new JwtSecurityTokenHandler();
                var decodedValue = handler.ReadJwtToken(tmp);
                var usr = decodedValue.Claims.Single(claim => claim.Type == "user_aabbcc");
                log.apilog_by = usr.Value;

                cls_ctTRProequipmentreq controller = new cls_ctTRProequipmentreq();
                List<cls_TRProequipmentreq> list = controller.getDataByFillter(ProjectCode == null ? "" : ProjectCode, ProJobCode == null ? "" : ProJobCode, ProUniformCode == null ? "" : ProUniformCode, "");

                JArray array = new JArray();

                if (list.Count > 0)
                {
                    foreach (cls_TRProequipmentreq data in list)
                    {
                        ProEquipmentReq proEquipmentReq = new ProEquipmentReq();
                        proEquipmentReq.ProEquipmentReqId = data.proequipmentreq_id;
                        proEquipmentReq.ProUniformCode = data.prouniform_code;
                        proEquipmentReq.ProEquipmentReqDate = data.proequipmentreq_date.ToString("dd/MM/yyyy");
                        proEquipmentReq.ProEquipmentReqQty = data.proequipmentreq_qty;
                        proEquipmentReq.ProEquipmentReqNote = data.proequipmentreq_note;
                        proEquipmentReq.ProEquipmentReqBy = data.proequipmentreq_by;
                        proEquipmentReq.ProEquipmentTypeCode = data.proequipmenttype_code;
                        proEquipmentReq.ProJobCode = data.projob_code;
                        proEquipmentReq.ProjectCode = data.project_code;
                        proEquipmentReq.ModifiedBy = data.modified_by;
                        proEquipmentReq.ModifiedDate = data.modified_date.ToString("dd/MM/yyyy");


                        response.data.Add(proEquipmentReq);
                    }

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                response.success = true;
                response.message = "indicates that the request succeeded and that the requested information is in the response.";
                log.apilog_status = "200";
                log.apilog_message = "indicates that the request succeeded and that the requested information is in the response.";
                controller.dispose();
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.ToString();
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return response;
        }

        public ApiResponse<ProEquipmentReq> APIHRUniformSummaryDelete(string ProjectCode, string ProJobCode, string ProUniformCode)
        {
            ApiResponse<ProEquipmentReq> response = new ApiResponse<ProEquipmentReq>();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BCO004.4";
            log.apilog_by = "";

            try
            {
                string url = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.ToString();
                log.apilog_data = url;
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    response.message = "indicates that the requested resource requires authentication.";

                    log.apilog_status = "401";
                    log.apilog_message = BpcOpr.MessageNotAuthen;

                    return response;
                }
                Authen objAuthen = new Authen();
                string tmp = authHeader.Substring(7);
                var handler = new JwtSecurityTokenHandler();
                var decodedValue = handler.ReadJwtToken(tmp);
                var usr = decodedValue.Claims.Single(claim => claim.Type == "user_aabbcc");
                log.apilog_by = usr.Value;
                cls_ctTRProequipmentreq controller = new cls_ctTRProequipmentreq();
                bool blnResult = controller.delete(ProjectCode, ProJobCode, ProUniformCode);

                if (blnResult)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                    response.success = true;
                    response.message = "indicates that the request succeeded and that the requested information is in the response.";
                    log.apilog_status = "200";
                    log.apilog_message = "indicates that the request succeeded and that the requested information is in the response.";
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    response.success = false;
                    response.message = "indicates that the request could not be understood by the server.";
                    log.apilog_status = "400";
                    log.apilog_message = "indicates that the request could not be understood by the server.";
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                log.apilog_message = ex.ToString();
                response.success = false;
                response.message = ex.ToString();
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return response;

        }

        
    }
}
