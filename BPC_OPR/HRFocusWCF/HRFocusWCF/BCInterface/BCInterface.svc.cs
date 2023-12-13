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

        public ApiResponse<APIHRProject> APIHRProjectCreate(APIHRProject input)
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

        public ApiResponse<APIHRProject> APIHRProjectUpdate(APIHRProject input)
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


        public ApiResponse<ProContract> APIHRProjectContractCreate(ProContract input)
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

        public ApiResponse<ProContract> APIHRProjectContractUpdate(ProContract input )
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

        
    }
}
