
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
    public interface IModuleSelfServices
    {

        #region TRTimeleave
        [OperationContract(Name = "timeleave_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRTimeleaveList(InputTRTimeleave input);

        [OperationContract(Name = "timeleave")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRTimeleave(InputTRTimeleave input);

        [OperationContract(Name = "timeleave_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRTimeleave(InputTRTimeleave input);
        #endregion

        #region TRTimeot
        [OperationContract(Name = "timeot_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRTimeotList(InputTRTimeot input);

        [OperationContract(Name = "timeot")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRTimeot(InputTRTimeot input);

        [OperationContract(Name = "timeot_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRTimeot(InputTRTimeot input);
        #endregion

        #region TRTimeshift
        [OperationContract(Name = "timeshift_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRTimeshiftList(InputTRTimeshift input);

        [OperationContract(Name = "timeshift")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRTimeshift(InputTRTimeshift input);

        [OperationContract(Name = "timeshift_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRTimeshift(InputTRTimeshift input);
        #endregion

        #region MTWorkflow
        [OperationContract(Name = "workflow_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTWorkflowList(InputMTWorkflow input);

        [OperationContract(Name = "workflow")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTWorkflow(InputMTWorkflow input);

        [OperationContract(Name = "workflow_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTWorkflow(InputMTWorkflow input);
        #endregion

        #region TRLineapprove
        [OperationContract(Name = "lineapprove_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRLineapproveList(InputTRLineapprove input);

        [OperationContract(Name = "lineapprove")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRLineapprove(InputTRLineapprove input);

        [OperationContract(Name = "lineapprove_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRLineapprove(InputTRLineapprove input);
        #endregion

        #region MTAccount
        [OperationContract(Name = "account_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTAccountList(InputMTAccount input);

        [OperationContract(Name = "account")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTAccount(InputMTAccount input);

        [OperationContract(Name = "account_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteeMTAccount(InputMTAccount input);
        #endregion


    }

}
