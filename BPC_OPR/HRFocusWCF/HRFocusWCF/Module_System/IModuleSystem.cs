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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IModuleSystem" in both code and config file together.
    [ServiceContract]
    public interface IModuleSystem
    {
        [OperationContract(Name = "bank_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getBankList(BasicRequest req);

        [OperationContract(Name = "bank")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTBank(InputMTBank input);

        [OperationContract(Name = "bank_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTBank(InputMTBank input);
        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadBank?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadBank(string token, string by, string fileName, Stream stream);
    }
}
