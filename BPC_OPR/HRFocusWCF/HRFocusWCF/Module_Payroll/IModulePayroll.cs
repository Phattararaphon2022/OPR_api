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
    public interface IModulePayroll
    {
        //[OperationContract(Name = "test")]
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        //string doTest();



        #region TRTaxrate
        [OperationContract(Name = "TRTaxrate_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRTaxrateList(BasicRequest req);

        [OperationContract(Name = "TRTaxrate")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRTaxrate(InputTRTaxrate input);

        [OperationContract(Name = "TRTaxrate_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRTaxrate(InputTRTaxrate input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadTRTaxrate?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadTRTaxrate(string token, string by, string fileName, Stream stream);

        #endregion

        #region MTItem
        [OperationContract(Name = "MTItem_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTItemList(BasicRequest req);

        [OperationContract(Name = "MTItem")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTItem(InputMTItem input);

        [OperationContract(Name = "MTItem_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTItem(InputMTItem input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTItem?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTItemr(string token, string by, string fileName, Stream stream);
        #endregion

        

        #region MTProvident
        [OperationContract(Name = "MTProvident_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTProvidentList(BasicRequest req);

        [OperationContract(Name = "MTProvident")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTProvident(InputMTProvident input);

        [OperationContract(Name = "MTProvident_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTProvident(InputMTProvident input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTProvident?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTProvident(string token, string by, string fileName, Stream stream);
        #endregion

        #region bonus 
        [OperationContract(Name = "bonus_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getBonusList(BasicRequest req);

        [OperationContract(Name = "bonus")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageBonus(InputMTBonus input);

        [OperationContract(Name = "bonus_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteBonus(InputMTBonus input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadBonus?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadBonus(string token, string by, string fileName, Stream stream);
        #endregion



    }
}
