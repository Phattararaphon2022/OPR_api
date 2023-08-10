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

        //#region imagemaps
        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "/doUploadImagesmapstest?ref_to={ref_to}", ResponseFormat = WebMessageFormat.Json)]
        //string doUploadImagesmapstest(string ref_to, Stream stream);

        //[OperationContract(Name = "doGetImagesmapstest")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string doGetImagesmapstest(FillterComimgemaps req);
        //#endregion

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

        //#region paybank
        ////
        //[OperationContract(Name = "paybank")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string getpaybank(BasicRequest req);

        ////[OperationContract]
        ////[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        ////string getpaybank(string com);
        //#endregion

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

        //#region paybonus
        //[OperationContract(Name = "doManageTRPaybonus_List")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string getTRPaybonusList(BasicRequest req);

        //[OperationContract(Name = "ManageTRPaybonus")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string doManageTRPaybonusList(InputTRPaybonus input);

        //[OperationContract(Name = "ManageTRPaybonus_list")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string doManageTRPaybonus(InputTRPaybonus input);

        //[OperationContract(Name = "ManageTRPaybonus_del")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string doDeleteTRPaybonus(InputTRPaybonus input);

        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "/doUploadManageTRPaybonus?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        //Task<string> doUploadManageTRPaybonus(string token, string by, string fileName, Stream stream);
        //#endregion

        #region MTPeriod
        [OperationContract(Name = "periods_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTPeriodList(InputMTPeriod input);

        [OperationContract(Name = "periods")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTPeriod(InputMTPeriod input);

        [OperationContract(Name = "periods_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTPeriod(InputMTPeriod input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTPeriods?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTPeriod(string token, string by, string fileName, Stream stream);
        #endregion


        #region payroll set batch

        #region set bonus
        [OperationContract(Name = "setbonus_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getBatchBonusList(InputTRList input);

        [OperationContract(Name = "setpaypolbonus")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doSetBatchBonus(InputTRList input);

        [OperationContract(Name = "setbonus_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteBatchBonus(InputTRPaypolbonus input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadSetBonus?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadBatchBonus(string token, string by, string fileName, Stream stream);
        #endregion

        #region set paypolprovident
        [OperationContract(Name = "setpolprovident_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getBatchPaypolprovidentList(InputTRList input);

        [OperationContract(Name = "setpaypolprovident")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doSetBatchPaypolprovident(InputTRList input);

        [OperationContract(Name = "setpolprovident_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteBatchPaypolprovident(InputTRList input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadSetPolprovident?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadBatchPaypolprovident(string token, string by, string fileName, Stream stream);
        #endregion

        #region set paypolitem
        [OperationContract(Name = "setpolpaypolitem_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getBatchPaypolitemList(InputTRList input);

        [OperationContract(Name = "setpaypolpaypolitem")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doSetBatchPaypolitem(InputTRList input);

        [OperationContract(Name = "paypolitem_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteBatchPaypolitem(InputTRList input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadSetPaypolitem?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadBatchPaypolPaypolitem(string token, string by, string fileName, Stream stream);
        #endregion


        #endregion

        #region set TRpayitem
        [OperationContract(Name = "TRpayitem_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRPayitemList(InputTRPayitem input);

        [OperationContract(Name = "TRpayitemlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRPayitemList(InputTRPayitem input);

        [OperationContract(Name = "TRpayitem")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRPayitem(InputTRPayitem input);


        [OperationContract(Name = "TRpayitem_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRPayitem(InputTRPayitem input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadTRpayitem?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadTRPayitem(string token, string by, string fileName, Stream stream);
        #endregion

        #region Paytran&Acc

        [OperationContract(Name = "getpaytranacc")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRPaytranAccList(InputTRPaytran input);

        [OperationContract(Name = "doManageTRPaytran")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRPaytran(InputTRPaytran input);

        [OperationContract(Name = "doDeleteTRPaytran")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRPaytran(InputTRPaytran input);
        #endregion


        #region Paytran
        [OperationContract(Name = "paytran_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRPaytranList(FillterPayroll req);
        #endregion

        #region Payreduce
        [OperationContract(Name = "payreduce_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRPayreduceList(FillterPayroll req);
        #endregion
    }
}
