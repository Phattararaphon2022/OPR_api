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

namespace BPC_OPR
{

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]  
    
    public class BpcOpr : IBpcOpr
    {
        #region Authen
        public static string MessageNotAuthen = "No authorization header was provided";
        public static string UserAuthen = "admin";
        public static string PwdAuthen = "2022*";
        
        string UserToken = string.Empty;

        public AuthResponse doAuthen2(RequestData input)
        {
            AuthResponse authResponse = new AuthResponse();
            authResponse.data = new List<UserData>();
            cls_SYSApilog log = new cls_SYSApilog();
            // ... (initialize log properties)
            log.apilog_code = "ATH001";
            log.apilog_by = input.usname;
            log.apilog_data = "";

            try
            {
                // ... (existing code)
                cls_ctMTAccount Account = new cls_ctMTAccount();
                List<cls_MTAccount> list = Account.getDataByFillter(input.company_code, input.usname, "", 0, "");
                authResponse.success = input.usname.Equals(list[0].account_user) && input.pwd.Equals(list[0].account_pwd);
                if (authResponse.success)
                {
                    RequestData aa = new RequestData();
                    aa.usname = input.usname;
                    aa.pwd = input.pwd;
                    aa.company_code = input.company_code;
                    ResponseData bb = Login(aa);

                    authResponse.token = bb.token;

                    // Convert the array to a List<UserData>
                    foreach (var model in list)
                    {
                        UserData userData = new UserData
                        {
                            CompanyCode = model.company_code,
                            AccountUser = model.account_user,
                            // ... set other properties
                        };

                        authResponse.data.Add(userData);
                    }
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                    authResponse.message = "indicates that the request succeeded and that the requested information is in the response.";
                    log.apilog_status = "200";
                    log.apilog_message = "indicates that the request succeeded and that the requested information is in the response.";
                }
                else
                {
                    authResponse.message = "indicates that the requested resource does not exist on the server.";
                    authResponse.success = false;
                    log.apilog_status = "404";
                    log.apilog_message = "No access rights";
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                authResponse.success = false;
                authResponse.message = ex.ToString();

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                // ... (existing code)
            }

            return authResponse;
        }

        //public AuthResponse doAuthen(RequestData input)
        //{
        //    List<Person> players = new List<Person>();
        //    players.Add(new Person("Peyton", "Manning", 35));
        //    players.Add(new Person("Drew", "Brees", 31));
        //    players.Add(new Person("Tony", "Romo", 29));
        //    listdata output = new listdata(players);
        //    List<AuthResponse> output = new List<AuthResponse>();
        //    AuthResponse tt = new AuthResponse();
        //    datatest uu = new datatest();
        //    List<datatest> yy = new List<datatest>();
        //    uu.Message = "TESTsss";
        //    tt.Success = true;
        //    tt.Message = "TEST";
        //    yy.Add(uu);
        //    tt.UserData = yy;
        //    output.Add(tt);
        //    return tt;
        //}
        public string doAuthen(RequestData input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATH001";
            log.apilog_by = input.usname;
            log.apilog_data = "";

            try
            {
                cls_ctMTAccount Account = new cls_ctMTAccount();
                List<cls_MTAccount> list = Account.getDataByFillter(input.company_code, input.usname, "", 0, "");

                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTAccount model in list)
                    {
                        JObject json = new JObject();

                        json.Add("company_code", model.company_code);
                        json.Add("account_user", model.account_user);
                        //json.Add("account_pwd", model.account_pwd);
                        json.Add("account_pwd", "");
                        json.Add("account_type", model.account_type);
                        json.Add("account_level", model.account_level);
                        json.Add("account_email", model.account_email);
                        json.Add("account_email_alert", model.account_email_alert);
                        json.Add("account_line", model.account_line);
                        json.Add("account_line_alert", model.account_line_alert);
                        json.Add("polmenu_code", model.polmenu_code);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        cls_ctTRAccountpos objTRAccountpos = new cls_ctTRAccountpos();
                        List<cls_TRAccountpos> listTRAccountpos = objTRAccountpos.getDataByFillter(model.company_code, model.account_user, model.account_type, "");
                        JArray arrayTRAccountpos = new JArray();
                        if (listTRAccountpos.Count > 0)
                        {
                            int indexTRAccount = 1;

                            foreach (cls_TRAccountpos modelTRAccount in listTRAccountpos)
                            {
                                JObject jsonTRPlan = new JObject();
                                jsonTRPlan.Add("company_code", modelTRAccount.company_code);
                                jsonTRPlan.Add("account_user", modelTRAccount.account_user);
                                jsonTRPlan.Add("account_type", modelTRAccount.account_type);
                                jsonTRPlan.Add("position_code", modelTRAccount.position_code);

                                jsonTRPlan.Add("index", indexTRAccount);


                                indexTRAccount++;

                                arrayTRAccountpos.Add(jsonTRPlan);
                            }
                            json.Add("position_data", arrayTRAccountpos);
                        }
                        else
                        {
                            json.Add("position_data", arrayTRAccountpos);
                        }
                        cls_ctTRAccountdep objTRAccountdep = new cls_ctTRAccountdep();
                        List<cls_TRAccountdep> listTRAccountdep = objTRAccountdep.getDataByFillter(model.company_code, model.account_user, model.account_type, "", "");
                        JArray arrayTRAccountdep = new JArray();
                        if (listTRAccountdep.Count > 0)
                        {
                            int indexTRAccountdep = 1;

                            foreach (cls_TRAccountdep modelTRAccountdep in listTRAccountdep)
                            {
                                JObject jsonTRdep = new JObject();
                                jsonTRdep.Add("company_code", modelTRAccountdep.company_code);
                                jsonTRdep.Add("account_user", modelTRAccountdep.account_user);
                                jsonTRdep.Add("account_type", modelTRAccountdep.account_type);
                                jsonTRdep.Add("level_code", modelTRAccountdep.level_code);
                                jsonTRdep.Add("dep_code", modelTRAccountdep.dep_code);

                                jsonTRdep.Add("index", indexTRAccountdep);


                                indexTRAccountdep++;

                                arrayTRAccountdep.Add(jsonTRdep);
                            }
                            json.Add("dep_data", arrayTRAccountdep);
                        }
                        else
                        {
                            json.Add("dep_data", arrayTRAccountdep);
                        }

                        cls_ctTRAccount objTRAccount = new cls_ctTRAccount();
                        List<cls_TRAccount> listTRAccount = objTRAccount.getDataByFillter(model.company_code, model.account_user, model.account_type, "");
                        JArray arrayTRAccount = new JArray();
                        if (listTRAccount.Count > 0)
                        {
                            int indexTRAccount = 1;

                            foreach (cls_TRAccount modelTRAccount in listTRAccount)
                            {
                                JObject jsonTRdep = new JObject();
                                jsonTRdep.Add("company_code", modelTRAccount.company_code);
                                jsonTRdep.Add("account_user", modelTRAccount.account_user);
                                jsonTRdep.Add("account_type", modelTRAccount.account_type);
                                jsonTRdep.Add("worker_code", modelTRAccount.worker_code);

                                jsonTRdep.Add("index", indexTRAccount);


                                indexTRAccount++;

                                arrayTRAccount.Add(jsonTRdep);
                            }
                            json.Add("worker_data", arrayTRAccount);
                        }
                        else
                        {
                            json.Add("worker_data", arrayTRAccount);
                        }
                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }
                    Authen objAuthen = new Authen();
                    if (input.usname.Equals(list[0].account_user) && input.pwd.Equals(list[0].account_pwd))
                    {
                        RequestData aa = new RequestData();
                        aa.usname = input.usname;
                        aa.pwd = input.pwd;
                        aa.company_code = input.company_code;
                        ResponseData bb = Login(aa);
                        output["success"] = true;
                        output["message"] = bb.token;
                        output["user_data"] = array;
                        log.apilog_status = "200";
                        log.apilog_message = "";
                    }
                    else
                    {
                        output["success"] = false;
                        output["message"] = "No access rights";

                        log.apilog_status = "404";
                        log.apilog_message = "No access rights";
                    }
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
                this.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }

        private ResponseData Login(RequestData data)
        {

            Authen objAuthen = new Authen();

            string secureToken = objAuthen.GetJwt(data.usname, data.pwd,data.company_code);
            var response = new ResponseData
            {
                token = secureToken,
                authenticated = true,
                employeeId = data.usname,
                firstname = data.usname,
                timestamp = DateTime.Now,
                userName = data.usname
            };        


            return response;
        }

        public bool doVerify(string token)
        {
            try
            {
                string tmp = token.Substring(7);

                Authen objAuthen = new Authen();

                var handler = new JwtSecurityTokenHandler();
                var decodedValue = handler.ReadJwtToken(tmp);

                var com = decodedValue.Claims.Single(claim => claim.Type == "company_code");
                var usr = decodedValue.Claims.Single(claim => claim.Type == "user_aabbcc");
                var pwd = decodedValue.Claims.Single(claim => claim.Type == "pass_qwer");
                var iat = decodedValue.Claims.Single(claim => claim.Type == "iat");
                cls_ctMTAccount Account = new cls_ctMTAccount();
                List<cls_MTAccount> list = Account.getDataByFillter(com.Value, usr.Value, "",0,"");

                if (usr.Value.Equals(list[0].account_user) && pwd.Value.Equals(list[0].account_pwd))                
                {

                    if (objAuthen.doCheckExpireToken(iat.Value))
                        return true;
                    else
                        return false;

                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }

        private object class2JsonData(object input)
        {
            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            return tmp;
        }
        #endregion

        public string checkToken(RequestData req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SYS004.1";
            log.apilog_by = req.usname;
            log.apilog_data = "all";

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !this.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    this.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                output["success"] = true;
                output["message"] = "Check Token successfully";

                log.apilog_status = "200";
                log.apilog_message = "Check Token";

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                this.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }

        public void doRecordLog(cls_SYSApilog log)
        {
            try {
                cls_ctSYSApilog controller = new cls_ctSYSApilog();
                controller.insert(log);
            }
            catch { }   
        }
       
    }

    [DataContract]
    public class BasicRequest
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company { get; set; }
        [DataMember]
        public string procost_code { get; set; } 
        [DataMember]
        public string language { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string protype_code { get; set; }
         [DataMember]
        public string item_code { get; set; }

        [DataMember]
        public string bonus_code { get; set; }

        [DataMember]
        public string com { get; set; }
        [DataMember]
        public string emp { get; set; }
        [DataMember]
        public DateTime date { get; set; }
        [DataMember]
        public string item_type { get; set; }
        [DataMember]
        public string item { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string emptype { get; set; }
        [DataMember]
        public string empbranch { get; set; }
        [DataMember]
        public string addresstype_code { get; set; }
        [DataMember]
        public string level_group { get; set; }
         [DataMember]
        public string proequipmenttype_code { get; set; }
        [DataMember]
         public string referral_code { get; set; }
        
    }

    public class RequestData
    {
        public RequestData() { }
        public string company_code { get; set; }
        public string usname { get; set; }
        public string pwd { get; set; }
    }

    [DataContract]
    public class ResponseData
    {
        [DataMember(Order = 0)]
        public string token { get; set; }
        [DataMember(Order = 1)]
        public bool authenticated { get; set; }
        [DataMember(Order = 2)]
        public string employeeId { get; set; }
        [DataMember(Order = 3)]
        public string firstname { get; set; }

        [DataMember(Order = 8)]
        public DateTime timestamp { get; set; }
        [DataMember(Order = 9)]
        public string userName { get; set; }
    }

    public class Person
    {
        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public int Age { get; set; }

        public Person(string firstName, string lastName, int age)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Age = age;
        }
    }
    [DataContract]
    public class listdata
    {

        [DataMember]
        public List<Person> data { get; set; }

        public listdata(List<Person> data)
        {
            this.data = data;
        }
    }

    
}
