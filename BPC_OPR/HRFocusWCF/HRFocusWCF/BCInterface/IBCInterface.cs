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
        [WebInvoke(Method = "POST", UriTemplate = "APIHRProject?TransactionId={id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<APIHRProject> APIHRProjectCreate(APIHRProject input, string id);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "APIHRProject?TransactionId={id}&OldTransactionId={oldid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<APIHRProject> APIHRProjectUpdate(APIHRProject input, string id, string oldid);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "APIHRProject?CompanyCode={com}&ProjectCode={code}&ProjectStatus={status}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<APIHRProject> APIHRProjectList(string com, string code, string status);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "APIHRProject?CompanyCode={com}&ProjectCode={code}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<APIHRProject> APIHRProjectDelete(string com, string code);
        #endregion


        #region APIHRProjectContract
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "APIHRProjectContract?TransactionId={id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProContract> APIHRProjectContractCreate(ProContract input, string id);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "APIHRProjectContract?TransactionId={id}&OldTransactionId={oldid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProContract> APIHRProjectContractUpdate(ProContract input, string id, string oldid);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "APIHRProjectContract?ProjectCode={code}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProContract> APIHRProjectContractList(string code);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "APIHRProjectContract?ProjectCode={code}&ProContractId={id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProContract> APIHRProjectContractDelete(string code, string id);
        #endregion


        #region APIHRJob2
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "APIHRJob?TransactionId={id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<APIHRJobmain> APIHRJobCreate(APIHRJobmain input, string id);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "APIHRJob?TransactionId={id}&OldTransactionId={oldid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<APIHRJobmain> APIHRJobUpdate(APIHRJobmain input, string id, string oldid);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "APIHRJob?ProjectCode={code}&ProJobMainCode={job}&Version={ver}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProJobMain> APIHRJobList(string code, string job, string ver);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "APIHRJob?ProjectCode={code}&ProJobMainCode={job}&Version={ver}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProJobMain> APIHRJobDelete(string code, string job, string ver);
        #endregion


        #region APIHRUniform
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "APIHRUniform?TransactionId={id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProUniform> APIHRUniformCreate(ProUniform input, string id);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "APIHRUniform?TransactionId={id}&OldTransactionId={oldid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProUniform> APIHRUniformUpdate(ProUniform input, string id, string oldid);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "APIHRUniform?CompanyCode={com}&ProUniformCode={code}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProUniform> APIHRUniformList(string com, string code);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "APIHRUniform?CompanyCode={com}&ProUniformCode={code}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<ProUniform> APIHRUniformDelete(string com, string code);
        #endregion


        #region APIHRUniformSummary
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "APIHRUniformSummary?TransactionId={id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<EmpTrUniform> APIHRUniformSummaryCreate(EmpTrUniform input, string id);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "APIHRUniformSummary?TransactionId={id}&OldTransactionId={oldid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<EmpTrUniform> APIHRUniformSummaryUpdate(EmpTrUniform input, string id, string oldid);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "APIHRUniformSummary?CompanyCode={com}&ProjectCode={code}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<EmpTrUniform> APIHRUniformSummaryList(string com,string code);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "APIHRUniformSummary?CompanyCode={com}&ProjectCode={code}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ApiResponse<EmpTrUniform> APIHRUniformSummaryDelete(string com, string code);
        #endregion
    }
}