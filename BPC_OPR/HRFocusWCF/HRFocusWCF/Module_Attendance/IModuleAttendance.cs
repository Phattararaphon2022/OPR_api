
using System.ServiceModel;
using System.ServiceModel.Web;

namespace BPC_OPR
{
    [ServiceContract]
    public interface IModuleAttendance
    {
        [OperationContract(Name = "year_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTYearList(InputMTYear input);

        [OperationContract(Name = "year")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTYear(InputMTYear input);

        [OperationContract(Name = "year_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTYear(InputMTYear input);
    }
}
