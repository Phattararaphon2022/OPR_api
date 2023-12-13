﻿using System;
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

    }
}