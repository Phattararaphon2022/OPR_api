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

        #region Cardtype
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
        #endregion

        #region FAMILY
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
        #endregion


        //#region LEVEL
        //[OperationContract(Name = "level_list")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string getLevelList(InputMTLevel input);

        //[OperationContract(Name = "level")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string doManageMTLevel(InputMTLevel input);

        //[OperationContract(Name = "level_del")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string doDeleteMTLevel(InputMTLevel input);

        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "/doUploadLevel?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        //Task<string> doUploadMTLevel(string token, string by, string fileName, Stream stream);

        //#endregion


        #region LEVEL
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
        #endregion

        #region Reduce
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
        #endregion

        #region RELIGION
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
        #endregion

        #region PROVINCE
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
#endregion

        #region Addresstype
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
        #endregion

        #region ComAddress
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
        #endregion

        #region Comaddlocation
        [OperationContract(Name = "comaddlocation_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getComaddlocationList(FillterCompany req);

        [OperationContract(Name = "comaddlocation")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTComaddlocation(InputComTransaction input);

        [OperationContract(Name = "comaddlocation_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTComaddlocation(InputMTComaddlocation input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadComaddlocation?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTComaddlocation(string token, string by, string fileName, Stream stream);
        #endregion

        #region COMPANY
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

        #endregion

        //#region test Code structure
        //[OperationContract]

        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        //string getSYSCodestructureList();

        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        //string getMTPolcode(InputMTPolcode req);

        //[OperationContract]
        //[WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string getNewCode(string com, string type, string emptype);

        //[OperationContract]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string getTRPolcode(FillterCompany req);

        //[OperationContract(Name = "doManagePolcode")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string doManagePolcode(InputMTPolcode input);

        //[OperationContract(Name = "doDeleteMTPolcode")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string doDeleteMTPolcode(InputMTPolcode input);
        //#endregion

        #region Codestructure

        [OperationContract(Name = "codestructure_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getCodestructureList(FillterCompany req);

        [OperationContract(Name = "codestructure")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageCodestructure(InputMTCodestructure input);

        [OperationContract(Name = "codestructure_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteCodestructure(InputMTCodestructure input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadCodestructure?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadCodestructure(string token, string by, string fileName, Stream stream);
        #endregion


        #region TRPolcode
        //-- TRPolcode
        [OperationContract(Name = "TRPolcode_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRPolcodeList(BasicRequest req);

        [OperationContract(Name = "TRPolcode")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRPolcode(InputTRPolcode input);

        [OperationContract(Name = "TRPolcode_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRPolcode(InputTRPolcode input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadTRPolcode?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadTRPolcode(string token, string by, string fileName, Stream stream);

        [OperationContract(Name = "getnewcode")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getNewCode(BasicRequest req);
        #endregion

        //#region TRPolcode

        //[OperationContract(Name = "TRPolcode_list")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string getTRPolcodeList(FillterCompany req);

        //[OperationContract(Name = "TRPolcode")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string doManageTRPolcode(InputTRPolcode input);

        //[OperationContract(Name = "TRPolcode_del")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string doDeleteTRPolcode(InputTRPolcode input);

        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "/doUploadTRPolcode?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        //Task<string> doUploadTRPolcode(string token, string by, string fileName, Stream stream);
        //#endregion

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

        #region Comcard

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
        #endregion

        #region request
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
        #endregion

        #region combranch
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
        #endregion

        #region ComLocation
        [OperationContract(Name = "comlocation_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getComlocationList(FillterCompany req);

        [OperationContract(Name = "comlocation")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTComlocation(InputMTComLocation input);

        [OperationContract(Name = "comlocation_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTComlocation(InputMTComLocation input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadcomlocation?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTComlocation(string token, string by, string fileName, Stream stream);
        #endregion

        #region Rounds
        [OperationContract(Name = "rounds_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTRoundList(InputMTRound input);

        [OperationContract(Name = "rounds")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTRound(InputMTRound input);

        [OperationContract(Name = "rounds_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTRound(InputMTRound input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadRounds?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTRound(string token, string by, string fileName, Stream stream);

        #endregion

        #region ETHNICITY
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
        #endregion

        #region Bloodtype
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
        #endregion

        #region Hospital
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
        #endregion

        #region Course
        [OperationContract(Name = "course_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getCourseList(BasicRequest req);

        [OperationContract(Name = "course")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTCourse(InputMTCourse input);

        [OperationContract(Name = "course_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTCourse(InputMTCourse input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadCourse?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTCourse(string token, string by, string fileName, Stream stream);
        #endregion

        #region Institute
        [OperationContract(Name = "institute_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getInstituteList(BasicRequest req);

        [OperationContract(Name = "institute")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTInstitute(InputMTInstitute input);

        [OperationContract(Name = "institute_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTInstitute(InputMTInstitute input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadInstitute?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTInstitute(string token, string by, string fileName, Stream stream);
        #endregion

        #region Faculty
        [OperationContract(Name = "faculty_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getFacultyList(BasicRequest req);

        [OperationContract(Name = "faculty")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTFaculty(InputMTFaculty input);

        [OperationContract(Name = "faculty_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTFaculty(InputMTFaculty input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadfaculty?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTFaculty(string token, string by, string fileName, Stream stream);
        #endregion

        #region Major
        [OperationContract(Name = "major_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMajorList(BasicRequest req);

        [OperationContract(Name = "major")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTMajor(InputMTMajor input);

        [OperationContract(Name = "major_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTMajor(InputMTMajor input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadmajor?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTMajor(string token, string by, string fileName, Stream stream);
        #endregion

        #region Qualification
        [OperationContract(Name = "qualification_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getQualificationList(BasicRequest req);

        [OperationContract(Name = "qualification")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTQualification(InputMTQualification input);

        [OperationContract(Name = "qualification_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTQualification(InputMTQualification input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadqualification?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTQualification(string token, string by, string fileName, Stream stream);
        #endregion


        #region MTpolround
        [OperationContract(Name = "polround_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTPolround(BasicRequest req);

        [OperationContract(Name = "polround")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTPolround(InputMTPolround input);

        [OperationContract(Name = "polround_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTPolround(InputMTPolround input);

        #endregion

        #region Supply
        [OperationContract(Name = "supply_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getSupplyList(BasicRequest req);

        [OperationContract(Name = "supply")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTSupply(InputMTSupply input);

        [OperationContract(Name = "supply_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTSupply(InputMTSupply input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadSupply?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadSupply(string token, string by, string fileName, Stream stream);
        #endregion

        #region Uniform
        [OperationContract(Name = "uniform_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getUniformList(BasicRequest req);

        [OperationContract(Name = "uniform")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTUniform(InputMTUniform input);

        [OperationContract(Name = "uniform_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTUniform(InputMTUniform input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadUniform?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]



        Task<string> doUploadUniform(string token, string by, string fileName, Stream stream);  
        #endregion
        #region image/ 
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadImageslogo?ref_to={ref_to}", ResponseFormat = WebMessageFormat.Json)]
        string doUploadImageslogo(string ref_to, Stream stream);

        [OperationContract(Name = "doGetImageslogo")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doGetImageslogo(FillterCompany req);
        #endregion

        #region imagemaps
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadImagesmaps?ref_to={ref_to}", ResponseFormat = WebMessageFormat.Json)]
        string doUploadImagesmaps(string ref_to, Stream stream);

        [OperationContract(Name = "doGetImagesmaps")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doGetImagesmaps(FillterCompany req);
        #endregion

        //#region image
        
        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "/doUploadComImages?ref_to={ref_to}", ResponseFormat = WebMessageFormat.Json)]
        //string doUploadComImages(string ref_to, Stream stream);
        ////string doUploadComImages(string ref_to, Stream streamlogo, Stream streammaps);

        //[OperationContract(Name = "doGetComImages")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string doGetComImages(FillterCompany req);
        //#endregion
        #region MTMainMenu
        [OperationContract(Name = "mainmenu_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTMainmenuList(InputMTMainmenu input);

        [OperationContract(Name = "mainmenu")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTMainmenu(InputMTMainmenu input);

        [OperationContract(Name = "mainmenu_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteeMTMainmenu(InputMTMainmenu input);
        #endregion

        #region MTPolmenu
        [OperationContract(Name = "polmenu_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTPolmenuList(InputMTPolmenu input);

        [OperationContract(Name = "polmenu")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTPolmenu(InputMTPolmenu input);

        [OperationContract(Name = "polmenu_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteeMTPolmenu(InputMTPolmenu input);
        #endregion

        #region TRWorkflow
        [OperationContract(Name = "sysworkflow_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRWorkflowList(InputTRWorkflow input);

        [OperationContract(Name = "sysworkflow")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRWorkflow(InputTRWorkflow input);

        [OperationContract(Name = "sysworkflow_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteeTRWorkflow(InputTRWorkflow input);
        #endregion
    }
}