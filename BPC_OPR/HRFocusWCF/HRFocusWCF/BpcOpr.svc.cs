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
        public string doAuthen(RequestData input)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATH001";
            log.apilog_by = input.usname;
            log.apilog_data = "";

            try
            {
                if (input.usname.Equals(UserAuthen) && input.pwd.Equals(PwdAuthen))
                {
                    RequestData aa = new RequestData();
                    aa.usname = input.usname;
                    aa.pwd = input.pwd;
                    ResponseData bb = Login(aa);
                    output["success"] = true;
                    output["message"] = bb.token;
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

            string secureToken = objAuthen.GetJwt(data.usname, data.pwd);
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

                var usr = decodedValue.Claims.Single(claim => claim.Type == "user_aabbcc");
                var pwd = decodedValue.Claims.Single(claim => claim.Type == "pass_qwer");
                var iat = decodedValue.Claims.Single(claim => claim.Type == "iat");


                if (usr.Value.Equals(UserAuthen) && pwd.Value.Equals(PwdAuthen))                
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
        public string language { get; set; }
        [DataMember]
        public string company_code { get; set; }
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
        
    }

    public class RequestData
    {
        public RequestData() { }
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

    
}
