using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace BPC_OPR
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IModuleProject" in both code and config file together.
    [ServiceContract]
    public interface IBCInterface
    {

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "APIHRJob", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageAPIHRJob(APIHRJob input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "APIHRProject", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        APIHRProjectResponse doManageAPIHRProject(APIHRProject input);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "APIHRProject?CompanyCode={com}&ProjectCode={code}&ProjectStatus={status}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        APIHRProjectResponse APIHRProjectList(string com, string code, string status);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "APIHRProject?CompanyCode={com}&ProjectCode={code}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        APIHRProjectResponse doDeleteAPIHRProject(string com, string code);

        
    }
}