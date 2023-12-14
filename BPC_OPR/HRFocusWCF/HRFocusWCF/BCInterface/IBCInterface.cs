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
        [WebInvoke(Method = "POST", UriTemplate = "APIHRJobs", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageAPIHRJob(APIHRJob input);

        #region APIHRProject
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "APIHRProject", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<APIHRProject> APIHRProjectCreate(APIHRProject input);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "APIHRProject", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<APIHRProject> APIHRProjectUpdate(APIHRProject input);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "APIHRProject?CompanyCode={com}&ProjectCode={code}&ProjectStatus={status}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<APIHRProject> APIHRProjectList(string com, string code, string status);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "APIHRProject?CompanyCode={com}&ProjectCode={code}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<APIHRProject> APIHRProjectDelete(string com, string code);
        #endregion


        #region APIHRProjectContract
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "APIHRProjectContract", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProContract> APIHRProjectContractCreate(ProContract input);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "APIHRProjectContract", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProContract> APIHRProjectContractUpdate(ProContract input);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "APIHRProjectContract?ProjectCode={code}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProContract> APIHRProjectContractList(string code);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "APIHRProjectContract?ProjectCode={code}&ProContractId={id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProContract> APIHRProjectContractDelete(string code, string id);
        #endregion


        #region APIHRJob2
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "APIHRJob", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<APIHRJobmain> APIHRJobCreate(APIHRJobmain input);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "APIHRJob", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<APIHRJobmain> APIHRJobUpdate(APIHRJobmain input);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "APIHRJob?ProjectCode={code}&ProJobMainCode={job}&Version={ver}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProJobMain> APIHRJobList(string code, string job, string ver);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "APIHRJob?ProjectCode={code}&ProJobMainCode={job}&Version={ver}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProJobMain> APIHRJobDelete(string code, string job, string ver);
        #endregion


        #region APIHRUniform
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "APIHRUniform", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProUniform> APIHRUniformCreate(ProUniform input);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "APIHRUniform", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProUniform> APIHRUniformUpdate(ProUniform input);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "APIHRUniform?CompanyCode={com}&ProUniformCode={code}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProUniform> APIHRUniformList(string com, string code);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "APIHRUniform?CompanyCode={com}&ProUniformCode={code}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProUniform> APIHRUniformDelete(string com, string code);
        #endregion


        #region APIHRUniformSummary
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "APIHRUniformSummary", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProEquipmentReq> APIHRUniformSummaryCreate(ProEquipmentReq input);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "APIHRUniformSummary", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProEquipmentReq> APIHRUniformSummaryUpdate(ProEquipmentReq input);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "APIHRUniformSummary?ProjectCode={projectcode}&ProJobCode={job}&ProUniformCode={code}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProEquipmentReq> APIHRUniformSummaryList(string projectcode, string job, string code);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "APIHRUniformSummary?ProjectCode={projectcode}&ProJobCode={job}&ProUniformCode={code}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProEquipmentReq> APIHRUniformSummaryDelete(string projectcode, string job, string code);
        #endregion
    }
}