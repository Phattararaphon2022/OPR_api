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


        //-- Reason
        [OperationContract(Name = "reason_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getReasonList(BasicRequest req);

        [OperationContract(Name = "reason")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageSYSReason(InputSYSReason input);

        [OperationContract(Name = "reason_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteSYSReason(InputSYSReason input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadReason?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadReason(string token, string by, string fileName, Stream stream);

        //--Cardtype
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

        //-- LOCATION
        [OperationContract(Name = "location_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getLocationList(BasicRequest req);

        [OperationContract(Name = "location")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTLocation(InputMTLocation input);

        [OperationContract(Name = "location_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTLocation(InputMTLocation input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadLocation?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTLocation(string token, string by, string fileName, Stream stream);

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
