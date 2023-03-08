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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IModuleEmployee" in both code and config file together.
    [ServiceContract]
    public interface IModuleEmployee
    {
        #region Emp

        #region Worker
        [OperationContract(Name = "worker_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getWorkerList(BasicRequest req);

        [OperationContract(Name = "worker")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTWorker(InputMTWorker input);

        [OperationContract(Name = "worker_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTWorker(InputMTWorker input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadWorker?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadWorker(string token, string by, string fileName, Stream stream);
        #endregion

        
        #region Dep
        [OperationContract(Name = "dep_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getDepList(BasicRequest req);

        [OperationContract(Name = "dep")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTDep(InputMTDep input);

        [OperationContract(Name = "dep_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTDep(InputMTDep input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadDep?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadDep(string token, string by, string fileName, Stream stream);
        #endregion

        #region Position
        [OperationContract(Name = "position_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getPositionList(BasicRequest req);

        [OperationContract(Name = "position")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTPosition(InputMTPosition input);

        [OperationContract(Name = "position_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTPosition(InputMTPosition input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadPosition?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadPosition(string token, string by, string fileName, Stream stream);
        #endregion

        #region Group
        [OperationContract(Name = "group_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getGroupList(BasicRequest req);

        [OperationContract(Name = "group")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTGroup(InputMTGroup input);

        [OperationContract(Name = "group_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTGroup(InputMTGroup input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadGroup?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadGroup(string token, string by, string fileName, Stream stream);
        #endregion

        #region Initial
        [OperationContract(Name = "initial_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getInitialList(BasicRequest req);

        [OperationContract(Name = "initial")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTInitial(InputMTInitial input);

        [OperationContract(Name = "initial_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTInitial(InputMTInitial input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadInitial?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadInitial(string token, string by, string fileName, Stream stream);
        #endregion

        #region Type
        [OperationContract(Name = "type_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTypeList(BasicRequest req);

        [OperationContract(Name = "type")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTType(InputMTType input);

        [OperationContract(Name = "type_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTType(InputMTType input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadType?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadType(string token, string by, string fileName, Stream stream);
        #endregion

        #region Status
        [OperationContract(Name = "status_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getStatusList(BasicRequest req);

        [OperationContract(Name = "status")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTStatus(InputMTStatus input);

        [OperationContract(Name = "status_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTStatus(InputMTStatus input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadStatus?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadStatus(string token, string by, string fileName, Stream stream);
        #endregion

        #region TR_Address
        [OperationContract(Name = "empaddlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRAddressList(InputTRAddress input);

        #endregion

        #region TR_CARD
        [OperationContract(Name = "empcardlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRCardList(InputTRCard input);
        #endregion

        #region TR_BANK
        [OperationContract(Name = "empbanklist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRBankList(InputTRBank input);
        #endregion

        #endregion
    }
}
