using HRFocusWCF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;
using Newtonsoft.Json.Linq;
using System.Web;

namespace BPC_OPR
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IBpcOpr
    {
                             

        [OperationContract(Name = "doAuthen2")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AuthResponse doAuthen2(RequestData input);

        [OperationContract(Name = "doAuthen")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doAuthen(RequestData input);


        [OperationContract(Name = "doCheckToken")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string checkToken(RequestData input);
        
        
        
    }
    public class AuthResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string token { get; set; }
        public List<UserData> data { get; set; }
    }

    public class UserData
    {
        // Define properties based on the structure of your user data
        public string CompanyCode { get; set; }
        public string AccountUser { get; set; }
        // ... add more properties as needed
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }



}
