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

        [OperationContract(Name = "APIHRJob")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageAPIHRJob(APIHRJob input);

        
    }
}