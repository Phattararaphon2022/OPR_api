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

        #region MTYear
        [OperationContract(Name = "year_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
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

        [OperationContract(Name = "cardtype_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getCardtypeList(BasicRequest req);

        [OperationContract(Name = "cardtype")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTCardtype(InputMTCardtype input);

        [OperationContract(Name = "cardtype_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTCardtype(InputMTCardtype input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadcardtype?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadCardType(string token, string by, string fileName, Stream stream);

        //-- FAMILY
        [OperationContract(Name = "family_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getFamilyList(BasicRequest req);

        [OperationContract(Name = "family")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTFamily(InputMTFamily input); 

        [OperationContract(Name = "family_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTFamily(InputMTFamily input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadFamily?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTFamily(string token, string by, string fileName, Stream stream);

        //-- LEVEL
        [OperationContract(Name = "level_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getLevelList(BasicRequest req);

        [OperationContract(Name = "level")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTLevel(InputMTLevel input);

        [OperationContract(Name = "level_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTLevel(InputMTLevel input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadLevel?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTLevel(string token, string by, string fileName, Stream stream);

        

        //-- Reduce
        [OperationContract(Name = "reduce_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getReduceList(BasicRequest req);

        [OperationContract(Name = "reduce")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTReduce(InputMTReduce input);

        [OperationContract(Name = "reduce_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTReduce(InputMTReduce input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadReduce?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTReduce(string token, string by, string fileName, Stream stream);


    //-- RELIGION
        [OperationContract(Name = "religion_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getReligionList(BasicRequest req);

        [OperationContract(Name = "religion")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTReligion(InputMTReligion input);

        [OperationContract(Name = "religion_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTReligion(InputMTReligion input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadReligion?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTReligion(string token, string by, string fileName, Stream stream);

        //-- PROVINCE
        [OperationContract(Name = "province_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getProvinceList(BasicRequest req);

        [OperationContract(Name = "province")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTProvince(InputMTProvince input);

        [OperationContract(Name = "province_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTProvince(InputMTProvince input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadProvince?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTProvince(string token, string by, string fileName, Stream stream);


        //-- Addresstype
        [OperationContract(Name = "addresstype_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getAddresstypeList(BasicRequest req);

        [OperationContract(Name = "addresstype")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTAddresstype(InputMTAddresstype input);

        [OperationContract(Name = "addresstype_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTAddresstype(InputMTAddresstype input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadAddresstype?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTAddresstype(string token, string by, string fileName, Stream stream);

        //-- ComAddress
        [OperationContract(Name = "comaddress_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getComAddressList(FillterCompany req);

        [OperationContract(Name = "comaddress")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTComAddress(InputComTransaction input);

        [OperationContract(Name = "comaddress_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTComAddress(InputMTComaddress input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadComaddress?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTComAddress(string token, string by, string fileName, Stream stream);

        //-- COMPANY
        [OperationContract(Name = "company_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getCompanyList(FillterCompany req);

        [OperationContract(Name = "company")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTCompany(InputMTCompany input);

        [OperationContract(Name = "company_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTCompany(InputMTCompany input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadCompany?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTCompany(string token, string by, string fileName, Stream stream);


        //-- empid
        [OperationContract(Name = "empid_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getEmpidList(BasicRequest req);

        [OperationContract(Name = "empid")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTEmpid(InputMTEmpId input);

        [OperationContract(Name = "empid_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTEmpid(InputMTEmpId input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadEmpid?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTEmpid(string token, string by, string fileName, Stream stream);

        #region comBANK
        [OperationContract(Name = "combank_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getCombankList(FillterCompany req);

        [OperationContract(Name = "combank")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTCombank(InputComTransaction input);

        [OperationContract(Name = "combank_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTCombank(InputMTCombank input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadCombank?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadCombank(string token, string by, string fileName, Stream stream);
        #endregion
        //#region combank
        //[OperationContract(Name = "combank_list")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string getCombankList(FillterCompany req);

        //[OperationContract(Name = "combank")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string doManageMTCombank(InputComTransaction input);

        //[OperationContract(Name = "combank_del")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string doDeleteMTCombank(InputMTCombank input);

        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "/doUploadCombank?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        //Task<string> doUploadMTCombank(string token, string by, string fileName, Stream stream);
        //#endregion

        //-- Comcard

        [OperationContract(Name = "comcard_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getComcardList(FillterCompany req);

        [OperationContract(Name = "comcard")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTComcard(InputComTransaction input);

        [OperationContract(Name = "comcard_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTComcard(InputMTComcard input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadComcard?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadComcard(string token, string by, string fileName, Stream stream);


        //-- request
        [OperationContract(Name = "request_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getRequestList(BasicRequest req);

        [OperationContract(Name = "request")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTRequest(InputMTRequest input);

        [OperationContract(Name = "request_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTRequest(InputMTRequest input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTRequest?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTRequest(string token, string by, string fileName, Stream stream);


        //-- combranch
        [OperationContract(Name = "combranch_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getCombranchList(FillterCompany req);

        [OperationContract(Name = "combranch")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTCombranch(InputMTCombranch input);

        [OperationContract(Name = "combranch_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTCombranch(InputMTCombranch input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadCombranch?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTCombranch(string token, string by, string fileName, Stream stream);

        //-- ETHNICITY
        [OperationContract(Name = "ethnicity_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getEthnicityList(BasicRequest req);

        [OperationContract(Name = "ethnicity")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTEthnicity(InputMTEthnicity input);

        [OperationContract(Name = "ethnicity_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTEthnicity(InputMTEthnicity input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadEthnicity?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTEthnicity(string token, string by, string fileName, Stream stream);

        //-- Bloodtype
        [OperationContract(Name = "bloodtype_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getBloodtypeList(BasicRequest req);

        [OperationContract(Name = "bloodtype")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManagemMTBloodtype(InputMTBloodtype input);

        [OperationContract(Name = "bloodtype_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTBloodtype(InputMTBloodtype input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadBloodtype?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadBloodtype(string token, string by, string fileName, Stream stream);

        //--  Hospital
        [OperationContract(Name = "hospital_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getHospitalList(BasicRequest req);

        [OperationContract(Name = "hospital")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManagemMTHospital(InputMTHospital input);

        [OperationContract(Name = "hospital_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTHospital(InputMTHospital input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadHospital?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadHospital(string token, string by, string fileName, Stream stream);

    }
}
