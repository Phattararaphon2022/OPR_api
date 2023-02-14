﻿
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using ClassLibrary_BPC.hrfocus.model;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
namespace BPC_OPR
{
    [ServiceContract]
    public interface IModuleAttendance
    {
        #region MTYear
        [OperationContract(Name = "year_list")]
        [WebInvoke(Method = "POST",RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTYearList(InputMTYear input);

        [OperationContract(Name = "year")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTYear(InputMTYear input);

        [OperationContract(Name = "year_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTYear(InputMTYear input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadYear?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadYear(string token, string by, string fileName, Stream stream);

        #endregion

        #region MTPeriod
        [OperationContract(Name = "period_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTPeriodList(InputMTPeriod input);

        [OperationContract(Name = "period")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTPeriod(InputMTPeriod input);

        [OperationContract(Name = "period_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTPeriod(InputMTPeriod input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTPeriod?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTPeriod(string token, string by, string fileName, Stream stream);
        #endregion

        #region MTReason
        [OperationContract(Name = "reason_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTReasonList(InputMTReason input);

        [OperationContract(Name = "reason")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTReason(InputMTReason input);

        [OperationContract(Name = "reason_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTReason(InputMTReason input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTReason?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTReason(string token, string by, string fileName, Stream stream);

        #endregion

        #region MTLocation
        [OperationContract(Name = "location_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTLocationList(InputMTLocation input);

        [OperationContract(Name = "location")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTLocation(InputMTLocation input);

        [OperationContract(Name = "location_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTLocation(InputMTLocation input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTLocation?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTLocation(string token, string by, string fileName, Stream stream);

        #endregion
    }

}
